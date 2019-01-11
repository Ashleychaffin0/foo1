using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Reporting.WinForms;

using nsAlbumData;

namespace nsAlbumData {

	public partial class Form1 : Form {

		public List<MyReportData> aaa__data {get; set;}

//---------------------------------------------------------------------------------------

		public Form1() {
			aaa__data  = new List<MyReportData>();
			aaa__data.Add(new MyReportData("Let It Be", "Beatles", new List<string>() {"Let It Be 1", "Let It Be 2"}));
			aaa__data.Add(new MyReportData("Portfolio", "Steeleye Span", new List<string>() {"Thomas the Rhymer", "Gaudete", "All Around My Hat"}));

			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
			// Binding newVariable = this.reportViewer1.DataBindings.Add("foo1", aaa__data, "AlbumName");
			// ReportDataSource rds = new ReportDataSource("aaa__data", aaa__data);
			// reportViewer1.LocalReport.DataSources.Add(rds);
			DataTable dt = new DataTable();
			dt.Columns.Add(new DataColumn("dtsObject"));
			dt.Columns.Add(new DataColumn("timeEstimate"));
			dt.Columns.Add(new DataColumn("cost"));

			DataRow dr;
			
			dr = dt.NewRow();
			dr[0] = "FileSystemTask";
			dr[1] = "2 days";
			dr[2] = "$1000";
			dt.Rows.Add(dr);

			dr = dt.NewRow();
			dr[0] = "DynamicPropertiesTask";
			dr[1] = "4 days";
			dr[2] = "$2000";
			dt.Rows.Add(dr);

			dr = dt.NewRow();
			dr[0] = "SendMailTask";
			dr[1] = "5 days";
			dr[2] = "$4000";
			dt.Rows.Add(dr);

			// ** Create an instance of our ReportManager Class

			nsSampleRpt.ReportManager reportMan = new nsSampleRpt.ReportManager(dt);
			// ** Set the DataSource by calling the GetProfilerObjects method

			//// this.ProfilerObjectsBindingSource.DataSource = reportMan.GetProfilerObjects();

			// ** This method will render the report
			this.reportViewer1.RefreshReport();
			this.reportViewer1.RefreshReport();
		}

//---------------------------------------------------------------------------------------

		private void reportViewer1_Load(object sender, EventArgs e) {

		}

		private void Form1BindingSource_CurrentChanged(object sender, EventArgs e) {

		}
	}
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

namespace nsSampleRpt {

	public class ReportManager {

		private List<HisSampleData> m_HisSampleData = new List<HisSampleData>();

//---------------------------------------------------------------------------------------

		public ReportManager(DataTable dt) {

			foreach (DataRow dr in dt.Rows) {
				m_HisSampleData.Add(new HisSampleData(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));
			}
		}

//---------------------------------------------------------------------------------------

		public List<HisSampleData> GetProfilerObjects() {
			return m_HisSampleData;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class HisSampleData {

		public string DTSObjectName {get; set;}
		public string TimeEstimate {get; set;}
		public string Cost {get; set;}

//---------------------------------------------------------------------------------------

		public HisSampleData(string dtsObjectName, string timeEstimate, string cost) {
			this.DTSObjectName	= dtsObjectName;
			this.TimeEstimate	= timeEstimate;
			this.Cost			= cost;
		}
	}
}

#if false
namespace nsSampleRpt {

	public class ReportManager {

		private List<ProfilerObjects> m_profilerObjects = new List<ProfilerObjects>();
		public ReportManager(DataTable dt) {

			foreach (DataRow dr in dt.Rows) {

				m_profilerObjects.Add(new ProfilerObjects(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));
			}

		}

		public List<ProfilerObjects> GetProfilerObjects() {
			return m_profilerObjects;
		}
	}
}
#endif
