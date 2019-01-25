// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

using System.IO;
using System.Diagnostics;

namespace LRSS {

	/// <summary>
	/// Represents an RSS feed.
	/// </summary>
	public class RSSFeed {
		/// <summary>
		/// The URL of the feed. e.g. http://news.com.com/2547-1_3-0-5.xml
		/// </summary>
		public string			URL;
		/// <summary>
		/// The title of the feed. e.g. CNET News.com
		/// </summary>
		public string			Title;
		/// <summary>
		/// The description of the feed. e.g. Tech news and business reports by CNET 
		/// News.com. Focused on information technology, core topics include computers,
		/// hardware, software, networking, and Internet media.
		/// </summary>
		public string			Description;
		/// <summary>
		/// When the feed was last published. e.g. Mon, 21 Aug 2006 12:00:05 PDT
		/// </summary>
		public DateTime			PubDate;
		/// <summary>
		/// Whether the user has indicated that this feed should be brought to his/her
		/// attention because it often contains interesting material that you don't
		/// want to miss. For programmers, this might include, say, Charles Petzold or
		/// Anders Hejlsberg (yeah, like he blogs). News junkies might include CNN, 
		/// and so on. Note that favorites will be displayed in the ListView with a
		/// colored background.
		/// </summary>
		public bool				Favorite;

        [XmlIgnore]
		public int				FeedID;

		// Image data
		/// <summary>
		/// If there's a graphics image (e.g. a logo) associated with this feed,
		/// this contains its URL. e.g. 
		/// http://i.i.com.com/cnwk.1d/i/ne/gr/prtnr/rss_logo.gif
		/// </summary>
		public string			ImageURL;
		/// <summary>
		/// The width of the Image, or 0 if not specified.
		/// </summary>
		public int				Width;					// 0 if field not found
		/// <summary>
		/// The height of the Image, or 0 if not specified.
		/// </summary>
		public int				Height;					// 0 if field not found

		Hashtable				OtherNodes;

        [XmlIgnore]
		public bool				LoadedOK;

		[XmlIgnore]
		public int				cntItemsRead;			// Number of items read
		[XmlIgnore]				
		public int				cntItemsTotal;			// Total number of items

		// TODO: Add Event for progress report while loading?

//---------------------------------------------------------------------------------------

		public RSSFeed() {
			URL				= "";
			Title			= "(No Title)";
			Description		= "";
			PubDate			= default(DateTime);
			Favorite		= false;
			FeedID			= -1;
			ImageURL		= null;
			Width			= 0;
			Height			= 0;
			OtherNodes		= new Hashtable();
			LoadedOK		= false;
			cntItemsRead	= 0;
			cntItemsTotal	= 0;
		}

//---------------------------------------------------------------------------------------

		public RSSFeed(string Url)
				: this() {
			URL	= Url;
		}

//---------------------------------------------------------------------------------------

		public List<RSSItem> Load() {
			// Load the document
			List <RSSItem>	Items = new List <RSSItem>();
			string s = Load(URL, out LoadedOK);
			if (s == null) {
				LoadedOK = false;
				return Items;
			}
			XmlDocument	xdoc = new XmlDocument();
			try {
				xdoc.LoadXml(s);
				OtherNodes.Clear();

				// Get the basic elements
				// TODO: In principle, when we call this routine more than once, the
				//		 following can occur. Suppose we have, say, a Description from a
				//		 previous invocation of this method. But in between the feed
				//		 at the web site was modified, and took out the Description tag.
				//		 The code below won't find it (of course, it's not there), but
				//		 we'll leave the Description field populated. So we should 
				//		 (arguably) reset all the properties to their default values
				//		 (e.g. Title = "* No Title *"). Later.
				XmlNodeList nl = xdoc.SelectNodes("/rss/channel/*");
				XmlNode		subNode;
				bool		bOK;
				string		InnerText;

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
						// TODO: This needs a lot of work
						if (InnerText.EndsWith(" PST") || InnerText.EndsWith(" PDT")
							|| InnerText.EndsWith(" EST") || InnerText.EndsWith(" EDT"))
							InnerText = InnerText.Substring(0, InnerText.Length - 4);
						PubDate = DateTime.Parse(InnerText);
						break;
					case "ITEM":
						// Ignore it, and pick them up below
						break;
					case "IMAGE":
						subNode = GetChildNodeElement(node, "url", true);
						if (subNode != null) {
							ImageURL = subNode.InnerText;
						}
						subNode = GetChildNodeElement(node, "Width", true);
						if (subNode != null) {
							bOK = int.TryParse(subNode.InnerText, out Width);
							if ((!bOK) || (Width < 0)) {
								Width = 0;
							}
						}
						subNode = GetChildNodeElement(node, "Height", true);
						if (subNode != null) {
							bOK = int.TryParse(subNode.InnerText, out Height);
							if ((!bOK) || (Height < 0)) {
								Height = 0;
							}
						}

						break;
						// LastBuildDate?
					default:
						OtherNodes[node.Name] = InnerText;
						break;
					}
				}

				// Get the items
				XmlNodeList list = xdoc.SelectNodes("/rss/channel/item");
				// We may be reloading the feed. Empty the Items list. Otherwise, if an
				// item was there the last time we looked, but no longer is (i.e. it was
				// deleted) then we'd have it in the list. Which might not be a bad
				// thing, as long as we somehow marked that it was deleted. But we'll
				// worry about that later. Uh, another thing. If the feed comes in with
				// items, then every time we call this routine, we'll add a new set of
				// items, mostly/totally  duplicating the entries on our list!
				foreach (XmlNode node in list) {
					Items.Add(new RSSItem(node));
				}
				LoadedOK = true;
			} catch (Exception ex) {
				// TODO: Do something here
				Trace.WriteLine("RSSFeed.Load() exception on " + URL + " (" + ex.Message + ")");
				LoadedOK = false;
			} 
			return Items;
		}

//---------------------------------------------------------------------------------------

		private XmlNode GetChildNodeElement(XmlNode node, string ElementName, bool NameIsCaseInsensitive) {
			string	elemName = ElementName;
			if (NameIsCaseInsensitive) {
				elemName = elemName.ToUpper();
			}
			foreach (XmlNode SubNode in node.ChildNodes) {
				string	name = SubNode.Name;
				if (NameIsCaseInsensitive) {
					name = name.ToUpper();
				}
				if (name == elemName) {
					return SubNode;
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		public static string Load(string Url, out bool LoadedOK) {
			LoadedOK = true;
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
#if true
			} catch (Exception ex) {			// Ignore any problems, other than setting LoadedOK
#else
			} catch (Exception ex) {
				string	msg = "Unexpected error in RSSFeed.Load(\"{0}\") -- {1}";
				msg = string.Format(msg, Url, ex.Message);
				MessageBox.Show(msg, "LRSS Internal Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
				LoadedOK = false;
				Trace.WriteLine("RSSFeed.Load(URL) exception on " + Url + " (" + ex.Message + ")");
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
		/// <summary>
		/// The title for an item. e.g. Contest for Mac users' wildest (software) 
		/// fantasies
		/// </summary>
        public string	Title;
		/// <summary>
		/// A description of the item. e.g. Blog: Those fun little Mac shareware apps 
		/// are all over the Web these days, and it seems like they can do just about 
		/// anything that an...
		/// </summary>
        public string	Description;
		/// <summary>
		/// The link to the article. e.g. 
		/// http://news.com.com/2061-10793_3-6107815.html?part=rss&tag=6107815&subj=news
		/// </summary>
        public string	Link;
		/// <summary>
		/// When the item was written. e.g. Mon, 21 Aug 2006 11:41:00 PDT
		/// </summary>
        public DateTime PubDate;
		/// <summary>
		/// What with multiple time zones, mis-set clocks, and so on, the PubDate field
		/// isn't very useful for reading just the records you haven't yet checked out.
		/// So we add our own date field which represents time time (and date) of the 
		/// last pass this program made over the feeds.
		/// </summary>
		public DateTime	DownloadDate;
		/// <summary>
		/// A numeric value to indicate how interesting/relevant/etc this item is. A
		/// </summary>
		public short	Flag;

		// TODO: Implement these
		public string	ImageURL;
		public DateTime	LastBuildDate;

		public string	ItemBody;

		public bool		bRead;

		public int		FeedItemID;

        Hashtable OtherElements;

//---------------------------------------------------------------------------------------

        public RSSItem() {
            Title			= "";
            Description		= "";
            Link			= "";
            PubDate			= default(DateTime);
			DownloadDate	= default(DateTime);
			FeedItemID		= -1;
            OtherElements	= new Hashtable();
			bRead			= false;
        }

//---------------------------------------------------------------------------------------

        public RSSItem(XmlNode node) {
            OtherElements = new Hashtable();
            string	InnerText;
			bool	LoadedOK = true;
			DownloadDate = LRSS.GlobalDownloadDate;
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
					ItemBody = RSSFeed.Load(Link, out LoadedOK);
					if (! LoadedOK)
						ItemBody = "";
                    break;
                case "PUBDATE":
                    // TODO: Define routine to parse dates better. Also use below.
					if (InnerText.EndsWith(" PST") || InnerText.EndsWith(" PDT")
						|| InnerText.EndsWith(" EST") || InnerText.EndsWith(" EDT"))
                        InnerText = InnerText.Substring(0, InnerText.Length - 4);
                    PubDate = DateTime.Parse(InnerText);
                    break;
				case "BREAD":
					MessageBox.Show("In RSSItem ctor(XmlNode): bRead from XML looks like: " + InnerText);
					bRead = false;
					break;
                default:
                    OtherElements[elem.Name] = InnerText;
                    break;
                }
            }
        }

//---------------------------------------------------------------------------------------

		public RSSItem(DataRow dr) {
			Title		 = (string)		GetNonDBNull(dr["FeedItemTitle"], "");
			Description	 = (string)		GetNonDBNull(dr["FeedItemDescription"], "");
			PubDate		 = (DateTime)	GetNonDBNull(dr["FeedItemPubDate"], DateTime.Now);
			Link		 = (string)		GetNonDBNull(dr["Link"], "");
			DownloadDate = (DateTime)	dr["DownloadDate"];
			FeedItemID	 = (int)		dr["FeedItemID"];
			// ItemBody	 = (string)		dr["FeedItem"];
			ItemBody	 = "";			// TODO: Not sure if I want this.
			bRead		 = (bool)		GetNonDBNull(dr["bRead"], false);
			Flag		 = (short)		dr["Flag"];
		}

//---------------------------------------------------------------------------------------

		public static object GetNonDBNull(object o, object def) {
			if (o == DBNull.Value)
				return def;
			return o;
		}
    }
}
