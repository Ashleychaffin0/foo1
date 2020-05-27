using System;

namespace LrsColinAssembler {
	public class OpcodeDef {
		public byte					Op;
		public ushort				Length;
		public byte					NumberOfArgs;
		public Action<ParsedLine>	Process;
		public Action				Exec;			// Execution routine

//---------------------------------------------------------------------------------------

		public OpcodeDef(
				byte				op,
				byte				length,
				byte				numberOfArgs,
				Action<ParsedLine>	process) {
			Op           = op;
			Length       = length;
			NumberOfArgs = numberOfArgs;
			Process      = process;
		}
	}
}
