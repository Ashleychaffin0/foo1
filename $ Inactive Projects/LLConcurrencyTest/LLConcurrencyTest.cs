// Copyright (c) 2008 by Bartizan Connects, LLC

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LLConcurrencyTest {
	public partial class LLConcurrencyTest : Form {

		List<ImporterStatusControl>	ImporterControls;
		List<ImportThreadInfo>		ThreadInfo;

//---------------------------------------------------------------------------------------

		public LLConcurrencyTest() {
			InitializeComponent();

			ImporterControls = new List<ImporterStatusControl>();
		}

//---------------------------------------------------------------------------------------

		private void LLConcurrencyTest_Load(object sender, EventArgs e) {
			cmbDatabase.SelectedIndex = cmbDatabase.Items.IndexOf("DEVEL");
			string	MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			txtOutputDirectory.Text = MyDocs;		// Default
		}

//---------------------------------------------------------------------------------------

		private void btnSetOutputDir_Click(object sender, EventArgs e) {
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.ShowNewFolderButton = true;
			fbd.SelectedPath = txtOutputDirectory.Text;
			DialogResult res = fbd.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			txtOutputDirectory.Text = fbd.SelectedPath;
		}

//---------------------------------------------------------------------------------------

		private void btnNewImporterInstance_Click(object sender, EventArgs e) {
			ImporterStatusControl ctlStat = new ImporterStatusControl();
			if (ctlStat.InitOK == true) {
				ImporterControls.Add(ctlStat);
				flowLayoutPanel1.Controls.Add(ctlStat);
			}
		}

//---------------------------------------------------------------------------------------

		private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e) {
			if ((string)cmbDatabase.SelectedItem != "DEVEL") {
				MessageBox.Show("TODO: Only DEVEL currently supported", "LLConcurrency Test");
				// TODO: Set it back
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// Do some error checking first
			string	RCUserid, RCPassword;
			int		EventID, MapCfgID;

			bool bOK = AreWeGo(out RCUserid, out RCPassword, out EventID, out MapCfgID);
			if (!bOK) {
				return;			// Any messages have been displayed
			}

			RunThreads(EventID, MapCfgID, RCUserid, RCPassword, txtOutputDirectory.Text.Trim());

			// I hate polling, but for now it solves my stack overflow problems
			bool	bDone;
			do {
				bDone = true;
				foreach (ImportThreadInfo info in ThreadInfo) {
					if (info.ThisThread.ThreadState != ThreadState.Stopped) {
						bDone = false;
						break;
					}
					// info.ThisThread.Join();
				}
				Application.DoEvents();
				Thread.Sleep(250);
			} while (bDone == false);

			MessageBox.Show("Done!", "LLConcurrency Test");
		}

//---------------------------------------------------------------------------------------

		private void RunThreads(
						int		EventID, 
						int		MapCfgID, 
						string	RCUserid, 
						string	RCPassword, 
						string	OutDir) {

			ThreadInfo = new List<ImportThreadInfo>();
			foreach (ImporterStatusControl ctl in ImporterControls) {
				ImportThreadInfo info = RunThread(EventID, MapCfgID, RCUserid, RCPassword, OutDir, ctl);
				ThreadInfo.Add(info);
			}
		}

//---------------------------------------------------------------------------------------

		private ImportThreadInfo RunThread(
						int					  EventID, 
						int					  MapCfgID, 
						string				  RCUserid, 
						string				  RCPassword, 
						string				  OutDir, 
						ImporterStatusControl ctl) {
			ImportThreadInfo	info = new ImportThreadInfo();
			info.EventID	= EventID;
			info.MapCfgID	= MapCfgID;
			info.RCUserID	= RCUserid;
			info.RCPassword	= RCPassword;
			info.TerminalID = ctl.TerminalID;
			info.OutputPath = OutDir;
			info.ctl		= ctl;

			info.ThisThread	= new Thread(DoWork);
			info.ThisThread.Start(info);
			return info;
		}

//---------------------------------------------------------------------------------------

		// The mainline for each thread
		private static void DoWork(object objparm) {
			ImportThreadInfo parm = (ImportThreadInfo)objparm;

			int		MinRecsPerImport = parm.ctl.MinRecsPerImport;
			int		MaxRecsPerImport = parm.ctl.MaxRecsPerImport;
			int		SleepIntervalMin = parm.ctl.ImportSleepMin;
			int		SleepIntervalMax = parm.ctl.ImportSleepMax;

			int		CurRecNo = 0;
			int		nLines = parm.ctl.FileLines.Count;	// Number of lines in file
			
			StringBuilder	sb = new StringBuilder();
			parm.ctl.SetProgressBarMinMax(0, nLines);
			parm.ctl.SetProgressBarProgress(0);
			while (CurRecNo < nLines) {
				sb.Length = 0;
				int	nRecs = GetRand(MinRecsPerImport, MaxRecsPerImport);
				if ((CurRecNo + nRecs) > nLines) {
					nRecs = nLines - CurRecNo;
				}
				// Prepend first line each time except the first, if necessary
				if (parm.ctl.IgnoreFirstRecord) {
					sb.Append(parm.ctl.FirstLine);
					sb.Append("\n");
				}
				for (int n = 0; n < nRecs; n++) {
					sb.Append(parm.ctl.FileLines[CurRecNo + n]);
					sb.Append("\n");
				}
#if true
				string	msg = string.Format("Terminal {0}: Importing {1} record(s) starting at {2}",
					parm.ctl.TerminalID, nRecs, CurRecNo);
				Console.WriteLine("{0}", msg);
				parm.ctl.SetStatusMsg(msg);
#endif
				CurRecNo += nRecs;
				DoTheImport(parm, sb.ToString());
				parm.ctl.SetProgressBarProgress(CurRecNo);
				
				int	SleepInterval = GetRand(SleepIntervalMin, SleepIntervalMax);
				Console.WriteLine("About to sleep for " + SleepInterval + " ms");
				// parm.ctl.DoEvents();
				Thread.Sleep(SleepInterval);
			}
		}

//---------------------------------------------------------------------------------------

		private static void DoTheImport(ImportThreadInfo parm, string RawSwipeData) {
			LLWS1 import = new LLWS1();
			import.Timeout = -1;
			ImportStatus	impStatus;
			ImportStatus[]	RecStats;
			string		MapData = parm.MapCfgID.ToString();
			bool		IsReplacementSwipe = false;		// Always
			int			Flags = 0;						// Always
			string		SetupCode = null;
			DateTime	SetupTimestamp = default(DateTime);

			Console.WriteLine("{0} About to import for User={1}, Thread={2}",
				DateTime.Now, parm.ctl.UserID, Thread.CurrentThread.ManagedThreadId);
			impStatus = import.Import2(
							parm.ctl.UserID,	"", 
							parm.RCUserID,		parm.RCPassword, 
							parm.EventID,
							RawSwipeData, 
							MapData,			MapType.MapTypeKey, 
							parm.TerminalID, 
							parm.ctl.IgnoreFirstRecord, 
							parm.ctl.DataIsExpanded,
							DataSource.LeadsLightning,	// Never WiFi
							IsReplacementSwipe,
							Flags,
							"",						// Media Directory
							SetupCode, 
							SetupTimestamp, 
							out RecStats);
			PrintStats(parm, impStatus, RecStats);
		}

//---------------------------------------------------------------------------------------

		private static void PrintStats(ImportThreadInfo parm, ImportStatus impStatus, ImportStatus[] RecStats) {
			lock (ImportThreadInfo.ReportLock) {
				string Filename = Path.Combine(parm.OutputPath, "LL Concurrency Test - " + ImportThreadInfo.ReportLock + ".txt");
				StreamWriter	wtr = new StreamWriter(Filename, true);
				wtr.WriteLine("Import-as-a-whole Debug Info:\n{0}\n", impStatus.DebugInfo);
				StringBuilder sb = new StringBuilder();
				foreach (ImportStatus stat in RecStats) {
					ShowRecStat(sb, stat);
				}
				sb.AppendFormat("Total import error code={0}\n", impStatus.ErrCode);
				ShowRecStat(sb, impStatus);
				string res2 = sb.ToString();
				res2 = res2.Replace("\n", "\r\n");
				wtr.WriteLine(res2);
				wtr.Close();
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowRecStat(StringBuilder sb, ImportStatus impStatus) {
			sb.AppendFormat("Swipe - Error Code={0}, Recno={1}, SwipeID={2}, Dup={3}\n",
				impStatus.ErrCode, impStatus.RecNo, impStatus.SwipeID, impStatus.Duplicate);
			sb.Append(impStatus.ErrMsgs);
			sb.Append(impStatus.DebugInfo);
		}

//---------------------------------------------------------------------------------------

		private static int GetRand(int min, int max) {
			if (min == max)	{
				return min;		
			}
			int		seed = Thread.CurrentThread.ManagedThreadId;
			seed		 = unchecked(seed * Environment.TickCount);	// Try for uniqueness
												// by thread by call
			Random	rand = new Random(seed);
			return min + rand.Next(0, max - min + 1);
		}

//---------------------------------------------------------------------------------------

		private bool AreWeGo(out string RCUserid, out string RCPassword, out int EventID, out int MapCfgID) {
			RCUserid	= null;
			RCPassword	= null;
			EventID		= -1;
			MapCfgID	= -1;

			bool	bOK;

			if (chkTxtOut.Checked || chkHtmlOut.Checked || chkXmlOut.Checked) {
				// Do nothing, we have at least one checked
			} else {
				MessageBox.Show("Must select at least one of .txt, .html or .xml", "LLConcurrency Test");
				return false;
			}

			if (txtEventID.Text.Trim().Length == 0) {
				MessageBox.Show("Must specify an EventID", "LLConcurrency Test");
				return false;
			}

			bOK = int.TryParse(txtEventID.Text, out EventID);
			if (! bOK) {
				MessageBox.Show("Must specify a numeric EventID", "LLConcurrency Test");
				return false;
			}
				
			if (txtMapCfgID.Text.Trim().Length == 0) {
				MessageBox.Show("Must specify a MapCfgID", "LLConcurrency Test");
				return false;
			}

			bOK = int.TryParse(txtMapCfgID.Text, out MapCfgID);
			if (! bOK) {
				MessageBox.Show("Must specify a numeric MapCfgID", "LLConcurrency Test");
				return false;
			}

			RCUserid = txtRCUID.Text.Trim();
			RCPassword = txtRCPassword.Text.Trim();
			if ((RCUserid.Length == 0) || (RCPassword.Length == 0)) {
				MessageBox.Show("Must supply RC UserID and Password", "LLConcurrency Test");
				return false;
			}
			return true;
		}
	}
}