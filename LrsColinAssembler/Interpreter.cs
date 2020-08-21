using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Intrinsics.X86;

namespace LrsColinAssembler {
	internal partial class Interpreter {
		Dictionary<byte, Action>	MapOpcodeToExec;

		static public byte[]		Ram;

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
		ushort		IP;					// Program Counter (aka Address)
		CondCode	CC = CondCode.None;	// Condition Code

		bool		IsRunning = true;

		bool								Tracing   = false;
		bool								LastOpWasPrint = false;
		internal static bool				ShowCC;
		internal static BitVector32			RegsChanged;
		static DebugInfo					dbgInfo;
		readonly Dictionary<string, ushort>	Symtab;

//---------------------------------------------------------------------------------------

		public Interpreter(byte[] ram, Dictionary<string, ushort> symtab) {
			Ram = ram;

			// Initialize registers
			for (int i = 0; i < Registers.Length; i++) {
				Registers[i] = 0;
			}

			SetupMapTable();

			Symtab = symtab;
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
				[0x5D] = Exec_Div,          // Divide

				// Branches et al
				[0x19] = Exec_CR,           // Compare registers
				[0x47] = Exec_Branches,		// All flavors
				[0x59] = Exec_Cmp,			// Compare

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

			// First, update our mapping table source line
			foreach (ushort key in Assembler.MapAddressToSource.Keys) {
				ParsedLine pline = Assembler.MapAddressToSource[key];
				string line      = pline.Source.Replace('\t', ' ');
				string[] src     = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				Assembler.MapAddressToSource[key].Source = string.Join(' ', src);
			}

			Console.WriteLine("Execution begins...");
			dbgInfo = new DebugInfo(Registers, CC);
			IP = 0;
			while (IsRunning) {
				byte opcode = Ram[IP];
				try {
					if (Tracing) {
						dbgInfo.ShowOpcode(IP, Ram, Symtab);
						dbgInfo = new DebugInfo(Registers, CC);
						LastOpWasPrint = false;
						ShowCC = false;
						RegsChanged = new BitVector32(0);
					}
					MapOpcodeToExec[opcode]();
					if (Tracing) {
						if (LastOpWasPrint) { Console.WriteLine(); }
						dbgInfo.ShowChanged(Registers, CC);
					}
				} catch (Exception ex) {
					Console.WriteLine($"*** Runtime exception -- {ex.Message} at {IP:X4}");
				}
			}
			Console.WriteLine();
			Console.WriteLine("Program has ended");
		}

//---------------------------------------------------------------------------------------

		private void TrcShowReg(byte regTarget) => RegsChanged[1 << regTarget] = true;
	}
}