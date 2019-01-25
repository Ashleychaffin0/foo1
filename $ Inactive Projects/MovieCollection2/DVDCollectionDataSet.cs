using System;
using System.Diagnostics;
using System.IO;
using MovieCollection2.Controls;

namespace MovieCollection2 {
	//Code behind for DVDCollectionDataset that defines user-owned utility functions for working with this dataset
	//and interactions with the custom DVD object returned by Amazon.com
	//Uses partial classes to compile one dataset/datatable type that combines designer code with user code.  
	//All user owned code for the dataset/datatable should be added to this file.  
	public partial class DVDCollectionDataSet {
		public partial class DVDsDataTable {

			/// <summary>
			/// Overrides default new row behavior to ensure that a unique ID is created for each record.  
			/// </summary>
			/// <remarks>Unique ID's are needed to properly look up rows and avoid conflicts with similar rows.  IDs will be GUIDs.  </remarks>
			internal void dvdsDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e) {
				DVDsRow newDVDRow = e.Row as DVDsRow;
				if (newDVDRow != null) {
					newDVDRow.ID = Guid.NewGuid().ToString();
					newDVDRow.Title = "[New row - select to enter details]";
				}
			}

			/// <summary>
			/// Overload that adds a DVD row to the datatable by passing in a custom DVD object.  
			/// </summary>
			/// <param name="myDVD">Custom DVD object that should be converted and added to the datatable</param>
			public void AddDVDsRow(DVD myDVD) {
				AddDVDsRow(this.DVDsRowFromDVDObject(myDVD));
			}

			/// <summary>
			/// Utility function to return a new DVDRow object converted from a custom DVD object.  
			/// </summary>
			/// <param name="myDvd">Custom DVD object to be converted to DVDRow.  </param>
			/// <returns>New DVDRow object initialized by the DVD object parameter.  </returns>
			/// <remarks>New DVDRow will be created in the context of this particular datatable instance.  </remarks>
			public DVDsRow DVDsRowFromDVDObject(DVD myDvd) {
				//objects used to store new row value 
				DVDsRow dvdRow;

				//create a new row in the context of this datatable: "Me"
				dvdRow = NewDVDsRow();

				//try to convert the DVD object to a DVDRow
				try {
					dvdRow.Title = myDvd.Title;
					dvdRow.Actors = myDvd.Actors;
					dvdRow.Director = myDvd.Directors;
					dvdRow.Rated = myDvd.Rating;
					dvdRow.YearReleased = myDvd.ReleasedDate;
					dvdRow.UPC = myDvd.UPC;
					dvdRow.ImageLink = myDvd.ImageUrl;
					dvdRow.WebPageLink = myDvd.WebPageUrl;
					dvdRow.Description = myDvd.Description;

					//initialize user-set fields; these are not converted from input
					dvdRow.MyRating = -1;
					dvdRow.Comments = "";
					dvdRow.Genre = "";

					//do more problematic conversions:
					//

					//convert length value from string to integer
					if (myDvd.RunningTime.Length > 0) {
						dvdRow.Length = Int32.Parse(myDvd.RunningTime);
					} else {
						dvdRow.Length = 0;
					}

					//convert image to Byte array to be saved in database
					if (myDvd.ImageCache != null) {
						using (MemoryStream memStream = new MemoryStream()) {
							myDvd.ImageCache.Save(memStream, myDvd.ImageCache.RawFormat);
							dvdRow.ImageBinary = memStream.GetBuffer();
						}
					}

				} catch (Exception ex) {
					Debug.WriteLine("An error occurred trying to convert DVD object to DVDTableRow. ");
					Debug.WriteLine(ex);
				}

				return dvdRow;
			}
		}
	}
}