using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.Model;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public class FieldsSetUpViewModel : Screen
	{
		public string AppTitle
		{
			get { return Constants.AppName; }
		}

		public string ViewTitle
		{
			get { return "Mezők"; }
		}

		private int maxNo;

		private int numberNo;

		private bool numbersChanged;
		public bool NumbersChanged
		{
			get { return numbersChanged; }
			set
			{
				numbersChanged = value;
				NotifyOfPropertyChange("NumbersChanged");
				FieldDeleted = false;
			}
		}

		private bool fieldDeleted;
		public bool FieldDeleted
		{
			get { return fieldDeleted; }
			set
			{
				fieldDeleted = value;
				NotifyOfPropertyChange("FieldDeleted");

				if (fieldDeleted)
					NumbersChanged = true;
			}
		}

		private bool isDeletable;
		public bool IsDeletable
		{
			get { return isDeletable; }
			set
			{
				isDeletable = value;
				NotifyOfPropertyChange("IsDeletable");
			}
		}

		private bool isAddable;
		public bool IsAddable
		{
			get { return isAddable; }
			set
			{
				isAddable = value;
				NotifyOfPropertyChange("IsAddable");
			}
		}

		public FieldsSetUpViewModel()
		{
			SystemTray.IsVisible = false;

			var ticketDetails = IoC.Get<IStateMachine>().TicketDetails;
			maxNo = Helpers.Utils.GetGameMaxNumber(ticketDetails.LotteryTypes);
			numberNo = Helpers.Utils.GetGameNumberNo(ticketDetails.LotteryTypes);
			IsDeletable = ticketDetails.Fields.Count != 1;
			IsAddable = ticketDetails.Fields.Count != Constants.MaxFieldNo;
		}

		public void AddNewField()
		{
			var ticketDetails = IoC.Get<IStateMachine>().TicketDetails;

			if (ticketDetails.Fields.Count == Constants.MaxFieldNo)
				return;

			if (ticketDetails.Fields.Count == Constants.MaxFieldNo - 1)
				IsAddable = false;

			IsDeletable = true;

			var newField = new Field
				{
					FieldNo = ticketDetails.Fields.Count + 1,
					Numbers = Helpers.Utils.RandomNumbersGenerator(numberNo, maxNo)
				};

			ticketDetails.Fields.Add(newField);

			NumbersChanged = true;
		}
	}
}
