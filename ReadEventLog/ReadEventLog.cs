using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadEventLog {
	public partial class ReadEventLog : Form {
		public ReadEventLog() {
			InitializeComponent();

			// var log = new EventLog("Security", System.Environment.MachineName);
			var logs = EventLog.GetEventLogs(".");
			foreach (var item in logs) {
				try {
					Console.WriteLine(item.LogDisplayName);
				} catch {

				}
			}
			var log = new EventLog("Dell", ".");
			// log.Log.
			foreach (EventLogEntry item in log.Entries) {
				try {
					Console.WriteLine(item.Message);
				} catch {

				}
			}
		}
	}
}
