using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectoryWalker_2 {
	class DirectoryWalker_2 {
		static void Main(string[] args) {
			string DirName;
			DirName = @"C:\LRS\$ BBC Consolidated";
			DirName = @"C:\lrs\bin";
			var dw = new DirectoryWalker(DirName);
			foreach (var item in dw.Walk()) {
				if (item.IsDirectory)
					Console.WriteLine("\n{0}", item.DirInfo.FullName);
				else
					Console.WriteLine("\t{0}", item.FileInfo.FullName);
			}
		}
	}
}
