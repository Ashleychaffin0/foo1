using System;
using System.Collections.Generic;
using System.IO;

// https://en.wikibooks.org/wiki/360_Assembly/360_Instructions
/*
 * L, ST
 * LA
 * ADD, SUB, MUL, Div
 * INC, DEC
 * C, CR
 * ALl the Branches
 * $PNUM reg
 * $PSTRING reg
 * $PAUSE
 * $STOP
 * 
*/

/*
L, LR, LA, reg, addr[ix]
ST, STC reg, addr[ix]
ADD/SUB reg, addr[ix]
AR/SR
MUL/DIV evenreg, addr[ix]
AND/OR/XOR
MVC target, src, length
CLC first, second, length
C, CR, COMPI reg, addr[ix]
JMP, JL, JLE, JE, JNE, JGE, JG addr[ix]
SLL, SLA, SRL, SRA
CALL addr[ix]
; set up BP
RET
PUSH reg, addr[ix]
POP reg
$READ, $PRINT addr[ix], length
$PAUSE, $STOP
DI, DS
IC/STC reg,addr[ix]		Insert character
*/

namespace LrsColinAssembler {
	public class Assembler {
		// Note: A real symbol table would contain more than just the
		//		 address of the symbol, but could contain things like
		//		 the data type, if this were the label of a field
		//		 definition.
		public static Dictionary<string, uint> symtab;

		public static Dictionary<string, Opcode> OpcodeTable = new Dictionary<string, Opcode> {
			["L"]      = new Opcode(0x58, 4),    // Load
			["LA"]     = new Opcode(0x41, 4),   // Load Address
			// ["LR"]  = new Opcode(0x18, 2),	// Load Register
			["ST"]     = new Opcode(0x50, 4),   // Store
			// ["STC"] = new Opcode(0x5C, 4),  // Store Character
			["ADD"]    = new Opcode(0x5A, 4), // Add
			// ["AR"]     = new Opcode(0x1A, 2),   // Add Register
			["SUB"]    = new Opcode(0x5B, 4), // Subtract
			// ["SR"]     = new Opcode(0x1B, 2), // Subtract Register
			["MUL"]    = new Opcode(0x5C, 4), // Multiply
			["DIV"]    = new Opcode(0x5D, 4), // Divide
			// Quasi-macros follow
			["$PAUSE"]   = new Opcode(0xfc, 1),
			["$PNUM"]    = new Opcode(0xfd, 2),
			["$PSTRING"] = new Opcode(0xfe, 4),
			["$STOP"]    = new Opcode(0xff, 1),
			// Declarations follow
			["DI"] = new Opcode(0x00, 4, OpcodeFlags.DI),
			["DS"] = new Opcode(0x00, 0, OpcodeFlags.DS)
		};

//---------------------------------------------------------------------------------------

		internal int Assemble(string filename) {
			var inFile = new StreamReader(filename);
			symtab = new Dictionary<string, uint>();
			int rc = Pass1(inFile);
			if (rc != 0) { return 1; }
			rc = Pass2(inFile);
			return rc;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Create Symbol Table
		/// </summary>
		/// <param name="inFile">The StreamReader for the input file</param>
		/// <returns></returns>
		private int Pass1(StreamReader inFile) {
			int rc = 0;			// s/b enum			
			uint addr = 0;
			string line;
			while ((line = inFile.ReadLine()) != null) {
				try {
					var srcline = new SourceLine(line, addr);
					addr += srcline.OpcodeEntry.Length;
				} catch (InvalidOpcodeException ex) {
					Console.WriteLine($"*** Invalid opcode found: {ex.Opcode}");
					rc = 1;
				}
			}
			return rc;
		}

//---------------------------------------------------------------------------------------

			/// <summary>
			/// Now that the symbol table has been built, actually do the assembly and
			/// generate object code.
			/// </summary>
			/// <param name="inFile">The StreamReader for the input file</param>
			/// <returns></returns>
			private int Pass2(StreamReader inFile) {
			// TODO: Need RAM allocated to generate object code into
			int rc = 0;
			string line;
			inFile.BaseStream.Seek(0, SeekOrigin.Begin);    // Restart at beginning
			uint addr = 0;
			while ((line = inFile.ReadLine()) != null) {
				// TODO: 
				// TODO: List this line along with address, machine code
				Console.WriteLine($"{addr:X4} {line}");
				try {
					var srcline = new SourceLine(line, addr);
					addr += srcline.OpcodeEntry.Length;
				} catch (UnknownSymbolException ex) {
					Console.WriteLine($"*** Field name not found: {ex.Symbol}");
				}
			}
			return rc;
		}

//---------------------------------------------------------------------------------------

		internal void Run(int memsize) {
			var Ram = new byte[memsize];	// TODO: Allocate this in Pass 2
		}
	}
}
