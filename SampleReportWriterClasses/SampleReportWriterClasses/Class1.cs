using System;
using System.Collections.Generic;

namespace SampleReportWriterClasses {
	public class ReportElement {
		Point	x, y;				// Where the element is
		Size	size;
		Color	ForeColor, BackColor;
		Font	font;
		string	Source;				// What field this is
		// Whatever other properties you want, e.g. Justification
		// Add methods here to set and retrieve these properties.
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class ReportBand {
		enum BandType {
			ReportHeader,
			PageHeader,
			GroupHeader,
			Detail,
			GroupFooter,
			PageFooter,
			ReportFooter
		}

		Color	BackColor;
		Image	BackgroundImage;

		List<ReportElement>	elements;
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
	
	class Report {
		string				ReportName;
		string				Source;			// I put in "string" here to avoid defining the exact source

		// Layout follows
		ReportBand			ReportHeader;
		ReportBand			PageHeader;
		List<ReportBand>	GroupHeaders;
		ReportBand			Detail;
		List<ReportBand>	GroupFooters;
		ReportBand			PageFooter;
		ReportBand			ReportFooter;

//---------------------------------------------------------------------------------------

		void SetReportHeader(List<ReportElement> ElementsInHeader) {
		}

//---------------------------------------------------------------------------------------

		void AddGroup() {
		}

//---------------------------------------------------------------------------------------

		void DeleteGroup() {
		}

//---------------------------------------------------------------------------------------

		abstract void Serialize(/* Parameters deliberately omitted */) {
			// Derive a class and serialize this report to SS Report Services, Oracle, etc
		}

//---------------------------------------------------------------------------------------

		abstract void Deserialize(/* Parameters deliberately omitted */) {
			// Derive a class and deserialize this report from SS Report Services, Oracle, etc
		}
	}
}
