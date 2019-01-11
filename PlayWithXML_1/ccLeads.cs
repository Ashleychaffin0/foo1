using System;
using System.Collections.Generic;
using System.Text;

namespace Bartizan.ccLeadsWorking {

	[Serializable]
	public class ccLeads {
		public List<Lead> Leads;

//---------------------------------------------------------------------------------------

		public ccLeads() {
			Leads = new List<Lead>();
		}
	}
}
