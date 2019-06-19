using System;
using System.Collections.Generic;
using System.Xml;

namespace RhythmAndClues {
	class MusicXml {
		internal string			filename;
		internal XmlDocument	xdoc;
		internal XmlElement		CurrentPart;
		internal XmlNodeList	Measures;			// In CurrentPart
		internal int			StartMeasure;		// Maybe someday use Span<Measure>?
		internal int			EndMeasure;
		internal Interpreter	Interp;

		internal List<(string PartName, XmlElement ScorePart, string id)> Parts;

//---------------------------------------------------------------------------------------

		public MusicXml(string filename, string programName = null) {
			this.filename = filename;

			xdoc = new XmlDocument();
			xdoc.Load("Music.xml");

			GetParts();
			CurrentPart = GetPart(Parts[0].id);     // Default to Part 0 (i.e. Part 1)
			Measures    = GetMeasures();
			Interp      = new Interpreter(this, programName);
		}

//---------------------------------------------------------------------------------------

		private void GetParts() {
			// TODO: Just get <score-part>?
			Parts = new List<(string PartName, XmlElement ScorePart, string id)>();
			var nodes = xdoc.GetElementsByTagName("part-name");
			foreach (XmlElement node in nodes) {
				XmlElement parent = node.ParentNode as XmlElement;
				string id = parent.GetAttribute("id");
				Parts.Add((node.InnerText, node.ParentNode as XmlElement, id));
			}
		}

//---------------------------------------------------------------------------------------

		public List<string> GetPartNames() {
			var names = new List<string>();
			foreach (var name in Parts) {
				names.Add(name.PartName);
			}
			return names;
		}

//---------------------------------------------------------------------------------------

		private XmlElement GetPart(string id) {
			var nodes = xdoc.GetElementsByTagName("part");
			foreach (XmlElement node in nodes) {
				if (node.GetAttribute("id") == id) {
					return node;
				}
			}
			throw new Exception($"Internal error - GetPart({id}) not found");
		}

//---------------------------------------------------------------------------------------

		public void SelectPart(int num) {
			// TODO: Test this with invalid name
			foreach (var item in Parts) {
				if (item.PartName == "P1") {
					CurrentPart = item.ScorePart;
					return;
				}
			}
			throw new Exception($"Part '{num}' not found");
		}

//---------------------------------------------------------------------------------------

		private XmlNodeList GetMeasures() => CurrentPart.GetElementsByTagName("measure");

//---------------------------------------------------------------------------------------

		public void SelectMeasures(int ixStartMeasure, int ixEndMeasure) {
			StartMeasure = ixStartMeasure;
			EndMeasure   = ixEndMeasure;
		}

//---------------------------------------------------------------------------------------

		public void Save(string filename) {
			// TODO: Add as command. Default filename if not on command line
			xdoc.Save(filename);
		}
	}
}
