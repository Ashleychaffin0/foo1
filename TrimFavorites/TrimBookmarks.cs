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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TrimFavorites {
	public partial class TrimBookmarks : Form {
		public TrimBookmarks() {
			InitializeComponent();
			
			string BookmarksPath = $@"C:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default\Bookmarks";
			// On macOS: /Users/{Environment.UserName}/Library/Application Support/Google/Chrome/Default/Bookmarks
			// Thanks to https://www.technewsera.com/chrome-bookmarks-location/
			string jsonText = File.ReadAllText(BookmarksPath);
			Clipboard.SetText(jsonText);

			using (var rdr = new JsonTextReader(new StringReader(jsonText))) {
				while (rdr.Read()) {
					Console.WriteLine("{0} - {1} - {2}",
									  rdr.TokenType, rdr.ValueType, rdr.Value);
				}
			}


			JObject jo = JObject.Parse(jsonText);
			MakeTree(jo, 0);
		}

//---------------------------------------------------------------------------------------

		private void MakeTree(JObject jo, int level) {
			foreach (KeyValuePair<string, JToken> item in jo) {
				string nl = "".PadRight(level, '\t');
				Console.Write($"{nl}XLevel {level}: {item.Key}/{item.Value.GetType().Name}: ");
				// if (item.Key == "name") Console.Write(": " + item.Value);
				// Console.WriteLine();
				switch (item.Value) {
				case JObject jo2:
					Console.WriteLine();
					MakeTree(jo2, ++level);
					break;
				case JArray ja:
					HandleJArray(ja);
					break;
				case JValue jv:
					Console.WriteLine(jv.Value);
					break;
				default:
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------
		
		private void HandleJArray(JArray ja) {
			for (int i = 0; i < ja.Count; i++) {
				var jaKid = ja.Children()[i];
				Console.WriteLine($"JaKid type: {ShowType(jaKid)}");
				switch (jaKid) {
				case JObject jo:
					Console.WriteLine();
					// MakeTree(jo2, ++level);
					break;
				case JArray ja2:
					for (int j = 0; j < ja2.Count; j++) {
						var ja2Kid = ja2.Children()[j];
						ShowType(ja2Kid);
					}
					// HandleJArray(ja);
					break;
				case JValue jv:
					Console.WriteLine(jv.Value);
					break;
				case JToken jt:
					Console.WriteLine(jt.ToString());
					break;
				case JEnumerable<JToken> jtEnum:
					Console.WriteLine($"HandleArray: Found IEnumerable<JToken>");
					var iter = jtEnum.GetEnumerator();
					if (iter.Current is null) { break; }
					foreach (JToken item in jtEnum) {

					}
					break;
				default:
					Console.WriteLine($"HandleArray default: jaKid is {ShowType(jaKid)}");
					var kids = jaKid.Children() as IEnumerable<JToken>;
#if true
					int k = 0;
					foreach (var jaKid3 in kids) {
						// var jaKid3 = jaKid[i];
						Console.WriteLine($"jaKid3[{k++}]: {ShowType(jaKid3)}");
					}
#else
					for (int k = 0; k < jaKid.Children().Count(); k++) {
						var jaKid3 = jaKid.Children()[i];
						Console.WriteLine($"jaKid3[{k++}]: {ShowType(jaKid3)}");

					}
#endif
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private string ShowType(object obj) => obj.GetType().Name;

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			MessageBox.Show("Nonce");
		}
	}
}
