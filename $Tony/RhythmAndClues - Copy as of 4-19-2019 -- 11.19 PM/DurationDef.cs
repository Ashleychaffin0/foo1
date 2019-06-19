using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class DurationDef {
		// Example -- ["1"] => 32, "Whole", false
		public double	Duration;
		public string	Name;
		public bool		IsDotted;

//---------------------------------------------------------------------------------------

		public DurationDef(string key, string name) {
			Name = name;
			double dur = double.Parse(key);
			if (key.EndsWith('.')) {
				IsDotted = true;
				dur /= 1.5;
			}
			Duration = 4 / dur;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return Name;
		}
	}
}
