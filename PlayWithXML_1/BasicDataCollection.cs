using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Bartizan.ccLeadsWorking {

	/// <summary>
	/// This class holds information in a Dictionary<>. However, the XmlSerialization
	/// methods won't read or write a Dictionary<>. So we have to provide our own
	/// formatting routines.
	/// </summary>
	[Serializable]
	public class BasicDataCollection : IXmlSerializable {
		public Dictionary<string, string>	BasicData;

//---------------------------------------------------------------------------------------

		public BasicDataCollection() {
			BasicData = new Dictionary<string,string>();
		}

//---------------------------------------------------------------------------------------

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema() {
			throw new System.NotImplementedException("The GetSchema method is not implemented.");
		}

//---------------------------------------------------------------------------------------

		public void ReadXml(XmlReader rdr) {
			string	FieldName = null, FieldValue = null, FieldText = null;
			rdr.ReadStartElement();
			do {
				Console.WriteLine("{0}[{1}] = {2}", rdr.Name, rdr.NodeType, rdr.Value);
				switch (rdr.NodeType) {
				case XmlNodeType.Element:
					switch (rdr.Name) {
					case "Name":
						FieldName = rdr.ReadString();
						break;
					case "Value":
						FieldValue = rdr.ReadString();
						break;
					default:
						break;
					}
					break;

				case XmlNodeType.EndElement:
					switch (rdr.Name) {
					case "Field": 
						BasicData.Add(FieldName, FieldValue);
						break;
					case "BasicData":
						rdr.ReadEndElement();
						return;
					default:
						break;
					}
					break;
				default:
					// Ignore all other element types
					break;
				}
			} while (rdr.Read());
		}

//---------------------------------------------------------------------------------------

		public void WriteXml(XmlWriter wtr) {
			foreach (string key in BasicData.Keys) {
				// wtr.WriteElementString(key, BasicData[key]);
				wtr.WriteStartElement("Field");
					wtr.WriteElementString("Name", key);
					wtr.WriteElementString("Value", BasicData[key]);
				wtr.WriteEndElement();
			}
		}

		#endregion
	}
}
