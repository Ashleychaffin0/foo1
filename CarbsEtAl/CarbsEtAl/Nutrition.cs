using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xamarin.Forms;

namespace CarbsEtAl {
	class Nutrition {
		public string Name { get; set; }
		public string Unit { get; set; }
		public string Protein { get; set; }
		public string Carbs { get; set; }
		public string Calories { get; set; }
		public string Fat { get; set; }
		public string SatFat { get; set; }

//---------------------------------------------------------------------------------------

		public Nutrition(XmlElement node) {
			foreach (XmlElement kid in node.ChildNodes) {
				switch (kid.Name) {
				case "Name":
					Name = kid.InnerText;
					break;
				case "Unit":
					Unit = kid.InnerText;
					break;
				case "Protein":
					Protein = kid.InnerText;
					break;
				case "Carbs":
					Carbs = kid.InnerText;
					break;
				case "Calories":
					Calories = kid.InnerText;
					break;
				case "Fat":
					Fat = kid.InnerText;
					break;
				case "SatFat":
					SatFat = kid.InnerText;
					break;
				default:
					break;
				}

			}
		}

//---------------------------------------------------------------------------------------

		public override string ToString() => Name;
	}
}
