using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nsBoondog2019.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BoondogPage : ContentPage {
		public BoondogPage() {
			InitializeComponent();
			DisplayAlert("In Boondog ctor", "In ctor", "Cancel");
		}

//---------------------------------------------------------------------------------------

		private void NewGame_Clicked(object sender, EventArgs e) {
			App.Board.NewGame();		// TODO:
			MusicFolderPath.Text = App.MusicDir;
			DisplayAlert("NewGame", "In NewGame_Clicked", "Cancel", "Good enough...");
		}

	}
}