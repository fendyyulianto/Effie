using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class CompanyCredit : Csla.BusinessBase<CompanyCredit>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _entryId = Guid.NewGuid();
        private int _no = 0;
        private string _contactType = string.Empty;
        private string _company = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private string _city = string.Empty;
        private string _postal = string.Empty;
        private string _country = string.Empty;
        private string _salutation = string.Empty;
        private string _fullname = string.Empty;
        private string _job = string.Empty;
        private string _contact = string.Empty;
        private string _email = string.Empty;
        private string _clientCompanyNetwork = string.Empty;
        private string _clientCompanyNetworkOthers = string.Empty;
        private string _network = string.Empty;
        private string _networkOthers = string.Empty;
        private string _holdingCompany = string.Empty;
        private string _holdingCompanyOthers = string.Empty;
        private string _companyType = string.Empty;
        private string _companyTypeOther = string.Empty;
        private string _status = string.Empty;
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
            set
            {
                CanWriteProperty("Id", true);
                if (value == null) value = Guid.Empty;
                if (!_id.Equals(value))
                {
                    _id = value;
                    PropertyHasChanged("Id");
                }
            }
        }

        public Guid EntryId
        {
            get
            {
                CanReadProperty("EntryId", true);
                return _entryId;
            }
            set
            {
                CanWriteProperty("EntryId", true);
                if (value == null) value = Guid.Empty;
                if (!_entryId.Equals(value))
                {
                    _entryId = value;
                    PropertyHasChanged("EntryId");
                }
            }
        }

        public int No
        {
            get
            {
                CanReadProperty("No", true);
                return _no;
            }
            set
            {
                CanWriteProperty("No", true);
                if (!_no.Equals(value))
                {
                    _no = value;
                    PropertyHasChanged("No");
                }
            }
        }

        public string ContactType
        {
            get
            {
                CanReadProperty("ContactType", true);
                return _contactType;
            }
            set
            {
                CanWriteProperty("ContactType", true);
                if (value == null) value = string.Empty;
                if (!_contactType.Equals(value))
                {
                    _contactType = value;
                    PropertyHasChanged("ContactType");
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

        public string Salutation
        {
            get
            {
                CanReadProperty("Salutation", true);
                return _salutation;
            }
            set
            {
                CanWriteProperty("Salutation", true);
                if (value == null) value = string.Empty;
                if (!_salutation.Equals(value))
                {
                    _salutation = value;
                    PropertyHasChanged("Salutation");
                }
            }
        }

        public string Fullname
        {
            get
            {
                CanReadProperty("Fullname", true);
                return _fullname;
            }
            set
            {
                CanWriteProperty("Fullname", true);
                if (value == null) value = string.Empty;
                if (!_fullname.Equals(value))
                {
                    _fullname = value;
                    PropertyHasChanged("Fullname");
                }
            }
        }

        public string Job
        {
            get
            {
                CanReadProperty("Job", true);
                return _job;
            }
            set
            {
                CanWriteProperty("Job", true);
                if (value == null) value = string.Empty;
                if (!_job.Equals(value))
                {
                    _job = value;
                    PropertyHasChanged("Job");
                }
            }
        }

        public string Contact
        {
            get
            {
                CanReadProperty("Contact", true);
                return _contact;
            }
            set
            {
                CanWriteProperty("Contact", true);
                if (value == null) value = string.Empty;
                if (!_contact.Equals(value))
                {
                    _contact = value;
                    PropertyHasChanged("Contact");
                }
            }
        }

        public string Email
        {
            get
            {
                CanReadProperty("Email", true);
                return _email;
            }
            set
            {
                CanWriteProperty("Email", true);
                if (value == null) value = string.Empty;
                if (!_email.Equals(value))
                {
                    _email = value;
                    PropertyHasChanged("Email");
                }
            }
        }

        public string ClientCompanyNetwork
        {
            get
            {
                CanReadProperty("ClientCompanyNetwork", true);
                return _clientCompanyNetwork;
            }
            set
            {
                CanWriteProperty("ClientCompanyNetwork", true);
                if (value == null) value = string.Empty;
                if (!_clientCompanyNetwork.Equals(value))
                {
                    _clientCompanyNetwork = value;
                    PropertyHasChanged("ClientCompanyNetwork");
                }
            }
        }

        public string ClientCompanyNetworkOthers
        {
            get
            {
                CanReadProperty("ClientCompanyNetworkOthers", true);
                return _clientCompanyNetworkOthers;
            }
            set
            {
                CanWriteProperty("ClientCompanyNetworkOthers", true);
                if (value == null) value = string.Empty;
                if (!_clientCompanyNetworkOthers.Equals(value))
                {
                    _clientCompanyNetworkOthers = value;
                    PropertyHasChanged("ClientCompanyNetworkOthers");
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

        public string Status
        {
            get
            {
                CanReadProperty("Status", true);
                return _status;
            }
            set
            {
                CanWriteProperty("Status", true);
                if (value == null) value = string.Empty;
                if (!_status.Equals(value))
                {
                    _status = value;
                    PropertyHasChanged("Status");
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
            // ContactType
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ContactType", 100));
            //
            // Company
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Company", 100));
            //
            // Address1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address1", 100));
            //
            // Address2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address2", 100));
            //
            // City
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("City", 100));
            //
            // Postal
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Postal", 10));
            //
            // Country
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Country", 100));
            //
            // Salutation
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Salutation", 10));
            //
            // Fullname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Fullname", 100));
            //
            // Job
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Job", 100));
            //
            // Contact
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Contact", 50));
            //
            // Email
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Email", 100));
            //
            // ClientCompanyNetwork
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ClientCompanyNetwork", 100));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ClientCompanyNetworkOthers", 100));
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
            //
            // CompanyType
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CompanyType", 100));
            //
            // CompanyTypeOther
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CompanyTypeOther", 100));
            //
            // Status
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Status", 3));
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
            //TODO: Define authorization rules in CompanyCredit
            //AuthorizationRules.AllowRead("Id", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("EntryId", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("No", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("ContactType", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Company", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Address1", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Address2", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("City", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Postal", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Country", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Salutation", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Fullname", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Job", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Contact", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Email", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("ClientCompanyNetwork", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("Network", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("HoldingCompany", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "CompanyCreditReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "CompanyCreditReadGroup");

            //AuthorizationRules.AllowWrite("No", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("ContactType", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Company", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Address1", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Address2", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("City", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Postal", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Country", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Salutation", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Fullname", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Job", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Contact", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Email", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("ClientCompanyNetwork", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Network", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("HoldingCompany", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "CompanyCreditWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "CompanyCreditWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in CompanyCredit
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyCreditViewGroup"))
            //    return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in CompanyCredit
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyCreditAddGroup"))
            //    return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in CompanyCredit
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyCreditEditGroup"))
            //    return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in CompanyCredit
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyCreditDeleteGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private CompanyCredit()
        { /* require use of factory method */ }

        private CompanyCredit(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static CompanyCredit NewCompanyCredit()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a CompanyCredit");
            return DataPortal.Create<CompanyCredit>();
        }

        public static CompanyCredit GetCompanyCredit(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a CompanyCredit");
            return DataPortal.Fetch<CompanyCredit>(new Criteria(id));
        }

        public static CompanyCredit GetCompanyCredit(SafeDataReader dr)
        {
            return new CompanyCredit(dr);
        }

        public static void DeleteCompanyCredit(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a CompanyCredit");
            DataPortal.Delete(new Criteria(id));
        }

        public override CompanyCredit Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a CompanyCredit");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a CompanyCredit");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a CompanyCredit");

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
            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "GetCompanyCredit";

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
            _entryId = dr.GetGuid("EntryId");
            _no = dr.GetInt32("No");
            _contactType = dr.GetString("ContactType");
            _company = dr.GetString("Company");
            _address1 = dr.GetString("Address1");
            _address2 = dr.GetString("Address2");
            _city = dr.GetString("City");
            _postal = dr.GetString("Postal");
            _country = dr.GetString("Country");
            _salutation = dr.GetString("Salutation");
            _fullname = dr.GetString("Fullname");
            _job = dr.GetString("Job");
            _contact = dr.GetString("Contact");
            _email = dr.GetString("Email");
            _clientCompanyNetwork = dr.GetString("ClientCompanyNetwork");
            _clientCompanyNetworkOthers = dr.GetString("ClientCompanyNetworkOthers");
            _network = dr.GetString("Network");
            _networkOthers = dr.GetString("NetworkOthers");
            _holdingCompany = dr.GetString("HoldingCompany");
            _holdingCompanyOthers = dr.GetString("HoldingCompanyOthers");
            _companyType = dr.GetString("CompanyType");
            _companyTypeOther = dr.GetString("CompanyTypeOther");
            _status = dr.GetString("Status");
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
            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "AddCompanyCredit";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

               // _id = (Guid)cm.Parameters["@NewId"].Value;
               // _entryId = (Guid)cm.Parameters["@NewEntryId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
           
                cm.Parameters.AddWithValue("@No", _no);                        
                cm.Parameters.AddWithValue("@ContactType", _contactType);           
                cm.Parameters.AddWithValue("@Company", _company);           
                cm.Parameters.AddWithValue("@Address1", _address1);           
                cm.Parameters.AddWithValue("@Address2", _address2);            
                cm.Parameters.AddWithValue("@City", _city);           
                cm.Parameters.AddWithValue("@Postal", _postal);           
                cm.Parameters.AddWithValue("@Country", _country);            
                cm.Parameters.AddWithValue("@Salutation", _salutation);           
                cm.Parameters.AddWithValue("@Fullname", _fullname);           
                cm.Parameters.AddWithValue("@Job", _job);            
                cm.Parameters.AddWithValue("@Contact", _contact);            
                cm.Parameters.AddWithValue("@Email", _email);           
                cm.Parameters.AddWithValue("@ClientCompanyNetwork", _clientCompanyNetwork);
                cm.Parameters.AddWithValue("@ClientCompanyNetworkOthers", _clientCompanyNetworkOthers);          
                cm.Parameters.AddWithValue("@Network", _network);
                cm.Parameters.AddWithValue("@NetworkOthers", _networkOthers);     
                cm.Parameters.AddWithValue("@HoldingCompany", _holdingCompany);
                cm.Parameters.AddWithValue("@HoldingCompanyOthers", _holdingCompanyOthers);
                cm.Parameters.AddWithValue("@CompanyType", _companyType);
                cm.Parameters.AddWithValue("@CompanyTypeOther", _companyTypeOther);
                cm.Parameters.AddWithValue("@Status", _status);
                cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
                cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
                cm.Parameters.AddWithValue("@Id", _id);
                // cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
                cm.Parameters.AddWithValue("@EntryId", _entryId);
                // cm.Parameters["@NewEntryId"].Direction = ParameterDirection.Output;
        }
        #endregion //Data Access - Insert

        #region Data Access - Update
        protected override void DataPortal_Update()
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "UpdateCompanyCredit";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@EntryId", _entryId);            
            cm.Parameters.AddWithValue("@No", _no);            
            cm.Parameters.AddWithValue("@ContactType", _contactType);            
            cm.Parameters.AddWithValue("@Company", _company);           
            cm.Parameters.AddWithValue("@Address1", _address1);           
            cm.Parameters.AddWithValue("@Address2", _address2);           
            cm.Parameters.AddWithValue("@City", _city);           
            cm.Parameters.AddWithValue("@Postal", _postal);           
            cm.Parameters.AddWithValue("@Country", _country);          
            cm.Parameters.AddWithValue("@Salutation", _salutation);           
            cm.Parameters.AddWithValue("@Fullname", _fullname);            
            cm.Parameters.AddWithValue("@Job", _job);            
            cm.Parameters.AddWithValue("@Contact", _contact);           
            cm.Parameters.AddWithValue("@Email", _email);           
            cm.Parameters.AddWithValue("@ClientCompanyNetwork", _clientCompanyNetwork);
            cm.Parameters.AddWithValue("@ClientCompanyNetworkOthers", _clientCompanyNetworkOthers);           
            cm.Parameters.AddWithValue("@Network", _network);
            cm.Parameters.AddWithValue("@NetworkOthers", _networkOthers);
            cm.Parameters.AddWithValue("@HoldingCompany", _holdingCompany);
            cm.Parameters.AddWithValue("@HoldingCompanyOthers", _holdingCompanyOthers);
            cm.Parameters.AddWithValue("@CompanyType", _companyType);
            cm.Parameters.AddWithValue("@CompanyTypeOther", _companyTypeOther);
            cm.Parameters.AddWithValue("@Status", _status);  
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
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
            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "DeleteCompanyCredit";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}