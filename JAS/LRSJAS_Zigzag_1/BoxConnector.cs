using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LRSJAS_Zigzag_1 {
	public class BoxConnector {
		public Box		 FromBox, ToBox;
		public Box.Sides FromSide, ToSide;
		public float	 FromPct, ToPct;

		public static float Start         = 0.0f;
		public static float Quarter       = 0.25f;
		public static float Middle        = 0.50f;
		public static float ThreeQuarters = 0.75f;
		public static float End           = 1.0f;

//---------------------------------------------------------------------------------------

		public BoxConnector(Box BoxA, Box.Sides SideA, float PctA, Box BoxB, Box.Sides SideB, float PctB) {
			FromBox  = BoxA;
			ToBox    = BoxB;
			FromSide = SideA;
			ToSide   = SideB;
			FromPct  = PctA;
			ToPct    = PctB;
		}

		// TODO: Define a GetConnectorRect that contains the connector. Then use that to
		//		 just Invalidate that area, rather than Invalidating the entire window.

		// TODO: Or (just maybe), have a routine here that can draw and/or erase a
		//		 connector (being passed in a relevant Graphics object).
	}
}
