// Copyright (c) 2008 by Bartizan Connect, LLC

// To create a plug-in:
//	1)	Create a new project of type library (i.e. DLL). In the "Using the Plug-in" 
//		section below, we'll assume that the source file is LRSDLL_1.cs and the output is
//		LRSDLL_1.dll.
//	2)	In the project properties.Application, set its Assembly Name to whatever you 
//		like. By default, this is the same as the project name, which is fine. But what
//		we're loading is an Assembly name, not a DLL name. In other words, make sure that
//		the Assembly name for all plugins are different. In the example below, we'll 
//		assume the Assembly name is LRSDLL_1_ASM. Note that this isn't necessarily
//		recommended (it would be fine to leave it at its default, LRSDLL_1). However,
//		we'll give it this name to ensure that the we have separate names for all the
//		pieces of the puzzle, so that in the example below, you'll be sure that we're
//		referring to, say, the Assembly name and not the file name.
//	3)	You'll also have to note the namespace of the class you want to use. You'll have
//		to use the fully qualified name (i.e. namespace.classname) later. We'll assume
//		below that you've put everything into namespace LRSDLL_1_NS.
//	4)	Your class must derive from both MarshalByRefObject and the interface it supports
//		(in that order). Currently we have defined only one interface, ICcLeads.
//		For example, <class Plugin_Wingate : MarshalByRefObject, ICcLeads>.
//	5)	Add the methods needed by ICcLeads. Also any constructors, fields, properties,
//		and, of course, any additional private methods you need.
//			5.1) In the example below, we assume that ICcLeads has two methods, both
//				 named Forward, one that takes explicit parameters, and one that picks up
//				 its parameters that were saved by its constructor.
//	6)	Add a reference to this DLL to your project, then add <using Bart.Plugins;> to
//		your code.

// To actually use the plug-in:
//	1)	Add a reference to this DLL to the project you want to call the plug-in from, 
//		then add <using Bart.Plugins;> to your code. Note: This is *this* DLL (Plugins),
//		*not* to the plug-in.
//	2)	Add code similar to the following.
//	2.1	object[] Parms = new object[] { "ConnectString", 123 };
//	2.2	CallPlugin("LRSDLL_1_ASM", "LRSDLL_1.Plugin_Wingate", Parms);
//	2.3	CallPlugin("LRSDLL_1_ASM", "LRSDLL_1.Plugin_Wingate", null);

//	2.4 private static void CallPlugin(string AssemblyName, string ClassName, object [] Parms) {
//  2.5	  using (BartPlugIn<ICcLeads> pi = new BartPlugIn<ICcLeads>()) {	
//  2.6      try {
//  2.7	          ICcLeads proxy = pi.GetProxy(AssemblyName, ClassName, Parms);
//  2.8	          bool o = proxy.Forward("DefaultConnectionString", 456);
//  2.9	          o = proxy.Import();
//  2.10          ICcLeads proxy2 = pi.GetProxy(DllName, ClassName, null);
//  2.11          o = proxy2.Forward();
//  2.12      } catch (Exception ex) {
//  2.13          Console.WriteLine("Exception in RunUsingClass: {0}", ex.ToString());
//  2.14      }
//  2.15  }
//	3)	Comments on the above:
//			Line 2.1 - Parms for the ctor are a database connection string and an EventID
//			Line 2.2 - Call the plug-in, passing parms to the ctor
//			Line 2.3 - Load and call the plug-in again, using the default ctor
//			Line 2.4 - Parms (if any) must be passed in the same order, and be of the
//					 - same type as the ctor expects them
//			Line 2.5 - I strongly recommend using "using". You *must* call Dispose() on
//					   the plugin to unload it, so that the next time it's called we'll
//					   pick up the an updated DLL, if one's available. But "using" calls
//					   Dispose() automatically.
//			Line 2.7 - Load the plugin, and get a reference to a proxy class that can
//					   bridge the AppDomain firewall to call the plug-in
//			Line 2.8 - The Forward() method returns void, so this line wouldn't compile.
//					   But this shows that plug-in methods can return typed values.
//					   It also calls the proxy with its own hard-coded parameters. Not
//					   recommended, of course, but here's how you'd do it.
//			Line 2.9 - Assuming the plug-in remembered the parameters its ctor was
//					   called with, call the plug-in's method with no parms.
//			Line 2.10 - 2.11 - You probably won't need to, but if you need another
//					    instance of the plug-in, you can do it.
//			Line 2.11 - This will probably fail, since you didn't supply parameters in
//						step 2.10, and you're calling the Forward() method also with no
//						parms. Presumably the Forward() method will throw an Exception.
//			Line 2.13 - If you throw an Exception in the plug-in, the proxy routine will
//						percolate it up to the 
//	4)	Make sure you deploy your DLL where we can find it, either in the same directory
//		as your web service, or in a subdirectory.


// Notes on passing data types:
//	1)	First of all we have to understand the concept of "marshaling". To "marshal" (in
//		this case) means "to usher or lead ceremoniously: Their host marshaled them into
//		the room."). In this case, it applies to the act of bridging the firewall
//		(nothing to do, directly, with networking firewalls) between two AppDomains (or
//		Processes), the main purpose of which is to *not* let programs in one access the
//		data in another. But we usually need to pass data (either to ordinary methods, or
//		to constructors) from the calling AppDomain to the callee, and later, for methods
//		to return values to the caller.
//	2)	I'm still not 100% sure I understand the rules for passing data, but here's my
//		current best guess. But take it with a gain of salt...
//	3)	All data passed to / returned from the plug-in must be one of
//			a) a value type (e.g. int, float, etc, or any struct)
//			b) a string
//			c) a reference type that is derived from MarshalByRefObject
//	4)	If you violate these data type rules, you'll likely wind up with a copy of your
//		plug-in inside the caller (e.g. LeadsLightning), and you won't be able to load
//		a new copy of the DLL (to fix bugs, add new features, etc) until LeadsLightning
//		is restarted.

// Debug methods
//	1)	A useful utility routine is dbgShowAppDomains, a static method in class
//		BartPlugIn. It shows all AppDomains, and the Assemblies within them. But one
//		MAJOR WARNING. If you call it when a plug-in is loaded, you will cause a copy of
//		the plug-in to be loaded into your main Assembly, and you won't be able to reload
//		the plug-in. So if you do call it, make sure you've Dispose'd() of the plug-in
//		AppDomain first.

// Other Notes:
//	1)	If you get a "constructor not found exception" on a call to GetProxy, check your
//		Parms parameter. You might be passing in null when the constructor has parameters
//		or vice versa. Or the object[] you're passing in might not agree with the types
//		and order of parameters in the constructor. For example, if the ctor wanted
//		an int and a string (in that order), then don't pass in object[] {"string", 5}.


using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Reflection;
using mscoree;
using System.Runtime.InteropServices;

namespace Bart.Plugins {

	public interface ICcLeads {
#if false	// Demo code to show how to put a property on a plug-in
		int foo { get; set; }
#endif
		void Forward(string DbConnectionString, int EventID);
		void Forward();
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class BartPlugIn<T> : MarshalByRefObject, IDisposable {
		AppDomain	ad;

//---------------------------------------------------------------------------------------

		public BartPlugIn() {
			string guid = Guid.NewGuid().ToString();
			ad = AppDomain.CreateDomain(guid);
		}

//---------------------------------------------------------------------------------------

		public T GetProxy(string AssemblyName, string ClassName) {
			T proxy = GetProxy(AssemblyName, ClassName, null);
			return proxy;		
		}

//---------------------------------------------------------------------------------------

		public T GetProxy(string AssemblyName, string ClassName, object[] Parms) {
			T proxy = (T)ad.CreateInstanceAndUnwrap(
				AssemblyName,			// Basically, DLL name
				ClassName,
				false,					// IgnoreCase
				0,						// BindingAttr
				null,					// Binder
				Parms,					
				null,					// CultureInfo
				null,					// Activation Attributes
				null					// Security Attributes
				);
			return proxy;		
		}

//---------------------------------------------------------------------------------------

		// Debug routine.
		//	DO NOT CALL WHEN A PLUG-IN IS IN MEMORY, ELSE A COPY OF IT WILL BE LOADED 
		//	INTO THE CALLING APPDOMAIN. CALL IT EITHER BEFORE YOU LOAD THE PLUG-IN, OR
		//	AFTER IT'S UNLOADED, TO ENSURE THAT YOU HAVEN'T INADVERTENTLY DONE SOMETHING
		//	TO LOAD IT INTO THE CALLER.
		public static void dbgShowAppDomains() {
			List<AppDomain> ads = dbgGetAppDomains();
			Console.Write("\n\n---------------");
			foreach (AppDomain appdom in ads) {
				Console.WriteLine("\n------------\nAppDomain: {0}", appdom.FriendlyName);
				dbgShowAsms(appdom);
			}
		}

//---------------------------------------------------------------------------------------

		// Debug routine.
		public static void dbgShowAsms() {
			dbgShowAsms(AppDomain.CurrentDomain);
		}

//---------------------------------------------------------------------------------------

		// Debug routine.
		public static void dbgShowAsms(AppDomain ad) {
			// Don't bother showing fairly standard Assemblies that are/might be in
			// any given AppDomain.
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

			foreach (var asm in asms) {
				bool bPrintit = true;
				foreach (var prefix in AsmsToIgnore) {
					if (asm.FullName.StartsWith(prefix)) {
						bPrintit = false;
						break;
					}
				}
				if (bPrintit) {
					Console.WriteLine("\n* {0}", asm.FullName);
					ShowTypes(asm);
				}
			}
		}

//---------------------------------------------------------------------------------------

		public static List<AppDomain> dbgGetAppDomains() {
			CorRuntimeHostClass host = new CorRuntimeHostClass();
			try {
				List<AppDomain> list = new List<AppDomain>();
				IntPtr enumHandle;
				host.EnumDomains(out enumHandle);
				while (true) {
					object domain;
					host.NextDomain(enumHandle, out domain);
					if (domain == null)
						break;
					list.Add((AppDomain)domain);
				}
				host.CloseEnum(enumHandle);
				return list;
			} finally {
				Marshal.ReleaseComObject(host);
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowTypes(Assembly asm) {
			Type[] types = asm.GetExportedTypes();
			foreach (Type t in types) {
				Console.WriteLine("\t ** {0}", t.FullName);
			}
		}

//---------------------------------------------------------------------------------------

		#region IDisposable Members

		public void Dispose() {
			if (ad != null) {
				AppDomain.Unload(ad);
				ad = null;
			}
		}

		#endregion
	}

#if false	// We don't seem to need the proxy class
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	// [Serializable]
	public class CcLeadsProxy : MarshalByRefObject, ICcLeads  {
		ICcLeads	cclProxy = null;

//---------------------------------------------------------------------------------------

		public bool Load(AppDomain Ad, string AssemblyName, string ClassName) {
			Console.WriteLine("In CcLeadsProxy ctor, AppDomain '{0}'", AppDomain.CurrentDomain.FriendlyName);
			cclProxy = (ICcLeads)Ad.CreateInstanceAndUnwrap(AssemblyName, ClassName);
			return true;
		}

//---------------------------------------------------------------------------------------

		#region ICcLeads Members

		public bool SayHi(string s) {
			// Console.WriteLine("Hello {0} from ICcLeads.SayHi", s);
			// return false;
			Console.WriteLine("About to call DLL_1");
			Console.WriteLine("Is DLL proxy remoteable? {0}", RemotingServices.IsTransparentProxy(cclProxy));
			return cclProxy.SayHi(s);
		}

		#endregion
	}
#endif
}