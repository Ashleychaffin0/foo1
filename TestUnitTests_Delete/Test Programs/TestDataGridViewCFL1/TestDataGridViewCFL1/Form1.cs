using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

// TODO: Note: All fields are public for this test file

namespace TestDataGridViewCFL1 {


    public partial class Form1 : Form {

        MultiField mf;
        CFL cfl;

		ComboBox	combo1, combo2;

        public Form1() {
            InitializeComponent();

			dgv.Columns.Add("Target", "Field");
			dgv.Columns.Add("Delimeter", "Delimeter");
			dgv.Columns.Add("AddSpaceAtEnd", "Add Space At End");

            cfl = new CFL();

            mf = new MultiField("FirstLast");
            mf.Add(new SingleField(new DataTarget("tblPerson", "First"), "", true));
            mf.Add(new SingleField(new DataTarget("tblPerson", "Last"), ", ", false));
            cfl.Add(mf);

            mf = new MultiField("CityStateZip");
            mf.Add(new SingleField(new DataTarget("tblPerson", "City"), "", true));
            mf.Add(new SingleField(new DataTarget("tblPerson", "State"), ", ", false));
            mf.Add(new SingleField(new DataTarget("tblPerson", "Zip"), ", ", false));
            cfl.Add(mf);

            BindingSource bndsrc = new BindingSource();
            bndsrc.DataSource = mf;
            //bndsrc.DataMember = "Fields";
            dgv.DataSource = bndsrc;
			// dgv.Refresh();
            // dgv.DataSource = cfl;
            // dgv.DataMember = cfl.CFLs;

			BindingSource	bs2 = new BindingSource();
			LRS				lrs = new LRS();
			List<int>		li  = new List<int>();
			li.Add(1);
			li.Add(2);
			li.Add(54);
			bs2.DataSource = li;

			BindingSource	bs3 = new BindingSource();
			BindingList<int>	li3 = new BindingList<int>();
			li3.Add(1);
			li3.Add(2);
			li3.Add(54);
			bs3.DataSource = li3;

			// dgv.AutoGenerateColumns = true;
			// dgv.DataSource = bs3;

			combo1 = new ComboBox();
			combo1.Parent = this;
			combo1.AutoSize = true;
			combo1.DropDownStyle = ComboBoxStyle.DropDownList;
			combo1.Width = 12 * Font.Height;
			combo1.DataSource = KnownColorClass.KnownColorArray;
			combo1.ValueMember = "Color";
			combo1.DisplayMember = "Name";
			// combo1.Click += new EventHandler(combo_Click);
			combo1.SelectionChangeCommitted += new EventHandler(combo_SelectionChangeCommitted);

			//combo1.DataBindings.Add("SelectedValue", this, "BackColor");
			//combo1.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

			combo2 = new ComboBox();
			combo2.Parent = this;
			combo2.AutoSize = true;
			combo2.Location = new Point(200, 0);
			combo2.DropDownStyle = ComboBoxStyle.DropDownList;
			combo2.Width = 12 * Font.Height;
			combo2.DataSource = KnownColorClass.KnownColorArray;
			combo2.ValueMember = "Color";
			combo2.DisplayMember = "Name";
			combo2.SelectionChangeCommitted += new EventHandler(combo_SelectionChangeCommitted);
			// combo2.Click += new EventHandler(combo_Click);

			//combo2.DataBindings.Add("SelectedValue", this, "BackColor");
			//combo2.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

			comboBox1.Items.Add("One");
 			comboBox1.Items.Add("Two");
			comboBox1.Items.Add("Three");

			checkedListBox1.Items.Add("xxx");
 			checkedListBox1.Items.Add("yyy");
 			checkedListBox1.Items.Add("zzz");
      }

		void combo_SelectionChangeCommitted(object sender, EventArgs e) {
			this.Invalidate(this.ClientRectangle);
		}

		void combo_Click(object sender, EventArgs e) {
			this.Invalidate(this.ClientRectangle);
		}

		private void textBox1_TextChanged(object sender, EventArgs e) {

		}

		private void checkBox1_TextChanged(object sender, EventArgs e) {

		}

		private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e) {

		}

		private void notifyIcon1_BalloonTipClosed(object sender, EventArgs e) {

		}

		private void notifyIcon1_BalloonTipShown(object sender, EventArgs e) {

		}

		private void notifyIcon1_Click(object sender, EventArgs e) {
			MessageBox.Show("mb hi from LRS");
		}

		private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e) {
			MessageBox.Show("ComboBox = SelectionChangeCommitted");
			this.Invalidate();
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e) {
			MessageBox.Show("CombdateTimePickeroBox = ValueChanged");
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e) {
			MessageBox.Show("checkBox = CheckedChanged");
		}

		private void checkBox1_CheckStateChanged(object sender, EventArgs e) {
			MessageBox.Show("checkBox = CheckStateChanged");
		}

		private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e) {
			MessageBox.Show("checkedListBox1_ItemCheck = ItemCheck");
		}

		private void Form1_Paint(object sender, PaintEventArgs e) {
			Color	color1, color2;
			color1 = ((KnownColorClass)combo1.SelectedItem).Color;
			color2 = ((KnownColorClass)combo2.SelectedItem).Color;
			LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, color1, color2, 45f);
			e.Graphics.FillRectangle(brush, this.ClientRectangle);
		}
    }

	public class KnownColorClass {
		KnownColor	kc;

		public KnownColorClass(KnownColor kc) {
			this.kc = kc;
		}

		public Color Color {
			get { return Color.FromKnownColor(kc); }
		}

		public string Name {
			get {
				string str = Enum.GetName(typeof(KnownColor), kc);

				for (int i = 1; i < str.Length; i++) {
					if (Char.IsUpper(str[i]))
						str = str.Insert(i++, " ");
				}
				return str;
			}
		}

		public static KnownColorClass[] KnownColorArray {
			get {
				// Create an array of KnownColorClass objects
				KnownColor [] akc = (KnownColor [])Enum.GetValues(typeof(KnownColor));
				KnownColorClass [] akcc = new KnownColorClass[akc.Length];

				for (int i = 0; i < akc.Length; i++) {
					akcc[i] = new KnownColorClass(akc[i]);
				}
				return akcc;
			}
		}
	}

	public class LRS {
		List<int>	ints;

		public LRS() {
			ints = new List<int>();
			ints.Add(3);
			ints.Add(1);
			ints.Add(4);
		}
	}

    public class DataTarget {
        public string   TableName, FieldName;

        public DataTarget(string TableName, string FieldName) {
            this.TableName = TableName;
            this.FieldName = FieldName;
        }
    }

    public class MultiField {
        public string name;
        public List<SingleField> Fields;

        public  MultiField(string name) {
            this.name = name;
            Fields = new List<SingleField>();
        }

        public void Add(SingleField field) {
            Fields.Add(field);
        }
    }

    public class SingleField {
        public DataTarget target;
        public string Delimeter;
        public bool AddSpaceAtEnd;

        public SingleField(DataTarget target, string Delimeter, bool AddSpaceAtEnd) {
            this.target = target;
            this.Delimeter = Delimeter;
            this.AddSpaceAtEnd = AddSpaceAtEnd;
        }
    }

    public class CFL {
        public List<MultiField> CFLs;

        public CFL() {
            CFLs = new List<MultiField>();
        }

        public void Add(MultiField mf) {
            CFLs.Add(mf);
        }
    }
}