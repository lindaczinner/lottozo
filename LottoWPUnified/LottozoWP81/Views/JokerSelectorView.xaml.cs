using Caliburn.Micro;
using LottozoCore.ViewModels;
using Microsoft.Phone.Controls;

namespace Lottozo.Views
{
	public partial class JokerSelectorView : PhoneApplicationPage
	{
		public JokerSelectorView()
		{
			InitializeComponent();
		}

		private void ApplicationBarOkIconButton_Click(object sender, System.EventArgs e)
		{
			if (IoC.Get<JokerSelectorViewModel>().SaveNewJoker())
				IoC.Get<INavigationService>().GoBack();
		}

		private void ApplicationBarCancelIconButton_Click(object sender, System.EventArgs e)
		{
			IoC.Get<INavigationService>().GoBack();
		}

		private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
		{
			ucJoker1.JokerNumber = null;
			ucJoker2.JokerNumber = null;
			ucJoker3.JokerNumber = null;
			ucJoker4.JokerNumber = null;
			ucJoker5.JokerNumber = null;
			ucJoker6.JokerNumber = null;
		}
	}
}