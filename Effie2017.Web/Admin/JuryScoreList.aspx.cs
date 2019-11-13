using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Effie2017.App;
using Telerik.Web.UI;

public partial class Admin_JuryScoreList : PageSecurity_Admin
{
    EffieJuryManagementApp.Jury jury;
    string round;

    protected void Page_Load(object sender, EventArgs e)
    {
        jury = EffieJuryManagementApp.Jury.GetJury(new Guid(Request.QueryString["juryId"]));
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

        ViewState["TabFilterValue"] = "";
        BindGrid(false, string.Empty, GridSortOrder.None, true);


        // Jury Info
        lbJuryId.Text = jury.SerialNo;
        lbJuryName.Text = jury.FirstName + " " + jury.LastName;
        lbCompany.Text = jury.Company;
        lbNetwork.Text = jury.Network;
        if (jury.NetworkOthers.Trim() != "") lbNetwork.Text += " - " + jury.NetworkOthers.Trim();
        lbHoldingCompany.Text = jury.HoldingCompany;
        if (jury.HoldingCompanyOthers.Trim() != "") lbHoldingCompany.Text += " - " + jury.HoldingCompany.Trim();
        lbRound.Text = round;

        string panel = "";
        if (round == "1") panel = GeneralFunction.CleanDelimiterList(jury.Round1PanelId, '|', ", ");
        if (round == "2") panel = GeneralFunction.CleanDelimiterList(jury.Round2PanelId, '|', ", ");
        lbPanel.Text = panel;

        lnkJury.NavigateUrl = "Jury.aspx?juryId=" + jury.Id.ToString();

        List<JuryPanelCategory> jcplist = GeneralFunction.GetJuryPanelCategoryFromPanelId(round.Equals("1") ? jury.Round1PanelId : jury.Round2PanelId, round).Where(m => !String.IsNullOrEmpty(m.CategoryPSDetail)).ToList();

        // get all his scoring
        List<Score> scores = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
        List<Score> fscores = scores.Where(p => p.IsSubmitted && !p.IsAdminRecuse && !p.IsRecuse).ToList();
        //List<Entry> fentries = GeneralFunction.GetEntryListValidFromJuryPanel(jury, round);

        lbScoreCompletion.Text = fscores.Count.ToString() + " / " + GeneralFunction.GetEntryCountValidFromJury(jury, jcplist, scores, round).ToString();

        //List<Score> scoresPending = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
        //List<Score> fscoresPending = scores.Where(p => !p.IsSubmitted && !p.IsAdminRecuse && !p.IsRecuse).ToList();

        lbScorePending.Text = GeneralFunction.GetPendingEntryCountValidFromJury(jury, jcplist, scores, round).ToString();
    }

    private void LoadForm()
    {
        GeneralFunction.GetAllJuryPanelCategoryCache(true);

        // Category
        string juryround = jury.Round1PanelId;
        if (round == "2") juryround = jury.Round2PanelId;
        List<JuryPanelCategory> jcplist = GeneralFunction.GetJuryPanelCategoryFromPanelId(juryround, round);
        ddlCategory.DataSource = jcplist.Where(m => !String.IsNullOrEmpty(m.CategoryPSDetail)).ToList();
        ddlCategory.DataValueField = "CategoryPSDetail";
        ddlCategory.DataTextField = "CategoryPSDetail";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("All", ""));



        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryList(false);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));

        // Panel
        ddlPanel.DataSource = GeneralFunction.GetJuryPanelList(round);
        ddlPanel.DataBind();
        ddlPanel.Items.Insert(0, new ListItem("All", ""));


        //Security.SecureControlByHiding(btnExport, "EXPORT");
        if (!Security.IsRoleAdmin() && !Security.IsRoleSuperAdmin())
        {
            btnExport.Visible = false;
        }
    }

    private void BindGrid(bool isCustomSort, string sortExpression, GridSortOrder order, bool needRebind)
    {
        List<Entry> list = GeneralFunction.GetEntryListFromJuryPanel(jury, round);

        // filter only completed
        List<Entry> slist = new List<Entry>();
        foreach (Entry entry in list)
        {
            if (entry.Status == StatusEntry.Completed)
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

            List<Guid> entryIdList5 = GeneralFunction.GetFilteredEntryListFromJuryPanel(ddlPanel.SelectedValue, round, true);

            foreach (Entry item in slist)
            {
                Score score = GeneralFunction.GetScoreFromMatchingEntryCache(item, jury.Id, round);


                // category strip out the prefix
                string category = ddlCategory.SelectedValue;
                if (category.Length > 3) category = category.Substring(3, category.Length - 3);

                if (
                    (ddlScoreStatus.SelectedValue == "" || (ddlScoreStatus.SelectedValue == "1" && score != null && score.IsSubmitted) || (ddlScoreStatus.SelectedValue == "0" && ((score == null) || (score != null && !score.IsSubmitted)))) &&
                    (ddlRecuse.SelectedValue == "" || (ddlRecuse.SelectedValue == "1" && score != null && (score.IsAdminRecuse || score.IsRecuse)) || (ddlRecuse.SelectedValue == "0" && ((score == null) || (score != null && !score.IsAdminRecuse && !score.IsRecuse)))) &&
                    //(ddlRecuse.SelectedValue == "" || (ddlRecuse.SelectedValue == "1" && score != null && score.IsAdminRecuse) || (ddlRecuse.SelectedValue == "0" && ((score == null) || (score != null && !score.IsAdminRecuse)))) &&
                    (category == "" || (category != "" && (item.CategoryPSDetailFromRound(round) == category || GeneralFunction.IsCategoryInCategoryGroup(category, item.CategoryPSDetailFromRound(round))))) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&
                    (ddlPanel.SelectedValue == "" || (ddlPanel.SelectedValue != "" && entryIdList5.Contains(item.Id))) &&


                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entryId") && item.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entrant") && entryIdList2.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "client") && item.Client.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) 
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


        // Sort
        flist = flist.OrderBy(p => p.Serial).ToList();

        #region CustomSorting

        if (isCustomSort)
        {
            List<Registration> regList = RegistrationList.GetRegistrationList(string.Empty, string.Empty, string.Empty).ToList();

            if (sortExpression.Equals("Serial"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist  orderby entry.Serial select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist orderby entry.Serial descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist  select entry).ToList();
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
            else if (sortExpression.Equals("ScoreStatus"))
            {
                List<Score> scoreList = GeneralFunction.GetAllScoreCache(false).Where(j => j.Juryid.Equals(jury.Id)).ToList();

                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist
                                 from score in scoreList
                                 where entry.Id == score.EntryId
                                 orderby score.IsSubmitted, score.IsAdminRecuse, score.IsRecuse
                                 select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist
                                 from score in scoreList
                                 where entry.Id == score.EntryId
                                 orderby score.IsSubmitted, score.IsAdminRecuse, score.IsRecuse descending
                                 select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("JuryFlag"))
            {
                List<Score> scoreList = GeneralFunction.GetAllScoreCache(false).Where(j => j.Juryid.Equals(jury.Id)).ToList();

                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist
                                 from score in scoreList
                                 where entry.Id == score.EntryId
                                 orderby score.Flag
                                 select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist
                                 from score in scoreList
                                 where entry.Id == score.EntryId
                                 orderby score.Flag descending
                                 select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist select entry).ToList();
                        break;
                }
            }            
        }
        else
            //flist = (from entry in flist orderby entry.Invoice descending, entry.DateCreated descending select entry).ToList();
            flist = (from entry in flist orderby entry.GetPanelId(round), entry.CategoryMarket, entry.CategoryPSDetail, entry.Serial select entry).ToList();

        #endregion

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

    #region Events

    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.Entry entry = (Effie2017.App.Entry)e.Item.DataItem;
            ScoreList slist = GeneralFunction.GetAllScoreCache(false);
            Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);
            if (entry == null) return;

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


            // Category
            lbl = (Label)e.Item.FindControl("lbCategory");
            //lbl.Text = GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + "<br/>" + entry.CategoryPSDetailFromRound(round);
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

            // Brand
            lbl = (Label)e.Item.FindControl("lbBrand");
            lbl.Text = entry.Brand;

            // Country
            lbl = (Label)e.Item.FindControl("lbCountry");
            lbl.Text = "-";
            if (reg != null)
            {
                lbl.Text = reg.Country;
            }


            if (score != null)
            {
                // Raw score
                lbl = (Label)e.Item.FindControl("lbScoreSCRaw");
                lbl.Text = score.ScoreSC.ToString();
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";

                // ID
                lbl = (Label)e.Item.FindControl("lbScoreIDRaw");
                lbl.Text = score.ScoreID.ToString();
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";

                // IL
                lbl = (Label)e.Item.FindControl("lbScoreILRaw");
                lbl.Text = score.ScoreIL.ToString();
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";

                // RE
                lbl = (Label)e.Item.FindControl("lbScoreRERaw");
                lbl.Text = score.ScoreRE.ToString();
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";



                // Weighted scores
                // SC
                lbl = (Label)e.Item.FindControl("lbScoreSC");
                lbl.Text = GeneralFunction.CalculateSC(score.ScoreSC).ToString("N");
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";

                // ID
                lbl = (Label)e.Item.FindControl("lbScoreID");
                lbl.Text = GeneralFunction.CalculateID(score.ScoreID).ToString("N");
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";

                // IL
                lbl = (Label)e.Item.FindControl("lbScoreIL");
                lbl.Text = GeneralFunction.CalculateIL(score.ScoreIL).ToString("N");
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";

                // RE
                lbl = (Label)e.Item.FindControl("lbScoreRE");
                lbl.Text = GeneralFunction.CalculateRE(score.ScoreRE).ToString("N");
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";


                // composite score
                lbl = (Label)e.Item.FindControl("lbScoreComposite");
                lbl.Text = score.ScoreComposite.ToString("N");
                if (score.IsAdminRecuse || score.IsRecuse) lbl.Text = "X";



                // Status
                lbl = (Label)e.Item.FindControl("lbScoreStatus");
                lbl.Text = "Pending";
                lbl.ForeColor = System.Drawing.Color.Gray;
                if (score.IsSubmitted)
                {
                    lbl.Text = "Completed";
                    lbl.ForeColor = System.Drawing.Color.Green;
                }
                if (score.IsAdminRecuse || score.IsRecuse)
                {
                    lbl.Text = "Recused";
                    lbl.ForeColor = System.Drawing.Color.Gray;
                }


                // Flag
                lbl = (Label)e.Item.FindControl("lbJuryFlag");
                lbl.Text = "-";
                if (score.Flag != "") lbl.Text = score.Flag;


                // Jury Recuse Flag
                lbl = (Label)e.Item.FindControl("lbJuryRecuse");
                lbl.Text = "";
                if (score.IsRecuse) lbl.Text = "Y";


                // Recuse
                lbl = (Label)e.Item.FindControl("lbRecuse");
                lbl.Text = "";
                if (score.IsAdminRecuse) lbl.Text = "Y";


                // Advancement
                lbl = (Label)e.Item.FindControl("lbAdvancement");
                lbl.Text = "-";
                lbl.Text = ((score.IsRecuse || score.IsAdminRecuse) ? "-" : ((score.IsAdvancement) ? "Y" : "N"));

            }


            //Action
            lnkBtn = (LinkButton)e.Item.FindControl("lnkScore");
            if (score != null)
            {
                lnkBtn.CommandArgument = score.EntryId.ToString();
                lnkBtn.Visible = true;
            }

            // Reset
            lnkBtn = (LinkButton)e.Item.FindControl("lnkReset");
            if (score != null && score.IsSubmitted)
            {
                lnkBtn.CommandArgument = score.Id.ToString();
                lnkBtn.Visible = true;
            }

            if (Security.IsReadOnlyAdmin())
            {
                lnkBtn.Visible = false;
            }
            
            // chkboxes
            //CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            //Security.SecureControlByHiding(chkbox);

        }
    }

    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "Edit")
        {
            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Main/Entry.aspx?db=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Open")
        {
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(new Guid(e.CommandArgument.ToString()));
            if (entry.Status == StatusEntry.Completed)
            {
                entry.Status = StatusEntry.UploadCompleted;
                entry.Save();
            }

            if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
            {
                foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
                {
                    BindGrid(true, expr.FieldName, expr.SortOrder, true);
                }
            }
            else
            {
                BindGrid(false, string.Empty, GridSortOrder.None, true);
            }
        }
        else if (e.CommandName == "View")
        {
            //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Admin/JuryEntryList.aspx?juryId=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Score")
        {
            GeneralFunction.SetRedirect("../Admin/JuryScoreList.aspx?r=" + round + "&juryId=" + jury.Id.ToString());  // to return from whereever
            Security.SetLoginSessionJury(jury);
            Response.Redirect("../Jury/EntryScore.aspx?eId=" + ((GridDataItem)e.Item)["Id"].Text + "&r=" + round + "&a=1");
        }
        else if (e.CommandName == "Reset")
        {
            Score score = Score.GetScore(new Guid(e.CommandArgument.ToString()));
            score.IsSubmitted = false;
            score.Save();

            PopulateForm();
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

        ViewState["AdvanceSearch"] = "1";
        BindGrid(false, string.Empty, GridSortOrder.None, true);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlCategory.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlRecuse.SelectedValue = "";
        ddlScoreStatus.SelectedValue = "";

        //rtabEntry.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        //rtabEntry.SelectedIndex = 0;
        BindGrid(false, string.Empty, GridSortOrder.None, true);
    }

    protected void btnEntryList_Click(object sender, EventArgs e)
    {
        Response.Redirect("JuryEntryList.aspx?r=" + round + "&juryId=" + jury.Id.ToString());
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
            string sheetName = "Jury View Scores";
            workbook.Worksheets.Add(sheetName);
            x = 1;

            // Jury information
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SerialNo); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.FirstName + " " + jury.LastName); x++;
            y++;

            #region Basic Entry Headers
            x = 1;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 3; x++;

            // basic details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 7.71; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 13.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;

            #endregion


            #region Score Headers

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("SC"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ID"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IL"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RE"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("SC(C)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ID(C)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IL(C)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RE(C)"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Total Composite Score"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Scoring Status"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 8.14; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Advancement"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Jury Flag"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 5.43; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Flag remarks"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 50; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reason remarks - Strongest element"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 50; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reason remarks - Weakest element"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 50; x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Recuse"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Additional remarks"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 50; x++;

            #endregion


            y++;

            foreach (Entry entry in flist)
            {
                x = 1;

                #region Basic Entry DataRows

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 2).ToString()); x++;

                // Basic Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Serial); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + " - " + entry.CategoryPSDetailFromRound(round)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Campaign); x++;

                // client and agency
                CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(entry.Id);
                string client = "";
                try
                {
                    client = cclist[0].Company;
                }
                catch { }

                Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
                if (reg != null)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;
                }
                else
                    x = x + 2;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(client); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Brand); x++;

                // scoring
                Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);
                if (score != null)
                {
                    bool isRecuse = false;
                    if (score.IsAdminRecuse || score.IsRecuse) isRecuse = true;

                    if (!isRecuse)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreSC); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreID); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreIL); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreRE); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CalculateSC(score.ScoreSC)); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CalculateID(score.ScoreID)); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CalculateIL(score.ScoreIL)); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CalculateRE(score.ScoreRE)); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.ScoreComposite); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                    }
                    else
                    {
                        // pop with X
                        for (int i = 0; i < 9; i++)
                        {
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("X"); x++;
                        }
                    }


                    // status
                    if (score.IsSubmitted)
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Completed");
                    else
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Pending");
                    
                    if (isRecuse)
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Recused");


                    x++;


                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((score.IsAdvancement) ? "Y" : "N"); x++;

                    // flag and remarks
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.Flag); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.FlagOthers); x++;

                    // comments
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.FeedbackStrong); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.FeedbackWeak); x++;
                    //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(recuse); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(score.AdditionalComments); x++;
                }
                else
                {
                    x = x + 9;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Pending");
                }

                #endregion

                y++;
            }



            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Jury_View_Scores_" + jury.SerialNo + ".xlsx");

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