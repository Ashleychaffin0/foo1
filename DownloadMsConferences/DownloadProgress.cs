using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using HAP = HtmlAgilityPack;

namespace DownloadMsConferences {
	// Note the inheritance. This is a custom user control
	public partial class DownloadProgress : UserControl {

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern long GetTickCount64();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool QueryPerformanceCounter(out long counter);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool QueryPerformanceFrequency(out long frequency);

		public TitledWebClient wc;

		// Support for download speed
		long WhenLastUpdated = 0;
		long TotalBytes      = 0;
		long BytesToDate     = -1;

		// On my machine, with a 50 Mbps Internet link, the download speed updates far
		// too quickly. So don't display the data on every UpdateProgess call. Do 1 out
		// of every <n> calls.
		const int	nEvery_N_Calls = 11;	// A prime number somehow seems wise
		int			CallNumber     = 0;

		long        PerfFrequency;

//---------------------------------------------------------------------------------------

		public DownloadProgress(TitledWebClient wc) {
			InitializeComponent();

			this.wc = wc;

			QueryPerformanceFrequency(out PerfFrequency);

			// LinkLabel1 is a hyperlink label
			linkLabel1.Text = wc.Title;

			lblTotalSize.Text        = "";
			lblAmountDownloaded.Text = "";
			lblPercentDone.Text      = "";
			lblDownloadSpeed.Text    = "";
			progressBar1.Minimum     = 0;
			progressBar1.Step        = 1;

			linkLabel1.Links.Add(0, wc.Title.Length, wc.SessionUrl);
			linkLabel1.Click += LinkLabel1_Click;

			// The Description seems to have a bunch of Html in it -- CDATA, tags, <img>
			// and so on. Do a quick parse on it and extract just the text
			var hDoc = new HAP.HtmlDocument();
			hDoc.LoadHtml(wc.Description);
			// toolTip1.SetToolTip(linkLabel1, hDoc.DocumentNode.InnerText);
			SetTooltip(toolTip1, linkLabel1, hDoc.DocumentNode.InnerText);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The default Tooltip processing seems to make the width of the window to be
		/// the width of the parent window. Which can make things awfully wide and
		/// correspondingly harder to read. To make the tooltip narrower, insert newlines
		/// into the text judiciuosly.
		/// </summary>
		/// <param name="Tt">Make this routine general. Pass in the Tooltip control
		/// </param>
		/// <param name="Ctl">The control we're tooltippingh</param>
		/// <param name="Text">The string we want to display</param>
		private void SetTooltip(ToolTip Tt, Control Ctl, string Text) {
			var Width      = Ctl.Size.Width;
			string[] Words = Text.Split(' ');
			int WidthSoFar = 0;
			int BlankWidth = TextRenderer.MeasureText(" ", Ctl.Font).Width;
			for(int n = 0; n < Words.Length; ++n) {
				string word = Words[n];
				int w = TextRenderer.MeasureText(word, Ctl.Font).Width + BlankWidth;
				WidthSoFar += w;
				if (WidthSoFar > Width) {
					Words[n - 1] += Environment.NewLine;
					WidthSoFar = w;
				}
			}
			string NewText = string.Join(" ", Words);
			Tt.SetToolTip(Ctl, NewText);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Click the session title. If the download is finished, launch the video. If
		/// not, bring up the web page.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LinkLabel1_Click(object sender, EventArgs e) {
			if (BytesToDate < TotalBytes) {
				string URL = (string)linkLabel1.Links[0].LinkData;
				URL = URL.ToUpper();
				if (!(URL.StartsWith("HTTP://") || URL.StartsWith("HTTPS://"))) {
					URL = "http://" + URL;
				}
				System.Diagnostics.Process.Start(URL);
			} else {
				System.Diagnostics.Process.Start(wc.FullFilename);
			}
		}

//---------------------------------------------------------------------------------------

		public void UpdateProgress(DownloadProgressChangedEventArgs e) {
			// long CurTime;

			TotalBytes = e.TotalBytesToReceive;
			lblTotalSize.Text        = AutoScale(TotalBytes);
			lblAmountDownloaded.Text = AutoScale(e.BytesReceived);
			lblPercentDone.Text      = ((int)((e.BytesReceived * 1.0 / e.TotalBytesToReceive) * 100.0)).ToString() + "%";

			// Houston, we have a problem here. e.TotalBytesToReceive is a long, but
			// progressBar1.Maximum is an int. But if the file being downloaded is
			// larger than 2GB (5,008,162,360 for the 2013 Build Day 2 Keynote), then
			// casting it to int gives us only 713,195,064. So ProgressBar1.Maximum
			// will be 713,195,064. And eventually e.BytesReceived will exceed that
			// (on its way to 5,008,162,360) and the progress bar will throw an
			// exception when the .Value exceeds the .Maximum. Sigh.

			// So what we'll do here is, if e.TotalBytesToReceive > int.MaxValue,
			// then we'll scale things by, oh, 1024, which will support files up to
			// 2048GB in size. Hopefully that should take care of things.
			if (e.TotalBytesToReceive > int.MaxValue) {
				progressBar1.Maximum = (int)(e.TotalBytesToReceive >> 10);
				progressBar1.Value   = (int)(e.BytesReceived >> 10);
			} else {
				progressBar1.Maximum = (int)e.TotalBytesToReceive;
				progressBar1.Value   = (int)e.BytesReceived;
			}

			if (++CallNumber > nEvery_N_Calls) {
				CallNumber = 0;
			}
			if (CallNumber == 0) {
				if (WhenLastUpdated > 0) {
					// Don't do first time through
					// Note that .Ticks is measured in milliseconds per tick
					var AmountOfData = e.BytesReceived - BytesToDate;
					var Denom        = GetTickCount64() - WhenLastUpdated;
					// QueryPerformanceCounter(out CurTime);
					// var Denom = CurTime - WhenLastUpdated;
					if (Denom != 0) {
						var Rate = 1000 * AmountOfData / Denom; // ms / tick
						lblDownloadSpeed.Text = $"{AutoScale(Rate)}/sec";
					}
				}
			}
			BytesToDate     = e.BytesReceived;
			// QueryPerformanceCounter(out WhenLastUpdated);
			WhenLastUpdated = GetTickCount64();
		}

//---------------------------------------------------------------------------------------

		private string AutoScale(long TotalBytes) {
			const long KB = 1024;
			const long MB = KB * 1024;
			// const long GB = MB * 1024;

			if (TotalBytes < KB) {
				return TotalBytes.ToString("N0");
			} else { // if (TotalBytesToReceive < MB) {
				return $"{((TotalBytes + KB - 1) / KB):N0} KB";
			}
			// else {
			//	return ((TotalBytesToReceive + MB - 1) / MB) + " MB";
			//}
		}

//---------------------------------------------------------------------------------------

		internal void DownloadDone(AsyncCompletedEventArgs e) {
			lblTotalSize.ForeColor        = Color.Black;
			lblAmountDownloaded.ForeColor = Color.Black;
			lblDownloadSpeed.ForeColor    = Color.Black;
			lblPercentDone.ForeColor      = Color.Black;
		}
	}
}
