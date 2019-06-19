using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CopyMusicFromOneDrive {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(true)]
	public partial class MainPage : ContentPage {
		public MainPage() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void Button_Clicked(object sender, EventArgs e) {
			var xxx = Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic);
			var yyy = Directory.EnumerateFileSystemEntries(xxx);

			var xxx = FileSystem.

			var nl = Environment.NewLine;
			string zzz = "";
			foreach (var y in yyy) {
				zzz += y + nl;
			}
			msg.Text = zzz + "Done" + nl;
		}
	}
}
