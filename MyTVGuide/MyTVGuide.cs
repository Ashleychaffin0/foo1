using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// Notes:
//	*	120 pixels = 30 minutes. Therefore 4 pixels/minute

using mshtml;

namespace MyTVGuide {
	public partial class MyTVGuide : Form {

		DateTime BaseDate = new DateTime(1969, 12, 31, 20, 0, 0);

//---------------------------------------------------------------------------------------

		public MyTVGuide() {
			InitializeComponent();

			var ts1              = 1278990000000;	// 11 PM, Monday, Jul 12, 2010
			var fromTimeInMillis = 1278990000000;
			var	matrixStartDate  = 1278905752795;
			var matrixEndDate    = 1280201752795;

			ShowDate(matrixEndDate);
		}

//---------------------------------------------------------------------------------------

		private void ShowDate(long ts1) {
			TimeSpan ts = TimeSpan.FromMilliseconds(ts1);
			Console.WriteLine("ShowDate of {0} = {1}", ts1, BaseDate + ts);
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			web1.Navigate("http://affiliate.zap2it.com/tvlistings/ZCGrid.do?aid=symbel2y2");
		}

//---------------------------------------------------------------------------------------

		private void web1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			string CurDate = GetCurrentDate();
			List<string> slots = GetTimeSlots();
			GetChannels();
		}

//---------------------------------------------------------------------------------------

		private void GetChannels() {
			int num = 0;
			while (true) {
				var chan = web1.Document.GetElementById((++num).ToString());
				if (chan == null) {
					break;
				}
				Console.WriteLine("Found element {0}", num);
				var sib = chan.NextSibling;						// Really, a Table
				var table = sib.DomElement as HTMLTable;		// See?
				var row0 = table.rows.item(0) as HTMLTableRow;	// First and only row
				var cells = row0.cells;
				for (int i = 0; i < cells.length; i++) {
					var cell = cells.item(i) as HTMLTableCell;
					var style = cell.style;
					var width = style.posWidth;
					// var attrs = cell.attributes;
					// var attr = attrs[0];
					// var x = 0;
					string innerText = cell.innerText;
					//string innerHTML = cell.innerHTML;
					//string outerText = cell.outerText;
					// string outerHTML = cell.outerHTML;
					// Console.WriteLine("[{0}] Width={1}, Cell.innerText={2}, .innerHtml={3}", i, width, cell.innerText, cell.innerHTML);
					Console.WriteLine("[{0}] Width={1}, Cell.outerText={2}, cell.outerHTML={3}, innerText={4}", i, width, cell.outerText, cell.outerHTML, innerText);
				}
				int j = 1;
			}
		}

//---------------------------------------------------------------------------------------

		private string GetCurrentDate() {
			return "N/A";						// TODO:
		}

//---------------------------------------------------------------------------------------

		private List<string> GetTimeSlots() {
			var list = new List<string>();
			var doc = web1.Document;
			var SlotSection = doc.GetElementById("zc-tn-top");
			foreach (HtmlElement Slot in SlotSection.All) {
				if (Slot.TagName == "DIV") {
					var SlotDiv = Slot.DomElement as mshtml.HTMLDivElement;
					string className = SlotDiv.className;
					if (className == "zc-tn-t") {
						list.Add(SlotDiv.innerText);
					}
				}
			}
			return list;
		}
	}
}
