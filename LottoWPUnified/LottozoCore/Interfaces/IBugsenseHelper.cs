using System;
using System.Collections.Generic;

namespace LottozoCore.Interfaces
{
	public interface IBugsenseHelper
	{
		void LogBugsenseException(Exception ex,  Dictionary<String, String> attributes);
	}
}
