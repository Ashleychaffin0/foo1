using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestReport_March_11_2010 {
	public class WorkItem {
		public int ID { get; set; }
		public string Description { get; set; }
		public int Planned { get; set; }
		public int Actual { get; set; }

		public WorkItem(int id, string desc, int planned, int actual) {
			ID          = id;
			Description = desc;
			Planned     = planned;
			Actual      = actual;
		}
	}
}
