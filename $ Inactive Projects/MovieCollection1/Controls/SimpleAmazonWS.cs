using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using MovieCollection1.Controls;

namespace MovieCollection1 {
	/// <summary>
	/// Simplified class responsible for making requests to Amazon.com Web service and returning results.  
	/// </summary>
	/// <remarks>
	/// ***Online Web service features are not implemented in this version of the starter kit.  Please see the Getting Started documentation to learn how to download an updated version.  
	/// </remarks>
	public class SimpleAmazonWS {

		/// <summary>
		/// Configures defaults for the search request
		/// </summary>
		/// <remarks>Some default values can be changed in the Settings designer without changing code.  </remarks>
		public SimpleAmazonWS() {

		}


		/// <summary>
		/// Simple search method that returns a list of DVD objects based on the search string argument. 
		/// </summary>
		/// <param name="searchString">dvd keyword to search for, e.g. name, actor, or UPC</param>
		/// <returns>BindingList of DVD search results</returns>
		/// <remarks></remarks>
		public BindingList<DVD> SearchDVDs(string searchString) {
			throw new NotImplementedException("Online Web service features are not implemented in this version of the starter kit.  Please see the Getting Started documentation to learn how to download an updated version.  ");
		}

		/// <summary>
		/// Looks up refreshed data about a particular DVD based on a UPC string argument.  
		/// </summary>
		/// <param name="upc">UPC code of the DVD to look up</param>
		/// <returns>First DVD object found with this matching UPC, or nothing if none were found</returns>
		/// <remarks></remarks>
		public DVD RefreshDVDByUPC(string upc) {
			throw new NotImplementedException("Online Web service features are not implemented in this version of the starter kit.  Please see the Getting Started documentation to learn how to download an updated version.  ");
		}

		/// <summary>
		/// Utility function to return Amazon's custom date formats into a .NET Date type
		/// </summary>
		/// <param name="amazonDate">Date string returned by Amazon.com's ProductInfo type</param>
		/// <returns>Date type converted from custom string, or Date.MinValue if parameter is nothing</returns>
		/// <remarks></remarks>
		private string AmazonDateFormatToDateString(string amazonDate) {
			// Format to handle Amazon's convention for date strings; 0 means not set
			string[] expectedFormats = { "d", "yyyymmdd", "yyyy0000", "yyyymm00", "yyyy00dd" };

			if (amazonDate != null) {
				DateTime date = DateTime.ParseExact(amazonDate, expectedFormats, Application.CurrentCulture, DateTimeStyles.AllowWhiteSpaces);
				return date.ToString("yyyy");
			}

			return null;
		}
	}
}