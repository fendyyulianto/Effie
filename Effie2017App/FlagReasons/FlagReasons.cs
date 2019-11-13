using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class FlagReasons : Csla.BusinessBase<FlagReasons>
    {
        #region Business Properties and Methods

		//declare members
		private Guid _id = Guid.NewGuid();
		private string _description = string.Empty;
		private string _bodyname = string.Empty;
		private string _header = string.Empty;
		private bool _isActive = false;
		private string _type = string.Empty;
		private string _subHeader = string.Empty;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private bool _isHasOther = false;

		[System.ComponentModel.DataObjectField(true, false)]
		public Guid Id
		{
			get
			{
				CanReadProperty("Id", true);
				return _id;
			}
		}

		public string Description
		{
			get
			{
				CanReadProperty("Description", true);
				return _description;
			}
			set
			{
				CanWriteProperty("Description", true);
				if (value == null) value = string.Empty;
				if (!_description.Equals(value))
				{
					_description = value;
					PropertyHasChanged("Description");
				}
			}
		}

		public string Bodyname
		{
			get
			{
				CanReadProperty("Bodyname", true);
				return _bodyname;
			}
			set
			{
				CanWriteProperty("Bodyname", true);
				if (value == null) value = string.Empty;
				if (!_bodyname.Equals(value))
				{
					_bodyname = value;
					PropertyHasChanged("Bodyname");
				}
			}
		}

		public string Header
		{
			get
			{
				CanReadProperty("Header", true);
				return _header;
			}
			set
			{
				CanWriteProperty("Header", true);
				if (value == null) value = string.Empty;
				if (!_header.Equals(value))
				{
					_header = value;
					PropertyHasChanged("Header");
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

		public string SubHeader
		{
			get
			{
				CanReadProperty("SubHeader", true);
				return _subHeader;
			}
			set
			{
				CanWriteProperty("SubHeader", true);
				if (value == null) value = string.Empty;
				if (!_subHeader.Equals(value))
				{
					_subHeader = value;
					PropertyHasChanged("SubHeader");
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

        public bool isHasOther
        {
            get
            {
                CanReadProperty("isHasOther", true);
                return _isHasOther;
            }
            set
            {
                CanWriteProperty("isHasOther", true);
                if (!_isHasOther.Equals(value))
                {
                    _isHasOther = value;
                    PropertyHasChanged("isHasOther");
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
			// Bodyname
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Bodyname", 1000));
			//
			// Header
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Header", 1000));
			//
			// Type
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Type", 1000));
			//
			// SubHeader
			//
			ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SubHeader", 1000));
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
            //TODO: Define authorization rules in FlagReasons
            //AuthorizationRules.AllowRead("Id", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("Description", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("Bodyname", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("Header", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("isActive", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("Type", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("SubHeader", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "FlagReasonsReadGroup");
            //AuthorizationRules.AllowRead("isHasOther", "FlagReasonsReadGroup");

            //AuthorizationRules.AllowWrite("Description", "FlagReasonsWriteGroup");
            //AuthorizationRules.AllowWrite("Bodyname", "FlagReasonsWriteGroup");
            //AuthorizationRules.AllowWrite("Header", "FlagReasonsWriteGroup");
            //AuthorizationRules.AllowWrite("isActive", "FlagReasonsWriteGroup");
            //AuthorizationRules.AllowWrite("Type", "FlagReasonsWriteGroup");
            //AuthorizationRules.AllowWrite("SubHeader", "FlagReasonsWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "FlagReasonsWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "FlagReasonsWriteGroup");
            //AuthorizationRules.AllowWrite("isHasOther", "FlagReasonsWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in FlagReasons
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("FlagReasonsViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in FlagReasons
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("FlagReasonsAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in FlagReasons
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("FlagReasonsEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in FlagReasons
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("FlagReasonsDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private FlagReasons()
        { /* require use of factory method */ }

        private FlagReasons(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static FlagReasons GetFlagReasons(SafeDataReader dr)
        {
            return new FlagReasons(dr);
        }

        public static FlagReasons NewFlagReasons()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a FlagReasons");
            return DataPortal.Create<FlagReasons>();
        }

        public static FlagReasons GetFlagReasons(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a FlagReasons");
            return DataPortal.Fetch<FlagReasons>(new Criteria(id));
        }

        public static void DeleteFlagReasons(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a FlagReasons");
            DataPortal.Delete(new Criteria(id));
        }

        public override FlagReasons Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a FlagReasons");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a FlagReasons");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a FlagReasons");

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
            using (SqlConnection cn =  new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "GetFlagReasons";

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
			_description = dr.GetString("Description");
			_bodyname = dr.GetString("Bodyname");
			_header = dr.GetString("Header");
			_isActive = dr.GetBoolean("isActive");
			_type = dr.GetString("Type");
			_subHeader = dr.GetString("SubHeader");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _isHasOther = dr.GetBoolean("isHasOther");
        }

        private void FetchChildren(SafeDataReader dr)
        {
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert
        protected override void DataPortal_Insert()
        {
            using (SqlConnection cn =  new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "AddFlagReasons";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                //_id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
			cm.Parameters.AddWithValue("@Id", _id);
			if (_description.Length > 0)
				cm.Parameters.AddWithValue("@Description", _description);
			else
				cm.Parameters.AddWithValue("@Description", DBNull.Value);
			if (_bodyname.Length > 0)
				cm.Parameters.AddWithValue("@Bodyname", _bodyname);
			else
				cm.Parameters.AddWithValue("@Bodyname", DBNull.Value);
			if (_header.Length > 0)
				cm.Parameters.AddWithValue("@Header", _header);
			else
				cm.Parameters.AddWithValue("@Header", DBNull.Value);
			if (_isActive != false)
				cm.Parameters.AddWithValue("@isActive", _isActive);
			else
				cm.Parameters.AddWithValue("@isActive", DBNull.Value);
			if (_type.Length > 0)
				cm.Parameters.AddWithValue("@Type", _type);
			else
				cm.Parameters.AddWithValue("@Type", DBNull.Value);
			if (_subHeader.Length > 0)
				cm.Parameters.AddWithValue("@SubHeader", _subHeader);
			else
				cm.Parameters.AddWithValue("@SubHeader", DBNull.Value);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            if (_isHasOther != false)
                cm.Parameters.AddWithValue("@isHasOther", _isHasOther);
            else
                cm.Parameters.AddWithValue("@isHasOther", DBNull.Value);
		}
        #endregion //Data Access - Insert

        #region Data Access - Update
        protected override void DataPortal_Update()
        {
            using (SqlConnection cn =  new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "UpdateFlagReasons";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
			cm.Parameters.AddWithValue("@Id", _id);
			if (_description.Length > 0)
				cm.Parameters.AddWithValue("@Description", _description);
			else
				cm.Parameters.AddWithValue("@Description", DBNull.Value);
			if (_bodyname.Length > 0)
				cm.Parameters.AddWithValue("@Bodyname", _bodyname);
			else
				cm.Parameters.AddWithValue("@Bodyname", DBNull.Value);
			if (_header.Length > 0)
				cm.Parameters.AddWithValue("@Header", _header);
			else
				cm.Parameters.AddWithValue("@Header", DBNull.Value);
			if (_isActive != false)
				cm.Parameters.AddWithValue("@isActive", _isActive);
			else
				cm.Parameters.AddWithValue("@isActive", DBNull.Value);
			if (_type.Length > 0)
				cm.Parameters.AddWithValue("@Type", _type);
			else
				cm.Parameters.AddWithValue("@Type", DBNull.Value);
			if (_subHeader.Length > 0)
				cm.Parameters.AddWithValue("@SubHeader", _subHeader);
			else
				cm.Parameters.AddWithValue("@SubHeader", DBNull.Value);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            if (_isHasOther != false)
                cm.Parameters.AddWithValue("@isHasOther", _isHasOther);
            else
                cm.Parameters.AddWithValue("@isHasOther", DBNull.Value);
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
            using (SqlConnection cn =  new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "DeleteFlagReasons";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
