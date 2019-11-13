using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class Administrator : Csla.BusinessBase<Administrator>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _name = string.Empty;
        private string _loginId = string.Empty;
        private string _password = string.Empty;
        private string _access = string.Empty;
        private SmartDate _dateLastLogin = new SmartDate(false);
        private SmartDate _dateCreated = new SmartDate(false);
        private bool _isActive = false;
        private SmartDate _dateModified = new SmartDate(false);
        private SmartDate _LastChangePassword = new SmartDate(false);
        
        private bool _IsExpired = false;
        private bool _IsLooked = false;
        private string _LogPassword = string.Empty;

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public string Name
        {
            get
            {
                CanReadProperty("Name", true);
                return _name;
            }
            set
            {
                CanWriteProperty("Name", true);
                if (value == null) value = string.Empty;
                if (!_name.Equals(value))
                {
                    _name = value;
                    PropertyHasChanged("Name");
                }
            }
        }

        public string LoginId
        {
            get
            {
                CanReadProperty("LoginId", true);
                return _loginId;
            }
            set
            {
                CanWriteProperty("LoginId", true);
                if (value == null) value = string.Empty;
                if (!_loginId.Equals(value))
                {
                    _loginId = value;
                    PropertyHasChanged("LoginId");
                }
            }
        }

        public string Password
        {
            get
            {
                CanReadProperty("Password", true);
                return _password;
            }
            set
            {
                CanWriteProperty("Password", true);
                if (value == null) value = string.Empty;
                if (!_password.Equals(value))
                {
                    _password = value;
                    PropertyHasChanged("Password");
                }
            }
        }

        public string Access
        {
            get
            {
                CanReadProperty("Access", true);
                return _access;
            }
            set
            {
                CanWriteProperty("Access", true);
                if (value == null) value = string.Empty;
                if (!_access.Equals(value))
                {
                    _access = value;
                    PropertyHasChanged("Access");
                }
            }
        }

        public string LogPassword 
        {
            get
            {
                CanReadProperty("LogPassword", true);
                return _LogPassword;
            }
            set
            {
                CanWriteProperty("LogPassword", true);
                if (value == null) value = string.Empty;
                if (!_LogPassword.Equals(value))
                {
                    _LogPassword = value;
                    PropertyHasChanged("LogPassword");
                }
            }
        }
        public DateTime DateLastLogin
        {
            get
            {
                CanReadProperty("DateLastLogin", true);
                return _dateLastLogin.Date;
            }
        }

        public string DateLastLoginString
        {
            get
            {
                CanReadProperty("DateLastLogin", true);
                return _dateLastLogin.Text;
            }
            set
            {
                CanWriteProperty("DateLastLogin", true);
                if (value == null) value = string.Empty;
                if (!_dateLastLogin.Equals(value))
                {
                    _dateLastLogin.Text = value;
                    PropertyHasChanged("DateLastLogin");
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

        public bool IsActive
        {
            get
            {
                CanReadProperty("IsActive", true);
                return _isActive;
            }
            set
            {
                CanWriteProperty("IsActive", true);
                if (!_isActive.Equals(value))
                {
                    _isActive = value;
                    PropertyHasChanged("IsActive");
                }
            }
        }

        

        public DateTime LastChangePassword
        {
            get
            {
                CanReadProperty("LastChangePassword", true);
                return _LastChangePassword.Date;
            }
        }

        public string LastChangePasswordString
        {
            get
            {
                CanReadProperty("LastChangePassword", true);
                return _LastChangePassword.Text;
            }
            set
            {
                CanWriteProperty("LastChangePassword", true);
                if (value == null) value = string.Empty;
                if (!_LastChangePassword.Equals(value))
                {
                    _LastChangePassword.Text = value;
                    PropertyHasChanged("LastChangePassword");
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

        public bool IsLooked
        {
            get
            {
                CanReadProperty("IsLooked", true);
                return _IsLooked;
            }
            set
            {
                CanWriteProperty("IsLooked", true);
                if (!_IsLooked.Equals(value))
                {
                    _IsLooked = value;
                    PropertyHasChanged("IsLooked");
                }
            }
        }
        public bool IsExpired
        {
            get
            {
                CanReadProperty("IsExpired", true);
                return _IsExpired;
            }
            set
            {
                CanWriteProperty("IsExpired", true);
                if (!_IsExpired.Equals(value))
                {
                    _IsExpired = value;
                    PropertyHasChanged("IsExpired");
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
            // Name
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Name", 100));
            //
            // LoginId
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LoginId", 50));
            //
            // Password
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Password", 50));
            //
            // Access
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Access", 50));
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
            //TODO: Define authorization rules in Administrator
            //AuthorizationRules.AllowRead("Id", "AdministratorReadGroup");
            //AuthorizationRules.AllowRead("Name", "AdministratorReadGroup");
            //AuthorizationRules.AllowRead("LoginId", "AdministratorReadGroup");
            //AuthorizationRules.AllowRead("Password", "AdministratorReadGroup");
            //AuthorizationRules.AllowRead("Access", "AdministratorReadGroup");
            //AuthorizationRules.AllowRead("DateLastLogin", "AdministratorReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "AdministratorReadGroup");
            //AuthorizationRules.AllowRead("IsActive", "AdministratorReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "AdministratorReadGroup");

            //AuthorizationRules.AllowWrite("Name", "AdministratorWriteGroup");
            //AuthorizationRules.AllowWrite("LoginId", "AdministratorWriteGroup");
            //AuthorizationRules.AllowWrite("Password", "AdministratorWriteGroup");
            //AuthorizationRules.AllowWrite("Access", "AdministratorWriteGroup");
            //AuthorizationRules.AllowWrite("DateLastLogin", "AdministratorWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "AdministratorWriteGroup");
            //AuthorizationRules.AllowWrite("IsActive", "AdministratorWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "AdministratorWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Administrator
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdministratorViewGroup"))
            //    return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Administrator
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdministratorAddGroup"))
            //    return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Administrator
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdministratorEditGroup"))
            //    return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Administrator
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdministratorDeleteGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Administrator()
        { /* require use of factory method */ }

        private Administrator(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static Administrator NewAdministrator()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Administrator");
            return DataPortal.Create<Administrator>();
        }

        public static Administrator GetAdministrator(SafeDataReader dr)
        {
            return new Administrator(dr);
        }
        public static Administrator GetAdministrator(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Administrator");
            return DataPortal.Fetch<Administrator>(new Criteria(id));
        }

        public static void DeleteAdministrator(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Administrator");
            DataPortal.Delete(new Criteria(id));
        }

        public override Administrator Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Administrator");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Administrator");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a Administrator");

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
                cm.CommandText = "GetAdministrator";

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
            _name = dr.GetString("Name");
            _loginId = dr.GetString("LoginId");
            _password = dr.GetString("Password");
            _access = dr.GetString("Access");
            _LogPassword = dr.GetString("LogPassword");
            _dateLastLogin = dr.GetSmartDate("DateLastLogin", _dateLastLogin.EmptyIsMin);
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _isActive = dr.GetBoolean("IsActive");
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _LastChangePassword = dr.GetSmartDate("LastChangePassword", _LastChangePassword.EmptyIsMin);
            
            _IsExpired = dr.GetBoolean("IsExpired");
            _IsLooked = dr.GetBoolean("IsLooked");
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
                cm.CommandText = "AddAdministrator";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

               // _id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            
                cm.Parameters.AddWithValue("@Name", _name);           
                cm.Parameters.AddWithValue("@LoginId", _loginId);          
                cm.Parameters.AddWithValue("@Password", _password);          
                cm.Parameters.AddWithValue("@Access", _access);          
                cm.Parameters.AddWithValue("@DateLastLogin", _dateLastLogin.DBValue);
                cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
                cm.Parameters.AddWithValue("@LogPassword", _LogPassword);
                cm.Parameters.AddWithValue("@IsActive", _isActive);
                cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
                cm.Parameters.AddWithValue("@LastChangePassword", _LastChangePassword.DBValue);
            
                cm.Parameters.AddWithValue("@IsExpired", _IsExpired);
                cm.Parameters.AddWithValue("@IsLooked", _IsLooked);
                cm.Parameters.AddWithValue("@Id", _id);
                //cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
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
                cm.CommandText = "UpdateAdministrator";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);            
            cm.Parameters.AddWithValue("@Name", _name);           
            cm.Parameters.AddWithValue("@LoginId", _loginId);          
            cm.Parameters.AddWithValue("@Password", _password);           
            cm.Parameters.AddWithValue("@Access", _access);           
            cm.Parameters.AddWithValue("@DateLastLogin", _dateLastLogin.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@LogPassword", _LogPassword);
            cm.Parameters.AddWithValue("@IsActive", _isActive);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@LastChangePassword", _LastChangePassword.DBValue);
            cm.Parameters.AddWithValue("@IsExpired", _IsExpired);
            cm.Parameters.AddWithValue("@IsLooked", _IsLooked);
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
                cm.CommandText = "DeleteAdministrator";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}