// Copyright (c) 2006 Bartizan Connects LLC


// #define		PROD			// Production, not debug, version
#define		DEVEL			// Debug, but on the remote Server
// #define		LOCAL			// Here, locally, but still on LLDevel

// #define BIRTHDAY

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;


namespace TestLLImporter {
	public partial class TestLLImporter : Form {
		public TestLLImporter() {
			InitializeComponent();

#if false
			int		testval = 63;
			Bartizan.Utils.Base_N	base_n = new Bartizan.Utils.Base_N();
			string ss = base_n.ToString(testval, 4);
			int val = base_n.ToInt(ss.ToUpper());
			if (val != testval) {
				MessageBox.Show("Whoops. Didn't roundtripe", "Test Base36", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseMapCfg_Click(object sender, EventArgs e) {
			OpenFileDialog	openFileDialog1 = new OpenFileDialog();
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.Filter = "Setup files (*.cfg)|*.cfg|All files|*.*";
			openFileDialog1.InitialDirectory = Path.GetDirectoryName(txtMapCfg.Text);
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				txtMapCfg.Text = openFileDialog1.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseVisitorTxt_Click(object sender, EventArgs e) {
			OpenFileDialog	openFileDialog1 = new OpenFileDialog();
			openFileDialog1.CheckFileExists = true;
			openFileDialog1.Filter = "Swipe data files (*.txt)|*.txt|All files|*.*";
			openFileDialog1.InitialDirectory = Path.GetDirectoryName(txtVisitorTxt.Text);
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				txtVisitorTxt.Text = openFileDialog1.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
#if DEBUG && BIRTHDAY
			double	odds;
			odds = BirthDayParadox(Math.Pow(2.0, 64), 60000);
			odds = BirthDayParadox(365, 23);
#endif

			string	MapData, RawSwipeData;
			// string	Results = "*MT*";
			MapData = GetFileContents(txtMapCfg.Text);
			RawSwipeData = GetFileContents(txtVisitorTxt.Text);

			if (MapData == null) {
				MessageBox.Show("Map.cfg data not available", "Test LL Importer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			
			if (RawSwipeData == null) {
				MessageBox.Show("Visitor.txt data not available", "Test LL Importer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (chkUseWebService.Checked == true) {
				CallWebService(MapData, RawSwipeData);
			} else {
				CallSQLServer(MapData, RawSwipeData);
			}
		}

//---------------------------------------------------------------------------------------

#if BIRTHDAY
		private double BirthDayParadox(double Days, double nPeople) {
			double odds = 1.0;
			for (int n = 1; n <= nPeople - 1; n++) {
				odds *= 1 - n / Days;
			}
			return 1 - odds;
		}
#endif

//---------------------------------------------------------------------------------------

		private void CallWebService(string MapData, string RawSwipeData) {
			LLImporter	import = new LLImporter();
			string	result;
			result = import.Import("AhmedEx", "pass", "Phony Event Name",
				RawSwipeData, MapData, 1, "LRSTerm");
			ShowStringsInListbox(result, "WebResult - {0}");
			MessageBox.Show("Done", "Test via Web Service", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

//---------------------------------------------------------------------------------------

		private void CallSQLServer(string MapData, string RawSwipeData) {
			string	prefix;
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
#if LOCAL
			builder.DataSource = "(local)";
			builder.IntegratedSecurity = true;
			builder.InitialCatalog = "LLDevel";
			prefix = "";
#elif DEVEL
			builder.DataSource = "SQLB5.webcontrolcenter.com";
			builder.InitialCatalog = "LLDevel";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
			prefix = "ahmed.";
#elif PROD
			#warning You really should use DEVEL or LOCAL
			builder.DataSource = "SQLB2.webcontrolcenter.com";
			builder.InitialCatalog = "LeadsLightning";
			builder.UserID = "ahmed";
			builder.Password = "i7e9dua$tda@";
			prefix = "ahmed.";
#else
			#error Must choose one of LOCAL / DEVEL / PROD
#endif
			using (SqlConnection conn = new SqlConnection(builder.ConnectionString)) {
				try {
					conn.Open();
				} catch (Exception ex) {
					MessageBox.Show(ex.Message, "Connection Open Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					throw;
				}
				conn.InfoMessage += new SqlInfoMessageEventHandler(conn_InfoMessage);

				SqlCommand cmd = new SqlCommand(prefix + "LL_sp_LLImport", conn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@UserID",		"Ahmedex");
				cmd.Parameters.AddWithValue("@Password",	"pass");
				cmd.Parameters.AddWithValue("@EventID",		2);		// TODO:
				cmd.Parameters.AddWithValue("@SwipeData",	RawSwipeData);
				cmd.Parameters.AddWithValue("@MapCfgFile",	MapData);
				cmd.Parameters.AddWithValue("@MapType",		1);
				cmd.Parameters.AddWithValue("@TerminalID",	"");
				// cmd.Parameters.AddWithValue("@Results", Results);
				cmd.CommandTimeout = 0;			// TODO:
				try {
					this.Cursor = Cursors.WaitCursor;
					int n = cmd.ExecuteNonQuery();
				} finally {
					this.Cursor = Cursors.Arrow;
				}
				conn.Close();
			}
			MessageBox.Show("Done", "Test via SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

//---------------------------------------------------------------------------------------

		void conn_InfoMessage(object sender, SqlInfoMessageEventArgs e) {
			ShowStringsInListbox(e.Message, "InfoMessage - {0}");
		}

//---------------------------------------------------------------------------------------

		private void ShowStringsInListbox(string msg, string fmt) {
			string[] msgs = msg.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			lbMsgs.Items.Add("***");
			foreach (string txt in msgs) {
				lbMsgs.Items.Add(string.Format(fmt, txt));
			}
		}

//---------------------------------------------------------------------------------------

		private string GetFileContents(string Filename) {
			try {
				return File.ReadAllText((Filename));
			} catch {
				return null;
			}
		}
	}
}

// Copyright (c) 2006 Bartizan Connects LLC

namespace Bartizan.Utils {
	//	$$ Duplicate
	public class Base_N {
		protected string	Alphabet;
		protected uint		AlphaLen;
		protected bool		IsCaseInsensitive;

//---------------------------------------------------------------------------------------

		public Base_N() : this("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ") {	// Base 36
		}

//---------------------------------------------------------------------------------------

		public Base_N(string Alphabet, bool IsCaseSensitive) {
			MyCtor(Alphabet, IsCaseSensitive);
		}

//---------------------------------------------------------------------------------------

		public Base_N(string Alphabet) : this(Alphabet, false) {
		}

//---------------------------------------------------------------------------------------

		// NOTE!!! Using this ctor with n=64 does *not* give the exact same results as
		// Convert.ToBase64String. See the Convert documentation for details.
		public Base_N(int n, bool IsCaseSensitive) {
			if ((n < 2) || (n == 63) || (n > 64)) {
				throw new ArgumentException("Base_N must be in the range 2..62, or 64");
			} 
			if (n < 64) {
				string Alpha = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
				MyCtor(Alpha.Substring(0, n), IsCaseInsensitive);
			} else {
				// Note a different ordering than what, say, base 16 uses.
				string	Alpha64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
				MyCtor(Alpha64.Substring(0, n), IsCaseInsensitive);
			}
		}

//---------------------------------------------------------------------------------------

		// NOTE!!! Using this ctor with n=64 does *not* give the exact same results as
		// Convert.ToBase64String. See the Convert documentation for details.
		public Base_N(int n) : this(n, false) {
		}

//---------------------------------------------------------------------------------------

		protected void MyCtor(string Alphabet, bool IsCaseSensitive) {
			this.Alphabet = Alphabet;
			AlphaLen = (uint)Alphabet.Length;
			this.IsCaseInsensitive = IsCaseSensitive;
		}

//---------------------------------------------------------------------------------------

		public string ToString(uint n, int nLength) {	// Don't support -ve nums
			// Note: nLength is the minimum length (left-padded with zeros). However if
			//		 the value <n> is too large, the result can be longer than this.
			// Note: For no padding, pass in nLength = 0
			StringBuilder	sb = new StringBuilder();
			while (n != 0) {
				uint i = n % AlphaLen;
				sb.Insert(0, Alphabet[(int)i]);
				n /= AlphaLen;
			}
			string	s = sb.ToString();
			s = s.PadLeft(nLength, '0');
			return s;
		}

//---------------------------------------------------------------------------------------

		public string ToString(uint n) {
			return ToString(n, 8);
		}

//---------------------------------------------------------------------------------------

		public string ToString(int n) {
			return ToString((uint)n);
		}

//---------------------------------------------------------------------------------------

		public string ToString(int n, int nLength) {
			return ToString((uint)n, nLength);
		}

//---------------------------------------------------------------------------------------

		public int ToInt(string s) {
			int		val = 0;
			string	Save_s = s;
			string	UseAlphabet = Alphabet;
			if (! IsCaseInsensitive) {
				s = s.ToUpper();
				UseAlphabet = Alphabet.ToUpper();
			}
			unchecked {
				int		ScaleFactor = 1;
				for (int n = s.Length - 1; n >= 0; n--) {
					int i = UseAlphabet.IndexOf(s[n]);
					if (i == -1) {
						string msg;
						msg = string.Format("Base_N:ToInt - Invalid character ({0}"
							+ ") found in string '{1}'.", s[n], Save_s);
						if (IsCaseInsensitive) {
							msg += " Remember that this instance is case sensitive.";
						}
						throw new ArgumentException(msg);
					}
					val += i * ScaleFactor;
					ScaleFactor *= (int)AlphaLen;
				}
			}
			return val;
		}
	}
}