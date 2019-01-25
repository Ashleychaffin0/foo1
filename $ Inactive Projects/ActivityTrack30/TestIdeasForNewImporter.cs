// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;

namespace BBartizan.ActivityTrack30.Importer {


	public enum SSFProperty {
		Numeric,
		String,
		Boolean,
		DateTime,
		SSN,
		ZipCode,
		ZipCodePlusFive,
		// Possible Candian stuff, such as SIN and Postal Code
		// TODO:
	}

	public enum SSFConstraint {
		Mandatory,				// i.e. Can't be Null
		MaxFieldLength,
		MinValue,
		MaxValue,
		LimitToList,
		// TODO:
	}

	public enum SSFDBProperty {
		TableName,
		FieldName,
		// TODO:
	}

	/// <summary>
	/// Summary description for TestIdeasForNewImporter.
	/// </summary>
	public class StandardSetupFormat {

		public Hashtable			Fields;
		public Hashtable			Constraints;

		public StandardSetupFormat() {
			Fields = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
		}

		public class ImporterField {
			public string			Name;

			// Note: We could have had a single Hashtable, but it's a bit clearer to have several,
			//		 just as a bit of self-documentation.
			public Hashtable		Properties;
			public Hashtable		Constraints;
			public Hashtable		DatabaseProperties;
			
			public ImporterField() {
				// TODO: You'd think you could just create one new CIHCP, and one new CIC, and just
				//		 use them as often as you'd like. But until I'm sure...
				Properties  = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
				Constraints = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
				DatabaseProperties = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
		}
	}
}
