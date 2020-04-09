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

namespace GetSyncFusion {
	public partial class GetSyncFusion : Form {

		string FusionDir = @"G:\WpSystem\S-1-5-21-3637119393-2137292043-3037908665-1001\AppData\Local\Packages\SyncfusionInc.SyncfusionSuccinctlySeries_ashqhsdddaksg\LocalState";
		string TargetBaseDir = @"F:\lrs\SuccinctTemp";

		public GetSyncFusion() {
			InitializeComponent();
			Doit();
		}

		private void Doit() {
			var qry1 = from file in Directory.EnumerateFiles(FusionDir)
					   where file != "Syncfusion.db"
					   group file by Path.GetFileNameWithoutExtension(file) into grp
					   select grp.Key; 
			foreach (var filename in qry1) {
				ProcessEBook(filename);
			}
		}

		private void ProcessEBook(string filename) {
			var names = Directory.GetFiles(FusionDir, filename + ".*");
			string tgtdir = Path.Combine(TargetBaseDir, filename);
			Directory.CreateDirectory(tgtdir);
			foreach (var name in names) {
				Console.WriteLine($"Copying from {name}");
				Console.WriteLine($"Copying to   {Path.Combine(tgtdir, Path.GetFileName(name))}");
				var s = File.ReadAllText(name);
				var fi = File.GetAttributes(name);
				fi &= ~ FileAttributes.Encrypted;
				File.SetAttributes(name, fi);
				File.Copy(name, Path.Combine(tgtdir, Path.GetFileName(name)));
			}
		}
	}
}
