using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class AdhocInvoiceAmountReceived : Csla.BusinessBase<AdhocInvoiceAmountReceived>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private SmartDate _dateReceived = new SmartDate(false);
        private Guid _paygroupId = Guid.Empty;
        private string _invoice = string.Empty;
        private decimal _amount = 0;
        private string _remarks = string.Empty;
        private bool _isSetPaid = false;
        private SmartDate _dateCreated = new SmartDate(false);
        private bool _isAdmin = false;
        private Guid _commentatorID = Guid.NewGuid();

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public DateTime DateReceived
        {
            get
            {
                CanReadProperty("DateReceived", true);
                return _dateReceived.Date;
            }
        }

        public string DateReceivedString
        {
            get
            {
                CanReadProperty("DateReceived", true);
                return _dateReceived.Text;
            }
            set
            {
                CanWriteProperty("DateReceived", true);
                if (value == null) value = string.Empty;
                if (!_dateReceived.Equals(value))
                {
                    _dateReceived.Text = value;
                    PropertyHasChanged("DateReceived");
                }
            }
        }

        public Guid PaygroupId
        {
            get
            {
                CanReadProperty("PaygroupId", true);
                return _paygroupId;
            }
            set
            {
                CanWriteProperty("PaygroupId", true);
                if (value == null) value = Guid.Empty;
                if (!_paygroupId.Equals(value))
                {
                    _paygroupId = value;
                    PropertyHasChanged("PaygroupId");
                }
            }
        }

        public string Invoice
        {
            get
            {
                CanReadProperty("Invoice", true);
                return _invoice;
            }
            set
            {
                CanWriteProperty("Invoice", true);
                if (value == null) value = string.Empty;
                if (!_invoice.Equals(value))
                {
                    _invoice = value;
                    PropertyHasChanged("Invoice");
                }
            }
        }

        public decimal Amount
        {
            get
            {
                CanReadProperty("Amount", true);
                return _amount;
            }
            set
            {
                CanWriteProperty("Amount", true);
                if (!_amount.Equals(value))
                {
                    _amount = value;
                    PropertyHasChanged("Amount");
                }
            }
        }

        public string Remarks
        {
            get
            {
                CanReadProperty("Remarks", true);
                return _remarks;
            }
            set
            {
                CanWriteProperty("Remarks", true);
                if (value == null) value = string.Empty;
                if (!_remarks.Equals(value))
                {
                    _remarks = value;
                    PropertyHasChanged("Remarks");
                }
            }
        }

        public bool IsSetPaid
        {
            get
            {
                CanReadProperty("IsSetPaid", true);
                return _isSetPaid;
            }
            set
            {
                CanWriteProperty("IsSetPaid", true);
                if (!_isSetPaid.Equals(value))
                {
                    _isSetPaid = value;
                    PropertyHasChanged("IsSetPaid");
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

		public bool isAdmin
		{
			get
			{
				CanReadProperty("isAdmin", true);
				return _isAdmin;
			}
			set
			{
				CanWriteProperty("isAdmin", true);
				if (!_isAdmin.Equals(value))
				{
					_isAdmin = value;
					PropertyHasChanged("isAdmin");
				}
			}
		}

		public Guid CommentatorID
		{
			get
			{
				CanReadProperty("CommentatorID", true);
				return _commentatorID;
            }
            set
            {
                CanWriteProperty("CommentatorID", true);
                if (value == null) value = Guid.Empty;
                if (!_commentatorID.Equals(value))
                {
                    _commentatorID = value;
                    PropertyHasChanged("CommentatorID");
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
            // Invoice
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Invoice", 100));
            //
            // Remarks
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Remarks", 1000));
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
            //TODO: Define authorization rules in AdhocInvoiceAmountReceived
            //AuthorizationRules.AllowRead("Id", "AdhocInvoiceAmountReceivedReadGroup");
            //AuthorizationRules.AllowRead("DateReceived", "AdhocInvoiceAmountReceivedReadGroup");
            //AuthorizationRules.AllowRead("PaygroupId", "AdhocInvoiceAmountReceivedReadGroup");
            //AuthorizationRules.AllowRead("Invoice", "AdhocInvoiceAmountReceivedReadGroup");
            //AuthorizationRules.AllowRead("Amount", "AdhocInvoiceAmountReceivedReadGroup");
            //AuthorizationRules.AllowRead("Remarks", "AdhocInvoiceAmountReceivedReadGroup");
            //AuthorizationRules.AllowRead("IsSetPaid", "AdhocInvoiceAmountReceivedReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "AdhocInvoiceAmountReceivedReadGroup");

            //AuthorizationRules.AllowWrite("DateReceived", "AdhocInvoiceAmountReceivedWriteGroup");
            //AuthorizationRules.AllowWrite("Invoice", "AdhocInvoiceAmountReceivedWriteGroup");
            //AuthorizationRules.AllowWrite("Amount", "AdhocInvoiceAmountReceivedWriteGroup");
            //AuthorizationRules.AllowWrite("Remarks", "AdhocInvoiceAmountReceivedWriteGroup");
            //AuthorizationRules.AllowWrite("IsSetPaid", "AdhocInvoiceAmountReceivedWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "AdhocInvoiceAmountReceivedWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in AdhocInvoiceAmountReceived
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceAmountReceivedViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in AdhocInvoiceAmountReceived
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceAmountReceivedAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in AdhocInvoiceAmountReceived
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceAmountReceivedEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in AdhocInvoiceAmountReceived
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceAmountReceivedDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private AdhocInvoiceAmountReceived()
        { /* require use of factory method */ }

        private AdhocInvoiceAmountReceived(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static AdhocInvoiceAmountReceived NewAdhocInvoiceAmountReceived()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a AdhocInvoiceAmountReceived");
            return DataPortal.Create<AdhocInvoiceAmountReceived>();
        }

        public static AdhocInvoiceAmountReceived GetAdhocInvoiceAmountReceived(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a AdhocInvoiceAmountReceived");
            return DataPortal.Fetch<AdhocInvoiceAmountReceived>(new Criteria(id));
        }

        public static AdhocInvoiceAmountReceived GetAdhocInvoiceAmountReceived(SafeDataReader dr)
        {
            return new AdhocInvoiceAmountReceived(dr);
        }

        public static void DeleteAdhocInvoiceAmountReceived(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a AdhocInvoiceAmountReceived");
            DataPortal.Delete(new Criteria(id));
        }

        public override AdhocInvoiceAmountReceived Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a AdhocInvoiceAmountReceived");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a AdhocInvoiceAmountReceived");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a AdhocInvoiceAmountReceived");

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
                cm.CommandText = "GetAdhocInvoiceAmountReceived";

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
            _dateReceived = dr.GetSmartDate("DateReceived", _dateReceived.EmptyIsMin);
            _paygroupId = dr.GetGuid("PaygroupId");
            _invoice = dr.GetString("Invoice");
            _amount = dr.GetDecimal("Amount");
            _remarks = dr.GetString("Remarks");
            _isSetPaid = dr.GetBoolean("IsSetPaid");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
			_isAdmin = dr.GetBoolean("isAdmin");
			_commentatorID = dr.GetGuid("CommentatorID");
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
                cm.CommandText = "AddAdhocInvoiceAmountReceived";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@DateReceived", _dateReceived.DBValue);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@Amount", _amount);
            cm.Parameters.AddWithValue("@Remarks", _remarks);
            cm.Parameters.AddWithValue("@IsSetPaid", _isSetPaid);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
			if (_isAdmin != false)
				cm.Parameters.AddWithValue("@isAdmin", _isAdmin);
			else
				cm.Parameters.AddWithValue("@isAdmin", DBNull.Value);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@PaygroupId", _paygroupId);
			cm.Parameters.AddWithValue("@CommentatorID", _commentatorID);
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
                cm.CommandText = "UpdateAdhocInvoiceAmountReceived";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@DateReceived", _dateReceived.DBValue);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@Amount", _amount);
            cm.Parameters.AddWithValue("@Remarks", _remarks);
            cm.Parameters.AddWithValue("@IsSetPaid", _isSetPaid);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@PaygroupId", _paygroupId);
			if (_isAdmin != false)
				cm.Parameters.AddWithValue("@isAdmin", _isAdmin);
			else
				cm.Parameters.AddWithValue("@isAdmin", DBNull.Value);
			cm.Parameters.AddWithValue("@CommentatorID", _commentatorID);
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
                cm.CommandText = "DeleteAdhocInvoiceAmountReceived";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
