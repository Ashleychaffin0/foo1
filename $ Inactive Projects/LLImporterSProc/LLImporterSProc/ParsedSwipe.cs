// Copyright (c) 2005-2006 Bartizan Connects, LLC

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;


namespace Bartizan.Importer {

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// A toy, which only does a Console.Writeline. Later it will probably put the 
    /// messages into a List<string>, which we can return to the caller (in some
	/// fashion).
	/// </summary>
	class LogFile {

//---------------------------------------------------------------------------------------

		public void Log(string fmt, params object [] args) {
#if ! SQLSERVER
			Debug.WriteLine(string.Format(fmt, args));
#endif
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// A small auxilliary class to hold config info from our Field XML file.
	/// </summary>
	public class FieldInfo {
		public string	    FieldName;		// e.g. "Card #"
		public string	    UserFieldName;	// user override, e.g. "Badge CurField"
		public string	    type;			// Text, DateTime, Survey, etc

		// The following must be kept in synch with the .xml file
		// Note: Arguably, we should just key off the numeric field codes (e.g.
		//		 Survey = 700). Let's face it, these IDs are in the firmware and are
		//		 bloody unlikely to change. But who knows? So as long as we've got the
		//		 XML file around, we'll put the information there and use it.
		public const string FieldType_Services				= "Services";
		public const string FieldType_Demographics			= "Demographics";
		public const string FieldType_Survey				= "Survey";
		public const string FieldType_Referrals_Provider	= "Referrals_Provider";
		public const string FieldType_Referrals_From		= "Referrals_From";
		public const string FieldType_SwipeTimestamp		= "SwipeTimeStamp";
		public const string FieldType_Referrals_To			= "Referrals_To";
		// TODO: Add Access_Control support
		public const string FieldType_Access_Control		= "Access_Control";  // aka Sessions

//---------------------------------------------------------------------------------------

		public FieldInfo(string name, string type) {
			this.FieldName		= name;
			this.UserFieldName	= null;
			this.type			= type;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	/// <summary>
	/// This class used to be called "Importer", until we realized that we needed to
    /// split the process up. Once we've got the setup (e.g. map.cfg) file read in 
    /// (parsed!), we match that against (i.e. parse) the swipe data, and pass it back
    /// to our caller. Then he/she/it can do with the data whatever they want. They can
    /// pump it out to a database, populate a spreadsheet with the data, export XML, 
    /// reformat it so it can be imported into another program, or whatever. But this
    /// stage is the swipe data parsing phase.
	/// </summary>
	public class ParsedSwipe {
		public Dictionary<string, FieldInfo>	FieldDefs;
		public Dictionary<string, string>		BasicData;
		public List<Demographic>	Demographics;
        public List<Survey>			Surveys;
		public List<Service>		Services;			// aka Questions

		public string				ServiceProvider;	// Referrals stuff
		public List<string>			FromProviders;
		public List<string>			ToProviders;

		public DateTime				SwipeTimestamp;
		public string				Session;

		public MapCfg				mapcfg;
		LogFile						log;            // TODO: Replace with some kind of
													//		 List<string> that has error
													//		 information.

        public Swipe				swp;            // The raw data, just in case the
													// user wants to see it (for logging
													// invalid input?)

		// When processing field type 12 = Services, things get a little crazy. We may
		// have, say, 100 services in the setup file, and 100 fields on the swipe
		// record, but we'll have only a single "12" in the [FILE] section. Thus any
		// fields after a "12" (e.g. Note="18") won't be, say, field 34, but 134.
		// We here define an offset field. It will be set to 0 for each new record,
		// but if we get a "12", we'll set it to the number of services minus 1 (e.g.
		// 99 - not 100, since the "12" counts as 1) and all subsequent fields 
		// in the record will be accessed with this offset.
		int				ServicesOffset;

        // Each instance of a Survey or Demographics code marches us through their data.
        // Keep track of them here.
        int             SurveyIndex, DemographicsIndex;




//---------------------------------------------------------------------------------------

        public ParsedSwipe(string xmlFirmwareFields, MapCfg imp) {
            this.mapcfg		= imp;
			log				= new LogFile();

            // FieldDefs = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
            // BasicData = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
			FieldDefs = new Dictionary<string, FieldInfo>(StringComparer.CurrentCultureIgnoreCase);
			BasicData = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            // FieldDefs = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            // BasicData = new Hashtable(StringComparer.CurrentCultureIgnoreCase);

			Demographics    = new List<Demographic>();
            Surveys         = new List<Survey>();
			Services	    = new List<Service>();

			ServiceProvider = "";
			FromProviders	= new List<string>();
			ToProviders		= new List<string>();

			SwipeTimestamp	= default(DateTime);
			Session			= null;

			// Load an XML file that has all the field codes that the firmware knows
			// about. One reason we need this is to be able to have known field names
			// in our database for our common fields. For example, while the user may
			// call his field "Badge Number", we need to put this into field "Card #".

			// Note: AT this level, we really don't need/want the data type (e.g. field
			//		 xxx is a date). It's up to a higher level to decide if the data is
			//		 correctly formatted. Also, if the user adds additional fields via
			//		 Bartset, there's no type information on those.
			try {
				XmlDocument	xdoc = new XmlDocument();
				xdoc.LoadXml(xmlFirmwareFields);
				XmlNodeList xnl = xdoc.GetElementsByTagName("Field");
				foreach (XmlNode node in xnl) {
                    FieldDefs[node.InnerText] = new FieldInfo(node.Attributes["Name"].Value, node.Attributes["Type"].Value);
				}
			} catch (Exception e) {
				log.Log("An error ({0}) occurred importing ImportFieldDefs.xml", e.Message);
				// TODO: Need to return retcode (hard to do from a ctor), or throw, or something
				return;
			}

			// OK, now we have all the field codes and their default names (e.g. field
			// code "1" has the default name "Card #". But the user can use Bartset to 
			// rename it to, say, "Badge CurField". So we'll go through the [LABELS] data and
			// update our table, merging the map.cfg data with the defaults.
			// Note that in some (probably most) cases, we'll overwrite the name
			// with the same value (i.e. for code "1", replace the name "Card #" with
			// "Card #". Big deal.
			string		Caption;
			foreach (Label lbl in imp.Labels) {
				Caption = lbl.Caption;
				// I've seen (sigh) a FileNum with an empty Caption. If the caption is
				// empty, and it's a FileNum we already know about, ignore it. If it's
				// a new one that we're about to add, give it a dummy name.
				if (FieldDefs.ContainsKey(lbl.FileNum)) {
					if (Caption.Length > 0)
						FieldDefs[lbl.FileNum].UserFieldName = Caption;
				} else {
					if (Caption.Length == 0)
						Caption = string.Format("FileNum_{0}_Missing_Caption", lbl.FileNum);
					// It would be nice if we knew the data type. But we don't. And we
					// shouldn't guess. So call it generically a Text field.
					FieldDefs[lbl.FileNum] = new FieldInfo(Caption, "Text");
				}
			}
		}

//---------------------------------------------------------------------------------------

        public void GetFields(string line) {
			
            swp = new Swipe();              // Start fresh each time

			// We had problems at one point with lines that had (only) a tab character.
			line = line.Trim();
			if (line.Length == 0)
				return;

			string		FieldID;

            if (! swp.Parse(line)) {
                log.Log("Unable to parse line '{0}'", line);
                // TODO: Throw? Return bool? But for now just return
                return;
            }

            // Reinitialize ourselves
            BasicData.Clear();
            Surveys.Clear();
			Demographics.Clear();
			Services.Clear();

			ServicesOffset    = 0;
            SurveyIndex       = 0;
            DemographicsIndex = 0;

			for (int i=0; i<mapcfg.File.Count; ++i) {
				List<int>	flds = mapcfg.File[i].FieldCodes;
				if (flds.Count == 1) {
					FieldID = flds[0].ToString();
				} else {
					// TODO: Kludge for concatenated fields
					FieldID = flds[0].ToString();
				}
                if (FieldDefs.ContainsKey(FieldID)) {
					ProcessField(i, FieldID);
				} else {
					// TODO: Probably want to fake a Labels entry
					log.Log("Don't know how to process {0}", FieldID);
				}
			}
			// We now have the information for a single swipe.
            // Just return, and let the caller do something with it. This might
            // involve being added to a database, updating a spreadsheet,
            // pumping out XML, etc. But that's the job of our caller. For our
            // part we just return the data in a standardized form, and we're done.
		}

//---------------------------------------------------------------------------------------

		void ProcessField(int i, string FieldID) {
			string		CurField;
			int			n;
			string		s;
			bool		bOK;

			// TODO: To remember the order that the fields came in (not currently
			//		 supported since data is stored in a Hashtable), store the
			//		 FieldID in a List<string> instance variable. Note however that
			//		 maybe we don't want to do it here (may be a problem with
			//		 concatenated fields), but rather one level up. Note also that
			//		 if/when we implement this, .Clear() the List<string> each time
			//		 we call GetFields().

			// The record number at the beginning of each record is an implicit field
			// and not included in the [File] descriptors. So get FieldCodes[i + 1]
			CurField = swp.Fields[i + 1 + ServicesOffset].Trim();

            FieldInfo	fi = FieldDefs[FieldID];
			switch (fi.type) {
			case FieldInfo.FieldType_SwipeTimestamp:
				bOK = DateTime.TryParse(CurField, out SwipeTimestamp);
				if (! bOK) {
					// TODO: Complain bitterly, somehow.
				}
				break;

			case FieldInfo.FieldType_Access_Control:
				Session = CurField;
				break;

			case FieldInfo.FieldType_Services:
				for (int j=0; j<mapcfg.Services.Count; ++j) {
					string	serv = swp.Fields[i + j + 1].ToUpper();
					if (serv == "Y") {
						Services.Add(mapcfg.Services[j]);	
					}
				}
				ServicesOffset = mapcfg.Services.Count - 1;
				break;

			case FieldInfo.FieldType_Demographics:
				Demographic	CurDemog = mapcfg.Demographics[DemographicsIndex++];
				if (CurField.Length == 0)
					break;
				bool	bFoundAnswer = false;
				foreach (DemographicAnswer demog_a in CurDemog.Answers) {
					if (string.Compare(CurField, demog_a.ID) == 0) {
						Demographic NewDemog = new Demographic(CurDemog.Question);
						NewDemog.Answers     = new List<DemographicAnswer>();
						NewDemog.Answers.Add(new DemographicAnswer(CurField, demog_a.Answer));
						Demographics.Add(NewDemog);
						bFoundAnswer = true;
					}
				}
				if (! bFoundAnswer) {
					// TODO: Set error if not found
				}
				break;

			case FieldInfo.FieldType_Survey:
				Survey	CurSurv = mapcfg.Surveys[SurveyIndex++];
				if (CurField.Length == 0)			// Ignore if not supplied
					break;
				int		nID;
				bOK = int.TryParse(CurField, out nID);
				if (!bOK) {
					// TODO: Uh, do something
				}
				Survey	surv = new Survey(CurSurv.Question);
                surv.Answers.Add(CurSurv.Answers[nID]);	// TODO: Check for in range
				Surveys.Add(surv);
				break;

			case FieldInfo.FieldType_Referrals_Provider:
				if (CurField.Length == 0)
					break;								// TODO: Is this what AT does?
				bOK = int.TryParse(CurField, out n);
				if ((bOK == false) || (n == -1))
					s = mapcfg.Refs.DefaultProvider;	
				else
					s = mapcfg.Refs.Providers[n];		// TODO: Check for in range
				ServiceProvider = s;
				break;

			case FieldInfo.FieldType_Referrals_From:
				// Note: This code supports multiple referrals
				if (CurField.Length == 0)
					break;								// TODO: Is this what AT does?
				bOK = int.TryParse(CurField, out n);
				if ((bOK == false) || (n == -1))
					s = mapcfg.Refs.DefaultFromPartner;	
				else
					s = mapcfg.Refs.FromPartners[n];	// TODO: Check for in range
				FromProviders.Add(s);
				break;

			case FieldInfo.FieldType_Referrals_To:
				// Note: This code supports multiple referrals
				if (CurField.Length == 0)
					break;								// TODO: Is this what AT does?
				bOK = int.TryParse(CurField, out n);
				if ((bOK == false) || (n == -1))
					s = mapcfg.Refs.DefaultToPartner;	
				else
					s = mapcfg.Refs.ToPartners[n];		// TODO: Check for in range
				ToProviders.Add(s);
				break;

			default:
                // BasicData[FieldID] = CurField;
				string		FieldName = fi.UserFieldName;
				if (FieldName == null) {
					FieldName = fi.FieldName;
				}
                BasicData[FieldName] = CurField;
				break;
			}
		}

//---------------------------------------------------------------------------------------

#if false		// GetSuffix() not needed in sproc version of Importer
		/// <summary>
		/// We may be importing, not MAP.CFG, but MAP1.CFG. Or MAP2.CFG. And so on.
		/// Return the x in MAPx.CFG (where x can be the empty string). We'll use the to
		/// generate the name of the corresponding VISITORx.TXT file, if any.
		/// </summary>
		/// <returns>The text (possibly empty) between "MAP" and ".CFG"</returns>
		static string GetSuffix(string filename) {
			string CFGName = Path.GetFileName(filename);	// Strip directory name
			// Calculate the length of the suffix. "MAP.CFG" is 7 chars,
			// so anything between "MAP" and ".CFG" is (length of entire
			// string - 7) characters long.
			int		SuffixLen = CFGName.Length - 7;	
			return CFGName.Substring(3, SuffixLen);	// Suffix starts at pos 3
		}
#endif

//---------------------------------------------------------------------------------------

		public void Dump(StreamWriter sw) {
			if (swp.Fields == null) {
				return;				// Problem parsing this record
			}

			StringBuilder	sb = new StringBuilder();
            
            // First, dump the raw data
			sw.WriteLine("\r\n*************\r\n");
			sb.Length = 0;
			foreach (string s in swp.Fields) {
				if (sb.Length > 0)
					sb.Append(",");
				sb.AppendFormat("\"{0}\"", s);
			}
			sw.WriteLine(sb.ToString()); 

			// Now format it
			sw.WriteLine("-------- Basic Data --------");
			foreach (string key in BasicData.Keys) {
				// TODO: Getting the name is ugly. Fix it somehow
				sw.WriteLine("    {0}/{1} = {2}", key, FieldDefs[key].FieldName, BasicData[key]);
			}
			sw.WriteLine("-------- Services --------");
			foreach (Service serv in Services) {
				sw.WriteLine("    {0}", serv.service);
			}
			sw.WriteLine("-------- Demographics --------");
			foreach (Demographic demog in Demographics) {
				sw.WriteLine("    Demographic question = {0}", demog.Question);
				foreach (DemographicAnswer ans in demog.Answers) {
					sw.WriteLine("        Answer={0}={1}", ans.ID, ans.Answer);
				}
			}
			sw.WriteLine("-------- Surveys --------");
			foreach (Survey surv in Surveys) {
				sw.WriteLine("    Survey question = {0}", surv.Question);
				foreach (string ans in surv.Answers) {
					sw.WriteLine("        Answer={0}", ans);
				}
			}
			sw.WriteLine("-------- Referrals --------");
			if (ServiceProvider.Length > 0)
				sw.WriteLine("    Service Provider = {0}", ServiceProvider);
			if (FromProviders.Count > 0) {
				foreach (string prov in FromProviders) 
				sw.WriteLine("    From    Provider = {0}", prov);
			}
			if (ToProviders.Count > 0) {
				foreach (string prov in ToProviders) 
				sw.WriteLine("    To      Provider = {0}", prov);
			}
        }
	}
}
