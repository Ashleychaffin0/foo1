using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#if false

                <Label Grid.Row="0" Grid.Column="0" Text="A"  FontSize="Large" BackgroundColor="Red" HorizontalTextAlignment="Center"/>
                <Label Grid.Row="1" Grid.Column="0" Text="B"  FontSize="Large" BackgroundColor="AliceBlue" HorizontalTextAlignment="Center"/>
                <Label Grid.Row="4" Grid.Column="1" Text="Qu" FontSize="Large" BackgroundColor="Blue" HorizontalTextAlignment="End"/>

#endif

namespace App3.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Boondog : ContentPage {
		Random rand;

		const int nRows = 5;
		const int nCols = 5;

//---------------------------------------------------------------------------------------

		public Boondog() {
			InitializeComponent();
			rand = new Random();

			DefineBoard();
		}

//---------------------------------------------------------------------------------------

		private void DefineBoard() {
			for (int row = 0; row < nRows; row++) {
				for (int col = 0; col < nCols; col++) {
					Color c = Color.FromRgb(rand.Next(255), rand.Next(255), rand.Next(255));

					var label = new Label {
						HorizontalTextAlignment = TextAlignment.Center,
						VerticalTextAlignment = TextAlignment.Center,
						TextColor = Color.Yellow,
						BackgroundColor = c,
						Text = "?",					// TODO:
						FontSize = 50
					};
					MainGrid.Children.Add(label, row, col);
				}
			}
		}

//---------------------------------------------------------------------------------------

		internal void NewGame() {
			foreach (Label cell in MainGrid.Children) {
				cell.TextColor = Color.Red;
				var r = rand.Next(26);
				string txt = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[r].ToString();
				cell.Text = txt;
				cell.BackgroundColor = Color.Bisque;
			}
		}

//---------------------------------------------------------------------------------------

		internal void Old_NewGame() {
			int nCells = MainGrid.Children.Count;
			for (int row = 0; row < nRows; row++) {
				for (int col = 0; col < nCols; col++) {
					int ix = row * nRows + col;
					if (ix > nCells) { continue;  }
					var cell = MainGrid.Children[ix] as Label;
					cell.TextColor = Color.Red;
					var r = rand.Next(24);
					string txt = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[r].ToString();
					cell.Text = txt;
				}
			}

		}

//---------------------------------------------------------------------------------------

		private void NewGame_Clicked(object sender, EventArgs e) {
			NewGame();
		}
	}
}