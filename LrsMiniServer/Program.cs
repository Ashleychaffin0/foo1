using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LrsMiniServer {
	class Program {
		static void Main(string[] args) {
			const int PORT = 5050;
			var server = new TcpListener(IPAddress.Parse("127.0.0.1"), PORT);
			server.Start();

			const int BUFSIZE = 40960;
			var bytes = new byte[BUFSIZE];

			// Enter the listening loop.
			while (true) {
				Console.Write("Waiting for a connection... ");

				// Perform a blocking call to accept requests.
				var client = server.AcceptTcpClient();
				Console.WriteLine("Connected!");

				// Get a stream object for reading and writing
				NetworkStream stream = client.GetStream();

				int i;
				// Loop to receive all the data sent by the client.
				try {
					while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) {
						string data = Encoding.ASCII.GetString(bytes, 0, i);
						Console.WriteLine($"Received: {data}");

						// Process the data sent by the client.
						data = data.ToUpper();

						byte[] msg = Encoding.ASCII.GetBytes(data);

						// Send back a response.
						stream.Write(msg, 0, msg.Length);
						Console.WriteLine($"Sent: {data}");
					}
				} catch	(IOException) {
					Console.WriteLine($"Presumably the client closed the connection.");
				}
			}

		}
	}
}
