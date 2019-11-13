using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class GalaOrder : Csla.BusinessBase<GalaOrder>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private int _tableCount = 0;
        private int _seatCount = 0;
        private string _shipping = string.Empty;
        private string _payCompany = string.Empty;
        private string _payAddress1 = string.Empty;
        private string _payAddress2 = string.Empty;
        private string _payCity = string.Empty;
        private string _payPostal = string.Empty;
        private string _payCountry = string.Empty;
        private string _payFirstname = string.Empty;
        private string _payLastname = string.Empty;
        private string _payContact = string.Empty;
        private string _payEmail = string.Empty;
        private string _paymentMethod = string.Empty;
        private string _status = string.Empty;
        private string _payStatus = string.Empty;
        private int _isReminded = 0;
        private SmartDate _lastSendPaymentReminderEmailDate = new SmartDate(false);
        private decimal _amount = 0;
        private decimal _fee = 0;
        private decimal _feeShipping = 0;
        private decimal _tax = 0;
        private decimal _amountReceived = 0;
        private string _invoice = string.Empty;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private SmartDate _datePaid = new SmartDate(false);
        private string _remarksPayment = string.Empty;

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public int TableCount
        {
            get
            {
                CanReadProperty("TableCount", true);
                return _tableCount;
            }
            set
            {
                CanWriteProperty("TableCount", true);
                if (!_tableCount.Equals(value))
                {
                    _tableCount = value;
                    PropertyHasChanged("TableCount");
                }
            }
        }

        public int SeatCount
        {
            get
            {
                CanReadProperty("SeatCount", true);
                return _seatCount;
            }
            set
            {
                CanWriteProperty("SeatCount", true);
                if (!_seatCount.Equals(value))
                {
                    _seatCount = value;
                    PropertyHasChanged("SeatCount");
                }
            }
        }

        public string Shipping
        {
            get
            {
                CanReadProperty("Shipping", true);
                return _shipping;
            }
            set
            {
                CanWriteProperty("Shipping", true);
                if (value == null) value = string.Empty;
                if (!_shipping.Equals(value))
                {
                    _shipping = value;
                    PropertyHasChanged("Shipping");
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

        public string PayEmail
        {
            get
            {
                CanReadProperty("PayEmail", true);
                return _payEmail;
            }
            set
            {
                CanWriteProperty("PayEmail", true);
                if (value == null) value = string.Empty;
                if (!_payEmail.Equals(value))
                {
                    _payEmail = value;
                    PropertyHasChanged("PayEmail");
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

        public string Status
        {
            get
            {
                CanReadProperty("Status", true);
                return _status;
            }
            set
            {
                CanWriteProperty("Status", true);
                if (value == null) value = string.Empty;
                if (!_status.Equals(value))
                {
                    _status = value;
                    PropertyHasChanged("Status");
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

        public int IsReminded
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

        public decimal FeeShipping
        {
            get
            {
                CanReadProperty("FeeShipping", true);
                return _feeShipping;
            }
            set
            {
                CanWriteProperty("FeeShipping", true);
                if (!_feeShipping.Equals(value))
                {
                    _feeShipping = value;
                    PropertyHasChanged("FeeShipping");
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

        public DateTime DatePaid
        {
            get
            {
                CanReadProperty("DatePaid", true);
                return _datePaid.Date;
            }
        }

        public string DatePaidString
        {
            get
            {
                CanReadProperty("DatePaidString", true);
                return _datePaid.Text;
            }
            set
            {
                CanWriteProperty("DatePaidString", true);
                if (value == null) value = string.Empty;
                if (!_datePaid.Equals(value))
                {
                    _datePaid.Text = value;
                    PropertyHasChanged("DatePaidString");
                }
            }
        }

        public string RemarksPayment
        {
            get
            {
                CanReadProperty("RemarksPayment", true);
                return _remarksPayment;
            }
            set
            {
                CanWriteProperty("RemarksPayment", true);
                if (value == null) value = string.Empty;
                if (!_remarksPayment.Equals(value))
                {
                    _remarksPayment = value;
                    PropertyHasChanged("RemarksPayment");
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
            // Shipping
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Shipping", 50));
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
            // PayEmail
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayEmail", 100));
            //
            // PaymentMethod
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PaymentMethod", 10));
            //
            // Status
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Status", 3));
            //
            // PayStatus
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayStatus", 3));
            //
            // Invoice
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Invoice", 100));


            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RemarksPayment", 1000));
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
            //TODO: Define authorization rules in GalaOrder
            //AuthorizationRules.AllowRead("Id", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("TableCount", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("SeatCount", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("Shipping", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayCompany", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayAddress1", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayAddress2", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayCity", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayPostal", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayCountry", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayFirstname", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayLastname", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayContact", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayEmail", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PaymentMethod", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("Status", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("PayStatus", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("IsReminded", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("LastSendPaymentReminderEmailDate", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("Amount", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("Fee", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("AmountReceived", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("Invoice", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "GalaOrderReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "GalaOrderReadGroup");

            //AuthorizationRules.AllowWrite("TableCount", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("SeatCount", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("Shipping", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayCompany", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayAddress1", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayAddress2", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayCity", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayPostal", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayCountry", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayFirstname", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayLastname", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayContact", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayEmail", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PaymentMethod", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("Status", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("PayStatus", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("IsReminded", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("LastSendPaymentReminderEmailDate", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("Amount", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("Fee", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("AmountReceived", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("Invoice", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "GalaOrderWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "GalaOrderWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in GalaOrder
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("GalaOrderViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in GalaOrder
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("GalaOrderAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in GalaOrder
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("GalaOrderEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in GalaOrder
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("GalaOrderDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private GalaOrder()
        { /* require use of factory method */ }

        private GalaOrder(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }
        public static GalaOrder NewGalaOrder()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a GalaOrder");
            return DataPortal.Create<GalaOrder>();
        }

        public static GalaOrder GetGalaOrder(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a GalaOrder");
            return DataPortal.Fetch<GalaOrder>(new Criteria(id));
        }

        public static GalaOrder GetGalaOrder(SafeDataReader dr)
        {
            return new GalaOrder(dr);
        }

        public static void DeleteGalaOrder(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a GalaOrder");
            DataPortal.Delete(new Criteria(id));
        }

        public override GalaOrder Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a GalaOrder");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a GalaOrder");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a GalaOrder");

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
                cm.CommandText = "GetGalaOrder";

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
            _tableCount = dr.GetInt32("TableCount");
            _seatCount = dr.GetInt32("SeatCount");
            _shipping = dr.GetString("Shipping");
            _payCompany = dr.GetString("PayCompany");
            _payAddress1 = dr.GetString("PayAddress1");
            _payAddress2 = dr.GetString("PayAddress2");
            _payCity = dr.GetString("PayCity");
            _payPostal = dr.GetString("PayPostal");
            _payCountry = dr.GetString("PayCountry");
            _payFirstname = dr.GetString("PayFirstname");
            _payLastname = dr.GetString("PayLastname");
            _payContact = dr.GetString("PayContact");
            _payEmail = dr.GetString("PayEmail");
            _paymentMethod = dr.GetString("PaymentMethod");
            _status = dr.GetString("Status");
            _payStatus = dr.GetString("PayStatus");
            _isReminded = dr.GetInt32("IsReminded");
            _lastSendPaymentReminderEmailDate = dr.GetSmartDate("LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.EmptyIsMin);
            _amount = dr.GetDecimal("Amount");
            _fee = dr.GetDecimal("Fee");
            _feeShipping = dr.GetDecimal("FeeShipping");
            _tax = dr.GetDecimal("Tax");
            _amountReceived = dr.GetDecimal("AmountReceived");
            _invoice = dr.GetString("Invoice");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _datePaid = dr.GetSmartDate("Datepaid", _datePaid.EmptyIsMin);
            _remarksPayment = dr.GetString("RemarksPayment");
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
                cm.CommandText = "AddGalaOrder";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                //_id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@TableCount", _tableCount);
            cm.Parameters.AddWithValue("@SeatCount", _seatCount);
            cm.Parameters.AddWithValue("@Shipping", _shipping);
            cm.Parameters.AddWithValue("@PayCompany", _payCompany);
            cm.Parameters.AddWithValue("@PayAddress1", _payAddress1);
            cm.Parameters.AddWithValue("@PayAddress2", _payAddress2);
            cm.Parameters.AddWithValue("@PayCity", _payCity);
            cm.Parameters.AddWithValue("@PayPostal", _payPostal);
            cm.Parameters.AddWithValue("@PayCountry", _payCountry);
            cm.Parameters.AddWithValue("@PayFirstname", _payFirstname);
            cm.Parameters.AddWithValue("@PayLastname", _payLastname);
            cm.Parameters.AddWithValue("@PayContact", _payContact);
            cm.Parameters.AddWithValue("@PayEmail", _payEmail);
            cm.Parameters.AddWithValue("@PaymentMethod", _paymentMethod);
            cm.Parameters.AddWithValue("@Status", _status);
            cm.Parameters.AddWithValue("@PayStatus", _payStatus);
            cm.Parameters.AddWithValue("@IsReminded", _isReminded);
            cm.Parameters.AddWithValue("@LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@Amount", _amount);
            cm.Parameters.AddWithValue("@Fee", _fee);
            cm.Parameters.AddWithValue("@FeeShipping", _feeShipping);
            cm.Parameters.AddWithValue("@Tax", _tax);
            cm.Parameters.AddWithValue("@AmountReceived", _amountReceived);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@DatePaid", _datePaid.DBValue);
            cm.Parameters.AddWithValue("@RemarksPayment", _remarksPayment);
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
                cm.CommandText = "UpdateGalaOrder";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@TableCount", _tableCount);
            cm.Parameters.AddWithValue("@SeatCount", _seatCount);
            cm.Parameters.AddWithValue("@Shipping", _shipping);
            cm.Parameters.AddWithValue("@PayCompany", _payCompany);
            cm.Parameters.AddWithValue("@PayAddress1", _payAddress1);
            cm.Parameters.AddWithValue("@PayAddress2", _payAddress2);
            cm.Parameters.AddWithValue("@PayCity", _payCity);
            cm.Parameters.AddWithValue("@PayPostal", _payPostal);
            cm.Parameters.AddWithValue("@PayCountry", _payCountry);
            cm.Parameters.AddWithValue("@PayFirstname", _payFirstname);
            cm.Parameters.AddWithValue("@PayLastname", _payLastname);
            cm.Parameters.AddWithValue("@PayContact", _payContact);
            cm.Parameters.AddWithValue("@PayEmail", _payEmail);
            cm.Parameters.AddWithValue("@PaymentMethod", _paymentMethod);
            cm.Parameters.AddWithValue("@Status", _status);
            cm.Parameters.AddWithValue("@PayStatus", _payStatus);
            cm.Parameters.AddWithValue("@IsReminded", _isReminded);
            cm.Parameters.AddWithValue("@LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@Amount", _amount);
            cm.Parameters.AddWithValue("@Fee", _fee);
            cm.Parameters.AddWithValue("@FeeShipping", _feeShipping);
            cm.Parameters.AddWithValue("@Tax", _tax);
            cm.Parameters.AddWithValue("@AmountReceived", _amountReceived);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@DatePaid", _datePaid.DBValue);
            cm.Parameters.AddWithValue("@RemarksPayment", _remarksPayment);
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
                cm.CommandText = "DeleteGalaOrder";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
