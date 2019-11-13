using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{ 
	[Serializable()] 
	public class RawLog : Csla.BusinessBase<RawLog>
	{
		#region Business Properties and Methods

		//declare members
		private Guid _id = Guid.NewGuid();
		private int _type = 0;
		private string _data = string.Empty;
		private SmartDate _date = new SmartDate(false);

		[System.ComponentModel.DataObjectField(true, true)]
		public Guid Id
		{
			get
			{
				CanReadProperty("Id", true);
				return _id;
			}
		}

		public int Type
		{
			get
			{
				CanReadProperty("Type", true);
				return _type;
			}
			set
			{
				CanWriteProperty("Type", true);
				if (!_type.Equals(value))
				{
					_type = value;
					PropertyHasChanged("Type");
				}
			}
		}

		public string Data
		{
			get
			{
				CanReadProperty("Data", true);
				return _data;
			}
			set
			{
				CanWriteProperty("Data", true);
				if (value == null) value = string.Empty;
				if (!_data.Equals(value))
				{
					_data = value;
					PropertyHasChanged("Data");
				}
			}
		}

		public DateTime Date
		{
			get
			{
				CanReadProperty("Date", true);
				return _date.Date;
			}
		}

		public string DateString
		{
			get
			{
				CanReadProperty("Date", true);
				return _date.Text;
			}
			set
			{
				CanWriteProperty("Date", true);
				if (value == null) value = string.Empty;
				if (!_date.Equals(value))
				{
					_date.Text = value;
					PropertyHasChanged("Date");
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
			//TODO: Define authorization rules in RawLog
			//AuthorizationRules.AllowRead("Id", "RawLogReadGroup");
			//AuthorizationRules.AllowRead("Type", "RawLogReadGroup");
			//AuthorizationRules.AllowRead("Data", "RawLogReadGroup");
			//AuthorizationRules.AllowRead("Date", "RawLogReadGroup");

			//AuthorizationRules.AllowWrite("Type", "RawLogWriteGroup");
			//AuthorizationRules.AllowWrite("Data", "RawLogWriteGroup");
			//AuthorizationRules.AllowWrite("Date", "RawLogWriteGroup");
		}


		public static bool CanGetObject()
		{
			//TODO: Define CanGetObject permission in RawLog
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("RawLogViewGroup"))
			//	return true;
			//return false;
		}

		public static bool CanAddObject()
		{
			//TODO: Define CanAddObject permission in RawLog
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("RawLogAddGroup"))
			//	return true;
			//return false;
		}

		public static bool CanEditObject()
		{
			//TODO: Define CanEditObject permission in RawLog
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("RawLogEditGroup"))
			//	return true;
			//return false;
		}

		public static bool CanDeleteObject()
		{
			//TODO: Define CanDeleteObject permission in RawLog
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("RawLogDeleteGroup"))
			//	return true;
			//return false;
		}
		#endregion //Authorization Rules

		#region Factory Methods
		private RawLog()
		{ /* require use of factory method */ }

        private RawLog(SafeDataReader dr)
        {
            FetchObject(dr);
        }

        public static RawLog GetRawLog(SafeDataReader dr)
        {
            return new RawLog(dr);
        }

		public static RawLog NewRawLog()
		{
			if (!CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a RawLog");
			return DataPortal.Create<RawLog>();
		}

		public static RawLog GetRawLog(Guid id)
		{
			if (!CanGetObject())
				throw new System.Security.SecurityException("User not authorized to view a RawLog");
			return DataPortal.Fetch<RawLog>(new Criteria(id));
		}

		public static void DeleteRawLog(Guid id)
		{
			if (!CanDeleteObject())
				throw new System.Security.SecurityException("User not authorized to remove a RawLog");
			DataPortal.Delete(new Criteria(id));
		}

		public override RawLog Save()
		{
			if (IsDeleted && !CanDeleteObject())
				throw new System.Security.SecurityException("User not authorized to remove a RawLog");
			else if (IsNew && !CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a RawLog");
			else if (!CanEditObject())
				throw new System.Security.SecurityException("User not authorized to update a RawLog");

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
				cm.CommandText = "GetRawLog";

				cm.Parameters.AddWithValue("@ID", criteria.Id);

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
			_id = dr.GetGuid("ID");
			_type = dr.GetInt32("Type");
			_data = dr.GetString("Data");
			_date = dr.GetSmartDate("Date", _date.EmptyIsMin);
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
				cm.CommandText = "AddRawLog";

				AddInsertParameters(cm);

				cm.ExecuteNonQuery();

				_id = (Guid)cm.Parameters["@ID"].Value;
			}//using
		}

		private void AddInsertParameters(SqlCommand cm)
		{
			//if (_type != 0)
				cm.Parameters.AddWithValue("@Type", _type);
			//else
			//	cm.Parameters.AddWithValue("@Type", DBNull.Value);
			if (_data != string.Empty)
				cm.Parameters.AddWithValue("@Data", _data);
			else
				cm.Parameters.AddWithValue("@Data", DBNull.Value);
			cm.Parameters.AddWithValue("@Date", _date.DBValue);
			cm.Parameters.AddWithValue("@ID", _id);
			cm.Parameters["@ID"].Direction = ParameterDirection.Output;
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
				cm.CommandText = "UpdateRawLog";

				AddUpdateParameters(cm);

				cm.ExecuteNonQuery();

			}//using
		}

		private void AddUpdateParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@ID", _id);
			//if (_type != 0)
				cm.Parameters.AddWithValue("@Type", _type);
			//else
			//	cm.Parameters.AddWithValue("@Type", DBNull.Value);
			if (_data != string.Empty)
				cm.Parameters.AddWithValue("@Data", _data);
			else
				cm.Parameters.AddWithValue("@Data", DBNull.Value);
			cm.Parameters.AddWithValue("@Date", _date.DBValue);
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
				cm.CommandText = "DeleteRawLog";

				cm.Parameters.AddWithValue("@ID", criteria.Id);

				cm.ExecuteNonQuery();
			}//using
		}
		#endregion //Data Access - Delete
		#endregion //Data Access
	}
}
