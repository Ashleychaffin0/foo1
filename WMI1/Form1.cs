using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;

namespace WMI1 {
	public partial class Form1 : Form {
		int		count = 0;

//---------------------------------------------------------------------------------------

		public Form1() {
			InitializeComponent();

			AddNamespacesToList();
		}

//---------------------------------------------------------------------------------------

		private void AddNamespacesToList() {
			try {
				// Enumerate all WMI instances of __namespace WMI class.
				ManagementClass nsClass = new ManagementClass(
				   new ManagementScope("root"),
				   new ManagementPath("__namespace"),
				   null);
				foreach (ManagementObject ns in nsClass.GetInstances()) {
					classList.Items.Add(ns["Name"].ToString());
					count++;
				}
				toolStripStatusLabel1.Text = count + " namespaces found.";
			} catch (Exception e) {
				toolStripStatusLabel1.Text = e.Message;
			}
		}

//---------------------------------------------------------------------------------------

		private void searchButton_Click(object sender, EventArgs e) {
			// Initialize class counter and clear list view.
			count = 0;
			classList.Items.Clear();
			Cursor	CurCurs = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			try {
				if (namespaceValue.Text.Equals("")) {
					AddNamespacesToList();
				} else {
					if (txtInstance.Text == "") {
						AddClassesToList();
					} else {
						AddDataToList();
					}
				}
			} finally {
				this.Cursor = CurCurs;
			}
		}

//---------------------------------------------------------------------------------------

		private void AddDataToList() {
			var MgmtScope = new ManagementScope(namespaceValue.Text);
				string	qry = "select * from " + txtInstance.Text;
				var qryObj = new WqlObjectQuery(qry);
				ManagementObjectSearcher searcher = new ManagementObjectSearcher(
						MgmtScope, qryObj, null);
				ManagementObjectCollection coll = searcher.Get();
				foreach (ManagementObject mo in coll) {
					Console.WriteLine("y={0}", mo);
					foreach (var item in mo.Qualifiers) {
						classList.Items.Add(string.Format("Qualifier : {0} = {1}", item.Name, item.Value));
					}
					classList.Items.Add("");
					foreach (var item in mo.Properties) {
						Application.DoEvents();
						classList.Items.Add(string.Format("Property : {0} = {1}", item.Name, item.Value));
						if (item.IsArray && item.Value != null) {
							// System.Diagnostics.Debugger.Break();
							// object [] objArray = item.Value as object[];
							// Type t = item.Value.GetType();
							Array a = item.Value as Array;
							foreach (var item2 in a) {
								classList.Items.Add("     " + item2.ToString());
							}
							
						}
					}
				}
		}

//---------------------------------------------------------------------------------------

		private void AddClassesToList() {
			try {
				// Perform WMI object query on selected namespace.
				var MgmtScope = new ManagementScope(namespaceValue.Text);
				string	qry = "select * from meta_class";
				var qryObj = new WqlObjectQuery(qry);
				ManagementObjectSearcher searcher = new ManagementObjectSearcher(
						MgmtScope, qryObj, null);
				ManagementObjectCollection coll = searcher.Get();
				List<string>	Classes = new List<string>();
				foreach (ManagementClass wmiClass in coll) {
					//classList.Items.Add(wmiClass["__CLASS"].ToString());
					Classes.Add(wmiClass["__CLASS"].ToString());
					count++;
				}
				Classes.Sort();
				foreach (string ClassName in Classes) {
					classList.Items.Add(ClassName);
				}
				toolStripStatusLabel1.Text = count + " classes found.";
			} catch (Exception ex) {
				toolStripStatusLabel1.Text = ex.Message;
			}
		}

//---------------------------------------------------------------------------------------

		private void classList_SelectedIndexChanged(object sender, EventArgs e) {
			txtInstance.Text = (string)classList.SelectedItem;
		}
	}
}
