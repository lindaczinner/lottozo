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
		private bool isJokerErrorHappened;
		public bool IsJokerErrorHappened
		{
			get { return isJokerErrorHappened; }
			set
			{
				isJokerErrorHappened = value;
				NotifyOfPropertyChange(() => IsJokerErrorHappened);
				NotifyOfPropertyChange(() => JokerNumbers);
				NotifyOfPropertyChange(() => IsJokerDetailButtonVisible);
			}
		}

		private bool isNoJokerResult;
		public bool IsNoJokerResult
		{
			get { return isNoJokerResult; }
			set
			{
				isNoJokerResult = value;
				NotifyOfPropertyChange(() => IsNoJokerResult);
				NotifyOfPropertyChange(() => IsJokerDetailButtonVisible);
			}
		}

		public bool IsJokerDetailButtonVisible
		{
			get { return !IsNoJokerResult && !IsJokerErrorHappened; }
		}

		public string JokerTitle
		{
			get
			{
				return Joker != null ? "Joker (" + Joker.Date.Date.ToString("MM.dd.") + ")" : "Joker";
			}
		}

		public string JokerNumbers
		{
			get
			{
				if (isJokerErrorHappened)
					return Constants.NoNumbersErrorMessage;
				return Joker != null ? Joker.ToString() : String.Empty;
			}
		}

		public JokerData Joker
		{
			get
			{
				return stateMachine.Joker;
			}
			set
			{
				stateMachine.Joker = value;
				NotifyOfPropertyChange(() => Joker);
				NotifyOfPropertyChange(() => JokerNumbers);
				NotifyOfPropertyChange(() => JokerTitle);
			}
		}

		public void GoToJokerPage()
		{
			IoC.Get<INavigationService>().UriFor<JokerViewModel>().Navigate();
		}

		private void LoadJokerData()
		{
			double cacheValue = stateMachine.LastRefreshed;

			var webClient = new WebClient();
			var address = new Uri(Constants.JokerUrl + cacheValue);
			webClient.OpenReadCompleted += Joker_OpenReadCompleted;
			webClient.OpenReadAsync(address);
		}

		void Joker_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				IsJokerErrorHappened = true;
				IsNoJokerResult = false;
				return;
			}

			var data = e.Result;
			using (var sr = new StreamReader(data))
			{
				var line = sr.ReadLine();
				var parts = CsvHelper.SplitText(line);
				if (parts == null)
					IsJokerErrorHappened = true;
				else
				{
					var joker = DataMapper.JokerDataMapper(parts);
					if (joker == null)
						IsJokerErrorHappened = true;
					else
						Joker = joker;
				}
				IsNoJokerResult = false;
			}
		}
	}
}
