using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class RegistrationRemarksList : Csla.ReadOnlyListBase<RegistrationRemarksList, RegistrationRemarks>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in RegistrationRemarksList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationRemarksListViewGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private RegistrationRemarksList()
        { /* require use of factory method */ }

        public static RegistrationRemarksList GetRegistrationRemarksList(Guid registrationId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a RegistrationRemarksList");
            return DataPortal.Fetch<RegistrationRemarksList>(new FilterCriteria(registrationId));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid RegistrationId = Guid.Empty;
            public FilterCriteria(Guid registrationId)
            {
                this.RegistrationId = registrationId;
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
                cm.CommandText = "GetRegistrationRemarksList";

                cm.Parameters.AddWithValue("@RegistrationId", criteria.RegistrationId);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(RegistrationRemarks.GetRegistrationRemarks(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}