namespace MyDumbInterpreter {
	internal partial class Symbol {
		internal string		Name;
		internal SymbolType	SymType;
		internal int?		Value;		// Line number if label, length if array?

//---------------------------------------------------------------------------------------

		public Symbol(string ID, SymbolType SymType = SymbolType.Name) {
			Name         = ID;
			this.SymType = SymType;
		}
	}
}