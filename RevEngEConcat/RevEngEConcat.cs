using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// TODO: This approach doesn't handle the case of comments associated with globals

/* Our approach will be as follows:
	*	Enumerate all the files in the specified directory
	*	Go through each file, parsing each line as either #include, #define (globals),
		struct or other. Save these for later (consolidating duplicates). Strip the
		#include's, #define's, typedef's, struct'sand maybe others, for later
		consolidation.
	*	Write out the function bodies to a temp file (or maybe even keep them in memory
		in a Dictionary<string FunctionName, string BodyOfFunction, so we can emit the
		functions in alphmeric order
	*	Write out the #include's, #define's, struct's and others.
	*	Write out each function definition, alphmerically
*/

namespace RevEngEConcat {
	public partial class RevEngEConcat : Form {

		string ProjectDir;				// So we don't save the master .c file in the
										// same directory as the individual .c files

		// Note: Includes *must* be a List, not a Dictoinary. Some #include's have
		//		 dependencies on previous #include's and must be in order. Note that this
		//		 isn't a foolproof algorithm, but it's better than nothing.
		List<string>					Includes;
		Dictionary<string, string>		Globals;
		// Again, a typedef might refer back to another typedef, so we need them in order
		List<string>					TypeDefs;
		Dictionary<string, Function>	Functions;

		Function CurrentFunction;   // In current file.
									// To distinguish before and after fn defn

		// Every line before the prototype that we don't recognize
		List<string>					Prefix;

		// Hold all lines afer function prototype, on a file-by-file basis
		List<string>					FunctionBody;

		// Regular expressions
		Regex	reInclude = new Regex(@"^
			\#include\s+
			(?<IncludeName>.*)$
", 
			RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

//-------

		Regex reGlobals = new Regex(@"^
					\#define\s+(?<GlobalName>(\w | _ | -)+)
					\s+
					(?<GlobalDefn>.*$)
", 
			RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

//-------

		Regex reTypedefs = new Regex(@"^
					typedef\s+
					(?<TypeDefn>.*$)
", 
				RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

		Regex reFunction = new Regex(@"^		# Let's start at the very beginning
												# A very good place to start
					(?<RetType>\w+)
					\s+
					(?<FnName>(\w | _ | -)+)	# Kludge: Allow hyphens in fn names
					\s*							# Maybe blanks before opening paren
					(?<FnPrototype>.*$)
",
				RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

//---------------------------------------------------------------------------------------

		public RevEngEConcat() {
			InitializeComponent();

			Includes  = new List<string>();
			Globals   = new Dictionary<string, string>();
			TypeDefs  = new List<string>();
			Functions = new Dictionary<string, Function>();
			Prefix    = new List<string>();

			// These are on a per-file basis
			FunctionBody = new List<string>();
		}

//---------------------------------------------------------------------------------------

		private void ParseTheFiles(string ProjectDir) {
			foreach (var file in Directory.EnumerateFiles(ProjectDir, "*.c")) {
				CurrentFunction = null;
				FunctionBody = new List<string>();
				bool bMatch;
				using (var sr = new StreamReader(file)) {
					string line;
					while ((line = sr.ReadLine()) != null) {
						if (CurrentFunction != null) {
							FunctionBody.Add(line);
						} else {
							bMatch = MatchInclude(line);
							if (! bMatch) bMatch |= MatchGlobal(line);
							if (! bMatch) bMatch |= MatchTypedef(line);
							if (! bMatch) bMatch |= MatchFunction(line);
							if (! bMatch) {
								// Found something before prototype we didn't recognize
								bool bFound = Prefix.Any(text => text == line);
								if (!bFound) {
									Prefix.Add(line);
								}
							}
						}
					}
				}
				if (CurrentFunction == null) {
					MessageBox.Show($"Could not find a function definition in {Path.GetFileName(file)}", "DANGER WILL ROBINSON!",
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
				} else {
					CurrentFunction.Body   = FunctionBody;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private bool MatchInclude(string line) {
			Match m = reInclude.Match(line);
			if (m.Success) {
				string IncludeName = m.Groups["IncludeName"].Value;
				bool bFound = Includes.Any(name => name == IncludeName);
				if (!bFound) {
					Includes.Add(IncludeName);
				}
			}
			return m.Success;
		}

//---------------------------------------------------------------------------------------

		private bool MatchGlobal(string line) {
			Match m = reGlobals.Match(line);
			if (m.Success) {
				string GlobalName = m.Groups["GlobalName"].Value;
				string GlobalDefn = m.Groups["GlobalDefn"].Value;
				// Note: We just check if we've seen this name before. If there's a
				//		 different definition (or even comment), then tough...
				bool bFound = Globals.ContainsKey(GlobalName);
				if (! bFound) {
					Globals[GlobalName] = GlobalDefn;
				}
			}
			return m.Success;
		}

//---------------------------------------------------------------------------------------

		private bool MatchTypedef(string line) {
			Match m = reTypedefs.Match(line);
			if (m.Success) {
				string Typedefn = m.Groups["TypeDefn"].Value;
				// Note: We just check if we've seen this name before. If there's a
				//		 different definition (or even comment), then tough...
				bool bFound = TypeDefs.Any(defn => defn == Typedefn);
				if (! bFound) {
					TypeDefs.Add(Typedefn);
				}
			}
			return m.Success;
		}

//---------------------------------------------------------------------------------------

		private bool MatchFunction(string line) {
			Match m = reFunction.Match(line);
			if (m.Success) {
				string RetType     = m.Groups["RetType"].Value;
				string FnName      = m.Groups["FnName"].Value;
				string FnPrototype = m.Groups["FnPrototype"].Value;
				// Bit of a kludge here. To avoid considering, say, "struct regs myregs"
				// as a function definition, we'll check that the first character of the
				// prototype is '('. If not, it's not a function.
				if (FnPrototype[0] != '(') {
					return false;
				}
				bool bFound = Functions.ContainsKey(FnName);
				if (!bFound) {
					CurrentFunction   = new Function(RetType, FnName);
					Functions[FnName] = CurrentFunction;
					FunctionBody.Add(line);
				}
			}
			return m.Success;
		}

//---------------------------------------------------------------------------------------

		private void mnuOpenProjectToolStripMenuItem_Click(object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog();
			fbd.Description = "Choose the directory where the *.c files are";
#if DEBUG
			fbd.SelectedPath = @"D:\Program Files (x86)\RevEngE6502\jobs\zol";
#endif
			var result = fbd.ShowDialog();
			if (result == DialogResult.OK) {
				ProjectDir = fbd.SelectedPath;
				ParseTheFiles(ProjectDir);
			}
		}

//---------------------------------------------------------------------------------------

		private void mnuSaveConsolidatedcFileToolStripMenuItem_Click(object sender, EventArgs e) {
			if (ProjectDir == null) {
				MessageBox.Show("Can't save. No project opened yet.", "RevEngE",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			var ofd             = new OpenFileDialog();
			ofd.CheckFileExists = false;
			ofd.CheckPathExists = true;
			ofd.DefaultExt      = ".c";
			ofd.Title           = "Set the name of the consolidated .c file";
			var res             = ofd.ShowDialog();
			if (res == DialogResult.Cancel) {
				return;
			}

			using (var sw = new StreamWriter(ofd.FileName)) {
				CreateConsolidatedCFile(sw);
				// System.Diagnostics.Process.Start("notepad++.exe", ofd.FileName);
			}
		}

//---------------------------------------------------------------------------------------

		private void CreateConsolidatedCFile(StreamWriter sw) {
			// Pump out #includes
			sw.WriteLine("/* Includes */");
			sw.WriteLine();
			foreach (var include in Includes) {
				sw.WriteLine($"#include {include}");
			}
			sw.WriteLine();

			// Pump out Globals
			var GlobalKeys = Globals.Keys.OrderBy(key => key);
			// Make initial pass, count the # of chars in each global, then write them
			// out with their values aligned

			int maxlen = 0;
			foreach (var key in GlobalKeys) {
				maxlen = Math.Max(maxlen, key.Length);
			}

			sw.WriteLine("/* Globals */");
			sw.WriteLine();
			foreach (var key in GlobalKeys) {
				string PaddedKey = key.PadRight(maxlen);
				sw.WriteLine($"#define {PaddedKey} {Globals[key]}");
			}
			sw.WriteLine();

			// Now Typedefs. These must stay in order
			sw.WriteLine("/* typedefs */");
			sw.WriteLine();
			foreach (var td in TypeDefs) {
				sw.WriteLine($"typedef {td}");
			}
			sw.WriteLine();

			// Prefix time
			sw.WriteLine("/* Other stuff before the functions */");
			sw.WriteLine();
			foreach (var pref in Prefix) {
				sw.WriteLine(pref);
			}
			sw.WriteLine();

			// We don't know what order to put the Functions in, so output prototypes
			var FnKeys = Functions.Keys.OrderBy(key => key);
			sw.WriteLine("/* Function Prototypes */");
			sw.WriteLine();
			foreach (var key in FnKeys) {
				var fn = Functions[key];
				string ProtoType = fn.Body[0];
				// I *could* just delete the last char of this string, which should just
				// be '{'. But just in case there's something after it (even just a
				// blank), I'll replace it with the empty string
				ProtoType = ProtoType.Replace("{", "").Trim() + ";";
				sw.WriteLine(ProtoType);
			}
			sw.WriteLine();

			// Finally the functions

			sw.WriteLine("/* Functions */");
			sw.WriteLine();
			string BoxTopBottom = "/****************************************************************************************/";
			string BoxMiddle    = "/*                                                                                      */";
			foreach (var key in FnKeys) {
				var fn = Functions[key];
				int DividerLength = BoxTopBottom.Length;
				string FnName = " " + fn.FunctionName + " ";
				int FnNameLength = FnName.Length;
				int InsertionPoint = (DividerLength - FnNameLength) / 2;
				sw.WriteLine(BoxTopBottom);
				string div = BoxMiddle.Substring(0, InsertionPoint)
					+ FnName
					+ BoxMiddle.Substring(InsertionPoint + FnNameLength);
				sw.WriteLine(div);
				sw.WriteLine(BoxTopBottom);
				sw.WriteLine();
				foreach (var line in fn.Body) {
					sw.WriteLine(line);
				}
				sw.WriteLine();
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class Function {
		public string		RetType;	// Return type of function (e.g. void, int, etc)
		public string		FunctionName;	// Name of the function itself
		public List<string>	Body;       // Include function definition line

//---------------------------------------------------------------------------------------

		public Function(string RetType, string FunctionName) {
			this.RetType      = RetType;
			this.FunctionName = FunctionName;
			// Body of function to be filled in later
		}
	}	 
}
