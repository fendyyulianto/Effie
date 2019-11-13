using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class Setting : Csla.BusinessBase<Setting>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _orderNo = string.Empty;
        private string _invoiceNo = string.Empty;
        private string _categoryCode = string.Empty;
        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public string OrderNo
        {
            get
            {
                CanReadProperty("OrderNo", true);
                return _orderNo;
            }
            set
            {
                CanWriteProperty("OrderNo", true);
                if (value == null) value = string.Empty;
                if (!_orderNo.Equals(value))
                {
                    _orderNo = value;
                    PropertyHasChanged("OrderNo");
                }
            }
        }

        public string InvoiceNo
        {
            get
            {
                CanReadProperty("InvoiceNo", true);
                return _invoiceNo;
            }
            set
            {
                CanWriteProperty("InvoiceNo", true);
                if (value == null) value = string.Empty;
                if (!_invoiceNo.Equals(value))
                {
                    _invoiceNo = value;
                    PropertyHasChanged("InvoiceNo");
                }
            }
        }

        public string CategoryCode
        {
            get
            {
                CanReadProperty("CategoryCode", true);
                return _categoryCode;
            }
            set
            {
                CanWriteProperty("CategoryCode", true);
                if (value == null) value = string.Empty;
                if (!_categoryCode.Equals(value))
                {
                    _categoryCode = value;
                    PropertyHasChanged("CategoryCode");
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
            // OrderNo
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OrderNo", 50));
            //
            // InvoiceNo
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("InvoiceNo", 50));
            //
            // CategoryCode
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CategoryCode", 10));
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
            //TODO: Define authorization rules in Setting
            //AuthorizationRules.AllowRead("Id", "SettingReadGroup");
            //AuthorizationRules.AllowRead("OrderNo", "SettingReadGroup");

            //AuthorizationRules.AllowWrite("OrderNo", "SettingWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Setting
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SettingViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Setting
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SettingAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Setting
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SettingEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Setting
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SettingDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Setting()
        { /* require use of factory method */ }

        private Setting(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static Setting NewSetting()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Setting");
            return DataPortal.Create<Setting>();
        }

        public static Setting GetSetting(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Setting");
            return DataPortal.Fetch<Setting>(new Criteria(id));
        }

        public static Setting GetSetting(SafeDataReader dr)
        {
            return new Setting(dr);
        }

        public static void DeleteSetting(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Setting");
            DataPortal.Delete(new Criteria(id));
        }

        public override Setting Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Setting");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Setting");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a Setting");

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
                cm.CommandText = "GetSetting";

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

            // dr.Read();
            _id = dr.GetGuid("Id");
            _orderNo = dr.GetString("OrderNo");
            _invoiceNo = dr.GetString("InvoiceNo");
            _categoryCode = dr.GetString("CategoryCode");
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
                cm.CommandText = "AddSetting";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                //  _id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {

            cm.Parameters.AddWithValue("@OrderNo", _orderNo);
            cm.Parameters.AddWithValue("@InvoiceNo", _invoiceNo);
            cm.Parameters.AddWithValue("@CategoryCode", _categoryCode);
            cm.Parameters.AddWithValue("@Id", _id);
            //  cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
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
                cm.CommandText = "UpdateSetting";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@OrderNo", _orderNo);
            cm.Parameters.AddWithValue("@InvoiceNo", _invoiceNo);
            cm.Parameters.AddWithValue("@CategoryCode", _categoryCode);
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
                cm.CommandText = "DeleteSetting";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
