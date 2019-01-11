using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotQuiteSoDumbInterpreter {
	class Token {

		public Token() {
			// TODO:
		}

//---------------------------------------------------------------------------------------

		public enum Type : byte {
			Unknown,
			Name,
			Numeric,
			Operator,
			Label
		}
	}

	class TokenName : Token {
		// TODO:
	}
}
