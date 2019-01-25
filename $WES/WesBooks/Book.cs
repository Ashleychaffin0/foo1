using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsWesBooks {
    public class WesBook {
        public string   Title;
        public string   Author;
        public string   Genre;
        public string   ISBN;
        public string   Publisher;
        public DateTime DateLastRead;
        public string   Cover;              // Base 64 representation

//---------------------------------------------------------------------------------------

        public WesBook() {
            // Empty ctor needed by serialization
        }

//---------------------------------------------------------------------------------------

        public WesBook(
            string      Title,
            string      Author,
            string      Genre,
            string      ISBN,
            string      Publisher,
            DateTime    DateLastRead,
            string      Cover) {

            this.Title          = Title;
            this.Author         = Author;
            this.Genre          = Genre;
            this.ISBN           = ISBN;
            this.Publisher      = Publisher;
            this.DateLastRead   = DateLastRead;
            this.Cover          = Cover;
        }

//---------------------------------------------------------------------------------------

        public Image GetImage() {
            byte[] imageBytes = Convert.FromBase64String(Cover);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
}
