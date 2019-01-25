using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CoreDownloadMsConferences {

	[Serializable]
	public class ConfigData {
		const string			ConfigFilename = "DownloadMicrosoftConferences.Config.xml";
		public List<string>		Topics;
		public string			DefaultTargetDir;
		public int				NumberOfSimultaneousDownloads;
		public int				ProcessorCountMultiplier;
		public Filters			GlobalFilters;
		public int				MoveToTheRearOfTheBusPlease;
		public List<Conference> Conferences;

//---------------------------------------------------------------------------------------

		public ConfigData() {
			Topics           = new List<string>();
			DefaultTargetDir = ".";
			GlobalFilters    = new Filters();
			Conferences      = new List<Conference>();
		}

//---------------------------------------------------------------------------------------

		public static ConfigData LoadConfig() {
			// TODO: Some error handling might be nice
			// string ConfigFilename = "DownloadMicrosoftConference-LRS.xml";
			using (Stream s = File.OpenRead(ConfigFilename)) {
				XmlSerializer x = new XmlSerializer(typeof(ConfigData));
				return (ConfigData)x.Deserialize(s);
			}
		}

#if true
		public static void SaveConfig(ConfigData cfg) {     // The basis for updating the config file, but not used
			// string ConfigFilename = "DownloadMicrosoftConference-LRS.xml";
			try {
				using (Stream s = File.OpenWrite(ConfigFilename)) {
					XmlSerializer x = new XmlSerializer(typeof(ConfigData));
					x.Serialize(s, cfg);
				}
			} catch (Exception /* ex */) {
				throw;                          // TODO: Do something better later
			}
		}
#endif
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[Serializable]
	public class Filters {
		[XmlArray]
		public List<string> Includes = new List<string>();
		[XmlArray]
		public List<string> Excludes = new List<string>();

//---------------------------------------------------------------------------------------

		public Filters() {
		}
	}
}
