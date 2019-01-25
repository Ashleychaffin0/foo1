// Copyright (c) 2007 Bartizan Connects, LLC

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// TODO:
//	*	Return from non-cancelled Filter form needs to rerun current form
//	*	Event/Company filtering - do it and return values
//	*	Show events just in the specified from/to time frame
//	*	Printing might be nice
//	*	Ability to select columns, especially on ImportStats
//	*	Persist filters, columns, etc

namespace LLImportStats {

	delegate void	delCurrentReport();

	public partial class LLImportStats : Form {
		string				FormOriginalCaption;

		LLStatsFilterInfo	StatsFilter;

		delCurrentReport	ShowCurrentReport;

//---------------------------------------------------------------------------------------

		public LLImportStats() {
			InitializeComponent();

			FormOriginalCaption = this.Text;

			ShowCurrentReport = new delCurrentReport(ShowEventSummary);

			StatsFilter = new LLStatsFilterInfo();

			cbSystem.Text = "DBMart";
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			ShowImportStats();
		}

//---------------------------------------------------------------------------------------

		private void ShowEventSummary() {
			string SQL = "WITH Details AS (" + GetDetailsSQL() + ")\n"
						+ "SELECT [RC Name], SUM([Swipe Count]) AS [Total Swipes]"
						+ "\n\t, COUNT(TermID) AS [Term Count]"
						+ "\n\t, [Event Name], [Event Start Date], [Event End Date], City, State"
						+ "\n\tFROM Details"
						+ "\n\tGROUP BY [RC Name], [Event Start Date], [Event Name], [Event End Date], City, State"
						+ "\n\tORDER BY [RC Name], [Event Start Date] DESC, [Event Name]";

			string strConn = GetConnectionString();
			using (SqlConnection conn = new SqlConnection(strConn)) {
				DataTable summary = new DataTable("Summary");
				SqlDataAdapter adapt = new SqlDataAdapter(SQL, conn);
				adapt.Fill(summary);
				dgvSummary.DataSource = summary;
				SetFormCaption(summary);
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowEventDetails() {
			string SQL = GetDetailsSQL();
			SQL += "\n\tORDER BY [RC Name], [Event Start Date] DESC, [Event Name], TermID";

			string strConn = GetConnectionString();
			using (SqlConnection conn = new SqlConnection(strConn)) {
				DataTable details = new DataTable("Details");
				SqlDataAdapter adapt = new SqlDataAdapter(SQL, conn);
				adapt.Fill(details);
				dgvDetails.DataSource = details;
				SetFormCaption(details);
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowImportStats() {
			string strConn = GetConnectionString();
			using (SqlConnection conn = new SqlConnection(strConn)) {
				conn.Open();
				int		rows = FillImportStatsGrid(conn);
				SetFormCaption(rows);
			}
		}

//---------------------------------------------------------------------------------------

		private int FillImportStatsGrid(SqlConnection conn) {
			string SQL = @"
SELECT	CAST(tblSavedImports.RecordCount / (tblSavedImports.MillisecondsToImport / 1000.0) AS int) AS SPS, 
		tblSavedImports.RecordCount AS Swipes, 
	    tblSavedImports.WhenImported AS [When Imported],
        tblAccounts.FirstName AS [First Name], tblAccounts.LastName AS [Last Name], 
		tblAccounts.Activated,
		tblEvents.EventName AS [Event Name], tblEvents.EventStartDate AS [Event Start Date], 
		tblEvents.EventEndDate AS [Event End Date], tblEvents.EventCity AS [Event City],
        tblSavedImports.TerminalSerial AS [Terminal Serial],
		tblAccounts.emailAddress AS email, 
		tblAccts.FirstName AS [RC FName], tblAccts.LastName AS [RC LName], tblAccts.AcctID as [RC AcctID],
		tblCompanies.CompanyName AS [RC Company Name],
        tblSavedImports.TallTableInsertions AS TtIns, tblSavedImports.ResponseInsertions AS RspIns,
	    tblAccounts.UserID, 
		tblSavedImports.AcctID, tblSavedImports.EventID, tblSavedImports.MapCfgID,
		tblSavedImports.MillisecondsToImport AS ms,  
	    tblSavedImports.BulkFallbacks_TallTable AS FB_TT, tblSavedImports.BulkFallbacks_Responses AS FB_Rsp,
		tblSavedImports.SavedImportID
FROM  tblSavedImports 
		INNER JOIN tblAccounts 
			ON tblSavedImports.AcctID = tblAccounts.AcctID 
		INNER JOIN tblEvents 
			ON tblSavedImports.EventID = tblEvents.EventID 
		INNER JOIN tblAccountsExtended
			ON tblAccounts.Creator = tblAccountsExtended.AcctID
		INNER JOIN tblCompanies
			ON tblAccountsExtended.CompanyID = tblCompanies.CompanyID
		INNER JOIN tblAccounts AS tblAccts 
			ON tblAccts.AcctID = tblAccounts.Creator";

			string	where = GetWhere();
			SQL += where;
			SQL += "\nORDER BY tblSavedImports.WhenImported DESC";
			// Console.WriteLine(where);		// TODO:
			//			tblAccounts ON tblAccountsExtended.AcctID = tblAccounts.AcctID INNER JOIN

			DataTable	stats = new DataTable("Stats");
			SqlDataAdapter	adapt = new SqlDataAdapter(SQL, conn);
			adapt.Fill(stats);
			foreach (DataRow row in stats.Rows) {
				DateTime	dt = (DateTime)row["When Imported"];
				dt = dt.ToLocalTime();
				row["When Imported"] = dt;
			}
			dgvStats.DataSource = stats;
			return stats.Rows.Count;
		}

//---------------------------------------------------------------------------------------

		private string GetWhere() {
			string	where = "\n\tWHERE";
			where += string.Format("\t\t(tblSavedImports.MillisecondsToImport > 0)"
				  + "\n\t  AND (tblSavedImports.WhenImported >= '{0}')",
				StatsFilter.ImportStartDate);

			if (StatsFilter.RCCompanies.Count > 0) {
				where += "\n\t  AND ";
				where += GetCompanyNameFilter();
			}
			return where;
		}

//---------------------------------------------------------------------------------------

		private string GetCompanyNameFilter() {
			if (StatsFilter.RCCompanies.Count == 0) {
				return "";
			}
			string CompanyNamesFilter = "tblCompanies.CompanyName IN (";
			for (int i = StatsFilter.RCCompanies.Count - 1; i >= 0; i--) {
				// TODO: Should replace single quotes in EventNames with ''.
				CompanyNamesFilter += string.Format("\n\t\t'{0}'{1}",
					StatsFilter.RCCompanies[i], i == 0 ? "" : ", ");
			}
			CompanyNamesFilter +=  ")";
			return CompanyNamesFilter;
		}

//---------------------------------------------------------------------------------------

		private string GetEventNameFilter() {
			if (StatsFilter.EventNames.Count == 0) {
				return "";
			}
			string EventNamesFilter = "tblCompanies.CompanyName IN (";
			for (int i = StatsFilter.EventNames.Count - 1; i >= 0; i--) {
				// TODO: Should replace single quotes in EventNames with ''.
				EventNamesFilter += string.Format("\n\t\t'{0}'{1}",
					StatsFilter.EventNames[i], i == 0 ? "" : ", ");
			}
			EventNamesFilter +=  ")";
			return EventNamesFilter;
		}

//---------------------------------------------------------------------------------------

		// TODO: Add Ahmed-Local cbSystem entry
		private string GetConnectionString() {
			SqlConnectionStringBuilder bld = new SqlConnectionStringBuilder();
			switch (cbSystem.Text) {
			case "DBMart":
				bld.DataSource		= "75.126.77.59,1092";
				bld.InitialCatalog	= "LeadsLightning";
				bld.UserID			= "sa";
				bld.Password		= "$yclahtw2007bycnmhd!";
				break;
			case "CrystalTech":
				bld.DataSource		= "SQLB5.webcontrolcenter.com";
				bld.InitialCatalog	= "LLDevel";
				bld.UserID			= "ahmed";
				bld.Password		= "i7e9dua$tda@";
				break;
#if false
			case "Ahmed-Local":
				throw new Exception("Ahmed-Local support not yet implemented");
				break;
#endif
			default:
				throw new Exception("Unknown System type - " + cbSystem.Text);
			}
			return bld.ConnectionString;
		}

//---------------------------------------------------------------------------------------

		private void cbSystem_SelectedIndexChanged(object sender, EventArgs e) {
			RestartTimer();
			ShowCurrentReport();
		}

//---------------------------------------------------------------------------------------

		private void btnLegend_Click(object sender, EventArgs e) {
			string msg = "SPS\t\t\t-   Swipes per second"
					 + "\nSwipes\t\t\t-   Number of swipes"
					 + "\nWhen Imported\t\t-   Local (Eastern) Date and Time of import"
					 + "\nFirst/Last Name\t\t-   User first and last name"
					 + "\nEvent Name\t\t-   Event name"
					 + "\nEvent Start/End Date/City\t-   Event Start and End date, City"
					 + "\nTtIns/RspIns\t\t-   Tall Table/Response Table Insertions"
					 + "\nms\t\t\t-   Elapsed time in milliseconds"
					 + "\nUserID\t\t\t-   UserID who owns the data"
					 + "\nTerminalSerial\t\t-   Terminal ID where data came from"
					 + "\nFB_TT/FB_Rsp\t\t-   Fallbacks Tall Table/Response Table";
			msg += "\n\nTODO: Update the above";
			MessageBox.Show(msg, "Report Legend", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

//---------------------------------------------------------------------------------------

		private void LLImportStats_Load(object sender, EventArgs e) {
			this.WindowState = FormWindowState.Maximized;
			RestartTimer();

			ShowEventSummary();		// Assumes this is first tab
		}

//---------------------------------------------------------------------------------------

		private void RestartTimer() {
			if (nudRefreshInterval.Value <= 0) {
				timer1.Stop();
				return;
			}
			timer1.Interval = (int)nudRefreshInterval.Value * 1000 * 60;
			timer1.Stop();
			timer1.Start();
		}

//---------------------------------------------------------------------------------------

		private void timer1_Tick(object sender, EventArgs e) {
			ShowCurrentReport();
		}

//---------------------------------------------------------------------------------------

		private void nudRefreshInterval_ValueChanged(object sender, EventArgs e) {
			RestartTimer();
		}

//---------------------------------------------------------------------------------------

		private void btnFilter_Click(object sender, EventArgs e) {
			string strConn = GetConnectionString();
			using (SqlConnection conn = new SqlConnection(strConn)) {
				conn.Open();
				LLFilterForm filter = new LLFilterForm(StatsFilter, conn);
				DialogResult	res = filter.ShowDialog();
				if (res == DialogResult.OK) {
					ShowCurrentReport();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
			TabPage	tab = tabControl1.SelectedTab;
			switch (tab.Name) {
			case "tabEventSummary":
				ShowCurrentReport = new delCurrentReport(ShowEventSummary);;
				break;
			case "tabEventDetails":
				ShowCurrentReport = new delCurrentReport(ShowEventDetails);
				break;
			case "tabImportStats":
				ShowCurrentReport = new delCurrentReport(ShowImportStats);
				break;
			default:
				MessageBox.Show("Can't find tab named " + tab.Name, "Internal error in tabControl1_SelectedIndexChanged", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				break;
			}
			ShowCurrentReport();
		}

//---------------------------------------------------------------------------------------

		private void SetFormCaption(DataTable table) {
			SetFormCaption(table.Rows.Count);
		}

//---------------------------------------------------------------------------------------

		private void SetFormCaption(int rows) {
			string msg = string.Format("{0} - {1} record{2}",
				FormOriginalCaption, rows, rows == 1 ? "" : "s");
			this.Text = msg;
		}

//---------------------------------------------------------------------------------------

		private string GetDetailsSQL() {
			string SQL = @"
SELECT tblCompanies.CompanyName AS [RC Name], 
				tblEvents.EventName AS [Event Name], 
				tblTerminal.TerminalSerial AS TermID, COUNT(*) AS [Swipe Count],
				tblEvents.EventStartDate AS [Event Start Date], 
				tblEvents.EventEndDate AS [Event End Date], 
				tblEvents.EventCity AS City, tblEvents.EventState AS State
	FROM  tblAccountsExtended INNER JOIN
				tblAccounts ON tblAccountsExtended.AcctID = tblAccounts.AcctID INNER JOIN
				tblCompanies ON tblAccountsExtended.CompanyID = tblCompanies.CompanyID INNER JOIN
				tblEvents ON tblAccounts.AcctID = tblEvents.EventRCID INNER JOIN
				tblSwipes ON tblEvents.EventID = tblSwipes.EventID INNER JOIN
				tblTerminal ON tblSwipes.TerminalID = tblTerminal.ID";

			string	CompanyFilter = GetCompanyNameFilter();
			string	EventFilter	  = GetEventNameFilter();
			SQL += "\n\tWHERE ";
			string	clause = CompanyFilter.Length > 0 ? CompanyFilter : "(1=1)";
			clause += "\n\t  AND " + (EventFilter.Length > 0 ? EventFilter : "(1=1)");
			SQL += clause;

			SQL += "\n\tGROUP BY tblCompanies.CompanyName,"
				+  "\n\t\ttblEvents.EventName, tblEvents.EventStartDate, tblEvents.EventEndDate,"
				+  "\n\t\ttblEvents.EventCity, tblEvents.EventState, "
				+  "\n\t\ttblTerminal.TerminalSerial";
			return SQL;
		}

//---------------------------------------------------------------------------------------

		private void btnRefreshSummary_Click(object sender, EventArgs e) {
			ShowEventSummary();
		}

//---------------------------------------------------------------------------------------

		private void btnRefreshEventDetails_Click(object sender, EventArgs e) {
			ShowEventDetails();
		}
	}
}