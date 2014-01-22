using System;
using System.Windows.Data;

namespace LottozoCore.Converters
{
	public class ToggleSwitchStateBoolConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is Boolean)
			{
				if ((bool)value == true)
					return ToggleSwitchState.Be;
			}
			return ToggleSwitchState.Ki;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is ToggleSwitchState)
			{
				if (((ToggleSwitchState)value) == ToggleSwitchState.Be)
					return 1;
			}
			return 0;
		}
	}

	public enum ToggleSwitchState
	{
		Ki = 0,
		Be = 1
	}
}
