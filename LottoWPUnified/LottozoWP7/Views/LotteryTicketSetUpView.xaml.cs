using Caliburn.Micro;
using Lottoszamok.Utils;
using LottozoCore.Interfaces;
using Microsoft.Phone.Controls;
using System.Windows;
using LottozoCore.Helpers;

namespace Lottoszamok.Views
{
	public partial class LotteryTicketSetUpView : PhoneApplicationPage
	{
		public LotteryTicketSetUpView()
		{
			InitializeComponent();
		}

		private void JokerToggle_Checked(object sender, RoutedEventArgs e)
		{
			scroll.ScrollToVerticalOffset(10000);
		}

		private void JokerToggle_Unchecked(object sender, RoutedEventArgs e)
		{
			scroll.ScrollToVerticalOffset(0);
		}

		private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
		{
			LottozoCore.Helpers.Utils.PutIntoSavedTickets(IoC.Get<IStateMachine>().TicketDetails);
			IoC.Get<StorageHelper>().Save(IoC.Get<IStateMachine>().SavedLottoTickets);
			SmsHelper.SendTextMessage();
		}
	}
}