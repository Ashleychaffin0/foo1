using System;
using System.Collections.Generic;

namespace TestEFFromDatabase.Models
{
    public partial class tblAuthor
    {
        public tblAuthor()
        {
            this.tblAuthorLists = new List<tblAuthorList>();
        }

        public int AuthorID { get; set; }
        public string Author_Name { get; set; }
        public virtual ICollection<tblAuthorList> tblAuthorLists { get; set; }
    }
}
