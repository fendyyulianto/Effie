using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Linq;

namespace EffieJuryManagementApp
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
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private JuryList()
        { /* require use of factory method */ }

        public static JuryList GetJuryList(string serialNo,string email)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JuryList");
            return DataPortal.Fetch<JuryList>(new FilterCriteria(serialNo, email,string.Empty));
        }

        public static JuryList GetJuryListLogin(string serialNo, string password)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JuryList");
            return DataPortal.Fetch<JuryList>(new FilterCriteria(serialNo,string.Empty, password));
        }

        public static JuryList GetJuryList()
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a JuryList");
            return DataPortal.Fetch<JuryList>(new FilterCriteria(string.Empty, string.Empty,string.Empty));
        }
        #endregion //Factory Methods

        #region Data Access

        #region Filter Criteria
        [Serializable()]
        private class FilterCriteria
        {
            public string SerialNo = string.Empty;
            public string Email = string.Empty;
            public string Password = string.Empty;
            public FilterCriteria(string serialNo, string email, string password)
            {
                this.SerialNo = serialNo;
                this.Email = email;
                this.Password = password;
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
                cm.CommandText = "GetJuryList";

                cm.Parameters.AddWithValue("@SerialNo", criteria.SerialNo);
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

        #region Custom 

        public static bool CheckIfRecordExists(string FirstName, string LastName, string Email,bool isNew,Guid juryId)
        {
            JuryList juryList = JuryList.GetJuryList();

            string firstName = FirstName;
            string lastName = LastName;
            string email = Email;

            var duplicateRecords = juryList.Where(m => (m.FirstName.Trim().ToUpper().Equals(firstName.Trim().ToUpper()) && m.LastName.Trim().ToUpper().Equals(lastName.Trim().ToUpper())) && m.Email.Trim().ToUpper().Equals(email.Trim().ToUpper())).ToList();

            if (isNew)
            {                
                return duplicateRecords.Count > 0;
            }
            else
            {
                Jury jury = null;

                try
                {
                    jury = Jury.GetJury(juryId);
                }
                catch { }


                if (duplicateRecords.Count() > 0 && jury != null)
                {
                    if ((firstName.Trim().ToUpper().Equals(jury.FirstName.Trim().ToUpper()) || lastName.Trim().ToUpper().Equals(jury.LastName.Trim().ToUpper())) && email.Trim().ToUpper().Equals(jury.Email.Trim().ToUpper()))
                    {
                        return duplicateRecords.Count > 1;
                    }
                }
                
                return false;
            }
        }

        public static string GetNewJuryId()
        {
            string prefix = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("JurySerialNoPrefix")[0].Value;
            string juryId = "";
            Gen_GeneralUseValueList gen_GeneralUseValueList = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("JurySerialNo");
            Gen_GeneralUseValue gen_GeneralUseValue = Gen_GeneralUseValue.GetGen_GeneralUseValue(gen_GeneralUseValueList[0].Id);

            if (gen_GeneralUseValue != null)
            {
                juryId = gen_GeneralUseValue.Value;
                juryId = prefix + juryId;

                gen_GeneralUseValue.Value = (Convert.ToInt32(gen_GeneralUseValue.Value) + 1).ToString();
                gen_GeneralUseValue.DateModifiedString = DateTime.Now.ToString();
                gen_GeneralUseValue.Save();
            }
            return juryId;
        }

        #endregion
    }
}
