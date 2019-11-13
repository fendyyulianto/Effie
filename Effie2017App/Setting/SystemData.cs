using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class SystemData : Csla.BusinessBase<SystemData>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private SmartDate _dateLastReminded = new SmartDate(false);
        private string _activePanelsRound1 = string.Empty;
        private string _activePanelsRound2 = string.Empty;
        private string _activeRound = string.Empty;

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public DateTime DateLastReminded
        {
            get
            {
                CanReadProperty("DateLastReminded", true);
                return _dateLastReminded.Date;
            }
        }

        public string DateLastRemindedString
        {
            get
            {
                CanReadProperty("DateLastReminded", true);
                return _dateLastReminded.Text;
            }
            set
            {
                CanWriteProperty("DateLastReminded", true);
                if (value == null) value = string.Empty;
                if (!_dateLastReminded.Equals(value))
                {
                    _dateLastReminded.Text = value;
                    PropertyHasChanged("DateLastReminded");
                }
            }
        }

        public string ActivePanelsRound1
        {
            get
            {
                CanReadProperty("ActivePanelsRound1", true);
                return _activePanelsRound1;
            }
            set
            {
                CanWriteProperty("ActivePanelsRound1", true);
                if (value == null) value = string.Empty;
                if (!_activePanelsRound1.Equals(value))
                {
                    _activePanelsRound1 = value;
                    PropertyHasChanged("ActivePanelsRound1");
                }
            }
        }

        public string ActivePanelsRound2
        {
            get
            {
                CanReadProperty("ActivePanelsRound2", true);
                return _activePanelsRound2;
            }
            set
            {
                CanWriteProperty("ActivePanelsRound2", true);
                if (value == null) value = string.Empty;
                if (!_activePanelsRound2.Equals(value))
                {
                    _activePanelsRound2 = value;
                    PropertyHasChanged("ActivePanelsRound2");
                }
            }
        }

        public string ActiveRound
        {
            get
            {
                CanReadProperty("ActiveRound", true);
                return _activeRound;
            }
            set
            {
                CanWriteProperty("ActiveRound", true);
                if (value == null) value = string.Empty;
                if (!_activeRound.Equals(value))
                {
                    _activeRound = value;
                    PropertyHasChanged("ActiveRound");
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
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ActivePanelsRound1", 200));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ActivePanelsRound2", 200));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ActiveRound", 5));
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
            //TODO: Define authorization rules in SystemData
            //AuthorizationRules.AllowRead("Id", "SystemDataReadGroup");
            //AuthorizationRules.AllowRead("DateLastReminded", "SystemDataReadGroup");

            //AuthorizationRules.AllowWrite("DateLastReminded", "SystemDataWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in SystemData
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SystemDataViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in SystemData
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SystemDataAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in SystemData
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SystemDataEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in SystemData
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SystemDataDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private SystemData()
        { /* require use of factory method */ }

        public static SystemData NewSystemData()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a SystemData");
            return DataPortal.Create<SystemData>();
        }

        public static SystemData GetSystemData(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a SystemData");
            return DataPortal.Fetch<SystemData>(new Criteria(id));
        }

        public static void DeleteSystemData(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a SystemData");
            DataPortal.Delete(new Criteria(id));
        }

        public override SystemData Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a SystemData");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a SystemData");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a SystemData");

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
                cm.CommandText = "GetSystemData";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    FetchObject(dr);
                    ValidationRules.CheckRules();

                    //load child object(s)
                    FetchChildren(dr);
                }
            }//using
        }

        private void FetchObject(SafeDataReader dr)
        {
            dr.Read();
            _id = dr.GetGuid("Id");
            _dateLastReminded = dr.GetSmartDate("DateLastReminded", _dateLastReminded.EmptyIsMin);
            _activePanelsRound1 = dr.GetString("ActivePanelsRound1");
            _activePanelsRound2 = dr.GetString("ActivePanelsRound2");
            _activeRound = dr.GetString("ActiveRound");
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
                cm.CommandText = "AddSystemData";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@DateLastReminded", _dateLastReminded.DBValue);
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
                cm.CommandText = "UpdateSystemData";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@DateLastReminded", _dateLastReminded.DBValue);
            cm.Parameters.AddWithValue("@ActivePanelsRound1", _activePanelsRound1);
            cm.Parameters.AddWithValue("@ActivePanelsRound2", _activePanelsRound2);
            cm.Parameters.AddWithValue("@ActiveRound", _activeRound);
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
                cm.CommandText = "DeleteSystemData";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
