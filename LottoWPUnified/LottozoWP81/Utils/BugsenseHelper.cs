using System;
using System.Collections.Generic;
using BugSense;
using BugSense.Core.Model;
using LottozoCore.Interfaces;


namespace Lottozo.Utils
{
	public class BugsenseHelper : IBugsenseHelper
	{
		public void LogBugsenseException(Exception ex,  Dictionary<String, String> attributes)
		{
			var data = new LimitedCrashExtraDataList();
			foreach (var attribute in attributes)
			{
				data.Add(attribute.Key, attribute.Value);
			}

			BugSenseHandler.Instance.LogException(ex, data);
		}
	}
}
