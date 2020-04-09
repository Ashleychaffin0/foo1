using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WesBooks3 {
	public class WesBook {
		public string	Title;
		public string	Author;
		public string	Genre;
		public string	ISBN;
		public string	Publisher;
		public DateTime DateLastRead;
		public string	CoverLocation;          // Location of cover file (JPEG, etc.)
		public string	Series;
		public bool		bSeries;
		public bool		bAnthology;

//---------------------------------------------------------------------------------------

		public WesBook() {
			// Empty ctor needed by serialization
		}

//---------------------------------------------------------------------------------------

		public WesBook(
				string	 Title,
				string	 Author,
				string	 Genre,
				string	 ISBN,
				string	 Publisher,
				string	 Series,
				DateTime DateLastRead,
				string	 CoverLocation,
				bool	 bAnthology,
				bool	 bSeries) {
			this.Title         = Title;
			this.Author        = Author;
			this.Genre         = Genre;
			this.ISBN          = ISBN;
			this.Publisher     = Publisher;
			this.Series        = Series;
			this.DateLastRead  = DateLastRead;
			this.CoverLocation = CoverLocation;
			this.bAnthology    = bAnthology;
			this.bSeries       = bSeries;
		}

//---------------------------------------------------------------------------------------

		public Image GetImage() {
			byte[] imageBytes = Convert.FromBase64String(CoverLocation);
			MemoryStream ms   = new MemoryStream(imageBytes, 0, imageBytes.Length);
			// Convert byte[] to Image
			ms.Write(imageBytes, 0, imageBytes.Length);
			Image image = Image.FromStream(ms, true);
			return image;
		}
	}
}
