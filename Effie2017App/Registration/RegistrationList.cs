using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class RegistrationList : Csla.ReadOnlyListBase<RegistrationList, Registration>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in RegistrationList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationListViewGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private RegistrationList()
        { /* require use of factory method */ }

        public static RegistrationList GetRegistrationList(string email, string password, string  status)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a RegistrationList");
            return DataPortal.Fetch<RegistrationList>(new FilterCriteria(email, password, status));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public string Email = "";
            public string Password = "";

            public string Status = "" ;

            public FilterCriteria(string email, string password, string status)
            {
                this.Email = email;
                this.Password = password;
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
                cm.CommandText = "GetRegistrationList";
                cm.Parameters.AddWithValue("@Email", criteria.Email);
                cm.Parameters.AddWithValue("@Password", criteria.Password);
                cm.Parameters.AddWithValue("@Status", criteria.Status);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(Registration.GetRegistration(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}
