using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{ 
	[Serializable()] 
	public class LoginHistory : Csla.BusinessBase<LoginHistory>
	{
		#region Business Properties and Methods

		//declare members
		private Guid _id = Guid.NewGuid();
		private SmartDate _dateCreated = new SmartDate(false);
		private SmartDate _dateModified = new SmartDate(false);
		private Guid _userId = Guid.Empty;
        private string _type = string.Empty;
        private string _IPAddress = string.Empty;

        [System.ComponentModel.DataObjectField(true, false)]
		public Guid Id
		{
			get
			{
				CanReadProperty("Id", true);
				return _id;
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
				CanReadProperty("DateCreatedString", true);
				return _dateCreated.Text;
			}
			set
			{
				CanWriteProperty("DateCreatedString", true);
				if (value == null) value = string.Empty;
				if (!_dateCreated.Equals(value))
				{
					_dateCreated.Text = value;
					PropertyHasChanged("DateCreatedString");
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
				CanReadProperty("DateModifiedString", true);
				return _dateModified.Text;
			}
			set
			{
				CanWriteProperty("DateModifiedString", true);
				if (value == null) value = string.Empty;
				if (!_dateModified.Equals(value))
				{
					_dateModified.Text = value;
					PropertyHasChanged("DateModifiedString");
				}
			}
		}

		public Guid UserId
		{
			get
			{
				CanReadProperty("UserId", true);
				return _userId;
			}
			set
			{
				CanWriteProperty("UserId", true);
				if (!_userId.Equals(value))
				{
					_userId = value;
					PropertyHasChanged("UserId");
				}
			}
        }

        public string IPAddress
        {
            get
            {
                CanReadProperty("IPAddress", true);
                return _IPAddress;
            }
            set
            {
                CanWriteProperty("IPAddress", true);
                if (value == null) value = string.Empty;
                if (!_IPAddress.Equals(value))
                {
                    _IPAddress = value;
                    PropertyHasChanged("IPAddress");
                }
            }
        }
        
        public string Type
		{
			get
			{
				CanReadProperty("Type", true);
				return _type;
			}
			set
			{
				CanWriteProperty("Type", true);
				if (value == null) value = string.Empty;
				if (!_type.Equals(value))
				{
					_type = value;
					PropertyHasChanged("Type");
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
			// Type
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Type", 100));
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
			//TODO: Define authorization rules in LoginHistory
			//AuthorizationRules.AllowRead("Id", "LoginHistoryReadGroup");
			//AuthorizationRules.AllowRead("DateCreated", "LoginHistoryReadGroup");
			//AuthorizationRules.AllowRead("DateCreatedString", "LoginHistoryReadGroup");
			//AuthorizationRules.AllowRead("DateModified", "LoginHistoryReadGroup");
			//AuthorizationRules.AllowRead("DateModifiedString", "LoginHistoryReadGroup");
			//AuthorizationRules.AllowRead("UserId", "LoginHistoryReadGroup");
			//AuthorizationRules.AllowRead("Type", "LoginHistoryReadGroup");

			//AuthorizationRules.AllowWrite("DateCreatedString", "LoginHistoryWriteGroup");
			//AuthorizationRules.AllowWrite("DateModifiedString", "LoginHistoryWriteGroup");
			//AuthorizationRules.AllowWrite("UserId", "LoginHistoryWriteGroup");
			//AuthorizationRules.AllowWrite("Type", "LoginHistoryWriteGroup");
		}


		public static bool CanGetObject()
		{
			//TODO: Define CanGetObject permission in LoginHistory
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("LoginHistoryViewGroup"))
			//	return true;
			//return false;
		}

		public static bool CanAddObject()
		{
			//TODO: Define CanAddObject permission in LoginHistory
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("LoginHistoryAddGroup"))
			//	return true;
			//return false;
		}

		public static bool CanEditObject()
		{
			//TODO: Define CanEditObject permission in LoginHistory
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("LoginHistoryEditGroup"))
			//	return true;
			//return false;
		}

		public static bool CanDeleteObject()
		{
			//TODO: Define CanDeleteObject permission in LoginHistory
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("LoginHistoryDeleteGroup"))
			//	return true;
			//return false;
		}
		#endregion //Authorization Rules

		#region Factory Methods
		private LoginHistory()
		{ /* require use of factory method */ }

		private LoginHistory(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }
		public static LoginHistory GetLoginHistory(SafeDataReader dr)
        {
            return new LoginHistory(dr);
        }
		public static LoginHistory NewLoginHistory()
		{
			if (!CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a LoginHistory");
			return DataPortal.Create<LoginHistory>();
		}

		public static LoginHistory GetLoginHistory(Guid id)
		{
			if (!CanGetObject())
				throw new System.Security.SecurityException("User not authorized to view a LoginHistory");
			return DataPortal.Fetch<LoginHistory>(new Criteria(id));
		}

		public static void DeleteLoginHistory(Guid id)
		{
			if (!CanDeleteObject())
				throw new System.Security.SecurityException("User not authorized to remove a LoginHistory");
			DataPortal.Delete(new Criteria(id));
		}

		public override LoginHistory Save()
		{
			if (IsDeleted && !CanDeleteObject())
				throw new System.Security.SecurityException("User not authorized to remove a LoginHistory");
			else if (IsNew && !CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a LoginHistory");
			else if (!CanEditObject())
				throw new System.Security.SecurityException("User not authorized to update a LoginHistory");

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
				cm.CommandText = "GetLoginHistory";

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
			_dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
			_dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
			_userId = dr.GetGuid("UserId");
            _type = dr.GetString("Type");
            _IPAddress = dr.GetString("IPAddress");
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
				cm.CommandText = "AddLoginHistory";

				AddInsertParameters(cm);

				cm.ExecuteNonQuery();

			}//using
		}

		private void AddInsertParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@Id", _id);
			cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
			cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
			if (_userId != Guid.Empty)
				cm.Parameters.AddWithValue("@UserId", _userId);
			else
				cm.Parameters.AddWithValue("@UserId", DBNull.Value);

            if (_type.Length > 0)
                cm.Parameters.AddWithValue("@Type", _type);
            else
                cm.Parameters.AddWithValue("@Type", DBNull.Value);

            if (_IPAddress.Length > 0)
                cm.Parameters.AddWithValue("@IPAddress", _IPAddress);
            else
                cm.Parameters.AddWithValue("@IPAddress", DBNull.Value);

            

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
				cm.CommandText = "UpdateLoginHistory";

				AddUpdateParameters(cm);

				cm.ExecuteNonQuery();

			}//using
		}

		private void AddUpdateParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@Id", _id);
			cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
			cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
			if (_userId != Guid.Empty)
				cm.Parameters.AddWithValue("@UserId", _userId);
			else
				cm.Parameters.AddWithValue("@UserId", DBNull.Value);

			if (_type.Length > 0)
				cm.Parameters.AddWithValue("@Type", _type);
			else
				cm.Parameters.AddWithValue("@Type", DBNull.Value);
            
            if (_IPAddress.Length > 0)
                cm.Parameters.AddWithValue("@IPAddress", _IPAddress);
            else
                cm.Parameters.AddWithValue("@IPAddress", DBNull.Value);

            

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
				cm.CommandText = "DeleteLoginHistory";

				cm.Parameters.AddWithValue("@Id", criteria.Id);

				cm.ExecuteNonQuery();
			}//using
		}
		#endregion //Data Access - Delete
		#endregion //Data Access
	}
}
