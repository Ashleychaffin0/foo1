#define DUMP_VERSION
#define	FORMAT_HTML

using System;
using System.Diagnostics;
using System.Collections;
using System.Text;

using SourceSafeTypeLib;

// Note: To get a Reference to VSS, browse to \\Bartsbs\Enggroup\shared\sourcesafe\win32\SSAPI.DLL
//		 In general, find it via HKEY_LOCAL_MACHINE\SOFTWARE\Classes\TypeLib\{783CD4E0-9D54-11CF-B8EE-00608CC9A71F}\5.1\0\win32

namespace Bartizan.TestPgms {
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class DumpSourceSafe {

		int _Indent = 0;
		string sIndent = "";
#if FORMAT_HTML
		const string sIndentBase = "&nbsp;";
		//const string	sIndentBase = " ";
#else
		const string	sIndentBase = "    ";
#endif
		const int IndentAmount = 2;

		// We do a lot of indenting. Set up a few fields to optimize
		string sSingleIndent;
		StringBuilder IndentBuilder;

		int Indent {
			get { return _Indent; }
			set {
				_Indent = value;
				for (int i = 0; i < _Indent; ++i) {
					IndentBuilder.Append(sSingleIndent);
				}
				sIndent = IndentBuilder.ToString();
				IndentBuilder.Length = 0;		// Free a bit of space
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			DumpSourceSafe dss = new DumpSourceSafe();
			dss.Run();
		}

//---------------------------------------------------------------------------------------

		DumpSourceSafe() {
			sSingleIndent = "";
			for (int i = 0; i < IndentAmount; ++i)
				sSingleIndent += sIndentBase;
			IndentBuilder = new StringBuilder();
		}

//---------------------------------------------------------------------------------------

		void Run() {
			VSSDatabase ssdb = new VSSDatabase();
			ssdb.Open(@"L:\Shared\SourceSafe\SrcSafe.ini", "larrys", "bartset");

			VSSItem projects = (VSSItem)ssdb.get_VSSItem("$/", false);

#if FORMAT_HTML
			Console.WriteLine("<HTML>\r\n<TITLE>Virtual SourceSafe Dump</TITLE>\r\n<BODY BGCOLOR=\"PowderBlue\">\r\n");
			Console.WriteLine("<BASEFONT FACE=\"COURIER\" SIZE=\"2\">\r\n");
#endif
			DumpItem(projects);
#if FORMAT_HTML
			Console.WriteLine("\r\n</BODY>\r\n</HTML>\r\n");
#endif
		}

//---------------------------------------------------------------------------------------

		void DumpItem(VSSItem MainItem) {
			try {
				if (MainItem.Type == (int)VSSItemType.VSSITEM_PROJECT) {
#if true
					if (MainItem.Spec == "$/Bartizan-Really Old") {
						Color("RED");
						WriteLineHTML("<FONT SIZE=\"+2\">");
						WriteLine("*** Bypassing Bartizan-Really Old ***");
						WriteLineHTML("</FONT>");
						Color();
						return;
					}
#endif
					DumpProjectInfo(MainItem);
					DumpVersionInfo(MainItem);
				} else if (MainItem.Type == (int)VSSItemType.VSSITEM_FILE) {
					if (true == DumpFileInfo(MainItem)) {
						DumpVersionInfo(MainItem);
					}
				} else {
					WriteLine("Unknown type - {0}", MainItem);
				}
			} catch (Exception ex) {
				Except("\n\nException in DumpItem - ", ex);
			}
		}

//---------------------------------------------------------------------------------------

		void DumpProjectInfo(VSSItem MainItem) {
			Color("Red");
			WriteLine("Project - Name = {0}, Spec={1}", MainItem.Name, MainItem.Spec);
			++Indent;
			foreach (VSSItem item in MainItem.get_Items(false)) {
				try {
					WriteLine("Project - First Child name = {0}, Link Count={1}, Spec={2}", item.Name, item.Links.Count, item.Spec);
					++Indent;
					DumpLinkInfo(item);
					DumpItem(item);
					--Indent;
				} catch (Exception ex) {
					Except("Exception in DumpProjectInfo -- ", ex);
				}
			}
			--Indent;
			Color();
		}

//---------------------------------------------------------------------------------------

		bool DumpFileInfo(VSSItem MainItem) {
			bool bOK = true;
			Color("Blue");
			try {
				WriteLine("File - Item name={0}, Link Count={1}", MainItem.Name, MainItem.Links.Count);
				DumpCheckouts(MainItem);
			} catch (Exception ex) {
				bOK = false;
				if (ex.Message == "File or project not found") {
					WriteLine("*File or project not found*");
				} else {
					Except("Exception in DumpFileInfo -- ", ex);
				}
			}
			Color();
			return bOK;
		}

//---------------------------------------------------------------------------------------

		void DumpCheckouts(VSSItem MainItem) {
			++Indent;
			Color("DarkCyan");
			foreach (VSSCheckout chkout in MainItem.Checkouts) {
				try {
					WriteLine("Checkout - Machine={0}, Version={1}, User={2}, Date={3}", chkout.Machine, chkout.VersionNumber, chkout.Username, chkout.Date);
				} catch (Exception ex) {
					Except("Exception in DumpCheckouts -- ", ex);
				}
			}
			--Indent;
			Color();
		}

//---------------------------------------------------------------------------------------

		[Conditional("DUMP_VERSION")]
		void DumpVersionInfo(VSSItem MainItem) {
			++Indent;		// Version Info is always indented
			Color("Purple");
			try {
				string[] acts;
				string act;
				foreach (VSSVersion version in MainItem.get_Versions((int)VSSFlags.VSSFLAG_RECURSYES)) {
					try {
						acts = version.Action.Split(new char[1] { ' ' }, 2);
						switch (acts[0]) {
						case "Created":
							act = "<FONT COLOR=\"DarkSeaGreen\">";
							break;
						case "Added":
							act = "<FONT COLOR=\"Orange\">";
							break;
						case "Deleted":
							act = "<FONT COLOR=\"Maroon\">";
							break;
						case "Checked":
							act = "<FONT COLOR=\"Green\">";
							break;
						case "Shared":
							act = "<FONT COLOR=\"Brown\">";
							break;
						case "Renamed":
							act = "<FONT COLOR=\"OrangeRed\">";
							break;
						case "Destroyed":
							act = "<FONT COLOR=\"Red\">";
							break;
						default:
							act = "<FONT COLOR=\"Lime\" SIZE=\"+2\">";
							break;
						}
						act += acts[0] + "</FONT> " + acts[1];
						WriteLine("Version={0}, Name={1}, Action={2}, Label={3}, Date={4}",
							version.VersionNumber, MainItem.Name, act, version.Label, version.Date);
					} catch (Exception ex) {
						Except("Exception in DumpVersionInfo -- ", ex);
					}
				}
			} catch (Exception ex) {
				Except("Exception in DumpVersionInfo -- ", ex);
			}
			Color();
			--Indent;
		}

//---------------------------------------------------------------------------------------

		void DumpLinkInfo(VSSItem MainItem) {
			Color("Brown");
			string Spec, LocalSpec;
			foreach (VSSItem link in MainItem.Links) {
				try {
					try { Spec = link.Spec; } catch { Spec = "N/A"; }
					try { LocalSpec = link.LocalSpec; } catch { LocalSpec = "N/A"; }
					WriteLine("Link={0}, Spec={1}, LocalSpec={2}", link.Name, Spec, LocalSpec);
					++Indent;
					DumpItem(link);
					--Indent;
				} catch (Exception ex) {
					Except("Exception in DumpLinkInfo -- ", ex);
				}
			}
			Color();
		}

//---------------------------------------------------------------------------------------

		void WriteLine(string fmt, params object[] args) {
			string s = string.Format(sIndent + fmt, args);
#if FORMAT_HTML
			if (s.IndexOf('\n') > 0)
				s = s.Replace("\n", "<BR>");
#endif
			Console.WriteLine(s);
			Console.WriteLine("<BR>");
		}

//---------------------------------------------------------------------------------------

		[Conditional("FORMAT_HTML")]
		void WriteLineHTML(string fmt, params object[] args) {
			WriteLine(fmt, args);
		}

//---------------------------------------------------------------------------------------

		[Conditional("FORMAT_HTML")]
		void Color(string name) {
			Console.WriteLine("<FONT COLOR=\"" + name + "\">");
		}

//---------------------------------------------------------------------------------------

		[Conditional("FORMAT_HTML")]
		void Color() {
			Console.WriteLine("</FONT>");
		}

//---------------------------------------------------------------------------------------

		void Except(string msg, Exception ex) {
			WriteLineHTML("<FONT COLOR=\"INDIGO\" SIZE=\"+1\">");
			WriteLine(msg + ex.Message);
			// WriteLine(ex.StackTrace);
			// WriteLine("\n");

			WriteLineHTML("</FONT>");
		}

//---------------------------------------------------------------------------------------

		void Break() {
#if true
			System.Diagnostics.Debugger.Break();
#endif
		}
	}
}
