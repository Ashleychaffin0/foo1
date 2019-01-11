using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;

using LRSUtils;

// TODO: Regex tolerates hypens in Function names
// TODO: It would be nice if Descriptions reformatted " => \" and \ => \\. Also on input.

namespace RevEngESymbols {
	public partial class RevEngESymbols : Form {

		Symbols		Symbols;

		string		ProjectDir;
		string		ParmsFilename;

		Regex		reFunction;
		Regex		reGlobal;
		Regex		reLocal;

		bool		bDirtyBit;

//---------------------------------------------------------------------------------------

		public RevEngESymbols() {
			InitializeComponent();

			ParmsFilename = "RevEngESymbols.xml";

			SetupRegexes();

			Symbols = new Symbols();
		}

//---------------------------------------------------------------------------------------

		private void SetupRegexes() {
			// TODO: Doesn't handle multiple parms properly

			// This re isn't totally accurate, for C, but close enough for what RevEngE
			// spits out
			reFunction = new Regex(@"^		# Let's start at the very beginning
											# A very good place to start
					(?<RetType>\w+)
					\s+
					(?<FnName>(\w | _ | -)+)	# Kludge: Allow hyphens in fn names
					\s*						# Maybe blanks before opening paren
					\(						# Opening paren of parm list
					(?<Parms>				# Start of (possibly empty) parm list
						(?<ParmType>\w+)
						\s+
						(?<ParmName>(\w | _)+)
# If we have more than 2 parms, we seem to get only the first and the last
						(\s*,\ *(?<ParmType2>\w+)\s+(?<ParmName2>(\w | _)+))*
					)*						# 0 or more parms
					\s*\)					# Closing paren of parm list
					\s*{					# Opening brace for the function
",
				RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

//-------

			reGlobal = new Regex(@"^
					\#define\s+(?<GlobalName>(\w | _)+)
					\s+
					\(\*\(
					(SC | UC | PUC | PPUC)
					\)
					(?<Address>\d+)
					\)
",
				RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

//-------

			reLocal = new Regex(@"^
					(int | SC | UC | PUC |PPUC)\ +
					(?<LocalName>(\w | _)+)
",
				RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
		}

//---------------------------------------------------------------------------------------

		private void WriteSymbols() {
			try {
				Stream s = File.OpenWrite(Path.Combine(ProjectDir, ParmsFilename)); 
				var x    = new XmlSerializer(typeof(Symbols));
				x.Serialize(s, Symbols);
				s.Close();
			} catch (Exception ex) {
				MessageBox.Show("Unable to write config file " + ParmsFilename + ", error = " + ex.Message, "RevEngE");
			}
		}

//---------------------------------------------------------------------------------------

		private void ReadSymbols() {
			string FullPath = Path.Combine(ProjectDir, ParmsFilename);
			if (File.Exists(FullPath)) {
				try {
					Stream s = File.OpenRead(FullPath);
					var x    = new XmlSerializer(typeof(Symbols));
					Symbols  = (Symbols)x.Deserialize(s);
					s.Close();
				} catch (Exception ex) {
					MessageBox.Show("Unable to read config file " + ParmsFilename + ", error = " + ex.Message, "RevEngE");
					Symbols = new Symbols();
				}
			} else {
				Symbols = new Symbols();
			}
		}

//---------------------------------------------------------------------------------------

		private void PopulateInitialSymbols() {
			// string SourceDir = @"D:\Program Files (x86)\RevEngE6502\jobs\zol";
			string SourceDir = ProjectDir;
			Function ThisFn = null;
			foreach (var file in Directory.GetFiles(SourceDir, "*.c")) {
				using (var src = new StreamReader(file)) {
					string line;
					while ((line = src.ReadLine()) != null) {
						Match mf = reFunction.Match(line);
						if (mf.Success) {
							ThisFn = MatchFunction(mf, file);
						} else {
							Match mg = reGlobal.Match(line);
							if (mg.Success) {
								MatchGlobal(mg);
							} else {
								Match ml = reLocal.Match(line);
								if (ml.Success) {
									MatchLocal(ml, ThisFn);
								}
							}
						}
					}
					if (ThisFn == null) {
						MessageBox.Show($"******** {file} -- Function not found", "RevEngE",
							MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void MatchGlobal(Match mg) {
			string GlobalName = mg.Groups["GlobalName"].Value;
			uint Address      = Convert.ToUInt32(mg.Groups["Address"].Value);
			var AllGlobals    = Symbols.Globals;
			Symbol g;
			if (AllGlobals.TryGetValue(GlobalName, out g)) {
				if (g.Address != Address) {
					// TODO: There's already a global for this name, but for a different
					//		 address. Something's screwy. May want to keep the filename
					//		 as part of the Globals definitions
					// TODO: Better error handling than just throwing an exception
					// TODO: Check that Descriptions match?
					throw new Exception($"Global {GlobalName} at {Address:X4} already defined at {g.Address:X4}");
				} else {
					// Just return. Ignore possible differening Descriptions
					return;
				}
			}
			// Global name not found. Add.
			Symbols.Globals[GlobalName] = new Symbol(Address, GlobalName, "");
		}

//---------------------------------------------------------------------------------------

		private void MatchLocal(Match ml, Function ThisFn) {
			if (ThisFn == null) {
				return;			// Couldn't find function name
			}
			string LocallName = ml.Groups["LocalName"].Value;
			var sym            = new Symbol(0, LocallName, "");
			ThisFn.Locals.Add(sym);
		}

//---------------------------------------------------------------------------------------

		private Function MatchFunction(Match m, string file) {
			string FnName  = m.Groups["FnName"].Value;
			// Get Address. Assume it's the end of the filename
			string fname   = Path.GetFileNameWithoutExtension(file);
			string HexAddr = fname.Substring(fname.Length - 4, 4);
			uint Addr      = Convert.ToUInt32(HexAddr, 16);

			var fn          = new Function(new Symbol(Addr, FnName, ""));
			string ParmName = m.Groups["ParmName"].Value;
			if (ParmName.Length > 0) {
				fn.Parms.Add(new Symbol(0, ParmName, ""));
			}
			string ParmName2 = m.Groups["ParmName2"].Value;
			if (ParmName2.Length > 0) {
				fn.Parms.Add(new Symbol(0, ParmName2, ""));
			}
			Symbols.Functions.Add(fn);
			return fn;
		}

//---------------------------------------------------------------------------------------

		private void WriteLisp() {
			string FullPath = Path.Combine(ProjectDir, ParmsFilename);
			var Filename = Path.ChangeExtension(FullPath, ".lsp");
			using (var sw = new StreamWriter(Filename)) {
				sw.Write("(");

				WriteLispGlobals(sw);

				sw.WriteLine("; Functions");
				sw.WriteLine("(");
				foreach (var f in Symbols.Functions) {
					sw.Write("\t");
					f.WriteLisp(sw);
				}
				sw.WriteLine(") ; End of Functions");

				sw.Write(")");						// End of everything
			}
		}

//---------------------------------------------------------------------------------------

		private void WriteLispGlobals(StreamWriter sw) {
				sw.WriteLine("; Globals");
				sw.WriteLine("(");
			foreach (var key in Symbols.Globals.Keys) {
				var g = Symbols.Globals[key];
				sw.WriteLine($"\t({g.Address} \"{g.Name}\" \"{g.Description}\")");
			}
			sw.WriteLine(")");
		}

//---------------------------------------------------------------------------------------

		private void radFunctions_CheckedChanged(object sender, EventArgs e) {
			if (radFunctions.Checked) {
				ShowFunctions();
			} else {
				ShowGlobals();
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowFunctions() {
			ShowFunctionsUi(true);
			// ShowGlobalsUi(false);
			lbThings.Items.Clear();
			foreach (var fn in Symbols.Functions) {
				lbThings.Items.Add(fn);
			}

			txtName.Text             = "";
			txtDescription.Text      = "";
			txtLocalName.Text        = "";
			txtLocalDescription.Text = "";

			txtLocalName.Enabled        = false;
			txtLocalDescription.Enabled = false;
		}

//---------------------------------------------------------------------------------------

		private void ShowGlobals() {
			ShowFunctionsUi(false);
			// ShowGlobalsUi(true);
			lbThings.Items.Clear();
			foreach (var key in Symbols.Globals.Keys) {
				lbThings.Items.Add(Symbols.Globals[key]);
			}

			txtName.Text        = "";
			txtDescription.Text = "";
		}

//---------------------------------------------------------------------------------------

		private void ShowFunctionsUi(bool Visible) {
			// TODO: Same code as ShowGlobalsUi
			lblLocals.Visible           = Visible;
			lbLocals.Visible            = Visible;
			lblLocalName.Visible        = Visible;
			txtLocalName.Visible        = Visible;
			lblLocalDescription.Visible = Visible;
			txtLocalDescription.Visible = Visible;
		}

//---------------------------------------------------------------------------------------

		private void ShowGlobalsUi(bool Visible) {
			// TODO: Same code as ShowFunctionsUi
			// TODO: Delete
			lblLocals.Visible           = Visible;
			lbLocals.Visible            = Visible;
			lblLocalName.Visible        = Visible;
			txtLocalName.Visible        = Visible;
			lblLocalDescription.Visible = Visible;
			txtLocalDescription.Visible = Visible;
		}

//---------------------------------------------------------------------------------------

		private void lbThings_SelectedIndexChanged(object sender, EventArgs e) {
			txtName.Enabled         = true;
			txtDescription.Enabled  = true;
			if (radFunctions.Checked) {
				FillFunctions();
			} else {
				// Must be Globals
				var g               = (Symbol)lbThings.SelectedItem;
				lblAddress.Text     = $"{g.Address:X4}";
				txtName.Text        = g.Name;
				txtDescription.Text = g.Description;
			}
		}

//---------------------------------------------------------------------------------------

		private void FillFunctions() {
			var fn              = (Function)lbThings.SelectedItem;
			lblAddress.Text     = $"0x{fn.Symbol.Address:X4}";
			txtName.Text        = fn.Symbol.Name;
			txtDescription.Text = fn.Symbol.Description;

			FillLocals(fn);
		}

//---------------------------------------------------------------------------------------

		private void FillLocals(Function fn) {
			lbLocals.Items.Clear();
			foreach (var local in fn.Locals) {
				lbLocals.Items.Add(local);
				txtLocalDescription.Text = local.Description;
			}
		}

//---------------------------------------------------------------------------------------

		private void lbLocals_SelectedIndexChanged(object sender, EventArgs e) {
			txtLocalName.Enabled        = true;
			txtLocalDescription.Enabled = true;
			var sym                     = (Symbol)lbLocals.SelectedItem;
			txtLocalName.Text           = sym.Name;
			txtLocalDescription.Text    = sym.Description;
		}

//---------------------------------------------------------------------------------------

		private void txtDescription_Leave(object sender, EventArgs e) {
			DoDescriptionLeave();
		}

//---------------------------------------------------------------------------------------

		private void txtLocalDescription_Leave(object sender, EventArgs e) {
			DoLocalDescriptionLeave();
		}

//---------------------------------------------------------------------------------------

		private void txtLocalName_Leave(object sender, EventArgs e) {
			DoLocalNameLeave();
		}

//---------------------------------------------------------------------------------------

		private void openProjectToolStripMenuItem_Click(object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog();
			fbd.Description = "Choose the directory where the *.c files are";
#if DEBUG
			fbd.SelectedPath = @"D:\Program Files (x86)\RevEngE6502\jobs\zol";
#endif
			var result = fbd.ShowDialog();
			if (result == DialogResult.OK) {
				ProjectDir = fbd.SelectedPath;
				// Open and read the ParmsFilename, if present. If not, create it and
				// run the Populate routine.
				string FullParmsName = Path.Combine(ProjectDir, ParmsFilename);
				if (File.Exists(FullParmsName)) {
					ReadSymbols();
				} else {
					PopulateInitialSymbols();
				}
				radFunctions.Checked = true;
				bDirtyBit = false;
			}
		}

//---------------------------------------------------------------------------------------

		private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e) {
			WriteSymbols();
			WriteLisp();
			bDirtyBit = false;
		}

//---------------------------------------------------------------------------------------

		private void RevEngESymbols_FormClosing(object sender, FormClosingEventArgs e) {
			if (bDirtyBit) {
				var res = MessageBox.Show("Changes have been made. Click OK to save any changes and exit, or Cancel to continue editing", "RevEngE",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				if (res == DialogResult.OK) {
					DoNameLeave();
					DoDescriptionLeave();
					DoLocalNameLeave();
					DoLocalDescriptionLeave();
					return;
				}
				e.Cancel = true;
			}
		}

//---------------------------------------------------------------------------------------

		private void txtLocalDescription_KeyPress(object sender, KeyPressEventArgs e) {
			var bEnter = e.KeyChar == (char)Keys.Enter;
			if (bEnter) {
				DoLocalDescriptionLeave();
				return;						// Don't set Dirty bit
			}
			bDirtyBit = true;
		}

//---------------------------------------------------------------------------------------

		private void txtLocalName_KeyPress(object sender, KeyPressEventArgs e) {
			var bEnter = e.KeyChar == (char)Keys.Enter;
			if (bEnter) {
				DoLocalNameLeave();
				return;						// Don't set Dirty bit
			}
			bDirtyBit = true;
		}

//---------------------------------------------------------------------------------------

		private void txtDescription_KeyPress(object sender, KeyPressEventArgs e) {
			var bEnter = e.KeyChar == (char)Keys.Enter;
			if (bEnter) {
				DoDescriptionLeave();
				return;						// Don't set Dirty bit
			}
			bDirtyBit = true;
		}

//---------------------------------------------------------------------------------------

		private void txtName_KeyPress(object sender, KeyPressEventArgs e) {
			var bEnter = e.KeyChar == (char)Keys.Enter;
			if (bEnter) {
				DoNameLeave();
				return;						// Don't set Dirty bit
			}
			bDirtyBit = true;
		}

//---------------------------------------------------------------------------------------

		private void txtName_Leave(object sender, EventArgs e) {
			DoNameLeave();
		}

//---------------------------------------------------------------------------------------

		private void DoNameLeave() {
			if (radFunctions.Checked) {
				var fn = (Function)lbThings.SelectedItem;
				fn.Symbol.Name = txtName.Text;

				ShowFunctions();
				// After ShowFunctions, no item is selected. Search for it and select that
				for (int i = 0; i < lbThings.Items.Count; i++) {
					if (lbThings.Items[i] == fn) {
						lbThings.SelectedIndex = i;
						break;
					}
				}

			} else {
				var g         = (Symbol)lbThings.SelectedItem;
				g.Name        = txtName.Text;
				g.Description = txtDescription.Text;

				ShowGlobals();
				// After ShowGlobals, no item is selected. Search for it and select that
				for (int i = 0; i < lbThings.Items.Count; i++) {
					if (lbThings.Items[i] == g) {
						lbThings.SelectedIndex = i;
						break;
					}
				}
		}
		}

//---------------------------------------------------------------------------------------

		private void DoDescriptionLeave() {
			if (radFunctions.Checked) {
				var fn                = (Function)lbThings.SelectedItem;
				fn.Symbol.Description = txtDescription.Text;
			} else {                    // Must be Globals
				var sym         = (Symbol)lbThings.SelectedItem;
				sym.Description = txtDescription.Text;
			}
		}

//---------------------------------------------------------------------------------------

		private void DoLocalNameLeave() {
			var fn   = (Function)lbThings.SelectedItem;
			var sym  = (Symbol)lbLocals.SelectedItem;
			// Must be in Functions mode
			sym.Name = txtLocalName.Text;
			FillLocals(fn);

			// After FillLocals, no item is selected. Search for it and select that
			for (int i = 0; i < lbLocals.Items.Count; i++) {
				if (lbLocals.Items[i] == sym) {
					lbLocals.SelectedIndex = i;
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void DoLocalDescriptionLeave() {
			// Must be in Functions mode
			var fn          = (Function)lbThings.SelectedItem;
			var sym         = (Symbol)lbLocals.SelectedItem;
			sym.Description = txtLocalDescription.Text;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[Serializable]
	public class Symbols {
		public SerializableDictionary<string, Symbol>	Globals;
		public List<Function>				Functions; // Note: s/b Dict<string, Symbol>?

//---------------------------------------------------------------------------------------

		public Symbols() {
			Globals   = new SerializableDictionary<string, Symbol>();
			Functions = new List<Function>();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[Serializable]
	public class Symbol {
		public uint		Address;
		public string	Name;
		public string	Description;

//---------------------------------------------------------------------------------------

		public Symbol() {
			// Parameterless ctor needed for serialization
		}

//---------------------------------------------------------------------------------------

		public Symbol(uint Address, string Name, string Description) {
			this.Address     = Address;
			this.Name        = Name;
			this.Description = Description;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() => Name;

//---------------------------------------------------------------------------------------

		public string AddrNameDesc => $"{Address} \"{Name}\" \"{Description}\"";

//---------------------------------------------------------------------------------------

		public string NameDesc => $"\"{Name}\" \"{Description}\"";

//---------------------------------------------------------------------------------------

		public void WriteLisp(StreamWriter sw) {
			sw.WriteLine($"({ToString()})");
        }
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[Serializable]
	public class Function {
		public Symbol		Symbol;
		public List<Symbol> Parms;			// Note: s/b Dict<string, Symbol>?
		public List<Symbol> Locals;         // Note: s/b Dict<string, Symbol>?

//---------------------------------------------------------------------------------------

		public Function() {
			// Parameterless ctor needed for serialization
		}

//---------------------------------------------------------------------------------------

		public Function(Symbol Symbol) {
			this.Symbol = Symbol;
			Parms       = new List<Symbol>();
			Locals      = new List<Symbol>();
		}

//---------------------------------------------------------------------------------------

		public override string ToString()
			=> Symbol.Name;

//---------------------------------------------------------------------------------------

		public void WriteLisp(StreamWriter sw) {
			sw.WriteLine($"({Symbol.AddrNameDesc}");
			sw.WriteLine("\t\t; Parms");
			sw.WriteLine("\t\t(");
			foreach (var p in Parms) {
				sw.Write("\t\t\t");
				p.WriteLisp(sw);
			}
			sw.WriteLine("\t\t)");

			sw.WriteLine("\t\t; Locals");
			sw.WriteLine("\t\t(");
			foreach (var l in Locals) {
				sw.Write("\t\t\t");
				sw.WriteLine($"({l.NameDesc})");
			}
			sw.WriteLine("\t\t)");

			sw.WriteLine("\t)");
		}
	}
}


