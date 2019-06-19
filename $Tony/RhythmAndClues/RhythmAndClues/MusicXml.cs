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

		internal XmlNodeList GetMeasures() => xdoc.GetElementsByTagName("measure");

//---------------------------------------------------------------------------------------

		internal XmlNodeList GetNotes() => xdoc.GetElementsByTagName("note");

//---------------------------------------------------------------------------------------

		public void Save(string filename) {
			xdoc.Save(filename);
		}
	}
}
