using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Bartizan.ccLeadsWorking {
	[Serializable]
	public class FollowUp {
		public string FollowUpName;

//---------------------------------------------------------------------------------------

		public FollowUp() {
			FollowUpName = "*N/A*";
		}

//---------------------------------------------------------------------------------------

		public FollowUp(string FollowUpName) {
			this.FollowUpName = FollowUpName;
		}
	}
}
