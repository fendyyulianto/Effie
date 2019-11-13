using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class ScoreList : Csla.ReadOnlyListBase<ScoreList, Score>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in ScoreList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("ScoreListViewGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private ScoreList()
        { /* require use of factory method */ }

        public static ScoreList GetScoreList(Guid entryId, Guid juryid)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a ScoreList");
            return DataPortal.Fetch<ScoreList>(new FilterCriteria(entryId, juryid));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid EntryId = Guid.Empty;
            public Guid Juryid = Guid.Empty;
            public FilterCriteria(Guid entryId, Guid juryid)
            {
                this.EntryId = entryId;
                this.Juryid = juryid;
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
                cm.CommandText = "GetScoreList";

                cm.Parameters.AddWithValue("@EntryId", criteria.EntryId);
                cm.Parameters.AddWithValue("@Juryid", criteria.Juryid);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(Score.GetScore(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}