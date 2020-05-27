using System;
using System.Collections.Generic;

namespace LrsColinAssembler {
	internal partial class Interpreter {

//---------------------------------------------------------------------------------------

		private short Word(short address) {
			short val = (short)((Ram[address] << 8) | Ram[address + 1]);
			return val;
		}

//---------------------------------------------------------------------------------------

		private short Word(int address) => Word((short)address);

//---------------------------------------------------------------------------------------

		private void SetWord(short address, short data) {
			Ram[address] = (byte)((data & 0xFF00) >> 8);
			Ram[address + 1] = (byte)(data & 0xFF);
		}

//---------------------------------------------------------------------------------------

		private (byte Reg, byte ixReg) DecodeRegs(int address) {
			byte regs  = Ram[address];
			byte reg   = (byte)(regs >> 4);
			byte ixreg = (byte)(regs & 0x0F);
			return (reg, ixreg);
		}

//---------------------------------------------------------------------------------------

		private short DecodeAddress(int address) {
			// TODO: Asssumes regs is in the preceding byte
			(_, byte ixReg) = DecodeRegs(address - 1);
			short addr      = Word(address);
			addr           += AddIndex(ixReg);
			return addr;
		}

//---------------------------------------------------------------------------------------

		private short AddIndex(byte n) {
			if (n == 0) { return 0; }		// Register 0 is special
			return Registers[n];
		}

//---------------------------------------------------------------------------------------

		private void Compare(short val1, short val2) {
			if (val1 < val2) {
				CC = CondCode.Low;
			} else if (val1 > val2) {
				CC = CondCode.High;
			} else {
				CC = CondCode.Equal;
			}
		}

//---------------------------------------------------------------------------------------

		private void SetCC(short val) => Compare(val, 0);
	}
}
