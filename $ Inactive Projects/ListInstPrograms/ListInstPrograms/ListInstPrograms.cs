using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;

namespace ListInstPrograms {
	/// <summary>
	/// Summary description for ListInstPrograms.
	/// </summary>
	class ListInstPrograms {

		List<Program>	Programs = new List<Program>();

		public class Program : IComparable {
			public string	_DisplayName,
							_DisplayVersion,
							_InstallSource,
							_Publisher;

			public int		EstimatedSize = 0;

			public string	RegKey;

			public string Publisher {
				get { return _Publisher; }
				set { _Publisher = value == null ? "" : value.Replace("\"", "'"); }
			}

			public string InstallSource {
				get { return _InstallSource; }
				set { _InstallSource = value == null ? "" : value.Replace("\"", "'"); }
			}

			public string DisplayVersion {
				get { return _DisplayVersion; }
				set { _DisplayVersion = value == null ? "" : value.Replace("\"", "'"); }
			}

			public string DisplayName {
				get { return _DisplayName; }
				set { _DisplayName = value == null ? "" : value.Replace("\"", "'"); }
			}

//---------------------------------------------------------------------------------------

			public Program() {
				_DisplayName	= "";
				_DisplayVersion = "";
				_InstallSource	= "";
				_Publisher		= "";
				EstimatedSize	= 0;
				RegKey			= "";
			}

//---------------------------------------------------------------------------------------

			public void Print() {
				StringBuilder	sb = new StringBuilder();
				sb.Append("\n");

				sb.Append("\"") ;sb.Append(DisplayName);sb.Append("\"");
				sb.Append(",\"");sb.Append(DisplayVersion);sb.Append("\"");
				sb.Append(",\"");sb.Append(InstallSource);sb.Append("\"");
				sb.Append(",\"");sb.Append(Publisher);sb.Append("\"");

				sb.Append(",\"");sb.Append(EstimatedSize.ToString("D"));sb.Append("\"");

				sb.Append(",\"");sb.Append(RegKey);sb.Append("\"");

				Console.WriteLine("{0}", sb.ToString());
			}

//---------------------------------------------------------------------------------------

			public static void DisplayHeader() {
				string	hdr = "'Name','Version','Install Source','Publisher','Estimated Size','RegKey'";
				hdr = hdr.Replace('\'', '"');
				Console.WriteLine(hdr);
			}


//---------------------------------------------------------------------------------------

			#region IComparable Members

			public int CompareTo(object obj) {
				Program	p2 = obj as Program;
				return string.Compare(DisplayName, p2.DisplayName, true);
			}

			#endregion
		}

//---------------------------------------------------------------------------------------

		void Run() {
			Program.DisplayHeader();

			// GetInstallerPrograms();
			GetUninstallerPrograms();

			Programs.Sort();

			foreach (Program pgm in Programs) {
				pgm.Print();
			}
		}

//---------------------------------------------------------------------------------------

#if false
		private void GetInstallerPrograms() {
			RegistryKey CurKey;
			string appsbase = @"Software\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products";
			RegistryKey apps = Registry.LocalMachine.OpenSubKey(appsbase, false);
			string[] subkeys = apps.GetSubKeyNames();
			foreach (string key in subkeys) {
				CurKey = apps.OpenSubKey(key + "\\InstallProperties");
				if (CurKey != null) {
					AddInstallerProgram(CurKey);
				}
			}
		}
#endif
//---------------------------------------------------------------------------------------

		private void GetUninstallerPrograms() {
			RegistryKey CurKey;
			string appsbase = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";
			// Note: There was another key we were originally processing,
			//	@"Software\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products";
			// However, this missed a number of apps. I suspect that this key contains
			// only those apps installed using the Microsoft Installer (.msi). But I
			// assume (haven't run Regmon yet) that this is the right one to process.
			RegistryKey apps = Registry.LocalMachine.OpenSubKey(appsbase, false);
			string[] subkeys = apps.GetSubKeyNames();
			foreach (string key in subkeys) {
				CurKey = apps.OpenSubKey(key);
				if ((CurKey != null) && (! key.StartsWith("{"))) {
					AddUninstallerProgram(CurKey);
				}
			}
		}

//---------------------------------------------------------------------------------------
	
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			ListInstPrograms	lip = new ListInstPrograms();
			lip.Run();
		}

//---------------------------------------------------------------------------------------

#if false
		private void AddInstallerProgram(RegistryKey CurKey) {
			Program		pgm = new Program();

			pgm.DisplayName = (string)CurKey.GetValue("DisplayName");
			pgm.DisplayVersion = (string)CurKey.GetValue("DisplayVersion");
			pgm.InstallSource = (string)CurKey.GetValue("InstallSource");
			pgm.Publisher = (string)CurKey.GetValue("Publisher");

			object		o = CurKey.GetValue("EstimatedSize");
			if (o != null)
				pgm.EstimatedSize = (int)o;

			Programs.Add(pgm);
		}
#endif

//---------------------------------------------------------------------------------------

		private void AddUninstallerProgram(RegistryKey CurKey) {
			Program		pgm = new Program();
			string		KeyName = CurKey.Name;

			pgm.DisplayName = (string)CurKey.GetValue("DisplayName");
			if (pgm.DisplayName.StartsWith("{")) {
				return;
			}
			pgm.DisplayVersion = (string)CurKey.GetValue("DisplayVersion");
			pgm.InstallSource = (string)CurKey.GetValue("InstallSource");
			pgm.Publisher = (string)CurKey.GetValue("Publisher");

			object		o = CurKey.GetValue("EstimatedSize");
			if (o != null)
				pgm.EstimatedSize = (int)o;

			pgm.RegKey = KeyName;

			if ((pgm.DisplayName != "") || (pgm.DisplayVersion != "")) {
				Programs.Add(pgm);
			}
		}
	}
}
