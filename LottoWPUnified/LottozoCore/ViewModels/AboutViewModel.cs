using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Caliburn.Micro;

namespace LottozoCore.ViewModels
{
	public class AboutViewModel : Screen
	{
		public string PageName
		{
			get { return "Névjegy"; }
		}

		public string FeedbackText
		{
			get { return "Alkalmazás értékelése"; }
		}

		public AboutViewModel()
		{
			SystemTray.IsVisible = false;
		}

		public void CreateReview()
		{
			var task = new MarketplaceReviewTask();
			task.Show();
		}
	}
}