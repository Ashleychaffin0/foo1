// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LRSS {
    public partial class frmAddRSSFeed : Form {

		public RSSFeed		feed;

//---------------------------------------------------------------------------------------

        public frmAddRSSFeed() {
            InitializeComponent();
        }

//---------------------------------------------------------------------------------------

        private void AddRSSFeed_Load(object sender, EventArgs e) {
			string URL = Clipboard.GetData("Text") as string;
            if (URL == null)
                return;
            txtURL.Text = URL;
			txtURL.SelectAll();
        }

//---------------------------------------------------------------------------------------

		private void btnGetFeed_Click(object sender, EventArgs e) {
			feed = new RSSFeed(txtURL.Text);
			try {
				this.Cursor = Cursors.WaitCursor;
				feed.Load();
			} finally {
				this.Cursor = Cursors.Default;
			}
			if (feed.LoadedOK) {
				txtTitle.Text = feed.Title;
				txtDescription.Text = feed.Description;
			} else {
				MessageBox.Show("*Unable to load feed*", "LRSRSS",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				feed = null;
			}
		}
    }
}