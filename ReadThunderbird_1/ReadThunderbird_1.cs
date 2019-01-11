using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ReadThunderbird_1 {
	public partial class ReadThunderbird_1 : Form {
		Regex									reEmailAddr;
		HashSet<(string Address, string Name)>	Addresses;
		List<(string Address, string Name)>		AddressKeys;

//---------------------------------------------------------------------------------------

		public ReadThunderbird_1() {
			InitializeComponent();

			string EmailAddrPattern = @"[a-z0-9\._]+@[a-z0-9\.]+";
			// TODO: Maybe EmailAddrPattern = @"[~ @,]+@[a-z0-9\.]";
			EmailAddrPattern = @"(?<Name>.*?)(?<Addr>[a-z0-9\._]+@[a-z0-9\.]+)";
			reEmailAddr = new Regex(EmailAddrPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

			Addresses = new HashSet<(string Address, string Name)>();
		}

//---------------------------------------------------------------------------------------

		private void ReadThunderbird_1_Load(object sender, EventArgs e) {
			// TODO: Cheat - hardcoding the filename
			// txtInboxFilename.Text = @"C:\Users\LRS9450\AppData\Roaming\Thunderbird\Profiles\2fseng4k.default\Mail\pop.broadband.rogers.com\Inbox";
			// txtInboxFilename.Text = @"C:\Users\LRS5\AppData\Roaming\Thunderbird\Profiles\v7tcbjhb.default\Mail\pop3.lrs5.net\Inbox";

			// TODO: See Registry key HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\UnreadMail\lrs5@lrs5.net
			//		 => "G:\Program Files (x86)\Mozilla Thunderbird\thunderbird.exe" -profile "C:\Users\lrs5\AppData\Roaming\Thunderbird\Profiles\v7tcbjhb.default" -mail
			txtInboxFilename.Text = @"C:\Users\LRS5\AppData\Roaming\Thunderbird\Profiles\v7tcbjhb.default\Mail\pop3.lrs5.net\Sent";
			txtInboxFilename.Text = @"C:/Users/lrs5/AppData/Roaming/Thunderbird/Profiles/v7tcbjhb.default/Mail/pop.verizon.net/Inbox";
			// Create an empty HTML Document that we can override with our email info
			webBrowser1.Navigate("about:blank");

			#region Test Regex
// TODO: Beware of To: Bobby Breen BBreen@greenburghny.com, Louanne Figliola lufigliola@gmail.com, 
// TODO: Beware of To: judie.tulip@verizon.net
// TODO: Beware of To: "Lana Hiller" lindsaymorgan120@gmail.com,

			string pat, txt;
			bool QuitSwitch = false;
			while (! QuitSwitch) {
				var dt = DateTime.Now;
				Console.WriteLine($"\r\n------------------------");
				Console.WriteLine($"{dt.ToLongTimeString()}");
				// pat = @"(?<Name>.*?)(?<Addr>[a-z0-9\._]+@[a-z0-9\.]+)";
				pat = @"((?<Name>.*?)(?<Addr>@[a-z0-9\.]+,?))+";
				pat = @"(?<as>(
							(a|b)+,?)
						)*";
				// pat = "((?<as>(a|b)+),)*";
				reEmailAddr = new Regex(pat, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
				// txt = "John Doe abc@def.com";
				txt = "abc@def.com";
				txt = "\"Lana Hiller\" lindsaymorgan120@gmail.com, larry lrs5@lrs5.net";
				txt = "aaa,bbb";
				Console.WriteLine($"txt = {txt}");
				var Matches = reEmailAddr.Matches(txt);
				foreach (Match match in Matches) {
					Console.WriteLine($"Match -- {match}");
					foreach (Match cap in match.Captures) {
						Console.WriteLine($"\tCapture -- " + cap);
						foreach (Group grp in cap.Groups) {
							Console.WriteLine($"\tCapture Group -- ['{grp.Name}'] = {grp}");
						}
					}
					foreach (Group grp in match.Groups) {
						Console.WriteLine($"\tGroup -- ['{grp.Name}'] = {grp.Value}");
					}
				}
				int i = 1;
				// QuitSwitch = true;
			}
			#endregion
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			string TBDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			TBDir = Path.Combine(TBDir, "Thunderbird");
			var ofd = new OpenFileDialog();
			ofd.InitialDirectory = TBDir;
			var rc = ofd.ShowDialog();
			if (rc == DialogResult.OK) {
				txtInboxFilename.Text = ofd.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			lbMsgs.Items.Clear();
			Addresses.Clear();
			var sw = new Stopwatch();
			sw.Start();
			ProcessFile();
			sw.Stop();
			string msg = string.Format("Elapsed time = {0}", sw.Elapsed);
			MessageBox.Show(msg);
			AddressKeys = Addresses.ToList();
			AddressKeys.Sort();
		}

//---------------------------------------------------------------------------------------

        private void ProcessFile() {
            var q1 = from msg in GetMailMessages()// .AsParallel()
                     select msg;
            foreach (var item in q1) {
                Console.WriteLine("msg = {0}", item);
           }
        }

//---------------------------------------------------------------------------------------

        private IEnumerable<TBirdMailMessage> GetMailMessages() {
            var MsgLines = new List<string>(5000);		// Allow for long emails
            int LineNo = 0;
            int nMsgs = 0;
            const int MaxMessages = 100000;				// For testing
			int StartingLine = 1;
            var lines = File.ReadAllLines(txtInboxFilename.Text);
            string line = "";
			foreach (string ln in lines) {
                ++LineNo;
                // if (ln.StartsWith("From ")) {
                if (ln.StartsWith("To: ")) {
                    line = ln.Replace("<", "")
                        .Replace(">", "")
                        .Replace("&lt;", "")
                        .Replace("&gt;", "")
						.Substring(4);
					var addrs = reEmailAddr.Matches(line);
					// TODO: Pick up person's name as well
					foreach (Match item in addrs) {
						if (item.Value.StartsWith("noreply@")) continue;	// TODO: Doesn't work???
						// TODO: Restore as tuple: Addresses.Add(item.Value);
					}
                    //if (line.Substring(4).ToLower().Contains("lrs5@lrs5.net")) continue;
                    // New message starts here. Process old one
                    // Console.WriteLine("Msg at line {0}", LineNo);
                    if (MsgLines.Count > 0) {        // First time through will be MT
						var tMsg = new TBirdMailMessage(StartingLine, MsgLines, this);
						Msg(tMsg);
						yield return tMsg;
						StartingLine = LineNo;
                        if (++nMsgs > MaxMessages) {
                            yield break;
                        }
                        MsgLines.Clear();
                    }
                }
                MsgLines.Add(line);
            }
            // We've got the final message to process. But we may have an emtpy file, so
            // check to see if we have anything.
            if (MsgLines.Count > 0) {
                yield return new TBirdMailMessage(LineNo, MsgLines, this);
            }
        }

//---------------------------------------------------------------------------------------

		public void Msg(string fmt, params object[] vals) {
			string msg = string.Format(fmt, vals);
			lbMsgs.Items.Add(msg);
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		public void Msg(object tag) {
			lbMsgs.Items.Add(tag);		// Assumes it has a decent ToString()
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void lbMsgs_Click(object sender, EventArgs e) {
			int n = lbMsgs.SelectedIndex;
			object o = lbMsgs.Items[n];
			if (o.GetType() != typeof(string)) {
				string body;
				TBirdMessagePart mp = o as TBirdMessagePart;
				if (mp != null) {
					switch (mp.ContentType) {
					case "text/html":
						body = string.Join("\n", mp.Body.ToArray());
						webBrowser1.Document.Body.InnerHtml = body;
						break;
					default:
						body = string.Join("\n", mp.Body.ToArray());
						body = body.Replace("\n", "\n<br>");
						webBrowser1.Document.Body.InnerHtml = body;
						break;
					}
				}
			}
		}
    }

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

    class TBirdMailMessage {
        public string		From;
        public string		To;
        public DateTime		Datetime;
        public string		Subject;
		public List<string> Body = new List<string>();

		public List<TBirdMessagePart> MsgParts = new List<TBirdMessagePart>();
		
		public int 		LineNo;				// Line # in file where msg starts
		public int		LineCount;

		private string  boundaryStart;		// e.g. "--Hello"
		private string	boundaryEnd;		// e.g. "--Hello--"

		// We'll implement a small FSM to keep track of what we're doing.
		enum ScanningState {			// What are we scanning at this point?
			Headers,
			ContentType_MultipartAlternative,
			Body,
			PartHeader,
			PartBody
		};

//---------------------------------------------------------------------------------------

		// Note: This routine processes Thunderbird Inbox files. It is *not* a full,
		//		 general purpose parser of the data. It does just enough to get the idea
		//		 across. Sort of a proof of concept thing.
        public TBirdMailMessage(int LineNo, List<string> RawMailMessage, ReadThunderbird_1 GUI) {
			// My first version of this used a regular expression to do an initial parse
			// on the RawMailMessage (then just a string). But it got confused when some
			// messages came in as "From .. To ...", while others came in as "To ...
			// From ...". So rather than using the following RE, appropriately 
			// generalized, we'll do a quick manual scan.
#if false
				string pat = @"From: (?<From>[^\n]*).*?"
					+ "To: (?<To>[^\n]*).*?" 
					+ "Subject: (?<Subject>[^\n]*).*?"
					+ "Date: (?<Date>[^\n]*).*?"
					;
                re = new Regex(pat, RegexOptions.Compiled | RegexOptions.Singleline);
#endif

			this.LineNo = LineNo;
			LineCount = RawMailMessage.Count;
			ScanningState State = ScanningState.Headers;
			TBirdMessagePart CurPart = null;
			// Brute force. We could make it fancier, table-driven, etc, but why bother?
			foreach (string line in RawMailMessage) {
				switch (State) {
				case ScanningState.Headers:
					State = ScanHeaders(line);
					break;
				case ScanningState.ContentType_MultipartAlternative:
					// Looking for multipart boundary string
					State = ScanMultipartAlternative(line);
					break;
				case ScanningState.Body:
					State = ScanBody(ref CurPart, line);
					break;
				case ScanningState.PartHeader:
					if (line.ToLower().StartsWith("content-type: ")) {
						CurPart.ContentType = line.Substring(14);
						int n = CurPart.ContentType.IndexOf(';');
						if (n >= 1) {
							CurPart.ContentType = CurPart.ContentType.Substring(0, n);
						}
					} else if (line.Length == 0) {
						State = ScanningState.PartBody;
					} else {
						// We're not interested in other parts of the Part header. Ignore
					}
					break;
				case ScanningState.PartBody:
					if (line == boundaryStart) {
						// Is this the first Part, or should we close out the previous
						// one and start another?
						if (CurPart.Body.Count > 0) {
							MsgParts.Add(CurPart);
							GUI.Msg(CurPart);
							CurPart = new TBirdMessagePart();
							State = ScanningState.PartHeader;
						}
					} else if (line == boundaryEnd) {
						MsgParts.Add(CurPart);
						GUI.Msg(CurPart);
						State = ScanningState.Body;
					} else {
						CurPart.Body.Add(line);
					}
					break;
				default:
					break;
				}
			}
        }

//---------------------------------------------------------------------------------------

		private ScanningState ScanBody(ref TBirdMessagePart CurPart, string line) {
			ScanningState State = ScanningState.Body;
			if (boundaryStart != null) {
				if (line.StartsWith(boundaryStart)) {
					CurPart = new TBirdMessagePart();
					State = ScanningState.PartHeader;
				} else if (line.StartsWith(boundaryEnd)) {
					State = ScanningState.Body;
				} else {
					Body.Add(line);
				}
			} else {
				Body.Add(line);
			}
			return State;
		}

//---------------------------------------------------------------------------------------

		private ScanningState ScanHeaders(string line) {
			ScanningState NextState = ScanningState.Headers;	// Still in Headers
			if		  (line.StartsWith("From: ")) {
				From = line.Substring(6);
			} else if (line.StartsWith("To: ")) {
				To = line.Substring(4);
			} else if (line.StartsWith("Subject: ")) {
				Subject = line.Substring(9);
			} else if (line.StartsWith("Date: ")) {
				string date = line.Substring(6);
				// TODO: Need more general date parser than this
				DateTime.TryParse(date, out Datetime);
			} else if (line.ToLower().StartsWith("content-type: multipart/alternative")) {
				// Duh, the boundary string may be on this line or the next. If on this
				// line, fine. If not, slip into the Multipart state for at most one line
				boundaryStart = GetBoundary(line);
				if (boundaryStart == null) {
					NextState = ScanningState.ContentType_MultipartAlternative;
				}
			} else if (line.Length == 0) {
				// End of headers. Before we go on to the next state, see if we got a
				// boundary. If so, prepend "--" to it.
				if (boundaryStart != null) {
					boundaryStart = "--" + boundaryStart;
					boundaryEnd = boundaryStart + "--";
				}
				NextState = ScanningState.Body;
			}
			return NextState;
		}

//---------------------------------------------------------------------------------------

		private string GetBoundary(string line) {
			int n = line.IndexOf("boundary=\"");
			if (n < 0)
				return null;
			boundaryStart = line.Substring(n + 10);	// 10 = length of "boundary=\""
			// This has a trailing ". Cut it.
			boundaryStart = boundaryStart.Substring(0, boundaryStart.Length - 1);
			return boundaryStart;
		}

//---------------------------------------------------------------------------------------

		private ScanningState ScanMultipartAlternative(string line) {
			// We didn't find the boundary string on the Content-Type line, so try the
			// next line. We always return in Header state.
			boundaryStart = GetBoundary(line);
			// If not found, tough. Remember, this isn't trying to be a bulletproof
			// program.
			return ScanningState.Headers;
		}

//---------------------------------------------------------------------------------------

        public override string ToString() {
            return string.Format("LineNos = {0}-{1}, From: {2}, To: {3}, Date: {4}, Subject: {5}, boundary = {6}",
                LineNo, LineNo + LineCount - 1, From, To, Datetime, Subject, boundaryStart);
        }
    }

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class TBirdMessagePart {
		public string		ContentType;
		public List<string>	Body = new List<string>();

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return "        content: " + ContentType;
		}
	}
}
