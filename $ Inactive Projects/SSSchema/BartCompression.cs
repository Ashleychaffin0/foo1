// Copyright (c) 2006-2007 by Bartizan Connects, LLC

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Bartizan.Utils.Compression {

	/// <summary>
	/// Allows the user to compress and decompress data, according to the standard PKZIP
	/// Deflate algorithm. 
	/// <para>Note that this is <b>not</b> the same as creating a .zip file.</para>
	/// <para>Several compression algorithms were developed by Phil Katz (the late
	/// inventor of .zip files, and the PK in PKZIP) and the .zip file directory
	/// contains, for each file (among other fields), one that specifies the algorithm 
	/// used to compress it.</para>
	/// <para>
	/// For example, a small file, when compressed, might actually be larger than the
	/// input data, given the additional data needed to allow the data to be 
	/// decompressed. In that case, the data is merely stored in the .zip file as-is
	/// and its compression type is set to not-compressed.</para>
	/// <para>
	/// So basically, a .zip file is header (the directory) and footer information, with
	/// zero or more files inside, compressed according to several possible algorithms. 
	/// However this class does <b>not</b> create headers or footers. You'd need another
	/// class that called this one to process .zip files.
	/// </para>
	/// </summary>
	public class BartCompress {

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Compresses a string according to the standard PKZIP Deflate algorithm, and
		/// returns the compressed version of the string.
		/// </summary>
		/// <param name="source">The string to be compressed.</param>
		/// <returns>The compressed version of the string.</returns>
		public static string Compress(string source) {
			MemoryStream msIn = new MemoryStream(Encoding.Default.GetBytes(source));
			MemoryStream msOut = new MemoryStream();
			Compress(msIn, msOut);
			string s = Encoding.Default.GetString(msOut.ToArray());
			return s;
		}

//---------------------------------------------------------------------------------------

		public static byte[] Compress(byte[] source) {
			MemoryStream msIn = new MemoryStream(source);
			MemoryStream msOut = new MemoryStream();
			Compress(msIn, msOut);
			byte [] bytes = msOut.ToArray();
			return bytes;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Compresses data from a Stream, according to the standard PKZIP Deflate 
		/// algorithm, and sends the compressed version of the data to an output Stream.
		/// </summary>
		/// <param name="source">An input stream with data to be compressed.</param>
		/// <param name="destination">An output stream which receives the compressed
		/// version of the data.
		/// </param>
		public static void Compress(Stream source, Stream destination) {
			// We must explicitly close the output stream, or GZipStream will not
			// write the compression's footer to the file.  So we'll get a file, but
			// we won't be able to decompress it.  We'll get back 0 bytes.
			using (GZipStream output = new GZipStream(destination, CompressionMode.Compress, true)) {
				Pump(source, output);
			}
		}

//---------------------------------------------------------------------------------------

		public static string Decompress(string source) {
			MemoryStream msIn = new MemoryStream(Encoding.Default.GetBytes(source));
			MemoryStream msOut = new MemoryStream();
			Decompress(msIn, msOut);
			byte[] temp = msOut.ToArray();
			string s = Encoding.Default.GetString(temp);
			s = s.Substring(0, (int)msOut.Length);
			return s;
		}

//---------------------------------------------------------------------------------------

		public static byte[] Decompress(byte[] source) {
			MemoryStream msIn = new MemoryStream(source);
			MemoryStream msOut = new MemoryStream();
			Decompress(msIn, msOut);
			byte[] bytes = msOut.ToArray();
			return bytes;
		}

//---------------------------------------------------------------------------------------

		public static void Decompress(Stream source, Stream destination) {
			using (GZipStream input = new GZipStream(source, CompressionMode.Decompress)) {
				Pump(input, destination);
			}
		}

//---------------------------------------------------------------------------------------

		private static void Pump(Stream input, Stream output) {
			byte[] bytes = new byte[4096];
			int n;
			while ((n = input.Read(bytes, 0, bytes.Length)) != 0) {
				output.Write(bytes, 0, n);
			}
		}
	}
}
