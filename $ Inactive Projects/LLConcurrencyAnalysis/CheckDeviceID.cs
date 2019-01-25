using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace LLConcurrencyAnalysis {

	/// <summary>
	/// A class to empirically check that the specified accounts
	/// in the specified Event, had their TerminalID match a Basic Data
	/// field called "Device ID".
	/// </summary>
	class CheckDeviceID {
		int			EventID;
		List<int>	AcctIDs;

//---------------------------------------------------------------------------------------

		public CheckDeviceID(int EventID, List<int> AcctIDs) {
			this.EventID = EventID;
			this.AcctIDs = AcctIDs;
		}

//---------------------------------------------------------------------------------------

		public void Run(ListBox	lbMsgs) {
			Stopwatch sw = new Stopwatch();
			sw.Start();

			LLDevelDataContext dc = new LLDevelDataContext();

			foreach (var AcctID in AcctIDs) {
				ProcessAcct(dc, AcctID, lbMsgs);
			}
			sw.Stop();
		}

//---------------------------------------------------------------------------------------

		private void ProcessAcct(LLDevelDataContext dc, int AcctID, ListBox lbMsgs) {
			lbMsgs.Items.Add("");
			lbMsgs.Items.Add(string.Format("Processing AcctID {0}", AcctID));
			Application.DoEvents();

			Stopwatch	sw = new Stopwatch();
			sw.Start();
			string	msg;
			int		nRecs = 0;
			int		nMismatches = 0;
			// Get all the swipes for this EventID/AcctID combination
			var q1 = from s in dc.tblSwipes
					 join t in dc.tblTerminals
						on s.TerminalID equals t.ID
					 where s.AcctID == AcctID && s.EventID == EventID
					 select new { s.SwipeID, s.PersonEventID, t.TerminalSerial };
			Console.WriteLine("q1={0}", q1);
			// For each swipe, check that the terminal IDs match in tblTerminal
			// and in the Device ID field in the Tall Table
			foreach (var Swipe in q1) {
				++nRecs;
				var q2 = from pe in dc.tblSwipesTexts
						 where pe.PersonEventID == Swipe.PersonEventID
							&& pe.FieldName == "Device ID"
							&& pe.FieldText != Swipe.TerminalSerial
						select new {Swipe, pe};
				if (nRecs == 1) {
					Console.WriteLine("q2={0}", q2);
				}
				foreach (var Mismatch in q2) {
					++nMismatches;
					msg = string.Format("CheckDeviceID mismatch - SwipeID={0}"
						+ ", Swipe TerminalID={1}, Tall Table Device ID={2}",
						Mismatch.Swipe.SwipeID, Mismatch.Swipe.TerminalSerial, Mismatch.pe.FieldText);
					Console.WriteLine("{0}", msg);
					lbMsgs.Items.Add(msg);
				}
			}
			sw.Stop();
			msg = string.Format("\t{0} record(s), {1} mismatch(es). Elapsed = {2}", nRecs, nMismatches, sw.Elapsed);
			Console.WriteLine("{0}", msg);
			lbMsgs.Items.Add(msg);
			Application.DoEvents();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// A class to empirically check that the specified accounts
	/// in the specified Event, had their TerminalID match a Basic Data
	/// field called "Device ID".
	/// </summary>
	class CheckDeviceID2 {
		int EventID;
		List<int> AcctIDs;

//---------------------------------------------------------------------------------------

		public CheckDeviceID2(int EventID, List<int> AcctIDs) {
			this.EventID = EventID;
			this.AcctIDs = AcctIDs;
		}

//---------------------------------------------------------------------------------------

		public void Run(ListBox lbMsgs) {
			Stopwatch sw = new Stopwatch();
			sw.Start();

			LLDevelDataContext dc = new LLDevelDataContext();

			foreach (var AcctID in AcctIDs) {
				ProcessAcct(dc, AcctID, lbMsgs);
			}
			sw.Stop();
		}

//---------------------------------------------------------------------------------------

		private void ProcessAcct(LLDevelDataContext dc, int AcctID, ListBox lbMsgs) {
			lbMsgs.Items.Add("");
			lbMsgs.Items.Add(string.Format("CheckDeviceID2: Processing AcctID {0}", AcctID));
			Application.DoEvents();
			
			Stopwatch	watch = new Stopwatch();
			watch.Start();
			string msg;
			int nMismatches = 0;
			// Get all the swipes for this EventID/AcctID combination
			var q1 = from s in dc.tblSwipes
					 join t in dc.tblTerminals
						on s.TerminalID equals t.ID
					 where s.AcctID == AcctID && s.EventID == EventID
					 select new { s, t.TerminalSerial };
			Console.WriteLine("q1={0}", q1);

			// For each swipe, check that the terminal IDs match in tblTerminal
			// and in the Device ID field in the Tall Table
			// foreach (var Swipe in q1) {
			var q2 = from sw in q1
						join pe in dc.tblSwipesTexts
							on sw.s.PersonEventID equals pe.PersonEventID
					where pe.FieldName == "Device ID" 
						&& pe.FieldText != sw.TerminalSerial
					select new { sw, pe };
			Console.WriteLine("q2={0}", q2);
			foreach (var Mismatch in q2) {
				++nMismatches; 
				msg = string.Format("CheckDeviceID mismatch - SwipeID={0}, SwipeTime={1}"
					+ ", Swipe TerminalID={2}, Tall Table Device ID={3}",
					Mismatch.sw.s.SwipeID, Mismatch.sw.s.SwipeDate, Mismatch.sw.TerminalSerial, Mismatch.pe.FieldText);
				Console.WriteLine("{0}", msg);
				lbMsgs.Items.Add(msg);
			}
			watch.Stop();
			msg = string.Format("\t{0} mismatch(es). Elapsed = {1}", nMismatches, watch.Elapsed);
			Console.WriteLine("{0}", msg);
			lbMsgs.Items.Add(msg);
			Application.DoEvents();
		}
	}
}
