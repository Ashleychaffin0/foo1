using System;
using System.Collections.Generic;

namespace CSharpVsRealBasic {
	class Program {
		public static char Chr(byte src) {
			return (System.Text.Encoding.GetEncoding("iso-8859-1").GetChars(new byte[] { src })[0]);
		}

//---------------------------------------------------------------------------------------

		static void Main(string[] args) {
			double ElapsedWith, ElapsedWithout;
			ElapsedWith = RunTest_WithChr();
			Console.WriteLine("Operation With Chr completed in " + ElapsedWith);
			ElapsedWithout = RunTest_WithoutChr();
			Console.WriteLine("Operation Without Chr completed in " + ElapsedWithout);

			Console.WriteLine("Operation Without ran {0:N2} times faster than With", ElapsedWith / ElapsedWithout);

			//Keep the window open at the end of the test
			Console.Write("Press any key to exit...");
			Console.ReadKey(true);
		}

//---------------------------------------------------------------------------------------

		private static double RunTest_WithChr() {
			Dictionary<String,String> nDic = new Dictionary<String, String>();
			int i;
			int x;
			Random rnd = new Random();
			Int64 tTimer = DateTime.Now.Ticks;
			for (i = 1; i <= 1000000; i++) {
				// Generate a random UUID
				String pUUID = "";
				for (x = 1; x <= 16; x++) {
					pUUID += Chr((byte)rnd.Next(0, 255));
				}

				//Check if pUUID exists in the dictionary
				if (!nDic.ContainsKey(pUUID)) {
					//If not, add it to the dictionary
					nDic.Add(pUUID, pUUID);
				}
			}

			tTimer = DateTime.Now.Ticks - tTimer; //Stop the clock!
			double Elapsed = (double)tTimer / (double)TimeSpan.TicksPerSecond;
			Console.WriteLine(nDic.Count.ToString() + " keys in dictionary");
			return Elapsed;
		}

//---------------------------------------------------------------------------------------

		private static double RunTest_WithoutChr() {
			Dictionary<String,String> nDic = new Dictionary<String, String>();
			int i;
			int x;
			Random rnd = new Random();
			Int64 tTimer = DateTime.Now.Ticks;
			for (i = 1; i <= 1000000; i++) {

				// Generate a random UUID
				string pUUID = "";
				// string pUUID2= "";
				for (x = 1; x <= 16; x++) {
					byte val = (byte)rnd.Next(0, 255);
					pUUID += (char) (val);
#if false

					pUUID2 += Chr(val);
					if (pUUID != pUUID2)
						System.Diagnostics.Debugger.Break();
#endif
				}

				//Check if pUUID exists in the dictionary
				if (!nDic.ContainsKey(pUUID)) {
					//If not, add it to the dictionary
					nDic.Add(pUUID, pUUID);
				}
			}

			tTimer = DateTime.Now.Ticks - tTimer; //Stop the clock!
			double Elapsed = (double)tTimer / (double)TimeSpan.TicksPerSecond;
			// Console.WriteLine(nDic.Count.ToString() + " keys in dictionary");
			return Elapsed;
		}
	}
}


