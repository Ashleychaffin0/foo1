using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

//	1)	Nuget Microsoft.EntityFrameworkCore.Sqlite
//	2)	Nuget Microsoft.EntityFrameworkCore; Add using
//	3)	Nuget Microsoft.EntityFrameworkCore.Tools
//	4)	Add database classes (all fields via Properties)
//	5)	Define PWContext, deriving from DbContext
//	6)	Modify OnModelCreating and OnConfiguring as below
// No... Doesn't need this //	7)	From Package Manager Console, run "Add-Migration Initial"
//	7)	For data annotations (e.g. [Required]), using System.ComponentModel.DataAnnotations

// TODO: Add feature to update all strips
// TODO: On session closed, automatic retry?

namespace GetGoComics {
	internal class TheApp {
		readonly WebClient wc;
		string   BaseDir;
		string   BaseUrl;
		readonly List<StripInfo> Strips;

		internal static string DbFilename;
		static readonly string BaseTargetFolder = @"lrs\Comics";
		readonly string Comics_txt = "comics.txt";

		static readonly StripContext db = new StripContext();

		// The web site seems to reset the Internet connection if it encounters too
		// many requests too frequently. I don't know their algorithm, so I'll throw
		// in a delay between downloading strips. With that said, I've seen several
		// cases that while downloading Foxtrot, there are large gaps between strips,
		// in the sense that the strip after, say, 1989-10-03 might not exist and the
		// next one available could be, say, 1989-11-15. But that would involves trying
		// all the dates in between, which seems to count against the too many requests
		// per hour limit. But since the delay is done only after a successful download,
		// we could still get the rug pulled out from under us, Internet-connection-wise,
		// even then. The good news is that since we'll automatically restart from where
		// we left off, it will be easy to just restart, but just wait for maybe 15
		// minutes to an hour before you try again.
		// P.S. I don't know why some strips (Foxtrot and others) have gaps. Some kind of
		//		licensing issue?
		readonly int InterComicDelay = 5 * 1000;        // 5 seconds
		const int RetryMinutes       = 31;
		const int OneMinute          = 60 * 1000;
		// const int RetryDelay         = RetryMinutes * OneMinute;

		int nStripsThisMonth        = 0;
		DateTime WhenLastStripFound = default;
		DateTime TimeStampThisMonth = default;
		string PrevMonth            = "";
		bool bSkippingMsgIssued     = false;

		string CurStripName;
		string CurMonth;
		string CurYear;
		int TotalStripsThisSession;
		int nDaysThisRun;				// Number of days before we're cut off

//---------------------------------------------------------------------------------------

		public TheApp() {
			string Drive = "C:";
			if (Environment.UserName.ToUpper().StartsWith("LRS")) {
				Drive = "G:";
			}
			DbFilename = Path.Combine(Drive, BaseTargetFolder, "Strips.db");
			wc = new WebClient();
			Strips = new List<StripInfo> {
				new StripInfo("Bloom-County",          new DateTime(2015, 7, 20)),
				new StripInfo("Broomhilda",            new DateTime(2001, 4, 8)),
				new StripInfo("Dilbert-Classics",      new DateTime(2012, 6, 13)),
				new StripInfo("FoxTrot",               new DateTime(1988, 4, 11)),
				new StripInfo("FoxTrotClassics",       new DateTime(2007, 1, 1)),
				new StripInfo("Lil-Abner",             new DateTime(1934, 8, 19)),
				new StripInfo("Outland",               new DateTime(1989, 9, 3)),
				new StripInfo("Peanuts",               new DateTime(1950, 10, 2)),
				new StripInfo("Peanuts-Begins",        new DateTime(1950, 10, 2)),	// In color
				new StripInfo("Shoe",                  new DateTime(2001, 4, 8)),   // By Susie, not Jeff MacNelly
				new StripInfo("TankMcNamara",          new DateTime(1998, 1, 1)),
				new StripInfo("WizardOfId",            new DateTime(2002, 1, 1)),
				new StripInfo("Wizard-of-ID-Classics", new DateTime(2014, 11, 17)),
				new StripInfo("Garfield",				new DateTime(1978, 6, 19)),
			};

			// ReadInputFile();

			// ReloadDatabase();
		}

//---------------------------------------------------------------------------------------

		private void ReadInputFile() {
			string fn = Path.Combine(BaseTargetFolder, Comics_txt);
			int linenum = 0;
			bool bOK = true;
			foreach (string line in File.ReadLines(fn)) {
				++linenum;
				string src = line.Trim();
				if (src.Length == 0) { continue; }
				if (src[0] == '#') { continue; }
				string[] fields = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				DateTime? StartDate = CheckInputLine(linenum, fields[1], fields); ;
				bool bOKLine = StartDate != null;
				bOK &= bOKLine;
				if (!bOKLine) { continue; }
				// OK, line looks good. 
				ProcessInputLine(fields[0], StartDate);
			}
		}

//---------------------------------------------------------------------------------------

		private static DateTime? CheckInputLine(int linenum, string startDate, string[] fields) {
			if (fields.Length != 2) {
				Console.WriteLine($"Error on input file line {linenum}. Must be exactly 2 fields");
				return null;
			}
			bool OkDate = DateTime.TryParse(startDate, out DateTime StartDate);
			if (!OkDate) {
				Console.WriteLine($"Error on input file line {linenum}. Second field not date");
				return null;
			}
			return StartDate;
		}

//---------------------------------------------------------------------------------------

		private void ProcessInputLine(string stripName, DateTime? startDate) {
			if (StripExists(stripName)) {
				// Ignore
				// TODO: Consider taking the StartDate field and using it as the 
				//		 done-up-to field
			} else {
				// TODO:
			}
		}

//---------------------------------------------------------------------------------------

		private void ReloadDatabase() {
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();

			foreach (StripInfo strip in Strips) {
				db.Add<StripInfo>(strip);
			}
			db.SaveChanges();
		}

//---------------------------------------------------------------------------------------

		private bool StripExists(string stripName) {
			bool bExists = (from name in db.StripInfos
						    where name.Title == stripName
						    select name).Count() > 0;
			return bExists;
		}

//---------------------------------------------------------------------------------------

		internal void Run() {
			StripInfo si = SelectStrip();
			CurStripName = si.Title;
			var web = new HtmlWeb();
			DoAllKindsaSetup(si, out DateTime StartDate, out DateTime EndDate);
			nDaysThisRun = 0;
			while (StartDate <= EndDate) {
				++TotalStripsThisSession;
				ProcessNextDate(StartDate, out string fmtDate, out string fullname);
				if (!File.Exists(fullname)) {
					try {
						GetStrip(web, StartDate, fmtDate, fullname);
					} catch (Exception) {
						ProcessException(StartDate);
						continue;
					}
				}
				si.LastDateCopied = StartDate;
				db.SaveChanges();
				StartDate = StartDate.AddDays(1);
			}
			FinishThingsOff();
		}

//---------------------------------------------------------------------------------------

		private void ProcessException(DateTime StartDate) {
			Console.WriteLine();    // Blank line
			Console.WriteLine($"Number of days processed: {nDaysThisRun}");
			Console.WriteLine($"This site seems to have a limit of 100 requests every half hour or so. About to sleep.");
			Console.WriteLine();    // Blank line
			Console.WriteLine($"It's currently {DateTime.Now.ToShortTimeString()}");
			string ReturnTime = DateTime.Now.AddMinutes(RetryMinutes).ToShortTimeString();
			Console.WriteLine($"Sleeping for {RetryMinutes} minutes, resuming at {ReturnTime}");
			for (int i = 1; i <= RetryMinutes - 1; i++) {
				string msg = $"{Ss(RetryMinutes - i, "minute")}";
				Thread.Sleep(OneMinute);
				Console.Beep();
				Console.Write($"Now {DateTime.Now.ToShortTimeString()}, {msg} left  \r");
			}
			Console.WriteLine();
			Console.WriteLine($"Resuming downloading at {DateTime.Now.ToShortTimeString()}");
			Console.Beep(880, 3 * 1000);
			StartDate.AddDays(-1);
			nDaysThisRun = 0;
		}

//---------------------------------------------------------------------------------------

		private void FinishThingsOff() {
			Console.WriteLine();        // Blank line
			Console.WriteLine($"Total strips this session: {TotalStripsThisSession}");
			int nGrandTotal = 0;
			foreach (string item in Directory.EnumerateFiles(BaseDir, "*.png", SearchOption.AllDirectories)) {
				++nGrandTotal;
			}
			Console.WriteLine($"Total strips in {BaseDir}): {nGrandTotal}");
		}

//---------------------------------------------------------------------------------------

		private void DoAllKindsaSetup(StripInfo si, out DateTime StartDate, out DateTime EndDate) {
			TotalStripsThisSession = 0;
			BaseDir = @"G:\lrs\Comics\" + si.Title;
			BaseUrl = $"https://www.gocomics.com/{si.Title}/";
			Directory.CreateDirectory(BaseDir);
			StartDate = si.LastDateCopied.AddDays(1);
			EndDate = DateTime.Now;
			Console.WriteLine($"Processing {StartDate.ToShortDateString()}");
		}

//---------------------------------------------------------------------------------------

		private void ProcessNextDate(DateTime StartDate, out string fmtDate, out string fullname) {
			CurMonth = StartDate.ToString("MMMM");
			CurYear  = StartDate.ToString("yyyy");
			DoEndOfMonth();
			SetupNewMonth(StartDate, out fmtDate, out fullname);
		}

//---------------------------------------------------------------------------------------

		private void GetStrip(HtmlWeb web, DateTime CurDate, string fmtDate, string fullname) {
			var doc = web.Load(BaseUrl + fmtDate);
			++nDaysThisRun;
			doc.DocumentNode.Descendants()
				.Where(n => n.Name == "picture")
				.ToList()
				.ForEach(n => ProcessPicture(n, CurDate, fullname));
			Thread.Sleep(InterComicDelay);
		}

//---------------------------------------------------------------------------------------

		private void SetupNewMonth(DateTime StartDate, out string fmtDate, out string fullname) {
			fmtDate = StartDate.ToString("yyyy/MM/dd");
			string targetDir = Path.Combine(BaseDir, StartDate.ToString(@"yyyy-MM-MMMM"));
			Directory.CreateDirectory( targetDir);   // Ignored if already exists
			string filename = $"{StartDate:dd-dddd-MMMM-yyyy}.png";
			fullname = Path.Combine(targetDir, filename);
		}

//---------------------------------------------------------------------------------------

		void DoEndOfMonth() {
			if (CurMonth != PrevMonth) {
				if (PrevMonth.Length > 0) {
					Console.Write($"{nStripsThisMonth,2} strip{Ss(nStripsThisMonth)} in ");
					var Elapsed = DateTime.Now - TimeStampThisMonth;
					Console.WriteLine(Elapsed.ToString(@"hh\:mm\:ss"));
					if (PrevMonth == "December") {
						Console.WriteLine("-----------------------");
					}
				}
				if ((nStripsThisMonth == 0) && !bSkippingMsgIssued) { GiveSkippingMessage(); }
				if (nStripsThisMonth > 0) { bSkippingMsgIssued = false; }
				string StartingMsg = $"Starting {CurYear} {CurMonth}";
				Console.Write(StartingMsg.PadRight(25));
				PrevMonth = CurMonth;
				nStripsThisMonth = 0;
				TimeStampThisMonth = DateTime.Now;
			}
		}

//---------------------------------------------------------------------------------------

		private void GiveSkippingMessage() {
			if (WhenLastStripFound != default) {
				Console.WriteLine();
				Console.WriteLine($"Empty month. Last strip found was {WhenLastStripFound.ToShortDateString()}");
				string datestamp = WhenLastStripFound.ToString("MM/dd");
				string url = $"https://www.gocomics.com/{CurStripName}/{datestamp}";
				Console.WriteLine($"Suggestion: Go manually to {url} and click the 'Next' button and note the date");
				Console.WriteLine("in the browser's address bar, e.g. https://www.gocomics.com/lil-abner/2011/07/16");
				Console.WriteLine($"Modify your {Comics_txt} file to include >{CurStripName} and the new date (e.g. 2011/07/16");
				Console.WriteLine("And restart this program. ");
				bSkippingMsgIssued = true;
				// TODO: Check to see if restart date in >xxx is earlier than next date in database. In which case, ignore line.
				// TODO: and delete the last msg (And restart...)
				Console.WriteLine("Meanwhile, we'll keep searching for non-empty months, in case you're running this overnight");
				Console.WriteLine();
			}
		}

//---------------------------------------------------------------------------------------

		string Ss(string s) => s.Length == 1 ? " " : "s";

//---------------------------------------------------------------------------------------

		string Ss(int n) => n == 1 ? " " : "s";

//---------------------------------------------------------------------------------------

		string Ss(int n, string s) => $"{n} {s}{Ss(n)}";

//---------------------------------------------------------------------------------------

		private StripInfo SelectStrip() {
			var OrderedStrips = Strips.OrderBy(s => s.Title).Select(s => s).ToArray();
			Console.WriteLine("The available strips are:");
			while (true) {
				for (int i = 0; i < OrderedStrips.Length; i++) {
					Console.WriteLine($"{i + 1,2}:  {OrderedStrips[i].Title}");
				}
				Console.Write("Please enter the number of the strip you want: ");
				bool bOK = int.TryParse(Console.ReadLine(), out int num);
				if (bOK && (num >= 1) && (num <= OrderedStrips.Length)) {
					var strip = OrderedStrips[num - 1];
					// var si = db.StripInfos.Where(s => s.Title == "Peanuts");
					StripInfo si = db.StripInfos.Where(s => s.Title == strip.Title).First();
					// return si.Take(1) as StripInfo;
					return si;
				}
				Console.WriteLine("Response was not valid. Try again");
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessPicture(HtmlNode n, DateTime CurDate, string fullname) {
			string val = n.Attributes["class"].Value;
			if (val == "item-comic-image") {
				string url = n.FirstChild.Attributes["data-srcset"].Value.Split(' ')[0];
				if (File.Exists(fullname)) { return; }
				wc.DownloadFile(url, fullname);
				WhenLastStripFound = CurDate;
				++nStripsThisMonth;
				return;
			}
		}
	}
}
