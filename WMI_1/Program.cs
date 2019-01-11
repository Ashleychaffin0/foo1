using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

// Note: This is a bit of a mess at the moment. I took some sample code from Microsoft
//		 and started (but didn't finish) playing with it.

namespace WMI_1 {
	class Program {
		static void Main(string[] args) {
			List<string> nss = GetWmiNamespaces();
			// GetWmiClasses(nss[18]);		// Microsoft
			// GetWmiClasses(nss[4]);			// CIMV2
			GetWmiClasses(nss[13]);			// Directory
		}

//---------------------------------------------------------------------------------------

		private static List<string> GetWmiNamespaces() {
			var NamespaceNames = new List<string>();
			try {
				Console.WriteLine("----- WMI Namespaces -----");
				// Enumerate all WMI instances of __namespace WMI class.
				ManagementClass nsClass = new ManagementClass(
				   new ManagementScope("root"),
				   new ManagementPath("__namespace"),
				   null);
				foreach (ManagementObject ns in nsClass.GetInstances()) {
					Console.WriteLine(ns["Name"].ToString());
					NamespaceNames.Add(ns["Name"].ToString());
				}
				Console.WriteLine("{0} namespaces found", NamespaceNames.Count);
			} catch (Exception ex) {
				Console.WriteLine("*** Exception - {0}", ex.Message);
			}
			return NamespaceNames;
		}

//---------------------------------------------------------------------------------------

		private static void GetWmiClasses(string NamespaceName) {
			try {
				int count = 0;
				Console.WriteLine("\r\n\r\n----- WMI Class {0} -----", NamespaceName);
				// Perform WMI object query on selected namespace.
				var Scope = new ManagementScope(@"\\.\ROOT\" + NamespaceName);
				var qry = new WqlObjectQuery("select * from meta_class");
				ManagementObjectSearcher searcher = new ManagementObjectSearcher(
					Scope, qry, null);
				foreach (ManagementClass wmiClass in searcher.Get()) {
					Console.WriteLine(wmiClass["__CLASS"].ToString());
					count++;
				}
				Console.WriteLine("{0} classes found", count);
			} catch (Exception ex) {
				Console.WriteLine("*** Exception - {0}", ex.Message);
			}
		}
	}
}
