using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DownloadMsConferences {

	[Serializable]
	public class ConfigData {
		const string ConfigFilename = "DownloadMicrosoftConferences.Config.xml";

//---------------------------------------------------------------------------------------

		public List<string>		Topics;
		public List<Conference> Conferences;

//---------------------------------------------------------------------------------------

		public static ConfigData GetConfig() {
			// TODO: Some error handling might be nice
			using (Stream s = File.OpenRead(ConfigFilename)) {
				XmlSerializer x = new XmlSerializer(typeof(ConfigData));
				return (ConfigData)x.Deserialize(s);
			}
		}

#if false      // PutConfigInfo() -- the basis for updating the config file, but not used
//---------------------------------------------------------------------------------------

		private void PutConfigInfo() {
			var cfg2 = new CONFIG();
			cfg2.Conferences = new List<Conference>();
			var conf1 = new Conference {
				Name = "Build 2015",
				TargetDir = @"D:\Downloads\Build2015",
				RssUrl = "http://s.ch9.ms/events/build/2015/rss/mp4",
				Dict = new List<MyStruct>(),
				// SpecialTopics = new List<string> { "Azure", "Cloud", "Android", "iOS" }
			};
			conf1.Dict.Add(new MyStruct { Key = "Hello", Value = "World" });
			conf1.Dict.Add(new MyStruct { Key = "Goodbye", Value = "Cruel" });
			cfg2.Conferences.Add(conf1);
			cfg2.Conferences.Add(new Conference {
				Name = "Connect 2015",
				TargetDir = @"D:\Downloads\Connect 2015",
				RssUrl = "http://channel9.msdn.com/Events/Visual-Studio/Connect-event-2015/RSS",
				Dict = new List<MyStruct>(),
				// SpecialTopics = new List<string> { "Azure", "Android", "Cloud", "iOS" }
			});

			cfg2.Topics = new List<string>();
			cfg2.Topics.Add("Entry 1");
			cfg2.Topics.Add("Entry 2");
			cfg2.Topics.Add("Entry 3");

			//Conferences        = new List<Conference>();
			//Conferences.Add(new Conference {
			//	Name      = "Build 2015",
			//	TargetDir = @"D:\Downloads\Build2015",
			//	RssUrl    = "http://s.ch9.ms/events/build/2015/rss/mp4",
			//	SpecialTopics = new List<string> { "Azure", "Cloud", "Android", "iOS"}
			//	//}
			//});
			//	//}
			//});

			// TODO: Put into base directory, not DEBUG directory
			string Filename = "DownloadMicrosoftConference-LRS.xml";
			try {
				using (Stream s = File.OpenWrite(Filename)) {
					XmlSerializer x = new XmlSerializer(typeof(CONFIG));
					x.Serialize(s, cfg2);
				}
			} catch (Exception ex) {
				MessageBox.Show("Unable to write config file " + Filename + ", error = " + ex.Message, "GetAPOD");
			}
		}
#endif
	}
}
