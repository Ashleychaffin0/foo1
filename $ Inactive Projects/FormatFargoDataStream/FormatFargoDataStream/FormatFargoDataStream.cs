using System;
using System.IO;
using System.Text;

namespace FormatFargoDataStream {
	/// <summary>
	/// Formats a Fargo DTCxxx data stream. Note that we assume
	/// that the stream is well-formed. For example, we don't bother
	/// checking for an ESC character at the very end of the file, with
	/// no further characters following it.
	/// </summary>
	class FormatFargoDataStream {

		string	filename;
		byte []	bytes;
		int		filelen;

		FormatFargoDataStream(string [] args) {
			filename = args[0];
		}

		void Run() {
			FileStream		fs;
			try {
				fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			} catch (FileNotFoundException) {
				Console.WriteLine("File {0} not found.", filename);
				return;
			}
			filelen = (int)fs.Length;
			bytes = new byte[filelen];
			fs.Read(bytes, 0, filelen);
			ProcessDataStream();
		}

		void ProcessDataStream() {
			int		i		= 0;
			byte	b;
			while (i < filelen) {
				b = bytes[i];
				switch (b) {
				case 0x1b:
					ProcessEscapeSequence(ref i); 
					break;
				case 0xA1:
					ProcessUncompressedData(ref i);
					break;
				case 0xA2:
					ProcessCD(ref i);
					break;
				default:
					Console.WriteLine("Unexpected character {0:x} at offset {1:x}", bytes[i], i);
					return;
				}
			}
		}

		void ProcessEscapeSequence(ref int i) {
			int		j;
			byte	b = bytes[++i];		// byte after the ESC
			Console.WriteLine("ESC {0:x} found at offset {1:x}", b, i);
			switch (b) {
			case 0x55:
				// Look for alpha
				for (j=i+1; j<filelen; ++j) {
					b = bytes[j];
					bool	bAlpha;
					bAlpha = b >= Convert.ToByte('A') && b <= Convert.ToByte('Z');
					if (! bAlpha)
						bAlpha = b >= Convert.ToByte('a') && b <= Convert.ToByte('z');
					if (bAlpha) {
						break;
					}
				}
				i = j + 1;
				break;
			case 0x00:
				break;
			default:
				Console.WriteLine("Invalid byte ({0:x}) after ESC at offset {1}", b, i);
				// We could throw, return bool, etc. But for now...
				return;
			}
		}

		void ProcessUncompressedData(ref int i) {
			byte RepeatCount = bytes[++i];
			Console.WriteLine("Uncompressed data found at offset {0:x}; Repeat count={1}", i-1, RepeatCount);
			i += 768;			// "Each scan line is comprised of 768 bytes of pixel data"
		}

		void ProcessCD(ref int i) {
			byte RepeatCount = bytes[++i];	
			Console.WriteLine("Compressed data found at offset {0:x}; Repeat count={1}", i-1, RepeatCount);
			StringBuilder	sb = new StringBuilder(800);
			int		j = ++i;
			int		len = 0;
			byte	b;
			string	s;
			while (len < 768) {
				b = bytes[j];
				if (b <= 0x7F) {
					Console.WriteLine("    Char {0:x} repeated {1} times at offset {2:x}", bytes[j+1], b, j);
					sb.Append(Convert.ToChar(bytes[j+1]), b);
					// bytes[j] = # of repetitions-1 of next char
					// bytes[j+1] = char to be repeated
					len += b;
					j += 2;				// Skip over count byte, and repeated byte
				} else {
					b -= 0x7F;
					s = Encoding.ASCII.GetString(bytes, j + 1, b);
					Console.WriteLine("    Unrepeated string of {0} chars at offset {1:x} - {2}", b, j, s);
					sb.Append(s);
					// byte[j] = # of uncompressed chars to follow - 0x7F
					// byte[j+1 ...] = data
					len += b;
					j += 1 + b;
				}
			}
			s = sb.ToString();
			Console.WriteLine("Compressed string-: " + s);
			i = j;
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
#if DEBUG
			args = new string[] {@"C:\fargo.txt"};
#else
			if (args.Length < 1) {
				Console.WriteLine("Usage: FormatFargoDataStream filename");
				return;
			}
#endif
			FormatFargoDataStream	ffds = new FormatFargoDataStream(args);
			ffds.Run();
		}
	}
}
