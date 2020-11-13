using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

// Notes about things potentially useful in other apps. But not here.
// Note: Use if necessary to convert to Big Endian
//		 var xxx = System.Net.IPAddress.HostToNetworkOrder(address);
// Note: Slightly obscure, but sometimes useful method
//		 Array.ConstrainedCopy(addrbytes, 0, Ram, CodeAddress, addrbytes.Length);

namespace LrsColinAssembler {
	public class Assembler {
		/// <summary>
		/// A real symbol table would contain more than just the
		/// address of the symbol, but could contain things like
		/// the data type if this were the label of a field definition.
		/// Note that the ushort is the address of the symbol in memory.
		/// </summary>
		public Dictionary<string, ushort> Symtab;

		// https://en.wikibooks.org/wiki/360_Assembly/360_Instructions
		public static Dictionary<string, OpcodeDef> OpcodeTable;

		// To distinguish BNE from BL from ...
		public static Dictionary<string, byte> BranchMasks;

		// The output from multiple calls to SourceLine
		List<ParsedLine> ParsedLines;
		public static Dictionary<ushort, ParsedLine> MapAddressToSource;

		// For parsing an address with an optional index register
		static readonly Regex reAddr = new Regex(@"(?<addr>\w+)(\[R?(?<ixreg>\d+)\])?",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);	// Regular Expression for address

		readonly byte[] Ram;					// Our actual Ram array
		ushort CodeAddress = 0;                 // Where we start executing
		const int AddrWidth = 4;				// Addresses are 16 bits (4 bytes)

		const int MAX_INSTRUCTION_LENGTH = 8;	// Just for padding the machine language
												// from the source listing


//---------------------------------------------------------------------------------------

		private void SetupTables() {
			// Map the symbolic opcodes to a class instance with the
			// info we need to do the assembly. For example, the first
			// parameter is the hex opcode for the symbolic opcode.
			// So a Load (L) opcode shows up in the machine language
			// as byte 0x58, and the L instruction is 4 bytes long,
			// takes 2 parameters, and uses the Handle_RX routine to
			// process the source code line.
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
				["CR"]  = new OpcodeDef(0x19, 2, 2, Handle_RR),     // Compare reg w/reg
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
				["$TRACEON"]  = new OpcodeDef(0xF9, 1, 0, Handle_NoArgs),	// Start tracing
				["$TRACEOFF"] = new OpcodeDef(0xFA, 1, 0, Handle_NoArgs),	// End tracing
				["$PREG"]     = new OpcodeDef(0xFB, 2, 1, Handle_OneReg),	// Print contents of register
				["$PREGHEX"]  = new OpcodeDef(0xFC, 2, 1, Handle_OneReg),	// Print contents of register in hex
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

		public Assembler(byte[] ram) => Ram = ram;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The assembler
		/// </summary>
		/// <param name="filename">The name of the file to assemble</param>
		/// <returns>0 is all is well, 1 if errors found</returns>
		internal int Assemble(string filename) {
			SetupTables();
			using (var inFile = new StreamReader(filename)) {
				Symtab             = new Dictionary<string, ushort>();
				ParsedLines        = new List<ParsedLine>();
				MapAddressToSource = new Dictionary<ushort, ParsedLine>();

				// The initial pass through the program scans for labels
				// and does a bit of syntax checking.
				int numPass1Errors = Pass1_ParseLineAndBuildSymbolTable(inFile);
				if (numPass1Errors != 0) {
					Console.WriteLine($"Pass 2 canceled; Pass 1 errors: {numPass1Errors}");
					return 1;
				}

				// If all went well, Pass 2 generates code. 
				int numPass2Erros = Pass2_GenerateCode();
				if (numPass2Erros > 0) { return 1; }
			}
			WriteBinFile(filename);
			return 0;	// No errors, so we can run this puppy
		}

//---------------------------------------------------------------------------------------

		private void WriteBinFile(string filename) {
			string fn = Path.GetFileNameWithoutExtension(filename) + ".bin";
			using var wtr = new BinaryWriter(File.OpenWrite(fn));
			wtr.Write(Ram, 0, CodeAddress);
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
			while ((line = inFile.ReadLine()) != null) {			// Read until EOF
				var srcline = new SourceLine(Symtab, line, addr);	// Parses the line
				if (srcline.ParsedSource.Length > 0) {
					MapAddressToSource[addr] = srcline.ParsedSource;
				}
				ParsedLines.Add(srcline.ParsedSource);		// Accumulate parsed lines
				if (srcline.ParsedSource.ErrorMessage.Length > 0) { ++numErrs; }
				addr += srcline.ParsedSource.Length;		// Address of next line
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
		/// <returns>0 if OK, 1 </returns>
		private int Pass2_GenerateCode() {
			int rc = 0;					// The eternal optimist
			// Note: This routine never returns anything but 0. However
			//		 if we ever enhance this processing to include some
			//		 error detections, maybe we'll need something like this.
			foreach (ParsedLine line in ParsedLines) {
				if (line.Length == 0) {
					ProcessComment(line);
					continue;
				}
				Console.Write($"{line.SourceAddress:X4} ");
				if (!line.OpDef.Process(line)) { rc = 1; }
				if (line.Mnemonic != "DS") { Console.WriteLine(line.Source); }
			}

			DumpMemory();				// Debug
			return rc;
		}

//---------------------------------------------------------------------------------------

		private void DumpMemory() {
			// Just a debug routine
			Console.WriteLine();
			Console.WriteLine("Memory dump...");
			Console.Write("       ");
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

		/// <summary>
		/// Comments in the source show up in the source listing as having no address
		/// and no data. This does that.
		/// </summary>
		/// <param name="line">The ParsedLine for the comment line</param>
		private void ProcessComment(ParsedLine line) {
			Pad(AddrWidth);
			Pad(0);
			Console.WriteLine(line.Source);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Simplified parsing (with essentially no error checking) for recognizing
		/// a register number.
		/// </summary>
		/// <param name="r">A string with a numeric value from 0-15, optionally
		/// preceded by the letter R (e.g. either 5 or R5, but not r5)
		/// </param>
		/// <returns>The numeric value of the string</returns>
		byte DecodeReg(string r) {
			if (r.StartsWith("R")) { r = r.Substring(1); }
			return byte.Parse(r);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Handles instructions with one byte for the opcode followed by one byte
		/// for a pair of registers, each encoded as 4 bits. For example, to add the
		/// contents of R5 to R9, we'd use the AR (Add Register) instruction which
		/// would look like 1A95.
		/// </summary>
		/// <param name="line">The ParsedLine for this line</param>
		bool Handle_RR(ParsedLine line) {
			const int RrWidth = 4;
			Emit(line.OpDef.Op);
			byte reg1 = DecodeReg(line.Parms[0]);
			byte reg2 = DecodeReg(line.Parms[1]);
			Emit(GlueNybbles(reg1, reg2));
			Pad(RrWidth);
			return true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Handle instructions that have 2 parameters, the first being a register and
		/// and the second being an address in the format of a symbol (e.g. DATA) or
		/// a number, optionally followed by "[register]". For example, L R5,DATA,
		/// or L R5,Data[R7], or L R5,0[R1].
		/// </summary>
		/// <param name="line">The ParsedLine for this line</param>
		private bool Handle_RX(ParsedLine line) {
			const int RxWidth = 8;
			Emit(line.OpDef.Op);
			byte reg1 = DecodeReg(line.Parms[0]);
			(bool OK, byte ixReg, ushort address) = ParseAddress(line.Parms[1]);
			if (!OK) { return false; }
			byte reg = GlueNybbles(reg1, ixReg);
			Emit(reg);
			Emit(address);
			Pad(RxWidth);
			return true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Glue two 4-bit nybbles into a single byte
		/// </summary>
		/// <param name="left">First nybble</param>
		/// <param name="right">Second nybble</param>
		/// <returns>The glued together single byte</returns>
		byte GlueNybbles(byte left, byte right) => (byte)((left << 4) | right);

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Parses an address
		/// </summary>
		/// <param name="parm">Of the form DATA or NUMBER or either of these optionally
		/// followed by [register]. For example, DATA or 0 or DATA[R5] or 0[R7]
		/// </param>
		/// <returns>A tuple, (OK, ixReg, address) where OK is true or false depending
		/// on whether there was an error (e.g. the address wasn't found); ixReg is the
		/// index register specified (defaults to 0 if not present); address is the
		/// address (e.g. of DATA, or a numeric constant)
		/// </returns>
		private (bool OK, byte ixReg, ushort address) ParseAddress(string parm) {
			var re       = reAddr.Match(parm);	// Parse the parm via our Regex
			// No error checking. We assume the Regex works.
			string label = re.Groups["addr"].Value;
			// An address may be a symbol or a numeric value, such as 
			// LA  R3,DATA or LA  R3,0[R1]
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

		/// <summary>
		/// Handle Branch instructions. Note that all branches have the same opcode
		/// and one of the nybbles in the instruction tells us under what condition(s)
		/// we should branch.
		/// </summary>
		/// <param name="line">The ParsedLine for this line</param>
		private bool Handle_BR(ParsedLine line) {
			const int BrWidth = 8;
			Emit(line.OpDef.Op);
			(bool OK, byte ixReg, ushort address) = ParseAddress(line.Parms[0]);
			if (!OK) { return false; }
			byte cond = BranchMasks[line.Mnemonic];
			byte reg  = GlueNybbles(cond, ixReg);
			Emit(reg);
			Emit(address);
			Pad(BrWidth);
			return true;
		}

//---------------------------------------------------------------------------------------

		// Processes the DI (Define Integer) psuedo-op.
		// Supports "DI <num>" and "DI <label>"
		private bool Handle_DI(ParsedLine line) {
			const int DiWidth = 4;
			string parm = line.Parms[0];
			bool bIsNum = ushort.TryParse(parm, out ushort num);
			if (! bIsNum) {
				var (_, _, numb) = ParseAddress(parm);
				num = numb;
			}
			byte[] bytes = BitConverter.GetBytes(num);
			Emit(bytes);
			Pad(DiWidth);
			return true;
		}

//---------------------------------------------------------------------------------------

		// Processes the DS (Define String) psuedo-op
		private bool Handle_DS(ParsedLine line) {
			string s = line.DsText;
			bool showsource = true;
			int HexLength = 0;
			int maxwidth = MAX_INSTRUCTION_LENGTH / 2;
			int addr = line.SourceAddress;
			// Here's where it gets messy. We have a couple of goals.
			// 1) We want to split the hex display of the text over
			//	  several lines
			// 2) We want to display the source line, but only on the first
			//	  line of hex
			// 3) We need to pad the last line with spaces, in case this is
			//	  the first (and only) line of hex, so the source displays
			//	  correctly
			for (int i = 0; i < s.Length; i++) {
				Emit((byte)s[i]);
				++HexLength;
				++addr;
				if (IsDoneShowingDsHexRow(s, maxwidth, i)) {
					if (IsFinalLineOfDsHex(s, i)) {
						Pad(HexLength * 2);
						if (showsource) {
							Console.WriteLine(line.Source);
						} else {
							Console.WriteLine();
						}
					} else {
						// End of hex line, but not end of all hex
						showsource = HandleDsNewline(line, showsource);
						Console.Write($"{addr:X4} ");
					}
					HexLength = 0;
				}
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		// TODO: Comment
		private static bool IsDoneShowingDsHexRow(string s, int maxwidth, int i) {
			if (IsFinalLineOfDsHex(s, i)) { return true; }
			return i % maxwidth == maxwidth - 1;        // If non-final line of hex
		}

//---------------------------------------------------------------------------------------

		private static bool IsFinalLineOfDsHex(string s, int i) => i == s.Length - 1;

//---------------------------------------------------------------------------------------

		// TODO: Comment
		private bool HandleDsNewline(ParsedLine line, bool showsource) {
			if (showsource) {
				// Show source on first line only
				Pad(MAX_INSTRUCTION_LENGTH);
				Console.WriteLine(line.Source);
				showsource = false;
			} else {
				Console.WriteLine();
			}
			return showsource;
		}

//---------------------------------------------------------------------------------------

		// Some opcodes (e.g. $TRACEON) have no arguments. Simple!
		private bool Handle_NoArgs(ParsedLine line) {
			const int NoArgWidth = 2;
			Emit(line.OpDef.Op);
			Pad(NoArgWidth);
			return true;
		}

//---------------------------------------------------------------------------------------

		// Some opcodes (e.g. RET) take only one argument which is a register.
		private bool Handle_OneReg(ParsedLine line) {
			const int NoArgWidth = 4;
			Emit(line.OpDef.Op);
			Emit(DecodeReg(line.Parms[0]));
			Pad(NoArgWidth);
			return true;
		}

//---------------------------------------------------------------------------------------

		// Some opcodes (e.g. $PNUM) take one argument which is an address
		private bool Handle_OneAddr(ParsedLine line) {
			const int OneArgWidth = 8;
			Emit(line.OpDef.Op);
			(_, byte ixReg, ushort address) = ParseAddress(line.Parms[0]);
			Emit(ixReg);
			Emit(address);
			Pad(OneArgWidth);
			return true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Adds blanks to the source listing so that things display pretty. Note:
		/// change the pad character to, say, '.' to see the padding during debugging
		/// </summary>
		/// <param name="width">The width of what we've printed so far that
		/// needs to be blank-padded out to a standard length</param>
		void Pad(int width) => Console.Write("".PadRight(MAX_INSTRUCTION_LENGTH - width + 1, ' '));

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Copy the specified byte to Ram, incrementing the address
		/// </summary>
		/// <param name="b">The byte to go into Ram</param>
		/// <param name="print">Whether to echo the hex for the byte to the screen</param>
		void Emit(byte b, bool print = true) {
			Ram[CodeAddress++] = b;
			if (print) { Console.Write($"{b:X2}"); }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Copies a vector of bytes to Ram. Note: For historical reasons, Intel chips
		/// read and write data into memory backwards. This is called Little Endian
		/// notation (look it up). For example, if we had a short value of 0xABCD,
		/// in Ram it would look like CD AB. To make it easier for a beginning
		/// assembler programmer to read, I'm going to make it look like the computer
		/// we're going to simulate (like the IBM 360 mainframe this is modelled 
		/// after) is Big Endian.
		/// </summary>
		/// <param name="bs">The bytes to go into Ram</param>
		/// <param name="print">Whether to echo the hex for the data to 
		/// the screen</param>
		void Emit(byte[] bs, bool print = true) {
			// Convert to big endian
			for (int i = bs.Length - 1; i >= 0; i--) {
				Emit(bs[i], print);
			}
		}

//---------------------------------------------------------------------------------------

		// Copies a numeric value to memory
		void Emit(ushort num, bool print = true) {
			byte[] bytes = BitConverter.GetBytes(num);
			Emit(bytes, print);
		}

//---------------------------------------------------------------------------------------

		// If we get here, the source program had no syntax errors. Amazing!
		internal void Run() {
			var interp = new Interpreter(Ram, Symtab);
			interp.Run();
		}
	}
}
