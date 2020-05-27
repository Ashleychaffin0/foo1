using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamarinTestShellTabs3.Services;
using XamarinTestShellTabs3.Views;

namespace XamarinTestShellTabs3 {
	public partial class App : Application {

		public App() {
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			MainPage = new AppShell();
		}

		protected override void OnStart() {
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
