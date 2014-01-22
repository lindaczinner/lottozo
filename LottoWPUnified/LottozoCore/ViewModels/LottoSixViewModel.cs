using System.Globalization;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.Model;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public class LottoSixViewModel : Screen
	{
		private IStateMachine stateMachine = IoC.Get<IStateMachine>();

		public string PageName
		{
			get
			{
				return "Hatos Lottó";
			}
		}

		public string TimeText
		{
			get
			{
				if (stateMachine.LottoSix != null)
				{
					return stateMachine.LottoSix.Date.Date.ToString("yyyy.MM.d.") + ", " + stateMachine.LottoSix.Week + ". játékhét";
				}
				return null;
			}
		}

		public string FirstNumber
		{
			get
			{
				return stateMachine.LottoSix.Numbers[0].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SecondNumber
		{
			get
			{
				return stateMachine.LottoSix.Numbers[1].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string ThirdNumber
		{
			get
			{
				return stateMachine.LottoSix.Numbers[2].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string FourthNumber
		{
			get
			{
				return stateMachine.LottoSix.Numbers[3].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string FifthNumber
		{
			get
			{
				return stateMachine.LottoSix.Numbers[4].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SixthNumber
		{
			get
			{
				return stateMachine.LottoSix.Numbers[5].ToString(CultureInfo.InvariantCulture);
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
				return stateMachine.LottoSix.SixValue;
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
				return stateMachine.LottoSix.FiveValue;
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
				return stateMachine.LottoSix.FourValue;
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
				return stateMachine.LottoSix.ThreeValue;
			}
		}

		public LottoSixViewModel()
		{
			SystemTray.IsVisible = false;
		}

		public void GoToLottoTicket()
		{
			if (IoC.Get<bool>("IsTrial"))
				IoC.Get<INavigationService>().UriFor<PurchaseViewModel>().Navigate();
			else
				IoC.Get<INavigationService>()
					.UriFor<LotteryTicketSetUpViewModel>()
					.WithParam(p => p.NavigatedFrom, LotteryTypes.LottoSix)
					.Navigate();
		}
	}
}