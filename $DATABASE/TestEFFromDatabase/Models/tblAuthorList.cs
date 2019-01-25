using System;
using System.Collections.Generic;

namespace TestEFFromDatabase.Models
{
    public partial class tblAuthorList
    {
        public int AuthorListID { get; set; }
        public int AuthorID { get; set; }
        public int ArticleID { get; set; }
        public virtual tblAuthor tblAuthor { get; set; }
    }
}
