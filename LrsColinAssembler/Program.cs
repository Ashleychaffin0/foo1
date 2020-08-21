// "A foolish consistency is the hobgoblin of little minds."
//		- Ralph Waldo Emerson

// Note: Since this is just a program to introduce my cousin Colin
//		 to a concrete example of machine/assembler language, it
//		 assumes that the source code is pretty well perfect and
//		 we'll do limited error detection. So no comments, please,
//		 along the lines of "But it could fail here!". Yep, you're
//		 right, it could. But this is hardly a production-class
//		 program!

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
			string temp = System.IO.Path.GetTempFileName();
			using (var cc = new ConsoleCopy(temp)) {
				int rc = avengers.Assemble(filename);
				if (rc != 0) { return 1; }
			}
			DebugInfo.CopyToClipboard(System.IO.File.ReadAllText(temp));
			avengers.Run();
			return 0;
		}
	}
}
