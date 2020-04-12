
using System;

namespace LrsColinAssembler {
	[Flags]
	public enum OpcodeFlags {
		None = 0b0000,
		DI = 0b0001,
		DS = 0b0010
	}
}
