using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Management;

namespace Bartizan.Utils {

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class BartSysInfo {

//---------------------------------------------------------------------------------------

		public static TimeSpan GetUptime() {
			return new TimeSpan(0, 0, 0, 0, Environment.TickCount);
		}

//---------------------------------------------------------------------------------------

		public static DateTime GetBootTime() {
			return DateTime.Now - GetUptime();
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public enum DriveType : uint {
			DRIVE_UNKNOWN		= 0,		// Unknown drive type 
			DRIVE_NO_ROOT_DIR	= 1,		// Invalid root path was given to the fn
			DRIVE_REMOVABLE		= 2,		// Removeable drive like a floppy or USB key
			DRIVE_FIXED			= 3,		// A fixed drive like a hard disk 
			DRIVE_REMOTE		= 4,		// A network drive 
			DRIVE_CDROM			= 5,		// A cd-rom drive 
			DRIVE_RAMDISK		= 6,		// A ram disk 
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class Bart_WMI_LogicalDisk {
		string drive;
		ManagementObjectCollection queryCollection;

//---------------------------------------------------------------------------------------

		public Bart_WMI_LogicalDisk(string drive) {
			this.drive = drive ?? "";		// Two chars, such as "C:"
			// TODO: Should check length and throw an exception if <> 2
		}

//---------------------------------------------------------------------------------------

		public Bart_WMI_LogicalDisk() {
			this.drive = "";
		}

//---------------------------------------------------------------------------------------

		public ManagementObjectCollection GetInfo(string WhereClause) {
			// e.g. WhereClause = "DriveType=3", or empty, or null
			// e.g. WhereClause = "DeviceID='K'"
			string SQL = "SELECT * From Win32_LogicalDisk";

			string where = "";
			string Clause = WhereClause ?? "";
			string WhereDrive = drive;
			if ((Clause.Length > 0) || (drive.Length > 0)) {
				// OK, we need a Where clause
				if (drive.Length > 0) {
					WhereDrive = "DeviceID='" + drive + "'";
				}
				if ((WhereDrive.Length > 0) && (Clause.Length > 0)) {
					where = WhereDrive + " AND " + Clause;
				} else {
					where = WhereDrive + Clause;
				}
				SQL += " WHERE " + where;
			}
			ManagementObjectSearcher query = new ManagementObjectSearcher(SQL);
			queryCollection = query.Get();
			return queryCollection;
		}

//---------------------------------------------------------------------------------------

		public ManagementObjectCollection GetDriveInfo(string drive) {
			if (drive.EndsWith("\\")) {
				drive = drive.Substring(0, drive.Length - 1);
			}
			return GetInfo("DeviceID='" + drive + "'");
		}

//---------------------------------------------------------------------------------------

		public ManagementObjectCollection GetInfo() {
			return GetInfo("");
		}

//---------------------------------------------------------------------------------------

		// Specialized accessors for the most common properties

		public int Count {
			get { return queryCollection.Count; }
		}

//---------------------------------------------------------------------------------------

		public string[] FileSystem {
			get {
				string[] FileSyss = new string[Count];
				int n = 0;
				foreach (ManagementObject mo in queryCollection) {
					FileSyss[n++] = (string)mo["FileSystem"];
				}
				return FileSyss;
			}
		}

//---------------------------------------------------------------------------------------

		public string[] ProviderName {
			get {
				string[] Providers = new string[Count];
				int n = 0;
				foreach (ManagementObject mo in queryCollection) {
					Providers[n++] = (string)mo["ProviderName"];
				}
				return Providers;
			}
		}

//---------------------------------------------------------------------------------------

		public string[] VolumeSerialNumber {
			get {
				string[] Volsers = new string[Count];
				int n = 0;
				foreach (ManagementObject mo in queryCollection) {
					Volsers[n++] = (string)mo["VolumeSerialNumber"];
				}
				return Volsers;
			}
		}

//---------------------------------------------------------------------------------------

		public uint[] GetDriveType {
			get {
				uint[] Types = new uint[Count];
				int n = 0;
				foreach (ManagementObject mo in queryCollection) {
					Types[n++] = (uint)mo["DriveType"];
				}
				return Types;
			}
		}

//---------------------------------------------------------------------------------------

		public string[] GetDriveTypeName {
			get {
				string[] TypeNames = {"Unknown", "Invalid Device Name", "Removable",
						"Fixed", "Remote", "CDROM", "RAMDISK"};
				string[] Types = new string[Count];
				int n = 0;
				foreach (ManagementObject mo in queryCollection) {
					uint type = (uint)mo["DriveType"];
					if (type >= TypeNames.Length) {
						Types[n++] = string.Format("Unknown name, type={0}", type);
					} else {
						Types[n++] = TypeNames[type];
					}
				}
				return Types;
			}
		}

//---------------------------------------------------------------------------------------

		public string[] this[string name] {
			get {
				string[] Props = new string[Count];
				int n = 0;
				foreach (ManagementObject mo in queryCollection) {
					object o = mo[name];
					Props[n++] = (o ?? "0").ToString();
				}
				return Props;
			}
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class BartNTP {
		string			_TimeServerName;
		int				_Port;				
		int				_TimeoutInMilliseconds;

		public static int DefaultPort = 37;		// RFC-868 uses port 37

		private bool	_bOK;
		private string	_ErrorMessage;

//---------------------------------------------------------------------------------------

		public string TimeServerName {
			get { return _TimeServerName; }
			set { _TimeServerName = value; }
		}

//---------------------------------------------------------------------------------------

		public int Port {
			get { return _Port; }
			set { _Port = value; }
		}

//---------------------------------------------------------------------------------------

		public int TimeoutInMilliseconds {
			get { return _TimeoutInMilliseconds; }
			set { _TimeoutInMilliseconds = value; }
		}

//---------------------------------------------------------------------------------------

		public bool bOK {
			get { return _bOK; }
			private set { _bOK = value; }
		}

//---------------------------------------------------------------------------------------

		public string ErrorMessage {
			get { return _ErrorMessage; }
			private set { _ErrorMessage = value; }
		}

//---------------------------------------------------------------------------------------

		public BartNTP(string TimeServerName, int Port, int TimeoutInMilliseconds) {
			this.TimeServerName = TimeServerName;
			this.Port = Port;
			this.TimeoutInMilliseconds = TimeoutInMilliseconds;

			bOK = false;
			ErrorMessage = null;
		}

//---------------------------------------------------------------------------------------

		public BartNTP()
			: this("time.nist.gov", DefaultPort, 0) {
		}

//---------------------------------------------------------------------------------------

		public DateTime GetNtpTod() {		// Get NTP TimeOfDay as local time. If the
											// user needs GMT, call .ToUniversalTime()
			ErrorMessage = null;			// Assume no errors
			TcpClient client	= new TcpClient();
			NetworkStream strm	= null;
			try {
				byte[] buf = new byte[4];	// The result is 4 bytes, unsigned (despite
											// what RFC-868 says). It's an offset, the 
											// number of seconds from midnight, 
											// Jan 1, 1900.
				client.Connect(TimeServerName, Port);
				if (!client.Connected) {
					bOK = false;
					ErrorMessage = "Could not connect to " + TimeServerName;
					return default(DateTime);
				}

				strm = client.GetStream();
				strm.Read(buf, 0, 4);

				// OK, we've got the data, but it's in Big Endian format. Convert it to our
				// Little Endian format, then convert to long (which is needed by TimeSpan).
				int TimeOffsetBigEndian = BitConverter.ToInt32(buf, 0);

				// First, a quick sanity check. We may have timed out and got nothing back.
				// So check for an offset of zero.
				if (TimeOffsetBigEndian == 0) {
					bOK = false;
					ErrorMessage = "Got 0 as a response from the time server";
					return default(DateTime);
				}

				int TimeOffsetLittleEndian = IPAddress.NetworkToHostOrder(TimeOffsetBigEndian);

				// Convert to long, even if the high-order bit is on.
				long TimeOffset = unchecked((uint)TimeOffsetLittleEndian);

				// We have an offset in number of seconds. But TimeSpan needs the number of
				// 100-nanosecond intervals.
				long SecsTo100NSecFactor = 1000L * 1000L * 1000L / 100L;
				TimeSpan ts  = new TimeSpan(TimeOffset * SecsTo100NSecFactor);
				DateTime TOD = (new DateTime(1900, 1, 1)) + ts;

				bOK = true;
				return TOD.ToLocalTime();
			} catch (Exception ex) {
				bOK = false;
				ErrorMessage = "Exception thrown - " + ex.Message;
				return default(DateTime);
			} finally {
				if (strm != null) {
					strm.Close();
				}
				if (client.Connected) {
					 client.Close();
				}
			}
		}
	}
}
