using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class JuryRemarksList : Csla.ReadOnlyListBase<JuryRemarksList, JuryRemarks>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in JuryRemarksList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryRemarksListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private JuryRemarksList()
        { /* require use of factory method */ }

        public static JuryRemarksList GetJuryRemarksList(Guid juryId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JuryRemarksList");
            return DataPortal.Fetch<JuryRemarksList>(new FilterCriteria(juryId));
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
                cm.CommandText = "GetJuryRemarksList";

                cm.Parameters.AddWithValue("@JuryId", criteria.JuryId);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(JuryRemarks.GetJuryRemarks(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
