// Copyright (c) 2007 Bartizan Connects, LLC


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

// TODO:
//	*	Return from non-cancelled Filter form needs to rerun current form
//	*	Event/Company filtering - do it and return values
//	*	Show events just in the specified from/to time frame
//	*	Printing might be nice
//	*	Ability to select columns, especially on ImportStats. Also for printing.
//	*	Persist filters, columns, etc
//	*	Add Activated/non-Activated counts, by Event

namespace LLImportStats {

	delegate void	delCurrentReport();

	public partial class LLImportStats : Form {
		string				FormOriginalCaption;
		DataGridView		CurrentDataGridView = null;

		LLStatsFilterInfo	StatsFilter;

		delCurrentReport	ShowCurrentReport;

//---------------------------------------------------------------------------------------

		public LLImportStats() {
			InitializeComponent();

			FormOriginalCaption = this.Text;

			ShowCurrentReport = new delCurrentReport(ShowEventSummary);

			StatsFilter = new LLStatsFilterInfo();

#if false
			Bartizan.Disk.Utils.VolumeInformation VolInfo =
				new Bartizan.Disk.Utils.VolumeInformation(@"C:");
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			ShowImportStats();
		}

//---------------------------------------------------------------------------------------

		private void ShowEventSummary() {
			string SQL = "WITH Details AS (" + GetDetailsSQL() + "\n)\n\n"
#if false
						+ "SELECT * FROM Details";
			MessageBox.Show("Fix this...", "Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#else
						+ "SELECT [RC Name], [Event Name], SUM([Swipe Count]) AS [Total Swipes], "
						+ "\n\tCOUNT(TermID) AS [Term Count], "
//						+ "\n\tCOUNT(IsBartizan) AS [IsBartizan Count], "
//	Dunno what the OVER is quite doing. Look into it later.
//						+ "\n\tCOUNT(*) OVER(PARTITION BY TermOwner) AS [LRS TODO Count], "
						+ "\n\tTermOwner AS [Terminal Owner], "
						+ "\n\t[Event Start Date], [Event End Date], City, State, [Import Option]"
						+ "\n\tFROM Details"
						+ "\n\tGROUP BY [RC Name], [Event Start Date], [Event Name], [Event End Date], City, State"
						+ ", TermOwner, [Import Option]"
						+ "\n\tORDER BY [RC Name], [Event Start Date] DESC, [Event Name]";
#endif

			string strConn = GetConnectionString();
			using (SqlConnection conn = new SqlConnection(strConn)) {
				DataTable summary = new DataTable("Summary");
				SqlDataAdapter adapt = new SqlDataAdapter(SQL, conn);
				adapt.Fill(summary);
				dgvSummary.DataSource = summary;
				SetFormCaption(summary);
			}

			CurrentDataGridView = dgvSummary;
			ColorizeGrid("RC Name", "Event Name");
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
			CurrentDataGridView = dgvDetails;
			ColorizeGrid("RC Name", "Event Name");
		}

//---------------------------------------------------------------------------------------

		private void ColorizeGrid(params string [] ColumnNames) {
			string	PreviousRowKey = "";
			string	CurrentRowKey;
			foreach (DataGridViewRow row in CurrentDataGridView.Rows) {
				CurrentRowKey = "";
				foreach (string ColName in ColumnNames) {
					CurrentRowKey += row.Cells[ColName].Value + "$@%%*#";	// Append swear word
				}
				if (CurrentRowKey != PreviousRowKey) {
					foreach (DataGridViewCell cell in row.Cells) {
						cell.Style.BackColor = Color.Cyan;
					}
					PreviousRowKey = CurrentRowKey;
				}
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
			CurrentDataGridView = dgvStats;
			ColorizeGrid("EventID");
		}

//---------------------------------------------------------------------------------------

		private int FillImportStatsGrid(SqlConnection conn) {
			string SQL = @"
SELECT	CAST(tblSavedImports.RecordCount / (tblSavedImports.MillisecondsToImport / 1000.0) AS int) AS SPS, 
		tblSavedImports.RecordCount AS Swipes, 
        tblSavedImports.TallTableInsertions AS TtIns, tblSavedImports.ResponseInsertions AS RspIns,
		tblSavedImports.MillisecondsToImport AS ms,  
	    tblSavedImports.WhenImported AS [When Imported],
        tblAccounts.FirstName AS [First Name], tblAccounts.LastName AS [Last Name], 
		tblAccounts.Activated,
		tblEvents.EventName AS [Event Name], tblEvents.EventStartDate AS [Event Start Date], 
		tblEvents.EventEndDate AS [Event End Date], tblEvents.EventCity AS [Event City],
        tblSavedImports.TerminalSerial AS [Terminal Serial],
		tblAccounts.emailAddress AS email, 
		tblAccts.FirstName AS [RC FName], tblAccts.LastName AS [RC LName], tblAccts.AcctID as [RC AcctID],
		tblCompanies.CompanyName AS [RC Company Name],
	    tblAccounts.UserID, 
		tblSavedImports.AcctID, tblSavedImports.EventID, tblSavedImports.MapCfgID,
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

			string	where = GetImportStatsWhere();
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

		private string GetImportStatsWhere() {
			string	where = "\n\tWHERE";
			where += string.Format("\t\t(tblSavedImports.MillisecondsToImport > 0)"
				  + "\n\t  AND (tblSavedImports.WhenImported >= '{0}')",
				StatsFilter.ImportStartDate);

			if (StatsFilter.RCCompanies.Count > 0) {
				where += "\n\t  AND ";
				where += GetCompanyNameFilter();
			}

			if (StatsFilter.bIgnoreBartizanTests) {
				where += "\n\t  AND (CompanyName NOT LIKE 'zzz$%')";
			}

			if (chkOnlyActivated.Checked) {
				where += "\n\t  AND (tblAccounts.Activated = 1)";
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
			string EventNamesFilter = "tblEvents.EventName IN (";
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
			case "Prod":
				bld.DataSource		= "198.64.249.6,1092";
				bld.InitialCatalog	= "LeadsLightning";
				bld.UserID			= "sa";
				bld.Password		= "$yclahtw2007bycnmhd!";
				break;
			case "OldDBMart":
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
			ShowCurrentReportWithHourglass();
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

			cbSystem.Text = "Prod";
			
			RestartTimer();
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
			ShowCurrentReportWithHourglass();
		}

//---------------------------------------------------------------------------------------

		private void ShowCurrentReportWithHourglass() {
			Cursor	SaveCursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			try {
				ShowCurrentReport();
			} finally {
				this.Cursor = SaveCursor;
			}
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
					ShowCurrentReportWithHourglass();
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

			ShowCurrentReportWithHourglass();
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
SELECT	tblCompanies.CompanyName AS [RC Name], 
		tblEvents.EventName AS [Event Name], 
		tblTerminal.TerminalSerial AS TermID, 
		tblTerminalOwnership.IsBartizan,
		TermOwner =
			  CASE tblTerminalOwnership.IsBartizan
				 WHEN 0 THEN tblCompanies_1.CompanyName
				 WHEN 1 THEN tblCompanies_1.CompanyName -- 'Bartizan'
				 ELSE '*Unknown*'
			  END,
		COUNT(*) AS [Swipe Count],
		tblEvents.EventStartDate AS [Event Start Date], 
		tblEvents.EventEndDate AS [Event End Date], 
		tblEvents.EventCity AS City, 
		tblEvents.EventState AS State,
		[Import Option] =
			CASE tblEvents.EvImOpID
				WHEN 1 THEN 'RC Only'
				WHEN 2 THEN 'Separate'
				WHEN 3 THEN 'Both'
			END
FROM	tblAccountsExtended 
		INNER JOIN tblAccounts 
			ON tblAccountsExtended.AcctID = tblAccounts.AcctID 
		INNER JOIN tblCompanies 
			ON tblAccountsExtended.CompanyID = tblCompanies.CompanyID 
		INNER JOIN tblEvents 
			ON tblAccounts.AcctID = tblEvents.EventRCID 
		INNER JOIN tblSwipes 
			ON tblEvents.EventID = tblSwipes.EventID 
		LEFT JOIN tblTerminal 
			ON tblSwipes.TerminalID = tblTerminal.ID
		LEFT JOIN tblTerminalOwnership
			ON tblTerminal.ID = tblTerminalOwnership.TerminalID
		LEFT JOIN tblAccounts AS tblAccounts_1
			ON tblTerminalOwnership.OwnerAcctID = tblAccounts_1.AcctID
		LEFT JOIN tblAccountsExtended AS tblAccountsExtended_1
			ON tblAccounts_1.AcctID = tblAccountsExtended_1.AcctID
		LEFT JOIN tblCompanies AS tblCompanies_1
			ON tblAccountsExtended_1.CompanyID = tblCompanies_1.CompanyID
";

			string	CompanyFilter = GetCompanyNameFilter();
			string	EventFilter	  = GetEventNameFilter();
			SQL += "\n\tWHERE ";
			string	clause = CompanyFilter.Length > 0 ? CompanyFilter : "(1=1)";
			clause += "\n\t  AND " + (EventFilter.Length > 0 ? EventFilter : "(1=1)");
			SQL += clause;

			if (StatsFilter.bIgnoreBartizanTests) {
				SQL += "\n\t  AND (tblCompanies.CompanyName NOT LIKE 'zzz$%')";
			}

			SQL += "\n\tGROUP BY tblCompanies.CompanyName, tblCompanies_1.CompanyName,"
				+  "\n\t\ttblEvents.EventName, tblEvents.EventStartDate, tblEvents.EventEndDate,"
				+  "\n\t\ttblEvents.EventCity, tblEvents.EventState, tblEvents.EvImOpID,"
				+ "\n\t\ttblTerminal.TerminalSerial, tblTerminalOwnership.IsBartizan";
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

//---------------------------------------------------------------------------------------

		private void btnPrint_Click(object sender, EventArgs e) {

			PrintDocument	pd = new PrintDocument();
			pd.DocumentName = "LL Import Stats";
			// pd.PrinterSettings.PrinterName = "Auto RICOH Aficio AP3800C PCL 5c on BARTSBS";

			pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
			pd.BeginPrint += new PrintEventHandler(pd_BeginPrint);

#if true	// For Print Preview support
			printPreviewDialog1.Document = pd;

			// We might as well center things
			int		w = SystemInformation.PrimaryMonitorSize.Width;
			int		h = SystemInformation.PrimaryMonitorSize.Height;
			printPreviewDialog1.Top	   = h / 4;
			printPreviewDialog1.Left   = w / 4;
			printPreviewDialog1.Width  = w / 2;
			printPreviewDialog1.Height = h / 2;
#endif
		}

//---------------------------------------------------------------------------------------

		void pd_BeginPrint(object sender, PrintEventArgs e) {
			// throw new Exception("The method or operation is not implemented.");
		}

//---------------------------------------------------------------------------------------

		void pd_PrintPage(object sender, PrintPageEventArgs e) {
			// DataGridView	dgv = dgvSummary;		// TODO:
			// Graphics		g = e.Graphics;
		}

//---------------------------------------------------------------------------------------

		private void btnExport_Click(object sender, EventArgs e) {
			openFileDialog1.Filter = "Comma-Separated files (*.csv)|*.csv|All Files (*.*)|*.*";
			openFileDialog1.FileName = "";
			openFileDialog1.CheckFileExists = false;
			DialogResult res = openFileDialog1.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}

			string			filename = openFileDialog1.FileName;
			StreamWriter	wtr = new StreamWriter(filename);

			// Write header line
			bool			bPrependComma = false;
			foreach (DataGridViewColumn	col in CurrentDataGridView.Columns) {
				WriteCSV(wtr, col.Name, bPrependComma);
				bPrependComma = true;
			}
			wtr.WriteLine();

			// Now write the data
			foreach (DataGridViewRow row in CurrentDataGridView.Rows) {	// Each row
				bPrependComma = false;
				foreach (DataGridViewCell cell in row.Cells) {			// Each column
					if (cell.Value == null) {
						WriteCSV(wtr, "", bPrependComma);
					} else {
						WriteCSV(wtr, cell.Value.ToString(), bPrependComma);
					}
					bPrependComma = true;
				}
				wtr.WriteLine();
			}
			wtr.WriteLine();
			wtr.Close();
		}

//---------------------------------------------------------------------------------------

		private void WriteCSV(StreamWriter wtr, string Field, bool bPrependComma) {
			if (bPrependComma) {
				wtr.Write(",");
			}
			Field = Field.Replace("\"", "'");
			wtr.Write("\"{0}\"", Field);
		}

//---------------------------------------------------------------------------------------

		private void dgvSummary_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
			if (e.RowIndex < 0) {		// Working with header? (e.g. to autosize column widths)
				return;
			}
			string	txt;
			StatsFilter.RCCompanies.Clear();
			txt = (string)dgvSummary.Rows[e.RowIndex].Cells["RC Name"].Value;
			StatsFilter.RCCompanies.Add(txt);

			StatsFilter.EventNames.Clear();
			txt = (string)dgvSummary.Rows[e.RowIndex].Cells["Event Name"].Value;
			StatsFilter.EventNames.Add(txt);

			tabControl1.SelectedTab = tabControl1.TabPages["tabEventDetails"];
		}

//---------------------------------------------------------------------------------------

		private void chkOnlyActivated_CheckedChanged(object sender, EventArgs e) {
			ShowImportStats();
		}
	}
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

namespace Bartizan.Disk.Utils {

	public class VolumeInformation {

		// Note: DllImport need System.Runtime.InteropServices
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		extern static bool GetVolumeInformation(
			  string		RootPathName,
			  StringBuilder VolumeNameBuffer,
			  int			VolumeNameSize,
			  out uint		VolumeSerialNumber,
			  out uint		MaximumComponentLength,
			  out uint		FileSystemFlags,
			  StringBuilder FileSystemNameBuffer,
			  int			nFileSystemNameSize);

		string		_RootPathName;
		string		_VolumeNameBuffer;
		uint		_VolumeSerialNumber;
		uint		_MaximumComponentLength;
		uint		_FileSystemFlags;
		string		_FileSystemNameBuffer;

		bool		_IsValid;

//---------------------------------------------------------------------------------------

		public VolumeInformation(string RootPath) {
			StringBuilder	VolumeNameBuffer = new StringBuilder();
			StringBuilder	FileSystemNameBuffer = new StringBuilder();

			// We want to support a RootPathName of C:, C:\, C:\foo\goo.txt, goo.txt and
			// ..\goo.txt. However we don't support UNCs.
			// TODO: Add try/catch so we don't abort on \\server (although we'll just get
			//		 a bOK = false on \\server\share).
			// Here's the easiest way to do that.
			_RootPathName = Path.GetFullPath(RootPath);
			_RootPathName = Path.GetPathRoot(_RootPathName);
			// We've now got @"C:\" (or whatever the given drive letter was).

			_IsValid = GetVolumeInformation(
				_RootPathName, 
				VolumeNameBuffer, 1024,
				out	_VolumeSerialNumber,
				out	_MaximumComponentLength,
				out _FileSystemFlags,
				FileSystemNameBuffer, 1024);

			if (IsValid) {
				this._VolumeNameBuffer = VolumeNameBuffer.ToString();
				this._FileSystemNameBuffer = FileSystemNameBuffer.ToString();
			}
		}

//---------------------------------------------------------------------------------------

		public string RootPathName {
			get { return _RootPathName; }
		}

//---------------------------------------------------------------------------------------

		public string VolumeNameBuffer {
			get { return _VolumeNameBuffer; }
		}

//---------------------------------------------------------------------------------------

		public uint VolumeSerialNumber {
			get { return _VolumeSerialNumber; }
		}

//---------------------------------------------------------------------------------------

		public uint MaximumComponentLength {
			get { return _MaximumComponentLength; }
		}

//---------------------------------------------------------------------------------------

		public uint FileSystemFlags {
			get { return _FileSystemFlags; }
		}

//---------------------------------------------------------------------------------------

		public string FileSystemNameBuffer {
			get { return _FileSystemNameBuffer; }
		}

//---------------------------------------------------------------------------------------

		public bool IsValid {
			get { return _IsValid; }
		}
	}
}