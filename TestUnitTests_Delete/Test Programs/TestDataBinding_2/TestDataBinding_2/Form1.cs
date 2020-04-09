using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace TestDataBinding_2	{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtArtist;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button BtnForward;
		private System.Windows.Forms.TextBox txtAlbumName;
		private System.Windows.Forms.Label label2;
		private System.Data.OleDb.OleDbConnection oleDbConnection1;
		private System.Data.OleDb.OleDbCommand oleDbSelectCommand1;
		private System.Data.OleDb.OleDbCommand oleDbInsertCommand1;
		private System.Data.OleDb.OleDbCommand oleDbUpdateCommand1;
		private System.Data.OleDb.OleDbCommand oleDbDeleteCommand1;
		private TestDataBinding_2.DataSet1 dsAlbums;
		private System.Data.OleDb.OleDbDataAdapter daAlbums;

		const string DBFilename = @"C:\LRS\Test - CD Catalog in C#.mdb";
		private System.Windows.Forms.Label lblOrdersPosition;
		private System.Windows.Forms.TextBox txtCategory;
		private System.Windows.Forms.Label label3;

		CurrencyManager cmOrders;


		public Form1() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			cmOrders = (CurrencyManager) BindingContext[dsAlbums, "Albums"];
			cmOrders.ItemChanged += 
				new ItemChangedEventHandler(cmOrders_ItemChanged);
			cmOrders.PositionChanged += new EventHandler(cmOrders_PositionChanged);
			DisplayOrdersPosition();
			daAlbums.Fill(dsAlbums, "Albums");
		}

		private void DisplayOrdersPosition() {
			lblOrdersPosition.Text = "Order " + (cmOrders.Position + 1) + 
				" of " + cmOrders.Count;
		}

		private void cmOrders_ItemChanged(object sender, ItemChangedEventArgs e) {
			DisplayOrdersPosition();
		}

		private void cmOrders_PositionChanged(object sender, EventArgs e) {
			DisplayOrdersPosition();
		}

		private void btnOrdersMoveFirst_Click(object sender, System.EventArgs e) {
			cmOrders.Position = 0;
		}

		private void btnOrdersMovePrevious_Click(object sender, System.EventArgs e) {
			cmOrders.Position--;
		}

		private void btnOrdersMoveNext_Click(object sender, System.EventArgs e) {
			cmOrders.Position++;
		}

		private void btnOrdersMoveLast_Click(object sender, System.EventArgs e) {
			cmOrders.Position = cmOrders.Count - 1;
		}
		

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.txtArtist = new System.Windows.Forms.TextBox();
			this.dsAlbums = new TestDataBinding_2.DataSet1();
			this.btnBack = new System.Windows.Forms.Button();
			this.BtnForward = new System.Windows.Forms.Button();
			this.txtAlbumName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.oleDbConnection1 = new System.Data.OleDb.OleDbConnection();
			this.oleDbSelectCommand1 = new System.Data.OleDb.OleDbCommand();
			this.oleDbInsertCommand1 = new System.Data.OleDb.OleDbCommand();
			this.oleDbUpdateCommand1 = new System.Data.OleDb.OleDbCommand();
			this.oleDbDeleteCommand1 = new System.Data.OleDb.OleDbCommand();
			this.daAlbums = new System.Data.OleDb.OleDbDataAdapter();
			this.lblOrdersPosition = new System.Windows.Forms.Label();
			this.txtCategory = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dsAlbums)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(40, 72);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Artist";
			// 
			// txtArtist
			// 
			this.txtArtist.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsAlbums, "Albums.Artist"));
			this.txtArtist.Location = new System.Drawing.Point(184, 72);
			this.txtArtist.Name = "txtArtist";
			this.txtArtist.Size = new System.Drawing.Size(200, 22);
			this.txtArtist.TabIndex = 1;
			this.txtArtist.Text = "textBox1";
			// 
			// dsAlbums
			// 
			this.dsAlbums.DataSetName = "dataSet1";
			this.dsAlbums.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// btnBack
			// 
			this.btnBack.Location = new System.Drawing.Point(440, 8);
			this.btnBack.Name = "btnBack";
			this.btnBack.TabIndex = 2;
			this.btnBack.Text = "<";
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// BtnForward
			// 
			this.BtnForward.Location = new System.Drawing.Point(528, 8);
			this.BtnForward.Name = "BtnForward";
			this.BtnForward.TabIndex = 3;
			this.BtnForward.Text = ">";
			this.BtnForward.Click += new System.EventHandler(this.BtnForward_Click);
			// 
			// txtAlbumName
			// 
			this.txtAlbumName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsAlbums, "Albums.Title"));
			this.txtAlbumName.Location = new System.Drawing.Point(184, 112);
			this.txtAlbumName.Name = "txtAlbumName";
			this.txtAlbumName.Size = new System.Drawing.Size(408, 22);
			this.txtAlbumName.TabIndex = 5;
			this.txtAlbumName.Text = "textBox2";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 112);
			this.label2.Name = "label2";
			this.label2.TabIndex = 4;
			this.label2.Text = "Album Name";
			// 
			// oleDbConnection1
			// 
			this.oleDbConnection1.ConnectionString = @"Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Registry Path=;Jet OLEDB:Database Locking Mode=1;Data Source=""C:\LRS\Test - CD Catalog in C#.mdb"";Jet OLEDB:Engine Type=5;Provider=""Microsoft.Jet.OLEDB.4.0"";Jet OLEDB:System database=;Jet OLEDB:SFP=False;persist security info=False;Extended Properties=;Mode=Share Deny None;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Create System Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;User ID=Admin;Jet OLEDB:Global Bulk Transactions=1";
			// 
			// oleDbSelectCommand1
			// 
			this.oleDbSelectCommand1.CommandText = @"SELECT [Album ID], Artist, Category, Comments, Cuts, DiscID, Extra, Label, LargeCoverURL, MoreInfoURL, [Need Cut Names Expanded], [Not Found on web], Owner, PlayLengthInSeconds, [Release Date], Site, SmallCoverURL, Style, Title, TotalLengthInFrames, TotalLengthInSeconds, TotalTime, Vetted, XML, XML2 FROM Albums ORDER BY Category, Artist, Title";
			this.oleDbSelectCommand1.Connection = this.oleDbConnection1;
			// 
			// oleDbInsertCommand1
			// 
			this.oleDbInsertCommand1.CommandText = @"INSERT INTO Albums([Album ID], Artist, Category, Comments, Cuts, DiscID, Extra, Label, LargeCoverURL, MoreInfoURL, [Need Cut Names Expanded], [Not Found on web], Owner, PlayLengthInSeconds, [Release Date], Site, SmallCoverURL, Style, Title, TotalLengthInFrames, TotalLengthInSeconds, TotalTime, Vetted, XML, XML2) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
			this.oleDbInsertCommand1.Connection = this.oleDbConnection1;
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Album_ID", System.Data.OleDb.OleDbType.Integer, 0, "Album ID"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Artist", System.Data.OleDb.OleDbType.VarWChar, 70, "Artist"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Category", System.Data.OleDb.OleDbType.VarWChar, 25, "Category"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Comments", System.Data.OleDb.OleDbType.VarWChar, 0, "Comments"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Cuts", System.Data.OleDb.OleDbType.Integer, 0, "Cuts"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("DiscID", System.Data.OleDb.OleDbType.Integer, 0, "DiscID"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Extra", System.Data.OleDb.OleDbType.VarWChar, 0, "Extra"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Label", System.Data.OleDb.OleDbType.VarWChar, 30, "Label"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("LargeCoverURL", System.Data.OleDb.OleDbType.VarWChar, 0, "LargeCoverURL"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("MoreInfoURL", System.Data.OleDb.OleDbType.VarWChar, 0, "MoreInfoURL"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Need_Cut_Names_Expanded", System.Data.OleDb.OleDbType.Boolean, 2, "Need Cut Names Expanded"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Not_Found_on_web", System.Data.OleDb.OleDbType.Boolean, 2, "Not Found on web"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Owner", System.Data.OleDb.OleDbType.VarWChar, 1, "Owner"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("PlayLengthInSeconds", System.Data.OleDb.OleDbType.Integer, 0, "PlayLengthInSeconds"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Release_Date", System.Data.OleDb.OleDbType.VarWChar, 10, "Release Date"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Site", System.Data.OleDb.OleDbType.VarWChar, 20, "Site"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("SmallCoverURL", System.Data.OleDb.OleDbType.VarWChar, 0, "SmallCoverURL"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Style", System.Data.OleDb.OleDbType.VarWChar, 15, "Style"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Title", System.Data.OleDb.OleDbType.VarWChar, 100, "Title"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("TotalLengthInFrames", System.Data.OleDb.OleDbType.Integer, 0, "TotalLengthInFrames"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("TotalLengthInSeconds", System.Data.OleDb.OleDbType.Integer, 0, "TotalLengthInSeconds"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("TotalTime", System.Data.OleDb.OleDbType.DBDate, 0, "TotalTime"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Vetted", System.Data.OleDb.OleDbType.Boolean, 2, "Vetted"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("XML", System.Data.OleDb.OleDbType.VarWChar, 0, "XML"));
			this.oleDbInsertCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("XML2", System.Data.OleDb.OleDbType.VarWChar, 0, "XML2"));
			// 
			// oleDbUpdateCommand1
			// 
			this.oleDbUpdateCommand1.CommandText = @"UPDATE Albums SET [Album ID] = ?, Artist = ?, Category = ?, Comments = ?, Cuts = ?, DiscID = ?, Extra = ?, Label = ?, LargeCoverURL = ?, MoreInfoURL = ?, [Need Cut Names Expanded] = ?, [Not Found on web] = ?, Owner = ?, PlayLengthInSeconds = ?, [Release Date] = ?, Site = ?, SmallCoverURL = ?, Style = ?, Title = ?, TotalLengthInFrames = ?, TotalLengthInSeconds = ?, TotalTime = ?, Vetted = ?, XML = ?, XML2 = ? WHERE (Artist = ?) AND (DiscID = ?) AND (Title = ?) AND ([Album ID] = ? OR ? IS NULL AND [Album ID] IS NULL) AND (Category = ? OR ? IS NULL AND Category IS NULL) AND (Cuts = ? OR ? IS NULL AND Cuts IS NULL) AND (Label = ? OR ? IS NULL AND Label IS NULL) AND ([Need Cut Names Expanded] = ?) AND ([Not Found on web] = ?) AND (Owner = ? OR ? IS NULL AND Owner IS NULL) AND (PlayLengthInSeconds = ? OR ? IS NULL AND PlayLengthInSeconds IS NULL) AND ([Release Date] = ? OR ? IS NULL AND [Release Date] IS NULL) AND (Site = ? OR ? IS NULL AND Site IS NULL) AND (Style = ? OR ? IS NULL AND Style IS NULL) AND (TotalLengthInFrames = ? OR ? IS NULL AND TotalLengthInFrames IS NULL) AND (TotalLengthInSeconds = ? OR ? IS NULL AND TotalLengthInSeconds IS NULL) AND (TotalTime = ? OR ? IS NULL AND TotalTime IS NULL) AND (Vetted = ?)";
			this.oleDbUpdateCommand1.Connection = this.oleDbConnection1;
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Album_ID", System.Data.OleDb.OleDbType.Integer, 0, "Album ID"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Artist", System.Data.OleDb.OleDbType.VarWChar, 70, "Artist"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Category", System.Data.OleDb.OleDbType.VarWChar, 25, "Category"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Comments", System.Data.OleDb.OleDbType.VarWChar, 0, "Comments"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Cuts", System.Data.OleDb.OleDbType.Integer, 0, "Cuts"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("DiscID", System.Data.OleDb.OleDbType.Integer, 0, "DiscID"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Extra", System.Data.OleDb.OleDbType.VarWChar, 0, "Extra"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Label", System.Data.OleDb.OleDbType.VarWChar, 30, "Label"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("LargeCoverURL", System.Data.OleDb.OleDbType.VarWChar, 0, "LargeCoverURL"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("MoreInfoURL", System.Data.OleDb.OleDbType.VarWChar, 0, "MoreInfoURL"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Need_Cut_Names_Expanded", System.Data.OleDb.OleDbType.Boolean, 2, "Need Cut Names Expanded"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Not_Found_on_web", System.Data.OleDb.OleDbType.Boolean, 2, "Not Found on web"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Owner", System.Data.OleDb.OleDbType.VarWChar, 1, "Owner"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("PlayLengthInSeconds", System.Data.OleDb.OleDbType.Integer, 0, "PlayLengthInSeconds"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Release_Date", System.Data.OleDb.OleDbType.VarWChar, 10, "Release Date"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Site", System.Data.OleDb.OleDbType.VarWChar, 20, "Site"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("SmallCoverURL", System.Data.OleDb.OleDbType.VarWChar, 0, "SmallCoverURL"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Style", System.Data.OleDb.OleDbType.VarWChar, 15, "Style"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Title", System.Data.OleDb.OleDbType.VarWChar, 100, "Title"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("TotalLengthInFrames", System.Data.OleDb.OleDbType.Integer, 0, "TotalLengthInFrames"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("TotalLengthInSeconds", System.Data.OleDb.OleDbType.Integer, 0, "TotalLengthInSeconds"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("TotalTime", System.Data.OleDb.OleDbType.DBDate, 0, "TotalTime"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Vetted", System.Data.OleDb.OleDbType.Boolean, 2, "Vetted"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("XML", System.Data.OleDb.OleDbType.VarWChar, 0, "XML"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("XML2", System.Data.OleDb.OleDbType.VarWChar, 0, "XML2"));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Artist", System.Data.OleDb.OleDbType.VarWChar, 70, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Artist", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_DiscID", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "DiscID", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Title", System.Data.OleDb.OleDbType.VarWChar, 100, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Album_ID", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Album ID", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Album_ID1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Album ID", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Category", System.Data.OleDb.OleDbType.VarWChar, 25, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Category", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Category1", System.Data.OleDb.OleDbType.VarWChar, 25, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Category", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Cuts", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Cuts", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Cuts1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Cuts", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Label", System.Data.OleDb.OleDbType.VarWChar, 30, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Label", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Label1", System.Data.OleDb.OleDbType.VarWChar, 30, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Label", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Need_Cut_Names_Expanded", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Need Cut Names Expanded", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Not_Found_on_web", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Not Found on web", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Owner", System.Data.OleDb.OleDbType.VarWChar, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Owner", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Owner1", System.Data.OleDb.OleDbType.VarWChar, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Owner", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_PlayLengthInSeconds", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PlayLengthInSeconds", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_PlayLengthInSeconds1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PlayLengthInSeconds", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Release_Date", System.Data.OleDb.OleDbType.VarWChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Release Date", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Release_Date1", System.Data.OleDb.OleDbType.VarWChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Release Date", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Site", System.Data.OleDb.OleDbType.VarWChar, 20, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Site", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Site1", System.Data.OleDb.OleDbType.VarWChar, 20, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Site", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Style", System.Data.OleDb.OleDbType.VarWChar, 15, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Style", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Style1", System.Data.OleDb.OleDbType.VarWChar, 15, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Style", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalLengthInFrames", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalLengthInFrames", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalLengthInFrames1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalLengthInFrames", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalLengthInSeconds", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalLengthInSeconds", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalLengthInSeconds1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalLengthInSeconds", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalTime", System.Data.OleDb.OleDbType.DBDate, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalTime", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalTime1", System.Data.OleDb.OleDbType.DBDate, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalTime", System.Data.DataRowVersion.Original, null));
			this.oleDbUpdateCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Vetted", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Vetted", System.Data.DataRowVersion.Original, null));
			// 
			// oleDbDeleteCommand1
			// 
			this.oleDbDeleteCommand1.CommandText = @"DELETE FROM Albums WHERE (Artist = ?) AND (DiscID = ?) AND (Title = ?) AND ([Album ID] = ? OR ? IS NULL AND [Album ID] IS NULL) AND (Category = ? OR ? IS NULL AND Category IS NULL) AND (Cuts = ? OR ? IS NULL AND Cuts IS NULL) AND (Label = ? OR ? IS NULL AND Label IS NULL) AND ([Need Cut Names Expanded] = ?) AND ([Not Found on web] = ?) AND (Owner = ? OR ? IS NULL AND Owner IS NULL) AND (PlayLengthInSeconds = ? OR ? IS NULL AND PlayLengthInSeconds IS NULL) AND ([Release Date] = ? OR ? IS NULL AND [Release Date] IS NULL) AND (Site = ? OR ? IS NULL AND Site IS NULL) AND (Style = ? OR ? IS NULL AND Style IS NULL) AND (TotalLengthInFrames = ? OR ? IS NULL AND TotalLengthInFrames IS NULL) AND (TotalLengthInSeconds = ? OR ? IS NULL AND TotalLengthInSeconds IS NULL) AND (TotalTime = ? OR ? IS NULL AND TotalTime IS NULL) AND (Vetted = ?)";
			this.oleDbDeleteCommand1.Connection = this.oleDbConnection1;
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Artist", System.Data.OleDb.OleDbType.VarWChar, 70, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Artist", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_DiscID", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "DiscID", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Title", System.Data.OleDb.OleDbType.VarWChar, 100, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Title", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Album_ID", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Album ID", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Album_ID1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Album ID", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Category", System.Data.OleDb.OleDbType.VarWChar, 25, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Category", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Category1", System.Data.OleDb.OleDbType.VarWChar, 25, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Category", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Cuts", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Cuts", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Cuts1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Cuts", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Label", System.Data.OleDb.OleDbType.VarWChar, 30, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Label", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Label1", System.Data.OleDb.OleDbType.VarWChar, 30, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Label", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Need_Cut_Names_Expanded", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Need Cut Names Expanded", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Not_Found_on_web", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Not Found on web", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Owner", System.Data.OleDb.OleDbType.VarWChar, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Owner", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Owner1", System.Data.OleDb.OleDbType.VarWChar, 1, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Owner", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_PlayLengthInSeconds", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PlayLengthInSeconds", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_PlayLengthInSeconds1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PlayLengthInSeconds", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Release_Date", System.Data.OleDb.OleDbType.VarWChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Release Date", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Release_Date1", System.Data.OleDb.OleDbType.VarWChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Release Date", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Site", System.Data.OleDb.OleDbType.VarWChar, 20, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Site", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Site1", System.Data.OleDb.OleDbType.VarWChar, 20, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Site", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Style", System.Data.OleDb.OleDbType.VarWChar, 15, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Style", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Style1", System.Data.OleDb.OleDbType.VarWChar, 15, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Style", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalLengthInFrames", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalLengthInFrames", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalLengthInFrames1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalLengthInFrames", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalLengthInSeconds", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalLengthInSeconds", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalLengthInSeconds1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalLengthInSeconds", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalTime", System.Data.OleDb.OleDbType.DBDate, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalTime", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_TotalTime1", System.Data.OleDb.OleDbType.DBDate, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "TotalTime", System.Data.DataRowVersion.Original, null));
			this.oleDbDeleteCommand1.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_Vetted", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "Vetted", System.Data.DataRowVersion.Original, null));
			// 
			// daAlbums
			// 
			this.daAlbums.DeleteCommand = this.oleDbDeleteCommand1;
			this.daAlbums.InsertCommand = this.oleDbInsertCommand1;
			this.daAlbums.SelectCommand = this.oleDbSelectCommand1;
			this.daAlbums.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							   new System.Data.Common.DataTableMapping("Table", "Albums", new System.Data.Common.DataColumnMapping[] {
																																																		 new System.Data.Common.DataColumnMapping("Album ID", "Album ID"),
																																																		 new System.Data.Common.DataColumnMapping("Artist", "Artist"),
																																																		 new System.Data.Common.DataColumnMapping("Category", "Category"),
																																																		 new System.Data.Common.DataColumnMapping("Comments", "Comments"),
																																																		 new System.Data.Common.DataColumnMapping("Cuts", "Cuts"),
																																																		 new System.Data.Common.DataColumnMapping("DiscID", "DiscID"),
																																																		 new System.Data.Common.DataColumnMapping("Extra", "Extra"),
																																																		 new System.Data.Common.DataColumnMapping("Label", "Label"),
																																																		 new System.Data.Common.DataColumnMapping("LargeCoverURL", "LargeCoverURL"),
																																																		 new System.Data.Common.DataColumnMapping("MoreInfoURL", "MoreInfoURL"),
																																																		 new System.Data.Common.DataColumnMapping("Need Cut Names Expanded", "Need Cut Names Expanded"),
																																																		 new System.Data.Common.DataColumnMapping("Not Found on web", "Not Found on web"),
																																																		 new System.Data.Common.DataColumnMapping("Owner", "Owner"),
																																																		 new System.Data.Common.DataColumnMapping("PlayLengthInSeconds", "PlayLengthInSeconds"),
																																																		 new System.Data.Common.DataColumnMapping("Release Date", "Release Date"),
																																																		 new System.Data.Common.DataColumnMapping("Site", "Site"),
																																																		 new System.Data.Common.DataColumnMapping("SmallCoverURL", "SmallCoverURL"),
																																																		 new System.Data.Common.DataColumnMapping("Style", "Style"),
																																																		 new System.Data.Common.DataColumnMapping("Title", "Title"),
																																																		 new System.Data.Common.DataColumnMapping("TotalLengthInFrames", "TotalLengthInFrames"),
																																																		 new System.Data.Common.DataColumnMapping("TotalLengthInSeconds", "TotalLengthInSeconds"),
																																																		 new System.Data.Common.DataColumnMapping("TotalTime", "TotalTime"),
																																																		 new System.Data.Common.DataColumnMapping("Vetted", "Vetted"),
																																																		 new System.Data.Common.DataColumnMapping("XML", "XML"),
																																																		 new System.Data.Common.DataColumnMapping("XML2", "XML2")})});
			this.daAlbums.UpdateCommand = this.oleDbUpdateCommand1;
			// 
			// lblOrdersPosition
			// 
			this.lblOrdersPosition.Location = new System.Drawing.Point(32, 8);
			this.lblOrdersPosition.Name = "lblOrdersPosition";
			this.lblOrdersPosition.Size = new System.Drawing.Size(168, 23);
			this.lblOrdersPosition.TabIndex = 6;
			this.lblOrdersPosition.Text = "label3";
			// 
			// txtCategory
			// 
			this.txtCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dsAlbums, "Albums.Category"));
			this.txtCategory.Location = new System.Drawing.Point(184, 40);
			this.txtCategory.Name = "txtCategory";
			this.txtCategory.Size = new System.Drawing.Size(200, 22);
			this.txtCategory.TabIndex = 8;
			this.txtCategory.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(40, 40);
			this.label3.Name = "label3";
			this.label3.TabIndex = 7;
			this.label3.Text = "Category";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(616, 408);
			this.Controls.Add(this.txtCategory);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblOrdersPosition);
			this.Controls.Add(this.txtAlbumName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BtnForward);
			this.Controls.Add(this.btnBack);
			this.Controls.Add(this.txtArtist);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.dsAlbums)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Form1());
		}

		private void BtnForward_Click(object sender, System.EventArgs e) {
// 			BindingContext[dsAlbums.Tables["Albums"]].Position++; 
#if true
			cmOrders.Position++;
#else
			DataTable	tbl = dsAlbums.Tables["Albums"];
			BindingManagerBase bmb = BindingContext[tbl];
			bmb.Position++;
#endif
		}

		private void btnBack_Click(object sender, System.EventArgs e) {
// 			BindingContext[dsAlbums.Tables["Albums"]].Position--; 
#if true
			cmOrders.Position--;
#else
			DataTable	tbl = dsAlbums.Tables["Albums"];
			BindingManagerBase bmb = BindingContext[tbl];
			bmb.Position--;
#endif
		}
	}
}
