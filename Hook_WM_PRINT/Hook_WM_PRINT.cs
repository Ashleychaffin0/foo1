using System;
using System.Text;
using System.Windows.Forms;
using NativeWifi;

// See https://stackoverflow.com/questions/28493169/managed-wifi-api-examples

// https://www.bing.com/search?q=hooking+windows+api&form=EDGTCT&qs=AS&cvid=071e2d4b9cf14db1a39d43aa643154b7&cc=US&setlang=en-US&elv=AXXfrEiqqD9r3GuelwApulqNWBgdvqASjf5dBw1UOtbfqRrvy3trkuJFL4dQ9TX*0nUtHZvISwJN9e14zyRFt9b4pfmtSchG2vkLPpqbUikI&PC=DCTS
// See https://msdn.microsoft.com/en-us/library/windows/desktop/ms632589(v=vs.85).aspx
// See https://msdn.microsoft.com/en-us/library/windows/desktop/ms644959(v=vs.85).aspx#wh_callwndproc_wh_callwndprocret
// See https://msdn.microsoft.com/en-us/library/windows/desktop/ms644960(v=vs.85).aspx

// https://easyhook.github.io/

// https://stackoverflow.com/questions/7625421/minimize-app-to-system-tray

// https://docs.microsoft.com/en-us/cpp/build/walkthrough-creating-and-using-a-dynamic-link-library-cpp

// See https://stackoverflow.com/questions/9102814/how-to-hook-an-application

namespace Hook_WM_PRINT {
	public partial class Hook_WM_PRINT : Form {
		public Hook_WM_PRINT() {
			InitializeComponent();
			var wlc = new WlanClient();
			foreach (var @if in wlc.Interfaces) {
				Console.WriteLine(@if.CurrentConnection.profileName);
				var x = @if.GetProfiles();
				foreach (var prof in @if.GetProfiles()) {
					// Console.WriteLine(prof.profileName);
				}
				DoNetworks(@if);
			}
		}

//---------------------------------------------------------------------------------------

		private void DoNetworks(WlanClient.WlanInterface @if) {
			var networks = @if.GetAvailableNetworkList(0);
			foreach (Wlan.WlanAvailableNetwork network in networks) {
				Wlan.Dot11Ssid ssid = network.dot11Ssid;
				string networkName = Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
				Console.WriteLine(networkName);
			}
		}

//---------------------------------------------------------------------------------------

		private void Hook_WM_PRINT_Load(object sender, EventArgs e) {
				notifyIcon1.BalloonTipText  = "Application Minimized.";
				notifyIcon1.BalloonTipTitle = "test";
		}

//---------------------------------------------------------------------------------------

		private void Form1_Resize(object sender, EventArgs e) {
			if (WindowState == FormWindowState.Minimized) {
				ShowInTaskbar       = false;
				notifyIcon1.Visible = true;
				notifyIcon1.ShowBalloonTip(1000);
			}
		}

//---------------------------------------------------------------------------------------

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
			ShowInTaskbar       = true;
			notifyIcon1.Visible = false;
			WindowState         = FormWindowState.Normal;
		}
	}
}
