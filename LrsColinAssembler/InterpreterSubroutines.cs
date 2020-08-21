using System;

namespace LrsColinAssembler {
	internal partial class Interpreter {

		/// <summary>
		/// Calls a subroutine
		/// </summary>
		private void Exec_Call() {                 // 0x45 - Call subroutine
			(byte Reg, _) = DecodeRegs(IP + 1);
			short address = DecodeAddress(IP + 2);
			Registers[Reg] = (short)(IP + 4);
			RegsChanged[1 << Reg] = true;
			IP = (ushort)address;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Returns from a subroutine
		/// </summary>
		private void Exec_Ret() {                // 0x07 -- Return from Subtroutine
			(byte _, byte RegAddr) = DecodeRegs(IP + 1);
			IP = (ushort)Registers[RegAddr];
			Console.WriteLine();
		}
	}
}