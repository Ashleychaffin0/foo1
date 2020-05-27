using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// See: https://anthonygiretti.com/2020/04/27/did-you-know-easy-and-custom-debugging-with-debuggerdisplay-and-debuggertypeproxy-attributes-on-visual-studio/

namespace DebuggerDisplayEtAl {
	class Program {
		static void Main(string[] args) {
			var me = new Identity() { FirstName = "Larry", LastName = "Smith" };
			Console.WriteLine(me);

			var identity = new Identity {
				FirstName = "Anthony",
				LastName  = "Giretti",
				Locations = new List<Location> {
					new Location {
						Country  = "Canada",
						Province = "Québec",
						City     = "Montréal"
					},
					new Location {
						Country  = "France",
						Province = "Ile-de-France",
						City     = "Paris"
					}
				}
			};

			Console.WriteLine(identity);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[DebuggerDisplay("FirstName: {FirstName} | LastName: {LastName} | Locations: {MyLocations}")]
	[DebuggerTypeProxy(typeof(LocationsDebugView))]
	public class Identity {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public List<Location> Locations { get; set; }

//---------------------------------------------------------------------------------------

		private string MyLocations {
			get {
				return $"{FirstName} {LastName} has lived in {Locations.Count} locations";
			}
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		private class LocationsDebugView {	// Local class
			private Identity identity;

//---------------------------------------------------------------------------------------

			public LocationsDebugView(Identity identity) {
				this.identity = identity;
			}

//---------------------------------------------------------------------------------------

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public List<string> Countries {
				get { return this.identity.Locations.Select(x => x.Country).ToList(); }
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[DebuggerDisplay("Country: {Country} | Location.Province: {Province} | City: {City}")]
	public class Location {
		public string Country { get; set; }
		public string Province { get; set; }
		public string City { get; set; }
	}
}
