namespace LrsColinAssembler {
	public class Opcode {
		public byte			Opcode;
		public byte			Length;
		// public byte			Register;
		// public byte			IndexRegister;
		// public uint			Address;
		public OpcodeFlags	Flags;

//---------------------------------------------------------------------------------------

		public Opcode(byte opcode, byte length, OpcodeFlags flags = 0) {
			Opcode = opcode;
			Length = length;
			flags  = flags;
		}
	}
}
