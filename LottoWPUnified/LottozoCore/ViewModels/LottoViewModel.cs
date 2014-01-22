using System.Globalization;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.Model;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public class LottoViewModel : Screen
	{
		private IStateMachine stateMachine = IoC.Get<IStateMachine>();

		public string PageName
		{
			get
			{
				return "Ötös Lottó";
			}
		}

		public string TimeText
		{
			get
			{
				if (stateMachine.Lotto != null)
				{
					return stateMachine.Lotto.Date.Date.ToString("yyyy.MM.d.") + ", " + stateMachine.Lotto.Week + ". játékhét";
				}
				return null;
			}
		}

		public string FirstNumber
		{
			get
			{
				return stateMachine.Lotto.Numbers[0].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SecondNumber
		{
			get
			{
				return stateMachine.Lotto.Numbers[1].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string ThirdNumber
		{
			get
			{
				return stateMachine.Lotto.Numbers[2].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string FourthNumber
		{
			get
			{
				return stateMachine.Lotto.Numbers[3].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string FifthNumber
		{
			get
			{
				return stateMachine.Lotto.Numbers[4].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string Award
		{
			get
			{
				return "Nyeremények";
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
				return stateMachine.Lotto.FiveValue;
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
				return stateMachine.Lotto.FourValue;
			}
		}

		public string ThreeMatch
		{
			get { return "Hármas:"; }
		}

		public string ThreeAward
		{
			get
			{
				return stateMachine.Lotto.ThreeValue;
			}
		}

		public string TwoMatch
		{
			get { return "Kettes:"; }
		}

		public string TwoAward
		{
			get
			{
				return stateMachine.Lotto.TwoValue;
			}
		}

		public LottoViewModel()
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
					.WithParam(p => p.NavigatedFrom, LotteryTypes.Lotto)
					.Navigate();
		}
	}
}