using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TestXmlSerializationWithAttributes {

//---------------------------------------------------------------------------------------

	public class IronPythonScript {
		// Note: This is a separate class mainly so we can extend the tag with
		//		 attributes.
		[XmlAttribute]
		public string	ScriptType;				// s/b either Text or Filename
												// TODO: An XML Schema would come
												//		 in handy here, to ensure
												//		 this field has only
												//		 valid values
		// TODO: Only ScriptType = Text is currently supported. This field is 
		//		 currently unused
		public string	Text;					// Either the script itself, or
												//   the name of a file
												//	 containing the script

//---------------------------------------------------------------------------------------

		public IronPythonScript() {
			ScriptType = "Text";
			Text = "print \"Hello\"";
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class xxxIronPythonScript {
		[XmlAttribute]
		public int		int1;
		public string	string1;
	}

	public class Test {
		
		static string	filename = "test.xml";

		private void Serialize() {
			IronPythonScript data = new IronPythonScript();
			// data.int1 = 42;
			// data.string1 = "Forty-two";
			using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.ASCII)) {
				XmlSerializer xsr = new XmlSerializer(typeof(IronPythonScript));
				// We've had problems with the last few bytes of the previous version
				// of the file being kept, making the xml file as a whole invalid.
				// So explicitly truncate the file to length zero to try to avoid this.
				sw.BaseStream.SetLength(0);
				xsr.Serialize(sw, data);
			}
		}

		private void Deserialize() {
			using (Stream s = File.OpenRead(filename)) {
				XmlSerializer ser = new XmlSerializer(typeof(IronPythonScript));
				IronPythonScript  data2 = (IronPythonScript)ser.Deserialize(s);
			}
		}

		public void Run() {
			try {
				Serialize();
				Deserialize();
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}

	}

	class Program {
		static void Main(string[] args) {

			Test test = new Test();
			test.Run();
		}
	}
}
