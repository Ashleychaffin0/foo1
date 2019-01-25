using System;
using System.Collections;
using System.IO;
using System.Xml;

using Bartizan.ActivityTrack30.Common;

namespace Bartizan.ActivityTrack30 {

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	abstract class BartDB {
		// I don't really know enough yet about SQL Server et al, but let's assume that
		// this is an abstract class with (at least) simple DAO-like methods, such as
		// Open[Recordset], AddNew, Update, Close, Seek, Move[First/Last/Next/Previous]
		// and so forth. We can then derive BartSQLServer2000, BartSQLServerYukon, 
		// BartJet, etc.
		// We'll also need methods for Security, "relinking tables" (so to speak), etc.
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// A toy, which only does a Console.Writeline. Later it has to write data to a
	/// database table.
	/// </summary>
	class LogFile {

		public void Log(string fmt, params object [] args) {
			Console.WriteLine(fmt, args);
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
	class FieldInfo {
		public string	name;
		public string	type;

		// The following must be kept in synch with the .xml file
		public const string FieldType_Services	   = "Services";
		public const string FieldType_Demographics = "Demographics";
		public const string FieldType_Survey	   = "Survey";

//---------------------------------------------------------------------------------------

		public FieldInfo(string name, string type) {
			this.name = name;
			this.type = type;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	/// <summary>
	/// 
	/// </summary>
	public class Importer 	{
		Hashtable		BBoxFieldDefs;
		Hashtable		SwipeData_Basic;// Name, addr, etc, but not Services, Demogs, etc
		ArrayList		SwipeData_Demographics;
		ArrayList		SwipeData_Services;
		ImportParms		imp;
		LogFile			log;

		// When processing field type 12 = Services, things get a little crazy. We may
		// have, say, 100 services in the setup file, and 100 fields on the swipe
		// record, but we'll have only a single "12" in the [FILE] section. Thus any
		// fields after a "12" (e.g. Note="18") won't be, say, field 34, but 134.
		// We here define an offset field. It will be set to 0 for each new record,
		// but if we get a "12", we'll set it to the number of services minus 1 (e.g.
		// 99 - not 100, since the "12" counts as 1) and all subsequent fields 
		// in the record will be accessed with this offset.
		int				ServicesOffset;

		// The following must be kept in synch with the .xml file
		const string	Field_CardNum			= "1";
		const string	Field_FirstName			= "14";
		const string	Field_Initials			= "507";
		const string	Field_LastName			= "15";
		const string	Field_Street			= "5";
		const string	Field_City				= "6";
		const string	Field_Zip				= "7";
		const string	Field_SSN				= "506";
		// TODO: Add the rest



//---------------------------------------------------------------------------------------

		public Importer(string FieldFilename) {
			log = new LogFile();

			BBoxFieldDefs   = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());
			SwipeData_Basic = new Hashtable(new CaseInsensitiveHashCodeProvider(), new CaseInsensitiveComparer());

			SwipeData_Demographics = new ArrayList();
			SwipeData_Services	   = new ArrayList();

			try {
				XmlDocument	xdoc = new XmlDocument();
				xdoc.Load(FieldFilename);
				XmlNodeList xnl = xdoc.GetElementsByTagName("Field");
				foreach (XmlNode node in xnl) {
					BBoxFieldDefs[node.InnerText] = new FieldInfo(node.Attributes["Name"].Value, node.Attributes["Type"].Value);
				}
			} catch (Exception e) {
				log.Log("An error ({0}) occurred importing ImportFieldDefs.xml", e.Message);
				// TODO: Need to return retcode (hard to do from a ctor), or throw, or something
				return;
			}
		}

//---------------------------------------------------------------------------------------

		public void ImportFromDirectory(string DirName) {
			string		VisitName;			// Name of Visitor*.txt
			string		VisitSuffix;		// "1" of "Visitor1.txt"
			string		MapFileName;
			string []	files = Directory.GetFiles(DirName, "Visitor*.txt");

			// These next two lines are no longer need, now that I know
			// about Path.Combine()
			// if (! DirName.EndsWith(@"\"))
				// DirName += @"\";
			foreach (string file in files) {
				try {						// Recover on a per-file basis
					VisitName = Path.GetFileName(file);	// Strip directory name
					// Calculate the length of the suffix. "Visitor.txt" is 11 chars,
					// so anything between "Visitor" and ".txt" is (length of entire
					// string - 11) characters long.
					int		SuffixLen = VisitName.Length - 11;	
					VisitSuffix = VisitName.Substring(7, SuffixLen);// Suffix starts at pos 7
					MapFileName = "Map" + VisitSuffix + ".cfg";
					ImportData(Path.Combine(DirName, MapFileName));
				} catch (Exception e) {
					// We've had a problem with the file. Oh well, log it and go on
					// to the next.
					log.Log("Exception ({0}) in ImportFromDirectory", e.Message);
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given the name of a setup file (e.g. MAP.CFG), get the setup information,
		/// then import the data from the associated data file (e.g. VISITOR.TXT).
		/// <p/>
		/// Note: The MAP.CFG suffix (e.g. "2" from MAP2.CFG) is picked up, and will
		/// be used to generate the name VISITOR2.TXT.
		/// <p/>
		/// Note: We don't currently support swipe data inside the setup file, but it
		/// should be possible to use a Stream on top of a Stream to do this.
		/// </summary>
		/// <param name="filename">The name of the file with the setup 
		/// information.</param>
		public void ImportData(string filename) {
			Console.WriteLine("Importing data from {0}", filename);	// TODO: Debug
			SetupFile	suf = new SetupFile(filename);
			imp = new ImportParms();
			imp.LoadFromSetupFile(suf);
			
			// TODO: Check to see if Visitor data in setup file. But for now, assume
			//		 it's in a separate file.
			string	path = Path.GetDirectoryName(filename);
			string	VisitFilename = path + @"\Visitor" + GetSuffix(Path.GetFileName(filename)) + ".txt";
			// TODO: Check that file exists

			StreamReader sr = new StreamReader(VisitFilename);
			string		line;
			string		FieldID;
			Swipe		swp = new Swipe();
			while ((line = sr.ReadLine()) != null) {
				swp.Parse(line);
				SwipeData_Basic.Clear();
				SwipeData_Demographics.Clear();
				SwipeData_Services.Clear();
				ServicesOffset = 0;
				for (int i=0; i<imp.file.Count; ++i) {
					FieldID = (string)imp.file[i];
					if (BBoxFieldDefs.Contains(FieldID)) {
						ProcessField(i, FieldID, swp);
					} else {
						// TODO:
						log.Log("Don't know how to process {0}", FieldID);
					}
				}
				// We now have in SwipeData, the information for a single swipe.
				// Do something with it, such as adding it to a database. TODO:
#if true			// TODO: Debugging
				Console.WriteLine("-------------------");
				FieldInfo	fi;
				foreach (string key in SwipeData_Basic.Keys) {
					fi = (FieldInfo)BBoxFieldDefs[key];
					Console.WriteLine("SwipeData[{0}] = {1}", fi.name, SwipeData_Basic[key]);
				}
				for (int i=0; i<SwipeData_Services.Count; ++i) {
					Console.WriteLine("\tService[{0}, {1}] = {2}", i, imp.Services[i],
						SwipeData_Services[i]);
				}
#endif
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessField(int i, string FieldID, Swipe swp) {
			// The record number at the beginning of each record is an implicit field
			// and not included in the [File] descriptors. So get Fields[i+1]
			FieldInfo	fi = (FieldInfo)BBoxFieldDefs[FieldID];
			switch (fi.type) {
			case FieldInfo.FieldType_Services:
				for (int j=0; j<imp.Services.Count; ++j) {
					SwipeData_Services.Add(swp.Fields[i + j + 1]);
				}
				ServicesOffset = imp.Services.Count - 1;
				break;
			case FieldInfo.FieldType_Demographics:
				// TODO:
				break;
			case FieldInfo.FieldType_Survey:
				break;
			default:
				SwipeData_Basic[FieldID] = swp.Fields[i + 1 + ServicesOffset];
				break;
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// We may be importing, not MAP.CFG, but MAP1.CFG. Or MAP2.CFG. And so on.
		/// Return the x in MAPx.CFG (where x can be the empty string). We'll use the to
		/// generate the name of the corresponding VISITORx.TXT file, if any.
		/// </summary>
		/// <returns>The text (possibly empty) between "MAP" and ".CFG"</returns>
		string GetSuffix(string filename) {
			string CFGName = Path.GetFileName(filename);	// Strip directory name
			// Calculate the length of the suffix. "MAP.CFG" is 7 chars,
			// so anything between "MAP" and ".CFG" is (length of entire
			// string - 7) characters long.
			int		SuffixLen = CFGName.Length - 7;	
			return CFGName.Substring(3, SuffixLen);	// Suffix starts at pos 3
		}
	}
}
