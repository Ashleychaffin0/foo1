/*
LD, LR, LA, LI reg, addr[ix] -- 0x5A, 0x41, 
LR
ST, STC reg, addr[ix]
ADD/SUB reg, addr[ix]
AR/SR
MUL/DIV evenreg, addr[ix]
AND/OR/XOR
MVC target, src, length
COMP, COMPI reg, addr[ix]
JMP, JL, JLE, JE, JNE, JGE, JG addr[ix]
CALL addr[ix]
; set up BP
RET
PUSH reg, addr[ix]
POP reg
$READ, $PRINT addr[ix], length
DC, DS
LC reg,addr[ix]		Insert character
*/

namespace LrsColinAssembler {
	public enum OpcodeFlags {
		None = 0,
	}
}
