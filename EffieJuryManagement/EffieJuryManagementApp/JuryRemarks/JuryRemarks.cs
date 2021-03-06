﻿using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class JuryRemarks : Csla.BusinessBase<JuryRemarks>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _juryId = Guid.NewGuid();
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
            //TODO: Define authorization rules in JuryRemarks
            //AuthorizationRules.AllowRead("Id", "JuryRemarksReadGroup");
            //AuthorizationRules.AllowRead("JuryId", "JuryRemarksReadGroup");
            //AuthorizationRules.AllowRead("Remarks", "JuryRemarksReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "JuryRemarksReadGroup");

            //AuthorizationRules.AllowWrite("Remarks", "JuryRemarksWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "JuryRemarksWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in JuryRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryRemarksViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in JuryRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryRemarksAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in JuryRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryRemarksEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in JuryRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryRemarksDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private JuryRemarks()
        { /* require use of factory method */ }

        private JuryRemarks(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static JuryRemarks NewJuryRemarks()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a JuryRemarks");
            return DataPortal.Create<JuryRemarks>();
        }

        public static JuryRemarks GetJuryRemarks(SafeDataReader dr)
        {
            return new JuryRemarks(dr);
        }

        public static JuryRemarks GetJuryRemarks(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JuryRemarks");
            return DataPortal.Fetch<JuryRemarks>(new Criteria(id));
        }

        public static void DeleteJuryRemarks(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a JuryRemarks");
            DataPortal.Delete(new Criteria(id));
        }

        public override JuryRemarks Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a JuryRemarks");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a JuryRemarks");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a JuryRemarks");

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
                cm.CommandText = "GetJuryRemarks";

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
                cm.CommandText = "AddJuryRemarks";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                //_id = (Guid)cm.Parameters["@NewId"].Value;
                //_juryId = (Guid)cm.Parameters["@NewJuryId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Remarks", _remarks);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            //cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
            cm.Parameters.AddWithValue("@JuryId", _juryId);
            //cm.Parameters["@NewEntryId"].Direction = ParameterDirection.Output;
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
                cm.CommandText = "UpdateJuryRemarks";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Remarks", _remarks);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            //cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
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
                cm.CommandText = "DeleteJuryRemarks";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
