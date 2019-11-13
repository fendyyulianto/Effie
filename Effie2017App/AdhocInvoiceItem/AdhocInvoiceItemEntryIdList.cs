using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class AdhocInvoiceItemEntryIdList : Csla.ReadOnlyListBase<AdhocInvoiceItemEntryIdList, AdhocInvoiceItem>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in AdhocInvoiceItemEntryIdList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("AdhocInvoiceItemEntryIdListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private AdhocInvoiceItemEntryIdList()
        { /* require use of factory method */ }

        public static AdhocInvoiceItemEntryIdList GetAdhocInvoiceItemEntryIdList(Guid entryId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a AdhocInvoiceItemEntryIdList");
            return DataPortal.Fetch<AdhocInvoiceItemEntryIdList>(new FilterCriteria(entryId));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid EntryId = Guid.Empty;
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
                cm.CommandText = "GetAdhocInvoiceItemEntryIdList";

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
