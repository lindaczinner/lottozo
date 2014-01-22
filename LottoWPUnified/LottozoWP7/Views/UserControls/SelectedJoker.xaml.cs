using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.ViewModels;

namespace Lottoszamok.Views.UserControls
{
	public partial class SelectedJoker : UserControl
	{
		public static readonly DependencyProperty JokerNumbersProperty = DependencyProperty.Register("JokerNumbers", typeof(BindableCollection<int>), typeof(SelectedJoker), new PropertyMetadata(OnJokerNumbersChanged));

		public BindableCollection<int> JokerNumbers
		{
			get { return (BindableCollection<int>)GetValue(JokerNumbersProperty); }
			set { SetValue(JokerNumbersProperty, value); }
		}

		public SelectedJoker()
		{
			InitializeComponent();
		}

		private void OnJokerNumbersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var num = sender as BindableCollection<int>;
			if (num != null && num.Count != 0)
				Render(this, num);
		}

		static void OnJokerNumbersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			var control = obj as SelectedJoker;

			if (control == null)
				return;

			var old = args.OldValue as BindableCollection<int>;

			if (old != null)
				old.CollectionChanged -= control.OnJokerNumbersCollectionChanged;

			var num = args.NewValue as BindableCollection<int>;

			if (num != null)
				num.CollectionChanged += control.OnJokerNumbersCollectionChanged;

			Render(control, num);
		}

		public static void Render(SelectedJoker control, BindableCollection<int> num)
		{
			if (num == null || !num.Any())
				return;

			control.LayoutRoot.ColumnDefinitions.Clear();

			for (int i = 0; i < num.Count; i++)
			{
				control.LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
			}

			for (int i = 0; i < num.Count; i++)
			{
				var uc = new NumberDisplay
				{
					Number = control.JokerNumbers[i].ToString(CultureInfo.InvariantCulture)
				};
				Grid.SetColumn(uc, i);
				Grid.SetRow(uc, 0);
				control.LayoutRoot.Children.Add(uc);
			}

			Grid.SetColumnSpan(control.rndButton, num.Count);
			Grid.SetColumnSpan(control.selectButton, num.Count);
		}



		private void rndButton_Click(object sender, RoutedEventArgs e)
		{
			var newNumbers = LottozoCore.Helpers.Utils.RandomJokerGenerator();
			var displays = LayoutRoot.Children.OfType<NumberDisplay>().ToList();

			for (int i = 0; i < JokerNumbers.Count; i++)
			{
				displays[i].Number = newNumbers[i].ToString(CultureInfo.InvariantCulture);
			}

			JokerNumbers.Clear();
			JokerNumbers.AddRange(newNumbers);

			var ticketDetails = IoC.Get<IStateMachine>().TicketDetails;
			if (ticketDetails != null)
			{
				ticketDetails.Joker.Clear();
				ticketDetails.Joker.AddRange(JokerNumbers);
			}
		}

		private void selectButton_Click(object sender, RoutedEventArgs e)
		{
			var ticketDetails = IoC.Get<IStateMachine>().TicketDetails;
			if (ticketDetails != null)
			{
				ticketDetails.Joker.Clear();
				ticketDetails.Joker.AddRange(JokerNumbers);
			}

			var jokerSelectorViewModel = IoC.Get<JokerSelectorViewModel>();
			jokerSelectorViewModel.FirstJoker = JokerNumbers[0];
			jokerSelectorViewModel.SecondJoker = JokerNumbers[1];
			jokerSelectorViewModel.ThirdJoker = JokerNumbers[2];
			jokerSelectorViewModel.FourthJoker = JokerNumbers[3];
			jokerSelectorViewModel.FifthJoker = JokerNumbers[4];
			jokerSelectorViewModel.SixthJoker = JokerNumbers[5];

			IoC.Get<INavigationService>().UriFor<JokerSelectorViewModel>().Navigate();
		}
	}
}
