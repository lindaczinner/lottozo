using System;
using System.Text;

namespace LottozoCore.Model
{
	public class SkandinavianLottoData
	{
		public const int MaxValue = 35;

		public const int NumberNo = 7;

        public const int MaxFieldValue = 5;

		public string Year { get; set; }

		public string Week { get; set; }

		public DateTime Date { get; set; }

		public int SevenNum { get; set; }

		public string SevenValue { get; set; }

		public int SixNum { get; set; }

		public string SixValue { get; set; }

		public int FivePlusOneNum { get; set; }

		public string FivePlusOneValue { get; set; }

		public int FiveNum { get; set; }

		public string FiveValue { get; set; }

		public int FourNum { get; set; }

		public string FourValue { get; set; }

		public int[] Numbers { get; set; }

		public int[] MachineNumbers { get; set; }

		public string MachineToString()
		{
			var sb = new StringBuilder();
			sb.Append("Gépi: ");
			Array.Sort(MachineNumbers);
			foreach (var number in MachineNumbers)
			{
				sb.Append(number);
				sb.Append(", ");
			}
			sb.Remove(sb.Length - 2, 2);
			return sb.ToString();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("Kézi: ");
			Array.Sort(Numbers);
			foreach (var number in Numbers)
			{
				sb.Append(number);
				sb.Append(", ");
			}
			sb.Remove(sb.Length - 2, 2);
			return sb.ToString();
		}
	}
}
