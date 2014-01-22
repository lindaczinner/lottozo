using System.Text;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.Model;
using Microsoft.Phone.Tasks;

namespace LottozoCore.Helpers
{
	public static class SmsHelper
	{
		public static void SendTextMessage()
		{
			var sct = new SmsComposeTask
			{
				To = Constants.TextNumber,
				Body = CreateText()
			};
			sct.Show();
		}

		private static string CreateText()
		{
			var ticketDetails = IoC.Get<IStateMachine>().TicketDetails;

			var sb = new StringBuilder();

			switch (ticketDetails.LotteryTypes)
			{
				case (LotteryTypes.Lotto):
					sb.Append("L5");
					break;
				case (LotteryTypes.LottoSix):
					sb.Append("L6");
					break;
				case (LotteryTypes.Skandinavian):
					sb.Append("LS");
					break;
			}

			if (ticketDetails.GameTypes == LotteryGameTypes.FiveWeeks)
				sb.Append("5,");
			else
				sb.Append(",");

			for (int i = 0; i < ticketDetails.Fields.Count; i++)
			{
				if (i != 0)
					sb.Append(",");

				foreach (var number in ticketDetails.Fields[i].Numbers)
				{
					sb.Append(number + ",");
				}
			}
			sb.Remove(sb.Length - 1, 1);

			if (!ticketDetails.IsJokerAvailable)
				return sb.ToString();

			sb.Append(",J");

			foreach (var joker in ticketDetails.Joker)
			{
				sb.Append(joker);
			}

			return sb.ToString();
		}
	}
}
