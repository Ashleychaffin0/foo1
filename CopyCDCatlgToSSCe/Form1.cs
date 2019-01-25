using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CopyCDCatlgToSSCe {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

//---------------------------------------------------------------------------------------

        private void albumsBindingNavigatorSaveItem_Click(object sender, EventArgs e) {
            this.Validate();
            this.albumsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this._CD_Catalog_in_C_DataSet);
        }

//---------------------------------------------------------------------------------------

        private void Form1_Load(object sender, EventArgs e) {
            // TODO: This line of code loads data into the '_CD_Catalog_in_C_DataSet.Albums' table. You can move, or remove it, as needed.
            this.albumsTableAdapter.Fill(this._CD_Catalog_in_C_DataSet.Albums);
        }
    }
}
