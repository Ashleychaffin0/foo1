using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolTableSample {
	internal class Class {
		// TODO:
	}

	internal class Procedure {
		private string _ProcName;
		public string ProcName {
			get { return _ProcName; }
			set { _ProcName = value; }
		}
		private List<Variable> Parameters;
		private List<Variable> Locals;
		// TODO:
	}

	internal class Variable {
		private string _Name;
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		SymbolTable.DataTypeValue VariableType;
		// TODO:
	}
}
