using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class CompanyHistoryList : Csla.ReadOnlyListBase<CompanyHistoryList, CompanyHistory>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in CompanyHistoryList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("CompanyHistoryListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private CompanyHistoryList()
        { /* require use of factory method */ }

        public static CompanyHistoryList GetCompanyHistoryList(Guid juryId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a CompanyHistoryList");
            return DataPortal.Fetch<CompanyHistoryList>(new FilterCriteria(juryId));
        }
        
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid JuryId = Guid.Empty;

            public FilterCriteria(Guid juryId)
            {
                this.JuryId = juryId;
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
                cm.CommandText = "GetCompanyHistoryList";

                cm.Parameters.AddWithValue("@JuryId", criteria.JuryId);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(CompanyHistory.GetCompanyHistory(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
