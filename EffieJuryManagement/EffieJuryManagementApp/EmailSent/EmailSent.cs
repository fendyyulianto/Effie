using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class EmailSent : Csla.BusinessBase<EmailSent>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _templateId = Guid.Empty;
        private Guid _juryId = Guid.Empty;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private string _templateName = string.Empty;
        private string _eventYear = string.Empty;

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
            //TODO: Define authorization rules in EmailSent
            //AuthorizationRules.AllowRead("Id", "EmailSentReadGroup");
            //AuthorizationRules.AllowRead("TemplateId", "EmailSentReadGroup");
            //AuthorizationRules.AllowRead("JuryId", "EmailSentReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "EmailSentReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "EmailSentReadGroup");

            //AuthorizationRules.AllowWrite("DateCreated", "EmailSentWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "EmailSentWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in EmailSent
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailSentViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in EmailSent
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailSentAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in EmailSent
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailSentEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in EmailSent
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailSentDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private EmailSent()
        { /* require use of factory method */ }

        private EmailSent(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static EmailSent NewEmailSent()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EmailSent");
            return DataPortal.Create<EmailSent>();
        }

        public static EmailSent GetEmailSent(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EmailSent");
            return DataPortal.Fetch<EmailSent>(new Criteria(id));
        }

        public static EmailSent GetEmailSent(SafeDataReader dr)
        {
            return new EmailSent(dr);
        }

        public static void DeleteEmailSent(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EmailSent");
            DataPortal.Delete(new Criteria(id));
        }

        public override EmailSent Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EmailSent");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EmailSent");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a EmailSent");

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
                cm.CommandText = "GetEmailSent";

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
            _juryId = dr.GetGuid("JuryId");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _templateName = dr.GetString("TemplateName");
            _eventYear = dr.GetString("EventYear");
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
                cm.CommandText = "AddEmailSent";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
                _templateId = (Guid)cm.Parameters["@TemplateId"].Value;
                _juryId = (Guid)cm.Parameters["@JuryId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);           
            cm.Parameters.AddWithValue("@TemplateId", _templateId);           
            cm.Parameters.AddWithValue("@JuryId", _juryId);
            cm.Parameters.AddWithValue("@TemplateName", _templateName);
            cm.Parameters.AddWithValue("@EventYear", _eventYear);
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
                cm.CommandText = "UpdateEmailSent";

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
            cm.Parameters.AddWithValue("@JuryId", _juryId);
            cm.Parameters.AddWithValue("@TemplateName", _templateName);
            cm.Parameters.AddWithValue("@EventYear", _eventYear);
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
                cm.CommandText = "DeleteEmailSent";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
