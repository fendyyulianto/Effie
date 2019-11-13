using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class Invitation : Csla.BusinessBase<Invitation>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _juryId = Guid.Empty;
        private string _eventCode = string.Empty;
        private bool _isRound1Invited = false;
        private bool _isRound2Invited = false;
        private bool _isDeclined = false;
        private bool _isRound1Accepted = false;
        private bool _isRound2Accepted = false;
        private bool _isRound1Assigned = false;
        private bool _isRound2Assigned = false;
        private bool _isRound1Shortlisted = false;
        private bool _isRound2Shortlisted = false;
        private SmartDate _dateRound1EmailSent = new SmartDate(false);
        private SmartDate _dateRound2EmailSent = new SmartDate(false);
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private string _userdata1 = string.Empty;
        private string _userdata2 = string.Empty;
        private string _userdata3 = string.Empty;
        private bool _isLocked = false;
        private bool _isRead = false;

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

        public string EventCode
        {
            get
            {
                CanReadProperty("EventCode", true);
                return _eventCode;
            }
            set
            {
                CanWriteProperty("EventCode", true);
                if (value == null) value = string.Empty;
                if (!_eventCode.Equals(value))
                {
                    _eventCode = value;
                    PropertyHasChanged("EventCode");
                }
            }
        }

        public bool IsRound1Invited
        {
            get
            {
                CanReadProperty("IsRound1Invited", true);
                return _isRound1Invited;
            }
            set
            {
                CanWriteProperty("IsRound1Invited", true);
                if (!_isRound1Invited.Equals(value))
                {
                    _isRound1Invited = value;
                    PropertyHasChanged("IsRound1Invited");
                }
            }
        }

        public bool IsRound2Invited
        {
            get
            {
                CanReadProperty("IsRound2Invited", true);
                return _isRound2Invited;
            }
            set
            {
                CanWriteProperty("IsRound2Invited", true);
                if (!_isRound2Invited.Equals(value))
                {
                    _isRound2Invited = value;
                    PropertyHasChanged("IsRound2Invited");
                }
            }
        }

        public bool IsDeclined
        {
            get
            {
                CanReadProperty("IsDeclined", true);
                return _isDeclined;
            }
            set
            {
                CanWriteProperty("IsDeclined", true);
                if (!_isDeclined.Equals(value))
                {
                    _isDeclined = value;
                    PropertyHasChanged("IsDeclined");
                }
            }
        }


        public bool IsRound1Accepted
        {
            get
            {
                CanReadProperty("IsRound1Accepted", true);
                return _isRound1Accepted;
            }
            set
            {
                CanWriteProperty("IsRound1Accepted", true);
                if (!_isRound1Accepted.Equals(value))
                {
                    _isRound1Accepted = value;
                    PropertyHasChanged("IsRound1Accepted");
                }
            }
        }

        public bool IsRound2Accepted
        {
            get
            {
                CanReadProperty("IsRound2Accepted", true);
                return _isRound2Accepted;
            }
            set
            {
                CanWriteProperty("IsRound2Accepted", true);
                if (!_isRound2Accepted.Equals(value))
                {
                    _isRound2Accepted = value;
                    PropertyHasChanged("IsRound2Accepted");
                }
            }
        }

        public bool IsRound1Assigned
        {
            get
            {
                CanReadProperty("IsRound1Assigned", true);
                return _isRound1Assigned;
            }
            set
            {
                CanWriteProperty("IsRound1Assigned", true);
                if (!_isRound1Assigned.Equals(value))
                {
                    _isRound1Assigned = value;
                    PropertyHasChanged("IsRound1Assigned");
                }
            }
        }

        public bool IsRound2Assigned
        {
            get
            {
                CanReadProperty("IsRound2Assigned", true);
                return _isRound2Assigned;
            }
            set
            {
                CanWriteProperty("IsRound2Assigned", true);
                if (!_isRound2Assigned.Equals(value))
                {
                    _isRound2Assigned = value;
                    PropertyHasChanged("IsRound2Assigned");
                }
            }
        }

        public bool IsRound1Shortlisted
        {
            get
            {
                CanReadProperty("IsRound1Shortlisted", true);
                return _isRound1Shortlisted;
            }
            set
            {
                CanWriteProperty("IsRound1Shortlisted", true);
                if (!_isRound1Shortlisted.Equals(value))
                {
                    _isRound1Shortlisted = value;
                    PropertyHasChanged("IsRound1Shortlisted");
                }
            }
        }

        public bool IsRound2Shortlisted
        {
            get
            {
                CanReadProperty("IsRound2Shortlisted", true);
                return _isRound2Shortlisted;
            }
            set
            {
                CanWriteProperty("IsRound2Shortlisted", true);
                if (!_isRound2Shortlisted.Equals(value))
                {
                    _isRound2Shortlisted = value;
                    PropertyHasChanged("IsRound2Shortlisted");
                }
            }
        }

        public DateTime DateRound1EmailSent
        {
            get
            {
                CanReadProperty("DateRound1EmailSent", true);
                return _dateRound1EmailSent.Date;
            }
        }

        public string DateRound1EmailSentString
        {
            get
            {
                CanReadProperty("DateRound1EmailSent", true);
                return _dateRound1EmailSent.Text;
            }
            set
            {
                CanWriteProperty("DateRound1EmailSent", true);
                if (value == null) value = string.Empty;
                if (!_dateRound1EmailSent.Equals(value))
                {
                    _dateRound1EmailSent.Text = value;
                    PropertyHasChanged("DateRound1EmailSent");
                }
            }
        }

        public DateTime DateRound2EmailSent
        {
            get
            {
                CanReadProperty("DateRound2EmailSent", true);
                return _dateRound2EmailSent.Date;
            }
        }

        public string DateRound2EmailSentString
        {
            get
            {
                CanReadProperty("DateRound2EmailSent", true);
                return _dateRound2EmailSent.Text;
            }
            set
            {
                CanWriteProperty("DateRound2EmailSent", true);
                if (value == null) value = string.Empty;
                if (!_dateRound2EmailSent.Equals(value))
                {
                    _dateRound2EmailSent.Text = value;
                    PropertyHasChanged("DateRound2EmailSent");
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

        public string Userdata1
        {
            get
            {
                CanReadProperty("Userdata1", true);
                return _userdata1;
            }
            set
            {
                CanWriteProperty("Userdata1", true);
                if (value == null) value = string.Empty;
                if (!_userdata1.Equals(value))
                {
                    _userdata1 = value;
                    PropertyHasChanged("Userdata1");
                }
            }
        }

        public string Userdata2
        {
            get
            {
                CanReadProperty("Userdata2", true);
                return _userdata2;
            }
            set
            {
                CanWriteProperty("Userdata2", true);
                if (value == null) value = string.Empty;
                if (!_userdata2.Equals(value))
                {
                    _userdata2 = value;
                    PropertyHasChanged("Userdata2");
                }
            }
        }

        public string Userdata3
        {
            get
            {
                CanReadProperty("Userdata3", true);
                return _userdata3;
            }
            set
            {
                CanWriteProperty("Userdata3", true);
                if (value == null) value = string.Empty;
                if (!_userdata3.Equals(value))
                {
                    _userdata3 = value;
                    PropertyHasChanged("Userdata3");
                }
            }
        }

        public bool IsLocked
        {
            get
            {
                CanReadProperty("IsLocked", true);
                return _isLocked;
            }
            set
            {
                CanWriteProperty("IsLocked", true);
                if (!_isLocked.Equals(value))
                {
                    _isLocked = value;
                    PropertyHasChanged("IsLocked");
                }
            }
        }

        public bool IsRead
        {
            get
            {
                CanReadProperty("IsRead", true);
                return _isRead;
            }
            set
            {
                CanWriteProperty("IsRead", true);
                if (!_isRead.Equals(value))
                {
                    _isRead = value;
                    PropertyHasChanged("IsRead");
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
            // EventCode
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EventCode", 20));
            //
            // Userdata1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Userdata1", 4000));
            //
            // Userdata2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Userdata2", 4000));
            //
            // Userdata3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Userdata3", 4000));
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
            //TODO: Define authorization rules in Invitation
            //AuthorizationRules.AllowRead("Id", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("JuryId", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("EventCode", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("IsRound1Invited", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("IsRound2Invited", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("IsRound1Accepted", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("IsRound2Accepted", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("IsRound1Assigned", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("IsRound2Assigned", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("DateRound1EmailSent", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("DateRound2EmailSent", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("Userdata1", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("Userdata2", "InvitationReadGroup");
            //AuthorizationRules.AllowRead("Userdata3", "InvitationReadGroup");

            //AuthorizationRules.AllowWrite("EventCode", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound1Invited", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound2Invited", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound1Accepted", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound2Accepted", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound1Assigned", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound2Assigned", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("DateRound1EmailSent", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("DateRound2EmailSent", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("Userdata1", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("Userdata2", "InvitationWriteGroup");
            //AuthorizationRules.AllowWrite("Userdata3", "InvitationWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Invitation
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvitationViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Invitation
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvitationAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Invitation
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvitationEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Invitation
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvitationDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Invitation()
        { /* require use of factory method */ }

        private Invitation(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static Invitation NewInvitation()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Invitation");
            return DataPortal.Create<Invitation>();
        }

        public static Invitation GetInvitation(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Invitation");
            return DataPortal.Fetch<Invitation>(new Criteria(id));
        }

        public static Invitation GetInvitation(SafeDataReader dr)
        {
            return new Invitation(dr);
        }

        public static void DeleteInvitation(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Invitation");
            DataPortal.Delete(new Criteria(id));
        }

        public override Invitation Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Invitation");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Invitation");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a Invitation");

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
                cm.CommandText = "GetInvitation";

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
            _eventCode = dr.GetString("EventCode");
            _isRound1Invited = dr.GetBoolean("IsRound1Invited");
            _isRound2Invited = dr.GetBoolean("IsRound2Invited");
            _isDeclined = dr.GetBoolean("IsDeclined");
            _isRound1Accepted = dr.GetBoolean("IsRound1Accepted");
            _isRound2Accepted = dr.GetBoolean("IsRound2Accepted");
            _isRound1Assigned = dr.GetBoolean("IsRound1Assigned");
            _isRound2Assigned = dr.GetBoolean("IsRound2Assigned");
            _isRound1Shortlisted = dr.GetBoolean("IsRound1Shortlisted");
            _isRound2Shortlisted = dr.GetBoolean("IsRound2Shortlisted");
            _dateRound1EmailSent = dr.GetSmartDate("DateRound1EmailSent", _dateRound1EmailSent.EmptyIsMin);
            _dateRound2EmailSent = dr.GetSmartDate("DateRound2EmailSent", _dateRound2EmailSent.EmptyIsMin);
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _userdata1 = dr.GetString("Userdata1");
            _userdata2 = dr.GetString("Userdata2");
            _userdata3 = dr.GetString("Userdata3");
            _isLocked = dr.GetBoolean("IsLocked");
            _isRead = dr.GetBoolean("IsRead");
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
                cm.CommandText = "AddInvitation";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
                _juryId = (Guid)cm.Parameters["@JuryId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@EventCode", _eventCode);
            cm.Parameters.AddWithValue("@IsRound1Invited", _isRound1Invited);
            cm.Parameters.AddWithValue("@IsRound2Invited", _isRound2Invited);
            cm.Parameters.AddWithValue("@IsDeclined", _isDeclined);
            cm.Parameters.AddWithValue("@IsRound1Accepted", _isRound1Accepted);
            cm.Parameters.AddWithValue("@IsRound2Accepted", _isRound2Accepted);
            cm.Parameters.AddWithValue("@IsRound1Assigned", _isRound1Assigned);
            cm.Parameters.AddWithValue("@IsRound2Assigned", _isRound2Assigned);
            cm.Parameters.AddWithValue("@IsRound1Shortlisted", _isRound1Shortlisted);
            cm.Parameters.AddWithValue("@IsRound2Shortlisted", _isRound2Shortlisted);
            cm.Parameters.AddWithValue("@DateRound1EmailSent", _dateRound1EmailSent.DBValue);
            cm.Parameters.AddWithValue("@DateRound2EmailSent", _dateRound2EmailSent.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Userdata1", _userdata1);
            cm.Parameters.AddWithValue("@Userdata2", _userdata2);
            cm.Parameters.AddWithValue("@Userdata3", _userdata3);
            cm.Parameters.AddWithValue("@IsLocked", _isLocked);
            cm.Parameters.AddWithValue("@IsRead", _isRead);
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
                cm.CommandText = "UpdateInvitation";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@EventCode", _eventCode);
            cm.Parameters.AddWithValue("@IsRound1Invited", _isRound1Invited);
            cm.Parameters.AddWithValue("@IsRound2Invited", _isRound2Invited);
            cm.Parameters.AddWithValue("@IsDeclined", _isDeclined);
            cm.Parameters.AddWithValue("@IsRound1Accepted", _isRound1Accepted);
            cm.Parameters.AddWithValue("@IsRound2Accepted", _isRound2Accepted);
            cm.Parameters.AddWithValue("@IsRound1Assigned", _isRound1Assigned);
            cm.Parameters.AddWithValue("@IsRound2Assigned", _isRound2Assigned);
            cm.Parameters.AddWithValue("@IsRound1Shortlisted", _isRound1Shortlisted);
            cm.Parameters.AddWithValue("@IsRound2Shortlisted", _isRound2Shortlisted);
            cm.Parameters.AddWithValue("@DateRound1EmailSent", _dateRound1EmailSent.DBValue);
            cm.Parameters.AddWithValue("@DateRound2EmailSent", _dateRound2EmailSent.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Userdata1", _userdata1);
            cm.Parameters.AddWithValue("@Userdata2", _userdata2);
            cm.Parameters.AddWithValue("@Userdata3", _userdata3);
            cm.Parameters.AddWithValue("@IsLocked", _isLocked);
            cm.Parameters.AddWithValue("@IsRead", _isRead);
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
                cm.CommandText = "DeleteInvitation";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
