// Implements the Load/Store opcodes

namespace LrsColinAssembler {
	internal partial class Interpreter {
		private void Exec_LTR() {               // 0x12 - Load and Test Register
			// Copies the contents of one register to the other and compares it to zero
			// Note: if the two registers are the same (e.g. LTR R3,R3) then this is
			//		 an easy way to compare a register to zero.
			(byte RegTarget, byte RegSource) = DecodeRegs(IP + 1);
			Registers[RegTarget] = Registers[RegSource];
			TrcShowReg(RegTarget);
			Compare(Registers[RegTarget], 0);
			IP += 2;
		}

//---------------------------------------------------------------------------------------

		private void Exec_LR() {                // 0x18 - Load Register
			// Copies the contents of one register to the other
			(byte RegTarget, byte RegSource) = DecodeRegs(IP + 1);
			TrcShowReg(RegTarget);
			Registers[RegTarget] = Registers[RegSource];
			IP += 2;
		}

//---------------------------------------------------------------------------------------

		private void Exec_LA() {                // 0x41 - Load Address
			// Loads the address of a field into a register. Note that if the field
			// 'name' is a numeric constant, then it just loads that value into the
			// register. Also notice that LA R5,1[R5] is a convenient way to add
			// 1 to a register. Other values work too, such as LA R5,1729[R5]
			(byte Reg, _)  = DecodeRegs(IP + 1);
			short addr     = DecodeAddress(IP + 2);
			Registers[Reg] = addr;
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_STC() {                // 0x42 - Store Character
			// Store the low-order (least significant) 8 bits of a register into Ram
			(byte Reg, _) = DecodeRegs(IP + 1);
			short addr    = DecodeAddress(IP + 2);
			Ram[addr]     = (byte)(Registers[Reg] & 0x00FF);
			// if (Tracing) {
				dbgInfo.RamChanges.Add((addr, 1));
			// }
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_IC() {                // 0x43 - Insert Character
			// The opposite of STC. Changes the low-order 8 bits of a register/
			(byte Reg, _)  = DecodeRegs(IP + 1);
			short addr     = DecodeAddress(IP + 2);
			Registers[Reg] = (short)((Registers[Reg] & 0xFF00) | Ram[addr]);
			TrcShowReg(Reg);
			dbgInfo.RamChanges.Add((addr, 2));
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_ST() {                // 0x50 - Store
			// Stores the contents of a register into Ram
			(byte RegTarget, _) = DecodeRegs(IP + 1);
			short address = DecodeAddress(IP + 2);
			SetWord(address, Registers[RegTarget]);
			TrcShowReg(RegTarget);
			dbgInfo.RamChanges.Add((address, 2));
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_L() {                 // 0x58 - Load
			// Loads a new value into a register from the specified address
			(byte RegTarget, _)  = DecodeRegs(IP + 1);
			short address  = DecodeAddress(IP + 2);
			short data     = Word(address);
			Registers[RegTarget] = data;
			TrcShowReg(RegTarget);
			IP += 4;
		}
	}
}