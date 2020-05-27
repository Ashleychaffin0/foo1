using System;

namespace LrsColinAssembler {
	internal partial class Interpreter {
		[Flags]
		enum CondCode : byte {      // Condition Code
			None  = 0x00,
			High  = 0x02,			// a > b
			Low   = 0x04,			// a < b
			Equal = 0x08			// a == b (or a == 0)
		}
	}
}