using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst_Test_1 {
	class Article {
		public int			ArticleID   { get; set; }
		public int			IssueID     { get; set; }
		public string		Name        { get; set; }
		public string		Description { get; set; }                     
		public List<Author>	Authors		{ get; set; }
		public Genre		Genre		{ get; set; }

//---------------------------------------------------------------------------------------

		public Article() {
			// Empty
		}
	}
}
