using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PortableDeviceApiLib;

// Note: See https://archive.codeplex.com/?p=portabledevicelib for other classes
// Note: See https://stackoverflow.com/questions/6553290/how-do-i-create-an-instance-of-iportabledevicemanager


namespace LRS.Utils {
	class PdInfo {		// Portable Device
		public string	ID			 { get; private set; }
		public string	FriendlyName { get; private set; }
		public string	Description	 { get; private set; }
		public string	Manufacturer { get; private set; }
		public bool		IsPrivate	 { get; private set; }

		private PortableDeviceManager mgr;

//---------------------------------------------------------------------------------------

		public PdInfo(PortableDeviceManager mgr, string id, bool isPrivate) {
			this.mgr     = mgr;
			ID           = id;
			FriendlyName = GetFriendlyName(id);
			Description  = GetDescription(id);
			Manufacturer = GetManufacturer(id);
			IsPrivate    = IsPrivate;
		}

//---------------------------------------------------------------------------------------

		public static IEnumerable<PdInfo> Pds() {
			var mgr = new PortableDeviceManager();
			mgr.RefreshDeviceList();
			foreach (var ID in GetDeviceIDs(mgr, false)) {
				yield return new PdInfo(mgr, ID, false);
			}
			foreach (var ID in GetDeviceIDs(mgr, true)) {
				yield return new PdInfo(mgr, ID, false);
			}
		}

//---------------------------------------------------------------------------------------

		private static IEnumerable<string> GetDeviceIDs(PortableDeviceManager mgr, bool isPrivate) {
			uint nDevices = 0;
			if (isPrivate) {
				mgr.GetPrivateDevices(null, ref nDevices);
			} else {
				mgr.GetDevices(null, ref nDevices);
			}
			if (nDevices == 0) { yield break; }
			string[] IDs = new string[nDevices];
			if (isPrivate) {
				//// mgr.GetPrivateDevices(IDs, ref nDevices);
			} else {
				//// mgr.GetDevices(IDs, ref nDevices);
			}
			foreach (var id in IDs) {
				yield return id;
			}
		}

//---------------------------------------------------------------------------------------

		private string GetFriendlyName(string deviceID) {
			uint namelen = 0;
			// The next line might blow.
			try {
				//// mgr.GetDeviceFriendlyName(deviceID, null, ref namelen);
			} catch {
				return "N/A";
			}
			string name = new string('?', (int)namelen);
			//// mgr.GetDeviceFriendlyName(deviceID, name, ref namelen);
			return TrimNull(name);
		}

//---------------------------------------------------------------------------------------

		private string GetDescription(string dev) {
			uint desclen = 0;
			//// mgr.GetDeviceManufacturer(dev, null, ref desclen);
			string desc = new string('?', (int)desclen);
			//// mgr.GetDeviceManufacturer(dev, desc, ref desclen);
			return TrimNull(desc);
		}

//---------------------------------------------------------------------------------------

		private string GetManufacturer(string dev) {
			uint namelen = 0;
			//// mgr.GetDeviceManufacturer(dev, null, ref namelen);
			string name = new string('?', (int)namelen);
			//// mgr.GetDeviceManufacturer(dev, name, ref namelen);
			return TrimNull(name);
		}

//---------------------------------------------------------------------------------------

		private static string TrimNull(string s) {
			if (s.EndsWith("\0")) { s = s.Substring(0, s.Length - 1); }
			return s;
		}
	}
}
