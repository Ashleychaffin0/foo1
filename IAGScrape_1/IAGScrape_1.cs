using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using mshtml;

namespace IAGScrape_1 {
	public partial class IAGScrape_1 : Form {

		string OriginalLink = "http://www.onmatrix.ca/matrix/public/portal.aspx?ID=1043019125";
		string DestDir = @"D:\IAG";
		string OutputFileName = "IAG Scrape 1.csv";

		StreamWriter sw;

		bool bProcessingRows;
		bool bFinishedRow;
		bool bProcessingMls;

		Dictionary<string, object>	Dict;
		Dictionary<string, object>	MlsDict;

		List<string>				MlsKeys;
		int							ixMls;


//---------------------------------------------------------------------------------------

		public IAGScrape_1() {
			InitializeComponent();

			sw = new StreamWriter(Path.Combine(DestDir, OutputFileName));

			bProcessingRows = true;
			bProcessingMls = false;

			Dict = new Dictionary<string, object>();
			MlsDict = new Dictionary<string, object>();
			bFinishedRow = false;
		}

//---------------------------------------------------------------------------------------

		private void IAGScrape_1_Load(object sender, EventArgs e) {
			this.Location = new Point(2000, 100);
			Application.DoEvents();

			webBrowser1.Navigate(OriginalLink);
		}

//---------------------------------------------------------------------------------------

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			if (webBrowser1.ReadyState != WebBrowserReadyState.Complete) {
				return;
			}

			if (bProcessingRows) {
				ProcessRows();
			} else if (bProcessingMls) {
				ProcessMls();
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessMls() {
			SaveMls(webBrowser1.DocumentText);
		}

//---------------------------------------------------------------------------------------

		private void SaveMls(string s) {
			string MLS_ID = MlsKeys[ixMls];
			using (var sw = new StreamWriter(Path.Combine(DestDir, MLS_ID + ".html"))) {
				sw.WriteLine(s);
			}

			if (++ixMls < MlsKeys.Count) {
				ClickMls();
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessRows() {
			var body = webBrowser1.Document.Body;
			for (int i = 0; i < body.All.Count; i++) {
				var elem = body.All[i];
#if false
				if (true || elem.Id == "wrapperTable") {
#endif
				var type = elem.DomElement as HTMLTableCell;
				if (type != null) {
					// var className = (type as mshtml.HTMLTable).className;
					var className = type.className;
					string name = null;

					switch (className) {
					case "d54m8":
					name = "Status";
					break;
					case "d54m9":
					name = "Address";
					break;
					case "d54m11":
					name = "D";
					break;
					case "d54m12":
					name = "SD";
					break;
					case "d54m13":
					name = "Price";
					break;
					case "d54m4":
					name = "UpDownLink";
					break;
					case "d54m14":
					name = "Style";
					break;
					case "d54m15":
					name = "Bds";
					break;
					case "d54m17":
					name = "Bth Sale Date";
					bFinishedRow = true;				// Assume this is the last one
					break;
					case "d54m5":
					name = "MLS";
#if true
					if (type.innerText != null) {
						var grandkid = elem.FirstChild.FirstChild;
						MlsDict["MLS-Link-" + type.innerText] = grandkid;
						// grandkid.InvokeMember("Click");
						// (elem as HtmlElement).InvokeMember("Click");
						// return;
					}
#endif
					break;
					}

					if (name != null) {
						string value = type.innerText;
						Dict[name] = value;
						if (bFinishedRow) {
							var s = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
								Quote(Dict["MLS"]),					// 0
								Quote(Dict["Status"]),				// 1
								Quote(Dict["Address"]),				// 2
								Quote(Dict["D"]),					// 3
								Quote(Dict["SD"]),					// 4
								Quote(Dict["Price"]),				// 5
								Quote(Dict["Style"]),				// 6
								Quote(Dict["Bds"]),					// 7
								Quote(Dict["Bth Sale Date"])		// 8
								);

							sw.WriteLine(s);
							Dict.Clear();
							bFinishedRow = false;
						}
					}
				}
			}
			sw.Close();

			bProcessingRows = false;
			bProcessingMls = true;

			MlsKeys = MlsDict.Keys.ToList<string>();
			// Assume we always have at least 1
			ixMls = 0;
			ClickMls();

		}

//---------------------------------------------------------------------------------------

		private void ClickMls() {
			string MLS_ID = MlsKeys[ixMls];

			var elem = MlsDict[MLS_ID] as HtmlElement;
			elem.InvokeMember("Click");
		}

//---------------------------------------------------------------------------------------

		private string Quote(object s) {
			// TODO: Worry about embedded quotes later
			return "\"" + (string)s + "\"";
		}
	}
}
