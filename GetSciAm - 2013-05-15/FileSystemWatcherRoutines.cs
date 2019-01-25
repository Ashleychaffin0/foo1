using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsGetSciAm {
	internal class FileSystemWatcherRoutines {

		internal enum FsmState {
			Login,
			LoggedIn,
			CurrentIssue,
			SelectIssueWithinYear,
			SpecificIssue,
			Idle
		}

		// Corresponds to FsmState. Keep in synch.
		internal static Color[] FsmColors = {
				Color.Red, 				// Login
				Color.Green, 			// LoggedIn
				Color.Blue, 			// CurrentIssue
				Color.Chocolate, 		// SelectIssueWithinYear
				Color.Magenta,			// Specific Issue
				Color.Black				// Idle
			};

		internal static FsmState _State;
		internal static FsmState State {
			get { return _State; }
			set {
				string msg = string.Format("Switching state from {0} to {1}", _State, value);
				Logger.Log(msg);
				// statusStrip1.Text = msg;
				_State = value;
			}
		}

		FileSystemWatcher fsw;

		public FileSystemWatcherRoutines () {
			fsw = new FileSystemWatcher();
		}
	}
}
