using Caliburn.Micro;
using LottozoCore.Interfaces;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public partial class MainViewModel : Screen
	{
		private IStateMachine stateMachine = IoC.Get<IStateMachine>();

		public string ApplicationTitle { get { return Constants.AppName; } }

		public int RandomNumberForRefresh { get; set; }

		public string PageName
		{
			get { return "Nyerőszámok"; }
		}

		public bool IsTrial
		{
			get { return IoC.Get<bool>("IsTrial"); }
		}

		/// <summary>
		/// Initializes a new instance of the MainViewModel class.
		/// </summary>
		public MainViewModel()
		{
			SystemTray.IsVisible = false;

			if (stateMachine.Lotto == null || stateMachine.IsRefresh)
				LoadLottoData();
			if (stateMachine.LottoSix == null || stateMachine.IsRefresh)
				LoadLottoSixData();
			if (stateMachine.SkandinavianLotto == null || stateMachine.IsRefresh)
				LoadSkandinavianLottoData();
			if (stateMachine.Joker == null || stateMachine.IsRefresh)
				LoadJokerData();

			Lotto = null;
			LottoSix = null;
			SkandinavianLotto = null;
			Joker = null;

			IsNoLottoResult = true;
			IsNoLottoSixResult = true;
			IsNoSkandinavianLottoResult = true;
			IsNoJokerResult = true;

			stateMachine.IsRefresh = false;

			//if page is refreshed, remove from backstack
			IoC.Get<INavigationService>().RemoveBackEntry();
		}
	}
}