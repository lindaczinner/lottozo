using LottozoCore.Model;

namespace LottozoCore.Interfaces
{
	public interface IStateMachine
	{
		LottoData Lotto { get; set; }

		LottoSixData LottoSix { get; set; }

		SkandinavianLottoData SkandinavianLotto { get; set; }

		JokerData Joker { get; set; }

		TicketDetails TicketDetails { get; set; }

		TicketDetails TempStoredTicket { get; set; }

		double LastRefreshed { get; set; }

		bool IsRefresh { get; set; }

		SavedTickets SavedLottoTickets { get; set; }

		void FillStateMachine(IStateMachine stateMachine);

		void GetSavedTickets();
	}
}
