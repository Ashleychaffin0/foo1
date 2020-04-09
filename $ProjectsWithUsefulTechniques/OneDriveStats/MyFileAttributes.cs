using System.IO;

namespace OneDriveStats {
	internal enum MyFileAttributes {
		Archive           = FileAttributes.Archive,				// 0x0020
		Compressed        = FileAttributes.Compressed,			// 0x0800
		Device            = FileAttributes.Device,				// 0x0040
		Directory         = FileAttributes.Directory,			// 0x0010
		Encrypted         = FileAttributes.Encrypted,			// 0x4000
		Hidden            = FileAttributes.Hidden,				// 0x0002
		IntegriyStream    = FileAttributes.IntegrityStream,		// 0x8000
		Normal            = FileAttributes.Normal,				// 0x0080
		NoScrubData       = FileAttributes.NoScrubData,			// 0x0002_0000
		NotContentIndexed = FileAttributes.NotContentIndexed,	// 0x2000
		Offline           = FileAttributes.Offline,				// 0x1000
		ReadOnly          = FileAttributes.ReadOnly,			// 0x0001
		ReparsePoint      = FileAttributes.ReparsePoint,		// 0x0400
		SparseFile        = FileAttributes.SparseFile,			// 0x0200
		System            = FileAttributes.System,				// 0x0004
		Temporary         = FileAttributes.Temporary,			// 0x0100

		// LRS Added. Note that we do it this way since enum's can't be inherited. Do
		// a search for <C# enum inheritance> for the rationale.
		Unpinned      = 0x0010_0000,
		RecallOnDataAccess = 0x0040_0000
	}
}
