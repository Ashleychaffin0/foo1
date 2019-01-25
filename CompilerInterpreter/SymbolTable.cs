using System.Collections.Generic;

// TODO: Add XML comments to each method

namespace MyDumbInterpreter {
	class SymbolTable {
		Dictionary<string, Symbol> Symtab;

//---------------------------------------------------------------------------------------

		public SymbolTable() {
			Symtab = new Dictionary<string, Symbol>();
		}

//---------------------------------------------------------------------------------------

		internal (bool bExists, Symbol Sym) GetSym(string s) {
			bool bExists = Symtab.TryGetValue(s, out Symbol Sym);
			return (bExists, Sym);
		}

//---------------------------------------------------------------------------------------

		internal (bool bExists, Symbol sym) AddSym(char Name, Symbol.SymbolType SymType = Symbol.SymbolType.Name) {
			return AddSym(Name.ToString(), SymType);
		}

//---------------------------------------------------------------------------------------

		internal (bool bExists, Symbol sym) AddSym(string Name, Symbol.SymbolType SymType = Symbol.SymbolType.Name) {
			var sym = GetSym(Name);
			if (sym.bExists) {
				return (true, sym.Sym);
			}
			var NewSym = new Symbol(Name, SymType);
			Symtab[Name] = NewSym;
			return (false, NewSym);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// See Interpreter.ScanForLabels for the need for this routine
		/// </summary>
		internal void RemoveAllLabels() {
			foreach (var KVPair in Symtab) {	// Key/Value Pair
			var sym = KVPair.Value;
				if (sym.SymType == Symbol.SymbolType.Label) {
					Symtab.Remove(KVPair.Key);
				}
			}
		}

//---------------------------------------------------------------------------------------

		internal bool AddLabel(char lbl, int IP) {
			string LblName = lbl.ToString();
			var sym = GetSym(LblName);
			if (sym.bExists) return true;
			var NewSym = new Symbol(LblName, Symbol.SymbolType.Label);
			NewSym.Value = IP;
			Symtab[LblName] = NewSym;
			return false;
		}

//---------------------------------------------------------------------------------------

		internal (bool bExists, Symbol Sym) GetValue(string Name) {
			var SymInfo = GetSym(Name);
			if (SymInfo.bExists) return (true, SymInfo.Sym);
			return (false, null);
			}
	}
}
