// Copyright (c) 2004-2006 Bartizan Connects LLC

using System;
using System.Reflection;

namespace Bartizan.Utils.AssemblyUtils {
	/// <summary>
	/// Summary description for AssemblyUtils.
	/// </summary>
	public static class AssemblyUtils {

		// Note: The original design of this referenced Assembly.GetExecutingAssembly
		//		 but this didn't work when this code was called from a DLL (in this
		//		 case, the Common library shared between BadgeMax Designer and 
		//		 Runtime. So we'll have the caller get the attrs and pass them to
		//		 us. This has the (very) minor performance improvement that we
		//		 don't have to get the custom attrs each time.

//---------------------------------------------------------------------------------------

		public static string GetProductNameFromAssembly(object[] Attrs) {
			foreach (object Attr in Attrs) {
				if (Attr is AssemblyProductAttribute) {
					return ((AssemblyProductAttribute)Attr).Product;
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		public static string GetCopyrightFromAssembly(object[] Attrs) {
			foreach (object Attr in Attrs) {
				if (Attr is AssemblyCopyrightAttribute) {
					return ((AssemblyCopyrightAttribute)Attr).Copyright;
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		public static Version GetVersionFromAssembly(Assembly asm) {
			Version vers = asm.GetName().Version;
			return vers;
		}

//---------------------------------------------------------------------------------------

		public static string FindResourceFromAssembly(Assembly asm, string resourceName) {
			string fullResourceName = string.Empty;

			string[] names = asm.GetManifestResourceNames();
			foreach (string name in names) {
				if (name.LastIndexOf(resourceName) > 0) {
					fullResourceName = name;
					break;
				}
			}
			return fullResourceName;
		}
	}
}
