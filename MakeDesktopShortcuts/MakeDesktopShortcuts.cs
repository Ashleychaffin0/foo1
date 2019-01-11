using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using IWshRuntimeLibrary;

// http://www.geekpedia.com/tutorial125_Create-shortcuts-with-a-.NET-application.html

// http://msdn.microsoft.com/en-us/library/bb776891%28VS.85%29.aspx

// From http://stackoverflow.com/questions/3906974/how-to-programmatically-create-a-shortcut-using-win32

/*
Parameters to CreateFile:
    pszTargetfile    - File name of the link's target.
    pszTargetargs    - Command line arguments passed to link's target.
    pszLinkfile      - File name of the actual link file being created.
    pszDescription   - Description of the linked item.
    iShowmode        - ShowWindow() constant for the link's target.
    pszCurdir        - Working directory of the active link. 
    pszIconfile      - File name of the icon file used for the link.
    iIconindex       - Index of the icon in the icon file.
*/

// From http://www.geekpedia.com/tutorial125_Create-shortcuts-with-a-.NET-application.html
// To set the icon for the link,
// add the (zero based) number of your icon to the path: 
// e.g. oLink:IconLocation:= sSystemPath +"\Shell32.dll,8"

namespace MakeDesktopShortcuts {
	public partial class MakeDesktopShortcuts : Form {
		public MakeDesktopShortcuts() {
			InitializeComponent();

			DoIt("Dell", @"D:\Dell", "Dell directory");
		}

//---------------------------------------------------------------------------------------

		private void DoIt(string LinkName, string TargetDir, string Description) {
			WshShell WshShell = new WshShell();

			IWshShortcut MyShortcut;
			string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string LinkPathName = Path.Combine(dir, LinkName + ".lnk");
			MyShortcut = (IWshRuntimeLibrary.IWshShortcut)WshShell.CreateShortcut(LinkPathName);

			MyShortcut.TargetPath = TargetDir;	//  Application.ExecutablePath;
			MyShortcut.Description = Description;
			// MyShortcut.IconLocation = ???;
			MyShortcut.Save();
        }

		void CreateGodModeIcon() {
			string GodMode = "GodMode.{ED7BA470-8E54-465E-825C-99712043E01C}";	// Windows 8
			string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string FolderName = Path.Combine(DesktopPath, GodMode);
			Directory.CreateDirectory(FolderName);
		}
	}
}
