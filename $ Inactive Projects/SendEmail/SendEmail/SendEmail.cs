using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SendEmail {
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class SendEmail	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)	{
#if DEBUG
			args = new string[5];
			args[0] = "mail.bartizan.com";
			args[1] = "larrysmith@bartizan.com";
			args[2] = "larry@bartizan.com";
			args[3] = "[AT]: ";
			args[4] = "visitor.txt";
#else
			if (args.Length != 5) {
				Console.WriteLine("Usage: SendEmail email_server    From                 To                    Subject filename");
				Console.WriteLine("  e.g. SendEmail www.hotmail.com John.Doe@hotmail.com Richard.Roe@yahoo.com Hiya!   foo.txt");
				return;
			}
#endif

			string	s;
			// TODO: Put try/catch 

			TcpClient	tcp = new TcpClient(args[0], 25);
			NetworkStream	ns = tcp.GetStream();
			s = Recv(ns);				// Get welcome message

			Send(ns, "HELO foo");
			s = Recv(ns);

			Send(ns, "mail from: test@bartizan.com");	// Return path. Also envelope from
			s = Recv(ns);

			Send(ns, "rcpt to: larry@bartizan.com");
			s = Recv(ns);

			Send(ns, "data");
			s = Recv(ns);
			Send(ns, "from: " + args[1]);				// Shows as From
			Send(ns, "to: " + args[2]);					// Shows as To
			Send(ns, "subject: " + args[3]);			// Real subject

			StreamReader	sr = new StreamReader(args[4], System.Text.Encoding.ASCII);
			string msg = sr.ReadToEnd();

			Send(ns, msg);
			Send(ns, ".");
			s = Recv(ns);

			Send(ns, "QUIT\r\n");
			s = Recv(ns);
		}

		static void Send(NetworkStream	ns, string msg) {
			msg = msg + "\n";
			Console.WriteLine(">>> {0}", msg);
			ns.Write(System.Text.Encoding.ASCII.GetBytes(msg), 0, msg.Length);
		}

		static string Recv(NetworkStream ns) {
			byte [] msg = new byte[1024];		// None of our messages will be longer than this
			int nBytes = ns.Read(msg, 0, msg.Length);
			string s = System.Text.Encoding.ASCII.GetString(msg, 0, nBytes);
			Console.WriteLine("<<< {0}", s);
			return s;
		}
	}
}
