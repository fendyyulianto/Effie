using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;

namespace Effie2017.App
{ 
	[Serializable()] 
	public class RawLogList : Csla.ReadOnlyListBase<RawLogList, RawLog>
	{

		#region Authorization Rules

		public static bool CanGetObject()
		{
			//TODO: Define CanGetObject permission in RawLogList
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("RawLogListViewGroup"))
			//	return true;
			//return false;
		}
		#endregion //Authorization Rules

		#region Factory Methods
		private RawLogList()
		{ /* require use of factory method */ }

		public static RawLogList GetRawLogList()
		{
			if (!CanGetObject())
				throw new System.Security.SecurityException("User not authorized to view a RawLogList");
			return DataPortal.Fetch<RawLogList>(new FilterCriteria());
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
				cm.CommandText = "GetRawLogList";


				using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
				{
					while (dr.Read())
						this.Add(RawLog.GetRawLog(dr));
				}
			}//using
		}
		#endregion //Data Access - Fetch
		#endregion //Data Access
	}
}
