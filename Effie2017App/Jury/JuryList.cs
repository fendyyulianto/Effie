using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{
    [Serializable()]
    public class JuryList : Csla.ReadOnlyListBase<JuryList, Jury>
    {

        #region Authorization Rules

        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in JuryList
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryListViewGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private JuryList()
        { /* require use of factory method */ }

        public static JuryList GetJuryList(string email, string password)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JuryList");
            return DataPortal.Fetch<JuryList>(new FilterCriteria(email, password));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public string Email = "";
            public string Password = "";
            public FilterCriteria(string Password, string password)
            {
                this.Email = Password;
                this.Password = password;
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
                cm.CommandText = "GetJuryList";
                cm.Parameters.AddWithValue("@Email", criteria.Email);
                cm.Parameters.AddWithValue("@Password", criteria.Password);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    while (dr.Read())
                        this.Add(Jury.GetJury(dr));
                }
            }//using
        }
        #endregion //Data Access - Fetch
        #endregion //Data Access
    }
}