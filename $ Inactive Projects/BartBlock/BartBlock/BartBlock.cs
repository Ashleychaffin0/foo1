// Copyright (c) 2006 Bartizan Connects LLC

// TODO:

//	*	Activation code.
//	*	Better security on password module

// Last minute TODO:
//	*	Copy AllowedSites.txt in distribution. Set as just SU4.

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
 * Note: Our BHO can be found in HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects\{f0527dbe-6a52-48fa-903f-8d2795419143}
 * 
 * Note: To install it manually, run regasm /codebase BartBlock.dll. To remove it, run 
 *		 regasm /u BartBlock.dll. 
 **/

/** A note about debugging this:
 * Approach 1:
 * You can debug the BHO code, but there's a catch. Since it's managed code and
 * obviously IE is not managed code, it's rather difficult to jump in right at the first 
 * instantiation of the first BHO. But that's not a big deal. Each BHO is loaded afresh
 * into each instance of IE.
 * 
 * So, start up IE. Then from VS.NET, use the Attach to Process item in the Debug menu
 * to attach to iexplore.exe. Set breakpoints in the BHO. To break within the 
 * constructor, just open a second IE window. 
 * 
 * Approach 2:
 * Put a call to System.Diagnostics.Debugger.Break() into the code. When this is hit, you
 * will be asked if you want to attach a debugger to the program. Go for it!
 **/

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

using SHDocVw;
using System.IO;						// Reference: In COM - Microsoft Internet Controls

using Bartizan.Utils.AssemblyUtils;
using Bartizan.Utils.CRC;
using Bartizan.BartSecure;
using BartBlockForms;

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


	/// <summary>
	/// Definition for the default BHO COM interface
	/// </summary>
	[Guid("f0527dbe-6a52-48fa-903f-8d2795419143")]
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
	[Guid("f0527dbe-6a52-48fa-903f-8d2795419143")]
	[ProgId("Bartizan.BartBlock")]
	public class BartBlock : IBartBlock, IObjectWithSite {

#if false	// Don't need. Just found out about System.Diagnostics.Debugger.Log
		[DllImport("kernel32.dll")]
		static extern void OutputDebugString(string lpOutputString);
#endif

		// See comments at the start of the module.
		const string OurGUID = "f0527dbe-6a52-48fa-903f-8d2795419143";

		protected SHDocVw.WebBrowser web = null;

		// HRESULT values used
		const int E_FAIL		= unchecked((int)0x80004005);
		const int E_NOINTERFACE = unchecked((int)0x80004002);

		string			DefaultSite;
		List<string>	AllowedSites;

		// Navigating to a site may be redirected several times, especially through
		// (sigh) doubleclick.com and their ilk. So if an initial navigation event is
		// approved, let all following sites through until we're actually through
		// to the site, at which point we'll reset this flag.
		bool			bLetItThrough;

		// Always work if we're not invoked from IE (e.g. from Windows Explorer), don't
		// pass our Activation code checking, etc.
		bool			AlwaysLetThrough;		

		public static string	RegKey;

//---------------------------------------------------------------------------------------

		public BartBlock() {
#if DEBUG
			BartCRC crc = new BartCRC();
			crc.AddData("pass");
			string pw = crc.GetCRC().ToString("X8");
			MessageBox.Show("5/3/06 2PM - BartBlock ctor - In DEBUG mode - pass = " + pw);
#endif

			AlwaysLetThrough = false;
#if DEBUG
			System.Diagnostics.Debugger.Break();
#endif

			string fmt = @"SOFTWARE\Bartizan\{0}\{1}.{2}\{3}";
			RegKey = string.Format(fmt, ProductActivation.BARTBLOCK_SUITE_NAME,
				1, 0,			// ver.Major, ver.Minor,
				ProductActivation.BARTBLOCK_PRODUCTNAME);

#if false	
		// At this point, we don't support an Activation code at all. Once we do,
		// re-enable this code. Note however that this code hasn't been tested yet.
#if ! DEBUG	// Don't force me to do a real install if I'm a developer
			bool bOK = ProductActivation.CheckActivationCode(
				"C:\\",	key, "ActivationCode");
			if (! bOK) {
				string msg = "We're sorry, but the BartBlock activation"
					+ " code was not valid. Perhaps you have not installed it"
					+ " correctly, or are not licensed to use it."
					+ "\n\nUntil this is fixed, and IE restarted, BartBlock will"
					+ " NOT block any sites.";
				MessageBox.Show(msg, "Security",
					MessageBoxButtons.OK, MessageBoxIcon.Stop);

				// Instead of refusing to run, we'll just let everything through
				AlwaysLetThrough = true;
			}
#endif
#endif

			bLetItThrough = false;

			AllowedSites = new List<string>();

			LoadAllowedSites();

			// Debugging note: At one point, I noticed that I had some code duplicated.
			// So I refactored it out into a routine called GoToDefaultSite(). And now 
			// that I had such a nice little routine, I decided to throw in a call to
			// it here.
			// 
			// Uh, except that it used the <web> variable, which, here in the ctor, 
			// hasn't been initialized yet. So the CLR threw a Null Object exception, 
			// which shut down the whole COM object. And there was nary a peep from
			// anyone about it.
			//
			// Which means that the SetSite routine was never called by IE, so this whole
			// routine didn't work. (Except for LoadAllowedSites, called above, which
			// might complain about, say, the list of allowed sites being missing.) 
			//
			// So if you ever find SetSite not being called, check to make sure that
			// there are no fatal errors in the code.
			//
			// Serves me right for using *any* type of go to! :-)

			// GoToDefaultSite();
		}

//---------------------------------------------------------------------------------------

		private void LoadAllowedSites() {
			string	msg;
			DefaultSite = "http://www.signup4.net";
			AllowedSites.Clear();
			try {
				string	Filename = "AllowedSites.txt";
				// Note: Look in ExecutablePath, not StartupPath. The latter will give
				//		 you the IE path (e.g. Program Files\Internet Explorer 
				//		 (or whatever))
				// Note: Hmmm, that didn't work either. Instead we just got StartupPath
				//		 with the executable name (Iexplore.exe) appended! So for now,
				//		 just go with My Documents.
				// Filename = Path.Combine(Application.ExecutablePath, Filename);
				Filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Filename);
				if (!File.Exists(Filename)) {
					msg = "The file with the list of allowed sites could not be found in\n\n{0}";
					msg = string.Format(msg, Filename);
					MessageBox.Show(msg, "Security", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					AllowedSites.Add(DefaultSite);
					return;
				}
				AllowedSites.Clear();
				using (StreamReader sr = new StreamReader(Filename)) {
					string	line;
					while ((line = sr.ReadLine()) != null) {
						// Note: Must *not* change case. Some sites (especially the
						//		 default site) may be (duh) case-sensitive.
						line = line.Trim();
						// Ignore comments and blank lines
						if ((line == "") || line.StartsWith("#")) {	
							continue;
						}
						// Do some validity checking
						string	LineLower = line.ToLower();
						if (LineLower.StartsWith("http://") || LineLower.StartsWith("https://")) {
							// Do nothing
						} else {
							line = "http://" + line;
						}
						Uri	uri;
						bool bOK = Uri.TryCreate(line, UriKind.Absolute, out uri);
						if ((! bOK) || (! uri.IsWellFormedOriginalString())) {
							msg = "The following is not a valid allowed site: " + line;
							MessageBox.Show(msg, "Security", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							continue;
						}
						// The first site is the default site
						if (AllowedSites.Count == 0)
							DefaultSite = line;
						AllowedSites.Add(line);
					}
				}
			} catch (Exception ex) {
				msg = string.Format("There was an unexpected error ({0})"
					+ " processing the list of allowed sites.", ex.Message);
				MessageBox.Show(msg, "Security", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				if (AllowedSites.Count == 0) {
					AllowedSites.Add(DefaultSite);
				}
			}

#if false
			DefaultSite = "http://www.signup4.net";
			AllowedSites.Add(DefaultSite);
			AllowedSites.Add("http://www.refdesk.com");
			AllowedSites.Add("http://www.bartizan.com");
			AllowedSites.Add("http://www.mitrainternational.com");
			AllowedSites.Add("http://msdn.microsoft.com");
#endif
		}

//---------------------------------------------------------------------------------------

		void web_BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel) {
			if (AlwaysLetThrough)
				return;

			// Note: Originally we had the url = (string)URL line. But when we did
			//		 url = url.ToLower() below, it updated the <URL> reference. And
			//		 that screwed up some case-sensitive stuff that SignUp4 needed
			//		 to remain in mixed-case. So clone the URL variable and work with
			//		 that, leaving the original URL unchanged.
			// string url = (string)URL;
			string url = (string)((string)URL).Clone();

			ODS("LRSBHO - BeforeNavigateTo - URL = {0}", url);

			if (bLetItThrough)
				return;

			url = url.ToLower();

#if false	// Admin code doesn't work yet
			if (url.EndsWith("www.bartblock.com")) {	// w/ or w/o "http://" prefix
				// And for that matter, it could be foowww.bartblock.com. Fine.
				DialogResult res = CheckPassword("enter Administrator mode.", RegKey);
				switch (res) {
				case DialogResult.Yes:
					ProcessAdminScreen();
					Cancel = true;
					GoToDefaultSite();
					return;
				default:
					MessageBox.Show("Invalid password", "Security", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
#endif

			foreach (string site in AllowedSites) {
				// Note: AllowedSites (or at least the first entry) must be, in 
				//		 principle, mixed case. So just ToLower() it here, and don't
				//		 worry about over-optimizing things
				if (url.StartsWith(site.ToLower())) {
					bLetItThrough = true;
					return;
				}
			}
			// TODO: I think, instead of setting Cancel = true, we can just set the
			//		 <URL> variable to our new site and return. To be tried...
			Cancel = true;
			ODS("*** Blocked access to {0}", URL);
			string msg = "Sorry, but this kiosk allows you to browse only to the following site";
			if (AllowedSites.Count != 1)
				msg += "s";
			msg += Environment.NewLine + Environment.NewLine;
			foreach (string site in AllowedSites)
				msg += "    " + site + Environment.NewLine;
			MessageBox.Show(msg);

			GoToDefaultSite();
		}

//---------------------------------------------------------------------------------------

		private void GoToDefaultSite() {
			// Bring user back to main site
			object	oDefaultSite = DefaultSite;
			int		flags = 0;
			object	oFlags = (object)flags;
			object	oTargetFrameName = null;
			object	oPostData = null;
			object	oHeaders = null;
			web.Navigate2(ref oDefaultSite, ref oFlags, ref oTargetFrameName, ref oPostData, ref oHeaders);
		}

//---------------------------------------------------------------------------------------

		private void ProcessAdminScreen() {
			frmAdminSwitchboard sb = new frmAdminSwitchboard();
			sb.ShowDialog();
		}

//---------------------------------------------------------------------------------------

		void web_DocumentComplete(object pDisp, ref object URL) {
			ODS("    DocumentComplete - URL = {0}", URL);
			SHDocVw.WebBrowser web2 = pDisp as SHDocVw.WebBrowser;
			if (web2 == (web.Application as SHDocVw.WebBrowser)) {
				ODS("        DocumentComplete - isFinal");
				bLetItThrough = false;
			}
		}

//---------------------------------------------------------------------------------------

		void web_OnQuit() {
			// TODO: We can't seem to say, no, don't close. Hmmm.
			ODS("******* In OnQuit ********");

			// If (for example) we failed the registration check, let the user do 
			// whatever they want, including closing IE.
			if (AlwaysLetThrough)
				return;

			string	msg;
			string msgCantClose = "Without a proper password, you cannot close"
			+ " the browser. It will now restart.";

			DialogResult res = CheckPassword("close IE.", RegKey);

			switch (res) {
			default:
			// Anything we don't recognize, fall through to Cancel
			case DialogResult.Cancel:
				msg = msgCantClose;
				break;
			case DialogResult.No:
				msg = "The password you entered was incorrect. " + msgCantClose;
				break;
			case DialogResult.Yes:
				msg = "Password accepted. The browser will now close.";
				break;
			}

			MessageBox.Show(msg, "Security", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			if (res != DialogResult.Yes) {
				System.Diagnostics.Process.Start(Application.ExecutablePath);				
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Displays the GetPassword form and returns a bool if the user supplied it
		/// correctly.
		/// </summary>
		/// <returns></returns>
		private static DialogResult CheckPassword(string text, string RegKey) {
			GetPassword		gpw = new GetPassword(text, RegKey);
			DialogResult	res = gpw.ShowDialog();

			return res;
		}



//---------------------------------------------------------------------------------------

		private static void ODS(string fmt, params object[] parms) {
			// Note: I *could* make this [Conditional("DEBUG")]. But I think I'll keep it
			//		 in as it stands. It might be useful to be able to see any diagnostic
			//		 messages that are produced. Especially when it's out in the field
			//		 and we're trying to debug problems with it. Also, note that in no
			//		 way is this any kind of back door!
			string	msg = string.Format(fmt, parms);
			System.Diagnostics.Debugger.Log(0, "BartBlock", msg);
			// OutputDebugString(msg);
		}

//---------------------------------------------------------------------------------------

#if false	// Not used, except for debugging
		private string GetOurModuleName() {
			string s = "";
			RegistryKey	key = Registry.ClassesRoot.OpenSubKey(@"CLSID\{" + OurGUID + @"}\InprocServer32");
			if (key != null) {
				s = (string)key.GetValue("CodeBase");
			}
			return s;
		}
#endif

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Invoked by the runtime when registered as COM server.
		/// </summary>
		[ComRegisterFunction]
		private static void Register(Type t) {
			// System.Diagnostics.Debugger.Break();
			// Register BHO
			string guid = t.GUID.ToString("B");

			// TODO: Needs try/catch (SecurityException)
			RegistryKey rkey =
				Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects");
			// RegistryKey rkeyBHO = rkey.CreateSubKey(guid);
			rkey.CreateSubKey(guid);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Invoked when this COM server is unregistered.
		/// </summary>
		[ComUnregisterFunction]
		private static void Unregister(Type t) {
			// Unregister the BHO
			string guid = t.GUID.ToString("B");
			// TODO: Needs try/catch (SecurityException)
			RegistryKey rkey =
					Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Browser Helper Objects");
			rkey.DeleteSubKey(guid, false);
		}

//---------------------------------------------------------------------------------------

		#region IObjectWithSite Members
		public void SetSite(object pUnkSite) {
			// Get IWebBrowser2 reference
			if (web != null)
				web = null;				// Does a Release
			if (pUnkSite == null)
				return;

#if DEBUG
			// System.Diagnostics.Debugger.Break();
			// MessageBox.Show("In SetSite");
#endif
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

			GoToDefaultSite();
		}

//---------------------------------------------------------------------------------------

		public void GetSite(ref Guid riid, out object ppvSite) {
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
