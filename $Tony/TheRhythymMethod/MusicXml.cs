using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TheRhythymMethod {
	class MusicXml {
		internal string		 filename;
		internal XmlDocument xdoc;
		internal List<(string PartName, XmlElement ScorePart, string id)> Parts;
		internal XmlElement  CurrentPart;
		internal XmlNodeList Measures;          // In CurrentPart
		internal int		 StartMeasure;		// Maybe someday use Span<Measure>?
		internal int		 EndMeasure;
		internal Interpreter Interp;

//---------------------------------------------------------------------------------------

		public MusicXml(string filename, string programName = null) {
			this.filename = filename;

			xdoc = new XmlDocument();
			xdoc.Load("Music.xml");

			GetParts();
			CurrentPart = GetPart(Parts[0].id);		// Default to Part 0 (i.e. Part 1)
			Measures    = GetMeasures();
			Interp      = new Interpreter(this, programName);
		}

//---------------------------------------------------------------------------------------

		private void GetParts() {
			Parts     = new List<(string PartName, XmlElement ScorePart, string id)>();
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

		public void SelectPart(string name) {
			foreach (var item in Parts) {
				if (item.PartName == name) {
					CurrentPart = item.ScorePart;
					return;
				}
			}
			throw new Exception($"Part '{name}' not found");
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
			xdoc.Save(filename);
		}
	}
}
