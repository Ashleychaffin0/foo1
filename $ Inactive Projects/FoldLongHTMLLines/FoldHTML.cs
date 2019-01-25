using System;
using System.IO;
using System.Text;

namespace FoldLongHTMLLines
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class FoldHTML
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)	{
			if (args.Length != 2) {
				Usage();
				return;
			}
			int maxlen;
			try {
				maxlen = int.Parse(args[0]);
			} catch (FormatException e) {
				Usage();
				return;
			}
			string	dir = args[1];
			string [] files = Directory.GetFiles(dir, "*.htm*");
			foreach (string file in files) {
				FoldHTMLFile(file, maxlen);
			}
		}

		static void FoldHTMLFile(string file, int maxlen) {
			Console.WriteLine("Processing file {0}", file);
			FileStream	f;
			try {
				f = File.Open(file, FileMode.Open,FileAccess.Read, FileShare.Read);
			} catch (IOException e) {
				Console.WriteLine("    Error opening file - {0}", e.ToString());
				return;
			}
			long	len = f.Length;
			byte [] data = new byte[len];
			f.Read(data, 0, (int)len);
			f.Close();
			f = File.OpenWrite(file);
			string s = Encoding.ASCII.GetString(data);
			FoldData(f, s, maxlen);
			f.Close();
		}

		static void FoldData(FileStream f, string s, int maxlen) {
			// First of all, normalize all line-ends to \n
			s = s.Replace("\r\n", "\n");	// All CRLFs to LFs
			s = s.Replace("\r", "");		// If any lone CRs are left, turf them
			string	txt;
			int		preflen;
			foreach (string line in s.Split(new char []{'\n'})) {
				txt = line;					// Since <line> is readonly
				while (txt.Length > maxlen) {
					preflen = Cutline(txt, maxlen);
					if (preflen == txt.Length) {
						// Might not have been able to cut it (e.g. long string)
						break;
					}
					Console.WriteLine("  %%% {0}", txt.Substring(0, preflen));
					txt = txt.Substring(preflen);
				}
				Console.WriteLine(txt);
			}
		}

		static int Cutline(string s, int maxlen) {
			bool	bInString = false, bInTag = false;
			char	c;
			int		len = -1;
			for (int i=0; i<s.Length; ++i) {
				if (len >= 0)
					break;
				c = s[i];
				switch (c) {
				case '<':
					// I suppose we could have nested <'s, but I'll ignore them
					if (! bInString)
						bInTag = true;
					break;
				case '>':
					if (! bInString) {
						if (bInTag) {
							bInTag = false;
							if (i >= maxlen)
								len = i;
						}
					}
					break;
				case '"':
					if (bInString) {
						bInString = false;
						if (i >= maxlen)
							len = i;
					}
					break;
				case ' ':
					if (! bInString) {
						if (i >= maxlen)
							len = i;
					}
					break;
				default:
					break;
				}
			}
			return len + 1;		// +1 to convert offset to length
		}

		static void Usage() {
			Console.WriteLine("Usage: FoldHTML maxlen directory");
		}
	}
}
