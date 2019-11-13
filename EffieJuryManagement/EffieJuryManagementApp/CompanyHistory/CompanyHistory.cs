using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class CompanyHistory : Csla.BusinessBase<CompanyHistory>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _juryId = Guid.Empty;
        private string _type = string.Empty;
        private string _designation = string.Empty;
        private string _company = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private string _city = string.Empty;
        private string _postal = string.Empty;
        private string _country = string.Empty;
        private string _companyType = string.Empty;
        private string _companyTypeOther = string.Empty;
        private string _network = string.Empty;
        private string _networkOthers = string.Empty;
        private string _holdingCompany = string.Empty;
        private string _holdingCompanyOthers = string.Empty;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public Guid JuryId
        {
            get
            {
                CanReadProperty("JuryId", true);
                return _juryId;
            }
            set
            {
                CanWriteProperty("JuryId", true);
                if (value == null) value = Guid.Empty;
                if (!_juryId.Equals(value))
                {
                    _juryId = value;
                    PropertyHasChanged("JuryId");
                }
            }
        }

        public string Type
        {
            get
            {
                CanReadProperty("Type", true);
                return _type;
            }
            set
            {
                CanWriteProperty("Type", true);
                if (value == null) value = string.Empty;
                if (!_type.Equals(value))
                {
                    _type = value;
                    PropertyHasChanged("Type");
                }
            }
        }

        public string Designation
        {
            get
            {
                CanReadProperty("Designation", true);
                return _designation;
            }
            set
            {
                CanWriteProperty("Designation", true);
                if (value == null) value = string.Empty;
                if (!_designation.Equals(value))
                {
                    _designation = value;
                    PropertyHasChanged("Designation");
                }
            }
        }

        public string Company
        {
            get
            {
                CanReadProperty("Company", true);
                return _company;
            }
            set
            {
                CanWriteProperty("Company", true);
                if (value == null) value = string.Empty;
                if (!_company.Equals(value))
                {
                    _company = value;
                    PropertyHasChanged("Company");
                }
            }
        }

        public string Address1
        {
            get
            {
                CanReadProperty("Address1", true);
                return _address1;
            }
            set
            {
                CanWriteProperty("Address1", true);
                if (value == null) value = string.Empty;
                if (!_address1.Equals(value))
                {
                    _address1 = value;
                    PropertyHasChanged("Address1");
                }
            }
        }

        public string Address2
        {
            get
            {
                CanReadProperty("Address2", true);
                return _address2;
            }
            set
            {
                CanWriteProperty("Address2", true);
                if (value == null) value = string.Empty;
                if (!_address2.Equals(value))
                {
                    _address2 = value;
                    PropertyHasChanged("Address2");
                }
            }
        }

        public string City
        {
            get
            {
                CanReadProperty("City", true);
                return _city;
            }
            set
            {
                CanWriteProperty("City", true);
                if (value == null) value = string.Empty;
                if (!_city.Equals(value))
                {
                    _city = value;
                    PropertyHasChanged("City");
                }
            }
        }

        public string Postal
        {
            get
            {
                CanReadProperty("Postal", true);
                return _postal;
            }
            set
            {
                CanWriteProperty("Postal", true);
                if (value == null) value = string.Empty;
                if (!_postal.Equals(value))
                {
                    _postal = value;
                    PropertyHasChanged("Postal");
                }
            }
        }

        public string Country
        {
            get
            {
                CanReadProperty("Country", true);
                return _country;
            }
            set
            {
                CanWriteProperty("Country", true);
                if (value == null) value = string.Empty;
                if (!_country.Equals(value))
                {
                    _country = value;
                    PropertyHasChanged("Country");
                }
            }
        }

        public string CompanyType
        {
            get
            {
                CanReadProperty("CompanyType", true);
                return _companyType;
            }
            set
            {
                CanWriteProperty("CompanyType", true);
                if (value == null) value = string.Empty;
                if (!_companyType.Equals(value))
                {
                    _companyType = value;
                    PropertyHasChanged("CompanyType");
                }
            }
        }

        public string CompanyTypeOther
        {
            get
            {
                CanReadProperty("CompanyTypeOther", true);
                return _companyTypeOther;
            }
            set
            {
                CanWriteProperty("CompanyTypeOther", true);
                if (value == null) value = string.Empty;
                if (!_companyTypeOther.Equals(value))
                {
                    _companyTypeOther = value;
                    PropertyHasChanged("CompanyTypeOther");
                }
            }
        }

        public string Network
        {
            get
            {
                CanReadProperty("Network", true);
                return _network;
            }
            set
            {
                CanWriteProperty("Network", true);
                if (value == null) value = string.Empty;
                if (!_network.Equals(value))
                {
                    _network = value;
                    PropertyHasChanged("Network");
                }
            }
        }

        public string NetworkOthers
        {
            get
            {
                CanReadProperty("NetworkOthers", true);
                return _networkOthers;
            }
            set
            {
                CanWriteProperty("NetworkOthers", true);
                if (value == null) value = string.Empty;
                if (!_networkOthers.Equals(value))
                {
                    _networkOthers = value;
                    PropertyHasChanged("NetworkOthers");
                }
            }
        }

        public string HoldingCompany
        {
            get
            {
                CanReadProperty("HoldingCompany", true);
                return _holdingCompany;
            }
            set
            {
                CanWriteProperty("HoldingCompany", true);
                if (value == null) value = string.Empty;
                if (!_holdingCompany.Equals(value))
                {
                    _holdingCompany = value;
                    PropertyHasChanged("HoldingCompany");
                }
            }
        }

        public string HoldingCompanyOthers
        {
            get
            {
                CanReadProperty("HoldingCompanyOthers", true);
                return _holdingCompanyOthers;
            }
            set
            {
                CanWriteProperty("HoldingCompanyOthers", true);
                if (value == null) value = string.Empty;
                if (!_holdingCompanyOthers.Equals(value))
                {
                    _holdingCompanyOthers = value;
                    PropertyHasChanged("HoldingCompanyOthers");
                }
            }
        }

        public DateTime DateCreated
        {
            get
            {
                CanReadProperty("DateCreated", true);
                return _dateCreated.Date;
            }
        }

        public string DateCreatedString
        {
            get
            {
                CanReadProperty("DateCreated", true);
                return _dateCreated.Text;
            }
            set
            {
                CanWriteProperty("DateCreated", true);
                if (value == null) value = string.Empty;
                if (!_dateCreated.Equals(value))
                {
                    _dateCreated.Text = value;
                    PropertyHasChanged("DateCreated");
                }
            }
        }

        public DateTime DateModified
        {
            get
            {
                CanReadProperty("DateModified", true);
                return _dateModified.Date;
            }
        }

        public string DateModifiedString
        {
            get
            {
                CanReadProperty("DateModified", true);
                return _dateModified.Text;
            }
            set
            {
                CanWriteProperty("DateModified", true);
                if (value == null) value = string.Empty;
                if (!_dateModified.Equals(value))
                {
                    _dateModified.Text = value;
                    PropertyHasChanged("DateModified");
                }
            }
        }

        protected override object GetIdValue()
        {
            return _id;
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            //
            // Type
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Type", 100));
            //
            // Designation
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Designation", 100));
            //
            // Company
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Company", 200));
            //
            // Address1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address1", 200));
            //
            // Address2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address2", 200));
            //
            // City
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("City", 100));
            //
            // Postal
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Postal", 50));
            //
            // Country
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Country", 100));
            //
            // CompanyType
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CompanyType", 100));
            //
            // CompanyTypeOther
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CompanyTypeOther", 100));
            //
            // Network
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Network", 100));
            //
            // NetworkOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("NetworkOthers", 100));
            //
            // HoldingCompany
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HoldingCompany", 100));
            //
            // HoldingCompanyOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HoldingCompanyOthers", 100));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Authorization Rules
        protected override void AddAuthorizationRules()
        {
            //TODO: Define authorization rules in CompanyHistory
            //AuthorizationRules.AllowRead("Id", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("JuryId", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("Type", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("Designation", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("Company", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("Address1", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("Address2", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("City", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("Postal", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("Country", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("CompanyType", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("CompanyTypeOther", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("Network", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("NetworkOthers", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("HoldingCompany", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("HoldingCompanyOthers", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "CompanyHistoryReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "CompanyHistoryReadGroup");

            //AuthorizationRules.AllowWrite("Type", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("Designation", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("Company", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("Address1", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("Address2", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("City", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("Postal", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("Country", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("CompanyType", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("CompanyTypeOther", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("Network", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("NetworkOthers", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("HoldingCompany", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("HoldingCompanyOthers", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "CompanyHistoryWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "CompanyHistoryWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in CompanyHistory
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyHistoryViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in CompanyHistory
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyHistoryAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in CompanyHistory
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyHistoryEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in CompanyHistory
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyHistoryDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private CompanyHistory()
        { /* require use of factory method */ }

        private CompanyHistory(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static CompanyHistory NewCompanyHistory()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a CompanyHistory");
            return DataPortal.Create<CompanyHistory>();
        }

        public static CompanyHistory GetCompanyHistory(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a CompanyHistory");
            return DataPortal.Fetch<CompanyHistory>(new Criteria(id));
        }

        public static CompanyHistory GetCompanyHistory(SafeDataReader dr)
        {
            return new CompanyHistory(dr);
        }

        public static void DeleteCompanyHistory(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a CompanyHistory");
            DataPortal.Delete(new Criteria(id));
        }

        public override CompanyHistory Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a CompanyHistory");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a CompanyHistory");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a CompanyHistory");

            return base.Save();
        }

        #endregion //Factory Methods

        #region Data Access

        #region Criteria

        [Serializable()]
        private class Criteria
        {
            public Guid Id;

            public Criteria(Guid id)
            {
                this.Id = id;
            }
        }

        #endregion //Criteria

        #region Data Access - Create
        [RunLocal]
        protected override void DataPortal_Create(object criteria)
        {
            ValidationRules.CheckRules();
        }

        #endregion //Data Access - Create

        #region Data Access - Fetch
        private void DataPortal_Fetch(Criteria criteria)
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
            {
                cn.Open();

                ExecuteFetch(cn, criteria);
            }//using
        }

        private void ExecuteFetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "GetCompanyHistory";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    dr.Read();
                    FetchObject(dr);
                    ValidationRules.CheckRules();

                    //load child object(s)
                    FetchChildren(dr);
                }
            }//using
        }

        private void FetchObject(SafeDataReader dr)
        {
            _id = dr.GetGuid("Id");
            _juryId = dr.GetGuid("JuryId");
            _type = dr.GetString("Type");
            _designation = dr.GetString("Designation");
            _company = dr.GetString("Company");
            _address1 = dr.GetString("Address1");
            _address2 = dr.GetString("Address2");
            _city = dr.GetString("City");
            _postal = dr.GetString("Postal");
            _country = dr.GetString("Country");
            _companyType = dr.GetString("CompanyType");
            _companyTypeOther = dr.GetString("CompanyTypeOther");
            _network = dr.GetString("Network");
            _networkOthers = dr.GetString("NetworkOthers");
            _holdingCompany = dr.GetString("HoldingCompany");
            _holdingCompanyOthers = dr.GetString("HoldingCompanyOthers");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
        }

        private void FetchChildren(SafeDataReader dr)
        {
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert
        protected override void DataPortal_Insert()
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
            {
                cn.Open();

                ExecuteInsert(cn);

                //update child object(s)
                UpdateChildren(cn);
            }//using

        }

        private void ExecuteInsert(SqlConnection cn)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "AddCompanyHistory";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
                _juryId = (Guid)cm.Parameters["@JuryId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Type", _type);
            cm.Parameters.AddWithValue("@Designation", _designation);
            cm.Parameters.AddWithValue("@Company", _company);
            cm.Parameters.AddWithValue("@Address1", _address1);
            cm.Parameters.AddWithValue("@Address2", _address2);
            cm.Parameters.AddWithValue("@City", _city);
            cm.Parameters.AddWithValue("@Postal", _postal);
            cm.Parameters.AddWithValue("@Country", _country);
            cm.Parameters.AddWithValue("@CompanyType", _companyType);
            cm.Parameters.AddWithValue("@CompanyTypeOther", _companyTypeOther);
            cm.Parameters.AddWithValue("@Network", _network);
            cm.Parameters.AddWithValue("@NetworkOthers", _networkOthers);
            cm.Parameters.AddWithValue("@HoldingCompany", _holdingCompany);
            cm.Parameters.AddWithValue("@HoldingCompanyOthers", _holdingCompanyOthers);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@JuryId", _juryId);

        }
        #endregion //Data Access - Insert

        #region Data Access - Update
        protected override void DataPortal_Update()
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
            {
                cn.Open();

                if (base.IsDirty)
                {
                    ExecuteUpdate(cn);
                }

                //update child object(s)
                UpdateChildren(cn);
            }//using

        }

        private void ExecuteUpdate(SqlConnection cn)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "UpdateCompanyHistory";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Type", _type);
            cm.Parameters.AddWithValue("@Designation", _designation);
            cm.Parameters.AddWithValue("@Company", _company);
            cm.Parameters.AddWithValue("@Address1", _address1);
            cm.Parameters.AddWithValue("@Address2", _address2);
            cm.Parameters.AddWithValue("@City", _city);
            cm.Parameters.AddWithValue("@Postal", _postal);
            cm.Parameters.AddWithValue("@Country", _country);
            cm.Parameters.AddWithValue("@CompanyType", _companyType);
            cm.Parameters.AddWithValue("@CompanyTypeOther", _companyTypeOther);
            cm.Parameters.AddWithValue("@Network", _network);
            cm.Parameters.AddWithValue("@NetworkOthers", _networkOthers);
            cm.Parameters.AddWithValue("@HoldingCompany", _holdingCompany);
            cm.Parameters.AddWithValue("@HoldingCompanyOthers", _holdingCompanyOthers);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@JuryId", _juryId);
        }

        private void UpdateChildren(SqlConnection cn)
        {
        }
        #endregion //Data Access - Update

        #region Data Access - Delete
        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(new Criteria(_id));
        }

        private void DataPortal_Delete(Criteria criteria)
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
            {
                cn.Open();

                ExecuteDelete(cn, criteria);

            }//using

        }

        private void ExecuteDelete(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "DeleteCompanyHistory";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
