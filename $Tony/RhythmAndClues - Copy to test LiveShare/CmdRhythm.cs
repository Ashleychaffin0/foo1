using System;
using System.Collections.Generic;

namespace RhythmAndClues {
	class CmdRhythm : ICommand {
		readonly Interpreter main;

		public List<object> Rhythm { get; private set; }
		List<DurationDef>		Durations;
		List<TupletDurationDef> Tuple;

//---------------------------------------------------------------------------------------

		public CmdRhythm(Interpreter main) {
			this.main = main;
			Rhythm    = new List<object>();
			Durations = new List<DurationDef>();
			Tuple     = new List<TupletDurationDef>();
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			TupletDurationDef	dur;
			bool				bOK;
			bool				retval = true;

			if (tokens.Length == 1) {
				main.SyntaxError("No parameters supplied to RHYTHM");
				return false;
			}

			var CurState = State.InDurations;
			for (int i = 1; i < tokens.Length; ++i) {
				CurState = (CurState, tokens[i]) switch {
					(State.InDurations, "[")	=> State.StartingTuplet,
					(State.InTuplet,    "]")	=> State.EndingTuplet,
					(State.InDurations, "]")	=> State.ErrorNotInTuplet,
					(State.InTuplet,    "[")	=> State.ErrorNestedTuplet,
					(_, _) => CurState			// Stay in current state
				};

				switch (CurState) {
				case State.InDurations:			// Scan token. If valid, add to Durations
					bOK = ParseToken(tokens, i, out dur);
					if (bOK) { Durations.Add((DurationDef)dur); }
					retval &= bOK;
					break;
				case State.InTuplet:			// Scan token. If valid, add to Tuple
					bOK = ParseToken(tokens, i, out dur);
					if (bOK) { Tuple.Add((TupletDurationDef) dur); }
					retval &= bOK;
					break;
				case State.StartingDurations:   // If restarting durations after end of tuple
					if (Tuple.Count > 0) {
						Rhythm.Add(Tuple);
						Tuple.Clear();
					}
					CurState = State.InDurations;
					break;
				case State.StartingTuplet:		// Save previous Duration
					if (Durations.Count > 0) {
						Rhythm.Add(Durations);
						Durations = new List<DurationDef>();
					}
					CurState = State.InTuplet;
					break;
				case State.EndingTuplet:
					if (Tuple.Count == 0) {
						main.SyntaxError("Empty tuple found");
						retval = false;
					} else {
						Rhythm.Add(Tuple);
						Tuple = new List<TupletDurationDef>();
					}
					CurState = State.InDurations;
					break;
				case State.ErrorNotInTuplet:
					main.SyntaxError("Closing ] found with no corresponding [");
					retval = false;
					CurState = State.InDurations;
					break;
				case State.ErrorNestedTuplet:
					main.SyntaxError("[ found in tuplet; nested tuplets not valid");
					retval = false;
					CurState = State.InTuplet;
					break;
				}
			}

			if (CurState == State.InTuplet) {
				main.SyntaxError("Tuplet doesn't end with closing ]");
				retval = false;
			} else {
				if (Durations.Count > 0) { Rhythm.Add(Durations); }
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		private bool ParseToken(string[] tokens, int i, out TupletDurationDef dur) {
			bool bFound = main.DurationIDs.TryGetValue(tokens[i], out dur);
			if (bFound) {
				return true;
			} else {
				main.SyntaxError($"Error: Parameter {i} to {tokens[0]} is invalid: {tokens[i]}");
				return false;
			}
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			throw new System.NotImplementedException();	// TODO:
		}

//---------------------------------------------------------------------------------------

		enum State {
			StartingDurations,
			StartingTuplet,
			EndingTuplet,
			InDurations,
			InTuplet,
			ErrorNotInTuplet,
			ErrorNestedTuplet
		}

#if false   // TODO: Delete once the dust settles

		//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (tokens.Length == 1) {
				main.Msg("Syntax: Rhythm <one or more durations>");
				retval = false;
			}
			retval &= ParsePatternNotes(tokens);
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			main.Msg("Nonce on execution of Rhythm");
			return true;
		}


//---------------------------------------------------------------------------------------

		// Parse parameters for RHYTHM command
		bool ParsePatternNotes(string[] tokens) {
			bool bAllParmsOK = true;
			for (int i = 1; i < tokens.Length; i++) {
				bool bFound = main.DurationIDs.TryGetValue(tokens[i], out DurationDef dur);
				if (bFound) {
					main.RhythmDurations.Add(dur);
				} else {
					main.SyntaxError($"Error: Parameter {i} to {tokens[0]} is invalid: {tokens[i]}");
					bAllParmsOK = false;
				}
			}
			return bAllParmsOK;
		}
#endif
	}
}
