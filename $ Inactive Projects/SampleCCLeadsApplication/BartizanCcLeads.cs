// Copyright (c) 2007 by Bartizan Connects, LLC

// A single file with all the classes needed to load an
// XML string from Bartizan's ccLeads feature.

// To use this file
//		a) Include this file in your project
//		b) Add a "using Bartizan.ccLeads;" statement to your mainline
//		c) Add "ccLeads records = ccLeads.Load(XmlText);" (assuming your XML data is
//			in a variable called "XmlText")
//		d) Process the "records" data

// An example method, FormatRecords, is provided for the ccLeads class to demonstrate
// how to walk through the data. It is not intended to be used in production code, but
// can be used as a kind of template for your own processing.


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given an XML string, loads it into an instance of the ccLeads class.
		/// </summary>
		/// <param name="xml">The string with the XML data that matches the ccLeads
		/// classes.</param>
		/// <returns>An instance of the ccLeads class, which in turn can contain
		/// data from multiple records.</returns>
		/// <remarks>If somehow this routine is given invalid data (e.g. non-XML data),
		/// it will throw an exception. The caller should surround a call to this routine
		/// with try/catch.
		/// </remarks>
		public static ccLeads Load(string xml) {
			byte []	buf = ASCIIEncoding.ASCII.GetBytes(xml);
			using (MemoryStream ms = new MemoryStream(buf, false)) {
				XmlSerializer xs = new XmlSerializer(typeof(ccLeads));
				return (ccLeads)xs.Deserialize(ms);
			}
		}

//---------------------------------------------------------------------------------------

#if true		// See comments at the beginning of this file. Change to #if false to
				// essentially delete this method, saving a few bytes of code and strings
		/// <summary>
		/// A sample method, not intended for production, but can be used as a template
		/// for processing the data returned from the ccLeads.Load() method. This routine
		/// walks through the data and displays it on the screen in a formatted fashion.
		/// </summary>
		public static void FormatRecords(ccLeads records) {
			foreach (Lead record in records.Leads) {
				// Format the start of the record
				Console.WriteLine("\n\nRecord number {0}, created at {1}, Notes: {2}",
					record.RecNo, record.Timestamp, record.Notes);

				// Format the basic data
				Console.WriteLine("----- Basic Data -----");
				foreach (string key in record.BasicData.BasicData.Keys) {
					Console.WriteLine("\tField: Name = '{0}', Value='{1}'",
						key, record.BasicData.BasicData[key]);
				}

				// Format the Demographics
				Console.WriteLine("----- Demographics -----");
				foreach (Demographic demog in record.Demographics) {
					Console.WriteLine("\tQuestion: {0}", demog.Question);
					foreach (DemographicAnswer ans in demog.Answers) {
						Console.WriteLine("\t\tAnswer: {0}", ans.Answer);
					}
				}

				// Format the Follow-Ups
				Console.WriteLine("----- Follow Ups -----");
				foreach (FollowUp follow in record.FollowUps) {
					Console.WriteLine("\tFollow Up: {0}", follow.FollowUpText);
				}

				// Format the Sessions
				Console.WriteLine("----- Sessions -----");
				foreach (Session sess in record.Sessions) {
					Console.WriteLine("\tSession Name: {0}, Type: {1}", sess.SessionName, sess.SessType);
					foreach (string status in sess.Status) {
						Console.WriteLine("\t\tStatus: {0}", status);
					}
				}

				// Format the Surveys
				Console.WriteLine("----- Surveys -----");
				foreach (Survey surv in record.Surveys) {
					Console.WriteLine("\tQuestion: {0}", surv.Question);
					foreach (SurveyAnswer ans in surv.Answers) {
						Console.WriteLine("\t\tAnswer: {0}", ans.Answer);
					}
				}
			}
		}
#endif
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
		public int		RecNo;
		[XmlAttribute]
		public DateTime Timestamp;
		[XmlAttribute]
		public string	Notes;

		public BasicDataCollection	BasicData;
		public List<Demographic>	Demographics;
		public List<FollowUp>		FollowUps;
		public List<Session>		Sessions;
		public List<Survey>			Surveys;

//---------------------------------------------------------------------------------------

		public Lead() {
			RecNo		= 0;
			Timestamp	= DateTime.Now;
			Notes		= "";

			BasicData		= new BasicDataCollection();
			Demographics	= new List<Demographic>();
			FollowUps		= new List<FollowUp>();
			Sessions		= new List<Session>();
			Surveys			= new List<Survey>();
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
	/// "Bartizan Connects", etc).
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
				switch (rdr.NodeType) {
				case XmlNodeType.Element:
					if (rdr.Name == "Field") {
						FieldName  = rdr.GetAttribute("Name");
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

	/// <summary>
	/// Represents a Demographic and its associated answer(s)
	/// </summary>
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

	/// <summary>
	/// Represents a Follow-up question (e.g. Send Catalog, Have sales rep call, etc)
	/// </summary>
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
		public static string StatYes = "Yes";

		// The statuses we support for Access Control is:
		public static string StatGranted = "Granted";
		public static string StatDenied = "Denied";

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
