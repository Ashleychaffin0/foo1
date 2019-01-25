using System;
using System.IO.MemoryMappedFiles;

namespace DumpPdb {
	class BasedView {
		MemoryMappedViewAccessor BaseView;
		int                      BaseOffset;
		int                      CurrentOffset;

//---------------------------------------------------------------------------------------

		public BasedView(MemoryMappedViewAccessor BaseView, int BaseOffset) {
			this.BaseView   = BaseView;
			this.BaseOffset = BaseOffset;
			CurrentOffset = 0;
		}

//---------------------------------------------------------------------------------------

		public byte ReadByte() {
			byte val = BaseView.ReadByte(BaseOffset + CurrentOffset);
			++CurrentOffset;
			return val;
		}

		//---------------------------------------------------------------------------------------

		public int ReadInt() {
			int val = BaseView.ReadInt32(BaseOffset + CurrentOffset);
			CurrentOffset += 4;
			return val;
		}

//---------------------------------------------------------------------------------------

		public int ReadInt(int Offset) {
			int val = BaseView.ReadInt32(BaseOffset + Offset);
			CurrentOffset += 4;
			return val;
		}

//---------------------------------------------------------------------------------------

		public void IncrementCurrentOffset(int Increment) {
			CurrentOffset += Increment;
		}

//---------------------------------------------------------------------------------------

		public void SetOffset(int Offset) {
			CurrentOffset = Offset;
		}
	}
}
