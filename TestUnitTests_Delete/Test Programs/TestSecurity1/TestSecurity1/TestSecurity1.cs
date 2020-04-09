using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// Don't need all of these, but until I know what I'm doing,
// let's bring them all in.
using System.Security;
using System.Security.AccessControl;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;

namespace TestSecurity1 {
	public partial class TestSecurity1 : Form {
		public TestSecurity1() {
			InitializeComponent();
		}

		private void TestSecurity1_Load(object sender, EventArgs e) {
			System.Security.Permissions.FileIOPermission x = new FileIOPermission(PermissionState.Unrestricted);
			x.d
		}
	}
}