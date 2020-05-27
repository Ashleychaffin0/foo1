using System;
using System.Collections.Generic;
using System.Text;

namespace LrsColinAssembler {
	class Program {
		public static int Main(string[] args) {
			string filename;
			// Support filename on command line, or default
			if (args.Length == 0) {
				filename = @"G:\lrs\FakeAssemblerProgram.asm";
			} else {
				filename = args[0];
			}
			var avengers = new Assembler();
			int rc = avengers.Assemble(filename);
			if (rc != 0) { return 1; }
			avengers.Run();
			return 0;
		}
	}
}
