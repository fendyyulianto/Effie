using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class JuryPanelCategory : Csla.BusinessBase<JuryPanelCategory>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _panelId = string.Empty;
        private string _categoryPSDetail = string.Empty;
        private int _orderNo = 0;
        private bool _isActive = false;
        private string _round = "";

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public string PanelId
        {
            get
            {
                CanReadProperty("PanelId", true);
                return _panelId;
            }
            set
            {
                CanWriteProperty("PanelId", true);
                if (value == null) value = string.Empty;
                if (!_panelId.Equals(value))
                {
                    _panelId = value;
                    PropertyHasChanged("PanelId");
                }
            }
        }

        public string CategoryPSDetail
        {
            get
            {
                CanReadProperty("CategoryPSDetail", true);
                return _categoryPSDetail;
            }
            set
            {
                CanWriteProperty("CategoryPSDetail", true);
                if (value == null) value = string.Empty;
                if (!_categoryPSDetail.Equals(value))
                {
                    _categoryPSDetail = value;
                    PropertyHasChanged("CategoryPSDetail");
                }
            }
        }

        public int OrderNo
        {
            get
            {
                CanReadProperty("OrderNo", true);
                return _orderNo;
            }
            set
            {
                CanWriteProperty("OrderNo", true);
                if (!_orderNo.Equals(value))
                {
                    _orderNo = value;
                    PropertyHasChanged("OrderNo");
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
                if (!_round.Equals(value))
                {
                    _round = value;
                    PropertyHasChanged("Round");
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
            // PanelId
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PanelId", 50));
            //
            // CategoryPSDetail
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CategoryPSDetail", 200));
            // Round
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
            //TODO: Define authorization rules in JuryPanelCategory
            //AuthorizationRules.AllowRead("Id", "JuryPanelCategoryReadGroup");
            //AuthorizationRules.AllowRead("PanelId", "JuryPanelCategoryReadGroup");
            //AuthorizationRules.AllowRead("CategoryPSDetail", "JuryPanelCategoryReadGroup");
            //AuthorizationRules.AllowRead("OrderNo", "JuryPanelCategoryReadGroup");
            //AuthorizationRules.AllowRead("IsActive", "JuryPanelCategoryReadGroup");

            //AuthorizationRules.AllowWrite("PanelId", "JuryPanelCategoryWriteGroup");
            //AuthorizationRules.AllowWrite("CategoryPSDetail", "JuryPanelCategoryWriteGroup");
            //AuthorizationRules.AllowWrite("OrderNo", "JuryPanelCategoryWriteGroup");
            //AuthorizationRules.AllowWrite("IsActive", "JuryPanelCategoryWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in JuryPanelCategory
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryPanelCategoryViewGroup"))
            //    return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in JuryPanelCategory
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryPanelCategoryAddGroup"))
            //    return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in JuryPanelCategory
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryPanelCategoryEditGroup"))
            //    return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in JuryPanelCategory
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryPanelCategoryDeleteGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private JuryPanelCategory()
        { /* require use of factory method */ }

        private JuryPanelCategory(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static JuryPanelCategory NewJuryPanelCategory()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a JuryPanelCategory");
            return DataPortal.Create<JuryPanelCategory>();
        }

        public static JuryPanelCategory GetJuryPanelCategory(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JuryPanelCategory");
            return DataPortal.Fetch<JuryPanelCategory>(new Criteria(id));
        }
        public static JuryPanelCategory GetJuryPanelCategory(SafeDataReader dr)
        {
            return new JuryPanelCategory(dr);
        }
        public static void DeleteJuryPanelCategory(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a JuryPanelCategory");
            DataPortal.Delete(new Criteria(id));
        }

        public override JuryPanelCategory Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a JuryPanelCategory");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a JuryPanelCategory");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a JuryPanelCategory");

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
                cm.CommandText = "GetJuryPanelCategory";

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
            _panelId = dr.GetString("PanelId");
            _categoryPSDetail = dr.GetString("CategoryPSDetail");
            _orderNo = dr.GetInt32("OrderNo");
            _isActive = dr.GetBoolean("IsActive");
            _round = dr.GetString("Round");
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
                cm.CommandText = "AddJuryPanelCategory";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

              //  _id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            
            cm.Parameters.AddWithValue("@PanelId", _panelId);            
            cm.Parameters.AddWithValue("@CategoryPSDetail", _categoryPSDetail);            
            cm.Parameters.AddWithValue("@OrderNo", _orderNo);            
            cm.Parameters.AddWithValue("@IsActive", _isActive);           
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@Round", _round);
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
                cm.CommandText = "UpdateJuryPanelCategory";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
                cm.Parameters.AddWithValue("@Id", _id);            
                cm.Parameters.AddWithValue("@PanelId", _panelId);           
                cm.Parameters.AddWithValue("@CategoryPSDetail", _categoryPSDetail);           
                cm.Parameters.AddWithValue("@OrderNo", _orderNo);           
                cm.Parameters.AddWithValue("@IsActive", _isActive);
                cm.Parameters.AddWithValue("@Round", _round);
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
                cm.CommandText = "DeleteJuryPanelCategory";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}