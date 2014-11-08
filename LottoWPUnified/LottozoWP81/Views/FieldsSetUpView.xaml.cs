using System;
using System.ComponentModel;
using Caliburn.Micro;
using Microsoft.Phone.Controls;
using LottozoCore.Interfaces;

namespace Lottozo.Views
{
	public partial class FieldsSetUpView : PhoneApplicationPage
	{
		public FieldsSetUpView()
		{
			InitializeComponent();
		}

		private void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
		{
			IoC.Get<INavigationService>().GoBack();
		}

		private void FieldsSetUpView_OnBackKeyPress(object sender, CancelEventArgs e)
		{
			var sm = IoC.Get<IStateMachine>();
			sm.TicketDetails = sm.TempStoredTicket.Clone();
			sm.TempStoredTicket = null;
		}

		private void ApplicationBarCancelIconButton_Click(object sender, EventArgs e)
		{
			var sm = IoC.Get<IStateMachine>();
			sm.TicketDetails = sm.TempStoredTicket.Clone();
			sm.TempStoredTicket = null;
			IoC.Get<INavigationService>().GoBack();
		}
		
	}
}