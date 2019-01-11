using System;
using System.Collections.Generic;
using System.Text;

namespace Bartizan.ccLeadsWorking {
	[Serializable]
	public class Lead {
		public int					RecNo;
		public DateTime				Timestamp;
		public string				Notes;
		public BasicDataCollection	BasicData;
		public List<Demographic>	Demographics;
		public List<FollowUp>		FollowUps;
		public List<Session>		Sessions;
		public List<Survey>			Surveys;

		private static int LastRecNo = 0;

//---------------------------------------------------------------------------------------

		public Lead() {
			BasicData	 = new BasicDataCollection();
			Demographics = new List<Demographic>();
			FollowUps	 = new List<FollowUp>();
			Sessions	 = new List<Session>();
			Surveys		 = new List<Survey>();
		}

//---------------------------------------------------------------------------------------

		public Lead(string Notes, 
						BasicDataCollection BasicData,
						List<Demographic>	Demographics, 
						List<FollowUp>		FollowUps,
						List<Session>		Sessions,
						List<Survey>		Surveys) {
			this.Notes		  = Notes;
			this.BasicData	  = BasicData;
			this.Demographics = Demographics;
			this.FollowUps	  = FollowUps;
			this.Sessions	  = Sessions;
			this.Surveys	  = Surveys;
			this.RecNo		  = ++LastRecNo;
			this.Timestamp	  = DateTime.Now;
		}
	}
}
