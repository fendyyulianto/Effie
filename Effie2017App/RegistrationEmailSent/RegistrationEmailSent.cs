using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class RegistrationEmailSent : Csla.BusinessBase<RegistrationEmailSent>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _templateId = Guid.Empty;
        private Guid _RegistrationId = Guid.Empty;
        private Guid _EntryID = Guid.Empty;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private string _templateName = string.Empty;
        private string _eventYear = string.Empty;
        private string _entryType = string.Empty;

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }

        }

        public Guid TemplateId
        {
            get
            {
                CanReadProperty("TemplateId", true);
                return _templateId;
            }
            set
            {
                CanWriteProperty("TemplateId", true);
                if (value == null) value = Guid.Empty;
                if (!_templateId.Equals(value))
                {
                    _templateId = value;
                    PropertyHasChanged("TemplateId");
                }
            }
        }

        public Guid RegistrationId
        {
            get
            {
                CanReadProperty("RegistrationId", true);
                return _RegistrationId;
            }
            set
            {
                CanWriteProperty("RegistrationId", true);
                if (value == null) value = Guid.Empty;
                if (!_RegistrationId.Equals(value))
                {
                    _RegistrationId = value;
                    PropertyHasChanged("RegistrationId");
                }
            }
        }
        public Guid EntryId
        {
            get
            {
                CanReadProperty("EntryId", true);
                return _EntryID;
            }
            set
            {
                CanWriteProperty("EntryId", true);
                if (value == null) value = Guid.Empty;
                if (!_EntryID.Equals(value))
                {
                    _EntryID = value;
                    PropertyHasChanged("EntryId");
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

        public string TemplateName
        {
            get
            {
                CanReadProperty("TemplateName", true);
                return _templateName;
            }
            set
            {
                CanWriteProperty("TemplateName", true);
                if (value == null) value = string.Empty;
                if (!_templateName.Equals(value))
                {
                    _templateName = value;
                    PropertyHasChanged("TemplateName");
                }
            }
        }

        public string EventYear
        {
            get
            {
                CanReadProperty("EventYear", true);
                return _eventYear;
            }
            set
            {
                CanWriteProperty("EventYear", true);
                if (value == null) value = string.Empty;
                if (!_eventYear.Equals(value))
                {
                    _eventYear = value;
                    PropertyHasChanged("EventYear");
                }
            }
        }

        public string EntryType
        {
            get
            {
                CanReadProperty("EntryType", true);
                return _entryType;
            }
            set
            {
                CanWriteProperty("EntryType", true);
                if (value == null) value = string.Empty;
                if (!_entryType.Equals(value))
                {
                    _entryType = value;
                    PropertyHasChanged("EntryType");
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
            // TemplateName
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TemplateName", 500));
            //
            // EventYear
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EventYear", 50));
            //
            // EntryType
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EntryType", 500));
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
            //TODO: Define authorization rules in RegistrationEmailSent
            //AuthorizationRules.AllowRead("Id", "RegistrationEmailSentReadGroup");
            //AuthorizationRules.AllowRead("TemplateId", "RegistrationEmailSentReadGroup");
            //AuthorizationRules.AllowRead("RegistrationId", "RegistrationEmailSentReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "RegistrationEmailSentReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "RegistrationEmailSentReadGroup");

            //AuthorizationRules.AllowWrite("DateCreated", "RegistrationEmailSentWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "RegistrationEmailSentWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in RegistrationEmailSent
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationEmailSentViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in RegistrationEmailSent
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationEmailSentAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in RegistrationEmailSent
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationEmailSentEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in RegistrationEmailSent
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationEmailSentDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private RegistrationEmailSent()
        { /* require use of factory method */ }

        private RegistrationEmailSent(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static RegistrationEmailSent NewRegistrationEmailSent()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a RegistrationEmailSent");
            return DataPortal.Create<RegistrationEmailSent>();
        }

        public static RegistrationEmailSent GetRegistrationEmailSent(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a RegistrationEmailSent");
            return DataPortal.Fetch<RegistrationEmailSent>(new Criteria(id));
        }

        public static RegistrationEmailSent GetRegistrationEmailSent(SafeDataReader dr)
        {
            return new RegistrationEmailSent(dr);
        }

        public static void DeleteRegistrationEmailSent(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a RegistrationEmailSent");
            DataPortal.Delete(new Criteria(id));
        }

        public override RegistrationEmailSent Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a RegistrationEmailSent");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a RegistrationEmailSent");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a RegistrationEmailSent");

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
                cm.CommandText = "GetRegistrationEmailSent";

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
            _templateId = dr.GetGuid("TemplateId");
            _RegistrationId = dr.GetGuid("RegistrationId");
            _EntryID = dr.GetGuid("EntryId");
            
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _templateName = dr.GetString("TemplateName");
            _eventYear = dr.GetString("EventYear");
            _entryType = dr.GetString("EntryType");
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
                cm.CommandText = "AddRegistrationEmailSent";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
                _templateId = (Guid)cm.Parameters["@TemplateId"].Value;
                _RegistrationId = (Guid)cm.Parameters["@RegistrationId"].Value;
                _EntryID = (Guid)cm.Parameters["@EntryId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);           
            cm.Parameters.AddWithValue("@TemplateId", _templateId); 
            cm.Parameters.AddWithValue("@RegistrationId", _RegistrationId);
            cm.Parameters.AddWithValue("@EntryId", _EntryID);
            cm.Parameters.AddWithValue("@TemplateName", _templateName);
            cm.Parameters.AddWithValue("@EventYear", _eventYear);
            cm.Parameters.AddWithValue("@EntryType", _entryType);
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
                cm.CommandText = "UpdateRegistrationEmailSent";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@TemplateId", _templateId);
            cm.Parameters.AddWithValue("@RegistrationId", _RegistrationId);
            cm.Parameters.AddWithValue("@EntryId", _EntryID);
            cm.Parameters.AddWithValue("@TemplateName", _templateName);
            cm.Parameters.AddWithValue("@EventYear", _eventYear);
            cm.Parameters.AddWithValue("@EntryType", _entryType);
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
                cm.CommandText = "DeleteRegistrationEmailSent";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
