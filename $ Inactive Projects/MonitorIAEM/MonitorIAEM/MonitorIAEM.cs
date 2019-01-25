using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace MonitorIAEM {
	public partial class MonitorIAEM : Form {
	
		SqlConnection	connDevel, connProd;
		int				PrevDevelSwipeCount = 0, PrevProdSwipeCount = 0;
		int				PrevDevelTemps = 0, PrevProdTemps = 0;

//---------------------------------------------------------------------------------------
	
		public MonitorIAEM() {
			InitializeComponent();

			lblSwipes.Width = lvSwipes.Width;	// Auto-adjust. I keep forgetting to.

			ConnectToDatabase();

			timer1.Enabled = true;
			btnGo_Click(null, null);
			timer1_Tick(null, null);
		}

//---------------------------------------------------------------------------------------

		private void ConnectToDatabase() {
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			builder.DataSource		= "SQLB5.webcontrolcenter.com";
			builder.InitialCatalog	= "LLDevel";
			builder.UserID			= "ahmed";
			builder.Password		= "i7e9dua$tda@";

			connDevel = new SqlConnection(builder.ConnectionString);
			connDevel.Open();

			builder = new SqlConnectionStringBuilder();
			builder.DataSource		= "SQLB2.webcontrolcenter.com";
			builder.InitialCatalog	= "LeadsLightning";
			builder.UserID			= "ahmed";
			builder.Password		= "i7e9dua$tda@";
			connProd = new SqlConnection(builder.ConnectionString);
			connProd.Open();
		}

//---------------------------------------------------------------------------------------

		private void timer1_Tick(object sender, EventArgs e) {
			lblLastUpdate.Text = string.Format("Last updated: {0}", DateTime.Now);

			DoSwipes();
		}

//---------------------------------------------------------------------------------------

		private void DoSwipes() {
			string		SQL = "SELECT COUNT(*) FROM tblSwipes";

			SqlCommand	cmd = new SqlCommand(SQL, connDevel);
			int			nSwipesDevel = (int)cmd.ExecuteScalar();

			cmd = new SqlCommand(SQL, connProd);
			int			nSwipesProd  = (int)cmd.ExecuteScalar();

			SQL = "SELECT COUNT(*) as nRecs FROM sys.tables WHERE name LIKE '_temp%'";
			cmd = new SqlCommand(SQL, connDevel);
			int		nDevelTemps = (int)cmd.ExecuteScalar();

			cmd = new SqlCommand(SQL, connProd);
			int		nProdTemps = (int)cmd.ExecuteScalar();

			// Don't show periods of no activity
			int			DeltaDevelSwipes = nSwipesDevel - PrevDevelSwipeCount;
			int			DeltaProdSwipes  = nSwipesProd  - PrevProdSwipeCount;
			int			DeltaDevelTemps	 = nDevelTemps  - PrevDevelTemps;
			int			DeltaProdTemps	 = nProdTemps	- PrevProdTemps;
			if ((DeltaDevelSwipes + DeltaProdSwipes + DeltaDevelTemps + DeltaProdTemps) == 0) {
				return;
			}

			//string [] items = new string[9];
			// ListViewItem.ListViewSubItemCollection	items = new ListViewItem.ListViewSubItemCollection(item);
			DateTime	WestCoast = DateTime.Now.AddHours(-3);
			/*0*/ ListViewItem	item = new ListViewItem(string.Format("{0:hh.mm.ss}", WestCoast));
			/*1*/ item.SubItems.Add(nSwipesDevel.ToString());

			/*2*/ if (DeltaDevelSwipes == 0)
				item.SubItems.Add("");
			else
				item.SubItems.Add(DeltaDevelSwipes.ToString(), Color.Red, Color.Blue, lvSwipes.Font);
			
			/*3*/item.SubItems.Add(nSwipesProd.ToString());
			/*4*/if (DeltaProdSwipes == 0)
				item.SubItems.Add("");
			else
				item.SubItems.Add(DeltaProdSwipes.ToString(), Color.Red, Color.Blue, lvSwipes.Font);
			
			/*5*/item.SubItems.Add(nDevelTemps.ToString());
			/*6*/if (DeltaDevelTemps == 0)
				item.SubItems.Add("");
			else
				item.SubItems.Add(DeltaDevelTemps.ToString(), Color.Red, Color.Blue, lvSwipes.Font);
			
			/*7*/item.SubItems.Add(nProdTemps.ToString());
			/*8*/if (DeltaProdTemps == 0)
				item.SubItems.Add("");
			else
				item.SubItems.Add(DeltaProdTemps.ToString(), Color.Red, Color.Blue, lvSwipes.Font);
			
			/*
			items[2] = DeltaDevelSwipes == 0 ? "" : DeltaDevelSwipes.ToString();
			items[3] = nSwipesProd.ToString();
			items[4] = DeltaProdSwipes == 0 ? "" : DeltaProdSwipes.ToString();
			items[5] = nDevelTemps.ToString();
			items[6] = DeltaDevelTemps == 0 ? "" : DeltaDevelTemps.ToString();
			items[7] = nProdTemps.ToString();
			items[8] = DeltaProdTemps == 0 ? "" : DeltaProdTemps.ToString();
			 * */

#if true
			lvSwipes.Items.Insert(0, item);
#else
			item = new ListViewItem(items);
			lvSwipes.Items.Insert(0, item);
#endif

			string	title = string.Format("{0} - {1} - Monitor IAEM", DeltaDevelSwipes, DeltaProdSwipes);
			this.Text = title;

			PrevDevelSwipeCount = nSwipesDevel;
			PrevProdSwipeCount  = nSwipesProd;
			PrevDevelTemps		= nDevelTemps;
			PrevProdTemps		= nProdTemps;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			int		nMins;
			bool bOK = int.TryParse(txtUpdateInterval.Text, out nMins);
			if ((!bOK) || (nMins <= 0)) {
				MessageBox.Show("Please enter a postive integer", "Tsk Tsk", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			timer1.Stop();
			timer1.Interval = nMins * 60 * 1000;
			timer1.Start();
		}
	}
}