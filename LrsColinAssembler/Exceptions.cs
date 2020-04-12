using System;

public class InvalidOpcodeException : Exception {
	public string Opcode;

//---------------------------------------------------------------------------------------

	public InvalidOpcodeException(string opcode) {
		Opcode = opcode;
	}
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

public class UnknownSymbolException : Exception {
	public string Symbol;

//---------------------------------------------------------------------------------------

	public UnknownSymbolException(string symbol) {
		Symbol = symbol;
	}
}
