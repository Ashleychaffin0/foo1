using System;
using System.Collections.Generic;

namespace TestEFFromDatabase.Models
{
    public partial class tblArticle
    {
        public int ArticleID { get; set; }
        public string ArticleName { get; set; }
        public Nullable<int> AuthorListID { get; set; }
        public int IssueID { get; set; }
        public virtual tblIssue tblIssue { get; set; }
    }
}
