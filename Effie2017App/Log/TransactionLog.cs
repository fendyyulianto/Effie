using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{ 
	[Serializable()] 
	public class TransactionLog : Csla.BusinessBase<TransactionLog>
	{
		#region Business Properties and Methods

		//declare members
		private Guid _id = Guid.NewGuid();
		private Guid _registrationID = Guid.NewGuid();
		private string _paypalID = string.Empty;
		private string _transactionID = string.Empty;
		private string _payerID = string.Empty;
		private string _receiverID = string.Empty;
		private string _firstName = string.Empty;
		private string _lastName = string.Empty;
		private string _business = string.Empty;
		private string _payerEmail = string.Empty;
		private string _receiverEmail = string.Empty;
		private string _itemName = string.Empty;
		private string _subject = string.Empty;
		private string _currency = string.Empty;
		private double _gross = 0;
		private double _transactionFee = 0;
		private double _tax = 0;
		private double _handling = 0;
		private double _shipping = 0;
		private string _country = string.Empty;
		private string _paymentDate = string.Empty;
		private string _paymentStatus = string.Empty;
		private string _protection = string.Empty;
		private string _verifySign = string.Empty;
		private string _paymentType = string.Empty;

		[System.ComponentModel.DataObjectField(true, true)]
		public Guid ID
		{
			get
			{
				CanReadProperty("ID", true);
				return _id;
			}
		}

		public Guid RegistrationID
		{
			get
			{
				CanReadProperty("RegistrationID", true);
				return _registrationID;
			}
            set
            {
                CanWriteProperty("RegistrationID", true);
                if (value == null) value = Guid.Empty;
                if (!_registrationID.Equals(value))
                {
                    _registrationID = value;
                    PropertyHasChanged("RegistrationID");
                }
            }
		}

		public string PaypalID
		{
			get
			{
				CanReadProperty("PaypalID", true);
				return _paypalID;
			}
			set
			{
				CanWriteProperty("PaypalID", true);
				if (value == null) value = string.Empty;
				if (!_paypalID.Equals(value))
				{
					_paypalID = value;
					PropertyHasChanged("PaypalID");
				}
			}
		}

		public string TransactionID
		{
			get
			{
				CanReadProperty("TransactionID", true);
				return _transactionID;
			}
			set
			{
				CanWriteProperty("TransactionID", true);
				if (value == null) value = string.Empty;
				if (!_transactionID.Equals(value))
				{
					_transactionID = value;
					PropertyHasChanged("TransactionID");
				}
			}
		}

		public string PayerID
		{
			get
			{
				CanReadProperty("PayerID", true);
				return _payerID;
			}
			set
			{
				CanWriteProperty("PayerID", true);
				if (value == null) value = string.Empty;
				if (!_payerID.Equals(value))
				{
					_payerID = value;
					PropertyHasChanged("PayerID");
				}
			}
		}

		public string ReceiverID
		{
			get
			{
				CanReadProperty("ReceiverID", true);
				return _receiverID;
			}
			set
			{
				CanWriteProperty("ReceiverID", true);
				if (value == null) value = string.Empty;
				if (!_receiverID.Equals(value))
				{
					_receiverID = value;
					PropertyHasChanged("ReceiverID");
				}
			}
		}

		public string FirstName
		{
			get
			{
				CanReadProperty("FirstName", true);
				return _firstName;
			}
			set
			{
				CanWriteProperty("FirstName", true);
				if (value == null) value = string.Empty;
				if (!_firstName.Equals(value))
				{
					_firstName = value;
					PropertyHasChanged("FirstName");
				}
			}
		}

		public string LastName
		{
			get
			{
				CanReadProperty("LastName", true);
				return _lastName;
			}
			set
			{
				CanWriteProperty("LastName", true);
				if (value == null) value = string.Empty;
				if (!_lastName.Equals(value))
				{
					_lastName = value;
					PropertyHasChanged("LastName");
				}
			}
		}

		public string Business
		{
			get
			{
				CanReadProperty("Business", true);
				return _business;
			}
			set
			{
				CanWriteProperty("Business", true);
				if (value == null) value = string.Empty;
				if (!_business.Equals(value))
				{
					_business = value;
					PropertyHasChanged("Business");
				}
			}
		}

		public string PayerEmail
		{
			get
			{
				CanReadProperty("PayerEmail", true);
				return _payerEmail;
			}
			set
			{
				CanWriteProperty("PayerEmail", true);
				if (value == null) value = string.Empty;
				if (!_payerEmail.Equals(value))
				{
					_payerEmail = value;
					PropertyHasChanged("PayerEmail");
				}
			}
		}

		public string ReceiverEmail
		{
			get
			{
				CanReadProperty("ReceiverEmail", true);
				return _receiverEmail;
			}
			set
			{
				CanWriteProperty("ReceiverEmail", true);
				if (value == null) value = string.Empty;
				if (!_receiverEmail.Equals(value))
				{
					_receiverEmail = value;
					PropertyHasChanged("ReceiverEmail");
				}
			}
		}

		public string ItemName
		{
			get
			{
				CanReadProperty("ItemName", true);
				return _itemName;
			}
			set
			{
				CanWriteProperty("ItemName", true);
				if (value == null) value = string.Empty;
				if (!_itemName.Equals(value))
				{
					_itemName = value;
					PropertyHasChanged("ItemName");
				}
			}
		}

		public string Subject
		{
			get
			{
				CanReadProperty("Subject", true);
				return _subject;
			}
			set
			{
				CanWriteProperty("Subject", true);
				if (value == null) value = string.Empty;
				if (!_subject.Equals(value))
				{
					_subject = value;
					PropertyHasChanged("Subject");
				}
			}
		}

		public string Currency
		{
			get
			{
				CanReadProperty("Currency", true);
				return _currency;
			}
			set
			{
				CanWriteProperty("Currency", true);
				if (value == null) value = string.Empty;
				if (!_currency.Equals(value))
				{
					_currency = value;
					PropertyHasChanged("Currency");
				}
			}
		}

		public double Gross
		{
			get
			{
				CanReadProperty("Gross", true);
				return _gross;
			}
			set
			{
				CanWriteProperty("Gross", true);
				if (!_gross.Equals(value))
				{
					_gross = value;
					PropertyHasChanged("Gross");
				}
			}
		}

		public double TransactionFee
		{
			get
			{
				CanReadProperty("TransactionFee", true);
				return _transactionFee;
			}
			set
			{
				CanWriteProperty("TransactionFee", true);
				if (!_transactionFee.Equals(value))
				{
					_transactionFee = value;
					PropertyHasChanged("TransactionFee");
				}
			}
		}

		public double Tax
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

		public double Handling
		{
			get
			{
				CanReadProperty("Handling", true);
				return _handling;
			}
			set
			{
				CanWriteProperty("Handling", true);
				if (!_handling.Equals(value))
				{
					_handling = value;
					PropertyHasChanged("Handling");
				}
			}
		}

		public double Shipping
		{
			get
			{
				CanReadProperty("Shipping", true);
				return _shipping;
			}
			set
			{
				CanWriteProperty("Shipping", true);
				if (!_shipping.Equals(value))
				{
					_shipping = value;
					PropertyHasChanged("Shipping");
				}
			}
		}

		public string Country
		{
			get
			{
				CanReadProperty("Country", true);
				return _country;
			}
			set
			{
				CanWriteProperty("Country", true);
				if (value == null) value = string.Empty;
				if (!_country.Equals(value))
				{
					_country = value;
					PropertyHasChanged("Country");
				}
			}
		}

		public string PaymentDate
		{
			get
			{
				CanReadProperty("PaymentDate", true);
				return _paymentDate;
			}
			set
			{
				CanWriteProperty("PaymentDate", true);
				if (value == null) value = string.Empty;
				if (!_paymentDate.Equals(value))
				{
					_paymentDate = value;
					PropertyHasChanged("PaymentDate");
				}
			}
		}

		public string PaymentStatus
		{
			get
			{
				CanReadProperty("PaymentStatus", true);
				return _paymentStatus;
			}
			set
			{
				CanWriteProperty("PaymentStatus", true);
				if (value == null) value = string.Empty;
				if (!_paymentStatus.Equals(value))
				{
					_paymentStatus = value;
					PropertyHasChanged("PaymentStatus");
				}
			}
		}

		public string Protection
		{
			get
			{
				CanReadProperty("Protection", true);
				return _protection;
			}
			set
			{
				CanWriteProperty("Protection", true);
				if (value == null) value = string.Empty;
				if (!_protection.Equals(value))
				{
					_protection = value;
					PropertyHasChanged("Protection");
				}
			}
		}

		public string VerifySign
		{
			get
			{
				CanReadProperty("VerifySign", true);
				return _verifySign;
			}
			set
			{
				CanWriteProperty("VerifySign", true);
				if (value == null) value = string.Empty;
				if (!_verifySign.Equals(value))
				{
					_verifySign = value;
					PropertyHasChanged("VerifySign");
				}
			}
		}

		public string PaymentType
		{
			get
			{
				CanReadProperty("PaymentType", true);
				return _paymentType;
			}
			set
			{
				CanWriteProperty("PaymentType", true);
				if (value == null) value = string.Empty;
				if (!_paymentType.Equals(value))
				{
					_paymentType = value;
					PropertyHasChanged("PaymentType");
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
			// PaypalID
			//
			ValidationRules.AddRule(CommonRules.StringRequired, "PaypalID");
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PaypalID", 500));
			//
			// TransactionID
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TransactionID", 20));
			//
			// PayerID
			//
			ValidationRules.AddRule(CommonRules.StringRequired, "PayerID");
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayerID", 200));
			//
			// ReceiverID
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ReceiverID", 100));
			//
			// FirstName
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("FirstName", 100));
			//
			// LastName
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LastName", 100));
			//
			// Business
			//
			ValidationRules.AddRule(CommonRules.StringRequired, "Business");
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Business", 100));
			//
			// PayerEmail
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayerEmail", 100));
			//
			// ReceiverEmail
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ReceiverEmail", 100));
			//
			// ItemName
			//
			ValidationRules.AddRule(CommonRules.StringRequired, "ItemName");
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ItemName", 500));
			//
			// Subject
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Subject", 500));
			//
			// Currency
			//
			ValidationRules.AddRule(CommonRules.StringRequired, "Currency");
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Currency", 50));
			//
			// Country
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Country", 200));
			//
			// PaymentDate
			//
			ValidationRules.AddRule(CommonRules.StringRequired, "PaymentDate");
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PaymentDate", 200));
			//
			// PaymentStatus
			//
			ValidationRules.AddRule(CommonRules.StringRequired, "PaymentStatus");
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PaymentStatus", 100));
			//
			// Protection
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Protection", 200));
			//
			// VerifySign
			//
			ValidationRules.AddRule(CommonRules.StringRequired, "VerifySign");
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("VerifySign", 200));
			//
			// PaymentType
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PaymentType", 100));
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
			//TODO: Define authorization rules in TransactionLog
			//AuthorizationRules.AllowRead("ID", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("RegistrationID", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("PaypalID", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("TransactionID", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("PayerID", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("ReceiverID", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("FirstName", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("LastName", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Business", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("PayerEmail", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("ReceiverEmail", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("ItemName", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Subject", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Currency", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Gross", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("TransactionFee", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Tax", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Handling", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Shipping", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Country", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("PaymentDate", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("PaymentStatus", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("Protection", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("VerifySign", "TransactionLogReadGroup");
			//AuthorizationRules.AllowRead("PaymentType", "TransactionLogReadGroup");

			//AuthorizationRules.AllowWrite("PaypalID", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("TransactionID", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("PayerID", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("ReceiverID", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("FirstName", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("LastName", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Business", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("PayerEmail", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("ReceiverEmail", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("ItemName", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Subject", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Currency", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Gross", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("TransactionFee", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Tax", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Handling", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Shipping", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Country", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("PaymentDate", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("PaymentStatus", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("Protection", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("VerifySign", "TransactionLogWriteGroup");
			//AuthorizationRules.AllowWrite("PaymentType", "TransactionLogWriteGroup");
		}


		public static bool CanGetObject()
		{
			//TODO: Define CanGetObject permission in TransactionLog
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("TransactionLogViewGroup"))
			//	return true;
			//return false;
		}

		public static bool CanAddObject()
		{
			//TODO: Define CanAddObject permission in TransactionLog
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("TransactionLogAddGroup"))
			//	return true;
			//return false;
		}

		public static bool CanEditObject()
		{
			//TODO: Define CanEditObject permission in TransactionLog
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("TransactionLogEditGroup"))
			//	return true;
			//return false;
		}

		public static bool CanDeleteObject()
		{
			//TODO: Define CanDeleteObject permission in TransactionLog
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("TransactionLogDeleteGroup"))
			//	return true;
			//return false;
		}
		#endregion //Authorization Rules

		#region Factory Methods
		private TransactionLog()
		{ /* require use of factory method */ }

        private TransactionLog(SafeDataReader dr)
        {
            FetchObject(dr);
        }

        public static TransactionLog GetTransactionLog(SafeDataReader dr)
        {
            return new TransactionLog(dr);
        }

		public static TransactionLog NewTransactionLog()
		{
			if (!CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a TransactionLog");
			return DataPortal.Create<TransactionLog>();
		}

		public static TransactionLog GetTransactionLog(Guid id)
		{
			if (!CanGetObject())
				throw new System.Security.SecurityException("User not authorized to view a TransactionLog");
			return DataPortal.Fetch<TransactionLog>(new Criteria(id));
		}

		public static void DeleteTransactionLog(Guid id)
		{
			if (!CanDeleteObject())
				throw new System.Security.SecurityException("User not authorized to remove a TransactionLog");
			DataPortal.Delete(new Criteria(id));
		}

		public override TransactionLog Save()
		{
			if (IsDeleted && !CanDeleteObject())
				throw new System.Security.SecurityException("User not authorized to remove a TransactionLog");
			else if (IsNew && !CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a TransactionLog");
			else if (!CanEditObject())
				throw new System.Security.SecurityException("User not authorized to update a TransactionLog");

			return base.Save();
		}

		#endregion //Factory Methods

		#region Data Access

		#region Criteria

		[Serializable()]
		private class Criteria
		{
			public Guid ID;

			public Criteria(Guid id)
			{
				this.ID = id;
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
				cm.CommandText = "GetTransactionLog";

				cm.Parameters.AddWithValue("@TransactionLogID", criteria.ID);

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
			_id = dr.GetGuid("TransactionLogID");
			_registrationID = dr.GetGuid("RegistrationID");
			_paypalID = dr.GetString("PaypalID");
			_transactionID = dr.GetString("TransactionID");
			_payerID = dr.GetString("PayerID");
			_receiverID = dr.GetString("ReceiverID");
			_firstName = dr.GetString("FirstName");
			_lastName = dr.GetString("LastName");
			_business = dr.GetString("Business");
			_payerEmail = dr.GetString("PayerEmail");
			_receiverEmail = dr.GetString("ReceiverEmail");
			_itemName = dr.GetString("ItemName");
			_subject = dr.GetString("Subject");
			_currency = dr.GetString("Currency");
			_gross = dr.GetDouble("Gross");
			_transactionFee = dr.GetDouble("TransactionFee");
			_tax = dr.GetDouble("Tax");
			_handling = dr.GetDouble("Handling");
			_shipping = dr.GetDouble("Shipping");
			_country = dr.GetString("Country");
			_paymentDate = dr.GetString("PaymentDate");
			_paymentStatus = dr.GetString("PaymentStatus");
			_protection = dr.GetString("Protection");
			_verifySign = dr.GetString("VerifySign");
			_paymentType = dr.GetString("PaymentType");
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
				cm.CommandText = "AddTransactionLog";

				AddInsertParameters(cm);

				cm.ExecuteNonQuery();

				_id = (Guid)cm.Parameters["@TransactionLogID"].Value;
				//_registrationID = (Guid)cm.Parameters["@NewRegistrationID"].Value;
			}//using
		}

		private void AddInsertParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@PaypalID", _paypalID);
			if (_transactionID != string.Empty)
				cm.Parameters.AddWithValue("@TransactionID", _transactionID);
			else
				cm.Parameters.AddWithValue("@TransactionID", DBNull.Value);
			cm.Parameters.AddWithValue("@PayerID", _payerID);
			if (_receiverID != string.Empty)
				cm.Parameters.AddWithValue("@ReceiverID", _receiverID);
			else
				cm.Parameters.AddWithValue("@ReceiverID", DBNull.Value);
			if (_firstName != string.Empty)
				cm.Parameters.AddWithValue("@FirstName", _firstName);
			else
				cm.Parameters.AddWithValue("@FirstName", DBNull.Value);
			if (_lastName != string.Empty)
				cm.Parameters.AddWithValue("@LastName", _lastName);
			else
				cm.Parameters.AddWithValue("@LastName", DBNull.Value);
			cm.Parameters.AddWithValue("@Business", _business);
			if (_payerEmail != string.Empty)
				cm.Parameters.AddWithValue("@PayerEmail", _payerEmail);
			else
				cm.Parameters.AddWithValue("@PayerEmail", DBNull.Value);
			if (_receiverEmail != string.Empty)
				cm.Parameters.AddWithValue("@ReceiverEmail", _receiverEmail);
			else
				cm.Parameters.AddWithValue("@ReceiverEmail", DBNull.Value);
			cm.Parameters.AddWithValue("@ItemName", _itemName);
			if (_subject != string.Empty)
				cm.Parameters.AddWithValue("@Subject", _subject);
			else
				cm.Parameters.AddWithValue("@Subject", DBNull.Value);
			cm.Parameters.AddWithValue("@Currency", _currency);
			cm.Parameters.AddWithValue("@Gross", _gross);
			cm.Parameters.AddWithValue("@TransactionFee", _transactionFee);
			if (_tax != 0)
				cm.Parameters.AddWithValue("@Tax", _tax);
			else
				cm.Parameters.AddWithValue("@Tax", DBNull.Value);
			cm.Parameters.AddWithValue("@Handling", _handling);
			cm.Parameters.AddWithValue("@Shipping", _shipping);
			if (_country != string.Empty)
				cm.Parameters.AddWithValue("@Country", _country);
			else
				cm.Parameters.AddWithValue("@Country", DBNull.Value);
			cm.Parameters.AddWithValue("@PaymentDate", _paymentDate);
			cm.Parameters.AddWithValue("@PaymentStatus", _paymentStatus);
			if (_protection != string.Empty)
				cm.Parameters.AddWithValue("@Protection", _protection);
			else
				cm.Parameters.AddWithValue("@Protection", DBNull.Value);
			cm.Parameters.AddWithValue("@VerifySign", _verifySign);
			if (_paymentType != string.Empty)
				cm.Parameters.AddWithValue("@PaymentType", _paymentType);
			else
				cm.Parameters.AddWithValue("@PaymentType", DBNull.Value);
			cm.Parameters.AddWithValue("@TransactionLogID", _id);
			cm.Parameters["@TransactionLogID"].Direction = ParameterDirection.Output;
			cm.Parameters.AddWithValue("@RegistrationID", _registrationID);
			//cm.Parameters["@NewRegistrationID"].Direction = ParameterDirection.Output;
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
				cm.CommandText = "UpdateTransactionLog";

				AddUpdateParameters(cm);

				cm.ExecuteNonQuery();

			}//using
		}

		private void AddUpdateParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@TransactionLogID", _id);
			cm.Parameters.AddWithValue("@RegistrationID", _registrationID);
			cm.Parameters.AddWithValue("@PaypalID", _paypalID);
			if (_transactionID != string.Empty)
				cm.Parameters.AddWithValue("@TransactionID", _transactionID);
			else
				cm.Parameters.AddWithValue("@TransactionID", DBNull.Value);
			cm.Parameters.AddWithValue("@PayerID", _payerID);
			if (_receiverID != string.Empty)
				cm.Parameters.AddWithValue("@ReceiverID", _receiverID);
			else
				cm.Parameters.AddWithValue("@ReceiverID", DBNull.Value);
			if (_firstName != string.Empty)
				cm.Parameters.AddWithValue("@FirstName", _firstName);
			else
				cm.Parameters.AddWithValue("@FirstName", DBNull.Value);
			if (_lastName != string.Empty)
				cm.Parameters.AddWithValue("@LastName", _lastName);
			else
				cm.Parameters.AddWithValue("@LastName", DBNull.Value);
			cm.Parameters.AddWithValue("@Business", _business);
			if (_payerEmail != string.Empty)
				cm.Parameters.AddWithValue("@PayerEmail", _payerEmail);
			else
				cm.Parameters.AddWithValue("@PayerEmail", DBNull.Value);
			if (_receiverEmail != string.Empty)
				cm.Parameters.AddWithValue("@ReceiverEmail", _receiverEmail);
			else
				cm.Parameters.AddWithValue("@ReceiverEmail", DBNull.Value);
			cm.Parameters.AddWithValue("@ItemName", _itemName);
			if (_subject != string.Empty)
				cm.Parameters.AddWithValue("@Subject", _subject);
			else
				cm.Parameters.AddWithValue("@Subject", DBNull.Value);
			cm.Parameters.AddWithValue("@Currency", _currency);
			cm.Parameters.AddWithValue("@Gross", _gross);
			cm.Parameters.AddWithValue("@TransactionFee", _transactionFee);
			if (_tax != 0)
				cm.Parameters.AddWithValue("@Tax", _tax);
			else
				cm.Parameters.AddWithValue("@Tax", DBNull.Value);
			cm.Parameters.AddWithValue("@Handling", _handling);
			cm.Parameters.AddWithValue("@Shipping", _shipping);
			if (_country != string.Empty)
				cm.Parameters.AddWithValue("@Country", _country);
			else
				cm.Parameters.AddWithValue("@Country", DBNull.Value);
			cm.Parameters.AddWithValue("@PaymentDate", _paymentDate);
			cm.Parameters.AddWithValue("@PaymentStatus", _paymentStatus);
			if (_protection != string.Empty)
				cm.Parameters.AddWithValue("@Protection", _protection);
			else
				cm.Parameters.AddWithValue("@Protection", DBNull.Value);
			cm.Parameters.AddWithValue("@VerifySign", _verifySign);
			if (_paymentType != string.Empty)
				cm.Parameters.AddWithValue("@PaymentType", _paymentType);
			else
				cm.Parameters.AddWithValue("@PaymentType", DBNull.Value);
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
				cm.CommandText = "DeleteTransactionLog";

				cm.Parameters.AddWithValue("@TransactionLogID", criteria.ID);

				cm.ExecuteNonQuery();
			}//using
		}
		#endregion //Data Access - Delete
		#endregion //Data Access
	}
}
