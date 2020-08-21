using System;

namespace LrsColinAssembler {
	public class OpcodeDef {
		public byte						Op;				// Numeric opcode
		public ushort					Length;			// # of bytes in this instruction
		public byte						NumberOfArgs;	// How many arguments the opcode has
		public Predicate<ParsedLine>	Process;		// How to process the opcode

//---------------------------------------------------------------------------------------

		public OpcodeDef(
				byte					op,
				byte					length,
				byte					numberOfArgs,
				Predicate<ParsedLine>	process) {
			Op           = op;
			Length       = length;
			NumberOfArgs = numberOfArgs;
			Process      = process;
		}
	}
}
