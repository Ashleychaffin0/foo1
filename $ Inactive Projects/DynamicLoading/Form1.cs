using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DynamicLoading {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		Assembly	AsmLL = null;
		object		LLWS1 = null;
		string		LLWS1Name = null;

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			ShowAsms();
		}

//---------------------------------------------------------------------------------------

		private void btnClear_Click(object sender, EventArgs e) {
			lbMsgs.Items.Clear();
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			DialogResult res = ofd.ShowDialog();
			if (res == DialogResult.OK) {
				txtDLLName.Text = ofd.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnLoadDLL_Click(object sender, EventArgs e) {
			LLWS1Name = Path.GetFileName(txtDLLName.Text);
			AsmLL = Assembly.LoadFile(txtDLLName.Text);
			LLWS1 = AsmLL.CreateInstance("LLWS1");
		}

//---------------------------------------------------------------------------------------

		private void ShowAsms() {
			Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly asm in asms) {
				string	msg = string.Format("Fullname = {0}", asm.FullName);
				lbMsgs.Items.Add(msg);
			}
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
			txtDLLName.Text = Path.Combine(ofd.InitialDirectory, ofd.FileName);
		}

//---------------------------------------------------------------------------------------

		private void btnCall_Click(object sender, EventArgs e) {
			if (LLWS1 == null) {
				MessageBox.Show("Must Load a DLL first");
			}

			string			MachineName = null;
			int				ProcessorCount= 0;
			string			os = null;
			int				TickCount = 0;
			string			UserName = null;
			string			UserDomainName = null;
			DateTime		CurrentTime = default(DateTime);
			string			CLRVersion = null;
			string []		EnvironmentVars = null;

			MethodInfo m = AsmLL.GetType("LLWS1").GetMethod("GetSystemInfo");
			object [] result = new Object[] { 
				MachineName, ProcessorCount, os, TickCount, UserName, UserDomainName,
				CurrentTime, CLRVersion, EnvironmentVars
			};
			object ret = m.Invoke(LLWS1, result);
			Console.WriteLine("SampleMethod returned {0}.", ret);
		}


	}
}