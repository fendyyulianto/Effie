using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class SettingList : Csla.ReadOnlyListBase<SettingList, Setting>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in SettingList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("SettingListViewGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private SettingList()
        { /* require use of factory method */ }

        public static SettingList GetSettingList(string categoryCode)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a SettingList");
            return DataPortal.Fetch<SettingList>(new FilterCriteria(categoryCode));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public string CategoryCode = "";
            public FilterCriteria(string categoryCode)
            {
                this.CategoryCode = categoryCode;
            }
        }
        #endregion //Filter Criteria

        #region Data Access - Fetch
        private void DataPortal_Fetch(FilterCriteria criteria)
        {
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
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
                cm.CommandText = "GetSettingList";

                cm.Parameters.AddWithValue("@CategoryCode", criteria.CategoryCode);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(Setting.GetSetting(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}