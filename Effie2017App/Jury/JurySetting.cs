using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class JurySetting : Csla.BusinessBase<JurySetting>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _juryCode = string.Empty;
        private int _juryNumber = 0;

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public string JuryCode
        {
            get
            {
                CanReadProperty("JuryCode", true);
                return _juryCode;
            }
            set
            {
                CanWriteProperty("JuryCode", true);
                if (value == null) value = string.Empty;
                if (!_juryCode.Equals(value))
                {
                    _juryCode = value;
                    PropertyHasChanged("JuryCode");
                }
            }
        }

        public int JuryNumber
        {
            get
            {
                CanReadProperty("JuryNumber", true);
                return _juryNumber;
            }
            set
            {
                CanWriteProperty("JuryNumber", true);
                if (!_juryNumber.Equals(value))
                {
                    _juryNumber = value;
                    PropertyHasChanged("JuryNumber");
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
            // JuryCode
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JuryCode", 5));
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
            //TODO: Define authorization rules in JurySetting
            //AuthorizationRules.AllowRead("Id", "JurySettingReadGroup");
            //AuthorizationRules.AllowRead("JuryCode", "JurySettingReadGroup");
            //AuthorizationRules.AllowRead("JuryNumber", "JurySettingReadGroup");

            //AuthorizationRules.AllowWrite("JuryCode", "JurySettingWriteGroup");
            //AuthorizationRules.AllowWrite("JuryNumber", "JurySettingWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in JurySetting
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JurySettingViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in JurySetting
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JurySettingAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in JurySetting
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JurySettingEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in JurySetting
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JurySettingDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private JurySetting()
        { /* require use of factory method */ }

        public static JurySetting NewJurySetting()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a JurySetting");
            return DataPortal.Create<JurySetting>();
        }

        public static JurySetting GetJurySetting(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JurySetting");
            return DataPortal.Fetch<JurySetting>(new Criteria(id));
        }

        public static JurySetting GetJurySetting(string code)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JurySetting");
            return DataPortal.Fetch<JurySetting>(new Criteria(code));
        }

        public static void DeleteJurySetting(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a JurySetting");
            DataPortal.Delete(new Criteria(id));
        }

        public override JurySetting Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a JurySetting");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a JurySetting");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a JurySetting");

            return base.Save();
        }

        #endregion //Factory Methods

        #region Data Access

        #region Criteria

        [Serializable()]
        private class Criteria
        {
            public Guid Id = Guid.Empty;
            public string Code = "";
            public Criteria(Guid id)
            {
                this.Id = id;
            }
            public Criteria(string code)
            {
                this.Code = code;
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
                cm.CommandText = "GetJurySetting";

                cm.Parameters.AddWithValue("@Id", criteria.Id);
                cm.Parameters.AddWithValue("@Code", criteria.Code);

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
            _juryCode = dr.GetString("JuryCode");
            _juryNumber = dr.GetInt32("JuryNumber");
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
                cm.CommandText = "AddJurySetting";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

               // _id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            if (_juryCode != string.Empty)
                cm.Parameters.AddWithValue("@JuryCode", _juryCode);
            else
                cm.Parameters.AddWithValue("@JuryCode", DBNull.Value);
            if (_juryNumber != 0)
                cm.Parameters.AddWithValue("@JuryNumber", _juryNumber);
            else
                cm.Parameters.AddWithValue("@JuryNumber", DBNull.Value);
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
                cm.CommandText = "UpdateJurySetting";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            if (_juryCode != string.Empty)
                cm.Parameters.AddWithValue("@JuryCode", _juryCode);
            else
                cm.Parameters.AddWithValue("@JuryCode", DBNull.Value);
            if (_juryNumber != 0)
                cm.Parameters.AddWithValue("@JuryNumber", _juryNumber);
            else
                cm.Parameters.AddWithValue("@JuryNumber", DBNull.Value);
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
                cm.CommandText = "DeleteJurySetting";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
