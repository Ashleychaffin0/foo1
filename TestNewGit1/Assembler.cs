using System.Collections.Generic;

// https://en.wikibooks.org/wiki/360_Assembly/360_Instructions
/*
 * L, ST, la
 * ADD, SUB, MUL, Div
 * INC, DEC
 * C, CR
 * ALl the Branches
 * $PNUM reg
 * $PSTRING
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
DC, DS
IC/STC reg,addr[ix]		Insert character
*/

namespace LrsColinAssembler {
	public class Assembler {
		public static Dictionary<string, Opcode> OpcodeTable = new Dictionary<string, Opcode> {
			["L"]   = new Opcode(0x58, 4),	// Load
			["LA"]  = new Opcode(0x41, 4),	// Load Address
			// ["LR"]  = new Opcode(0x18, 2),	// Load Register
			["ST"]  = new Opcode(0x50, 4),	// Store
			["STC"] = new Opcode(0x5C, 4),	// Store Character
			["ADD"] = new Opcode(0x5A, 4), // Add
			["AR"]  = new Opcode(0x1A, 2),	// Add Register
			["SUB"] = new Opcode(0x5B, 4), // Subtract
			["SR"]  = new Opcode(0x1B, 2), // Subtract Register
			["MUL"] = new Opcode(0x5C, 4), // Multiply
			["DIV"] = new Opcode(0x5D, 4), // Divide

		};
	}
}
