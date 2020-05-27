namespace LrsColinAssembler {
	internal partial class Interpreter {
		private void Exec_LTR() {               // 0x12 - Load Register
			(byte RegTarget, byte RegSource) = DecodeRegs(PC + 1);
			Compare(Registers[RegTarget], Registers[RegSource]);
			PC += 2;
		}

//---------------------------------------------------------------------------------------

		private void Exec_LR() {                // 0x18 - Load Register
			(byte RegTarget, byte RegSource) = DecodeRegs(PC + 1);
			Registers[RegTarget] = Registers[RegSource];
			PC += 2;
		}

//---------------------------------------------------------------------------------------

		private void Exec_LA() {                // 0x41 - Load Address
			(byte Reg, _)  = DecodeRegs(PC + 1);
			short addr     = DecodeAddress(PC + 2);
			Registers[Reg] = addr;
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_STC() {                // 0x42 - Store Character
			(byte Reg, _) = DecodeRegs(PC + 1);
			short addr = DecodeAddress(PC + 2);
			Ram[addr] = (byte)(Registers[Reg] & 0x00FF);
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_IC() {                // 0x43 - Insert Character
			(byte Reg, _) = DecodeRegs(PC + 1);
			short addr = DecodeAddress(PC + 2);
			Registers[Reg] = (short)((Registers[Reg] & 0xFF00) | Ram[addr]);
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_ST() {                // 0x50 - Store
			(byte Reg, _) = DecodeRegs(PC + 1);
			short address = DecodeAddress(PC + 2);
			SetWord(address, Registers[Reg]);
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_L() {                 // 0x58 - Load
			(byte Reg, _) = DecodeRegs(PC + 1);
			short address = DecodeAddress(PC + 2);
			short data = Word(address);
			Registers[Reg] = data;
			PC += 4;
		}
	}
}