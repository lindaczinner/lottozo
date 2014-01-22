using System.Globalization;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.Model;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public class SkandinavianLottoViewModel : Screen
	{
		private IStateMachine stateMachine = IoC.Get<IStateMachine>();

		public string PageName
		{
			get
			{
				return "Skandináv Lottó";
			}
		}

		public string TimeText
		{
			get
			{
				if (stateMachine.SkandinavianLotto != null)
				{
					return stateMachine.SkandinavianLotto.Date.Date.ToString("yyyy.MM.d.") + ", " + stateMachine.SkandinavianLotto.Week + ". játékhét";
				}
				return null;
			}
		}

		public string NumbersText
		{
			get
			{
				return "Kézi húzás:";
			}
		}


		public string FirstNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.Numbers[0].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SecondNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.Numbers[1].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string ThirdNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.Numbers[2].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string FourthNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.Numbers[3].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string FifthNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.Numbers[4].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SixthNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.Numbers[5].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SeventhNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.Numbers[6].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string MachineNumbersText
		{
			get
			{
				return "Gépi húzás:";
			}
		}

		public string FirstMachineNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.MachineNumbers[0].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SecondMachineNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.MachineNumbers[1].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string ThirdMachineNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.MachineNumbers[2].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string FourthMachineNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.MachineNumbers[3].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string FifthMachineNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.MachineNumbers[4].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SixthMachineNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.MachineNumbers[5].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string SeventhMachineNumber
		{
			get
			{
				return stateMachine.SkandinavianLotto.MachineNumbers[6].ToString(CultureInfo.InvariantCulture);
			}
		}

		public string Award
		{
			get
			{
				return "Nyeremények";
			}
		}

		public string SevenMatch
		{
			get
			{
				return "Hetes:";
			}
		}

		public string SevenAward
		{
			get
			{
				return stateMachine.SkandinavianLotto.SevenValue;
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
				return stateMachine.SkandinavianLotto.SixValue;
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
				return stateMachine.SkandinavianLotto.FiveValue;
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
				return stateMachine.SkandinavianLotto.FourValue;
			}
		}

		public SkandinavianLottoViewModel()
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
					.WithParam(p => p.NavigatedFrom, LotteryTypes.Skandinavian)
					.Navigate();
		}
	}
}