using Caliburn.Micro;
using LottozoCore.Interfaces;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public class JokerViewModel : Screen
	{
		private IStateMachine stateMachine = IoC.Get<IStateMachine>();

		public string PageName
		{
			get
			{
				return "Joker";
			}
		}

		public string TimeText
		{
			get
			{
				if (stateMachine.Joker != null)
				{
					return stateMachine.Joker.Date.Date.ToString("yyyy.MM.d.") + ", " + stateMachine.Joker.Week + ". játékhét";
				}
				return null;
			}
		}

		public string FirstNumber
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.Numbers[0].ToString() : null;
			}
		}

		public string SecondNumber
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.Numbers[1].ToString() : null;
			}
		}

		public string ThirdNumber
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.Numbers[2].ToString() : null;
			}
		}

		public string FourthNumber
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.Numbers[3].ToString() : null;
			}
		}

		public string FifthNumber
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.Numbers[4].ToString() : null;
			}
		}

		public string SixthNumber
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.Numbers[5].ToString() : null;
			}
		}

		public string Award
		{
			get
			{
				return "Nyeremények";
			}
		}

		public string SixMatch
		{
			get
			{
				return "Hatos:";
			}
		}

		public string SixAward
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.SixValue : null;
			}
		}

		public string FiveMatch
		{
			get
			{
				return "Ötös:";
			}
		}

		public string FiveAward
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.FiveValue : null;
			}
		}

		public string FourMatch
		{
			get
			{
				return "Négyes:";
			}
		}

		public string FourAward
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.FourValue : null;
			}
		}

		public string ThreeMatch
		{
			get
			{
				return "Hármas:";
			}
		}

		public string ThreeAward
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.ThreeValue : null;
			}
		}

		public string TwoMatch
		{
			get
			{
				return "Kettes:";
			}
		}

		public string TwoAward
		{
			get
			{
				return stateMachine.Joker != null ? stateMachine.Joker.TwoValue : null;
			}
		}

		public JokerViewModel()
		{
			SystemTray.IsVisible = false;
		}
	}
}