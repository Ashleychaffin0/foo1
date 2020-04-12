using System;
using System.Collections.Generic;

namespace LrsColinAssembler {
	class Program {
		static void Main(string[] args) {
			List<string> src = new List<string>() {
				"   L R3,foo\t; Hello",
				"lbl   l R3,foo\t; Hello",
				"lbl2   bad R3,foo\t; Hello"
			};
			foreach (var line in src) {
				Console.WriteLine(line);        // TODO: Add address
				try {
					var srcline = new SourceLine(line);
				} catch (InvalidOpcodeException ex) {
					Console.WriteLine($"*** Invalid opcode found: {ex.Opcode}");
				} catch (UnknownSymbolException ex) {
					Console.WriteLine($"*** Field name not found: {ex.Symbol}");
				}
			}
		}
	}

	public class InvalidOpcodeException : Exception {
		public string Opcode;

//---------------------------------------------------------------------------------------

		public InvalidOpcodeException(string opcode) {
			Opcode = opcode;
		}
	}

	public class UnknownSymbolException : Exception {
		public string Symbol;

//---------------------------------------------------------------------------------------

		public UnknownSymbolException(string symbol) {
			Symbol = symbol;
		}
	}

	/// <summary>
	/// Take an input assembler line and parse it
	/// </summary>
	public class SourceLine {
		static readonly char[] seps = new char[] { ' ', ',' };
		public bool IsEmptyLine = false;
		public string Label = "";
		public byte Opcode;

//---------------------------------------------------------------------------------------

		public SourceLine(string line) {
			// Strip off comments
			int ix = line.IndexOf(';');     // Note: Doesn't support ; inside strings
			if (ix >= 0) line = line.Substring(0, ix);
			// Handle empty lines
			if (line.Length == 0) {
				IsEmptyLine = true;
				return;
			}
			// Canonicalize input
			line = line.Replace('\t', ' ');
			line = line.ToUpper();
			// Simple parsing
			var tokens = line.Split(seps, StringSplitOptions.RemoveEmptyEntries);
			// Handle label
			int ixToken = 0;				// Token scan starts at first token
			if (line[0] != ' ') {			// A label starts in the first column
				Label = tokens[0];
				ixToken = 1;				// Skip over token[0]; 
			}
			string opcode = tokens[ixToken];
			bool bFound = Assembler.OpcodeTable.TryGetValue(opcode, out Opcode entry);
			if (! bFound) {
				throw new InvalidOpcodeException(opcode);
			}
			Opcode = entry.Mnemonic;
		}
	}
}
