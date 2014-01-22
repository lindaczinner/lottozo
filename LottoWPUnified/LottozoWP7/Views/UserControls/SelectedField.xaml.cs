using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using LottozoCore.Interfaces;

namespace Lottoszamok.Views.UserControls
{
	public partial class SelectedField : UserControl
	{
		public static readonly DependencyProperty NumbersChangedProperty = DependencyProperty.Register("NumbersChanged", typeof(bool), typeof(SelectedField), new PropertyMetadata(OnNumbersChanged));

		public bool NumbersChanged
		{
			get { return (bool)GetValue(NumbersChangedProperty); }
			set { SetValue(NumbersChangedProperty, value); }
		}

		public int FieldNo { get; set; }

		public BindableCollection<int> Numbers { get; set; }

		public SelectedField()
		{
			InitializeComponent();

			Numbers = new BindableCollection<int>();
		}

		private static void OnNumbersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var numbersChanged = (bool)e.NewValue;
			var control = d as SelectedField;

			if (numbersChanged && control != null)
			{
				var field = IoC.Get<IStateMachine>().TicketDetails.Fields.SingleOrDefault(p => p.FieldNo == control.FieldNo);
				if (field == null || field.Numbers == null || field.Numbers.Count == 0)
					control.Visibility = Visibility.Collapsed;
				else
				{
					control.Numbers.Clear();
					control.Numbers.AddRange(field.Numbers);
					Render(control);
					control.Visibility = Visibility.Visible;
				}
				control.NumbersChanged = false;
			}
		}

		private static void Render(SelectedField control)
		{
			control.FieldTitle.Text = control.FieldNo + ". mező";
			control.LayoutRoot.ColumnDefinitions.Clear();

			for (int i = 0; i < control.Numbers.Count; i++)
			{
				control.LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
			}

			for (int i = 0; i < control.Numbers.Count; i++)
			{
				var uc = new NumberDisplay
				{
					Number = control.Numbers[i].ToString(),
					Margin = new Thickness(0, 0, 0, 12)
				};
				Grid.SetColumn(uc, i);
				Grid.SetRow(uc, 1);
				control.LayoutRoot.Children.Add(uc);
			}

			Grid.SetColumnSpan(control.FieldTitle, control.Numbers.Count);
		}
	}
}
