using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class EntryList : Csla.ReadOnlyListBase<EntryList, Entry>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in EntryList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private EntryList()
        { /* require use of factory method */ }

        public static EntryList GetEntryList(Guid payGroupId, Guid registrationId, string serial)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EntryList");
            return DataPortal.Fetch<EntryList>(new FilterCriteria(payGroupId, registrationId, serial));
        }

        public static EntryList GetEntryList(Guid payGroupId, Guid registrationId, string serial, string status)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EntryList");
            return DataPortal.Fetch<EntryList>(new FilterCriteria(payGroupId, registrationId, serial, status));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public Guid PayGroupId = Guid.Empty;
            public Guid RegistrationId = Guid.Empty;
            public string Serial = "";
            public string Status = "";

            public FilterCriteria(Guid payGroupId, Guid registrationId, string serial)
            {
                this.PayGroupId = payGroupId;
                this.RegistrationId = registrationId;
                this.Serial = serial;
            }

            public FilterCriteria(Guid payGroupId, Guid registrationId, string serial, string status)
            {
                this.PayGroupId = payGroupId;
                this.RegistrationId = registrationId;
                this.Serial = serial;
                this.Status = status;
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
                cm.CommandText = "GetEntryList";
                cm.Parameters.AddWithValue("@PayGroupId", criteria.PayGroupId);
                cm.Parameters.AddWithValue("@RegistrationId", criteria.RegistrationId);
                cm.Parameters.AddWithValue("@Serial", criteria.Serial);
                cm.Parameters.AddWithValue("@Status", criteria.Status);
                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(Entry.GetEntry(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
