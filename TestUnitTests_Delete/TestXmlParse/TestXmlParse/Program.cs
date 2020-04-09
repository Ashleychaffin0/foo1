using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace TestXmlParse {
	class Program {
		static void Main(string[] args) {
			Program pgm = new Program();
			pgm.Run();
		}

//---------------------------------------------------------------------------------------

		private void Run() {
			StringBuilder	sb = new StringBuilder();
			XmlWriterSettings	xset = new XmlWriterSettings();
			xset.Indent = true;
			XmlWriter	xwtr = XmlWriter.Create(sb, xset);
			//xwtr.Settings
			xwtr.WriteStartElement("LEADS");

			ProcessSwipe(xwtr);

			xwtr.WriteEndElement();		// </LEADS>
			xwtr.Close();
			Console.WriteLine(sb);
		}

//---------------------------------------------------------------------------------------

		private void ProcessSwipe(XmlWriter xwtr) {
			XmlDocument x = new XmlDocument();
			// TODO: Need try/catch (either here, or in caller), in case of bad XML
			//		 in XmlParms
			string s = @"<?xml version=""1.0"" encoding=""utf-8""?>
<SwipeInfo>
  <BasicData>
    <UNIT_ID>X123-1</UNIT_ID>
    <BADGE_ID>16932</BADGE_ID>
    <FIRSTNAME>Larry</FIRSTNAME>
    <LASTNAME>Smith</LASTNAME>
    <EMAIL>abc@def.com</EMAIL>
    <NOTE></NOTE>
  </BasicData>
  <DEFINED_Q>
    <DQ0>1</DQ0>
    <DQ1>0</DQ1>
  </DEFINED_Q>
  <CUSTOM_Q>
    <CQ0></CQ0>
  </CUSTOM_Q>
  <SC></SC>
</SwipeInfo>
";	
			x.LoadXml(s);
			xwtr.WriteStartElement("LEAD");

			XmlNode	n1 = x.SelectSingleNode("SwipeInfo/BasicData");
			XmlNode n2 = x.SelectSingleNode("SwipeInfo/DEFINED_Q");
			XmlNode n3 = x.SelectSingleNode("SwipeInfo/CUSTOM_Q");
			XmlNode n4 = x.SelectSingleNode("SwipeInfo/SC");

			foreach (XmlElement elem in n1.ChildNodes) {
				Console.WriteLine("{0}={1}", elem.Name, elem.InnerText);
				xwtr.WriteElementString(elem.Name, elem.InnerText);
			}

			xwtr.WriteStartElement("DEFINED_Q");
			foreach (XmlElement elem in n2.ChildNodes) {
				Console.WriteLine("{0}={1}", elem.Name, elem.InnerText);
				xwtr.WriteElementString(elem.Name, elem.InnerText);
			}
			xwtr.WriteEndElement();

			xwtr.WriteStartElement("CUSTOM_Q");
			foreach (XmlElement elem in n3.ChildNodes) {
				Console.WriteLine("{0}={1}", elem.Name, elem.InnerText);
				xwtr.WriteElementString(elem.Name, elem.InnerText);
			}
			xwtr.WriteEndElement();

			xwtr.WriteStartElement("SC");
			foreach (XmlElement elem in n4.ChildNodes) {
				Console.WriteLine("{0}={1}", elem.Name, elem.InnerText);
				xwtr.WriteElementString(elem.Name, elem.InnerText);
			}
			xwtr.WriteEndElement();

			xwtr.WriteEndElement();			// </LEAD>
		}

//---------------------------------------------------------------------------------------

		// The XML serialization routines won't automatically serialize a Dictionary<>,
		// so we have to implement our own.
		// TODO: Sample code
		void ReadXml(XmlReader rdr) {
			var BasicData = new Dictionary<string, string>();	// TODO:
			string FieldName = null, FieldValue = null;
			rdr.ReadStartElement();
			do {
				Console.WriteLine("{0}[{1}] = {2}", rdr.Name, rdr.NodeType, rdr.Value);	// TODO:
				switch (rdr.NodeType) {
				case XmlNodeType.Element:
					if (rdr.Name == "Field") {
						FieldName = rdr.GetAttribute("Name");
						FieldValue = rdr.GetAttribute("Value");
						if ((FieldName == null) || (FieldValue == null)) {
							// Incomplete record. Do nothing. Ignore the tag
						} else {
							BasicData.Add(FieldName, FieldValue);
						}
					}
					break;

				case XmlNodeType.EndElement:
					if (rdr.Name == "BasicData") {
						rdr.ReadEndElement();
						return;
					}
					break;

				default:
					// Ignore all other element types
					break;
				}
			} while (rdr.Read());
		}

//---------------------------------------------------------------------------------------

		// See comment at start of ReadXml method
		// TODO: Sample code
		void WriteXml(XmlWriter wtr) {
			var BasicData = new Dictionary<string, string>();	// TODO:
			foreach (string key in BasicData.Keys) {
				wtr.WriteStartElement("Field");
				wtr.WriteAttributeString("Name", key);
				wtr.WriteAttributeString("Value", BasicData[key]);
				wtr.WriteEndElement();
			}
		}
	}
}
