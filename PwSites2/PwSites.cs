using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PwSites2 {
	public partial class PwSites : Form {
		public PwSites() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void PwSites_Load(object sender, EventArgs e) {
			SetupUsers();
			CreateTables();

			using var ctx = new SiteContext();
			var site = new Site {
				SiteName  = "Amazon",
				Comments  = "None at the moment"
			};
			ctx.Sites.Add(site);
			ctx.SaveChanges();
		}

//---------------------------------------------------------------------------------------

		private void SetupUsers() {
			// Could get these from the database, but doesn't seem to be worth it
			CmbUsers.Items.Add("LRS");
			CmbUsers.Items.Add("BGA");
			CmbUsers.Items.Add("Judie");
			var who = Environment.UserName;
			CmbUsers.SelectedIndex = who switch {
				"lrs5" => 0,
				_ => 1
			};
		}

//---------------------------------------------------------------------------------------

		private void CreateTables() {
			string Sql = @"CREATE TABLE TblSite (
				SiteId INTEGER NOT NULL CONSTRAINT PK_TblSite PRIMARY KEY AUTOINCREMENT,
				SiteName TEXT NULL,
				Comments TEXT NULL,
				UidPwId INTEGER NOT NULL)";

			//

			Sql = @"CREATE TABLE TblUidPW (
			UidPwId INTEGER NOT NULL CONSTRAINT PK_TblUidPW PRIMARY KEY AUTOINCREMENT,
			UserName TEXT NULL,
			Password TEXT NULL)";

			//

			Sql = @"CREATE TABLE TblMapSiteToUidPW (
				UidPwId INTEGER NOT NULL CONSTRAINT PK_TblMapSiteToUidPW PRIMARY KEY AUTOINCREMENT,
				IDID INTEGER NOT NULL,
				SiteId INTEGER NOT NULL)";

			//
		}
	}
}
