using System;
using System.IO.MemoryMappedFiles;

// Note: May need Buffer.BlockCopy 

namespace DumpPdb {
	internal class StreamDirectory {
		int[][] StreamBlocks;

//---------------------------------------------------------------------------------------

		public StreamDirectory(BasedView view, BlockMap blockmap, PdbHeader hdr) {
			view.SetOffset(hdr.BlockAddress(blockmap.Blocks[0]));
			int NumStreams = view.ReadInt();;
			var StreamSizes = new int[NumStreams];
			for (int i = 0; i < NumStreams; i++) {
				StreamSizes[i] = view.ReadInt();
			}

			StreamBlocks = new int[NumStreams][];
			var nTotBlocks = 0;
			for (int i = 0; i < NumStreams; i++) {
				var NumBlocks   = (int)((StreamSizes[i] + hdr.PageSize - 1) / hdr.PageSize);
				nTotBlocks     += NumBlocks;
				StreamBlocks[i] = new int[NumBlocks];
				for (int j = 0; j < NumBlocks; j++) {
					StreamBlocks[i][j] = view.ReadInt();
				}
			}

			// StreamDir = new byte[nTotBlocks * hdr.BlockSize];
			for (int i = 0; i < NumStreams; i++) {
				var addr = hdr.BlockAddress(StreamBlocks[i][0]);
				var sig  = view.ReadInt(addr);
				switch (sig) {
					case (int)PdbStreamVersion.VC70:
					case (int)PdbStreamVersion.VC90:
						var dt  = DateTimeOffset.FromUnixTimeSeconds(view.ReadInt());
						var age = view.ReadInt();
						Console.WriteLine($"Dir {i} at {addr:x8}, sig: {sig:x8}/{sig}, Age = {age}, date = {dt}");
						break;
					case -1:
						// Unused stream. Ignore
						break;
					default:
						Console.WriteLine($"Dir {i} at {addr:x8} -- Unknown sig: {sig:x8}/{sig}");
						break;
				}
			}
		}
	}

	enum PdbStreamVersion : int {
		VC2		= 19941610,             // 0x013048ea
		VC4     = 19950623,             // 0x01306c1f
		VC41    = 19950814,             // 0x01306cde
		VC50    = 19960307,             // 0x013091f3
		VC98    = 19970604,             // 0x0130ba2c
		VC70Dep = 19990604,             // 0x0131084c
		VC70    = 20000404,             // 0x01312e94
		VC80    = 20030901,             // 0x0131a5b5
		VC90	= 20040203,				// 0x0131ca0b	// Maybe VC90?
		VC110   = 20091201,             // 0x01329141
		VC140   = 20140508              // 0x013351dc
	}
}