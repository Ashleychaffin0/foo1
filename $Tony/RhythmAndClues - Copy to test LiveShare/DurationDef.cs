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

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	// Nothing new here. Just distinguish Tuplets from ordinary Durations
	class TupletDurationDef : DurationDef {
		public TupletDurationDef(string key, string name) : base(key, name) {
			
		}
	}
}
