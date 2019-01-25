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
		LLStatsFilterInfo	FilterInfo;

		SqlConnection		conn;

		//	Filtering on the Companies text box will (in general) modify the Events
		//		selected.
		//	Filtering on the Events    text box will (in general) modify the Companies
		//		selected.
		//	Hands up everyone who sees the potential for an infinite loop.
		//
		//	So we'll define a flag that will help us avoid such a loop. It will be
		//	checked in the _TextChanged events for both text boxes and will make them
		//	ignore changes when necessary.
		bool				bFilling;

//---------------------------------------------------------------------------------------

		public LLFilterForm(LLStatsFilterInfo FilterInfo, SqlConnection conn) {
			InitializeComponent();

			this.conn = conn;

			this.FilterInfo = FilterInfo;

			dateTimePicker1.Value = FilterInfo.ImportStartDate;

			// Here in the ctor, we set txtCompany.Text to FilterInfo.CompanyName, so 
			// that we remember the last filtering option used. This assignment will
			// trigger the textbox's Changed event, and we'll refill the company 
			// listbox at that point. However, if we ever enter with an empty
			// FilterInfo.CompanyName (and we're guaranteed to, at least the first time),
			// then the assignment won't change anything, and the listbox won't be 
			// filled. So handle this case specially.

			bFilling = true;	// Turn off AutoFilter
			txtCompanyName.Text = FilterInfo.CompanyName;
			txtEventName.Text	= FilterInfo.EventName;

#if true
			FillCompanies();
			FillEvents();
			bFilling = false;
#else		
			FillCompaniesAndEvents(true);	// Will turn off AutoFilter
#endif
		}

//---------------------------------------------------------------------------------------

		private void FillCompanies() {
			string SQL = @"
SELECT DISTINCT CompanyName
FROM	tblCompanies INNER JOIN
        tblAccountsExtended ON tblCompanies.CompanyID = tblAccountsExtended.CompanyID INNER JOIN
        tblAccounts ON tblAccountsExtended.AcctID = tblAccounts.AcctID INNER JOIN
        tblEvents ON tblAccounts.AcctID = tblEvents.EventRCID
";
			string	CompanyName = txtCompanyName.Text.Trim();
			if ((CompanyName.Length > 0) && (CompanyName[0] == '$'))	// Turn off implicit %
				SQL += "\n\tWHERE (CompanyName LIKE '" + CompanyName.Substring(1) + "%')";
			else
				SQL += "\n\tWHERE (CompanyName LIKE '%" + CompanyName + "%')";
			string	EventClause = InEventClause();
			if (EventClause != null) {
				SQL += "\n\t  AND EventName " + EventClause;		
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

		private void FillEvents() {
#if false
		SELECT tblEvents.EventName
		WHERE (tblCompanies.CompanyName = 'conv')	
#endif
			string	SQL = @"
SELECT DISTINCT EventName
FROM	tblCompanies INNER JOIN
        tblAccountsExtended ON tblCompanies.CompanyID = tblAccountsExtended.CompanyID INNER JOIN
        tblAccounts ON tblAccountsExtended.AcctID = tblAccounts.AcctID INNER JOIN
        tblEvents ON tblAccounts.AcctID = tblEvents.EventRCID
";	// non-DISTINCT
			string	EventName = txtEventName.Text.Trim();
			if ((EventName.Length > 0) && (EventName[0] == '$'))	// Turn off implicit %
				SQL += "\n\tWHERE (EventName LIKE '" + EventName.Substring(1) + "%')";
			else
				SQL += "\n\tWHERE (EventName LIKE '%" + EventName + "%')";
			string CompanyClause = InCompanyClause();
			if (CompanyClause!= null) {
				SQL += "\n\t  AND CompanyName " + CompanyClause;
			}
			SQL += "\n\tORDER BY EventName";

			DataTable dtEvents = new DataTable();
			SqlDataAdapter adapt = new SqlDataAdapter(SQL, conn);
			adapt.Fill(dtEvents);
			lbEvents.DataSource = dtEvents;
			lbEvents.DisplayMember = "EventName";
			lbEvents.SelectedIndex = -1;		// Remove selection from first item
		}

//---------------------------------------------------------------------------------------

		private void FillCompaniesAndEvents(bool bFillCompaniesFirst) {
			bFilling = true;
			try {
				if (bFillCompaniesFirst) {
					// ((DataTable)lbEvents.DataSource).Rows.Clear();
					// lbEvents.Items.Clear();
					FillCompanies();
					// FillEvents();
				} else {
					// ((DataTable)lbCompanies.DataSource).Rows.Clear();
					// lbCompanies.Items.Clear();
					FillEvents();
					// FillCompanies();
				}
			} finally {
				bFilling = false;
			}
		}

//---------------------------------------------------------------------------------------

		private string InCompanyClause() {
			// Returns the IN ('abc', 'def') clause, or the empty string
			StringBuilder	sb = new StringBuilder();
			sb.Append("IN (");
			string			comma = "";
			if (lbCompanies.SelectedItems.Count > 0) {
				// With or without filtering, but with one or more selected
				foreach (DataRowView row in lbCompanies.SelectedItems) {
					string	CompanyName = (string)row["CompanyName"];
					CompanyName = CompanyName.Replace("'", "''");
					sb.AppendFormat("{0}'{1}'", comma, CompanyName);
					comma = ", ";
				}
				sb.Append(")");
				return sb.ToString();
			}
			if (IsEmptyFilterText(txtCompanyName.Text)) {
				// No filtering applied. Return null.
				return null;
			}	
			// Filtering, but no matches. Return null.
			if (lbCompanies.Items.Count == 0) {
				return null;
			}
			// Filtering, but no selections made. Return the filtered list.
			// Note: Essentially the same code as above
			foreach (DataRowView row in lbCompanies.Items) {
				string CompanyName = (string)row["CompanyName"];
				CompanyName = CompanyName.Replace("'", "''");
				sb.AppendFormat("{0}'{1}'", comma, CompanyName);
				comma = ", ";
			}
			sb.Append(")");
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		private string InEventClause() {
			StringBuilder	sb = new StringBuilder();
			sb.Append("IN (");
			string			comma = "";
			if (lbEvents.SelectedItems.Count > 0) {
				// With or without filtering, but with one or more selected
				foreach (DataRowView row in lbEvents.SelectedItems) {
					string EventName = (string)row["EventName"];
					EventName = EventName.Replace("'", "''");
					sb.AppendFormat("{0}'{1}'", comma, EventName);
					comma = ", ";
				}
				sb.Append(")");
				return sb.ToString();
			}
			if (IsEmptyFilterText(txtEventName.Text)) {
				// No filtering applied. Return null.
				return null;
			}
			// Filtering, but no matches. Return null.
			if (lbEvents.Items.Count == 0) {
				return null;
			}
			// Filtering, but no selections made. Return the filtered list.
			// Note: Essentially the same code as above
			foreach (DataRowView row in lbEvents.Items) {
				string EventName = (string)row["EventName"];
				EventName = EventName.Replace("'", "''");
				sb.AppendFormat("{0}'{1}'", comma, EventName);
				comma = ", ";
			}
			sb.Append(")");
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		private bool IsEmptyFilterText(string text) {
			// An empty filter is one that either has length 0, or consists of a single
			// "$".
			text = text.Trim();
			if (text.Length == 0)
				return true;
			if (text == "$") {
				return true;
			}
			return false;
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
			if (bFilling) {
				return;
			}
			FillCompaniesAndEvents(true);
		}

//---------------------------------------------------------------------------------------

		private void txtEventName_TextChanged(object sender, EventArgs e) {
			if (bFilling) {
				return;
			}
			FillCompaniesAndEvents(false);
		}

//---------------------------------------------------------------------------------------

		private void LLFilterForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult != DialogResult.OK) {
				return;
			}
			FilterInfo.CompanyName = txtCompanyName.Text.Trim();
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
			if (FilterInfo.CompanyName.Length == 0) {
				return;
			}
			//	3)	Else return the entire (filtered by txtCompanyName) list
			foreach (DataRowView view in lbCompanies.Items) {
				FilterInfo.RCCompanies.Add((string)view["CompanyName"]);
			}

			// Event processing -- essentially the same as Companies processing above

			//	1)	If there are selected items, return them in RCCompanies.
			if (lbEvents.SelectedIndex != -1) {
				foreach (DataRowView view in lbEvents.SelectedItems) {
					FilterInfo.EventNames.Add((string)view["EventName"]);
				}
				return;
			}
			//	2)	If txtCompanyName is empty, return an empty RCCompanies.
			if (FilterInfo.EventName.Length == 0) {
				return;
			}
			//	3)	Else return the entire (filtered by txtCompanyName) list
			foreach (DataRowView view in lbEvents.Items) {
				FilterInfo.EventNames.Add((string)view["EventName"]);
			}

			return;
		}

//---------------------------------------------------------------------------------------

		private void lbCompanies_SelectedIndexChanged(object sender, EventArgs e) {
			if (bFilling) {
				return;
			}
			FillCompaniesAndEvents(false);
		}

//---------------------------------------------------------------------------------------

		private void lbEvents_SelectedIndexChanged(object sender, EventArgs e) {
			if (bFilling) {
				return;
			}
			FillCompaniesAndEvents(true);
		}

//---------------------------------------------------------------------------------------

		private void btnClearSelections_Click(object sender, EventArgs e) {
			lbCompanies.SelectedIndex = -1;
			lbEvents.SelectedIndex	  = -1;
			FillCompaniesAndEvents(false);
		}
	}
}