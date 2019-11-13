using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using Telerik.Web.UI;
using System.Data;
using ClosedXML.Excel;
using System.IO;

public partial class Admin_JuryAssignList : PageSecurity_Admin
{
    string round;
    public static int PageSizeDefault = 50;

    protected void Page_Load(object sender, EventArgs e)
    {
        round = Request.QueryString["r"];

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
        GeneralFunction.GetAllJuryCache(true,round);
        GeneralFunction.GetAllJuryPanelCategoryCache(true);


        ViewState["TabFilterValue"] = "";
        // preload saved filters
        if (GeneralFunction.GetFilterPageId() == "JuryAssignList")
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
        ltRound.Text = "Round " + round;

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
            btnExport.Visible = false;
        }

        for (int i = 16; i <= 20; i++)
        {
            radGridEntry.MasterTableView.GetColumn("Judge" + i).Visible = false;
        }
    }
    private void BindGrid(bool isCustomSort, string sortExpression, GridSortOrder order, bool needRebind)
    {
        EntryList list = GeneralFunction.GetAllEntryCache(false);

        // filter only completed
        List<Entry> slist = new List<Entry>();
        foreach (Entry entry in list)
        {
            if (entry.Status == StatusEntry.Completed && entry.WithdrawnStatus == "" && ((round == "1") || (round == "2" && entry.IsRound2)))
                slist.Add(entry);
        }

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
            List<Guid> entryIdList3 = GeneralFunction.GetFilteredEntryListFromClientCC(txtSearch.Text.Trim(), true);
            List<Guid> entryIdList4 = GeneralFunction.GetFilteredEntryListFromJuryAssignment(txtSearch.Text.Trim(), round, true);
            List<Guid> entryIdList6 = GeneralFunction.GetFilteredEntryListFromAgencyCC(txtSearch.Text.Trim(), true);

            // jury panel list
            List<Guid> entryIdList5 = GeneralFunction.GetFilteredEntryListFromJuryPanel(ddlPanel.SelectedValue, round, true);

            foreach (Entry item in slist)
            {
                // category strip out the prefix
                string category = ddlCategory.SelectedValue;
                if (category.Length > 3) category = category.Substring(3, category.Length - 3);

                if (
                    (ddlMarket.SelectedValue == "" || (ddlMarket.SelectedValue != "" && item.CategoryMarket == ddlMarket.SelectedValue)) &&
                    (category == "" || (category != "" && (item.CategoryPSDetailFromRound(round) == category || GeneralFunction.IsCategoryInCategoryGroup(category, item.CategoryPSDetailFromRound(round))))) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&
                    (ddlPanel.SelectedValue == "" || (ddlPanel.SelectedValue != "" && entryIdList5.Contains(item.Id))) &&


                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entryId") && item.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entrant") && entryIdList2.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "client") && entryIdList3.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "agency") && entryIdList6.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "juryname") && entryIdList4.Contains(item.Id))) 
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
            //foreach (Entry item in list)
            //    if (item.WithdrawnStatus != "") flist.Add(item);
            //}
            //else
            //{
            //    foreach (Entry item in list)
            //        if (status == "" || (status != "" && item.Status == status)) flist.Add(item);
            //}

            flist = slist.ToList();
        }

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
        }
        else
        {
            flist = (from entry in flist orderby entry.Serial select entry).ToList();
            //flist = (from entry in flist orderby entry.PanelId, entry.CategoryMarket, entry.CategoryPSDetail, entry.Serial select entry).ToList();
        }

        #endregion

        radGridEntry.DataSource = flist;
        if (needRebind) radGridEntry.Rebind();

        GeneralFunction.SetReportDataCache(flist);
    }

    #region Events
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.Entry entry = (Effie2017.App.Entry)e.Item.DataItem;
            

            LinkButton lnkBtn, lnkBtn2 = null;
            Label lbl = null;
            HyperLink lnk = null;


            // Categories
            lbl = (Label)e.Item.FindControl("lbCategory");//GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + "<br/>" + 
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

            // Country
            lbl = (Label)e.Item.FindControl("lbCountry");
            lbl.Text = "-";
            if (reg != null)
            {
                lbl.Text = reg.Country;
            }

            // client and agency
            CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(entry.Id);
            try
            {
                lbl = (Label)e.Item.FindControl("lbClient");
                lbl.Text = cclist[0].Company;

                lbl = (Label)e.Item.FindControl("lbAgency1");
                lbl.Text = cclist[1].Company;

                lbl = (Label)e.Item.FindControl("lbAgency2");
                lbl.Text = cclist[2].Company;

                lbl = (Label)e.Item.FindControl("lbAgency3");
                lbl.Text = cclist[3].Company;

                lbl = (Label)e.Item.FindControl("lbAgency4");
                lbl.Text = cclist[4].Company;

                lbl = (Label)e.Item.FindControl("lbAgency5");
                lbl.Text = cclist[5].Company;

            }
            catch { }


            //// submitted details
            //lbl = (Label)e.Item.FindControl("lblSubmittedDetails");
            //lbl.Text = reg.Firstname + " " + reg.Lastname + "<br/>" + reg.Job + "<br/>" + GeneralFunction.ShowFriendlyContact(reg.Contact) + "<br/>" + reg.Email;


            // Jury panel 1
            List<JuryPanelCategory> jcpList = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);
            if (jcpList != null && jcpList.Count > 0)
            {
                lbl = (Label)e.Item.FindControl("lbJuryPanel");
                lbl.Text = jcpList[0].PanelId;
            }


            // Jury panel round2



            // Jury
            List<EffieJuryManagementApp.Jury> jlist = GeneralFunction.GetJuryListFromEntryCategory(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);

            DisplayJury(e, "lbJury1", jlist, 0);
            DisplayJury(e, "lbJury2", jlist, 1);
            DisplayJury(e, "lbJury3", jlist, 2);
            DisplayJury(e, "lbJury4", jlist, 3);
            DisplayJury(e, "lbJury5", jlist, 4);
            DisplayJury(e, "lbJury6", jlist, 5);
            DisplayJury(e, "lbJury7", jlist, 6);
            DisplayJury(e, "lbJury8", jlist, 7);
            DisplayJury(e, "lbJury9", jlist, 8);
            DisplayJury(e, "lbJury10", jlist, 9);
            DisplayJury(e, "lbJury11", jlist, 10);
            DisplayJury(e, "lbJury12", jlist, 11);
            DisplayJury(e, "lbJury13", jlist, 12);
            DisplayJury(e, "lbJury14", jlist, 13);
            DisplayJury(e, "lbJury15", jlist, 14);
            DisplayJury(e, "lbJury16", jlist, 15);
            DisplayJury(e, "lbJury17", jlist, 16);
            DisplayJury(e, "lbJury18", jlist, 17);
            DisplayJury(e, "lbJury19", jlist, 18);
            DisplayJury(e, "lbJury20", jlist, 19);

            //// chkboxes
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

        GeneralFunction.SetFilter("JuryAssignList", txtSearch.Text, ddlSearch.SelectedValue, ddlPanel.SelectedValue, ddlCategory.SelectedValue,
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
    protected void btnExport_Click(object sender, EventArgs e)
    {
        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Entry> flist = (List<Entry>)data;

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Jury Assignment";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;


            // Entry headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CategoryPS"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;


            // agency headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency3"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency4"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency5"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency6"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency7"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency8"); x++;
            

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Round 1 Jury Panel"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Round 2 Jury Panel"); x++;
            
            // jude headers
            for (int i = 1; i <= 20; i++)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Judge" + i.ToString()); x++;
            }

            #endregion


            y++;

            foreach (Entry ent in flist)
            {
                x = 1;

                Registration reg = GeneralFunction.GetRegistrationFromEntry(ent);
                CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(ent.Id);
                List<JuryPanelCategory> jcpList = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(ent.CategoryMarketFromRound(round), ent.CategoryPSFromRound(round), ent.CategoryPSDetailFromRound(round), round);
                List<EffieJuryManagementApp.Jury> jlist = GeneralFunction.GetJuryListFromEntryCategory(ent.CategoryMarketFromRound(round), ent.CategoryPSFromRound(round), ent.CategoryPSDetailFromRound(round), round);


               

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;

                // Entry Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryPSDetailFromRound(round)); x++; //GeneralFunction.GetEntryMarket(ent.CategoryMarketFromRound(round)) + " - " + 
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Campaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[0].Company); x++; } catch { x++; }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Brand); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;


                // Agency Details
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[1].Company); x++; } catch { x++; }
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[2].Company); x++; } catch { x++; }
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[3].Company); x++; } catch { x++; }
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[4].Company); x++; } catch { x++; }
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[5].Company); x++; } catch { x++; }
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[6].Company); x++; } catch { x++; }
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[7].Company); x++; } catch { x++; }
                try { workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[8].Company); x++; } catch { x++; }

                // Jury Panel 1
                if (jcpList != null && jcpList.Count > 0)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jcpList[0].PanelId); x++;
                }

                // Jury Panel 2
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(""); x++;


                // Jury 1 to 20
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        if (!GeneralFunction.IsRecuse(ent, jlist[i].Id, round))
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jlist[i].FirstName + " " + jlist[i].LastName + " / " + jlist[i].Company);
                        x++;
                    }
                    catch { x++; }
                }

                y++;
            }



            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Jury_Assignment.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }
    #endregion

    #region Helper
    private ListItem SeparatorItem()
    {
        ListItem separator = new ListItem("-------------------------------", "");
        separator.Attributes.Add("disabled", "disabled");

        return separator;
    }
    private void DisplayJury(GridItemEventArgs e, string label, List<EffieJuryManagementApp.Jury> jlist, int index)
    {
        try
        {
            if (!GeneralFunction.IsRecuse((Entry)e.Item.DataItem, jlist[index].Id, round))
            {
                HyperLink lbl = (HyperLink)e.Item.FindControl(label);
                lbl.Text = jlist[index].FirstName + " " + jlist[index].LastName + " / " + jlist[index].Company;
                lbl.NavigateUrl = "JuryEntryList.aspx?r=" + round + "&juryId=" + jlist[index].Id.ToString();
            }
        }
        catch { }
    }

    #endregion
}