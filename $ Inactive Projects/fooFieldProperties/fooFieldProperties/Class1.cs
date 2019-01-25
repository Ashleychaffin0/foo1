using System;
using System.Collections;

namespace fooFieldProperties {

	public enum FieldType {
		Label,
		Text,
		DateTime,
		boolean,
		etc
	}

	public enum FieldSource {
		Access,
		Excel,
		etc
	}

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class FieldProperties {
		public FieldType	FieldType;
		public string		Filename;		// e.g. for Image
		public string		FieldName;		// For messages
		public string		FieldSource;	
		// And so on

		public FieldProperties(FieldType FieldType, string Filename) { // Other parms elided
			this.FieldType = FieldType;
			this.Filename  = Filename;
			// etc
		}
	}

	public class ControlDef_Base {
		public FieldProperties GetFieldProperties() {
			return new FieldProperties(FieldType.Text, "foo");	// Do better here
		}
	}

	public class foo {
		ControlDef_Base []	CDefs = new ControlDef_Base[10];
		ArrayList	Props = new ArrayList();

		void Process() {
			foreach (ControlDef_Base cdb in Cdefs) {
				Props.Add(cdb.GetFieldProperties());
			}
		}
	}
}
