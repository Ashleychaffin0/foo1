// Copyright (c) 2004 Bartizan Data Systems

using System;

// Since I don't know enough about SQL Server (Oracle, etc) to know
// what their connection strings have in common (or not) with Jet's,
// we'll just do a class for Jet. Later we may have a parallel class
// for SQL Server, or maybe a base class that does things that are
// common. But for now it's Jet all the way.

namespace Bartizan.Utils.Database {

	[Flags]
	public enum ConnectionMode {
		Read			= 0x0001,				
		Write			= 0x0002,				
		ReadWrite		= Read | Write,
		ShareDenyNone	= 0x0004,
		ShareDenyRead	= 0x0008,
		ShareDenyWrite	= 0x0010,
		ShareExclusive	= 0x0020
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Summary description for JetConnectionString.
	/// </summary>
	public class JetConnectionString {

		// The following fields are the components of the connection string. Any
		// that are null aren't returned as part of the string.

		string			_Provider;
		// The following fields all have public properties
		ConnectionMode	_Mode;
		string			_DataSource;
		string			_MDWFile;
		string			_UserID;
		string			_Password;
		string			_DatabasePassword;

		// Note: The best reference I've found so far for these fields is the MSDN
		//		 article titled "ADO Provider Properties and Settings"

		// TODO: Other fields we could add later.
		// Jet OLEDB:Global Partial Bulk Ops=2
		// Jet OLEDB:Registry Path=
		// Jet OLEDB:Database Locking Mode=1
		// Jet OLEDB:Database Password=
		// Jet OLEDB:Engine Type=5
		// Jet OLEDB:Global Bulk Transactions=1
		// Jet OLEDB:SFP=False
		// Extended Properties=
		// Jet OLEDB:New Database Password=
		// Jet OLEDB:Create System Database=False
		// Jet OLEDB:Don't Copy Locale on Compact=False
		// Jet OLEDB:Compact Without Replica Repair=False
		// Jet OLEDB:Encrypt Database=False

//---------------------------------------------------------------------------------------

		public JetConnectionString() {
			_Provider	= "Microsoft.Jet.OLEDB.4.0";
			Reset();
		}

//---------------------------------------------------------------------------------------

		public void Reset() {
			_Mode				= 0;
			_DataSource			= null;
			_MDWFile			= null;
			_UserID				= null;
			_Password			= null;
			_DatabasePassword	= null;
		}

//---------------------------------------------------------------------------------------

		public ConnectionMode Mode {		// e.g. ReadWrite|Share Deny None
			// TODO: Some enums for the mode attributes might be nice
			get { return _Mode; }
			set { _Mode = value; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Filename of the database being connected to
		/// </summary>
		public string DataSource {		
			get { return _DataSource; }
			set { _DataSource = value; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// AKA the System Database. The filename of the .MDW file, if the db is secured.
		/// </summary>
		public string MDWFile {		
			get { return _MDWFile; }
			set { _MDWFile = value; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// For a user-level secured file, the UserID trying to open it. See also the
		/// Password property
		/// </summary>
		public string UserID {		
			get { return _UserID; }
			set { _UserID = value; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// For a user-level secured file, the password of the UserID trying to open it
		/// </summary>
		public string Password {		
			get { return _Password; }
			set { _Password = value; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// For a file secured by share level security, the password of the database
		/// </summary>
		public string DatabasePassword {		
			get { return _DatabasePassword; }
			set { _DatabasePassword = value; }
		}

//---------------------------------------------------------------------------------------

		public string ConnectionString {
			get {
				string	s = "";
				s += Append("Provider",			_Provider);
				s += AppendMode();						// Handle Mode separately
				s += Append("Data Source",		_DataSource);
				s += Append("System Database",	_MDWFile);
				s += Append("User ID",			_UserID);
				s += Append("Password",			_Password);
				s += Append("Jet OLEDB:Database Password",	_DatabasePassword);
				return s;
			}
		}

//---------------------------------------------------------------------------------------

		string Append(string Name, string Value) {
			if (Value == null)
				return "";
			string	s = Name + "=" + Value;
			if (! Value.EndsWith(";"))
				s += ";";
			return s;
		}

//---------------------------------------------------------------------------------------

		string AppendMode() {
			string		s = "";
			if (_Mode == 0)
				return s;

			if ((_Mode & ConnectionMode.ReadWrite) == ConnectionMode.ReadWrite) 
				s += "|ReadWrite";
			else if ((_Mode & ConnectionMode.Read) != 0)
				s += "|Read";
			else if ((_Mode & ConnectionMode.Write) != 0)
				s += "|Write";

			if ((_Mode & ConnectionMode.ShareDenyNone) != 0)
				s += "|Share Deny None";
			if ((_Mode & ConnectionMode.ShareDenyRead) != 0)
				s += "|Share Deny Read";
			if ((_Mode & ConnectionMode.ShareDenyWrite) != 0)
				s += "|Share Deny Write";
			if ((_Mode & ConnectionMode.ShareExclusive) != 0)
				s += "|Share Exclusive";

			if (s.StartsWith("|"))
				s = s.Remove(0, 1);
			return "Mode=" + s + ";";
		}
	}
}
