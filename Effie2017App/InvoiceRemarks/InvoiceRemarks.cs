using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class InvoiceRemarks : Csla.BusinessBase<InvoiceRemarks>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private Guid _payGroupId = Guid.NewGuid();

        private string _remarks = string.Empty;
        private SmartDate _dateTimeCreated = new SmartDate(false);
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

        public DateTime DateTimeCreated
        {
            get
            {
                CanReadProperty("DateTimeCreated", true);
                return _dateTimeCreated.Date;
            }
        }

        public string DateTimeCreatedString
        {
            get
            {
                CanReadProperty("DateTimeCreated", true);
                return _dateTimeCreated.Text;
            }
            set
            {
                CanWriteProperty("DateTimeCreated", true);

                if (value == null) value = string.Empty;
                if (!_dateTimeCreated.Equals(value))
                {
                    _dateTimeCreated.Text = value;
                    PropertyHasChanged("DateTimeCreated");

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
            //TODO: Define authorization rules in InvoiceRemarks
            //AuthorizationRules.AllowRead("Id", "InvoiceRemarksReadGroup");
            //AuthorizationRules.AllowRead("PayGroupId", "InvoiceRemarksReadGroup");
            //AuthorizationRules.AllowRead("Remarks", "InvoiceRemarksReadGroup");
            //AuthorizationRules.AllowRead("DateTimeCreated", "InvoiceRemarksReadGroup");

            //AuthorizationRules.AllowWrite("Remarks", "InvoiceRemarksWriteGroup");
            //AuthorizationRules.AllowWrite("DateTimeCreated", "InvoiceRemarksWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in InvoiceRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvoiceRemarksViewGroup"))

            //    return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in InvoiceRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvoiceRemarksAddGroup"))

            //    return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in InvoiceRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvoiceRemarksEditGroup"))

            //    return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in InvoiceRemarks
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvoiceRemarksDeleteGroup"))

            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private InvoiceRemarks()

        { /* require use of factory method */ }

        private InvoiceRemarks(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static InvoiceRemarks NewInvoiceRemarks()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a InvoiceRemarks");
            return DataPortal.Create<InvoiceRemarks>();
        }

        public static InvoiceRemarks GetInvoiceRemarks(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a InvoiceRemarks");
            return DataPortal.Fetch<InvoiceRemarks>(new Criteria(id));
        }

        public static InvoiceRemarks GetInvoiceRemarks(SafeDataReader dr)
        {
            return new InvoiceRemarks(dr);
        }

        public static void DeleteInvoiceRemarks(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a InvoiceRemarks");
            DataPortal.Delete(new Criteria(id));
        }

        public override InvoiceRemarks Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a InvoiceRemarks");

            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a InvoiceRemarks");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a InvoiceRemarks");


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
                cm.CommandText = "GetInvoiceRemarks";


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

            _remarks = dr.GetString("Remarks");
            _dateTimeCreated = dr.GetSmartDate("DateTimeCreated", _dateTimeCreated.EmptyIsMin);
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
                cm.CommandText = "AddInvoiceRemarks";


                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

              //  _id = (Guid)cm.Parameters["@NewId"].Value;
              //  _payGroupId = (Guid)cm.Parameters["@NewPayGroupId"].Value;

            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            
            cm.Parameters.AddWithValue("@Remarks", _remarks);            
            cm.Parameters.AddWithValue("@DateTimeCreated", _dateTimeCreated.DBValue);
			if (_isAdmin != false)
				cm.Parameters.AddWithValue("@isAdmin", _isAdmin);
			else
				cm.Parameters.AddWithValue("@isAdmin", DBNull.Value);
            cm.Parameters.AddWithValue("@Id", _id);
            // cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
            cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
            //   cm.Parameters["@NewPayGroupId"].Direction = ParameterDirection.Output;
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
                cm.CommandText = "UpdateInvoiceRemarks";


                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);           
            cm.Parameters.AddWithValue("@Remarks", _remarks);            
            cm.Parameters.AddWithValue("@DateTimeCreated", _dateTimeCreated.DBValue);
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
                cm.CommandText = "DeleteInvoiceRemarks";



                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}