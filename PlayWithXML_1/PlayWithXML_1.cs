using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using Bartizan.ccLeads;

namespace PlayWithXML_1 {
	public partial class PlayWithXML_1 : Form {

		string	XmlFilename = "C:\\lrs\\foo.xml";

		public PlayWithXML_1() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnSerialize_Click(object sender, EventArgs e) {
			ccLeads data = new ccLeads();
			SetLead_1(data);
			SetLead_2(data);
			SetLead_3(data);
			// SetLead_4(data);

			try {
#if true
				// XmlSerializer xs = new XmlSerializer(typeof(List<Demographic>));
				XmlSerializer xs = new XmlSerializer(typeof(ccLeads));
				using (StreamWriter s = new StreamWriter(XmlFilename)) {
					xs.Serialize(s, data);
					s.Close();
				}
				MessageBox.Show("Done");
#else
				GenericSerialization<demog
#endif
			} catch (Exception ex) {
				string msg = ex.ToString();
				MessageBox.Show(msg, "Ugh");
			}
		}

//---------------------------------------------------------------------------------------

		private void btnDeserialize_Click(object sender, EventArgs e) {
			ccLeads cc2 = GenericSerialization<ccLeads>.Load(XmlFilename);	
			// MessageBox.Show("Done");
		}

//---------------------------------------------------------------------------------------

		private void SetLead_1(ccLeads data) {
			List<Demographic>	Demogs = new List<Demographic>();
			Demographic demo = new Demographic();
			demo.Question = "Job Title";
			DemographicAnswer	demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "CEO";
			demo.Answers.Add(demogAns1);
			Demogs.Add(demo);

			demo = new Demographic();
			demo.Question = "Products You Currently Use";
			demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "Widgets";
			DemographicAnswer	demogAns2 = new DemographicAnswer();
			demogAns2.Answer = "Wadgets";
			demo.Answers.Add(demogAns1);
			demo.Answers.Add(demogAns2);
			Demogs.Add(demo);

			BasicDataCollection BaseData = new BasicDataCollection();
			BaseData.BasicData["FirstName"] = "Larry";
			BaseData.BasicData["LastName"] = "Smith";
			BaseData.BasicData["BadgeID"] = "3141";

			List<FollowUp> FollowUps = new List<FollowUp>();
			FollowUp	fu1, fu2;
			fu1 = new FollowUp();
			fu1.FollowUpText = "Send Catalog";
			FollowUps.Add(fu1);
			fu2 = new FollowUp();
			fu2.FollowUpText = "Have Sales Rep Call";
			FollowUps.Add(fu2);

			List<Session>	Sessions = new List<Session>();
			Session	s1, s2, s3, s4;
			s1 = new Session();
			s2 = new Session();
			s3 = new Session();
			s4 = new Session();
			s1.SessionName = "LRS-01 Long Range Study";
			s1.Status = new List<string>(new string [] { "Yes" });
			s1.SessType = Session.SessionType.Session;
			Sessions.Add(s1);
#if false		// Don't really want more than one session per swipe
			s2.SessionName = "BGA-23 Customer Support";
			s2.Status = new List<string>(new string[] { "Yes" });
			s2.SessType = Session.SessionType.Session;
			Sessions.Add(s2);
			s3.SessionName = "ABC-99 How to Invest";
			s3.Status = new List<string>(new string[] { "Granted" });
			s3.SessType = Session.SessionType.AccessControl;
			Sessions.Add(s3);
			s4.SessionName = "LHC-31 Advanced Investing";
			s4.Status = new List<string>(new string[] { "Denied" });
			s4.SessType = Session.SessionType.AccessControl;
			Sessions.Add(s4);
#endif

			List<Survey>	Surveys = new List<Survey>();
			Survey		Surv = new Survey();
			Surv.Question = "How would you rate our Customer Service?";
			SurveyAnswer	ans = new SurveyAnswer();
			ans.Answer = "Above Average";
			Surv.Answers.Add(ans);
			Surveys.Add(Surv); 

			Lead	l = new Lead();
			l.RecNo = 1;
			l.Timestamp = DateTime.Now;
			l.Notes = "Hot lead. Make sure we follow up";
			l.BasicData = BaseData;
			l.Demographics = Demogs;
			l.FollowUps = FollowUps;
			l.Sessions = Sessions;
			l.Surveys = Surveys;
			data.Leads.Add(l);
		}

//---------------------------------------------------------------------------------------

		private void SetLead_2(ccLeads data) {
			List<Demographic>	Demogs = new List<Demographic>();
			Demographic demo = new Demographic();
			demo.Question = "Job Title";
			DemographicAnswer demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "CEO";
			demo.Answers.Add(demogAns1);
			Demogs.Add(demo);

			demo = new Demographic();
			demo.Question = "Products You Currently Use";
			demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "Wadgets";
			demo.Answers.Add(demogAns1);
			Demogs.Add(demo);

			BasicDataCollection BaseData = new BasicDataCollection();
			BaseData.BasicData["FirstName"] = "John";
			BaseData.BasicData["LastName"] = "Jones";
			BaseData.BasicData["BadgeID"] = "2718";

			List<FollowUp> FollowUps = new List<FollowUp>();
			FollowUp	fu1, fu2;
			fu1 = new FollowUp();
			fu1.FollowUpText = "Provide Quote";
			FollowUps.Add(fu1);
			fu2 = new FollowUp();
			fu2.FollowUpText = "Have immediate need";
			FollowUps.Add(fu2);

			List<Session>	Sessions = new List<Session>();
			Session	s1;
			s1 = new Session();
			s1.SessionName = "KRC-01 Advanced Planning";
			s1.Status = new List<string>(new string [] { "Denied" });
			s1.SessType = Session.SessionType.AccessControl;
			Sessions.Add(s1);

			List<Survey>	Surveys = new List<Survey>();


			Lead l = new Lead();
			l.RecNo = 2;
			l.Timestamp = DateTime.Now;
			l.Notes = "Sample note";
			l.BasicData = BaseData;
			l.Demographics = Demogs;
			l.FollowUps = FollowUps;
			l.Sessions = Sessions;
			l.Surveys = Surveys;
			data.Leads.Add(l);
		}

//---------------------------------------------------------------------------------------

		private void SetLead_4(ccLeads data) {
			List<Demographic> Demogs = new List<Demographic>();
			Demographic demo = new Demographic();
			demo.Question = "Job Title";
			DemographicAnswer demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "CIO";
			demo.Answers.Add(demogAns1);
			Demogs.Add(demo);

			demo = new Demographic();
			demo.Question = "Products You Currently Use";
			demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "Wadgets";
			demo.Answers.Add(demogAns1);
			Demogs.Add(demo);
			demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "Super Gadgets";
			demo.Answers.Add(demogAns1);

			BasicDataCollection BaseData = new BasicDataCollection();
			BaseData.BasicData["FirstName"] = "Fred";
			BaseData.BasicData["LastName"] = "Golden";
			BaseData.BasicData["BadgeID"] = "1618";

			List<FollowUp> FollowUps = new List<FollowUp>();
			FollowUp fu1, fu2;
			fu1 = new FollowUp();
			fu1.FollowUpText = "Setup demo";
			FollowUps.Add(fu1);

			List<Session> Sessions = new List<Session>();
			Session s1;
			s1 = new Session();
			s1.SessionName = "KRC-01 Advanced Planning";
			s1.Status = new List<string>(new string[] { "Granted" });
			s1.SessType = Session.SessionType.AccessControl;
			Sessions.Add(s1);

			List<Survey> Surveys = new List<Survey>();
			// TODO: Add survey data


			Lead l = new Lead();
			l.RecNo = 4;
			l.Timestamp = DateTime.Now;
			l.Notes = "Sample note";
			l.BasicData = BaseData;
			l.Demographics = Demogs;
			l.FollowUps = FollowUps;
			l.Sessions = Sessions;
			l.Surveys = Surveys;
			data.Leads.Add(l);
		}

//---------------------------------------------------------------------------------------

		private void SetLead_3(ccLeads data) {
#if false
			List<Demographic> Demogs = new List<Demographic>();
			Demographic demo = new Demographic();
			demo.Question = "Job Title";
			DemographicAnswer demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "CEO";
			demo.Answers.Add(demogAns1);
			Demogs.Add(demo);

			demo = new Demographic();
			demo.Question = "Products You Currently Use";
			demogAns1 = new DemographicAnswer();
			demogAns1.Answer = "Wadgets";
			demo.Answers.Add(demogAns1);
			Demogs.Add(demo);
#endif

			BasicDataCollection BaseData = new BasicDataCollection();
			// BaseData.BasicData["FirstName"] = "John";
			// BaseData.BasicData["LastName"] = "Jones";
			BaseData.BasicData["BadgeID"] = "7071";

#if false
			List<FollowUp> FollowUps = new List<FollowUp>();
			FollowUp fu1, fu2;
			fu1 = new FollowUp();
			fu1.FollowUpText = "Provide Quote";
			FollowUps.Add(fu1);
			fu2 = new FollowUp();
			fu2.FollowUpText = "Have immediate need";
			FollowUps.Add(fu2);
#endif
			List<Session> Sessions = new List<Session>();
			Session s1;
			s1 = new Session();
			s1.SessionName = "BGA-23 Customer Support";
			s1.Status = new List<string>(new string[] { "Granted" });
			s1.SessType = Session.SessionType.AccessControl;
			Sessions.Add(s1);

			List<Survey> Surveys = new List<Survey>();
			// TODO: Add survey data


			Lead l = new Lead();
			l.RecNo = 3;
			l.Timestamp = DateTime.Now;
			l.Notes = "";
			l.BasicData = BaseData;
			// l.Demographics = Demogs;
			// l.FollowUps = FollowUps;
			l.Sessions = Sessions;
			// l.Surveys = Surveys;
			data.Leads.Add(l);
		}
	}
}