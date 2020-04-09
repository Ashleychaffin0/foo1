using System;
using System.Collections.Generic;
using System.Text;

namespace Bartizan {
	public class BalanceLine<T> {

		public static void Balance(IEnumerable<T> left, IEnumerable<T> right,
			Func<T, T, int> Comp,				// Comparison routine
			Action<T, bool, T, bool> Act) {		// What to do each time
			
			// TODO: Do I need 
			//				using (var LeftEnum = left.GetEnumerator()) {
			//		 and similarly for RightEnum?

			var LeftEnum 	 = left.GetEnumerator();
			bool bIsMoreLeft = LeftEnum.MoveNext();

			var RightEnum 	  = right.GetEnumerator();
			bool bIsMoreRight = RightEnum.MoveNext();

			bool bIsMoreBoth = bIsMoreLeft && bIsMoreRight;

			while (bIsMoreBoth) {
				int cmp = Comp(LeftEnum.Current, RightEnum.Current);
				if (cmp == 0) {			// Check most likely case first
					Act(LeftEnum.Current, true, RightEnum.Current, true);
					bIsMoreLeft = LeftEnum.MoveNext();
					bIsMoreRight = RightEnum.MoveNext();
				}  else if (cmp < 0) {
					Act(LeftEnum.Current, true, RightEnum.Current, false);
					bIsMoreLeft = LeftEnum.MoveNext();
				} else {
					Act(LeftEnum.Current, false, RightEnum.Current, true);
					bIsMoreRight = RightEnum.MoveNext();
				}
				bIsMoreBoth = bIsMoreLeft && bIsMoreRight;
			}

			// Now drain each
			while (bIsMoreLeft) {
				Act(LeftEnum.Current, true, default(T), false);
				bIsMoreLeft = LeftEnum.MoveNext();
			}

			while (bIsMoreRight) {
				Act(default(T), false, RightEnum.Current, true);
				bIsMoreRight = RightEnum.MoveNext();
			}
		}
	}
}
