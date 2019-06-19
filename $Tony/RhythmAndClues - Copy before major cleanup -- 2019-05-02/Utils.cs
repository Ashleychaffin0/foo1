using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class Utils {

//---------------------------------------------------------------------------------------

		public static bool ParseDuration(Interpreter main, string[] tokens, int i, out DurationDef dur) {
			bool bFound = main.DurationIDs.TryGetValue(tokens[i], out dur);
			if (bFound) {
				return true;
			} else {
				main.SyntaxError($"Error: Parameter {i} to {tokens[0]} is invalid: {tokens[i]}");
				return false;
			}
		}

//---------------------------------------------------------------------------------------

		internal static (TupleRateErrors ErrorCode, int NumberOfNotes, int TargetDuration) ParseTupleRate(string s) {
			int NumberOfNotes         = -1;
			int TargetDuration        = -1;
			TupleRateErrors ErrorCode = TupleRateErrors.OK;

			if (s.Length < 3) {
				ErrorCode |= TupleRateErrors.Too_Short; 
			}

			int ix = s.IndexOf(":");
			if (ix < 0) {
				ErrorCode = TupleRateErrors.No_Colon_Found;
			}

			if ((ix == 0) || (ix == s.Length)) {
				ErrorCode |= TupleRateErrors.Misplaced_Colon;
			}

			var nums = s.Split(':');
			if (nums.Length != 2) {
				ErrorCode = TupleRateErrors.Too_Many_Numbers;
			}

			bool bOK = int.TryParse(nums[0], out NumberOfNotes);
			if (! (bOK && NumberOfNotes > 0)) {
				ErrorCode = TupleRateErrors.Invalid_Numeric;
			}

			bOK = int.TryParse(nums[1], out TargetDuration);
			if (! (bOK && TargetDuration > 0)) {
				ErrorCode = TupleRateErrors.Invalid_Numeric;
			}

			if (NumberOfNotes == TargetDuration) {
				ErrorCode = TupleRateErrors.Both_Values_The_Same;
			}

			return (ErrorCode, NumberOfNotes, TargetDuration);
		}

//---------------------------------------------------------------------------------------
		[Flags]
		public enum TupleRateErrors {
			OK                   = 0,
			Too_Short            = 1 << 0,
			No_Colon_Found       = 1 << 1,
			Misplaced_Colon      = 1 << 2,
			Too_Many_Numbers     = 1 << 3,
			Invalid_Numeric      = 1 << 4,
			Both_Values_The_Same = 1 << 5,
		}
	}

}
