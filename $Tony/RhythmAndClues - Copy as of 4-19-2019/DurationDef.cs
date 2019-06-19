using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class DurationDef {
		// Example -- ["1"] => 32, "Whole", false
		public int		Duration;
		public string	Name;
		public bool		IsDotted;	// I'm not(e) sure if we need this. Prolly delete.

//---------------------------------------------------------------------------------------

		public DurationDef(int Duration, string Name, bool IsDotted) {
			this.Duration = Duration;
			this.Name     = Name;
			this.IsDotted = IsDotted;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return Name;
		}
	}
}
