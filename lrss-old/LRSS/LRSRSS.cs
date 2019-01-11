// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

using System.IO;

namespace LRSS {

	public class RSSFeed {
		public string			URL;
		public string			Title;
		public string			Description;
		public DateTime			PubDate;
		public int				FeedID;

        [XmlIgnore]
		public List<RSSItem>	Items;

		Hashtable				OtherNodes;

        [XmlIgnore]
		public bool				LoadedOK;

		// TODO: Add Event for progress report while loading?

//---------------------------------------------------------------------------------------

		public RSSFeed() {
			URL			= "";
			Title		= "(No Title)";
			Description = "(No Description)";
			PubDate		= default(DateTime);
			FeedID		= -1;
			Items		= new List<RSSItem>();
			OtherNodes	= new Hashtable();
			LoadedOK	= false;
		}

//---------------------------------------------------------------------------------------

		public RSSFeed(string Url)
				: this() {
			URL			= Url;
		}

//---------------------------------------------------------------------------------------

		public void Load() {
			// Load the document
			string s = Load(URL);
			if (s == null) {
				LoadedOK = false;
				return;
			}
			XmlDocument	xdoc = new XmlDocument();
			xdoc.LoadXml(s);

			// Get the basic elements
			XmlNodeList		nl = xdoc.SelectNodes("/rss/channel/*");
			string			InnerText;
			foreach (XmlNode node in nl) {
				InnerText = node.InnerText;
				switch (node.Name.ToUpper()) {
				case "TITLE":
					Title = InnerText;
					break;
				case "DESCRIPTION":
					Description = InnerText;
					break;
				case "PUBDATE":
					if (InnerText.EndsWith(" PST"))
						InnerText = InnerText.Substring(0, InnerText.Length - 4);
					PubDate = DateTime.Parse(InnerText);
					break;
				case "ITEM":
					// Ignore it, and pick them up below
					break;
				default:
					OtherNodes[node.Name] = InnerText;
					break;
				}
			}
			
			// Get the items
			XmlNodeList	list = xdoc.SelectNodes("/rss/channel/item");
			foreach (XmlNode node in list) {
				Items.Add(new RSSItem(node));
			}
			LoadedOK = true;
		}

//---------------------------------------------------------------------------------------

		private string Load(string Url) {
			LoadedOK = true;
			// TODO: Note: This should be done on a worker thread.
			if (Url.ToLower().StartsWith("http://") || (Url.ToLower().StartsWith("https://"))) {
				// Do nothing
			} else {
				Url = "http://" + Url;
			}
			try {
				WebRequest wreq = WebRequest.Create(Url);
				WebResponse wresp = wreq.GetResponse();
				// Console.WriteLine(wresp.StatusDescription);	// Display the status.
				// Get the stream containing content returned by the server.
				Stream dataStream = wresp.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				StreamReader reader = new StreamReader(dataStream);
				// Read the content.
				string responseFromServer = reader.ReadToEnd();	
				// Cleanup the streams and the response.
				reader.Close();
				dataStream.Close();
				wresp.Close();
				return responseFromServer;
			} catch {
				LoadedOK = false;
				return null;
			}
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


    public class RSSItem {
        public string Title;
        public string Description;
        public string Link;
        public DateTime pubDate;

        Hashtable OtherElements;

//---------------------------------------------------------------------------------------

        public RSSItem() {
            Title = "(No Title)";
            Description = "(No Description)";
            Link = "";
            pubDate = default(DateTime);
            OtherElements = new Hashtable();
        }

//---------------------------------------------------------------------------------------

        public RSSItem(XmlNode node) {
            OtherElements = new Hashtable();
            string InnerText;
            foreach (XmlElement elem in node.ChildNodes) {
                InnerText = elem.InnerText;
                switch (elem.Name.ToUpper()) {
                    case "TITLE":
                        Title = InnerText;
                        break;
                    case "DESCRIPTION":
                        Description = InnerText;
                        break;
                    case "LINK":
                        Link = InnerText;
                        break;
                    case "PUBDATE":
                        // TODO: Define routine to parse dates better. Also use below.
                        if (InnerText.EndsWith(" PST"))
                            InnerText = InnerText.Substring(0, InnerText.Length - 4);
                        pubDate = DateTime.Parse(InnerText);
                        break;
                    default:
                        OtherElements[elem.Name] = InnerText;
                        break;
                }
            }
        }
    }
}
