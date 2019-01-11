using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace NetRWStatsW {
	public partial class NetRWStatsW : Form {

		long		TotSent = 0, 
					TotReceived = 0;
		bool		Paused;

//---------------------------------------------------------------------------------------

		public NetRWStatsW() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void NeyRWStatsW_Load(object sender, EventArgs e) {
			cmbUnits.SelectedIndex = 0;
			Paused = false;
			timer1.Start();
		}

//---------------------------------------------------------------------------------------

		private void timer1_Tick(object sender, EventArgs e) {
			Recalc();
		}

//---------------------------------------------------------------------------------------

		private void Recalc() {
			// TODO: Minor bug -- If we're paused, but we then change the Units, it won't
			//		 just reformat what we have on the screen, but will update the
			//		 stats. But I'll worry about that later.
			long	OldTotReceived = TotReceived,
					OldTotSent	 = TotSent;
			long	Received = 0, Sent = 0;
			long	DeltaReceived, DeltaSent;
			var ifaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (var iface in ifaces) {
				var stats = iface.GetIPv4Statistics();
				// if ((stats.BytesReceived + stats.BytesSent) == 0)
				// 	continue;
				Received += stats.BytesReceived;
				Sent	 += stats.BytesSent;
			}
			DeltaReceived = Received - OldTotReceived;
			DeltaSent	  = Sent - OldTotSent;
			TotReceived  += DeltaReceived;
			TotSent		 += DeltaSent;

			lblDeltaReceived.Text = fmt(DeltaReceived);
			lblDeltaSent.Text	  = fmt(DeltaSent);

			lblTotalReceived.Text = fmt(TotReceived);
			lblTotalSent.Text	  = fmt(TotSent);
		}

//---------------------------------------------------------------------------------------

		private string fmt(long n) {
			string	Units = (string)cmbUnits.SelectedItem;
			long	Factor = 1;
			string suf = "";
			switch (Units) {
			case "Bytes":
				Factor = 1;
				suf = "";
				break;
			case "MB":
				Factor = 1000 * 1000;
				suf = " MB";
				break;
			case "GB":
				Factor = 1000 * 1000 * 1000;
				suf = " GB";
				break;
			default:
				// We don't want the timer to fire while we're waiting for the user to
				// click OK. Turn it off, then afterwards, turn it back on again.
				timer1.Stop();
				MessageBox.Show("Internal Error - Unrecognized Units value - " + Units + ". Using Bytes.",
					"NetIOStats", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbUnits.SelectedItem = "Bytes";
				if (!Paused) {
					timer1.Start();
				}
				goto case "Bytes";
			}
			long val = (n + Factor / 2) / Factor;
			return string.Format("{0:N0} {1}", val, suf);
		}

//---------------------------------------------------------------------------------------

		private void cmbUnits_SelectedIndexChanged(object sender, EventArgs e) {
			Recalc();
		}

//---------------------------------------------------------------------------------------

		private void BtnStopGo_Click(object sender, EventArgs e) {
			switch (BtnStopGo.Text) {
			case "Pause":
				timer1.Stop();
				Paused = true;
				BtnStopGo.Text = "Resume";
				break;
			case "Resume":
				BtnStopGo.Text = "Pause";
				Paused = false;
				Recalc();
				timer1.Start();
				break;
			}
		}
	}
}
