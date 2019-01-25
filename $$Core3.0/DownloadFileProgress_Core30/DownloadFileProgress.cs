// https://github.com/dotnet/corefx/commit/7ce9270ac7

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using LRSNativeMethodsNamespace;

using HAP = HtmlAgilityPack;

namespace LRS.Utils {
	// Note the inheritance. This is a custom user control
	public partial class DownloadFileProgress : UserControl {

		public TitledWebClient twc;

		// Support for download speed
		public long WhenLastUpdated = 0;
		public long TotalBytes      = 0;
		public long BytesToDate     = -1;

		// On my machine, with a 300 Mbps Internet link, the download speed updates far
		// too quickly. So don't display the data on every UpdateProgess call. Do 1 out
		// of every <n> calls.
		const int	nEvery_N_Calls = 3;	// A prime number somehow seems wise
		int			CallNumber     = 0;

		// Fields to support class FlowManager
		public string	ProgressName;
		public bool		IsDownloadDone;
		public bool		IsRelocated;        //  Has already been moved to the end

		Action<string>	msg;
		Func<bool, object, DownloadProgressChangedEventArgs, bool> UserProgress;

//---------------------------------------------------------------------------------------

		public DownloadFileProgress(
				TitledWebClient twc, 
				string			ProgressName,
				Action<string>	msg, 
				Func<bool, object, DownloadProgressChangedEventArgs, bool> UserProgress) {
			InitializeComponent();

			this.twc           = twc;
			twc.Progress       = this;
			this.ProgressName = ProgressName;
			this.msg         += msg;
			this.UserProgress = UserProgress;

			// LinkLabel1 is a hyperlink label
			LinkLabel1.Text = twc.Title;

			lblTotalSize.Text        = AutoScale(twc.UrlSize);
			lblAmountDownloaded.Text = "";
			lblPercentDone.Text      = "";
			lblDownloadSpeed.Text    = "";
			ProgressBar1.Minimum     = 0;
			ProgressBar1.Step        = 1;

			IsDownloadDone = false;
			IsRelocated    = false;

			LinkLabel1.Links.Add(0, twc.Title.Length, twc.SessionUrl);
			LinkLabel1.Click += LinkLabel1_Click;

			twc.DownloadProgressChanged += Wc_DownloadProgressChanged;
			twc.DownloadFileCompleted   += Wc_DownloadFileCompleted;

			// The Description seems to have a bunch of Html in it -- CDATA, tags, <img>
			// and so on. Do a quick parse on it and extract just the text
			var hDoc = new HAP.HtmlDocument();
			hDoc.LoadHtml(twc.Description);
			// toolTip1.SetToolTip(linkLabel1, hDoc.DocumentNode.InnerText);
			SetTooltip(ToolTip1, LinkLabel1, hDoc.DocumentNode.InnerText);
		}

//---------------------------------------------------------------------------------------

		public void DoDownloadAsync() {
			twc.DownloadFileAsync(new Uri(twc.ActualUrl), twc.FullFilename);
		}

//---------------------------------------------------------------------------------------

		private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
			var wc = sender as TitledWebClient;
			wc.Progress.UpdateProgress(e);
		}

//---------------------------------------------------------------------------------------

		private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
			var wc = sender as TitledWebClient;
			if (UserProgress != null)
				UserProgress(false, wc.Tag, null);	

			if (e.Error != null) {
				msg($"******** Error on {wc.FullFilename} -- ({e.Error}) in DownloadFileCompleted)");
				PaintItBlack_er_Cyan(wc);
				try {
					File.Delete(wc.FullFilename);
				} catch (Exception ex) {
					msg($"******** Error deleting {wc.FullFilename} - {ex.Message}");
				}
				wc.Progress.ProgressBar1.ForeColor = Color.Cyan;
				wc.Progress.ProgressBar1.Value     = wc.Progress.ProgressBar1.Maximum;
			} else {
				// Set Last Write Time on file based on Response Header
#if false    // Re-enable for diagnostics
				foreach (var hdr in wc.ResponseHeaders) {
					msg($"*** {wc.Title}/{wc.ActualUrl} -- {hdr}='{wc.ResponseHeaders[(string)hdr]}'");
				}
#endif
				string LastMod = wc.ResponseHeaders["Last-Modified"];
				if (LastMod != null) {
					var LastUsed = DateTime.Parse(LastMod);
					File.SetLastWriteTime(wc.FullFilename, LastUsed);
				}
			}

			IsDownloadDone = true;
			wc.Progress.DownloadDone(e);
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
			// Lines 2..n have a leading ' ' from .Join(). Strip them
			NewText = NewText.Replace(Environment.NewLine + " ", Environment.NewLine);
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
				string URL = (string)LinkLabel1.Links[0].LinkData;
				URL = URL.ToUpper();
				if (!(URL.StartsWith("HTTP://") || URL.StartsWith("HTTPS://"))) {
					URL = "http://" + URL;
				}
				Process.Start(URL);
			} else {
				Process.Start(twc.FullFilename);
			}
		}

//---------------------------------------------------------------------------------------

		public void UpdateProgress(DownloadProgressChangedEventArgs e) {
			// long CurTime;

			if (UserProgress != null) { UserProgress(true, twc.Tag, e); }

			TotalBytes               = e.TotalBytesToReceive;
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
				ProgressBar1.Maximum = (int)(e.TotalBytesToReceive >> 10);
				ProgressBar1.Value   = (int)(e.BytesReceived >> 10);
			} else {
				ProgressBar1.Maximum = (int)e.TotalBytesToReceive;
				ProgressBar1.Value   = (int)e.BytesReceived;
			}

			if (++CallNumber > nEvery_N_Calls) {
				CallNumber = 0;
			}
			// msg($"CallNumber = {CallNumber}, ProgressName = {ProgressName}");
			if (CallNumber == 0) {              // Don't do first time through
				// msg($"Whenlastupdated = {WhenLastUpdated}");
				if (WhenLastUpdated > 0) {
					// Note that .Ticks is measured in milliseconds per tick
					var AmountOfData = e.BytesReceived - BytesToDate;
					var Denom        = LRSNativeMethods.GetTickCount64() - WhenLastUpdated;
					msg($"e.BytesReceived = {e.BytesReceived}, AmountOfData = {AmountOfData}, Denom = {Denom}");
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
			WhenLastUpdated = LRSNativeMethods.GetTickCount64();
		}

//---------------------------------------------------------------------------------------

		private string AutoScale(long TotalBytes) {
			const long KB = 1024;
			// const long MB = KB * 1024;
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

		public void DownloadDone(AsyncCompletedEventArgs e) {
			lblTotalSize.ForeColor        = Color.Black;
			lblAmountDownloaded.ForeColor = Color.Black;
			lblDownloadSpeed.ForeColor    = Color.Black;
			lblPercentDone.ForeColor      = Color.Black;
		}


//---------------------------------------------------------------------------------------

		private static void PaintItBlack_er_Cyan(TitledWebClient wc) {
			wc.Progress.lblTotalSize.BackColor        = Color.Cyan;
			wc.Progress.lblAmountDownloaded.BackColor = Color.Cyan;
			wc.Progress.lblDownloadSpeed.BackColor    = Color.Cyan;
			wc.Progress.lblPercentDone.BackColor      = Color.Cyan;
			wc.Progress.LinkLabel1.BackColor          = Color.Cyan;
		}

	}
}
