// Copyright (c) 2004, Bartizan Data Systems LLC

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.IO;


namespace FeetWetADO {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ADODotNet : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtSSN;
		private System.Windows.Forms.ComboBox cbDemogQues;
		private System.Windows.Forms.TextBox txtDemogAns;
		private System.Windows.Forms.TextBox txtDbLocation;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button cmdBrowse;
		private System.Windows.Forms.Button cmdGo;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ADODotNet() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

//---------------------------------------------------------------------------------------

		public void PopulateDemogQuesComboBox(String dbPath) {
			//create the database connection
			string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbPath;
			OleDbConnection aConnection = new OleDbConnection(ConnStr);

			//create the command object and store the sql query
			OleDbCommand aCommand = new OleDbCommand("SELECT DemogQuesStr FROM tblDemogQuestions", aConnection);

			try	{
				aConnection.Open();

				OleDbDataReader aReader = aCommand.ExecuteReader();
				
				//Iterate throuth the database
				while (aReader.Read()) {
					cbDemogQues.Items.Add(aReader.GetValue(0));
				}

				//close the reader 
				aReader.Close();

				aConnection.Close();
			} catch (OleDbException e) {
				Console.WriteLine("Error: {0}", e.Errors[0].Message);
			}
		}

//---------------------------------------------------------------------------------------

		private void cmdGo_Click(object sender, System.EventArgs e)	{
			string SSN;
			string DemogQuesStr;
			string SQL;
			string dbPath;

			SSN			 = txtSSN.Text;
			DemogQuesStr = cbDemogQues.Text;
			dbPath		 = txtDbLocation.Text;

			SQL  = "SELECT DemogAnsStr FROM tblPersonnelDemographics INNER JOIN";
			SQL += " (tblDemogQuestions INNER JOIN tblDemogAnswers ON tblDemogQuestions.DemogQuesID = tblDemogAnswers.DemogQuesID)";
			SQL += " ON tblPersonnelDemographics.DemogQuesID = tblDemogQuestions.DemogQuesID";
			SQL += " WHERE SSN ='" + SSN + "' AND DemogQuesStr ='" + DemogQuesStr + "'";
			

			string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbPath;
			OleDbConnection aConnection = new OleDbConnection(ConnStr);

			// create the command object and store the sql query
			OleDbCommand aCommand = new OleDbCommand(SQL, aConnection);

			try	{
				aConnection.Open();

				OleDbDataReader aReader = aCommand.ExecuteReader();
						
				if (aReader.Read()) {	
					// Note: We assume we've got something
					txtDemogAns.Text = (string)aReader.GetValue(0);
				}

				// close the reader 
				aReader.Close();

				// close the connection
				aConnection.Close();
			} catch (Exception ex) {
				// Note: We really should loop through all the Error's, creating a
				//		 nicely formatted error message. But for now.
				// Note: We could also just use ex.Message.
				string	msg;
				msg = string.Format("Error: {0}", ex.Message);
				MessageBox.Show(msg, "Feet Wet ADO.NET",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

#if false	
			try {
			} catch (Exception ex) {
				if (ex is OleDbException) {
					//
				} else
					throw;
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void cmdBrowse_Click(object sender, System.EventArgs e)	{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			
			openFileDialog1.InitialDirectory = "c:\\" ;
			openFileDialog1.Filter			 = "mdb files (*.mdb)|*.mdb|All files (*.*)|*.*" ;
			openFileDialog1.FilterIndex		 = 2;		// TODO: Why 2?
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
					txtDbLocation.Text  = openFileDialog1.FileName;
				
			}
			
			if (txtDbLocation.Text.Length != 0)	{
				PopulateDemogQuesComboBox(txtDbLocation.Text);
			}
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)	{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.txtSSN = new System.Windows.Forms.TextBox();
			this.cbDemogQues = new System.Windows.Forms.ComboBox();
			this.txtDemogAns = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtDbLocation = new System.Windows.Forms.TextBox();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.cmdGo = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// txtSSN
			// 
			this.txtSSN.Location = new System.Drawing.Point(24, 120);
			this.txtSSN.Name = "txtSSN";
			this.txtSSN.Size = new System.Drawing.Size(128, 20);
			this.txtSSN.TabIndex = 0;
			this.txtSSN.Text = "003-54-3576";
			// 
			// cbDemogQues
			// 
			this.cbDemogQues.Location = new System.Drawing.Point(192, 120);
			this.cbDemogQues.Name = "cbDemogQues";
			this.cbDemogQues.Size = new System.Drawing.Size(384, 21);
			this.cbDemogQues.TabIndex = 1;
			// 
			// txtDemogAns
			// 
			this.txtDemogAns.Location = new System.Drawing.Point(24, 192);
			this.txtDemogAns.Name = "txtDemogAns";
			this.txtDemogAns.Size = new System.Drawing.Size(640, 20);
			this.txtDemogAns.TabIndex = 2;
			this.txtDemogAns.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "SSN";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(192, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(232, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Demog Question";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 168);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(208, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Demog Answer";
			// 
			// txtDbLocation
			// 
			this.txtDbLocation.Location = new System.Drawing.Point(24, 48);
			this.txtDbLocation.Name = "txtDbLocation";
			this.txtDbLocation.Size = new System.Drawing.Size(544, 20);
			this.txtDbLocation.TabIndex = 6;
			this.txtDbLocation.Text = "";
			// 
			// cmdBrowse
			// 
			this.cmdBrowse.Location = new System.Drawing.Point(584, 48);
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.Size = new System.Drawing.Size(80, 24);
			this.cmdBrowse.TabIndex = 7;
			this.cmdBrowse.Text = "Browse";
			this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(224, 16);
			this.label4.TabIndex = 8;
			this.label4.Text = "Database Location";
			// 
			// cmdGo
			// 
			this.cmdGo.Location = new System.Drawing.Point(584, 120);
			this.cmdGo.Name = "cmdGo";
			this.cmdGo.Size = new System.Drawing.Size(80, 24);
			this.cmdGo.TabIndex = 9;
			this.cmdGo.Text = "Go";
			this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
			// 
			// ADODotNet
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 282);
			this.Controls.Add(this.cmdGo);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cmdBrowse);
			this.Controls.Add(this.txtDbLocation);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtDemogAns);
			this.Controls.Add(this.cbDemogQues);
			this.Controls.Add(this.txtSSN);
			this.Name = "ADODotNet";
			this.Text = "ADO ";
			this.ResumeLayout(false);

		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new ADODotNet());
		}
		#endregion	
	}
}
