using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using LumiSoft.Net.Dns;

namespace DnsQuery
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button m_pQuery;
		private System.Windows.Forms.ComboBox m_pQType;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.TextBox m_pQName;
		private System.Windows.Forms.TextBox m_pDnsServer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			m_pQType.Items.Add("Host address (A)");
			m_pQType.Items.Add("Domain name pointer (PTR)");
			m_pQType.Items.Add("Mail exchange (MX)");
			m_pQType.SelectedIndex = 0;
		}

		#region method Dispose

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_pQuery = new System.Windows.Forms.Button();
			this.m_pQType = new System.Windows.Forms.ComboBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.m_pQName = new System.Windows.Forms.TextBox();
			this.m_pDnsServer = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_pQuery
			// 
			this.m_pQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_pQuery.Location = new System.Drawing.Point(360, 144);
			this.m_pQuery.Name = "m_pQuery";
			this.m_pQuery.Size = new System.Drawing.Size(64, 24);
			this.m_pQuery.TabIndex = 0;
			this.m_pQuery.Text = "Query";
			this.m_pQuery.Click += new System.EventHandler(this.m_pQuery_Click);
			// 
			// m_pQType
			// 
			this.m_pQType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_pQType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_pQType.Location = new System.Drawing.Point(200, 144);
			this.m_pQType.Name = "m_pQType";
			this.m_pQType.Size = new System.Drawing.Size(152, 21);
			this.m_pQType.TabIndex = 1;
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1,
																						this.columnHeader2,
																						this.columnHeader3,
																						this.columnHeader4});
			this.listView1.Location = new System.Drawing.Point(16, 16);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(408, 96);
			this.listView1.TabIndex = 2;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Value";
			this.columnHeader1.Width = 180;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Type";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "TTL";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Preference";
			this.columnHeader4.Width = 80;
			// 
			// m_pQName
			// 
			this.m_pQName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_pQName.Location = new System.Drawing.Point(16, 144);
			this.m_pQName.Name = "m_pQName";
			this.m_pQName.Size = new System.Drawing.Size(176, 20);
			this.m_pQName.TabIndex = 3;
			this.m_pQName.Text = "";
			// 
			// m_pDnsServer
			// 
			this.m_pDnsServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_pDnsServer.Location = new System.Drawing.Point(80, 120);
			this.m_pDnsServer.Name = "m_pDnsServer";
			this.m_pDnsServer.Size = new System.Drawing.Size(112, 20);
			this.m_pDnsServer.TabIndex = 4;
			this.m_pDnsServer.Text = "194.126.115.18";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(16, 120);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Dns server";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 181);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_pDnsServer);
			this.Controls.Add(this.m_pQName);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.m_pQType);
			this.Controls.Add(this.m_pQuery);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DnsQuery";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void m_pQuery_Click(object sender, System.EventArgs e)
		{
			try{
				DnsEx.DnsServers = new string[]{m_pDnsServer.Text};
				DnsEx dns = new DnsEx();

				listView1.Items.Clear();

				if(m_pQType.SelectedIndex == 0){
					A_Record[] records = dns.GetARecords(m_pQName.Text);
					foreach(A_Record rec in records){
						ListViewItem it = new ListViewItem();
						it.Text = rec.IP;
						it.SubItems.Add("1");
						it.SubItems.Add("");

						listView1.Items.Add(it);
					}
				}
				if(m_pQType.SelectedIndex == 1){
					PTR_Record[] records = dns.GetPTRRecords(m_pQName.Text);
					foreach(PTR_Record rec in records){
						ListViewItem it = new ListViewItem();
						it.Text = rec.DomainName;
						it.SubItems.Add("12");

						listView1.Items.Add(it);
					}
				}
				if(m_pQType.SelectedIndex == 2){
					MX_Record[] records = dns.GetMXRecords(m_pQName.Text);
					foreach(MX_Record rec in records){
						ListViewItem it = new ListViewItem();
						it.Text = rec.Host;
						it.SubItems.Add("13");
						it.SubItems.Add("");
						it.SubItems.Add(rec.Preference.ToString());

						listView1.Items.Add(it);
					}
				}
			}
			catch(Exception x){
				System.Windows.Forms.MessageBox.Show(this,"Error:" + x.Message,"Error:",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}
	}
}
