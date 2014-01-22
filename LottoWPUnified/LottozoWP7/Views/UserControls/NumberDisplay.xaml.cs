using System.Windows;
using System.Windows.Controls;

namespace Lottoszamok.Views.UserControls
{
	public partial class NumberDisplay : UserControl
	{
		public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(string), typeof(NumberDisplay), new PropertyMetadata(OnNumberChanged));
		
		public string Number
		{
			get { return (string)GetValue(NumberProperty); }
			set { SetValue(NumberProperty, value); }
		}

		static void OnNumberChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var num = args.NewValue as string;
			var control = obj as NumberDisplay;

			if (num != null && control!= null)
			{
				if (num.Length == 1)
				{
					control.OneDigitPlaceholder.Visibility = Visibility.Visible;
					control.TwoDigitPlaceholder.Visibility = Visibility.Collapsed;
					control.OneDigitPlaceholder.Text = num;
				}
				else if (num.Length == 2)
				{
					control.OneDigitPlaceholder.Visibility = Visibility.Collapsed;
					control.TwoDigitPlaceholder.Visibility = Visibility.Visible;
					control.TwoDigitPlaceholder.Text = num;
				}
			}
		}

		public NumberDisplay()
		{
			InitializeComponent();
		}
	}
}
