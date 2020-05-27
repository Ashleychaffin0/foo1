using System;

public class InvalidOpcodeException : Exception {
	public string Opcode;

//---------------------------------------------------------------------------------------

	public InvalidOpcodeException(string opcode) => Opcode = opcode;
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

public class UnknownSymbolException : Exception {
	public string Symbol;

//---------------------------------------------------------------------------------------

	public UnknownSymbolException(string symbol) => Symbol = symbol;
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

public class InvalidNumberOfArgsException : Exception {
	public int ExpectedNumberOfArgs;
	public int NumberOfArgs;

//---------------------------------------------------------------------------------------
	public InvalidNumberOfArgsException(int expectedNumberOfArgs, int numberOfArgs) {
		ExpectedNumberOfArgs = expectedNumberOfArgs;
		NumberOfArgs         = numberOfArgs;
	}
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

public class DuplicateLabelException : Exception {
	public string Label;

//---------------------------------------------------------------------------------------

	public DuplicateLabelException(string label) => Label = label;
}
