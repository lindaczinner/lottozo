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
		private bool isLottoErrorHappened;
		public bool IsLottoErrorHappened
		{
			get { return isLottoErrorHappened; }
			set
			{
				isLottoErrorHappened = value;
				NotifyOfPropertyChange(() => IsLottoErrorHappened);
				NotifyOfPropertyChange(() => LottoNumbers);
				NotifyOfPropertyChange(() => IsLottoDetailButtonVisible);
			}
		}

		private bool isNoLottoResult;
		public bool IsNoLottoResult
		{
			get { return isNoLottoResult; }
			set
			{
				isNoLottoResult = value;
				NotifyOfPropertyChange(() => IsNoLottoResult);
				NotifyOfPropertyChange(() => IsLottoDetailButtonVisible);
			}
		}

		public bool IsLottoDetailButtonVisible
		{
			get { return !IsNoLottoResult && !IsLottoErrorHappened; }
		}

		public string LottoTitle
		{
			get
			{
				return Lotto != null ? "Ötös lottó (" + Lotto.Date.Date.ToString("MM.dd.") + ")" : "Ötös lottó";
			}
		}

		public string LottoNumbers
		{
			get
			{
				if (isLottoErrorHappened)
					return Constants.NoNumbersErrorMessage;
				return Lotto != null ? Lotto.ToString() : String.Empty;
			}
		}

		public LottoData Lotto
		{
			get
			{
				return stateMachine.Lotto;
			}
			set
			{
				stateMachine.Lotto = value;
				NotifyOfPropertyChange(() => Lotto);
				NotifyOfPropertyChange(() => LottoNumbers);
				NotifyOfPropertyChange(() => LottoTitle);
			}
		}

		public void GoToLottoPage()
		{
			IoC.Get<INavigationService>().UriFor<LottoViewModel>().Navigate();
		}

		private void LoadLottoData()
		{
			double cacheValue = stateMachine.LastRefreshed;

			var webClient = new WebClient();
			var address = new Uri(Constants.LottoUrl + cacheValue);
			webClient.OpenReadCompleted += webClient_OpenReadCompleted;
			webClient.OpenReadAsync(address);
		}

		void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				IsLottoErrorHappened = true;
				IsNoLottoResult = false;
				return;
			}

			var data = e.Result;
			using (var sr = new StreamReader(data))
			{
				var line = sr.ReadLine();
				var parts = CsvHelper.SplitText(line);
				if (parts == null)
				{
					IsLottoErrorHappened = true;
				}
				else
				{
					var lotto = DataMapper.LottoDataMapper(parts);
					if (lotto == null)
						IsLottoErrorHappened = true;
					else
						Lotto = lotto;
				}
				IsNoLottoResult = false;
			}
		}
	}
}
