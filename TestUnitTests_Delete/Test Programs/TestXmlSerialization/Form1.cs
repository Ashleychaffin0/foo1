using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

using System.Xml.Serialization;

namespace TestXmlSerialization {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			MyClass	mc1 = new MyClass();
			mc1.ID	= 387;
			mc1.FieldName = "First Name";
			mc1.FieldValue = "Larry";
			MyClass	mc2 = new MyClass();
			mc2.ID = 544;
			mc2.FieldName = "Last Name";
			mc2.FieldValue = "Smith";

			List<MyClass>	lst = new List<MyClass>();
			lst.Add(mc1);
			lst.Add(mc2);
			
			string s = ObjectToXml(lst);

			List<MyClass> lst2 = (List<MyClass>)XmlToObject(s, typeof(List<MyClass>));
		}

		public static string ObjectToXml<T>(T val) {
			XmlSerializer xs = new XmlSerializer(typeof(T));
			MemoryStream ms = new MemoryStream();
			xs.Serialize(ms, val);
			byte[] buf = ms.GetBuffer();
			string xmlOut = ASCIIEncoding.ASCII.GetString(buf);
			ms.Close();
			return xmlOut;
		}

		public static object XmlToObject(string s, Type t) {
			// XmlSerializer xsin = new XmlSerializer(typeof(List<MyClass>));
			byte[] buf = ASCIIEncoding.ASCII.GetBytes(s);
			MemoryStream	ms = new MemoryStream(buf);
			XmlSerializer xs = new XmlSerializer(t);
			object o = xs.Deserialize(ms); 
			ms.Close();
			return o;
		}
	}

	public class MyClass {
		public int		ID;
		public string	FieldName;
		public string	FieldValue;
	}
}
