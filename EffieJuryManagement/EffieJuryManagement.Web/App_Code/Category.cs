using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Category
/// </summary>
public class Category
{
	public Category()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    static string[,] _categories = new string[,]
    {
        // ColumnId, Prefix, Name
        {"01", "SP-AU", "Automotive"},
        {"01", "SP-BW", "Beauty & Wellness"},
        {"01", "SP-BA", "Beverages – Alcohol"},
        {"01", "SP-BN", "Beverages Non-Alcohol"},
        {"01", "SP-CE", "Consumer Electronics and Durables"},
        {"01", "SP-CR", "Corporate Reputation/Professional Services"},
        {"01", "SP-RS", "Restaurants"},
        {"01", "SP-FP", "Financial Products & Services"},
        {"01", "SP-FD", "Food"},
        {"01", "SP-GV", "Government / Institutional"},
        {"01", "SP-HC", "Healthcare"},
        {"01", "SP-HS", "Household/Home Products & Services "},
        {"01", "SP-IT", "IT /Telco"},
        {"01", "SP-ME", "Media, Entertainment & Leisure "},
        {"01", "SP-RE", "Real Estate"},
        {"01", "SP-RT", "Retail "},
        {"01", "SP-TT", "Travel / Tourism "},
        {"01", "SP-OT", "Other Products & Services"},


        {"02", "SS-AB", "Asia Pacific Brands"},
        {"02", "SS-BE", "Brand Experience"},
        {"02", "SS-BR", "Brand Revitalisation"},
        //{"02", "SS-BU", "Branded Utility"},
        //{"02", "SS-BB", "Business-to-Business"},
        {"02", "SS-DG", "David vs Goliath"},
        {"02", "SS-GB", "GoodWorks - Brand"},
        {"02", "SS-GN", "GoodWorks – Non Profit"},
        {"02", "SS-MV", "Media Innovation"},
        {"02", "SS-NP", "New Product or Service"},
        //{"02", "SS-RM", "Real Time Marketing"},
        //{"02", "SS-SM", "Shopper Marketing"},
        //{"02", "SS-SP", "Small Budget-Products"},
        //{"02", "SS-SV", "Small Budget-Services"},
        //{"02", "SS-SE", "Sponsorship & Event Marketing"},
        {"02", "SS-SS", "Sustained Success"},
        //{"02", "SS-YM", "Youth Marketing"},


        {"03", "MP-PP", "Products"},
        {"03", "MP-SV", "Services"},


        {"04", "MS-BE", "Brand Experience"},

    };

    public static DataTable GetSubcategories(string columnId)
    {
        DataTable tblData = new DataTable();
        tblData.Columns.Add(new DataColumn("ColumnId"));
        tblData.Columns.Add(new DataColumn("Prefix"));
        tblData.Columns.Add(new DataColumn("Name"));

        for (int x = 0; x < _categories.Length / 3; x++)
        {
            if (_categories[x, 0] == columnId || columnId.Trim() == "")
            {
                DataRow dr = tblData.NewRow();

                dr[0] = _categories[x, 0];
                dr[1] = _categories[x, 1];
                dr[2] = _categories[x, 2];

                tblData.Rows.Add(dr);
            }
        }

        return tblData;
    }

    public static string GetCategoryCode(string market, string categoryname)
    {
        DataTable dt = GetSubcategories("");

        string marketcode = "";
        if (market.Trim() == "SP") marketcode = "01";
        if (market.Trim() == "SS") marketcode = "02";
        if (market.Trim() == "MP") marketcode = "03";
        if (market.Trim() == "MS") marketcode = "04";



        foreach (DataRow row in dt.Rows)
        {
            if (row["ColumnId"].ToString() == marketcode && row["Name"].ToString() == categoryname)
            {
                return row["Prefix"].ToString();
            }
        }
        return "";
    }
}