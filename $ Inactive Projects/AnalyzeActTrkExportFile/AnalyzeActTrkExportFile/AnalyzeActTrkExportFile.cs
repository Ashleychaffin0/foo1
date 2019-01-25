using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace AnalyzeActTrkExportFile	{
	/// <summary>
	/// Summary description for AnalyzeActTrkExportFile.
	/// </summary>
	public class AnalyzeActTrkExportFile : System.Windows.Forms.Form {
		#region Designer stuff
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label lblServicesChars;
		private System.Windows.Forms.Label lblServicesPct;
		private System.Windows.Forms.Label lblServicesLines;
		private System.Windows.Forms.Label lblDemographicsChars;
		private System.Windows.Forms.Label lblDemographicsPct;
		private System.Windows.Forms.Label lblDemographicsLines;
		private System.Windows.Forms.Label lblSurveysChars;
		private System.Windows.Forms.Label lblSurveysPct;
		private System.Windows.Forms.Label lblSurveysLines;
		private System.Windows.Forms.Label lblReferralsChars;
		private System.Windows.Forms.Label lblReferralsPct;
		private System.Windows.Forms.Label lblReferralsLines;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label lblFileSize;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button btnGo;
		#endregion
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		// User fields
		StreamReader	sr = null;
		long			FileSize;
		private System.Windows.Forms.Label lblCustomersLines;
		private System.Windows.Forms.Label lblCustomersPct;
		private System.Windows.Forms.Label lblCustomersChars;
		private System.Windows.Forms.Label lblPlacementsChars;
		private System.Windows.Forms.Label lblPlacementsPct;
		private System.Windows.Forms.Label lblPlacementsLines;
		private System.Windows.Forms.Label lblPlacementsCount;
		private System.Windows.Forms.Label lblReferralsCount;
		private System.Windows.Forms.Label lblSurveysCount;
		private System.Windows.Forms.Label lblDemographicsCount;
		private System.Windows.Forms.Label lblServicesCount;
		private System.Windows.Forms.Label lblCustomersCount;
		private System.Windows.Forms.Label label19;
		Type			thisType;

		public AnalyzeActTrkExportFile() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			thisType = this.GetType();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)	{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.lblCustomersLines = new System.Windows.Forms.Label();
			this.lblCustomersPct = new System.Windows.Forms.Label();
			this.lblCustomersChars = new System.Windows.Forms.Label();
			this.lblServicesChars = new System.Windows.Forms.Label();
			this.lblServicesPct = new System.Windows.Forms.Label();
			this.lblServicesLines = new System.Windows.Forms.Label();
			this.lblDemographicsChars = new System.Windows.Forms.Label();
			this.lblDemographicsPct = new System.Windows.Forms.Label();
			this.lblDemographicsLines = new System.Windows.Forms.Label();
			this.lblSurveysChars = new System.Windows.Forms.Label();
			this.lblSurveysPct = new System.Windows.Forms.Label();
			this.lblSurveysLines = new System.Windows.Forms.Label();
			this.lblReferralsChars = new System.Windows.Forms.Label();
			this.lblReferralsPct = new System.Windows.Forms.Label();
			this.lblReferralsLines = new System.Windows.Forms.Label();
			this.lblPlacementsChars = new System.Windows.Forms.Label();
			this.lblPlacementsPct = new System.Windows.Forms.Label();
			this.lblPlacementsLines = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.lblFileSize = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.btnGo = new System.Windows.Forms.Button();
			this.lblPlacementsCount = new System.Windows.Forms.Label();
			this.lblReferralsCount = new System.Windows.Forms.Label();
			this.lblSurveysCount = new System.Windows.Forms.Label();
			this.lblDemographicsCount = new System.Windows.Forms.Label();
			this.lblServicesCount = new System.Windows.Forms.Label();
			this.lblCustomersCount = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "File Name";
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Location = new System.Drawing.Point(136, 16);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(560, 22);
			this.txtFilename.TabIndex = 1;
			this.txtFilename.Text = "C:\\ExportAll.xml";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(720, 16);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(136, 24);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 128);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 24);
			this.label2.TabIndex = 3;
			this.label2.Text = "Section";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 248);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 24);
			this.label3.TabIndex = 4;
			this.label3.Text = "Characters";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 208);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 24);
			this.label4.TabIndex = 5;
			this.label4.Text = "Lines";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 288);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 24);
			this.label5.TabIndex = 6;
			this.label5.Text = "Percent";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(256, 128);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 24);
			this.label7.TabIndex = 8;
			this.label7.Text = "Services";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(136, 128);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(104, 24);
			this.label6.TabIndex = 9;
			this.label6.Text = "Customers";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(376, 128);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 24);
			this.label8.TabIndex = 10;
			this.label8.Text = "Demographics";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label9.Location = new System.Drawing.Point(736, 128);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 24);
			this.label9.TabIndex = 11;
			this.label9.Text = "Placement";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label10.Location = new System.Drawing.Point(616, 128);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 24);
			this.label10.TabIndex = 12;
			this.label10.Text = "Referrals";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label11.Location = new System.Drawing.Point(496, 128);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 24);
			this.label11.TabIndex = 13;
			this.label11.Text = "Surveys";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCustomersLines
			// 
			this.lblCustomersLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCustomersLines.Location = new System.Drawing.Point(136, 208);
			this.lblCustomersLines.Name = "lblCustomersLines";
			this.lblCustomersLines.Size = new System.Drawing.Size(104, 24);
			this.lblCustomersLines.TabIndex = 14;
			this.lblCustomersLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCustomersPct
			// 
			this.lblCustomersPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCustomersPct.Location = new System.Drawing.Point(136, 288);
			this.lblCustomersPct.Name = "lblCustomersPct";
			this.lblCustomersPct.Size = new System.Drawing.Size(104, 24);
			this.lblCustomersPct.TabIndex = 15;
			this.lblCustomersPct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCustomersChars
			// 
			this.lblCustomersChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCustomersChars.Location = new System.Drawing.Point(136, 248);
			this.lblCustomersChars.Name = "lblCustomersChars";
			this.lblCustomersChars.Size = new System.Drawing.Size(104, 24);
			this.lblCustomersChars.TabIndex = 16;
			this.lblCustomersChars.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblServicesChars
			// 
			this.lblServicesChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblServicesChars.Location = new System.Drawing.Point(256, 248);
			this.lblServicesChars.Name = "lblServicesChars";
			this.lblServicesChars.Size = new System.Drawing.Size(104, 24);
			this.lblServicesChars.TabIndex = 19;
			this.lblServicesChars.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblServicesPct
			// 
			this.lblServicesPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblServicesPct.Location = new System.Drawing.Point(256, 288);
			this.lblServicesPct.Name = "lblServicesPct";
			this.lblServicesPct.Size = new System.Drawing.Size(104, 24);
			this.lblServicesPct.TabIndex = 18;
			this.lblServicesPct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblServicesLines
			// 
			this.lblServicesLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblServicesLines.Location = new System.Drawing.Point(256, 208);
			this.lblServicesLines.Name = "lblServicesLines";
			this.lblServicesLines.Size = new System.Drawing.Size(104, 24);
			this.lblServicesLines.TabIndex = 17;
			this.lblServicesLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDemographicsChars
			// 
			this.lblDemographicsChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDemographicsChars.Location = new System.Drawing.Point(376, 248);
			this.lblDemographicsChars.Name = "lblDemographicsChars";
			this.lblDemographicsChars.Size = new System.Drawing.Size(104, 24);
			this.lblDemographicsChars.TabIndex = 22;
			this.lblDemographicsChars.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDemographicsPct
			// 
			this.lblDemographicsPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDemographicsPct.Location = new System.Drawing.Point(376, 288);
			this.lblDemographicsPct.Name = "lblDemographicsPct";
			this.lblDemographicsPct.Size = new System.Drawing.Size(104, 24);
			this.lblDemographicsPct.TabIndex = 21;
			this.lblDemographicsPct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDemographicsLines
			// 
			this.lblDemographicsLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDemographicsLines.Location = new System.Drawing.Point(376, 208);
			this.lblDemographicsLines.Name = "lblDemographicsLines";
			this.lblDemographicsLines.Size = new System.Drawing.Size(104, 24);
			this.lblDemographicsLines.TabIndex = 20;
			this.lblDemographicsLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSurveysChars
			// 
			this.lblSurveysChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblSurveysChars.Location = new System.Drawing.Point(496, 248);
			this.lblSurveysChars.Name = "lblSurveysChars";
			this.lblSurveysChars.Size = new System.Drawing.Size(104, 24);
			this.lblSurveysChars.TabIndex = 25;
			this.lblSurveysChars.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSurveysPct
			// 
			this.lblSurveysPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblSurveysPct.Location = new System.Drawing.Point(496, 288);
			this.lblSurveysPct.Name = "lblSurveysPct";
			this.lblSurveysPct.Size = new System.Drawing.Size(104, 24);
			this.lblSurveysPct.TabIndex = 24;
			this.lblSurveysPct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSurveysLines
			// 
			this.lblSurveysLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblSurveysLines.Location = new System.Drawing.Point(496, 208);
			this.lblSurveysLines.Name = "lblSurveysLines";
			this.lblSurveysLines.Size = new System.Drawing.Size(104, 24);
			this.lblSurveysLines.TabIndex = 23;
			this.lblSurveysLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblReferralsChars
			// 
			this.lblReferralsChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblReferralsChars.Location = new System.Drawing.Point(616, 248);
			this.lblReferralsChars.Name = "lblReferralsChars";
			this.lblReferralsChars.Size = new System.Drawing.Size(104, 24);
			this.lblReferralsChars.TabIndex = 28;
			this.lblReferralsChars.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblReferralsPct
			// 
			this.lblReferralsPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblReferralsPct.Location = new System.Drawing.Point(616, 288);
			this.lblReferralsPct.Name = "lblReferralsPct";
			this.lblReferralsPct.Size = new System.Drawing.Size(104, 24);
			this.lblReferralsPct.TabIndex = 27;
			this.lblReferralsPct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblReferralsLines
			// 
			this.lblReferralsLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblReferralsLines.Location = new System.Drawing.Point(616, 208);
			this.lblReferralsLines.Name = "lblReferralsLines";
			this.lblReferralsLines.Size = new System.Drawing.Size(104, 24);
			this.lblReferralsLines.TabIndex = 26;
			this.lblReferralsLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblPlacementsChars
			// 
			this.lblPlacementsChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPlacementsChars.Location = new System.Drawing.Point(736, 248);
			this.lblPlacementsChars.Name = "lblPlacementsChars";
			this.lblPlacementsChars.Size = new System.Drawing.Size(104, 24);
			this.lblPlacementsChars.TabIndex = 31;
			this.lblPlacementsChars.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblPlacementsPct
			// 
			this.lblPlacementsPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPlacementsPct.Location = new System.Drawing.Point(736, 288);
			this.lblPlacementsPct.Name = "lblPlacementsPct";
			this.lblPlacementsPct.Size = new System.Drawing.Size(104, 24);
			this.lblPlacementsPct.TabIndex = 30;
			this.lblPlacementsPct.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblPlacementsLines
			// 
			this.lblPlacementsLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPlacementsLines.Location = new System.Drawing.Point(736, 208);
			this.lblPlacementsLines.Name = "lblPlacementsLines";
			this.lblPlacementsLines.Size = new System.Drawing.Size(104, 24);
			this.lblPlacementsLines.TabIndex = 29;
			this.lblPlacementsLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(24, 64);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(88, 24);
			this.label12.TabIndex = 32;
			this.label12.Text = "File Size";
			// 
			// lblFileSize
			// 
			this.lblFileSize.Location = new System.Drawing.Point(136, 64);
			this.lblFileSize.Name = "lblFileSize";
			this.lblFileSize.Size = new System.Drawing.Size(152, 24);
			this.lblFileSize.TabIndex = 33;
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(720, 56);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(136, 24);
			this.btnGo.TabIndex = 34;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lblPlacementsCount
			// 
			this.lblPlacementsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPlacementsCount.Location = new System.Drawing.Point(736, 168);
			this.lblPlacementsCount.Name = "lblPlacementsCount";
			this.lblPlacementsCount.Size = new System.Drawing.Size(104, 24);
			this.lblPlacementsCount.TabIndex = 41;
			this.lblPlacementsCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblReferralsCount
			// 
			this.lblReferralsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblReferralsCount.Location = new System.Drawing.Point(616, 168);
			this.lblReferralsCount.Name = "lblReferralsCount";
			this.lblReferralsCount.Size = new System.Drawing.Size(104, 24);
			this.lblReferralsCount.TabIndex = 40;
			this.lblReferralsCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSurveysCount
			// 
			this.lblSurveysCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblSurveysCount.Location = new System.Drawing.Point(496, 168);
			this.lblSurveysCount.Name = "lblSurveysCount";
			this.lblSurveysCount.Size = new System.Drawing.Size(104, 24);
			this.lblSurveysCount.TabIndex = 39;
			this.lblSurveysCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDemographicsCount
			// 
			this.lblDemographicsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDemographicsCount.Location = new System.Drawing.Point(376, 168);
			this.lblDemographicsCount.Name = "lblDemographicsCount";
			this.lblDemographicsCount.Size = new System.Drawing.Size(104, 24);
			this.lblDemographicsCount.TabIndex = 38;
			this.lblDemographicsCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblServicesCount
			// 
			this.lblServicesCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblServicesCount.Location = new System.Drawing.Point(256, 168);
			this.lblServicesCount.Name = "lblServicesCount";
			this.lblServicesCount.Size = new System.Drawing.Size(104, 24);
			this.lblServicesCount.TabIndex = 37;
			this.lblServicesCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCustomersCount
			// 
			this.lblCustomersCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCustomersCount.Location = new System.Drawing.Point(136, 168);
			this.lblCustomersCount.Name = "lblCustomersCount";
			this.lblCustomersCount.Size = new System.Drawing.Size(104, 24);
			this.lblCustomersCount.TabIndex = 36;
			this.lblCustomersCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(24, 168);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(72, 24);
			this.label19.TabIndex = 35;
			this.label19.Text = "Count";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AnalyzeActTrkExportFile
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(864, 352);
			this.Controls.Add(this.lblPlacementsCount);
			this.Controls.Add(this.lblReferralsCount);
			this.Controls.Add(this.lblSurveysCount);
			this.Controls.Add(this.lblDemographicsCount);
			this.Controls.Add(this.lblServicesCount);
			this.Controls.Add(this.lblCustomersCount);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.lblFileSize);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.lblPlacementsChars);
			this.Controls.Add(this.lblPlacementsPct);
			this.Controls.Add(this.lblPlacementsLines);
			this.Controls.Add(this.lblReferralsChars);
			this.Controls.Add(this.lblReferralsPct);
			this.Controls.Add(this.lblReferralsLines);
			this.Controls.Add(this.lblSurveysChars);
			this.Controls.Add(this.lblSurveysPct);
			this.Controls.Add(this.lblSurveysLines);
			this.Controls.Add(this.lblDemographicsChars);
			this.Controls.Add(this.lblDemographicsPct);
			this.Controls.Add(this.lblDemographicsLines);
			this.Controls.Add(this.lblServicesChars);
			this.Controls.Add(this.lblServicesPct);
			this.Controls.Add(this.lblServicesLines);
			this.Controls.Add(this.lblCustomersChars);
			this.Controls.Add(this.lblCustomersPct);
			this.Controls.Add(this.lblCustomersLines);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.label1);
			this.Name = "AnalyzeActTrkExportFile";
			this.Text = "Analyze Activity Track Export File";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new AnalyzeActTrkExportFile());
		}

		
		private void btnGo_Click(object sender, System.EventArgs e) {
			ResetAllFields();
			try {
				sr = new StreamReader(txtFilename.Text);
				FileSize = sr.BaseStream.Length;
				lblFileSize.Text = string.Format("{0:N0}", FileSize);

				ProcessSection("Customers",		"Customer");
				ProcessSection("Services",		"Service");
				ProcessSection("Demographics",	"Demographic");
				ProcessSection("Surveys",		"Survey");
				ProcessSection("Referrals",		"Referral");
				ProcessSection("Placements",	"Placement");
			} catch (Exception ex) {
				MessageBox.Show("Error processing file - " + ex.Message);
			}
			if (sr != null) {
				sr.Close();
				sr = null;
			}
		}

		void ProcessSection(string SectionName, string ItemName) {
			int		lines = 0;
			int		chars = 0;
			int		count = 0;
			string	SectionStart = "<" + SectionName + ">";
			string	SectionEnd =  "</" + SectionName + ">";
			string	ItemNameStart = "<" + ItemName + " ";
			string	line;

			while ((line = sr.ReadLine()) != null) {
				if (line.IndexOf(SectionStart) >= 0) {
					// Found Section start
					++lines;
					chars += line.Length;
					while ((line = sr.ReadLine()) != null) {
						++lines;
						chars += line.Length;
						if (line.IndexOf(ItemNameStart) >= 0)
							++count;
						if (line.IndexOf(SectionEnd) >= 0) {
							ShowData(SectionName, count, lines, chars);
							return;
						}
					}
				}
			}
		}

		void ResetAllFields() {
			ResetSection("Customers");
			ResetSection("Services");
			ResetSection("Demographics");
			ResetSection("Surveys");
			ResetSection("Referrals");
			ResetSection("Placements");
			Application.DoEvents();
		}

		void ResetSection(string SectionName) {
			ResetField(SectionName, "Count");
			ResetField(SectionName, "Lines");
			ResetField(SectionName, "Chars");
			ResetField(SectionName, "Pct");
		}

		void ResetField(string SectionName, string Suffix) {
			Label	lbl = GetFieldLabel(SectionName, Suffix);
			lbl.Text = "";
		}

		void ShowData(string SectionName, int count, int lines, int chars) {
			SetField(SectionName, "Count", count);
			SetField(SectionName, "Lines", lines);
			SetField(SectionName, "Chars", chars);
			SetField(SectionName, "Pct", (int)(100 * (float)chars / (float)FileSize));
			Application.DoEvents();
		}

		void SetField(string SectionName, string Suffix, int val) {
			Label	lbl = GetFieldLabel(SectionName, Suffix);
			lbl.Text = string.Format("{0:N0}", val);
		}

		Label GetFieldLabel(string SectionName, string Suffix) {
			string		FieldName;
			FieldInfo	fi;
			FieldName = "lbl" + SectionName + Suffix;
			fi = thisType.GetField(FieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			return (Label)fi.GetValue(this);
		}

		private void btnBrowse_Click(object sender, System.EventArgs e) {
			openFileDialog1.Filter = "Export files (*.xml)|*.xml|All files (*.*)|*.*" ;
			DialogResult res = openFileDialog1.ShowDialog();
			if (res == DialogResult.OK) {
				txtFilename.Text = openFileDialog1.FileName;
			}
		}
	}
}
