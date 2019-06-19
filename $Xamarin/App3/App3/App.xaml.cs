using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App3.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BogDroid_2019_03 {
	public partial class App : Application {

		Boondog bDog;
		public static MainPage BogMain;
		public static BogBoard Board;
		public static Boondog2019 BogForm;

//---------------------------------------------------------------------------------------

		public App() {
			InitializeComponent();

			MainPage = new MainPage();
			bDog = new Boondog();
		}

//---------------------------------------------------------------------------------------

		protected override void OnStart() {
			// Handle when your app starts
			// Properties["foo"] = 3.2;
			bDog.NewGame();
		}

//---------------------------------------------------------------------------------------

		protected override void OnSleep() {
			// Handle when your app sleeps
		}

//---------------------------------------------------------------------------------------

		protected override void OnResume() {
			// Handle when your app resumes
		}
	}
}

#if false		// TODO: Delete
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
			 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
			 x:Class="App3.Views.Boondog">
	<ContentPage.Content>
		<StackLayout>
			<Label Text="Welcome to Boondoggle!"
				VerticalOptions="CenterAndExpand" 
				HorizontalOptions="CenterAndExpand" />
			<Grid>
				<Grid.RowDefinitions>
					<RowDefinition/>
					<RowDefinition/>
					<RowDefinition/>
					<RowDefinition/>
					<RowDefinition/>
				</Grid.RowDefinitions>

				<Grid.ColumnDefinitions>
					<ColumnDefinition/>
					<ColumnDefinition/>
					<ColumnDefinition/>
					<ColumnDefinition/>
					<ColumnDefinition/>
				</Grid.ColumnDefinitions>
				
			</Grid>
		</StackLayout>
	</ContentPage.Content>
</ContentPage>
#endif
