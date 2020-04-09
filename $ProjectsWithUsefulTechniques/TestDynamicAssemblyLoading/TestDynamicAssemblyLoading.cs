using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestDynamicAssemblyLoading {
	public partial class TestDynamicAssemblyLoading : Form {
		string RefSource = @"G:\LRS\$Dev\C#-2016\DownloadFileProgess\bin\debug\DownloadFileProgess.dll";
		public TestDynamicAssemblyLoading() {
			InitializeComponent();

			var asm = Assembly.LoadFrom(RefSource);
			Type type = asm.GetType("DownloadFileProgress.DownloadFileProgress");
			var ti = type.GetTypeInfo();
			var methods = type.GetMethods();
			var xxxmsg = type.GetMethod("get_Handle", BindingFlags.NonPublic);
			var xxx = xxxmsg.Invoke(new object(), new object[] { "Hello dere" });
		}
	}
}
