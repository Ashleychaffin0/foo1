using System;
using System.Net.Sockets;
using System.Text;

namespace LrsTcpMiniClient {
	class MiniClient {
		int			Port;
		TcpClient	client;
		const int	BUFSIZE = 40960;

//---------------------------------------------------------------------------------------

		public MiniClient(int port) {
			Port = port;
			client = new TcpClient("127.0.0.1", Port);
		}

//---------------------------------------------------------------------------------------

		public string SendRecv(string msg) {
			var data = Encoding.ASCII.GetBytes(msg);
			NetworkStream stream = client.GetStream();

			// Send the message to the connected TcpServer.
			stream.Write(data, 0, data.Length);

			Console.WriteLine($"Sent: {msg}");
			data = new byte[BUFSIZE];

			// Read the first batch of the TcpServer response bytes.
			int nbytes = stream.Read(data, 0, data.Length);
			string responseData = Encoding.ASCII.GetString(data, 0, nbytes);
			Console.WriteLine($"Received: {responseData}");
			return responseData;
		}
	}
}
