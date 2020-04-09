using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;

namespace WesBooks3 {
	class IsbnLookup {
		string	Title;
		string	AuthorFirstName;
		string  AuthorLastName;
		string	PublisherName;

		List<KeyValuePair<string, string>> OtherInfo;

//---------------------------------------------------------------------------------------

		public bool Read(string ISBN) {
			OtherInfo = new List<KeyValuePair<string, string>>();

			var client = new WebClient();

			string key = "RWHETRSL";
			// Uri uri = new Uri("http://isbndb.com/api/v2/xml/" + key + "/books?q=science&p=3 ");
			Uri uri = new Uri("http://isbndb.com/api/v2/xml/" + key + "/books?q=" + ISBN.Replace("-", ""));

			byte[] dbytes = client.DownloadData(uri);
			string resp = System.Text.Encoding.ASCII.GetString(dbytes);
			resp = resp.Replace("\n", "\r\n");


			var doc = new XmlDocument();
			doc.LoadXml(resp);

			XmlElement root = doc.DocumentElement;
			// XmlNodeList nodes = root.SelectSingleNode("isbn_db");   // You can also use XPath here
			XmlNode data = root.SelectSingleNode("data");	// You can also use XPath here
			if (data == null) {
				return false;
			}
			// nodes = root.ChildNodes;
			foreach (XmlNode node in data.ChildNodes) {
				switch (node.Name) {
				case "author_data":
					ProcessAuthorData(node);
					break;
				case "title":
					Title = node.InnerText;
					break;
				case "publisher_name":
					PublisherName = node.InnerText;
					break;
				// Elements we don't care about
				case "book_id":
				case "marc_enc_level":
					case "publisher_id":
					break;
				default:
					if ((node.FirstChild == null) || (node.FirstChild.Value == null)) {
						continue;
					}
					OtherInfo.Add(new KeyValuePair<string, string>(node.Name, node.FirstChild.Value));
					Console.WriteLine("Other Node = {0} / {1}", node.Name, node.FirstChild.Value);
					break;
				}


			}

			return true;
		}

//---------------------------------------------------------------------------------------

		private void ProcessAuthorData(XmlNode node) {
			// TODO: This routine needs more checking for cases that can lead to
			//		 subscripts out of range. For example, a FullName of "Saki".
			// Note: I've seen cases where the author's name was backwards. For example,
			//		 0-425-04934-5 is by Simon Brett, but ISBNdb has it as Brett Simon.
			// Note: Another bug for the same book. It's title is "Cast in Order of
			//		 Disappearance", but ISBNdb has it as merely "Cast in Order of".
			var NameNode    = node.SelectSingleNode("name");
			string FullName = NameNode.InnerText.Trim();
			int ixComma     = FullName.IndexOf(',');
			if (ixComma >= 0) {
				// Assume it's LastName, FirstName
				AuthorLastName  = FullName.Substring(0, ixComma).Trim();
				AuthorFirstName = FullName.Substring(ixComma + 1).Trim();
			} else {
				// Assume it's FirstName LastName, but be careful. The FirstName may be
				// multi-part, like H. G. Wells, or Edgar Rice Burroughs
				var names = FullName.Split(' ');
				// TODO: Put in checking for things like multiple internal blanks
				AuthorLastName = names[names.Length - 1].Trim();
				var sb = new StringBuilder();
				for (int i = 0; i < names.Length - 1; i++) {
					sb.Append(names[i]);
				}
				AuthorFirstName = sb.ToString();
			}
		}
	}
}
