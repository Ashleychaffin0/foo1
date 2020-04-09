//using System.Data;
using System.Windows.Forms;

namespace LRSAntlr {
	class TargetLanguage {
		// C++ target: https://github.com/antlr/antlr4/blob/master/doc/cpp-target.md
		public string	Language;         // e.g. "CSharp"
		public CheckBox ChkBox;
		public string	CompilerString;     // e.g. "java.exe", "csc.exe" + parms

//---------------------------------------------------------------------------------------

		public TargetLanguage(string Language, CheckBox ChkBox, string CompilerString) {
			this.Language       = Language;
			this.ChkBox         = ChkBox;
			this.CompilerString = CompilerString;
		}
	}
}
