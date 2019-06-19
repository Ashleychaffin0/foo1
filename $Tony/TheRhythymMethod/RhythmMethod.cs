using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TheRhythymMethod {
		public class RhythmMethod {
			Dictionary<string, string> DictScoreParts;

//---------------------------------------------------------------------------------------

		public RhythmMethod() {
			foo();

			DictScoreParts = new Dictionary<string, string>();
			var doc        = new XmlDocument();
			doc.Load("Music.xml");

			StringBuilder sb = new StringBuilder();
			XmlNodeList Credits = doc.GetElementsByTagName("credit");
			foreach (XmlElement credit in Credits) {
				Console.WriteLine($"Credit: {credit.InnerText}");
			}

			XmlNodeList ScoreParts = doc.GetElementsByTagName("score-part");
			foreach (XmlElement ScorePart in ScoreParts) {
				ProcessScorePart(ScorePart);
			}

			var Parts = doc.GetElementsByTagName("part");
			foreach (XmlElement part in Parts) {
				XmlNodeList Measures = part.GetElementsByTagName("measure");
				foreach (XmlElement measure in Measures) {

#if false
					// XmlElement xxx = (XmlElement)measure.ChildNodes.Item(0);
					var lrs = doc.CreateElement("LRS");
					lrs.InnerText = "Now is the time";
					measure.AppendChild(lrs);
					doc.Save("Music With LRS.xml");
#endif

					int MeasureNumber = Convert.ToInt32(measure.GetAttribute("number"));
					Console.WriteLine($"Measure {MeasureNumber}");
					// TODO: <attributes>
					XmlNodeList Notes = measure.GetElementsByTagName("note");
					foreach (XmlElement note in Notes) {
						ProcessNote(note);
					}
				}
			}
			doc.Save("Music With LRS2.xml");
		}

//---------------------------------------------------------------------------------------

		private void foo() {
			var xml = new MusicXml("Music.xml", "pgm.txt");	// TODO: Parms from Main(args)
			var names = xml.GetPartNames();
			xml.SelectPart(names[0]);
			bool ExecutedOK = xml.Interp.Run();
			if (ExecutedOK) {
				xml.Save("lrs.xml");
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessScorePart(XmlElement ScorePart) {
			string id                 = ScorePart.GetAttribute("id");
			XmlNodeList PartNameNodes = ScorePart.GetElementsByTagName("part-name");
			string ScorePartName      = PartNameNodes[0].InnerText;
			DictScoreParts[id]        = ScorePartName;
			Console.WriteLine($"Part['{id}']: {ScorePartName}");
		}

//---------------------------------------------------------------------------------------

		private void ProcessNote(XmlElement note) {
			var ElemPitch = FirstElement(note, "pitch");
			string pitch = "N/A";
			if (ElemPitch != null) {
				pitch = ElemPitch.InnerText;
			}
			var ElemDuration = FirstElement(note, "duration");
			int duration = 0;
			if (ElemDuration != null) {              // TODO: Grace note or maybe Rest
				string s = ElemDuration.InnerText;
				if (s.Length > 0) {duration = Convert.ToInt32(s); }
				ElemDuration.InnerText = ElemDuration.InnerText + " LRS2";
			}
			string type = FirstElement(note, "type").InnerText;
			Console.WriteLine($"Note: {pitch} for {duration}, type {type}");
		}

//---------------------------------------------------------------------------------------

		private XmlElement FirstElement(XmlElement elem, string tagName) {
			XmlNodeList el = elem.GetElementsByTagName(tagName);
			if (el.Count == 0) { return null; }
			return el.Item(0) as XmlElement;
		}
	}
}
