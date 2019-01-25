using System.Windows.Forms;

namespace MovieCollection1 {
	/// <summary>
	/// Host form responsible for navigating between view and search online modes of the application.  
	/// </summary>
	/// <remarks>
	/// View MainForm.vb file in designer to modify additional properties.  
	/// </remarks>
	public partial class MainForm : Form {
		//User control objects responsible for viewing the collection and searching online
		ListDetails listDetailsPart;
		SearchOnline searchOnlinePart;

		public MainForm() {
			InitializeComponent();
		}

		/// <summary>
		/// Gets user control instance responsible for viewing the collection of movies.  
		/// </summary>
		/// <value>Live instance of the ListDetails user control.  </value>
		/// <remarks>Singleton instance pattern; the object is created upon first access, and the reused for all future accesses. </remarks>
		internal ListDetails ListDetailsPart {
			get {
				//Initialize the variable with a new object instance if nothing exists
				if (this.listDetailsPart == null)
				//creating object
                {
					this.listDetailsPart = new ListDetails();

					//site the control on the host user control and dock fill it
					this.targetPanel.Controls.Add(this.listDetailsPart);
					this.listDetailsPart.Dock = DockStyle.Fill;
				}

				return this.listDetailsPart;
			}
		}

		/// <summary>
		/// Gets user control property responsible for viewing the collection of movies.  
		/// </summary>
		/// <value>Live instance of the ListDetails user control.  </value>
		/// <remarks>Singleton instance pattern; the object is created upon first access, and the reused for all future accesses. </remarks>
		internal SearchOnline SearchOnlinePart {
			get {
				//Initialize the variable with a new object instance if nothing exists
				if (this.searchOnlinePart == null)
				//creating object
                {
					this.searchOnlinePart = new SearchOnline();
					this.searchOnlinePart.MainForm = this;

					//site the control on the host user control and dock fill it
					this.targetPanel.Controls.Add(this.searchOnlinePart);
					this.searchOnlinePart.Dock = DockStyle.Fill;
				}

				return this.searchOnlinePart;
			}
		}

		/// <summary>
		/// Shows the ListDetails user control on first load of the application. 
		/// </summary>
		private void MainForm_Load(System.Object sender, System.EventArgs e) {
			this.ShowListDetailsPart();
		}

		/// <summary>
		/// Restores window when the tray icon is clicked.
		/// </summary>
		private void NotifyIcon1_Click(object sender, System.EventArgs e) {
			this.RestoreWindow();
		}

		/// <summary>
		/// Restores window when the try icon is double clicked. 
		/// </summary>
		private void NotifyIcon1_DoubleClick(object sender, System.EventArgs e) {
			this.RestoreWindow();
		}

		/// <summary>
		/// Makes the ListDetails user control visible and hides the SearchOnline user control.
		/// </summary>
		private void ViewDetailsButton_Click(object sender, System.EventArgs e) {
			this.ShowListDetailsPart();
		}

		/// <summary>
		/// Makes the SearchOnline user control visible and hides the ListDetails user control.
		/// </summary>
		private void SearchOnlineButton_Click(object sender, System.EventArgs e) {
			this.ShowSearchOnlinePart();
		}

		/// <summary>
		/// Restores window to the normal size and ensures that it is visible.
		/// </summary>
		private void RestoreWindow() {
			this.WindowState = FormWindowState.Normal;
			this.Visible = true;
		}

		/// <summary>
		/// Displays the ListDetails user control and hides the SearchOnline user control.  
		/// </summary>
		/// <remarks>Enables the swapping behavior of list and search modes.  </remarks>
		internal void ShowListDetailsPart() {
			//performance optimization - skip hiding this control if it has not been created yet
			if (this.SearchOnlinePart != null) {
				this.SearchOnlinePart.Visible = false;
			}

			this.ListDetailsPart.Visible = true;
		}

		/// <summary>
		/// Displays the SearchOnline user control and hides the ListDetails user control.  
		/// </summary>
		/// <remarks>Enables the swapping behavior of list and search modes.  </remarks>
		internal void ShowSearchOnlinePart() {
			//performance optimization - skip hiding this control if it has not been created yet
			if (this.ListDetailsPart != null) {
				this.ListDetailsPart.Visible = false;
			}

			//Show this control and hide all others
			this.SearchOnlinePart.Visible = true;
		}
	}
}
