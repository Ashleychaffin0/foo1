using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace foo5 {

	public class MyClass {
		public int			i;
		public DateTime		dt;
		public string		s;
		public ArrayList	al;

		public MyClass(int i, DateTime dt, string s, ArrayList al) {
			this.i	= i;
			this.dt = dt;
			this.s	= s;
			this.al	= al;
		}

		public MyClass() {
		}
	}

	public class MyClass_2 {
		public List<MyClass>	classes;

		public MyClass_2() {
			classes = new List<MyClass>();
		}

		public void Add(MyClass mc) {
			classes.Add(mc);
		}
	}

	class Program {
		static void Main(string[] args) {

			ArrayList al1 = new ArrayList();
			al1.AddRange(new string[] {"Now", "is", "the", "time"});
			MyClass	mc1 = new MyClass(2, DateTime.Now, "Hello", al1);

			ArrayList al2 = new ArrayList();
			al1.AddRange(new object[] {"For", "all", 3, "good", "men"});
			MyClass mc2 = new MyClass(42, DateTime.Now + new TimeSpan(1, 0, 0), "world", al2);

			MyClass_2	mc_2 = new MyClass_2();
			mc_2.Add(mc1);
			mc_2.Add(mc2);

			GenericSerialization<MyClass_2>.Save("C:\\LRS\\MyClass.xml", mc_2);
			// GenericSerialization<MyClass>.Save("MyClass.xml", mc1);
		}
	}

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
				//		So I guess the only real solution is to *not* catch the Exception
				//		and let the user put this into his/her own try/catch. Oh well...
				// MessageBox.Show("Could not load " + FileName + " - Error text: " + ex.Message, "LRSS", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				// MessageBox.Show("Error (" + ex.Message + ") saving " + FileName, "LRSS", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} finally {
				if (sw != null)
					sw.Close();
			}
		}
	}

}
