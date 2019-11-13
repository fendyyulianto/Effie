using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class AdhocInvoiceAmountReceivedList : Csla.ReadOnlyListBase<AdhocInvoiceAmountReceivedList, AdhocInvoiceAmountReceived>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in AdhocInvoiceAmountReceivedList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceAmountReceivedListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private AdhocInvoiceAmountReceivedList()
        { /* require use of factory method */ }

        public static AdhocInvoiceAmountReceivedList GetAdhocInvoiceAmountReceivedList(Guid paygroupId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a AdhocInvoiceAmountReceivedList");
            return DataPortal.Fetch<AdhocInvoiceAmountReceivedList>(new FilterCriteria(paygroupId));
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
                cm.CommandText = "GetAdhocInvoiceAmountReceivedList";

                cm.Parameters.AddWithValue("@PaygroupId", criteria.PaygroupId);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(AdhocInvoiceAmountReceived.GetAdhocInvoiceAmountReceived(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
