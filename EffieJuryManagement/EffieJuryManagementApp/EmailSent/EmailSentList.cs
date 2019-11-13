using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class EmailSentList : Csla.ReadOnlyListBase<EmailSentList, EmailSent>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in EmailSentList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailSentListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private EmailSentList()
        { /* require use of factory method */ }

        public static EmailSentList GetEmailSentList()
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EmailSentList");
            return DataPortal.Fetch<EmailSentList>(new FilterCriteria(Guid.Empty,Guid.Empty));
        }
        public static EmailSentList GetEmailSentList(Guid templateId,Guid juryId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EmailSentList");
            return DataPortal.Fetch<EmailSentList>(new FilterCriteria(templateId, juryId));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid TemplateId = Guid.Empty;
            public Guid JuryId = Guid.Empty;

            public FilterCriteria(Guid templateId,Guid juryId)
            {
                this.TemplateId = templateId;
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
                cm.CommandText = "GetEmailSentList";

                cm.Parameters.AddWithValue("@TemplateId", criteria.TemplateId);
                cm.Parameters.AddWithValue("@JuryId", criteria.JuryId);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(EmailSent.GetEmailSent(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
