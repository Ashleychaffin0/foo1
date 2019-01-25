using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst_Test_1 {
	class Issue {
		public int				IssueID  { get; set; }
		public int				Year	 { get; set; }
		public int				Month	 { get; set; }
		public string			Title	 { get; set; }
		public List<Article>	Articles { get; set; }
		public int				Genre	 { get; set; }				// TODO: Enum?

		public Issue() {
			Articles = new List<Article>();
		}

		// TODO: ctor
	}
}
