using System;
using System.IO;

namespace FlipBits {
	class Program {
		static int Main(string[] args) {
			if (args.Length != 1) {
				Console.WriteLine("Usage: FlipBits <filename>");
				return 1;
			}
			if (!File.Exists(args[0])) {
				Console.WriteLine($"File {args[0]} does not exist");
				return 2;
			}
			byte[] bytes = File.ReadAllBytes(args[0]);
			for (int i = 0; i < bytes.Length; i++) {
				bytes[i] = (byte)(bytes[i] ^ 0xFF);
			}
			File.WriteAllBytes(args[0], bytes);
			return 0;
		}
	}
}
