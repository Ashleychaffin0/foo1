using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;

namespace TestCreateExcel {
	public partial class Form1 : Form {

		Excel.ApplicationClass	xl;
		object miss = Type.Missing;

//---------------------------------------------------------------------------------------

		public Form1() {
			InitializeComponent();

			xl = new Excel.ApplicationClass();
			xl.Visible = true;			// Mostly for debugging
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			string	Filename = string.Format("XL-{0:D}", DateTime.Now);
			Excel.Workbook wb = xl.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
			Excel.Worksheet	sheet = (Excel.Worksheet)wb.ActiveSheet;
			sheet.Name = "Ahmed";
			Excel.Range	r = (Excel.Range)sheet.Cells[1, 1];
			r.Value2 = "Hello";
			wb.SaveAs(Filename, miss, miss, miss, miss, miss, 
				Excel.XlSaveAsAccessMode.xlExclusive,
				miss, miss, miss, miss, miss);
		}

//---------------------------------------------------------------------------------------

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			xl.Quit();
		}
	}
}