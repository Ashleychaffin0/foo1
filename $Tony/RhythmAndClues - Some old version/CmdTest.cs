using System.Collections.Generic;
using System.Diagnostics;

namespace RhythmAndClues {
	internal class CmdTest : ICommand {
		private Interpreter main;
		public List<object> Tuples;		// TODO: Make <Tuplet>

//---------------------------------------------------------------------------------------

		public CmdTest(Interpreter main) {
			this.main = main;
			Tuples = new List<object>();	// TODO: Make Tuplet
		}

//---------------------------------------------------------------------------------------

		bool ICommand.CheckSyntax(string[] tokens) {
			bool retval = false;
			for (int i = 1; i < tokens.Length; i++) {
				var bOK = Utils.ParseToken(main, tokens, i, out DurationDef dur);
				if (bOK) {
					Tuples.Add(dur);
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
		public bool			IsTuplet;
		public bool			IsDotted;		// TODO: Needed?
		public DurationDef	dur;

//---------------------------------------------------------------------------------------

		public Tuplet(DurationDef dur) {
			IsTuplet = false;
			IsDotted = dur.IsDotted;        // TODO: Needed?
			this.dur = dur;
		}

//---------------------------------------------------------------------------------------

		public Tuplet(DurationDur dur, ) {    // Assume of the form 8, 3:2, 1 2 4
			if (tokens.Length < 3) {
				main.sy
			}
		}
	}
}