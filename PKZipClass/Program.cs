using System;
using System.IO;
using System.Text;

namespace PKZipClass {
	class Program {
		static void Main(string[] args) {
			var zip = new LocalHeader(@"G:\LRS\$People\Colin\Archive.zip");
		}
	}

	public class LocalHeader {
		public readonly string	Signature;
		public readonly ushort	MinExtractionVersion;
		public readonly ushort	Flags;
		public readonly ushort	CompressionMethod;
		public readonly ushort	LastModificationTime;
		public readonly ushort	LastModificationDate;
		public readonly uint	CRC32;
		public readonly uint    CompressedSize;
		public readonly uint    UncompressedSize;
		public readonly ushort  FilenameLength;
		public readonly ushort  ExtraFieldLength;
		public readonly string  Filename;
		public readonly byte[]  ExtraField;
		// Extra field follows filename

//---------------------------------------------------------------------------------------

		public LocalHeader(string filename) {
			using var rdr        = new BinaryReader(File.OpenRead(filename));
			var sig              = rdr.ReadBytes(4);
			Signature            = Encoding.ASCII.GetString(sig);
			MinExtractionVersion = rdr.ReadUInt16();
			Flags                = rdr.ReadUInt16();
			CompressionMethod    = rdr.ReadUInt16();
			LastModificationTime = rdr.ReadUInt16();
			LastModificationDate = rdr.ReadUInt16();
			CRC32                = rdr.ReadUInt32();
			CompressedSize       = rdr.ReadUInt32();
			UncompressedSize     = rdr.ReadUInt32();
			uint filenameLength  = rdr.ReadUInt16();
			ExtraFieldLength     = rdr.ReadUInt16();
			var fn               = rdr.ReadBytes(FilenameLength);
			Filename             = Encoding.ASCII.GetString(fn);
			ExtraField           = rdr.ReadBytes(ExtraFieldLength);
			string x = Encoding.ASCII.GetString(ExtraField);
		}
	}
}
