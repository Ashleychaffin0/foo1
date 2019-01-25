using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace WinFormsChannel9 {
	static class DoOutput {

		public static void PumpOutAsHtml(List<Channel9Video> Videos) {
			PumpOutAsHtmShort(Videos);
			PumpOutAsHtmlFull(Videos);
		}

//---------------------------------------------------------------------------------------

		private static void PumpOutAsHtmShort(List<Channel9Video> Videos) {
			var sb = new StringBuilder();
			sb.Append("<HTML>\n<HEAD><TITLE>LRS Channel 9 List (Short)</TITLE></HEAD>");
			sb.Append("\n<BODY>");

			sb.Append("\n<TABLE BORDER=\"1\">");

			sb.Append("\n\t<THEAD>");
			sb.Append("\n\t\t<TR>");
			sb.Append("\n\t\t\t<TH>Title</TH>");
			sb.Append("\n\t\t\t<TH>Description</TH>");
			sb.Append("\n\t\t</TR>");
			sb.Append("\n\t</THEAD>");

			foreach (var vid in Videos) {
				sb.Append("\n\t<tr>");
				sb.WriteTd(2, vid.Title);
				sb.WriteTd(2, vid.Description);

				sb.Append("\n\t</tr>");
			}

			sb.Append("\n</TABLE>");

			sb.Append("\n</BODY>\n</HTML>\n");

			string FileName = @"C:\LRS\Channel9-Short.html";
			using (var sw = new StreamWriter(FileName)) {
				sw.Write(sb.ToString());
			}

			Process.Start(FileName);
		}

//---------------------------------------------------------------------------------------

		private static void PumpOutAsHtmlFull(List<Channel9Video> Videos) {
			var sb = new StringBuilder();
			sb.Append("<HTML>\n<HEAD><TITLE>LRS Channel 9 List</TITLE></HEAD>");
			sb.Append("\n<BODY>");

			sb.Append("\n<TABLE BORDER=\"1\">");

			sb.Append("\n\t<THEAD>");
			sb.Append("\n\t\t<TR>");
			sb.Append("\n\t\t\t<TH>Page</TH>");
			sb.Append("\n\t\t\t<TH>Date</TH>");
			sb.Append("\n\t\t\t<TH>Title</TH>");
			sb.Append("\n\t\t\t<TH>Description</TH>");
			sb.Append("\n\t\t\t<TH>Topics</TH>");
			sb.Append("\n\t\t\t<TH>Time</TH>");
			sb.Append("\n\t\t\t<TH WIDTH=\"100\">Link</TH>");
			sb.Append("\n\t\t</TR>");
			sb.Append("\n\t</THEAD>");

			foreach (var vid in Videos) {
				sb.Append("\n\t<tr>");
				sb.WriteTd(2, vid.PageNumber.ToString());
				sb.WriteTd(2, vid.ArticleDate.ToShortDateString());
				sb.WriteTd(2, vid.Title);
				sb.WriteTd(2, vid.Description);

				sb.WriteTd(2, vid.KeywordsString);
				sb.WriteTd(2, vid.TimeCaption);

				string s = "<FONT SIZE=\"-2\">" 
					+ "<A HREF=\"" + vid.Link + "\">" + vid.Link.Replace("/", " / ")
					+ "</FONT></A>";
				sb.WriteTd(100, 2, s);

				sb.Append("\n\t</tr>");
			}

			sb.Append("\n</TABLE>");

			sb.Append("\n</BODY>\n</HTML>\n");

			string FileName = @"C:\LRS\Channel9.html";
			using (var sw = new StreamWriter(FileName)) {
				sw.Write(sb.ToString());
			}

			Process.Start(FileName);
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	static class ExtensionMethods {

//---------------------------------------------------------------------------------------

		public static void WriteTd(this StringBuilder sb, int IndentLevel, string Text) {
			sb.Append("\n");
			sb.Append("".PadRight(IndentLevel, '\t'));
			sb.Append("<td>");
			sb.Append(Text);
			sb.Append("</td>");
		}

//---------------------------------------------------------------------------------------

		public static void WriteTd(this StringBuilder sb, int Width, int IndentLevel, string Text) {
			sb.Append("\n");
			sb.Append("".PadRight(IndentLevel, '\t'));
			sb.Append("<td width=\"" + Width + "\">");
			sb.Append(Text);
			sb.Append("</td>");
		}

	}
}
