using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

// TODO: Add Scan Another Page button
// TODO: Populate Printer combo box; select default printer

using WIA;

namespace ScanToLaserJet {
	public partial class ScanToLaserJet : Form {
		Bitmap bmp = null;
		public ScanToLaserJet() {
			InitializeComponent();
		}

		private void Form1_Load(Object sender, EventArgs e) {
			ScanNewPage();
		}

		private void ScanNewPage() {
			var CD = new WIA.CommonDialog();
			WIA.ImageFile f = CD.ShowAcquireImage(WIA.WiaDeviceType.ScannerDeviceType);
			byte[] BinData = f.FileData.get_BinaryData();
			var Msteam = new MemoryStream(BinData);
			bmp = new Bitmap(Msteam);
			PicBox.Image = bmp;
		}

		private void Pd_PrintPage(Object sender, PrintPageEventArgs e) {
			e.Graphics.DrawImage(bmp, new Point(0, 0));
		}

		private void btnPrint_Click(Object sender, EventArgs e) {
			var pd = new PrintDocument();
			pd.PrintPage += Pd_PrintPage;
			pd.Print();
		}

		private void BtnScanNewPage_Click(Object sender, EventArgs e) {
			ScanNewPage();
		}
	}
}
