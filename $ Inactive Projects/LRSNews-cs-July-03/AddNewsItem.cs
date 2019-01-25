using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LRSNews_cs_July_03 {
	/// <summary>
	/// Summary description for AddNewsItem.
	/// </summary>
	public class AddNewsItem : System.Windows.Forms.Form {
		#region GUI
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.TextBox txtTitle;
		private System.Windows.Forms.TextBox txtComments;
		private System.Windows.Forms.TextBox txtDate;
		private System.Windows.Forms.TextBox txtArticleText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnPaste;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnPastAndScrubDate;
		private System.Windows.Forms.Button btnTodayAndPasteAndScrub;
		private System.Windows.Forms.Button btnToday;
		private System.Windows.Forms.Button btnYesterday;
		private System.Windows.Forms.Button btnSwapMM_DD;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnDropPrefix;
		private System.Windows.Forms.CheckBox chkSendAsFullMsg;
		private System.Windows.Forms.CheckBox chkSendToKRC;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtArticleTextLength;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		LRSNewsItem	NewsItem;
		DataSet		dsLRSNews;
		DataTable	dtMags;

		// Info from Mags table
		bool		ReverseDates;
		Regex		reDate,
					reTitle,
					reBody,
					reTimeAndDate;
		private System.Windows.Forms.Button btnReloadMags;

		LRSNewsForm	MainForm;


//---------------------------------------------------------------------------------------

		public AddNewsItem(DataSet ds, LRSNewsForm MainForm) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			dsLRSNews = ds;

			dtMags = ds.Tables["Mags"];

			this.MainForm = MainForm;
		}

//---------------------------------------------------------------------------------------

		public void ShowNewItem(LRSNewsItem NewsItem) {
			DataRow			dr;

			this.NewsItem		= NewsItem;
			txtURL.Text			= NewsItem.URL;
			txtTitle.Text		= MainForm.StripPrefixesSuffixes(NewsItem.Title);
			txtArticleText.Text = NewsItem.doc.body.innerText;
			dr = FindMag();
			SetupRegexs(dr);
			if (dr != null) {
				// TODO:
				FindAndShowDateAndTime();
			}
			this.Show();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Finds the DataRow in the dtMags table corresponding to the current URL.
		/// </summary>
		/// <param name="URL">The URL to search for.</param>
		/// <returns>Either the corresponding DataRow, or null if not found.</returns>
		DataRow	FindMag() {
			Uri			URI		= new Uri(NewsItem.doc.url);		
			string		host	= URI.Host;
#if false
			string []	hosts	= host.Split(".".ToCharArray());
			string		site	= hosts[hosts.Length - 2];
#endif
			foreach (DataRow dr in dtMags.Rows) {
				// TODO: The following test isn't the strongest one possible,
				// but it's the one we'll use for now. I'm not sure, but maybe
				// EndsWith is the right one to use.
				if (host.IndexOf((string)dr["NodesToMatch"]) >= 0)
					return dr;
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		void FindAndShowDateAndTime() {
			// TODO: Temp routine at the moment
			Match		m;
			Group		g;
			Capture		c;
			string		InnerText = NewsItem.doc.body.innerText;
			string		dateString = null;
			string		title = NewsItem.doc.title;

			// TODO: Just playing around at the moment

			// In case not found
			txtDate.Text = "";
			txtArticleText.Text = InnerText;

			// First, try the Date re
			if (reDate != null) {
				m = reDate.Match(InnerText);
				if (m.Success) {
					g = m.Groups["date"];
					dateString = g.ToString();
					c = g.Captures[0];
					int ix = c.Index;
					int len = c.Length;
					string intext = InnerText.Substring(ix, len);
					txtDate.Text = intext;
				}
			}

			// Now find the body
			if (reBody != null) {
				m = reBody.Match(InnerText);
				if (m.Success) {
					g = m.Groups["body"];
					dateString = g.ToString();
					c = g.Captures[0];
					int ix = c.Index;
					string intext = InnerText.Substring(ix);
					txtArticleText.Text = intext;
					int len = c.Length;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void btnCancel_Click(object sender, System.EventArgs e) {
			this.Hide();
		}

//---------------------------------------------------------------------------------------

		void SetupRegexs(DataRow dr) {
			// TODO: This method's name is a misnomer; there's more going on here
			// than setting up regexes.

			// Assume the worst, either because we don't have a DataRow, or because one
			// or more of the regex strings are missing.
			reDate		  = null;
			reTitle		  = null;
			reBody		  = null;
			reTimeAndDate = null;

			if (dr == null) {
				return;
			}

			ReverseDates = (bool)dr["ReverseDates"];

			// Note: Instead of RegexOptions.Singleline below, I think we can put
			// (?s) or (?-s) inline.
			// TODO: Test the above

			string		s;
			if (! Convert.IsDBNull(dr["RegexToFindDate"])) {
				s = (string)dr["RegexToFindDate"];
				reDate = new Regex(s, RegexOptions.Singleline);
			}
			if (! Convert.IsDBNull(dr["RegexToFindTitle"])) {
				s = (string)dr["RegexToFindTitle"];
				reTitle = new Regex(s, RegexOptions.Singleline);
			}
			if (! Convert.IsDBNull(dr["RegexToFindBody"])) {
				s = (string)dr["RegexToFindBody"];
				reBody = new Regex(s, RegexOptions.Singleline);
			}
			if (! Convert.IsDBNull(dr["RegexToFindTitleAndDate"])) {
				s = (string)dr["RegexToFindTitle"];
				reTimeAndDate = new Regex(s, RegexOptions.Singleline);
			}
		}

//---------------------------------------------------------------------------------------

		private void AddNewsItem_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			e.Cancel = true;
			this.Hide();
		}

//---------------------------------------------------------------------------------------

		private void btnReloadMags_Click(object sender, System.EventArgs e) {
			MainForm.ReadMagsTable();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)	{
			if (disposing) {
				if (components != null)	{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtURL = new System.Windows.Forms.TextBox();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.txtComments = new System.Windows.Forms.TextBox();
			this.txtDate = new System.Windows.Forms.TextBox();
			this.txtArticleText = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnPaste = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnPastAndScrubDate = new System.Windows.Forms.Button();
			this.btnTodayAndPasteAndScrub = new System.Windows.Forms.Button();
			this.btnToday = new System.Windows.Forms.Button();
			this.btnYesterday = new System.Windows.Forms.Button();
			this.btnSwapMM_DD = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnDropPrefix = new System.Windows.Forms.Button();
			this.chkSendAsFullMsg = new System.Windows.Forms.CheckBox();
			this.chkSendToKRC = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtArticleTextLength = new System.Windows.Forms.TextBox();
			this.btnReloadMags = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtURL
			// 
			this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtURL.Location = new System.Drawing.Point(80, 16);
			this.txtURL.Multiline = true;
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(496, 32);
			this.txtURL.TabIndex = 0;
			this.txtURL.Text = "";
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitle.Location = new System.Drawing.Point(80, 56);
			this.txtTitle.Multiline = true;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(640, 32);
			this.txtTitle.TabIndex = 1;
			this.txtTitle.Text = "";
			// 
			// txtComments
			// 
			this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtComments.Location = new System.Drawing.Point(80, 96);
			this.txtComments.Multiline = true;
			this.txtComments.Name = "txtComments";
			this.txtComments.Size = new System.Drawing.Size(640, 48);
			this.txtComments.TabIndex = 2;
			this.txtComments.Text = "";
			// 
			// txtDate
			// 
			this.txtDate.Location = new System.Drawing.Point(80, 152);
			this.txtDate.Multiline = true;
			this.txtDate.Name = "txtDate";
			this.txtDate.Size = new System.Drawing.Size(160, 24);
			this.txtDate.TabIndex = 3;
			this.txtDate.Text = "";
			// 
			// txtArticleText
			// 
			this.txtArticleText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtArticleText.Location = new System.Drawing.Point(8, 240);
			this.txtArticleText.Multiline = true;
			this.txtArticleText.Name = "txtArticleText";
			this.txtArticleText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtArticleText.Size = new System.Drawing.Size(712, 136);
			this.txtArticleText.TabIndex = 4;
			this.txtArticleText.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 32);
			this.label1.TabIndex = 5;
			this.label1.Text = "URL";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 32);
			this.label2.TabIndex = 6;
			this.label2.Text = "Title";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 32);
			this.label3.TabIndex = 7;
			this.label3.Text = "Comments";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 152);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 24);
			this.label4.TabIndex = 8;
			this.label4.Text = "Date";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnPaste
			// 
			this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPaste.BackColor = System.Drawing.SystemColors.Control;
			this.btnPaste.Location = new System.Drawing.Point(592, 16);
			this.btnPaste.Name = "btnPaste";
			this.btnPaste.Size = new System.Drawing.Size(56, 32);
			this.btnPaste.TabIndex = 9;
			this.btnPaste.Text = "Paste";
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClear.BackColor = System.Drawing.SystemColors.Control;
			this.btnClear.Location = new System.Drawing.Point(664, 16);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(56, 32);
			this.btnClear.TabIndex = 10;
			this.btnClear.Text = "Clear";
			// 
			// btnPastAndScrubDate
			// 
			this.btnPastAndScrubDate.BackColor = System.Drawing.SystemColors.Control;
			this.btnPastAndScrubDate.Location = new System.Drawing.Point(280, 152);
			this.btnPastAndScrubDate.Name = "btnPastAndScrubDate";
			this.btnPastAndScrubDate.Size = new System.Drawing.Size(96, 32);
			this.btnPastAndScrubDate.TabIndex = 11;
			this.btnPastAndScrubDate.Text = "Paste and Scrub Date";
			// 
			// btnTodayAndPasteAndScrub
			// 
			this.btnTodayAndPasteAndScrub.BackColor = System.Drawing.SystemColors.Control;
			this.btnTodayAndPasteAndScrub.Location = new System.Drawing.Point(384, 152);
			this.btnTodayAndPasteAndScrub.Name = "btnTodayAndPasteAndScrub";
			this.btnTodayAndPasteAndScrub.Size = new System.Drawing.Size(96, 32);
			this.btnTodayAndPasteAndScrub.TabIndex = 12;
			this.btnTodayAndPasteAndScrub.Text = "Today + Paste + Scrub";
			// 
			// btnToday
			// 
			this.btnToday.BackColor = System.Drawing.SystemColors.Control;
			this.btnToday.Location = new System.Drawing.Point(648, 152);
			this.btnToday.Name = "btnToday";
			this.btnToday.Size = new System.Drawing.Size(72, 32);
			this.btnToday.TabIndex = 13;
			this.btnToday.Text = "Today";
			// 
			// btnYesterday
			// 
			this.btnYesterday.BackColor = System.Drawing.SystemColors.Control;
			this.btnYesterday.Location = new System.Drawing.Point(568, 152);
			this.btnYesterday.Name = "btnYesterday";
			this.btnYesterday.Size = new System.Drawing.Size(72, 32);
			this.btnYesterday.TabIndex = 14;
			this.btnYesterday.Text = "Yesterday";
			// 
			// btnSwapMM_DD
			// 
			this.btnSwapMM_DD.BackColor = System.Drawing.SystemColors.Control;
			this.btnSwapMM_DD.Location = new System.Drawing.Point(488, 152);
			this.btnSwapMM_DD.Name = "btnSwapMM_DD";
			this.btnSwapMM_DD.Size = new System.Drawing.Size(72, 32);
			this.btnSwapMM_DD.TabIndex = 15;
			this.btnSwapMM_DD.Text = "Swap mm/dd";
			// 
			// btnOK
			// 
			this.btnOK.BackColor = System.Drawing.SystemColors.Control;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(432, 200);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 32);
			this.btnOK.TabIndex = 16;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(504, 200);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 32);
			this.btnCancel.TabIndex = 17;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnDropPrefix
			// 
			this.btnDropPrefix.BackColor = System.Drawing.SystemColors.Control;
			this.btnDropPrefix.Location = new System.Drawing.Point(624, 200);
			this.btnDropPrefix.Name = "btnDropPrefix";
			this.btnDropPrefix.Size = new System.Drawing.Size(96, 32);
			this.btnDropPrefix.TabIndex = 18;
			this.btnDropPrefix.Text = "Drop Prefix";
			// 
			// chkSendAsFullMsg
			// 
			this.chkSendAsFullMsg.Location = new System.Drawing.Point(8, 184);
			this.chkSendAsFullMsg.Name = "chkSendAsFullMsg";
			this.chkSendAsFullMsg.Size = new System.Drawing.Size(176, 24);
			this.chkSendAsFullMsg.TabIndex = 19;
			this.chkSendAsFullMsg.Text = "Send as Full Msg";
			// 
			// chkSendToKRC
			// 
			this.chkSendToKRC.Checked = true;
			this.chkSendToKRC.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSendToKRC.Location = new System.Drawing.Point(8, 208);
			this.chkSendToKRC.Name = "chkSendToKRC";
			this.chkSendToKRC.Size = new System.Drawing.Size(176, 24);
			this.chkSendToKRC.TabIndex = 20;
			this.chkSendToKRC.Text = "Send to KRC";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(200, 192);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 21;
			this.label5.Text = "Text Length";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtArticleTextLength
			// 
			this.txtArticleTextLength.Location = new System.Drawing.Point(200, 208);
			this.txtArticleTextLength.Name = "txtArticleTextLength";
			this.txtArticleTextLength.Size = new System.Drawing.Size(64, 20);
			this.txtArticleTextLength.TabIndex = 22;
			this.txtArticleTextLength.Text = "";
			this.txtArticleTextLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnReloadMags
			// 
			this.btnReloadMags.BackColor = System.Drawing.SystemColors.Control;
			this.btnReloadMags.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnReloadMags.Location = new System.Drawing.Point(280, 200);
			this.btnReloadMags.Name = "btnReloadMags";
			this.btnReloadMags.Size = new System.Drawing.Size(88, 32);
			this.btnReloadMags.TabIndex = 23;
			this.btnReloadMags.Text = "Reload Mags";
			this.btnReloadMags.Click += new System.EventHandler(this.btnReloadMags_Click);
			// 
			// AddNewsItem
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Aqua;
			this.ClientSize = new System.Drawing.Size(728, 382);
			this.Controls.Add(this.btnReloadMags);
			this.Controls.Add(this.txtArticleTextLength);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.chkSendToKRC);
			this.Controls.Add(this.chkSendAsFullMsg);
			this.Controls.Add(this.btnDropPrefix);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnSwapMM_DD);
			this.Controls.Add(this.btnYesterday);
			this.Controls.Add(this.btnToday);
			this.Controls.Add(this.btnTodayAndPasteAndScrub);
			this.Controls.Add(this.btnPastAndScrubDate);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnPaste);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtArticleText);
			this.Controls.Add(this.txtDate);
			this.Controls.Add(this.txtComments);
			this.Controls.Add(this.txtTitle);
			this.Controls.Add(this.txtURL);
			this.Name = "AddNewsItem";
			this.Text = "AddNewsItem";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.AddNewsItem_Closing);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
