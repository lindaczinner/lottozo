using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace LottozoCore.Helpers
{
	public static class LogHelper
	{
		public static void LogException(string tag, Exception ex, Dictionary<String, String> otherAttributes)
		{
			var localyticsSession = IoC.Get<LocalyticsSession>();
			if (localyticsSession == null)
				return;

			var attributes = new Dictionary<string, string>
					{
						{"message", ex.Message},
						{"stackTrace", ex.StackTrace},
					};

			if (ex.InnerException != null)
			{
				attributes.Add("inner ex message", ex.InnerException.Message);
				attributes.Add("inner ex stacktrace", ex.InnerException.StackTrace);
			}

			foreach (var attribute in otherAttributes)
			{
				attributes.Add(attribute.Key, attribute.Value);
			}

			localyticsSession.tagEvent(tag, attributes);
		}
	}
}
