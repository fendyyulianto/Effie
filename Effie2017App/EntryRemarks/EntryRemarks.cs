﻿using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class EntryRemarks : Csla.BusinessBase<EntryRemarks>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _entryId = Guid.NewGuid();
        private string _remarks = string.Empty;
        private SmartDate _dateCreated = new SmartDate(false);
        private bool _isAdmin = false;
        private Guid _commentatorID = Guid.NewGuid();

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

		public bool isAdmin
		{
			get
			{
				CanReadProperty("isAdmin", true);
				return _isAdmin;
			}
			set
			{
				CanWriteProperty("isAdmin", true);
				if (!_isAdmin.Equals(value))
				{
					_isAdmin = value;
					PropertyHasChanged("isAdmin");
				}
			}
		}

		public Guid CommentatorID
		{
			get
			{
				CanReadProperty("CommentatorID", true);
				return _commentatorID;
            }
            set
            {
                CanWriteProperty("CommentatorID", true);
                if (value == null) value = Guid.Empty;
                if (!_commentatorID.Equals(value))
                {
                    _commentatorID = value;
                    PropertyHasChanged("CommentatorID");
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
            //TODO: Define authorization rules in EntryRemarks
            //AuthorizationRules.AllowRead("Id", "EntryRemarksReadGroup");
            //AuthorizationRules.AllowRead("EntryId", "EntryRemarksReadGroup");
            //AuthorizationRules.AllowRead("Remarks", "EntryRemarksReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "EntryRemarksReadGroup");

            //AuthorizationRules.AllowWrite("Remarks", "EntryRemarksWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "EntryRemarksWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in EntryRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryRemarksViewGroup"))
            //    return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in EntryRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryRemarksAddGroup"))
            //    return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in EntryRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryRemarksEditGroup"))
            //    return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in EntryRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryRemarksDeleteGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private EntryRemarks()
        { /* require use of factory method */ }

        private EntryRemarks(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static EntryRemarks NewEntryRemarks()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EntryRemarks");
            return DataPortal.Create<EntryRemarks>();
        }

        public static EntryRemarks GetEntryRemarks(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EntryRemarks");
            return DataPortal.Fetch<EntryRemarks>(new Criteria(id));
        }
        public static EntryRemarks GetEntryRemarks(SafeDataReader dr)
        {
            return new EntryRemarks(dr);
        }
        public static void DeleteEntryRemarks(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EntryRemarks");
            DataPortal.Delete(new Criteria(id));
        }

        public override EntryRemarks Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EntryRemarks");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EntryRemarks");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a EntryRemarks");

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
                cm.CommandText = "GetEntryRemarks";

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
            _remarks = dr.GetString("Remarks");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
			_isAdmin = dr.GetBoolean("isAdmin");
			_commentatorID = dr.GetGuid("CommentatorID");
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
                cm.CommandText = "AddEntryRemarks";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                //_id = (Guid)cm.Parameters["@NewId"].Value;
                //_entryId = (Guid)cm.Parameters["@NewEntryId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
           
            cm.Parameters.AddWithValue("@Remarks", _remarks);          
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
			if (_isAdmin != false)
				cm.Parameters.AddWithValue("@isAdmin", _isAdmin);
			else
				cm.Parameters.AddWithValue("@isAdmin", DBNull.Value);
            cm.Parameters.AddWithValue("@Id", _id);
            //cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
            cm.Parameters.AddWithValue("@EntryId", _entryId);
            //cm.Parameters["@NewEntryId"].Direction = ParameterDirection.Output;
			cm.Parameters.AddWithValue("@CommentatorID", _commentatorID);
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
                cm.CommandText = "UpdateEntryRemarks";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@EntryId", _entryId);            
            cm.Parameters.AddWithValue("@Remarks", _remarks);           
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
			if (_isAdmin != false)
				cm.Parameters.AddWithValue("@isAdmin", _isAdmin);
			else
				cm.Parameters.AddWithValue("@isAdmin", DBNull.Value);
			cm.Parameters.AddWithValue("@CommentatorID", _commentatorID);
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
                cm.CommandText = "DeleteEntryRemarks";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}