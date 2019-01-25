// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections.Generic;
using System.Text;

// The [FILE] (aka the [SERIALSTREAM]) section of the MAP.CFG file is basically a
// List<int> of field codes (including the possibility of user-defined codes). But to
// complicate matters, fields can be concatenated. For example, "1 2 3 4,5,6 7". This
// would represent 5 fields, the fourth being a triplet of the field codes 4, 5 and 6.

// So the FileFields class contains a vector of FieldCodeList's, each of which is a
// vector of int's. Note that in most cases, an instance of a FieldCodeList will have
// only a single int in its List<>.

namespace Bartizan.Importer {
	public class FileFields {
		public List<int>		FieldCodes;

//---------------------------------------------------------------------------------------

		public FileFields(List<int> Codes) {
			FieldCodes = Codes;
		}

//---------------------------------------------------------------------------------------

		public static List<FileFields> ParseSetupFile(BartIniFile suf) {
			List<FileFields> Fields = new List<FileFields>();
			// The <file> List<string> has been set to empty, so if any of our
			// checks fail, just return.

			// The [File] section exists if the data is on a floppy. But if it came
			// from either the serial port or the Ethernet connection, it's called
			// [SerialStream]. So check for either. Note: If both sections are present,
			// I suppose we could throw an exception. But we'll just take the first one
			// we find.
			// TODO: Can this also be called [AT-Mobile]?
			bool bFoundit;
			bFoundit = suf.FindSection("File");
			if (!bFoundit) {
				bFoundit = suf.FindSection("SerialStream");
			}
			if (!bFoundit)
				return Fields;				

			// Read first (if any) line, and ignore the rest (if any)

			string s;
			if ((s = suf.ReadLine()) == null)
				return Fields;				

			// Now get the pieces. Cupla comments. 1) We do a simple Split on ' '. 
			// However, if there are extra blanks in the string (e.g. "1   3 4  5"),
			// there will be extra empty ("") fields. So we have to take those out.
			// And 2), we tried to use a Regex to do the Split for us, but that didn't
			// really pan out. Finally 3), we could have used a Regex to get the fields
			// out, without worrying about empty strings et al, but ya know what? With
			// all the extra work to set up the Regex string, worry about Matches and
			// Captures and so on, it was easier, simpler and clearer just to do it
			// this way.
			List<string>	flds;
			flds = new List<string>(s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
#if false	// Code before I found the RemoveEmptyEntries parm
			int count;
			// Remove the "" entries. Keep looping until we don't find one to remove.
			do {
				count = flds.Count;
				flds.Remove("");
			} while (count != flds.Count);	// Count remains the same if none found
#endif

			// OK, we now have a vector of strings, something like "1", "2", "3,4,5", "6".
			// For each string, create a FileField.

			foreach (string fld in flds) {
				List<int>	Field = GetFields(fld);
				Fields.Add(new FileFields(Field));
			}

			// OK, at this point we have the fields stripped off. But there's some 
			// logical (semantic) processing to be done, namely that 
			return Fields;
		}

//---------------------------------------------------------------------------------------

		static List<int> GetFields(string s) {
			List<int>	FieldCodeNumbers = new List<int>();
			// We expect a string of the form "1,2,3"
			string [] flds = s.Split(',');
			// TODO: Do better (i.e. any) checking later
			int		n;
			bool	bOK;
			foreach (string fld in flds) {
				bOK = int.TryParse(fld, out n);
				if (bOK) {
					FieldCodeNumbers.Add(n);
				} else {
					// TODO: Uh, do something here
				}
			}
			return FieldCodeNumbers;
		}
	}
}
