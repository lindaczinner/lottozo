using System;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Lottozo.Views
{
	public partial class MainView : PhoneApplicationPage
	{
		// Constructor
		public MainView()
		{
			InitializeComponent();
		}

		private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
		{
			IoC.Get<INavigationService>().UriFor<AboutViewModel>().Navigate();
		}

		private void ApplicationBarAddIconButton_Click(object sender, EventArgs e)
		{
			if (IoC.Get<bool>("IsTrial"))
				IoC.Get<INavigationService>().UriFor<PurchaseViewModel>().Navigate();
			else
				IoC.Get<INavigationService>().UriFor<LotteryTicketSetUpViewModel>().Navigate();
		}

		private void ApplicationBarRefreshIconButton_Click(object sender, EventArgs e)
		{
			var stateMachine = IoC.Get<IStateMachine>();
			if (stateMachine != null)
			{
				stateMachine.LastRefreshed = DateTime.Now.TimeOfDay.TotalSeconds;
				stateMachine.IsRefresh = true;
			}

			IoC.Get<INavigationService>().UriFor<MainViewModel>().WithParam(p => p.RandomNumberForRefresh, new Random().Next(0, 100000)).Navigate();
			
		}
	}
}
