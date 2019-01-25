using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace DataTableXML_Test {
	public class Program {
		static void Main(string[] args) {
			MyApp	me = new MyApp();
			me.Run();
		}
	}

	public class MyApp {
		public void Run() {
			DataTable	dt = new DataTable("MyTable");
			DataColumn	col1 = new DataColumn("Ques", typeof(string));
			DataColumn	col2 = new DataColumn("Ans", typeof(string));
			dt.Columns.Add(col1);
			dt.Columns.Add(col2);
			DataRow	dr = dt.NewRow();
			dr["Ques"] = "Ques 1";
			dr["Ans"]  = "Ans 1";
			dt.Rows.Add(dr);
			dr = dt.NewRow();
			dr["Ques"] = "Ques 2";
			dr["Ans"]  = "Ans 2";
			dt.Rows.Add(dr);

			MemoryStream	ms = new MemoryStream();
			dt.WriteXml(ms);
			ms.Close();
			string xml = Encoding.ASCII.GetString(ms.ToArray());
			XmlDocument xdoc = new XmlDocument();
			xdoc.LoadXml(xml);
			XslCompiledTransform xslt = new XslCompiledTransform();
        
        xslt.Load("theXsltFile.xslt");
        xslt.Transform("theXmlFile.xml", "theOutputFile.html");
			Console.WriteLine("{0}", xdoc.);
		}
	}
}
