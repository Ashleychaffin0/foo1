using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;

namespace TrySSCompact3._5_1 {
	class Program {
		static void Main(string[] args) {

			Program pgm = new Program();
			pgm.Run();
		}

//---------------------------------------------------------------------------------------

		private void Run() {
			string srcString = "Datasource=c:\\LRS\\ZuneStore.sdf";
			srcString = "Datasource=c:\\LRS\\ZuneStore - Copy (2).sdf";
			string destString = "Datasource=c:\\LRS\\ZuneStore-Upgraded to 3.5.sdf";
			try {
				var eng = new SqlCeEngine(srcString);
				eng.Upgrade(destString);
				srcString = destString;
				SqlCeConnection conn = new SqlCeConnection(srcString);
				conn.Open();
				conn.Close();
			} catch (Exception ex) {
				Console.WriteLine("Exception: {0}", ex.Message);
			}
		}
	}
}
