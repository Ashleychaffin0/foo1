// Copyright (c) 2003-2006 by Bartizan Connects, LLC

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace Bartizan.Importer {
    /// <summary>
    /// Represents a setup file (or rather its external representation, a MAP.CFG file)
	/// for a Bartizan Box. It loads all the data into memory. This can then be used as 
	/// part of the import process. One note: there is no constructor that takes a 
	/// string as a filename. While today we use only a simple ASCII file, later we may
	/// want to support, say, an XML file with the parameters. And there's no trivial 
	/// way to say "this time the string names a file in the traditional format, but that
	/// time it's an XML file". (Yeah, we could pass a second parm, an enum stating what
	/// kind of file it was. But it's more type safe to have separate types. But with 
	/// that said, I'm not 100% sure that this is the right approach. Quite possibly we
	/// need a MapCfg base class, with derived classes of MapCfgTraditional, MapCfgXML,
	/// etc. But for now...
    /// </summary>
	public class MapCfg {

		public List<string>			FrontSection;
		public string				TerminalName;
        public string				ShowHeader;
        public string				DateDisplay;    // e.g. "MM/DD/YY"
        public List<Label>			Labels;

		// The File section is just a bit complex. In the basic case, we could get by
		// with File being just a List<int>. But concatenated fields complicate matters.
		// A single File entry could contain multiple int's. So each File is a List of
		// FileFields, which contain (usually) 1 or (occasionally) more int's.
		public List<FileFields>		File;			// The [FILE} section, not a real file

		public List<Service>		Services;		// aka [QUESTIONS]
		public List<Survey>			Surveys;
		public List<Demographic>	Demographics;
        public Referrals			Refs;			// Referrals, not References

		public string		filename;		// Where the data came from

		public const string UnknownTerminalName = "*Unknown*";
		public const string UnknownShowHeader	= "*No Name*";

//---------------------------------------------------------------------------------------

		public MapCfg() {
			TerminalName = UnknownTerminalName;
            ShowHeader   = "";
            DateDisplay  = "MM/DD/YY";
			File		 = new List<FileFields>();

			// Note: We don't need to set defaults for Services or Surveys. The
			//		 Read_xxx_Section routines for these will return (possibly empty)
			//		 Lists for us.
		}

//---------------------------------------------------------------------------------------

        public MapCfg(BartIniFile suf, string TerminalID) : this() {
			filename = suf.filename;
			// If we're importing from a PDA, the Map.Cfg file may be on the database, 
			// with no specific TerminalID on it (shared setup file). So set the 
			// TerminalID, if given.
			if (TerminalID.Trim().Length > 0) {
				TerminalName = TerminalID;
			}

            // LATER: We can have overloads to let us load from, say, a BartXMLFile
            LoadFromBartIniFile(suf);
        }

//---------------------------------------------------------------------------------------

		public void LoadFromBartIniFile(BartIniFile suf) {

			// Yeah, I admit it. We're being inconsistent here. In some routines below
			// we process the data ourselves, and in others, we have the class load 
			// itself. The dividing line is whether we really need a class for, say,
			// the [FILE] section. Which we don't, because the data is a simple vector. 
			// Least of all is [TERMINAL]. OTOH, there's a bit more structure to the 
			// Survey class, so the ReadSurveySection routine below will invoke 
			// a static method of that class to load itself.

			ReadFrontSection(suf);

			ReadTerminalSection(suf);

            ReadShowHeaderSection(suf);

            ReadTimeSection(suf);

			ReadServicesSection(suf);

            ReadLabelsSection(suf);

			ReadFileSection(suf);

			ReadSurveySection(suf);

            ReadDemographicsSection(suf);

            ReadReferralsSection(suf);

            suf.Close();

            // TODO: Major - Now call methods of each type (Services, Surveys, etc) to
            //       do any error checking required. For example, the [Survey] section
            //       may have 3 questions, but the [FILE] section has 2 (or worse, 4)
            //       field codes for Surveys.
		}

//---------------------------------------------------------------------------------------

		void ReadFrontSection(BartIniFile suf) {
			FrontSection = new List<string>();

			if (! suf.FindSection(BartIniFile.FrontName)) {
				return;			// Should never happen
			}

			string	line;
			while ((line = suf.ReadLine()) != null) {
				line = line.Trim();
				if (line.Length > 0)
					FrontSection.Add(line);
			}
		}

//---------------------------------------------------------------------------------------

        void ReadTerminalSection(BartIniFile suf) {
			string		s;

			// The TerminalName has been set in the ctor to be UnknownTerminalName. So
			// if any of our checks fail, we can just return.
			if (! suf.FindSection("Terminal"))
				return;
 			if ((s = suf.ReadLine()) == null)
				return;

			TerminalName = s.Trim();

			// To show how regular expressions can really, really help in certain
			// situations (like this one), here's the (#if'd out) code to parse the
			// terminal name using traditional techniques, then the same result
			// achieved using re's. You decide.
#if false
			if (TerminalName.Length == 0)
				TerminalName = UnknownTerminalName;
			else {
				// Now it gets really boring. We expect ID=TerminalName.
				// But error check to make sure that a) we found an equals
				// sign, and that there was indeed something to its right.
				char [] seps = {'='};
				string [] parts = TerminalName.Split(seps, 2);
				if (parts.Length == 1)		// Check if no "=" found
					TerminalName = UnknownTerminalName;
				else {
					TerminalName = parts[1].Trim();
					if (TerminalName.Length == 0) 
						TerminalName = UnknownTerminalName;
				}
			}
#else
			// All we're really looking for is something on the RHS of an '='. We don't
			// really care about what's on the LHS. 
			Regex	re = new Regex(@"^.+?=(?<TermName>.+)$");
			Match	m  = re.Match(TerminalName);
			if (m.Success)
				TerminalName = (string)m.Groups["TermName"].Captures[0].Value;
			else
				TerminalName = UnknownTerminalName;
#endif
				
			// Ignore subsequent lines, if any. Note however that we could keep scanning
			// until we find a non-empty line, and use that as the TerminalName. 
		}

 //---------------------------------------------------------------------------------------

        void ReadShowHeaderSection(BartIniFile suf) {
            string s;

            // The ShowHeader has been set in the ctor to be empty. So
            // if any of our checks fail, we can just return.
            if (! suf.FindSection("ShowHeader"))
                return;
            if ((s = suf.ReadLine()) == null)
                return;

            ShowHeader = s.Trim();

            // Ignore subsequent lines, if any. Note however that we could keep scanning
            // until we find a non-empty line, and use that as the ShowHeader. 
        }

//---------------------------------------------------------------------------------------

        void ReadTimeSection(BartIniFile suf) {
            string s;

            // The DateDisplay has been set in the ctor to a default value. So
            // if any of our checks fail, we can just return.
            if (! suf.FindSection("TIME"))
                return;
            if ((s = suf.ReadLine()) == null)
                return;

            s = s.Trim();
            // All we're really looking for is something on the RHS of an '='. We don't
            // really care about what's on the LHS. 
            Regex re = new Regex(@"^.+?=(?<DateDisplay>.+)$");
            Match m = re.Match(s);
            if (m.Success)
                DateDisplay = ((string)m.Groups["DateDisplay"].Captures[0].Value).Trim();

            // Ignore subsequent lines, if any. Note however that we could keep scanning
            // until we find a non-empty line, and use that as the TerminalName. 
        }

//---------------------------------------------------------------------------------------

        void ReadLabelsSection(BartIniFile suf) {
			Labels = Label.ParseSetupFile(suf);	
		}

//---------------------------------------------------------------------------------------

        void ReadServicesSection(BartIniFile suf) {
			Services = Service.ParseSetupFile(suf);	
		}

//---------------------------------------------------------------------------------------

        void ReadFileSection(BartIniFile suf) {
			File = FileFields.ParseSetupFile(suf);

#if DEBUG && true && ! SQLSERVER
			string		s = "";
			string		nums;
			foreach (FileFields fld in File) {
				if (s.Length > 0) {
					s += " ";
				}
				nums = "";
				foreach (int n in fld.FieldCodes) {
					if (nums.Length > 0) {
						nums += ",";
					}
					nums += n.ToString();
				}
				s += nums;
			}
			Console.WriteLine("FILE: {0}", s);
#endif
		}

//---------------------------------------------------------------------------------------

        void ReadSurveySection(BartIniFile suf) {
			Surveys = Survey.ParseSetupFile(suf);

#if DEBUG && true && ! SQLSERVER
			Survey	sur;
			for (int i=0; i<Surveys.Count; ++i) {
				sur = Surveys[i];
				Console.WriteLine("Survey[{0}] Question = {1}", i, sur.Question);
				for (int j=0; j<sur.Answers.Count; ++j) {
					Console.WriteLine("\t\tAnswer[{0}] = {1}", j, sur.Answers[j]);
				}
			}
#endif
		}

//---------------------------------------------------------------------------------------

        void ReadDemographicsSection(BartIniFile suf) {
            Demographics = Demographic.ParseSetupFile(suf);

#if DEBUG && true && ! SQLSERVER
            Demographic     dem;
            for (int i = 0; i < Demographics.Count; ++i) {
                dem = Demographics[i];
                Console.WriteLine("Demographics[{0}] Question = {1}", i, dem.Question);
                for (int j = 0; j < dem.Answers.Count; ++j) {
                    Console.WriteLine("\t\tAnswer[{0}] = {1}", j, dem.Answers[j]);
                }
            }
#endif
		}

//---------------------------------------------------------------------------------------

        void ReadReferralsSection(BartIniFile suf) {
            Refs = new Referrals();
            Refs.ParseSetupFile(suf);   // TODO: Make part of ctor
        }

//---------------------------------------------------------------------------------------

		[Conditional("DEBUG")]
        public void Dump(StreamWriter sw) {
#if DEBUG && true && ! SQLSERVER
            StringBuilder   sb = new StringBuilder();   // Work area
            string          s;                          // Another work area
            int             n;                          // Generic counter

            // ### HEADER ###
			sb.Length = 0;
			sb.AppendFormat("### File dump of map.cfg from {0} ###", filename);
            sw.WriteLine(sb.ToString());
			foreach (string str in FrontSection) {
				sw.WriteLine(str);
			}
            sw.WriteLine();

            // [TERMINAL]
            if (TerminalName != UnknownTerminalName) {
                sw.WriteLine("[TERMINAL]");
                sw.WriteLine(TerminalName);
            }
            sw.WriteLine();

            // [SHOWHEADER]
            if (ShowHeader != "") {
                sw.WriteLine("[SHOWHEADER]");
                sw.WriteLine(ShowHeader);
            }
            sw.WriteLine();

            // [TIME]
            sw.WriteLine("[TIME]");
            sw.WriteLine(DateDisplay);
            sw.WriteLine();

            // [LABELS]
            sw.WriteLine("[LABELS]");
            foreach (Label lbl in Labels) {
                sb.Length = 0;
                sb.AppendFormat("{0,-3}   {1}", lbl.FileNum, lbl.Caption);
                sw.WriteLine(sb.ToString());
            }
            sw.WriteLine();

            // [FILE]
            // Note: We don't keep track of whether this data came in as FILE or
            //       SERIALSTREAM. Here we'll just call it FILE
            // TODO: We don't currently support "5,6,7"
            sw.WriteLine("[FILE]");
			foreach (FileFields flds in File) {
                sw.Write(flds.ToString()); sw.Write(" ");
            }
            sw.WriteLine();             // Finish off the [FILE] line
            sw.WriteLine();             // Add standard blank line afterwards

            // [QUESTIONS] (aka Services)
            sw.WriteLine("[QUESTIONS]");
            foreach (Service srv in Services) {
                sw.WriteLine(srv.service);
            }
            sw.WriteLine();

            // [DEMOGRAPHICS]
            sw.WriteLine("[DEMOGRAPHICS]");
            n = 1;
            foreach (Demographic demog in Demographics) {
                sb.Length = 0;
                sb.AppendFormat("DHEADER{0}={1}", n++, demog.Question);
                sw.WriteLine(sb.ToString());
                foreach (DemographicAnswer ans in demog.Answers) {
                    sb.Length = 0;
                    sb.AppendFormat("{0},{1}", ans.ID, ans.Answer);
                    sw.WriteLine(sb.ToString());
                }
            }
            sw.WriteLine();

			// [SURVEYS]
			sw.WriteLine("[SURVEYQUES]");
			n = 1;
			foreach (Survey surv in Surveys) {
				sb.Length = 0;
				sb.AppendFormat("SURQUES{0}={1}", n++, surv.Question);
				sw.WriteLine(sb.ToString());
				foreach (string ans in surv.Answers) {
					sw.WriteLine(ans);
				}
			}
			sw.WriteLine();

            // [REFERRALS]
            sw.WriteLine("[REFERRALS]");
            s = Refs.DefaultFromPartner;
            sw.Write("DEFTOPARTNER=");
            sw.WriteLine(s == null ? "(null)" : s);
            s = Refs.DefaultToPartner;
            sw.Write("DEFFROMPARTNER=");
            sw.WriteLine(s == null ? "(null)" : s);
            s = Refs.DefaultProvider;
            sw.Write("DEFPROVIDER=");
            sw.WriteLine(s == null ? "(null)" : s);
            sw.WriteLine("<@@FROMPARTNER@@>");
            foreach (string str in Refs.FromPartners) {
                sw.WriteLine(str);
            }
            sw.WriteLine("<@@TOPARTNER@@>");
            foreach (string str in Refs.ToPartners) {
                sw.WriteLine(str);
            }
            sw.WriteLine("<@@PROVIDER@@>");
            foreach (string str in Refs.Providers) {
                sw.WriteLine(str);
            }
            sw.WriteLine();
#endif
        }
    }
}
