using System;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MovieCollection1.Controls;

namespace MovieCollection1 {
	/// <summary>
	/// User control responsible viewing, managing, and live editing the details of the movie collection.  
	/// </summary>
	/// <remarks>
	/// Movies are added programatically by the SearchOnline control or are added manually after pressing the 
	/// Add button.  Movies can be live edited by selecting a value in the grid.  They can be deleted by selecting and pressing 
	/// Delete button.  Press the View Online button to visit the Amazon.com Web site.  Press Refresh button 
	/// to refresh data fields with live information from the Amazon.com Web service.  
	/// Movies are read from and written to MovieDatabase.mdf.  It is recommended that movie information is 
	/// refreshed regularly.  
	/// </remarks>
	public partial class ListDetails : UserControl {

		System.Windows.Forms.Form hostingForm;

		//Control object responsible for rating movie titles.  
		//Workaround: project cannot dynamically build this custom control type
		//                      after using New Project wizard; the control instance is created and sited at runtime instead of 
		//                      adding to design surface.  
		internal RatingPickerControl ratingPickerControl;

		//Gets turned on, while database updates are occuring.
		bool isUpdating;

		/// <summary>
		/// Initializes this control and loads data.    
		/// </summary>
		/// <remarks>
		/// All movie titles in the collection are loaded into the in-memory datatable;  
		/// The control monitors for changes to the datatable and auto saves changes made to each field. 
		/// </remarks>
		private void ListDetails_Load(System.Object sender, System.EventArgs e) {
			//dynamically create rating picker control and site on the form
			InitRatingPickerControl();

			//listen for data changes when adding, modifying, and deleting in UI
			this.dvdCollectionDataSet.DVDs.TableNewRow += new DataTableNewRowEventHandler(this.dvdCollectionDataSet.DVDs.dvdsDataTable_TableNewRow);
			this.dvdCollectionDataSet.DVDs.RowChanged += new DataRowChangeEventHandler(dt_RowChanged);

			//load data from database
			if (this.LoadData() == 0) {
				//if no rows were available, create a new row so users can start entering a new dvd
				this.dvdsDataConnector.AddNew();
			}

			this.titleTextBox.Select();
		}

		/// <summary>
		/// Detects any changes made to the datatable and saves changes to the database.
		/// </summary>
		private void dt_RowChanged(object sender, System.Data.DataRowChangeEventArgs e) {
			if (isUpdating) {
				return;
			}

			MovieCollection1.DVDCollectionDataSet.DVDsRow dvdDataRow = (MovieCollection1.DVDCollectionDataSet.DVDsRow)e.Row;
			if (dvdDataRow != null) {
				SaveData();
			}
		}

		/// <summary>
		/// Creates a new movie in the collection when Add button is clicked.  
		/// </summary>
		/// <remarks>It is preferable to call AddNew from the dataconnector to keep data binding between the control and datatable in sync.  </remarks>
		private void AddTitleButton_Click(System.Object sender, System.EventArgs e) {
			this.dvdsDataConnector.AddNew();
		}

		/// <summary>
		/// Deletes the currently selected movie.
		/// </summary>
		private void RemoveButton_Click(System.Object sender, System.EventArgs e) {
			DeleteCurrent();
		}

		/// <summary>
		/// Applies the user-specified filter when clicked.
		/// </summary>
		private void FilterButton_Clicked(System.Object sender, System.EventArgs e) {
			ApplyTextboxFilter();
		}

		/// <summary>
		/// Applies the user-specified filter when filter string is entered.
		/// </summary>
		private void FilterTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				ApplyTextboxFilter();
			}
		}

		/// <summary>
		/// Resets DataConnector to be unfiltered when button is clicked.
		/// </summary>
		private void ShowAllButton_Click(System.Object sender, System.EventArgs e) {
			this.dvdsDataConnector.Filter = "";
		}

		/// <summary>
		/// Refreshes the current movie selection with data from the Amazon.com Web service after prompting the user.  
		/// </summary>
		/// <remarks>
		/// Uses UPC value to fetch the movie title from the Web service, and merges/overwrites the 
		/// existing data.  Comments and MyRating fields are preserved as a special case.  
		/// </remarks>
		private void RefreshOnlineButton_Click(System.Object sender, System.EventArgs e) {
			//prompt the user to refresh and store the result
			DialogResult dialogResult = MessageBox.Show("This feature will potentially overwrite your changes.  Do you want to proceed? ", "Refresh data from online Web service?", MessageBoxButtons.YesNo);

			//if yes, then begin refresh process
			if (dialogResult == DialogResult.Yes) {
				//object used to call Amazon.com Web service
				SimpleAmazonWS amazonService = new SimpleAmazonWS();

				//objects used to store search result
				DVD refreshedDVD;
				MovieCollection1.DVDCollectionDataSet.DVDsRow refreshedDVDRow;

				try {
					//boundary check: make sure movie datatable is not empty
					if (this.dvdsDataConnector.Count > 0) {
						//gets the current movie selection and converts to a typed movie row object
						DataRowView rowView = (DataRowView)this.dvdsDataConnector.Current;
						MovieCollection1.DVDCollectionDataSet.DVDsRow dvdRow = (MovieCollection1.DVDCollectionDataSet.DVDsRow)rowView.Row;

						// check to see if UPC value is null
						if ((!dvdRow.IsUPCNull()) && dvdRow.UPC != "") {
							//variables to store and preserve custom comment and rating values
							string currentComments = dvdRow.Comments;
							int currentRating = dvdRow.MyRating;

							//get updated DVD object from Web service
							refreshedDVD = amazonService.RefreshDVDByUPC(dvdRow.UPC);

							//update cached image
							this.boxArtPictureBox.Load(refreshedDVD.ImageUrl);
							refreshedDVD.ImageCache = (Bitmap)this.boxArtPictureBox.Image;

							//convert DVD to dvdRow
							refreshedDVDRow = this.dvdCollectionDataSet.DVDs.DVDsRowFromDVDObject(refreshedDVD);

							//set the same ID as original row so row look up in the in-memory datatable succeeds
							refreshedDVDRow.ID = dvdRow.ID;

							//restore/merge back the custom comment and rating values
							refreshedDVDRow.Comments = currentComments;
							refreshedDVDRow.MyRating = currentRating;

							//merge dvdRow with matching row
							this.dvdCollectionDataSet.DVDs.LoadDataRow(refreshedDVDRow.ItemArray, LoadOption.Upsert);
						}
					}

				} catch (Exception ex) {
					MessageBox.Show(String.Format("There was a problem refreshing the data from the online Web service: {0}", ex.Message));
					Debug.WriteLine(ex.ToString());
				}
			}

		}

		/// <summary>
		/// Navigates the to Amazon.com Web page for the current movie selection in an external browser after prompting the user.  
		/// </summary>
		/// <remarks>The user should see the hyperlink before starting the browser process and navigating to the URL.  </remarks>
		private void ViewOnlineButton_Click(System.Object sender, System.EventArgs e) {
			//boundary check: make sure movie datatable is not empty
			if (this.dvdsDataConnector.Count > 0) {
				//variables to store and preserve custom comment and rating values
				DataRowView rowView = (DataRowView)this.dvdsDataConnector.Current;
				DVDCollectionDataSet.DVDsRow dvdRow = (MovieCollection1.DVDCollectionDataSet.DVDsRow)rowView.Row;

				//variable to store the Web page URL value
				string webLink = "";

				//check for null before accessing WebPageLink property
				if (!dvdRow.IsWebPageLinkNull()) { webLink = dvdRow.WebPageLink; }

				//prompts to see if end user trusts URL and stores the prompt result value
				string msgText = String.Format("About to view this URL in an external browser: '{0}'. Is it safe to proceed?", webLink);
				DialogResult yesNoResult = MessageBox.Show(msgText, "OK to browse?", MessageBoxButtons.YesNo);

				//start the URL path if the user approves the prompt
				if ((yesNoResult == DialogResult.Yes) && (webLink != "")) {
					Process.Start(webLink);
				}
			}
		}

		/// <summary>
		/// Prompts user for file to import when Clicked.  
		/// </summary>
		private void ImportButton_Click(object sender, System.EventArgs e) {
			this.openFileDialog1.ShowDialog();
		}

		/// <summary>
		/// Imports image provided by user when user clicks OK to open file prompt.   
		/// </summary>
		private void OpenFileDialog1_FileOk(System.Object sender, System.ComponentModel.CancelEventArgs e) {
			this.ImportImage(this.openFileDialog1.FileName);
		}

		/// <summary>
		/// Loads movie collection data.  
		/// </summary>
		/// <returns>Positive integer for number of rows loaded; 0 if no rows are loaded.  </returns>
		/// <remarks>Loads movie collection data into the in-memory datatable</remarks>
		internal int LoadData() {
			int rowsLoaded = 0;

			//try to fill the datatable with the query of all movies
			//note: if the database contains a large amount of movies you'll likely want to add a query to the
			//TableAdapter and only fill a subset of the movies in the database into memory.
			try {
				isUpdating = true;
				rowsLoaded = this.dvdsTableAdapter.Fill(this.dvdCollectionDataSet.DVDs);
			} catch (Exception ex) {
				MessageBox.Show(String.Format("There was a problem loading data: {0}", ex.Message));
				Debug.WriteLine(ex.ToString());
			} finally {
				isUpdating = false;
			}

			return rowsLoaded;
		}

		/// <summary>
		/// Saves changes made to movie collection data.  
		/// </summary>
		/// <returns>Positive integer for number of rows updated in database; 0 if no rows are updated.  </returns>
		/// <remarks>Saves the changes from in-memory datatable and bound UI.  </remarks>
		internal int SaveData() {
			if (isUpdating) {
				return 0;
			}

			int rowsSaved = 0;

			//try to get changes and save to database
			try {
				//only push update to database if changes are detected
				if (dvdCollectionDataSet.HasChanges()) {
					isUpdating = true;
					rowsSaved = this.dvdsTableAdapter.Update(this.dvdCollectionDataSet.DVDs);
				}
			} catch {
			} finally {
				isUpdating = false;
			}
			return rowsSaved;
		}

		/// <summary>
		/// Applies filter over the in-memory movie records using the user-specified input in the text box.  
		/// </summary>
		/// <remarks>DataConnector object's Filter property is applied to all controls bound to the DataConnector. </remarks>
		private void ApplyTextboxFilter() {
			try {
				//sets filter string WHERE clause in the form "Where COLUMNNAME like '@FilterText'"
				this.dvdsDataConnector.Filter = String.Format("{0} like '%{1}%'", this.dvdCollectionDataSet.DVDs.TitleColumn.ColumnName, this.filterTextBox.Text);
			} catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
			}

		}

		/// <summary>
		/// Deletes the currently selected movie after prompting the user.
		/// </summary>
		internal void DeleteCurrent() {
			//check for boundaries before performing delete: datatable is empty, or there is no selection
			if ((this.dvdsDataConnector.Count > 0) && (this.dvdsDataConnector.Current != null))
			//convert generic Current object returned by DataConnector to the typed movie row object
            {
				DataRowView rowView = (DataRowView)this.dvdsDataConnector.Current;
				MovieCollection1.DVDCollectionDataSet.DVDsRow dvdRow = (MovieCollection1.DVDCollectionDataSet.DVDsRow)rowView.Row;

				//string representing the friendly name of the current movie to be deleted
				string title = "[No Title]";

				//use movie title property in the typed row object if it contains valid data
				if ((!dvdRow.IsTitleNull()) && (dvdRow.Title != null)) {
					title = dvdRow.Title;
				}

				//text displayed in a prompt to the user before deleting
				string msgText = String.Format("Are you sure you want to permanently delete {0} from your collection? ", title);

				//display Yes/No prompt to the user and store the result
				DialogResult dialogResult = MessageBox.Show(msgText, "OK to delete?", MessageBoxButtons.YesNo);

				//remove the current movie if the user accepts the prompt
				if (dialogResult == DialogResult.Yes) {
					this.dvdsDataConnector.RemoveCurrent();
				}

				//make sure we always have a row available
				if (dvdsDataConnector.Count == 0) {
					this.dvdsDataConnector.AddNew();
				}
			}
		}

		/// <summary>
		/// Initializes the rating picker control used on this user control.  
		/// </summary>
		/// <remarks>
		/// Called by ListDetails.Load event.  This special initialization code is not required if the 
		/// RatingPickerControl is built and referenced in a separate control library.  
		/// </remarks>
		private void InitRatingPickerControl() {
			//create rating picker control object and site it on the parent user control
			this.ratingPickerControl = new RatingPickerControl();
			this.dvdDetailsPanel.Controls.Add(this.ratingPickerControl);

			//set default properties and data binding
			((System.ComponentModel.ISupportInitialize)this.ratingPickerControl).BeginInit();
			this.ratingPickerControl.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.ratingPickerControl.BackColor = System.Drawing.Color.Transparent;
			this.ratingPickerControl.CurrentRating = -1;
			this.ratingPickerControl.DataBindings.Add("CurrentRating", this.dvdsDataConnector, "MyRating", true, DataSourceUpdateMode.OnPropertyChanged, null);
			this.ratingPickerControl.Location = new System.Drawing.Point(128, 388);
			this.ratingPickerControl.Name = "RatingPickerControl1";
			this.ratingPickerControl.Size = new System.Drawing.Size(80, 16);
			this.ratingPickerControl.TabIndex = 7;
			((System.ComponentModel.ISupportInitialize)this.ratingPickerControl).EndInit();
		}

		/// <summary>
		/// Imports image value into datatable from file provided by user.  
		/// </summary>
		/// <remarks></remarks>
		private void ImportImage(string filename) {
			try {
				//check for boundaries before performing delete: datatable is empty, or there is no selection
				if (this.dvdsDataConnector.Current != null) {
					//convert generic Current object returned by DataConnector to the typed movie row object
					DataRowView rowView = (DataRowView)this.dvdsDataConnector.Current;
					MovieCollection1.DVDCollectionDataSet.DVDsRow dvdRow = (MovieCollection1.DVDCollectionDataSet.DVDsRow)rowView.Row;

					//open file as Readonly from file system, copy bytes, and assign to the image property of the current row
					dvdRow.ImageBinary = File.ReadAllBytes(filename);

					this.dvdsDataConnector.ResetCurrentItem();
				}
			} catch (Exception ex) {
				MessageBox.Show(String.Format("There was a problem loading the image: {0}", ex.Message));
				Debug.WriteLine(ex.ToString());
			}
		}

		private void HostingForm_FormClosed(System.Object sender, System.Windows.Forms.FormClosedEventArgs e) {
			//call EndEdit to capture all changes made in the UI
			this.dvdsDataConnector.EndEdit();

			//when the parent form is closing, be sure to save the data on this control
			SaveData();
		}

		private void ListDetails_ParentChanged(System.Object sender, System.EventArgs e) {
			////if the parent form for this user control ever changes we need to reset the Form_Closed event handler for the new parent form
			if (hostingForm != null) {
				this.hostingForm.FormClosed -= new System.Windows.Forms.FormClosedEventHandler(HostingForm_FormClosed);
			}
			this.hostingForm = (System.Windows.Forms.Form)this.ParentForm;
			this.hostingForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(HostingForm_FormClosed);
		}

		private void TitleTextBox_Click(System.Object sender, System.EventArgs e) {
			if (this.titleTextBox.Text == "[New row - select to enter details]") {
				this.titleTextBox.SelectAll();
			}
		}
	}
}


