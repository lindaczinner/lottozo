using System;
using System.IO;
using System.Net;
using Caliburn.Micro;
using LottozoCore.Helpers;
using LottozoCore.Model;

namespace LottozoCore.ViewModels
{
	public partial class MainViewModel
	{
		private bool isLottoSixErrorHappened;
		public bool IsLottoSixErrorHappened
		{
			get { return isLottoSixErrorHappened; }
			set
			{
				isLottoSixErrorHappened = value;
				NotifyOfPropertyChange(() => IsLottoSixErrorHappened);
				NotifyOfPropertyChange(() => LottoSixNumbers);
				NotifyOfPropertyChange(() => IsLottoSixDetailButtonVisible);
			}
		}

		private bool isNoLottoSixResult;
		public bool IsNoLottoSixResult
		{
			get { return isNoLottoSixResult; }
			set
			{
				isNoLottoSixResult = value;
				NotifyOfPropertyChange(() => IsNoLottoSixResult);
				NotifyOfPropertyChange(() => IsLottoSixDetailButtonVisible);
			}
		}

		public bool IsLottoSixDetailButtonVisible
		{
			get { return !IsNoLottoSixResult && !IsLottoSixErrorHappened; }
		}

		public string LottoSixTitle
		{
			get
			{
				return LottoSix != null ? "Hatos lottó (" + LottoSix.Date.Date.ToString("MM.dd.") + ")" : "Hatos lottó";
			}
		}

		public string LottoSixNumbers
		{
			get
			{
				if (isLottoSixErrorHappened)
					return Constants.NoNumbersErrorMessage;
				return LottoSix != null ? LottoSix.ToString() : String.Empty;
			}
		}

		public LottoSixData LottoSix
		{
			get
			{
				return stateMachine.LottoSix;
			}
			set
			{
				stateMachine.LottoSix = value;
				NotifyOfPropertyChange(() => LottoSix);
				NotifyOfPropertyChange(() => LottoSixNumbers);
				NotifyOfPropertyChange(() => LottoSixTitle);
			}
		}

		public void GoToLottoSixPage()
		{
			IoC.Get<INavigationService>().UriFor<LottoSixViewModel>().Navigate();
		}

		private void LoadLottoSixData()
		{
			double cacheValue = stateMachine.LastRefreshed;

			var webClient = new WebClient();
			var address = new Uri(Constants.LottoSixUrl + cacheValue);
			webClient.OpenReadCompleted += LottoSix_OpenReadCompleted;
			webClient.OpenReadAsync(address);
		}

		void LottoSix_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				IsLottoSixErrorHappened = true;
				IsNoLottoSixResult = false;
				return;
			}

			var data = e.Result;
			using (var sr = new StreamReader(data))
			{
				var line = sr.ReadLine();
				var parts = CsvHelper.SplitText(line);
				if (parts == null)
				{
					IsLottoSixErrorHappened = true;
				}
				else
				{
					var lottoSix = DataMapper.LottoSixDataMapper(parts);
					if (lottoSix == null)
						IsLottoSixErrorHappened = true;
					else
						LottoSix = lottoSix;
				}
				IsNoLottoSixResult = false;
			}
		}
	}
}
