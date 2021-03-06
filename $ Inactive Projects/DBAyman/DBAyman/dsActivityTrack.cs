//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace DBAyman {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class dsActivityTrack : DataSet {
        
        private tblPersonDataTable tabletblPerson;
        
        public dsActivityTrack() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected dsActivityTrack(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["tblPerson"] != null)) {
                    this.Tables.Add(new tblPersonDataTable(ds.Tables["tblPerson"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public tblPersonDataTable tblPerson {
            get {
                return this.tabletblPerson;
            }
        }
        
        public override DataSet Clone() {
            dsActivityTrack cln = ((dsActivityTrack)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["tblPerson"] != null)) {
                this.Tables.Add(new tblPersonDataTable(ds.Tables["tblPerson"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        internal void InitVars() {
            this.tabletblPerson = ((tblPersonDataTable)(this.Tables["tblPerson"]));
            if ((this.tabletblPerson != null)) {
                this.tabletblPerson.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "dsActivityTrack";
            this.Prefix = "";
            this.Namespace = "http://www.tempuri.org/dsActivityTrack.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tabletblPerson = new tblPersonDataTable();
            this.Tables.Add(this.tabletblPerson);
        }
        
        private bool ShouldSerializetblPerson() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void tblPersonRowChangeEventHandler(object sender, tblPersonRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class tblPersonDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnAddress;
            
            private DataColumn columnAddress2;
            
            private DataColumn columnCity;
            
            private DataColumn columnCounty;
            
            private DataColumn columnDateEntered;
            
            private DataColumn columnDOB;
            
            private DataColumn columnFName;
            
            private DataColumn columnHomePhone;
            
            private DataColumn columnHomeSiteID;
            
            private DataColumn columnLName;
            
            private DataColumn columnMidName;
            
            private DataColumn columnPhone;
            
            private DataColumn columnPubAssistance;
            
            private DataColumn columnSSN;
            
            private DataColumn columnSSNId;
            
            private DataColumn columnState;
            
            private DataColumn columnVeteran;
            
            private DataColumn columnZip;
            
            internal tblPersonDataTable() : 
                    base("tblPerson") {
                this.InitClass();
            }
            
            internal tblPersonDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            internal DataColumn AddressColumn {
                get {
                    return this.columnAddress;
                }
            }
            
            internal DataColumn Address2Column {
                get {
                    return this.columnAddress2;
                }
            }
            
            internal DataColumn CityColumn {
                get {
                    return this.columnCity;
                }
            }
            
            internal DataColumn CountyColumn {
                get {
                    return this.columnCounty;
                }
            }
            
            internal DataColumn DateEnteredColumn {
                get {
                    return this.columnDateEntered;
                }
            }
            
            internal DataColumn DOBColumn {
                get {
                    return this.columnDOB;
                }
            }
            
            internal DataColumn FNameColumn {
                get {
                    return this.columnFName;
                }
            }
            
            internal DataColumn HomePhoneColumn {
                get {
                    return this.columnHomePhone;
                }
            }
            
            internal DataColumn HomeSiteIDColumn {
                get {
                    return this.columnHomeSiteID;
                }
            }
            
            internal DataColumn LNameColumn {
                get {
                    return this.columnLName;
                }
            }
            
            internal DataColumn MidNameColumn {
                get {
                    return this.columnMidName;
                }
            }
            
            internal DataColumn PhoneColumn {
                get {
                    return this.columnPhone;
                }
            }
            
            internal DataColumn PubAssistanceColumn {
                get {
                    return this.columnPubAssistance;
                }
            }
            
            internal DataColumn SSNColumn {
                get {
                    return this.columnSSN;
                }
            }
            
            internal DataColumn SSNIdColumn {
                get {
                    return this.columnSSNId;
                }
            }
            
            internal DataColumn StateColumn {
                get {
                    return this.columnState;
                }
            }
            
            internal DataColumn VeteranColumn {
                get {
                    return this.columnVeteran;
                }
            }
            
            internal DataColumn ZipColumn {
                get {
                    return this.columnZip;
                }
            }
            
            public tblPersonRow this[int index] {
                get {
                    return ((tblPersonRow)(this.Rows[index]));
                }
            }
            
            public event tblPersonRowChangeEventHandler tblPersonRowChanged;
            
            public event tblPersonRowChangeEventHandler tblPersonRowChanging;
            
            public event tblPersonRowChangeEventHandler tblPersonRowDeleted;
            
            public event tblPersonRowChangeEventHandler tblPersonRowDeleting;
            
            public void AddtblPersonRow(tblPersonRow row) {
                this.Rows.Add(row);
            }
            
            public tblPersonRow AddtblPersonRow(
                        string Address, 
                        string Address2, 
                        string City, 
                        string County, 
                        System.DateTime DateEntered, 
                        System.DateTime DOB, 
                        string FName, 
                        string HomePhone, 
                        string HomeSiteID, 
                        string LName, 
                        string MidName, 
                        string Phone, 
                        bool PubAssistance, 
                        string SSN, 
                        string State, 
                        bool Veteran, 
                        string Zip) {
                tblPersonRow rowtblPersonRow = ((tblPersonRow)(this.NewRow()));
                rowtblPersonRow.ItemArray = new object[] {
                        Address,
                        Address2,
                        City,
                        County,
                        DateEntered,
                        DOB,
                        FName,
                        HomePhone,
                        HomeSiteID,
                        LName,
                        MidName,
                        Phone,
                        PubAssistance,
                        SSN,
                        null,
                        State,
                        Veteran,
                        Zip};
                this.Rows.Add(rowtblPersonRow);
                return rowtblPersonRow;
            }
            
            public tblPersonRow FindBySSN(string SSN) {
                return ((tblPersonRow)(this.Rows.Find(new object[] {
                            SSN})));
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                tblPersonDataTable cln = ((tblPersonDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new tblPersonDataTable();
            }
            
            internal void InitVars() {
                this.columnAddress = this.Columns["Address"];
                this.columnAddress2 = this.Columns["Address2"];
                this.columnCity = this.Columns["City"];
                this.columnCounty = this.Columns["County"];
                this.columnDateEntered = this.Columns["DateEntered"];
                this.columnDOB = this.Columns["DOB"];
                this.columnFName = this.Columns["FName"];
                this.columnHomePhone = this.Columns["HomePhone"];
                this.columnHomeSiteID = this.Columns["HomeSiteID"];
                this.columnLName = this.Columns["LName"];
                this.columnMidName = this.Columns["MidName"];
                this.columnPhone = this.Columns["Phone"];
                this.columnPubAssistance = this.Columns["PubAssistance"];
                this.columnSSN = this.Columns["SSN"];
                this.columnSSNId = this.Columns["SSNId"];
                this.columnState = this.Columns["State"];
                this.columnVeteran = this.Columns["Veteran"];
                this.columnZip = this.Columns["Zip"];
            }
            
            private void InitClass() {
                this.columnAddress = new DataColumn("Address", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnAddress);
                this.columnAddress2 = new DataColumn("Address2", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnAddress2);
                this.columnCity = new DataColumn("City", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCity);
                this.columnCounty = new DataColumn("County", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCounty);
                this.columnDateEntered = new DataColumn("DateEntered", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateEntered);
                this.columnDOB = new DataColumn("DOB", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDOB);
                this.columnFName = new DataColumn("FName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnFName);
                this.columnHomePhone = new DataColumn("HomePhone", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnHomePhone);
                this.columnHomeSiteID = new DataColumn("HomeSiteID", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnHomeSiteID);
                this.columnLName = new DataColumn("LName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnLName);
                this.columnMidName = new DataColumn("MidName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnMidName);
                this.columnPhone = new DataColumn("Phone", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPhone);
                this.columnPubAssistance = new DataColumn("PubAssistance", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPubAssistance);
                this.columnSSN = new DataColumn("SSN", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnSSN);
                this.columnSSNId = new DataColumn("SSNId", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnSSNId);
                this.columnState = new DataColumn("State", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnState);
                this.columnVeteran = new DataColumn("Veteran", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnVeteran);
                this.columnZip = new DataColumn("Zip", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnZip);
                this.Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[] {
                                this.columnSSN}, true));
                this.columnSSN.AllowDBNull = false;
                this.columnSSN.Unique = true;
                this.columnSSNId.AutoIncrement = true;
            }
            
            public tblPersonRow NewtblPersonRow() {
                return ((tblPersonRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new tblPersonRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(tblPersonRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.tblPersonRowChanged != null)) {
                    this.tblPersonRowChanged(this, new tblPersonRowChangeEvent(((tblPersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.tblPersonRowChanging != null)) {
                    this.tblPersonRowChanging(this, new tblPersonRowChangeEvent(((tblPersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.tblPersonRowDeleted != null)) {
                    this.tblPersonRowDeleted(this, new tblPersonRowChangeEvent(((tblPersonRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.tblPersonRowDeleting != null)) {
                    this.tblPersonRowDeleting(this, new tblPersonRowChangeEvent(((tblPersonRow)(e.Row)), e.Action));
                }
            }
            
            public void RemovetblPersonRow(tblPersonRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class tblPersonRow : DataRow {
            
            private tblPersonDataTable tabletblPerson;
            
            internal tblPersonRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tabletblPerson = ((tblPersonDataTable)(this.Table));
            }
            
            public string Address {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.AddressColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.AddressColumn] = value;
                }
            }
            
            public string Address2 {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.Address2Column]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.Address2Column] = value;
                }
            }
            
            public string City {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.CityColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.CityColumn] = value;
                }
            }
            
            public string County {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.CountyColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.CountyColumn] = value;
                }
            }
            
            public System.DateTime DateEntered {
                get {
                    try {
                        return ((System.DateTime)(this[this.tabletblPerson.DateEnteredColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.DateEnteredColumn] = value;
                }
            }
            
            public System.DateTime DOB {
                get {
                    try {
                        return ((System.DateTime)(this[this.tabletblPerson.DOBColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.DOBColumn] = value;
                }
            }
            
            public string FName {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.FNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.FNameColumn] = value;
                }
            }
            
            public string HomePhone {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.HomePhoneColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.HomePhoneColumn] = value;
                }
            }
            
            public string HomeSiteID {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.HomeSiteIDColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.HomeSiteIDColumn] = value;
                }
            }
            
            public string LName {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.LNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.LNameColumn] = value;
                }
            }
            
            public string MidName {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.MidNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.MidNameColumn] = value;
                }
            }
            
            public string Phone {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.PhoneColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.PhoneColumn] = value;
                }
            }
            
            public bool PubAssistance {
                get {
                    try {
                        return ((bool)(this[this.tabletblPerson.PubAssistanceColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.PubAssistanceColumn] = value;
                }
            }
            
            public string SSN {
                get {
                    return ((string)(this[this.tabletblPerson.SSNColumn]));
                }
                set {
                    this[this.tabletblPerson.SSNColumn] = value;
                }
            }
            
            public int SSNId {
                get {
                    try {
                        return ((int)(this[this.tabletblPerson.SSNIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.SSNIdColumn] = value;
                }
            }
            
            public string State {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.StateColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.StateColumn] = value;
                }
            }
            
            public bool Veteran {
                get {
                    try {
                        return ((bool)(this[this.tabletblPerson.VeteranColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.VeteranColumn] = value;
                }
            }
            
            public string Zip {
                get {
                    try {
                        return ((string)(this[this.tabletblPerson.ZipColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Cannot get value because it is DBNull.", e);
                    }
                }
                set {
                    this[this.tabletblPerson.ZipColumn] = value;
                }
            }
            
            public bool IsAddressNull() {
                return this.IsNull(this.tabletblPerson.AddressColumn);
            }
            
            public void SetAddressNull() {
                this[this.tabletblPerson.AddressColumn] = System.Convert.DBNull;
            }
            
            public bool IsAddress2Null() {
                return this.IsNull(this.tabletblPerson.Address2Column);
            }
            
            public void SetAddress2Null() {
                this[this.tabletblPerson.Address2Column] = System.Convert.DBNull;
            }
            
            public bool IsCityNull() {
                return this.IsNull(this.tabletblPerson.CityColumn);
            }
            
            public void SetCityNull() {
                this[this.tabletblPerson.CityColumn] = System.Convert.DBNull;
            }
            
            public bool IsCountyNull() {
                return this.IsNull(this.tabletblPerson.CountyColumn);
            }
            
            public void SetCountyNull() {
                this[this.tabletblPerson.CountyColumn] = System.Convert.DBNull;
            }
            
            public bool IsDateEnteredNull() {
                return this.IsNull(this.tabletblPerson.DateEnteredColumn);
            }
            
            public void SetDateEnteredNull() {
                this[this.tabletblPerson.DateEnteredColumn] = System.Convert.DBNull;
            }
            
            public bool IsDOBNull() {
                return this.IsNull(this.tabletblPerson.DOBColumn);
            }
            
            public void SetDOBNull() {
                this[this.tabletblPerson.DOBColumn] = System.Convert.DBNull;
            }
            
            public bool IsFNameNull() {
                return this.IsNull(this.tabletblPerson.FNameColumn);
            }
            
            public void SetFNameNull() {
                this[this.tabletblPerson.FNameColumn] = System.Convert.DBNull;
            }
            
            public bool IsHomePhoneNull() {
                return this.IsNull(this.tabletblPerson.HomePhoneColumn);
            }
            
            public void SetHomePhoneNull() {
                this[this.tabletblPerson.HomePhoneColumn] = System.Convert.DBNull;
            }
            
            public bool IsHomeSiteIDNull() {
                return this.IsNull(this.tabletblPerson.HomeSiteIDColumn);
            }
            
            public void SetHomeSiteIDNull() {
                this[this.tabletblPerson.HomeSiteIDColumn] = System.Convert.DBNull;
            }
            
            public bool IsLNameNull() {
                return this.IsNull(this.tabletblPerson.LNameColumn);
            }
            
            public void SetLNameNull() {
                this[this.tabletblPerson.LNameColumn] = System.Convert.DBNull;
            }
            
            public bool IsMidNameNull() {
                return this.IsNull(this.tabletblPerson.MidNameColumn);
            }
            
            public void SetMidNameNull() {
                this[this.tabletblPerson.MidNameColumn] = System.Convert.DBNull;
            }
            
            public bool IsPhoneNull() {
                return this.IsNull(this.tabletblPerson.PhoneColumn);
            }
            
            public void SetPhoneNull() {
                this[this.tabletblPerson.PhoneColumn] = System.Convert.DBNull;
            }
            
            public bool IsPubAssistanceNull() {
                return this.IsNull(this.tabletblPerson.PubAssistanceColumn);
            }
            
            public void SetPubAssistanceNull() {
                this[this.tabletblPerson.PubAssistanceColumn] = System.Convert.DBNull;
            }
            
            public bool IsSSNIdNull() {
                return this.IsNull(this.tabletblPerson.SSNIdColumn);
            }
            
            public void SetSSNIdNull() {
                this[this.tabletblPerson.SSNIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsStateNull() {
                return this.IsNull(this.tabletblPerson.StateColumn);
            }
            
            public void SetStateNull() {
                this[this.tabletblPerson.StateColumn] = System.Convert.DBNull;
            }
            
            public bool IsVeteranNull() {
                return this.IsNull(this.tabletblPerson.VeteranColumn);
            }
            
            public void SetVeteranNull() {
                this[this.tabletblPerson.VeteranColumn] = System.Convert.DBNull;
            }
            
            public bool IsZipNull() {
                return this.IsNull(this.tabletblPerson.ZipColumn);
            }
            
            public void SetZipNull() {
                this[this.tabletblPerson.ZipColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class tblPersonRowChangeEvent : EventArgs {
            
            private tblPersonRow eventRow;
            
            private DataRowAction eventAction;
            
            public tblPersonRowChangeEvent(tblPersonRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public tblPersonRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
