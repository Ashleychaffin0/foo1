// #define TESTING

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

// TODO: (See list)
//	*	None at the moment

namespace MonitorClipboardChange {
	public partial class MonitorClipboardChange : Form {
		private const int WM_DRAWCLIPBOARD = 0x0308;
		private const int WM_CHANGECBCHAIN = 0x030D;

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

		IntPtr NextViewer = IntPtr.Zero;

		bool Monitoring = true;
#if false
		bool bRecursing = false;    // TODO: Why isn't this working?
#endif

		string LastClipboardText = "";

		const string TitlePattern = @" [-A-Z :.'""0-9?]+";
		Regex re = new Regex(TitlePattern, RegexOptions.Compiled);

		FileSystemWatcher fsw;

		SynchronizationContext scGui;

//---------------------------------------------------------------------------------------

		public MonitorClipboardChange() {
			InitializeComponent();

			scGui = SynchronizationContext.Current;

			// foo();

			SetupFilewatcher();

			NextViewer = SetClipboardViewer(this.Handle);
		}

//---------------------------------------------------------------------------------------

		private void foo() {
			string fn = @"G:\LRS-8500\Words.txt";
			var rdr = new System.IO.StreamReader(fn);
			var lines = rdr.ReadToEnd().Split('\n');
			var qry = from line in lines
					  where (line.Trim().Length == 13)
						// && ! line.ToUpper().Contains('A')
						&& ! line.ToUpper().Contains('B')
						// && ! line.ToUpper().Contains('C') 
						&& ! line.ToUpper().Contains('D')
						// && ! line.ToUpper().Contains('E')
						&& ! line.ToUpper().Contains('F')
						// && ! line.ToUpper().Contains('G')
						&& ! line.ToUpper().Contains('H')
						// && ! line.ToUpper().Contains('I')
						&& ! line.ToUpper().Contains('J')
						&& ! line.ToUpper().Contains('K')
						// && ! line.ToUpper().Contains('L')
						// && ! line.ToUpper().Contains('M')
						// && ! line.ToUpper().Contains('N')
						// && ! line.ToUpper().Contains('O')
						&& ! line.ToUpper().Contains('P')
						&& ! line.ToUpper().Contains('Q')
						// && ! line.ToUpper().Contains('R')
						// && ! line.ToUpper().Contains('S')
						// && ! line.ToUpper().Contains('T')
						&& ! line.ToUpper().Contains('U')
						&& ! line.ToUpper().Contains('V')
						&& ! line.ToUpper().Contains('W')
						&& ! line.ToUpper().Contains('X')
						// && ! line.ToUpper().Contains('Y')
						&& ! line.ToUpper().Contains('Z')
						
						&& ! line.ToUpper().Contains("AC")
						&& ! line.ToUpper().Contains("AG")
						&& ! line.ToUpper().Contains("AN")
						&& ! line.ToUpper().Contains("AR")
						&& ! line.ToUpper().Contains("AS")
						&& ! line.ToUpper().Contains("AT")
						&& ! line.ToUpper().Contains("CA")
						&& ! line.ToUpper().Contains("CE")
						&& ! line.ToUpper().Contains("EN")
						&& ! line.ToUpper().Contains("SE")
						&& ! line.ToUpper().Contains("ES")
						&& ! line.ToUpper().Contains("RE")
						&& ! line.ToUpper().Contains("ER")
						&& ! line.ToUpper().Contains("TR")
						&& ! line.ToUpper().Contains("INT")
						&& ! line.ToUpper().Contains("NON")
					  select line;
			foreach (var item in qry) {
				Console.WriteLine(item);
			}
		}

//---------------------------------------------------------------------------------------

		private void SetupFilewatcher() {
			// var path = Path.GetDirectoryName(Application.ExecutablePath);
			// var path = Environment.GetEnvironmentVariable("TEMP");
			var path = @"G:\Downloads";
			fsw = new FileSystemWatcher(path, "*.tmp.txt") {
				IncludeSubdirectories = false
			};
			fsw.Renamed += Fsw_Renamed; // See comments in that routine
			fsw.NotifyFilter = NotifyFilters.FileName;
			fsw.EnableRaisingEvents = true;
		}

//---------------------------------------------------------------------------------------

		// When a files is downloaded, it's downloaded into a temp file name, such as
		// G:\Downloads\tmp367E.tmp.txt.crdownload. It's then renamed to
		// G:\Downloads\tmp367E.tmp.txt. Thus we know that the file's been downloaded,
		// and the rename is the final step.
		private void Fsw_Renamed(object sender, RenamedEventArgs e) {
			Thread.Sleep(200);      // Give Rename a chance to finish
			string txt = File.ReadAllText(e.FullPath);
			scGui.Send(o => Clipboard.SetText(txt) , EventArgs.Empty);
		}

//---------------------------------------------------------------------------------------

		protected override void WndProc(ref Message m) {
			base.WndProc(ref m);
			switch (m.Msg) {
				case WM_DRAWCLIPBOARD:
#if false
					if (bRecursing || !bRecursing) {	// TODO: Get rid of?
						bRecursing = false;
					} else
#endif
					if (Monitoring && Clipboard.ContainsText()) {
#if TESTING
						string Text = TestData();
#else
						string Text = Clipboard.GetText();
#endif
						if (Text.EndsWith(".pdf")) return;
						if (Text != LastClipboardText) {    // i.e. recursing?
							var Vintage = VintageBase.VintageFactory(Text);
							if (Vintage == null) { // SciAm stuff?
								// MessageBox.Show("Can't find Vintage for this file");
								return;
							} else {
#if false
								var lines = ProcessText(Text);
#else
								Vintage.ParseToc();
#endif
// MessageBox.Show(lines);
#if false
								bRecursing = true;
#endif
								// string lines = string.Join("\r\n", Vintage.Lines);
								string lines = Vintage.GetData();
								LastClipboardText = lines;
								Clipboard.SetText(lines);
							}
						}
					}
					if (NextViewer != IntPtr.Zero) {
						SendMessage(NextViewer, m.Msg, m.WParam, m.LParam);
					}
					break;
				case WM_CHANGECBCHAIN:
					NextViewer = m.LParam;
					break;
				default:
					break;
			}
		}

//---------------------------------------------------------------------------------------

#if false   // ProcessText
		private string ProcessText(string s) {
			var InputLines = s.Split(NewLines, StringSplitOptions.RemoveEmptyEntries);
			// We assume the format is as follows:
			//	*	Zero or more lines before "ARTICLES"
			//	*	Zero or more lines in the form <nnn> by <author(s)> <description>
			//	*	"DEPARTMENTS"
			//	*	One line with departments: <nn> text <nn> text ...
			var OutputLines  = new List<string>();
			var ArticleNames = new List<string>();
			bool bInArticles = false;
			for (int i = 0; i < InputLines.Length; i++) {
				string line = InputLines[i].Trim();
				if (line == "ARTICLES") {
					OutputLines.Add("O");
					OutputLines.Add(line);
					bInArticles = true;
					continue;
				}
				if (!bInArticles) continue;
				bool bContinue = ProcessLine(line, InputLines, i, OutputLines, ArticleNames);
				if (!bContinue) break;
			}
			OutputLines.Add("$");
			for (int i = ArticleNames.Count - 1; i >= 0; i--) {
				OutputLines.Insert(0, $"A {ArticleNames[i]}");
			}

			return string.Join("\r\n", OutputLines);
		}
#endif

//---------------------------------------------------------------------------------------

		private bool ProcessLine(string line, List<string> InputLines, int i, List<string> OutputLines, List<string> ArticleNames) {
			// A sample line looks as follows:
			// 28 THE INDUCTION OF CANCER BY VIRUSES, by Renato Dulbecco How do the viruses of known animal cancers cause cells to become cancerous? 
			// Our problem here is to recognize where the authors end and where the 
			// description begins. 
			var words = line.Split(' ').ToList();
			// But it might also be: DEPARTMENTS
			if (words[0].Trim() == "DEPARTMENTS") {
				// May be "DEPARTMENTS" or "DEPARTMENTS 8 LETTERS ..."
				int ix = (words.Count > 1) ? i : i + 1;
				ProcessDepartments(InputLines[ix], OutputLines);
				return false;
			}
			int ixBy = words.FindIndex(w => w.Trim() == "by");
			if (ixBy < 0) {
				OutputLines.Add(line);      // Just pass it on for manual editing
				return true;
			}
			// OK, we know where the author(s) begin. We'll assume that all (see note)
			// the author names start with a capital letter, is a capitalized initial,
			// or is the one of "and", "et" or "al". (Note: This fails on, say, "Neil
			// de Grasse Tyson" or "Otto von Bismarck". We expect such cases to be rare
			// enough that we'll accept the mis-parse and require the user to do a
			// manual fixup.)
			for (int ix = ixBy + 1; ix < words.Count; ix++) {
				string word = words[ix].Trim();
				if ((word == "and") || (word == "et") || (word == "al.")) continue;
				// TODO: Replace above with something like <if EscapeWords.Contains...>
				char FirstLetter = word[0];

				if (! char.IsUpper(FirstLetter)) {
					var LeftPart = string.Join(" ", words.Take(ix - 1));
					OutputLines.Add(LeftPart);
					var m = re.Match(LeftPart);
					if (m.Success) ArticleNames.Add(m.Value);
					// What I want here is either some sort of <words.Split(ix) and give
					// me a LeftPart and a RightPart. Or some sort of List-Based
					// SubString(StartIndex, Length). I can do part of it with Take(),
					// but I can't think of how to do the second part without a loop.
					var RightPart = new List<string>();
					for (int j = ix - 1; j < words.Count; j++) {
						RightPart.Add(words[j]);
					}
					OutputLines.Add("\t" + string.Join(" ", RightPart));
					break;
				}
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private void ProcessDepartments(string s, List<string> OutputLines) {
			// Line looks like: 8 LETTERS 12 50 AND 100 YEARS AGO 21 THE AUTHORS 54 SCIENCE AND THE CITIZEN 136 MATHEMATICAL GAMES 142 THE AMATEUR SCIENTIST 155 BOOKS 167 BIBLIOGRAPHY 
#if true
			string PatNorm = @"\d+ (\w+\s+)+?";
			PatNorm = @"((?<Thing>\d+ (\w+ *)+)+)";		// DEBUG
			var ReNorm = new Regex(PatNorm);
#endif
			if (s.StartsWith("DEPARTMENTS")) s = s.Substring(11);
			string PatHistorial = @"\d+ \d+ (AND \d+ )+YEARS AGO";
			var ReHist = new Regex(PatHistorial);
			while (s.Length > 0) {
				// The historical pattern is the oddball. Get rid of it first.
				var m = ReHist.Match(s);
				if (m.Success) {
					OutputLines.Add(m.Value);
					s = RemoveSubstring(s, m.Index, m.Length);
				}
#if true   // I couldn't get the Regex to work!
				var m2 = ReNorm.Matches(s);
				// if (m2.Success) {
					// OutputLines.Add(m2.Value);
					// s = RemoveSubstring(s, m2.Index, m2.Length);
				// }
#endif
				s = ParseDepartments(s, OutputLines);
			}
		}

//---------------------------------------------------------------------------------------

		private string ParseDepartments(string s, List<string> OutputLines) {
			// Brute force
			s = s.Trim();
			if (s.Length == 0) return "";
			int i = 0;
			while (char.IsDigit(s[i])) {
				++i;
				continue;		// Skip over page #
			}
			while (s.Length > 0) {
				if (++i >= s.Length) {
					OutputLines.Add(s);
					return "";
				}
				if (char.IsDigit(s[i])) {
					string entry = s.Substring(0, i);
					OutputLines.Add(entry);
					s = s.Substring(i);
					return s;
				}
			}
			return s;
		}

//---------------------------------------------------------------------------------------

		private string RemoveSubstring(string s, int Start, int Length) {
			string s2 = "";
			if (Start > 0) s2 = s.Substring(0, Start - 1);
			s2 += s.Substring(Start + Length);
			return s2;
		}

//---------------------------------------------------------------------------------------

		private void BtnEnableDisable_Click(object sender, EventArgs e) {
			Monitoring = !Monitoring;
			BtnEnableDisable.Text = Monitoring ? "Enabled" : "Disabled";
		}

#if false   // ParseText
		//---------------------------------------------------------------------------------------

		private void ParseText(string s) {
			var Articles = new List<ArticleInfo>();
			var pat = @"^\s*\d+\s+";		// e.g. "  38    "
			var re = new Regex(pat);
			var Lines = s.Split(new char[] { '\n' });
			var MarginSizes = GetMarginSizes(re);
			int ixMargins = 0;
			int ofs = MarginSizes[ixMargins];
			for (int i = 0; i < Lines.Length; i++) {
				string line = Lines[i];
				if ((line.Length > 0) && (line[0] == '\f') && (++ixMargins < MarginSizes.Count)) {
					ofs = MarginSizes[ixMargins];
				}
				line = line.Trim();			// Get rid of trailing \r's
				if (line.Length == 0) continue;
				var m = re.Match(line);
				if (m.Success) {    // Start of article
					int.TryParse(m.Value, out int PageNum);
					i = ProcessArticle(Lines, i, ofs, out ArticleInfo art);
					Articles.Add(art);
				}
			}
		}
#endif

//---------------------------------------------------------------------------------------

		private string TestData() {
			return @"  SCIENTIFIC

                                             September 1987 Volume 257 Number 3
 A_VIERICAN



   39                           Strategic Defense and Directed-Energy Weapons 
                                C. Kumar N. Patel and Nicolaas Bloembergen


                                SDI partisans attack and SDI critics invoke the findings of the American 
                                Physical Society panel that evaluated the feasibility of lasers and particle- 
                                beam weapons as instruments of strategic defense. Here, for the public 
                                record, is a summary of what the panel—which consisted of weapons 
                                experts and knowledgeable physicists—actually said.



   46                           The Large-Scale Streaming of Galaxies 
                                Alan Dressler


                                Even as they are swept apart by the expanding fabric of space, the Milky 
                                Way and a host of other galaxies show motion of their own; the speed 
                                and direction of the motion indicate that the galaxies are caught in the 
                                sway of a Great Attractor, a distant concentration of mass that appears to 
                                be larger than any proposed by existing cosmologies.



   56                           Reverse Transcription 
       /VIM. VIRAL R            Harold Varmus

        REVERSE TRANSCRIPTION   When first discovered, the ability to transcribe DNA from RNA was 
       7%0*         II 14'      thought to be unique to a few cancer-causing viruses. Now it seems the 
              \          /      process may be part of the genetic machinery of other viruses and even 
             INVERTED REPEATS   of higher organisms; studies of it could further understanding of AIDS and 
           - - T.- - - - -      oncogenes and may illuminate the evolution of hereditary material itself.



   66                           Electrides 
                                James L. Dye


                                In this new class of crystalline materials, derived from cesium, potassium 
                                and other alkali metals, a lattice of positively charged atoms caged in 
                                neutral molecules is held together by trapped clouds of free-floating 
                                electrons. Various kinds of electrides can be made, whose electronic and 
                                optical properties range from those of nonmetals to those of metals.



   76                           Mimicry in Plants 
                                Spencer C. H. Barrett


                                Life can be a competitive struggle even for plants. In cultivated environ- 
                                ments some weeds imitate crops and thereby avoid being destroyed. In 
                                natural habitats orchids that mimic female insects attract male insects, 
                                which proceed to carry pollen from flower to flower. Some plants have 
                                even come to resemble stones and thus deter predators.

2

                                           © 1987 SCIENTIFIC AMERICAN, INC

86             The Swahili Corridor
               Mark Horton

               As 10th-century Europe awoke from the Dark Ages an energetic revival 
               of the arts generated a demand for such exotic materials as transparent 
               quartz, ivory and gold. To meet the demand Swahili traders plied the 
               waters of the East African coast, sailing 3,000 kilometers to forge a trade 
               route that reached all the way to the Mediterranean.


94             How Children Learn Words
               George A. Miller and Patricia M Gildea

               To attain the vocabulary of a high school graduate, some 80,000 words, a 
               child needs to learn new words at the rate of about 5,000 a year. The way 
               to aid the process is to exploit the ability of a child to learn by seeing 
               words in context. An interactive video display presenting definitions, 
               sample sentences and pictures may be the best teaching device.


100            Coal-fired Power Plants for the Future
               Richard E. Balzhiser and Kurt E. Yeager

               By the end of the century some 70 percent of the electricity generated in 
               the U.S. may come from coal-fired power plants. To improve their 
               efficiency and to reduce environmental impact new kinds of plants are 
               called for, based on fluidized-bed combustion and coal gasification; such 
               plants may also be able to yield sulfur in economically useful forms.



   DEPARTMENTS


 8 Letters                   108 The Amateur Scientist


 12         50 and 100       112         Computer 
            Years Ago                    Recreations

            September, 1887: The Thistle Social distance and the way 
            has arrived in New York to   to the refreshment table: 
            go after the America's Cup.  PARTY PLANNER figures it out.


 16 The Authors              116 Books


 18 Science and the Citizen  120 Bibliography





                        © 1987 SCIENTIFIC AMERICAN, INC

";
		}
	}
}
