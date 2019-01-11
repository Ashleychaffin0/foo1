using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// Note: Based on video at http://windowsclient.net/learn/video.aspx?v=142074

// Summary of above video

/*
 * Create a project
 * 
 * Create a WorkItem class (Add Class)
 * Within WorkItem, create properties (not fields) ID, Description, TimePlanned,
	and TimeActual 
 * Create a ctor for class WorkItem
 * 
 * Create a new class called WorkItemList of type List<WorkItem>, and a ctor that adds 
	sample data to the List<>.
		* Note: Below I just define a class variable called Items and add the sample data
				in the Form ctor. 
 * Go into Design mode on the form and from the Toolbox's Reporting section, drag a
	MicrosoftReportViewer control onto the form
 * Compile the Solution, so that there's now metadata about the WorkItem class for the
	Report Viewer to find.
 * In the upper right of the ReportViewer control, there's a little right-arrow. Click
	that, then click Design New Report
 * A Choose Data Source Type dialog comes up. Choose Object, then click Next.
 * From the tree that comes up, click on WorkItem, then click Next, then Finish.
 * The Data Source wizard then lets me see the possible fields for my report
	(Description, ID,etc). Click Next.
 * The next wizard dialog lets you choose Tablular or Matrix. Your call. Click Next
 * The next wizard dialog let you choose the fields and their column order, grouped into
	Page, Group, Details
 * Now Table Layout (Stepped, Block, (include Subtotals)). Click Next.
 * Choose Table Style. Click Next.
 * Give the Report a name and click Finish. For this program I've let it default to
	Report1.
 * Creates a .rdlc file (.Report Definition [lc, whatever that means]). You can edit this
	in the obvious ways.
 * Now switch back to the Form designer. The Report viewer control doesn't know what
	report to display. Click the right-arrow at the top right of the control and from the
	Choose Report combo box, select Report1.
 * Now at this point, he claims that Visual Studio automatically creates a BindingSource
	instance down in the whatever-it's-called area under the main Form designer window.
	However, in my case, it's a WorkItemBindingSource.
 * In Form_Load, it looks like the wizard has added Me.ReportViewer1.RefreshReport().
 * But just before that he's added Me.BindingSource.DataSource = new WorkItemList().
	As you can see from the code below, for me it's WorkItemBindingSource.DataSource =
	Items.
*/

namespace TestReport_March_11_2010 {
	public partial class Form1 : Form {

		public List<WorkItem> Items = new List<WorkItem>();

		public Form1() {
			InitializeComponent();

			Items.Add(new WorkItem(1, "Project 1", 30, 240));
			Items.Add(new WorkItem(2, "Project 2", 50, 40));
			Items.Add(new WorkItem(3, "Project 3", 60, 10));
			Items.Add(new WorkItem(4, "Project 4", 30, 20));
			Items.Add(new WorkItem(5, "Project 5", 20, 40));
		}

		private void Form1_Load(object sender, EventArgs e) {
			WorkItemBindingSource.DataSource = Items;
			this.reportViewer1.RefreshReport();
		}
	}
}
