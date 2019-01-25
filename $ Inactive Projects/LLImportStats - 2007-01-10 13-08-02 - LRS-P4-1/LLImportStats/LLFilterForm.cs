// Copyright (c) 2007 Bartizan Connects, LLC

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LLImportStats {
	public partial class LLFilterForm : Form {
		internal LLStatsFilterInfo	FilterInfo;

		SqlConnection	conn;

//---------------------------------------------------------------------------------------

		public LLFilterForm(LLStatsFilterInfo FilterInfo, SqlConnection conn) {
			InitializeComponent();

			this.conn = conn;

			this.FilterInfo = FilterInfo;

			dateTimePicker1.Value = FilterInfo.ImportStartDate;

			// The following two fields may be equal, in particular, the first time we're
			// entered, both will be blank. But in that case, the FillCompanies() routine
			// won't be called. So set a bool as to whether we need to call the routine.
			// Otherwise, it will be called by the txtCompanyName_TextChanged event
			// handler.
			bool	bMustFill = txtCompanyName.Text == FilterInfo.CompanyFilter;
			txtCompanyName.Text = FilterInfo.CompanyFilter;
			if (bMustFill) {
				FillCompanies();
			}
		}

//---------------------------------------------------------------------------------------

		private void FillCompanies() {
			string SQL = "SELECT DISTINCT CompanyName FROM tblCompanies";
			string	txtCompany = txtCompanyName.Text.Trim();
			if (txtCompany.Length > 0) {
				if (txtCompany[0] == '$')	// Turn off implicit %
					SQL += "\n\tWHERE (CompanyName LIKE '" + txtCompany.Substring(1) + "%')";
				else
					SQL += "\n\tWHERE (CompanyName LIKE '%" + txtCompany + "%')";
			}
			SQL += "\n\tORDER BY CompanyName";
			DataTable dtCompanies = new DataTable();
			SqlDataAdapter adapt = new SqlDataAdapter(SQL, conn);
			adapt.Fill(dtCompanies);
			lbCompanies.DataSource = dtCompanies;
			lbCompanies.DisplayMember = "CompanyName";
			lbCompanies.SelectedIndex = -1;		// Remove selection from first item
		}

//---------------------------------------------------------------------------------------

		private void nudLastNDays_ValueChanged(object sender, EventArgs e) {
			dateTimePicker1.Value = DateTime.Now - new TimeSpan((int)nudLastNDays.Value, 0, 0, 0);
			if (nudLastNDays.Value == 1) {
				lblDays.Text = "Day";
			} else {
				lblDays.Text = "Days";
			}
		}

//---------------------------------------------------------------------------------------

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e) {
			TimeSpan ts = DateTime.Now - dateTimePicker1.Value;
			nudLastNDays.Value = (decimal)ts.Days;
		}

//---------------------------------------------------------------------------------------

		private void txtCompanyName_TextChanged(object sender, EventArgs e) {
			FillCompanies();
		}

//---------------------------------------------------------------------------------------

		private void LLFilterForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult != DialogResult.OK) {
				return;
			}
			FilterInfo.CompanyFilter = txtCompanyName.Text.Trim();
			FilterInfo.ImportStartDate = dateTimePicker1.Value;

			FilterInfo.RCCompanies.Clear();
			// Here the semantics get a little complex.
			//	a)	When we initially fill the lbCompanies listbox, we set 
			//		SelectedIndexes = -1, so as not to select anything.
			//	b)	But if we don't select anything, we don't want to return the entire
			//		list as a filtering criterion.
			//	c)	Except that if we've used the txtCompanyName text box to narrow down
			//		our choices, we *do* want the whole list (if nothing's selected)

			// So our logic is this.
			//	1)	If there are selected items, return them in RCCompanies.
			//	2)	If txtCompanyName is empty, return an empty RCCompanies.
			//	3)	Else return the entire (filtered by txtCompanyName) list

			//	1)	If there are selected items, return them in RCCompanies.
			if (lbCompanies.SelectedIndex != -1) {
				foreach (DataRowView view in lbCompanies.SelectedItems) {
					FilterInfo.RCCompanies.Add((string)view["CompanyName"]);
				}
				return;
			}
			//	2)	If txtCompanyName is empty, return an empty RCCompanies.
			if (FilterInfo.CompanyFilter.Length == 0) {
				return;
			}
			//	3)	Else return the entire (filtered by txtCompanyName) list
				foreach (DataRowView view in lbCompanies.Items) {
					FilterInfo.RCCompanies.Add((string)view["CompanyName"]);
				}
				return;
		}
	}
}