using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DownloadMsConferences {
	[Serializable]
	public class Conference {
		public string					Name;
		public string					TargetDir;
		public string					RssUrl;
		public List<string>				Topics;   // e.g. Azure/Cloud, Android, iOS. May be null
		public List<MyPseudoDictionary> Dict;

		[XmlIgnore]
		public Dictionary<string, string> ConferenceTopics;

//---------------------------------------------------------------------------------------

		public override string ToString() => Name;      // For combo box

//---------------------------------------------------------------------------------------

		// Categorize talks by keywords in their titles, according to what's in
		// the Topics / SpecialTopics entries in the config file. Put the files in
		//  either the main directory, or else in a sub-directory based on the 
		// topic name.
		// If a Title contains more than one Topic phrase, in all other directories,
		// put a link to the original file. For example, a Title of, say, "Using C#
		// to Write Programs for Android, IOS and the Windows Store" would (assuming
		// the Config file is set up this way) put the .mp4 file into the C#
		// sub-directory, and put links in all the other relevant sub-directories
		// (Android, IOS, Windows Store) to the .mp4 file in the C# directory.

		/// <summary>
		/// Takes a session title and, based on the Conferences instance variable (from
		/// the config file) identifies the main (Primary) topic for the session and
		/// any other (secondary) topics.
		/// </summary>
		/// <param name="Title">A session title</param>
		/// <param name="SecondaryTopics">A List of secondary topics</param>
		/// <returns>The main topic for this session</returns>
		public string CategorizeSessionTopics(string Title, out List<string> SecondaryTopics) {
			string PrimaryTopic = null;
			SecondaryTopics     = new List<string>();

			// Originally we just got a topic (e.g. "Git") and looked to see if that
			// string (case-insensitive) was anywhere in the session Title. But a Title
			// with the word "digital" would match "Git", which isn't what we'd want.
			// So we'll work at the word level. We'll tidy up both the Title and the
			// topic (i.e. replace all non-letters/numbers by blanks) and use .Split() to
			// get the words. And since topics may be a phrase (e.g. "Windows 10"), once
			// we find a word we have to look at the rest of the words in the title to
			// see if they're an exact (but case-insensitive) match for the rest of the
			// topic phrase.

			// Note: This isn't perfect. For example, if we were looking for a topic of
			//		 "Windows 10", and the Title was something like "Enhancing Windows -
			//		 10 ways to do so", we'd think this was a "Windows 10" article when
			//		 it might not be. If this ever becomes an issue I might revisit this
			//		 code. But until then...

			// Small side-note: I originally thought of simply forcing blanks around a
			// topic, but quickly realized that finding, say, " Git " wouldn't work if
			// it was the first or last word in the Title. So we'll do it right. Sigh.

#if false		
				// The original code for this routine, just to show you what
				// we're trying to accomplish (albeit in a what-turned-out-to-be a too
				// simple approach.
				if (Title.IndexOf(topic, StringComparison.OrdinalIgnoreCase) >= 0) {
					if (PrimaryTopic == null) {
						PrimaryTopic = ConferenceTopics[topic];
					} else {
						SecondaryTopics.Add(topic);
					}
				}
#endif

			var Blanks = new char[] { ' ' };
			var TitleWords = Tidy(Title).Split(Blanks, StringSplitOptions.RemoveEmptyEntries);
			foreach (string topic in ConferenceTopics.Keys) {

				var TopicWords = Tidy(topic).Split(Blanks, StringSplitOptions.RemoveEmptyEntries);
				// Go through each topic
				for (int i = 0; i < TopicWords.Length; ++i) {
					if (i > 0) {
						// We haven't matched the first word. Can't have a match
						goto NextTopic;
					}
					string TopicWord = TopicWords[i];
					// Go through each word in the title, looking for the topic
					for (int j = 0; j < TitleWords.Length; j++) {
						if (string.Equals(TopicWord, TitleWords[j], StringComparison.OrdinalIgnoreCase)) {
							// OK, we have a match on the first word of the topic. Now
							// run through the rest of the topic words matching against
							// each word of the Title. Make sure we don't overrun the
							// end of the Title (e.g. looking for { "Windows", "10"} and
							// the Title ends with just "Windows".
							bool bAllWordsMatched = true;
							// We have the start of a match between the first word of the
							// topic, and a word in the Title. See if the rest of the
							// words in the Title match the rest of the word in the
							// topic. March along each in turn. If we match all of the
							// words in the topic, fine. If not, we haven't found a match
							// and we'll keep looking. For example, suppose the topic
							// were "Microsoft Edge" and the Title was "Using the
							// Microsoft Band as part of a healthy life style". Then
							// eventually we'd find the word "Microsoft" in the title.
							// Then "i" would be "0" (hence "k" in the next line would
							// start at "1") and "j" would be "2".
							for (int k = 1; k < TopicWords.Length; k++) {
								if (++j >= TitleWords.Length) {	// Check for end of Title
									bAllWordsMatched = false;
									break;
								}
								if (! string.Equals(TopicWords[k], TitleWords[j], StringComparison.OrdinalIgnoreCase)) {
									bAllWordsMatched = false;  // Didn't match every word
									break;
								}
							}
							if (bAllWordsMatched) {
								if (PrimaryTopic == null) {
									PrimaryTopic = ConferenceTopics[topic];
								} else {
									SecondaryTopics.Add(topic);
								}
								// Until C# gets labeled break's, I'll stick to goto's!
								goto NextTopic;
							}
						}
					}
				}
				NextTopic:
				;
			}
			return PrimaryTopic;
		}

//---------------------------------------------------------------------------------------

		private string Tidy(string Title) {
			var sb = new StringBuilder(Title.Length);
			foreach (char c in Title) {
				// Must leave '#' and '+' as-is for "C#" and "C++". Also '.' for ".Net"
				if (char.IsLetter(c) || char.IsDigit(c) || (c == '#') || (c == '+') || (c == '.')) {
					sb.Append(c);
				} else {
					sb.Append(' ');
				}
			}
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		public void SetupTopics(ConfigData cfg) {
			// We want to put files in sub-directories. For example, any files with, say,
			// "Android" in their title would go into an "Android" subdirectory. Note
			// however that we look for the keywords in the order they occur in the
			// config file. So if a Title were, say, "Using C# under Xamarin to program
			// for Android, IOS and Windows Phone", then (if the following keywords are
			// in the config file), the file could be placed in the C#, Xamarin, Android,
			// IOS or Windows Phone subdirectory, depending on which text is found first.
			ConferenceTopics = new Dictionary<string, string>();
			if (Topics.Count == 0) {
				// If the Conference definition in the config file doesn't have its own
				// SpecialTopics list, then use the default set of topics
				Topics = cfg.Topics;
			}
			foreach (string topic in Topics) {
				// We want to support aliases. For example, a topic of "Azure" would, of
				// course, go into the "Azure" sub-directory. But we might also want any
				// reference to "Cloud" to go there too. So I've defined the syntax in
				// the config file that if a topic looks like "Cloud\Azure", then all
				// titles with "Cloud" in their name (unless selected by an earlier
				// keyword, as described above) would be put into the "Azure"
				// sub -directory.
				var topics = topic.Split('\\');
				if (topics.Length > 1) {
					ConferenceTopics[topics[0]] = topics[1];
				} else {
					ConferenceTopics[topics[0]] = topics[0];   // No slash. "Azure" -> "Azure"
				}
			}
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public struct MyPseudoDictionary {
			public string Key;
			public string Value;
		}

	}
}

