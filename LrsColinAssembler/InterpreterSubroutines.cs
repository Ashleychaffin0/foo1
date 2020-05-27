using System;

namespace LrsColinAssembler {
	internal partial class Interpreter {
		private void Exec_Call() {                 // 0x45 - Call subroutine
			(byte Reg, _) = DecodeRegs(PC + 1);
			short address = DecodeAddress(PC + 2);
			Registers[Reg] = (short)(PC + 4);
			PC = (ushort)address;
		}

//---------------------------------------------------------------------------------------

		private void Exec_Ret() {                // 0x07 -- Return from Subtroutine
			(byte _, byte RegAddr) = DecodeRegs(PC + 1);
			PC = (ushort)Registers[RegAddr];
		}
	}
}