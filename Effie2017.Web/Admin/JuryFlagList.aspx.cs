using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Effie2017.App;
using System.Data;
using ClosedXML.Excel;
using System.IO;

public partial class Admin_JuryFlagList : PageSecurity_Admin
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
        GeneralFunction.ResetReportDataCache();

        GeneralFunction.GetAllEntryCache(true);
        GeneralFunction.GetAllScoreCache(true);
        GeneralFunction.GetAllJuryCache(true, round);
        GeneralFunction.GetAllJuryPanelCategoryCache(true);

        ViewState["TabFilterValue"] = "";
        // preload saved filters
        if (GeneralFunction.GetFilterPageId() == "JuryFlagList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            
            //ddlNetwork.SelectedValue = GeneralFunction.GetFilterF3();
            try { ddlNetwork.SelectedValue = GeneralFunction.GetFilterF3(); }
            catch { ddlNetwork.SelectedValue = ""; }

            //ddlHoldingCompany.SelectedValue = GeneralFunction.GetFilterF5();
            try { ddlHoldingCompany.SelectedValue = GeneralFunction.GetFilterF5(); }
            catch { ddlHoldingCompany.SelectedValue = ""; }

            ddlJuryFlag.SelectedValue = GeneralFunction.GetFilterF4();
            ddlCategory.SelectedValue = GeneralFunction.GetFilterF6();
            ddlCountry.SelectedValue = GeneralFunction.GetFilterF7();
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


        //Security.SecureControlByHiding(btnExport, "EXPORT");
        if (!Security.IsRoleAdmin() && !Security.IsRoleSuperAdmin())
        {
            btnExport.Visible = false;
        }
    }

    private void BindGrid(bool isCustomSort, string sortExpression, GridSortOrder order, bool needRebind)
    {
        ScoreList list = GeneralFunction.GetAllScoreCache(needRebind);

        // filter only flagged, not recused, submitted
        List<Score> slist = new List<Score>();
        foreach (Score score in list)
        {
            /* Commented because have to show recused entries also */

            //if (score.Flag != "" && score.IsSubmitted && !score.IsAdminRecuse && !score.IsRecuse && score.Round == round)
            //    slist.Add(score);

            if (((score.Flag != "" && score.IsSubmitted) || (score.IsRecuse)) && score.Round == round)
                slist.Add(score);
        }

        // filter
        string status = (string)ViewState["TabFilterValue"];
        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Score> flist = new List<Score>();

        if (advanceSearch == "1")
        {
            // Advanced search 
            // basic fields
            // user's country

            List<Guid> entryIdList = GeneralFunction.GetFilteredEntryListFromRegCountry(ddlCountry.SelectedValue, true);
            List<Guid> entryIdList2 = GeneralFunction.GetFilteredScoreListFromRegCompany(txtSearch.Text.Trim(), true);

            // juryname
            List<Guid> entryIdList4 = GeneralFunction.GetFilteredScoreListFromJuryAssignment(txtSearch.Text.Trim(), "", "", round, true);

            // juryid
            List<Guid> entryIdList5 = GeneralFunction.GetFilteredScoreListFromJuryAssignment("", txtSearch.Text.Trim(), "", round, true);

            // jurycompany
            List<Guid> entryIdList6 = GeneralFunction.GetFilteredScoreListFromJuryAssignment("", "", txtSearch.Text.Trim(), round, true);



            foreach (Score item in slist)
            {
                // category strip out the prefix
                string category = ddlCategory.SelectedValue;
                if (category.Length > 3) category = category.Substring(3, category.Length - 3);

                // Jury who scored
                EffieJuryManagementApp.Jury jury = GeneralFunction.GetJuryFromIDCache(item.Juryid, round);

                // Entry
                Entry entry = GeneralFunction.GetEntryFromIDCache(item.EntryId);

                if (
                    (ddlNetwork.SelectedValue == "" || (ddlNetwork.SelectedValue != "" && jury.Network == ddlNetwork.SelectedValue)) &&
                    (ddlHoldingCompany.SelectedValue == "" || (ddlHoldingCompany.SelectedValue != "" && jury.HoldingCompany == ddlHoldingCompany.SelectedValue)) &&

                    (ddlMarket.SelectedValue == "" || (ddlMarket.SelectedValue != "" && entry.CategoryMarket == ddlMarket.SelectedValue)) &&
                    (category == "" || (category != "" && (entry.CategoryPSDetailFromRound(round) == category || GeneralFunction.IsCategoryInCategoryGroup(category, entry.CategoryPSDetailFromRound(round))))) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(entry.Id))) &&
                    (
                    (ddlJuryFlag.SelectedValue == "" || (ddlJuryFlag.SelectedValue != "" && item.Flag == ddlJuryFlag.SelectedValue)) ||
                    (ddlJuryFlag.SelectedValue == "JuryRecusal" && item.IsRecuse)
                    ) &&
                    (ddlRecuse.SelectedValue == "" || (ddlRecuse.SelectedValue != "" && (item.IsAdminRecuse.ToString() == ddlRecuse.SelectedValue || item.IsRecuse.ToString() == ddlRecuse.SelectedValue))) &&
                    
                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entryId") && entry.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && entry.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entrant") && entryIdList2.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "juryname") && entryIdList4.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "juryId") && entryIdList5.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "jurycompany") && entryIdList6.Contains(item.Id)))

                    )
                   )
                {
                    flist.Add(item);
                }
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

        // Sort
        List<Entry> sortedEntries = GeneralFunction.GetAllEntryCache(false).ToList();
        var result = flist.Join(sortedEntries,score => score.EntryId,entry => entry.Id,(score, entry) => new { Score = score, Entry = entry }).OrderBy(x => x.Entry.Serial).Select(x => x.Score).ToList();

        #region CustomSorting

        if (isCustomSort)
        {
            List<Registration> regList = RegistrationList.GetRegistrationList(string.Empty, string.Empty, string.Empty).ToList();
            List<EffieJuryManagementApp.Jury> juryList = EffieJuryManagementApp.JuryList.GetJuryList(string.Empty, string.Empty).ToList();

            if (sortExpression.Equals("Serial"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.Serial select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.Serial descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Country"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Campaign"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.Campaign select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.Campaign descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("CategoryMarket"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.CategoryMarket select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.CategoryMarket descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Client"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.Client select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.Client descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Panel"))
            {
                List<Score> sortedfList = new List<Score>();
                List<Score> filteredfList = new List<Score>();
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
                                                      join score in flist on entry.Id equals score.EntryId
                                                      select score).ToList());
                            }

                            filteredfList.AddRange(sortedfList);

                            List<Score> preFilteredList = flist.Except(sortedfList).ToList();

                            preFilteredList = (from score in preFilteredList join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.CategoryPSDetail select score).ToList();

                            filteredfList.AddRange(preFilteredList);

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

                            List<Score> preFilteredList = flist.Except(sortedfList).ToList();

                            preFilteredList = (from score in preFilteredList join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.CategoryPSDetail descending select score).ToList();

                            filteredfList.AddRange(preFilteredList);

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
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Company select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Company descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Brand"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.Brand select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join entry in sortedEntries on score.EntryId equals entry.Id orderby entry.Brand descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("JuryID"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.SerialNo select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.SerialNo descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("JuryName"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.FirstName, jury.LastName select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.FirstName, jury.LastName descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("JuryTitle"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.Designation select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.Designation descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("JuryCompany"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.Company select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.Company descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("JuryCountry"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.Country select score).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from score in flist join jury in juryList on score.Juryid equals jury.Id orderby jury.Country descending select score).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from score in flist select score).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("JuryFlag"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Flags).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Flags).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Flags).ToList();
                        break;
                }
            }
        }
        else
            flist = (from score in flist select score).ToList();

        #endregion


        radGridEntry.DataSource = flist;
        if (needRebind) radGridEntry.Rebind();

        GeneralFunction.SetReportDataCache(result);


        // hide show checkboxes for certain status
        //if (status == StatusEntry.UploadPending || status == StatusEntry.UploadCompleted)
        //{
        //    foreach (GridDataItem item in radGridEntry.Items)
        //    {
        //        CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
        //        if (chkbox != null) chkbox.Visible = true;
        //        Security.SecureControlByHiding(chkbox);
        //    }
        //}
    }

    #region Events
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Score score = (Score)e.Item.DataItem;
            EffieJuryManagementApp.Jury jury = GeneralFunction.GetAllJuryCache(true, round).Where(x => x.Id == score.Juryid).FirstOrDefault();
            
            LinkButton lnkBtn, lnkBtn2 = null;
            Label lbl = null;
            HyperLink lnk = null;
            Entry entry = GeneralFunction.GetEntryFromIDCache(score.EntryId);
            if (entry != null)
            {
                // Entry Id
                lbl = (Label)e.Item.FindControl("lbSerial");
                lbl.Text = entry.Serial;


                // Title
                lbl = (Label)e.Item.FindControl("lbTitle");
                lbl.Text = entry.Campaign;

                lbl = (Label)e.Item.FindControl("lbClient");
                lbl.Text = entry.Client;

                // Categories
                lbl = (Label)e.Item.FindControl("lbCategory");
                //lbl.Text = GeneralFunction.GetEntryMarket(entry.CategoryMarket) + "<br/>" + entry.CategoryPSDetail;
                lbl.Text = entry.CategoryPSDetail;


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

            }

            if (jury != null)
            {

                List<JuryPanelCategory> jcpList = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);
                // Panel
                lbl = (Label)e.Item.FindControl("lbPanel");
                if (jcpList.Count > 0)
                    lbl.Text = jcpList[0].PanelId;

                // Jury
                lbl = (Label)e.Item.FindControl("lbJuryId");
                lbl.Text = jury.SerialNo;
                lbl = (Label)e.Item.FindControl("lbJuryName");
                lbl.Text = jury.FirstName + " " + jury.LastName;
                lbl = (Label)e.Item.FindControl("lbJuryTitle");
                lbl.Text = jury.Designation;
                lbl = (Label)e.Item.FindControl("lbJuryCompany");
                lbl.Text = jury.Company;
                //lbl = (Label)e.Item.FindControl("lbJuryNetwork");
                //lbl.Text = jury.Network;
                //if (jury.Network == "Others") lbl.Text += "-" + jury.NetworkOthers;
                //lbl = (Label)e.Item.FindControl("lbJuryHoldingCompany");
                //lbl.Text = jury.HoldingCompany;
                if (jury.HoldingCompany == "Others") lbl.Text += "-" + jury.HoldingCompanyOthers;
                lbl = (Label)e.Item.FindControl("lbJuryCountry");
                lbl.Text = jury.Country;
            }



            //// Flag
            lbl = (Label)e.Item.FindControl("lbJuryFlag");
            lbl.Text = score.IsRecuse ? "Jury Recusal" : score.Flag;
            lbl = (Label)e.Item.FindControl("lbJuryFlagReason");
            lbl.Text = score.FlagOthers;
            //lbl = (Label)e.Item.FindControl("lbJuryRecuseFlag");
            //lbl.Text = "Yes";
            //if (!score.IsRecuse) lbl.Text = "No"; 
            lbl = (Label)e.Item.FindControl("lbJuryRecuseReason");
            lbl.Text = score.RecuseRemarks;


            lbl = (Label)e.Item.FindControl("lbDateSubmitted");
            lbl.Text = "";
            if (score.DateSubmitted != DateTime.MinValue)
            {
                lbl.Text = score.DateSubmitted.ToString("dd/MM/yy");
            }
            
            //Action
            lnkBtn = (LinkButton)e.Item.FindControl("lnkScore");
            if (score != null)
            {
                lnkBtn.CommandArgument = score.Id.ToString();
                lnkBtn.Visible = true;
            }


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
        if (e.CommandName == "Score")
        {
            try
            {
                Score score = Score.GetScore(new Guid(e.CommandArgument.ToString()));
                EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(score.Juryid);

                GeneralFunction.SetRedirect("../Admin/JuryScoreList.aspx?r=" + round + "&juryId=" + jury.Id.ToString());  // to return from whereever
                Security.SetLoginSessionJury(jury);
                Response.Redirect("../Jury/EntryScore.aspx?eId=" + score.EntryId.ToString() + "&r=" + round + "&a=1");
            }
            catch { }
        }
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

        GeneralFunction.SetFilter("JuryFlagList", txtSearch.Text, ddlSearch.SelectedValue, ddlNetwork.SelectedValue, ddlJuryFlag.SelectedValue,
                          ddlHoldingCompany.SelectedValue, ddlCategory.SelectedValue, ddlCountry.SelectedValue, "");


    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlNetwork.SelectedValue = "";
        ddlHoldingCompany.SelectedValue = "";
        ddlMarket.SelectedValue = "";
        ddlCategory.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlRecuse.SelectedValue = "";
        ddlJuryFlag.SelectedValue = "";

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
            List<Score> flist = (List<Score>)data;

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Flag Report";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;


            // basic details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryId"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Panel"); x++;


            #endregion


            #region Jury Headers

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("JuryId"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Jury Name"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Network"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Holding Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Jury Flag"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Flag Reason"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date"); x++;

            #endregion
            
            y++;
            flist = flist.OrderByDescending(p => p.DateSubmitted).ToList();
            foreach (Score score in flist)
            {
                x = 1;
                Entry entry = GeneralFunction.GetEntryFromIDCache(score.EntryId);
                EffieJuryManagementApp.Jury jury = GeneralFunction.GetJuryFromIDCache(score.Juryid, round);
                List<JuryPanelCategory> jcpList = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);

                #region Basic Entry DataRows

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;

                // Basic Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Serial); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + " - " + entry.CategoryPSDetailFromRound(round)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.CategoryPSDetailFromRound(round)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Campaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Client); x++;

                Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
                if (reg != null)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;

                    string panel = "";
                    if (jcpList.Count > 0)
                        panel = jcpList[0].PanelId;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(panel); x++;
                }
                else
                    x = x + 3;


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


                // flag and remarks
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.IsRecuse ? "Jury Recusal" : score.Flag); x++;

                //string recuse = "No";
                //if (score.IsAdminRecuse) recuse = "Yes";
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(recuse); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.IsRecuse ? score.RecuseRemarks : score.FlagOthers); x++;

                string DateSubmitted = "";
                if (score.DateSubmitted != DateTime.MinValue)
                {
                    DateSubmitted = score.DateSubmitted.ToString("dd MMM yyyy");
                }

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(DateSubmitted); x++;

                #endregion

                y++;
            }



            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Flag_Report.xlsx");

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
    #endregion
}