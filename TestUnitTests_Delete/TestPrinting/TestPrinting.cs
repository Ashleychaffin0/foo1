using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPrinting {
	public partial class TestPrinting : Form {
		PrintDocument pDoc;

		public TestPrinting() {
			InitializeComponent();

			pDoc = new PrintDocument();
			pDoc.PrintPage += PDoc_PrintPage;
			pDoc.DocumentName = "Test print";
			pDoc.Print();
		}

		private void PDoc_PrintPage(object sender, PrintPageEventArgs e) {
			var font = new Font("Arial", 32);
			e.Graphics.DrawString("Hello printer", font, Brushes.Black, 200, 300);
			e.HasMorePages = false;
		}
	}
}
