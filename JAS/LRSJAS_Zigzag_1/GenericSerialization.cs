using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LRSUtils {
	public class GenericSerialization<TypeToBeSerialized> {

//---------------------------------------------------------------------------------------

		public static TypeToBeSerialized Load(string FileName) {
			// Note: This routine could blow up for any number of reasons, such as
			//		 the input file not existing. Originally I had a try/catch in here,
			//		 but I didn't really know what to do if it did fail. So the code
			//		 here doesn't do any error recovery at all. It's the responsibility
			//		 of the caller to throw a try/catch around invoking this, and at
			//		 that point s/he can do what is appropriate.
			using (StreamReader sr = new StreamReader(FileName)) {
				XmlSerializer xs = new XmlSerializer(typeof(TypeToBeSerialized));
				object o = xs.Deserialize(sr);
				return (TypeToBeSerialized)o;
			}
		}

//---------------------------------------------------------------------------------------

		public static void Save(string FileName, TypeToBeSerialized obj) {
			// See comments about try/catch in the Load method.
			using (StreamWriter sw = new StreamWriter(FileName)) {
				XmlSerializer xs = new XmlSerializer(typeof(TypeToBeSerialized));
				xs.Serialize(sw, obj);
			}
		}
	}
}
