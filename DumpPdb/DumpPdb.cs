using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Windows.Forms;

namespace DumpPdb {
	public partial class DumpPdb : Form {

		public static MemoryMappedViewAccessor	view;
		public static StreamWriter				OutFile;

//---------------------------------------------------------------------------------------

		public DumpPdb() {
			InitializeComponent();

			TxtPdbFilename.Text = @"C:\Program Files\Python36\python.pdb";
			TxtPdbFilename.Text = @"C:\ProgramData\dbg\sym\explorer.pdb\651169A1113DC523BDF3D53CD3539B5D1\explorer - Copy.pdb";
			TxtOutputFilename.Text = @"G:\lrs\fooPDB.txt";
		}

//---------------------------------------------------------------------------------------

		private void BtnBrowsePdb_Click(object sender, EventArgs e) {
			MessageBox.Show("Nonce on Browse for PDB file", "Dump PDB");
		}

//---------------------------------------------------------------------------------------

		private void BtnBrowseOutput_Click(object sender, EventArgs e) {
			MessageBox.Show("Nonce on Browse for Output file", "Dump PDB");

		}

		private void Msg(string txt) {
			LbMsgs.Items.Insert(0, txt);
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			using (var mmf = MemoryMappedFile.CreateFromFile(
				TxtPdbFilename.Text, FileMode.Open, "LRS",
					0, MemoryMappedFileAccess.Read)) {
				var fi = new FileInfo(TxtPdbFilename.Text);
				using (view = mmf.CreateViewAccessor(0, fi.Length, MemoryMappedFileAccess.Read)) {
					var MyView = new BasedView(view, 0);
					using (var OutFile = new StreamWriter(
						TxtOutputFilename.Text, false)) {
						DoTheDeed(MyView);
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void DoTheDeed(BasedView MyView) {
			var hdr = new PdbHeader(MyView);
			var ofs = hdr.BlockAddress(hdr.BlockMapAddr);
			var blockmap = new BlockMap(MyView, ofs, hdr);
			var StreamDir = new StreamDirectory(MyView, blockmap, hdr);
		}
	}
}
