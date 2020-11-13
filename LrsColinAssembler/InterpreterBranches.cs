using System;
using System.Runtime.InteropServices.ComTypes;

namespace LrsColinAssembler {
	internal partial class Interpreter {
		/// <summary>
		/// Compares the contents of the first register with the second.
		/// </summary>
		private void Exec_CR() {
			(byte first, byte second) = DecodeRegs(IP + 1);
			Compare(Registers[first], Registers[second]);
			IP += 2;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Compares data in a register with data in memory
		/// </summary>
		private void Exec_Cmp() {
			(byte RegTarget, _) = DecodeRegs(IP + 1);
			short address = DecodeAddress(IP + 2);
			short data = Word(address);
			Compare(Registers[RegTarget], data);
			TrcShowReg(RegTarget);
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Implements the various Branch opcodes
		/// </summary>
		private void Exec_Branches() {                // 0x47 -- All branches
			(byte CCMask, _) = DecodeRegs(IP + 1);
			short address    = DecodeAddress(IP + 2);
			if ((CCMask & (byte)CC) != 0) {
				IP = (ushort)address;
				if (Tracing && !bIsRunOnly) { Console.Write(" (Taken)"); }
			} else {
				IP += 4;
				if (Tracing && !bIsRunOnly) { Console.Write(" (Fall through)"); }
			}
			if (Tracing && !bIsRunOnly) { Console.WriteLine(); }
		}
	}
}