using System;
using System.Net;

namespace LrsTcpMiniClient {
	class Program {
		static void Main(string[] args) {
			const int PORT = 5050;
			var client = new MiniClient(PORT);
			Console.WriteLine(client.SendRecv("the cat is black"));
			Console.WriteLine(client.SendRecv("my dog has fleas"));
		}
	}
}
