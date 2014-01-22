using Caliburn.Micro;
using LottozoCore.ViewModels;
using Microsoft.Phone.Controls;

namespace Lottoszamok.Views
{
	public partial class LottoTicketView : PhoneApplicationPage
	{
		public LottoTicketView()
		{
			InitializeComponent();
		}

		private void ApplicationBarOkIconButton_Click(object sender, System.EventArgs e)
		{
			if (IoC.Get<LottoTicketViewModel>().SaveNewNumbers())
				IoC.Get<INavigationService>().GoBack();
		}

		private void ApplicationBarRefreshIconButton_Click(object sender, System.EventArgs e)
		{
			ucTicket.GenerateRandomNumbers();
		}

		private void ApplicationBarCancelIconButton_Click(object sender, System.EventArgs e)
		{
			IoC.Get<INavigationService>().GoBack();
		}

		private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
		{
			ucTicket.ClearNumbers();
			//ucValidator.ClearNumbers();
		}
	}
}