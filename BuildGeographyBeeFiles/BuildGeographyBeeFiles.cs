using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// For mode demographic info, see the following links:
//	*	http://www.nationsonline.org/oneworld/famous-cities.htm
//	*	http://www.nationsonline.org/oneworld/countries_of_the_world.htm
//	*	http://demographia.com/db-worldua.pdf

namespace BuildGeographyBeeFiles {
	public partial class BuildGeographyBeeFiles : Form {

		// Categories
		const string Continents          = "Continents";
		const string Countries           = "Countries";
		const string AmericanCities      = "American Cities";
		const string InternationalCities = "International Cities";
		const string BodiesOfWater       = "Bodies of Water";

//---------------------------------------------------------------------------------------

		public BuildGeographyBeeFiles() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void BuildGeographyBeeFiles_Load(object sender, EventArgs e) {
			cmbCategory.Items.Add(Continents);
			cmbCategory.Items.Add(Countries);
			cmbCategory.Items.Add(BodiesOfWater);	// Oceans, Rivers, Seas, etc
			cmbCategory.Items.Add(AmericanCities);
			cmbCategory.Items.Add(InternationalCities);

			cmbCategory.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// TODO: Once all the files have been processed, write them out somewhere
			switch (cmbCategory.SelectedItem) {
				case Continents:
					DoContinents();
					break;
				case Countries:
					DoCountries();
					break;
				case AmericanCities:
					var AmericanCitiesList = DoAmericanCities();
					break;
				case InternationalCities:
					DoInternationalCities();
					break;
				case BodiesOfWater:
					DoBodiesOfWater();
					break;
				default:
					break;
			}
		}

//---------------------------------------------------------------------------------------

		private void DoContinents() {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private void DoCountries() {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private List<string> DoAmericanCities() {
			var hs = new HashSet<string>();
			string FileName = $"../../{AmericanCities}.txt";
			using (var rdr = new StreamReader(FileName)) {
				string line;
				while ((line = rdr.ReadLine()) != null) {
					int len = line.Length;	// Assume len >= 2
					// Some cities have the same name in different states. For example,
					// there's an Albany in New York (NY) and Georgia (GA). Strip off
					// the State abbreviation if there. In this case we'll wind up with
					// two Albany's, but our HashSet will hold just one of them.
					if (char.IsUpper(line[len - 1]) && char.IsUpper(line[len - 2])) {
						line = line.Substring(0, len - 2).Trim();
					}
					line = CanonicalizeName(line);
					if (line != null) {
						hs.Add(CanonicalizeName(line));
					}
				}
			}
			return hs.OrderBy(key => key).ToList();
		}

//---------------------------------------------------------------------------------------

		private void DoInternationalCities() {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private void DoBodiesOfWater() {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private string CanonicalizeName(string line) {
			// Note that some cities (e.g. Cape Town) can end with "Town". But that's
			// OK; we check for " town", not " Town".
			if (line.EndsWith(" town")) {
				line = line.Substring(0, line.Length - 5);
			} else if (line.EndsWith(" village")) {
				line = line.Substring(0, line.Length - 8);
			}

			// Get rid of bad characters (e.g. blank and period) in, say, "St. Lucie")
			var sb = new StringBuilder(line.Length);
			foreach (char c in line) {
				if (char.IsLetter(c)) {
					sb.Append(c);
				}
			}
			line = sb.ToString();

			// Also check that the name doesn't contain more than 7 unique characters
			// or fewer than 5
			int len = UniqueCharacterCount(line);
			if ((len > 7) || (len < 5)) {
				return null;
			}

			return line.ToUpper();
		}

//---------------------------------------------------------------------------------------

		private int UniqueCharacterCount(string s) {
			// Assumes parameter has already been canonicalized
			var hs = new HashSet<char>();
			foreach (char c in s) {
				hs.Add(char.ToLower(c));
			}
			return hs.Count();
		}
	}
}
