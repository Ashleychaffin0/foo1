namespace LrsColinAssembler {
	public class Opcode {
		public byte Op;
		public byte Length;
		// public byte			Register;
		// public byte			IndexRegister;
		// public uint			Address;
		public OpcodeFlags Flags;

		//---------------------------------------------------------------------------------------

		public Opcode(byte op, byte length, OpcodeFlags flags = 0) {
			Op     = op;
			Length = length;
			Flags  = flags;
		}
	}
}
