using System;
using System.Collections.Generic;

namespace TestEFFromDatabase.Models
{
    public partial class tblIssue
    {
        public tblIssue()
        {
            this.tblArticles = new List<tblArticle>();
        }

        public int IssueID { get; set; }
        public System.DateTime Issue_Date { get; set; }
        public string Cover_Description { get; set; }
        public virtual ICollection<tblArticle> tblArticles { get; set; }
    }
}
