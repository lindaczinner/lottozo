using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Lottozo.Views.UserControls
{
	public partial class JokerNumberSelectorControl : UserControl
	{
		public static readonly DependencyProperty JokerNumberProperty = DependencyProperty.Register("JokerNumber", typeof(int?), typeof(JokerNumberSelectorControl), new PropertyMetadata(OnJokerNumberChanged));

		public int? JokerNumber
		{
			get { return (int?)GetValue(JokerNumberProperty); }
			set { SetValue(JokerNumberProperty, value); }
		}

		public static readonly DependencyProperty OrderProperty = DependencyProperty.Register("Order", typeof(string), typeof(JokerNumberSelectorControl), new PropertyMetadata(OnOrderChanged));

		public string Order
		{
			get { return (string)GetValue(OrderProperty); }
			set { SetValue(OrderProperty, value); }
		}

		public JokerNumberSelectorControl()
		{
			InitializeComponent();
		}

		private void ToggleButton_Click(object sender, RoutedEventArgs e)
		{
			var control = sender as ToggleButton;

			if (control == null)
				return;

			if (control.IsChecked == true)
				JokerNumber = Int32.Parse(control.Content.ToString());
			else
				JokerNumber = null;
		}

		static void OnOrderChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var num = args.NewValue.ToString();
			var control = obj as JokerNumberSelectorControl;
			if (control != null)
				control.Title.Text = num;
		}

		static void OnJokerNumberChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var control = obj as JokerNumberSelectorControl;
			if (control != null)
			{
				if (args.OldValue != null)
				{
					var old =
						control.LayoutRoot.Children.Single(
							c => c is ToggleButton && ((ToggleButton) c).Content.ToString().Equals(args.OldValue.ToString()));
					((ToggleButton) old).IsChecked = false;
				}

				if (args.NewValue != null)
				{
					var old =
						control.LayoutRoot.Children.Single(
							c => c is ToggleButton && ((ToggleButton)c).Content.ToString().Equals(args.NewValue.ToString()));
					((ToggleButton)old).IsChecked = true;
				}
			}
		}
	}
}
