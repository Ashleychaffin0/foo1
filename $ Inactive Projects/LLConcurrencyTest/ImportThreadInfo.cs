// Copyright (c) 2008 by Bartizan Connects, LLC

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace LLConcurrencyTest {
	class ImportThreadInfo {
		// Since this is only an internal utility, I'm not going to bother with
		// a cumbersome constructor. Instantiate this class and set the values
		// manually.
		public string	UserID;
		public string	RCUserID;
		public string	RCPassword;
		
		public int		MapCfgID;
		public int		EventID;

		public string	TerminalID;

		public ImporterStatusControl	ctl;
		
		[XmlIgnore]
		public Thread	ThisThread;

		public string	OutputPath;

		// Used to serialize reporting
		public static string	ReportLock = DateTime.Now.ToString().Replace("/", "-").Replace(":", ".");
	}
}
