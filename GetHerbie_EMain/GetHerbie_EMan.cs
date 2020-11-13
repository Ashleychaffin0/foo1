// Copyright (c) 2020 by Larry Smith

// See https://docs.microsoft.com/en-us/microsoft-edge/webview2/gettingstarted/win32

// See table of contents at https://readcomiconline.to/ComicList?c=a&page=0
/*
Magnus-Robot-Fighter-4000-AD
75-Years-Of-DC-Comics
A-Date-with-Judy
Abbott-Costello
Action-Comics-1938
Action-Comics-80-Years-of-Superman-The-Deluxe-Edition
Adventure-Comics-1938
Adventures-of-Supergirl
Agent-Carter-S-H-I-E-L-D-50th-Anniversary
Agents-of-S-H-I-E-L-D
Agents-of-S-H-I-E-L-D-The-Chase
Al-Williamson-s-Flash-Gordon-A-Lifelong-Vision-of-the-Heroic
All-of-Scrooge-McDuck-s-Millions
All-Star-Comics-1999
All-Star-Comics-Only-Legends-Live-Forever
All-Winners-Comics-70th-Anniversary-Special
All-Flash
All-New-Guardians-of-the-Galaxy
All-New-Official-Handbook-of-the-Marvel-Universe-A-to-Z
All-New-Official-Handbook-of-the-Marvel-Universe-A-to-Z-Update
All-New-All-Different-Marvel-Universe
All-Star-Comics
All-Star-Comics-80-Page-Giant
All-Star-Squadron
All-Winners-Comics
Amazing-Adult-Fantasy
Amazing-World-of-Carmine-Infantino
Amazing-Spider-Man-Going-Bi
Amazing-World-of-DC-Comics
Animaniacs
Animaniacs-1995
Archie-1000-Page-Comic-Jamboree
	See other Archie 1000 page
Arrgh
Asgardians-of-the-Galaxy
Asterix
Atomic-Bunny
Avengers-1996
	See other Avengers

********** B **********

********** C **********

Cerebus
Cerebus-Jam
Cerebus-in-Hell
Classic-Star-Wars

********** M **********

Metal-Men-1963

********** N **********

Not-Brand-Echh











*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using HtmlAgilityPack;

using HAP = HtmlAgilityPack;

namespace nsGetHerbie_EMan {
	public partial class GetHerbie_EMan : Form {
		const string BaseUrl = "https://readcomiconline.to/";
		string MagUrl		 = "";
		string MagName		 = "";
		HAP.HtmlWeb web		 = new();
		HAP.HtmlDocument doc = new();
		Regex re			 = new Regex("");
		bool QuitSwitch		 = false;

		readonly WebClient wc = new();

//---------------------------------------------------------------------------------------

		public GetHerbie_EMan() {
			InitializeComponent();
			// TODO: Read in mag names from file
			ReadMagsFromFile();
		}

//---------------------------------------------------------------------------------------

		private void ReadMagsFromFile() {
			// TODO: This doesn't really read a file
			var mags = new List<string> {
				"Cerebus",
				"Cerebus-Jam",
				"Cerebus-in-Hell",
				"Classic-Star-Wars",
				"Metal-Men-1963",
				"Not-Brand-Echh",
				"Asterix",
			};
			mags.Sort();
			// TODO: CmbMag.DataSource = mags???
			foreach (string mag in mags) {
				CmbMag.Items.Add(mag);
			}
		}

//---------------------------------------------------------------------------------------

		private void GetHerbie_EMan_Load(object sender, System.EventArgs e) {
			CmbMag.SelectedIndex = 0;
			string pat = "[^\\']+\"(?<url>.*?)\\'".Replace('\'', '"');
			re = new Regex(pat, RegexOptions.Compiled);
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, System.EventArgs e) {
			LbMsgs.Items.Clear();
			doc = web.Load(MagUrl);
			QuitSwitch = false;
			web2.CoreWebView2.Navigate(MagUrl);
			doc.DocumentNode.Descendants()
				 .Where(node => node.Name == "td")
				 .ToList()
				 .ForEach(node => ProcessMag(node));
			Msg("Done!");
		}

//---------------------------------------------------------------------------------------

		private void ProcessMag(HtmlNode node) {
			if (QuitSwitch) { return; }
			if (node.ChildNodes.Count < 2) { return; }
			var a = node.ChildNodes[1];
			string title = a.InnerText.Trim();
			string HrefMag = a.Attributes["href"].Value;
			string url = BaseUrl + HrefMag;
			Msg(url);
			// web = new HAP.HtmlWeb();
			web2.CoreWebView2.Navigate(url);
			var magDoc = web.Load(url);
			string magHtml = magDoc.DocumentNode.InnerHtml;
			int ix = magHtml.IndexOf("var lstImages = new Array();");
			if (ix < 0) {
				if (magDoc.DocumentNode.InnerText.Contains("Are you human")) {
					Msg("Found: Are you human");
				} else {
					Msg("Whoops. Expected script not found");
				}
				// web2.CoreWebView2.Navigate(url);
				// Debugger.Break();
				QuitSwitch = true;
				return;
			}
			string[] lines = magHtml[ix..].Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
			int PageNo = 0;
			foreach (string src in lines) {
				string line = src.Trim();
				if (line.Length == 0) { continue; }
				if (!line.StartsWith("lstImages.push(\"")) { continue; }
				if (line.StartsWith("https://readcomiconline.to/")) { continue; }
				var matches = re.Matches(line);
				foreach (Match match in matches) {
					string pageUrl = match.Groups["url"].Value;
					// Msg(pageUrl);
					string path = Path.Combine($@"G:\lrs\Comics", (CmbMag.SelectedItem as string)!, $"{title}");
					System.IO.Directory.CreateDirectory(path);
					string filename = System.IO.Path.Combine(path, $"{title} - Page " + (++PageNo).ToString("D3") + ".jpg");
					if (!File.Exists(filename)) {
						wc.DownloadFile(pageUrl, filename);
						byte[] jpgData = wc.DownloadData(pageUrl);
						var bm = new Bitmap(filename);
						bm.SetResolution(bm.Width, bm.Height);
						Pages.Image = bm;
					}
					Application.DoEvents();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void CmbMag_SelectedIndexChanged(object sender, System.EventArgs e) {
			var cb  = (sender! as ComboBox)!;
			MagName = (string)cb.SelectedItem;
			MagUrl  = BaseUrl + "Comic/" + MagName;
		}

//---------------------------------------------------------------------------------------

		private void Msg(string s) {
			int ix = LbMsgs.Items.Add(s);
			Debug.WriteLine(s);
			LbMsgs.SelectedIndex = ix;
			Application.DoEvents();
		}
	}
}
