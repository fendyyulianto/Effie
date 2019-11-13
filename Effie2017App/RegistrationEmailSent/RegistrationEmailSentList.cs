using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class RegistrationEmailSentList : Csla.ReadOnlyListBase<RegistrationEmailSentList, RegistrationEmailSent>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in RegistrationEmailSentList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationEmailSentListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private RegistrationEmailSentList()
        { /* require use of factory method */ }

        public static RegistrationEmailSentList GetRegistrationEmailSentList()
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a RegistrationEmailSentList");
            return DataPortal.Fetch<RegistrationEmailSentList>(new FilterCriteria(Guid.Empty,Guid.Empty));
        }
        public static RegistrationEmailSentList GetRegistrationEmailSentList(Guid templateId,Guid RegistrationId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a RegistrationEmailSentList");
            return DataPortal.Fetch<RegistrationEmailSentList>(new FilterCriteria(templateId, RegistrationId));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid TemplateId = Guid.Empty;
            public Guid RegistrationId = Guid.Empty;

            public FilterCriteria(Guid templateId,Guid RegistrationId)
            {
                this.TemplateId = templateId;
                this.RegistrationId = RegistrationId;
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
                cm.CommandText = "GetRegistrationEmailSentList";

                cm.Parameters.AddWithValue("@TemplateId", criteria.TemplateId);
                cm.Parameters.AddWithValue("@RegistrationId", criteria.RegistrationId);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(RegistrationEmailSent.GetRegistrationEmailSent(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
