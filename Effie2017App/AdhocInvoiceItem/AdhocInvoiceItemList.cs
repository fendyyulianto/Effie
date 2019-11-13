using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class AdhocInvoiceItemList : Csla.ReadOnlyListBase<AdhocInvoiceItemList, AdhocInvoiceItem>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in AdhocInvoiceItemList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceItemListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private AdhocInvoiceItemList()
        { /* require use of factory method */ }

        public static AdhocInvoiceItemList GetAdhocInvoiceItemList(Guid paygroupId,Guid adhocInvoiceId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a AdhocInvoiceItemList");
            return DataPortal.Fetch<AdhocInvoiceItemList>(new FilterCriteria(paygroupId, adhocInvoiceId));
        }

        public static AdhocInvoiceItemList GetAdhocInvoiceItemList(Guid entryId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a AdhocInvoiceItemList");
            return DataPortal.Fetch<AdhocInvoiceItemList>(new FilterCriteria(entryId));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid PaygroupId = Guid.Empty;
            public Guid AdhocInvoiceId = Guid.Empty;
            public Guid EntryId = Guid.Empty;
            public FilterCriteria(Guid paygroupId,Guid adhocInvoiceId)
            {
                this.PaygroupId = paygroupId;
                this.AdhocInvoiceId = adhocInvoiceId;
            }
            public FilterCriteria(Guid entryId)
            {
                this.EntryId = entryId;
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
                cm.CommandText = "GetAdhocInvoiceItemList";

                cm.Parameters.AddWithValue("@AdhocInvoiceId", criteria.AdhocInvoiceId);
                cm.Parameters.AddWithValue("@PaygroupId", criteria.PaygroupId);
                cm.Parameters.AddWithValue("@EntryId", criteria.EntryId);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(AdhocInvoiceItem.GetAdhocInvoiceItem(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
