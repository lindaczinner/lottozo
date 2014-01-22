using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.Model;

namespace LottozoCore.Helpers
{
	public static class Utils
	{
		public static List<int> RandomNumbersGenerator(int amount, int maxValue)
		{
			if (amount > maxValue)
				throw new Exception("Cannot generate distinct numbers");

			var nums = new List<int>();
			var rnd = new Random();
			for (var i = 0; i < amount; i++)
			{
				int num;
				do
				{
					num = rnd.Next(1, maxValue + 1);
				} while (nums.Contains(num));
				nums.Add(num);
			}

			nums.Sort();

			return nums;
		}

		public static List<int> RandomJokerGenerator()
		{
			var nums = new List<int>();
			var rnd = new Random();
			for (int i = 0; i < JokerData.NumberNo; i++)
			{
				int num = rnd.Next(0, JokerData.MaxValue + 1);
				nums.Add(num);
			}

			return nums;
		}

		public static List<Enum> EnumToList(Enum enumeration)
		{
			//get the enumeration type
			var et = enumeration.GetType();

			//get the public static fields (members of the enum)
			var fi = et.GetFields(BindingFlags.Static | BindingFlags.Public);

			return fi.Select(t => (Enum)t.GetValue(enumeration)).ToList();
		}

		public static string EnumToString(Enum e)
		{
			switch (e.ToString())
			{
				case "Lotto":
					return "Ötös Lottó";
				case "LottoSix":
					return "Hatos Lottó";
				case "Skandinavian":
					return "Skandináv Lottó";
				case "OneWeek":
					return "Egy hetes";
				case "FiveWeeks":
					return "Öt hetes";
				default:
					return String.Empty;
			}
		}

		public static Enum StringToEnum(string s)
		{
			switch (s)
			{
				case "Ötös Lottó":
					return LotteryTypes.Lotto;
				case "Hatos Lottó":
					return LotteryTypes.LottoSix;
				case "Skandináv Lottó":
					return LotteryTypes.Skandinavian;
				case "Egy hetes":
					return LotteryGameTypes.OneWeek;
				case "Öt hetes":
					return LotteryGameTypes.FiveWeeks;
				default:
					throw new Exception("Cannot convert to Enum: " + s);
			}
		}

		public static void PutIntoSavedTickets(TicketDetails ticketDetails)
		{
			switch (ticketDetails.LotteryTypes)
			{
				case LotteryTypes.Lotto:
					IoC.Get<IStateMachine>().SavedLottoTickets.LottoTicket = ticketDetails;
					break;
				case LotteryTypes.LottoSix:
					IoC.Get<IStateMachine>().SavedLottoTickets.LottoSixTicket = ticketDetails;
					break;
				case LotteryTypes.Skandinavian:
					IoC.Get<IStateMachine>().SavedLottoTickets.SkandinavianLottoTicket = ticketDetails;
					break;
				case LotteryTypes.Undefined:
					break;
			}
		}

		public static int GetGameMaxNumber(LotteryTypes lotteryType)
		{
			switch (lotteryType)
			{
				case LotteryTypes.Lotto:
					return LottoData.MaxValue;
				case LotteryTypes.LottoSix:
					return LottoSixData.MaxValue;
				case LotteryTypes.Skandinavian:
					return SkandinavianLottoData.MaxValue;
			}

			return 0;
		}

		public static int GetGameNumberNo(LotteryTypes lotteryType)
		{
			switch (lotteryType)
			{
				case LotteryTypes.Lotto:
					return LottoData.NumberNo;
				case LotteryTypes.LottoSix:
					return LottoSixData.NumberNo;
				case LotteryTypes.Skandinavian:
					return SkandinavianLottoData.NumberNo;
			}

			return 0;
		}
	}
}