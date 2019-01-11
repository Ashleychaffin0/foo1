using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;

// Features of this program
//	*	Downloads the HTML source from a specified web page and scrapes data from it.
//	*	Loads historical data from an XML file, adds the scraped data, and saves it back.
//	*	Displays the data in an Excel-like chart. Allows you to select the type of 
//		chart (Line, Doughnut, etc) and to optionally show it in 3D.
//	*	Displays the data in a report which can be printed, exported (Excel, PDF, etc),
//		Searched, Zoomed, etc
//	*	The chart/report are goverened by a splitter control
//	*	Will speak the latest status (text-to-speech).
//	*	Automatically resizes the chart and the report as the window resizes
//	*	In well under 300 lines (including blank lines and comments!)

// Useful link(s);
//	http://www.gotreportviewer.com/
//	http://msdn.microsoft.com/en-us/library/ms251686(v=VS.80).aspx
//	

// TODO:
//	*	Add second graph (Series) if possible, for # of Accounts

namespace nsTrackFoldingAtHome {
	public partial class TrackF_H : Form {

		public List<RawData> StatsData {get; set;}

		const string StatsFilename = "Folding@Home_Stats.xml";

		SeriesChartType ChartType = SeriesChartType.Line;	// Default

//---------------------------------------------------------------------------------------

		public TrackF_H() {
			InitializeComponent();

#if false	// Only enable to create new file
			Data = new List<RawData>();
			SaveData(StatsFilename, Data);
#else
			StatsData = LoadData(StatsFilename);
#endif
		}

//---------------------------------------------------------------------------------------

		private void TrackF_H_Load(object sender, EventArgs e) {
			this.WindowState = FormWindowState.Maximized;
			SetChartTypes();

			chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;

			chart1.Series[0].Name                = "LRS F@H Stats";
			chart1.Series[0]["ShowMarkerLines"]  = "true";
			chart1.Series[0].MarkerStyle         = MarkerStyle.Diamond;
			chart1.Series[0].IsValueShownAsLabel = true;

			cmbChartTypes.Text = "Line";

#if false		// TODO: Some sample code just in case I want to do grouping myself
			var qry = from data in StatsData
					  let Hourly = new DateTime(data.Date.Year, data.Date.Month, data.Date.Day, data.Date.Hour, 0, 0)
					  group data by Hourly;
			foreach (var item in qry) {
				Console.WriteLine("{0}", item.Key);
				foreach (var thing in item) {
					Console.WriteLine("\tDate={0}, Rank={1}, #Accts={2}", thing.Date, thing.Rank, thing.TotalAccounts);
				}
			}
#endif
			lblRank.Text = GetStatusText();
			TrackF_HBindingSource.DataSource = StatsData;

			this.reportViewer1.RefreshReport();
		}

//---------------------------------------------------------------------------------------

		private void SetChartTypes() {
			cmbChartTypes.DataSource =
				(from name in Enum.GetNames(typeof(SeriesChartType))
				orderby name
				select name).ToList();
		}

//---------------------------------------------------------------------------------------

		private void ShowChart() {
			chart1.Series[0].ChartType = ChartType;
			chart1.DataSource = StatsData;

			// Set series members names for the X and Y values 
			chart1.Series[0].XValueMember  = "Date";
			chart1.Series[0].YValueMembers = "Rank";
		}

//---------------------------------------------------------------------------------------

		private RawData ScrapeF_H(string Html) {
			string s = GetTextAfter(Html, "Overall rank (if points are combined)");
			s = GetTextAfter(s, "<TD align=left><font size=4> ");

			var words = s.Split(new char[] { ' ' });
			int Rank, TotalAccounts;
			bool bOK = int.TryParse(words[0], out Rank);
			if (! bOK) {
				throw new Exception("Unable to parse Rank");
			}
			bOK = int.TryParse(words[2], out TotalAccounts);
			if (! bOK) {
				throw new Exception("Unable to parse TotalAccounts");
			}
			return new RawData(Rank, TotalAccounts);
		}

//---------------------------------------------------------------------------------------

		private string GetTextAfter(string Html, string SearchString) {
			int n = Html.IndexOf(SearchString);
			if (n < 0) {
				string msg = string.Format("Unable to scrape the F@H stats page (looking for \"{0}\"", 
					SearchString);
				throw new Exception(msg);
			}
			return Html.Substring(n + SearchString.Length);
		}

//---------------------------------------------------------------------------------------

		string GetURL(string Url) {
			var req        = WebRequest.Create(new Uri(Url));
			var resp       = req.GetResponse();
			var stream     = resp.GetResponseStream();
			using (var rdr = new StreamReader(stream)) {
				return rdr.ReadToEnd();
			}
		}

//---------------------------------------------------------------------------------------

		private List<RawData> LoadData(string Filename) {
			object o = null;
			using (StreamReader sr = new StreamReader(Filename)) {
				XmlSerializer xs = new XmlSerializer(typeof(List<RawData>));
				o = xs.Deserialize(sr);
			}
			return (List<RawData>)o;
		}

//---------------------------------------------------------------------------------------

		private void SaveData(string Filename, List<RawData> Data) {
			StreamWriter sw = null;
			sw = new StreamWriter(Filename);
			XmlSerializer xs = new XmlSerializer(typeof(List<RawData>));
			xs.Serialize(sw, Data);
		}

//---------------------------------------------------------------------------------------

		private void cmbChartTypes_SelectedIndexChanged(object sender, EventArgs e) {
			string s  = cmbChartTypes.SelectedItem.ToString();
			ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), s);
			// var y  = Convert.ToInt32(ChartType);
			ShowChart();
		}

//---------------------------------------------------------------------------------------

		private void btnGetData_Click(object sender, EventArgs e) {
			string PageHtml = GetURL("http://fah-web.stanford.edu/cgi-bin/main.py?qtype=userpage&username=lrs5");

			if (PageHtml.Contains("Stats update in progress")) {
				MessageBox.Show("Can't continue. Stats update in progress.", "TrackF@H");
				return;
			}

			// Scrape the data off the web site, add it to our list and save
			// it back to disk.
			RawData ScrapedData;
			try {
				ScrapedData = ScrapeF_H(PageHtml);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "TrackF@H");
				return;
			}
			StatsData.Add(ScrapedData);
			SaveData(StatsFilename, StatsData);

			lblRank.Text = GetStatusText();

			ShowChart();
			reportViewer1.RefreshReport();

			SpeakToMe(lblRank.Text);
		}

//---------------------------------------------------------------------------------------

		string GetStatusText() {
			RawData LastReading = StatsData[StatsData.Count - 1];
			var Percentile = 100 * (1 - (float)LastReading.Rank / (float)LastReading.TotalAccounts);
			return string.Format("Rank {0:N0} of {1:N0}. In top {2:N4}%.",
							LastReading.Rank, LastReading.TotalAccounts, Percentile);
		}

//---------------------------------------------------------------------------------------

		private void chk3D_CheckedChanged(object sender, EventArgs e) {
			chart1.ChartAreas[0].Area3DStyle.Enable3D = chk3D.Checked;
		}

//---------------------------------------------------------------------------------------

		private void TrackF_HBindingSource_CurrentChanged(object sender, EventArgs e) {
			// No functionality. Just to see what a BindingSource looks like
			var foo = sender as BindingSource;
		}

//---------------------------------------------------------------------------------------

		private void btnTalkToMe_Click(object sender, EventArgs e) {
			SpeakToMe(GetStatusText());
		}
		
//---------------------------------------------------------------------------------------

		private void SpeakToMe(string s) {
			var synth = new SpeechSynthesizer();
			synth.SpeakAsync(s);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class RawData {
		public int		Rank { get; set; }
		public int		TotalAccounts { get; set; }
		public DateTime Date { get; set; }

//---------------------------------------------------------------------------------------

		public RawData(int Rank, int TotalAccounts) {
			this.Rank          = Rank;
			this.TotalAccounts = TotalAccounts;
			this.Date          = DateTime.Now;
		}

//---------------------------------------------------------------------------------------

		public RawData() {
			// Serializer requires a public parameterless ctor
		}
	}
}
