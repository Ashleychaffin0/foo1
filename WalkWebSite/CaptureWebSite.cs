// Copyright (c) 2009 by somebody or other...

// TODO: The "HtmlSuffixes" below was a shortcut that didn't quite work. We probably
//		 need Mime types for .zip, .exe, .jpeg, ad nauseam. The problem is that we
//		 picked up the link to http://www.sanfransys.com/pretext, but didn't navigate to
//		 it because it didn't end in .html etc.

// TODO: Actual file saving isn't done. That's deliberate at this point. Later we'll
//		 want a subroutine to take a URL and convert it to a filename within the
//		 user-specified output directory, taking care to cope with funny characters
//		 in the name (e.g. colons, question marks, etc).

// TODO: A link such as <A href="..."><IMG src="..."></A> doesn't process the IMG. IOW,
//		 we won't download the graphical link files.

// TODO: Need better offsite identification. For example, www.ibm.com may link to
//		 www.software.ibm.com. Ditto for http://... linking to http*s*://...

// TODO: We seem to double-process the main_downloads file. Maybe check into why this is.

// TODO: Not processing www.foo.com/abc.php?q=1 type URLs. Probably just look for "?"
//		 and handle. Update: Partially done, but not working quite right.

// TODO: Doesn't handle bookmarks properly. Look for #.

// TODO: Don't default to www.sanfransys.com or bartizan.com

// TODO: No way to give user an "All Done" message.

// See also other TODO:s in the comments in the code

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

using mshtml;

namespace WalkWebSite {
	public partial class CaptureWebSite : Form {

		HashSet<string> VisitedUrls = new HashSet<string>();

		bool BigBrowser = true;		// For Swap button support

		// We want to navigate only to URLs that end in the following file types.
		// For now we'll settle for this. 
		// I've never looked at PHP. Maybe we should add .php.
		// Anything else?
		// Note: These *must* be in lower case. We compare them via ToLower().
		// TODO: This approach is probably obsolete.
		List<string> HtmlSuffixes = new List<string> { 
			".htm", ".html", ".asp", ".aspx", ".jsp" 
		};

//---------------------------------------------------------------------------------------

		public CaptureWebSite() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog();
			fbd.ShowNewFolderButton = true;
			fbd.SelectedPath = txtOutputDir.Text;
			DialogResult res = fbd.ShowDialog();
			if (res == DialogResult.OK) {
				txtOutputDir.Text = fbd.SelectedPath;
			}
		}

//---------------------------------------------------------------------------------------

		private void CaptureWebSite_Load(object sender, EventArgs e) {
			// Set default output directory
			string MyDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			txtOutputDir.Text = Path.Combine(MyDoc, "My Geocities Pages");

#if DEBUG	// For testing
			txtURL.Text = "http://www.geocities.com/jeremyalansmith/level9/";
			// txtURL.Text = "http://www.sanfransys.com";
			// txtURL.Text = "http://www.bartizan.com";
#else
			txtURL.Text = "http://www.geocities.com/";
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			string Url = txtURL.Text;
			// Note: We don't support https, ftp, etc.
			if (! Url.ToLower().StartsWith("http://")) {
				Url = "http://" + Url;
			}
			WalkAndSave(txtURL.Text);
		}

//---------------------------------------------------------------------------------------

		private void WalkAndSave(string URL) {
			web.Navigate(URL);
		}

//---------------------------------------------------------------------------------------

		private void Msg(string msg) {
			lbMsgs.Items.Add(msg);
		}

//---------------------------------------------------------------------------------------

		private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			Msg("Download complete for " + e.Url);
			var web = sender as WebBrowser;
#if false
			if (web.ReadyState != WebBrowserReadyState.Complete) {
				// May be downloading frames or something. Don't worry, we'll be called
				// again.
				return;
			}
#endif
			Msg("");
			Msg("About to process " + web.Url);
			Application.DoEvents();
			var doc = web.Document;
			string PageSource = doc.Body.OuterHtml;
			HTMLDocumentClass doc2 = doc.DomDocument as HTMLDocumentClass;
			HTMLWindow2 frames = doc2.frames as HTMLWindow2;
			for (int i = 0; i < frames.length; i++) {
				object o = i;
				var framewin = frames.item(ref o) as HTMLWindow2Class;
				var fdoc = framewin.document;
				var title = fdoc.title;
				var framePageSource = fdoc.body.outerHTML;
			}

			// TODO: Write out the PageSource
			var links = doc.Links;
			foreach (HtmlElement link in links) {
				var anchor = link.DomElement as HTMLAnchorElement;
				var href = anchor.href;
				if (! href.StartsWith(txtURL.Text)) {
					Msg("        Ignoring offsite URL - " + href);
					continue;
				}
				if (VisitedUrls.Contains(href)) {
					Msg("        Skipping already visited URL - " + href);
					continue;
				}

				// Handle links to, say, .zip files. Just download those.
				// We have a list of valid files of type .html (and the like). Walk
				// those, but download the rest.
				// TODO: Doesn't quite work. We probably need to check Mime types, not
				//		 .html et al


				// The following code is a bit kludgy. TODO:
				int LastSlash = href.LastIndexOf('/');
				int LastDot = href.LastIndexOf('.');
				// If there's no dot after the last /, then assume this is a link to
				// a page. For example, "www.sanfransys.com/pretext"
				if ((LastDot < LastSlash) || (href.Contains('?'))) {
					WalkAndSave(href);
					VisitedUrls.Add(href);
					continue;
				}

				bool bMustDownload = true;
				foreach (string suffix in HtmlSuffixes) {
					if (href.ToLower().EndsWith(suffix)) {
						WalkAndSave(href);
						bMustDownload = false;
						break;
					}
				}
				if (bMustDownload) {
					// TODO:
					Msg("        Nonce - not downloading " + href);
					// var wc = new WebClient();
					// wc.DownloadFile(href, "TODO:");
					VisitedUrls.Add(href);
				}
			}
			VisitedUrls.Add(web.Url.ToString());
		}

//---------------------------------------------------------------------------------------

		private void btnSwap_Click(object sender, EventArgs e) {
			BigBrowser = !BigBrowser;	// Toggle
			int TotalWidth = lbMsgs.Width + web.Width;
			int gutter = web.Left - (lbMsgs.Left + lbMsgs.Width);
			const float BigWidth = .80f;		// 80%
			int	MsgsWidth, BrowserWidth;
			if (BigBrowser) {
				BrowserWidth = (int)(TotalWidth * BigWidth);
			} else {
				BrowserWidth = (int)(TotalWidth * (1.0f - BigWidth));
			}
			MsgsWidth = TotalWidth - BrowserWidth;
			web.Width = BrowserWidth;
			web.Left = lbMsgs.Left + MsgsWidth + gutter;
			lbMsgs.Width = MsgsWidth;
		}
	}
}
