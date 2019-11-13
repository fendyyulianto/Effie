using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class InvoiceRemarksList : Csla.ReadOnlyListBase<InvoiceRemarksList, InvoiceRemarks>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in InvoiceRemarksList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvoiceRemarksListViewGroup"))

            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private InvoiceRemarksList()

        { /* require use of factory method */ }

        public static InvoiceRemarksList GetInvoiceRemarksList(Guid payGroupId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a InvoiceRemarksList");
            return DataPortal.Fetch<InvoiceRemarksList>(new FilterCriteria(payGroupId));

        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid PayGroupId = Guid.Empty;
            public FilterCriteria(Guid payGroupId)
            {
                this.PayGroupId = payGroupId;
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
                cm.CommandText = "GetInvoiceRemarksList";


                cm.Parameters.AddWithValue("@PayGroupId", criteria.PayGroupId);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(InvoiceRemarks.GetInvoiceRemarks(dr));


                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
