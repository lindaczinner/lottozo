using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.Model;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public class JokerSelectorViewModel : Screen
	{
		private int? firstJoker;
		public int? FirstJoker
		{
			get { return firstJoker; }
			set
			{
				if (JokerNumbers == null)
					JokerNumbers = new BindableCollection<int?> { null, null, null, null, null, null };

				JokerNumbers[0] = firstJoker = value;
				NotifyOfPropertyChange("FirstJoker");
			}
		}

		private int? secondJoker;
		public int? SecondJoker
		{
			get { return secondJoker; }
			set
			{
				if (JokerNumbers == null)
					JokerNumbers = new BindableCollection<int?> { null, null, null, null, null, null };

				JokerNumbers[1] = secondJoker = value;
				NotifyOfPropertyChange("SecondJoker");
			}
		}

		private int? thirdJoker;
		public int? ThirdJoker
		{
			get { return thirdJoker; }
			set
			{
				if (JokerNumbers == null)
					JokerNumbers = new BindableCollection<int?> { null, null, null, null, null, null };

				JokerNumbers[2] = thirdJoker = value;
				NotifyOfPropertyChange("ThirdJoker");
			}
		}

		private int? fourthJoker;
		public int? FourthJoker
		{
			get { return fourthJoker; }
			set
			{
				if (JokerNumbers == null)
					JokerNumbers = new BindableCollection<int?> { null, null, null, null, null, null };

				JokerNumbers[3] = fourthJoker = value;
				NotifyOfPropertyChange("FourthJoker");
			}
		}

		private int? fifthJoker;
		public int? FifthJoker
		{
			get { return fifthJoker; }
			set
			{
				if (JokerNumbers == null)
					JokerNumbers = new BindableCollection<int?> { null, null, null, null, null, null };

				JokerNumbers[4] = fifthJoker = value;
				NotifyOfPropertyChange("FifthJoker");
			}
		}

		private int? sixthJoker;
		public int? SixthJoker
		{
			get { return sixthJoker; }
			set
			{
				if (JokerNumbers == null)
					JokerNumbers = new BindableCollection<int?> { null, null, null, null, null, null };

				JokerNumbers[5] = sixthJoker = value;
				NotifyOfPropertyChange("SixthJoker");
			}
		}

		public bool IsError
		{
			get
			{
				return (FirstJoker == null || SecondJoker == null || ThirdJoker == null || 
						FourthJoker == null || FifthJoker == null || SixthJoker == null);
			}
		}

		public BindableCollection<int?> JokerNumbers { get; set; }

		public string AppTitle
		{
			get { return Constants.AppName; }
		}

		public string ViewTitle
		{
			get { return "Joker számok"; }
		}

		public void Loaded()
		{
			SystemTray.IsVisible = false;
		}

		public bool SaveNewJoker()
		{
			var ticketDetails = IoC.Get<IStateMachine>().TicketDetails;

			if (ticketDetails == null || IsError)
				return false;

			ticketDetails.Joker.Clear();
			ticketDetails.Joker.Add((int)FirstJoker);
			ticketDetails.Joker.Add((int)SecondJoker);
			ticketDetails.Joker.Add((int)ThirdJoker);
			ticketDetails.Joker.Add((int)FourthJoker);
			ticketDetails.Joker.Add((int)FifthJoker);
			ticketDetails.Joker.Add((int)SixthJoker);

			return true;
		}
	}
}
