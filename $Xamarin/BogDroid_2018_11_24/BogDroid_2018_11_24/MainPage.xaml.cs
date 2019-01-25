using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BogDroid_2018_11_24 {
	public partial class MainPage : ContentPage {
		public MainPage() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		void BtnClickMe_Clicked(object sender, EventArgs args) {
			var BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string nl = Environment.NewLine;
			string txt = $"Newline length = {nl.Length}" + nl;
			lvProgramWords.ItemsSource = Directory.GetFiles(BaseDir);

			foreach (var dir in Directory.GetDirectories(BaseDir)) {
				txt += $"Directory -- {dir}{nl}";
				try {
					foreach (var file in Directory.GetFiles(Path.Combine(dir, "*.*"))) {
						txt += $"      {file}{nl}";
					}
				} catch {
					// Ignore
				}
			}
			LblMessages.Text = txt;
		}

//---------------------------------------------------------------------------------------

		void BtnWriteTOD_Click(object sender, EventArgs args) {
			string TOD = DateTime.Now.ToString();
			var BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var fn = Path.Combine(BaseDir, "TOD.txt");
			using (var sw = new StreamWriter(fn)) {
				sw.WriteLine(TOD);
			}
			LblMessages.Text = $"Got to Write TOD -- {TOD}";
		}

//---------------------------------------------------------------------------------------

		void BtnReadTOD_Click(object sender, EventArgs args) {
			var BaseDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var fn = Path.Combine(BaseDir, "TOD.txt");
			string TOD = "*Not initialized*";
			using (var sr = new StreamReader(fn)) {
				TOD = sr.ReadToEnd();
			}
			LblMessages.Text = $"Got to Read TOD -- {TOD}";
		}

//---------------------------------------------------------------------------------------

		void BtnSquare_Click(object sender, EventArgs args) {
			var btn = sender as BogCube;
			LblMessages.Text = $"Got to Btn Click - {btn.Text}";
			
		}

//---------------------------------------------------------------------------------------

		void BtnNewGame_Click(object sender, EventArgs args) {
			BogDice.ShakeDice(5, BogBoard.Cubes);
			// TODO: Next lines from Boondog2014.cs, method NewGame
#if false
			FoundWords.Clear();
			game.Reset();
			SetupListboxes();
			board.NewGame();
#endif

			App.Board.prof = App.BogForm.ProgramProf;
			App.Board.PgmWords.Clear();
			App.Board.UserWords.Clear();
			// lvProgramWords.ItemsSource = null;
			// lvPlayerWords.ItemsSource  = null;
			lvProgramWords.ItemsSource = App.Board.PgmWords;
			lvPlayerWords.ItemsSource = App.Board.UserWords;
			var WordFinder = new GenerateProgramWords(App.BogForm, App.Board.prof, BogBoard.Cubes, App.Board.PgmDict, App.Board);

			// game.ProgramWords = 0;
			// game.ProgramScore = 0;
			WordFinder.FindProgramWords();
			App.Board.PgmWords.Sort();
			App.Board.UserWords.Sort();
		}

//---------------------------------------------------------------------------------------

		void BtnReadBogDicts_Click(object sender, EventArgs args) {
			DisplayAlert("Nonce", "Hang in there...", "OK");
			var btn = sender as Button;
			LblMessages.Text = $"Got to Btn Click - {btn.Text}";
		}
	}
}
