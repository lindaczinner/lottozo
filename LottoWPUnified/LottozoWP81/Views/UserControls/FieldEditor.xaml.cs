using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using LottozoCore.Interfaces;
using LottozoCore.ViewModels;

namespace Lottozo.Views.UserControls
{
	public partial class FieldEditor : UserControl
	{
		public int MaxValue { get; set; }

		public int FieldNo { get; set; }

		public BindableCollection<int?> Numbers { get; set; }

		public static readonly DependencyProperty NumbersChangedProperty = DependencyProperty.Register("NumbersChanged", typeof(bool), typeof(FieldEditor), new PropertyMetadata(OnNumbersChanged));

		public bool NumbersChanged
		{
			get { return (bool)GetValue(NumbersChangedProperty); }
			set { SetValue(NumbersChangedProperty, value); }
		}

		public static readonly DependencyProperty FieldDeletedProperty = DependencyProperty.Register("FieldDeleted", typeof(bool), typeof(FieldEditor), null);

		public bool FieldDeleted
		{
			get { return (bool)GetValue(FieldDeletedProperty); }
			set { SetValue(FieldDeletedProperty, value); }
		}

		public static readonly DependencyProperty IsDeletableProperty = DependencyProperty.Register("IsDeletable", typeof(bool), typeof(FieldEditor), new PropertyMetadata(OnDeletableChanged));

		public bool IsDeletable
		{
			get { return (bool)GetValue(IsDeletableProperty); }
			set { SetValue(IsDeletableProperty, value); }
		}

		public static readonly DependencyProperty IsAddableProperty = DependencyProperty.Register("IsAddable", typeof(bool), typeof(FieldEditor), null);

		public bool IsAddable
		{
			get { return (bool)GetValue(IsAddableProperty); }
			set { SetValue(IsAddableProperty, value); }
		}

		public FieldEditor()
		{
			InitializeComponent();
			Numbers = new BindableCollection<int?>();
		}

		private static void OnNumbersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as FieldEditor;
			if (control != null)
			{
				control.RefershNumbers();
				control.NumbersChanged = false;
			}
		}

		private static void OnDeletableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as FieldEditor;
			if (control != null)
				control.deleteButton.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
		}

		public static void Render(FieldEditor control)
		{
			if (control.Numbers != null)
			{
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
							Margin = new Thickness(0, 12, 0, 0)
						};
					Grid.SetColumn(uc, i);
					Grid.SetRow(uc, 1);
					control.LayoutRoot.Children.Add(uc);
				}

				Grid.SetColumnSpan(control.deleteButton, control.Numbers.Count);
				Grid.SetColumnSpan(control.selectButton, control.Numbers.Count);
				Grid.SetColumnSpan(control.FieldTitle, control.Numbers.Count);
				control.FieldTitle.Text = control.FieldNo + ". mező";
			}
		}

		private void selectButton_Click(object sender, RoutedEventArgs e)
		{
			IoC.Get<LottoTicketViewModel>().FieldNo = FieldNo;
			IoC.Get<LottoTicketViewModel>().NumberNo = Numbers.Count;
			IoC.Get<LottoTicketViewModel>().MaxValue = MaxValue;
			IoC.Get<LottoTicketViewModel>().LottoNumbers = new BindableCollection<int?>();
			foreach (var lottoNumber in Numbers)
			{
				IoC.Get<LottoTicketViewModel>().LottoNumbers.Add(lottoNumber);
			}

			IoC.Get<INavigationService>().UriFor<LottoTicketViewModel>().Navigate();
		}

		private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
		{
			var fields = IoC.Get<IStateMachine>().TicketDetails.Fields;
			fields.Remove(fields.Single(p => p.FieldNo == FieldNo));
			foreach (var field in fields)
			{
				if (field.FieldNo > FieldNo)
					field.FieldNo--;
			}
			FieldDeleted = true;
			IsAddable = true;
			IsDeletable = fields.Count > 1;
		}

		private void FieldsEditor_OnLoaded(object sender, RoutedEventArgs e)
		{
			RefershNumbers();
		}

		private void RefershNumbers()
		{
			var ticketDetails = IoC.Get<IStateMachine>().TicketDetails;
			MaxValue = LottozoCore.Helpers.Utils.GetGameMaxNumber(ticketDetails.LotteryTypes);

			var field = ticketDetails.Fields.SingleOrDefault(p => p.FieldNo == this.FieldNo);
			if (field == null || field.Numbers == null || field.Numbers.Count == 0)
				this.Visibility = Visibility.Collapsed;
			else
			{
				this.Visibility = Visibility.Visible;
				this.Numbers.Clear();
				foreach (var number in field.Numbers)
				{
					this.Numbers.Add(number);
				}
				Render(this);
			}
		}
	}
}
