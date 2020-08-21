using System;
using System.Collections.Generic;

namespace LrsColinAssembler {
	internal partial class Interpreter {

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Gets a word from memory
		/// </summary>
		/// <param name="address">Where the word is located</param>
		/// <returns>The contents of that word</returns>
		private short Word(ushort address) {
			short val = (short)((Ram[address] << 8) | Ram[address + 1]);
			return val;
		}

//---------------------------------------------------------------------------------------

		private short Word(int address) => Word((ushort)address);

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Stores data into Ram
		/// </summary>
		/// <param name="address">Where the data is to be stored</param>
		/// <param name="data">The data</param>
		private void SetWord(short address, short data) {
			Ram[address] = (byte)((data & 0xFF00) >> 8);
			Ram[address + 1] = (byte)(data & 0xFF);
			if (Tracing) {
				dbgInfo.RamChanges.Add((address, 2));
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Takes a byte at an address and splits it into two nybbles
		/// </summary>
		/// <param name="address">The address of the byte</param>
		/// <returns>A tuple with the two nybbles</returns>
		private (byte Reg, byte ixReg) DecodeRegs(int address) {
			byte regs  = Ram[address];
			byte reg   = (byte)(regs >> 4);
			byte ixreg = (byte)(regs & 0x0F);
			return (reg, ixreg);
		}

//---------------------------------------------------------------------------------------

		// TODO: Put comment here
		private short DecodeAddress(int address) {
			// Note: Asssumes regs is in the preceding byte
			(_, byte ixReg) = DecodeRegs(address - 1);
			short addr      = Word(address);
			addr           += GetIndexRegisterValue(ixReg);
			return addr;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Gets the value of an index register. Note that R0 as an index register
		/// always returns 0.
		/// </summary>
		/// <param name="n">The register number</param>
		/// <returns>The contents of the register, or 0 for register 0</returns>
		private short GetIndexRegisterValue(byte n) {
			if (n == 0) { return 0; }       // Register 0 is special
			TrcShowReg(n);
			return Registers[n];
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Compares two numbers and sets the condition code
		/// </summary>
		/// <param name="val1">The first value</param>
		/// <param name="val2">The second value</param>
		private void Compare(short val1, short val2) {
			if (val1 < val2) {
				CC = CondCode.Low;
			} else if (val1 > val2) {
				CC = CondCode.High;
			} else {
				CC = CondCode.Equal;
			}
			ShowCC = true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Sets the condition code by comparing the specified value to 0
		/// </summary>
		/// <param name="val">The specified value</param>
		private void SetCC(short val) => Compare(val, 0);
	}
}
