using System;
using System.IO;

namespace hd {
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class HexDump {

		readonly int	linesize = 16;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)	{
			if (args.Length < 1) {
				Console.Error.WriteLine("Usage: hd filename");
				return 1;
			}
			string	filename = args[0];
			if (! File.Exists(filename)) {
				Console.Error.WriteLine("File {0} does not exist", filename);
				return 2;
			}

			HexDump	hd = new HexDump();
			hd.Dump(filename);

			// WriteASCIIChart();

			return 0;
		}

//---------------------------------------------------------------------------------------

		static void WriteASCIIChart() {
			char	c;
			Console.WriteLine("    0 1 2 3 4 5 6 7 8 9 A B C D E F");
			for (int row=0; row<255; row+=16) {
				Console.Write("{0:X1}x:", row / 16);
				for (int col=0; col<=15; ++col) {
					c = (char)(row + col);		// For debugging, not otherwise needed
					switch (row + col) {
					case 0: case 7: case 8: case 9: case 10: case 11: case 12: case 13:
						Console.Write(" .");
						break;
					default:
						Console.Write(" {0}", (char)(row + col));
						break;
					}
				}
				Console.WriteLine();
			}
		}

//---------------------------------------------------------------------------------------

		void Dump(string filename) {
			int			offset, LenRead;
			byte []		line = new byte[linesize];		
			FileStream	fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			int			filesize = (int)fs.Length;
			for (offset=0; offset < filesize; offset += linesize) {
				LenRead = fs.Read(line, 0, linesize); 
				FormatLine(offset, line, LenRead);
			}
			fs.Close();
		}

//---------------------------------------------------------------------------------------

		void FormatLine(int offset, byte [] line, int LenRead) {
			int		i;
			Console.Write("{0:X8}:", offset);
			for (i=0; i<LenRead; ++i) {
				Console.Write(" {0:X2}", line[i]);
			}
			for (; i<linesize; ++i) {				// Pad out rest of line, if necessary
				Console.Write("   ");
			}
			Console.Write("  ");
			char	c;
			for (i=0; i<LenRead; ++i) {
				c = Convert.ToChar(line[i]);		// Or just (char)line[i]
#if true
				switch (c) {
				case '\u0000':
				case '\a': case '\b': case '\t': case '\r': case '\v': case '\f': case '\n':
					c = '.';
					break;
				default:
					break;
				}
#else
				if (char.IsControl(c)) {
					c = '.';
				}
#endif
				Console.Write("{0}", c);
			}
			Console.WriteLine();
		}
	}
}
