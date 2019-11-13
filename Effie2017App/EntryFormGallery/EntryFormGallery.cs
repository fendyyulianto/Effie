using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class EntryFormGallery : Csla.BusinessBase<EntryFormGallery>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _imageA = string.Empty;
        private string _imageB = string.Empty;
        private string _imageC = string.Empty;
        private string _imageD = string.Empty;
        private string _imageE = string.Empty;
        private string _imageF = string.Empty;
        private string _imageG = string.Empty;
        private string _imagesPath = string.Empty;
        
        private string _type = string.Empty;
        private Guid _entryId = Guid.NewGuid();

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public string ImagesPath
        {
            get
            {
                CanReadProperty("ImagesPath", true);
                return _imagesPath;
            }
            set
            {
                CanWriteProperty("ImagesPath", true);
                if (value == null) value = string.Empty;
                if (!_imagesPath.Equals(value))
                {
                    _imagesPath = value;
                    PropertyHasChanged("ImagesPath");
                }
            }
        }
        
        public string ImageA
        {
            get
            {
                CanReadProperty("ImageA", true);
                return _imageA;
            }
            set
            {
                CanWriteProperty("ImageA", true);
                if (value == null) value = string.Empty;
                if (!_imageA.Equals(value))
                {
                    _imageA = value;
                    PropertyHasChanged("ImageA");
                }
            }
        }

        public string ImageB
        {
            get
            {
                CanReadProperty("ImageB", true);
                return _imageB;
            }
            set
            {
                CanWriteProperty("ImageB", true);
                if (value == null) value = string.Empty;
                if (!_imageB.Equals(value))
                {
                    _imageB = value;
                    PropertyHasChanged("ImageB");
                }
            }
        }

        public string ImageC
        {
            get
            {
                CanReadProperty("ImageC", true);
                return _imageC;
            }
            set
            {
                CanWriteProperty("ImageC", true);
                if (value == null) value = string.Empty;
                if (!_imageC.Equals(value))
                {
                    _imageC = value;
                    PropertyHasChanged("ImageC");
                }
            }
        }

        public string ImageD
        {
            get
            {
                CanReadProperty("ImageD", true);
                return _imageD;
            }
            set
            {
                CanWriteProperty("ImageD", true);
                if (value == null) value = string.Empty;
                if (!_imageD.Equals(value))
                {
                    _imageD = value;
                    PropertyHasChanged("ImageD");
                }
            }
        }

        public string ImageE
        {
            get
            {
                CanReadProperty("ImageE", true);
                return _imageE;
            }
            set
            {
                CanWriteProperty("ImageE", true);
                if (value == null) value = string.Empty;
                if (!_imageE.Equals(value))
                {
                    _imageE = value;
                    PropertyHasChanged("ImageE");
                }
            }
        }
        
        public string ImageF
        {
            get
            {
                CanReadProperty("ImageF", true);
                return _imageF;
            }
            set
            {
                CanWriteProperty("ImageF", true);
                if (value == null) value = string.Empty;
                if (!_imageF.Equals(value))
                {
                    _imageF = value;
                    PropertyHasChanged("ImageF");
                }
            }
        }


        public string ImageG
        {
            get
            {
                CanReadProperty("ImageG", true);
                return _imageG;
            }
            set
            {
                CanWriteProperty("ImageG", true);
                if (value == null) value = string.Empty;
                if (!_imageG.Equals(value))
                {
                    _imageG = value;
                    PropertyHasChanged("ImageG");
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

        public Guid EntryId
        {
            get
            {
                CanReadProperty("EntryId", true);
                return _entryId;
            }
            set
            {
                CanWriteProperty("IdEntry", true);
                if (!_entryId.Equals(value))
                {
                    _entryId = value;
                    PropertyHasChanged("IdEntry");
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
            // ImageA
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ImageA", 500));
            //
            // ImageB
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ImageB", 500));
            //
            // ImageC
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ImageC", 500));
            //
            // ImageD
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ImageD", 500));
            //
            // ImageE
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ImageE", 500));
            //
            // ImageF
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ImageF", 500));
            //
            // ImageG
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ImageG", 500));
            //
            // Type
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Type", 500));
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
            //TODO: Define authorization rules in EntryFormGallery
            //AuthorizationRules.AllowRead("Id", "EntryFormGalleryReadGroup");
            //AuthorizationRules.AllowRead("ImageA", "EntryFormGalleryReadGroup");
            //AuthorizationRules.AllowRead("ImageB", "EntryFormGalleryReadGroup");
            //AuthorizationRules.AllowRead("ImageC", "EntryFormGalleryReadGroup");
            //AuthorizationRules.AllowRead("Type", "EntryFormGalleryReadGroup");
            //AuthorizationRules.AllowRead("EntryId", "EntryFormGalleryReadGroup");

            //AuthorizationRules.AllowWrite("ImageA", "EntryFormGalleryWriteGroup");
            //AuthorizationRules.AllowWrite("ImageB", "EntryFormGalleryWriteGroup");
            //AuthorizationRules.AllowWrite("ImageC", "EntryFormGalleryWriteGroup");
            //AuthorizationRules.AllowWrite("Type", "EntryFormGalleryWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in EntryFormGallery
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryFormGalleryViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in EntryFormGallery
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryFormGalleryAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in EntryFormGallery
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryFormGalleryEditGroup"))
            //	return true;
            //return false;
        }

        private EntryFormGallery(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static EntryFormGallery GetEntryFormGallery(SafeDataReader dr)
        {
            return new EntryFormGallery(dr);
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in EntryFormGallery
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryFormGalleryDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private EntryFormGallery()
        { /* require use of factory method */ }

        public static EntryFormGallery NewEntryFormGallery()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EntryFormGallery");
            return DataPortal.Create<EntryFormGallery>();
        }

        public static EntryFormGallery GetEntryFormGallery(Guid id, Guid entryid, string type)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EntryFormGallery");
            return DataPortal.Fetch<EntryFormGallery>(new Criteria(id, entryid, type));
        }

        public static void DeleteEntryFormGallery(Guid id, Guid entryid, string type)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EntryFormGallery");
            DataPortal.Delete(new Criteria(id, entryid, type));
        }

        public override EntryFormGallery Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EntryFormGallery");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EntryFormGallery");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a EntryFormGallery");

            return base.Save();
        }

        #endregion //Factory Methods

        #region Data Access

        #region Criteria

        [Serializable()]
        private class Criteria
        {
            public Guid Id;
            public Guid IdEntry;
            public string Type;
            
            public Criteria(Guid id, Guid entryid, string type)
            {
                this.Id = id;
                this.IdEntry = entryid;
                this.Type = type;
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
                cm.CommandText = "GetEntryFormGallery";
                
                cm.Parameters.AddWithValue("@Id", criteria.Id);
                cm.Parameters.AddWithValue("@IdEntry", criteria.IdEntry);
                cm.Parameters.AddWithValue("@Type", criteria.Type);
                
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
            _imageA = dr.GetString("ImageA");
            _imageB = dr.GetString("ImageB");
            _imageC = dr.GetString("ImageC");
            _imageD = dr.GetString("ImageD");
            _imageE = dr.GetString("ImageE");
            _imageF = dr.GetString("ImageF");
            _imageG = dr.GetString("ImageG");
            _imagesPath = dr.GetString("ImagesPath");
            _type = dr.GetString("Type");
            _entryId = dr.GetGuid("EntryId");
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
                cm.CommandText = "AddEntryFormGallery";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();
                
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {

            if (_imagesPath != string.Empty)
                cm.Parameters.AddWithValue("@ImagesPath", _imagesPath);
            else
                cm.Parameters.AddWithValue("@ImagesPath", DBNull.Value);

            if (_imageA != string.Empty)
                cm.Parameters.AddWithValue("@ImageA", _imageA);
            else
                cm.Parameters.AddWithValue("@ImageA", DBNull.Value);
            if (_imageB != string.Empty)
                cm.Parameters.AddWithValue("@ImageB", _imageB);
            else
                cm.Parameters.AddWithValue("@ImageB", DBNull.Value);
            if (_imageC != string.Empty)
                cm.Parameters.AddWithValue("@ImageC", _imageC);
            else
                cm.Parameters.AddWithValue("@ImageC", DBNull.Value);




            if (_imageD != string.Empty)
                cm.Parameters.AddWithValue("@ImageD", _imageD);
            else
                cm.Parameters.AddWithValue("@ImageD", DBNull.Value);
            if (_imageE != string.Empty)
                cm.Parameters.AddWithValue("@ImageE", _imageE);
            else
                cm.Parameters.AddWithValue("@ImageE", DBNull.Value);
            if (_imageF != string.Empty)
                cm.Parameters.AddWithValue("@ImageF", _imageF);
            else
                cm.Parameters.AddWithValue("@ImageF", DBNull.Value);
            if (_imageG != string.Empty)
                cm.Parameters.AddWithValue("@ImageG", _imageG);
            else
                cm.Parameters.AddWithValue("@ImageG", DBNull.Value);



            if (_type != string.Empty)
                cm.Parameters.AddWithValue("@Type", _type);
            else
                cm.Parameters.AddWithValue("@Type", DBNull.Value);


            cm.Parameters.AddWithValue("@Id", _id);
            if (_entryId != Guid.Empty)
                cm.Parameters.AddWithValue("@EntryId", _entryId);
            else
                cm.Parameters.AddWithValue("@EntryId", DBNull.Value);
            
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
                cm.CommandText = "UpdateEntryFormGallery";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);

            if (_imagesPath != string.Empty)
                cm.Parameters.AddWithValue("@ImagesPath", _imagesPath);
            else
                cm.Parameters.AddWithValue("@ImagesPath", DBNull.Value);


            if (_imageA != string.Empty)
                cm.Parameters.AddWithValue("@ImageA", _imageA);
            else
                cm.Parameters.AddWithValue("@ImageA", DBNull.Value);
            if (_imageB != string.Empty)
                cm.Parameters.AddWithValue("@ImageB", _imageB);
            else
                cm.Parameters.AddWithValue("@ImageB", DBNull.Value);
            if (_imageC != string.Empty)
                cm.Parameters.AddWithValue("@ImageC", _imageC);
            else
                cm.Parameters.AddWithValue("@ImageC", DBNull.Value);
            
            if (_imageD != string.Empty)
                cm.Parameters.AddWithValue("@ImageD", _imageD);
            else
                cm.Parameters.AddWithValue("@ImageD", DBNull.Value);
            if (_imageE != string.Empty)
                cm.Parameters.AddWithValue("@ImageE", _imageE);
            else
                cm.Parameters.AddWithValue("@ImageE", DBNull.Value);
            if (_imageF != string.Empty)
                cm.Parameters.AddWithValue("@ImageF", _imageF);
            else
                cm.Parameters.AddWithValue("@ImageF", DBNull.Value);
            if (_imageG != string.Empty)
                cm.Parameters.AddWithValue("@ImageG", _imageG);
            else
                cm.Parameters.AddWithValue("@ImageG", DBNull.Value);


            if (_type != string.Empty)
                cm.Parameters.AddWithValue("@Type", _type);
            else
                cm.Parameters.AddWithValue("@Type", DBNull.Value);
            cm.Parameters.AddWithValue("@EntryId", _entryId);
        }

        private void UpdateChildren(SqlConnection cn)
        {
        }
        #endregion //Data Access - Update

        #region Data Access - Delete
        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(new Criteria(_id, _entryId, _type));
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
                cm.CommandText = "DeleteEntryFormGallery";
                
                cm.Parameters.AddWithValue("@Id", criteria.Id);
                cm.Parameters.AddWithValue("@IdEntry", criteria.IdEntry);
                cm.Parameters.AddWithValue("@Type", criteria.Type);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
