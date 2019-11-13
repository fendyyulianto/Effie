using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class RegistrationRemarks : Csla.BusinessBase<RegistrationRemarks>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _registrationId = Guid.NewGuid();
        private string _remarks = string.Empty;
        private SmartDate _dateCreated = new SmartDate(false);

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public Guid RegistrationId
        {
            get
            {
                CanReadProperty("RegistrationId", true);
                return _registrationId;
            }
            set
            {
                CanWriteProperty("RegistrationId", true);
                if (value == null) value = Guid.Empty;
                if (!_registrationId.Equals(value))
                {
                    _registrationId = value;
                    PropertyHasChanged("RegistrationId");
                }
            }
        }

        public string Remarks
        {
            get
            {
                CanReadProperty("Remarks", true);
                return _remarks;
            }
            set
            {
                CanWriteProperty("Remarks", true);
                if (value == null) value = string.Empty;
                if (!_remarks.Equals(value))
                {
                    _remarks = value;
                    PropertyHasChanged("Remarks");
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
            // Remarks
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Remarks", 1000));
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
            //TODO: Define authorization rules in RegistrationRemarks
            //AuthorizationRules.AllowRead("Id", "RegistrationRemarksReadGroup");
            //AuthorizationRules.AllowRead("RegistrationId", "RegistrationRemarksReadGroup");
            //AuthorizationRules.AllowRead("Remarks", "RegistrationRemarksReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "RegistrationRemarksReadGroup");

            //AuthorizationRules.AllowWrite("Remarks", "RegistrationRemarksWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "RegistrationRemarksWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in RegistrationRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationRemarksViewGroup"))
            //    return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in RegistrationRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationRemarksAddGroup"))
            //    return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in RegistrationRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationRemarksEditGroup"))
            //    return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in RegistrationRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationRemarksDeleteGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private RegistrationRemarks()
        { /* require use of factory method */ }

        private RegistrationRemarks(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }
        public static RegistrationRemarks NewRegistrationRemarks()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a RegistrationRemarks");
            return DataPortal.Create<RegistrationRemarks>();
        }

        public static RegistrationRemarks GetRegistrationRemarks(SafeDataReader dr)
        {
            return new RegistrationRemarks(dr);
        }

        public static RegistrationRemarks GetRegistrationRemarks(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a RegistrationRemarks");
            return DataPortal.Fetch<RegistrationRemarks>(new Criteria(id));
        }

        public static void DeleteRegistrationRemarks(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a RegistrationRemarks");
            DataPortal.Delete(new Criteria(id));
        }

        public override RegistrationRemarks Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a RegistrationRemarks");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a RegistrationRemarks");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a RegistrationRemarks");

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
                cm.CommandText = "GetRegistrationRemarks";

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
            _registrationId = dr.GetGuid("RegistrationId");
            _remarks = dr.GetString("Remarks");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
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
                cm.CommandText = "AddRegistrationRemarks";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

               // _id = (Guid)cm.Parameters["@NewId"].Value;
              //  _registrationId = (Guid)cm.Parameters["@NewRegistrationId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {            
            cm.Parameters.AddWithValue("@Remarks", _remarks);            
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            // cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
            cm.Parameters.AddWithValue("@RegistrationId", _registrationId);
            //cm.Parameters["@NewRegistrationId"].Direction = ParameterDirection.Output;
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
                cm.CommandText = "UpdateRegistrationRemarks";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@RegistrationId", _registrationId);            
            cm.Parameters.AddWithValue("@Remarks", _remarks);            
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
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
                cm.CommandText = "DeleteRegistrationRemarks";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}