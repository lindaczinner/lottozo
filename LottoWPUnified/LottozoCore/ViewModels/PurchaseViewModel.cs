using Caliburn.Micro;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace LottozoCore.ViewModels
{
	public class PurchaseViewModel : Screen
	{
		public PurchaseViewModel()
		{
			SystemTray.IsVisible = false;
		}

		public string ApplicationTitle { get { return Constants.AppName; } }

		public string PageName
		{
			get
			{
				return "Próba verzió";
			}
		}

		public void PurchaseApp()
		{
			var marketplaceDetailTask = new MarketplaceDetailTask();
			marketplaceDetailTask.Show();
		}

	}
}
