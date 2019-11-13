using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Linq;

namespace Effie2017.App
{ 
	[Serializable()] 
	public class EntryPayment : Csla.BusinessBase<EntryPayment>
	{
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _payGroupId = Guid.Empty;
        private Guid _EntryId = Guid.Empty;
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
		private decimal _amount = 0;
		private decimal _fee = 0;
		private decimal _tax = 0;
		private decimal _grandAmount = 0;
		private string _invoice = string.Empty;
		private decimal _amountReceived = 0;

		[System.ComponentModel.DataObjectField(true, false)]
		public Guid Id
		{
			get
			{
				CanReadProperty("Id", true);
				return _id;
            }
            set
            {
                CanWriteProperty("Id", true);
                if (!_id.Equals(value))
                {
                    _id = value;
                    PropertyHasChanged("Id");
                }
            }
        }
        public Guid EntryId
        {
            get
            {
                CanReadProperty("EntryId", true);
                return _EntryId;
            }
            set
            {
                CanWriteProperty("EntryId", true);
                if (!_EntryId.Equals(value))
                {
                    _EntryId = value;
                    PropertyHasChanged("EntryId");
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
			//TODO: Define authorization rules in EntryPayment
			//AuthorizationRules.AllowRead("Id", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayGroupId", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PaymentMethod", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayCompany", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayAddress1", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayAddress2", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayCity", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayPostal", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayCountry", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayFirstname", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayLastname", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("PayContact", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("Amount", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("Fee", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("Tax", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("GrandAmount", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("Invoice", "EntryPaymentReadGroup");
			//AuthorizationRules.AllowRead("AmountReceived", "EntryPaymentReadGroup");

			//AuthorizationRules.AllowWrite("PayGroupId", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PaymentMethod", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayCompany", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayAddress1", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayAddress2", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayCity", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayPostal", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayCountry", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayFirstname", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayLastname", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("PayContact", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("Amount", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("Fee", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("Tax", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("GrandAmount", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("Invoice", "EntryPaymentWriteGroup");
			//AuthorizationRules.AllowWrite("AmountReceived", "EntryPaymentWriteGroup");
		}


		public static bool CanGetObject()
		{
			//TODO: Define CanGetObject permission in EntryPayment
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("EntryPaymentViewGroup"))
			//	return true;
			//return false;
		}

		public static bool CanAddObject()
		{
			//TODO: Define CanAddObject permission in EntryPayment
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("EntryPaymentAddGroup"))
			//	return true;
			//return false;
		}

		public static bool CanEditObject()
		{
			//TODO: Define CanEditObject permission in EntryPayment
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("EntryPaymentEditGroup"))
			//	return true;
			//return false;
		}

		public static bool CanDeleteObject()
		{
			//TODO: Define CanDeleteObject permission in EntryPayment
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("EntryPaymentDeleteGroup"))
			//	return true;
			//return false;
		}
		#endregion //Authorization Rules

		#region Factory Methods
		private EntryPayment()
		{ /* require use of factory method */ }

		private EntryPayment(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }
		public static EntryPayment NewEntryPayment()
		{
			if (!CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a EntryPayment");
			return DataPortal.Create<EntryPayment>();
		}

		public static EntryPayment GetEntryPayment(Guid id)
		{
			if (!CanGetObject())
				throw new System.Security.SecurityException("User not authorized to view a EntryPayment");
			return DataPortal.Fetch<EntryPayment>(new Criteria(id));
		}

		public static EntryPayment GetEntryPayment(SafeDataReader dr)
        {
            return new EntryPayment(dr);
        }
		public static void DeleteEntryPayment(Guid id)
		{
			if (!CanDeleteObject())
				throw new System.Security.SecurityException("User not authorized to remove a EntryPayment");
			DataPortal.Delete(new Criteria(id));
		}

		public override EntryPayment Save()
		{
			if (IsDeleted && !CanDeleteObject())
				throw new System.Security.SecurityException("User not authorized to remove a EntryPayment");
			else if (IsNew && !CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a EntryPayment");
			else if (!CanEditObject())
				throw new System.Security.SecurityException("User not authorized to update a EntryPayment");

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
				cm.CommandText = "GetEntryPayment";

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
            _EntryId = dr.GetGuid("EntryId");
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
			_amount = dr.GetDecimal("Amount");
			_fee = dr.GetDecimal("Fee");
			_tax = dr.GetDecimal("Tax");
			_grandAmount = dr.GetDecimal("GrandAmount");
			_invoice = dr.GetString("Invoice");
			_amountReceived = dr.GetDecimal("AmountReceived");
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
				cm.CommandText = "AddEntryPayment";

				AddInsertParameters(cm);

				cm.ExecuteNonQuery();

			}//using
		}

		private void AddInsertParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@Id", _id);
            if (_payGroupId != Guid.Empty)
                cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
            else
                cm.Parameters.AddWithValue("@PayGroupId", DBNull.Value);


            if (_EntryId != Guid.Empty)
                cm.Parameters.AddWithValue("@EntryId", _EntryId);
            else
                cm.Parameters.AddWithValue("@EntryId", DBNull.Value);


            if (_paymentMethod.Length > 0)
				cm.Parameters.AddWithValue("@PaymentMethod", _paymentMethod);
			else
				cm.Parameters.AddWithValue("@PaymentMethod", DBNull.Value);
			if (_payCompany.Length > 0)
				cm.Parameters.AddWithValue("@PayCompany", _payCompany);
			else
				cm.Parameters.AddWithValue("@PayCompany", DBNull.Value);
			if (_payAddress1.Length > 0)
				cm.Parameters.AddWithValue("@PayAddress1", _payAddress1);
			else
				cm.Parameters.AddWithValue("@PayAddress1", DBNull.Value);
			if (_payAddress2.Length > 0)
				cm.Parameters.AddWithValue("@PayAddress2", _payAddress2);
			else
				cm.Parameters.AddWithValue("@PayAddress2", DBNull.Value);
			if (_payCity.Length > 0)
				cm.Parameters.AddWithValue("@PayCity", _payCity);
			else
				cm.Parameters.AddWithValue("@PayCity", DBNull.Value);
			if (_payPostal.Length > 0)
				cm.Parameters.AddWithValue("@PayPostal", _payPostal);
			else
				cm.Parameters.AddWithValue("@PayPostal", DBNull.Value);
			if (_payCountry.Length > 0)
				cm.Parameters.AddWithValue("@PayCountry", _payCountry);
			else
				cm.Parameters.AddWithValue("@PayCountry", DBNull.Value);
			if (_payFirstname.Length > 0)
				cm.Parameters.AddWithValue("@PayFirstname", _payFirstname);
			else
				cm.Parameters.AddWithValue("@PayFirstname", DBNull.Value);
			if (_payLastname.Length > 0)
				cm.Parameters.AddWithValue("@PayLastname", _payLastname);
			else
				cm.Parameters.AddWithValue("@PayLastname", DBNull.Value);
			if (_payContact.Length > 0)
				cm.Parameters.AddWithValue("@PayContact", _payContact);
			else
				cm.Parameters.AddWithValue("@PayContact", DBNull.Value);
			if (_amount != 0)
				cm.Parameters.AddWithValue("@Amount", _amount);
			else
				cm.Parameters.AddWithValue("@Amount", DBNull.Value);
			if (_fee != 0)
				cm.Parameters.AddWithValue("@Fee", _fee);
			else
				cm.Parameters.AddWithValue("@Fee", DBNull.Value);
			if (_tax != 0)
				cm.Parameters.AddWithValue("@Tax", _tax);
			else
				cm.Parameters.AddWithValue("@Tax", DBNull.Value);
			if (_grandAmount != 0)
				cm.Parameters.AddWithValue("@GrandAmount", _grandAmount);
			else
				cm.Parameters.AddWithValue("@GrandAmount", DBNull.Value);
			if (_invoice.Length > 0)
				cm.Parameters.AddWithValue("@Invoice", _invoice);
			else
				cm.Parameters.AddWithValue("@Invoice", DBNull.Value);
			if (_amountReceived != 0)
				cm.Parameters.AddWithValue("@AmountReceived", _amountReceived);
			else
				cm.Parameters.AddWithValue("@AmountReceived", DBNull.Value);
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
				cm.CommandText = "UpdateEntryPayment";

				AddUpdateParameters(cm);

				cm.ExecuteNonQuery();

			}//using
		}

		private void AddUpdateParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@Id", _id);
			if (_payGroupId != Guid.Empty)
				cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
			else
				cm.Parameters.AddWithValue("@PayGroupId", DBNull.Value);


            if (_EntryId != Guid.Empty)
                cm.Parameters.AddWithValue("@EntryId", _EntryId);
            else
                cm.Parameters.AddWithValue("@EntryId", DBNull.Value);

            if (_paymentMethod.Length > 0)
				cm.Parameters.AddWithValue("@PaymentMethod", _paymentMethod);
			else
				cm.Parameters.AddWithValue("@PaymentMethod", DBNull.Value);
			if (_payCompany.Length > 0)
				cm.Parameters.AddWithValue("@PayCompany", _payCompany);
			else
				cm.Parameters.AddWithValue("@PayCompany", DBNull.Value);
			if (_payAddress1.Length > 0)
				cm.Parameters.AddWithValue("@PayAddress1", _payAddress1);
			else
				cm.Parameters.AddWithValue("@PayAddress1", DBNull.Value);
			if (_payAddress2.Length > 0)
				cm.Parameters.AddWithValue("@PayAddress2", _payAddress2);
			else
				cm.Parameters.AddWithValue("@PayAddress2", DBNull.Value);
			if (_payCity.Length > 0)
				cm.Parameters.AddWithValue("@PayCity", _payCity);
			else
				cm.Parameters.AddWithValue("@PayCity", DBNull.Value);
			if (_payPostal.Length > 0)
				cm.Parameters.AddWithValue("@PayPostal", _payPostal);
			else
				cm.Parameters.AddWithValue("@PayPostal", DBNull.Value);
			if (_payCountry.Length > 0)
				cm.Parameters.AddWithValue("@PayCountry", _payCountry);
			else
				cm.Parameters.AddWithValue("@PayCountry", DBNull.Value);
			if (_payFirstname.Length > 0)
				cm.Parameters.AddWithValue("@PayFirstname", _payFirstname);
			else
				cm.Parameters.AddWithValue("@PayFirstname", DBNull.Value);
			if (_payLastname.Length > 0)
				cm.Parameters.AddWithValue("@PayLastname", _payLastname);
			else
				cm.Parameters.AddWithValue("@PayLastname", DBNull.Value);
			if (_payContact.Length > 0)
				cm.Parameters.AddWithValue("@PayContact", _payContact);
			else
				cm.Parameters.AddWithValue("@PayContact", DBNull.Value);
			if (_amount != 0)
				cm.Parameters.AddWithValue("@Amount", _amount);
			else
				cm.Parameters.AddWithValue("@Amount", DBNull.Value);
			if (_fee != 0)
				cm.Parameters.AddWithValue("@Fee", _fee);
			else
				cm.Parameters.AddWithValue("@Fee", DBNull.Value);
			if (_tax != 0)
				cm.Parameters.AddWithValue("@Tax", _tax);
			else
				cm.Parameters.AddWithValue("@Tax", DBNull.Value);
			if (_grandAmount != 0)
				cm.Parameters.AddWithValue("@GrandAmount", _grandAmount);
			else
				cm.Parameters.AddWithValue("@GrandAmount", DBNull.Value);
			if (_invoice.Length > 0)
				cm.Parameters.AddWithValue("@Invoice", _invoice);
			else
				cm.Parameters.AddWithValue("@Invoice", DBNull.Value);
			if (_amountReceived != 0)
				cm.Parameters.AddWithValue("@AmountReceived", _amountReceived);
			else
				cm.Parameters.AddWithValue("@AmountReceived", DBNull.Value);
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
				cm.CommandText = "DeleteEntryPayment";

				cm.Parameters.AddWithValue("@Id", criteria.Id);

				cm.ExecuteNonQuery();
			}//using
		}
		#endregion //Data Access - Delete
		#endregion //Data Access
	}
}
