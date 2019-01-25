using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

// Note: The data in this database is hardly consistent in its formatting.
//		 For example, a book title might be ALL IN CAPITALS. So we may well
//		 want to add some utility routines (invokable from the UI?) to
//		 normalize some of the data.

// Note: If you search by Title, there are many books with the same title. For example,
//		 Glory Road. We probably need to return a List<> of books (ideally with only one
//		 item in it) and have something on the UI to let a user go to the next/previous
//		 book.

// Note: Sometimes the web service returns a 500 Internal Server Error. We'd probably
//		 need to check for that and retry some number of times.

// Note: Things like genre and publisher seem to vary all over the map. So we might want
//		 a database mapping table or three.

// Note: We can get multiple <subject_ids> for a given book.

namespace WesIsbnDemo {
	class IsbnLookup {
		public string Title;
		public string AuthorFirstName;
		public string AuthorLastName;
		public string PublisherName;

		public List<KeyValuePair<string, string>> OtherInfo;

//---------------------------------------------------------------------------------------

		public bool Read(string ISBN_or_Title) {
			OtherInfo = new List<KeyValuePair<string, string>>();

			System.Net.WebClient client = new System.Net.WebClient();

			string key = "RWHETRSL";
			// Uri uri = new Uri("http://isbndb.com/api/v2/xml/" + key + "/books?q=science&p=3 ");
			Uri uri = new Uri("http://isbndb.com/api/v2/xml/" + key + "/books?q=" + ISBN_or_Title.Replace("-", ""));

			byte[] dbytes = client.DownloadData(uri);
			string resp = System.Text.Encoding.ASCII.GetString(dbytes);
			resp = resp.Replace("\n", "\r\n");


			var doc = new XmlDocument();
			doc.LoadXml(resp);

			XmlElement root = doc.DocumentElement;
			XmlNode data = root.SelectSingleNode("data");	// You can also use XPath here
			if (data == null) {
				return false;
			}
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
					// Ignore
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
			var NameNode = node.SelectSingleNode("name");
			string FullName = NameNode.InnerText.Trim();
			int ixComma = FullName.IndexOf(',');
			if (ixComma >= 0) {
				// Assume it's LastName, FirstName
				AuthorLastName = FullName.Substring(0, ixComma).Trim();
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
