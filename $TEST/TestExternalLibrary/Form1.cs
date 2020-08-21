using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// See https://stackoverflow.com/questions/40108106/using-external-dll-in-dot-net-core

using LRSNativeMethodsNamespace;

namespace TestExternalLibrary {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
			
			LRSNativeMethods.AllocConsole();
			Console.WriteLine("Hi there, world!");
			Debug.WriteLine("Hi from Debug");
		}

	}
}
