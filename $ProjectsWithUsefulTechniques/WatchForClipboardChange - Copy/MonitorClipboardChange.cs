using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MonitorClipboardChange {
	public partial class MonitorClipboardChange : Form {
		private const int WM_DRAWCLIPBOARD = 0x0308;
		private const int WM_CHANGECBCHAIN = 0x030D;

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

		IntPtr NextViewer = IntPtr.Zero;

		static char[] NewLines = new char[] { '\r', '\n' };

		bool Monitoring = true;
		bool bRecursing = false;    // TODO: Why isn't this working?

		string LastClipboardText = "";

//---------------------------------------------------------------------------------------

		public MonitorClipboardChange() {
			InitializeComponent();

			NextViewer = SetClipboardViewer(this.Handle);
		}

//---------------------------------------------------------------------------------------

		protected override void WndProc(ref Message m) {
			base.WndProc(ref m);
			switch (m.Msg) {
				case WM_DRAWCLIPBOARD:
					if (bRecursing) {
						bRecursing = false;
					} else if (Monitoring && Clipboard.ContainsText()) {
						string Text = Clipboard.GetText();
						if (Text != LastClipboardText) {    // i.e. recursing?
							if (Text.Contains("ARTICLE")) { // SciAm stuff?
								var lines = ProcessText(Text);
								// MessageBox.Show(lines);
								bRecursing = true;
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

		private string ProcessText(string s) {
			var InputLines = s.Split(NewLines, StringSplitOptions.RemoveEmptyEntries);
			// We assume the format is as follows:
			//	*	Zero or more lines before "ARTICLES"
			//	*	Zero or more lines in the form <nnn> by <author(s)> <description>
			//	*	"DEPARTMENTS"
			//	*	One line with departments: <nn> text <nn> text ...
#if false // For example
SCIENTIFIC 
Established 1845 AMERICAN April 1967 Volume 216 Number 4 
ARTICLES 
21 THE SOCIAL POWER OF THE NEGRO, by James P. Comer The social progress of Negroes has been hindered by a deep division among them. 
28 THE INDUCTION OF CANCER BY VIRUSES, by Renato Dulbecco How do the viruses of known animal cancers cause cells to become cancerous? 
38 THE CHANGING HELICOPTER, by Alfred Gessow Larger craft and new applications are in prospect after 30 years of development. 
56 THE ANTIQUITY OF HUMAN WALKING, by John Napier An ancient toe bone shows that man's erect gait is more than a million years old. 
68 NEUTRON-ACTIVATION ANALYSIS, by W. H. Wahl and H. H. Kramer Tiny amounts of chemical elements are measured by making them radioactive. 
84 RIVERS IN THE MAKING, by H. F. Garner In many parts of the world the runoff flows in a curious network of channels. 
96 THE EVOLUTION OF BEE LANGUAGE, by Harald Esch Recent studies suggest that sound preceded dancing in the "talk" of the insects. 
106 ANTIMATTER AND COSMOLOGY, by Hannes Alfven A hypothesis indicates how matter and antimatter might coexist in the universe. DEPARTMENTS 
8 LETTERS 10 50 AND 100 YEARS AGO 14 THE AUTHORS 48 SCIENCE AND THE CITIZEN 116 MATHEMATICAL GAMES 124 THE AMATEUR SCIENTIST 135 BOOKS 146 BIBLIOGRAPHY 
BOARD OF EDITORS Gerard Piel (Publisher), Dennis Flanagan (Editor), Francis Bello (Associate Editor), Philip Morrison (Book Editor), John Purcell, James T. Rogers, Armand Schwab, Jr., C. L. Stong, Joseph Wisnovsky ART DEPARTMENT Jerome Snyder (Art Director), Samuel L. Howard (Assistant Art Director) PRODUCTION DEPARTMENT Richard Sasso (Production Manager), Hugh M. Main (Assistant Production Manager), Frank V. Musco COPY DEPARTMENT Sally Porter Jenks (Copy Chief), Dorothy Bates, Julio E. Xavier GENERAL MANAGER Donald H. Miller, Jr. ADVERTISING MANAGER Martin M. Davidson ASSISTANT TO THE PUBLISHER Stephen M. Fischer 
PUBLISHED MONTHLY BY SCIENTIFIC AMERICAN; INC., 415 MADISON AVENUE, NEW YORK, N.Y. 10017. COPYRIGHT ® 1967 BY SCIENTIFIC AMERICAN, INC. ALL RIGHTS RESERVED. SECOND-CLASS POSTAGE PAID AT NEW YORK, N.Y., AND AT ADDITIONAL MAILING OFFICES. AUTHORIZED AS SEC-OND-CLASS MAIL BY THE POST OFFICE DEPARTMENT, OTTAWA, CANADA, AND FOR PAYMENT OF POSTAGE IN CASH. SUBSCRIPTION RATE: $7 PER YEAR. 
3 
© 1967 SCIENTIFIC AMERICAN, INC 
#endif
			var OutputLines = new List<string>();
			// TODO: At some point, put the article names here, then generate "A" entries
			//		 for all of them. Then it's easier to just delete the ones I'm not
			//		 interested in.
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
			return string.Join("\r\n", OutputLines);
		}

//---------------------------------------------------------------------------------------

		private bool ProcessLine(string line, string[] InputLines, int i, List<string> OutputLines, List<string> ArticleNames) {
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
#if false
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
#if false   // I couldn't get the Regex to work!
				m = ReNorm.Match(s);
				if (m.Success) {
					OutputLines.Add(m.Value);
					s = RemoveSubstring(s, m.Index, m.Length);
				}
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
			BtnEnableDisable.Text = Monitoring ? "Disable" : "Enable";
		}
	}
}
