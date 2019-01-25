using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.Data.OleDb;

using System.Runtime.InteropServices;

using Microsoft.Win32;

using Visio;

namespace DBSchema {

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class DBSchema : System.Windows.Forms.Form {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btnDumpTables;
		private System.Windows.Forms.Button btnAnalyze;
		private System.Windows.Forms.Button btnVisio;				
		private System.Windows.Forms.Button btnXMLSchema;
		private System.Windows.Forms.Button btnExportTableInXML;
		private System.Windows.Forms.Button cmdROT;

		[DllImport("ole32.dll")]  
		public static extern int GetRunningObjectTable(int reserved, out 
			UCOMIRunningObjectTable prot); 
 
		[DllImport("ole32.dll")]  
		public static extern int  CreateBindCtx(int reserved, out UCOMIBindCtx ppbc);


		OleDbConnection	conn;

		int			nTables;
		bool [,]	cm;				// Connectivity Matrix
		int [,]		dm;				// Distance matrix
		int			MaxDepth = 0;
		string []	TableNames;

		// Visio stuff
		Visio.Application	visApp;
		Visio.Master		TableMaster;
		Visio.Master		DynConnector;

//---------------------------------------------------------------------------------------

		public DBSchema() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			string		connstring;
			string		path, dbName;
			string		mdw, uid, pw;
			
#if true
			// path = @"C:\Copy of Activity Track 2.0 For Larry\Consolidated User and Data database\";
			// dbName = "db1.mdb";
			// path	= @"L:\Shared\lrs\Ahmed\";
			path	= @"C:\Activity Track\Activity Track 2.0\";
			dbName	= @"Activity Track Data 2.0.mdb";
			mdw		= "AT 2.0.mdw";
			uid		= "Administrator";
			pw		= "";
#else
			path	= @"C:\Activity Track 2.0\";
			dbName	= "Test Copy of Activity Track Data 2.0 for LRS DBSchema program.mdb";
			mdw		= null;
			uid		= null;
			pw		= null;
#endif
			connstring = "Provider=Microsoft.Jet.OLEDB.4.0";
			connstring += ";Mode=ReadWrite|Share Deny None;";
			connstring += "Data Source=";
			connstring += path;
			connstring += dbName;
			if (mdw != null) {
				connstring += @";Jet OLEDB:System database=" + path + mdw;
				connstring += ";User ID=" + uid;
				connstring += ";Password=" + pw;
			}
	
			conn = new OleDbConnection(connstring);
			conn.Open();

		}

//---------------------------------------------------------------------------------------

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
			this.btnDumpTables = new System.Windows.Forms.Button();
			this.btnAnalyze = new System.Windows.Forms.Button();
			this.btnVisio = new System.Windows.Forms.Button();
			this.btnXMLSchema = new System.Windows.Forms.Button();
			this.btnExportTableInXML = new System.Windows.Forms.Button();
			this.cmdROT = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnDumpTables
			// 
			this.btnDumpTables.Location = new System.Drawing.Point(8, 8);
			this.btnDumpTables.Name = "btnDumpTables";
			this.btnDumpTables.Size = new System.Drawing.Size(64, 48);
			this.btnDumpTables.TabIndex = 0;
			this.btnDumpTables.Text = "Dump Tables";
			this.btnDumpTables.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnAnalyze
			// 
			this.btnAnalyze.Location = new System.Drawing.Point(104, 8);
			this.btnAnalyze.Name = "btnAnalyze";
			this.btnAnalyze.Size = new System.Drawing.Size(64, 48);
			this.btnAnalyze.TabIndex = 1;
			this.btnAnalyze.Text = "Analyze";
			this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
			// 
			// btnVisio
			// 
			this.btnVisio.Location = new System.Drawing.Point(200, 8);
			this.btnVisio.Name = "btnVisio";
			this.btnVisio.Size = new System.Drawing.Size(64, 48);
			this.btnVisio.TabIndex = 2;
			this.btnVisio.Text = "Visio";
			this.btnVisio.Click += new System.EventHandler(this.btnVisio_Click);
			// 
			// btnXMLSchema
			// 
			this.btnXMLSchema.Location = new System.Drawing.Point(8, 80);
			this.btnXMLSchema.Name = "btnXMLSchema";
			this.btnXMLSchema.Size = new System.Drawing.Size(64, 48);
			this.btnXMLSchema.TabIndex = 3;
			this.btnXMLSchema.Text = "XML Schema";
			this.btnXMLSchema.Click += new System.EventHandler(this.btnXMLSchema_Click);
			// 
			// btnExportTableInXML
			// 
			this.btnExportTableInXML.Location = new System.Drawing.Point(104, 80);
			this.btnExportTableInXML.Name = "btnExportTableInXML";
			this.btnExportTableInXML.Size = new System.Drawing.Size(64, 48);
			this.btnExportTableInXML.TabIndex = 4;
			this.btnExportTableInXML.Text = "Export Table in XML";
			this.btnExportTableInXML.Click += new System.EventHandler(this.btnExportTableInXML_Click);
			// 
			// cmdROT
			// 
			this.cmdROT.Location = new System.Drawing.Point(200, 80);
			this.cmdROT.Name = "cmdROT";
			this.cmdROT.Size = new System.Drawing.Size(64, 48);
			this.cmdROT.TabIndex = 5;
			this.cmdROT.Text = "ROT";
			this.cmdROT.Click += new System.EventHandler(this.cmdROT_Click);
			// 
			// DBSchema
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.cmdROT);
			this.Controls.Add(this.btnExportTableInXML);
			this.Controls.Add(this.btnXMLSchema);
			this.Controls.Add(this.btnVisio);
			this.Controls.Add(this.btnAnalyze);
			this.Controls.Add(this.btnDumpTables);
			this.Name = "DBSchema";
			this.Text = "DBSchema";
			this.ResumeLayout(false);

		}
		#endregion

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			System.Windows.Forms.Application.Run(new DBSchema());
		}


//---------------------------------------------------------------------------------------
		
		private void button1_Click(object sender, System.EventArgs e) {
			// DumpTables(false);
			// DumpConstraints();
			DumpRelations();
			// DumpColumns();
						// DumpConstraintsByTable();		Not supported by Access
						// DumpTableConstraints();			Not supported by Access
			// DumpColumnConstraints();
			// DumpForeignKeys(false);
		}

//---------------------------------------------------------------------------------------

		void DumpColumnHeaders(DataTable tbl) {
			for (int i=0; i<tbl.Columns.Count; ++i) {
				Console.WriteLine("Column[{0}] name={1}", i, tbl.Columns[i].ColumnName);
			}
		}

//---------------------------------------------------------------------------------------

		void DumpTables(bool bShowHeaders) {
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, "TABLE"});
			// DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, null});
			DataRow	dr; 

			if (bShowHeaders)
				DumpColumnHeaders(SchemaTable);

			for (int i=0; i<SchemaTable.Rows.Count; ++i) {
				dr = SchemaTable.Rows[i];
				Console.WriteLine("Table[{0}] = {1}, Type={2}, Description={3}", 
					i, dr["TABLE_NAME"], dr["TABLE_TYPE"], dr["DESCRIPTION"]);
			}
		}

//---------------------------------------------------------------------------------------

		void DumpColumnConstraints() {
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Constraint_Column_Usage, new object[] {null, null, null});
			DumpColumnHeaders(SchemaTable); 
			DataRow	dr; 
			for (int i=0; i<SchemaTable.Rows.Count; ++i) {
				dr = SchemaTable.Rows[i];
				Console.WriteLine("Column Constraint[{0}] = {1} in table {2}, constraint name {3}", i, dr[3], dr[2], dr[8]);
			}
		}

//---------------------------------------------------------------------------------------

		void DumpForeignKeys(bool bShowHeaders) {
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new object[] {null, null, null});
			if (bShowHeaders)
				DumpColumnHeaders(SchemaTable); 
			DataRow	dr; 
			for (int i=0; i<SchemaTable.Rows.Count; ++i) {
				dr = SchemaTable.Rows[i];
				// Console.WriteLine("Foreign Key[{0}] = {1}, PK_Table={2}, FK_Table={3}, FK_Column={4}, PK_Name={5}, FK_Name={6}", i, dr[3], dr[2], dr[8], dr[9], dr[15], dr[17]);
				// Console.WriteLine("Foreign Key[{0}] = PK {2}(\"{1}\") <=> FK {3}(\"{4}\"), PK_Name={5}, FK_Name={6}", i, dr[3], dr[2], dr[8], dr[9], dr[15], dr[16]);
#if false
				Console.WriteLine("Foreign Key[{0}] = PK {1}(\"{2}\") <=> FK {3}(\"{4}\"), PK_Name={5}, FK_Name={6}", 
					i, 
					dr["PK_TABLE_NAME"], dr["PK_COLUMN_NAME"], 
					dr["FK_TABLE_NAME"], dr["FK_COLUMN_NAME"], 
					dr["PK_NAME"],		 dr["FK_NAME"]);
#else
				string	pk, fk;
				pk = string.Format("{0}(\"{1}\")", dr["PK_TABLE_NAME"], dr["PK_COLUMN_NAME"]);
				fk = string.Format("{0}(\"{1}\")", dr["FK_TABLE_NAME"], dr["FK_COLUMN_NAME"]);
				Console.WriteLine("{0,50} - {1,50}", pk, fk);
#endif
			}
		}

//---------------------------------------------------------------------------------------

#if false	// Constraint_Table_Usage - Not supported by Access
		void DumpTableConstraints() {
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Constraint_Table_Usage, new object[] {null, null, null});
			DumpColumnHeaders(SchemaTable); 
			DataRow	dr; 
			for (int i=0; i<SchemaTable.Rows.Count; ++i) {
				dr = SchemaTable.Rows[i];
				Console.WriteLine("Table Constraint[{0}] = {1} in table {2}", i, dr[3], dr[2], dr[8]);
			}
		}
#endif

//---------------------------------------------------------------------------------------

#if false	// Check_Constraints_By_Table - Not supported by Access
		void DumpConstraintsByTable() {
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Check_Constraints_By_Table, new object[] {null, null, null});
			DumpColumnHeaders(SchemaTable); 
			DataRow	dr; 
			for (int i=0; i<SchemaTable.Rows.Count; ++i) {
				dr = SchemaTable.Rows[i];
				Console.WriteLine("TableConstraint[{0}] = {1}", i, dr[2]);
			}
		}
#endif

//---------------------------------------------------------------------------------------

		void DumpColumns() {
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] {null, null, null, null});
			DumpColumnHeaders(SchemaTable); 
			DataRow	dr; 
			for (int i=0; i<SchemaTable.Rows.Count; ++i) {
				dr = SchemaTable.Rows[i];
				Console.WriteLine("Columns[{0}] = {1} in table {2}, description = {3}", i, dr[3], dr[2], dr[27]);
			}
		}

//---------------------------------------------------------------------------------------

		void DumpRelations() {
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Referential_Constraints, new object[] {null, null, null});
			DumpColumnHeaders(SchemaTable); 
			DataRow	dr; 
			for (int i=0; i<SchemaTable.Rows.Count; ++i) {
				dr = SchemaTable.Rows[i];
				Console.WriteLine("Relations[{0}] = {1}", i, dr[2]);
			}
		}

//---------------------------------------------------------------------------------------

		void DumpConstraints() {
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Table_Constraints, new object[] {null, null, null, null});
			DumpColumnHeaders(SchemaTable);
			for (int i=0; i<SchemaTable.Rows.Count; ++i) {
				Console.WriteLine("Constraint[{0}] = {1}, Type={2}, Description={3}"
//					+ ", Catalog={4}, Schema={5}, TableCatalog={6}, TableSchema={7}"
					+ ", TableName={4}",
//					+ ", IsDeferrable={5}, InitiallyDeferred={6}",
					i, 
					SchemaTable.Rows[i].ItemArray[2],
					SchemaTable.Rows[i].ItemArray[6],
					SchemaTable.Rows[i].ItemArray[9],
					//SchemaTable.Rows[i].ItemArray[0],
					//SchemaTable.Rows[i].ItemArray[1],
					//SchemaTable.Rows[i].ItemArray[3],
					//SchemaTable.Rows[i].ItemArray[4],
					SchemaTable.Rows[i].ItemArray[5]
					//SchemaTable.Rows[i].ItemArray[7],
					//SchemaTable.Rows[i].ItemArray[8]
					);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnAnalyze_Click(object sender, System.EventArgs e) {
			// Get list of tables, and initial relations
			DataTable tblTables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, "TABLE"});
			DataTable tblForeignKeys = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new object[] {null, null, null});

			// Create empty connectivity matrix
			nTables = tblTables.Rows.Count;
			cm = new bool[nTables, nTables];	// Entries initialized to 0, which is fine

			// Get table names into array, and sort them
			TableNames = new string[nTables];
			int		i = 0;
			foreach (DataRow dr in tblTables.Rows) {
				TableNames[i++] = (string)dr["TABLE_NAME"];
			}
			Array.Sort(TableNames);

			// Populate connectivity matrix based on foreign keys
			string	pk, fk;
			foreach (DataRow dr in tblForeignKeys.Rows) {
				pk = (string)dr["PK_TABLE_NAME"];
				fk = (string)dr["FK_TABLE_NAME"];
				AddRelation(pk, fk);
			}
			ModifyRelations();

			DumpRawCM(cm);
			DumpCM(cm);

			Console.WriteLine("\nDM ---------\n");
			dm = CalcDistanceMatrix(cm);
			DumpDM(dm);

			FindTreeRoots();
		}

//---------------------------------------------------------------------------------------

		void ModifyRelations() {
			// At some point, this really should read in data from a file. But for now,
			// we'll hardcode everything.
			AddRelation("tblPerson",			"tblPersonService");
			AddRelation("tblPerson",			"tblPersonWithoutServices");
			AddRelation("tblPerson",			"tblPlacement");
			AddRelation("tblPerson",			"tblIssuerSite");
			AddRelation("tblPerson",			"tblPersonWithoutServices");
			AddRelation("tblPersonService",		"tblServices");
			AddRelation("tblPersonService",		"tblProviders");
			AddRelation("tblPersonService",		"tblPersonServiceOrphaned");
			AddRelation("tblServices",			"tblCategories");
			AddRelation("tblNetGroupDefs",		"tblNetAddresses");
			// Security stuff
			AddRelation("tblSecUsersGroups",	"tblSecUsers");
			AddRelation("tblSecUsersGroups",	"tblSecGroups");
			AddRelation("tblSecUsers",			"tblSecUserPermissions");
			AddRelation("tblSecUsers",			"tblSecUserPermissionsTemp");
			AddRelation("tblSecGroups",			"tblSecGroupPermissions");
			AddRelation("tblSecGroups",			"tblSecGroupPermissionsTemp");
			AddRelation("tblSecUserExpAndImpPermissions",	"tblSecUsers");
			AddRelation("tblSecUserExpAndImpPermissions",	"tblSecFormsAndReports");
			AddRelation("tblSecControlFieldsProperties",	"tblSecProperties");
			AddRelation("tblSecControlFieldsProperties",	"tblSecControlFields");
			AddRelation("tblSecUserExpAndImpPermissions",	"tblSecUsers");
			AddRelation("tblSecUserExpAndImpPermissions",	"tblSecFormsAndReports");
			AddRelation("tblSecUserExpAndImpPermissions",	"tblSecControlSets");
			AddRelation("tblSecUserExpAndImpPermissions",	"tblSecControlFields");
			AddRelation("tblSecUserExpAndImpPermissions",	"tblSecProperties");
			AddRelation("tblSecUserExpAndImpPermissions",	"tblSecValues");
		}

//---------------------------------------------------------------------------------------

		void AddRelation(string CallerName, string CalleeName) {
			int		i, j;
			if (IsIgnoredTableName(CallerName))
				return;
			i = Array.BinarySearch(TableNames, CallerName);
			j = Array.BinarySearch(TableNames, CalleeName);
			cm[i, j] = true;
			// I've seen cases where I want to specify A "calls" B, but (probably for
			// one-to-many reasons), Access's Relations form forces B "calls" A. To
			// get around this, at least in part, if we're told that A calls B, then
			// delete any B calls A relation (which we wouldn't want anyway, since that
			// would introduce an infinite loop in the call tree.
			cm[j, i] = false;
			// Yeah, I could have written DeleteRelation(CalleeName, CallerName);
			// but I decided not to use the "official" way.
		}

//---------------------------------------------------------------------------------------

		void DeleteRelation(string CallerName, string CalleeName) {
			int		i, j;
			if (IsIgnoredTableName(CallerName))
				return;
			i = Array.BinarySearch(TableNames, CallerName);
			j = Array.BinarySearch(TableNames, CalleeName);
			cm[i, j] = false;
		}

//---------------------------------------------------------------------------------------

		bool IsIgnoredTableName(string tblName) {
			if (tblName.StartsWith("tblAudit"))
				return true;
			if (tblName.StartsWith("tblSec"))
				return true;
			if (tblName.StartsWith("tblNet"))
				return true;
			if (tblName.StartsWith("Switchboard"))
				return true;
			if (tblName.StartsWith("tblActivityTrack"))
				return true;
			if (tblName.StartsWith("tblRemoteTable"))
				return true;
			if (tblName.StartsWith("tblDemogAnswersAllAnswers"))
				return true;
			return false;
		}

//---------------------------------------------------------------------------------------

		bool [,] Warshall(bool [,] cm) {   // Warshall's algorithm for transitive closure
			int			i, j, k;
			int			n = cm.GetUpperBound(0) + 1;	// Assume square matrix
			bool [,]	tc = (bool[,])cm.Clone();
			for (k = 0; k < n; k++)
				for (i = 0; i < n; i++)
					for (j = 0; j < n; j++)
						if (! tc[i, j])
							tc[i, j] = tc[i, k] && tc[k, j];
			return tc;
		}


//---------------------------------------------------------------------------------------

		int [,] CalcDistanceMatrix(bool [,] cm) {
			int		i, j;
			bool [,]	mat = (bool [,])cm.Clone();	// Don't disturb original
			int		n = mat.GetUpperBound(0) + 1;		// Assume square matrix
			int [,]	dm = new int[n, n];				// Distance matrix
			// Copy bool cm to int dm
			for (i=0; i<n; i++) {
				for (j=0; j<n; ++j) {
					dm[i, j] = mat[i, j] ? 1 : 0;
				}
			}

			// Now calculate successive transitive closures (XC). At each step compare
			// the new XC to the original, and update the distance matrix with the
			// info.
			// Note: There are a couple of potentially major inefficienies in the 
			// following code. The main one is that the Warshall routine instantiates
			// a new matrix each time. More efficient would be to move the Warshall
			// algorithm inline, and reuse the existing arrays. Similarly, the .Clone
			// operation at the end of each loop could be just a copy. But for now,
			// it's fast enough.
			bool	bMore = false;
			bool [,]	matXC;
			int		pass = 1;
			do {
				++pass;
				Console.WriteLine("Processing pass {0}", pass);
				bMore = false;
				matXC = Warshall(mat);
				for (i=0; i<n; ++i) {
					for (j=0; j<n; ++j) {
						if (matXC[i, j] != mat[i, j]) {
							dm[i, j] = pass;
							bMore = true;
						}
					}
				}
				if (bMore) 
					mat = (bool [,])matXC.Clone();
			} while (bMore);
			MaxDepth = pass - 1;
			return dm;
		}

//---------------------------------------------------------------------------------------

		void FindTreeRoots() {
			// Go through the distance matrix. Any column with all zeros is a root
			bool	bFoundRoot = false;
			Console.WriteLine("\nTree roots...\n");
			for (int col=0; col<nTables; ++col) {
				if (IsIgnoredTableName(TableNames[col]))
					continue;
				bFoundRoot = true;
				for (int row=0; row<nTables; ++row) {
					if (dm[row, col] > 0) {
						bFoundRoot = false;
						break;
					}
				}
				if (bFoundRoot) {
					Console.WriteLine("Root found - [{0}] : {1}", col, TableNames[col]);
					DumpCallees(dm, col);
				}
			}
		}

//---------------------------------------------------------------------------------------

		void DumpCallees(int [,] mat, int row) {
			string	indent;
			for (int pass=1; pass<=MaxDepth; ++pass) {
				indent = new string(' ', pass * 4);
				for (int j=0; j<nTables; ++j) {
					if (mat[row, j] == pass) {
						Console.WriteLine("{0}{1}[{2}] at level {3}", indent, TableNames[j], j, pass);
					}
				}
			}
			}

//---------------------------------------------------------------------------------------

		void DumpCM(bool [,] mat) {
			bool	NameShown;
			for (int i=0; i<nTables; ++i) {
				NameShown = false;
				for (int j=0; j<nTables; ++j) {
					if (mat[i, j]) {
						if (! NameShown) {
							Console.Write("{0}[{1}] calls", TableNames[i], i);
							NameShown = true;
						}
						Console.Write(" {0}[{1}]", TableNames[j], j);
					}
				}
				if (NameShown)
					Console.WriteLine();
			}
		}

//---------------------------------------------------------------------------------------

		void DumpRawCM(bool[,] mat) {
			int		n = mat.GetUpperBound(0) + 1;	// Assume square matrix
			ShowMatrixHeader(7, n, 3);
			for (int i=0; i<n; ++i) {
				Console.Write("[{0,3}] :", i);
				for (int j=0; j<n; ++j) {
					Console.Write("{0,3}", mat[i, j] ? "1" : "_");
				}
				Console.WriteLine();
			}
		}

//---------------------------------------------------------------------------------------

		void DumpDM(int[,] mat) {
			int		n = mat.GetUpperBound(0) + 1;	// Assume square matrix
			ShowMatrixHeader(7, n, 3);
			for (int i=0; i<n; ++i) {
				Console.Write("[{0,3}] :", i);
				for (int j=0; j<n; ++j) {
					if (mat[i, j] > 0)
						Console.Write("{0,3}", mat[i, j]);
					else
						Console.Write("  _");
				}
				Console.WriteLine();
			}

			bool	NameShown;
			string	indent;
#if true
			int		row, col, pass;
			for (row=0; row<n; ++row) {
				NameShown = false;
				for (pass=1; pass<=MaxDepth; ++pass) {
					indent = new string(' ', pass * 4);
					// Console.WriteLine("{0}Starting pass {1}", indent, pass);
					for (col=0; col<n; ++col) {
						if (mat[row, col] == pass) {
							if (! NameShown) {
								Console.WriteLine("{0}[{1}] calls", TableNames[row], row);
								NameShown = true;
							}
							Console.WriteLine("{0}{1}[{2}] at level {3}", indent, TableNames[col], col, pass);
						}
					}
				}
			}
#else
			for (int i=0; i<n; ++i) {
				NameShown = false;
				for (int j=0; j<n; ++j) {
					if (mat[i, j] > 0) {
						if (! NameShown) {
							Console.WriteLine("{0}[{1}] calls", TableNames[i], i);
							NameShown = true;
						}
						Console.WriteLine("\t\t{0}[{1}] at level {2}", TableNames[j], j, mat[i, j]);
					}
				}
				// if (NameShown)
					// Console.WriteLine();
			}
#endif
		}

//---------------------------------------------------------------------------------------

		void ShowMatrixHeader(int nPadLeft, int nCols, int colwidth) {
			// Do first line. Assume < 100 columns
			Console.Write("".PadLeft(nPadLeft));
			for (int i=0; i<nCols; ++i) {
				if (i != 0 && (i % 10) == 0)
					Console.Write("{0,3}", i / 10);
				else
					Console.Write("   ");
			}
			Console.WriteLine();
			// Do second line
			Console.Write(new string(' ', nPadLeft));
			for (int i=0; i<nCols; ++i) {
				Console.Write("{0,3}", i % 10);
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		private void btnVisio_Click(object sender, System.EventArgs e) {
			string VisioDir = GetVisioDir();
			visApp = new Visio.ApplicationClass();
			Visio.Document visDoc = visApp.Documents.Add("");

			Visio.Document	vStencil;
			string	Stencil;
//			Stencil = VisioDir + @"Solutions\Database\Database Model Diagram (US units).vst";
			Stencil = VisioDir + @"Solutions\Database\Entity Relationship (US units).vss";
			vStencil = visApp.Documents.OpenEx(Stencil, 4 + 2);	// 4=Docked, 2=R/O

			DynConnector = vStencil.Masters["Dynamic Connector"];
			TableMaster = vStencil.Masters["Entity"];

			double		x, y;
			x = 4; y = 10;
#if false
			foreach (Master mast in vStencil.Masters) {
				Console.WriteLine("master name={0}", mast.Name);
				try {
					shape = visApp.ActivePage.Drop(mast, x, y);
					shape.Text = string.Format("{0} at ({1}, {2})", mast.Name, x, y);
				} catch (Exception exp) {
					MessageBox.Show("Error " + exp.Message + " processing " + mast.Name);
				}
				x -= .5;
				y -= .5;
			}
#endif
			Visio.Shape		FromShape = null, ToShape;

			FromShape = visApp.ActivePage.Drop(TableMaster, x, y);
			FromShape.Text = "tblPerson, I suppose";
			y -= 1.5;
			ToShape = visApp.ActivePage.Drop(TableMaster, x - 2, y);
			// ToShape.Text = string.Format("{0} at\n({1},\n {2})", "Table 12345678", x, y);
			ToShape.Text = "tblPersonService, I suppose";
			Glue(FromShape, ToShape);

			ToShape = visApp.ActivePage.Drop(TableMaster, x + 2, y);
			ToShape.Text = "tblPersonNoService, I suppose";
			Glue(FromShape, ToShape);

			if (FromShape != null) {
				// Well, nothing really at this point, but if we were in a loop...
			}
			FromShape = ToShape;		// TODO: Again, for loop support
		}

//---------------------------------------------------------------------------------------

		void Glue(Visio.Shape FromShape, Visio.Shape ToShape) {
			Visio.Shape		connector;
			Visio.Cell		FromCell, ToCell;

			connector = visApp.ActivePage.Drop(DynConnector, 0, 0);
			FromCell = connector.get_Cells("BeginX");
			FromCell.GlueTo(FromShape.get_Cells("AlignBottom"));
			ToCell = connector.get_Cells("EndX");
			ToCell.GlueTo(ToShape.get_Cells("AlignTop"));
			connector.SendToBack();
		}

//---------------------------------------------------------------------------------------

		string GetVisioDir() {
			// HKCR\Visio.Application\CurVer\{Default Value}
			//	-> Visio.Application.6\Clsid -> {00021A20-0000-0000-C000-000000000046}
			//  -> LocalServer32\{Default} = C:\PROGRA~1\MICROS~2\Visio10\Visio.exe /Automation
			RegistryKey		HKCR = Registry.ClassesRoot;
			RegistryKey		VisAppCurVersion = HKCR.OpenSubKey(@"Visio.Application\CurVer");
			string	CurVer = (string)VisAppCurVersion.GetValue("");
			RegistryKey		CurCLSID = HKCR.OpenSubKey(CurVer + @"\CLSID");
			string	CLSID = (string)CurCLSID.GetValue("");
			RegistryKey		CurServer = HKCR.OpenSubKey(@"CLSID\" + CLSID + @"\LocalServer32");
			string	server = (string)CurServer.GetValue("");
			string  dir;
			dir = server.Substring(0, server.LastIndexOf("\\") + 1);

			// dir = @"C:\Program Files\Microsoft Office\Visio10\";
			return dir + @"1033\";			// Assumes Visio 10 and beyond
		}

//---------------------------------------------------------------------------------------

		private void btnXMLSchema_Click(object sender, System.EventArgs e) {
			OleDbCommand		cmd = new OleDbCommand();
			OleDbDataAdapter	adapt = new OleDbDataAdapter(cmd);
			DataSet				ds = new DataSet("LRS Schemae");
			string				TableName;

			cmd.CommandType = CommandType.TableDirect;
			cmd.Connection  = conn;
			DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, "TABLE"});
			foreach (DataRow dr in SchemaTable.Rows) {
				TableName = (string)dr["TABLE_NAME"];
				cmd.CommandText = TableName;
				adapt.FillSchema(ds, SchemaType.Source, TableName);
			}
			ds.WriteXmlSchema(@"C:\XmlSchema.xml");
		}

//---------------------------------------------------------------------------------------

		private void btnExportTableInXML_Click(object sender, System.EventArgs e) {
			OleDbCommand	cmd = new OleDbCommand("tblServices");
			cmd.CommandType = CommandType.TableDirect;
			cmd.Connection = conn;
			OleDbDataAdapter adapt = new OleDbDataAdapter(cmd);
			DataSet	ds = new DataSet();
			adapt.Fill(ds, "tblServicesLRS");
			// ds.WriteXml(@"C:\tblServices.xml", XmlWriteMode.IgnoreSchema);
			ds.WriteXml(@"C:\tblServices.xml");
		}

//---------------------------------------------------------------------------------------

		private void cmdROT_Click(object sender, System.EventArgs e) {
			UCOMIRunningObjectTable prot;   
			UCOMIEnumMoniker pMonkEnum;    

			GetRunningObjectTable(0, out prot);    
			prot.EnumRunning(out pMonkEnum);    
			pMonkEnum.Reset();          // Churn through enumeration.    
			int fetched;    
			UCOMIMoniker []pmon = new UCOMIMoniker[1];    
			while (pMonkEnum.Next(1, pmon, out fetched) == 0) {     
				UCOMIBindCtx pCtx;     
				CreateBindCtx(0, out pCtx);     
				string str;     
				pmon[0].GetDisplayName(pCtx, null, out str);  
				Console.WriteLine("{0}", str);
				// if (str == strProgID) {  
				{
					object objReturnObject;  
					prot.GetObject(pmon[0], out objReturnObject); 
					object ide = objReturnObject;  
					// return;  
				}  
			} 
			return; 
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class Database {
		Table []	tables;
		Query []	queries;
	}

	public class Table {
		string		name;
		string		comment;		// aka Descripti Field []	fields;
		Index []	indexes;
	}

	public class Field {
		string		name;
		int			type;
		string		description;
		// Other stuff, such as length, required, can be null, etc
	}

	public class Index {
		string		ixName;
		string []	fields;
	}

	public class Query {
		string		name;
		string		comment;		// aka Description
	}
}
