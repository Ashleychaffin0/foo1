// Copyright (c) 2007 by Bartizan Connects, LLC

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Bartizan.ccLeadsWorking {
	/// <summary>
	/// This class does double duty, both for Sessions and Access Control. For a session,
	/// this contains the name of the session and a list of Status's, which will be only
	/// "Yes".
	/// <para>
	/// For Access Control, the Session Name is the same, but the Status will be either
	/// "Granted" or "Denied".
	/// </para>
	/// </summary>
	[Serializable]
	public class Session {
		public string		SessionName;
		public List<string> Status;
		public SessionType	SessType;

		private const string NoSession = "Terminal is not set for any session";
		private const string NoAccessControl = "Terminal not set for Access Control";
		private const string MultiSessions = "multiple sessions setup: ";
		private const string MultiGranted = "Access Granted for multiple sessions setup: ";
		private const string MultiDenied = "Access Denied for multiple sessions setup: ";

		private const string Granted = "Access Granted for ";
		private const string Denied = "Access Denied for ";

		// Other pieces of the code may need to key off whether we set status to be, say,
		// Yes vs Y, Granted vs OK, Denied vs Rejected, etc. So set up some public static
		// constants with the values we use.
		public static string StatYes = "Yes";
		public static string StatGranted = "Granted";
		public static string StatDenied = "Denied";

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public enum SessionType {
			Session,
			AccessControl
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public Session() {
			SessionName = "N/A";
			Status = new List<string>();
			SessType = SessionType.Session;
		}

//---------------------------------------------------------------------------------------

		public Session(string SessionName, string Stat, SessionType type) {
			this.SessionName = SessionName;
			Status = new List<string>();
			this.SessType = type;
			Status.Add(Stat);
		}

//---------------------------------------------------------------------------------------

		public static List<Session> GetSessionList(string SessionName) {
			List<Session> list = new List<Session>();

			string Name = SessionName.Trim();
			string ucName = Name.ToUpper();

			// If it's empty, or says explicitly that there's no session, we're done
			if ((ucName.Length == 0) || (ucName == NoSession.ToUpper())) {
				list.Add(new Session("*No Session*", StatYes, SessionType.Session));
				return list;
			}

			// See if it says explicitly that there's no session. If so, we're also done
			if (ucName == NoAccessControl.ToUpper()) {
				list.Add(new Session("*Access Control Disabled*", StatYes, SessionType.Session));
				return list;
			}

			// We don't currently support multiple sessions. So check for a multisession
			// prefix and if so, ignore it.
			if ((ucName.StartsWith(MultiSessions.ToUpper()))
				|| (ucName.StartsWith(MultiGranted.ToUpper()))
				|| (ucName.StartsWith(MultiDenied.ToUpper()))) {
				return list;				// Empty list
			}

			// OK, we've got something we can process. But is the status "Yes" (for 
			// a Session name, or "Granted" / "Denied" for Access Control
			string stat = StatYes;					// Assume simple session
			SessionType type = SessionType.Session;

			// Check for Access Control Granted
			if (ucName.StartsWith(Granted.ToUpper())) {
				Name = Name.Substring(Granted.Length);
				stat = StatGranted;
				type = SessionType.AccessControl;
			}

			// Check for Access Control Denied
			if (ucName.StartsWith(Denied.ToUpper())) {
				Name = Name.Substring(Denied.Length);
				stat = StatDenied;
				type = SessionType.AccessControl;
			}

			// OK, we've got a session name!
			list.Add(new Session(Name, stat, type));
			return list;
		}
	}
}
