using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BugSense;
using BugSense.Core.Model;
using Lottozo.Utils;
using LottozoCore.Interfaces;
using LottozoCore.ViewModels;
using Microsoft.Phone.Controls;
using Caliburn.Micro;
using Microsoft.Phone.Shell;

namespace Lottozo
{
	public class AppBootstrapper : PhoneBootstrapperBase
	{
		PhoneContainer container;

		public LocalyticsSession appSession;

        public AppBootstrapper()
        {
            Initialize();
        }

		protected override void Configure()
		{
			container = new PhoneContainer();
			if (!Execute.InDesignMode)
				container.RegisterPhoneServices(RootFrame);

			container.Instance(appSession);

			container.RegisterInstance(typeof(bool), "IsTrial", CheckIsTrial());

			container.RegisterSingleton(typeof(IStateMachine), null, typeof(StateMachine));
			container.RegisterSingleton(typeof(IBugsenseHelper), null, typeof(BugsenseHelper));
			
			container.PerRequest<MainViewModel>();
			container.PerRequest<LottoViewModel>();
			container.PerRequest<LottoSixViewModel>();
			container.PerRequest<SkandinavianLottoViewModel>();
			container.PerRequest<JokerViewModel>();
			container.PerRequest<AboutViewModel>();
			container.PerRequest<PurchaseViewModel>();
			container.PerRequest<LotteryTicketSetUpViewModel>();
			container.PerRequest<FieldsSetUpViewModel>();
			
			container.Instance(new LottoTicketViewModel());
			container.Instance(new JokerSelectorViewModel());

			container.Instance(new StorageHelper());

			AddCustomConventions();


			OverrideColors();
			}

		protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
		{
			return new TransitionFrame();
		}

		protected override object GetInstance(Type service, string key)
		{
			return container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			container.BuildUp(instance);
		}

		static void AddCustomConventions()
		{
			ConventionManager.AddElementConvention<Pivot>(Pivot.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
				(viewModelType, path, property, element, convention) =>
				{
					if (ConventionManager
						.GetElementConvention(typeof(ItemsControl))
						.ApplyBinding(viewModelType, path, property, element, convention))
					{
						ConventionManager
							.ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
						ConventionManager
							.ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, null, viewModelType);
						return true;
					}

					return false;
				};

			ConventionManager.AddElementConvention<Panorama>(Panorama.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
				(viewModelType, path, property, element, convention) =>
				{
					if (ConventionManager
						.GetElementConvention(typeof(ItemsControl))
						.ApplyBinding(viewModelType, path, property, element, convention))
					{
						ConventionManager
							.ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
						ConventionManager
							.ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, null, viewModelType);
						return true;
					}

					return false;
				};
		}

		//protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		//{
		//	if (Debugger.IsAttached)
		//	{
		//		Debugger.Break();
		//		e.Handled = true;
		//	}
		//	else
		//	{
		//		MessageBox.Show("Váratlan hiba történt, elnézést!", "Upsz...", MessageBoxButton.OK);
		//		e.Handled = true;
		//	}

		//	var attributes = new Dictionary<string, string> { { "exception", e.ExceptionObject.Message } };
		//	appSession.tagEvent("App crash", attributes);

		//	base.OnUnhandledException(sender, e);
		//}

		protected override void OnActivate(object sender, ActivatedEventArgs e)
		{
			var stateMachine =  PhoneApplicationService.Current.State["StateMachine"] as StateMachine;
			if (stateMachine != null)
				IoC.Get<IStateMachine>().FillStateMachine(stateMachine);

			appSession = new LocalyticsSession("fbcd3ef1ac779ef3a6c4da9-f49e34a6-b727-11e2-88f5-005cf8cbabd8");
			appSession.open();
			appSession.upload();

			base.OnActivate(sender, e);
		}

		protected override void OnLaunch(object sender, LaunchingEventArgs e)
		{
			IoC.Get<IStateMachine>().GetSavedTickets();
			PhoneApplicationService.Current.State.Clear();
			
			appSession = new LocalyticsSession("fbcd3ef1ac779ef3a6c4da9-f49e34a6-b727-11e2-88f5-005cf8cbabd8");
			appSession.open();
			appSession.upload();

			base.OnLaunch(sender, e);
		}

		protected override void OnDeactivate(object sender, DeactivatedEventArgs e)
		{
			PhoneApplicationService.Current.State["StateMachine"] = IoC.Get<IStateMachine>();

			appSession.close();

			base.OnDeactivate(sender, e);
		}

		protected override void OnClose(object sender, ClosingEventArgs e)
		{
			PhoneApplicationService.Current.State.Clear();

			appSession.close();

			base.OnClose(sender, e);
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Application), RootFrame,"7acc2269");
			BugSenseHandler.Instance.LastActionBeforeTerminate(BeforeAppCrash);

			var config = new TypeMappingConfiguration
			{
				DefaultSubNamespaceForViewModels = "LottozoCore.ViewModels",
				DefaultSubNamespaceForViews = "Lottozo.Views",
			};

			ViewLocator.ConfigureTypeMappings(config);
			ViewModelLocator.ConfigureTypeMappings(config);

			base.OnStartup(sender, e);
		}

		public void BeforeAppCrash()
		{
			appSession.tagEvent("App crash");
		}

		protected override IEnumerable<Assembly> SelectAssemblies()
		{
			var assemblies = new List<Assembly>();
			var refAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			assemblies.AddRange(refAssemblies);

			return assemblies;
		}

		/// <summary>
		/// Check the current license information for this application
		/// </summary>
		private bool CheckIsTrial()
		{
			// When debugging, we want to simulate a trial mode experience. The following conditional allows us to set the _isTrial 
			// property to simulate trial mode being on or off. 
#if DEBUG
			return true;
#else
			var licenseInfo = new Microsoft.Phone.Marketplace.LicenseInformation();
			return licenseInfo.IsTrial();
#endif
		}

		private void OverrideColors()
		{
			(App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush).Color = Colors.Orange;
			
			(App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush).Color = Colors.White;
			(App.Current.Resources["PhoneContrastForegroundBrush"] as SolidColorBrush).Color = Colors.Black;
			
			(App.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush).Color = Colors.Black;
			(App.Current.Resources["PhoneContrastBackgroundBrush"] as SolidColorBrush).Color = Colors.White;
			
			(App.Current.Resources["PhoneChromeBrush"] as SolidColorBrush).Color = Color.FromArgb(255, 40, 40, 40);
			
			}
	}
}
