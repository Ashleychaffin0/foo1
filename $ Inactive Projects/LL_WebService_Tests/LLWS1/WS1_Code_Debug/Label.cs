// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Collections.Generic;

namespace Bartizan.Importer {
    public class Label {
        // TODO: FileNum should really be numeric. But until we resolve the issue
        //       of concatenated fields in the [FILE] section, we'll leave them
        //       here as strings.
        public string   FileNum;    // Numeric value the Box uses internally
        public string   Caption;

//---------------------------------------------------------------------------------------

        public Label(string line) {
            string [] fields = line.Split(new char[] { ' ' }, 2);
            // TODO: Check that we do indeed have two fields. For now, fake it
			if (fields.Length == 2) {
				FileNum = fields[0].Trim();
				Caption = fields[1].Trim();
			} else if (fields.Length == 1) {
				FileNum = fields[0].Trim();
				Caption = "";
			} else {
				FileNum = "";
				Caption = "";
			}
        }

//---------------------------------------------------------------------------------------

        public static List<Label> ParseSetupFile(BartIniFile suf) {
			List<Label> labels = new List<Label>();
            if (suf.FindSection("Labels") == false)
                return labels;			// i.e. Empty array
            string s;
            while ((s = suf.ReadLine()) != null) {
                s = s.Trim();
                if (s.Length != 0) {
					// Since label fields probably will be used as database table
					// field names, make sure the label has no invalid characters
					s.Replace(".", "");
					s.Replace("[", "");
					s.Replace("]", "");
					s.Replace(":", "");
					// TODO: Check to see if there are any we missed
                    labels.Add(new Label(s));
                } // else we've got an empty line, which we'll ignore
            }
            return labels;
        }
    }
}
