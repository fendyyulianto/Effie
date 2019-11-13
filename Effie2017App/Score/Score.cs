using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class Score : Csla.BusinessBase<Score>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _entryId = Guid.Empty;
        private Guid _juryid = Guid.Empty;
        private int _scoresc = 0;
        private int _scoreid = 0;
        private int _scoreil = 0;
        private int _scorere = 0;
        private double _scorecomposite = 0;
        private string _feedbackStrong = string.Empty;
        private string _feedbackWeak = string.Empty;
        private string _flag = string.Empty;
        private string _flagOthers = string.Empty;
        private bool _isRecuse = false;
        private bool _isSubmitted = false;
        private bool _isAdminRecuse = false;
        private string _recuseRemarks = string.Empty;
        private string _additionalComments = string.Empty;
        private bool _isAdvancement = false;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateSubmitted = new SmartDate(false);
        private string _round = string.Empty;
        private string _Nomination = string.Empty;
        

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

        public Guid Juryid
        {
            get
            {
                CanReadProperty("Juryid", true);
                return _juryid;
            }
            set
            {
                CanWriteProperty("Juryid", true);
                if (value == null) value = Guid.Empty;
                if (!_juryid.Equals(value))
                {
                    _juryid = value;
                    PropertyHasChanged("Juryid");
                }
            }
        }

        public int ScoreSC
        {
            get
            {
                CanReadProperty("ScoreSC", true);
                return _scoresc;
            }
            set
            {
                CanWriteProperty("ScoreSC", true);
                if (!_scoresc.Equals(value))
                {
                    _scoresc = value;
                    PropertyHasChanged("ScoreSC");
                }
            }
        }

        public int ScoreID
        {
            get
            {
                CanReadProperty("ScoreID", true);
                return _scoreid;
            }
            set
            {
                CanWriteProperty("ScoreID", true);
                if (!_scoreid.Equals(value))
                {
                    _scoreid = value;
                    PropertyHasChanged("ScoreID");
                }
            }
        }

        public int ScoreIL
        {
            get
            {
                CanReadProperty("ScoreIL", true);
                return _scoreil;
            }
            set
            {
                CanWriteProperty("ScoreIL", true);
                if (!_scoreil.Equals(value))
                {
                    _scoreil = value;
                    PropertyHasChanged("ScoreIL");
                }
            }
        }

        public int ScoreRE
        {
            get
            {
                CanReadProperty("ScoreRE", true);
                return _scorere;
            }
            set
            {
                CanWriteProperty("ScoreRE", true);
                if (!_scorere.Equals(value))
                {
                    _scorere = value;
                    PropertyHasChanged("ScoreRE");
                }
            }
        }

        public double ScoreComposite
        {
            get
            {
                CanReadProperty("ScoreComposite", true);
                return _scorecomposite;
            }
            set
            {
                CanWriteProperty("ScoreComposite", true);
                if (!_scorecomposite.Equals(value))
                {
                    _scorecomposite = value;
                    PropertyHasChanged("ScoreComposite");
                }
            }
        }

        public string FeedbackStrong
        {
            get
            {
                CanReadProperty("FeedbackStrong", true);
                return _feedbackStrong;
            }
            set
            {
                CanWriteProperty("FeedbackStrong", true);
                if (value == null) value = string.Empty;
                if (!_feedbackStrong.Equals(value))
                {
                    _feedbackStrong = value;
                    PropertyHasChanged("FeedbackStrong");
                }
            }
        }

        public string FeedbackWeak
        {
            get
            {
                CanReadProperty("FeedbackWeak", true);
                return _feedbackWeak;
            }
            set
            {
                CanWriteProperty("FeedbackWeak", true);
                if (value == null) value = string.Empty;
                if (!_feedbackWeak.Equals(value))
                {
                    _feedbackWeak = value;
                    PropertyHasChanged("FeedbackWeak");
                }
            }
        }


        public string Flag
        {
            get
            {
                CanReadProperty("Flag", true);
                return _flag;
            }
            set
            {
                CanWriteProperty("Flag", true);
                if (value == null) value = string.Empty;
                if (!_flag.Equals(value))
                {
                    _flag = value;
                    PropertyHasChanged("Flag");
                }
            }
        }

        public string Flags
        {
            get
            {
                return _isRecuse ? "Jury Recusal" : _flag;
            }
        }
        

        public string FlagOthers
        {
            get
            {
                CanReadProperty("FlagOthers", true);
                return _flagOthers;
            }
            set
            {
                CanWriteProperty("FlagOthers", true);
                if (value == null) value = string.Empty;
                if (!_flagOthers.Equals(value))
                {
                    _flagOthers = value;
                    PropertyHasChanged("FlagOthers");
                }
            }
        }

        public bool IsRecuse
        {
            get
            {
                CanReadProperty("IsRecuse", true);
                return _isRecuse;
            }
            set
            {
                CanWriteProperty("IsRecuse", true);
                if (!_isRecuse.Equals(value))
                {
                    _isRecuse = value;
                    PropertyHasChanged("IsRecuse");
                }
            }
        }

        public bool IsSubmitted
        {
            get
            {
                CanReadProperty("IsSubmitted", true);
                return _isSubmitted;
            }
            set
            {
                CanWriteProperty("IsSubmitted", true);
                if (!_isSubmitted.Equals(value))
                {
                    _isSubmitted = value;
                    PropertyHasChanged("IsSubmitted");
                }
            }
        }

        public bool IsAdminRecuse
        {
            get
            {
                CanReadProperty("IsAdminRecuse", true);
                return _isAdminRecuse;
            }
            set
            {
                CanWriteProperty("IsAdminRecuse", true);
                if (!_isAdminRecuse.Equals(value))
                {
                    _isAdminRecuse = value;
                    PropertyHasChanged("IsAdminRecuse");
                }
            }
        }

        public string RecuseRemarks
        {
            get
            {
                CanReadProperty("RecuseRemarks", true);
                return _recuseRemarks;
            }
            set
            {
                CanWriteProperty("RecuseRemarks", true);
                if (value == null) value = string.Empty;
                if (!_recuseRemarks.Equals(value))
                {
                    _recuseRemarks = value;
                    PropertyHasChanged("RecuseRemarks");
                }
            }
        }

        public string AdditionalComments
        {
            get
            {
                CanReadProperty("AdditionalComments", true);
                return _additionalComments;
            }
            set
            {
                CanWriteProperty("AdditionalComments", true);
                if (value == null) value = string.Empty;
                if (!_additionalComments.Equals(value))
                {
                    _additionalComments = value;
                    PropertyHasChanged("AdditionalComments");
                }
            }
        }

        public bool IsAdvancement
        {
            get
            {
                CanReadProperty("IsAdvancement", true);
                return _isAdvancement;
            }
            set
            {
                CanWriteProperty("IsAdvancement", true);
                if (!_isAdvancement.Equals(value))
                {
                    _isAdvancement = value;
                    PropertyHasChanged("IsAdvancement");
                }
            }
        }

        public DateTime DateSubmitted
        {
            get
            {
                CanReadProperty("DateSubmitted", true);
                if (_dateSubmitted.Date == DateTime.MaxValue) _dateSubmitted.Date = DateTime.MinValue;
                return _dateSubmitted.Date;
            }
        }

        public string DateSubmittedString
        {
            get
            {
                CanReadProperty("DateSubmitted", true);
                return _dateSubmitted.Text;
            }
            set
            {
                CanWriteProperty("DateSubmitted", true);
                if (value == null) value = string.Empty;
                if (!_dateSubmitted.Equals(value))
                {
                    _dateSubmitted.Text = value;
                    PropertyHasChanged("DateSubmitted");
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

        public string Round
        {
            get
            {
                CanReadProperty("Round", true);
                return _round;
            }
            set
            {
                CanWriteProperty("Round", true);
                if (value == null) value = string.Empty;
                if (!_round.Equals(value))
                {
                    _round = value;
                    PropertyHasChanged("Round");
                }
            }
        }

        public string Nomination
        {
            get
            {
                CanReadProperty("Nomination", true);
                return _Nomination;
            }
            set
            {
                CanWriteProperty("Nomination", true);
                if (value == null) value = string.Empty;
                if (!_Nomination.Equals(value))
                {
                    _Nomination = value;
                    PropertyHasChanged("Nomination");
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
            // FeedbackStrong
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("FeedbackStrong", 1000));
            //
            // FeedbackWeak
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("FeedbackWeak", 1000));
            //
            // Flag
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Flag", 100));
            //
            // FlagOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("FlagOthers", 100));
            //
            // RecuseRemarks
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RecuseRemarks", 1000));
            //
            // AdditionalComments
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("AdditionalComments", 1000));
            //
            // round
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Round", 5));
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
            //TODO: Define authorization rules in Score
            //AuthorizationRules.AllowRead("Id", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("EntryId", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("Juryid", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("SC", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("ID", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("IL", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("RE", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("Composite", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("Feedback", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("Flag", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("FlagOthers", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("IsRecuse", "ScoreReadGroup");
            //AuthorizationRules.AllowRead("IsSubmitted", "ScoreReadGroup");

            //AuthorizationRules.AllowWrite("SC", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("ID", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("IL", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("RE", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("Composite", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("Feedback", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("Flag", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("FlagOthers", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("IsRecuse", "ScoreWriteGroup");
            //AuthorizationRules.AllowWrite("IsSubmitted", "ScoreWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Score
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("ScoreViewGroup"))
            //    return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Score
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("ScoreAddGroup"))
            //    return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Score
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("ScoreEditGroup"))
            //    return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Score
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("ScoreDeleteGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Score()
        { /* require use of factory method */ }

        private Score(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static Score NewScore()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Score");
            return DataPortal.Create<Score>();
        }

        public static Score GetScore(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Score");
            return DataPortal.Fetch<Score>(new Criteria(id));
        }

        public static Score GetScore(SafeDataReader dr)
        {
            return new Score(dr);
        }

        public static void DeleteScore(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Score");
            DataPortal.Delete(new Criteria(id));
        }

        public override Score Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Score");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Score");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a Score");

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
                cm.CommandText = "GetScore";

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
            _juryid = dr.GetGuid("Juryid");
            _scoresc = dr.GetInt32("ScoreSC");
            _scoreid = dr.GetInt32("ScoreID");
            _scoreil = dr.GetInt32("ScoreIL");
            _scorere = dr.GetInt32("ScoreRE");
            _scorecomposite = dr.GetDouble("ScoreComposite");
            _feedbackStrong = dr.GetString("FeedbackStrong");
            _feedbackWeak = dr.GetString("FeedbackWeak");
            _flag = dr.GetString("Flag");
            _flagOthers = dr.GetString("FlagOthers");
            _isRecuse = dr.GetBoolean("IsRecuse");
            _isSubmitted = dr.GetBoolean("IsSubmitted");
            _isAdminRecuse = dr.GetBoolean("IsAdminRecuse");
            _recuseRemarks = dr.GetString("RecuseRemarks");
            _additionalComments = dr.GetString("AdditionalComments");
            _isAdvancement = dr.GetBoolean("IsAdvancement");
            _dateSubmitted = dr.GetSmartDate("DateSubmitted", _dateSubmitted.EmptyIsMin);
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _round = dr.GetString("Round");
            _Nomination = dr.GetString("Nomination");
            
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
                cm.CommandText = "AddScore";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
                _entryId = (Guid)cm.Parameters["@EntryId"].Value;
                _juryid = (Guid)cm.Parameters["@Juryid"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            
            cm.Parameters.AddWithValue("@ScoreSC", _scoresc);           
            cm.Parameters.AddWithValue("@ScoreID", _scoreid);           
            cm.Parameters.AddWithValue("@ScoreIL", _scoreil);           
            cm.Parameters.AddWithValue("@ScoreRE", _scorere);            
            cm.Parameters.AddWithValue("@ScoreComposite", _scorecomposite);
            cm.Parameters.AddWithValue("@FeedbackStrong", _feedbackStrong);
            cm.Parameters.AddWithValue("@FeedbackWeak", _feedbackWeak);
            cm.Parameters.AddWithValue("@Flag", _flag);            
            cm.Parameters.AddWithValue("@FlagOthers", _flagOthers);           
            cm.Parameters.AddWithValue("@IsRecuse", _isRecuse);            
            cm.Parameters.AddWithValue("@IsSubmitted", _isSubmitted);            
            cm.Parameters.AddWithValue("@Id", _id);
            // cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
            cm.Parameters.AddWithValue("@EntryId", _entryId);
            // cm.Parameters["@NewEntryId"].Direction = ParameterDirection.Output;
            cm.Parameters.AddWithValue("@Juryid", _juryid);
            //cm.Parameters["@NewJuryid"].Direction = ParameterDirection.Output;
            cm.Parameters.AddWithValue("@IsAdminRecuse", _isAdminRecuse);
            cm.Parameters.AddWithValue("@RecuseRemarks", _recuseRemarks);
            cm.Parameters.AddWithValue("@AdditionalComments", _additionalComments);
            cm.Parameters.AddWithValue("@IsAdvancement", _isAdvancement);
            cm.Parameters.AddWithValue("@DateSubmitted", _dateSubmitted.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@Round", _round);
            cm.Parameters.AddWithValue("@Nomination", _Nomination);
            
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
                cm.CommandText = "UpdateScore";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@EntryId", _entryId);
            cm.Parameters.AddWithValue("@Juryid", _juryid);
            
            cm.Parameters.AddWithValue("@ScoreSC", _scoresc);            
            cm.Parameters.AddWithValue("@ScoreID", _scoreid);           
            cm.Parameters.AddWithValue("@ScoreIL", _scoreil);            
            cm.Parameters.AddWithValue("@ScoreRE", _scorere);            
            cm.Parameters.AddWithValue("@ScoreComposite", _scorecomposite);
            cm.Parameters.AddWithValue("@FeedbackStrong", _feedbackStrong);
            cm.Parameters.AddWithValue("@FeedbackWeak", _feedbackWeak);           
            cm.Parameters.AddWithValue("@Flag", _flag);            
            cm.Parameters.AddWithValue("@FlagOthers", _flagOthers);           
            cm.Parameters.AddWithValue("@IsRecuse", _isRecuse);            
            cm.Parameters.AddWithValue("@IsSubmitted", _isSubmitted);
            cm.Parameters.AddWithValue("@IsAdminRecuse", _isAdminRecuse);
            cm.Parameters.AddWithValue("@RecuseRemarks", _recuseRemarks);
            cm.Parameters.AddWithValue("@AdditionalComments", _additionalComments);
            cm.Parameters.AddWithValue("@IsAdvancement", _isAdvancement);
            cm.Parameters.AddWithValue("@DateSubmitted", _dateSubmitted.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@Round", _round);
            cm.Parameters.AddWithValue("@Nomination", _Nomination);
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
                cm.CommandText = "DeleteScore";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}