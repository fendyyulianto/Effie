using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class EmailTemplateList : Csla.ReadOnlyListBase<EmailTemplateList, EmailTemplate>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in EmailTemplateList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EmailTemplateListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private EmailTemplateList()
        { /* require use of factory method */ }

        public static EmailTemplateList GetEmailTemplateList()
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EmailTemplateList");
            return DataPortal.Fetch<EmailTemplateList>(new FilterCriteria(Guid.Empty));
        }
        public static EmailTemplateList GetEmailTemplateList(Guid templateId)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EmailTemplateList");
            return DataPortal.Fetch<EmailTemplateList>(new FilterCriteria(templateId));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid TemplateId = Guid.Empty;
            public FilterCriteria(Guid templateId)
            {
                this.TemplateId = templateId;
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
                cm.CommandText = "GetEmailTemplateList";

                cm.Parameters.AddWithValue("@TemplateId", criteria.TemplateId);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(EmailTemplate.GetEmailTemplate(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
