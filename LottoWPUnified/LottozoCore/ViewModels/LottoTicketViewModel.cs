using System.Collections.Generic;
using System.Linq;
using LottozoCore.Interfaces;
using LottozoCore.Model;
using Caliburn.Micro;
using Microsoft.Phone.Shell;

namespace LottozoCore.ViewModels
{
	public class LottoTicketViewModel : Screen
	{
		public string AppTitle
		{
			get { return Constants.AppName; }
		}

		public string ViewTitle
		{
			get
			{
				TicketDetails ticketDetails = IoC.Get<IStateMachine>().TicketDetails;

				if (ticketDetails == null)
					return string.Empty;

				switch (ticketDetails.LotteryTypes)
				{
					case LotteryTypes.LottoSix:
						return "Hatos szelvény";
					case LotteryTypes.Skandinavian:
						return "Skandi szelvény";
					default:
						return "Ötös szelvény";
				}
			}
		}

		private int maxValue;

		public int MaxValue
		{
			get { return maxValue; }
			set
			{
				maxValue = value;
				NotifyOfPropertyChange("MaxValue");
			}
		}

		private int numberNo;

		public int NumberNo
		{
			get { return numberNo; }
			set
			{
				numberNo = value;
				NotifyOfPropertyChange("NumberNo");
			}
		}

		private int fieldNo;

		public int FieldNo
		{
			get { return fieldNo; }
			set
			{
				fieldNo = value;
				NotifyOfPropertyChange("FieldNo");
			}
		}

		public bool IsError
		{
			get
			{
				return LottoNumbers == null || LottoNumbers.Count != NumberNo;
			}
		}

		public BindableCollection<int?> LottoNumbers { get; set; }

		public void Loaded()
		{
			SystemTray.IsVisible = false;
		}

		public bool SaveNewNumbers()
		{
			var ticketDetails = IoC.Get<IStateMachine>().TicketDetails;

			if (ticketDetails == null || IsError || LottoNumbers.Any(p => !p.HasValue))
				return false;

			ticketDetails.Fields.Single(p => p.FieldNo == FieldNo).Numbers.Clear();
			var newNumbers = LottoNumbers.Select(lottoNumber => (int) lottoNumber).ToList();
			ticketDetails.Fields.Single(p => p.FieldNo == FieldNo).Numbers.AddRange(newNumbers);
			ticketDetails.Fields.Single(p => p.FieldNo == FieldNo).Numbers.Sort();

			return true;
		}
	}
}
