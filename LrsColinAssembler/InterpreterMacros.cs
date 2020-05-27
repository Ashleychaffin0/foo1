using System;

namespace LrsColinAssembler {
	internal partial class Interpreter {

//---------------------------------------------------------------------------------------

		private void Exec_TraceOn() {                 // 0xFA
			Tracing = true;
			++PC;
		}

//---------------------------------------------------------------------------------------

		private void Exec_TraceOff() {                 // 0xFB
			Tracing = false;										
			++PC;
		}

//---------------------------------------------------------------------------------------

		private void Exec_PREG() {                  // 0xFC
			byte Reg = Ram[PC + 1];
			Console.Write(Registers[Reg]);
			PC += 2;
		}

//---------------------------------------------------------------------------------------

		private void Exec_PNUM() {                  // 0xFD
			short addr = DecodeAddress(PC + 1);
			Console.Write(Word(addr));
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_PSTRING() {               // 0xFE
			short addr = DecodeAddress(PC + 2);
			while (true) {
				byte b = Ram[addr++];
				if (b == 0) { break; }
				Console.Write((char)b);
			}
			PC += 4;
		}

//---------------------------------------------------------------------------------------

		private void Exec_STOP() => IsRunning = false;	// 0xFF
	}
}
