using System;
using System.Collections.Generic;

namespace LrsColinAssembler {
	/// <summary>
	/// Take an input assembler line and parse it
	/// </summary>
	public class SourceLine {
		static readonly char[] seps = new char[] { ' ', ',' };
		public bool		IsEmptyLine = false;
		public string	Label = "";
		public Opcode	OpcodeEntry;

//---------------------------------------------------------------------------------------

		public SourceLine(string line, uint addr) {
			// Strip off comments
			int ix = line.IndexOf(';');     // Note: Doesn't support ; inside strings
			if (ix >= 0) line = line.Substring(0, ix);

			// Handle empty lines
			if (line.Trim().Length == 0) {
				IsEmptyLine = true;
				OpcodeEntry = new Opcode(0x00, 0);	// Need opcode to have length of 0
				return;
			}
			// Canonicalize input
			line = line.Replace('\t', ' ');
			line = line.ToUpper();

			// Simple parsing
			var tokens = line.Split(seps, StringSplitOptions.RemoveEmptyEntries);

			// Handle label
			int ixToken = 0;                // Token scan starts at first token
			if (line[0] != ' ') {           // A label starts in the first column
				Label = tokens[0];
				Assembler.symtab[Label] = addr;	// Ignoring duplicate labels
				ixToken = 1;                // Skip over token[0]
			}

			string opcode = tokens[ixToken];
			bool bFound = Assembler.OpcodeTable.TryGetValue(opcode, out OpcodeEntry);
			if (!bFound) {
				throw new InvalidOpcodeException(opcode);
			}
		}
	}
}
