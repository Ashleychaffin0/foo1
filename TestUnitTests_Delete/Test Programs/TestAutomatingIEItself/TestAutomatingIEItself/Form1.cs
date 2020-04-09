using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using mshtml;

namespace TestAutomatingIEItself {

	delegate void SetText(string text);
	delegate void MsgBox(string msg, string Caption, MessageBoxButtons buttons, MessageBoxIcon icon);

	public partial class Form1 : Form {

		[DllImport("user32.dll")]
		static extern IntPtr SetFocus(IntPtr hWnd);			// TODO: Not used

		[DllImport("user32.dll")]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
				int Y, int cx, int cy, uint uFlags);

		static readonly IntPtr HWND_TOPMOST		  = new IntPtr(-1);
		static readonly IntPtr HWND_NOTOPMOST	  = new IntPtr(-2);
		static readonly IntPtr HWND_TOP			  = IntPtr.Zero;
		static readonly IntPtr HWND_BOTTOM		  = new IntPtr(1);
/*
 * SetWindowPos Flags
 */
		static readonly uint  SWP_NOSIZE          = 0x0001;
		static readonly uint  SWP_NOMOVE          = 0x0002;
		static readonly uint  SWP_NOZORDER        = 0x0004;
		static readonly uint  SWP_NOREDRAW        = 0x0008;
		static readonly uint  SWP_NOACTIVATE      = 0x0010;
		static readonly uint  SWP_FRAMECHANGED    = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
		static readonly uint  SWP_SHOWWINDOW      = 0x0040;
		static readonly uint  SWP_HIDEWINDOW      = 0x0080;
		static readonly uint  SWP_NOCOPYBITS      = 0x0100;
		static readonly uint  SWP_NOOWNERZORDER   = 0x0200;  /* Don't do owner Z ordering */
		static readonly uint  SWP_NOSENDCHANGING  = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

		static readonly uint  SWP_DRAWFRAME       = SWP_FRAMECHANGED;
		static readonly uint  SWP_NOREPOSITION    = SWP_NOOWNERZORDER;

		static readonly uint  SWP_DEFERERASE      = 0x2000;
		static readonly uint  SWP_ASYNCWINDOWPOS  = 0x4000;


		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public enum SHOWWINDOW : int {
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_FORCEMINIMIZE = 11,
			SW_MAX = 11,
		}

		[DllImport("user32.dll", SetLastError = true)]
		static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam,
		   IntPtr lParam);

		private const uint WM_CLOSE = 0x0010;

		Timer	myTimer = new Timer();



		SHDocVw.InternetExplorerClass IE;

		MethodInvoker	dlgSetText;
		string			ConfNum;

		public Form1() {
			InitializeComponent();
			dlgSetText = new MethodInvoker(SetConfNum);
		}

		private void Navigate(string URL) {
			object zero = 0;
			object nil = null;
			IE.Navigate(URL, ref zero, ref nil, ref nil, ref nil);
		}

		private void Form1_Load(object sender, EventArgs e) {
			IE = new SHDocVw.InternetExplorerClass();

			// These are the only events we're interested in.
			IE.DocumentComplete += new SHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(IE_DocumentComplete);
			IE.OnQuit += new SHDocVw.DWebBrowserEvents2_OnQuitEventHandler(IE_OnQuit);

			// Definitions for these are #if'd out at the bottom of this file. They show proper casts
			// of some of the parameters
			// IE.NewWindow2 += new SHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(IE_NewWindow2);
			// IE.NavigateComplete2 += new SHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(IE_NavigateComplete2);

			// The following event doesn't seem to fire. Which is too bad since it would give us a chance
			// to *not* close the window if the user tried to close it in error. Oh well, maybe some day
			// we'll figure out a way to do it
			// Doesn't fire: IE.WindowClosing += new SHDocVw.DWebBrowserEvents2_WindowClosingEventHandler(IE_WindowClosing);

			// The following are examples of hooking in other events. The VS 2003/2005 
			// IDEs will generate skeleton code for you. For example (using the first 
			// event below), if you type in 
			//		"IE.Bef", hit {TAB}, Intellisense will fill in IE.BeforeNavigate. 
			//		If you then type in " += ", it will offer to fill in the rest. It 
			//		will also offer to create the skeleton IE_BeforeNavigate routine, 
			//		complete with proper parameters.
			// In other words, don't just uncomment the next lines. Type them anew and let the IDE do most
			// of the work.
			// IE.BeforeNavigate += new SHDocVw.DWebBrowserEvents_BeforeNavigateEventHandler(IE_BeforeNavigate);
			// IE.BeforeNavigate2 += new SHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(IE_BeforeNavigate2);
			// IE.FrameNavigateComplete += new SHDocVw.DWebBrowserEvents_FrameNavigateCompleteEventHandler(IE_FrameNavigateComplete);
			// IE.FrameNewWindow += new SHDocVw.DWebBrowserEvents_FrameNewWindowEventHandler(IE_FrameNewWindow);
			// IE.NavigateComplete += new SHDocVw.DWebBrowserEvents_NavigateCompleteEventHandler(IE_NavigateComplete);
			// IE.NavigateError += new SHDocVw.DWebBrowserEvents2_NavigateErrorEventHandler(IE_NavigateError);

			txtConfNum.Text = "***ConfNum***";

			Rectangle ScreenBounds = Screen.PrimaryScreen.Bounds;

			this.Top = 0;
			this.Left = 0;
			this.Width = ScreenBounds.Width;

			this.Visible = true;

			IE.Top = this.Height;
			IE.Left = 0;
			IE.Height = ScreenBounds.Height - IE.Top;
			IE.Width = ScreenBounds.Width;

			IE.Visible = true;
			// SetFocus(new IntPtr(IE.HWND));	// TODO: Probably not getting the right control.

			Navigate("www.signup4.net");

			Application.DoEvents();

			// System.Threading.Thread.Sleep(5000);		// TODO: Didn't work

			// TODO: Doesn't work here or below
			ShowWindow(new IntPtr(IE.HWND), (int)SHOWWINDOW.SW_SHOW);

			myTimer.Tick += new EventHandler(myTimer_Tick);
			myTimer.Interval = 1500;
			myTimer.Start();
		}

		void myTimer_Tick(object sender, EventArgs e) {
			ShowWindow(new IntPtr(IE.HWND), (int)SHOWWINDOW.SW_SHOW);
			myTimer.Stop();
			myTimer.Dispose();
		}

		void IE_DocumentComplete(object pDisp, ref object URL) {
			ShowWindow(new IntPtr(IE.HWND), (int)SHOWWINDOW.SW_SHOW);
			SHDocVw.InternetExplorer ie = (SHDocVw.InternetExplorer)pDisp;
			mshtml.HTMLDocumentClass doc = (HTMLDocumentClass)ie.Document;
			IHTMLElement elemConfNum = doc.getElementById("BadgeMaxConfirmationNumber");
			if (elemConfNum == null)
				return;
			ConfNum = (string)elemConfNum.getAttribute("value", 0);
			try {
				SetText setText = delegate(string text) { this.txtConfNum.Text = text; };
				txtConfNum.Invoke(setText, ConfNum);
			} catch (Exception) {
				// Silently ignore any errors. TODO: Put msg on status line
			}
		}

		private void btnGo_Click(object sender, EventArgs e) {
			// TODO: Navigate(txtURL.Text);
		}

		private void SetConfNum() {
			txtConfNum.Text = ConfNum;
		}

		void IE_OnQuit() {
			if (!this.InvokeRequired) {
				this.BringToFront();
			} else {
				SetWindowPos(new IntPtr(IE.HWND), HWND_BOTTOM, 0, 0, 0, 0,
					(SWP_NOSIZE | SWP_NOMOVE));
				MsgBox mb = delegate(string msg, string Caption, MessageBoxButtons buttons, MessageBoxIcon icon) {
					MessageBox.Show(msg, Caption, buttons, icon);
				};
				this.Invoke(mb, 
					"This copy of Internet Explorer is being controlled by BadgeMax Connects. Closing it forces BadgeMax Connects to also close.",
					"BadgeMax Connects", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}

			Application.Exit();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			// Close down IE
			PostMessage(new IntPtr(IE.HWND), WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
		}

		private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			// TODO: Not yet firing
			ShowWindow(new IntPtr(IE.HWND), (int)SHOWWINDOW.SW_SHOW);
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e) {
			// TODO: Fires, but doesn't work
			ShowWindow(new IntPtr(IE.HWND), (int)SHOWWINDOW.SW_SHOW);
		}

#if false
		void IE_NewWindow2(ref object ppDisp, ref bool Cancel) {
			// ppDisp = IE.Application;
		}

		void IE_NavigateComplete2(object pDisp, ref object URL) {
			SHDocVw.InternetExplorerClass ie = (SHDocVw.InternetExplorerClass)pDisp;
		}
#endif
	}
}