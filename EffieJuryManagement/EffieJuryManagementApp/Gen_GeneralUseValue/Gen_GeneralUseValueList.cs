using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class Gen_GeneralUseValueList : Csla.ReadOnlyListBase<Gen_GeneralUseValueList, Gen_GeneralUseValue>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Gen_GeneralUseValueList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("Gen_GeneralUseValueListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Gen_GeneralUseValueList()
        { /* require use of factory method */ }

        public static Gen_GeneralUseValueList GetGen_GeneralUseValueList()
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Gen_GeneralUseValueList");
            return DataPortal.Fetch<Gen_GeneralUseValueList>(new FilterCriteria());
        }

        public static Gen_GeneralUseValueList GetGen_GeneralUseValueList(string code)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Gen_GeneralUseValueList");
            return DataPortal.Fetch<Gen_GeneralUseValueList>(new FilterCriteria(code));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public string Code = "";

            public FilterCriteria()
            {

            }

            public FilterCriteria(string code)
            {
                this.Code = code;
            }
        }
        #endregion //Filter Criteria

        #region Data Access - Fetch
        private void DataPortal_Fetch(FilterCriteria criteria)
        {
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
            {
                cn.Open();

                ExecuteFetch(cn, criteria);
            }//using

            IsReadOnly = true;
            RaiseListChangedEvents = true;
        }

        private void ExecuteFetch(SqlConnection cn, FilterCriteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "GetGen_GeneralUseValueList";

                cm.Parameters.AddWithValue("@Code", criteria.Code);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(Gen_GeneralUseValue.GetGen_GeneralUseValue(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
