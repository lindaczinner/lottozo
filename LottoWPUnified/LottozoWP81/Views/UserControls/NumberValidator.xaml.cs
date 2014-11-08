using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Caliburn.Micro;

namespace Lottozo.Views.UserControls
{
	public partial class NumberValidator : UserControl
	{
		public static readonly DependencyProperty SelectedNumbersProperty = DependencyProperty.Register("SelectedNumbers", typeof(BindableCollection<int?>), typeof(NumberValidator), new PropertyMetadata(OnSelectedNumbersChanged));

		public BindableCollection<int?> SelectedNumbers
		{
			get { return (BindableCollection<int?>)GetValue(SelectedNumbersProperty); }
			set { SetValue(SelectedNumbersProperty, value); }
		}

		public static readonly DependencyProperty SelectableNoProperty = DependencyProperty.Register("SelectableNo", typeof(int), typeof(NumberValidator), null);

		public int SelectableNo
		{
			get { return (int)GetValue(SelectableNoProperty); }
			set { SetValue(SelectableNoProperty, value); }
		}

		public static readonly DependencyProperty IsOrderableProperty = DependencyProperty.Register("IsOrderable", typeof(bool), typeof(NumberValidator), null);

		public bool IsOrderable
		{
			get { return (bool)GetValue(IsOrderableProperty); }
			set { SetValue(IsOrderableProperty, value); }
		}

		public NumberValidator()
		{
			InitializeComponent();
		}

		static void OnSelectedNumbersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var control = obj as NumberValidator;

			if (control == null)
				return;

			var old = args.OldValue as BindableCollection<int?>;

			if (old != null)
				old.CollectionChanged -= control.OnNumbersCollectionChanged;

			var num = args.NewValue as BindableCollection<int?>;

			if (num != null)
				num.CollectionChanged += control.OnNumbersCollectionChanged;

			Render(control, num);
		}

		private void OnNumbersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var num = sender as BindableCollection<int?>;
			if (num != null)
				Render(this, num);
		}

		public static void Render(NumberValidator control, BindableCollection<int?> num)
		{
			var counter = 0;
			var sb = new StringBuilder();
			sb.Append("Számok: ");

			var containsNum = false;
			foreach (var n in num)
			{
				if (n != null)
					containsNum = true;
			}

			if (num.Count == 0 || !containsNum)
				sb.Append("-");
			else if (control.IsOrderable)
			{
				var temp = new List<int?>();
				temp.AddRange(num);
				temp.Sort();

				foreach (var i in temp)
				{
					if (i == null)
						continue;

					sb.Append(i);
					sb.Append(", ");
					counter++;
				}
			}
			else
			{
				foreach (var i in num)
				{
					if (i == null)
						continue;

					sb.Append(i);
					sb.Append(", ");
					counter++;
				}
			}

			if (num.Count != 0 && containsNum)
				sb.Remove(sb.Length - 2, 2);
			
			control.SelectedNumbersTextBlock.Text = sb.ToString();

			var missing = control.SelectableNo - counter;
			if (missing == 0)
			{
				control.ErrorTextBlock.Text = "Több szám nem választható ki.";
				control.ErrorTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
			}
			else
			{
				control.ErrorTextBlock.Text = missing + " szám hiányzik";
				control.ErrorTextBlock.Foreground = new SolidColorBrush(Colors.Red);
			}
		}
	}
}
