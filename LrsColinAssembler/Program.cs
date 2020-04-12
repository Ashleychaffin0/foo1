using System;
using System.Collections.Generic;
using System.Text;

namespace LrsColinAssembler {
	class Program {
		public static int Main(string[] args) {
			string filename;
			if (args.Length == 0) {
				filename = @"G:\lrs\FakeAssemblerProgram.asm";
			} else {
				filename = args[0];
			}
			var avengers = new Assembler();
			int rc = avengers.Assemble(filename);
			if (rc != 0) { return rc; }
			avengers.Run(1024);		// Could make this an arg
			return 0;
		}
	}
}
