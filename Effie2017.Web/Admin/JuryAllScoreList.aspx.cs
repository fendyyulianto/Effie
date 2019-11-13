using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Effie2017.App;
using System.Data;
using System.Collections;
using ClosedXML.Excel;
using System.IO;

public partial class Admin_JuryAllScoreList : PageSecurity_Admin
{
    string round;
    Hashtable ht;
    Hashtable ht2;
    public static int PageSizeDefault = 50;

    SortedList<Guid, int> lstRank = new SortedList<Guid, int>();
    SortedList<Guid, int> lstRankHL = new SortedList<Guid, int>();

    protected void Page_Load(object sender, EventArgs e)
    {
        round = Request.QueryString["r"];
        ht = new Hashtable();
        ht2 = new Hashtable();

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    private void PopulateForm()
    {
        // Refresh the cache
        //GeneralFunction.ResetReportDataCache();
        GeneralFunction.GetAllEntryCache(true);
        GeneralFunction.GetAllJuryCache(true, round);
        GeneralFunction.GetAllScoreCache(true);
        GeneralFunction.GetAllJuryPanelCategoryCache(true);

        ViewState["TabFilterValue"] = "";
        // preload saved filters
        if (GeneralFunction.GetFilterPageId() == "JuryAllScoreList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            ddlPanel.SelectedValue = GeneralFunction.GetFilterF3();
            ddlCategory.SelectedValue = GeneralFunction.GetFilterF4();
            ddlCountry.SelectedValue = GeneralFunction.GetFilterF5();
            ViewState["AdvanceSearch"] = "1";
        }
        BindGrid(false, string.Empty, GridSortOrder.None, true);

    }

    private void LoadForm()
    {
        ltRound.Text = round;

        // Category
        DataTable dt1 = Category.GetSubcategories("01");
        DataTable dt2 = Category.GetSubcategories("02");
        DataTable dt3 = Category.GetSubcategories("03");
        DataTable dt4 = Category.GetSubcategories("04");

        ddlCategory.Items.Add(new ListItem("All", ""));

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Single - Products and Services", "01"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt1.Rows)
            ddlCategory.Items.Add("SP-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Single - Specialty", "02"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt2.Rows)
            ddlCategory.Items.Add("SS-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Multi - Products and Services", "03"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt3.Rows)
            ddlCategory.Items.Add("MP-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Multi - Specialty", "04"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt4.Rows)
            ddlCategory.Items.Add("MS-" + dr["Name"].ToString());



        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryList(false);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));


        // Panel
        ddlPanel.DataSource = GeneralFunction.GetJuryPanelList(round);
        ddlPanel.DataBind();
        ddlPanel.Items.Insert(0, new ListItem("All", ""));

        if (!Security.IsRoleAdmin() && !Security.IsRoleSuperAdmin())
        {
            btnExportAllSummary.Visible = false;
            btnExportMaster.Visible = false;
            btnExportRecuse.Visible = false;
            btnExportSummary.Visible = false;
            btnExportSummaryComments.Visible = false;
        }


        for (int i = 16; i <= 20; i++)
        {
            radGridEntry.MasterTableView.GetColumn("J" + i).Visible = false;
        }

    }

    private void BindGrid(bool isCustomSort, string sortExpression, GridSortOrder order, bool needRebind)
    {
        EntryList list = GeneralFunction.GetAllEntryCache(false);

        // filter only completed
        List<Entry> slist = new List<Entry>();

        slist = list.Where(entry => entry.Status == StatusEntry.Completed && entry.WithdrawnStatus == "" && ((round == "2" && entry.IsRound2) || round != "2")).ToList();

        //foreach (Entry entry in list)
        //{
        //    if (entry.Status == StatusEntry.Completed && entry.WithdrawnStatus == "" && ((round == "2" && entry.IsRound2) || round != "2"))
        //        slist.Add(entry);
        //}

        // filter
        string status = (string)ViewState["TabFilterValue"];
        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Entry> flist = new List<Entry>();

        if (advanceSearch == "1")
        {
            // Advanced search 
            // basic fields
            // user's country
            List<Guid> entryIdList = GeneralFunction.GetFilteredEntryListFromRegCountry(ddlCountry.SelectedValue, true);
            List<Guid> entryIdList2 = GeneralFunction.GetFilteredEntryListFromRegCompany(txtSearch.Text.Trim(), true);

            // jury panel list
            List<Guid> entryIdList5 = GeneralFunction.GetFilteredEntryListFromJuryPanel(ddlPanel.SelectedValue, round, true);

            foreach (Entry item in slist)
            {
                // category strip out the prefix
                string category = ddlCategory.SelectedValue;
                if (category.Length > 3) category = category.Substring(3, category.Length - 3);

                if (
                    (category == "" || (category != "" && (item.CategoryPSDetailFromRound(round) == category || GeneralFunction.IsCategoryInCategoryGroup(category, item.CategoryPSDetailFromRound(round))))) &&
                    (ddlMarket.SelectedValue == "" || (ddlMarket.SelectedValue != "" && item.CategoryMarket == ddlMarket.SelectedValue)) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&
                    (ddlPanel.SelectedValue == "" || (ddlPanel.SelectedValue != "" && entryIdList5.Contains(item.Id))) &&



                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entryId") && item.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entrant") && entryIdList2.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "brand") && item.Brand.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))
                    //(txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "panel") && entryIdList5.Contains(item.Id)))
                    )



                   )
                    flist.Add(item);
            }



        }
        else
        {
            // tab filtering
            //if (status == "WDN")
            //{
            //    foreach (Entry item in list)
            //        if (item.WithdrawnStatus != "") flist.Add(item);
            //}
            //else
            //{
            //    foreach (Entry item in list)
            //        if (status == "" || (status != "" && item.Status == status)) flist.Add(item);
            //}
            flist = slist;
        }

        //// Sort
        //flist = flist.OrderBy(p => p.Serial).ToList();

        #region CustomSorting

        if (isCustomSort)
        {
            List<Registration> regList = RegistrationList.GetRegistrationList(string.Empty, string.Empty, string.Empty).ToList();

            if (sortExpression.Equals("Serial"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Serial select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Serial descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Country"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("DateSubmitted"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Campaign"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Campaign select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Campaign descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Client"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Client select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Client descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("CategoryMarket"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.CategoryMarket orderby entry.CategoryPSDetail select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.CategoryMarket descending orderby entry.CategoryPSDetail descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Panel"))
            {
                List<Entry> sortedfList = new List<Entry>();
                List<Entry> filteredfList = new List<Entry>();
                List<JuryPanelCategory> pnlCategory = new List<JuryPanelCategory>();

                switch (order)
                {
                    case GridSortOrder.Ascending:
                        {
                            pnlCategory = JuryPanelCategoryList.GetJuryPanelCategoryList(string.Empty, string.Empty).OrderBy(m => m.PanelId).ToList();

                            foreach (JuryPanelCategory pnl in pnlCategory)
                            {
                                List<Entry> pnlEntries = GeneralFunction.GetEntryListFromCategory(pnl.CategoryPSDetail, round).OrderBy(m => m.CategoryPSDetail).ToList();

                                sortedfList.AddRange((from entry in pnlEntries
                                                      join mainentry in flist on entry.Id equals mainentry.Id
                                                      select mainentry).ToList());
                            }

                            filteredfList.AddRange(sortedfList);
                            filteredfList.AddRange(flist.Except(sortedfList).OrderBy(m => m.CategoryPSDetail).ToList());

                            break;
                        }
                    case GridSortOrder.Descending:
                        {
                            pnlCategory = JuryPanelCategoryList.GetJuryPanelCategoryList(string.Empty, string.Empty).OrderByDescending(m => m.PanelId).ToList();

                            foreach (JuryPanelCategory pnl in pnlCategory)
                            {
                                List<Entry> pnlEntries = GeneralFunction.GetEntryListFromCategory(pnl.CategoryPSDetail, round).OrderByDescending(m => m.CategoryPSDetail).ToList();

                                sortedfList.AddRange((from entry in pnlEntries
                                                      join mainentry in flist on entry.Id equals mainentry.Id
                                                      select mainentry).ToList());
                            }

                            filteredfList.AddRange(sortedfList);
                            filteredfList.AddRange(flist.Except(sortedfList).OrderByDescending(m => m.CategoryPSDetail).ToList());

                            break;
                        }
                    case GridSortOrder.None:
                        {
                            pnlCategory = JuryPanelCategoryList.GetJuryPanelCategoryList(string.Empty, string.Empty).ToList();

                            foreach (JuryPanelCategory pnl in pnlCategory)
                            {
                                List<Entry> pnlEntries = GeneralFunction.GetEntryListFromCategory(pnl.CategoryPSDetail, round);

                                sortedfList.AddRange((from entry in pnlEntries
                                                      join mainentry in flist on entry.Id equals mainentry.Id
                                                      select mainentry).ToList());
                            }

                            filteredfList.AddRange(sortedfList);
                            filteredfList.AddRange(flist.Except(sortedfList).ToList());

                            break;
                        }
                }

                flist = filteredfList;
            }
            else if (sortExpression.Equals("Entrant"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Company select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Company descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id select entry).ToList();
                        break;
                }
            }

            else if (sortExpression.Equals("Brand"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist orderby entry.Brand select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist orderby entry.Brand descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist select entry).ToList();
                        break;
                }
            }
        }
        else
        {
            //flist = (from entry in flist orderby entry.Invoice descending, entry.DateCreated descending select entry).ToList();
            //flist = (from entry in flist orderby entry.PanelId select entry).ToList();
            flist = flist.OrderBy(x => x.GetPanelId(round)).ThenBy(x => x.CategoryPSDetail).ToList();
        }

        #endregion

        ht.Clear();
        ht2.Clear();

        lstRank.Clear();
        lstRankHL.Clear();
        
        /*
        foreach (Entry entry in flist)
        {
            List<EffieJuryManagementApp.Jury> jurylist = GeneralFunction.GetJuryListFromEntryCategory(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);
            
            // Composite Weighted scores of all possible jury
            double s1 = DisplayScoreForEntry(jurylist, entry,0);
            double s2 = DisplayScoreForEntry(jurylist, entry, 1);
            double s3 = DisplayScoreForEntry(jurylist, entry, 2);
            double s4 = DisplayScoreForEntry(jurylist, entry, 3);
            double s5 = DisplayScoreForEntry(jurylist, entry, 4);
            double s6 = DisplayScoreForEntry(jurylist, entry, 5);
            double s7 = DisplayScoreForEntry(jurylist, entry, 6);
            double s8 = DisplayScoreForEntry(jurylist, entry, 7);
            double s9 = DisplayScoreForEntry(jurylist, entry, 8);
            double s10 = DisplayScoreForEntry(jurylist, entry,9);
            double s11 = DisplayScoreForEntry(jurylist, entry, 10);
            double s12 = DisplayScoreForEntry(jurylist, entry, 11);
            double s13 = DisplayScoreForEntry(jurylist, entry, 12);
            double s14 = DisplayScoreForEntry(jurylist, entry, 13);
            double s15 = DisplayScoreForEntry(jurylist, entry, 14);
            double s16 = DisplayScoreForEntry(jurylist, entry, 15);
            double s17 = DisplayScoreForEntry(jurylist, entry, 16);
            double s18 = DisplayScoreForEntry(jurylist, entry, 17);
            double s19 = DisplayScoreForEntry(jurylist, entry, 18);
            double s20 = DisplayScoreForEntry(jurylist, entry, 19);

            // Total
            int count = 0;
            int hscount = 0;
            double low = 100;
            double high = 0;
            double total = 0;
            double hstotal = 0;

            
            Calculate(s1, ref total, ref count, ref high, ref low);
            Calculate(s2, ref total, ref count, ref high, ref low);
            Calculate(s3, ref total, ref count, ref high, ref low);
            Calculate(s4, ref total, ref count, ref high, ref low);
            Calculate(s5, ref total, ref count, ref high, ref low);
            Calculate(s6, ref total, ref count, ref high, ref low);
            Calculate(s7, ref total, ref count, ref high, ref low);
            Calculate(s8, ref total, ref count, ref high, ref low);
            Calculate(s9, ref total, ref count, ref high, ref low);
            Calculate(s10, ref total, ref count, ref high, ref low);
            Calculate(s11, ref total, ref count, ref high, ref low);
            Calculate(s12, ref total, ref count, ref high, ref low);
            Calculate(s13, ref total, ref count, ref high, ref low);
            Calculate(s14, ref total, ref count, ref high, ref low);
            Calculate(s15, ref total, ref count, ref high, ref low);
            Calculate(s16, ref total, ref count, ref high, ref low);
            Calculate(s17, ref total, ref count, ref high, ref low);
            Calculate(s18, ref total, ref count, ref high, ref low);
            Calculate(s19, ref total, ref count, ref high, ref low);
            Calculate(s20, ref total, ref count, ref high, ref low);


            // total cal
            double avg = total / count;
            if (count == 0) avg = 0;


            // save to ht for later processing of rank
            if (avg > 0)
                ht.Add(entry, avg);



            // hl cal
            if (low != 100) hstotal = total - low;
            hstotal = hstotal - high;

            if (hstotal < 0) hstotal = 0;

            if (low != 100) hscount = count - 1;
            if (high != 0) hscount = hscount - 1;

            if (hscount <= 0) hscount = 0;

            double hsavg = hstotal / hscount;
            if (hscount == 0) hsavg = 0;

            // save to ht for later processing of rank
            if (hsavg > 0)
                ht2.Add(entry, hsavg);
        }
        

        // Ranking
        List<DictionaryEntry> sorted = ht.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();
        List<DictionaryEntry> sorted2 = ht2.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();

        int rank = 1;
        int rank2 = 1;

        foreach (DictionaryEntry item in sorted)
        {
            foreach (Entry entry in flist)
            {
                if (((Entry)item.Key).Id == entry.Id)
                {
                    lstRank.Add(entry.Id, rank);
                    break;
                }
            }
            rank++;
        }

        foreach (DictionaryEntry item in sorted2)
        {
            foreach (Entry entry in flist)
            {
                if (((Entry)item.Key).Id == entry.Id)
                {
                    lstRankHL.Add(entry.Id, rank2);
                    break;
                }
            }
            rank2++;
        }
        */
                               
        radGridEntry.DataSource = flist;

        if (needRebind) radGridEntry.DataBind();

        GeneralFunction.SetReportDataCache(flist);

    }

    #region Events

    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {            
            Entry entry = (Entry)e.Item.DataItem;
                        
            List<EffieJuryManagementApp.Jury> jurylist = GeneralFunction.GetJuryListFromEntryCategory(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);
            List<JuryPanelCategory> jcpList = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);

            
            LinkButton lnkBtn = null;
            Label lbl = null;
            HyperLink lnk = null;

            //// Edit button
            //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            //Security.SecureControlByHiding(lnkBtn);

            // Entry Id
            lbl = (Label)e.Item.FindControl("lbSerial");
            lbl.Text = entry.Serial;


            // Campaign title
            lbl = (Label)e.Item.FindControl("lbCampaign");
            lbl.Text = entry.Campaign;

            // Brand
            lbl = (Label)e.Item.FindControl("lbBrand");
            lbl.Text = entry.Brand;


            // Category
            lbl = (Label)e.Item.FindControl("lbCategory"); //GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + "<br/>" + 
            lbl.Text = entry.CategoryPSDetailFromRound(round);


            // submitted by
            lbl = (Label)e.Item.FindControl("lnkBtnBuSubmittedBy");
            Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
            lbl.Text = "-";
            if (reg != null)
            {
                lbl.Text = reg.Company;
                //lnkBtn.CommandArgument = reg.Id.ToString();
            }


            // client and agency
            CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(entry.Id);
            try
            {
                lbl = (Label)e.Item.FindControl("lbClient");
                lbl.Text = cclist[0].Company;

                //lbl = (Label)e.Item.FindControl("lbAgency1");
                //lbl.Text = cclist[1].Company;

                //lbl = (Label)e.Item.FindControl("lbAgency2");
                //lbl.Text = cclist[2].Company;

                //lbl = (Label)e.Item.FindControl("lbAgency3");
                //lbl.Text = cclist[3].Company;

                //lbl = (Label)e.Item.FindControl("lbAgency4");
                //lbl.Text = cclist[4].Company;

                //lbl = (Label)e.Item.FindControl("lbAgency5");
                //lbl.Text = cclist[5].Company;

            }
            catch { }


            // Country
            lbl = (Label)e.Item.FindControl("lbCountry");
            lbl.Text = "-";
            if (reg != null)
            {
                lbl.Text = reg.Country;
            }


            // Panel
            lbl = (Label)e.Item.FindControl("lbPanel");
            if (jcpList.Count > 0)
                lbl.Text = jcpList[0].PanelId;
            lbl.Text = entry.GetPanelId(round);



            // Advancement vars
            int totaladv = 0;
            int totaladvyes = 0;



            // Composite Weighted scores of all possible jury
            double s1 = DisplayScore(e, jurylist, entry, "lbScoreJ1", 0, ref totaladv, ref totaladvyes);
            double s2 = DisplayScore(e, jurylist, entry, "lbScoreJ2", 1, ref totaladv, ref totaladvyes);
            double s3 = DisplayScore(e, jurylist, entry, "lbScoreJ3", 2, ref totaladv, ref totaladvyes);
            double s4 = DisplayScore(e, jurylist, entry, "lbScoreJ4", 3, ref totaladv, ref totaladvyes);
            double s5 = DisplayScore(e, jurylist, entry, "lbScoreJ5", 4, ref totaladv, ref totaladvyes);
            double s6 = DisplayScore(e, jurylist, entry, "lbScoreJ6", 5, ref totaladv, ref totaladvyes);
            double s7 = DisplayScore(e, jurylist, entry, "lbScoreJ7", 6, ref totaladv, ref totaladvyes);
            double s8 = DisplayScore(e, jurylist, entry, "lbScoreJ8", 7, ref totaladv, ref totaladvyes);
            double s9 = DisplayScore(e, jurylist, entry, "lbScoreJ9", 8, ref totaladv, ref totaladvyes);
            double s10 = DisplayScore(e, jurylist, entry, "lbScoreJ10", 9, ref totaladv, ref totaladvyes);
            double s11 = DisplayScore(e, jurylist, entry, "lbScoreJ11", 10, ref totaladv, ref totaladvyes);
            double s12 = DisplayScore(e, jurylist, entry, "lbScoreJ12", 11, ref totaladv, ref totaladvyes);
            double s13 = DisplayScore(e, jurylist, entry, "lbScoreJ13", 12, ref totaladv, ref totaladvyes);
            double s14 = DisplayScore(e, jurylist, entry, "lbScoreJ14", 13, ref totaladv, ref totaladvyes);
            double s15 = DisplayScore(e, jurylist, entry, "lbScoreJ15", 14, ref totaladv, ref totaladvyes);
            double s16 = DisplayScore(e, jurylist, entry, "lbScoreJ16", 15, ref totaladv, ref totaladvyes);
            double s17 = DisplayScore(e, jurylist, entry, "lbScoreJ17", 16, ref totaladv, ref totaladvyes);
            double s18 = DisplayScore(e, jurylist, entry, "lbScoreJ18", 17, ref totaladv, ref totaladvyes);
            double s19 = DisplayScore(e, jurylist, entry, "lbScoreJ19", 18, ref totaladv, ref totaladvyes);
            double s20 = DisplayScore(e, jurylist, entry, "lbScoreJ20", 19, ref totaladv, ref totaladvyes);

            int rank = 0;
            int rankHL = 0;
            
            try
            {
                rank = lstRank.Where(m => m.Key == entry.Id).Single().Value;
                rankHL = lstRankHL.Where(m => m.Key == entry.Id).Single().Value;
            }
            catch{}

            
            lbl = (Label)e.Item.FindControl("lbRank");
            lbl.Text = (rank == 0 ? "-" :rank.ToString());

            lbl = (Label)e.Item.FindControl("lbHSRank");
            lbl.Text = (rankHL == 0 ? "-" : rankHL.ToString());

            // Advancement
            lbl = (Label)e.Item.FindControl("lbAdv");
            lbl.Text = totaladvyes.ToString();
            lbl = (Label)e.Item.FindControl("lbAdvPercent");
            lbl.Text = "0%";
            if (totaladv != 0 && totaladvyes != 0)
            {
                lbl.Text = (100 * totaladvyes / totaladv).ToString("0") + "%";
            }

            // Total
            int count = 0;
            int hscount = 0;
            double low = 100;
            double high = 0;
            double total = 0;
            double hstotal = 0;
           
            Calculate(s1, ref total, ref count, ref high, ref low);
            Calculate(s2, ref total, ref count, ref high, ref low);
            Calculate(s3, ref total, ref count, ref high, ref low);
            Calculate(s4, ref total, ref count, ref high, ref low);
            Calculate(s5, ref total, ref count, ref high, ref low);
            Calculate(s6, ref total, ref count, ref high, ref low);
            Calculate(s7, ref total, ref count, ref high, ref low);
            Calculate(s8, ref total, ref count, ref high, ref low);
            Calculate(s9, ref total, ref count, ref high, ref low);
            Calculate(s10, ref total, ref count, ref high, ref low);
            Calculate(s11, ref total, ref count, ref high, ref low);
            Calculate(s12, ref total, ref count, ref high, ref low);
            Calculate(s13, ref total, ref count, ref high, ref low);
            Calculate(s14, ref total, ref count, ref high, ref low);
            Calculate(s15, ref total, ref count, ref high, ref low);
            Calculate(s16, ref total, ref count, ref high, ref low);
            Calculate(s17, ref total, ref count, ref high, ref low);
            Calculate(s18, ref total, ref count, ref high, ref low);
            Calculate(s19, ref total, ref count, ref high, ref low);
            Calculate(s20, ref total, ref count, ref high, ref low);


            // total cal
            double avg = total / count;
            if (count == 0) avg = 0;


            // total
            lbl = (Label)e.Item.FindControl("lbScoretotal");
            lbl.Text = total.ToString("N");

            lbl = (Label)e.Item.FindControl("lbScoreCount");
            lbl.Text = count.ToString();

            lbl = (Label)e.Item.FindControl("lbAvgScore");
            lbl.Text = avg.ToString("N");



            //// save to ht for later processing of rank
            //ht.Add(entry, avg);



            // hl cal
            if (low != 100) hstotal = total - low;
            hstotal = hstotal - high;

            if (hstotal < 0) hstotal = 0;

            if (low != 100) hscount = count - 1;
            if (high != 0) hscount = hscount - 1;

            if (hscount <= 0) hscount = 0;

            double hsavg = hstotal / hscount;
            if (hscount == 0) hsavg = 0;


            // High Low
            lbl = (Label)e.Item.FindControl("lbHSScoretotal");
            lbl.Text = hstotal.ToString("N");

            lbl = (Label)e.Item.FindControl("lbHSScoreCount");
            lbl.Text = hscount.ToString();

            lbl = (Label)e.Item.FindControl("lbHSAvgScore");
            lbl.Text = hsavg.ToString("N");


            //// save to ht for later processing of rank
            //ht2.Add(entry, hsavg);            

            // Highlight high and low
            Highlight(e, high, low, "lbScoreJ1");
            Highlight(e, high, low, "lbScoreJ2");
            Highlight(e, high, low, "lbScoreJ3");
            Highlight(e, high, low, "lbScoreJ4");
            Highlight(e, high, low, "lbScoreJ5");
            Highlight(e, high, low, "lbScoreJ6");
            Highlight(e, high, low, "lbScoreJ7");
            Highlight(e, high, low, "lbScoreJ8");
            Highlight(e, high, low, "lbScoreJ9");
            Highlight(e, high, low, "lbScoreJ10");
            Highlight(e, high, low, "lbScoreJ11");
            Highlight(e, high, low, "lbScoreJ12");
            Highlight(e, high, low, "lbScoreJ13");
            Highlight(e, high, low, "lbScoreJ14");
            Highlight(e, high, low, "lbScoreJ15");
            Highlight(e, high, low, "lbScoreJ16");
            Highlight(e, high, low, "lbScoreJ17");
            Highlight(e, high, low, "lbScoreJ18");
            Highlight(e, high, low, "lbScoreJ19");
            Highlight(e, high, low, "lbScoreJ20");


            // chkboxes
            //CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            //Security.SecureControlByHiding(chkbox);

        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            PageSizeCombo.Items.Clear();
            //PageSizeCombo.Items.Add(new RadComboBoxItem("20", "20"));
            //PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            string PageSize = e.Item.OwnerTableView.PageSize.ToString();
            PageSizeCombo.FindItemByValue(PageSizeDefault.ToString()).Selected = true;
        }
    }

    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "score")
        {
            GeneralFunction.SetRedirect("../Admin/JuryAllScoreList.aspx?r=" + round);  // to return from whereever


            try
            {
                // if the score id is available
                Score score = Score.GetScore(new Guid(e.CommandArgument.ToString()));

                Security.SetLoginSessionJury(GeneralFunction.GetDummyJuryForAdminSpoof(score.Juryid));
                Response.Redirect("../Jury/EntryScore.aspx?a=1&r=" + round + "&eId=" + score.EntryId);
            }
            catch
            {
                // no score id, means there is no score record, so then the arg is an entry id | jury Id
                Guid entryId = new Guid(e.CommandArgument.ToString().Split('|')[0]);
                Guid juryId = new Guid(e.CommandArgument.ToString().Split('|')[1]);

                Security.SetLoginSessionJury(GeneralFunction.GetDummyJuryForAdminSpoof(juryId));
                Response.Redirect("../Jury/EntryScore.aspx?a=1&r=" + round + "&eId=" + entryId.ToString());
            }



        }
        //else if (e.CommandName == "Open")
        //{
        //    Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(new Guid(e.CommandArgument.ToString()));
        //    if (entry.Status == StatusEntry.Completed)
        //    {
        //        entry.Status = StatusEntry.UploadCompleted;
        //        entry.Save();
        //    }
        //    BindGrid(true);
        //}
        //else if (e.CommandName == "View")
        //{
        //    //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
        //    Response.Redirect("../Admin/JuryEntryList.aspx?juryId=" + ((GridDataItem)e.Item)["Id"].Text);
        //}
    }

    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
        {
            foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
            {
                BindGrid(true, expr.FieldName, expr.SortOrder, false);
            }
        }
        else
        {
            BindGrid(false, string.Empty, GridSortOrder.None, false);
        }
    }

    protected void rtabEntry_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        //Telerik.Web.UI.RadTab TabClicked = e.Tab;
        //string tabvalue = TabClicked.Value;

        //ViewState["TabFilterValue"] = tabvalue;
        //ViewState["AdvanceSearch"] = "";
        //BindEntry(true);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //rtabEntry.Visible = false;

        // preselect the market if the category was selected
        string cat = ddlCategory.SelectedValue;
        if (cat.Length > 3)
        {
            // First 2 chars is SP / SS / MP / MS
            // convert to SM / MM
            if (cat.Substring(0, 1) == "S") ddlMarket.SelectedValue = "SM";
            if (cat.Substring(0, 1) == "M") ddlMarket.SelectedValue = "MM";
        }
        if (cat.Length == 2)
        {
            // also for selection of category grouping
            if (cat == "01" || cat == "02") ddlMarket.SelectedValue = "SM";
            if (cat == "03" || cat == "04") ddlMarket.SelectedValue = "MM";
        }


        ViewState["AdvanceSearch"] = "1";
        BindGrid(false, string.Empty, GridSortOrder.None, true);

        GeneralFunction.SetFilter("JuryAllScoreList", txtSearch.Text, ddlSearch.SelectedValue, ddlPanel.SelectedValue, ddlCategory.SelectedValue,
                          ddlCountry.SelectedValue, "", "", "");

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlMarket.SelectedValue = "";
        ddlCategory.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlPanel.SelectedValue = "";

        //rtabEntry.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        //rtabEntry.SelectedIndex = 0;
        BindGrid(false, string.Empty, GridSortOrder.None, true);

        GeneralFunction.ResetFilter();
    }

    protected void btnExportSummary_Click(object sender, EventArgs e)
    {
        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Entry> flist = (List<Entry>)data;

            List<Entry> sortedfList = new List<Entry>();
            List<Entry> filteredfList = new List<Entry>();

            List<JuryPanelCategory> pnlCategory = JuryPanelCategoryList.GetJuryPanelCategoryList(string.Empty, string.Empty).Where(m => m.Round == round).OrderBy(m => m.PanelId).ToList();

            foreach (JuryPanelCategory pnl in pnlCategory)
            {
                List<Entry> pnlEntries = GeneralFunction.GetEntryListFromCategory(pnl.CategoryPSDetail, round);

                sortedfList.AddRange((from entry in pnlEntries
                                      join mainentry in flist on entry.Id equals mainentry.Id
                                      select mainentry).ToList());                
            }

            filteredfList.AddRange(sortedfList);
            filteredfList.AddRange(flist.Except(sortedfList).OrderBy(m => m.CategoryPSDetail).ToList());
            
            

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Score Summary";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 3; x++;


            // Entry headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 7.71; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Panel"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;

            // jury score headers
            for (int i = 1; i <= 20; i++)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("J" + i.ToString()); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            }

            // all scores
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Total"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No. of Scores"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Avg. Comp.Score"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.29; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Ranking"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            // adv
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv(%)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            // High low
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Total"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No. of Scores"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Avg. Comp.Score"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Ranking"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;


            // adv
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv(%)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            
            // NEW Nomination
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Grand Effie"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Gold"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Silver"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Bronze"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Finalist"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            #endregion


            y++;

            foreach (Entry ent in filteredfList)
            {
                x = 1;
                Registration reg = GeneralFunction.GetRegistrationFromEntry(ent);
                List<EffieJuryManagementApp.Jury> jlist = GeneralFunction.GetJuryListFromEntryCategory(ent.CategoryMarketFromRound(round), ent.CategoryPSFromRound(round), ent.CategoryPSDetailFromRound(round), round);
                List<JuryPanelCategory> jcpList = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(ent.CategoryMarketFromRound(round), ent.CategoryPSFromRound(round), ent.CategoryPSDetailFromRound(round), round);

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;



                // Entry Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryPSDetailFromRound(round)); x++; //GeneralFunction.GetEntryMarket(ent.CategoryMarketFromRound(round)) + " - " + 
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Campaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;

                // client and agency
                CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(ent.Id);
                string client = "";
                try
                {
                    client = cclist[0].Company;
                }
                catch { }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(client); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Brand); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;

                string panel = "";
                if (jcpList.Count > 0)
                    panel = jcpList[0].PanelId;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(panel); x++;

                // Jury 1 to 20 scores
                int count = 0;
                int hscount = 0;
                double low = 100;
                double high = 0;
                double total = 0;
                double hstotal = 0;
                string NominationLow = "";
                string NominationHigh = "";

                int totaladv = 0;
                int totaladvyes = 0;

                List<Score> submittedscores = new List<Score>();

                Nomination nomination = new Nomination();
                nomination.GrandEffie = 0;
                nomination.Gold = 0;
                nomination.Silver = 0;
                nomination.Bronze = 0;
                nomination.Finalist = 0;

                List<Score> lstCalScore = new List<Score>();

                Score s1 = GetScoreFromJury(jlist, ent, 0, true);
                Score s2 = GetScoreFromJury(jlist, ent, 1, true);
                Score s3 = GetScoreFromJury(jlist, ent, 2, true);
                Score s4 = GetScoreFromJury(jlist, ent, 3, true);
                Score s5 = GetScoreFromJury(jlist, ent, 4, true);
                Score s6 = GetScoreFromJury(jlist, ent, 5, true);
                Score s7 = GetScoreFromJury(jlist, ent, 6, true);
                Score s8 = GetScoreFromJury(jlist, ent, 7, true);
                Score s9 = GetScoreFromJury(jlist, ent, 8, true);
                Score s10 = GetScoreFromJury(jlist, ent, 9, true);
                Score s11 = GetScoreFromJury(jlist, ent, 10, true);
                Score s12 = GetScoreFromJury(jlist, ent, 11, true);
                Score s13 = GetScoreFromJury(jlist, ent, 12, true);
                Score s14 = GetScoreFromJury(jlist, ent, 13, true);
                Score s15 = GetScoreFromJury(jlist, ent, 14, true);
                Score s16 = GetScoreFromJury(jlist, ent, 15, true);
                Score s17 = GetScoreFromJury(jlist, ent, 16, true);
                Score s18 = GetScoreFromJury(jlist, ent, 17, true);
                Score s19 = GetScoreFromJury(jlist, ent, 18, true);
                Score s20 = GetScoreFromJury(jlist, ent, 19, true);

                lstCalScore.Add(s1);
                lstCalScore.Add(s2);
                lstCalScore.Add(s3);
                lstCalScore.Add(s4);
                lstCalScore.Add(s5);
                lstCalScore.Add(s6);
                lstCalScore.Add(s7);
                lstCalScore.Add(s8);
                lstCalScore.Add(s9);
                lstCalScore.Add(s10);
                lstCalScore.Add(s11);
                lstCalScore.Add(s12);
                lstCalScore.Add(s13);
                lstCalScore.Add(s14);
                lstCalScore.Add(s15);
                lstCalScore.Add(s16);
                lstCalScore.Add(s17);
                lstCalScore.Add(s18);
                lstCalScore.Add(s19);
                lstCalScore.Add(s20);

                lstCalScore = lstCalScore.Where(m => m.IsSubmitted && !m.IsAdminRecuse && !m.IsRecuse).ToList();

                

                try
                {
                    if (lstCalScore.Where(m => m.ScoreComposite > 0).Count() > 1) {
                        high = lstCalScore.Where(m => m.ScoreComposite > 0).OrderByDescending(m => m.ScoreComposite).First().ScoreComposite;
                        NominationHigh = lstCalScore.Where(m => m.ScoreComposite > 0).OrderByDescending(m => m.ScoreComposite).First().Nomination;
                    }
                }
                catch { }

                try
                {
                    low = lstCalScore.Where(m => m.ScoreComposite > 0).OrderBy(m => m.ScoreComposite).First().ScoreComposite;
                    NominationLow = lstCalScore.Where(m => m.ScoreComposite > 0).OrderBy(m => m.ScoreComposite).First().Nomination;
                }
                catch { }

                HighLightScoreInReport(workbook, sheetName, s1, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s2, y, x, ref total, ref count,high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s3, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s4, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s5, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s6, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s7, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s8, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s9, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s10, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s11, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s12, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s13, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s14, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s15, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s16, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s17, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s18, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s19, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s20, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;                


                // total cal
                double avg = total / count;
                if (count == 0) avg = 0;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(total); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(count); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(avg); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;

                // rank
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(""); x++;

                //adv
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(totaladvyes); x++;
                string advpercent = "0 %";
                if (totaladv != 0) advpercent = Math.Round(100 * (double)totaladvyes / (double)totaladv).ToString() + " %";
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(advpercent); x++;


                // hl cal
                if (low != 100) hstotal = total - low;
                hstotal = hstotal - high;

                if (hstotal < 0) hstotal = 0;

                if (low != 100) hscount = count - 1;
                if (high != 0) hscount = hscount - 1;

                if (hscount <= 0) hscount = 0;

                double hsavg = hstotal / hscount;
                if (hscount == 0) hsavg = 0;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hstotal); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hscount); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hsavg); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;


                // rank
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(""); x++;


                //adv
                int[] advresults = GetAdvanceCount(FilterHLScoreList(submittedscores));
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(advresults[1]); x++;
                string advpercent2 = "0 %";
                if (advresults[0] != 0) advpercent2 = Math.Round(100 * (double)advresults[1] / (double)advresults[0]).ToString() + " %";
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(advpercent2); x++;


                // NEW Nomination
                if (low != 100) NominationProcess(NominationLow, ref nomination, -1); //Nomination Process Low
                NominationProcess(NominationHigh, ref nomination, -1); //Nomination Process High
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.GrandEffie); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.Gold); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.Silver); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.Bronze); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.Finalist); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                
                // save to ht for later processing of rank
                if (avg > 0)
                    ht.Add(ent, avg);

                // save to ht for later processing of rank
                if (hsavg > 0)
                    ht2.Add(ent, hsavg);





                y++;
            }




            // Ranking
            List<DictionaryEntry> sorted = ht.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();
            int rank = 1;
            foreach (DictionaryEntry item in sorted)
            {
                int rowcounter = 2;
                foreach (Entry ent in filteredfList)
                {
                    if (((Entry)item.Key).Serial.ToString() == ent.Serial)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(rowcounter, 33).SetValue(rank);
                        break;
                    }
                    rowcounter++;
                }
                rank++;
            }



            // Ranking HL
            List<DictionaryEntry> sorted2 = ht2.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();
            int rank2 = 1;
            foreach (DictionaryEntry item in sorted2)
            {
                int rowcounter = 2;
                foreach (Entry ent in filteredfList)
                {
                    if (((Entry)item.Key).Serial.ToString() == ent.Serial)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(rowcounter, 39).SetValue(rank2);
                        break;
                    }
                    rowcounter++;
                }
                rank2++;
            }





            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Score_Summary.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    protected void btnExportSummaryComments_Click(object sender, EventArgs e)
    {
        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Entry> flist = (List<Entry>)data;

            List<Entry> sortedfList = new List<Entry>();
            List<Entry> filteredfList = new List<Entry>();

            List<JuryPanelCategory> pnlCategory = JuryPanelCategoryList.GetJuryPanelCategoryList(string.Empty, string.Empty).Where(m => m.Round == round).OrderBy(m => m.PanelId).ToList();

            foreach (JuryPanelCategory pnl in pnlCategory)
            {
                List<Entry> pnlEntries = GeneralFunction.GetEntryListFromCategory(pnl.CategoryPSDetail, round);

                sortedfList.AddRange((from entry in pnlEntries
                                      join mainentry in flist on entry.Id equals mainentry.Id
                                      select mainentry).ToList());
            }

            filteredfList.AddRange(sortedfList);
            filteredfList.AddRange(flist.Except(sortedfList).OrderBy(m => m.CategoryPSDetail).ToList());

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Score Summary";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 3; x++;


            // Entry headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 7.71; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Panel"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;

            // jury score headers
            for (int i = 1; i <= 20; i++)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("J" + i.ToString()); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            }

            // all scores FIRST
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Total"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No. of Scores"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Avg. Comp.Score"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.29; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Ranking"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            // adv
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv(%)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            // High low SECOND
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Total"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;


            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No. of Scores"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Avg. Comp.Score"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            // NEW Nomination
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Grand Effie"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            
            // adv
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv(%)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            //RANK
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Ranking"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            // NEW Nomination
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Grand Effie"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Gold"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Silver"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Bronze"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Finalist"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            #endregion


            #region Jury feedback headers
            for (int i = 1; i <= 20; i++)
            {
                // 2 columns for feedback and additional comments
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("J" + i.ToString() + "  Feedback - Strongest element"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 40; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("J" + i.ToString() + "  Feedback - Weakest element"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 40; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Add. Comments"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 40; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv."); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            }

            #endregion

            y++;

            foreach (Entry ent in filteredfList)
            {
                x = 1;

                Registration reg = GeneralFunction.GetRegistrationFromEntry(ent);
                List<EffieJuryManagementApp.Jury> jlist = GeneralFunction.GetJuryListFromEntryCategory(ent.CategoryMarketFromRound(round), ent.CategoryPSFromRound(round), ent.CategoryPSDetailFromRound(round), round);
                List<JuryPanelCategory> jcpList = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(ent.CategoryMarketFromRound(round), ent.CategoryPSFromRound(round), ent.CategoryPSDetailFromRound(round), round);

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;



                // Entry Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryPSDetailFromRound(round)); x++; //GeneralFunction.GetEntryMarket(ent.CategoryMarketFromRound(round)) + " - " + 
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Campaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;

                // client and agency
                CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(ent.Id);
                string client = "";
                try
                {
                    client = cclist[0].Company;
                }
                catch { }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(client); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Brand); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;

                string panel = "";
                if (jcpList.Count > 0)
                    panel = jcpList[0].PanelId;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(panel); x++;

                // Jury 1 to 20 scores
                int count = 0;
                int hscount = 0;
                double low = 100;
                double high = 0;
                string NominationLow = "";
                string NominationHigh = "";
                double total = 0;
                double hstotal = 0;

                int totaladv = 0;
                int totaladvyes = 0;

                List<Score> submittedscores = new List<Score>();

                Nomination nomination = new Nomination();
                nomination.GrandEffie = 0;
                nomination.Gold = 0;
                nomination.Silver = 0;
                nomination.Bronze = 0;
                nomination.Finalist = 0;

                List<Score> lstCalScore = new List<Score>();

                Score s1 = GetScoreFromJury(jlist, ent, 0, true);
                Score s2 = GetScoreFromJury(jlist, ent, 1, true);
                Score s3 = GetScoreFromJury(jlist, ent, 2, true);
                Score s4 = GetScoreFromJury(jlist, ent, 3, true);
                Score s5 = GetScoreFromJury(jlist, ent, 4, true);
                Score s6 = GetScoreFromJury(jlist, ent, 5, true);
                Score s7 = GetScoreFromJury(jlist, ent, 6, true);
                Score s8 = GetScoreFromJury(jlist, ent, 7, true);
                Score s9 = GetScoreFromJury(jlist, ent, 8, true);
                Score s10 = GetScoreFromJury(jlist, ent, 9, true);
                Score s11 = GetScoreFromJury(jlist, ent, 10, true);
                Score s12 = GetScoreFromJury(jlist, ent, 11, true);
                Score s13 = GetScoreFromJury(jlist, ent, 12, true);
                Score s14 = GetScoreFromJury(jlist, ent, 13, true);
                Score s15 = GetScoreFromJury(jlist, ent, 14, true);
                Score s16 = GetScoreFromJury(jlist, ent, 15, true);
                Score s17 = GetScoreFromJury(jlist, ent, 16, true);
                Score s18 = GetScoreFromJury(jlist, ent, 17, true);
                Score s19 = GetScoreFromJury(jlist, ent, 18, true);
                Score s20 = GetScoreFromJury(jlist, ent, 19, true);

                lstCalScore.Add(s1);
                lstCalScore.Add(s2);
                lstCalScore.Add(s3);
                lstCalScore.Add(s4);
                lstCalScore.Add(s5);
                lstCalScore.Add(s6);
                lstCalScore.Add(s7);
                lstCalScore.Add(s8);
                lstCalScore.Add(s9);
                lstCalScore.Add(s10);
                lstCalScore.Add(s11);
                lstCalScore.Add(s12);
                lstCalScore.Add(s13);
                lstCalScore.Add(s14);
                lstCalScore.Add(s15);
                lstCalScore.Add(s16);
                lstCalScore.Add(s17);
                lstCalScore.Add(s18);
                lstCalScore.Add(s19);
                lstCalScore.Add(s20);

                lstCalScore = lstCalScore.Where(m => m.IsSubmitted && !m.IsAdminRecuse && !m.IsRecuse).ToList();

                try
                {
                    if (lstCalScore.Where(m => m.ScoreComposite > 0).Count() > 1)
                    {
                        high = lstCalScore.Where(m => m.ScoreComposite > 0).OrderByDescending(m => m.ScoreComposite).First().ScoreComposite;
                        NominationHigh = lstCalScore.Where(m => m.ScoreComposite > 0).OrderByDescending(m => m.ScoreComposite).First().Nomination;
                    }
                       
                }
                catch { }

                try
                {
                    low = lstCalScore.Where(m => m.ScoreComposite > 0).OrderBy(m => m.ScoreComposite).First().ScoreComposite;
                    NominationLow = lstCalScore.Where(m => m.ScoreComposite > 0).OrderBy(m => m.ScoreComposite).First().Nomination;
                }
                catch { }

                HighLightScoreInReport(workbook, sheetName, s1, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s2, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s3, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s4, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s5, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s6, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s7, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s8, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s9, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s10, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s11, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s12, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s13, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s14, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s15, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s16, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s17, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s18, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s19, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s20, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;        
                
                // total cal FIRST
                double avg = total / count;
                if (count == 0) avg = 0;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(total); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(count); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(avg); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;

                // rank
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(""); x++;
                
                // hl cal
                if (low != 100) hstotal = total - low;
                hstotal = hstotal - high;

                if (hstotal < 0) hstotal = 0;

                if (low != 100) hscount = count - 1;
                if (high != 0) hscount = hscount - 1;

                if (hscount <= 0) hscount = 0;

                double hsavg = hstotal / hscount;
                if (hscount == 0) hsavg = 0;
                
                //adv
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(totaladvyes); x++;
                string advpercent = "0 %";
                if (totaladv != 0) advpercent = Math.Round(100 * (double)totaladvyes / (double)totaladv).ToString() + " %";
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(advpercent); x++;

                //TOTAL SECOND
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hstotal); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;

                // No.of Scores
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hscount); x++;

                //Avg.Comp.Score
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hsavg); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;

                // GrandEffie
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.GrandEffie); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                
                //adv
                int[] advresults = GetAdvanceCount(FilterHLScoreList(submittedscores));
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(advresults[1]); x++;
                string advpercent2 = "0 %";
                if (advresults[0] != 0) advpercent2 = Math.Round(100 * (double)advresults[1] / (double)advresults[0]).ToString() + " %";
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(advpercent2); x++;

                // rank
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(""); x++;

                // NEW Nomination
                if (low != 100) NominationProcess(NominationLow, ref nomination, -1); //Nomination Process Low
                NominationProcess(NominationHigh, ref nomination, -1); //Nomination Process High
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.GrandEffie); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.Gold); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.Silver); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.Bronze); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(nomination.Finalist); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;


                // save to ht for later processing of rank
                if (avg > 0)
                    ht.Add(ent, avg);

                // save to ht for later processing of rank
                if (hsavg > 0)
                    ht2.Add(ent, hsavg);



                // feedback and additional comments
                for (int i = 0; i < 20; i++)
                {
                    Score score = null;
                    try
                    {
                        score = GeneralFunction.GetScoreFromMatchingEntryCache(ent, jlist[i].Id, round);
                    }
                    catch
                    {
                        x += (20 - i) * 3;
                        break;
                    }

                    if (score != null && !score.IsRecuse && !score.IsAdminRecuse)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.FeedbackStrong); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.FeedbackWeak); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.AdditionalComments); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((score.IsAdvancement) ? "Yes" : "No"); x++;
                    }
                    else
                    {
                        x = x + 4;
                    }
                }



                y++;
            }




            // Ranking
            List<DictionaryEntry> sorted = ht.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();
            int rank = 1;
            foreach (DictionaryEntry item in sorted)
            {
                int rowcounter = 2;
                foreach (Entry ent in filteredfList)
                {
                    if (((Entry)item.Key).Serial.ToString() == ent.Serial)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(rowcounter, 33).SetValue(rank);
                        break;
                    }
                    rowcounter++;
                }
                rank++;
            }



            // Ranking HL
            List<DictionaryEntry> sorted2 = ht2.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();
            int rank2 = 1;
            foreach (DictionaryEntry item in sorted2)
            {
                int rowcounter = 2;
                foreach (Entry ent in filteredfList)
                {
                    if (((Entry)item.Key).Serial.ToString() == ent.Serial)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(rowcounter, 42).SetValue(rank2);
                        break;
                    }
                    rowcounter++;
                }
                rank2++;
            }





            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Score_SummaryComments.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    protected void NominationProcess(string Nomination, ref Nomination nomination, int num)
    {
        if (Nomination == EnumNomination.Bronze.ToString())
        {
            nomination.Bronze += num;
        }
        else if (Nomination == EnumNomination.Finalist.ToString())
        {
            nomination.Finalist += num;
        }
        else if (Nomination == EnumNomination.Gold.ToString())
        {
            nomination.Gold += num;
        }
        else if (Nomination == EnumNomination.GrandEffie.ToString())
        {
            nomination.GrandEffie += num;
        }
        else if (Nomination == EnumNomination.Silver.ToString())
        {
            nomination.Silver += num;
        }
    }
    
    protected void btnExportAllSummary_Click(object sender, EventArgs e)
    {
        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Entry> flist = (List<Entry>)data;

            List<Entry> sortedfList = new List<Entry>();
            List<Entry> filteredfList = new List<Entry>();

            List<JuryPanelCategory> pnlCategory = JuryPanelCategoryList.GetJuryPanelCategoryList(string.Empty, string.Empty).Where(m => m.Round == round).OrderBy(m => m.PanelId).ToList();

            foreach (JuryPanelCategory pnl in pnlCategory)
            {
                List<Entry> pnlEntries = GeneralFunction.GetEntryListFromCategory(pnl.CategoryPSDetail, round);

                sortedfList.AddRange((from entry in pnlEntries
                                      join mainentry in flist on entry.Id equals mainentry.Id
                                      select mainentry).ToList());
            }

            filteredfList.AddRange(sortedfList);
            filteredfList.AddRange(flist.Except(sortedfList).OrderBy(m => m.CategoryPSDetail).ToList());

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Score All Summary";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 3; x++;


            // Entry headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 7.71; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Panel"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;

            // jury score headers
            for (int i = 1; i <= 20; i++)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("J" + i.ToString()); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            }

            // all scores
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Total"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No. of Scores"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Avg. Comp.Score"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.29; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Ranking"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            // High low
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Total"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No. of Scores"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Avg. Comp.Score"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Ranking"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 6.43; x++;

            #endregion


            y++;

            foreach (Entry ent in filteredfList)
            {
                x = 1;

                Registration reg = GeneralFunction.GetRegistrationFromEntry(ent);
                List<EffieJuryManagementApp.Jury> jlist = GeneralFunction.GetJuryListFromEntryCategory(ent.CategoryMarketFromRound(round), ent.CategoryPSFromRound(round), ent.CategoryPSDetailFromRound(round), round);
                List<JuryPanelCategory> jcpList = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(ent.CategoryMarketFromRound(round), ent.CategoryPSFromRound(round), ent.CategoryPSDetailFromRound(round), round);

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;



                // Entry Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryPSDetailFromRound(round)); x++;//ent.CategoryMarketFromRound(round)) + " - " + 
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Campaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;

                // client and agency
                CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(ent.Id);
                string client = "";
                try
                {
                    client = cclist[0].Company;
                }
                catch { }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(client); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Brand); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;

                string panel = "";
                if (jcpList.Count > 0)
                    panel = jcpList[0].PanelId;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(panel); x++;

                // Jury 1 to 20 scores
                int count = 0;
                int hscount = 0;
                double low = 100;
                double high = 0;
                double total = 0;
                double hstotal = 0;

                int totaladv = 0;
                int totaladvyes = 0;

                List<Score> submittedscores = new List<Score>();

                Nomination nomination = new Nomination();
                nomination.GrandEffie = 0;
                nomination.Gold = 0;
                nomination.Silver = 0;
                nomination.Bronze = 0;
                nomination.Finalist = 0;

                List<Score> lstCalScore = new List<Score>();

                Score s1 = GetScoreFromJury(jlist, ent, 0, false);
                Score s2 = GetScoreFromJury(jlist, ent, 1, false);
                Score s3 = GetScoreFromJury(jlist, ent, 2, false);
                Score s4 = GetScoreFromJury(jlist, ent, 3, false);
                Score s5 = GetScoreFromJury(jlist, ent, 4, false);
                Score s6 = GetScoreFromJury(jlist, ent, 5, false);
                Score s7 = GetScoreFromJury(jlist, ent, 6, false);
                Score s8 = GetScoreFromJury(jlist, ent, 7, false);
                Score s9 = GetScoreFromJury(jlist, ent, 8, false);
                Score s10 = GetScoreFromJury(jlist, ent, 9, false);
                Score s11 = GetScoreFromJury(jlist, ent, 10, false);
                Score s12 = GetScoreFromJury(jlist, ent, 11, false);
                Score s13 = GetScoreFromJury(jlist, ent, 12, false);
                Score s14 = GetScoreFromJury(jlist, ent, 13, false);
                Score s15 = GetScoreFromJury(jlist, ent, 14, false);
                Score s16 = GetScoreFromJury(jlist, ent, 15, false);
                Score s17 = GetScoreFromJury(jlist, ent, 16, false);
                Score s18 = GetScoreFromJury(jlist, ent, 17, false);
                Score s19 = GetScoreFromJury(jlist, ent, 18, false);
                Score s20 = GetScoreFromJury(jlist, ent, 19, false);

                lstCalScore.Add(s1);
                lstCalScore.Add(s2);
                lstCalScore.Add(s3);
                lstCalScore.Add(s4);
                lstCalScore.Add(s5);
                lstCalScore.Add(s6);
                lstCalScore.Add(s7);
                lstCalScore.Add(s8);
                lstCalScore.Add(s9);
                lstCalScore.Add(s10);
                lstCalScore.Add(s11);
                lstCalScore.Add(s12);
                lstCalScore.Add(s13);
                lstCalScore.Add(s14);
                lstCalScore.Add(s15);
                lstCalScore.Add(s16);
                lstCalScore.Add(s17);
                lstCalScore.Add(s18);
                lstCalScore.Add(s19);
                lstCalScore.Add(s20);

                lstCalScore = lstCalScore.Where(m => m.IsSubmitted && !m.IsAdminRecuse && !m.IsRecuse).ToList();

                try
                {
                    if (lstCalScore.Where(m => m.ScoreComposite > 0).Count() > 1)
                        high = lstCalScore.Where(m => m.ScoreComposite > 0).OrderByDescending(m => m.ScoreComposite).First().ScoreComposite;
                }
                catch { }

                try
                {
                    low = lstCalScore.Where(m => m.ScoreComposite > 0).OrderBy(m => m.ScoreComposite).First().ScoreComposite;
                }
                catch { }

                HighLightScoreInReport(workbook, sheetName, s1, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s2, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s3, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s4, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s5, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s6, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s7, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s8, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s9, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s10, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s11, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s12, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s13, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s14, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s15, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s16, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s17, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s18, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s19, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;
                HighLightScoreInReport(workbook, sheetName, s20, y, x, ref total, ref count, high, low, ref totaladvyes, ref totaladv, ref submittedscores, ref nomination); x++;        

                // total cal
                double avg = total / count;
                if (count == 0) avg = 0;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(total); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(count); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(avg); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;

                // rank
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(""); x++;



                // hl cal
                if (low != 100) hstotal = total - low;
                hstotal = hstotal - high;

                if (hstotal < 0) hstotal = 0;

                if (low != 100) hscount = count - 1;
                if (high != 0) hscount = hscount - 1;

                if (hscount <= 0) hscount = 0;

                double hsavg = hstotal / hscount;
                if (hscount == 0) hsavg = 0;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hstotal); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hscount); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(hsavg); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;


                // rank
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(""); x++;


                // save to ht for later processing of rank
                if (avg > 0)
                    ht.Add(ent, avg);

                // save to ht for later processing of rank
                if (hsavg > 0)
                    ht2.Add(ent, hsavg);





                y++;
            }




            // Ranking
            List<DictionaryEntry> sorted = ht.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();
            int rank = 1;
            foreach (DictionaryEntry item in sorted)
            {
                int rowcounter = 2;
                foreach (Entry ent in filteredfList)
                {
                    if (((Entry)item.Key).Serial.ToString() == ent.Serial)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(rowcounter, 33).SetValue(rank);
                        break;
                    }
                    rowcounter++;
                }
                rank++;
            }



            // Ranking HL
            List<DictionaryEntry> sorted2 = ht2.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();
            int rank2 = 1;
            foreach (DictionaryEntry item in sorted2)
            {
                int rowcounter = 2;
                foreach (Entry ent in filteredfList)
                {
                    if (((Entry)item.Key).Serial.ToString() == ent.Serial)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(rowcounter, 37).SetValue(rank2);
                        break;
                    }
                    rowcounter++;
                }
                rank2++;
            }





            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Score_All_Summary.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    protected void btnExportMaster_Click(object sender, EventArgs e)
    {
        //object data = GeneralFunction.GetReportDataCache();
        //List<Entry> flist = (List<Entry>)data;

        XLWorkbook workbook = new XLWorkbook();
        MemoryStream memoryStream = new MemoryStream();

        DataTable cats = Category.GetSubcategories("");

        foreach (DataRow row in cats.Rows)
        {
            string sheetName = row["Prefix"].ToString();
            workbook.Worksheets.Add(sheetName);


            int x = 1;
            int y = 3;
            GenerateMasterReport("all", workbook, sheetName, "SCORE REPORT BY JURY (ALL ENTRIES)", row, x, ref y);


            x = 1;
            y += 5;
            GenerateMasterReport("highlow", workbook, sheetName, "SCORE REPORT BY ENTRY (DROP HIGHEST & LOWEST SCORES)", row, x, ref y);

        }




        workbook.SaveAs(memoryStream);

        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=Effie_Score_Master.xlsx");

        memoryStream.WriteTo(Response.OutputStream);
        Response.End();

    }

    protected void btnExportRecuse_Click(object sender, EventArgs e)
    {
        //object data = GeneralFunction.GetReportDataCache();


        //if (data != null)
        //{
        //List<Score> flist = (List<Score>)data;
        ScoreList list = GeneralFunction.GetAllScoreCache(true);

        // filter only recused
        List<Score> flist = new List<Score>();
        foreach (Score score in list)
        {
            if ((score.IsAdminRecuse || score.IsRecuse) && score.Round == round)
                flist.Add(score);
        }



        XLWorkbook workbook = new XLWorkbook();
        MemoryStream memoryStream = new MemoryStream();
        int x = 1;
        int y = 1;
        string sheetName = "Recuse Report";
        workbook.Worksheets.Add(sheetName);
        x = 1;


        #region Basic Entry Headers
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;


        // basic details
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 30; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 30; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;

        #endregion


        #region Jury Headers

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("JuryId"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Jury Name"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Network"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Holding Company"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Jury Recuse"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Admin Recuse"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Recuse Reason"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 30; x++;

        #endregion


        y++;

        foreach (Score score in flist)
        {
            x = 1;
            Entry entry = GeneralFunction.GetEntryFromIDCache(score.EntryId);
            EffieJuryManagementApp.Jury jury = GeneralFunction.GetJuryFromIDCache(score.Juryid, round);

            #region Basic Entry DataRows

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;

            // Basic Details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Serial); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + " - " + entry.CategoryPSDetailFromRound(round)); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Campaign); x++;

            Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
            if (reg != null)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;
            }
            else
                x = x + 2;


            // jury
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SerialNo); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.FirstName + " " + jury.LastName); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Designation); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Company); x++;

            string network = jury.Network;
            if (jury.NetworkOthers != "") network += " - " + jury.NetworkOthers;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(network); x++;
            string holdingcompany = jury.HoldingCompany;
            if (jury.HoldingCompanyOthers != "") holdingcompany += " - " + jury.HoldingCompanyOthers;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(holdingcompany); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Country); x++;


            // recuse and remarks
            string recuse = "";
            if (score.IsRecuse) recuse = "Yes";
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(recuse); x++;


            string adminrecuse = "";
            if (score.IsAdminRecuse) adminrecuse = "Yes";
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(adminrecuse); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.RecuseRemarks); x++;

            #endregion

            y++;
        }



        workbook.SaveAs(memoryStream);

        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=Effie_Flag_Recuse.xlsx");

        memoryStream.WriteTo(Response.OutputStream);
        Response.End();
        //}
    }

    #endregion

    #region Helper

    private ListItem SeparatorItem()
    {
        ListItem separator = new ListItem("-------------------------------", "");
        separator.Attributes.Add("disabled", "disabled");

        return separator;
    }

    private double DisplayScore(GridItemEventArgs e, List<EffieJuryManagementApp.Jury> jurylist, Entry entry, string labelname, int index, ref int totaladv, ref int totaladvyes)
    {
        try
        {
            Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jurylist[index].Id, round);
            LinkButton lbl = (LinkButton)e.Item.FindControl(labelname);

            if (score != null)
            {
                if (score.IsAdminRecuse || score.IsRecuse)
                {
                    lbl.Text = "X";
                    lbl.CommandArgument = score.Id.ToString();
                    //lbl.Enabled = false;
                    return -1;
                }

                //Moved
                //// Total possible scores adv
                //totaladv++;

                if (score.IsSubmitted)
                {
                    // Total possible scores adv
                    totaladv++;

                    lbl.Text = score.ScoreComposite.ToString("N");
                    lbl.CommandArgument = score.Id.ToString();

                    // Advancement yes
                    if (score.IsAdvancement) totaladvyes++;

                    return score.ScoreComposite;
                }
                else
                {
                    lbl.Text = "NS";
                    lbl.CommandArgument = score.Id.ToString();
                    //return -1;
                }
            }
            else
            {
                // no physical score record, so we embed the entry id | jury Id
                lbl.Text = "NS";
                lbl.CommandArgument = entry.Id.ToString() + "|" + jurylist[index].Id.ToString();
                //return -1;

                //No need anymore
                //// Total possible scores adv
                //totaladv++;
            }

            //lbl.Enabled = false;
            return -1;
        }
        catch { return -1; }
    }

    private double DisplayScoreForEntry(List<EffieJuryManagementApp.Jury> jurylist, Entry entry, int index)
    {
        try
        {
            Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jurylist[index].Id, round);

            if (score != null)
            {
                if (score.IsAdminRecuse || score.IsRecuse)
                {
                    return -1;
                }

                if (score.IsSubmitted)
                {
                    return score.ScoreComposite;
                }
            }
            return -1;
        }
        catch { return -1; }
    }

    private Score GetScoreFromJury(List<EffieJuryManagementApp.Jury> jurylist, Entry entry, int index, bool IsSubmitScoreOnly)
    {
        try
        {
            Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jurylist[index].Id, round);

            if (score != null)
            {
                if (IsSubmitScoreOnly && !score.IsSubmitted && !score.IsAdminRecuse && !score.IsRecuse)
                    return Score.NewScore();

                return score;
            }
            if (jurylist.Count <= index)
            {
                Score scoreDummy = Score.NewScore();
                scoreDummy.Juryid = new Guid("11111111-1111-1111-1111-111111111111");
                return scoreDummy;
            }
            return Score.NewScore();
        }
        catch
        {
            try
            {
                if (jurylist.Count <= index)
                {
                    Score scoreDummy = Score.NewScore();
                    scoreDummy.Juryid = new Guid("11111111-1111-1111-1111-111111111111");
                    return scoreDummy;
                }
            }
            catch
            {
                Score scoreDummy = Score.NewScore();
                scoreDummy.Juryid = new Guid("11111111-1111-1111-1111-111111111111");
                return scoreDummy;
            }
            return Score.NewScore();
        }
    }

    private void Calculate(double s, ref double total, ref int count, ref double high, ref double low)
    {
        if (s >= 0)
        {
            count++;
            if (s > high) high = s;
            if (s < low) low = s;
            total += s;
        }
        //else
        //    s = 0;
    }

    private void Highlight(GridItemEventArgs e, double high, double low, string labelname)
    {
        LinkButton lbl = (LinkButton)e.Item.FindControl(labelname);
        if (lbl.Text == high.ToString("N"))
            lbl.ForeColor = System.Drawing.Color.Green;
        if (lbl.Text == low.ToString("N"))
            lbl.ForeColor = System.Drawing.Color.Red;

    }

    private void HighLightScoreInReport(XLWorkbook workbook, string sheetName, Score score, int y, int x, ref double total, ref int count, double high, double low, ref int totaladvyes, ref int totaladv, ref List<Score> submittedscores, ref Nomination nomination)
    {
        double scorevalue = -1;
        bool isScorePending = false;

        if (score != null)
        {
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS");

            if (score.IsAdminRecuse || score.IsRecuse)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("X");
                scorevalue = -1;
            }
            else
            {
                // Change color to indicate that this score is a pending status
                if (!score.IsSubmitted && !score.IsAdminRecuse && !score.IsRecuse && score.ScoreComposite > 0)
                {                    
                    isScorePending = true;
                }

                if (score.IsSubmitted)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreComposite);
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00";
                    scorevalue = score.ScoreComposite;

                    // yes adv
                    if (score.IsAdvancement) totaladvyes++;

                    // increase the total count
                    totaladv++;

                    submittedscores.Add(score);
                }
                //else
                //{
                //    //// draft scores
                //    //totaladv++;
                //    //submittedscores.Add(Score.NewScore());
                //}

                //Clean up Cell if no jury
                if (score.Juryid == new Guid("11111111-1111-1111-1111-111111111111"))
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("");
            }
        }
        else
        {
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS");

            //// increase the total count
            //totaladv++;

            //// new dummy scores are added
            //submittedscores.Add(Score.NewScore());
        }

        if (score.IsSubmitted && !score.IsAdminRecuse && !score.IsRecuse && score.ScoreComposite >= 0)
        {
            count++;

            total += score.ScoreComposite;
        }

        if (score.Nomination == EnumNomination.Bronze.ToString())
        {
            nomination.Bronze++;
        }
        else if (score.Nomination == EnumNomination.Finalist.ToString())
        {
            nomination.Finalist++;
        }
        else if (score.Nomination == EnumNomination.Gold.ToString())
        {
            nomination.Gold++;
        }
        else if (score.Nomination == EnumNomination.GrandEffie.ToString())
        {
            nomination.GrandEffie++;
        }
        else if (score.Nomination == EnumNomination.Silver.ToString())
        {
            nomination.Silver++;
        }

        if (isScorePending)
        {
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreComposite);
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00";
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.Font.FontColor = XLColor.Silver;
        }
        else
        {
            if (scorevalue == high)
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.Font.FontColor = XLColor.Green;
            if (scorevalue == low)
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.Font.FontColor = XLColor.Red;
        }
    }

    private void GenerateMasterReport(string type, XLWorkbook workbook, string sheetName, string sheettitle, DataRow row, int x, ref int y)
    {
        int y_origin = y;

        Hashtable ht3 = new Hashtable();

        // Data
        string categoryMarket = "";
        string categoryPS = "";
        string categoryPSDetail = row["Name"].ToString();

        // market
        if (row["ColumnId"].ToString() == "01" || row["ColumnId"].ToString() == "02") categoryMarket = "SM";
        if (row["ColumnId"].ToString() == "03" || row["ColumnId"].ToString() == "04") categoryMarket = "MM";

        // p or s
        if (row["ColumnId"].ToString() == "01" || row["ColumnId"].ToString() == "03") categoryPS = "PSC";
        if (row["ColumnId"].ToString() == "02" || row["ColumnId"].ToString() == "04") categoryPS = "SC";

        List<Entry> flist = GeneralFunction.GetEntryListFromCategory(categoryMarket, categoryPS, categoryPSDetail, round);
        List<EffieJuryManagementApp.Jury> jurylist = GeneralFunction.GetJuryListFromEntryCategory(categoryMarket, categoryPS, categoryPSDetail, round);

        workbook.Worksheets.Worksheet(sheetName).Cell(y - 2, x).Style.Font.Bold = true;

        #region Basic Entry Headers


        workbook.Worksheets.Worksheet(sheetName).Cell(y - 2, x).SetValue(sheettitle);
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;

        // Entry headers
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 23; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;


        // jury score headers
        for (int i = 1; i <= 20; i++)
        {
            // Jury header
            workbook.Worksheets.Worksheet(sheetName).Cell(y - 2, x).SetValue("Judge " + i.ToString());
            workbook.Worksheets.Worksheet(sheetName).Cell(y - 2, x).Style.Font.Bold = true;
            string juryId = "";
            try
            {
                juryId = jurylist[i - 1].SerialNo;
            }
            catch { }
            workbook.Worksheets.Worksheet(sheetName).Cell(y - 1, x).SetValue("Jury Id:" + juryId);
            workbook.Worksheets.Worksheet(sheetName).Cell(y - 1, x).Style.Font.Bold = true;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("SC"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 2.57; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ID"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 2.57; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IL"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 2.57; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RE"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 2.57; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Comp."); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
        }

        // avg score by section
        workbook.Worksheets.Worksheet(sheetName).Cell(y - 1, x).SetValue("Avg score by section");
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("SC"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.57; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ID"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.57; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IL"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.57; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RE"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.57; x++;


        // Misc
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Total"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.43; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No. of Scores"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.43; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Avg. Comp.Score"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.43; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Ranking"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.43; x++;


        // Adv
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.43; x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Adv(%)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;


        #endregion


        y++;

        int counter = 1;
        foreach (Entry ent in flist)
        {
            x = 1;

            Registration reg = GeneralFunction.GetRegistrationFromEntry(ent);

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(counter.ToString()); x++;


            #region Entry Details

            // Entry Details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryPSDetailFromRound(round)); x++; //GeneralFunction.GetEntryMarket(ent.CategoryMarketFromRound(round)) + " - " + 
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Campaign); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;

            // client and agency
            CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(ent.Id);
            string client = "";
            try
            {
                client = cclist[0].Company;
            }
            catch { }
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(client); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Brand); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;

            #endregion


            #region Jury Scores

            // jury scores
            int totalsc = 0;
            int totalid = 0;
            int totalil = 0;
            int totalre = 0;
            double totalcomp = 0;
            int scorecount = 0;

            double low = 100;
            double high = 0;

            int totaladv = 0;
            int totaladvyes = 0;
            List<Score> submittedscores = new List<Score>();

            for (int i = 1; i <= 20; i++)
            {
                EffieJuryManagementApp.Jury jury = null;
                try
                {
                    jury = jurylist[i - 1];

                }
                catch { }
                Score score = null;
                if (jury != null) score = GeneralFunction.GetScoreFromMatchingEntryCache(ent, jury.Id, round);

                if (score != null)
                {
                    if (!score.IsRecuse && !score.IsAdminRecuse)
                    {
                        if (score.IsSubmitted)
                        {
                            totalsc += score.ScoreSC;
                            totalid += score.ScoreID;
                            totalil += score.ScoreIL;
                            totalre += score.ScoreRE;
                            totalcomp += score.ScoreComposite;
                            scorecount++;

                            if (score.ScoreComposite > high) high = score.ScoreComposite;
                            if (score.ScoreComposite < low) low = score.ScoreComposite;


                            // bg fill for DQ flagged scores
                            XLColor bgcolor = XLColor.LightSteelBlue;

                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreSC); if (score.Flag != "") workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.Fill.BackgroundColor = bgcolor; x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreID); if (score.Flag != "") workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.Fill.BackgroundColor = bgcolor; x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreIL); if (score.Flag != "") workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.Fill.BackgroundColor = bgcolor; x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreRE); if (score.Flag != "") workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.Fill.BackgroundColor = bgcolor; x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreComposite); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; if (score.Flag != "") workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.Fill.BackgroundColor = bgcolor; x++;

                            // yes adv
                            if (score.IsAdvancement) totaladvyes++;

                            submittedscores.Add(score);

                            // increase the total count
                            totaladv++;
                        }
                        else // jury and score is present but not submitted
                        {
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;

                            //// new dummy scores are added
                            //submittedscores.Add(Score.NewScore());

                        }

                        //// increase the total count
                        //totaladv++;

                    }
                    else // jury recused this entry
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("X"); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("X"); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("X"); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("X"); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("X"); x++;
                    }
                }
                else if (jury != null)  // there is a jury but no score, means NS
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("NS"); x++;

                    //// increase the total count
                    //totaladv++;

                    //// new dummy scores are added
                    //submittedscores.Add(Score.NewScore());
                }
                else // no jury at all, skip
                    x = x + 5;
            }


            // Highlight high and low scores
            if (type == "highlow")
            {
                int xoffset = -1;
                for (int i = 0; i < 20; i++)
                {
                    string strvalue = workbook.Worksheets.Worksheet(sheetName).Cell(y, x + xoffset).Value.ToString();
                    if (strvalue != "NS" && strvalue != "" && strvalue != "X")
                    {
                        double value = double.Parse(double.Parse(strvalue).ToString("N"));
                        if (value == double.Parse(high.ToString("N")))
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x + xoffset).Style.Font.FontColor = XLColor.Green;

                        if (value == double.Parse(low.ToString("N")))
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x + xoffset).Style.Font.FontColor = XLColor.Red;
                    }
                    xoffset -= 5;
                }
            }




            #endregion


            #region Misc

            // avg score by section
            double avgsc = 0; double avgid = 0; double avgil = 0; double avgre = 0; double avgcomp = 0;

            if (type == "highlow")
            {
                // hl cal
                if (low != 100) totalcomp = totalcomp - low;
                totalcomp = totalcomp - high;

                if (totalcomp < 0) totalcomp = 0;

                if (low != 100) scorecount = scorecount - 1;
                if (high != 0) scorecount = scorecount - 1;

                if (scorecount <= 0) scorecount = 0;

            }

            if (scorecount != 0)
            {
                avgsc = totalsc / scorecount;
                avgid = totalid / scorecount;
                avgil = totalil / scorecount;
                avgre = totalre / scorecount;
                avgcomp = totalcomp / scorecount;
            }

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(avgsc); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(avgid); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(avgil); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(avgre); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;

            // Misc
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(totalcomp); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(scorecount); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(avgcomp); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(""); x++;  // ranking

            #endregion


            #region Adv
            //adv
            int[] advresults = new int[0];
            if (type == "highlow")
                advresults = GetAdvanceCount(FilterHLScoreList(submittedscores));
            else
                advresults = GetAdvanceCount(submittedscores);
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(advresults[1]); x++;
            string advpercent = "0 %";
            if (advresults[0] != 0) advpercent = Math.Round(100 * (double)advresults[1] / (double)advresults[0]).ToString() + " %";
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(advpercent); x++;
            #endregion

            // for ranking
            if (avgcomp > 0)
                ht3.Add(ent, avgcomp);



            y++;
            counter++;
        }




        // Ranking
        List<DictionaryEntry> sorted = ht3.Cast<DictionaryEntry>().OrderByDescending(entry => entry.Value).ToList();
        int rank = 1;
        foreach (DictionaryEntry item in sorted)
        {
            int rowcounter = y_origin + 1;  // starting row count in the worksheet that the 1st data row starts
            foreach (Entry ent in flist)
            {
                if (((Entry)item.Key).Serial.ToString() == ent.Serial)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(rowcounter, 116).SetValue(rank);
                    break;
                }
                rowcounter++;
            }
            rank++;
        }
    }

    private List<Score> FilterHLScoreList(List<Score> scores)
    {
        double high = 0;
        double low = Double.MaxValue;

        Score scorelow = null;
        Score scorehigh = null;

        foreach (Score score in scores)
        {
            if (!score.IsNew && score.IsSubmitted)
            {
                if (score.ScoreComposite > high)
                {
                    high = score.ScoreComposite;
                    scorehigh = score;
                }
                if (score.ScoreComposite < low)
                {
                    low = score.ScoreComposite;
                    scorelow = score;
                }
            }
        }

        // Remove the H and L scores
        List<Score> flist = new List<Score>();
        foreach (Score score in scores)
        {
            if (score != scorelow && score != scorehigh)
                flist.Add(score);
        }

        return flist;
    }

    private int[] GetAdvanceCount(List<Score> scores)
    {
        int totaladv = 0;
        int totaladvyes = 0;

        foreach (Score score in scores)
        {
            if (score.IsAdvancement) totaladvyes++;
            totaladv++;
        }

        return new int[] { totaladv, totaladvyes };
    }

    #endregion
}