using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using LRSUtils;

// TODO: Add Listener / Visitor modules to .csproj as required
// TODO: Multiple skeletons for Java/Python/C++/JavaScript/Go/Swift?
// TODO: If Java, show (and implement) grun options
// TODO: Add JAVA_DIR to Environment. Update a4.cmd + jc.cmd
// TODO: Add tabs to UI? One for options, some for edit src (.g4 and .lexer)/msgs
// TODO: Or on single tab, with split screen? Or with buttons to select which?
// TODO: Program skeleton -- input from stdin? hard coded? From parm (e.g. from textbox)?
// TODO: Package mainline as class/dll. Update project for Reference to it.
// TODO: Make MyListener/MyVisitor their own files. Also rename to <GrammarName>Visitor etc
// TODO: Filewatcher on .g4. Return here on Save
// TODO: Option to write AntlrMsgs out to a file
// TODO: Save/Restore Grun parms
// TODO: Support filename(s?) for Grun input

namespace LRSAntlr {
	public partial class LRSAntlr : Form {

		[DllImport("user32.dll", SetLastError = true)]
		static extern bool BringWindowToTop(IntPtr hWnd);

		// TODO: These will go eventually into a config file
		string GrammarName;

		string ParmsFilename = "LRSAntlrParms.config";
		LrsAntlrParms Parms;

		List<TargetLanguage> TargetLanguages;
		List<string>		 AntlrMsgs;

//---------------------------------------------------------------------------------------

		public LRSAntlr() {
			InitializeComponent();

			// TODO: Test code to find devenv.exe
			var Progs = Environment.SpecialFolder.CommonStartMenu; // + "\Programs\"
			var path  = Environment.GetFolderPath(Progs);

			SetupParms();
			SetupLanguages();
		}

//---------------------------------------------------------------------------------------

		private void SetupParms() {
			if (!File.Exists(ParmsFilename)) {
				Parms = new LrsAntlrParms(ParmsFilename) {
					Listener			= true,
					Visitor				= true,
					TargetDir			= ".",
					CompileAsCSharp		= true,
					CompileAsJava		= true,
					CompileAsPython2	= false,
					CompileAsPython3	= false,
					CompileAsCpp		= false,
					CompileAsJavaScript = false,
					CompileAsGo			= false,
					CompileAsSwift		= false,
					// UIOption		 = 
				};
				GenericSerializer<LrsAntlrParms>.Save(ParmsFilename, Parms);
			} else {
				Parms = new LrsAntlrParms(ParmsFilename).Load();
				CopyParmsToUI();
			}
		}

//---------------------------------------------------------------------------------------

		private void SetupLanguages() {
			TargetLanguages = new List<TargetLanguage> {
				// https://github.com/antlr/antlr4/blob/master/doc/csharp-target.md
				new TargetLanguage("CSharp",	 ChkCompileAsCSharp,	"Nonce"),
				new TargetLanguage("Java",		 ChkCompileAsJava,		"Nonce"),
				new TargetLanguage("Python2",	 ChkCompileAsPython2,	"Nonce"),
				new TargetLanguage("Python3",	 ChkCompileAsPython3,	"Nonce"),
				// https://github.com/antlr/antlr4/blob/master/doc/cpp-target.md
				new TargetLanguage("C++",		 ChkCompileAsCpp,		"Nonce"),
				new TargetLanguage("JavaScript", ChkCompileAsJavaScript, "Nonce"),
				new TargetLanguage("Go",		 ChkCompileAsGo,		 "Nonce"),
				new TargetLanguage("Swift",		 ChkCompileAsSwift,		 "Nonce")
			};
		}

//---------------------------------------------------------------------------------------

		private void CopyParmsToUI() {
			TxtFolderName.Text			= Parms.TargetDir;
			// TODO: Fill combo box
			ChkListener.Checked				= Parms.Listener;
			ChkVisitor.Checked				= Parms.Visitor;
			ChkCompileAsCSharp.Checked		= Parms.CompileAsCSharp;
			ChkCompileAsJava.Checked		= Parms.CompileAsJava;
			ChkCompileAsPython2.Checked		= Parms.CompileAsPython2;
			ChkCompileAsPython3.Checked		= Parms.CompileAsPython3;
			ChkCompileAsCpp.Checked			= Parms.CompileAsCpp;
			// TODO: Put in CheckedChanged events for these next 3
			ChkCompileAsJavaScript.Checked	= Parms.CompileAsJavaScript;
			ChkCompileAsGo.Checked			= Parms.CompileAsGo;
			ChkCompileAsSwift.Checked		= Parms.CompileAsSwift;
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			CreateVisualStudioFiles();
			int ExitCode = RunAntlr();
			if (ExitCode == 0) ShowVisualStudio();
		}

//---------------------------------------------------------------------------------------

		private int RunAntlr() {
			// TODO: Pass more parms (e.g. grun)
			string JavaDir = GetJavaPath();

			AntlrMsgs = new List<string>();
			int MaxReturnCode = 0;
			foreach (var Lang in TargetLanguages) {
				if (!Lang.ChkBox.Checked) {
					continue;
				}
				if (AntlrMsgs.Count > 0) AddToMsgs("");
				AddToMsgs($"Compiling for target {Lang.Language}");

				var proc = new Process {
					StartInfo = new ProcessStartInfo {
						// FileName = @"g:\lrs-8500\bin\a4.cmd",
						FileName  = Path.Combine(JavaDir, "java.exe"),
						Arguments = "org.antlr.v4.Tool " + 
							Path.Combine(Parms.TargetDir, GrammarName + ".g4"),
						UseShellExecute        = false,
						RedirectStandardOutput = true,
						RedirectStandardError  = true,
						CreateNoWindow         = true
					}
				};

				proc.StartInfo.Arguments += AddCommandLineOptions(Lang.Language);
				AddToMsgs($"Running: {proc.StartInfo.FileName} {proc.StartInfo.Arguments}");

				// https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output
				// I've seen cases where the following lambdas merely did "msgs.Add(...)" and
				// we wound up with "msgs" containing null entries. I assume this is related
				// to a race condition. So we'll call an AddToMessages routine that will do
				// locking to try to prevent this.
				proc.OutputDataReceived += (s, e) => { AddToMsgs(e.Data); };
				proc.ErrorDataReceived += (s, e) => { AddToMsgs(e.Data); };
				proc.Start();
				proc.BeginOutputReadLine();
				proc.BeginErrorReadLine();
				proc.WaitForExit();
				if (proc.ExitCode > 0) AddToMsgs($"{Lang.Language} returned code {proc.ExitCode}");
				MaxReturnCode = Math.Max(MaxReturnCode, proc.ExitCode);
			}
			LbMsgs.Items.Clear();
			LbMsgs.Items.AddRange(AntlrMsgs.ToArray<string>());
			if (MaxReturnCode > 0) AddToMsgs($"Max return code was {MaxReturnCode}");
			return MaxReturnCode;
#if false
			// while (!proc.StandardOutput.EndOfStream) {
			while (!proc.StandardError.EndOfStream) {
				string line = proc.StandardError.ReadLine();
				LbMsgs.Items.Add(line);
				Application.DoEvents();
			}
#endif
		}

//---------------------------------------------------------------------------------------

		void AddToMsgs(string data) {
			if (data == null) return;
			lock(AntlrMsgs) {
				AntlrMsgs.Add(data);
			};
		}

//---------------------------------------------------------------------------------------

		private string GetJavaPath() {
			string JavaPath = Environment.GetEnvironmentVariable("JAVA_DIR");
			return JavaPath;
		}

//---------------------------------------------------------------------------------------

		private string AddCommandLineOptions(string Language) {
			var sb = new StringBuilder();
			if (ChkListener.Checked) {
				sb.Append(" -listener");
			} else {
				sb.Append(" -no-listener");
			}
			if (ChkVisitor.Checked) {
				sb.Append(" -visitor");
			} else {
				sb.Append(" -no-visitor");
			}

			sb.Append($" -Dlanguage={Language}");

			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		private void ShowVisualStudio() {
			var p = from proc in Process.GetProcesses()
					where proc.MainWindowTitle.StartsWith(GrammarName)
					select new { proc, proc.MainWindowHandle };
			var q = p.FirstOrDefault();
			if (q == null) {
				Process.Start(Path.Combine(Parms.TargetDir, $"{GrammarName}.sln"));
			} else { 
				IntPtr handle = q.MainWindowHandle;
				BringWindowToTop(handle);
			}
		}

//---------------------------------------------------------------------------------------

		private void CreateVisualStudioFiles() {
			string Dir = Parms.TargetDir;     // Short synonym for TargetDir
			string SlnName = Path.Combine(Dir, GrammarName + ".sln");
			if (!File.Exists(SlnName)) {
				string SolutionGuid = "{" + Guid.NewGuid().ToString() + "}";
				string ProjectGuid = "{" + Guid.NewGuid().ToString() + "}";

				var parms = (GrammarName, SolutionGuid, ProjectGuid);

				CopySkeleton("SkeletonSln.txt", SlnName, parms);
				CopySkeleton("SkeletonProject.txt", Dir + $@"\{GrammarName}.csproj", parms);
				CopySkeleton("SkeletonProgram.txt", Dir + $@"\{GrammarName}.cs", parms);
				CopySkeleton("SkeletonApp.Config.txt", Dir + @"\app.config", parms);
				string DirProps = Path.Combine(Dir, "Properties");
				Directory.CreateDirectory(DirProps);
				CopySkeleton("SkeletonAssemblyInfo.txt", DirProps + @"\AssemblyInfo.cs", parms);
				// TODO: Add separate Listener (and maybe Visitor) module
			}
		}

//---------------------------------------------------------------------------------------

		private void CopySkeleton(string fromFile, string toFile,
						(string GrammarName, string SolutionGuid, string ProjectGuid) p) {
			string txt = File.ReadAllText(fromFile);
			txt = txt
					.Replace("$$GrammarName$$",  p.GrammarName)
					.Replace("$$ProjectGuid$$",  p.ProjectGuid)
					.Replace("$$SolutionGuid$$", p.SolutionGuid);
			File.WriteAllText(toFile, txt);
		}

//---------------------------------------------------------------------------------------

		private void BtnBrowseDir_Click(object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog();
			fbd.SelectedPath = Parms.TargetDir;
			DialogResult dr = fbd.ShowDialog();
			if (dr == DialogResult.OK) {
				Parms.TargetDir = fbd.SelectedPath;
				TxtFolderName.Text = Parms.TargetDir;
				// For some reason the above isn't triggering a TextChanged event.
				// So do it ourselves
				TxtFolderName_TextChanged(TxtFolderName, EventArgs.Empty);
			}
		}

//---------------------------------------------------------------------------------------

		private void TxtFolderName_TextChanged(object sender, EventArgs e) {
			Parms.TargetDir = TxtFolderName.Text;
			Parms.Save();
			CmbGrammar.Items.Clear();
			foreach (var item in Directory.EnumerateFiles(Parms.TargetDir, "*.g4")) {
				string fname = Path.GetFileNameWithoutExtension(item);
				CmbGrammar.Items.Add(fname);
		}
			CmbGrammar.SelectedIndex = 0;
			GrammarName = CmbGrammar.Items[0] as string;
		}

//---------------------------------------------------------------------------------------

		private void ChkListener_CheckedChanged(object sender, EventArgs e) {
			Parms.Listener = ChkListener.Checked;
			Parms.Save();
		}

//---------------------------------------------------------------------------------------

		private void ChkVisitor_CheckedChanged(object sender, EventArgs e) {
			Parms.Visitor = ChkVisitor.Checked;
			Parms.Save();
		}

//---------------------------------------------------------------------------------------

		private void ChkCompileAsCSharp_CheckedChanged(object sender, EventArgs e) {
			Parms.CompileAsCSharp = ChkCompileAsCSharp.Checked;
			Parms.Save();
		}

//---------------------------------------------------------------------------------------

		private void ChkCompileAsJava_CheckedChanged(object sender, EventArgs e) {
			Parms.CompileAsJava = ChkCompileAsJava.Checked;
			bool GrunVisible    = Parms.CompileAsJava == true;

			lblGrunOptions      .Visible = GrunVisible;
			LblGrunStartRuleName.Visible = GrunVisible;
			TxtGrunStartRuleName.Visible = GrunVisible;
			RadGrunTokens		.Visible = GrunVisible;
			RadGrunTree			.Visible = GrunVisible;
			RadGrunGui			.Visible = GrunVisible;
			RadGrunTrace		.Visible = GrunVisible;
			RadGrunDiagnostics	.Visible = GrunVisible;
			Parms.Save();
		}

//---------------------------------------------------------------------------------------

		private void ChkCompileAsPython2_CheckedChanged(object sender, EventArgs e) {
			Parms.CompileAsPython2 = ChkCompileAsPython2.Checked;
			Parms.Save();
		}

//---------------------------------------------------------------------------------------

		private void ChkCompileAsPython3_CheckedChanged(object sender, EventArgs e) {
			Parms.CompileAsPython3 = ChkCompileAsPython3.Checked;
			Parms.Save();
		}

//---------------------------------------------------------------------------------------

		private void ChkCompileAsCpp_CheckedChanged(object sender, EventArgs e) {
			Parms.CompileAsCpp = ChkCompileAsCpp.Checked;
			Parms.Save();
		}

//---------------------------------------------------------------------------------------

		private void ChkCompileAsJavaScript_CheckedChanged(object sender, EventArgs e) {
			Parms.CompileAsJavaScript = ChkCompileAsJavaScript.Checked;
		}

//---------------------------------------------------------------------------------------

		private void ChkCompileAsGo_CheckedChanged(object sender, EventArgs e) {
			Parms.CompileAsGo = ChkCompileAsGo.Checked;
		}

//---------------------------------------------------------------------------------------

		private void ChkCompileAsSwift_CheckedChanged(object sender, EventArgs e) {
			Parms.CompileAsSwift = ChkCompileAsSwift.Checked;
		}

//---------------------------------------------------------------------------------------

		private void CmbGrammar_SelectedIndexChanged(object sender, EventArgs e) {
			GrammarName = CmbGrammar.SelectedItem.ToString();
		}

//---------------------------------------------------------------------------------------

		private void LbMsgs_MouseMove(object sender, MouseEventArgs e) {	// TODO: ???
			int y = e.Y;
			var h = LbMsgs.Font.SizeInPoints;
			int line = (int)Math.Floor(y / h);
			if (line >= LbMsgs.Items.Count) return;
			string text = (string)LbMsgs.Items[line];
			toolTip1.SetToolTip(LbMsgs, text);
		}
	}
}
