// #define USE_REGEX		// OBSOLETE. Do NOT uncomment

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Bartizan.Input.Utils;

using LRS;
using LRSCDDB;

namespace TestOldParseSwipe {
	class Program {
		static void Main(string[] args) {
#if false
			List<int[]> vals; 
			vals = LRSScanRun.Scan("1,3,3-12, 4*2, 12");
			int cnt = LRSScanRun.Count(vals);
			// vals = LRSScanRun.Scan("-1,3-,3-12, 4*2, 12");
			foreach (var s in LineFile.ReadLinesFromFile(@"C:\LRS\ASCIIChart.txt")) {
				Console.WriteLine("{0}", s);
			}
#endif

#if false
			List<string>	fields;
			string s = "'hello','world''";
			BartCSV.Parse(s.Replace('\'', '"'), out fields);
			s = "'abc','def','gh''";
			BartCSV.Parse(s.Replace('\'', '"'), out fields);
			s = "'abc','de'','ghi";
			BartCSV.Parse(s.Replace('\'', '"'), out fields);
			s = "'abc','de'','ghi";
			BartCSV.Parse(s.Replace('\'', '"'), out fields, true);
		}
#endif

	}

	/// <summary>
	/// This represents a single line from a VISITOR.TXT file.
	/// </summary>
	public class Swipe {

		public string[] Fields;				// The parsed (but still raw) data

		public int RecNo;				// First field is record (not seq) number
		public bool bIsRecNoValid;		// Numeric, >0, etc

#if USE_REGEX
		Regex				re = null;
#endif

//---------------------------------------------------------------------------------------

		public Swipe() {
			RecNo = -1;
			bIsRecNoValid = false;

#if USE_REGEX
			if (re == null) {
				string s;

				// In October 2007 we needed to change the parsing. Instead of the boxes
				// replacing double-quotes ("s) with single quotes ('s), before putting
				// text into the Visitor file, the double-quotes were left intact. Sigh.

				// So now we're going to compromise. A Visitor file is no longer a
				// comma-separated list of quoted fields. It's a list of text, separated
				// by doublequote-comma-doublequote delimeters, with, oh yes, a double
				// quote at the beginning and a double quote at the end.

				// One of the main reasons I don't like this definition is that the user
				// can, in principle, validly include data with that 3-character sequence
				// and screw up our processing. So it's another trip to kludge city.

				s  = @"^""";						// Beginning (double) quote
				s += @"((?<Fields>[^""]*?)"","")*";	// Zero or more strings without 
													// a quote, but followed by the
													// quote-comma-quote delimeter
				s += @"(?<LastField>.*)";			// The last field doesn't end with
													// quote-comma-quote, so has to be
													// handled separately. Note though
													// that this will include the
													// trailing double-quote, so we'll
													// need to strip it off when we
													// get it.


				// Note: The following commentary applies to the original Regex (quoted
				//		 strings separated by commas). I've left it in because it
				//		 documents a major (major!) problem we had with the performance
				//		 of the first Regex we tried for parsing. READ IT IF YOU EVER
				//		 FIND ONE OF YOUR REGEX's TAKING *WAY* TOO MUCH CPU TIME!

				// This is semi-amazing. The following commented-out code was what I'd
				// originally written for the regex to parse a fully-quoted CSV line.
				// But if the line were mal-formed (e.g. a trailing comma), and the line
				// was long (as Visitor.txt lines tend to be), then the regex would
				// take ***forever***. Like maybe 10 minutes of CPU time to scan the 
				// text before deciding it was bad.

				// Then we got Jeff Friedl's Mastering Regular Expressions book, and
				// just flipping through it I noticed he mentioned that character
				// classes (e.g. "[^0-9]") could be *much* more efficient than ".".
				// Something to do with subtle implications of when backtracking was
				// invoked, and to what extent. Maybe after I've actually *read* the
				// book I can elaborate on this. However, when I tried the character
				// class technique, the regex that took over 10 minutes now took just
				// over 1/100th of a second. A speedup factor of 10,000+!!!

				// So here's the original regex. I've commented it out rather than
				// #if'ing it out, since the #-comments confuse the compiler.
				/*
				// The following is the line as (more or less) originally written
				// s = @"^(""(?<SeqNo>.*?)"")(,""(?<FieldCodes>.*?)"")*\s*$";
				// A commented version follows. 
				// NOTE: Since we've now commented out this block of code using #if,
				//		 the pound signs (comments) in the regex's below confuse the poor
				//		 little C-Sharp (whoops, almost used a pound sign there!)
				//		 compiler. So we'll replace all pound signs with dollar signs ($)
				//		 and hope that people won't confuse this with the 
				//		"start-of-string" regex metacharacter.
				s = @"^(""(?<RecNo>.*?)"")	$ The beginning of the string (the ^)
										$ A quoted field, named RecNo.
										$ Note .*?, not .*. Pick up the fewest
										$ characters possible, not the maximal,
										$ or we'll gobble up the rest of the line.
					$ Again a quoted field, but this time with a leading comma.
					$ We have zero or more of these. The actual contents inside the
					$ quotes we call Fields.
					$ We'll also allow white space at the end of the line.
					$ We'll follow this with end-of-line (the $) to make sure there's
					$ no garbage at the end (e.g. ...,'last parm'garbage)
					(,""(?<Fields>.*?)"")*\s*$";
				 */

				// OK, here's the updated regex. It's exactly the same as the above, but 
				// the two instances of "." are replaced with "[^""]".
				// The following is the line as (more or less) originally written
				// s = @"^(""(?<SeqNo>[^""]*?)"")(,""(?<FieldCodes>[^""]*?)"")*\s*$";
				// A commented version follows
/*		// Original Regex (i.e. comma-separated quoted strings)
				s = @"^(""(?<RecNo>[^""]*?)"")	# The beginning of the string (the ^)
										$ A quoted field, named RecNo.
										$ Note the ? Pick up the fewest
										$ characters possible, not the maximal,
										$ or we'll gobble up the rest of the line.
					$ Again a quoted field, but this time with a leading comma.
					$ We have zero or more of these. The actual contents inside the
					$ quotes we call Fields.
					$ We'll also allow white space at the end of the line.
					$ We'll follow this with end-of-line (the $) to make sure there's
					$ no garbage at the end (e.g. ...,'last parm'garbage)
					(,""(?<Fields>[^""]*?)"")*\s*$";
*/

				re = new Regex(s, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			}
#endif
			Fields = null;
		}

#if ! USE_REGEX
		//---------------------------------------------------------------------------------------

		public bool Parse(string line, List<string> msgs) {
			List<string> vals;
			BartCSV.Parse(line, out vals);
			Fields = vals.ToArray();
			bIsRecNoValid = int.TryParse(Fields[0], out RecNo);
			return true;
		}
#endif

#if USE_REGEX	// Previous regex-based parser
//---------------------------------------------------------------------------------------

		public bool Parse(string line, List<string> msgs) {
			Match	m;
			m = re.Match(line);
			if (m.Success) {
				CaptureCollection Caps = m.Groups["Fields"].Captures;
				CaptureCollection CapsLast = m.Groups["LastField"].Captures;
				string sRecNo = Caps[0].Value;
				bIsRecNoValid = int.TryParse(sRecNo, out RecNo);
				int nCaps = Caps.Count;
				Fields = new string[nCaps + 1];		// +1 for LastField
				for (int i = 0; i < nCaps; ++i) {
					Fields[i] = Caps[i].Value;
				}
				string	LastField = CapsLast[0].Value;
				if (LastField.EndsWith("\"")) {		// See comments in <re> construction
					LastField = LastField.Substring(0, LastField.Length - 1);
				}
				Fields[nCaps] = LastField;
#if false			// Debug
				for (int i=0; i<Fields.Length; ++i) {
					Console.WriteLine("Fields[{0}]={1}", i, Fields[i]);
				}
#endif
				return true;
			} else {
				return false;
			}
		}
#endif

#if USE_REGEX
//---------------------------------------------------------------------------------------
		//		Previous parse routines
				public void  OldSwipe() {		// The old ctor
					RecNo = -1;
					bIsRecNoValid = false;
					if (re == null) {
						string s;

				// This is semi-amazing. The following commented-out code was what I'd
				// originally written for the regex to parse a fully-quoted CSV line.
				// But if the line were mal-formed (e.g. a trailing comma), and the line
				// was long (as Visitor.txt lines tend to be), then the regex would
				// take ***forever***. Like maybe 10 minutes of CPU time to scan the 
				// text before deciding it was bad.

				// Then we got Jeff Friedl's Mastering Regular Expressions book, and
				// just flipping through it I noticed he mentioned that character
				// classes (e.g. "[^0-9]") could be *much* more efficient than ".".
				// Something to do with subtle implications of when backtracking was
				// invoked, and to what extent. Maybe after I've actually *read* the
				// book I can elaborate on this. However, when I tried the character
				// class technique, the regex that took over 10 minutes now took just
				// over 1/100th of a second. A speedup factor of 10,000+!!!

				// So here's the original regex. I've commented it out rather than
				// #if'ing it out, since the #-comments confuse the compiler.
				/*
				// The following is the line as (more or less) originally written
				// s = @"^(""(?<SeqNo>.*?)"")(,""(?<FieldCodes>.*?)"")*\s*$";
				// A commented version follows.
 * 				// NOTE: Since we've now commented out this block of code using #if,
				//		 the pound signs (comments) in the regex's below confuse the poor
				//		 little C-Sharp (whoops, almost used a pound sign there!)
				//		 compiler. So we'll replace all pound signs with dollar signs ($)
				//		 and hope that people won't confuse this with the 
				//		"start-of-string" regex metacharacter.

						s = @"^(""(?<RecNo>.*?)"")	$ The beginning of the string (the ^)
												$ A quoted field, named RecNo.
												$ Note .*?, not .*. Pick up the fewest
												$ characters possible, not the maximal,
												$ or we'll gobble up the rest of the line
							$ Again a quoted field, but this time with a leading comma.
							$ We have zero or more of these. The actual contents inside
							$ the quotes we call Fields.
							$ We'll also allow white space at the end of the line.
							$ We'll follow this with end-of-line (the $) to make sure
							$ there's no garbage at the end (e.g. ...,'last parm'garbage)
							(,""(?<Fields>.*?)"")*\s*$";
						 */
		/*

	// OK, here's the updated regex. It's exactly the same as the above, but 
	// the two instances of "." are replaced with "[^""]".
	// The following is the line as (more or less) originally written
	// s = @"^(""(?<SeqNo>[^""]*?)"")(,""(?<FieldCodes>[^""]*?)"")*\s*$";
	// A commented version follows
	s = @"^(""(?<RecNo>[^""]*?)"")	$ The beginning of the string (the ^)
							$ A quoted field, named RecNo.
							$ Note the ? Pick up the fewest
							$ characters possible, not the maximal,
							$ or we'll gobble up the rest of the line.
		$ Again a quoted field, but this time with a leading comma.
		$ We have zero or more of these. The actual contents inside the
		$ quotes we call Fields.
		$ We'll also allow white space at the end of the line.
		$ We'll follow this with end-of-line (the $) to make sure there's
		$ no garbage at the end (e.g. ...,'last parm'garbage)
		(,""(?<Fields>[^""]*?)"")*\s*$";
				 
	re = new Regex(s, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
}

Fields = null;
}
*/
#endif

#if false			// Previous parse routine

//---------------------------------------------------------------------------------------

		public bool OldParse(string line, List<string> msgs) {
			Match	m;
			m = re.Match(line);
			if (m.Success) {
				string sRecNo = (string)m.Groups["RecNo"].Value;
				bIsRecNoValid = int.TryParse(sRecNo, out RecNo);
				CaptureCollection Caps = m.Groups["Fields"].Captures;
				int nCaps = Caps.Count;
				Fields = new string[nCaps + 1];		// +1 for RecNo
				Fields[0] = sRecNo;
				for (int i = 0; i < nCaps; ++i) {
					Fields[i + 1] = Caps[i].Value;
				}
#if false			// Debug
				for (int i=0; i<m.Groups["Fields"].Captures.Count+1; ++i) {
					Console.WriteLine("Fields[{0}]={1}", i, Fields[i]);
				}
#endif
				return true;
			} else {
				return false;
			}
		}
#endif
	}

	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------

	public static class LRSScanRun {
		// Sorta kludgy (especially the error reporting), but what the hell, it's for
		// a one-of.
		public static List<int[]> Scan(string line) {
			var result = new List<int[]>();
			string[] vals = line.Split(',');
			foreach (var val in vals) {
				int From, To, Count;
				ParseItem(val, out From, out To, out Count);
				result.Add(new int[] { From, To, Count });
			}
			return result;
		}

		//---------------------------------------------------------------------------------------

		private static void ParseItem(string val, out int From, out int To, out int Count) {
			Count = 1;		// Assume only 1
			string[] s = val.Trim().Split('-');
			if (s.Length == 0 || s.Length > 2) {
				throw new ArgumentException("Found span != 1 or 2 at " + val);
			}
			for (int i = 0; i < s.Length; i++) {
				s[i] = s[i].Trim();			// Trim everything (i.e. one or both)
				if (s[i].Length == 0) {
					throw new ArgumentException("Found empty span: " + val);
				}
			}
			// Must have at least 1 item
			s[0] = CheckForMultiples(s[0], ref Count);
			From = GetVal(s[0], val);
			To = From;
			if (s.Length == 2) {
				To = GetVal(s[1], val);
			}
		}

		//---------------------------------------------------------------------------------------

		private static int GetVal(string p, string item) {
			int val;
			bool bOK = int.TryParse(p, out val);
			if (!bOK) {
				throw new ArgumentException("Invalid numeric: " + p);
			}
			return val;
		}

		//---------------------------------------------------------------------------------------

		private static string CheckForMultiples(string p, ref int Count) {
			string[] s = p.Split('*');
			if (s.Length == 1) {
				return p;
			}
			if (s.Length > 2) {		// Can't be == 0; we've Trim'ed above
				throw new ArgumentException("Found mult, e.g., 2*3*5 in " + p);
			}
			Count = GetVal(s[1], p);
			return s[0];
		}

		//---------------------------------------------------------------------------------------

		public static int Count(List<int[]> Run) {
			int n = 0;
			foreach (var item in Run) {
				n += (item[1] - item[0] + 1) + (item[2] - 1);	// (To - From + 1) + (Count - 1)
			}
			return n;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	static class LineFile {

//---------------------------------------------------------------------------------------

		public static IEnumerable<string> ReadLinesFromFile(string filename) {
			using (StreamReader reader = new StreamReader(filename)) {
				while (true) {
					string s = reader.ReadLine();
					if (s == null)
						break;
					yield return s;
				}
			}
		}
	}
}
