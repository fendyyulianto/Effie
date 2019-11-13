using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class AmountReceivedList : Csla.ReadOnlyListBase<AmountReceivedList, AmountReceived>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in AmountReceivedList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AmountReceivedListViewGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private AmountReceivedList()
        { /* require use of factory method */ }

        public static AmountReceivedList GetAmountReceivedList(Guid paygroupId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a AmountReceivedList");
            return DataPortal.Fetch<AmountReceivedList>(new FilterCriteria(paygroupId));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid PaygroupId = Guid.Empty;
            public FilterCriteria(Guid paygroupId)
            {
                this.PaygroupId = paygroupId;
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
                cm.CommandText = "GetAmountReceivedList";

                cm.Parameters.AddWithValue("@PaygroupId", criteria.PaygroupId);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(AmountReceived.GetAmountReceived(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}