// Copyright (c) 2006 Bartizan Connects LLC

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.EnterpriseServices.Internal; 

namespace BartGACInstall {
	class Program {
		static int Main(string[] args) {
			if (args.Length == 0)
				return 1;
			Publish objPublish = new Publish(); 
			objPublish.GacInstall(args[0]); 
			return 0;
		}
	}
}
