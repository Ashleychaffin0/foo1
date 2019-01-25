// TODO: Support for <n>-letter words (n = 4, 5, 6, 7, etc)
// TODO: Support for presenting words of different lengths in a session
// TODO: Add support to name an output file
// TODO: Add support to open one of several Gutenberg in a web browser
//		 .org, .ca, .au
//		http://www.gutenberg.org
//		http://www.gutenberg.ca/
//		http://gutenberg.net.au/
//		http://freeread.com.au/index.html -- Roy Glashan's Library
// TODO: I suppose we can have a .xml config file with, e.g. list of
//		 Gutenberg sites, authors inside them, etc. See below.

// TODO: Make suggestions for authors (w/links)
//		Doc Smith
//		Dorothy Sayers
//		G.K. Chesterton
//		Conan Doyle
//		Charles Dickens
//		Jane Austen
//		Ian Fleming
//		Dashiell Hammett
//		Ermest Hemingway
//		Mark Twain
//		etc

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Build6LetterWords {
	public partial class Build6LetterWords : Form {
		HashSet<string> Words;
		StringBuilder sb;

//---------------------------------------------------------------------------------------

		public Build6LetterWords() {
			InitializeComponent();

			// Hash words the same that differ only in case (e.g. "dog" and "Dog")
			Words = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
			sb = new StringBuilder();
		}

//---------------------------------------------------------------------------------------

		private void Build6LetterWords_Load(object sender, EventArgs e) {
			// TODO: Don't default here
			TxtInputFile.Text = @"D:\lrs\Pride and Prejudice -- 1342-0.txt";
			TxtInputFile.Text = "http://www.gutenberg.org/ebooks/42671.txt.utf-8";
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			var lines = ReadSource(TxtInputFile.Text);
			// TODO: Do error checking on input file -- exists, etc
			var BlankSep = new char[] { ' ' };
			foreach (string l in lines) {
				string line = Clean(l);
				var wds = line.Split(BlankSep, StringSplitOptions.RemoveEmptyEntries);
				foreach (var word in wds) {
					if (word.Length == 6) {
						Words.Add(word);
					}
				}
			}
			var WordsSorted = Words.OrderBy(word => word);
			MessageBox.Show($"Words.Count == {Words.Count}");
		}

//---------------------------------------------------------------------------------------

		private string[] ReadSource(string text) {
			// TODO: Add error checking (e.g. file or URL not found)
			string s;
			string name = text.ToUpper();	// Upper-case for next line
			if (name.StartsWith("HTTP://") || name.StartsWith("HTTPS://")) {
				var wc = new WebClient();
				s = wc.DownloadString(text);
			} else {
				s = File.ReadAllText(text);
			}
			var lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			return lines;
		}

//---------------------------------------------------------------------------------------

		// Replace all non-alphabetics by blanks
		private string Clean(string s) {
			sb.Clear();
			foreach (char c in s) {
				if (char.IsLetter(c) || c == ' ') {
					sb.Append(c);
				} else {
					// Take care of things like <ma'am?--is> in Pride and Prejudice
					// giving the word "maamis".
					sb.Append(' ');
				}
			}
			return sb.ToString();
		}
	}
}
