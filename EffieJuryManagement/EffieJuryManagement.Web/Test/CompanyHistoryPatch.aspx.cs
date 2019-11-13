using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;
using System.Data;
using System.Configuration;
using System.IO;
using Telerik.Web.UI;

public partial class Test_CompanyHistoryPatch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        JuryList JuryLists = JuryList.GetJuryList("", "");
        foreach (Jury jury in JuryLists)
        {
            List<CompanyHistory> companies = CompanyHistoryList.GetCompanyHistoryList(jury.Id).OrderByDescending(x => x.DateCreated).ToList();
            if (companies.Count > 0)
            { //EDIT HERE

                int count = 1;
                CompanyHistory CurrCompanyHistory = null;
                //EDIT DATE CREATED
                foreach (CompanyHistory com in companies)
                {
                    if (count == 1)
                    {
                        //NEW COMPANY HISTORY
                        CompanyHistory compnyHistroy = CompanyHistory.NewCompanyHistory();
                        SaveCompanyHistory(jury, compnyHistroy, com.DateCreatedString);
                    }
                    else
                    {
                        SaveCompanyHistory(jury, CurrCompanyHistory, com.DateCreatedString);
                    }

                    if (count != companies.Count || companies.Count == 1)
                        CurrCompanyHistory = com;

                    count++;
                }

                //FOR LATEST COMPANY HISTORY
                SaveCompanyHistory(jury, CurrCompanyHistory, jury.DateCreatedString);

            }
            else
            {   //NEW COMPANY HISTORY
                CompanyHistory compnyHistroy = CompanyHistory.NewCompanyHistory();
                SaveCompanyHistory(jury, compnyHistroy, "Empty");
            }
        }
    }


    public void SaveCompanyHistory(Jury jury, CompanyHistory compnyHistroy, string DateCreated = "")
    {
        if (!string.IsNullOrEmpty(DateCreated) && !compnyHistroy.IsNew)
        {
            compnyHistroy.DateCreatedString = DateCreated;
        }
        else
        {
            compnyHistroy.JuryId = jury.Id;
            compnyHistroy.Type = jury.Type;
            compnyHistroy.Designation = jury.Designation;
            compnyHistroy.Company = jury.Company;
            compnyHistroy.Address1 = jury.Address1;
            compnyHistroy.Address2 = jury.Address2;
            compnyHistroy.City = jury.City;
            compnyHistroy.Postal = jury.Postal;
            compnyHistroy.Country = jury.Country;

            if (DateCreated == "Empty")
                compnyHistroy.DateCreatedString = jury.DateCreatedString;
            else if (!string.IsNullOrEmpty(DateCreated))
                compnyHistroy.DateCreatedString = DateCreated;
            
            compnyHistroy.CompanyType = jury.CompanyType;
            compnyHistroy.CompanyTypeOther = jury.CompanyTypeOther;

            compnyHistroy.Network = jury.Network;
            if (compnyHistroy.Network == "Others") compnyHistroy.NetworkOthers = jury.NetworkOthers.Trim();

            compnyHistroy.HoldingCompany = jury.HoldingCompany;
            if (compnyHistroy.HoldingCompany == "Others") compnyHistroy.HoldingCompanyOthers = jury.HoldingCompanyOthers.Trim();
            
        }

        compnyHistroy.Save();

        string msg = "JURY ID : " + jury.Id + " || CompnyHistroy ID : " + compnyHistroy.Id + " || IsNew : " + compnyHistroy.IsNew + " || DateCreated : " + DateCreated + "<br>";
        Response.Write(msg);
    }
}