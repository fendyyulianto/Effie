using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class JuryPanelCategoryList : Csla.ReadOnlyListBase<JuryPanelCategoryList, JuryPanelCategory>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in JuryPanelCategoryList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryPanelCategoryListViewGroup"))

            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private JuryPanelCategoryList()

        { /* require use of factory method */ }

        public static JuryPanelCategoryList GetJuryPanelCategoryList(string panelId, string categoryPSDetail)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JuryPanelCategoryList");
            return DataPortal.Fetch<JuryPanelCategoryList>(new FilterCriteria(panelId, categoryPSDetail));

        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public string PanelId = "";
            public string CategoryPSDetail = "";
            public FilterCriteria(string panelId, string categoryPSDetail)
            {
                this.PanelId = panelId;
                this.CategoryPSDetail = categoryPSDetail;
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
                cm.CommandText = "GetJuryPanelCategoryList";

                cm.Parameters.AddWithValue("@PanelId", criteria.PanelId);
                cm.Parameters.AddWithValue("@CategoryPSDetail", criteria.CategoryPSDetail);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(JuryPanelCategory.GetJuryPanelCategory(dr));


                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}