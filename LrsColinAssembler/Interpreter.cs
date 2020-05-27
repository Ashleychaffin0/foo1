using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

namespace LrsColinAssembler {
	internal partial class Interpreter {
		Dictionary<byte, Action> MapOpcodeToExec;

		readonly byte[]	  Ram;

		// I had a bug whereby I was picking up the wrong
		// value for an index register. Previously I just
		// had <short[] Registers = new short[16];>. But
		// now, doing it this way, with an indexer, I could
		// be notified if a register ever got out of range
		// (for whatever the code below considered "out of
		// range"). Anyway, I'm putting #if/#endif around
		// this code, in case I ever have to track down
		// such a bug in the future, but otherwise it will
		// be a simple <private short[] Registers = new
		// short[16]> again.
#if true
		private readonly short[] Registers = new short[16];
#else
		private xxRegisters Registers = new xxRegisters();

		private class xxRegisters {
			readonly short[]  xRegisters = new short[16];
			public short this[int i] {
				get { return xRegisters[i]; }
				set {
					xRegisters[i] = value;
					if ((value > 200) || (value < -100)) {
						System.Diagnostics.Debugger.Break();
					}
				}
			}

			public int Length {
				get => 16;
			}
		}
#endif
		ushort		PC;					// Program Counter (aka Address)
		CondCode	CC = CondCode.None;	// Condition Code

		bool	IsRunning = true;
		bool	Tracing   = false;

//---------------------------------------------------------------------------------------

		public Interpreter(byte[] ram) {
			Ram = ram;

			// Initialize registers
			for (int i = 0; i < Registers.Length; i++) {
				Registers[i] = 0;
			}

			SetupMapTable();
		}

//---------------------------------------------------------------------------------------

		private void SetupMapTable() {
#pragma warning disable IDE0022 // Use expression body for methods
			MapOpcodeToExec = new Dictionary<byte, Action>() {
				// Load/Store opcodes
				[0x12] = Exec_LTR,			// Load and Test Register
				[0x18] = Exec_LR,			// Load Register
				[0x41] = Exec_LA,			// Load Address
				[0x42] = Exec_STC,          // Store Character
				[0x43] = Exec_IC,			// Insert Character
				[0x50] = Exec_ST,			// Store
				[0x58] = Exec_L,			// Load

				// Aritmetic opcodes
				[0x1A] = Exec_AR,			// Add Register
				[0x1B] = Exec_SR,			// Add Register
				[0x5A] = Exec_Add,			// Add (duh!)
				[0x5B] = Exec_Sub,			// Subtract Register
				[0x5C] = Exec_Mul,			// Multiply
				[0x5D] = Exec_Div,			// Divide

				// Branches
				[0x47] = Exec_Branches,		// All flavors

				// Subroutines
				[0x45] = Exec_Call,			// Call subroutine
				[0x07] = Exec_Ret,			// Return from subroutine

				// Quasi-macros
				[0xFA] = Exec_TraceOn,		// Start tracing
				[0xFB] = Exec_TraceOff,		// End tracing
				[0xFC] = Exec_PREG,			// Print register
				[0xFD] = Exec_PNUM,			// Print number field
				[0xFE] = Exec_PSTRING,		// Print string field
				[0xFF] = Exec_STOP			// Buh-bye
			};
#pragma warning restore IDE0022 // Use expression body for methods
		}

//---------------------------------------------------------------------------------------

		internal void Run() {
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Execution begins...");
			PC = 0;
			while (IsRunning) {
				byte opcode = Ram[PC];
				// TODO: Needs try/catch
				try {
					if (Tracing) {
						Console.WriteLine($"Trace: {opcode:X2} at {PC:X4}");
						DumpRegs();
					}
					MapOpcodeToExec[opcode]();
					if (Tracing) { Console.WriteLine($"CC = {CC}"); }
				} catch (Exception ex) {
					Console.WriteLine($"*** Runtime exception -- {ex.Message} at {PC:X4}");
				}
			}
			Console.WriteLine("Program has ended");
		}

//---------------------------------------------------------------------------------------

		private void DumpRegs() {
			for (int i = 0; i < 16; i++) {
				string msg = $"R{i}".PadRight(4);
				msg += $"{Registers[i]:X4}";
				msg += $" ({Registers[i]:N0})".PadRight(8);
				Console.Write(msg);
				if ((i > 0) && ((i % 4) == 3)) { Console.WriteLine(); }
			}
		}
	}
}