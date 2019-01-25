// Copyright (c) 2007 Bartizan Connects LLC

// Overview -- We have a file with Bartizan box (including Leads2Go) serial numbers.
//	All the serials are either Bartizan units (Ultra, Leads2Go, etc), or (later on)
//	may be units from other sources (e.g. Trakker).
//
//	If the units are Bartizan products, they may be owned by us or by a Reg Contractor.
//	However, I've been assured by Lew that no Exhibitor owns (or will own) a box.
//
//	If a Reg Contractor owns the box, and uses LeadsLightning, then we need to know that
//	fact so we can bill him. But if Bartizan owns the box, then any LeadsLightning usage
//	will either be handled by coupons, direct billing, included in the rental of a
//	Leads2Go unit, etc.
//
//	The file we process will be used to create/update entries in tblTerminalOwnership
//	so that we can do some kind of billing.
//
//	The GUI has the name of the file, and also lists the reg contractors (of which
//	Bartizan is one), so we can (manually) specify who owns these terminals. There's
//	also a check box to indicate whether the first row (presumably with a title) is to
//	be skipped or not).
//
//	Note: Units can change owners. If, say, CDS wants to buy a unit, we can sell them one
//	of ours. Or they might exchange units. Or we may take one back and give them credit.
//	So we need multiple rows per Terminal ID in tblTerminalOwnership, each with a Start
//	and End Date. HOWEVER, THIS FEATURE IS NOT IMPLEMENTED IN THE CURRENT VERSION OF
//	THIS PROGRAM. IT WILL BE ADDED LATER. BUT FOR NOW, WE ASSUME THAT THERE IS A 1-1
//	RELATIONSHIP BETWEEN tblTerminal AND tblTerminalOwnership.
//
//	Note: We currently assume that all units are Bartizan devices. There is no support
//	for third-party (e.g. Trakker) devices. Later we might add a field (e.g. to 
//	tblTerminal?) to indicate the Manufacturer of the device).
//
// The processing is as follows:
//	*	Make an initial pass through tblTerminal. Any entries in that table that don't
//		have corresponding entries in tblTerminalOwnership will have entries added, 
//		with an owner of Unknown. Note that this makes the system a bit more robust. If
//		ever we (perhaps inadvertantly) delete rows from tblTerminalOwnership, we can
//		restore them (albeit without ownership info) just by running an empty file 
//		through this program.
//			*	Note that at this point, while we'll ensure that every entry in
//				tblTerminal has a corresponding entry in tblTerminalOwnership, we won't
//				go in the other direction. IOW, there may be more rows in 
//				tblTerminalOwnership than in tblTerminal. Maybe we'll do something in 
//				this area some day.
//	*	Read each serial number (optionally skipping the first row).
//			*	If it's not in tblTerminal, add it, and add to tblTerminalOwnership with
//				the specified owner.
//			*	Get the tblTerminalOwnership entry, and check who owns it.
//					*	If it's Unknown, update it to the specified owner. Also set the
//						IsBartizan flag (true or false).
//					*	If it's the specified owner, do nothing. We assume that if 
//						there's an owner, the IsBartizan flag has been set.
//					*	If it's a different owner, ask the user (via MsgBox) if we should
//						change the ownership (and concommitant IsBartizan flag) or not. 
//						(Later, we may add extra rows to the table to indicate multiple 
//						owners.)

#define		DBG

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

// TODO:
//	*	Add Log file w/GUI interface - default to My Documents\llTerminalMaintenance.log

using LL.Database;

namespace LLTerminalMaintenance {

	public partial class LLTerminalMaintenance : Form {

		// We need a valid AcctID in tblTerminalOwnership, else several
		// queries might fail. During initiation, we'll find our reserved ID

		string		BartizanUserID = "zzz$LLTerminalMaintenance";
		int			BartizanAcctID;

		// I hate magic numbers, but here there's little (if any) choice.
		internal const int SexDuplicateKey = 2601;	// SqlException.Number if INSERT 
		// fails on duplicate key.
		internal const int SexUniqueViol = 2627;	// Or if INSERT violates UNIQUE
		// key constraint

		StreamWriter	LogFile;


//---------------------------------------------------------------------------------------

		public LLTerminalMaintenance() {
			InitializeComponent();

			cmbHosts.DataSource = DatabaseConnectionString_Explicit.GetSystemNames();

			string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			path = Path.Combine(path, "llTerminalMaintenance.txt");
			LogFile = new StreamWriter(path, true);
		}

//---------------------------------------------------------------------------------------

		private void LLTerminalMaintenance_Load(object sender, EventArgs e) {
			FillRCCombobox();
		}

//---------------------------------------------------------------------------------------

		private void FillRCCombobox() {
			string SQL = @"
SELECT	DISTINCT tblAccounts.AcctID, tblCompanies.CompanyName
FROM	tblCompanies 
			INNER JOIN tblAccountsExtended 
			   ON tblCompanies.CompanyID = tblAccountsExtended.CompanyID 
			INNER JOIN tblAccounts 
			   ON tblAccountsExtended.AcctID = tblAccounts.AcctID
			INNER JOIN tblEvents 
			   ON tblAccounts.AcctID = tblEvents.EventRCID  -- Must have at least 1 event
WHERE (tblAccounts.AcctType = 4)
  AND (CompanyName NOT LIKE 'zzz$%')
ORDER BY tblCompanies.CompanyName";

			using (SqlConnection conn = GetConnection()) {
				conn.Open();
				SqlDataAdapter adapt = new SqlDataAdapter(SQL, conn);
				DataTable dt = new DataTable();
				adapt.Fill(dt);
				cmbOwners.DataSource = dt;
				cmbOwners.DisplayMember = "CompanyName";
			}
		}

//---------------------------------------------------------------------------------------

		private SqlConnection GetConnection() {
			string System = (string)cmbHosts.SelectedValue;
			string sConn = DatabaseConnectionString_Explicit.GetConnectionString(System);
			SqlConnection conn = new SqlConnection(sConn);
			return conn;
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			openFileDialog1.Filter = "Simple text files (*.txt)|*.txt|All Files (*.*)|*.*";
			openFileDialog1.FileName = "";
			DialogResult res = openFileDialog1.ShowDialog();
			if (res == DialogResult.OK) {
				txtTermFile.Text = openFileDialog1.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void cmbHosts_SelectedIndexChanged(object sender, EventArgs e) {
			FillRCCombobox();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			string Filename = txtTermFile.Text.Trim();
			if (Filename.Length == 0) {
				MessageBox.Show("Please specify a filename", this.Text,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			lbMsgs.Items.Clear();		// Start again

			AddMsg("Processing file " + Filename);
			using (SqlConnection conn = GetConnection()) {
				conn.Open();

				Stopwatch	sw = new Stopwatch();

				GetBartizanAcctID(conn);

				sw.Start();
				SynchTables(conn);
				sw.Stop();
				TimeSpan	SynchTime = sw.Elapsed;

				sw.Reset();
				sw.Start();
				ProcessTerminalFile(Filename, conn);
				sw.Stop();
				TimeSpan	ProcFileTime = sw.Elapsed;

				string		msg = "Serial Number import done. SynchTables took {0}, ProcessFile took {1}";
				msg = string.Format(msg, SynchTime, ProcFileTime);

				AddMsg(msg);
				AddMsg("");		// Separator, in case we process another file

				MessageBox.Show(msg, "LeadsLightning Terminal Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Go through tblTerminal and ensure that there's a corresponding row in
		/// tblTerminalOwnership. At some later date we might go in the other direction.
		/// But not today.
		/// </summary>
		/// <param name="conn"></param>
		private void SynchTables(SqlConnection conn) {

			string SQLTerm = @"
SELECT ID, TerminalSerial FROM tblTerminal
			LEFT JOIN tblTerminalOwnership
				ON tblTerminal.ID = tblTerminalOwnership.TerminalID
WHERE TerminalID IS NULL
";
			SqlCommand cmdTerm = new SqlCommand(SQLTerm, conn);
			SqlDataReader rdrTerm = cmdTerm.ExecuteReader();
			while (rdrTerm.Read()) {
				int TermID = (int)rdrTerm["ID"];
				string Serial = (string)rdrTerm["TerminalSerial"];
				dbg("Synching {0}", Serial);
				string SQLOwner = @"
SELECT	COUNT(*) FROM tblTerminalOwnership
WHERE	TerminalID = {0}";
				SQLOwner = string.Format(SQLOwner, TermID);
				using (SqlConnection connowner = GetConnection()) {
					connowner.Open();
					SqlCommand cmdOwner = new SqlCommand(SQLOwner, connowner);
					int nRows = (int)cmdOwner.ExecuteScalar();
					// TODO: We assume here there is only one row. Later this won't be true.
					if (nRows > 0) {
						dbg("Found Owner record");
						continue;
					}
					InsertOwnerRecord(TermID, connowner, null);
				}
			}
			rdrTerm.Close();
		}

//---------------------------------------------------------------------------------------

		private void InsertOwnerRecord(int TermID, SqlConnection conn) {
			InsertOwnerRecord(TermID, conn, chkIsOwnerBartizan.Checked);
		}

//---------------------------------------------------------------------------------------

		private void InsertOwnerRecord(int TermID, SqlConnection conn, bool? IsBartOwner) {
			// Note: tblTerminalOwnership.IsBartizan is tri-state. Values of true or 
			//		 false are obvious. If it's null, then we don't know, one way or the
			//		 other, who the terminal belongs to.
			AddMsg("Inserting Owner Record for TermID={0}", TermID);
			string SQL = @"
INSERT	INTO tblTerminalOwnership(TerminalID, StartDate, EndDate, OwnerAcctID, IsBartizan)
VALUES	(@TerminalID, @StartDate, @EndDate, @OwnerAcctID, @IsBartizan)
";
			SqlCommand cmdInsOwner = new SqlCommand(SQL, conn);
			cmdInsOwner.Parameters.AddWithValue("@TerminalID", TermID);
			// SQL Server uses a different datetime representation than the .NET
			// framework, so we can't use DateTime.[Min/Max]Value. Just hand-code
			// it.
			cmdInsOwner.Parameters.AddWithValue("@StartDate", new DateTime(1900, 1, 1));
			// Worry about Y3K issues later...
			cmdInsOwner.Parameters.AddWithValue("@EndDate", new DateTime(3000, 12, 25));
			// cmdInsOwner.Parameters.AddWithValue("@OwnerAcctID", GetcmbOwnersAcctID());
			cmdInsOwner.Parameters.AddWithValue("@OwnerAcctID", BartizanAcctID);
			if (IsBartOwner.HasValue) {
				cmdInsOwner.Parameters.AddWithValue("@IsBartizan", IsBartOwner);
			} else {
				cmdInsOwner.Parameters.AddWithValue("@IsBartizan", DBNull.Value);
			}

			try {
				int nRowsOwner = cmdInsOwner.ExecuteNonQuery();
			} catch (SqlException sex) {
				if (!IsDupRecord(sex)) {
					throw;							// Not dup key. Bombs away!
				}
				// Ignore this if there's a duplicate. It shouldn't happen, and it's no
				// big deal if there is a duplicate.
			}
		}

//---------------------------------------------------------------------------------------

		int GetcmbOwnersAcctID() {
			// If it's a Bartizan terminal, don't use the one displayed (even if it says
			// "Bartizan"). What we need instead is the AcctID of the reserved Bartizan
			// account for this program.
			if (chkIsOwnerBartizan.Checked) {
				return BartizanAcctID;
			}
			DataRowView	view = (DataRowView)cmbOwners.SelectedItem;
			return (int)view["AcctID"];
		}

//---------------------------------------------------------------------------------------

		internal static bool IsDupRecord(SqlException sex) {
			return (sex.Number == SexDuplicateKey) || (sex.Number == SexUniqueViol);
		}

//---------------------------------------------------------------------------------------

		private void ProcessTerminalFile(string Filename, SqlConnection conn) {
			StreamReader rdr = null;
			try {
				rdr = new StreamReader(Filename);
				string line;
				bool bProcessLine = !chkIgnoreFirstLine.Checked;
				while ((line = rdr.ReadLine()) != null) {
					if (bProcessLine) {		// Skip first line?
						line = line.Trim();
						if (line.Length > 0) {
							ProcessSerial(line, conn);
						}
					}
					bProcessLine = true;
				}
			} catch (FileNotFoundException) {
				MessageBox.Show("Could not find the file. Please try again.", this.Text,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			} catch (Exception ex) {
				string msg = string.Format("Unexpected error occurred processing file {0}"
							+ "\n\nDiagnostic info - {1}", Filename, ex);
				MessageBox.Show(msg, this.Text,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			} finally {
				if (rdr != null) {
					rdr.Close();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessSerial(string TermSerial, SqlConnection conn) {
			string[] Terms = TermSerial.Split(' ');	// Ignore extra stuff on the line
			string Term = Terms[0];
			// string Term = "x" + Terms[0];		// For debugging
			AddMsg("Processing " + Term);

			int		NewOwnerAcctID = GetcmbOwnersAcctID();

			int TermID = GetTermID(Term, conn);
			// Note -- This is a bit inefficient if we wound up adding the terminal to
			//		tblTerminal while in GetTermID. We could have added it there, and
			//		returned a parameter to bypass the SELECT we're about to do. But hey,
			//		this is hardly a data intensive application, running thousands of
			//		times a day. So we'll accept a bit of inefficiency (which the user
			//		won't even notice) to keep the code a bit simpler.
			
			// So get the Owner record corresponding to this TermID and make sure that
			// the owner matches. If not...
			string SQL = @"
SELECT	* 
FROM	tblTerminalOwnerShip
WHERE	TerminalID = " + TermID;
			SqlCommand		cmd = new SqlCommand(SQL, conn);
			SqlDataReader	rdr = cmd.ExecuteReader();
			// Note: Here's one place where we assume a max of 1 Owner record per term
			bool bReadOK = rdr.Read();
			if (! bReadOK) {
				throw new Exception("Couldn't find an Owner record for TermID=" + TermID);
			}

			// OK, we have the owner record for the entry in tblTerminal.
			int		CurOwnerAcctID = (int)rdr["OwnerAcctID"];
			object	oIsBart = rdr["IsBartizan"];
			rdr.Close();

			if (Convert.IsDBNull(oIsBart)) {
				// At this point, We previously didn't know who owned this. Now we do.
				// Update the record with IsBart, OwnerAcctID
				SQL = @"
UPDATE tblTerminalOwnership 
SET		OwnerAcctID = {0},
		IsBartizan  = {1}
WHERE	TerminalID  = {2}
";
				SQL = string.Format(SQL, NewOwnerAcctID, 
					Convert.ToInt32(chkIsOwnerBartizan.Checked),
					TermID);
				cmd = new SqlCommand(SQL, conn);
				cmd.ExecuteNonQuery();
				return;
			}

			// At this point, we know that someone owned it. Check that the owners
			// match. If so, we're done (we assume that IsBart is set to that owner).
			// If not, ask the user what to do.
			if (NewOwnerAcctID == CurOwnerAcctID) {
				return;
			}

			// The owners don't match. Ask the user what to do.
			// TODO: MsgBox and update the database.
			string	CurOwner = GetCompanyName(CurOwnerAcctID, conn);
			CurOwner = CurOwner ?? "*Unknown*";
			DataRowView	view = (DataRowView)cmbOwners.SelectedItem;
			string NewOwner = (string)view["CompanyName"];

		
			string	msg = string.Format("Terminal '{0}' currently belongs to '{1}',"
				+ " but the input file specifies that it belongs to '{2}'."
				+ "\nThe IsBartizan flag is set to {3}."
				+ "\n\nTo change ownership (and set the IsBartizan flag to {4}), click Yes."
				+ "\nTo keep the current owner (but set the IsBartizan flag to {4}), click No."
				+ "\nTo ignore this entry, click Cancel.",
				Term, CurOwner, NewOwner, oIsBart, chkIsOwnerBartizan.Checked);
			DialogResult res = MessageBox.Show(msg, 
				"Change Owner?", 
				MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
			switch (res) {
			case DialogResult.Yes:
				SQL = @"
UPDATE tblTerminalOwnership 
SET		OwnerAcctID = {0},
		IsBartizan  = {1}
WHERE	TerminalID  = {2}
";
				SQL = string.Format(SQL, NewOwnerAcctID,
					Convert.ToInt32(chkIsOwnerBartizan.Checked),
					TermID);
				cmd = new SqlCommand(SQL, conn);
				cmd.ExecuteNonQuery();
				return;
			case DialogResult.No:
				SQL = @"
UPDATE tblTerminalOwnership 
SET		IsBartizan  = {0}
WHERE	TerminalID  = {1}
";
				SQL = string.Format(SQL,
					Convert.ToInt32(chkIsOwnerBartizan.Checked),
					TermID);
				cmd = new SqlCommand(SQL, conn);
				cmd.ExecuteNonQuery();
				return;
			case DialogResult.Cancel:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private string GetCompanyName(int AcctID, SqlConnection conn) {
			string SQL = @"
SELECT tblCompanies.CompanyName
FROM	tblAccounts 
			INNER JOIN tblAccountsExtended 
			   ON tblAccounts.AcctID = tblAccountsExtended.AcctID 
			INNER JOIN tblCompanies 
			   ON tblAccountsExtended.CompanyID = tblCompanies.CompanyID
WHERE (tblAccounts.AcctID = {0})";
			SQL = string.Format(SQL, AcctID);
			SqlCommand	cmd = new SqlCommand(SQL, conn);
			object	o = cmd.ExecuteScalar();
			return o as string;			// May be null
		}

//---------------------------------------------------------------------------------------

#if false		// No longer needed
		private int GetAcctIDForCompanyName(string CompanyName, SqlConnection conn) {
			string	SQL = @"
SELECT	tblAccountsExtended.AcctID
FROM	tblCompanies
			INNER JOIN tblAccountsExtended
				ON tblCompanies.CompanyID = tblAccountsExtended.CompanyID
WHERE	CompanyName = @CompanyName
";
			SqlCommand	cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@CompanyName", CompanyName);
			object	oAcctID = cmd.ExecuteScalar();
			if (oAcctID == null) {
				throw new Exception("Unable to find Company " + CompanyName);
			}
			return (int) oAcctID;
		}
#endif

//---------------------------------------------------------------------------------------

		private void GetBartizanAcctID(SqlConnection conn) {
			string	SQL = @"
SELECT	AcctID
FROM	tblAccounts
WHERE	UserID = '{0}'
";
			SQL = string.Format(SQL, BartizanUserID);
			SqlCommand	cmd = new SqlCommand(SQL, conn);
			object	oAcctID = cmd.ExecuteScalar();
			if (oAcctID == null) {
				throw new Exception("Unable to find reserved Bartizan UserID " + BartizanUserID);
			}
			BartizanAcctID = (int)oAcctID;
		}

//---------------------------------------------------------------------------------------

		private int GetTermID(string Term, SqlConnection conn) {
			string SQL = @"
SELECT ID FROM tblTerminal WHERE TerminalSerial = '{0}'
";
			SQL = string.Format(SQL, Term);
			SqlCommand cmd = new SqlCommand(SQL, conn);
			object oID = cmd.ExecuteScalar();
			if (oID == null) {
				return AddTerminalToTable(Term, conn);
			}
			return (int)oID;
		}

//---------------------------------------------------------------------------------------

		private int AddTerminalToTable(string Term, SqlConnection conn) {
			AddMsg("Inserting '{0}' into both tblTerm and tblTermOwner", Term);
			string SQL = @"
INSERT INTO tblTerminal(TerminalSerial) VALUES(@TermSerial)
";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@TermSerial", Term);
			int nRows = cmd.ExecuteNonQuery();
			if (nRows != 1) {
				throw new Exception("Unable to add '" + Term + "' to tblTerminals.");
			}
			int		TermID = GetAutonumID(conn, "tblTerminal");

			InsertOwnerRecord(TermID, conn);

			return TermID;
		}

//---------------------------------------------------------------------------------------

		public static int GetAutonumID(SqlConnection conn, string tblName) {
			string SQL = "SELECT IDENT_CURRENT('" + tblName + "')";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			return GetID(cmd.ExecuteScalar());
		}

//---------------------------------------------------------------------------------------

		private static int GetID(object ID) {
			if ((ID == null) || (ID is DBNull)) {
				return -1;
			}
			// Sigh. Access returns an int for @@IDENTITY, but SQL Server returns
			// a Decimal, so we have to use Convert.
			return Convert.ToInt32(ID);
		}

//---------------------------------------------------------------------------------------

		private void AddMsg(string fmt, params object[] args) {
			string msg = string.Format(DateTime.Now + " - " + fmt, args);
			AddMsg(msg);
		}

//---------------------------------------------------------------------------------------

		private void AddMsg(string msg) {
			lbMsgs.Items.Add(msg);
			Application.DoEvents();
			LogFile.WriteLine(msg);
		}

//---------------------------------------------------------------------------------------

		[Conditional("DBG")]
		private void dbg(string fmt, params object[] args) {
			AddMsg(fmt, args);
		}

//---------------------------------------------------------------------------------------

		[Conditional("DBG")]
		private void dbg(string msg) {
			AddMsg(msg);
		}

//---------------------------------------------------------------------------------------

		private void cmbOwners_SelectedIndexChanged(object sender, EventArgs e) {
			DataRowView	view = (DataRowView)cmbOwners.SelectedItem;
			chkIsOwnerBartizan.Checked = ((string)view["CompanyName"]).ToUpper().Contains("BARTIZAN");
		}
	}
}