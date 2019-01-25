using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace GetPiPublicLectures_4 {
	[Serializable]
	public class PiConfigData {
		const string		ConfigFilename = "PiConfigData.xml";
		public string		DefaultTargetDir;
		public int			NumberOfSimultaneousDownloads;
		public int			ProcessorCountMultiplier;
		public int			MoveToTheRearOfTheBusPlease;
		public List<string>	FileTypes;

//---------------------------------------------------------------------------------------

		public PiConfigData() {
			DefaultTargetDir = ".";
			FileTypes        = new List<string>();
		}

//---------------------------------------------------------------------------------------

		public static PiConfigData LoadConfig() {
			// TODO: Some error handling might be nice
			using (Stream s = File.OpenRead(ConfigFilename)) {
				XmlSerializer x = new XmlSerializer(typeof(PiConfigData));
				return (PiConfigData)x.Deserialize(s);
			}
		}

//---------------------------------------------------------------------------------------

		public static void SaveConfig(PiConfigData cfg) {     // The basis for updating the config file, but not used
			try {
				using (Stream s = File.OpenWrite(ConfigFilename)) {
					XmlSerializer x = new XmlSerializer(typeof(PiConfigData));
					x.Serialize(s, cfg);
				}
			} catch (Exception /* ex */) {
				throw;                          // TODO: Do something better later
			}
		}
	}
}

