using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using System.Text;

using System.IO;

namespace LRSS {
	public class RSSItem {
		public string	Title;
		public string	Description;
		public string	Link;
		public DateTime pubDate;

		Hashtable		OtherElements;

//---------------------------------------------------------------------------------------

		public RSSItem() {
			Title		  = "";
			Description   = "";
			Link		  = "";
			pubDate		  = default(DateTime);
			OtherElements = new Hashtable();
		}

//---------------------------------------------------------------------------------------

		public RSSItem(XmlNode node) {
			OtherElements = new Hashtable();
			string		InnerText;
			foreach (XmlElement elem in node.ChildNodes) {
				InnerText = elem.InnerText;
				switch (elem.Name.ToLower()) {
				case "title":
					Title = InnerText;
					break;
				case "description":
					Description = InnerText;
					break;
				case "link":
					Link = InnerText;
					break;
				case "pubdate":
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



//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class RSSFeed {
		public string			URL;
		public string			Title;
		public string			Description;
		public DateTime			PubDate;

		public List<RSSItem>	Items;

		Hashtable				OtherNodes;

		public bool				LoadedOK;

		// TODO: Add Event for progress report while loading?

//---------------------------------------------------------------------------------------

		public RSSFeed() {
			URL			= "";
			Title		= "";
			Description = "";
			PubDate		= default(DateTime);
			Items		= new List<RSSItem>();
			OtherNodes	= new Hashtable();
			LoadedOK	= false;
		}

//---------------------------------------------------------------------------------------

		public RSSFeed(string Url) {
			URL			= Url;
			Title		= "";
			Description = "";
			OtherNodes	= new Hashtable();
			Items		= new List<RSSItem>();
			LoadedOK	= true;

			// Load the document
			// TODO: Try/catch + set LoadedOK
			string s = Load(Url);
			XmlDocument	xdoc = new XmlDocument();
			xdoc.LoadXml(s);

			// Get the basic elements
			XmlNodeList		nl = xdoc.SelectNodes("/rss/channel/*");
			string			InnerText;
			foreach (XmlNode node in nl) {
				InnerText = node.InnerText;
				switch (node.Name.ToLower()) {
				case "title":
					Title = InnerText;
					break;
				case "description":
					Description = InnerText;
					break;
				case "pubdate":
					if (InnerText.EndsWith(" PST"))
						InnerText = InnerText.Substring(0, InnerText.Length - 4);
					PubDate = DateTime.Parse(InnerText);
					break;
				case "item":
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
		}

//---------------------------------------------------------------------------------------

		private string Load(string Url) {
			LoadedOK = true;
			// TODO: Note: This should be done on a worker thread.
			if (Url.ToLower().StartsWith("http://") || (Url.ToLower().StartsWith("https://")))
				;	// Do nothing
			else
				Url = "http://" + Url;
			WebRequest	wreq = WebRequest.Create(Url);
			WebResponse wresp = wreq.GetResponse();
			// Display the status.
			// Console.WriteLine(wresp.StatusDescription);
			// Get the stream containing content returned by the server.
			Stream dataStream = wresp.GetResponseStream();
			// Open the stream using a StreamReader for easy access.
			StreamReader reader = new StreamReader(dataStream);
			// Read the content.
			string responseFromServer = reader.ReadToEnd();	// TODO: Add event support here
			// Cleanup the streams and the response.
			reader.Close();
			dataStream.Close();
			wresp.Close();
			return responseFromServer;
		}
	}
}
