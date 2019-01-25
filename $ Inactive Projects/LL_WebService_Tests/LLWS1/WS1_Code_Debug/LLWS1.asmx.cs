// Copyright (c) 2006 by Bartizan Connects, LLC

using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Bartizan.LL.Importer;
using Bartizan.LL.RealTimer;

// namespace LLWS1 {
	/// <summary>
	/// Summary description for LLWS1
	/// </summary>
	[WebService(Namespace = "http://www.LeadsLightning.com/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	public class LLWS1 : System.Web.Services.WebService {

//---------------------------------------------------------------------------------------

		[WebMethod]
		public string Import(string UserID, string Password, int EventID,
						string SwipeData, string MapCfgFile, int MapType, 
						string TerminalID, bool IgnoreFirstRecord,
						bool DataIsExpanded,
						out int ErrorCode) {
			string	res;
			StoredProcedures	sp = new StoredProcedures();
			res = sp.LL_sp_LLImport(UserID, Password, EventID, SwipeData, MapCfgFile,
				MapType, TerminalID, IgnoreFirstRecord, DataIsExpanded, out ErrorCode);
			return res;
		}

//---------------------------------------------------------------------------------------

		[WebMethod]
		public string GetSetupInfo(string SetupID, 
					out string UserID, out string Password,
					out int SetupFileLength,
					out int MapCfgID,
					out int EventID, 
					out int ErrorCode) {
			LL_sp_SetupFileRoutines	set = new LL_sp_SetupFileRoutines();
			string	msg;
			msg = set.GetSetupInfo(SetupID, out UserID, out Password,
					out SetupFileLength,
					out MapCfgID,
					out EventID, 
					out ErrorCode);
			return msg;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a SetupID, lets the caller (e.g. a RealTimer) download the setup file
		/// for the event (the EventID is implicit in the SetupID). Note that since the
		/// setup file is large, and wireless communication can be flaky, we support
		/// downloading the file in chunks.
		/// </summary>
		/// <param name="SetupID">Our (currently) 9-character, base 36 ID.</param>
		/// <param name="PacketSize">The size of the packet, e.g. 2048.</param>
		/// <param name="PacketNum">0 = first (say) 2K, 1 = second 2K, etc.</param>
		/// <param name="Packet">The actual data.</param>
		/// <param name="OutputPacketLength">The size of the data returned. For all
		/// but the last packet, this would be equal to the packet size.</param>
		/// <param name="ErrorCode">Really, this should be an enum, but until we
		/// get SourceSafe working, we're just going to use const int's all over
		/// the place. Sigh.</param>
		/// <returns>Any messages (especially errors) that could/should be
		/// displayed to the user.</returns>
		[WebMethod]
		public string GetSetupFile(string SetupID, 
					int PacketSize, int PacketNum,
					out string Packet, out int OutputPacketLength,
					out int ErrorCode) {
			LL_sp_SetupFileRoutines	set = new LL_sp_SetupFileRoutines();
			string	msg;
			msg = set.GetSetupFile(SetupID, PacketSize, PacketNum,
				out Packet, out OutputPacketLength, out ErrorCode);
			return msg;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// We don't want the RealTimer to have to send the entire ASCII Map.Cfg file
		/// on every swipe. Now ideally, when the Setup file is uploaded to LL, we'd
		/// create the corresponding Map.Cfg file then and there. Unfortunately, the
		/// code to do this is written in C++ not C#. So for expediency, the conversion
		/// code lives in the RealTimer, and it is its responsibility to send the
		/// file back up to the server.
		/// </summary>
		/// <param name="MapCfgFileContents">The file, in ASCII.</param>
		/// <param name="MapCfgID">The ID of the Map file, which can subsequently be 
		/// passed to the Importer.</param>
		/// <param name="ErrorCode"></param>
		/// <returns>Any messages (error or otherwise).</returns>
		[WebMethod]
		public string GetMapFileID(string SetupID, string MapCfgFile,
						out int MapCfgID,
						out int ErrorCode) {
			LL_sp_SetupFileRoutines	set = new LL_sp_SetupFileRoutines();
			string	msg;
			msg = set.GetMapFileID(SetupID, MapCfgFile, out MapCfgID, out ErrorCode);
			return msg;
		}
	}
//}
