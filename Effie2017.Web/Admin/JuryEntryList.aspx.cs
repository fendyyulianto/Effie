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

public partial class Admin_JuryEntryList : PageSecurity_Admin
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
        BindGrid(true);


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

        // get all his scoring
        List<Score> scores = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
        List<Score> fscores = scores.Where(p => p.IsSubmitted && !p.IsAdminRecuse && !p.IsRecuse).ToList();
        List<Entry> fentries = GeneralFunction.GetEntryListValidFromJuryPanel(jury, round);

        lbScoreCompletion.Text = fscores.Count.ToString() + " / " + fentries.Count.ToString();


    }
    private void LoadForm()
    {
        GeneralFunction.GetAllJuryPanelCategoryCache(true);

        // Category
        string juryround = jury.Round1PanelId;
        if (round == "2") juryround = jury.Round2PanelId;
        List <JuryPanelCategory> jcplist = GeneralFunction.GetJuryPanelCategoryFromPanelId(juryround, round);
        ddlCategory.DataSource = jcplist;
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
    private void BindGrid(bool needRebind)
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
            List<Guid> entryIdList = GeneralFunction.GetFilteredEntryListFromRegCountry(ddlCountry.SelectedValue, true);
            List<Guid> entryIdList2 = GeneralFunction.GetFilteredEntryListFromRegCompany(txtSearch.Text.Trim(), true);
            List<Guid> entryIdList3 = GeneralFunction.GetFilteredEntryListFromClientCC(txtSearch.Text.Trim(), true);

            List<Guid> entryIdList5 = GeneralFunction.GetFilteredEntryListFromJuryPanel(ddlPanel.SelectedValue, round, true);

            foreach (Entry item in slist)
            {
                Score score = GeneralFunction.GetScoreFromMatchingEntryCache(item, jury.Id, round);
                Registration reg = GeneralFunction.GetRegistrationFromEntry(item);

                // category strip out the prefix
                string category = ddlCategory.SelectedValue;
                if (category.Length > 3) category = category.Substring(3, category.Length - 3);

                if (
                    (ddlRecuse.SelectedValue == "" || (ddlRecuse.SelectedValue == "1" && score != null && (score.IsAdminRecuse || score.IsRecuse)) || (ddlRecuse.SelectedValue == "0" && ((score == null) || (score != null && !score.IsAdminRecuse && !score.IsRecuse)))) &&
                    (category == "" || (category != "" && (item.CategoryPSDetailFromRound(round) == category || GeneralFunction.IsCategoryInCategoryGroup(category, item.CategoryPSDetailFromRound(round))))) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&
                    (ddlPanel.SelectedValue == "" || (ddlPanel.SelectedValue != "" && entryIdList5.Contains(item.Id))) &&


                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entryId") && item.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entrant") && entryIdList2.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "client") && entryIdList3.Contains(item.Id)))
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

            LinkButton lnkBtn, lnkBtn2 = null;
            Label lbl = null;
            HyperLink lnk = null;

            //// Edit button
            //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            //Security.SecureControlByHiding(lnkBtn);


            // Categories
            lbl = (Label)e.Item.FindControl("lbCategory");
            lbl.Text = GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + "<br/>" + entry.CategoryPSDetailFromRound(round);


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



            // recuse or not to recuse
            lnkBtn = (LinkButton)e.Item.FindControl("lnkRecuse");
            lnkBtn2 = (LinkButton)e.Item.FindControl("LnkUnrecuse");
            Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);
            if (score != null)
            {
                lnkBtn.Visible = !score.IsAdminRecuse && !score.IsRecuse;
                lnkBtn2.Visible = score.IsAdminRecuse || score.IsRecuse;
            }
            else
            {
                // no score data therefore no recuse information saved
                lnkBtn.Visible = true;
            }
            lnkBtn.CommandArgument = entry.Id.ToString();
            lnkBtn2.CommandArgument = entry.Id.ToString();

            if (Security.IsReadOnlyAdmin())
            {
                lnkBtn.Visible = false;
                lnkBtn2.Visible = false;
            }

            //// chkboxes
            //CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            //Security.SecureControlByHiding(chkbox);

        }
    }
    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        // Recuse / Unrecuse
        if (e.CommandName == "Recuse" || e.CommandName == "Unrecuse")
        {
            lblError.Text = "";
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(new Guid(e.CommandArgument.ToString()));

            Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);
            if (score == null)
            {
                score = Score.NewScore();
                score.EntryId = entry.Id;
                score.Juryid = jury.Id;
                score.Round = round;
                score.DateCreatedString = DateTime.Now.ToString();
                score.DateSubmittedString = DateTime.Now.ToString();
            }

            // Recuse / Unrecuse
            if (e.CommandName == "Recuse")
            {
                score.IsAdminRecuse = true;
            }

            if (e.CommandName == "Unrecuse")
            {
                score.IsAdminRecuse = false;
                score.IsRecuse = false;
            }

            if (score.IsValid)
            {
                score.Save();
                PopulateForm();
            }
        }
    }
    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
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

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlCategory.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlRecuse.SelectedValue = "";

        //rtabEntry.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        //rtabEntry.SelectedIndex = 0;
        BindGrid(true);
    }
    protected void btnScores_Click(object sender, EventArgs e)
    {
        Response.Redirect("JuryScoreList.aspx?r=" + round + "&juryId=" + jury.Id.ToString());
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
            string sheetName = "Jury View Entries";
            workbook.Worksheets.Add(sheetName);
            x = 1;

            // Jury information
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SerialNo); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.FirstName + " " + jury.LastName); x++;
            y++;

            #region Basic Entry Headers
            x = 1;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;


            // basic details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryId"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entrant"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency3"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency4"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Agency5"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Recuse"); x++;

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

                Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
                if (reg != null)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;
                }
                else
                    x = x + 2;


                // client and agency
                CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(entry.Id);
                if (cclist.Count > 0) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[0].Company); x++;
                if (cclist.Count > 1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[1].Company); x++;
                if (cclist.Count > 2) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[2].Company); x++;
                if (cclist.Count > 3) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[3].Company); x++;
                if (cclist.Count > 4) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[4].Company); x++;
                if (cclist.Count > 5) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[5].Company); x++;
                if (cclist.Count > 6) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[6].Company); x++;
                if (cclist.Count > 7) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cclist[7].Company); x++;                


                // Recuse
                Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);
                string recuse = "No";
                if (score != null && score.IsAdminRecuse) recuse = "Yes";
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(recuse); x++;

                #endregion

                y++;
            }



            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Jury_View_Entries_" + jury.SerialNo + ".xlsx");

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