using System;
using System.IO;

namespace QnDSearch {
	/// <summary>
	/// Summary description for QnDSearch.
	/// </summary>
	class QnDSearch {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args) {
			if (args.Length < 2) {
				Console.WriteLine("Usage: QnDSearch filename word1 [word2 ...]");
				return 1;
			}
			bool	bFoundAll;
			// TODO: Should have try/catch, in case of malformed filename
			foreach (string filename in Directory.GetFiles(".", args[0])) {
				// Console.WriteLine("\nSearching file: {0}", filename);
				bFoundAll = Find(filename, args);
				if (bFoundAll) {
					Console.WriteLine("\tFound specified text in file {0}", filename);
				}
			}
			Console.WriteLine("Done");
			return 0;
		}

		static bool Find(string filename, string [] args) {
			TextReader	tr = File.OpenText(filename);
			string		txt = tr.ReadToEnd();
			tr.Close();
			// Ignore args[0]; it's the filename pattern
			for (int i=1; i<args.Length; ++i) {
				if (txt.IndexOf(args[i]) == -1)
					return false;
			}
			return true;
		}
	}
}
