using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace LLConcurrencyAnalysis {
	public partial class LLConcurrencyAnalysis : Form {
		public LLConcurrencyAnalysis() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void LLConcurrencyAnalysis_Load(object sender, EventArgs e) {
		}

//---------------------------------------------------------------------------------------

		private void btnDeviceID_Click(object sender, EventArgs e) {
			int			EventID;
			List<int>	AcctIDs;

			// TODO: Worry about GUI later
			EventID                  = 313;
			// AcctIDs               = new List<int>() {2670, 2671};
			AcctIDs                  = Enumerable.Range(2739, 1).ToList();
			CheckDeviceID chkDevID   = new CheckDeviceID(EventID, AcctIDs);
			CheckDeviceID2 chkDevID2 = new CheckDeviceID2(EventID, AcctIDs);
			Stopwatch	sw           = new Stopwatch();

			sw.Start();
			// chkDevID.Run(lbMsgs);
			chkDevID2.Run(lbMsgs);
			sw.Stop();

			string msg = string.Format("Total elapsed time = {0}", sw.Elapsed);
			Console.WriteLine("{0}", msg);
			lbMsgs.Items.Add(msg);

			MessageBox.Show("Done");
		}
	}
}
