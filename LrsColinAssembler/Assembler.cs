using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

// Notes in general
// Note: Use if necessary to convert to Big Endian
//		 var xxx = System.Net.IPAddress.HostToNetworkOrder(address);
// Note: Slightly obscure, but sometimes useful method
//		 Array.ConstrainedCopy(addrbytes, 0, Ram, CodeAddress, addrbytes.Length);

namespace LrsColinAssembler {
	public class Assembler {
		// Note: A real symbol table would contain more than just the
		//		 address of the symbol, but could contain things like
		//		 the data type if this were the label of a field
		//		 definition.
		public static Dictionary<string, ushort> Symtab;

		// https://en.wikibooks.org/wiki/360_Assembly/360_Instructions
		public static Dictionary<string, OpcodeDef> OpcodeTable;

		// To distinguish BNE from BL from ...
		public static Dictionary<string, byte> BranchMasks;

		// The output from multiple calls to SourceLine
		List<ParsedLine> ParsedLines;

		// For parsing an address with an optional index register
		static readonly Regex reAddr = new Regex(@"(?<addr>\w+)(\[R?(?<ixreg>\d+)\])?",
	RegexOptions.IgnoreCase | RegexOptions.Compiled);       // Regular Expression for address

		const int RAMSIZE = 1024 * 10;			// 10K should be enough for our toy pgms
		byte[] Ram;								// Our actual Ram array
		ushort CodeAddress = 0;

		const int MAX_INSTRUCTION_LENGTH = 8;	// Just for padding the machine language
												// from the source listing


//---------------------------------------------------------------------------------------

		private void SetupTables() {
			OpcodeTable = new Dictionary<string, OpcodeDef> {
				// Load/Store opcodes
				["L"]   = new OpcodeDef(0x58, 4, 2, Handle_RX),		// Load
				["LA"]  = new OpcodeDef(0x41, 4, 2, Handle_RX),		// Load Address
				["LR"]  = new OpcodeDef(0x18, 2, 2, Handle_RR),     // Load Register
				["LTR"] = new OpcodeDef(0x12, 2, 2, Handle_RR),     // Load and Test Register
				["ST"]  = new OpcodeDef(0x50, 4, 2, Handle_RX),     // Store
				["IC"]  = new OpcodeDef(0x43, 4, 2, Handle_RX),     // Insert Character
				["STC"] = new OpcodeDef(0x42, 4, 2, Handle_RX),     // Store Character

				// Aritmetic opcodes
				["ADD"] = new OpcodeDef(0x5A, 4, 2, Handle_RX),     // Add
				["AR"]  = new OpcodeDef(0x1A, 2, 2, Handle_RR),     // Add Register
				["SUB"] = new OpcodeDef(0x5B, 4, 2, Handle_RX),     // Subtract
				["SR"]  = new OpcodeDef(0x1B, 2, 2, Handle_RR),     // Subtract Register
				["MUL"] = new OpcodeDef(0x5C, 4, 2, Handle_RX),     // Multiply
				["DIV"] = new OpcodeDef(0x5D, 4, 2, Handle_RX),     // Divide

				// Subroutine opcodes
				["CALL"] = new OpcodeDef(0x45, 4, 2, Handle_RX),    // Call Subroutine
				["RET"]  = new OpcodeDef(0x07, 2, 1, Handle_OneReg),// Return from Subroutine

				// Compare and Branch opcodes
				["CMP"] = new OpcodeDef(0x59, 4, 2, Handle_RX),     // Compare reg w/mem
				["BL"]  = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch if a < b
				["BLE"] = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch if a <= b
				["BE"]  = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch if a == b
				["BZ"]  = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch if a == b
				["BH"]  = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch if a > b
				["BHE"] = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch if a >= b
				["BNE"] = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch if a != b
				["BNZ"] = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch if a != b
				["B"]   = new OpcodeDef(0x47, 4, 1, Handle_BR),     // Branch unconditionally

				// Quasi-macros
				["$TRACEON"]  = new OpcodeDef(0xFA, 1, 0, Handle_NoArgs),	// Start tracing
				["$TRACEOFF"] = new OpcodeDef(0xFB, 1, 0, Handle_NoArgs),	// End tracing
				["$PREG"]     = new OpcodeDef(0xFC, 2, 1, Handle_OneReg),	// Print contents of register
				["$PNUM"]     = new OpcodeDef(0xFD, 4, 1, Handle_OneAddr),	// Print numeric field
				["$PSTRING"]  = new OpcodeDef(0xFE, 4, 1, Handle_OneAddr),  // Print string
				["$STOP"]     = new OpcodeDef(0xFF, 1, 0, Handle_NoArgs),	// Return to "OS"

				// Declarations
				["DI"] = new OpcodeDef(0x00, 2, 1, Handle_DI),
				["DS"] = new OpcodeDef(0x00, 0, 1, Handle_DS)
			};

			// Map a branch opcode to its bitmask
			BranchMasks = new Dictionary<string, byte> {
				["BH"]  = 0x02,			// a > b
				["BL"]  = 0x04,			// a < b
				["BNE"] = 0x07,			// a != b
				["BNZ"] = 0x07,			// a != b
				["BE"]  = 0x08,			// a == b
				["BZ"]  = 0x08,			// a == b, or calc (e.g. ADD) gives 0
				["BHE"] = 0x0B,			// a >= b
				["BLE"] = 0x0D,			// a <= b
				["B"]   = 0x0F			// Branch always
			};
		}
//---------------------------------------------------------------------------------------

		internal int Assemble(string filename) {
			SetupTables();
			using (var inFile = new StreamReader(filename)) {
				Symtab = new Dictionary<string, ushort>();
				ParsedLines = new List<ParsedLine>();

				int numPass1Errors = Pass1_ParseLineAndBuildSymbolTable(inFile);
				if (numPass1Errors != 0) {
					Console.WriteLine($"Pass 2 canceled; Pass 1 errors: {numPass1Errors}");
					return 1;
				}
				int numPass2Erros = Pass2_GenerateCode();
				if (numPass2Erros > 0) { return 1; }
			}
			return 0;			// Use try/catch?
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Parse an input line and create the symbol table. Also handle some errors.
		/// </summary>
		/// <param name="inFile">The StreamReader for the input file</param>
		/// <returns>Number of Pass 1 errors or 0 if OK</returns>
		private int Pass1_ParseLineAndBuildSymbolTable(StreamReader inFile) {
			ushort addr = 0;
			int numErrs = 0;
			string line;
			while ((line = inFile.ReadLine()) != null) {
				var srcline = new SourceLine(line, addr);
				ParsedLines.Add(srcline.ParsedSource);
				if (srcline.ParsedSource.ErrorMessage.Length > 0) { ++numErrs; }
				addr += srcline.ParsedSource.Length;
			}
			if (numErrs > 0) {
				// Just list the program with any error messages
				foreach (var item in ParsedLines) {
					Console.WriteLine(item.Source);
					if (item.ErrorMessage.Length > 0) {
						Console.WriteLine(item.ErrorMessage);
					}
				}
			}
			return numErrs;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Now that the symbol table has been built, actually do the assembly and
		/// generate object code.
		/// </summary>
		/// <returns></returns>
		private int Pass2_GenerateCode() {
			Ram = new byte[RAMSIZE];
			int rc = 0;         // The eternal optimist
			foreach (ParsedLine line in ParsedLines) {
				if (line.Length == 0) {
					ProcessComment(line);
					continue;
				}
				Console.Write($"{line.SourceAddress:X4} ");
				line.OpDef.Process(line);
				if (line.Mnemonic != "DS") { Console.WriteLine(line.Source); }
			}

			DumpMemory();			// Debug
			return rc;
		}

//---------------------------------------------------------------------------------------

		private void DumpMemory() {
			Console.WriteLine("Memory dump...");
			Console.Write("        ");
			string pad = "".PadRight(3 * 4 - 2);
			for (int n = 4; n <= 12; n += 4) {
				Console.Write($"{pad} {n:X1}");
			}
			Console.WriteLine();
			Console.Write("0000: ");
			for (int i = 0; i < CodeAddress; i++) {
				Console.Write($"{Ram[i]:X2} ");
				if (i % 16 == 15) {
					Console.WriteLine();
					Console.Write($"{i + 1:X4}: ");
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessComment(ParsedLine line) {
			const int AddrWidth = 4;	// TODO: Make global
			Pad(AddrWidth);
			Pad(0);
			Console.WriteLine(line.Source);
		}

//---------------------------------------------------------------------------------------

		byte DecodeReg(string r) {
			if (r.StartsWith("R")) { r = r.Substring(1); }
			return byte.Parse(r);
		}

//---------------------------------------------------------------------------------------

		void Handle_RR(ParsedLine line) {
			const int RrWidth = 4;
			Emit(line.OpDef.Op);
			byte reg1 = DecodeReg(line.Parms[0]);
			byte reg2 = DecodeReg(line.Parms[1]);
			Emit(GlueNibbles(reg1, reg2));
			Pad(RrWidth);
		}

//---------------------------------------------------------------------------------------

		private void Handle_RX(ParsedLine line) {
			const int RxWidth = 8;
			Emit(line.OpDef.Op);
			byte reg1 = DecodeReg(line.Parms[0]);
			(bool OK, byte ixReg, ushort address) = ParseAddress(line.Parms[1]);
			if (!OK) { return;  }
			byte reg = GlueNibbles(reg1, ixReg);
			Emit(reg);
			Emit(address);
			Pad(RxWidth);
		}

//---------------------------------------------------------------------------------------

		byte GlueNibbles(byte left, byte right) => (byte)((left << 4) | right);

//---------------------------------------------------------------------------------------

		private (bool OK, byte ixReg, ushort address) ParseAddress(string parm) {
			var re       = reAddr.Match(parm);
			string label = re.Groups["addr"].Value;
			// An address may be a symbol or a numeric value, such as LA  R3,0[R1]
			bool bIsNum = ushort.TryParse(label, out ushort addr);
			if (!bIsNum) {
				bool bOK = Symtab.TryGetValue(label, out addr);
				if (!bOK) {
					Console.WriteLine($"*** Error: Label {label} not found");
					return (false, 0, 0);
				}
			}
			string ix = re.Groups["ixreg"].Value;
			byte reg  = 0;
			if (!(string.IsNullOrEmpty(ix))) { reg = Convert.ToByte(ix); }
			return (true, reg, addr);
		}

//---------------------------------------------------------------------------------------

		private void Handle_BR(ParsedLine line) {
			const int BrWidth = 8;
			Emit(line.OpDef.Op);
			(bool OK, byte ixReg, ushort address) = ParseAddress(line.Parms[0]);
			if (!OK) { return; }
			byte cond = BranchMasks[line.Mnemonic];
			byte reg = GlueNibbles(cond, ixReg);
			Emit(reg);
			Emit(address);
			Pad(BrWidth);
		}

//---------------------------------------------------------------------------------------

		private void Handle_DI(ParsedLine line) {
			const int DiWidth = 4;
			ushort n = Convert.ToUInt16(line.Parms[0]);
			byte[] bytes = BitConverter.GetBytes(n);
			Emit(bytes);
			Pad(DiWidth);
		}

//---------------------------------------------------------------------------------------

		private void Handle_DS(ParsedLine line) {
			string s = line.DsText;
			bool showsource = true;
			int maxwidth = MAX_INSTRUCTION_LENGTH / 2;
			for (int i = 0; i < s.Length; i++) {
				Emit((byte)s[i]);
				if (i % maxwidth == maxwidth - 1) {
					if (showsource) {
						Console.WriteLine(" " + line.Source);
						showsource = false;
					} else {
						Console.WriteLine();
					}
					// TODO: Use Pad(AddrWidth) in next line
					Console.Write("     ");		// Skip over address field
				}
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		private void Handle_NoArgs(ParsedLine line) {
			const int NoArgWidth = 2;
			Emit(line.OpDef.Op);
			Pad(NoArgWidth);
		}

//---------------------------------------------------------------------------------------

		private void Handle_OneReg(ParsedLine line) {
			const int NoArgWidth = 2;
			Emit(line.OpDef.Op);
			Emit(DecodeReg(line.Parms[0]));
			Pad(NoArgWidth);
		}

//---------------------------------------------------------------------------------------

		private void Handle_OneAddr(ParsedLine line) {
			const int OneArgWidth = 8;
			string parm = line.Parms[0];
			Emit(line.OpDef.Op);
			(bool OK, byte ixReg, ushort address) = ParseAddress(line.Parms[0]);
			Emit(ixReg);
			Emit(address);
#if false
			bool bOK = ushort.TryParse(parm, out ushort num);	// TODO: Support R0
			if (bOK) {
				Emit(num);
			} else {
				Emit(Symtab[parm]);
			}
#endif
			Pad(OneArgWidth);
		}

//---------------------------------------------------------------------------------------

		void Pad(int width) => Console.Write("".PadRight(MAX_INSTRUCTION_LENGTH - width + 1));

//---------------------------------------------------------------------------------------

		void Emit(byte b, bool print = true) {
			Ram[CodeAddress++] = b;
			if (print) { Console.Write($"{b:X2}"); }
		}

//---------------------------------------------------------------------------------------

		void Emit(byte[] bs, bool print = true) {
			// Convert to big endian
			for (int i = bs.Length - 1; i >= 0; i--) {
				Emit(bs[i], print);
			}
		}

//---------------------------------------------------------------------------------------

		void Emit(ushort num, bool print = true) {
			byte[] bytes = BitConverter.GetBytes(num);
			Emit(bytes, print);
		}

//---------------------------------------------------------------------------------------

		internal void Run() {
			var interp = new Interpreter(Ram);
			interp.Run();
		}
	}
}
