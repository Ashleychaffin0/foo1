using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace PlayWithXML_1 {

	public class GenericSerialization<TypeToBeSerialized> {

//---------------------------------------------------------------------------------------

		public static TypeToBeSerialized Load(string FileName) {
			StreamReader sr = null;
			try {
				sr = new StreamReader(FileName);
				XmlSerializer xs = new XmlSerializer(typeof(TypeToBeSerialized));
				object o = xs.Deserialize(sr);
				return (TypeToBeSerialized)o;
			} catch (Exception ex) {
				// TODO: I don't really like this MessageBox here, for several reasons.
				//		 * For one thing, the title (LRSS) is hard-coded.
				//		 * For another, the caller might not want a MessageBox shown,
				//		   (e.g. for console apps).
				//		 * It would be nice if the text, buttons and/or icons were 
				//		   customizable.
				//		 * Finally, and perhaps most importantly, the true cause of the 
				//		   inability to [de]serialize might be buried in several levels 
				//		   of InnerException's, and as it stands, the caller has no
				//		   access to these possibly crucial messages.
				//		 OTOH, I don't like to force the user to specify all these
				//		 parameters, so I'm going to leave things as-is for now. As for
				//		 the inner exceptions, that should show up during development,
				//		 and it should be easy enough to set a breakpoint here and look
				//		 at the nested exceptions.
				MessageBox.Show("Could not load " + FileName + " - Error text: " + ex.Message, "LRSS", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return default(TypeToBeSerialized);
			} finally {
				if (sr != null)
					sr.Close();
			}
		}

		//---------------------------------------------------------------------------------------

		public static void Save(string FileName, TypeToBeSerialized obj) {
			StreamWriter sw = null;
			try {
				sw = new StreamWriter(FileName);
				XmlSerializer xs = new XmlSerializer(typeof(TypeToBeSerialized));
				xs.Serialize(sw, obj);
			} catch (Exception ex) {
				// TODO: See comments in the Load method
				MessageBox.Show("Error (" + ex.Message + ") saving " + FileName, "LRSS", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				if (sw != null)
					sw.Close();
			}
		}
	}
}
