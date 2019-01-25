// Copyright (c) 2006 Bartizan Connects LLC

/**
 * Note: This module was inspired by a number of sources. In no particular order they were:
 * 
 * The book Programming Microsoft Internet Explorer 5 by Scott Roberts
 * 
 * Build a Managed BHO and Plug into the Browser -- http://www.15seconds.com/issue/040331.htm
 * 
 * Browser Helper Objects: The Browser the Way You Want it -- http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwebgen/html/bho.asp
 * 
 * IE Browser Helper Objects -- http://www.developerfusion.co.uk/show/4644/
 **/

/**
 * The GUID for this COM object is specified in several places, not all of which can
 * reference a single location. In particular, we can't declare a const string, then
 * use it in a GuidAttribute. Sigh. What we want is #define OURGUID "xxx...". But no.
 * So we'll just duplicate it where we must.
 * 
 * Note: Our BHO can be found in HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects\{AC0F7A58-55CC-4992-9CE6-CAA324453983}
 **/

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using SHDocVw;						// Reference: In COM - Microsoft Internet Controls

namespace BartBlock {

	// Import IObjectWithSite interface
	[ComImport(), Guid("fc4801a3-2ba9-11cf-a229-00aa003d7352")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IObjectWithSite {
		[PreserveSig]
		void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSite);

		[PreserveSig]
		void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvSite);
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	// {AC0F7A58-55CC-4992-9CE6-CAA324453983}

	/// <summary>
	/// Definition for the default BHO COM interface
	/// </summary>
	[Guid("AC0F7A58-55CC-4992-9CE6-CAA324453983")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IBartBlock {
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Definition for the BHO class that will be instantiated by the browser. IE will be
	/// looking for the IObjectWithSite interface to invoke SetSite() and pass this
	/// component the container site.
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("AC0F7A58-55CC-4992-9CE6-CAA324453983")]
	// TODO: Change to Bartizan.BartBlock
	[ProgId("Bartizan.BartBlockOld")]
	public class BartBlock : IBartBlock, IObjectWithSite {

		[DllImport("kernel32.dll")]
		static extern void OutputDebugString(string lpOutputString);

		// See comments at the start of the module.
		const string OurGUID = "AC0F7A58-55CC-4992-9CE6-CAA324453983";

		protected SHDocVw.WebBrowser web = null;

		// HRESULT values used
		const int E_FAIL = unchecked((int)0x80004005);
		const int E_NOINTERFACE = unchecked((int)0x80004002);

		string			DefaultSite;
		List<string>	AllowedSites;

		bool			bLetItThrough;			// Navigating to a site may be redirected
												// several times, especially through
												// (sigh) doubleclick.com and their ilk.
												// So if an initial navigation event is
												// approved, let all following sites
												// through until we're actually through
												// to the site, at which point we'll
												// reset this flag.

		// TODO: The following flag is mostly (for now) for debugging
		bool			AlwaysLetThrough;		// Always work if we're not invoked from
												// IE (e.g. from Windows Explorer)

//---------------------------------------------------------------------------------------

		public BartBlock() {
#if DEBUG
			MessageBox.Show("BartBlock-Old ctor");
#endif
			AllowedSites = new List<string>();

			// Note: *Must* be in lower case
			DefaultSite = "http://www.signup4.net";
			AllowedSites.Add(DefaultSite);
			AllowedSites.Add("http://www.refdesk.com");
			AllowedSites.Add("http://www.bartizan.com");
			AllowedSites.Add("http://www.mitrainternational.com");
			AllowedSites.Add("http://msdn.microsoft.com");

			bLetItThrough = false;

			AlwaysLetThrough = false;
		}

//---------------------------------------------------------------------------------------

		void web_BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel) {
			if (AlwaysLetThrough)
				return;

			string url = (string)URL;
			// MessageBox.Show("LRSBHO - BeforeNavigateTo - URL = " + url);
			ODS("LRSBHO - BeforeNavigateTo - URL = {0}", url);

			if (bLetItThrough)
				return;

			url = url.ToLower();
			foreach (string site in AllowedSites) {
				if (url.StartsWith(site)) {
					bLetItThrough = true;
					return;
				}
			}
			Cancel = true;
			ODS("*** Blocked access to {0}", URL);
			string msg = "Sorry, but this kiosk allows you to browse only to the following site";
			if (AllowedSites.Count != 0)
				msg += "s";
			msg += Environment.NewLine + Environment.NewLine;
			foreach (string site in AllowedSites)
				msg += "    " + site + Environment.NewLine;
			MessageBox.Show(msg);

			// Bring user back to main site
			object	oDefaultSite = (object)DefaultSite;
			int		flags = 0;
			object	oFlags = (object)flags;
			object	oTargetFrameName = null;
			object	oPostData = null;
			object	oHeaders = null;
			web.Navigate2(ref oDefaultSite, ref oFlags, ref oTargetFrameName, ref oPostData, ref oHeaders);
		}

//---------------------------------------------------------------------------------------

		void web_DocumentComplete(object pDisp, ref object URL) {
			ODS("    DocumentComplet - URL = {0}", URL);
			SHDocVw.WebBrowser web2 = pDisp as SHDocVw.WebBrowser;
			if (web2 == (web.Application as SHDocVw.WebBrowser)) {
				ODS("        DocumentComplete - isFinal");
				bLetItThrough = false;
			}
		}

//---------------------------------------------------------------------------------------

		void web_OnQuit() {
			// TODO: We can't seem to say, no, don't close. Hmmm.
			// throw new Exception("The method or operation is not implemented.");
			ODS("******* In OnQuit ********");
			System.Diagnostics.Process.Start("IEXPLORE.EXE");
		}

//---------------------------------------------------------------------------------------

		private void ODS(string fmt, params object[] parms) {
			string	msg = string.Format(fmt, parms);
			OutputDebugString(msg);
		}

//---------------------------------------------------------------------------------------

		private string GetOurModuleName() {
			string s = "";
			RegistryKey	key = Registry.ClassesRoot.OpenSubKey(@"CLSID\{" + OurGUID + @"}\InprocServer32");
			if (key != null) {
				s = (string)key.GetValue("CodeBase");
			}
			return s;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Invoked by the runtime when registered as COM server.
		/// </summary>
		[ComRegisterFunctionAttribute]
		public static void Register(Type t) {

			// register BHO
			string guid = t.GUID.ToString("B");

			RegistryKey rkey =
				Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects");
			RegistryKey rkeyBHO = rkey.CreateSubKey(guid);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Invoked when this COM server is unregistered.
		/// </summary>
		[ComUnregisterFunctionAttribute]
		public static void Unregister(Type t) {
			// unregister bho
			string guid = t.GUID.ToString("B");
			RegistryKey rkey =
					Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects");
			rkey.DeleteSubKey(guid, false);
		}

//---------------------------------------------------------------------------------------

		#region IObjectWithSite Members
		public void SetSite(object pUnkSite) {
#if DEBUG
			MessageBox.Show("SetSite reached", "BartBlock-Old", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
			// get IWebBrowser2 reference
			if (web != null)
				web = null;				// Does a Release
			if (pUnkSite == null)
				return;
#if DEBUG
			System.Diagnostics.Debugger.Break();
#endif

string ModName = GetOurModuleName();
			web = pUnkSite as SHDocVw.WebBrowser;
			if (web == null)
				return;
			string sHostName = web.FullName;
			if (!(sHostName.ToUpper().EndsWith("\\IEXPLORE.EXE"))) {
				web = null;
				AlwaysLetThrough = true;
				return;
			}
			web.BeforeNavigate2 += new DWebBrowserEvents2_BeforeNavigate2EventHandler(web_BeforeNavigate2);
			web.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(web_DocumentComplete);
			web.OnQuit += new DWebBrowserEvents2_OnQuitEventHandler(web_OnQuit);
		}

//---------------------------------------------------------------------------------------

		public void GetSite(ref Guid riid, out object ppvSite) {
#if DEBUG
			MessageBox.Show("GetSite reached", "BartBlock-Old", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
			// The default implementation should return the stored site pointer
			// but following the implementation rules means we check for the
			// requested interface using QueryInterface()
			ppvSite = null;
			if (web != null) {
				IntPtr pSite = IntPtr.Zero;
				IntPtr pUnk = Marshal.GetIUnknownForObject(web); // adds ref
				Marshal.QueryInterface(pUnk, ref riid, out pSite); // adds ref
				Marshal.Release(pUnk);  // reduce reference count, we won't be needing pUnk 
				Marshal.Release(pUnk);

				if (!pSite.Equals(IntPtr.Zero)) {
					ppvSite = pSite;
				} else {
					// E_NOINTERFACE should be returned if the requested interface is not found
					web = null;
					Marshal.ThrowExceptionForHR(E_NOINTERFACE);
				}
			} else {
				// E_FAIL should be returned if the requested interface is not found
				web = null;
				Marshal.ThrowExceptionForHR(E_FAIL);
			}
		}
		#endregion
	}
}
