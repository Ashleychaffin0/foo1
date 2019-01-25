// Copyright (c) 2007 by Bartizan Connects, LLC

// This sample program reads a file from disk that contains sample data that
// would be passed as XML to a Web Service. It then passes it to our sample
// "Web Service", which shows how to easily convert the XML into a data structure 
// that can be easily processed. At that point, we need not worry about XML
// any more.

// The name of the XML file is specified as the first parameter of this console
// mode program. However to ease typing, if no filename is specified, we will
// default the filename to "C:\CCLeadsSampleData.xml".

// Note: To keep things simple, this program does minimal error checking, such
//		 as whether the disk file exists, etc

using System;
using System.Collections.Generic;
using System.IO;

using Bartizan.ccLeads;

namespace SampleCCLeadsApplication {
	class TestCCLeads {
		// Usage: SampleCCLeadsApplication filename.xml
		static void Main(string[] args) {
			TestCCLeads	test = new TestCCLeads();

			string	xml = GetXmlFromFile(args);

			// Call our "Web Service"
			test.Import("UserID", "Password", "Event Name", "Terminal ID", xml);
		}

//---------------------------------------------------------------------------------------

		// Note: The only parameter used here is <XmlText>. 
		void Import(string	UserID,			
					string	Password,
					string	EventName,
					string	TerminalID,
					string	XmlText) {

			// This is all that's required to load the data into memory.
			ccLeads records = ccLeads.Load(XmlText);

			// For the real web service, we'd presumably process the data and send it to 
			// some kind of database. But for this sample program, we'll merely format it
			// out to the console.
			ccLeads.FormatRecords(records);
		}

//---------------------------------------------------------------------------------------

		private static string GetXmlFromFile(string[] args) {
			string Filename;
			if (args.Length > 0) {
				Filename = args[0];
			} else {
				Filename = @"C:\CCLeadsSampleData.xml";
			}
			using (StreamReader sr = new StreamReader(Filename)) {
				string xml = sr.ReadToEnd();
				sr.Close();
				return xml;
			}
		}
	}
}
