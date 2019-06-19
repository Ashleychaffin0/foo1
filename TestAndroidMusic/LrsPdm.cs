using System;
using System.Text;
using PortableDeviceApiLib;

// TODO: In References, the property for the Interop DLL is flagged as EMBED INTEROP
//		 TYPES = TRUE. But https://cgeers.wordpress.com/2011/05/22/enumerating-windows-portable-devices/
//		 says to set it to FALSE. Check this out to see exactly what is needed.

namespace LRS.Utils {
	class LrsPdm {
		PortableDeviceManager Mgr;
		uint nDevices;
		string[] deviceIDs;

//---------------------------------------------------------------------------------------

		public LrsPdm() {
			var stuff = PdInfo.Pds();
			foreach (var s in stuff) {
				Console.WriteLine(s.FriendlyName);
			}

			Mgr = new PortableDeviceManager();
			Mgr.RefreshDeviceList();
			nDevices = 0;
			// string[] Empty = null;
			Mgr.GetDevices(null, ref nDevices);
			if (nDevices < 1) { throw new Exception("No PDM devices found"); }
			deviceIDs = new string[nDevices];
			Mgr.GetDevices(deviceIDs, nDevices);
			Dump("Normal", deviceIDs);

			uint nDevices_2 = 0;
			Mgr.GetPrivateDevices(null, ref nDevices_2);
			string[] deviceIDs_2 = new string[nDevices_2];
			Mgr.GetPrivateDevices(deviceIDs_2, ref nDevices_2);

			// uint pdwType;
			// byte[] pData = new byte[1000];
			// Mgr.GetDeviceProperty(deviceIDs[0], null, ref pData, ref pdwType);

			Dump("Private", deviceIDs_2);
		}

//---------------------------------------------------------------------------------------

		public void Dump(string Which, string[] Names) {
			Console.WriteLine("---------------------------");
			Console.WriteLine($"Dumping IDs of type {Which}");

			for (int ixDev = 0; ixDev < Names.Length; ixDev++) {
				var dev = Names[ixDev];
				string Fname = GetFriendlyName(dev);
				Console.WriteLine($"Device[{ixDev}]: {Fname} -- {dev}");
				string manu = GetManufacturer(dev);
				Console.WriteLine($"\tManufacturer: {manu}");
				string desc = GetDescription(dev);
				Console.WriteLine($"\tDescription: {desc}");
			}
		}

//---------------------------------------------------------------------------------------

		private string GetDescription(string dev) {
			uint desclen = 0;
			Mgr.GetDeviceManufacturer(dev, null, ref desclen);
			string desc = new string('?', (int)desclen);
			Mgr.GetDeviceManufacturer(dev, desc, ref desclen);
			return desc;

		}

//---------------------------------------------------------------------------------------

		private string GetManufacturer(string dev) {
			uint namelen = 0;
			Mgr.GetDeviceManufacturer(dev, null, ref namelen);
			string name = new string('?', (int)namelen);
			Mgr.GetDeviceManufacturer(dev, name, ref namelen);
			return name;
		}

//---------------------------------------------------------------------------------------

		private string GetFriendlyName(string deviceID) {
			uint namelen = 0;
			// The next line will return <namelen>, but will blow. Sigh. Ignore.
			try {
				Mgr.GetDeviceFriendlyName(deviceID, null, ref namelen);
			} catch {
				return "N/A";
			}
			string name = new string('?', (int)namelen);
			Mgr.GetDeviceFriendlyName(deviceID, name, ref namelen);
			return name;
		}

//---------------------------------------------------------------------------------------

		string Obsolete_UshortVectorToString(ushort[] shorts) {
			byte[] asBytes = new byte[shorts.Length * sizeof(ushort)];
			Buffer.BlockCopy(shorts, 0, asBytes, 0, asBytes.Length);
			return Encoding.Unicode.GetString(asBytes);
#if false    // This also works
			var sb = new StringBuilder();
			foreach (var item in us) {
				sb.Append(Convert.ToChar(item));
			}
			return sb.ToString();
#endif
		}
	}
}

