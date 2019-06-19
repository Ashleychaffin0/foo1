using System.Collections.Generic;

namespace RhythmAndClues {
	class DurationList {
		public bool				 IsTuplet;
		public List<DurationDef> Durations;

//---------------------------------------------------------------------------------------

		public DurationList(bool IsTuplet = false) {
			this.IsTuplet = IsTuplet;
			Durations     = new List<DurationDef>();
		}

//---------------------------------------------------------------------------------------

		public void Add(DurationDef dur) {
			Durations.Add(dur);
		}

//---------------------------------------------------------------------------------------

		public int Count => Durations.Count;
	}
}
