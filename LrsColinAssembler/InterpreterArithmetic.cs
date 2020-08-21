using System;

namespace LrsColinAssembler {
	internal partial class Interpreter {
		/// <summary>
		/// Adds the contents of the second register into the first
		/// </summary>
		private void Exec_AR() {                // 0x1A - Add Register
			(byte RegTarget, byte RegSource) = DecodeRegs(IP + 1);
			Registers[RegTarget] += Registers[RegSource];
			TrcShowReg(RegTarget);
			TrcShowReg(RegSource);
			SetCC(Registers[RegTarget]);
			IP += 2;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Subtracts the contents of the second register from the first. Note that
		/// subtracting a register from itself (e.g. SR R5,R5) is a good way to 
		/// get zero into a register.
		/// </summary>
		private void Exec_SR() {                // 0x1B - Subtract Register
			(byte RegTarget, byte RegSource) = DecodeRegs(IP + 1);
			Registers[RegTarget] -= Registers[RegSource];
			TrcShowReg(RegTarget);
			TrcShowReg(RegSource);
			SetCC(Registers[RegTarget]);
			IP += 2;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Adds data in memory to the specified register
		/// </summary>
		private void Exec_Add() {                // 0x5A - Add
			(byte RegTarget, _)   = DecodeRegs(IP + 1);
			short address         = DecodeAddress(IP + 2);
			short data            = Word(address);
			Registers[RegTarget] += data;
			TrcShowReg(RegTarget);
			SetCC(data);
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Subtracts data in memory from the specified register
		/// </summary>
		private void Exec_Sub() {                // 0x5B - Subtract
			(byte RegTarget, _)   = DecodeRegs(IP + 1);
			short address         = DecodeAddress(IP + 2);
			short data            = Word(address);
			Registers[RegTarget] -= data;
			TrcShowReg(RegTarget);
			SetCC(Registers[RegTarget]);
			IP += 4; ;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Multiplies a register from data in memory
		/// </summary>
		private void Exec_Mul() {                // 0x5C - Multiply
			(byte RegTarget, _) = DecodeRegs(IP + 1);
			short address = DecodeAddress(IP + 2);
			short data = Word(address);
			Registers[RegTarget] *= data; 
			TrcShowReg(RegTarget);
			SetCC(Registers[RegTarget]);
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Divides a register by data in memory
		/// </summary>
		private void Exec_Div() {                // 0x5D - Divide
			(byte RegTarget, _) = DecodeRegs(IP + 1);
			short address = DecodeAddress(IP + 2);
			short data = Word(address);
			Registers[RegTarget] /= data;
			TrcShowReg(RegTarget);
			SetCC(Registers[RegTarget]);
			IP += 4;
		}
	}
}