using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;

namespace Lottozo.Views.UserControls
{
	public partial class LotteryTicket : UserControl
	{
		private const int NumbersPerRow = 5;

		public static readonly DependencyProperty NumberNoProperty = DependencyProperty.Register("NumberNo", typeof(int), typeof(LotteryTicket), null);

		public int NumberNo
		{
			get { return (int)GetValue(NumberNoProperty); }
			set { SetValue(NumberNoProperty, value); }
		}

		public static readonly DependencyProperty MaxNumberProperty = DependencyProperty.Register("MaxNumber", typeof(int), typeof(LotteryTicket), new PropertyMetadata(OnMaxNumberChanged));

		public int MaxNumber
		{
			get { return (int)GetValue(MaxNumberProperty); }
			set { SetValue(MaxNumberProperty, value); }
		}

		public static readonly DependencyProperty NumbersProperty = DependencyProperty.Register("Numbers", typeof(BindableCollection<int?>),
																						typeof(LotteryTicket),
																						new PropertyMetadata(OnNumbersChanged));

		public BindableCollection<int?> Numbers
		{
			get { return (BindableCollection<int?>)GetValue(NumbersProperty); }
			set { SetValue(NumbersProperty, value); }
		}

		public bool IsAvailableSpot
		{
			get { return NumberNo > Numbers.Count; }
		}

		public LotteryTicket()
		{
			InitializeComponent();
		}

		static void OnNumbersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var numbers = args.NewValue as BindableCollection<int?>;
			var control = obj as LotteryTicket;

			if (control == null || numbers == null)
				return;

			var old = args.OldValue as BindableCollection<int?>;

			if (old != null)
				old.CollectionChanged -= control.OnNumbersCollectionChanged;

			numbers.CollectionChanged += control.OnNumbersCollectionChanged;

			Render(control, numbers);
		
		}

		private void OnNumbersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var num = sender as BindableCollection<int?>;
			if (num != null)
				Render(this, num);
		}

		private static void Render(LotteryTicket control, IEnumerable<int?> numbers)
		{
			var toggleButtons = control.LayoutRoot.Children.Where(p => ((NumberButton)p).IsSelected == true);
			foreach (var tb in toggleButtons)
			{
				((NumberButton)tb).IsSelected = false;
			}

			foreach (var number in numbers)
			{
				var toggleButton = (NumberButton)control.LayoutRoot.Children.Single(p => ((NumberButton)p).NumberText.ToString() == number.ToString());
				toggleButton.IsSelected = true;
			}
		}


		static void OnMaxNumberChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var maxNum = (int)args.NewValue;
			var control = obj as LotteryTicket;

			if (maxNum == 0 || control == null)
				return;

			var rows = Math.Ceiling((double)(maxNum / NumbersPerRow));

			control.LayoutRoot.RowDefinitions.Clear();

			for (int i = 0; i < rows; i++)
			{
				control.LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(92) });
			}

			control.LayoutRoot.ColumnDefinitions.Clear();

			for (int i = 0; i < NumbersPerRow; i++)
			{
				control.LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(92) });
			}

			control.LayoutRoot.Children.Clear();

			for (int i = 0; i < maxNum; i++)
			{
				var colNo = i % NumbersPerRow;
				var rowNo = (int)Math.Floor((double)(i / NumbersPerRow));

				
				var button = new NumberButton
					{
						IsSelected = false,
						NumberText = (i + 1).ToString(CultureInfo.InvariantCulture)
					};

				button.Click += control.button_Click;
				Grid.SetColumn(button, colNo);
				Grid.SetRow(button, rowNo);
				control.LayoutRoot.Children.Add(button);
			}
		}

		public void ClearNumbers()
		{
			Numbers.Clear();

			var toggleButtons = this.LayoutRoot.Children.Where(p => ((NumberButton)p).IsSelected == true);
			foreach (var tb in toggleButtons)
			{
				((NumberButton)tb).IsSelected = false;
			}
		}

		public void GenerateRandomNumbers()
		{
			var newNumbers = LottozoCore.Helpers.Utils.RandomNumbersGenerator(NumberNo, MaxNumber);

			ClearNumbers();

			foreach (var newNumber in newNumbers)
			{
				Numbers.Add(newNumber);
			}
		}

		void button_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as NumberButton;
			if (button == null)
				return;

			var number = Int32.Parse(button.NumberText);

			if (Numbers.Contains(number))
				Numbers.Remove(number);
			else if (IsAvailableSpot)
			{
				Numbers.Add(number);
				var temp = new List<int?>();
				temp.AddRange(Numbers);
				temp.Sort();
				Numbers.Clear();
				Numbers.AddRange(temp);
				button.IsSelected = true;
			}
			else
				button.IsSelected = false;
		}

		
	}
}
