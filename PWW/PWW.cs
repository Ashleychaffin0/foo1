using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace PWW {
	public partial class PWW : Form {
		readonly TheApp app;

//---------------------------------------------------------------------------------------

		public PWW() {
			InitializeComponent();

			var TheTest = new TestApp();
			TheTest.Doit();

			app = new TheApp();
			var users = app.db.Users.Select(u => u).ToArray();
			cmbUsers.DataSource = users;		// TODO: Default to Env.UserName
		}

//---------------------------------------------------------------------------------------

		private void CmbUsers_SelectedIndexChanged(object sender, EventArgs e) {
			int userID = ((User)cmbUsers.SelectedItem).UserId;

			var qryCategories = from cat in app.db.Categories
								where cat.UserId == userID
								orderby cat.CategoryName
								select cat;
			CmbCategories.DataSource = qryCategories.ToArray();

			var qrySites = from site in app.db.Sites
					  where site.UserId == userID
					  orderby site.SiteName
					  select site;
			CmbSites.DataSource = qrySites.ToArray();
		}

//---------------------------------------------------------------------------------------

		private void CmbCategories_SelectedIndexChanged(object sender, EventArgs e) {
			var cat = (Category)CmbCategories.SelectedItem;
			// TODO:
		}

//---------------------------------------------------------------------------------------

		private void CmbSites_SelectedIndexChanged(object sender, EventArgs e) {
			var site = (Site)CmbSites.SelectedItem;
			LblSiteUrl.Text = site.SiteUrl;

			var qry = from logpass in app.db.LogPasses
					  where logpass.SiteId == site.SiteId
						&& logpass.UserId  == site.UserId
					  select logpass;
			LogPass logpassInfo = qry.First();
			LblLoginID.Text		= logpassInfo.LogonID;
			LblPassword.Text	= logpassInfo.Password;

			var qryComments = from comment in app.db.Comments
							  where comment.SiteId == site.SiteId
								&& comment.UserId == site.UserId
							  select comment;
			Comment comm = qryComments.First();
			LblComments.Text = comm.Text;
		}

//---------------------------------------------------------------------------------------

		private void BtnCopyLoginID_Click(object sender, EventArgs e) => CopyToClipboard(LblLoginID.Text);

//---------------------------------------------------------------------------------------

		private void BtnCopyPassword_Click(object sender, EventArgs e) => CopyToClipboard(LblPassword.Text);

//---------------------------------------------------------------------------------------

		private void BtnCopySiteUrl_Click(object sender, EventArgs e) => CopyToClipboard(LblSiteUrl.Text);

//---------------------------------------------------------------------------------------

		private static void CopyToClipboard(string text) {
			var psi = new ProcessStartInfo("powershell") {
				Arguments = $"write-output '{text}' | CLIP",
				CreateNoWindow = true
			};
			var proc = Process.Start(psi);
			proc!.WaitForExit();
		}
	}
}
