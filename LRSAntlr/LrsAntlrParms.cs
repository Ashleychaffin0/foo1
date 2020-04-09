using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils;

namespace LRSAntlr {
	public class LrsAntlrParms {
		// TODO: Derive from a generic base class that has Load/Save implemented.

		// Load/Save thyself
		public string ParmsFilename;

		// Antlr parms
		public bool		Listener;
		public bool		Visitor;

		// Other config parms
		public string	TargetDir;
		public bool		CompileAsJava;
		public bool		CompileAsCSharp;
		public bool		CompileAsPython3;
		public bool		CompileAsPython2;
		public bool		CompileAsCpp;

		// TODO: Not yet implemented
		public bool		CompileAsJavaScript;
		public bool		CompileAsGo;
		public bool		CompileAsSwift;

		// UI options
		// TODO: Next line must be some enum, not bool
		// public bool UIOption;		// Gui, Console, DLL

//---------------------------------------------------------------------------------------

		public LrsAntlrParms() {
			// Empty ctor needed for serialization
		}

//---------------------------------------------------------------------------------------

		public LrsAntlrParms(string ParmsFilename) {
			this.ParmsFilename = ParmsFilename;
		}

//---------------------------------------------------------------------------------------

		public LrsAntlrParms Load() {
			return  GenericSerializer<LrsAntlrParms>.Load(ParmsFilename);
		}

//---------------------------------------------------------------------------------------

		public void Save(string ParmsFilename) {
			GenericSerializer<LrsAntlrParms>.Save(ParmsFilename, this);
		}

//---------------------------------------------------------------------------------------

		public void Save() {
			GenericSerializer<LrsAntlrParms>.Save(ParmsFilename, this);
		}
	}
}
