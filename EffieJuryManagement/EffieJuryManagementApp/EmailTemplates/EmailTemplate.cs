using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class EmailTemplate : Csla.BusinessBase<EmailTemplate>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _templateId = Guid.Empty;
        private string _title = string.Empty;
        private string _body = string.Empty;
        private string _subject = string.Empty;
        private bool _isActive = false;
        private string _userData1 = string.Empty;
        private string _userData2 = string.Empty;
        private string _userData3 = string.Empty;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private bool _isInvitation = false;
        private bool _isDelete = false;

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

        public string Body
        {
            get
            {
                CanReadProperty("Body", true);
                return _body;
            }
            set
            {
                CanWriteProperty("Body", true);
                if (value == null) value = string.Empty;
                if (!_body.Equals(value))
                {
                    _body = value;
                    PropertyHasChanged("Body");
                }
            }
        }

        public string Subject
        {
            get
            {
                CanReadProperty("Subject", true);
                return _subject;
            }
            set
            {
                CanWriteProperty("Subject", true);
                if (value == null) value = string.Empty;
                if (!_subject.Equals(value))
                {
                    _subject = value;
                    PropertyHasChanged("Subject");
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

        public bool IsInvitation
        {
            get
            {
                CanReadProperty("IsInvitation", true);
                return _isInvitation;
            }
            set
            {
                CanWriteProperty("IsInvitation", true);
                if (!_isInvitation.Equals(value))
                {
                    _isInvitation = value;
                    PropertyHasChanged("IsInvitation");
                }
            }
        }

        public bool IsDelete
        {
            get
            {
                CanReadProperty("IsDelete", true);
                return _isDelete;
            }
            set
            {
                CanWriteProperty("IsDelete", true);
                if (!_isDelete.Equals(value))
                {
                    _isDelete = value;
                    PropertyHasChanged("IsDelete");
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
            // Title
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Title", 100));
            //
            // Subject
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Subject", 200));
           
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
            //TODO: Define authorization rules in EmailTemplate
            //AuthorizationRules.AllowRead("Id", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("Title", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("Body", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("Subject", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("IsActive", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("UserData1", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("UserData2", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("UserData3", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "EmailTemplateReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "EmailTemplateReadGroup");

            //AuthorizationRules.AllowWrite("Title", "EmailTemplateWriteGroup");
            //AuthorizationRules.AllowWrite("Body", "EmailTemplateWriteGroup");
            //AuthorizationRules.AllowWrite("Subject", "EmailTemplateWriteGroup");
            //AuthorizationRules.AllowWrite("IsActive", "EmailTemplateWriteGroup");
            //AuthorizationRules.AllowWrite("UserData1", "EmailTemplateWriteGroup");
            //AuthorizationRules.AllowWrite("UserData2", "EmailTemplateWriteGroup");
            //AuthorizationRules.AllowWrite("UserData3", "EmailTemplateWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "EmailTemplateWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "EmailTemplateWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in EmailTemplate
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailTemplateViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in EmailTemplate
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailTemplateAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in EmailTemplate
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailTemplateEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in EmailTemplate
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailTemplateDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private EmailTemplate()
        { /* require use of factory method */ }

        private EmailTemplate(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static EmailTemplate NewEmailTemplate()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EmailTemplate");
            return DataPortal.Create<EmailTemplate>();
        }

        public static EmailTemplate GetEmailTemplate(SafeDataReader dr)
        {
            return new EmailTemplate(dr);
        }

        public static EmailTemplate GetEmailTemplate(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EmailTemplate");
            return DataPortal.Fetch<EmailTemplate>(new Criteria(id));
        }

        public static void DeleteEmailTemplate(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EmailTemplate");
            DataPortal.Delete(new Criteria(id));
        }

        public override EmailTemplate Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EmailTemplate");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EmailTemplate");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a EmailTemplate");

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
                cm.CommandText = "GetEmailTemplate";

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
            _title = dr.GetString("Title");
            _body = dr.GetString("Body");
            _subject = dr.GetString("Subject");
            _isActive = dr.GetBoolean("IsActive");
            _userData1 = dr.GetString("UserData1");
            _userData2 = dr.GetString("UserData2");
            _userData3 = dr.GetString("UserData3");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _isInvitation = dr.GetBoolean("IsInvitation");
            _isDelete = dr.GetBoolean("IsDelete");
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
                cm.CommandText = "AddEmailTemplate";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
                _templateId = (Guid)cm.Parameters["@TemplateId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@TemplateId", _templateId);
            cm.Parameters.AddWithValue("@Title", _title);
            cm.Parameters.AddWithValue("@Body", _body);
            cm.Parameters.AddWithValue("@Subject", _subject);
            cm.Parameters.AddWithValue("@IsActive", _isActive);
            cm.Parameters.AddWithValue("@UserData1", _userData1);
            cm.Parameters.AddWithValue("@UserData2", _userData2);
            cm.Parameters.AddWithValue("@UserData3", _userData3);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@IsInvitation", _isInvitation);
            cm.Parameters.AddWithValue("@IsDelete", _isDelete);
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
                cm.CommandText = "UpdateEmailTemplate";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@TemplateId", _templateId);
            cm.Parameters.AddWithValue("@Title", _title);
            cm.Parameters.AddWithValue("@Body", _body);
            cm.Parameters.AddWithValue("@Subject", _subject);
            cm.Parameters.AddWithValue("@IsActive", _isActive);
            cm.Parameters.AddWithValue("@UserData1", _userData1);
            cm.Parameters.AddWithValue("@UserData2", _userData2);
            cm.Parameters.AddWithValue("@UserData3", _userData3);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@IsInvitation", _isInvitation);
            cm.Parameters.AddWithValue("@IsDelete", _isDelete);
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
                cm.CommandText = "DeleteEmailTemplate";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
