using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ListDotNet {
	public partial class ListDotNet : Form {

		string DotNetDir = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319";
		string GacDir;

		TreeNode TopNode;

//---------------------------------------------------------------------------------------

		public ListDotNet() {
			InitializeComponent();

			string win = Path.GetDirectoryName(Environment.SystemDirectory);
			DotNetDir = Path.Combine(win, @"Microsoft.NET\Framework");
			GacDir = Path.Combine(win, @"Assembly");

			TopNode = new TreeNode("Files");
			treeView1.Nodes.Add(TopNode);
		}

//---------------------------------------------------------------------------------------

		private void ListDotNet_Load(object sender, EventArgs e) {
			// foreach (var filename in Directory.GetDirectories(DotNetDir)) {
			foreach (var filename in Directory.GetDirectories(GacDir)) {
				// lbFrameworks.Items.Add(filename.Substring(DotNetDir.Length + 1));
				lbFrameworks.Items.Add(filename.Substring(GacDir.Length + 1));
			}
		}

//---------------------------------------------------------------------------------------

		private void lbFrameworks_DoubleClick(object sender, EventArgs e) {
			var s = sender as ListBox;
			var item = s.SelectedItem;
			TopNode.Nodes.Clear();
			ProcessDir(item.ToString());
		}

//---------------------------------------------------------------------------------------

		private void ProcessDir(string DotNetVersionID) {
			listBox1.Items.Clear();
			var ad = AppDomain.CreateDomain("LRS");
			// string FullName = Path.Combine(DotNetDir, DotNetVersionID);
			string FullName = Path.Combine(GacDir, DotNetVersionID);
			Console.WriteLine("Managed files in {0}", FullName);
			Console.WriteLine("-----------------{0}", "".PadRight(FullName.Length, '-'));
			foreach (string filename in Directory.GetFiles(FullName, "*.dll", SearchOption.AllDirectories)) {
				Assembly asm = null;
				try {
					// var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(filename);
					// Console.WriteLine("File {0}, Desc={1}", filename, fvi.FileDescription);
					// if (fvi.FileDescription.Contains(".NET")) {
					if (true) {
						asm = Assembly.ReflectionOnlyLoadFrom(filename);
						if (asm != null) {
							TreeNode tn = new TreeNode(Path.GetFileNameWithoutExtension(filename));
							tn.Tag = filename;
							TopNode.Nodes.Add(tn);
							// Console.WriteLine("\t\t***************** Assembly loaded OK for {0}", filename);
							// listBox1.Items.Add(filename);
							// ListAsm(asm);
						} else {
							Console.WriteLine("\tAssembly loaded as (null) - {0}", filename);
						}
					}
				} catch (BadImageFormatException ex) {
					Console.WriteLine("\t$$$$$$$ Bad Image for {0}", filename);
				} catch {
					// Ignore. It's presumably a non-managed DLL.
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ListAsm(Assembly asm) {
			listBox1.Items.Add("\t" + asm.FullName);
			Application.DoEvents();
			var ExportedTypes = asm.GetExportedTypes();
			var ResourceNames = asm.GetManifestResourceNames();
			foreach (var name in ResourceNames) {
				ManifestResourceInfo x = asm.GetManifestResourceInfo(name);
				var filename = x.FileName;
				// Console.WriteLine("\tResouce '{0}' in {1}", name, filename ?? "(null)");
			}
		}

//---------------------------------------------------------------------------------------

		private void treeView1_Click(object sender, EventArgs e) {
			var tv = sender as TreeView;
			if (tv.SelectedNode != null) {
				var filename = tv.SelectedNode.Tag as string;
				if (filename != null) {
					var asm = Assembly.ReflectionOnlyLoadFrom(filename);
					tv.SelectedNode.Nodes.Clear();		// TODO: Optimize later
					var tys = asm.GetExportedTypes();
					var Types = from mod in asm.GetExportedTypes()
								  select mod;
					foreach (var ty in Types) {
						TreeNode tn = new TreeNode(ty.Name);
						tv.SelectedNode.Nodes.Add(tn);
					}
				}
			}
		}
	}
}
