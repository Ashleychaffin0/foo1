using System;

namespace LrsColinAssembler {
	internal partial class Interpreter {

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Starts tracing
		/// </summary>
		private void Exec_TraceOn() {                 // 0xFA
			Tracing = true;
			++IP;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Stops tracing
		/// </summary>
		private void Exec_TraceOff() {                 // 0xFB
			Tracing = false;										
			++IP;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Displays the contents of a register
		/// </summary>
		private void Exec_PREG() {                  // 0xFC
			LastOpWasPrint = true;
			byte Reg = Ram[IP + 1];
			Console.Write(Registers[Reg]);
			IP += 2;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Displays the contents of a numeric field in memory
		/// </summary>
		private void Exec_PNUM() {                  // 0xFD
			LastOpWasPrint = true;
			short addr = DecodeAddress(IP + 1);
			Console.Write(Word(addr));
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Displays the contents of a string in memory
		/// </summary>
		private void Exec_PSTRING() {               // 0xFE
			short addr = DecodeAddress(IP + 2);
			LastOpWasPrint = true;
			while (true) {
				byte b = Ram[addr++];
				if (b == 0) { break; }
				Console.Write((char)b);
			}
			IP += 4;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Stops execution of the program
		/// </summary>
		private void Exec_STOP() => IsRunning = false;	// 0xFF
	}
}
