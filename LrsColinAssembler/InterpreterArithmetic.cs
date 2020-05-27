using System;

namespace LrsColinAssembler {
	internal partial class Interpreter {
		private void Exec_AR() {                // 0x1A - Add Register
			(byte RegTarget, byte RegSource) = DecodeRegs(PC + 1);
			Registers[RegTarget] += Registers[RegSource];
			PC += 2;
		}

//---------------------------------------------------------------------------------------

		private void Exec_SR() {                // 0x1B - Subtract Register
			(byte RegTarget, byte RegSource) = DecodeRegs(PC + 1);
			Registers[RegTarget] -= Registers[RegSource];
			PC += 2;
		}

//---------------------------------------------------------------------------------------

		private void Exec_Add() {                // 0x5A - Add
			(byte Reg, _) = DecodeRegs(PC + 1);
			short address = DecodeAddress(PC + 2);
			short data = Word(address);
			Registers[Reg] += data;
			SetCC(data);
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_Sub() {                // 0x5B - Subtract
			(byte Reg, _) = DecodeRegs(PC + 1);
			short address = DecodeAddress(PC + 2);
			short data = Word(address);
			Registers[Reg] -= data;
			SetCC(Registers[Reg]);
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_Mul() {                // 0x5C - Multiply
			(byte Reg, _) = DecodeRegs(PC + 1);
			short address = DecodeAddress(PC + 2);
			short data = Word(address);
			Registers[Reg] *= data;
			SetCC(Registers[Reg]);
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_Div() {                // 0x5D - Divide
			(byte Reg, _) = DecodeRegs(PC + 1);
			short address = DecodeAddress(PC + 2);
			short data = Word(address);
			Registers[Reg] /= data;
			SetCC(Registers[Reg]);
			PC += 4;
		}
	}
}