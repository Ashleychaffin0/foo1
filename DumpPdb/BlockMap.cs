using System.IO.MemoryMappedFiles;

namespace DumpPdb {
	internal class BlockMap {
		public int[] Blocks;

//---------------------------------------------------------------------------------------

		public BlockMap(BasedView view, int ofs, PdbHeader hdr) {
			view.SetOffset(ofs);
			var nBlocks = (hdr.NumDirectoryBytes + hdr.PageSize - 1) / hdr.PageSize;
			Blocks = new int[nBlocks];
			for (long i = 0; i < nBlocks; i++) {
				Blocks[i] = view.ReadInt();
			}
		}
	}
}