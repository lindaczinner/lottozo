using System.Collections.Generic;

namespace LottozoCore.Model
{
	public class TicketDetails
	{
		public LotteryGameTypes GameTypes { get; set; }

		public LotteryTypes LotteryTypes { get; set; }

		public List<Field> Fields { get; set; }

		public bool IsJokerAvailable { get; set; }

		public List<int> Joker { get; set; }

		public TicketDetails Clone()
		{
			var newInstance = new TicketDetails
				{
					GameTypes = this.GameTypes,
					LotteryTypes = this.LotteryTypes,
					Fields = new List<Field>(),
					IsJokerAvailable = this.IsJokerAvailable,
					Joker = new List<int>()
				};

			foreach (var field in this.Fields)
			{
				newInstance.Fields.Add(field.Clone());
			}
			
			newInstance.Joker.AddRange(this.Joker);

			return newInstance;
		}
	}

	public class Field
	{
		public int FieldNo { get; set; }

		public List<int> Numbers { get; set; } 

		public Field Clone()
		{
			var newInstance = new Field
				{
					FieldNo = this.FieldNo,
					Numbers = new List<int>()
				};

			newInstance.Numbers.AddRange(this.Numbers);

			return newInstance;
		}
	}
}
