<%@ WebService Language="C#" Class="LLImporter" %>

// Copyright (c) 2006 Bartizan Connects LLC

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.Data;
using System.Data.SqlClient;

[WebService(Namespace = "http://www.LeadsLightning.com/LeadsLightningWS")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class LLImporter : System.Web.Services.WebService {
	string		ConnMsgs;

//---------------------------------------------------------------------------------------

	public LLImporter() {
		//Uncomment the following line if using designed components 
		//InitializeComponent(); 

		ConnMsgs = "";
	}

//---------------------------------------------------------------------------------------

	[WebMethod]
	public string Import(string UserId, string Password, string EventName, string SwipeData, string MapCfgFile, int MapType, string TerminalId) {
#if true
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			builder.DataSource = "SQLB2.webcontrolcenter.com";
			builder.InitialCatalog = "LeadsLightning";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";

		using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
			try {
				conn.Open();
			} catch (Exception ex) {
				return "Connection Open Exception - " + ex.Message;
			}
			conn.InfoMessage += new SqlInfoMessageEventHandler(conn_InfoMessage);

			// TODO: For now, kludge things so that we make sure the
			//		 sproc we're about to call uses nvarchar(MAX).
			SqlCommand cmdKludge = new SqlCommand("ahmed.LL_sp_Kludge1", conn);
			cmdKludge.CommandType = CommandType.StoredProcedure;
			int nKludge = cmdKludge.ExecuteNonQuery();

			SqlCommand cmd = new SqlCommand("ahmed.LL_sp_LLImport", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@UserID", UserId));
			cmd.Parameters.Add(new SqlParameter("@Password", Password));
			cmd.Parameters.Add(new SqlParameter("@EventName", EventName));
			cmd.Parameters.Add(new SqlParameter("@SwipeData", SwipeData));
			cmd.Parameters.Add(new SqlParameter("@MapCfgFile", MapCfgFile));
			cmd.Parameters.Add(new SqlParameter("@MapType", MapType));
			cmd.Parameters.Add(new SqlParameter("@TerminalID", TerminalId));
			// cmd.Parameters.Add(new SqlParameter("@Results", Results));
			cmd.CommandTimeout = 0;			// TODO:
			try {
				int n = cmd.ExecuteNonQuery();
			} catch (Exception ex) {
				ConnMsgs += "Exception while importing - " + ex.Message + "\n";
			} finally {
			}
			conn.Close();
		}
		if (ConnMsgs.Length == 0) {
			ConnMsgs = "Import Completed";
		}
		return ConnMsgs;
#else
		return "foo";
#endif
	}

//---------------------------------------------------------------------------------------

	void conn_InfoMessage(object sender, SqlInfoMessageEventArgs e) {
		string[] msgs = e.Message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		ConnMsgs += "***\n";
		foreach (string msg in msgs) {
			// lbMsgs.Items.Add(string.Format(fmt, msg, e.Source));	
			ConnMsgs += msg + "\n";
		}
	}

}
