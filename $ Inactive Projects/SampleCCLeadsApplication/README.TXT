The SampleCCLeadsApplication program is a demonstration on how to process data
sent via Bartizan's ccLeads feature.

It is a console application that consists of a mainline program (TestCCLeads.cs)
and an auxiliary file, BartizanCcLeads.cs. This latter module contains class
definitions that allow easy access to ccLeads data.

The program loads in an XML file from disk, parses it, then writes out a simple
formatted version of the XML data.

To run it, simply invoke the program from the command line with the name of the 
XML file as the only parameter. For example,

	SampleCCLeadsApplication SampleCCLeadsData.xml
	
A sample XML file called SampleCCLeadsData.xml can be found in the same
directory as the *.cs files.
	
See the comments at the beginning of each source module for more details.