using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciAmToc {
	class DateRange {
		int CurYear;
		int CurMonth;
		int EndYear;
		int EndMonth;

//---------------------------------------------------------------------------------------

		public DateRange(int StartYear, int StartMonth, int EndYear, int EndMonth) {
			this.CurYear  = StartYear;
			this.CurMonth = StartMonth;
			this.EndYear  = EndYear;
			this.EndMonth = EndMonth;
		}

//---------------------------------------------------------------------------------------

		public DateRange(string StartYear, int StartMonth, string EndYear, int EndMonth) {
			this.CurYear  = Convert.ToInt32(StartYear);
			this.CurMonth = StartMonth;
			this.EndYear  = Convert.ToInt32(EndYear);
			this.EndMonth = EndMonth;
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<(int Year, int Month)> Dates() {
			while ((EndYear > CurYear) || ((CurYear == EndYear) && (CurMonth <= EndMonth))) {
				int Year  = CurYear;
				int Month = CurMonth;
				yield return (Year, Month);
				if (++CurMonth > 12) {
					CurMonth = 1;
					++CurYear;
				}
			}
		}
	}
}
