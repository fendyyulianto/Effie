using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class RSVP : Csla.BusinessBase<RSVP>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _type = string.Empty;
        private string _salutation = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _company = string.Empty;
        private string _dietary = string.Empty;
        private bool _isJuryCocktail = false;
        private bool _isWelcomeDinner = false;
        private bool _isGalaDinner = false;
        private bool _respond = false;
        private string _userData1 = string.Empty;
        private string _userData2 = string.Empty;
        private string _userData3 = string.Empty;
        private string _userData4 = string.Empty;
        private string _userData5 = string.Empty;
        private bool _isInvitingGalaDinner = false;
        private string _round1PanelID = string.Empty;
        private string _round2PanelID = string.Empty;
        private string _workflowStatus = string.Empty;
        private string _location = string.Empty;
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

        public string Type
        {
            get
            {
                CanReadProperty("Type", true);
                return _type;
            }
            set
            {
                CanWriteProperty("Type", true);
                if (value == null) value = string.Empty;
                if (!_type.Equals(value))
                {
                    _type = value;
                    PropertyHasChanged("Type");
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

        public string FirstName
        {
            get
            {
                CanReadProperty("FirstName", true);
                return _firstName;
            }
            set
            {
                CanWriteProperty("FirstName", true);
                if (value == null) value = string.Empty;
                if (!_firstName.Equals(value))
                {
                    _firstName = value;
                    PropertyHasChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get
            {
                CanReadProperty("LastName", true);
                return _lastName;
            }
            set
            {
                CanWriteProperty("LastName", true);
                if (value == null) value = string.Empty;
                if (!_lastName.Equals(value))
                {
                    _lastName = value;
                    PropertyHasChanged("LastName");
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

        public string Dietary
        {
            get
            {
                CanReadProperty("Dietary", true);
                return _dietary;
            }
            set
            {
                CanWriteProperty("Dietary", true);
                if (value == null) value = string.Empty;
                if (!_dietary.Equals(value))
                {
                    _dietary = value;
                    PropertyHasChanged("Dietary");
                }
            }
        }

        public bool IsJuryCocktail
        {
            get
            {
                CanReadProperty("IsJuryCocktail", true);
                return _isJuryCocktail;
            }
            set
            {
                CanWriteProperty("IsJuryCocktail", true);
                if (!_isJuryCocktail.Equals(value))
                {
                    _isJuryCocktail = value;
                    PropertyHasChanged("IsJuryCocktail");
                }
            }
        }

        public bool IsWelcomeDinner
        {
            get
            {
                CanReadProperty("IsWelcomeDinner", true);
                return _isWelcomeDinner;
            }
            set
            {
                CanWriteProperty("IsWelcomeDinner", true);
                if (!_isWelcomeDinner.Equals(value))
                {
                    _isWelcomeDinner = value;
                    PropertyHasChanged("IsWelcomeDinner");
                }
            }
        }

        public bool IsGalaDinner
        {
            get
            {
                CanReadProperty("IsGalaDinner", true);
                return _isGalaDinner;
            }
            set
            {
                CanWriteProperty("IsGalaDinner", true);
                if (!_isGalaDinner.Equals(value))
                {
                    _isGalaDinner = value;
                    PropertyHasChanged("IsGalaDinner");
                }
            }
        }

        public bool Respond
        {
            get
            {
                CanReadProperty("Respond", true);
                return _respond;
            }
            set
            {
                CanWriteProperty("Respond", true);
                if (!_respond.Equals(value))
                {
                    _respond = value;
                    PropertyHasChanged("Respond");
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

        public string UserData4
        {
            get
            {
                CanReadProperty("UserData4", true);
                return _userData4;
            }
            set
            {
                CanWriteProperty("UserData4", true);
                if (value == null) value = string.Empty;
                if (!_userData4.Equals(value))
                {
                    _userData4 = value;
                    PropertyHasChanged("UserData4");
                }
            }
        }

        public string UserData5
        {
            get
            {
                CanReadProperty("UserData5", true);
                return _userData5;
            }
            set
            {
                CanWriteProperty("UserData5", true);
                if (value == null) value = string.Empty;
                if (!_userData5.Equals(value))
                {
                    _userData5 = value;
                    PropertyHasChanged("UserData5");
                }
            }
        }

        public bool IsInvitingGalaDinner
        {
            get
            {
                CanReadProperty("IsInvitingGalaDinner", true);
                return _isInvitingGalaDinner;
            }
            set
            {
                CanWriteProperty("IsInvitingGalaDinner", true);
                if (!_isInvitingGalaDinner.Equals(value))
                {
                    _isInvitingGalaDinner = value;
                    PropertyHasChanged("IsInvitingGalaDinner");
                }
            }
        }

        public string Round1PanelID
        {
            get
            {
                CanReadProperty("Round1PanelID", true);
                return _round1PanelID;
            }
            set
            {
                CanWriteProperty("Round1PanelID", true);
                if (value == null) value = string.Empty;
                if (!_round1PanelID.Equals(value))
                {
                    _round1PanelID = value;
                    PropertyHasChanged("Round1PanelID");
                }
            }
        }

        public string Round2PanelID
        {
            get
            {
                CanReadProperty("Round2PanelID", true);
                return _round2PanelID;
            }
            set
            {
                CanWriteProperty("Round2PanelID", true);
                if (value == null) value = string.Empty;
                if (!_round2PanelID.Equals(value))
                {
                    _round2PanelID = value;
                    PropertyHasChanged("Round2PanelID");
                }
            }
        }

        public string WorkflowStatus
        {
            get
            {
                CanReadProperty("WorkflowStatus", true);
                return _workflowStatus;
            }
            set
            {
                CanWriteProperty("WorkflowStatus", true);
                if (value == null) value = string.Empty;
                if (!_workflowStatus.Equals(value))
                {
                    _workflowStatus = value;
                    PropertyHasChanged("WorkflowStatus");
                }
            }
        }

        public string Location
        {
            get
            {
                CanReadProperty("Location", true);
                return _location;
            }
            set
            {
                CanWriteProperty("Location", true);
                if (value == null) value = string.Empty;
                if (!_location.Equals(value))
                {
                    _location = value;
                    PropertyHasChanged("Location");
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
            // Type
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Type", 50));
            //
            // Salutation
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Salutation", 50));
            //
            // FirstName
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("FirstName", 200));
            //
            // LastName
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LastName", 200));
            //
            // Email
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Email", 200));
            //
            // Company
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Company", 200));
            //
            // Dietary
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Dietary", 2000));
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
            //
            // UserData4
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserData4", 500));
            //
            // UserData5
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("UserData5", 500));
            //
            // Round1PanelID
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Round1PanelID", 100));
            //
            // Round2PanelID
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Round2PanelID", 100));
            //
            // WorkflowStatus
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("WorkflowStatus", 50));
            //
            // Location
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Location", 50));
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
            //TODO: Define authorization rules in RSVP
            //AuthorizationRules.AllowRead("Id", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("Name", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("Email", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("Company", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("Dietary", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("IsJuryCocktail", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("IsWelcomeDinner", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("Respond", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("UserData1", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("UserData2", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("UserData3", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("WorkflowStatus", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("Location", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "RSVPReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "RSVPReadGroup");

            //AuthorizationRules.AllowWrite("Name", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("Email", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("Company", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("Dietary", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("IsJuryCocktail", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("IsWelcomeDinner", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("Respond", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("UserData1", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("UserData2", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("UserData3", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("WorkflowStatus", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("Location", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "RSVPWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "RSVPWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in RSVP
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RSVPViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in RSVP
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RSVPAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in RSVP
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RSVPEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in RSVP
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RSVPDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private RSVP()
        { /* require use of factory method */ }

        private RSVP(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static RSVP NewRSVP()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a RSVP");
            return DataPortal.Create<RSVP>();
        }

        public static RSVP GetRSVP(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a RSVP");
            return DataPortal.Fetch<RSVP>(new Criteria(id));
        }

        public static RSVP GetRSVP(SafeDataReader dr)
        {
            return new RSVP(dr);
        }

        public static void DeleteRSVP(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a RSVP");
            DataPortal.Delete(new Criteria(id));
        }

        public override RSVP Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a RSVP");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a RSVP");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a RSVP");

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
                cm.CommandText = "GetRSVP";

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
            _type = dr.GetString("Type");
            _salutation = dr.GetString("Salutation");
            _firstName = dr.GetString("FirstName");
            _lastName = dr.GetString("LastName");
            _email = dr.GetString("Email");
            _company = dr.GetString("Company");
            _dietary = dr.GetString("Dietary");
            _isJuryCocktail = dr.GetBoolean("IsJuryCocktail");
            _isWelcomeDinner = dr.GetBoolean("IsWelcomeDinner");
            _isGalaDinner = dr.GetBoolean("IsGalaDinner");
            _respond = dr.GetBoolean("RSVPRespond");
            _userData1 = dr.GetString("UserData1");
            _userData2 = dr.GetString("UserData2");
            _userData3 = dr.GetString("UserData3");
            _userData4 = dr.GetString("UserData4");
            _userData5 = dr.GetString("UserData5");
            _isInvitingGalaDinner = dr.GetBoolean("IsInvitingGalaDinner");
            _round1PanelID = dr.GetString("Round1PanelID");
            _round2PanelID = dr.GetString("Round2PanelID");
            _workflowStatus = dr.GetString("WorkflowStatus");
            _location = dr.GetString("Location");
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
                cm.CommandText = "AddRSVP";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Type", _type);
            cm.Parameters.AddWithValue("@Salutation", _salutation);
            cm.Parameters.AddWithValue("@FirstName", _firstName);
            cm.Parameters.AddWithValue("@LastName", _lastName);
            cm.Parameters.AddWithValue("@Email", _email);
            cm.Parameters.AddWithValue("@Company", _company);
            cm.Parameters.AddWithValue("@Dietary", _dietary);
            cm.Parameters.AddWithValue("@IsJuryCocktail", _isJuryCocktail);
            cm.Parameters.AddWithValue("@IsWelcomeDinner", _isWelcomeDinner);
            cm.Parameters.AddWithValue("@IsGalaDinner", _isGalaDinner);
            cm.Parameters.AddWithValue("@RSVPRespond", _respond);
            cm.Parameters.AddWithValue("@UserData1", _userData1);
            cm.Parameters.AddWithValue("@UserData2", _userData2);
            cm.Parameters.AddWithValue("@UserData3", _userData3);
            cm.Parameters.AddWithValue("@UserData4", _userData4);
            cm.Parameters.AddWithValue("@UserData5", _userData5);
            cm.Parameters.AddWithValue("@IsInvitingGalaDinner", _isInvitingGalaDinner);
            cm.Parameters.AddWithValue("@Round1PanelID", _round1PanelID);
            cm.Parameters.AddWithValue("@Round2PanelID", _round2PanelID);            
            cm.Parameters.AddWithValue("@WorkflowStatus", _workflowStatus);
            cm.Parameters.AddWithValue("@Location", _location);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
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
                cm.CommandText = "UpdateRSVP";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Type", _type);
            cm.Parameters.AddWithValue("@Salutation", _salutation);
            cm.Parameters.AddWithValue("@FirstName", _firstName);
            cm.Parameters.AddWithValue("@LastName", _lastName);
            cm.Parameters.AddWithValue("@Email", _email);
            cm.Parameters.AddWithValue("@Company", _company);
            cm.Parameters.AddWithValue("@Dietary", _dietary);
            cm.Parameters.AddWithValue("@IsJuryCocktail", _isJuryCocktail);
            cm.Parameters.AddWithValue("@IsWelcomeDinner", _isWelcomeDinner);
            cm.Parameters.AddWithValue("@IsGalaDinner", _isGalaDinner);
            cm.Parameters.AddWithValue("@RSVPRespond", _respond);
            cm.Parameters.AddWithValue("@UserData1", _userData1);
            cm.Parameters.AddWithValue("@UserData2", _userData2);
            cm.Parameters.AddWithValue("@UserData3", _userData3);
            cm.Parameters.AddWithValue("@UserData4", _userData4);
            cm.Parameters.AddWithValue("@UserData5", _userData5);
            cm.Parameters.AddWithValue("@IsInvitingGalaDinner", _isInvitingGalaDinner);
            cm.Parameters.AddWithValue("@Round1PanelID", _round1PanelID);
            cm.Parameters.AddWithValue("@Round2PanelID", _round2PanelID);
            cm.Parameters.AddWithValue("@WorkflowStatus", _workflowStatus);
            cm.Parameters.AddWithValue("@Location", _location);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
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
                cm.CommandText = "DeleteRSVP";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
