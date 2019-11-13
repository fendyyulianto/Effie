using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{ 
	[Serializable()] 
	public class LoginHistoryList : Csla.ReadOnlyListBase<LoginHistoryList, LoginHistory>
	{

		#region Authorization Rules

		public static bool CanGetObject()
		{
			//TODO: Define CanGetObject permission in LoginHistoryList
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("LoginHistoryListViewGroup"))
			//	return true;
			//return false;
		}
		#endregion //Authorization Rules

		#region Factory Methods
		private LoginHistoryList()
		{ /* require use of factory method */ }

		public static LoginHistoryList GetLoginHistoryList()
		{
			if (!CanGetObject())
				throw new System.Security.SecurityException("User not authorized to view a LoginHistoryList");
			return DataPortal.Fetch<LoginHistoryList>(new FilterCriteria());
		}
		#endregion //Factory Methods

		#region Data Access

		#region Filter Criteria
		[Serializable()]
		private class FilterCriteria
		{

			public FilterCriteria()
			{

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
				cm.CommandText = "GetLoginHistoryList";


				using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
				{
					while (dr.Read())
						this.Add(LoginHistory.GetLoginHistory(dr));
				}
			}//using
		}
		#endregion //Data Access - Fetch
		#endregion //Data Access
	}
}
