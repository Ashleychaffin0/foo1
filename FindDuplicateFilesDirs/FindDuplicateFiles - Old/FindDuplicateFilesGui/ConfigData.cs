// Copyright (c) 2010 by Larry Smith

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FindDuplicateFilesGui {
	public class ConfigData {
		public List<string>		DirectoriesToInclude;
		public List<string>		DirectoriesToExclude;
		public List<string>		FileMasksToInclude;
		public List<string>		FileMasksToExclude;

		// Note: FileInfo gives file sizes in terms of long's, not ulongs, so we'll do
		//		things its way (even though the filesize *really* should be a ulong).
		[XmlIgnore]
		public long		MinSize, MaxSize;

		public int		ShiftFactor;		// Used for setting which radio button

//---------------------------------------------------------------------------------------

		public ConfigData() {
			DirectoriesToInclude = new List<string>();
			DirectoriesToExclude = new List<string>();
			FileMasksToInclude   = new List<string>();
			FileMasksToExclude   = new List<string>();

			ShiftFactor = 20;			// Assume MB
		}

//---------------------------------------------------------------------------------------

		public void SetupDefaults() {
			// Directories to include
			DirectoriesToInclude.Add(@"C:\");

			// Directories to exclude
			DirectoriesToExclude.Add(@"C:\Windows");
			// The TEMP directory. Find out what it is
			string temp = Environment.GetEnvironmentVariable("TEMP");
			if (temp != null)
				DirectoriesToExclude.Add(temp);

			// File masks to include
			// TODO: Replace next line *.pdf with *
			FileMasksToInclude.Add("*.pdf");

			// File masks to exclude
			FileMasksToExclude.Add("*.exe");
			FileMasksToExclude.Add("*.dll");
		}
	}
}
