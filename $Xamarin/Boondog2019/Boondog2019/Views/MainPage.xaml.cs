using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nsBoondog2019.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage {
		public MainPage() {
			InitializeComponent();
			DisplayAlert("In MainPage ctor", "In ctor", "Cancel");
		}
	}
}