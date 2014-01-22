using System;
using System.Collections.Generic;
using Caliburn.Micro;
using LottozoCore.Interfaces;

namespace LottozoCore.Helpers
{
	public static class CsvHelper
	{
		public static string[] SplitText(string text)
		{
			if (string.IsNullOrEmpty(text))
				return null;

			try
			{
				if (!text.Contains("\""))
					return text.Split(Constants.Delimiter);

				int first = 0;
				bool noSplit = false;
				bool wasComma = false;
				var parts = new List<string>();
				for (int i = 0; i < text.Length; i++)
				{
					if (text[i] == ',' && noSplit == false)
					{
						if (wasComma)
						{
							parts.Add(text.Substring(first + 1, i - first - 2));
							first = i + 1;
							wasComma = false;
						}
						else
						{
							parts.Add(text.Substring(first, i - first));
							first = i + 1;
						}
					}
					else if (text[i] == '\"')
					{
						noSplit = !noSplit;
						wasComma = true;
					}
				}
				parts.Add(text.Substring(text.LastIndexOf(',') + 1, text.Length - first));

				return parts.ToArray();
			}
			catch (Exception ex)
			{
				var attributes = new Dictionary<String, String>
					{
						{"text", text}
					};

				LogHelper.LogException("CsvHelper exception", ex, attributes);

				IoC.Get<IBugsenseHelper>().LogBugsenseException(ex, attributes);

				return null;
			}

		}
	}
}
