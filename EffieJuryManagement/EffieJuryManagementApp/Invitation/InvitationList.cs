using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class InvitationList : Csla.ReadOnlyListBase<InvitationList, Invitation>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in InvitationList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("InvitationListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private InvitationList()
        { /* require use of factory method */ }

        public static InvitationList GetInvitationList(Guid juryId,string EventCode)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a InvitationList");
            return DataPortal.Fetch<InvitationList>(new FilterCriteria(juryId, EventCode));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {            
            public Guid JuryId = Guid.Empty;
            public string EventCode = string.Empty;

            public FilterCriteria(Guid juryId,string eventCode)
            {
                this.JuryId = juryId;
                this.EventCode = eventCode;
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
                cm.CommandText = "GetInvitationList";

                cm.Parameters.AddWithValue("@JuryId", criteria.JuryId);
                cm.Parameters.AddWithValue("@EventCode", criteria.EventCode);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(Invitation.GetInvitation(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
