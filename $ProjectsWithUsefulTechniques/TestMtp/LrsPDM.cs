// Note: https://blogs.msdn.microsoft.com/dimeby8/2006/12/05/c-and-the-wpd-api/
// See: https://blogs.msdn.microsoft.com/dimeby8/2006/12/05/enumerating-wpd-devices-in-c/

// ***IMPORTANT ***
// Using custom version of Interop.PortableDeviceApiLib
// See https://stackoverflow.com/questions/6162046/enumerating-windows-portable-devices-in-c-sharp

// From https://stackoverflow.com/questions/6162046/enumerating-windows-portable-devices-in-c-sharp
// But note the correction from https://www.codeproject.com/Messages/3187340/Re-Using-WPD-api-Windows-Portable-Devices-modified.aspx
#if false
Disassemble the PortableDeviceApi Interop assembly using the command:
	ildasm Interop.PortableDeviceApiLib.dll /out:pdapi.il
Open the IL in Notepad and search for the following string:
	instance void GetDevices([in][out] string& marshal( lpwstr) pPnPDeviceIDs,
Replace all instances of the string above with the following string:
	instance void GetDevices([in][out] string[] marshal([]) pPnPDeviceIDs,
Save the IL and reassemble the interop using the command:
	ilasm pdapi.il /dll /output=Interop.PortableDeviceApiLib.dll

Correction:
GetDevices([in][out] string[] marshal([]) pPnPDeviceIDs ← the modification of dimeby8
GetDevices([in][out] string[] marshal(lpwstr[]) pPnPDeviceIDs ← I modify
#endif

// Note: Also fix GetPrivateDevices, same as above (including correction)
// Note: Probably have to fix GetDeviceFriendlyName as well as GetDeviceDescription,
//		 GetDeviceManufacturer
// Note: The .il file is in G:\LRS\$Dev\$$$$$ PortableDeviceApiLib.dll FIX
// Note: When the .il file has been modified, run MakeNewDll.cmd from 
//		 G:\LRS\$Dev\$$$$$ PortableDeviceApiLib.dll FIX
//		 But note that this must be done from the Visual Studio Developer Command Prompt,
//		 and then the output file must be copied to the Debug/Release folder for whatever
//		 program wants to use these routines. Amen.
// Note: Added marshal( lpwstr) to GetDeviceFriendlyName. Not currently working...


using System;
using System.Collections.Generic;
using System.Text;

using PortableDeviceApiLib;
using PortableDeviceTypesLib;

using LRS.Utils;


namespace TestMtp {
	class LrsPdm {
		PortableDeviceManager Mgr;
		uint	 nDevices;
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
			//// Mgr.GetDevices(deviceIDs, nDevices);
			Dump("Normal", deviceIDs);

			uint nDevices_2 = 0;
			Mgr.GetPrivateDevices(null, ref nDevices_2);
			string[] deviceIDs_2 = new string[nDevices_2];
			//// Mgr.GetPrivateDevices(deviceIDs_2, ref nDevices_2);

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
			//// Mgr.GetDeviceManufacturer(dev, null, ref desclen);
			string desc = new string('?', (int)desclen);
			//// Mgr.GetDeviceManufacturer(dev, desc, ref desclen);
			return desc;
			
		}

//---------------------------------------------------------------------------------------

		private string GetManufacturer(string dev) {
			uint namelen = 0;
			//// Mgr.GetDeviceManufacturer(dev, null, ref namelen);
			string name = new string('?', (int)namelen);
			//// Mgr.GetDeviceManufacturer(dev, name, ref namelen);
			return name;
		}

//---------------------------------------------------------------------------------------

		private string GetFriendlyName(string deviceID) {
			uint namelen = 0;
			// The next line will return <namelen>, but will blow. Sigh. Ignore.
			try {
				//// Mgr.GetDeviceFriendlyName(deviceID, null, ref namelen);
			} catch {
				return "N/A";
			}
			string name = new string('?', (int)namelen);
			//// Mgr.GetDeviceFriendlyName(deviceID, name, ref namelen);
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
