using System;
using System.Windows.Forms;

namespace SciAmToc {
	public partial class SciAmToc {

//---------------------------------------------------------------------------------------

		private void CmbDecade_SelectedIndexChanged(object sender, EventArgs e) {
			int decade = (int)(sender as ComboBox).SelectedItem;
			if (sender == CmbFirstDecade) {
				CmbFirstYear.Items.Clear();
				for (int i = 0; i < 10; i++) {
					CmbFirstYear.Items.Add(decade + i);
				}
				CmbFirstYear.SelectedIndex = 0;

				if (CmbFirstMonth.Items.Count == 0) {
					CmbFirstMonth.Items.AddRange(Months);
				}
				CmbFirstMonth.SelectedIndex = 0;
				CmbMonth_SelectedIndexChanged(CmbFirstMonth, null);

				CmbLastDecade.SelectedIndex = CmbFirstDecade.SelectedIndex;
			} else {
				CmbLastYear.Items.Clear();
				for (int i = 0; i < 10; i++) {
					CmbLastYear.Items.Add(decade + i);
				}
				CmbLastYear.SelectedIndex = 9;

				if (CmbLastMonth.Items.Count == 0) {
					CmbLastMonth.Items.AddRange(Months);
				}
				CmbLastMonth.SelectedIndex = 11;
				CmbMonth_SelectedIndexChanged(CmbLastMonth, null);
			}
		}

//---------------------------------------------------------------------------------------

		private void CmbMonth_SelectedIndexChanged(object sender, EventArgs e) {
			int Year, Month;
			if (sender == CmbFirstMonth) {
				Year = (int)CmbFirstYear.SelectedItem;
				Month = (int)CmbFirstMonth.SelectedIndex + 1;
				// LblFirstTitle.Text = GetIssueTitle(Year, Month);
			} else {		// Last month
				Year = (int)CmbLastYear.SelectedItem;
				Month = (int)CmbLastMonth.SelectedIndex + 1;

			}
		}
	}
}
