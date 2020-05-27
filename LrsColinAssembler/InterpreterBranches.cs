namespace LrsColinAssembler {
	internal partial class Interpreter {
		private void Exec_Branches() {                // 0x47 -- All branches
			(byte CCMask, _) = DecodeRegs(PC + 1);
			short address    = DecodeAddress(PC + 2);
			if ((CCMask & (byte)CC) != 0) {
				PC = (ushort)address;
			} else {
				PC += 4;
			}
		}
	}
}