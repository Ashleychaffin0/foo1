using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// TODO: Add UI support for things like
//	*	The URL to check
//	*	How often to check (timer interval)
//	*	Regex pattern (\bdelivered\b)
//	*	Number of patterns found if not delivered
//	*	Sound file to play
//	*	Whether it's UPS or Fedex
//	*	Define options (.xml) file, for things like the above points

namespace CheckForUpsDelivery {
	public partial class CheckForUpsDelivery : Form {

		Timer tmr;
		WebClient wc;
		Regex re;

		// string UpsUrl = "https://www.ups.com/WebTracking/processInputRequest?loc=en_US&Requester=DAN&tracknum=1ZY549V71351363638&AgreeToTermsAndConditions=yes&WT.z_eCTAid=ct1_eml_Tracking__ct1_eml_tra_eml_1day&WT.z_edatesent=12122016";
		// string FedExUrl = "https://www.fedex.com/apps/fedextrack/?action=track&ascend_header=1&clienttype=dotcom&cntry_code=us&language=english&tracknumbers=718331205304";
		// string DeliveryUrl;

		bool bIsFedEx;					// TODO: Never used. But keep, just in case...
		int Max_Delivered_Count;		// TODO: Need better name

		string SoundToPlay = @"D:\$ Zune Master\Albert Collins\Cold Snap\1 Cash Talkin' (The Workingman's Blues).wma";

//---------------------------------------------------------------------------------------

		public CheckForUpsDelivery() {
            InitializeComponent();
        }

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			wc = new WebClient();
			re = new Regex(@"\bDelivered\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			if (TxtUrl.Text.ToUpper().Contains(".FEDEX.COM")) {
				// Note: Could be other shipper, maybe DHL. Fix that when it happens
				bIsFedEx = true;
				Max_Delivered_Count = 1;
			} else {
				bIsFedEx = false;
				Max_Delivered_Count = 2;
			}

			tmr = new Timer() {
				Interval = 10 * 60 * 1000  // 10 minutes
				// Interval = 1000
			};
			tmr.Tick += Tmr_Tick;
			// The first check won't be done until the first Tick event (e.g. 10 minutes from now)
			// So do a manual check right away
			Check();
			tmr.Start();
		}

		private void Tmr_Tick(object sender, EventArgs e) {
			Check();
		}

		private void Check() {
			Msg("Checking...");
			var html = wc.DownloadString(TxtUrl.Text);
			var m = re.Matches(html);
			if (m.Count > Max_Delivered_Count) {
				tmr.Stop();
				Msg("Got it!");
				Process.Start(SoundToPlay);
			}
		}

		private void Msg(string s) {
			lbMsgs.Items.Insert(0, $"{DateTime.Now:hh:mm:ss} {s}");
		}
	}
}
