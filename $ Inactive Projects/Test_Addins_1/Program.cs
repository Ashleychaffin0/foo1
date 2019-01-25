using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using mscoree;

using Bart.Plugins;

using System.Net.NetworkInformation;

namespace Test_Addins_1 {
	public class Program {

		static void Main(string[] args) {

			// Note: CANNOT just include ICcLeads.cs into here and the DLL project(s).
			//		 They would be taken to be two separate interface definitions
			//		 (albeit with the same name). If you tried here to cast the main DLL
			//		 class to one of the ICcLeads methods, the cast would fail at
			//		 runtime, again, because of the duplicate definitions. There must be 
			//		 only one definition, and thus that must be in a DLL (possibly with 
			//		 other items, such as a proxy class definition.

			//		 With that said, maybe there's a way to define the interface here,
			//		 and have the DLLs reference this .exe. But I'm not sure I like
			//		 this very much. So for now, the interface goes into a separate DLL.

			Stopwatch sw = new Stopwatch();
			while (true) {
				Console.WriteLine("Press 1 or 2 to process DLL, 0 to use default DLL, 'x' to exit");
				ConsoleKeyInfo ckinf = Console.ReadKey(false);
				if (ckinf.KeyChar == 'x') {
					break;
				}
				Console.WriteLine();
				string s = new string(ckinf.KeyChar, 1);
				int n;
				bool bOK = int.TryParse(s, out n);
				if (! bOK)
					continue;
				if (n != 0) {
					SetDLL(n);
				}
				sw.Reset();
				sw.Start();
				// LoadAndProcessDLL();
				// RunUsingClass();
				string	Parm1 = "*DbConnStr*";
				int		Parm2 = 123;
				object[] Parms = new object[] { Parm1, Parm2 };
				// object[] Parms = null;
				CallPlugin("LRSDLL_1_ASM", "LRSDLL_1.Plugin_Wingate", Parms);
				Parms[0] = "*Connect2*";
				Parms[1] = 456;
				CallPlugin("LRSDLL_2", "LRSDLL_2.LRSDLL_2", Parms);
				sw.Stop();
				Console.WriteLine("Elapsed time: {0}", sw.Elapsed);
			}
		}

//---------------------------------------------------------------------------------------

		private static void CallPlugin(string AssemblyName, string ClassName, object [] Parms) {
			using (BartPlugIn<ICcLeads> pi = new BartPlugIn<ICcLeads>()) {
				try {
					ICcLeads proxy = pi.GetProxy(AssemblyName, ClassName, Parms);
					proxy.Forward();
					ICcLeads proxy2 = pi.GetProxy(AssemblyName, ClassName, Parms);
					string	dbConnectionString = "***";
					int		EventID = 345;
					proxy2.Forward(dbConnectionString, EventID);
				} catch (Exception ex) {
					Console.WriteLine("Exception in CallPlugin: {0}", ex.ToString());
				}
			}
			BartPlugIn<ICcLeads>.dbgShowAppDomains();
		}

//---------------------------------------------------------------------------------------

		private static void RunUsingClass() {
			string	DllName, ClassName;
			DllName = "LRSDLL_1_ASM";
			ClassName = "LRSDLL_1.Hello1";
			using (BartPlugIn<ICcLeads> pi = new BartPlugIn<ICcLeads>()) {
				try {
					object[] Parms = new object[] { "ParmName" };
					ICcLeads proxy = pi.GetProxy(DllName, ClassName, Parms);
					proxy.Forward("*DBS*", 111);
					// b = proxy.Forward();
					Parms[0] = "Proxy 2";
					ICcLeads proxy2 = pi.GetProxy(DllName, ClassName, Parms);
					proxy2.Forward("*DB2S*", 1211);
				} catch (Exception ex) {
					Console.WriteLine("Exception in RunUsingClass: {0}", ex.ToString());
				}
			}
			BartPlugIn<ICcLeads>.dbgShowAppDomains();
		}

//---------------------------------------------------------------------------------------

		private static void SetDLL(int n) {
			string src = string.Format("LRSDLL_1_ASM - Copy - Version {0}.dll", n);
			string dest = "LRSDLL_1_ASM.dll";
			System.IO.File.Copy(src, dest, true);
		}

//---------------------------------------------------------------------------------------

		private static void LoadAndProcessDLL() {
			string	guid = Guid.NewGuid().ToString();
			// Note: An AppDomainSetup is used to pass parameters to 
			//		 AppDomain.CreateDomain. But we don't need that.
			// AppDomainSetup	ads = new AppDomainSetup();
			// AppDomain ad = AppDomain.CreateDomain(guid, null, ads);
			AppDomain ad = AppDomain.CreateDomain(guid);
			// ad.ExecuteAssemblyByName("ICcLeads", null, "Hello", "world");
#if true
			ICcLeads proxy = (ICcLeads)ad.CreateInstanceAndUnwrap("LRSDLL_1_ASM", "LRSDLL_1.Hello1");
			// The following works, but instantiates the DLL class in this AppDomain,
			// which doesn't fit our requirements.
			// Type t = proxy.GetType();
			// object n = t.GetMethod("SayHi2", new Type[] { typeof(string) }).Invoke(proxy, new object[] { "LRSxyz2" });
#else
			CcLeadsProxy proxy = (CcLeadsProxy)ad.CreateInstanceAndUnwrap("ISampleSayHi", "Bart.CcLeads.CcLeadsProxy");
			// ShowAppDomains();
			proxy.Load(ad, "LRSDLL_1_ASM", "LRSDLL_1.Hello1");
#endif
			Console.WriteLine("Is proxy remoteable? {0}", RemotingServices.IsTransparentProxy(proxy));
			proxy.Forward("ljksdf", 234);
			// proxy.SayHi("Second call to DLL");
			// ShowAppDomains();

			// Console.WriteLine("About to unload secondary AppDomain");
			AppDomain.Unload(ad);
			BartPlugIn<ICcLeads>.dbgShowAppDomains();
		}

//---------------------------------------------------------------------------------------

		private static void Display_1(AppDomain ad) {
			Assembly asm = ad.Load("LRSDLL_1_ASM");
			object o = asm.CreateInstance("LRSDLL_1.Hello1");
			object p = ad.CreateInstanceAndUnwrap("LRSDLL_1_ASM", "LRSDLL_1.Hello1");
			Type t = p.GetType();
			InterfaceMapping ifm = t.GetInterfaceMap(typeof(ICcLeads));
			object n = t.GetMethod("Hiya", System.Type.EmptyTypes).Invoke(o, null);
			n = t.GetMethod("Hiya", new Type[] { typeof(string) }).Invoke(o, new object[] { "LRSxyz" });
			// n = o.GetType().GetMethod("SayHi", new Type[] { typeof(string) }).Invoke(o, new object[] { "LRSpqrs" });

			ICcLeads ccl = (ICcLeads)p;
			ccl.Forward("23423", 654);


			//----------------------------------------------------------------
			//----------------------------------------------------------------
			//----------------------------------------------------------------

			BartPlugIn<ICcLeads>.dbgShowAppDomains();
		}

//---------------------------------------------------------------------------------------

		private static void ShowAsms_DoesntWork(AppDomain ad) {
			string[] AsmsToIgnore = new string[]  {
				"Microsoft.VisualStudio.",
				"System,",
				"System.Core,",
				"mscorlib,",
				"System.Drawing",
				"System.Windows.",
				"Accessability",
				"Interop.mscoree",
				"System.Xml",
				"System.Configuration,",
				"vshost,"
			};

			Assembly[] asms = ad.GetAssemblies();

			var qry =	
				from asm in asms 
						from name in AsmsToIgnore
						where ! asm.FullName.StartsWith(name)
						select asm;
						 

			var qry2 = from asm in asms
					 select (from name in AsmsToIgnore
							 where ! asm.FullName.StartsWith(name)
							 select asm);

			var qry3 = from name in AsmsToIgnore
					  select (from asm in asms
							  where ! asm.FullName.StartsWith(name)
							  select asm);

			foreach (var asm in qry) {
				Console.WriteLine("\n* {0}", asm.FullName);
				// ShowTypes(asm);
			}
		}
	}
}
