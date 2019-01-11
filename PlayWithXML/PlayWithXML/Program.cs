using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PlayWithXML {
	class Program {
		static void Main(string[] args) {
			string ba = @"C:\Documents and Settings\Larrys\Application Data\SharpReader\cache\blogs.msdn.com-brada-rss.xml";
			XmlDocument doc = new XmlDocument();
			doc.Load(ba);
			string	title = "", description = "", url = "";
			foreach (XmlElement elem in doc.DocumentElement.ChildNodes) {
				Console.WriteLine("\n\n{0} = {1}", elem.Name, elem.InnerXml);
				switch (elem.Name.ToUpper()) {
				case "TITLE":
					title = elem.InnerText;
					break;
				case "WEBPAGEURL":
					url = elem.InnerText;
					break;
				case "DESCRIPTION":
					description = elem.InnerText;
					break;
				default:
					break;
				}
			}
			// string name = doc.DocumentElement.FirstChild.Name;
		}
	}
}
