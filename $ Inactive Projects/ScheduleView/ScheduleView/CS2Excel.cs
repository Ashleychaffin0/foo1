using System;
using System.Reflection;

using Excel;

namespace LRS {
	/// <summary>
	/// Summary description for CS2Excel.
	/// </summary>
	public class CS2Excel : IDisposable {

		string		FileName, SheetName;

		Application	oXL;
		Workbook	wkbook;

		public CS2Excel(string FileName, string SheetName) {
			this.FileName  = FileName;
			this.SheetName = SheetName;
			StartExcel(FileName);
			OpenFile(FileName);
			GoToSheet(SheetName);
		}

		public CS2Excel(string Filename) : this(Filename, null) {
			// Body deliberately empty
		}

		public CS2Excel() : this(null, null) {
			// Body deliberately empty
		}

		~CS2Excel() {
			Dispose();
		}

		void StartExcel(string FileName) {
			// TODO: Need try/catch
			oXL = new Excel.Application();
			// TODO: All the other routines really need checks
			//		 that oXL has been properly instantiated, and
			//		 throw an exception if not.
		}

		public void OpenFile(string FileName) {
			CloseCurrentFile();
			if (FileName != null)
				wkbook = oXL.Workbooks.Open(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		}

		public void CloseCurrentFile() {
			if (wkbook != null)
				wkbook.Close(false, Type.Missing, Type.Missing);
			// TODO: Actually close the file. Right now we just close all
			//		 the workbooks, which isn't (I don't think), the same thing.
			FileName = null;
		}

		public void GoToSheet(string SheetName) {
			// TODO:
		}

		public void Show(bool bShow) {
			oXL.Visible = bShow;
		}

		public void CloseAllWorkbooks() {
			foreach (Excel._Workbook book in oXL.Workbooks) {
				book.Close(false, Type.Missing, Type.Missing);
			}
		}

		public bool IsOpen() {
			return oXL != null;
		}

		#region IDisposable Members
		public void Dispose() {
			if (oXL != null) {
				CloseAllWorkbooks();
				oXL.Quit();
				oXL = null;
			}
		}
		#endregion
	}
}
