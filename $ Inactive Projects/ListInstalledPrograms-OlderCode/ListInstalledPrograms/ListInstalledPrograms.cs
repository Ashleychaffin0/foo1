using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

namespace ListInstalledPrograms {
    public partial class ListInstalledPrograms : Form {
        public ListInstalledPrograms() {
            InitializeComponent();
        }

        private void ListInstalledPrograms_Load(object sender, EventArgs e) {
            string  instbase = @"Software\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18";
            RegistryKey key = Registry.LocalMachine;
            RegistryKey apps = key.OpenSubKey(instbase, false);
        }
    }
}