using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MovieCollection2.Controls;

namespace MovieCollection2 {
	/// <summary>
	/// User control responsible for searching Amazon.com Web service for movie titles, and adding titles to the collection. 
	/// </summary>
	/// <remarks>
	/// Click on the search button or enter search text to perform an online search.  
	/// Select a valid movie title and Add To Collection to save the data locally.  
	/// The simplified Amazon.com search class returns a generic BindingList of DVD objects.  The user control UI elements are 
	/// data bound to the list of DVDs using the Data Source Object binding feature.  Adding the title to collection converts
	/// the DVD object to the in-memory datatable format, which the rest of the application uses.  
	/// </remarks>
	public partial class SearchOnline : UserControl {
		private MainForm mainForm;
		public MainForm MainForm { get { return mainForm; } set { mainForm = value; } }

		public SearchOnline() {
			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Calls Amazon.com Web service and binds dvd bindinglist result to the UI.  
		/// </summary>
		private void PerformSearch() {
			//object responsible for containing dvd search results
			BindingList<DVD> searchResults = new BindingList<DVD>();

			//simple wrapper object responsible for handling requests and responses from the Amazon.com Web service
			SimpleAmazonWS amazonService = new SimpleAmazonWS();

			//show hour glass during the search to tell users that work is being done
			this.Cursor = Cursors.WaitCursor;

			try {
				//request search results from the Web service passing in the user's search criteria
				searchResults = amazonService.SearchDVDs(this.searchTextBox.Text);

				//data bind the search results to the form UI
				this.dvdDataConnector.DataSource = searchResults;
			} catch (Exception ex) {
				MessageBox.Show(String.Format("There was a problem connecting to the Web service.  Please verify that you are connected to the Internet.  Additional details: {0}", ex.Message));
				Debug.WriteLine(ex);
			} finally {
				//set cursor back to the default now that work is done
				this.Cursor = Cursors.Default;
			}

			//tell the user how many results were found.  Use String.Format feature to concat strings in a Localization-friendly way
			this.label2.Text = String.Format("{0} results found.  ", searchResults.Count.ToString());
		}

		/// <summary>
		/// Adds the current movie selection to the local movie collection, and displays the ListDetails user control.
		/// </summary>
		private void AddCurrentToCollection() {
			//try to add data to the collection
			try {
				//check boundary cases where DVD list is empty or there is no current selection
				if ((this.dvdDataConnector.Count > 0) && (this.dvdDataConnector.Current != null)) {
					//object that stores the current DVD selection
					DVD currentDVD = (DVD)this.dvdDataConnector.Current;

					//try to cache the image to the dvd object
					if (this.pictureBox1.Image != null) {
						//try loading from picture box first
						currentDVD.ImageCache = (Bitmap)this.pictureBox1.Image;
					} else if (!String.IsNullOrEmpty(currentDVD.ImageUrl)) {
						//else try from the source URL
						this.pictureBox1.Load(currentDVD.ImageUrl);
						currentDVD.ImageCache = (Bitmap)this.pictureBox1.Image;
					}

					//convert custom DVD object to the datable / datarow format used by the application, and
					//add the datarow to the datatable
					mainForm.ListDetailsPart.dvdCollectionDataSet.DVDs.AddDVDsRow(currentDVD);

					//update display to show ListDetailsPart with new row selected
					mainForm.ListDetailsPart.dvdsDataConnector.Position = mainForm.ListDetailsPart.dvdsDataConnector.Count - 1;
					mainForm.ShowListDetailsPart();
				} else {
					MessageBox.Show("No DVD is selected.  Please select a DVD before adding to collection.  ");
				}

			} catch (Exception ex) {
				MessageBox.Show("There was a problem adding this DVD to the collection.  ");
				Debug.WriteLine(ex);
			}
		}

		/// <summary>
		/// Performs online search when button is clicked.
		/// </summary>
		private void SearchButton_Click(object sender, System.EventArgs e) {
			PerformSearch();
		}

		/// <summary>
		/// Performs online search when enter is pressed in the search textbox.
		/// </summary>
		private void SearchTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				PerformSearch();
			}
		}

		/// <summary>
		/// Adds the current selection to the local collection when the button is pressed.
		/// </summary>
		private void AddToCollectionButton_Click(System.Object sender, System.EventArgs e) {
			AddCurrentToCollection();
		}

		/// <summary>
		/// Adds the current selection to the local collection when the datagridview is double clicked.
		/// </summary>
		private void dvdDataGridView_DoubleClick(System.Object sender, System.EventArgs e) {
			if (this.dvdDataConnector.Current != null) {
				AddCurrentToCollection();
			}
		}
	}
}
