using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SysParms {
	public partial class SysParms : Form {
		public SysParms() {
			InitializeComponent();

			// var yyy = new SPI();

			// SPI.Init();
			var Config = SPI.GetConfig();
		}
	}
}
