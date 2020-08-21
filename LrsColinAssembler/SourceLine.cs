using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace LrsColinAssembler {
	/// <summary>
	/// Take an input assembler line, parses it and does a bit of error checking
	/// </summary>
	class SourceLine {
		static readonly char[] seps = new char[] { ' ', ',' };
		public OpcodeDef					OpcodeEntry;
		public ParsedLine					ParsedSource;
		readonly Dictionary<string, ushort> Symtab;

//---------------------------------------------------------------------------------------

		// TODO: Comment
		public SourceLine(Dictionary<string, ushort> symtab, string line, ushort addr) {
			Symtab = symtab;
			ParsedSource = new ParsedLine(addr, line);

			// Strip off comments
			int ix = line.IndexOf(';');     // Note: Doesn't support ; inside strings
			if (ix >= 0) line = line.Substring(0, ix);

			// Handle empty lines
			if (line.Trim().Length == 0) {
				ParsedSource.Length = 0;
				return;
			}
			// Canonicalize input
			line = line.Replace('\t', ' ');

			// Simple parsing
			var tokens = line.Split(seps, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

			// Handle label
			HandleLabel(line, addr, tokens);

			// Handle opcode
			HandleOpcode(tokens, out ParsedSource.Mnemonic);
			if (ParsedSource.Mnemonic.Length == 0) { return; }
			ParsedSource.Parms = new List<string>(tokens);

			// Special processing for "DefineString" pseudo-op
			if (ParsedSource.Mnemonic != "DS") {
				// Check for the right number of arguments
				int NumArgs = CheckArgumentCount(tokens);
				if (NumArgs <= 0) { return; }
			} else {
				ParsedSource.Length = 1;	// Just so we won't be considered a comment
				// Can have " around the string, but will ignore multiple blanks
				// Map \n to newline, \z to 0x00, \b to blank, \c to comma
				string s = string.Join(' ', tokens)
					.Replace("\"", "")		// Get rid of surrounding quotes
					.Replace(@"\n", Environment.NewLine)
					.Replace(@"\z", "")		// For empty strings
					.Replace(@"\s", ";")	// For semicolons
					.Replace(@"\t", "\t")	// For tab character
					.Replace(@"\b", " ")	// Allow multiple blanks
					.Replace(@"\c", ",")	// We strip out commas above. Put them back in
					+ '\0';
				ParsedSource.DsText = s;
				ParsedSource.Length = (ushort)s.Length;
				ParsedSource.Parms = tokens;
				return;
			}
		}

//---------------------------------------------------------------------------------------

		// TODO: Comment
		private void HandleLabel(string line, ushort addr, List<string> tokens) {
			if (line[0] != ' ') {               // A label starts in the first column
				string Label = tokens[0];
				tokens.RemoveAt(0);
				if (Symtab.ContainsKey(Label)) {
					ParsedSource.ErrorMessage = $"*** Duplicate label found: {Label} -- ignored";
				} else {
					Symtab[Label] = addr;
				}
			}
		}

//---------------------------------------------------------------------------------------

		// TODO: Comment
		private void HandleOpcode(List<string> tokens, out string Mnemonic) {
			Mnemonic = tokens[0].ToUpper();
			bool bFound = Assembler.OpcodeTable.TryGetValue(Mnemonic, out OpcodeEntry);
			if (!bFound) {
				ParsedSource.ErrorMessage = $"*** Invalid opcode found: {Mnemonic}";
				ParsedSource.Opcode = 0x00;
				ParsedSource.Length = 0;
				Mnemonic = "";
				return;
			}
			ParsedSource.OpDef  = OpcodeEntry;
			ParsedSource.Opcode = OpcodeEntry.Op;
			ParsedSource.Length = OpcodeEntry.Length;
			tokens.RemoveAt(0);		// Don't need opcode any more
		}

//---------------------------------------------------------------------------------------

		// TODO: Comment
		private int CheckArgumentCount(List<string> tokens) {
			int NumArgs = tokens.Count;
			if (OpcodeEntry.NumberOfArgs != NumArgs) {
				ParsedSource.ErrorMessage = $"*** Invalid number of arguments. Expecting {OpcodeEntry.NumberOfArgs}, found {NumArgs}";
				return -1;
			}
			return NumArgs;
		}
	}
}
