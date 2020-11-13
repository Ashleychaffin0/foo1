// Copyright (c) 2020 by Larry Smith
//

using System;
using System.IO;
using Microsoft.Win32;

// g:\OneDrive\$Dev\C#\ZuneGenerateClasses\ZuneGenerateClasses
// c:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Tools\MSVC\14.27.29110\bin\Hostx86\x86

namespace ZuneTest02 {
	class ZuneUtils {
		//  		bool bGotZuneDbDir			= false;
		// 		string _ZuneInstallationDir = null;

//---------------------------------------------------------------------------------------

		public static string ZuneDbDir {
			get {
				string user = Environment.GetEnvironmentVariable("USERPROFILE");
				return Path.Combine(user, @"Appdata\Local\Microsoft\Zune");
			}
		}

//---------------------------------------------------------------------------------------

		public static string ZuneInstallationDir {
			get {
				using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)) {
					using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))  {
						// key now points to the 64-bit key
						string ZuneNode = @"SOFTWARE\Microsoft\Zune";
						var zkey = hklm.OpenSubKey(ZuneNode);
						return (string)zkey.GetValue("Installation Directory");
					}
				}
			}
		}
	}

}
