// Copyright (c) 2006 by Bartizan Connects, LLC

// Note: These must be kept in synch with other factors, such as who's calling
//		 us (test driver, production web service, etc), and also the deployment
//		 parameter in VS2005.
// #define		PROD			// Production, not debug, version
#define		DEVEL			// Debug, but on the remote Server
// #define		LOCAL			// Here, locally, but still on LLDevel


#define	CALL_SQL_DIRECTLY

using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using Bartizan.Utils.CRC;
using Bartizan.LL.Importer;

#if LOCAL
			// Do nothing
#elif DEVEL
#if false
#endif
#elif PROD
#else
#error Must choose one of LOCAL / DEVEL / PROD
#endif

namespace Bartizan.LL.RealTimer {

	/// <summary>
	/// Summary description for LL_sp_SetupFileRoutines
	/// </summary>
	public class LL_sp_SetupFileRoutines {

//---------------------------------------------------------------------------------------

		public LL_sp_SetupFileRoutines() {
			//
			// TODO: Add constructor logic here
			//
		}

//---------------------------------------------------------------------------------------

		public string GetSetupInfo(string SetupID,
					out string UserID, out string Password,
					out int SetupFileLength,
					out int MapCfgID,
					out int EventID,
					out int ErrorCode) {

			// Set the <out> parameters, else the compile will complain (correctly)
			// that they might not be set inside the following try block.
			UserID			= "";
			Password		= "";
			SetupFileLength = -1;
			MapCfgID		= -1;
			EventID			= -1;
			ErrorCode		= -1;

			try {
				// This part's easy.
				// Well, not really. We really should (must!) make sure we haven't
				// accidently generated the same name more than once. TODO:

				// We can't just call GenerateRandom twice in a row. We'll get the
				// same values out for the UserID and the Password. So we'll have to
				// seed them. We'll calculate the current system tick count and use
				// it, and an arbitrary seed of that + 123.
				
				// Note: Some might be concerned that DateTime.Now.Ticks is a long,
				//		 and that, if the system stayed up long enough, this might
				//		 not fit in an int, and the cast below would blow up on an
				//		 overflow. But not to worry. The cast will silently ignore
				//		 the high-order 32 bits. However, we might wind up with the
				//		 0x80000000 bit on, which will make Seed negative. Big deal.
				int Seed = (int)DateTime.Now.Ticks; 

				UserID = GenerateRandomPwd(8, Seed);
				Password = GenerateRandomPwd(8, Seed + 123);

				using (SqlConnection conn = new SqlConnection()) {
					SetConnectionString(conn);
					conn.Open();

					string SetupFileContents;
					int ErrCode = GetSetupFile(conn, SetupID.ToUpper(), out EventID, out MapCfgID,
						out SetupFileContents);
					if (ErrCode == -1) {
						string msg = string.Format("Error - We could not find the setup file"
							+ " for ID '{0}'. Please check that you entered it correctly",
							SetupID);
						return msg;
					}
					SetupFileLength = SetupFileContents.Length;
					// OK, everything looks OK. Add UserID/Password
					string	msg2;
					bool	bOK = AddUserIDPassword(conn, UserID, Password, EventID, out msg2);
					if (!bOK) {
						ErrorCode = -1;
						return "Error - Could not add new userid and password - " + msg2;
					} else {
						ErrorCode = 0;
						return "End of GetSetupInfo - " + msg2;		// TODO:
					}
				}
			} catch (Exception ex) {
				return "An unexpected error occurred in GetSetupInfo. Diagnostic info =\n"
					+ ex.Message;
			}
		}

//---------------------------------------------------------------------------------------

		private bool AddUserIDPassword(SqlConnection conn, string UserID, string Password,
						int EventID, out string msg2) {
			// TODO: Yeah, we should use sprocs here. But I'm in a hurry. We'll do it
			//		 ourselves.

			// TODO: Next field s/b enum
			int		TypeExhibitor = 5;

			// Set default out parm value(s)
			msg2 = "";

			string		SQL;
			SqlCommand	cmd;
			try {
				// First, we need to get the Reg Contractor ID for this event. It will be
				// used as an owner/creator of this Exhibitor account.
				// msg2 += "\nAbout to get EventRCID";
				SQL = "SELECT EventRCID FROM tblEvents WHERE EventID = @EventID";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@EventID", EventID);
				object objRCID = cmd.ExecuteScalar();
				// msg2 += string.Format("\nobjRCID={0}, type={1}", objRCID, objRCID.GetType());
				if (objRCID is DBNull) {
					msg2 = string.Format("\nUnable to find EventRCID for EventID={0}", EventID);
					return false;
				}
				int CreatorID = (int)objRCID;
				// msg2 += string.Format("\nGot CreatorID = {0}", CreatorID);	

				SQL = "INSERT INTO tblAccounts (UserID, Password, PasswordInPlainText,"
					+ " FirstName, LastName, emailAddress, Creator, AcctType, Enabled)"
					+ "\nVALUES (@UserID, @Password, @PasswordInPlainText,"
					+ " @FirstName, @LastName, @emailAddress, @Creator, @AcctType,"
					+ " @Enabled)";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@UserID", UserID);
				cmd.Parameters.AddWithValue("@Password", Password);
				cmd.Parameters.AddWithValue("@PasswordInPlainText", Password);
				cmd.Parameters.AddWithValue("@FirstName", "*Unknown*");
				cmd.Parameters.AddWithValue("@LastName", "*Unknown*");
				cmd.Parameters.AddWithValue("@emailAddress", "");
				cmd.Parameters.AddWithValue("@Creator", CreatorID);
				cmd.Parameters.AddWithValue("@AcctType", TypeExhibitor);
				cmd.Parameters.AddWithValue("@Enabled", false);
				int nRows = cmd.ExecuteNonQuery();
				if (nRows == -1) {
					msg2 += string.Format("\nUnable to insert UserID and Password into tblAccounts");
					return false;
				}

				// msg2 += "\nAbout to get AcctID";		
				int AcctID = StoredProcedures.GetIdent(conn, "tblAccounts");
				// msg2 += string.Format("\nGot AcctID = {0}", AcctID);

				// OK, we've got both the AcctID and EventID. Update two other tables.

				// msg2 += "\nAbout to INSERT into tblOwnerAcct_Account";
				SQL = "INSERT INTO tblOwnerAcct_Account (AcctOwnerID, AcctID)"
					+ "\nVALUES(@AcctOwnerID, @AcctID)";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@AcctOwnerID", CreatorID);
				cmd.Parameters.AddWithValue("@AcctID", AcctID);
				nRows = cmd.ExecuteNonQuery();
				if (nRows == -1) {
					msg2 += string.Format("\nUnable to insert account info into tblOwnerAcct_Account");
					return false;
				}

				// msg2 += "\nAbout to INSERT into tblExhib_Acct_Event";
				SQL	= "INSERT INTO tblExhib_Acct_Event (AcctID, EventID)"
					+ "\nVALUES(@AcctID, @EventID)";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@AcctID", AcctID);
				cmd.Parameters.AddWithValue("@EventID", EventID);
				nRows = cmd.ExecuteNonQuery();
				if (nRows == -1) {
					msg2 += string.Format("\nUnable to insert account info into tblExhib_Acct_Event");
					return false;
				}

				// Well, it seems that we have two more tables to set up. Here goes.

				// msg2 += "\nAbout to SELECT FROM tblCompanies";
				// msg2 += string.Format("\nCreatorID={0}, EventID={1}", CreatorID, EventID);
				SQL = "SELECT CompanyID FROM tblCompanies"
					+ "\nWHERE AcctOwnerID = @AcctOwnerID"
					+ "\n  AND EventID     = @EventID"
					+ "\n  AND CompanyName = @CompanyName";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@AcctOwnerID", CreatorID);
				cmd.Parameters.AddWithValue("@EventID", EventID);
				cmd.Parameters.AddWithValue("@CompanyName", "*Unknown*");
				// msg2 += string.Format("\nAbout to cmd.ExecuteScalar on tblCompanies");
				object	ID = cmd.ExecuteScalar();
				// msg2 += string.Format("\nBack from cmd.ExecuteScalar on tblCompanies. ID={0}", ID);
				int		CompanyID;
				if (ID == null) {
					// msg2 += "\nAbout to INSERT into tblCompanies";
					SQL = "INSERT INTO tblCompanies (AcctOwnerID, EventID, CompanyName)"
						+ "\nVALUES(@AcctOwnerID, @EventID, @CompanyName)";
					cmd = new SqlCommand(SQL, conn);
					cmd.Parameters.AddWithValue("@AcctOwnerID", CreatorID);
					cmd.Parameters.AddWithValue("@EventID", EventID);
					cmd.Parameters.AddWithValue("@CompanyName", "*Unknown*");
					nRows = cmd.ExecuteNonQuery();
					if (nRows == -1) {
						msg2 += string.Format("Unable to insert account info into tblCompanies");
						return false;
					}
					CompanyID = StoredProcedures.GetIdent(conn, "tblCompanies");
				} else {
					// msg2 += string.Format("\nAddUserIDPassword - CompanyID = {0}, type={1}", ID, ID.GetType());
					CompanyID = (int)ID;
				}

				// msg2 += "\nAbout to INSERT into tblAccountsExtended";
				SQL = "INSERT INTO tblAccountsExtended (AcctID, CompanyID, CreationDate)"
					+ "\nVALUES(@AcctID, @CompanyID, @CreationDate)";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@AcctID", AcctID);
				cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
				cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
				nRows = cmd.ExecuteNonQuery();
				if (nRows == -1) {
					msg2 += string.Format("\nUnable to insert account info into tblAccountsExtended");
					return false;
				}

				return true;			
			} catch (Exception ex) {
				msg2 += "\nUnexpected error in AddUserIDPassword - Diagnostic info:\n"
					 + ex.Message;
				return false;		
			}
		}

//---------------------------------------------------------------------------------------

		private int GetSetupFile(SqlConnection conn, string SetupCode, out int EventID,
						out int MapCfgID, out string SetupFileContents) {
			// Set output parms, in case we leave prematurely
			EventID			  = -1;
			MapCfgID		  = -1;
			SetupFileContents = "";

			string			SQL;
			SqlCommand		cmd;
			SqlDataReader	rdr;
			int				SetupID;

			SetupID = GetSetupAndEventIDs(conn, SetupCode, out EventID);

			// Now get the actual contents of the setup file
			SQL = "SELECT * FROM tblSetupFile WHERE ID = @SetupID";
			cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@SetupID", SetupID);
			rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
			rdr.Read();
			try {
				if (!rdr.HasRows) {
					return -1;
				}
				SetupFileContents = (string)rdr["SetupFileText"];

				object MapID = rdr["MapCfgID"];
				// throw new Exception(string.Format("GetSetupFile - MapID={0}, type={1}", MapID, MapID.GetType()));
				if (MapID is DBNull) {
					MapCfgID = -1;
				} else {
					MapCfgID = (int)MapID;
				}
			} finally {
				if (!rdr.IsClosed) {
					rdr.Close();
				}
			}
			return 0;
		}

//---------------------------------------------------------------------------------------

		private static int GetSetupAndEventIDs(SqlConnection conn, string SetupCode, out int EventID) {
			string			SQL;
			SqlCommand		cmd;
			SqlDataReader	rdr;
			int				SetupID;

			// Default out parm(s)
			EventID = -1;

			SQL = "SELECT * FROM tblEvent_SetupFile WHERE SetupCode = @SetupCode";
			cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@SetupCode", SetupCode);
			rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
			try {
				rdr.Read();
				if (!rdr.HasRows) {		// Check if not found
					return -1;
				}

				EventID = (int)rdr["EventID"];
				SetupID = (int)rdr["SetupID"];
			} finally {
				if (!rdr.IsClosed) {
					rdr.Close();
				}
			}
			return SetupID;
		}

//---------------------------------------------------------------------------------------

		private string GenerateRandomPwd(int pwdLength, int Seed) {
			// $$ Duplicate
			Random rGen = new Random(Seed);
			int p;
			string strPwd = "";
			string pwdChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@$?";
			int pwdCharsCount = pwdChars.Length;
			for (int i = 0; i < pwdLength; i++) {
				p = rGen.Next(0, pwdCharsCount - 1);
				strPwd += pwdChars[p];
			}
			return strPwd;
		}

//---------------------------------------------------------------------------------------

		public string GetSetupFile(string SetupID,
					int PacketSize, int PacketNum,
					out string Packet, out int OutputPacketLength,
					out int ErrorCode) {

			int		EventID, MapCfgID;
			string	SetupFileContents;
			string	msgs = "";

			// Set output values, in case we return early
			Packet = "";
			OutputPacketLength = 0;
			ErrorCode = -1;

			if (PacketSize <= 0) {
				msgs = string.Format("GetSetupFile - PacketSize of {0} must be > 0",
					PacketSize);
				return msgs;
			}

			if (PacketNum < 0) {
				msgs = string.Format("GetSetupFile - PacketNum of {0} must be >= 0",
					PacketNum);
				return msgs;
			}

			using (SqlConnection conn = new SqlConnection()) {
				SetConnectionString(conn);
				conn.Open();
				int ErrCode = GetSetupFile(conn, SetupID.ToUpper(), out EventID, out MapCfgID,
							out SetupFileContents);
				int Start = PacketSize * PacketNum;
				if (Start >= SetupFileContents.Length) {
					msgs = string.Format("Invalid Packet values - PacketSize={0}"
						 + ", PacketNum={1}, Start={2}",
						 PacketSize, PacketNum, Start);	
					return msgs;
				}
				int SizeLeft = SetupFileContents.Length - Start;
				PacketSize = Math.Min(SizeLeft, PacketSize);
				Packet = SetupFileContents.Substring(Start, PacketSize);
				OutputPacketLength = Packet.Length;
				ErrorCode = 0;
				return msgs;
			}
		}

//---------------------------------------------------------------------------------------

		public string GetMapFileID(string SetupName, string MapCfgFileContents,
				out int MapCfgID,
				out int ErrorCode) {
			// Set default output parms
			MapCfgID  = -1;
			ErrorCode = -1;

			string	msg = "";

			BartCRC	bc = new BartCRC();
			bc.AddData(MapCfgFileContents);
			int		CRC = unchecked((int)bc.GetCRC());

			using (SqlConnection conn = new SqlConnection()) {
				SetConnectionString(conn);
				conn.Open();

				string SQL  = "SELECT * FROM tblMapCfg"
							+ " WHERE MapCfgCRC = @MapCfgCRC";
				SqlCommand	cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@MapCfgCRC", CRC);
				SqlDataReader	rdr = cmd.ExecuteReader();
				string	CfgData;
				try {
					while (rdr.Read()) {
						CfgData = (string)rdr["MapCfgContents"];
						if (string.Compare(CfgData, MapCfgFileContents, true) == 0) {
							MapCfgID = (int)rdr["MapCfgID"];
							// We've found our MapCfgID. But there's a problem. Suppose
							// this Map.Cfg file had already been uploaded (maybe even
							// for a different event). We'd find it in tblMapCfg, but
							// we'd also need to update tblSetupFile with the MapCfgID
							// for the specified SetupName. So do that now.
							// msg += "\nAbout to call UpdateTblSetupFile - 1";	
							rdr.Close();		// Note: We seem to have to close the rdr
												//		 here, else we get a "Reader 
												//		 already open on this Command."
												//		 error. But, uh, inside the
												//		 UpdateTblSetupFile, it allocates
												//		 its own SQLCommand object. Oh
												//		 well, close ours, if it'll make
												//		 the Framework happy.
							UpdateTblSetupFile(conn, SetupName, MapCfgID, out ErrorCode, ref msg);
							return "";
						}
					}
				} finally {
					if (!rdr.IsClosed) {
						rdr.Close();
					}
				}

				// We got here without a match (possibly since this is the first time
				// we've seen this Map.Cfg file). Add it, and also update the Setup
				// file table.

				SQL = "INSERT INTO tblMapCfg(MapCfgContents, MapCfgCRC)"
					+ " VALUES(@MapCfgContents, @MapCfgCRC)";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@MapCfgContents", MapCfgFileContents);
				cmd.Parameters.AddWithValue("@MapCfgCRC", CRC);
				int rc = cmd.ExecuteNonQuery();
				if (rc == -1) {
					ErrorCode = -1;
					return "Could not add to tblMapCfg";
				}

				// msg += string.Format("\nAbout to get MapCfgID");
				MapCfgID = StoredProcedures.GetIdent(conn, "tblMapCfg");
				// msg += string.Format("\nMapCfgID = {0}", MapCfgID);

				// msg += "\nAbout to call UpdateTblSetupFile - 2";	// TODO:
				UpdateTblSetupFile(conn, SetupName, MapCfgID, out ErrorCode, ref msg);
			}
			return msg;
		}

//---------------------------------------------------------------------------------------

		private static void UpdateTblSetupFile(SqlConnection conn, string SetupName, int MapCfgID, out int ErrorCode, ref string msg) {
			ErrorCode = -1;

			int EventID;	// Not needed here, but returned by GetSetupAndEventIDs
			int SetupID = GetSetupAndEventIDs(conn, SetupName, out EventID);
			// msg += string.Format("\nSetupID = {0}", SetupID);
			string SQL = "SELECT MapCfgID FROM tblSetupFile WHERE ID = @SetupID";
			SqlCommand cmd = new SqlCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@SetupID", SetupID);
			object ID = cmd.ExecuteScalar();
			if (ID is DBNull) {
				// msg += string.Format("\nID is DBNull - value = {0}", ID);
				SQL = "UPDATE tblSetupFile"
					+ " SET MapCfgID=@MapCfgID"
					+ " WHERE ID = @SetupID";
				cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@SetupID", SetupID);
				cmd.Parameters.AddWithValue("@MapCfgID", MapCfgID);
				int nRows = cmd.ExecuteNonQuery();
				// msg += string.Format("UPDATE tblSetupFile, MapCfgID={0}, SetupID={1}, nRows={2}",
				// 	MapCfgID, SetupID, nRows);
				if (nRows == -1) {
					msg += "Unable to add MapCfgID to tblSetupFile";
				} else {
					ErrorCode = 0;
				}
			} else {
				// msg += string.Format("\nID is **not** DBNull - value = {0}, type = {1}", ID, ID.GetType());
				int IDFromTable = (int)ID;
				if (IDFromTable != MapCfgID) {
					ErrorCode = -1;
					msg += string.Format("\nWarning - Internal issue -"
						+ " MapCfgID in tblMapCfg ({0}) not consistent"
						+ " with value in tblSetupFile({1})"
						+ " for SetupID {2}",
							MapCfgID, IDFromTable, SetupID);
				}
				// else { msg += "\nIDs match"; }				
				// Else MapCfgID matches. All's well.
				ErrorCode = 0;
			}
		}

//---------------------------------------------------------------------------------------

		private void SetConnectionString(SqlConnection conn) {
#if CALL_SQL_DIRECTLY
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			// string prefix;
#if LOCAL
			builder.DataSource = "(local)";
			builder.IntegratedSecurity = true;
			builder.InitialCatalog = "LLDevel";
			// prefix = "";
			conn.ConnectionString = builder.ConnectionString;
#elif DEVEL
			builder.DataSource = "SQLB5.webcontrolcenter.com";
			builder.InitialCatalog = "LLDevel";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
			// prefix = "ahmed.";
			conn.ConnectionString = builder.ConnectionString;
#elif PROD
#warning You really should use DEVEL or LOCAL
			builder.DataSource = "SQLB2.webcontrolcenter.com";
			builder.InitialCatalog = "LeadsLightning";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
			// prefix = "ahmed.";
			conn.ConnectionString = builder.ConnectionString;
#else
#error Must choose one of LOCAL / DEVEL / PROD
#endif
#else
 			conn.ConnectionString = "context connection=true";
#endif
		}
	}
}
