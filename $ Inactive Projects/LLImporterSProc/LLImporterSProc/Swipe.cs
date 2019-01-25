// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Text.RegularExpressions;

namespace Bartizan.Importer {
	/// <summary>
	/// This represents a single line from a VISITOR.TXT file.
	/// </summary>
	public class Swipe {

		public string []	Fields;				// The parsed (but still raw) data

		Regex		re = null;

//---------------------------------------------------------------------------------------

		public Swipe() {
			if (re == null) {
				string s;
				// The following is the line as (more or less) originally written
				// s = @"^(""(?<SeqNo>.*?)"")(,""(?<FieldCodes>.*?)"")*\s*$";
				// A commented version follows
				s = @"^(""(?<SeqNo>.*?)"")	# The beginning of the string (the ^)
										# A quoted field, named SeqNo.
										# Note .*?, not .*. Pick up the fewest
										# characters possible, not the maximal,
										# or we'll gobble up the rest of the line.
					# Again a quoted field, but this time with a leading comma.
					# We have zero or more of these. The actual contents inside the
					# quotes we call Fields.
					# We'll also allow white space at the end of the line.
					# We'll follow this with end-of-line (the $) to make sure there's
					# no garbage at the end (e.g. ...,'last parm'garbage)
					(,""(?<Fields>.*?)"")*\s*$";
				re = new Regex(s, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			}

			Fields = null;
		}

//---------------------------------------------------------------------------------------

		public bool Parse(string line) {
			Match	m;
			m = re.Match(line);
			if (m.Success) {
				string	SeqNo = (string)m.Groups["SeqNo"].Value;
				CaptureCollection	Caps  = m.Groups["Fields"].Captures;
				int					nCaps = Caps.Count;
				Fields = new string[nCaps + 1];		// +1 for SeqNo
				Fields[0] = SeqNo;
				for (int i=0; i<nCaps; ++i) {
					Fields[i + 1] = Caps[i].Value;
				}
#if false			// TODO: Debug
				for (int i=0; i<m.Groups["Fields"].Captures.Count+1; ++i) {
					Console.WriteLine("Fields[{0}]={1}", i, Fields[i]);
				}
#endif
				return true;
			} else {
				return false;
			}
		}
	}
}
