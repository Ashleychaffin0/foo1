using System;
using System.Xml;
using System.Collections;

namespace FixRSSBanditXML {
	/// <summary>
	/// Summary description for FixRSSBanditXML.
	/// </summary>
	class FixRSSBanditXML {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			if (args.Length != 2) {
				Console.WriteLine("Usage: FixRSSBanditXML infile outfile");
				return;
			}
			FixRSSBanditXML	fix = new FixRSSBanditXML();
			fix.Run(args[0], args[1]);
		}

//---------------------------------------------------------------------------------------

		void Run(string inFilename, string outFilename) {

			Hashtable	ht = new Hashtable();
			string		cat, feed, catFeed;

			try {
				XmlDocument	xdoc = new XmlDocument();
				xdoc.Load(inFilename);
				XmlNodeList xnl = xdoc.GetElementsByTagName("feed");
				foreach (XmlNode node in xnl) {
					if (node.Name == "feed") {
						feed = "";
						foreach (XmlNode kid in node.ChildNodes) {
							if (kid.Name == "title") {
								feed = kid.InnerText;
								break;
							}
						}
						cat = node.Attributes["category"].Value;
						catFeed = cat + "\\" + feed;
						if (! ht.Contains(catFeed)) {
							ht[catFeed] = 1;
						} else {
							ht[catFeed] = (int)ht[catFeed] + 1;		
						}
						Console.WriteLine("Node name is {0}, Category={1}, title={2}", node.Name, cat, feed);
					}
				}
#if false
					// Console.WriteLine("LRS={0}", node.Attributes.GetNamedItem("xxxLRS").Value);
					// Add the key in lower case. When we search, we'll also convert first
					// to lower case. Hopefully this should save us some spurious
					// mismatches that differ only in case.
					// Note: In principle we could have created the hashtable with a
					// System.Collections.CaseInsensitiveHashCodeProvider. But rather
					// than experimenting with it, and taking the time to ensure I got
					// it right, I just threw in a ToLower(). Close enough for now.
					// Note: Actually, the ToLower() comes in handy for things like
					// validation deletion strings. It saves us from having to call
					// ToLower() every time.
					if (bFlip) {
						// e.g. ht["36037"] = "Genesee"
						// But we don't support this if AttrName is null
						if (AttrName != null)
							ht[node.Attributes[AttrName].Value.ToLower()] = node.InnerText;
					} else {
						// e.g. ht["genesee"] = "36037"
						if (AttrName == null) {
							// A null AttrName means we don't care what the associated
							// value is; we're just interested in the keys
							ht[node.InnerText.ToLower()] = 0;
						} else {
							ht[node.InnerText.ToLower()] = node.Attributes[AttrName].Value;
						}
					}
				}
#endif
			} catch (Exception e) {
				string	msg;
				msg = "Error '{0}' processing input file {1}";
				Console.WriteLine(msg, e.Message, inFilename);
#if false
				msg = "The configuation file '" + filename + "' either could not be found"
					+ " in the WIALink directory or is invalid."
					+ "\n\nThe '" + ErrorText + "' field may not be added to OSOS correctly. It is strongly"
					+ " suggested that you exit WIALink and fix the problem."
					+ "\n\nDiagnostic information: " + e.Message;
				MessageBox.Show(msg + e.Message, "WIALink Configuration Warning",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
			}

			foreach (string key in ht.Keys) {
				if (key.IndexOf("Gunner") > 0)
					Console.WriteLine("Count = {0}, catFeed = {1}", ht[key], key);
			}
		}
	}
}
