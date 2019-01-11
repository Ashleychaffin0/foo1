using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using MicrosoftZuneLibrary;

namespace LRS.Zune.ClassGeneration {
	class GenerateQueriesAndSchemae {

//---------------------------------------------------------------------------------------

		public static Dictionary<EQueryType, List<SchemaMapAndType>> 
					GetQueriesAndSchemae(ZuneLibrary zl) {
			MicrosoftZuneLibrary.ZuneQueryList ZQList;
			Dictionary<EQueryType, List<SchemaMapAndType>> QueriesAndSchemae;
			QueriesAndSchemae = new Dictionary<EQueryType, List<SchemaMapAndType>>();
			List<SchemaMapAndType> Schemae;
			foreach (var EQType in Enum.GetValues(typeof(EQueryType))) {
				Schemae = new List<SchemaMapAndType>();
				try {
					// Note: SortAtom is just 0 in QueryDatabase call
					using (ZQList = zl.QueryDatabase((EQueryType)EQType, 0, EQuerySortType.eQuerySortOrderAscending, 0, null)) {
						if ((ZQList != null) && (ZQList.Count > 0)) {
							Schemae = GetSchemaeForEqueryType(ZQList, EQType);
						}
					}
				} catch {
					// Ignore exception and continue
				}
				QueriesAndSchemae[(EQueryType)EQType] = Schemae;
			}
			return QueriesAndSchemae;
		}

//---------------------------------------------------------------------------------------

		public static XDocument GetQueriesAndSchemaeAsXdoc(ZuneLibrary zl) {
			var QandS = GenerateQueriesAndSchemae.GetQueriesAndSchemae(zl);
			// This is way cool!
			XDocument xd = new XDocument(
				new XElement("QueryTypes",
					from key in QandS.Keys
					select new XElement("QueryType",
						new XAttribute("qtype", key),
						// new XElement("SchemaMapAndType",
							from smt in QandS[key]
							select new XElement("SchemaMap", smt.smt,
										new XAttribute("type", smt.type)
					))));
			// Sample code from another app:
			//		select new XElement("Customer"),
			//			new XElement("CompanyName", c.CompanyName),
			//			new XElement("City", c.City)
			return xd;
		}

//---------------------------------------------------------------------------------------

		private static List<SchemaMapAndType> GetSchemaeForEqueryType(ZuneQueryList ZQList, object EQType) {
			List<SchemaMapAndType> Schemae = new List<SchemaMapAndType>();
			uint usmt;		// // smt = SchemaMap Type
			uint ix = 0;
			Type t;
			foreach (var smt in Enum.GetValues(typeof(SchemaMap))) {
				usmt = (uint)(int)smt;
				t = FieldType(ZQList, usmt, ix);
				if (t != null) {
					Schemae.Add(new SchemaMapAndType((SchemaMap)smt, t));
				}
			}
			return Schemae;
		}

//---------------------------------------------------------------------------------------

		private static Type FieldType(ZuneQueryList ZQList, uint usmt, uint ix) {
			// AFAICT, the Zune software uses SQL Server Compact Edition (or possibly an
			// earlier version). It supports only a limited number of managed data types
			// and these are what we check for below.

			// Note: Opening the .sdf file in Sql Server Management Studio 2005 shows
			//		 that the database is SQL Server Compact Edition version 3.0.5300.0

			// Again, according to SSMS, the types it supports are
			//	bigint, binary, bit, datetime, float, image, int, money, nchar, ntext
			//	numeric, nvarchar, real, smallint, tinyint, uniqueidentifier, varbinary.

			// See http://technet.microsoft.com/en-us/library/ms174642.aspx
			object o;

			// Some fields show up as int's, since they're enums. But we may know
			// better. So do some special checking first.

			if (usmt == (uint)SchemaMap.kiIndex_MediaType) {
				return typeof(EMediaTypes);
			}

			// End of special checking

			o = ZQList.GetFieldValue(ix, typeof(int), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(string), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(DateTime), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(Guid), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(bool), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(byte[]), usmt);	// varbinary. Also Unicode?
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(byte), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(decimal), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(double), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(System.Drawing.Image), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(short), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(int), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(ushort), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(long), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(ulong), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(float), usmt);
			if (o != null)
				return o.GetType();

			// Dunno if these are possible, but let's try
			o = ZQList.GetFieldValue(ix, typeof(string[]), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(int[]), usmt);
			if (o != null)
				return o.GetType();

			o = ZQList.GetFieldValue(ix, typeof(byte[,]), usmt);
			if (o != null)
				return o.GetType();

			return null;
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	internal class SchemaMapAndType {
		internal SchemaMap smt;
		internal Type type;

//---------------------------------------------------------------------------------------

		internal SchemaMapAndType(SchemaMap smt, Type type) {
			this.smt = smt;
			this.type = type;
		}
	}
}
