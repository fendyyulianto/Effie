using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using Telerik.Web.UI;
using System.Configuration;

public partial class Jury_Dashboard : PageSecurity_Jury
{
    EffieJuryManagementApp.Jury jury;
    string round;

    protected void Page_Load(object sender, EventArgs e)
    {
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "start:" + DateTime.Now.ToString());
        jury = Security.GetLoginSessionJury();
        //round = ConfigurationManager.AppSettings["CurrentJuryRound"]; // "1"; // hardcoded to round 1 for now   Request.QueryString["r"];
        round = GeneralFunction.GetSystemData().ActiveRound;

        if (!IsPostBack)
        {
            // Enable this for Round 1 Jury
            if (jury.IsFirstTimeLogin)
                Response.Redirect("../Jury/JuryTC.aspx");

            LoadForm();
            PopulateForm();
        }
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "end:" + DateTime.Now.ToString());
    }
    private void PopulateForm()
    {
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "startPopulate:" + DateTime.Now.ToString());
        // Refresh the cache
        //GeneralFunction.ResetReportDataCache();
        GeneralFunction.GetAllEntryCache(true);
        GeneralFunction.GetAllScoreCache(true);
        GeneralFunction.GetAllJuryPanelCategoryCache(true);

        ViewState["TabFilterValue"] = "";
        BindGrid(true);


        // Jury Info
        lbJuryId.Text = jury.SerialNo;
        lbJuryName.Text = jury.FirstName + " " + jury.LastName;
        lbCompany.Text = jury.Company;
        lbRound.Text = round;

        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "001:" + DateTime.Now.ToString());
        // panel information, filter off inactive panels
        SystemData sys = GeneralFunction.GetSystemData();
        string jurypanelinfo = jury.Round1PanelId;
        string activepanels = sys.ActivePanelsRound1;
        if (round == "2") 
        {
            jurypanelinfo = jury.Round2PanelId;
            activepanels = sys.ActivePanelsRound2;
        }
        string[] jurypanelinfoitem = jurypanelinfo.Split('|');

        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "002:" + DateTime.Now.ToString());
        for (int i = 0; i < jurypanelinfoitem.Length; i++)
        {
            if (GeneralFunction.IsInList(jurypanelinfoitem[i], activepanels, '|'))
                lbPanel.Text += jurypanelinfoitem[i] + "&nbsp;";
            //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "003:" + DateTime.Now.ToString());
        }


        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "startget all his scoring:" + DateTime.Now.ToString());
        #region Disable because takeing to long to process
        //// get all his scoring, filter off inactive panels
        //List<Score> scores = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
        //List<Score> fscores = scores.Where(p => p.IsSubmitted && !p.IsAdminRecuse && !p.IsRecuse).ToList();
        //List<Score> ascores = GeneralFunction.FilterScoreListFromActivePanels(fscores, round);

        //List<Entry> fentries = GeneralFunction.GetEntryListValidFromJuryPanel(jury, round);
        //List<Entry> aentries = GeneralFunction.FilterEntryListFromActivePanels(fentries, round);
        //lbScoreCompletion.Text = ascores.Count.ToString() + " / " + aentries.Count.ToString();
        #endregion
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "endget all his scoring:" + DateTime.Now.ToString());

        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "endPopulate:" + DateTime.Now.ToString());

    }
    private void LoadForm()
    {
        //// Payment Status
        //ddlPaymentStatus.Items.Add(new ListItem("All", ""));
        //ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentEntryStatus(StatusPaymentEntry.Paid), StatusPaymentEntry.Paid));
        //ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentEntryStatus(StatusPaymentEntry.NotPaid), StatusPaymentEntry.NotPaid));

        //// Entry Status
        //ddlEntryStatus.Items.Add(new ListItem("All", ""));
        //ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadPending), StatusEntry.UploadPending));
        //ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadCompleted), StatusEntry.UploadCompleted));
        //ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.Completed), StatusEntry.Completed));

        //// Category
        //DataTable dt1 = Category.GetSubcategories("01");
        //DataTable dt2 = Category.GetSubcategories("02");
        //DataTable dt3 = Category.GetSubcategories("03");
        //DataTable dt4 = Category.GetSubcategories("04");

        //ddlCategory.Items.Add(new ListItem("All", ""));

        //ddlCategory.Items.Add(SeparatorItem());
        //ddlCategory.Items.Add(new ListItem("Single - Products and Services", "01"));
        //ddlCategory.Items.Add(SeparatorItem());
        //foreach (DataRow dr in dt1.Rows)
        //    ddlCategory.Items.Add("SP-" + dr["Name"].ToString());

        //ddlCategory.Items.Add(SeparatorItem());
        //ddlCategory.Items.Add(new ListItem("Single - Specialty", "02"));
        //ddlCategory.Items.Add(SeparatorItem());
        //foreach (DataRow dr in dt2.Rows)
        //    ddlCategory.Items.Add("SS-" + dr["Name"].ToString());

        //ddlCategory.Items.Add(SeparatorItem());
        //ddlCategory.Items.Add(new ListItem("Multi - Products and Services", "03"));
        //ddlCategory.Items.Add(SeparatorItem());
        //foreach (DataRow dr in dt3.Rows)
        //    ddlCategory.Items.Add("MP-" + dr["Name"].ToString());

        //ddlCategory.Items.Add(SeparatorItem());
        //ddlCategory.Items.Add(new ListItem("Multi - Specialty", "04"));
        //ddlCategory.Items.Add(SeparatorItem());
        //foreach (DataRow dr in dt4.Rows)
        //    ddlCategory.Items.Add("MS-" + dr["Name"].ToString());



        //// Country
        //ddlCountry.DataSource = GeneralFunction.GetFilteredCountryList(false);
        //ddlCountry.DataBind();
        //ddlCountry.Items.Insert(0, new ListItem("All", ""));


        //Security.SecureControlByHiding(btnExport, "EXPORT");
    }
    private void BindGrid(bool needRebind)
    {
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "startBindGrid:" + DateTime.Now.ToString());

        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "GetEntryListFromJuryPanel:" + DateTime.Now.ToString());
        List<Entry> entries = GeneralFunction.GetEntryListFromJuryPanel(jury, round);
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "EndGetEntryListFromJuryPanel:" + DateTime.Now.ToString());

        // filter these entries such that they belong to active panels for the round
        List<Entry> jlist = new List<Entry>();
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "GetSystemData:" + DateTime.Now.ToString());
        SystemData sys = GeneralFunction.GetSystemData();
        foreach (Entry entry in entries)
        {
            List<JuryPanelCategory> jpclist = GeneralFunction.GetJuryPanelCategoryFromCategoryPS(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);
            if (jpclist.Count > 0) // there should only be 1, because only 1 cat per round per jpc
            {
                string activelist = sys.ActivePanelsRound1;
                if (round == "2") activelist = sys.ActivePanelsRound2;
                if (GeneralFunction.IsInList(jpclist[0].PanelId, activelist, '|')) jlist.Add(entry);
            }

        }
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "EndGetSystemData:" + DateTime.Now.ToString());


        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "filter not recused by admin:" + DateTime.Now.ToString());
        // filter not recused by admin
        List<Entry> slist = new List<Entry>();
        foreach (Entry entry in jlist)
        {
            Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);

            if (score == null ||(score != null && !score.IsAdminRecuse && !score.IsRecuse))
                slist.Add(entry);
        }
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "End filter not recused by admin:" + DateTime.Now.ToString());

        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "Bind:" + DateTime.Now.ToString());
        radGridEntry.DataSource = slist;
        if (needRebind) radGridEntry.DataBind();
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "EndBind:" + DateTime.Now.ToString());
        //GeneralFunction.WriteTxtFile("F:\\www\\Effie2018.Web\\storage\\llog\\test.txt", "endBindGrid:" + DateTime.Now.ToString());
    }
    

    protected void btnClose_Click(object sender, EventArgs e)
    {
        phRecuseEntry.Visible = false;
    }
    
    protected void btnSubmitRecuseEntry_Click(object sender, EventArgs e)
    {
        phRecuseEntry.Visible = false;
        Guid EntryId = new Guid(txtEntryId.Value);
        bool IsRecuse = false;

        if (ddlRecuse.SelectedValue == "Yes") IsRecuse = true;
        else IsRecuse = false;

        RecuseEntry(txtRecuseRemarks.Text, EntryId, IsRecuse);
    }

    protected void RecuseEntry(string Text, Guid EntryId, bool IsRecuse)
    {
        lblError.Text = "";
        Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(EntryId);

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

        if (IsRecuse)
        {
            score.IsRecuse = true;
        }
        else
        {
            score.IsRecuse = false;
        }

        if (score.IsValid)
        {
            score.RecuseRemarks = Text;
            score.Save();
            PopulateForm();
        }
    }

    #region Events
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Entry entry = (Entry)e.Item.DataItem;
            //List<Score> scores = GeneralFunction.GetScoreListFromJuryCache(jury.Id, round);
            Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);
            
            LinkButton lnkBtn, lnkBtn2 = null;
            Label lbl = null;
            HyperLink lnk = null;

            HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");
            hdfId.Value = entry.Id.ToString();

            CheckBox chkBox = (CheckBox)e.Item.FindControl("chkbox");

            // Entry Id
            lbl = (Label)e.Item.FindControl("lbSerial");
            lbl.Text = entry.Serial;


            // Campaign title
            lbl = (Label)e.Item.FindControl("lbCampaign");
            lbl.Text = entry.Campaign;


            // Category
            lbl = (Label)e.Item.FindControl("lbCategory");
            lbl.Text = GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + "<br/>" + entry.CategoryPSDetailFromRound(round);



            // Client
            lbl = (Label)e.Item.FindControl("lbClient");
            lbl.Text = entry.Client;



            // Brand
            lbl = (Label)e.Item.FindControl("lbBrand");
            lbl.Text = entry.Brand;


            // submitted by
            lbl = (Label)e.Item.FindControl("lnkBtnBuSubmittedBy");
            Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
            lbl.Text = "-";
            if (reg != null)
            {
                lbl.Text = reg.Company;
                //lnkBtn.CommandArgument = reg.Id.ToString();
            }


            // Score Status
            lbl = (Label)e.Item.FindControl("lbScoreStatus");
            lbl.Text = "Pending";
            lbl.ForeColor = System.Drawing.Color.Gray;


            // ACtion
            lnkBtn = (LinkButton)e.Item.FindControl("lnkScore");
            lnkBtn.CommandArgument = entry.Id.ToString();
            if (score != null && score.IsSubmitted)
            {
                lnkBtn.Text = "View";
            }


            // recuse or not to recuse
            lnkBtn = (LinkButton)e.Item.FindControl("lnkRecuse");
            lnkBtn2 = (LinkButton)e.Item.FindControl("LnkUnrecuse");
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


            if (score != null)
            {
                // there is a matching score

                // SC
                lbl = (Label)e.Item.FindControl("lbScoreSC");
                lbl.Text = GeneralFunction.CalculateSC(score.ScoreSC).ToString("N");

                // ID
                lbl = (Label)e.Item.FindControl("lbScoreID");
                lbl.Text = GeneralFunction.CalculateID(score.ScoreID).ToString("N");

                // IL
                lbl = (Label)e.Item.FindControl("lbScoreIL");
                lbl.Text = GeneralFunction.CalculateIL(score.ScoreIL).ToString("N");

                // RE
                lbl = (Label)e.Item.FindControl("lbScoreRE");
                lbl.Text = GeneralFunction.CalculateRE(score.ScoreRE).ToString("N");

                // Composite
                lbl = (Label)e.Item.FindControl("lbScoreComposite");
                lbl.Text = score.ScoreComposite.ToString("N");



                // Flag
                lbl = (Label)e.Item.FindControl("lbJuryFlag");
                lbl.Text = "-";
                if (score.Flag != "") lbl.Text = score.Flag;


                //// Jury Recuse
                //lbl = (Label)e.Item.FindControl("lbJuryRecuse");
                //lbl.Text = "";
                //if (score.IsRecuse) lbl.Text = "Yes";



                // Score Status
                lbl = (Label)e.Item.FindControl("lbScoreStatus");
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

                chkBox.Visible = !score.IsAdminRecuse && !score.IsRecuse && !score.IsSubmitted;

                // ACtion, if recuse then no action
                lnkBtn = (LinkButton)e.Item.FindControl("lnkScore");
                if (score.IsAdminRecuse)
                {
                    lnkBtn.Text = "Recused";
                    lnkBtn.ForeColor = System.Drawing.Color.Gray;
                    lnkBtn.Enabled = false;
                }

            }
            

            // chkboxes
            //CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            //Security.SecureControlByHiding(chkbox);

        }
    }
    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "Score")
        {
            //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Jury/EntryScore.aspx?eId=" + e.CommandArgument.ToString() + "&r=" + round);
        }

        // Recuse / Unrecuse
        if (e.CommandName == "Recuse" || e.CommandName == "Unrecuse")
        {
            Guid EntryId = new Guid(e.CommandArgument.ToString());

            //***** SHOW POPUP REMARKS BEFORE RECUSE ******
            //phRecuseEntry.Visible = true;
            //txtEntryId.Value = EntryId.ToString();


            bool IsRecuse = false;
            if (e.CommandName == "Recuse") IsRecuse = true;
            else IsRecuse = false;

            RecuseEntry(txtRecuseRemarks.Text, EntryId, IsRecuse);
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

        //ViewState["AdvanceSearch"] = "1";
        //BindEntry(true);

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        //txtSearch.Text = "";
        //ddlSearch.SelectedValue = "";
        //ddlEntryStatus.SelectedValue = "";
        //ddlCategory.SelectedValue = "";
        //ddlCountry.SelectedValue = "";
        //ddlMarket.SelectedValue = "";

        //rtabEntry.Visible = true;

        //ViewState["AdvanceSearch"] = "";
        //ViewState["TabFilterValue"] = "";
        //rtabEntry.SelectedIndex = 0;
        //BindEntry(true);
    }
    protected void btnSubmitScore_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        int counter = 0;
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

            if (chkbox.Checked)
            {
                Entry entry = Entry.GetEntry(new Guid(hdfId.Value.ToString()));
                
                Score score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);

                if (score != null)
                {
                    score.IsSubmitted = true;
                    score.Round = round;
                    score.DateSubmittedString = DateTime.Now.ToString();

                    if (score.IsValid)
                    {
                        score.Save();

                        chkbox.Checked = false;
                        counter++;
                    }
                    else
                        lblError.Text +=  "Error : " + score.BrokenRulesCollection.ToString();                    
                }
            }
        }

        PopulateForm();

        if (counter == 0)
            lblError.Text = "Please select at least one Entry to submit Score.<br/>";
        else
            lblError.Text = "Score(s) submitted for " + (counter).ToString() + " Entry(s).<br/>";
    }

    #endregion   
}