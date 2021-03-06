#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShowDecompressImportData
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="lldevel")]
	public partial class DataClasses1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InserttblSavedImport(tblSavedImport instance);
    partial void UpdatetblSavedImport(tblSavedImport instance);
    partial void DeletetblSavedImport(tblSavedImport instance);
    partial void InserttblSwipe(tblSwipe instance);
    partial void UpdatetblSwipe(tblSwipe instance);
    partial void DeletetblSwipe(tblSwipe instance);
    partial void InserttblSwipesText(tblSwipesText instance);
    partial void UpdatetblSwipesText(tblSwipesText instance);
    partial void DeletetblSwipesText(tblSwipesText instance);
    partial void InserttblTerminal(tblTerminal instance);
    partial void UpdatetblTerminal(tblTerminal instance);
    partial void DeletetblTerminal(tblTerminal instance);
    partial void InserttblPersonByEvent(tblPersonByEvent instance);
    partial void UpdatetblPersonByEvent(tblPersonByEvent instance);
    partial void DeletetblPersonByEvent(tblPersonByEvent instance);
    partial void InserttblMapCfg(tblMapCfg instance);
    partial void UpdatetblMapCfg(tblMapCfg instance);
    partial void DeletetblMapCfg(tblMapCfg instance);
    #endregion
		
		public DataClasses1DataContext() : 
				base(global::ShowDecompressImportData.Properties.Settings.Default.lldevelConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<tblSavedImport> tblSavedImports
		{
			get
			{
				return this.GetTable<tblSavedImport>();
			}
		}
		
		public System.Data.Linq.Table<tblSwipe> tblSwipes
		{
			get
			{
				return this.GetTable<tblSwipe>();
			}
		}
		
		public System.Data.Linq.Table<tblSwipesText> tblSwipesTexts
		{
			get
			{
				return this.GetTable<tblSwipesText>();
			}
		}
		
		public System.Data.Linq.Table<tblTerminal> tblTerminals
		{
			get
			{
				return this.GetTable<tblTerminal>();
			}
		}
		
		public System.Data.Linq.Table<tblPersonByEvent> tblPersonByEvents
		{
			get
			{
				return this.GetTable<tblPersonByEvent>();
			}
		}
		
		public System.Data.Linq.Table<tblImportTracking> tblImportTrackings
		{
			get
			{
				return this.GetTable<tblImportTracking>();
			}
		}
		
		public System.Data.Linq.Table<tblMapCfg> tblMapCfgs
		{
			get
			{
				return this.GetTable<tblMapCfg>();
			}
		}
	}
	
	[Table(Name="dbo.tblSavedImports")]
	public partial class tblSavedImport : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _SavedImportID;
		
		private System.DateTime _WhenImported;
		
		private int _AcctID;
		
		private int _EventID;
		
		private System.Nullable<int> _RCAcctID;
		
		private int _MapCfgID;
		
		private bool _IgnoreFirstRecord;
		
		private bool _DataIsExpanded;
		
		private int _Flags;
		
		private bool _IsVisitorDataCompressed;
		
		private string _TerminalSerial;
		
		private int _DataLen;
		
		private string _VisitorData;
		
		private int _DataLenCompressed;
		
		private System.Nullable<int> _RecordCount;
		
		private System.Nullable<int> _TallTableInsertions;
		
		private System.Nullable<int> _ResponseInsertions;
		
		private System.Nullable<int> _BulkFallbacks_TallTable;
		
		private System.Nullable<int> _BulkFallbacks_Responses;
		
		private System.Nullable<int> _MillisecondsToImport;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSavedImportIDChanging(int value);
    partial void OnSavedImportIDChanged();
    partial void OnWhenImportedChanging(System.DateTime value);
    partial void OnWhenImportedChanged();
    partial void OnAcctIDChanging(int value);
    partial void OnAcctIDChanged();
    partial void OnEventIDChanging(int value);
    partial void OnEventIDChanged();
    partial void OnRCAcctIDChanging(System.Nullable<int> value);
    partial void OnRCAcctIDChanged();
    partial void OnMapCfgIDChanging(int value);
    partial void OnMapCfgIDChanged();
    partial void OnIgnoreFirstRecordChanging(bool value);
    partial void OnIgnoreFirstRecordChanged();
    partial void OnDataIsExpandedChanging(bool value);
    partial void OnDataIsExpandedChanged();
    partial void OnFlagsChanging(int value);
    partial void OnFlagsChanged();
    partial void OnIsVisitorDataCompressedChanging(bool value);
    partial void OnIsVisitorDataCompressedChanged();
    partial void OnTerminalSerialChanging(string value);
    partial void OnTerminalSerialChanged();
    partial void OnDataLenChanging(int value);
    partial void OnDataLenChanged();
    partial void OnVisitorDataChanging(string value);
    partial void OnVisitorDataChanged();
    partial void OnDataLenCompressedChanging(int value);
    partial void OnDataLenCompressedChanged();
    partial void OnRecordCountChanging(System.Nullable<int> value);
    partial void OnRecordCountChanged();
    partial void OnTallTableInsertionsChanging(System.Nullable<int> value);
    partial void OnTallTableInsertionsChanged();
    partial void OnResponseInsertionsChanging(System.Nullable<int> value);
    partial void OnResponseInsertionsChanged();
    partial void OnBulkFallbacks_TallTableChanging(System.Nullable<int> value);
    partial void OnBulkFallbacks_TallTableChanged();
    partial void OnBulkFallbacks_ResponsesChanging(System.Nullable<int> value);
    partial void OnBulkFallbacks_ResponsesChanged();
    partial void OnMillisecondsToImportChanging(System.Nullable<int> value);
    partial void OnMillisecondsToImportChanged();
    #endregion
		
		public tblSavedImport()
		{
			OnCreated();
		}
		
		[Column(Storage="_SavedImportID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int SavedImportID
		{
			get
			{
				return this._SavedImportID;
			}
			set
			{
				if ((this._SavedImportID != value))
				{
					this.OnSavedImportIDChanging(value);
					this.SendPropertyChanging();
					this._SavedImportID = value;
					this.SendPropertyChanged("SavedImportID");
					this.OnSavedImportIDChanged();
				}
			}
		}
		
		[Column(Storage="_WhenImported", DbType="DateTime NOT NULL")]
		public System.DateTime WhenImported
		{
			get
			{
				return this._WhenImported;
			}
			set
			{
				if ((this._WhenImported != value))
				{
					this.OnWhenImportedChanging(value);
					this.SendPropertyChanging();
					this._WhenImported = value;
					this.SendPropertyChanged("WhenImported");
					this.OnWhenImportedChanged();
				}
			}
		}
		
		[Column(Storage="_AcctID", DbType="Int NOT NULL")]
		public int AcctID
		{
			get
			{
				return this._AcctID;
			}
			set
			{
				if ((this._AcctID != value))
				{
					this.OnAcctIDChanging(value);
					this.SendPropertyChanging();
					this._AcctID = value;
					this.SendPropertyChanged("AcctID");
					this.OnAcctIDChanged();
				}
			}
		}
		
		[Column(Storage="_EventID", DbType="Int NOT NULL")]
		public int EventID
		{
			get
			{
				return this._EventID;
			}
			set
			{
				if ((this._EventID != value))
				{
					this.OnEventIDChanging(value);
					this.SendPropertyChanging();
					this._EventID = value;
					this.SendPropertyChanged("EventID");
					this.OnEventIDChanged();
				}
			}
		}
		
		[Column(Storage="_RCAcctID", DbType="Int")]
		public System.Nullable<int> RCAcctID
		{
			get
			{
				return this._RCAcctID;
			}
			set
			{
				if ((this._RCAcctID != value))
				{
					this.OnRCAcctIDChanging(value);
					this.SendPropertyChanging();
					this._RCAcctID = value;
					this.SendPropertyChanged("RCAcctID");
					this.OnRCAcctIDChanged();
				}
			}
		}
		
		[Column(Storage="_MapCfgID", DbType="Int NOT NULL")]
		public int MapCfgID
		{
			get
			{
				return this._MapCfgID;
			}
			set
			{
				if ((this._MapCfgID != value))
				{
					this.OnMapCfgIDChanging(value);
					this.SendPropertyChanging();
					this._MapCfgID = value;
					this.SendPropertyChanged("MapCfgID");
					this.OnMapCfgIDChanged();
				}
			}
		}
		
		[Column(Storage="_IgnoreFirstRecord", DbType="Bit NOT NULL")]
		public bool IgnoreFirstRecord
		{
			get
			{
				return this._IgnoreFirstRecord;
			}
			set
			{
				if ((this._IgnoreFirstRecord != value))
				{
					this.OnIgnoreFirstRecordChanging(value);
					this.SendPropertyChanging();
					this._IgnoreFirstRecord = value;
					this.SendPropertyChanged("IgnoreFirstRecord");
					this.OnIgnoreFirstRecordChanged();
				}
			}
		}
		
		[Column(Storage="_DataIsExpanded", DbType="Bit NOT NULL")]
		public bool DataIsExpanded
		{
			get
			{
				return this._DataIsExpanded;
			}
			set
			{
				if ((this._DataIsExpanded != value))
				{
					this.OnDataIsExpandedChanging(value);
					this.SendPropertyChanging();
					this._DataIsExpanded = value;
					this.SendPropertyChanged("DataIsExpanded");
					this.OnDataIsExpandedChanged();
				}
			}
		}
		
		[Column(Storage="_Flags", DbType="Int NOT NULL")]
		public int Flags
		{
			get
			{
				return this._Flags;
			}
			set
			{
				if ((this._Flags != value))
				{
					this.OnFlagsChanging(value);
					this.SendPropertyChanging();
					this._Flags = value;
					this.SendPropertyChanged("Flags");
					this.OnFlagsChanged();
				}
			}
		}
		
		[Column(Storage="_IsVisitorDataCompressed", DbType="Bit NOT NULL")]
		public bool IsVisitorDataCompressed
		{
			get
			{
				return this._IsVisitorDataCompressed;
			}
			set
			{
				if ((this._IsVisitorDataCompressed != value))
				{
					this.OnIsVisitorDataCompressedChanging(value);
					this.SendPropertyChanging();
					this._IsVisitorDataCompressed = value;
					this.SendPropertyChanged("IsVisitorDataCompressed");
					this.OnIsVisitorDataCompressedChanged();
				}
			}
		}
		
		[Column(Storage="_TerminalSerial", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string TerminalSerial
		{
			get
			{
				return this._TerminalSerial;
			}
			set
			{
				if ((this._TerminalSerial != value))
				{
					this.OnTerminalSerialChanging(value);
					this.SendPropertyChanging();
					this._TerminalSerial = value;
					this.SendPropertyChanged("TerminalSerial");
					this.OnTerminalSerialChanged();
				}
			}
		}
		
		[Column(Storage="_DataLen", DbType="Int NOT NULL")]
		public int DataLen
		{
			get
			{
				return this._DataLen;
			}
			set
			{
				if ((this._DataLen != value))
				{
					this.OnDataLenChanging(value);
					this.SendPropertyChanging();
					this._DataLen = value;
					this.SendPropertyChanged("DataLen");
					this.OnDataLenChanged();
				}
			}
		}
		
		[Column(Storage="_VisitorData", DbType="VarChar(MAX) NOT NULL", CanBeNull=false)]
		public string VisitorData
		{
			get
			{
				return this._VisitorData;
			}
			set
			{
				if ((this._VisitorData != value))
				{
					this.OnVisitorDataChanging(value);
					this.SendPropertyChanging();
					this._VisitorData = value;
					this.SendPropertyChanged("VisitorData");
					this.OnVisitorDataChanged();
				}
			}
		}
		
		[Column(Storage="_DataLenCompressed", DbType="Int NOT NULL")]
		public int DataLenCompressed
		{
			get
			{
				return this._DataLenCompressed;
			}
			set
			{
				if ((this._DataLenCompressed != value))
				{
					this.OnDataLenCompressedChanging(value);
					this.SendPropertyChanging();
					this._DataLenCompressed = value;
					this.SendPropertyChanged("DataLenCompressed");
					this.OnDataLenCompressedChanged();
				}
			}
		}
		
		[Column(Storage="_RecordCount", DbType="Int")]
		public System.Nullable<int> RecordCount
		{
			get
			{
				return this._RecordCount;
			}
			set
			{
				if ((this._RecordCount != value))
				{
					this.OnRecordCountChanging(value);
					this.SendPropertyChanging();
					this._RecordCount = value;
					this.SendPropertyChanged("RecordCount");
					this.OnRecordCountChanged();
				}
			}
		}
		
		[Column(Storage="_TallTableInsertions", DbType="Int")]
		public System.Nullable<int> TallTableInsertions
		{
			get
			{
				return this._TallTableInsertions;
			}
			set
			{
				if ((this._TallTableInsertions != value))
				{
					this.OnTallTableInsertionsChanging(value);
					this.SendPropertyChanging();
					this._TallTableInsertions = value;
					this.SendPropertyChanged("TallTableInsertions");
					this.OnTallTableInsertionsChanged();
				}
			}
		}
		
		[Column(Storage="_ResponseInsertions", DbType="Int")]
		public System.Nullable<int> ResponseInsertions
		{
			get
			{
				return this._ResponseInsertions;
			}
			set
			{
				if ((this._ResponseInsertions != value))
				{
					this.OnResponseInsertionsChanging(value);
					this.SendPropertyChanging();
					this._ResponseInsertions = value;
					this.SendPropertyChanged("ResponseInsertions");
					this.OnResponseInsertionsChanged();
				}
			}
		}
		
		[Column(Storage="_BulkFallbacks_TallTable", DbType="Int")]
		public System.Nullable<int> BulkFallbacks_TallTable
		{
			get
			{
				return this._BulkFallbacks_TallTable;
			}
			set
			{
				if ((this._BulkFallbacks_TallTable != value))
				{
					this.OnBulkFallbacks_TallTableChanging(value);
					this.SendPropertyChanging();
					this._BulkFallbacks_TallTable = value;
					this.SendPropertyChanged("BulkFallbacks_TallTable");
					this.OnBulkFallbacks_TallTableChanged();
				}
			}
		}
		
		[Column(Storage="_BulkFallbacks_Responses", DbType="Int")]
		public System.Nullable<int> BulkFallbacks_Responses
		{
			get
			{
				return this._BulkFallbacks_Responses;
			}
			set
			{
				if ((this._BulkFallbacks_Responses != value))
				{
					this.OnBulkFallbacks_ResponsesChanging(value);
					this.SendPropertyChanging();
					this._BulkFallbacks_Responses = value;
					this.SendPropertyChanged("BulkFallbacks_Responses");
					this.OnBulkFallbacks_ResponsesChanged();
				}
			}
		}
		
		[Column(Storage="_MillisecondsToImport", DbType="Int")]
		public System.Nullable<int> MillisecondsToImport
		{
			get
			{
				return this._MillisecondsToImport;
			}
			set
			{
				if ((this._MillisecondsToImport != value))
				{
					this.OnMillisecondsToImportChanging(value);
					this.SendPropertyChanging();
					this._MillisecondsToImport = value;
					this.SendPropertyChanged("MillisecondsToImport");
					this.OnMillisecondsToImportChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.tblSwipes")]
	public partial class tblSwipe : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _SwipeID;
		
		private int _PersonEventID;
		
		private int _AcctID;
		
		private int _EventID;
		
		private System.DateTime _SwipeDate;
		
		private int _TerminalID;
		
		private string _Notes;
		
		private byte _DataSource;
		
		private System.Nullable<long> _VisitorRecordCRC;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSwipeIDChanging(int value);
    partial void OnSwipeIDChanged();
    partial void OnPersonEventIDChanging(int value);
    partial void OnPersonEventIDChanged();
    partial void OnAcctIDChanging(int value);
    partial void OnAcctIDChanged();
    partial void OnEventIDChanging(int value);
    partial void OnEventIDChanged();
    partial void OnSwipeDateChanging(System.DateTime value);
    partial void OnSwipeDateChanged();
    partial void OnTerminalIDChanging(int value);
    partial void OnTerminalIDChanged();
    partial void OnNotesChanging(string value);
    partial void OnNotesChanged();
    partial void OnDataSourceChanging(byte value);
    partial void OnDataSourceChanged();
    partial void OnVisitorRecordCRCChanging(System.Nullable<long> value);
    partial void OnVisitorRecordCRCChanged();
    #endregion
		
		public tblSwipe()
		{
			OnCreated();
		}
		
		[Column(Storage="_SwipeID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int SwipeID
		{
			get
			{
				return this._SwipeID;
			}
			set
			{
				if ((this._SwipeID != value))
				{
					this.OnSwipeIDChanging(value);
					this.SendPropertyChanging();
					this._SwipeID = value;
					this.SendPropertyChanged("SwipeID");
					this.OnSwipeIDChanged();
				}
			}
		}
		
		[Column(Storage="_PersonEventID", DbType="Int NOT NULL")]
		public int PersonEventID
		{
			get
			{
				return this._PersonEventID;
			}
			set
			{
				if ((this._PersonEventID != value))
				{
					this.OnPersonEventIDChanging(value);
					this.SendPropertyChanging();
					this._PersonEventID = value;
					this.SendPropertyChanged("PersonEventID");
					this.OnPersonEventIDChanged();
				}
			}
		}
		
		[Column(Storage="_AcctID", DbType="Int NOT NULL")]
		public int AcctID
		{
			get
			{
				return this._AcctID;
			}
			set
			{
				if ((this._AcctID != value))
				{
					this.OnAcctIDChanging(value);
					this.SendPropertyChanging();
					this._AcctID = value;
					this.SendPropertyChanged("AcctID");
					this.OnAcctIDChanged();
				}
			}
		}
		
		[Column(Storage="_EventID", DbType="Int NOT NULL")]
		public int EventID
		{
			get
			{
				return this._EventID;
			}
			set
			{
				if ((this._EventID != value))
				{
					this.OnEventIDChanging(value);
					this.SendPropertyChanging();
					this._EventID = value;
					this.SendPropertyChanged("EventID");
					this.OnEventIDChanged();
				}
			}
		}
		
		[Column(Storage="_SwipeDate", DbType="DateTime NOT NULL")]
		public System.DateTime SwipeDate
		{
			get
			{
				return this._SwipeDate;
			}
			set
			{
				if ((this._SwipeDate != value))
				{
					this.OnSwipeDateChanging(value);
					this.SendPropertyChanging();
					this._SwipeDate = value;
					this.SendPropertyChanged("SwipeDate");
					this.OnSwipeDateChanged();
				}
			}
		}
		
		[Column(Storage="_TerminalID", DbType="Int NOT NULL")]
		public int TerminalID
		{
			get
			{
				return this._TerminalID;
			}
			set
			{
				if ((this._TerminalID != value))
				{
					this.OnTerminalIDChanging(value);
					this.SendPropertyChanging();
					this._TerminalID = value;
					this.SendPropertyChanged("TerminalID");
					this.OnTerminalIDChanged();
				}
			}
		}
		
		[Column(Storage="_Notes", DbType="VarChar(MAX)")]
		public string Notes
		{
			get
			{
				return this._Notes;
			}
			set
			{
				if ((this._Notes != value))
				{
					this.OnNotesChanging(value);
					this.SendPropertyChanging();
					this._Notes = value;
					this.SendPropertyChanged("Notes");
					this.OnNotesChanged();
				}
			}
		}
		
		[Column(Storage="_DataSource", DbType="TinyInt NOT NULL")]
		public byte DataSource
		{
			get
			{
				return this._DataSource;
			}
			set
			{
				if ((this._DataSource != value))
				{
					this.OnDataSourceChanging(value);
					this.SendPropertyChanging();
					this._DataSource = value;
					this.SendPropertyChanged("DataSource");
					this.OnDataSourceChanged();
				}
			}
		}
		
		[Column(Storage="_VisitorRecordCRC", DbType="BigInt")]
		public System.Nullable<long> VisitorRecordCRC
		{
			get
			{
				return this._VisitorRecordCRC;
			}
			set
			{
				if ((this._VisitorRecordCRC != value))
				{
					this.OnVisitorRecordCRCChanging(value);
					this.SendPropertyChanging();
					this._VisitorRecordCRC = value;
					this.SendPropertyChanged("VisitorRecordCRC");
					this.OnVisitorRecordCRCChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.tblSwipesText")]
	public partial class tblSwipesText : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _PersonEventID;
		
		private string _FieldName;
		
		private string _FieldText;
		
		private System.Nullable<int> _SeqNo;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPersonEventIDChanging(int value);
    partial void OnPersonEventIDChanged();
    partial void OnFieldNameChanging(string value);
    partial void OnFieldNameChanged();
    partial void OnFieldTextChanging(string value);
    partial void OnFieldTextChanged();
    partial void OnSeqNoChanging(System.Nullable<int> value);
    partial void OnSeqNoChanged();
    #endregion
		
		public tblSwipesText()
		{
			OnCreated();
		}
		
		[Column(Storage="_PersonEventID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int PersonEventID
		{
			get
			{
				return this._PersonEventID;
			}
			set
			{
				if ((this._PersonEventID != value))
				{
					this.OnPersonEventIDChanging(value);
					this.SendPropertyChanging();
					this._PersonEventID = value;
					this.SendPropertyChanged("PersonEventID");
					this.OnPersonEventIDChanged();
				}
			}
		}
		
		[Column(Storage="_FieldName", DbType="VarChar(200) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string FieldName
		{
			get
			{
				return this._FieldName;
			}
			set
			{
				if ((this._FieldName != value))
				{
					this.OnFieldNameChanging(value);
					this.SendPropertyChanging();
					this._FieldName = value;
					this.SendPropertyChanged("FieldName");
					this.OnFieldNameChanged();
				}
			}
		}
		
		[Column(Storage="_FieldText", DbType="VarChar(500)")]
		public string FieldText
		{
			get
			{
				return this._FieldText;
			}
			set
			{
				if ((this._FieldText != value))
				{
					this.OnFieldTextChanging(value);
					this.SendPropertyChanging();
					this._FieldText = value;
					this.SendPropertyChanged("FieldText");
					this.OnFieldTextChanged();
				}
			}
		}
		
		[Column(Storage="_SeqNo", DbType="Int")]
		public System.Nullable<int> SeqNo
		{
			get
			{
				return this._SeqNo;
			}
			set
			{
				if ((this._SeqNo != value))
				{
					this.OnSeqNoChanging(value);
					this.SendPropertyChanging();
					this._SeqNo = value;
					this.SendPropertyChanged("SeqNo");
					this.OnSeqNoChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.tblTerminal")]
	public partial class tblTerminal : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _TerminalSerial;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnTerminalSerialChanging(string value);
    partial void OnTerminalSerialChanged();
    #endregion
		
		public tblTerminal()
		{
			OnCreated();
		}
		
		[Column(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[Column(Storage="_TerminalSerial", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string TerminalSerial
		{
			get
			{
				return this._TerminalSerial;
			}
			set
			{
				if ((this._TerminalSerial != value))
				{
					this.OnTerminalSerialChanging(value);
					this.SendPropertyChanging();
					this._TerminalSerial = value;
					this.SendPropertyChanged("TerminalSerial");
					this.OnTerminalSerialChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.tblPersonByEvent")]
	public partial class tblPersonByEvent : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _PersonEventID;
		
		private int _EventID;
		
		private long _Hashvalue;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPersonEventIDChanging(int value);
    partial void OnPersonEventIDChanged();
    partial void OnEventIDChanging(int value);
    partial void OnEventIDChanged();
    partial void OnHashvalueChanging(long value);
    partial void OnHashvalueChanged();
    #endregion
		
		public tblPersonByEvent()
		{
			OnCreated();
		}
		
		[Column(Storage="_PersonEventID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int PersonEventID
		{
			get
			{
				return this._PersonEventID;
			}
			set
			{
				if ((this._PersonEventID != value))
				{
					this.OnPersonEventIDChanging(value);
					this.SendPropertyChanging();
					this._PersonEventID = value;
					this.SendPropertyChanged("PersonEventID");
					this.OnPersonEventIDChanged();
				}
			}
		}
		
		[Column(Storage="_EventID", DbType="Int NOT NULL")]
		public int EventID
		{
			get
			{
				return this._EventID;
			}
			set
			{
				if ((this._EventID != value))
				{
					this.OnEventIDChanging(value);
					this.SendPropertyChanging();
					this._EventID = value;
					this.SendPropertyChanged("EventID");
					this.OnEventIDChanged();
				}
			}
		}
		
		[Column(Storage="_Hashvalue", DbType="BigInt NOT NULL")]
		public long Hashvalue
		{
			get
			{
				return this._Hashvalue;
			}
			set
			{
				if ((this._Hashvalue != value))
				{
					this.OnHashvalueChanging(value);
					this.SendPropertyChanging();
					this._Hashvalue = value;
					this.SendPropertyChanged("Hashvalue");
					this.OnHashvalueChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.tblImportTracking")]
	public partial class tblImportTracking
	{
		
		private int _SwipeID;
		
		private int _SeqNum;
		
		private int _EventID;
		
		private int _AcctID;
		
		private System.DateTime _ImportTimeStamp;
		
		public tblImportTracking()
		{
		}
		
		[Column(Storage="_SwipeID", DbType="Int NOT NULL")]
		public int SwipeID
		{
			get
			{
				return this._SwipeID;
			}
			set
			{
				if ((this._SwipeID != value))
				{
					this._SwipeID = value;
				}
			}
		}
		
		[Column(Storage="_SeqNum", DbType="Int NOT NULL")]
		public int SeqNum
		{
			get
			{
				return this._SeqNum;
			}
			set
			{
				if ((this._SeqNum != value))
				{
					this._SeqNum = value;
				}
			}
		}
		
		[Column(Storage="_EventID", DbType="Int NOT NULL")]
		public int EventID
		{
			get
			{
				return this._EventID;
			}
			set
			{
				if ((this._EventID != value))
				{
					this._EventID = value;
				}
			}
		}
		
		[Column(Storage="_AcctID", DbType="Int NOT NULL")]
		public int AcctID
		{
			get
			{
				return this._AcctID;
			}
			set
			{
				if ((this._AcctID != value))
				{
					this._AcctID = value;
				}
			}
		}
		
		[Column(Storage="_ImportTimeStamp", DbType="DateTime NOT NULL")]
		public System.DateTime ImportTimeStamp
		{
			get
			{
				return this._ImportTimeStamp;
			}
			set
			{
				if ((this._ImportTimeStamp != value))
				{
					this._ImportTimeStamp = value;
				}
			}
		}
	}
	
	[Table(Name="dbo.tblMapCfg")]
	public partial class tblMapCfg : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _MapCfgID;
		
		private string _MapCfgContents;
		
		private int _MapCfgCRC;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMapCfgIDChanging(int value);
    partial void OnMapCfgIDChanged();
    partial void OnMapCfgContentsChanging(string value);
    partial void OnMapCfgContentsChanged();
    partial void OnMapCfgCRCChanging(int value);
    partial void OnMapCfgCRCChanged();
    #endregion
		
		public tblMapCfg()
		{
			OnCreated();
		}
		
		[Column(Storage="_MapCfgID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int MapCfgID
		{
			get
			{
				return this._MapCfgID;
			}
			set
			{
				if ((this._MapCfgID != value))
				{
					this.OnMapCfgIDChanging(value);
					this.SendPropertyChanging();
					this._MapCfgID = value;
					this.SendPropertyChanged("MapCfgID");
					this.OnMapCfgIDChanged();
				}
			}
		}
		
		[Column(Storage="_MapCfgContents", DbType="VarChar(MAX) NOT NULL", CanBeNull=false)]
		public string MapCfgContents
		{
			get
			{
				return this._MapCfgContents;
			}
			set
			{
				if ((this._MapCfgContents != value))
				{
					this.OnMapCfgContentsChanging(value);
					this.SendPropertyChanging();
					this._MapCfgContents = value;
					this.SendPropertyChanged("MapCfgContents");
					this.OnMapCfgContentsChanged();
				}
			}
		}
		
		[Column(Storage="_MapCfgCRC", DbType="Int NOT NULL")]
		public int MapCfgCRC
		{
			get
			{
				return this._MapCfgCRC;
			}
			set
			{
				if ((this._MapCfgCRC != value))
				{
					this.OnMapCfgCRCChanging(value);
					this.SendPropertyChanging();
					this._MapCfgCRC = value;
					this.SendPropertyChanged("MapCfgCRC");
					this.OnMapCfgCRCChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
