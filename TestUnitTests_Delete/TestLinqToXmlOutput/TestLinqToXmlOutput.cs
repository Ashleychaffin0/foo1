using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;

/*
 * This program shows how LINQ to XML can be used to create a .xml file from a 
 * semi-complex data structure in a surprisingly easy fashion.
 * 
 * Imagine you're the Westchester Library System. You have several branches (Getty 
 * Square, Scarsdale, etc) and each has a set of books. Each book has a name and a genre
 * (Mystery, SciFi, etc).
*/

namespace TestLinqToXmlOutput {

//---------------------------------------------------------------------------------------

		public enum BranchId {
			GettySquare,
			GrintonIWill,
			Greenburgh,
			Scarsdale
		}

//---------------------------------------------------------------------------------------

		public enum BookId {
			GoneWithTheWind,
			CalculusCanBeFun,
			JeevesAndWooster,
			MaryPoppins,
			TheLordOfTheRings,
			AdvancedCSharp
		}

//---------------------------------------------------------------------------------------

	public partial class TestLinqToXmlOutput : Form {

//---------------------------------------------------------------------------------------

		public TestLinqToXmlOutput() {
			InitializeComponent();

			var Inv = GetInventory();
			SaveInventory("foo.xml", Inv);
		}

//---------------------------------------------------------------------------------------

		static Dictionary<BranchId, List<BookInfo>>	GetInventory() {
			Dictionary<BranchId, List<BookInfo>> Inventory;
			Inventory = new Dictionary<BranchId, List<BookInfo>>();
			List<BookInfo> Schemae;
			foreach (var ID in Enum.GetValues(typeof(BranchId))) {
				Schemae = GetInventoryForEachBranch();
				Inventory[(BranchId)ID] = Schemae;
			}
			return Inventory;
		}

//---------------------------------------------------------------------------------------

		public static void SaveInventory(string Filename, Dictionary<BranchId, List<BookInfo>> Inv) {
			// This is way cool!
			XDocument xd = new XDocument(
				new XElement("LibraryBooks",
					from key in Inv.Keys
					select new XElement("Branch",
						new XAttribute("Name", key),
							from TypeAndId in Inv[key]
							select new XElement("Book", TypeAndId.Id,
										new XAttribute("Genre", TypeAndId.Genre)
					))));
			xd.Save(Filename);
			System.Diagnostics.Process.Start(Filename);
		}

//---------------------------------------------------------------------------------------

		private static List<BookInfo> GetInventoryForEachBranch() {
			List<BookInfo> Schemae = new List<BookInfo>();
			Type t = typeof(Mystery);
			// If we left the following code out, we'd have all books flagged as Mystery.
			// But to make the data look a little more real, we'll generate some random
			// Type's for each book.
			int seed = Environment.TickCount;
			Random rand = new Random(seed);
			System.Threading.Thread.Sleep(50);	// For a given BookId, this whole
						// program probably runs in less than a tick. Which means that
						// the next book would probably give the same results. So wait
						// a bit and (likely) get a new see each time.
			foreach (var ID in Enum.GetValues(typeof(BookId))) {
				switch (rand.Next(3)) {
				case 0:
					t = typeof(Mystery);
					break;
				case 1:
					t = typeof(SciFi);
					break;
				default:
					t = typeof(Fiction);
					break;
				}
				Schemae.Add(new BookInfo((BookId)ID, t));
			}
			return Schemae;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class BookInfo {
		public BookId	Id;
		public Type		Genre;

//---------------------------------------------------------------------------------------

		public BookInfo(BookId FldId, Type type) {
			this.Id		= FldId;
			this.Genre  = type;
		}
	}
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

public class Mystery { }

public class SciFi { }

public class Fiction { }
