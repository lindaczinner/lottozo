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
		private bool isSkandinavianLottoErrorHappened;
		public bool IsSkandinavianLottoErrorHappened
		{
			get { return isSkandinavianLottoErrorHappened; }
			set
			{
				isSkandinavianLottoErrorHappened = value;
				NotifyOfPropertyChange(() => IsSkandinavianLottoErrorHappened);
				NotifyOfPropertyChange(() => SkandinavianLottoNumbers);
				NotifyOfPropertyChange(() => SkandinavianLottoMachineNumbers);
				NotifyOfPropertyChange(() => IsSkandinavianLottoDetailButtonVisible);
			}
		}

		private bool isNoSkandinavianLottoResult;
		public bool IsNoSkandinavianLottoResult
		{
			get { return isNoSkandinavianLottoResult; }
			set
			{
				isNoSkandinavianLottoResult = value;
				NotifyOfPropertyChange(() => IsNoSkandinavianLottoResult);
				NotifyOfPropertyChange(() => IsSkandinavianLottoDetailButtonVisible);
			}
		}

		public bool IsSkandinavianLottoDetailButtonVisible
		{
			get { return !IsNoSkandinavianLottoResult && !IsSkandinavianLottoErrorHappened; }
		}

		public string SkandinavianLottoTitle
		{
			get { return SkandinavianLotto != null ? "Skandináv lottó (" + SkandinavianLotto.Date.Date.ToString("MM.dd.") + ")" : "Skandináv lottó"; }
		}

		public string SkandinavianLottoNumbers
		{
			get
			{
				if (isSkandinavianLottoErrorHappened)
					return Constants.NoNumbersErrorMessage;
				return SkandinavianLotto != null ? SkandinavianLotto.ToString() : String.Empty;
			}
		}

		public string SkandinavianLottoMachineNumbers
		{
			get
			{
				if (isSkandinavianLottoErrorHappened)
					return Constants.NoNumbersErrorMessage;
				return SkandinavianLotto != null ? SkandinavianLotto.MachineToString() : String.Empty;
			}
		}

		public SkandinavianLottoData SkandinavianLotto
		{
			get
			{
				return stateMachine.SkandinavianLotto;
			}
			set
			{
				stateMachine.SkandinavianLotto = value;
				NotifyOfPropertyChange(() => SkandinavianLotto);
				NotifyOfPropertyChange(() => SkandinavianLottoNumbers);
				NotifyOfPropertyChange(() => SkandinavianLottoMachineNumbers);
				NotifyOfPropertyChange(() => SkandinavianLottoTitle);
			}
		}

		public void GoToSkandinavianLottoPage()
		{
			IoC.Get<INavigationService>().UriFor<SkandinavianLottoViewModel>().Navigate();
		}

		private void LoadSkandinavianLottoData()
		{
			double cacheValue = stateMachine.LastRefreshed;

			var webClient = new WebClient();
			var address = new Uri(Constants.SkandinavianLottoUrl + cacheValue);
			webClient.OpenReadCompleted += SkandinavianLotto_OpenReadCompleted;
			webClient.OpenReadAsync(address);
		}

		void SkandinavianLotto_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				IsSkandinavianLottoErrorHappened = true;
				IsNoSkandinavianLottoResult = false;
				return;
			}

			var data = e.Result;
			using (var sr = new StreamReader(data))
			{
				var line = sr.ReadLine();
				var parts = CsvHelper.SplitText(line);
				if (parts == null)
				{
					IsSkandinavianLottoErrorHappened = true;
				}
				else
				{
					var skandinavianLotto = DataMapper.SkandinavianLottoDataMapper(parts);
					if (skandinavianLotto == null)
						IsSkandinavianLottoErrorHappened = true;
					else
						SkandinavianLotto = skandinavianLotto;
				}
				IsNoSkandinavianLottoResult = false;
			}
		}
	}
}
