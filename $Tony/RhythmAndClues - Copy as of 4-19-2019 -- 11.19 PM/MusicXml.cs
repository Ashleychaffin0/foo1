using System.Xml;

namespace RhythmAndClues {
	class MusicXml {
		internal string			filename;
		internal XmlDocument	xdoc;
		internal XmlNodeList	Measures;

//---------------------------------------------------------------------------------------

		public MusicXml(string filename) {
			this.filename = filename;

			xdoc = new XmlDocument();
			xdoc.Load(filename);

			Measures = GetMeasures();
		}

//---------------------------------------------------------------------------------------

		internal XmlElement FirstPart => xdoc.GetElementsByTagName("part")[0] as XmlElement;

//---------------------------------------------------------------------------------------

		private XmlNodeList GetMeasures() => xdoc.GetElementsByTagName("measure");

//---------------------------------------------------------------------------------------

		internal XmlNodeList GetNotes() => xdoc.GetElementsByTagName("note");

//---------------------------------------------------------------------------------------

		public void Save(string filename) {
			xdoc.Save(filename);
		}

#if false       // Routines and fields from version 1
		// Fields from Version 1
		internal XmlElement		CurrentPart;
		internal int			StartMeasure;       // Note: Maybe someday use 
													//		 Span<Measure>, especially
													//		 for measures[1..]?
		internal int			EndMeasure;

		internal List<(string PartName, XmlElement ScorePart, string id)> Parts;

//---------------------------------------------------------------------------------------

		private void xxxGetParts() {
			Parts = new List<(string PartName, XmlElement ScorePart, string id)>();
			var ScoreParts = xdoc.GetElementsByTagName("score-part");
			foreach (XmlElement node in ScoreParts) {
				string id = node.GetAttribute("id");
				Parts.Add((node.InnerText, node, id));
			}
		}

//---------------------------------------------------------------------------------------

		public List<string> xxxGetPartNames() {
			var names = new List<string>();
			foreach (var name in Parts) {
				names.Add(name.PartName);
			}
			return names;
		}

//---------------------------------------------------------------------------------------

		private XmlElement xxxGetPart(string id) {
			var nodes = xdoc.GetElementsByTagName("part");
			foreach (XmlElement node in nodes) {
				if (node.GetAttribute("id") == id) {
					return node;
				}
			}
			throw new Exception($"Internal error - GetPart({id}) not found");
		}

//---------------------------------------------------------------------------------------

		public void xxxSelectPart(int num) {
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

			public void SelectMeasures(int ixStartMeasure, int ixEndMeasure) {
			StartMeasure = ixStartMeasure;
			EndMeasure   = ixEndMeasure;
		}
#endif
	}
}
