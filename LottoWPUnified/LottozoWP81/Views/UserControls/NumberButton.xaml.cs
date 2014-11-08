using System.Windows;
using System.Windows.Controls;

namespace Lottozo.Views.UserControls
{
	public partial class NumberButton : Button
	{
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(NumberButton), new PropertyMetadata(OnIsSelectedChanged));

		public bool IsSelected
		{
			get
			{
				return (bool)GetValue(IsSelectedProperty);
			}
			set
			{
				SetValue(IsSelectedProperty, value);
			}
		}

		public static readonly DependencyProperty NumberTextProperty = DependencyProperty.Register("NumberText", typeof(string), typeof(NumberButton), new PropertyMetadata(OnNumberTextChanged));

		public string NumberText
		{
			get
			{
				return (string)GetValue(NumberTextProperty);
			}
			set
			{
				SetValue(NumberTextProperty, value);
			}
		}

		public NumberButton()
		{
			InitializeComponent();
		}

		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as NumberButton;
			if (control == null)
				return;

			if ((bool)e.NewValue)
				control.IsSelectedImage.Visibility = Visibility.Visible;
			else
				control.IsSelectedImage.Visibility = Visibility.Collapsed;
		}

		private static void OnNumberTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as NumberButton;
			if (control == null)
				return;
			
			control.NumberTextBlock.Text = e.NewValue.ToString();

		}
	}
}
