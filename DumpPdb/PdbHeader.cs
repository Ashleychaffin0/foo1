using System;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace DumpPdb {
	internal class PdbHeader {		// aka SuperBlock
		public StringBuilder HeaderSignatureText;
		public byte[]        HeaderSignatureBinary;
		public int           PageSize;
		public int           FreeBlockMapBlock;
		public int           NumPages;
		public int           NumDirectoryBytes;
		public int           Unknown;
		public int           BlockMapAddr;

//---------------------------------------------------------------------------------------

		public PdbHeader(BasedView view) {
			view.SetOffset(0);

			const int HeaderTextLength = 26;
			HeaderSignatureText = new StringBuilder(HeaderTextLength);
			for (int i = 0; i < HeaderTextLength; i++) {
				byte b = view.ReadByte();
				HeaderSignatureText.Append(Convert.ToChar(b));
			}
			const int HeaderBinaryLength = 6;
			HeaderSignatureBinary = new byte[6];
			for (int i = 0; i < HeaderBinaryLength; i++) {
				HeaderSignatureBinary[i] = view.ReadByte();
			}
			// TODO: Check that the two signatures are valid

			PageSize          = view.ReadInt();
			FreeBlockMapBlock = view.ReadInt();
			NumPages          = view.ReadInt();
			NumDirectoryBytes = view.ReadInt();
			Unknown           = view.ReadInt();
			BlockMapAddr      = view.ReadInt();
		}

//---------------------------------------------------------------------------------------

		// Converts a block number to an offset into the file
		public int BlockAddress(int BlockNum) => BlockNum * PageSize;
	}
}