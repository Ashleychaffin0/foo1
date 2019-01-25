using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace MovieCollection2.Controls {
	/// <summary>
	/// Represents DVD title.  
	/// </summary>
	/// <remarks>SimpleAmazon Web service class returns a generic list of these DVD classes.  </remarks>
	public class DVD {

		private string m_UPC = "";
		private string m_Title = "";
		private string m_Description = "";
		private string[] m_Actors = new string[] { };
		private string[] m_Directors = new string[] { };
		private string m_Rating = "";
		private string m_RunningTime = "";
		private string m_ReleasedDate = "";
		private string m_WebPageUrl = "";
		private string m_ImageUrl = "";
		public Bitmap ImageCache = null;

		public DVD() {

		}

		public DVD(string title, string upc, string description) {
			m_UPC = CheckString(upc);
			m_Title = CheckString(title);
			m_Description = CheckString(description);
		}

		public DVD(string title, string upc, string description, string[] actors, string[] directors, string rating, string runningtime, string releasedDate, string webPageUrl, string imageUrl) {
			m_UPC = CheckString(upc);
			m_Title = CheckString(title);
			m_Description = CheckString(description);
			m_Actors = actors;
			m_Directors = directors;
			m_Rating = CheckString(rating);
			m_RunningTime = CheckString(runningtime);
			m_ReleasedDate = CheckString(releasedDate);
			m_WebPageUrl = CheckString(webPageUrl);
			m_ImageUrl = CheckString(imageUrl);
		}

		public DVD(string title, string upc, string description, string actors, string directors, string rating, string runningtime, string releasedDate, string webPageUrl, string imageUrl) {
			m_UPC = CheckString(upc);
			m_Title = CheckString(title);
			m_Description = CheckString(description);
			m_Actors[0] = actors;
			m_Directors[0] = directors;
			m_Rating = CheckString(rating);
			m_RunningTime = CheckString(runningtime);
			m_ReleasedDate = CheckString(releasedDate);
			m_WebPageUrl = CheckString(webPageUrl);
			m_ImageUrl = CheckString(imageUrl);
		}

		public string UPC {
			get {
				return this.m_UPC;
			}
		}

		public string Title {
			get {
				return m_Title;
			}
		}

		public string Description {
			get {
				return m_Description;
			}
		}

		public string Actors {
			get {
				string singleString = "";
				try {
					if (this.m_Actors != null) {
						foreach (string part in this.m_Actors) {
							if (singleString == "") {
								singleString = part;
							} else {
								if (part != null) { singleString = singleString + ";" + part; }
							}
						}
					}
				} catch (Exception ex) {
					Debug.WriteLine(ex);
				}

				return singleString;
			}
		}

		public string Directors {
			get {
				string singleString = "";
				try {
					if (this.m_Directors != null) {
						foreach (string part in this.m_Directors) {
							if (singleString == "") {
								singleString = part;
							} else {
								if (part != null) { singleString = singleString + ";" + part; }
							}
						}
					}

				} catch (Exception ex) {
					Debug.WriteLine(ex);
				}

				return singleString;
			}
		}

		public string ImageUrl {
			get {
				return this.m_ImageUrl;
			}
			set {
				m_ImageUrl = value;
			}
		}

		public string Rating {
			get {
				return this.m_Rating;
			}
		}

		public string ReleasedDate {
			get {
				return this.m_ReleasedDate;
			}

		}


		public string RunningTime {
			get {
				return this.m_RunningTime;
			}
		}


		public string WebPageUrl {
			get {
				return this.m_WebPageUrl;
			}
		}

		public override string ToString() {
			return String.Format("{0} {1}", this.m_Title, this.m_ReleasedDate);
		}

		/// <summary>
		/// Utility Function that converts null strings to the empty string.
		/// </summary>
		/// <param name="OriginalValue">The string to check against Nothing.</param>
		/// <returns>If the original value is nothing, returns the empty string.  Else, returns the original value.</returns>
		/// <remarks></remarks>
		private static string CheckString(string OriginalValue) {
			if (OriginalValue == null) {
				return "";
			} else {
				return OriginalValue;
			}
		}
	}
}



