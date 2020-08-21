using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace LrsColinAssembler {

	// TODO: Lotsa comments
	public class ParsedLine {	
		public ushort		SourceAddress;
		public string		Source;
		public string		Mnemonic;
		public OpcodeDef	OpDef;
		public byte			Opcode;
		public ushort		Length;
		public List<string>	Parms;
		public string		DsText;
		public string		ErrorMessage;

//---------------------------------------------------------------------------------------
		
		public ParsedLine(ushort addr, string source) {
			SourceAddress    = addr;
			Source           = source;
			ErrorMessage     = "";
			DsText           = "";
			// Other fields filled in later
		}
	}
}
