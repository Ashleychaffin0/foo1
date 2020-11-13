using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace LrsColinAssembler {
	class DebugInfo {
		readonly short[] Regs;
		readonly Interpreter.CondCode CC;
		public List<(short addr, ushort length)> RamChanges;

//---------------------------------------------------------------------------------------

		public DebugInfo(short[] regs, Interpreter.CondCode cc) {
			Regs = new short[16];
			Array.Copy(regs, Regs, 16);
			CC = cc;
			RamChanges = new List<(short addr, ushort length)>();
		}

//---------------------------------------------------------------------------------------

		public void ShowChanged(short[] regs,
								Interpreter.CondCode cc) {
			bool HasChanges = false;
			string sep = ", ";
			for (int i = 0; i < 16; i++) {
				if ((Regs[i] != regs[i]) || Interpreter.RegsChanged[1 << i]) {
					Console.Write($"{sep}R{i} -> {regs[i]}(0x{regs[i]:X1})");
					HasChanges = true;
				}
			}
			if ((CC != cc) || Interpreter.ShowCC) { 
				Console.Write($"{sep}CC -> {cc}");
				HasChanges = true;
			}

			foreach (var (addr, length) in RamChanges) {
				Console.Write($"{sep}@{addr:X4} -> ");
				HasChanges = true;
				for (int i = 0; i < length; i++) {
					Console.Write($"{Interpreter.Ram[addr + i]:X2}");
				}

			}
			if (HasChanges) { Console.WriteLine(); }
		}

//---------------------------------------------------------------------------------------

		internal void ShowOpcode(ushort pc, byte[] ram, Dictionary<string, ushort> symtab) {
			string label = GetLabel(pc, symtab);
			if (label.Length > 0) { Console.WriteLine($"At {label}"); }
			byte opcode = ram[pc];
			var (mnemonic, opdef) = GetOpcodeInfo(opcode, ram, pc);
			string instruction = "";
			if (mnemonic.StartsWith("$")) {
				// instruction = $"{opcode:X2}"; }
				for (ushort addr = pc; addr < pc + opdef.Length; addr++) {
					instruction += $"{ram[addr]:X2}";
				}
				for (int i = opdef.Length; i < 4; i++) {
					instruction += "  ";
				}
			}  else {
				int len = 2 * (1 + ((opcode & 0xC0) >> 6));
				for (int i = 0; i < len; i++) {
					instruction += $"{ram[pc + i]:X2}"; // Note: Bad way to concatenate
				}
				for (int i = len; i < 4; i++) {
					instruction += "  ";
				}
			}
			// Console.Write($"Trace:  {pc:X4}: {instruction} ({mnemonic})");
			string stmt = Assembler.MapAddressToSource[pc].Source;
			Console.Write($"Trace:  {pc:X4}: {instruction} ");
			var oldfg = Console.ForegroundColor;
			// var oldbg = Console.BackgroundColor;
			Console.ForegroundColor = ConsoleColor.Green;
			// Console.BackgroundColor = ConsoleColor.Cyan;
			Console.Write($"\"{stmt}\"");
			Console.ForegroundColor = oldfg;
			// Console.BackgroundColor = oldbg;
		}

//---------------------------------------------------------------------------------------

		private string GetLabel(ushort pc, Dictionary<string, ushort> symtab) {
			// Yeah, we could do this more efficiently, but for our toy programs,
			// I won't bother
			foreach (string key in symtab.Keys) {
				ushort addr = symtab[key];
				if (addr == pc) { return key; }
			}
			return "";
		}

//---------------------------------------------------------------------------------------

		private (string key, OpcodeDef entry) GetOpcodeInfo(byte opcode, byte[] ram, ushort pc) {
			foreach (string key in Assembler.OpcodeTable.Keys) {
				var entry = Assembler.OpcodeTable[key];
				if (entry.Op == opcode) {
					if (opcode != 0x47) { return (key, entry); }
					byte mask = (byte)((ram[pc + 1] & 0xf0) >> 4);
					foreach (string bkey in Assembler.BranchMasks.Keys) {
						if (Assembler.BranchMasks[bkey] == mask) { return (bkey, entry); }
					}
					throw new Exception($"can't find branch mask {mask:X2}");
				}
			}
			throw new Exception($"*** Error finding mnemonic for opcode {opcode:X2}");
		}

//---------------------------------------------------------------------------------------

		public static void CopyToClipboard(string s) {
			string temp = Path.GetTempFileName();
			// string script = Path.GetTempFileName();

			try {
				File.WriteAllText(temp, s);
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
					// script += ".bat";
					// File.WriteAllText(script, $"clip <{temp}");
					Exec("cmd", $"/K clip <{temp}");
				} else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
					// script += ".sh";
					// TODO: May have to run chmod +x first
					// File.WriteAllText(script, $"pbcopy < {temp}");
					// Exec($"chmod +x {temp}");
					Exec("pbcopy", $"<{temp}");
				} else {
					// Presumably some flavor of *nix. Don't know how to do that.
					// Just ignore
				}
				// Exec(script);
			} finally {
				File.Delete(temp);
				// File.Delete(script);
			}
		}

//---------------------------------------------------------------------------------------

		private static void Exec(string cmd, string args) {
			var proc = Process.Start(cmd, args);
			proc.WaitForExit();
		}
	}

#if false  // No longer used

//---------------------------------------------------------------------------------------

		// Tracing routine
		private void DumpRegs() {
			for (int i = 0; i < 16; i++) {
				string msg = $"R{i}".PadRight(4);
				msg += $"{Registers[i]:X4}";
				msg += $" ({Registers[i]:N0})".PadRight(8);
				Console.Write(msg);
				if ((i > 0) && ((i % 4) == 3)) { Console.WriteLine(); }
			}
		}
#endif
}