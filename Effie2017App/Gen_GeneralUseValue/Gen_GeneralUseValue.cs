using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class Gen_GeneralUseValue : Csla.BusinessBase<Gen_GeneralUseValue>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _code = string.Empty;
        private string _value = string.Empty;
        private string _description = string.Empty;
        private SmartDate _dateModified = new SmartDate(false);
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

        public string Code
        {
            get
            {
                CanReadProperty("Code", true);
                return _code;
            }
            set
            {
                CanWriteProperty("Code", true);
                if (value == null) value = string.Empty;
                if (!_code.Equals(value))
                {
                    _code = value;
                    PropertyHasChanged("Code");
                }
            }
        }

        public string Value
        {
            get
            {
                CanReadProperty("Value", true);
                return _value;
            }
            set
            {
                CanWriteProperty("Value", true);
                if (value == null) value = string.Empty;
                if (!_value.Equals(value))
                {
                    _value = value;
                    PropertyHasChanged("Value");
                }
            }
        }

        public string Description
        {
            get
            {
                CanReadProperty("Description", true);
                return _description;
            }
            set
            {
                CanWriteProperty("Description", true);
                if (value == null) value = string.Empty;
                if (!_description.Equals(value))
                {
                    _description = value;
                    PropertyHasChanged("Description");
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
            // Code
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Code", 50));
            //
            // Value
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Value", 200));
            //
            // Description
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Description", 300));
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
            //TODO: Define authorization rules in Gen_GeneralUseValue
            //AuthorizationRules.AllowRead("Id", "Gen_GeneralUseValueReadGroup");
            //AuthorizationRules.AllowRead("Code", "Gen_GeneralUseValueReadGroup");
            //AuthorizationRules.AllowRead("Value", "Gen_GeneralUseValueReadGroup");
            //AuthorizationRules.AllowRead("Description", "Gen_GeneralUseValueReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "Gen_GeneralUseValueReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "Gen_GeneralUseValueReadGroup");

            //AuthorizationRules.AllowWrite("Code", "Gen_GeneralUseValueWriteGroup");
            //AuthorizationRules.AllowWrite("Value", "Gen_GeneralUseValueWriteGroup");
            //AuthorizationRules.AllowWrite("Description", "Gen_GeneralUseValueWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "Gen_GeneralUseValueWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "Gen_GeneralUseValueWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Gen_GeneralUseValue
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("Gen_GeneralUseValueViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Gen_GeneralUseValue
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("Gen_GeneralUseValueAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Gen_GeneralUseValue
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("Gen_GeneralUseValueEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Gen_GeneralUseValue
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("Gen_GeneralUseValueDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Gen_GeneralUseValue()
        { /* require use of factory method */ }

        private Gen_GeneralUseValue(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static Gen_GeneralUseValue NewGen_GeneralUseValue()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Gen_GeneralUseValue");
            return DataPortal.Create<Gen_GeneralUseValue>();
        }

        public static Gen_GeneralUseValue GetGen_GeneralUseValue(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Gen_GeneralUseValue");
            return DataPortal.Fetch<Gen_GeneralUseValue>(new Criteria(id));
        }

        public static Gen_GeneralUseValue GetGen_GeneralUseValue(SafeDataReader dr)
        {
            return new Gen_GeneralUseValue(dr);
        }

        public static void DeleteGen_GeneralUseValue(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Gen_GeneralUseValue");
            DataPortal.Delete(new Criteria(id));
        }

        public override Gen_GeneralUseValue Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Gen_GeneralUseValue");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Gen_GeneralUseValue");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a Gen_GeneralUseValue");

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
                cm.CommandText = "GetGen_GeneralUseValue";

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
            _code = dr.GetString("Code");
            _value = dr.GetString("Value");
            _description = dr.GetString("Description");
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
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
                cm.CommandText = "AddGen_GeneralUseValue";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {           
            cm.Parameters.AddWithValue("@Code", _code);           
            cm.Parameters.AddWithValue("@Value", _value);         
            cm.Parameters.AddWithValue("@Description", _description);           
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
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
                cm.CommandText = "UpdateGen_GeneralUseValue";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Code", _code);
            cm.Parameters.AddWithValue("@Value", _value);
            cm.Parameters.AddWithValue("@Description", _description);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
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
                cm.CommandText = "DeleteGen_GeneralUseValue";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
