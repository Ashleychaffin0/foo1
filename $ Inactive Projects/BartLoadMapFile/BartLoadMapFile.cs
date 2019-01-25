using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Bartizan.BartTrack {
	/// <summary>
	/// Summary description for BartGetMapFile.
	/// </summary>
	public class BartGetMapFile {

		public Hashtable		Labels;
		public string []		Fields;
		public StringCollection	Services;

		StreamReader			sr;

//---------------------------------------------------------------------------------------

		public BartGetMapFile(string filename)	{
			try {
				string		line;
				FileStream	fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				
				sr = new StreamReader(fs);
				while ((line = sr.ReadLine()) != null) {
					line = line.Trim();
					Console.WriteLine("line - " + line);
					if (line.Length > 0 && line.Substring(0, 1) == "[") {
						ProcessSection(line.Trim());
					}
				}
				sr.Close();
				fs.Close();
			} catch (FileNotFoundException e) {
				// TODO:
				throw;
			} catch {
				throw;
			}

			dbgDumpCollections();
		}

//---------------------------------------------------------------------------------------

		void dbgDumpCollections() {
			// TODO: Dump labels
			Console.WriteLine("EOF -- Dump tables\n");
			Console.WriteLine("LABELS ----------------------------");
			foreach (string ServiceNumber in Labels.Keys) {
				Console.WriteLine("Label[{0}] = {1}", ServiceNumber, Labels[ServiceNumber]);
			}
			// TODO: Dump Fields
			Console.WriteLine("FIELDS ----------------------------");
			foreach (string s in Fields) {
				Console.Write("{0,3} ", s);
			}
			Console.WriteLine("");
			// TODO: Dump Services
			Console.WriteLine("SERVICES ----------------------------");
			foreach (string ServiceName in Services) {
				Console.WriteLine("Service = {0}", ServiceName);
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessSection(string SectionID) {

			switch(SectionID) {
			case "[LABELS]":
				ProcessLabels();		// e.g. "1 ID #"
				break;
			case "[FILE]":
				ProcessFile();
				break;
			case "[QUESTIONS]":
				ProcessQuestions();
				break;
			default:
				EatUnknownSection(SectionID);
				break;
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessLabels() {
			string		line;
			string []	KeyAndVal;

			Labels = new Hashtable();		// Assume only one [LABELS] section
			while ((line = sr.ReadLine()) != null) {
				line = line.Trim();
				if (line.Length == 0)
					break;
				KeyAndVal = line.Split(new char[] {' '}, 2);
				Labels[KeyAndVal[0]] = KeyAndVal[1];;
			}
		}

//---------------------------------------------------------------------------------------
#if false
		void ProcessLabels_old_using_Regex() {
			string				line;
			Regex				re;
			Match				m;
			GroupCollection		g;

			Labels = new Hashtable();		// Assume only one [LABELS] section
			re = new Regex(@"([0-9]+)\s+(.*)");
			while ((line = sr.ReadLine()) != null) {
				line = line.Trim();
				if (line.Length == 0)
					break;
				m = re.Match(line);
				g = m.Groups;
				if (g.Count != 3) {
					// TODO: Some kind of error. For now, ignore
				} else {
					Labels[int.Parse((string)g[1].Value)] = (string)g[2].Value;
				}
			}
		}
#endif

//---------------------------------------------------------------------------------------

		void ProcessFile() {
			string		line;

			while ((line = sr.ReadLine()) != null) {
				// Really should have exactly one line here, but don't check for now
				line = line.Trim();
				if (line.Length == 0)
					return;	
				Fields = line.Split(new char[] {' '});
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessQuestions() {
			string	line;

			Services = new StringCollection();
			while ((line = sr.ReadLine()) != null) {
				line = line.Trim();
				if (line.Length == 0)
					return;	
				Services.Add(line);
			}
		}

//---------------------------------------------------------------------------------------

		void EatUnknownSection(string SectionID) {
			string	line;

			Console.WriteLine("Unrecognized section name - {0}", SectionID);
			while ((line = sr.ReadLine()) != null) {
				line = line.Trim();
				if (line.Length == 0)
					return;	
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class BartGetSwipeData {
		public ArrayList	ImportData;

		StreamReader	sr;
		Regex			re = new Regex("\",\"");

		public BartGetSwipeData(string filename) {
			FileStream	fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			sr = new StreamReader(fs);
			string		line;
			string []	LineFields;

			ImportData = new ArrayList();

			while ((line = sr.ReadLine()) != null) {
				line = line.Trim();
				Console.WriteLine(line);
				LineFields = re.Split(line);
				// The first field has an extra " at the front, and the last has a 
				// trailing ". Strip them off.
				if (LineFields.Length > 0) {
					// The leading " on the first element is easy
					if (LineFields[0].StartsWith("\""))
						LineFields[0] = LineFields[0].Substring(1);
					// The trailing " on the last element is a bit messier
					if (LineFields[LineFields.Length - 1].EndsWith("\"")) {
						int		len = LineFields[LineFields.Length - 1].Length;
						LineFields[LineFields.Length - 1] = 
							LineFields[LineFields.Length - 1].Substring(0, len - 1);
					}
				}
				ImportData.Add(LineFields);
			}
			sr.Close();
			fs.Close();
		}
	}
}
