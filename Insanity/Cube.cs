using System.Collections.Generic;

namespace Insanity {
	class Cube {
		string[] Colors;

		// Sample data:
		// In this order: Top, Bottom, Left, Right, Front, Back
		//	{"SPSR", "RSSS", "RPSS", "PSSP", "SPRP", "SSSS"},

		enum Faces {
			Top,
			Bottom,
			Left,
			Right,
			Front,
			Back
		}

//---------------------------------------------------------------------------------------

		public Cube(string t, string b, string l, string r, string f, string x) {
			Colors = new string[] { t, b, l, r, f, x };
		}

		// TODO: Add Twist support

//---------------------------------------------------------------------------------------

		// 90 degree rotations clockwise
		public IEnumerable<Cube> Rotations() {
			var cube = this;
			yield return this;
			for (int i = 1; i < 4; i++) {
				cube = cube.RotateRight();
				yield return cube;
			}
		}

//---------------------------------------------------------------------------------------

		// Creates a new Cube by rotating the given cube 90 degrees to the right
		public Cube RotateRight() {
			// Rotating a cube does the following:
			//	*	The Top layer remains the top layer, with the colors cycled
			//	*	The Bottom layer -- likewise
			//	*	Front->Left, Right->Front, Back->Right, Left->Back
			string t = Rot(Top);
			string b = Rot(Bottom);
			// tblrfx
			return new Cube(t, b, Front, Back, Right, Left);
		}

		// Creates a new Cube by rotating the current cube  90 degrees upwards
		public Cube RotateUp() {
			// Rotating a cube upwards does the following:
			//	*	Left/Right are rotated right
			//	*	Top->Back, Front->Top, Bottom->Front, Back->Bottom
			string l = Rot(Left);
			string r = Rot(Right);
			return new Cube(Front, Back, l, r, Bottom, Top);
		}

//---------------------------------------------------------------------------------------

		static string Rot(string s) => s.Substring(1) + s[0];    // ABCD -> BCDA

//---------------------------------------------------------------------------------------

		public string Top	 => Colors[(int)Faces.Top];
		public string Bottom => Colors[(int)Faces.Bottom];
		public string Left	 => Colors[(int)Faces.Left];
		public string Right	 => Colors[(int)Faces.Right];
		public string Front	 => Colors[(int)Faces.Front];
		public string Back	 => Colors[(int)Faces.Back];
	}
}
