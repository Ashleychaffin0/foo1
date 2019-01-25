// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Data;

namespace Bartizan.Importer {
	class Import_Demographics {

		IDbConnection	conn;
		ParsedSwipe		SwipeData;
		DBCoordinator	DBCoord;
		ImporterTable	impDemogQuestions;		// Stuff about tblDemographicsQuestions
		ImporterTable	impDemogAnswers;		// Stuff about tblDemographicsAnswers
		ImporterTable	impPersonnelDemog;		// Stuff about tblPersonnelDemographics

//---------------------------------------------------------------------------------------

		public Import_Demographics(IDbConnection conn, DatabaseType dbType, DBCoordinator DBCoord) {
			this.conn		= conn;
			this.DBCoord	= DBCoord;

			impDemogQuestions = DBCoord.GetImporterTable(ImporterTable.TableName_tblDemographicsQuestions);
			impDemogAnswers	  = DBCoord.GetImporterTable(ImporterTable.TableName_tblDemographicsAnswers);
			impPersonnelDemog = DBCoord.GetImporterTable(ImporterTable.TableName_tblPersonnelDemographics);
		}

//---------------------------------------------------------------------------------------

		public void Import(ParsedSwipe SwipeData) {
			string		Question, Answer;
			int			QID, AID;			// Question and Answer IDs
			this.SwipeData = SwipeData;		// TODO: Do we need this, here and above?
			foreach (Demographic demog in SwipeData.Demographics) {
				Question = demog.Question;
				// In SwipeData, each Question has exactly one answer
				Answer   = ((DemographicAnswer)demog.Answers[0]).Answer;
				QID = AddDemographicQuestion(Question);
				AID = AddDemographicAnswer(QID, Answer);
			}
		}

//---------------------------------------------------------------------------------------

		int AddDemographicQuestion(string Question) {
			// TODO:
			return -1;
		}

//---------------------------------------------------------------------------------------

		int AddDemographicAnswer(int QID, string Answer) {
			// TODO:
			return -1;
		}
	}
}
