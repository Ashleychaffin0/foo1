// Copyright (c) 2007 by Bartizan Connects, LLC

// A single file with all the classes needed to load an
// XML string from Bartizan's ccLeads feature

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace Bartizan.ccLeads {

	/// <summary>
	/// The top of the structure. Contains information for all the leads.
	/// </summary>
	[Serializable]
	public class ccLeads {
		public List<Lead> Leads;

//---------------------------------------------------------------------------------------

		public ccLeads() {
			Leads = new List<Lead>();
		}


	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Represents a single lead. It consists of a record number, the date and time of
	/// the lead, and any notes added by an Exhibitor. It's followed by the more
	/// configurable aspects of a lead.
	/// </summary>
	[Serializable]
	public class Lead {
		[XmlAttribute]
		public int					RecNo;
		[XmlAttribute]
		public DateTime				Timestamp;
		[XmlAttribute]
		public string				Notes;

		public BasicDataCollection	BasicData;
		public List<Demographic>	Demographics;
		public List<FollowUp>		FollowUps;
		public List<Session>		Sessions;
		public List<Survey>			Surveys;

//---------------------------------------------------------------------------------------

		public Lead() {
			RecNo		 = 0;
			Timestamp	 = DateTime.Now;
			Notes		 = "";

			BasicData	 = new BasicDataCollection();
			Demographics = new List<Demographic>();
			FollowUps	 = new List<FollowUp>();
			Sessions	 = new List<Session>();
			Surveys		 = new List<Survey>();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// This class holds basic information about an Attendee, such as a Name, an Address,
	/// the Company name, Phone numbers, etc. This is highly configurable, so the data is
	/// kept in a Dictionary<>. The "Key" is the name of the field (e.g. "First Name",
	/// "Company", etc), and the corresponding "Value" is, well, its value (e.g. "Larry",
	/// "Bartizan Connects").
	/// information in a Dictionary<>. However, the XmlSerialization
	/// methods won't read or write a Dictionary<>. So we have to provide our own
	/// formatting routines.
	/// </summary>
	[Serializable]
	public class BasicDataCollection : IXmlSerializable {
		public Dictionary<string, string> BasicData;

//---------------------------------------------------------------------------------------

		public BasicDataCollection() {
			BasicData = new Dictionary<string, string>();
		}

//---------------------------------------------------------------------------------------

		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema() {
			throw new System.NotImplementedException("The GetSchema method is not implemented.");
		}

//---------------------------------------------------------------------------------------

		// The XML serialization routines won't automatically serialize a Dictionary<>,
		// so we have to implement our own.
		public void ReadXml(XmlReader rdr) {
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
		public void WriteXml(XmlWriter wtr) {
			foreach (string key in BasicData.Keys) {
				wtr.WriteStartElement("Field");
				wtr.WriteAttributeString("Name", key);
				wtr.WriteAttributeString("Value", BasicData[key]);
				wtr.WriteEndElement();
			}
		}

		#endregion
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
	
	[Serializable]
	public class DemographicAnswer {
		[XmlAttribute]
		public string Answer;

//---------------------------------------------------------------------------------------

		public DemographicAnswer() {
			Answer = "N/A";
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[Serializable]
	public class Demographic {
		[XmlAttribute]
		public string					Question;
		public List<DemographicAnswer>	Answers;
		// A single demographic swipe may may have multiple answers associated with it
		// ("Check all that apply"). 

//---------------------------------------------------------------------------------------

		public Demographic() {
			Question = "*N/A*";
			Answers = new List<DemographicAnswer>();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
	
	[Serializable]
	public class FollowUp {
		[XmlAttribute]
		public string FollowUpText;

//---------------------------------------------------------------------------------------

		public FollowUp() {
			FollowUpText = "*N/A*";
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// This class does double duty, both for Sessions and Access Control. For a session,
	/// this contains the name of the session and a list of Status's, which will be only
	/// "Yes".
	/// <para>
	/// For Access Control, the Session Name is the same, but the Status will be either
	/// "Granted" or "Denied".
	/// </para>
	/// </summary>
	[Serializable]
	public class Session {
		[XmlAttribute]
		public string		SessionName;
		[XmlAttribute]
		public SessionType	SessType;
		public List<string> Status;

		// The SessionName value is either the name of the session, or the special
		// string "*No Session*" if there is no session configured when the Attendee
		// swiped/scanned. Similarly for Access Control, this could also be the
		// special string "*Access Control Disabled*".

		// The statuses we support for Sessions is:
		public static string StatYes	 = "Yes";

		// The statuses we support for Access Control is:
		public static string StatGranted = "Granted";
		public static string StatDenied  = "Denied";

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public enum SessionType {
			Session,
			AccessControl
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public Session() {
			SessionName = "N/A";
			Status = new List<string>();
			SessType = SessionType.Session;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
	
	/// <summary>
	/// Represents a survey question and its associated answers.
	/// </summary>
	[Serializable]
	public class Survey {
		[XmlAttribute]
		public string				Question;
		public List<SurveyAnswer>	Answers;

//---------------------------------------------------------------------------------------

		public Survey() {
			this.Question = "N/A";
			Answers = new List<SurveyAnswer>();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class SurveyAnswer {
		[XmlAttribute]
		public string Answer;

//---------------------------------------------------------------------------------------

		public SurveyAnswer() {
			Answer = "N/A";
		}
	}
}
