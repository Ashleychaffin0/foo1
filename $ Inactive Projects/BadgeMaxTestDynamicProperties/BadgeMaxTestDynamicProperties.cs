// #define BADGEMAXFIELD

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

using IronPython.Hosting;
using IronPython.Compiler;

namespace BadgeMaxTestDynamicProperties {
	public partial class BadgeMaxTestDynamicProperties : Form {

		PythonEngine ScriptEngine;
		Dictionary<string, object> locals = new Dictionary<string, object>();
		EngineModule mod;

//---------------------------------------------------------------------------------------

		public BadgeMaxTestDynamicProperties() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void BadgeMaxTestDynamicProperties_Load(object sender, EventArgs e) {
			EngineOptions opts = new EngineOptions();
			opts.ClrDebuggingEnabled = true; 
			ScriptEngine = new PythonEngine(opts);
			ScriptEngine.AddToPath(Path.GetDirectoryName(Application.ExecutablePath));
			mod = ScriptEngine.CreateModule("__main__", true);
			// ScriptEngine.DefaultModule = mod;

#if false		// Stuff you might want to do during scripting initialization
			ScriptEngine.Import("Site");
			// Add global variables to the scripting namespace
			ScriptEngine.Globals["dateentries"] = dateEntries;
#endif
			Dictionary<string, string> SwipeData = new Dictionary<string, string>();
			SwipeData["FName"] = "Larry";
			SwipeData["LName"] = "Smith";

#if BADGEMAXFIELD
			BadgeMaxField fld = new BadgeMaxField();
			fld.FieldName = "FirstName";
			fld.Data = "Larry";
#endif
#if false				
			Dictionary<string, object> globals = new Dictionary<string, object>();
			globals["SwipeData"] = SwipeData;
			ScriptEngine.Globals["BadgeData"] = globals;
#endif

			string filename = "LRSDynPropTest.py";
			string dirname = FindFileUpInHierarchy(filename);	// Assume found
			string fullname = Path.Combine(dirname, filename);

			locals["SwipeData"] = SwipeData;
#if BADGEMAXFIELD
			locals["BMField"] = fld;
#endif

#if false		// Common Library doesn't exist in this program
			string CommLibName = "CommonLibrary.dll"; // See comments below about '.'
			string commlibdir = FindFileUpInHierarchy(CommLibName);
			locals["CommonLibraryName"] = CommLibName;
			// The following line doesn't work, since the pathname has a '.' in it,
			// and import r"C:\LRS\$BadgeMax Devel\BadgeMax 1.2 ..." has that pesky
			// '.' inside '1.2'. So it's taking it as import x.y.
			// locals["CommonLibraryName"] = Path.Combine(commlibdir, CommLibName);
#endif

			// ExecutablePath added to sys.path above
			// mod.Import(Path.GetFileName(Application.ExecutablePath));
			ScriptEngine.ExecuteFile(fullname, mod, locals);
			// object omod = mod.Import(filename);
			// ScriptEngine.DefaultModule = mod;

			// mod.Import(Path.Combine(commlibdir, commlib));
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			if (ScriptEngine != null) {			// TODO:
				try {
					string filename = "LRSDynPropTest.py";
					string dirname = FindFileUpInHierarchy(filename);	// Assume found
					string fullname = Path.Combine(dirname, filename);
					Console.WriteLine("About to compile and execute code");
					CompiledCode code = ScriptEngine.CompileFile(fullname);
					code.Execute(mod, locals);
					Console.WriteLine("Back from compiling and executing code");
#if false
					// ScriptEngine.Execute("print dir(CommonLibrary)", mod);
					ScriptEngine.Execute("import sys");
					object o = ScriptEngine.Evaluate("dir()");
					ScriptEngine.ExecuteToConsole("dir(sys.modules)");
					ScriptEngine.ExecuteToConsole("LRSSub()", mod, locals);
					ScriptEngine.ExecuteToConsole("LRSSub2(5, 'asdf', '31415', 42)", mod, locals);
#endif
				} catch (IronPython.Runtime.Exceptions.PythonNameErrorException exc) {
					MessageBox.Show("IronPython engine complained: " + exc.Message);
				} catch (Exception exc) {
					MessageBox.Show("Unexpected IronPython exception: " + exc.ToString());
				}
			}
		}

//---------------------------------------------------------------------------------------

		public static string FindFileUpInHierarchy(string filename) {
			// TODO: Need overload - with and without default starting directory
			string path = Application.StartupPath + @"\";
			bool bFound = false;

			// TODO: Not 16. Should stop when it gets to merely a drive letter
			for (int i = 0; i < 16; ++i) {				// Sixteen levels should be enough!
				if (File.Exists(path + filename)) {
					bFound = true;
					break;
				} else {
					filename = @"..\" + filename;
				}
			}
			if (!bFound) {
				return null;
			}
			FileInfo fi = new FileInfo(path + filename);
			return fi.DirectoryName;
		}
	}
}