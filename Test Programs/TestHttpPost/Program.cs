using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using System.Threading;
using System.IO;
using System.IO.Pipes;

using System.AddIn;
using System.AddIn.Hosting;
using System.AddIn.Pipeline;

// Sample HTTP POST code from http://msdn2.microsoft.com/en-us/library/system.net.httpwebrequest.method.aspx
// Set the 'Method' property of the 'Webrequest' to 'POST'.
//myHttpWebRequest.Method = "POST";
//Console.WriteLine ("\nPlease enter the data to be posted to the (http://www.contoso.com/codesnippets/next.asp) Uri :");

//// Create a new string object to POST data to the Url.
//string inputData = Console.ReadLine ();


//string postData = "firstone=" + inputData;
//ASCIIEncoding encoding = new ASCIIEncoding ();
//byte[] byte1 = encoding.GetBytes (postData);

//// Set the content type of the data being posted.
//myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";

//// Set the content length of the string being posted.
//myHttpWebRequest.ContentLength = byte1.Length;

//Stream newStream = myHttpWebRequest.GetRequestStream ();

//newStream.Write (byte1, 0, byte1.Length);
//Console.WriteLine ("The value of 'ContentLength' property after sending the data is {0}", myHttpWebRequest.ContentLength);

//// Close the Stream object.
//newStream.Close ();



namespace TestHttpPost {
	class Program {

//---------------------------------------------------------------------------------------
		
		static void Main(string[] args) {

			// PlayWithPOST();
			// object o = NonFunctionalEventStuff();
			// PlayWithPipes(o);

			PlayWithAdd_Ins();
		}

//---------------------------------------------------------------------------------------

		private static void PlayWithAdd_Ins() {
			string	AddInRoot = Environment.CurrentDirectory;	// Our DLL here
		}

//---------------------------------------------------------------------------------------

		private static void PlayWithPipes(object o) {
			NamedPipeServerStream npss = new NamedPipeServerStream("PipenameServer");
			NamedPipeClientStream npcs = new NamedPipeClientStream("PipenameClient");
			byte[] buf = null;
			npcs.BeginRead(buf, 0, 10, new AsyncCallback(AsyncTgt), o);
		}

//---------------------------------------------------------------------------------------

		private static object NonFunctionalEventStuff() {
			object o = null;
			WaitHandle wh = null;

			AutoResetEvent are = new AutoResetEvent(false);
			ManualResetEvent mre = new ManualResetEvent(false);
			AutoResetEvent.SignalAndWait(wh, wh);


			Monitor.Pulse(o);
			Mutex mut = new Mutex();
			Semaphore sem = new Semaphore(0, 5);
			var tie = new ThreadInterruptedException();
			var ti = new System.Threading.Timer(new TimerCallback(CallBack));
			ti.Change(500, Timeout.Infinite);

			WaitCallback wcb = new WaitCallback(CallBack);
			WaitOrTimerCallback wtc = new WaitOrTimerCallback(CallBack2);

			wh.WaitOne();
			WaitHandle.SignalAndWait(wh /*ToSignal*/, wh /*ToWaitOn*/);
			WaitHandle.WaitAll(new WaitHandle[] { wh });
			WaitHandle.WaitAny(new WaitHandle[] { wh });

			EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
			return o;
		}

//---------------------------------------------------------------------------------------

		private static void PlayWithPOST() {
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://127.0.0.1");
			req.Method = "POST";
		}

//---------------------------------------------------------------------------------------

		static void CallBack(object o) {
		}

//---------------------------------------------------------------------------------------

		static void CallBack2(object o, bool b) {
		}

//---------------------------------------------------------------------------------------

		static void AsyncTgt(IAsyncResult ar) {
			
		}

//---------------------------------------------------------------------------------------

		// http://blogs.msdn.com/bclteam/archive/2006/12/07/introducing-pipes-justin-van-patten.aspx
		public static void PipeServer() {
			using (var pipeStream = new NamedPipeServerStream("testpipe")) {
				pipeStream.WaitForConnection();

				using (StreamReader sr = new StreamReader(pipeStream)) {
					string temp;
					while ((temp = sr.ReadLine()) != null) {
						Console.WriteLine("{0}: {1}", DateTime.Now, temp);
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		public static void PipeClient() {
			using (var pipeStream = new NamedPipeClientStream("testpipe")) {
				pipeStream.Connect();

				using (StreamWriter sw = new StreamWriter(pipeStream)) {
					sw.AutoFlush = true;
					string temp;
					while ((temp = Console.ReadLine()) != null) {
						sw.WriteLine(temp);
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

	public class CCLeadsWaitObject : WaitHandle {
		ManualResetEvent	mre;
		Func<bool, object>	Process;	// i.e. bool Process(object state)

//---------------------------------------------------------------------------------------

		public CCLeadsWaitObject(Func<bool, object> Process) {
			mre = new ManualResetEvent(false);
			this.Process = Process;
		}
	}
}
