using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Effie2017.App;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;

public partial class Admin_JuryList : PageSecurity_Admin
{
    int counter;
    public static string round = "";
    public static int PageSizeDefault = 50;

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(Request.QueryString["r"]))
            round = Request.QueryString["r"];

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }

        string parameter = Request["__EVENTARGUMENT"];
        if (parameter == "inviteEmail")
            btnSend_Click(sender, e);
    }

    private void PopulateForm()
    {
        // Refresh the cache
        GeneralFunction.ResetReportDataCache();
        GeneralFunction.GetAllJuryCache(true,round);
        GeneralFunction.GetAllJuryPanelCategoryCache(true);

        ViewState["TabFilterValue"] = "";
        // preload saved filters
        if (GeneralFunction.GetFilterPageId() == "JuryList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            ddlJuryPanel1.SelectedValue = GeneralFunction.GetFilterF4();

            //ddlNetwork.SelectedValue = GeneralFunction.GetFilterF3();
            //ddlHoldingCompany.SelectedValue = GeneralFunction.GetFilterF5();
            
            try { ddlNetwork.SelectedValue = GeneralFunction.GetFilterF3(); }
            catch { ddlNetwork.SelectedValue = ""; }

            try { ddlHoldingCompany.SelectedValue = GeneralFunction.GetFilterF5(); }
            catch { ddlHoldingCompany.SelectedValue = ""; }

            ddlJuryPanel2.SelectedValue = GeneralFunction.GetFilterF6();
            ddlCountry.SelectedValue = GeneralFunction.GetFilterF7();
            ddlAssignedRound.SelectedValue = GeneralFunction.GetFilterF8();
            ViewState["AdvanceSearch"] = "1";
        }

        BindGrid(true);
        PopulateLastReminderDate();
        btnRemind.Visible = !Security.IsReadOnlyAdmin();
        lbLastReminderDate.Visible = !Security.IsReadOnlyAdmin();
    }

    private void LoadForm()
    {
        ltRound.Text = round;

        // Panel1
        ddlJuryPanel1.DataSource = GeneralFunction.GetJuryPanelList("1");
        ddlJuryPanel1.DataBind();
        ddlJuryPanel1.Items.Insert(0, new ListItem("All", ""));

        // Panel2
        ddlJuryPanel2.DataSource = GeneralFunction.GetJuryPanelList("2");
        ddlJuryPanel2.DataBind();
        ddlJuryPanel2.Items.Insert(0, new ListItem("All", ""));


        List<string> effieExpYears = GeneralFunction.GetEffieExperienceYears();
        ddlYear.Items.Insert(0, new ListItem("All", ""));
        foreach (var year in effieExpYears)
        {
            ddlYear.Items.Add(year);
        }
        ddlYear.DataBind();

        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryListFromJury(false,round);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));

        // Network
        GeneralFunction.LoadddlNetwork(ddlNetwork, false, false);
        ddlNetwork.Items.Insert(0, new ListItem("All", ""));

        // Holding Company
        GeneralFunction.LoadddlHoldingCompany(ddlHoldingCompany, false, false);
        ddlHoldingCompany.Items.Insert(0, new ListItem("All", ""));

        if (!Security.IsRoleAdmin() && !Security.IsRoleSuperAdmin())
            btnExport.Visible = false;

            Security.SecureControlByHiding(btnNew, "AddJudge");
    }

    protected bool isJuryInvited(EffieJuryManagementApp.Jury jury, string Year)
    {
        EffieJuryManagementApp.InvitationList invitations = EffieJuryManagementApp.InvitationList.GetInvitationList(jury.Id, Year);
        if(invitations.Any())
            return true;
        return false;
    }
    private void BindGrid(bool needRebind)
    {
        // && ((round == "1" && x.IsRound1) || round == "2" && x.IsRound2)
        List<EffieJuryManagementApp.Jury> list = GeneralFunction.GetAllJuryCache(true, round);
        
        if (string.IsNullOrEmpty(Request.QueryString["r"]))
        {
            List<EffieJuryManagementApp.Invitation> invList = EffieJuryManagementApp.InvitationList.GetInvitationList(Guid.Empty, 
                       EffieJuryManagementApp.Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value).ToList();

            list = (from jury in list
                    join inv in invList
                    on jury.Id equals inv.JuryId
                    where (inv.IsRound1Accepted || inv.IsRound2Accepted)
                    orderby jury.SerialNo
                    select jury).ToList();
        }

        // .Where(x => x.IsActive).ToList(); // && ((round == "1" && x.IsRound1) || round == "2" && x.IsRound2)


        // filter off the draft and ready
        //List<Entry> slist = new List<Entry>();
        //foreach (Entry entry in list)
        //{
        //    if (entry.Status != StatusEntry.Draft && entry.Status != StatusEntry.Ready)
        //        slist.Add(entry);
        //}

        // filter
        string status = (string)ViewState["TabFilterValue"];
        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<EffieJuryManagementApp.Jury> flist = new List<EffieJuryManagementApp.Jury>();

        if (advanceSearch == "1")
        {
            // Advanced search 
            // basic fields
            //List<Guid> entryIdList = GeneralFunction.GetFilteredEntryListFromRegCountry(ddlCountry.SelectedValue, true);

            foreach (EffieJuryManagementApp.Jury item in list)
            {
                if (
                    (ddlJuryPanel1.SelectedValue == "" || (ddlJuryPanel1.SelectedValue != "" && GeneralFunction.IsInList(ddlJuryPanel1.SelectedValue, item.Round1PanelId, '|'))) &&
                    (ddlJuryPanel2.SelectedValue == "" || (ddlJuryPanel2.SelectedValue != "" && GeneralFunction.IsInList(ddlJuryPanel2.SelectedValue, item.Round2PanelId, '|'))) &&
                    (ddlNetwork.SelectedValue == "" || (ddlNetwork.SelectedValue != "" && item.Network == ddlNetwork.SelectedValue)) &&
                    (ddlHoldingCompany.SelectedValue == "" || (ddlHoldingCompany.SelectedValue != "" && item.HoldingCompany == ddlHoldingCompany.SelectedValue)) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && item.Country == ddlCountry.SelectedValue)) &&

                    (ddlAssignedRound.SelectedValue == "" || (ddlAssignedRound.SelectedValue == "1" && item.Round1PanelId != "") || (ddlAssignedRound.SelectedValue == "2" && item.Round2PanelId != "")) &&

                    (ddlType.SelectedValue == "" || item.Type == ddlType.SelectedValue) &&
                    (ddlYear.SelectedValue == "" || isJuryInvited(item, ddlYear.SelectedValue)) && 
                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "judgeId") && item.SerialNo.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "name") && (item.FirstName.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1 || item.LastName.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Designation.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && item.Company.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))
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

            flist = list.ToList();
        }


        // Sort
        flist = flist.OrderBy(p => p.SerialNo).ToList();
        
        radGridEntry.DataSource = flist;
        if (needRebind) radGridEntry.Rebind();

        GeneralFunction.SetReportDataCache(flist);


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

    private void PopulateLastReminderDate()
    {
        // last reminder date
        SystemData data = GeneralFunction.GetSystemData();
        lbLastReminderDate.Text = "&nbsp;Last sent: " + GeneralFunction.CleanDateTimeToString(data.DateLastReminded, "dd MMM yyyy HH:mm tt");
    }

    #region Events
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            EffieJuryManagementApp.Jury jury = (EffieJuryManagementApp.Jury)e.Item.DataItem;
            
            LinkButton lnkBtn = null;
            Label lbl = null;
            HyperLink lnk = null;

            // Jury Id
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnJuryId");
            lnkBtn.Text = jury.SerialNo;
            lnkBtn.CommandArgument = jury.Id.ToString();


            // Jury Name
            lnk = (HyperLink)e.Item.FindControl("lnkJuryName");
            lnk.Text = jury.FirstName + " " + jury.LastName;
            lnk.NavigateUrl = "mailto:" + jury.Email;


            // Round 1 Panel
            lbl = (Label)e.Item.FindControl("lbJuryPanelRound1");
            string round1PanelId = GeneralFunction.CleanDelimiterList(jury.Round1PanelId, '|', ", ");

            if (String.IsNullOrEmpty(round1PanelId))
                lbl.Text = "-";
            else
                lbl.Text = round1PanelId;


            //// Round 1
            //lbl = (Label)e.Item.FindControl("lbRound1");
            //lbl.Text = "N";
            //if (jury.Round1PanelId != "") lbl.Text = "Y";


            // Round 2 Panel
            lbl = (Label)e.Item.FindControl("lbJuryPanelRound2");            
            string round2PanelId = GeneralFunction.CleanDelimiterList(jury.Round2PanelId, '|', ", ");

            if (String.IsNullOrEmpty(round2PanelId))
                lbl.Text = "-";
            else
                lbl.Text = round2PanelId;

            //// Round 2
            //lbl = (Label)e.Item.FindControl("lbRound2");
            //lbl.Text = "N";
            //if (jury.Round2PanelId != "") lbl.Text = "Y";


            // Categories R1
            lbl = (Label)e.Item.FindControl("lbCategory");
            List<JuryPanelCategory> jcplist = GeneralFunction.GetJuryPanelCategoryFromPanelId(jury.Round1PanelId, "1").Where(m => !String.IsNullOrEmpty(m.CategoryPSDetail)).ToList();
            if (!String.IsNullOrEmpty(round1PanelId))
            {                
                foreach (JuryPanelCategory jcp in jcplist)
                {
                    string market = jcp.CategoryPSDetail.Substring(0, 2);
                    string categoryname = jcp.CategoryPSDetail.Substring(3, jcp.CategoryPSDetail.Length - 3);
                    lbl.Text += Category.GetCategoryCode(market, categoryname) + "<br/>";// +parts[1] + "<br/>";
                }
            }

            // Categories R2
            lbl = (Label)e.Item.FindControl("lbCategory2");
            List<JuryPanelCategory> jcplist2 = GeneralFunction.GetJuryPanelCategoryFromPanelId(jury.Round2PanelId, "2").Where(m => !String.IsNullOrEmpty(m.CategoryPSDetail)).ToList();
            if (!String.IsNullOrEmpty(round2PanelId))
            {                
                foreach (JuryPanelCategory jcp in jcplist2)
                {
                    string market = jcp.CategoryPSDetail.Substring(0, 2);
                    string categoryname = jcp.CategoryPSDetail.Substring(3, jcp.CategoryPSDetail.Length - 3);
                    lbl.Text += Category.GetCategoryCode(market, categoryname) + "<br/>";// +parts[1] + "<br/>";
                }
            }


            // Scoring Completion
            // get all his scoring
            lbl = (Label)e.Item.FindControl("lbScoringCompletion");
            List<Score> scores = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
            List<Score> fscores = scores.Where(p => p.IsSubmitted && !p.IsAdminRecuse && !p.IsRecuse).ToList();
            //List<Score> ascores = GeneralFunction.FilterScoreListFromActivePanels(fscores, round);

            //List<Entry> fentries = GeneralFunction.GetEntryListValidFromJuryPanel(jury, round);
            //List<Entry> aentries = GeneralFunction.FilterEntryListFromActivePanels(fentries, round);

            //lbl.Text = fscores.Count.ToString() + " / " + fentries.Count.ToString();
            lbl.Text = fscores.Count.ToString() + " / " + GeneralFunction.GetEntryCountValidFromJury(jury, round.Equals("1") ? jcplist : jcplist2, scores, round).ToString();

            // Scoring Pending
            // get all his scoring
            lbl = (Label)e.Item.FindControl("lbScoringPending");
            //List<Score> scoresPending = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
            //List<Score> fscoresPending = scores.Where(p => !p.IsSubmitted && !p.IsAdminRecuse && !p.IsRecuse).ToList();

            lbl.Text = GeneralFunction.GetPendingEntryCountValidFromJury(jury, round.Equals("1") ? jcplist : jcplist2, scores, round).ToString();



            // chkboxes
            CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            Security.SecureControlByHiding(chkbox);


            lnk = (HyperLink)e.Item.FindControl("hlkEmailHistory");
            lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
            lnk.NavigateUrl = "./JuryEmailSentHistory.aspx?JuryId=" + jury.Id.ToString();


            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            lnkBtn.Visible = !Security.IsReadOnlyAdmin();
            lnkBtn = (LinkButton)e.Item.FindControl("lnkAssign");
            lnkBtn.Visible = !Security.IsReadOnlyAdmin();


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

        if (e.CommandName == "Edit" || e.CommandName == "Assign")
        {
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Admin/Jury.aspx?r=" + round + "&juryId=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "ViewScores")
        {
            //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Admin/JuryScoreList.aspx?r=" + round + "&juryId=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "View")
        {
            //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Admin/JuryEntryList.aspx?r=" + round + "&juryId=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "ViewJury")
        {
            //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Admin/Jury.aspx?r=" + round + "&juryId=" + ((GridDataItem)e.Item)["Id"].Text + "&v=1");
        }
    }
    protected void radGridEntry_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
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

        ViewState["AdvanceSearch"] = "1";
        BindGrid(true);
        lblError.Text = "";

        GeneralFunction.SetFilter("JuryList", txtSearch.Text, ddlSearch.SelectedValue, ddlNetwork.SelectedValue, ddlJuryPanel1.SelectedValue,
                                  ddlHoldingCompany.SelectedValue, ddlJuryPanel2.SelectedValue, ddlCountry.SelectedValue, ddlAssignedRound.SelectedValue);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlNetwork.SelectedValue = "";
        ddlHoldingCompany.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlJuryPanel1.SelectedValue = "";
        ddlJuryPanel2.SelectedValue = "";
        ddlAssignedRound.SelectedValue = "";
        //rtabEntry.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        //rtabEntry.SelectedIndex = 0;
        BindGrid(true);
        lblError.Text = "";

        GeneralFunction.ResetFilter();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Jury.aspx?r=" + round);
    }

    protected void btnEditPanel_Click(object sender, EventArgs e)
    {
        string ids = "";
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");

            if (chkbox.Checked)
            {
                ids += item["Id"].Text + "|";
            }
        }

        if (ids != "")
        {
            Session["idsForPanel"] = ids;
            Response.Redirect("JuryPanel.aspx?r=" + round);
        }
        else
            lblError.Text = "Please select at least one jury";
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<string> effieExpYears = GeneralFunction.GetEffieExperienceYears();

            List<EffieJuryManagementApp.Jury> flist = (List<EffieJuryManagementApp.Jury>)data;

            flist = flist.OrderBy(m => round.Equals("1") ? m.Round1PanelId : m.Round2PanelId).ToList();
            
            List<EffieJuryManagementApp.Jury> filteredfList = new List<EffieJuryManagementApp.Jury>();
            
            List<JuryPanelCategory> jcplist = JuryPanelCategoryList.GetJuryPanelCategoryList(string.Empty, string.Empty).OrderBy(m => m.PanelId).OrderBy(m => m.CategoryPSDetail).ToList();

            filteredfList.AddRange((from jury in flist
                                    from juryCat in jcplist
                                    where round.Equals("1") ? jury.Round1PanelId.Contains(juryCat.PanelId) : jury.Round2PanelId.Contains(juryCat.PanelId)
                                    orderby juryCat.PanelId, juryCat.CategoryPSDetail                                     
                                    select jury).ToList());

            filteredfList = filteredfList.Distinct().ToList();


            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Jury Listing";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;


            // basic details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Type"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("JudgeId"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Password"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Salutation"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Contact"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Mobile"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("City"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Postal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;

            ////Based on feedback from Charmaine on 08/02/2018

            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Type of Company"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Network"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Holding Company"); x++;

            #endregion

            ////Based on feedback from Charmaine on 08/02/2018

            //#region Market Exp Headers

            //List<string> marketExpItems = GeneralFunction.GetMarketExperienceItems();
            //foreach (string marketExp in marketExpItems)
            //{
            //    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(marketExp); x++;
            //}
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Others"); x++;

            //#endregion


            ////Based on feedback from Charmaine on 08/02/2018

            //#region Industry Exp Headers

            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AG"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AU"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("BW"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("BA"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CE"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CR"); x++;

            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FP"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FM"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FD"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("GV"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HC"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HS"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IT"); x++;

            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ME"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RE"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RS"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RT"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TT"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Others"); x++;

            //#endregion


            #region Misc Headers

            ////Based on feedback from Charmaine on 08/02/2018

            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Specialist Skills"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Specialist Skills - Others"); x++;

            //foreach (string effieExpYear in effieExpYears)
            //{
            //    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("APAC Effie Exp - " + effieExpYear); x++;
            //    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("APAC Effie Exp Remarks - " + effieExpYear); x++;
            //}

            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Effie Programs"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Effie Programs - Others"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Round1 Panel"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Round2 Panel"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Completion"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Account Activated"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Accepted Confidentiality Statement"); x++;

            #endregion

            y++;

            foreach (EffieJuryManagementApp.Jury jury in filteredfList)
            {
                x = 1;


                #region Basic Jury DataRows

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;

               
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Type); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SerialNo); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Email); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Password); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.FirstName); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.LastName); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Designation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(jury.Contact)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(jury.Mobile)); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Company); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Address1); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Address2); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.City); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Postal); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Country); x++;

                ////Based on feedback from Charmaine on 08/02/2018

                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.CompanyType); x++;
                //string network = jury.Network;
                //if (jury.NetworkOthers != "") network += " - " + jury.NetworkOthers;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(network); x++;
                //string holdingcompany = jury.HoldingCompany;
                //if (jury.HoldingCompanyOthers != "") holdingcompany += " - " + jury.HoldingCompanyOthers;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(holdingcompany); x++;


                #endregion

                ////Based on feedback from Charmaine on 08/02/2018

                //#region Market Exp

                //foreach (string marketExp in marketExpItems)
                //{
                //    if (jury.MarketExp.IndexOf(marketExp) != -1)
                //        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //    x++;
                //}
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.MarketExpOthers); x++;

                //#endregion

                ////Based on feedback from Charmaine on 08/02/2018

                //#region Industry Exp

                //if (jury.IndustryExp.IndexOf("Agricultural & Industrial") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Automotive") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Beauty & Wellness") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Beverages") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Consumer Electronics and Durables") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Corporate Reputation/Professional Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Financial Products & Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("FMCG") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Food") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Government / Institutional") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Healthcare") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Household Supplies & Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("IT /Telco") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Media, Entertainment & Leisure") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Real Estate") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Restaurants") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Retail") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;
                //if (jury.IndustryExp.IndexOf("Travel / Tourism") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                //x++;

                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.IndustryExpOthers); x++;

                //#endregion


                #region Misc

                ////Based on feedback from Charmaine on 08/02/2018

                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Skills); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SkillsOthers); x++;

                //int effieExpCounter = 0;
                //foreach (string effieExpYear in effieExpYears)
                //{
                //    string yearString = string.Empty;
                //    string year = string.Empty;
                //    string yearRemark = string.Empty;

                //    try
                //    {
                //        yearString = jury.EffieExpYear.Split('|')[effieExpCounter];
                //        year = yearString.Split('#')[0];
                //        yearRemark = yearString.Split('#')[1];
                //    }
                //    catch { }

                //    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(effieExpYear.Equals(year) ? "Yes" : "No"); x++;
                //    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(yearRemark); x++;

                //    effieExpCounter++;
                //}

                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.EffieExpProgram); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.EffieExpProgramOthers); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Round1PanelId); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Round2PanelId); x++;


                // Completion
                List<Score> scores = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
                List<Score> fscores = scores.Where(p => p.IsSubmitted && !p.IsAdminRecuse && !p.IsRecuse).ToList();
                List<Entry> fentries = GeneralFunction.GetEntryListValidFromJuryPanel(jury, round);

                string completion = fscores.Count.ToString() + " / " + fentries.Count.ToString();
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(completion); x++;

                #endregion

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.IsActive ? "Yes" : "No"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(!jury.IsFirstTimeLogin ? "Yes" : "No"); x++;

                y++;
            }



            GeneralFunction.StyleReport(workbook).SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Jury_Master.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        lblTempError.Text = string.Empty;

        lblTempError.Text += IptechLib.Validation.ValidateDropDownList("Select Rounds", ddlRounds, roundRow.Visible, string.Empty.ToString());
        lblTempError.Text += IptechLib.Validation.ValidateDropDownList("Select Template", ddlTemplateList, templateRow.Visible, Guid.Empty.ToString());

        if (String.IsNullOrEmpty(lblTempError.Text))
        {
            SendInvitationCriteria invCriteria = new SendInvitationCriteria();

            if (roundRow.Visible)
            {
                invCriteria.isRound1 = ddlRounds.SelectedValue.Equals(RoundsType.Round1) || ddlRounds.SelectedValue.Equals(RoundsType.BothRounds);
                invCriteria.isRound2 = ddlRounds.SelectedValue.Equals(RoundsType.Round2) || ddlRounds.SelectedValue.Equals(RoundsType.BothRounds);
                invCriteria.isSend = false;

                GenerateInvitation(invCriteria, new Guid(ddlTemplateList.SelectedValue));
            }
            else
            {
                EmailTemplate choosenTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));

                if (choosenTemplate != null)
                {
                    invCriteria.isRound1 = choosenTemplate.UserData1.Equals(RoundsType.Round1) || choosenTemplate.UserData1.Equals(RoundsType.BothRounds);
                    invCriteria.isRound2 = choosenTemplate.UserData1.Equals(RoundsType.Round2) || choosenTemplate.UserData1.Equals(RoundsType.BothRounds);
                    invCriteria.isSend = true;

                    if (choosenTemplate.IsInvitation)
                        GenerateInvitation(invCriteria, new Guid(ddlTemplateList.SelectedValue));
                    else
                        GenerateEmails(new Guid(ddlTemplateList.SelectedValue));
                }
            }

            phSelectTemplate.Visible = false;

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "none", "<script>ClearDataOnLoad();</script>", false);
        }
    }


    public void GenerateInvitation(SendInvitationCriteria invCriteria, Guid templateId)
    {
        string evetnYear = string.Empty;
        try
        {
            evetnYear = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value;
        }
        catch { }

        lblError.Text = string.Empty;
        int counter = 0;
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

            if (chkbox.Checked)
            {
                EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(new Guid(hdfId.Value.ToString()));

                EffieJuryManagementApp.InvitationList invList = EffieJuryManagementApp.InvitationList.GetInvitationList(jury.Id, Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value);

                EffieJuryManagementApp.Invitation inv = null;

                if (invList.Count > 0)
                {
                    inv = invList[0];
                }
                else
                {
                    inv = EffieJuryManagementApp.Invitation.NewInvitation();
                }

                inv.EventCode = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value.ToString();
                inv.JuryId = jury.Id;

                if (invCriteria.isRound1)
                {
                    inv.IsRound1Invited = true;
                    inv.DateRound1EmailSentString = DateTime.Now.ToString();
                }
                if (invCriteria.isRound2)
                {
                    inv.IsRound2Invited = true;
                    inv.DateRound2EmailSentString = DateTime.Now.ToString();
                }

                if (inv.IsNew)
                    inv.DateCreatedString = DateTime.Now.ToString();

                inv.DateModifiedString = DateTime.Now.ToString();

                if (invCriteria.isSend)
                {
                    Email.SendInvitationTemplateEmail(inv, templateId);
                    GeneralFunction.SaveEmailSentLogJury(jury, templateId, evetnYear);
                }
                else
                    inv.IsLocked = true;

                if (inv.IsValid)
                    inv.Save();

                chkbox.Checked = false;
                counter++;
            }
        }

        if (counter == 0)
            lblError.Text = "Please select at least one jury to send Invitation.<br/>";
        else
        {
            if (invCriteria.isSend)
                lblError.Text = "Email sent to " + (counter).ToString() + " Jury(s).<br/>";
            else
                lblError.Text = "Invitation added for " + (counter).ToString() + " Jury(s).<br/>";
        }
    }

    public void GenerateEmails(Guid templateId)
    {
        string evetnYear = string.Empty;
        try
        {
            evetnYear = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value;
        }
        catch { }

        lblError.Text = string.Empty;
        int counter = 0;

        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

            if (chkbox.Checked)
            {
                EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(new Guid(hdfId.Value.ToString()));
                Email.SendTemplateEmailJury(jury, templateId);
                GeneralFunction.SaveEmailSentLogJury(jury, templateId, evetnYear);

                chkbox.Checked = false;
                counter++;
            }
        }

        if (counter == 0)
            lblError.Text = "Please select at least one jury to send email.<br/>";
        else
        {
            phSelectTemplate.Visible = false;
            lblError.Text = "Email sent to " + (counter).ToString() + " Jury(s).<br/>";
        }
    }

    protected void ddlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString()))
        {
            hlkPreview.CssClass = "fancybox fancybox.iframe tblLinkRed";
            hlkPreview.NavigateUrl = "./EmailPreview.aspx?etmId=" + ddlTemplateList.SelectedValue.ToString();
        }

        hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = false;
    }

    public void PopulateTemplatePanel(Button pressedButton)
    {
        lblTempError.Text = string.Empty;
        lblError.Text = string.Empty;

        IptechLib.Forms.RemoveHighlightControls(phSelectTemplate);

        ddlRounds.Items.Clear();
        ddlRounds.Items.Add(new ListItem("Round 1", RoundsType.Round1));
        ddlRounds.Items.Add(new ListItem("Round 2", RoundsType.Round2));
        ddlRounds.Items.Add(new ListItem("Round 1 & 2", RoundsType.BothRounds));

        if (!pressedButton.CommandName.Equals("add"))
            ddlRounds.Items.Add(new ListItem("N/A", RoundsType.NotApplicable));

        ddlRounds.Items.Insert(0, new ListItem("Please Select", string.Empty.ToString()));

        List<EmailTemplate> defaultEmailTempalteList = EmailTemplateList.GetEmailTemplateList(Guid.Empty).Where(m => m.TemplateId != new Guid(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultTemplateId")[0].Value)
            && m.IsActive && m.UserData2.Equals(EmailCategory.Invitation) && !m.IsDelete).ToList();

        if (defaultEmailTempalteList.Count == 0)
            lblError.Text = "No Email Template found.<br/>";

        roundRow.Visible = pressedButton.CommandName.Equals("add");
        templateRow.Visible = !pressedButton.CommandName.Equals("add");

        phSelectTemplate.Visible = (defaultEmailTempalteList.Count != 0);

        if (defaultEmailTempalteList.Count != 0)
        {
            ddlTemplateList.DataSource = defaultEmailTempalteList;
            ddlTemplateList.DataTextField = "Title";
            ddlTemplateList.DataValueField = "Id";
            ddlTemplateList.DataBind();

            ddlTemplateList.Items.Insert(0, new ListItem("Please Select", Guid.Empty.ToString()));

            hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());

            lblTitle.Text = pressedButton.Text;
        }
    }


    protected void btnRemind_Click(object sender, EventArgs e)
    {

        if (sender != null)
        {
            Button btnCliced = (Button)sender;

            PopulateTemplatePanel(btnCliced);
        }


        //// sends all jury who have not completed judging
        //GeneralFunction.GetAllScoreCache(true);
        ////List<EffieJuryManagementApp.Jury> juryList = GeneralFunction.GetAllJuryCache(false,round);
        //int counter = 0;
        //foreach (GridDataItem item in radGridEntry.Items)
        //{
        //    CheckBox chkbox = (CheckBox)item.FindControl("chkbox");

        //    if (chkbox.Checked)
        //    {
        //        EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(new Guid(item["Id"].Text.ToString()));

        //        if (jury != null)
        //        {
        //            // get his scoring
        //            List<Score> scores = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
        //            List<Score> fscores = scores.Where(p => p.IsSubmitted && !p.IsAdminRecuse && !p.IsRecuse).ToList();
        //            List<Entry> fentries = GeneralFunction.GetEntryListValidFromJuryPanel(jury, round);

        //            if (fscores.Count < fentries.Count && fentries.Count != 0)
        //            {
        //                if (round == "1") Email.SendJuryReminderRound1Email(jury);
        //                if (round == "2") Email.SendJuryReminderRound2Email(jury);
        //            }

        //            jury.DateLastRemindedString = DateTime.Now.ToString();

        //            if (jury.IsValid) 
        //                jury.Save();

        //            chkbox.Checked = false;
        //            counter++;
        //        }
        //    }
        //}

        //SystemData data = GeneralFunction.GetSystemData();
        //data.DateLastRemindedString = DateTime.Now.ToString();
        //data.Save();

        //lblError.Text = "Reminder emails sent to " + (counter).ToString() + " Jury(s).<br/>";
        //PopulateLastReminderDate();
        //Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
    }
    #endregion
}

[Serializable]
public class SendInvitationCriteria
{
    public bool isRound1 { get; set; }
    public bool isRound2 { get; set; }
    public bool isSend { get; set; }
    public string title { get; set; }
    public Guid templateId { get; set; }
}