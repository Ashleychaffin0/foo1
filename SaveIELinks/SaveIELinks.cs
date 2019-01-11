using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// See sample at http://h7labs.wordpress.com/2009/10/08/cenumerate-all-opened-windows/
// Also http://h7labs.wordpress.com/2009/10/08/cenumerate-all-opened-windows/
// Also http://www.vbforums.com/showthread.php?354809-Enumerate-Open-Windows-in-.NET


namespace SaveIELinks {
	public partial class SaveIELinks : Form {

		const int MAXTITLE = 255;

		[DllImport("user32.dll")]
		static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);
		private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
		// private delegate bool EnumWindowsProc(IntPtr hWnd, ref IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "GetWindowText",
		 ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int _GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

		[DllImport("user32.dll")]
		private static extern IntPtr GetDesktopWindow();

#if false
		private static extern bool EnumWindows(CallBackPtr lpEnumFunc, ArrayList lParam);
		static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

// Callback EnumWindowsProc should return true to continue enumerating or false to stop.


	private static extern int EnumWindows(CallBackPtr callPtr, int lPar); 

		[DllImport("user32.dll")]
 [return: MarshalAs(UnmanagedType.Bool)]
 static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);
public delegate bool EnumThreadDelegate (IntPtr hwnd, IntPtr lParam);
	 
	 // callBackPtr as a member variable so it doesnt GC while you're calling EnumWindows

	 
	EnumReport.EnumWindows(callBackPtr, 0);

	 
	  public static extern bool EnumWindows(CallBackPtr lpEnumFunc, ArrayList lParam);

	 
	 EnumWindows(callBackPtr, windowHandles);

	 // • Using a ref to that class in the EnumWindows() signature.
	 
		EnumWindows(new EnumWindowsProc(EnumProc), ref sd);

	 
	private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

	 
	private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);
#endif 

		List<string> Titles = new List<string>();

		public SaveIELinks() {
			InitializeComponent();

			Test();
		}

		private void Test() {
			IntPtr hWnd = GetDesktopWindow();
			bool bOK = EnumChildWindows(hWnd, MyCallback, new IntPtr(1729));
		}

		private bool MyCallback(IntPtr hWnd, IntPtr lParam) {
			if (hWnd == IntPtr.Zero) {
				return false;
			}
			var title = GetWindowText(hWnd);
			if ((title.EndsWith(" - Windows Internet Explorer")) || (title.EndsWith(" - Google Chrome"))) {
				Titles.Add(title);
			}
			return true;
		}

		public static string GetWindowText(IntPtr hWnd) {
			var strbTitle = new StringBuilder(MAXTITLE);
			int nLength = _GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
			strbTitle.Length = nLength;
			return strbTitle.ToString();
		}
	}
}
