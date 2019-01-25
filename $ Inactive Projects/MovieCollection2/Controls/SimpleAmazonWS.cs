using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using MovieCollection2.AmazonServiceSDK;
using MovieCollection2.Controls;

namespace MovieCollection2.Controls {

	/// <summary>
	/// Provides simple search functionality using Amazon.com Web Services v4.0
	/// </summary>
	/// <remarks>
	/// Wrapper over the AmazonServiceSDK. See http://www.amazon.com/aws/
	/// for more information about the service and SDK.  
	/// </remarks>
	public class SimpleAmazonWS {
		//Web service proxy object
		private AWSECommerceService amazonService = new MovieCollection2.AmazonServiceSDK.AWSECommerceService();

		//flag - if True virtually all HTML markup from the Web service will be filtered out
		private bool HTMLFilteringEnabled = true;

		/// <summary>
		/// Search for DVDs by Keywords
		/// </summary>
		/// <param name="searchString">dvd keyword to search for, e.g. name, actor, or UPC</param>
		/// <returns>BindingList of DVD search results</returns>
		/// <remarks>Several search criterial values can be altered in the Settings designer</remarks>
		public BindingList<DVD> SearchDVDs(string searchString) {
			//bindinglist of DVD objects to store and return results
			BindingList<DVD> searchResults;

			//objects needed to define search and search criteria
			ItemSearchRequest itemSearchRequest = new ItemSearchRequest();
			ItemSearch itemSearch = new ItemSearch();

			//initialize objects
			itemSearchRequest.Keywords = searchString;

			//set the size of the response, e.g. "Medium"
			itemSearchRequest.ResponseGroup = new string[] { "Medium" };

			//set the SearchIndex or search mode, e.g. "DVD"
			itemSearchRequest.SearchIndex = Properties.Settings.Default.AmazonSearchMode;

			itemSearch.SubscriptionId = Properties.Settings.Default.AmazonSubscriptionId;

			itemSearch.Request = new ItemSearchRequest[] { itemSearchRequest };

			//objects to store the response of the Web service
			ItemSearchResponse amazonResponse = null;
			Item[] amazonItems = null;

			//bind the Web service proxy to the appropriate service end-point URL for the current locale
			this.amazonService.Url = GetLocUrl();

			//call the Web service and assign the response
			amazonResponse = this.amazonService.ItemSearch(itemSearch);

			//access the array of returned items is the response is not Nothing
			if (amazonResponse != null) {
				amazonItems = amazonResponse.Items[0].Item;
			}

			//convert Amazon Items to generic collection of DVDs
			searchResults = GetListOfDVDs(amazonItems);

			return searchResults;
		}

		/// <summary>
		/// Looks up refreshed data about a particular DVD based on a UPC string argument.  
		/// </summary>
		/// <param name="upc">UPC code of the DVD to look up</param>
		/// <returns>First DVD object found with this matching ASIN or UPC, or nothing if none were found</returns>
		/// <remarks></remarks>
		public DVD RefreshDVDByUPC(string upc) {
			//object to store refreshed DVD result; initialized to empty DVD
			DVD refreshedDVD = new DVD();

			//objects that store all lookup results
			BindingList<DVD> foundDVDs;
			Item[] foundItems;

			//lookup item by ASIN or UPC
			foundItems = this.LookupItemByASIN(upc);

			//convert to BindingList of DVD objects
			foundDVDs = this.GetListOfDVDs(foundItems);

			//return the first item
			if (foundDVDs.Count > 0) {
				refreshedDVD = foundDVDs[0];
			}

			return refreshedDVD;
		}

		/// <summary>
		/// Looks up particular Amazon Item based on an ASIN or UPC string argument.  
		/// </summary>
		/// <param name="asinString">ASIN code of the DVD to look up</param>
		/// <returns>First DVD object found with this matching ASIN or UPC, or nothing if none were found</returns>
		/// <remarks>Used by RefreshDVDByUPC method.  </remarks>
		private Item[] LookupItemByASIN(string asinString) {
			//object that stores Items from lookup
			Item[] amazonItems = null;

			ItemLookup itemLookup = new ItemLookup();
			ItemLookupRequest request = new ItemLookupRequest();

			request.ResponseGroup = new string[] { "Medium" };
			request.ItemId = new string[] { asinString };
			request.IdType = ItemLookupRequestIdType.ASIN;
			request.IdTypeSpecified = true;

			itemLookup.SubscriptionId = Properties.Settings.Default.AmazonSubscriptionId;

			//set request 
			itemLookup.Request = new ItemLookupRequest[] { request };

			//object to store the response of the Web service
			ItemLookupResponse amazonResponse = null;

			//bind the Web service proxy to the appropriate service end-point URL for the current locale
			this.amazonService.Url = GetLocUrl();

			//call the Web service and assign the response
			amazonResponse = this.amazonService.ItemLookup(itemLookup);

			//access the array of returned items is the response is not Nothing
			if (amazonResponse != null) {
				if ((amazonResponse.Items != null) && (amazonResponse.Items.Length > 0)) {
					amazonItems = amazonResponse.Items[0].Item;
				}
			}

			return amazonItems;
		}

		/// <summary>
		/// Utility function that converts the Amazon running time field to a string
		/// </summary>
		private string AmazonItemRunningTimeToString(Item amazonItem) {
			string runningTime = "";
			{
				{
					ItemAttributes itemAttributes = amazonItem.ItemAttributes;
					if ((itemAttributes != null) && (itemAttributes.RunningTime != null)) {
						runningTime = itemAttributes.RunningTime.Value;
					}

				}
			}
			return runningTime;
		}

		/// <summary>
		/// Utility function that converts the Amazon editorial review field to a string
		/// </summary>
		/// <remarks>Used to provide a description for an item</remarks>
		private string AmazonItemDescriptionToString(Item amazonItem) {
			string description = "";
			EditorialReview[] editorialReviews = amazonItem.EditorialReviews;
			if ((editorialReviews != null) && (editorialReviews.Length > 0)) {
				if (HTMLFilteringEnabled == true) {
					description = this.FilterHTMLText(editorialReviews[0].Content);
				} else {
					description = editorialReviews[0].Content;
				}
			}
			return description;
		}

		/// <summary>
		/// Utility function that converts the Amazon ImageURL to a URL string
		/// </summary>
		/// <remarks>Used to load the item image</remarks>
		private string AmazonItemLargeImageURLToString(Item amazonItem) {
			string largeImageUrl = "";
			{
				if (amazonItem.LargeImage != null) {
					largeImageUrl = amazonItem.LargeImage.URL;
				}
			}
			return largeImageUrl;
		}

		/// <summary>
		/// Utility function that returns ASIN string from an Item
		/// </summary>
		/// <remarks>Used to look up DVD objects by ASIN or UPC</remarks>
		private string AmazonASINToString(Item amazonItem) {
			string asinString = "";

			if ((amazonItem != null) && (amazonItem.ASIN.Length > 0)) {
				asinString = amazonItem.ASIN.ToString();
			}

			return asinString;
		}

		/// <summary>
		/// Utility function that converts Amazon Item array objects to a bindinglist of DVD objects
		/// </summary>
		/// <returns>BindingList of DVD objects, or an empty list if the array is nothing</returns>
		private BindingList<DVD> GetListOfDVDs(Item[] amazonItems) {
			BindingList<DVD> dvds = new BindingList<DVD>();
			DVD dvd;

			if (amazonItems != null) {
				foreach (Item amazonItem in amazonItems) {

					{
						dvd = new DVD(amazonItem.ItemAttributes.Title, this.AmazonASINToString(amazonItem), this.AmazonItemDescriptionToString(amazonItem), amazonItem.ItemAttributes.Actor, amazonItem.ItemAttributes.Director, amazonItem.ItemAttributes.AudienceRating, this.AmazonItemRunningTimeToString(amazonItem), this.AmazonDateFormatToString(amazonItem.ItemAttributes.TheatricalReleaseDate), amazonItem.DetailPageURL, this.AmazonItemLargeImageURLToString(amazonItem));


					}
					dvds.Add(dvd);
				}
			}
			return dvds;
		}

		/// <summary>
		/// Utility function that looks up the appropriate localized service end-point 
		/// URL based on AmazonLocale setting
		/// </summary>
		/// <returns></returns>
		/// <remarks>Used by SimpleAmazonWS to change the locale of the data results.  
		/// Supports AmazonLocale setting values {"EN", "UK", "DE", "JP", "FR", "CA", "US"}
		/// </remarks>
		private string GetLocUrl() {
			string locUrl = "";
			string localeString = Properties.Settings.Default.AmazonLocale.ToUpper();

			switch (localeString) {
			case "EN":
				locUrl = Properties.Settings.Default.AmazonUrlEN;
				break;
			case "UK":
				locUrl = Properties.Settings.Default.AmazonUrlUK;
				break;
			case "DE":
				locUrl = Properties.Settings.Default.AmazonUrlDE;
				break;
			case "JP":
				locUrl = Properties.Settings.Default.AmazonUrlJP;
				break;
			case "FR":
				locUrl = Properties.Settings.Default.AmazonUrlFR;
				break;
			case "CA":
				locUrl = Properties.Settings.Default.AmazonUrlCA;
				break;
			case "US":
				locUrl = Properties.Settings.Default.AmazonUrlEN;
				break;
			default:
				locUrl = Properties.Settings.Default.AmazonUrlEN;
				break;
			}

			return locUrl;
		}

		/// <summary>
		/// Utility function to return Amazon's custom date/timestamp formats into a .NET Date type
		/// </summary>
		/// <param name="amazonDate">Date string returned by Amazon.com's ProductInfo type</param>
		/// <returns>Date type converted from custom string, or empty string if parameter is nothing</returns>
		/// <remarks></remarks>
		private string AmazonDateFormatToString(string amazonDate) {
			// Format to handle Amazon's convention for date strings; 0 means not set
			string[] expectedFormats = { "d", "yyyy", "yyyy-mm", "yyyymmdd", "yyyy0000", "yyyymm00", "yyyy00dd", "yyyy-mm-dd" };
			System.DateTime myDate;
			if (amazonDate != null) {
				myDate = System.DateTime.ParseExact(amazonDate, expectedFormats, System.Windows.Forms.Application.CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
			} else {
				return "";
			}

			return myDate.ToString("yyyy");
		}

		/// <summary>
		/// Utility function that strips HTML tags and whitespace from the input string.
		/// </summary>
		/// <param name="htmlText"></param>
		/// <remarks>Used by GetListOfDVDs method. </remarks>
		private string FilterHTMLText(string htmlText) {
			string filteredText = "";

			if (htmlText != null) {
				filteredText = htmlText;

				//remove HTML tags using a regular expression
				filteredText = Regex.Replace(filteredText, "(<[^>]+>)", "");

				//remove whitespace characters using a regular expression
				filteredText = Regex.Replace(filteredText, "&(nbsp|#160);", "");

				//optional: format out additional characters 
				//...
			}

			return filteredText;
		}
	}
}


