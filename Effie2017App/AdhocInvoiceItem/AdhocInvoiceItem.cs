using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class AdhocInvoiceItem : Csla.BusinessBase<AdhocInvoiceItem>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _payGroupId = Guid.Empty;
        private Guid _entryId = Guid.Empty;
        private Guid _adhocInvoiceId = Guid.Empty;
        private decimal _amount = 0;
        private decimal _fee = 0;
        private decimal _tax = 0;
        private decimal _grandAmount = 0;
        private string _invoice = string.Empty;
        private string _invoiceType = string.Empty;
        private string _invoiceTypeOthers = string.Empty;
        private bool _isReminded = false;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private SmartDate _lastSendPaidEmailDate = new SmartDate(false);
        private SmartDate _lastSendPaymentReminderEmailDate = new SmartDate(false);

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public Guid PayGroupId
        {
            get
            {
                CanReadProperty("PayGroupId", true);
                return _payGroupId;
            }
            set
            {
                CanWriteProperty("PayGroupId", true);
                if (!_payGroupId.Equals(value))
                {
                    _payGroupId = value;
                    PropertyHasChanged("PayGroupId");
                }
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
                if (!_entryId.Equals(value))
                {
                    _entryId = value;
                    PropertyHasChanged("EntryId");
                }
            }
        }

        public Guid AdhocInvoiceId
        {
            get
            {
                CanReadProperty("AdhocInvoiceId", true);
                return _adhocInvoiceId;
            }
            set
            {
                CanWriteProperty("AdhocInvoiceId", true);
                if (!_adhocInvoiceId.Equals(value))
                {
                    _adhocInvoiceId = value;
                    PropertyHasChanged("AdhocInvoiceId");
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

        public decimal Fee
        {
            get
            {
                CanReadProperty("Fee", true);
                return _fee;
            }
            set
            {
                CanWriteProperty("Fee", true);
                if (!_fee.Equals(value))
                {
                    _fee = value;
                    PropertyHasChanged("Fee");
                }
            }
        }

        public decimal Tax
        {
            get
            {
                CanReadProperty("Tax", true);
                return _tax;
            }
            set
            {
                CanWriteProperty("Tax", true);
                if (!_tax.Equals(value))
                {
                    _tax = value;
                    PropertyHasChanged("Tax");
                }
            }
        }

        public decimal GrandAmount
        {
            get
            {
                CanReadProperty("GrandAmount", true);
                return _grandAmount;
            }
            set
            {
                CanWriteProperty("GrandAmount", true);
                if (!_grandAmount.Equals(value))
                {
                    _grandAmount = value;
                    PropertyHasChanged("GrandAmount");
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

        public string InvoiceType
        {
            get
            {
                CanReadProperty("InvoiceType", true);
                return _invoiceType;
            }
            set
            {
                CanWriteProperty("InvoiceType", true);
                if (value == null) value = string.Empty;
                if (!_invoiceType.Equals(value))
                {
                    _invoiceType = value;
                    PropertyHasChanged("InvoiceType");
                }
            }
        }

        public string InvoiceTypeOthers
        {
            get
            {
                CanReadProperty("InvoiceTypeOthers", true);
                return _invoiceTypeOthers;
            }
            set
            {
                CanWriteProperty("InvoiceTypeOthers", true);
                if (value == null) value = string.Empty;
                if (!_invoiceTypeOthers.Equals(value))
                {
                    _invoiceTypeOthers = value;
                    PropertyHasChanged("InvoiceTypeOthers");
                }
            }
        }

        public bool IsReminded
        {
            get
            {
                CanReadProperty("IsReminded", true);
                return _isReminded;
            }
            set
            {
                CanWriteProperty("IsReminded", true);
                if (!_isReminded.Equals(value))
                {
                    _isReminded = value;
                    PropertyHasChanged("IsReminded");
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

        public DateTime LastSendPaidEmailDate
        {
            get
            {
                CanReadProperty("LastSendPaidEmailDate", true);
                return _lastSendPaidEmailDate.Date;
            }
        }

        public string LastSendPaidEmailDateString
        {
            get
            {
                CanReadProperty("LastSendPaidEmailDate", true);
                return _lastSendPaidEmailDate.Text;
            }
            set
            {
                CanWriteProperty("LastSendPaidEmailDate", true);
                if (value == null) value = string.Empty;
                if (!_lastSendPaidEmailDate.Equals(value))
                {
                    _lastSendPaidEmailDate.Text = value;
                    PropertyHasChanged("LastSendPaidEmailDate");
                }
            }
        }

        public DateTime LastSendPaymentReminderEmailDate
        {
            get
            {
                CanReadProperty("LastSendPaymentReminderEmailDate", true);
                return _lastSendPaymentReminderEmailDate.Date;
            }
        }

        public string LastSendPaymentReminderEmailDateString
        {
            get
            {
                CanReadProperty("LastSendPaymentReminderEmailDate", true);
                return _lastSendPaymentReminderEmailDate.Text;
            }
            set
            {
                CanWriteProperty("LastSendPaymentReminderEmailDate", true);
                if (value == null) value = string.Empty;
                if (!_lastSendPaymentReminderEmailDate.Equals(value))
                {
                    _lastSendPaymentReminderEmailDate.Text = value;
                    PropertyHasChanged("LastSendPaymentReminderEmailDate");
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
            // InvoiceType
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("InvoiceType", 100));
            //
            // InvoiceTypeOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("InvoiceTypeOthers", 500));
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
            //TODO: Define authorization rules in AdhocInvoiceItem
            //AuthorizationRules.AllowRead("Id", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("PayGroupId", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("EntryId", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("Amount", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("Fee", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("Tax", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("GrandAmount", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("Invoice", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("IsReminded", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("LastSendPaidEmailDate", "AdhocInvoiceItemReadGroup");
            //AuthorizationRules.AllowRead("LastSendPaymentReminderEmailDate", "AdhocInvoiceItemReadGroup");

            //AuthorizationRules.AllowWrite("Amount", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("Fee", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("Tax", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("GrandAmount", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("Invoice", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("IsReminded", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("LastSendPaidEmailDate", "AdhocInvoiceItemWriteGroup");
            //AuthorizationRules.AllowWrite("LastSendPaymentReminderEmailDate", "AdhocInvoiceItemWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in AdhocInvoiceItem
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceItemViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in AdhocInvoiceItem
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceItemAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in AdhocInvoiceItem
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceItemEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in AdhocInvoiceItem
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceItemDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private AdhocInvoiceItem()
        { /* require use of factory method */ }

        private AdhocInvoiceItem(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static AdhocInvoiceItem NewAdhocInvoiceItem()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a AdhocInvoiceItem");
            return DataPortal.Create<AdhocInvoiceItem>();
        }

        public static AdhocInvoiceItem GetAdhocInvoiceItem(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a AdhocInvoiceItem");
            return DataPortal.Fetch<AdhocInvoiceItem>(new Criteria(id));
        }

        public static AdhocInvoiceItem GetAdhocInvoiceItem(SafeDataReader dr)
        {
            return new AdhocInvoiceItem(dr);
        }

        public static void DeleteAdhocInvoiceItem(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a AdhocInvoiceItem");
            DataPortal.Delete(new Criteria(id));
        }

        public override AdhocInvoiceItem Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a AdhocInvoiceItem");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a AdhocInvoiceItem");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a AdhocInvoiceItem");

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
                cm.CommandText = "GetAdhocInvoiceItem";

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
            _payGroupId = dr.GetGuid("PayGroupId");
            _entryId = dr.GetGuid("EntryId");
            _adhocInvoiceId = dr.GetGuid("AdhocInvoiceId");
            _amount = dr.GetDecimal("Amount");
            _fee = dr.GetDecimal("Fee");
            _tax = dr.GetDecimal("Tax");
            _grandAmount = dr.GetDecimal("GrandAmount");
            _invoice = dr.GetString("Invoice");
            _invoiceType = dr.GetString("InvoiceType");
            _invoiceTypeOthers = dr.GetString("InvoiceTypeOthers");
            _isReminded = dr.GetBoolean("IsReminded");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _lastSendPaidEmailDate = dr.GetSmartDate("LastSendPaidEmailDate", _lastSendPaidEmailDate.EmptyIsMin);
            _lastSendPaymentReminderEmailDate = dr.GetSmartDate("LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.EmptyIsMin);
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
                cm.CommandText = "AddAdhocInvoiceItem";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                _id = (Guid)cm.Parameters["@Id"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Amount", _amount);
            cm.Parameters.AddWithValue("@Fee", _fee);
            cm.Parameters.AddWithValue("@Tax", _tax);
            cm.Parameters.AddWithValue("@GrandAmount", _grandAmount);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@InvoiceType", _invoiceType);
            cm.Parameters.AddWithValue("@InvoiceTypeOthers", _invoiceTypeOthers);
            cm.Parameters.AddWithValue("@IsReminded", _isReminded);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@LastSendPaidEmailDate", _lastSendPaidEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
            cm.Parameters.AddWithValue("@AdhocInvoiceId", _adhocInvoiceId);
            cm.Parameters.AddWithValue("@EntryId", _entryId);
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
                cm.CommandText = "UpdateAdhocInvoiceItem";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Amount", _amount);
            cm.Parameters.AddWithValue("@Fee", _fee);
            cm.Parameters.AddWithValue("@Tax", _tax);
            cm.Parameters.AddWithValue("@GrandAmount", _grandAmount);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@InvoiceType", _invoiceType);
            cm.Parameters.AddWithValue("@InvoiceTypeOthers", _invoiceTypeOthers);
            cm.Parameters.AddWithValue("@IsReminded", _isReminded);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@LastSendPaidEmailDate", _lastSendPaidEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
            cm.Parameters.AddWithValue("@AdhocInvoiceId", _adhocInvoiceId);
            cm.Parameters.AddWithValue("@EntryId", _entryId);
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
                cm.CommandText = "DeleteAdhocInvoiceItem";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
