// Copyright (c) 2006 Bartizan Connects LLC

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Win32;

using Bartizan.Utils.AssemblyUtils;
using Bartizan.Utils.CRC;

namespace Bartizan.BartSecure {
	internal class DiskVolumeInfo {

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		extern static bool GetVolumeInformation(
		  string RootPathName,
		  StringBuilder VolumeNameBuffer,
		  int VolumeNameSize,
		  out uint VolumeSerialNumber,
		  out uint MaximumComponentLength,
		  out uint FileSystemFlags,
		  StringBuilder FileSystemNameBuffer,
		  int nFileSystemNameSize);

//---------------------------------------------------------------------------------------

		uint _VolumeSerialNumber;
		uint _MaximumComponentLength;
		uint _FileSystemFlags;
		bool _OK;

//---------------------------------------------------------------------------------------

		public DiskVolumeInfo(string RootPathName) {
			StringBuilder vnb = new StringBuilder(256);
			StringBuilder fsnb = new StringBuilder(256);

			_OK = GetVolumeInformation(RootPathName,
				vnb, vnb.Capacity,
				out _VolumeSerialNumber,
				out _MaximumComponentLength,
				out _FileSystemFlags,
				fsnb, fsnb.Capacity);
		}

//---------------------------------------------------------------------------------------

		public bool OK {
			get { return _OK; }
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
	}




//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	public static class ProductActivation {

		public const string BADGEMAX_SUITE_NAME = "BadgeMax";
		public const string BADGEMAX_DESIGNER_PRODUCTNAME = "BadgeMax Designer";
		public const string BADGEMAX_RUNTIME_PRODUCTNAME = "BadgeMax Runtime";
		public const string BADGEMAX_CONNECTS_PRODUCTNAME = "BadgeMax Connects";

		public const string BARTBLOCK_SUITE_NAME = "BartBlock";
		public const string BARTBLOCK_PRODUCTNAME = "BartBlock";


//---------------------------------------------------------------------------------------

// This function returns null if it failed to get the activation code for any reason
		private static string GetActivationCode(string Key, string FieldName) {
			RegistryKey myKey;
			myKey = Registry.LocalMachine.OpenSubKey(Key);

			if (myKey == null)
				return null;

			string acStr = (string)myKey.GetValue(FieldName);
			if (acStr == null)
				return null;
			return acStr;
		}

//---------------------------------------------------------------------------------------

// Returns true if Activation (CRC matches) succeeded, false if not.
		public static bool CheckActivationCode(string VolumeName, string SubKey, string KeyName) {
#if BYPASS_ACTIVATION	// Temporarily bypass security (sigh).
			return true;
#else
			string crcInRegistry = GetActivationCode(SubKey, KeyName);
			if (crcInRegistry == null)
				return false;

			BartCRC crc = new BartCRC();
			DiskVolumeInfo si = new DiskVolumeInfo(VolumeName);
			crc.AddData(si.VolumeSerialNumber.ToString("X8"));
			uint crcCalculated = crc.GetCRC();
			return crcCalculated.ToString() == crcInRegistry;
#endif
		}

//---------------------------------------------------------------------------------------

		public static bool VerifyActivationCode(string SuiteName, string ProductName, string VolumeName) {
			Assembly asm = Assembly.GetEntryAssembly();
			Version ver = AssemblyUtils.GetVersionFromAssembly(asm);
			string fmt = @"SOFTWARE\Bartizan\{0}\{1}.{2}\{3}";
			string key = string.Format(fmt, SuiteName, ver.Major, ver.Minor, ProductName);
			return CheckActivationCode(VolumeName, key, "ActivationCode");
		}

#if false
//---------------------------------------------------------------------------------------

		public bool IsBadgeMaxRTActivated() {
			return VerifyCRC(BADGEMAX_RT_SUBKEY, BADGEMAX_KEYVALUE);
		}

//---------------------------------------------------------------------------------------

		public bool IsBadgeMaxDesignerActivated() {
			return VerifyCRC(BADGEMAX_DESIGNER_SUBKEY, BADGEMAX_KEYVALUE);
		}

//---------------------------------------------------------------------------------------

		public bool IsBadgeMaxConnectsActivated() {
			return VerifyCRC(BADGEMAX_CONNECTS_SUBKEY, BADGEMAX_KEYVALUE);
		}
#endif
	}
}
