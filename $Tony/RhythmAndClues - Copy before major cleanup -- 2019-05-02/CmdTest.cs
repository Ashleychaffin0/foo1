#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RhythmAndClues {
	internal class CmdTest : ICommand {
		private Interpreter main;
		public List<Tuplet> RhythmPattern;

//---------------------------------------------------------------------------------------

		public CmdTest(Interpreter main) {
			if (main != null) { return; }
			this.main = main;
			RhythmPattern = new List<Tuplet>();

			try {
				TestNormal();
				TestTuplet();
				TestFindRuns();
				// TestMixedNormalAndTuplets();
				// TestNestedTuplet();
			} catch (Exception ex) {
				Debugger.Break();
			}
		}

//---------------------------------------------------------------------------------------

		private void TestNormal() {
			RhythmPattern = new List<Tuplet>();
			string[] tokens = new string[] { "TEST", "4.", "8" };
			DurationDef dur;
			for (int i = 1; i < tokens.Length; i++) {
				bool bOK = Utils.ParseDuration(main, tokens, i, out dur);
				// TODO: Check bOK
				var tup = new Tuplet(dur);
				RhythmPattern.Add(tup);
			}
		}

//---------------------------------------------------------------------------------------

		private void TestTuplet() {				// Tuplets
			// Assume in tuplet mode. As usual, no error checking
			string[] tokens = new string[] { "TEST", "16", "3:2", "8", "16", "32" };
			bool bOK = Utils.ParseDuration(main, tokens, 1, out DurationDef MainDur);
			// bOK = Utils.ParseDuration(main, tokens, 2, out int FullDuration);
			(Utils.TupleRateErrors ErrorCode, int NumberOfNotes, int TargetDuration) 
				= Utils.ParseTupleRate(tokens[2]);
			// List<Tuplet> tuples = new List<Tuplet>();
			var tuplet = new Tuplet(MainDur, NumberOfNotes, TargetDuration);
			for (int i = 3; i < tokens.Length; i++) {
				bOK = Utils.ParseDuration(main, tokens, i, out DurationDef dur);
				var tup = new Tuplet(dur);
				tuplet.Add(tup);
			}
			RhythmPattern.Add(tuplet);
		}

//---------------------------------------------------------------------------------------

		private void TestFindRuns() {
			
		}

//---------------------------------------------------------------------------------------

		private void TestMixedNormalAndTuplets() {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private void TestNestedTuplet() {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		bool ICommand.CheckSyntax(string[] tokens) {
			bool retval = false;
			for (int i = 1; i < tokens.Length; i++) {
				var bOK = Utils.ParseDuration(main, tokens, i, out DurationDef dur);
				if (bOK) {
					RhythmPattern.Add(new Tuplet(dur));
				} else {
					main.SyntaxError("Nonce in Test.CheckSyntax");	// TODO:
					retval = false;
				}
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		bool ICommand.Execute(string[] tokens) {
			throw new System.NotImplementedException();
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class Tuplet {
		public bool			 IsTuplet;

		// Tuplet syntax: [ 4 3:2 8 8 8 ]
		public DurationDef	 BaseNotes;		// of the desired tuplet group (4)
		public int			 ActualNotes;		// 3
		public int			 NormalNotes;	// 2
		// Description
		//	*	The humber of durations in the tuplet (8 8 8) must equal the count (3)
		//	*	
		// TODO: Finish doc'n

		public List<Tuplet>	 Tuplets;

		// public List<Tuplet> SubTup;

//---------------------------------------------------------------------------------------

		// public Tuplet() {
		// 	Tuplets = new List<Tuplet>();
			// SubTup  = new List<Tuplet>();		// This might have length 0
		// }

//---------------------------------------------------------------------------------------

		public Tuplet(DurationDef dur) {
			IsTuplet       = false;
			BaseNotes   = dur;
			ActualNotes = 1;
			NormalNotes = 1;
			Tuplets        = new List<Tuplet>();
		}

//---------------------------------------------------------------------------------------

		public Tuplet(DurationDef baseNotes,
					  int		  numberOfNotes,
					  int		  targetDuration) {
			IsTuplet       = true;
			BaseNotes   = baseNotes;
			ActualNotes = numberOfNotes;
			NormalNotes = targetDuration;
			Tuplets = new List<Tuplet>();
		}

//---------------------------------------------------------------------------------------

		public void Add(Tuplet tup) {		// TODO: Needed?
			Tuplets.Add(tup);
		}
	}
}