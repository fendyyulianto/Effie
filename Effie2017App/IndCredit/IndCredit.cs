using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class IndCredit : Csla.BusinessBase<IndCredit>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _entryId = Guid.NewGuid();
        private int _no = 0;
        private string _contactName = string.Empty;
        private string _title = string.Empty;
        private string _email = string.Empty;
        private string _company = string.Empty;
        private string _userData1 = string.Empty;
        private string _userData2 = string.Empty;
        private string _userData3 = string.Empty;
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

        public string ContactName
        {
            get
            {
                CanReadProperty("ContactName", true);
                return _contactName;
            }
            set
            {
                CanWriteProperty("ContactName", true);
                if (value == null) value = string.Empty;
                if (!_contactName.Equals(value))
                {
                    _contactName = value;
                    PropertyHasChanged("ContactName");
                }
            }
        }

        public string Title
        {
            get
            {
                CanReadProperty("Title", true);
                return _title;
            }
            set
            {
                CanWriteProperty("Title", true);
                if (value == null) value = string.Empty;
                if (!_title.Equals(value))
                {
                    _title = value;
                    PropertyHasChanged("Title");
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

        public string UserData1
        {
            get
            {
                CanReadProperty("UserData1", true);
                return _userData1;
            }
            set
            {
                CanWriteProperty("UserData1", true);
                if (value == null) value = string.Empty;
                if (!_userData1.Equals(value))
                {
                    _userData1 = value;
                    PropertyHasChanged("UserData1");
                }
            }
        }

        public string UserData2
        {
            get
            {
                CanReadProperty("UserData2", true);
                return _userData2;
            }
            set
            {
                CanWriteProperty("UserData2", true);
                if (value == null) value = string.Empty;
                if (!_userData2.Equals(value))
                {
                    _userData2 = value;
                    PropertyHasChanged("UserData2");
                }
            }
        }

        public string UserData3
        {
            get
            {
                CanReadProperty("UserData3", true);
                return _userData3;
            }
            set
            {
                CanWriteProperty("UserData3", true);
                if (value == null) value = string.Empty;
                if (!_userData3.Equals(value))
                {
                    _userData3 = value;
                    PropertyHasChanged("UserData3");
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
            // ContactName
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ContactName", 100));
            //
            // Title
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Title", 100));
            //
            // Email
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Email", 100));
            //
            // Company
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Company", 100));
            //
            // UserData1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserData1", 500));
            //
            // UserData2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserData2", 500));
            //
            // UserData3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserData3", 500));
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
            //TODO: Define authorization rules in IndCredit
            //AuthorizationRules.AllowRead("Id", "IndCreditReadGroup");
            //AuthorizationRules.AllowRead("EntryId", "IndCreditReadGroup");
            //AuthorizationRules.AllowRead("No", "IndCreditReadGroup");
            //AuthorizationRules.AllowRead("ContactName", "IndCreditReadGroup");
            //AuthorizationRules.AllowRead("Title", "IndCreditReadGroup");
            //AuthorizationRules.AllowRead("Email", "IndCreditReadGroup");
            //AuthorizationRules.AllowRead("Company", "IndCreditReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "IndCreditReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "IndCreditReadGroup");

            //AuthorizationRules.AllowWrite("No", "IndCreditWriteGroup");
            //AuthorizationRules.AllowWrite("ContactName", "IndCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Title", "IndCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Email", "IndCreditWriteGroup");
            //AuthorizationRules.AllowWrite("Company", "IndCreditWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "IndCreditWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "IndCreditWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in IndCredit
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("IndCreditViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in IndCredit
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("IndCreditAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in IndCredit
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("IndCreditEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in IndCredit
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("IndCreditDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private IndCredit()
        { /* require use of factory method */ }

        private IndCredit(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }


        public static IndCredit NewIndCredit()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a IndCredit");
            return DataPortal.Create<IndCredit>();
        }

        public static IndCredit GetIndCredit(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a IndCredit");
            return DataPortal.Fetch<IndCredit>(new Criteria(id));
        }

        public static IndCredit GetIndCredit(SafeDataReader dr)
        {
            return new IndCredit(dr);
        }
        public static void DeleteIndCredit(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a IndCredit");
            DataPortal.Delete(new Criteria(id));
        }

        public override IndCredit Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a IndCredit");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a IndCredit");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a IndCredit");

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
                cm.CommandText = "GetIndCredit";

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
            _contactName = dr.GetString("ContactName");
            _title = dr.GetString("Title");
            _email = dr.GetString("Email");
            _company = dr.GetString("Company");
            _userData1 = dr.GetString("UserData1");
            _userData2 = dr.GetString("UserData2");
            _userData3 = dr.GetString("UserData3");
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
                cm.CommandText = "AddIndCredit";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

              //  _id = (Guid)cm.Parameters["@NewId"].Value;
               // _entryId = (Guid)cm.Parameters["@NewEntryId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {            
            cm.Parameters.AddWithValue("@No", _no);                        
            cm.Parameters.AddWithValue("@ContactName", _contactName);           
            cm.Parameters.AddWithValue("@Title", _title);            
            cm.Parameters.AddWithValue("@Email", _email);            
            cm.Parameters.AddWithValue("@Company", _company);
            cm.Parameters.AddWithValue("@UserData1", _userData1);
            cm.Parameters.AddWithValue("@UserData2", _userData2);
            cm.Parameters.AddWithValue("@UserData3", _userData3);
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
                cm.CommandText = "UpdateIndCredit";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@EntryId", _entryId);            
            cm.Parameters.AddWithValue("@No", _no);            
            cm.Parameters.AddWithValue("@ContactName", _contactName);            
            cm.Parameters.AddWithValue("@Title", _title);           
            cm.Parameters.AddWithValue("@Email", _email);           
            cm.Parameters.AddWithValue("@Company", _company);
            cm.Parameters.AddWithValue("@UserData1", _userData1);
            cm.Parameters.AddWithValue("@UserData2", _userData2);
            cm.Parameters.AddWithValue("@UserData3", _userData3);
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
                cm.CommandText = "DeleteIndCredit";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
