using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Microsoft.Win32;

namespace AlarmClock {
	public partial class AlarmClock : Form {
		const string ZuneMusicDirectory = @"C:\$ Zune Master";

		DateTime AlarmTime;
		Timer	 Timer2;				// For showing TOD
		string	 ZuneDir;

//---------------------------------------------------------------------------------------

		public AlarmClock() {
			InitializeComponent();

			ZuneDir = FindZuneDirectory();

			Timer2 = new Timer();
			Timer2.Interval = 1000;				// One second
			Timer2.Tick += new EventHandler(Timer2_Tick);
			Timer2.Start();
		}

//---------------------------------------------------------------------------------------

		private static string FindZuneDirectory() {
			RegistryKey zKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Zune");
			if (zKey == null) {
				MessageBox.Show("It looks like the Zune software isn't installed on this machine.");
				return null;
			}
			string ZuneDirName = (string)zKey.GetValue("Installation Directory");
			zKey.Close();
			if (ZuneDirName == null) {
				MessageBox.Show("Unable to find Zune installation directory.");
				return null;
			}

			if (!System.IO.Directory.Exists(ZuneDirName)) {
				MessageBox.Show("Can't find Zune directory.");
				return null;
			}
			return ZuneDirName;
		}

//---------------------------------------------------------------------------------------

		void Timer2_Tick(object sender, EventArgs e) {
			lblCurrentTime.Text = DateTime.Now.ToLongTimeString();
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseFileToPlay_Click(object sender, EventArgs e) {
			openFileDialog1.FileName = "";
			openFileDialog1.Filter = "WMA files|*.wma|MP3 files|*.mp3|Wave files|*.wav|All files|*.*";
			// openFileDialog1.DefaultExt = ".wav";
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.InitialDirectory = ZuneMusicDirectory;
			var ok = openFileDialog1.ShowDialog();
			if (ok == DialogResult.OK) {
				txtFilename.Text = openFileDialog1.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			if (btnGo.Text == "Go") {
				ProcessGoClick();
			} else {
				ProcessCancelClick();
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessCancelClick() {
			timer1.Enabled = false;
			btnGo.Text = "Go";
		}

//---------------------------------------------------------------------------------------

		private void ProcessGoClick() {
			if (!ParseSpecifiedTime()) {
				MessageBox.Show("Either the specified time is invalid, or it does not lie in the future.", "Alarm Clock");
				return;
			}
#if false
			if (!File.Exists(txtFilename.Text)) {
				MessageBox.Show("The specified file does not exist.");
				return;
			}
#endif

			if (txtFilename.Text.Length == 0 && txtFolderName.Text.Length == 0) {
				MessageBox.Show("You must specify a file or a folder");
				return;
			}

			if (AlarmTime != default(DateTime)) {
				WaitUntil(AlarmTime);
			}

			btnGo.Text = "Cancel";
		}

//---------------------------------------------------------------------------------------

		private void WaitUntil(DateTime AlarmTime) {
			timer1.Tick += new EventHandler(timer1_Tick);
			timer1.Interval = (int)((AlarmTime - DateTime.Now).TotalMilliseconds);
			timer1.Enabled = true;
			timer1.Start();
		}

//---------------------------------------------------------------------------------------

		void timer1_Tick(object sender, EventArgs e) {
			//var sp = new SoundPlayer(txtFilename.Text);
			timer1.Stop();
			ProcessCancelClick();
			//sp.PlayLooping();

			if (ZuneDir != null) {		// Use Zune if available
				// zune -playmedia:"C:\$ Zune Master\The Beatles\Love"
				string parms = "-playmedia:";
				if (txtFolderName.Text.Length > 0) {
					parms += @"""" + txtFolderName.Text + @"""";
				} else {
					parms +=  @"""" + txtFilename.Text + @"""";
				}
				Process.Start(Path.Combine(ZuneDir, "zune.exe"), parms);
			} else {
				Process.Start(txtFilename.Text);
			}
		}

//---------------------------------------------------------------------------------------

		private bool ParseSpecifiedTime() {
			bool bOK;
			int hour, minute, second;
			bOK = int.TryParse(txtHour.Text, out hour);
			if (! bOK) return false;
			bOK = int.TryParse(txtMinute.Text, out minute);
			if (!bOK) return false;
			bOK = int.TryParse(txtSecond.Text, out second);
			if (!bOK) return false;

			if (hour > 12) return false;
			if (minute > 60) return false;
			if (second > 60) return false;

			if (radPM.Checked) {
				hour += 12;
			}

			AlarmTime = dateTimePicker1.Value.Date + new TimeSpan(hour, minute, second);
			if (AlarmTime <= DateTime.Now) {
				AlarmTime = default(DateTime);
				return false;
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private void btnClearFileToPlay_Click(object sender, EventArgs e) {
			txtFilename.Text = "";
		}

//---------------------------------------------------------------------------------------

		private void btnClearFolder_Click(object sender, EventArgs e) {
			txtFolderName.Text = "";
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseForFolder_Click(object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog();
			fbd.SelectedPath = ZuneMusicDirectory;
			fbd.ShowNewFolderButton = false;
			if (fbd.ShowDialog() == DialogResult.OK) {
				txtFolderName.Text = fbd.SelectedPath;
			}
		}
	}
}
