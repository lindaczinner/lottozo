using System;
using System.Collections.Generic;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.Model;

namespace LottozoCore.Helpers
{
	public static class DataMapper
	{
		public static LottoData LottoDataMapper(string[] props)
		{
			try
			{
				var lottoData = new LottoData()
				{
					Year = props[0],
					Week = props[1],
					Date = CreateDateTime(props[2]),
					FiveNum = Convert.ToInt32(props[3]),
					FiveValue = props[4],
					FourNum = Convert.ToInt32(props[5]),
					FourValue = props[6],
					ThreeNum = Convert.ToInt32(props[7]),
					ThreeValue = props[8],
					TwoNum = Convert.ToInt32(props[9]),
					TwoValue = props[10],
					Numbers = new int[5]
				};
				for (int i = 0; i < 5; ++i)
				{
					lottoData.Numbers[i] = Convert.ToInt32(props[i + 11]);
				}

				return lottoData;
			}
			catch (Exception ex)
			{
				var attributes = new Dictionary<String, String>();

				if (props != null)
				{
					for (int i = 0; i < props.Length; i++)
					{
						attributes.Add("props" + i, props[i]);
					}
				}

				LogHelper.LogException("LottoDataMapper exception", ex, attributes);
				IoC.Get<IBugsenseHelper>().LogBugsenseException(ex, attributes);

				return null;
			}

		}

		public static LottoSixData LottoSixDataMapper(string[] props)
		{
			try
			{
				var lottoData = new LottoSixData()
				{
					Year = props[0],
					Week = props[1],
					Date = CreateDateTime(props[2]),
					SixNum = Convert.ToInt32(props[3]),
					SixValue = props[4],
					FivePlusOneNum = Convert.ToInt32(props[5]),
					FivePlusOneValue = props[6],
					FiveNum = Convert.ToInt32(props[7]),
					FiveValue = props[8],
					FourNum = Convert.ToInt32(props[9]),
					FourValue = props[10],
					ThreeNum = Convert.ToInt32(props[11]),
					ThreeValue = props[12],
					Numbers = new int[6]
				};
				for (int i = 0; i < 6; ++i)
				{
					lottoData.Numbers[i] = Convert.ToInt32(props[i + 13]);
				}

				return lottoData;
			}
			catch (Exception ex)
			{
				var attributes = new Dictionary<String, String>();

				if (props != null)
				{
					for (int i = 0; i < props.Length; i++)
					{
						attributes.Add("props" + i, props[i]);
					}
				}

				LogHelper.LogException("LottoSixDataMapper exception", ex, attributes);
				IoC.Get<IBugsenseHelper>().LogBugsenseException(ex, attributes);

				return null;
			}
		}

		public static SkandinavianLottoData SkandinavianLottoDataMapper(string[] props)
		{
			try
			{
				var lottoData = new SkandinavianLottoData()
				{
					Year = props[0],
					Week = props[1],
					Date = CreateDateTime(props[2]),
					SevenNum = Convert.ToInt32(props[3]),
					SevenValue = props[4],
					SixNum = Convert.ToInt32(props[5]),
					SixValue = props[6],
					FiveNum = Convert.ToInt32(props[7]),
					FiveValue = props[8],
					FourNum = Convert.ToInt32(props[9]),
					FourValue = props[10],
					Numbers = new int[7],
					MachineNumbers = new int[7],
				};
				for (int i = 0; i < 7; ++i)
				{
					lottoData.MachineNumbers[i] = Convert.ToInt32(props[i + 11]);
				}
				for (int i = 0; i < 7; ++i)
				{
					lottoData.Numbers[i] = Convert.ToInt32(props[i + 18]);
				}

				return lottoData;
			}
			catch (Exception ex)
			{
				var attributes = new Dictionary<String, String>();

				if (props != null)
				{
					for (int i = 0; i < props.Length; i++)
					{
						attributes.Add("props" + i, props[i]);
					}
				}

				LogHelper.LogException("SkandinavianLottoDataMapper exception", ex, attributes);
				IoC.Get<IBugsenseHelper>().LogBugsenseException(ex, attributes);

				return null;
			}
		}

		public static JokerData JokerDataMapper(string[] props)
		{
			try
			{
				var lottoData = new JokerData()
				{
					Year = props[0],
					Week = props[1],
					Date = CreateDateTime(props[2]),
					SixNum = Convert.ToInt32(props[3]),
					SixValue = props[4],
					FiveNum = Convert.ToInt32(props[5]),
					FiveValue = props[6],
					FourNum = Convert.ToInt32(props[7]),
					FourValue = props[8],
					ThreeNum = Convert.ToInt32(props[9]),
					ThreeValue = props[10],
					TwoNum = Convert.ToInt32(props[11]),
					TwoValue = props[12],
					Numbers = new int[6]
				};
				for (int i = 0; i < 6; ++i)
				{
					lottoData.Numbers[i] = Convert.ToInt32(props[i + 13]);
				}

				return lottoData;
			}
			catch (Exception ex)
			{
				var attributes = new Dictionary<String, String>();

				if (props != null)
				{
					for (int i = 0; i < props.Length; i++)
					{
						attributes.Add("props" + i, props[i]);
					}
				}

				LogHelper.LogException("JokerDataMapper exception", ex, attributes);
				IoC.Get<IBugsenseHelper>().LogBugsenseException(ex, attributes);

				return null;
			}
		}

		private static DateTime CreateDateTime(string data)
		{
			var splits = data.Split('.');
			return new DateTime(Convert.ToInt32(splits[0]), Convert.ToInt32(splits[1]), Convert.ToInt32(splits[2]));
		}
	}
}
