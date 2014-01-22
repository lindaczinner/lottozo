using System;
using System.Collections.Generic;
using Caliburn.Micro;
using LottozoCore.Model;
using LottozoCore.Interfaces;

namespace Lottoszamok.Utils
{
	public class StateMachine : IStateMachine
	{
		public LottoData Lotto { get; set; }

		public LottoSixData LottoSix { get; set; }

		public SkandinavianLottoData SkandinavianLotto { get; set; }

		public JokerData Joker { get; set; }

		public TicketDetails TicketDetails { get; set; }

		public double LastRefreshed { get; set; }

		public bool IsRefresh { get; set; }

		public SavedTickets SavedLottoTickets { get; set; }

		public TicketDetails TempStoredTicket { get; set; }

		public StateMachine()
		{
			TicketDetails = new TicketDetails
			{
				LotteryTypes = LotteryTypes.Lotto,
				GameTypes = LotteryGameTypes.OneWeek,
				Fields = new List<Field>(),
				Joker = new List<int>()
			};

			SavedLottoTickets = new SavedTickets();

			LastRefreshed = DateTime.Now.TimeOfDay.TotalSeconds;

			//ThreadPool.QueueUserWorkItem(async p =>
			//{
			//	var savedTicketDetails = await IoC.Get<StorageHelper>().LoadAsync<TicketDetails>();

			//	if (savedTicketDetails != null)
			//		TicketDetails = savedTicketDetails;
			//});
		}

		public void FillStateMachine(IStateMachine stateMachine)
		{
			Lotto = stateMachine.Lotto;
			LottoSix = stateMachine.LottoSix;
			SkandinavianLotto = stateMachine.SkandinavianLotto;
			Joker = stateMachine.Joker;
			TicketDetails = stateMachine.TicketDetails;
			LastRefreshed = stateMachine.LastRefreshed;
			IsRefresh = stateMachine.IsRefresh;
		}

		public void GetSavedTickets()
		{
			GetSavedTicketsAsync();
		}

		private void GetSavedTicketsAsync()
		{
			var savedTicketDetails = IoC.Get<StorageHelper>().Load<SavedTickets>();
			if (savedTicketDetails != null)
				SavedLottoTickets = savedTicketDetails;
		}
	}
}
