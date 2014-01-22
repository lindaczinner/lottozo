using System.Collections.Generic;
using Caliburn.Micro;
using LottozoCore.Helpers;
using LottozoCore.Interfaces;
using LottozoCore.Model;
using System;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public class LotteryTicketSetUpViewModel : Screen
	{
		private bool isAfterCtor;

		public LotteryTypes NavigatedFrom { get; set; }

		public string ApplicationName { get { return Constants.AppName; } }

		public bool IsNumbersEditable { get { return false; } }

		public List<string> GameTypes { get; private set; }

		private string selectedGameType;
		public string SelectedGameType
		{
			get { return selectedGameType; }
			set
			{
				var hasChanged = selectedGameType != value;

				selectedGameType = value;
				NotifyOfPropertyChange("SelectedGameType");

				if (hasChanged)
					GameChanged((LotteryTypes)Utils.StringToEnum(selectedGameType));
			}
		}

		public List<string> GameWeekTypes { get; private set; }

		private string selectedGameWeekType;
		public string SelectedGameWeekType
		{
			get { return selectedGameWeekType; }
			set
			{
				selectedGameWeekType = value;
				NotifyOfPropertyChange("SelectedGameWeekType");

				IoC.Get<IStateMachine>().TicketDetails.GameTypes = (LotteryGameTypes)Utils.StringToEnum(selectedGameWeekType);
			}
		}

		private bool numbersChanged;
		public bool NumbersChanged
		{
			get { return numbersChanged; }
			set
			{
				numbersChanged = value;
				NotifyOfPropertyChange("NumbersChanged");
			}
		}

		private bool isJokerAvailable;
		public bool IsJokerAvailable
		{
			get { return isJokerAvailable; }
			set
			{
				var hasChanged = isJokerAvailable != value;

				isJokerAvailable = value;
				NotifyOfPropertyChange("IsJokerAvailable");

				if (hasChanged)
					JokerChanged(isJokerAvailable);

			}
		}

		public BindableCollection<int> JokerNumbers { get; set; }

		private int maxValue;

		public int MaxValue
		{
			get { return maxValue; }
			set
			{
				maxValue = value;
				NotifyOfPropertyChange("MaxValue");
			}
		}

		private int numberNo;

		public int NumberNo
		{
			get { return numberNo; }
			set
			{
				numberNo = value;
				NotifyOfPropertyChange("NumberNo");
			}
		}

		public LotteryTicketSetUpViewModel()
		{
			SystemTray.IsVisible = false;

			JokerNumbers = new BindableCollection<int>();

			SetGameTypeValues();
			SetGameWeekTypes();

			// set game
			SelectedGameType = Utils.EnumToString(IoC.Get<IStateMachine>().TicketDetails.LotteryTypes);

			isAfterCtor = true;
		}

		public void ValidateView()
		{
			if (isAfterCtor)
			{
				var nfs = Utils.EnumToString(NavigatedFrom);
				if (nfs != String.Empty)
					SelectedGameType = nfs;

				NavigatedFrom = LotteryTypes.Undefined;
				isAfterCtor = false;
				return;
			}

			NavigatedFrom = LotteryTypes.Undefined;

			JokerNumbers.Clear();
			JokerNumbers.AddRange(IoC.Get<IStateMachine>().TicketDetails.Joker);

			NumbersChanged = true;
		}

		public void ModifyFields()
		{
			//store a copy to revert
			IoC.Get<IStateMachine>().TempStoredTicket = IoC.Get<IStateMachine>().TicketDetails.Clone();

			IoC.Get<INavigationService>().UriFor<FieldsSetUpViewModel>().Navigate();
		}

		private void GameChanged(LotteryTypes gameType)
		{
			SetGameParameters();

			// save game type
			IoC.Get<IStateMachine>().TicketDetails.LotteryTypes = gameType;

			TicketDetails savedTicket;

			//is there any favorite ticket saved?
			savedTicket = GetFavoriteTicket(gameType);

			if (savedTicket == null || savedTicket.Fields == null)
			{
				// set week type (save is in the property)
				SelectedGameWeekType = Utils.EnumToString(IoC.Get<IStateMachine>().TicketDetails.GameTypes);

				// set numbers
				var generatedNums = Utils.RandomNumbersGenerator(NumberNo, MaxValue);

				// save
				IoC.Get<IStateMachine>().TicketDetails.Fields = new List<Field> {new Field {FieldNo = 1, Numbers = generatedNums}};

				// set joker (save is in the property)
				IsJokerAvailable = false;
			}
			else
			{
				// set week type (save is in the property)
				SelectedGameWeekType = Utils.EnumToString(savedTicket.GameTypes);

				// save
				IoC.Get<IStateMachine>().TicketDetails.Fields = new List<Field>();
				foreach (var field in savedTicket.Fields)
				{
					IoC.Get<IStateMachine>().TicketDetails.Fields.Add(field);
				}

				// set joker (save is in the property)
				IsJokerAvailable = savedTicket.IsJokerAvailable;
				if (JokerNumbers == null)
					JokerNumbers = new BindableCollection<int>();
				JokerNumbers.Clear();
				JokerNumbers.AddRange(savedTicket.Joker);
				IoC.Get<IStateMachine>().TicketDetails.Joker.Clear();
				IoC.Get<IStateMachine>().TicketDetails.Joker.AddRange(savedTicket.Joker);
			}

			NumbersChanged = true;
		}

		private void JokerChanged(bool isAvailable)
		{
			IoC.Get<IStateMachine>().TicketDetails.IsJokerAvailable = isAvailable;

			JokerNumbers.Clear();
			IoC.Get<IStateMachine>().TicketDetails.Joker.Clear();

			if (!isAvailable)
				return;

			// generate new joker numbers
			var jokerNumbers = Utils.RandomJokerGenerator();
			IoC.Get<IStateMachine>().TicketDetails.Joker.Clear();
			IoC.Get<IStateMachine>().TicketDetails.Joker.AddRange(jokerNumbers);

			if (JokerNumbers == null)
				JokerNumbers = new BindableCollection<int>();

			JokerNumbers.AddRange(jokerNumbers);
		}

		private void SetGameParameters()
		{
			var gt = (LotteryTypes)Utils.StringToEnum(selectedGameType);
			MaxValue = Utils.GetGameMaxNumber(gt);
			NumberNo = Utils.GetGameNumberNo(gt);
		}

		private void SetGameWeekTypes()
		{
			GameWeekTypes = new List<string>();
			var enumGameWeekTypes = Utils.EnumToList(new LotteryGameTypes());
			foreach (var enumGameWeekType in enumGameWeekTypes)
			{
				GameWeekTypes.Add(Utils.EnumToString(enumGameWeekType));
			}
		}

		private void SetGameTypeValues()
		{
			GameTypes = new List<string>();
			var enumGameTypes = Utils.EnumToList(new LotteryTypes());
			foreach (var enumGameType in enumGameTypes)
			{
				var enumText = Utils.EnumToString(enumGameType);
				if (enumText != String.Empty)
					GameTypes.Add(enumText);
			}
		}

		private TicketDetails GetFavoriteTicket(LotteryTypes gameType)
		{
			var savedTickets = IoC.Get<IStateMachine>().SavedLottoTickets;
			switch (gameType)
			{
				case LotteryTypes.Lotto:
					if (savedTickets.LottoTicket != null)
						return savedTickets.LottoTicket;
					break;
				case LotteryTypes.LottoSix:
					if (savedTickets.LottoSixTicket != null)
						return savedTickets.LottoSixTicket;
					break;
				case LotteryTypes.Skandinavian:
					if (savedTickets.SkandinavianLottoTicket != null)
						return savedTickets.SkandinavianLottoTicket;
					break;
				case LotteryTypes.Undefined:
					return null;
			}

			return null;
		}
	}
}
