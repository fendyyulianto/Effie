using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Linq;
using System.Collections.Generic;

namespace Effie2017.App
{
    [Serializable()]
    public class AdhocInvoice : Csla.BusinessBase<AdhocInvoice>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _registrationId = Guid.Empty;
        private decimal _amount = 0;
        private decimal _fee = 0;
        private decimal _tax = 0;
        private decimal _grandAmount = 0;
        private Guid _payGroupId = Guid.NewGuid();
        private string _paymentMethod = string.Empty;
        private string _payCompany = string.Empty;
        private string _payAddress1 = string.Empty;
        private string _payAddress2 = string.Empty;
        private string _payCity = string.Empty;
        private string _payPostal = string.Empty;
        private string _payCountry = string.Empty;
        private string _payFirstname = string.Empty;
        private string _payLastname = string.Empty;
        private string _payContact = string.Empty;
        private string _payStatus = string.Empty;
        private decimal _amountReceived = 0;
        private bool _isReminded = false;
        private SmartDate _lastSendPaidEmailDate = new SmartDate(false);
        private SmartDate _lastSendPaymentReminderEmailDate = new SmartDate(false);
        private SmartDate _invoiceDate = new SmartDate(false);
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private string _invoice = string.Empty;

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public Guid RegistrationId
        {
            get
            {
                CanReadProperty("RegistrationId", true);
                return _registrationId;
            }
            set
            {
                CanWriteProperty("RegistrationId", true);
                if (value == null) value = Guid.Empty;
                if (!_registrationId.Equals(value))
                {
                    _registrationId = value;
                    PropertyHasChanged("RegistrationId");
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
                if (value == null) value = Guid.Empty;
                if (!_payGroupId.Equals(value))
                {
                    _payGroupId = value;
                    PropertyHasChanged("PayGroupId");
                }
            }
        }

        public string PaymentMethod
        {
            get
            {
                CanReadProperty("PaymentMethod", true);
                return _paymentMethod;
            }
            set
            {
                CanWriteProperty("PaymentMethod", true);
                if (value == null) value = string.Empty;
                if (!_paymentMethod.Equals(value))
                {
                    _paymentMethod = value;
                    PropertyHasChanged("PaymentMethod");
                }
            }
        }

        public string PayCompany
        {
            get
            {
                CanReadProperty("PayCompany", true);
                return _payCompany;
            }
            set
            {
                CanWriteProperty("PayCompany", true);
                if (value == null) value = string.Empty;
                if (!_payCompany.Equals(value))
                {
                    _payCompany = value;
                    PropertyHasChanged("PayCompany");
                }
            }
        }

        public string PayAddress1
        {
            get
            {
                CanReadProperty("PayAddress1", true);
                return _payAddress1;
            }
            set
            {
                CanWriteProperty("PayAddress1", true);
                if (value == null) value = string.Empty;
                if (!_payAddress1.Equals(value))
                {
                    _payAddress1 = value;
                    PropertyHasChanged("PayAddress1");
                }
            }
        }

        public string PayAddress2
        {
            get
            {
                CanReadProperty("PayAddress2", true);
                return _payAddress2;
            }
            set
            {
                CanWriteProperty("PayAddress2", true);
                if (value == null) value = string.Empty;
                if (!_payAddress2.Equals(value))
                {
                    _payAddress2 = value;
                    PropertyHasChanged("PayAddress2");
                }
            }
        }

        public string PayCity
        {
            get
            {
                CanReadProperty("PayCity", true);
                return _payCity;
            }
            set
            {
                CanWriteProperty("PayCity", true);
                if (value == null) value = string.Empty;
                if (!_payCity.Equals(value))
                {
                    _payCity = value;
                    PropertyHasChanged("PayCity");
                }
            }
        }

        public string PayPostal
        {
            get
            {
                CanReadProperty("PayPostal", true);
                return _payPostal;
            }
            set
            {
                CanWriteProperty("PayPostal", true);
                if (value == null) value = string.Empty;
                if (!_payPostal.Equals(value))
                {
                    _payPostal = value;
                    PropertyHasChanged("PayPostal");
                }
            }
        }

        public string PayCountry
        {
            get
            {
                CanReadProperty("PayCountry", true);
                return _payCountry;
            }
            set
            {
                CanWriteProperty("PayCountry", true);
                if (value == null) value = string.Empty;
                if (!_payCountry.Equals(value))
                {
                    _payCountry = value;
                    PropertyHasChanged("PayCountry");
                }
            }
        }

        public string PayFirstname
        {
            get
            {
                CanReadProperty("PayFirstname", true);
                return _payFirstname;
            }
            set
            {
                CanWriteProperty("PayFirstname", true);
                if (value == null) value = string.Empty;
                if (!_payFirstname.Equals(value))
                {
                    _payFirstname = value;
                    PropertyHasChanged("PayFirstname");
                }
            }
        }

        public string PayLastname
        {
            get
            {
                CanReadProperty("PayLastname", true);
                return _payLastname;
            }
            set
            {
                CanWriteProperty("PayLastname", true);
                if (value == null) value = string.Empty;
                if (!_payLastname.Equals(value))
                {
                    _payLastname = value;
                    PropertyHasChanged("PayLastname");
                }
            }
        }

        public string PayContact
        {
            get
            {
                CanReadProperty("PayContact", true);
                return _payContact;
            }
            set
            {
                CanWriteProperty("PayContact", true);
                if (value == null) value = string.Empty;
                if (!_payContact.Equals(value))
                {
                    _payContact = value;
                    PropertyHasChanged("PayContact");
                }
            }
        }

        public string PayStatus
        {
            get
            {
                CanReadProperty("PayStatus", true);
                return _payStatus;
            }
            set
            {
                CanWriteProperty("PayStatus", true);
                if (value == null) value = string.Empty;
                if (!_payStatus.Equals(value))
                {
                    _payStatus = value;
                    PropertyHasChanged("PayStatus");
                }
            }
        }

        public decimal AmountReceived
        {
            get
            {
                CanReadProperty("AmountReceived", true);
                return _amountReceived;
            }
            set
            {
                CanWriteProperty("AmountReceived", true);
                if (!_amountReceived.Equals(value))
                {
                    _amountReceived = value;
                    PropertyHasChanged("AmountReceived");
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

        public DateTime InvoiceDate
        {
            get
            {
                CanReadProperty("InvoiceDate", true);
                return _invoiceDate.Date;
            }
        }

        public string InvoiceDateString
        {
            get
            {
                CanReadProperty("InvoiceDate", true);
                return _invoiceDate.Text;
            }
            set
            {
                CanWriteProperty("InvoiceDate", true);
                if (value == null) value = string.Empty;
                if (!_invoiceDate.Equals(value))
                {
                    _invoiceDate.Text = value;
                    PropertyHasChanged("InvoiceDate");
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


        public string Deadline
        {
            get
            {
                try
                {
                    AdhocInvoiceItem adhocInvoiceItem = AdhocInvoiceItemList.GetAdhocInvoiceItemList(_payGroupId, _id).FirstOrDefault();
                    Entry entry = Entry.GetEntry(adhocInvoiceItem.EntryId);
                    return GeneralFunctionEffie2017App.GetDateDepentent(entry.PayGroupId, entry.Id, entry.DateSubmitted, "D_String");
                }
                catch { return ""; }
            }
        }


        public Guid AdminidAssignedto
        {
            get
            {
                return GeneralFunctionEffie2017App.GetAdminidAssignedto(_payGroupId, _id);
            }

        }

        public DateTime DateReminder(Guid ID, string type)
        {
            return GeneralFunctionEffie2017App.GetDateReminder(ID, type);
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
            // PaymentMethod
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PaymentMethod", 10));
            //
            // PayCompany
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayCompany", 100));
            //
            // PayAddress1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayAddress1", 100));
            //
            // PayAddress2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayAddress2", 100));
            //
            // PayCity
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayCity", 100));
            //
            // PayPostal
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayPostal", 50));
            //
            // PayCountry
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayCountry", 100));
            //
            // PayFirstname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayFirstname", 100));
            //
            // PayLastname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayLastname", 100));
            //
            // PayContact
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayContact", 50));
            //
            // PayStatus
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayStatus", 3));
            //
            // Invoice
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Invoice", 100));
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
            //TODO: Define authorization rules in AdhocInvoice
            //AuthorizationRules.AllowRead("Id", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("RegistrationId", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("Amount", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("Fee", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("Tax", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("GrandAmount", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayGroupId", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PaymentMethod", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayCompany", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayAddress1", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayAddress2", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayCity", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayPostal", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayCountry", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayFirstname", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayLastname", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayContact", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("PayStatus", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("AmountReceived", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("IsReminded", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("LastSendPaidEmailDate", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("LastSendPaymentReminderEmailDate", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "AdhocInvoiceReadGroup");
            //AuthorizationRules.AllowRead("Invoice", "AdhocInvoiceReadGroup");

            //AuthorizationRules.AllowWrite("Amount", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("Fee", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("Tax", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("GrandAmount", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PaymentMethod", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayCompany", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayAddress1", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayAddress2", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayCity", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayPostal", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayCountry", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayFirstname", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayLastname", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayContact", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("PayStatus", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("AmountReceived", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("IsReminded", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("LastSendPaidEmailDate", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("LastSendPaymentReminderEmailDate", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "AdhocInvoiceWriteGroup");
            //AuthorizationRules.AllowWrite("Invoice", "AdhocInvoiceWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in AdhocInvoice
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in AdhocInvoice
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in AdhocInvoice
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in AdhocInvoice
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private AdhocInvoice()
        { /* require use of factory method */ }

        private AdhocInvoice(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static AdhocInvoice NewAdhocInvoice()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a AdhocInvoice");
            return DataPortal.Create<AdhocInvoice>();
        }

        public static AdhocInvoice GetAdhocInvoice(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a AdhocInvoice");
            return DataPortal.Fetch<AdhocInvoice>(new Criteria(id));
        }

        public static AdhocInvoice GetAdhocInvoice(SafeDataReader dr)
        {
            return new AdhocInvoice(dr);
        }

        public static void DeleteAdhocInvoice(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a AdhocInvoice");
            DataPortal.Delete(new Criteria(id));
        }

        public override AdhocInvoice Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a AdhocInvoice");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a AdhocInvoice");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a AdhocInvoice");

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
                cm.CommandText = "GetAdhocInvoice";

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
            _registrationId = dr.GetGuid("RegistrationId");
            _amount = dr.GetDecimal("Amount");
            _fee = dr.GetDecimal("Fee");
            _tax = dr.GetDecimal("Tax");
            _grandAmount = dr.GetDecimal("GrandAmount");
            _payGroupId = dr.GetGuid("PayGroupId");
            _paymentMethod = dr.GetString("PaymentMethod");
            _payCompany = dr.GetString("PayCompany");
            _payAddress1 = dr.GetString("PayAddress1");
            _payAddress2 = dr.GetString("PayAddress2");
            _payCity = dr.GetString("PayCity");
            _payPostal = dr.GetString("PayPostal");
            _payCountry = dr.GetString("PayCountry");
            _payFirstname = dr.GetString("PayFirstname");
            _payLastname = dr.GetString("PayLastname");
            _payContact = dr.GetString("PayContact");
            _payStatus = dr.GetString("PayStatus");
            _amountReceived = dr.GetDecimal("AmountReceived");
            _isReminded = dr.GetBoolean("IsReminded");
            _lastSendPaidEmailDate = dr.GetSmartDate("LastSendPaidEmailDate", _lastSendPaidEmailDate.EmptyIsMin);
            _lastSendPaymentReminderEmailDate = dr.GetSmartDate("LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.EmptyIsMin);
            _invoiceDate = dr.GetSmartDate("InvoiceDate", _invoiceDate.EmptyIsMin);
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _invoice = dr.GetString("Invoice");
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
                cm.CommandText = "AddAdhocInvoice";

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
            cm.Parameters.AddWithValue("@PaymentMethod", _paymentMethod);
            cm.Parameters.AddWithValue("@PayCompany", _payCompany);
            cm.Parameters.AddWithValue("@PayAddress1", _payAddress1);
            cm.Parameters.AddWithValue("@PayAddress2", _payAddress2);
            cm.Parameters.AddWithValue("@PayCity", _payCity);
            cm.Parameters.AddWithValue("@PayPostal", _payPostal);
            cm.Parameters.AddWithValue("@PayCountry", _payCountry);
            cm.Parameters.AddWithValue("@PayFirstname", _payFirstname);
            cm.Parameters.AddWithValue("@PayLastname", _payLastname);
            cm.Parameters.AddWithValue("@PayContact", _payContact);
            cm.Parameters.AddWithValue("@PayStatus", _payStatus);
            cm.Parameters.AddWithValue("@AmountReceived", _amountReceived);
            cm.Parameters.AddWithValue("@IsReminded", _isReminded);
            cm.Parameters.AddWithValue("@LastSendPaidEmailDate", _lastSendPaidEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@InvoiceDate", _invoiceDate.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@RegistrationId", _registrationId);
            cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
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
                cm.CommandText = "UpdateAdhocInvoice";

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
            cm.Parameters.AddWithValue("@PaymentMethod", _paymentMethod);
            cm.Parameters.AddWithValue("@PayCompany", _payCompany);
            cm.Parameters.AddWithValue("@PayAddress1", _payAddress1);
            cm.Parameters.AddWithValue("@PayAddress2", _payAddress2);
            cm.Parameters.AddWithValue("@PayCity", _payCity);
            cm.Parameters.AddWithValue("@PayPostal", _payPostal);
            cm.Parameters.AddWithValue("@PayCountry", _payCountry);
            cm.Parameters.AddWithValue("@PayFirstname", _payFirstname);
            cm.Parameters.AddWithValue("@PayLastname", _payLastname);
            cm.Parameters.AddWithValue("@PayContact", _payContact);
            cm.Parameters.AddWithValue("@PayStatus", _payStatus);
            cm.Parameters.AddWithValue("@AmountReceived", _amountReceived);
            cm.Parameters.AddWithValue("@IsReminded", _isReminded);
            cm.Parameters.AddWithValue("@LastSendPaidEmailDate", _lastSendPaidEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@InvoiceDate", _invoiceDate.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@RegistrationId", _registrationId);
            cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
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
                cm.CommandText = "DeleteAdhocInvoice";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
    
    public class AdhocInvoiceModel
    {
        public AdhocInvoice adhocInvoices;
        public List<AdhocInvoiceItem> adhocInvoiceItemList;
        public AdhocInvoiceModel(AdhocInvoice _adhocInvoices, List<AdhocInvoiceItem> _adhocInvoiceItemList)
        {
            this.adhocInvoices = _adhocInvoices;
            this.adhocInvoiceItemList = _adhocInvoiceItemList;
        }
    }
}
