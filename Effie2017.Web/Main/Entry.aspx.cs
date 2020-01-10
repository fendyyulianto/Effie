using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Effie2017.App;
using System.Data;
using System.IO;

public partial class Main_Entry : System.Web.UI.Page
{
    Entry entry = null;
    public static DateTime CurrentCaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseStartDate"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {

        cc1.Save_Clicked += new EventHandler(CCSave_Clicked);
        cc1.Cancel_Clicked += new EventHandler(CCCancel_Clicked);

        if (Request.QueryString["Id"] != null && Request.QueryString["Id"] != "")
        {
            entry = Entry.GetEntry(new Guid(Request.QueryString["Id"]));
        }
        else
            entry = Entry.NewEntry();


        // Check for add entry cutoff
        if (entry.IsNew && GeneralFunction.IsAddNewEntryCutOff()) Response.Redirect("../Main/Dashboard.aspx");


        // hackish
        // load back the previous saved entry object, because to prevent double saved if user clicks Next and then Edit, then Save Draft or Next
        if (ViewState["Effie.Entry.SavedPrevious"] != null)
        {
            entry = Entry.GetEntry(new Guid(ViewState["Effie.Entry.SavedPrevious"].ToString()));
        }



        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }


        if(!string.IsNullOrWhiteSpace(ddlEffectiveness.SelectedValue))
        {
            foreach (ListItem aListItem in cblCaseData.Items)
            {
                if (ddlEffectiveness.SelectedValue == aListItem.Value)
                {
                    aListItem.Enabled = false;
                    aListItem.Selected = true;
                }
            }
        }
    }

    private void LoadForm()
    {
        //Populate rblPermission Text
        rblPermission.Items[0].Text = "<span style=\"font-weight:bold\">\"Publish as the case was submitted \"</span> You agree that the written entry form may be published as it was submitted and reproduced or displayed for educational purposes.";
        rblPermission.Items[1].Text = "<span style=\"font-weight:bold\">\"Publish an edited version of the written case</span> You agree to submit an edited version of your case study for publication which will be reproduced or displayed for educational purposes.<br> You may redact any confidential information.  ";
        rblPermission.Items[2].Text = "<span style=\"font-weight:bold\">\"Publish the case as it was submitted after 3 years\"</span> The written case will be published as it was submitted after 3 years. The case will be published as it was submitted and reproduced or displayed for educational purposes ";
        rblPermission.Items[3].Text = "<span style=\"font-weight:bold\">\"Publish an edited version of the written case after 3 years.\"</span> You agree to submit and edited version of your case study for publication with will be reproduced or displayed for educational purposes after 3 years.<br> You may redact any confidential information.";

        DateTime normalCaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseStartDate"].ToString());
        DateTime specialCaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["SpecialCaseStartDate"].ToString());
        DateTime caseEndDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseEndDate"].ToString());
        
        lblCaseStartDate.Text = normalCaseStartDate.ToString("MMMM d, yyyy");
        lblCaseEndDate.Text = caseEndDate.ToString("MMMM d, yyyy");
        //lblSpecialCaseEndDate.Text = "For Sustained Success category, results must minimally date back to " + specialCaseStartDate.ToString("yyyy") + ", with 3 years of data presented.";
        
        PopulateDateList();
        
        // Salutation
        ddlRepSalutation.Items.Add(new ListItem("Select", ""));
        ddlRepSalutation.Items.Add("Dr.");
        ddlRepSalutation.Items.Add("Mr.");
        ddlRepSalutation.Items.Add("Ms.");
        ddlRepSalutation.Items.Add("Mrs.");
        
        // Effectiveness and case data
        List<string> items = GeneralFunction.GetEffectivenessItems();
        ddlEffectiveness.DataSource = items;
        ddlEffectiveness.DataBind();
        ddlEffectiveness.Items.Insert(0, new ListItem("Select", ""));

        cblEffectiveness.DataSource = items;
        cblEffectiveness.DataBind();

        cblCaseData.DataSource = items;
        cblCaseData.DataBind();


        // CC table style
        gvCC.Attributes.Add("style", "font-size:100%;");

        // word count
        txtSummary.Attributes.Add("onkeyup", "cnt();");

        // hero touchpoints
        List<string> herotouchpoints = new List<string>();
        herotouchpoints.Add("Branded Content");
        herotouchpoints.Add("Cinema");
        herotouchpoints.Add("Consumer Involvement");
        herotouchpoints.Add("Direct");
        herotouchpoints.Add("Ecommerce");
        herotouchpoints.Add("Events");
        herotouchpoints.Add("Guerrilla");
        herotouchpoints.Add("Interactive/Online");
        herotouchpoints.Add("Internal Marketing");
        herotouchpoints.Add("Mobile/Tablet");
        herotouchpoints.Add("OOH");
        herotouchpoints.Add("Packaging");
        herotouchpoints.Add("Point of Care (POC)");
        herotouchpoints.Add("PR");
        herotouchpoints.Add("Print");
        herotouchpoints.Add("Product Design");
        herotouchpoints.Add("Professional Engagement");
        herotouchpoints.Add("Radio");
        herotouchpoints.Add("Retail Experience");
        herotouchpoints.Add("Sales Promotion");
        herotouchpoints.Add("Search Engine Marketing (SEM/SEO)");
        herotouchpoints.Add("Social Networking Sites/Applications");
        herotouchpoints.Add("Sponsorship");
        herotouchpoints.Add("Trade Shows");
        herotouchpoints.Add("TV");
        herotouchpoints.Add("Not Applicable");
        herotouchpoints.Add("Others, please specify");

        ddlHeroTouchPoint.DataSource = herotouchpoints;
        ddlHeroTouchPoint2.DataSource = herotouchpoints;
        ddlHeroTouchPoint3.DataSource = herotouchpoints;

        ddlHeroTouchPoint.DataBind();
        ddlHeroTouchPoint2.DataBind();
        ddlHeroTouchPoint3.DataBind();

        ddlHeroTouchPoint.Items.Insert(0, new ListItem("Select", ""));
        ddlHeroTouchPoint2.Items.Insert(0, new ListItem("Select", ""));
        ddlHeroTouchPoint3.Items.Insert(0, new ListItem("Select", ""));


        // Social Platforms
        cblSocialPlatforms.Items.Add("Blog");
        cblSocialPlatforms.Items.Add("Facebook");
        cblSocialPlatforms.Items.Add("Flickr");
        cblSocialPlatforms.Items.Add("Foursquare");
        cblSocialPlatforms.Items.Add("Google+");
        cblSocialPlatforms.Items.Add("Instagram");
        cblSocialPlatforms.Items.Add("Line");
        cblSocialPlatforms.Items.Add("LinkedIn");
        cblSocialPlatforms.Items.Add("MySpace");
        //cblSocialPlatforms.Items.Add("Pandora");
        cblSocialPlatforms.Items.Add("Pinterest");
        cblSocialPlatforms.Items.Add("Reddit");
        //cblSocialPlatforms.Items.Add("Shazam");
        cblSocialPlatforms.Items.Add("Spotify");
        cblSocialPlatforms.Items.Add("Twitter");
        cblSocialPlatforms.Items.Add("Vimeo");
        cblSocialPlatforms.Items.Add("Wechat");
        cblSocialPlatforms.Items.Add("Weibo");
        cblSocialPlatforms.Items.Add("Yelp");
        cblSocialPlatforms.Items.Add("Youku");
        cblSocialPlatforms.Items.Add("YouTube");
        cblSocialPlatforms.Items.Add("Others, please specify");

        // SDG Options
        // Effectiveness and case data
        List<string> sdgItems = GeneralFunction.GetSDGItems();
        ddlSDGOption1.DataSource = sdgItems;
        ddlSDGOption1.DataBind();
        ddlSDGOption1.Items.Insert(0, new ListItem("Select", ""));

        cblSDGOption2.DataSource = sdgItems;
        cblSDGOption2.DataBind();

        // scroll top for submit
        btnNext.Attributes.Add("onclick", "javascript:scroll(0,0);return true;");

        // confirm for cancel button if status is draft
        if (entry.IsNew || entry.Status == StatusEntry.Draft) btnCancel.Attributes.Add("onclick", "return confirm('Confirm to cancel? Any changes will not be saved.');");

        // masaterpage navi & user info
        ((Common_MasterPage)Page.Master).SetConfirmLogout();
        ((Common_MasterPage)Page.Master).ShowUser();



        if (Security.IsUserAdminSpoof())
        {
            // Category
            DataTable dt1 = Category.GetSubcategories("01");
            DataTable dt2 = Category.GetSubcategories("02");
            DataTable dt3 = Category.GetSubcategories("03");
            DataTable dt4 = Category.GetSubcategories("04");

            ddlCategoryR2.Items.Add(new ListItem("None", ""));

            foreach (DataRow dr in dt1.Rows)
                ddlCategoryR2.Items.Add("SP-" + dr["Name"].ToString());

            foreach (DataRow dr in dt2.Rows)
                ddlCategoryR2.Items.Add("SS-" + dr["Name"].ToString());

            foreach (DataRow dr in dt3.Rows)
                ddlCategoryR2.Items.Add("MP-" + dr["Name"].ToString());

            foreach (DataRow dr in dt4.Rows)
                ddlCategoryR2.Items.Add("MS-" + dr["Name"].ToString());
        }

    }

    private void PopulateDateList()
    {
        string ddlStartDayVal = ddlStartDay.SelectedValue;
        string ddlEndDayVal = ddlEndDay.SelectedValue;
        string ddlStartMonthVal = ddlStartMonth.SelectedValue;
        string ddlEndMonthVal = ddlEndMonth.SelectedValue;

        string ddlStartYearVal = ddlStartYear.SelectedValue;
        string ddlEndYearVal = ddlEndYear.SelectedValue;

        ddlStartDay.Items.Clear();
        ddlEndDay.Items.Clear();
        ddlStartMonth.Items.Clear();
        ddlEndMonth.Items.Clear();

        ddlStartYear.Items.Clear();
        ddlEndYear.Items.Clear();

        // Bind date droplists
        for (int i = 1; i <= 31; i++)
        {
            ddlStartDay.Items.Add(i.ToString());
            ddlEndDay.Items.Add(i.ToString());
        }
        ddlStartDay.Items.Insert(0, new ListItem("Select", ""));
        ddlEndDay.Items.Insert(0, new ListItem("Select", ""));

        for (int i = 1; i <= 12; i++)
        {
            ddlStartMonth.Items.Add(new ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), i.ToString()));
            ddlEndMonth.Items.Add(new ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), i.ToString()));
        }
        ddlStartMonth.Items.Insert(0, new ListItem("Select", ""));
        ddlEndMonth.Items.Insert(0, new ListItem("Select", ""));

        for (int i = 2014; i <= 2020; i++)
        {
            ddlStartYear.Items.Add(i.ToString());
            //ddlEndYear.Items.Add(i.ToString());
        }
        ddlStartYear.Items.Insert(0, new ListItem("Select", ""));
        //ddlEndYear.Items.Insert(0, new ListItem("Select", ""));

        for (int i = 2014; i <= 2020; i++)
        {
            //ddlStartYear.Items.Add(i.ToString());
            ddlEndYear.Items.Add(i.ToString());
        }
        //ddlStartYear.Items.Insert(0, new ListItem("Select", ""));
        ddlEndYear.Items.Insert(0, new ListItem("Select", ""));

        //Date for Sustained Success
        if (ddlCategoryPSDetail.SelectedValue == "Sustained Success")
        {
            ddlStartYear.Items.Clear();
            for (int i = 2013; i <= 2020; i++)
            {
                ddlStartYear.Items.Add(i.ToString());
            }
            ddlStartYear.Items.Insert(0, new ListItem("Select", ""));

            //ddlEndYear.Items.Clear();
            //for (int i = 2014; i <= 2019; i++)
            //{
            //    ddlEndYear.Items.Add(i.ToString());
            //}
            //ddlEndYear.Items.Insert(0, new ListItem("Select", ""));

            lblSpecialCaseEndDate.Text = "For Sustained Success category, results must minimally date back to 31 August 2016, with 3 years of data presented.";
        }
        //lblSpecialCaseEndDate.Text = ddlCategoryPSDetail.SelectedValue;

        //PrePopulate back
        ddlStartDay.SelectedValue = ddlStartDayVal;
        ddlEndDay.SelectedValue = ddlEndDayVal;
        ddlStartMonth.SelectedValue = ddlStartMonthVal;
        ddlEndMonth.SelectedValue = ddlEndMonthVal;

        try
        {
            ddlStartYear.SelectedValue = ddlStartYearVal;
        }
        catch { }
        try
        {
            ddlEndYear.SelectedValue = ddlEndYearVal;
        }
        catch { }
    }

    private void PopulateForm()
    {
        // Pop the fields
        //entry.RegistrationId = Security.GetLoginSessionUser().Id;
        txtCampaign.Text = entry.Campaign;
        txtClient.Text = entry.Client;
        txtBrand.Text = entry.Brand;
        rblCategoryMarket.SelectedValue = entry.CategoryMarket;
        rblCategoryPS.SelectedValue = entry.CategoryPS;

        //rblCategoryMarket_SelectedIndexChanged(null, null);
        PopulateCategoryData();

        ddlCategoryPSDetail.SelectedValue = entry.CategoryPSDetail;
        PopulateDateList();

        if (!entry.IsNew)
        {
            if (entry.DateCampaignStart != DateTime.Parse("1/1/2000"))
            {
                ddlStartMonth.SelectedValue = entry.DateCampaignStart.Month.ToString();
                ddlStartDay.SelectedValue = entry.DateCampaignStart.Day.ToString();
                ddlStartYear.SelectedValue = entry.DateCampaignStart.Year.ToString();
            }

            if (entry.DateCampaignEnd != DateTime.Parse("1/1/2000"))
            {
                ddlEndMonth.SelectedValue = entry.DateCampaignEnd.Month.ToString();
                ddlEndDay.SelectedValue = entry.DateCampaignEnd.Day.ToString();
                ddlEndYear.SelectedValue = entry.DateCampaignEnd.Year.ToString();

            }

            chkIsCampaignOngoing.Checked = entry.IsCampaignOngoing;
            PopulateDateEnableDisable();
        }


        if (rblCategoryMarket.SelectedValue == "SM")
        {
            ddlEffectiveness.SelectedValue = entry.Effectiveness;
            ddlEffectiveness.Visible = true;
            lbEffectivenessSM.Visible = true;
        }

        if (rblCategoryMarket.SelectedValue == "MM")
        {
            GeneralFunction.AssignValueCheckBoxList(cblEffectiveness, entry.Effectiveness, "|");
            cblEffectiveness.Visible = true;
            lbEffectivenessMM.Visible = true;
        }

        ddlRepSalutation.SelectedValue = entry.RepSalutation;
        txtRepFirstname.Text = entry.RepFirstname;
        txtRepLastname.Text = entry.RepLastname;
        txtRepJob.Text = entry.RepJob;
        txtRepCompany.Text = entry.RepCompany;
        txtRepContactCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(entry.RepContact);
        txtRepContactArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(entry.RepContact);
        txtRepContactNumber.Text = GeneralFunction.GetNumberFromContactNumber(entry.RepContact);
        txtRepMobileCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(entry.RepMobile);
        txtRepMobileArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(entry.RepMobile);
        txtRepMobileNumber.Text = GeneralFunction.GetNumberFromContactNumber(entry.RepMobile);
        txtRepEmail.Text = entry.RepEmail;

        txtSummary.Text = entry.Summary;


        // Load cc
        // pop the grid only if its not draft
        GeneralFunction.ResetCCCache();
        CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(entry.Id);
        foreach (CompanyCredit cc in cclist)
            GeneralFunction.AddCCtoListCache(cc);

        // Show the CC for new Entry object or no cc list or draft cc status
        if (entry.IsNew || GeneralFunction.GetCCListCache() == null) // || (cclist.Count > 1 && cclist[0].Status == StatusEntry.Draft))
        {
            // add draft cc into the cache
            CompanyCredit c1 = CompanyCredit.NewCompanyCredit();
            c1.ContactType = "Client";
            c1.EntryId = entry.Id;
            c1.No = 1;
            c1.Status = StatusEntry.Draft;
            GeneralFunction.AddCCtoListCache(c1);

            CompanyCredit c2 = CompanyCredit.NewCompanyCredit();
            c2.ContactType = "Lead Agency";
            c2.EntryId = entry.Id;
            c2.No = 2;
            c2.Status = StatusEntry.Draft;
            GeneralFunction.AddCCtoListCache(c2);

        }

        // load ic        
        PopulateIndCredits();

        // load company credit
        BindCompanyCreditGrid();

        //Load all check to populate ddl


        GeneralFunction.AssignValueCheckBoxList(cblSDGOption2, entry.SDGData2, "|");
        GeneralFunction.AssignValueCheckBoxList(cblObjective, entry.EntryObjective, "|");
        GeneralFunction.AssignValueCheckBoxList(cblResearch, entry.Research, "|");
        GeneralFunction.AssignValueCheckBoxList(cblTargetAudience, entry.TargetAudience, "|");
        PopulateDropdownlist("ALL");

        ddlProductClassification.SelectedValue = entry.ProductClassification;
        phddlProductClassificationOther.Visible = (entry.ProductClassificationOthers.Trim().Length != 0);
        txtProductClassificationOther.Text = entry.ProductClassificationOthers;

        ddlObjective2.SelectedValue = entry.EntryObjective2;
        
        ddlTargetAudiencePri.SelectedValue = entry.TargetAudiencePri;
        phcblTargetAudienceOther.Visible = (entry.TargetAudienceOthers.Trim().Length != 0);
        txtTargetAudienceOther.Text = entry.TargetAudienceOthers;
        phddlTargetAudiencePriOther.Visible = (entry.TargetAudiencePriOthers.Trim().Length != 0);
        txtTargetAudiencePriOther.Text = entry.TargetAudiencePriOthers;

        ddlHeroTouchPoint.SelectedValue = entry.HeroTouchPoint;
        phHeroTouchPointOther.Visible = (entry.HeroTouchPointOthers.Trim().Length != 0);
        txtHeroTouchPointOther.Text = entry.HeroTouchPointOthers;

        ddlHeroTouchPoint2.SelectedValue = entry.HeroTouchPoint2;
        phHeroTouchPointOther2.Visible = (entry.HeroTouchPointOthers2.Trim().Length != 0);
        txtHeroTouchPointOther2.Text = entry.HeroTouchPointOthers2;

        ddlHeroTouchPoint3.SelectedValue = entry.HeroTouchPoint3;
        phHeroTouchPointOther3.Visible = (entry.HeroTouchPointOthers3.Trim().Length != 0);
        txtHeroTouchPointOther3.Text = entry.HeroTouchPointOthers3;

        //phSocialPlatforms.Visible = (ddlCategoryPSDetail.SelectedValue == "Real Time Marketing");
        GeneralFunction.AssignValueCheckBoxList(cblSocialPlatforms, entry.SocialPlatforms, "|");
        txtSocialPlatformsOthers.Text = entry.SocialPlatformsOthers.Trim();
        phSocialPlatformsOthers.Visible = (entry.SocialPlatformsOthers.Trim() != "");

        ddlResearchImp.SelectedValue = entry.ResearchImp;
        txtResearchOther.Text = entry.ResearchOther;
        
        GeneralFunction.AssignValueCheckBoxList(cblCaseData, entry.CaseData, "|");

        ddlSDGOption1.SelectedValue = entry.SDGData1;

        rblPermission.SelectedValue = entry.Permission;

        txtName.Text = entry.Name;
        txtTitle.Text = entry.Title;
        txtCompany.Text = entry.Company;

        // Show the bottom buttons according to the status
        btnCancel.Visible = false;
        btnNext.Visible = false;
        btnSaveDraft.Visible = false;

        //btnEdit.Visible = false;
        //btnConfirm.Visible = false;
        //btnSaveAdd.Visible = false;
        phButtonsReviewMode.Visible = false;
        
        ddlObjective2.SelectedValue = entry.EntryObjective2;
        ddlResearchImp.SelectedValue = entry.ResearchImp;
        ddlTargetAudiencePri.SelectedValue = entry.TargetAudiencePri;

        // Overwrite the panels visiblity if its admin viewing the page
        if (Security.IsUserAdminSpoof())
        {
            // admin user 
            ltMainTitle.Text = "Edit";
            chkAgree.Checked = true;

            // bottom buttons
            pbButtonsAdminMode.Visible = true;

            // top status
            phStatus.Visible = true;
            phAdminRemarks.Visible = true;
            phAdminRemarksEdit.Visible = true;
            lbWithdrawStatusEntry.Visible = false;
            ddlWithdraw.Visible = true;
            btnAdminSubmitStatus.Visible = true;
            ddlStatusEntry.Visible = true;
            ddlRound2.Visible = true;
            ddlCategoryR2.Visible = true;

            // pop the status ddl but disable it if its not allowed to be selected and changed
            if (entry.Status != StatusEntry.Completed && entry.Status != StatusEntry.UploadCompleted)
            {
                ddlStatusEntry.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(entry.Status), entry.Status));
                ddlStatusEntry.Enabled = false;
            }
            else
            {
                ddlStatusEntry.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadCompleted), StatusEntry.UploadCompleted));
                ddlStatusEntry.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.Completed), StatusEntry.Completed));
                ddlStatusEntry.SelectedValue = entry.Status;
            }

            // admin remarks
            BindAdminRemarks();

            // Access Control for administrators
            //Security.SecureControlByHiding(btnSubmitAdmin);
            //Security.SecureControlByHiding(btnAdminSubmitStatus);
            //Security.SecureControlByHiding(btnAdminSubmitRemarks);

            if (Security.IsReadOnlyAdmin() || Security.IsRoleST())
            {
                btnSubmitAdmin.Visible = false;
                btnAdminSubmitStatus.Visible = false;
                btnAdminSubmitRemarks.Visible = false;
            }

            // Admin view
            if (Request.QueryString["v"] == "1")
            {
                ltMainTitle.Text = "View";
                EnableViewMode();

                // hide the admin submit button
                btnSubmitAdmin.Visible = false;
                btnAdminSubmitStatus.Visible = false;

                // hide the remarks and show the withdraw status label
                phAdminRemarksEdit.Visible = false;
                ddlRound2.Visible = false;
                ddlWithdraw.Visible = false;
                ddlCategoryR2.Visible = false;

                lbWithdrawStatusEntry.Visible = true;
                ddlStatusEntry.Visible = false;
                lbStatusEntry.Visible = true;
                lbRound2.Visible = true;
                lbCategoryR2.Visible = true;
            }


        }
        else
        {
            // Normal user 

            if (entry.Status == StatusEntry.Draft)
            {

                btnCancel.Visible = true;
                btnNext.Visible = true;
                btnSaveDraft.Visible = true;
            }

            if (entry.IsNew /*|| entry.Status == StatusEntry.Draft*/)
            {
                btnCancel.Visible = true;
                btnNext.Visible = true;
                btnSaveDraft.Visible = true;

                //Populate Certification of Entry by Company Representative
                Registration reg = Security.GetLoginSessionUser();

                ddlRepSalutation.SelectedValue = reg.Salutation +".";
                txtRepFirstname.Text = reg.Firstname;
                txtRepLastname.Text = reg.Lastname;
                txtRepJob.Text = reg.Job;
                txtRepCompany.Text = reg.Company;
                txtRepContactCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(reg.Contact);
                txtRepContactArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(reg.Contact);
                txtRepContactNumber.Text = GeneralFunction.GetNumberFromContactNumber(reg.Contact);
                txtRepMobileCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(reg.Mobile);
                txtRepMobileArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(reg.Mobile);
                txtRepMobileNumber.Text = GeneralFunction.GetNumberFromContactNumber(reg.Mobile);
                txtRepEmail.Text = reg.Email;

            }
            if (entry.Status == StatusEntry.Ready)
            {
                if (Request.QueryString["v"] == "1")
                {
                    // Preview mode
                    EnableReviewMode();
                    chkAgree.Checked = true;
                }
                else
                {
                    // edit mode
                    btnCancel.Visible = true;
                    btnNext.Visible = true;
                    btnSaveDraft.Visible = true;
                    chkAgree.Checked = true;
                }
            }
            if (entry.Status == StatusEntry.PaymentPending || entry.Status == StatusEntry.UploadPending || entry.Status == StatusEntry.UploadCompleted || entry.Status == StatusEntry.Completed)
            {
                EnableViewMode();
                btnCancel.Visible = true;
                chkAgree.Checked = true;
                ltMainTitle.Text = "View";

                // status information
                phStatus.Visible = true;
            }
        }

        // status information
        lbSubmittedOn.Text = entry.DateSubmitted.ToString("dd/MM/yyyy hh:mm tt");
        lbStatusPayment.Text = GeneralFunction.GetPaymentEntryStatus(entry.PayStatus);
        lbStatusEntry.Text = GeneralFunction.GetEntryStatus(entry.Status);
        lbWithdrawStatusEntry.Text = GeneralFunction.GetWithdrawnStatus(entry.WithdrawnStatus);
        if (lbWithdrawStatusEntry.Text == "") lbWithdrawStatusEntry.Text = "-";
        ddlWithdraw.SelectedValue = entry.WithdrawnStatus;
        lbRound2.Text = "No";
        ddlRound2.SelectedValue = "No";
        if (entry.IsRound2)
        {
            lbRound2.Text = "Yes";
            ddlRound2.SelectedValue = "Yes";
        }
        lbCategoryR2.Text = "None";
        ddlCategoryR2.Text = "None";
        if (entry.CategoryMarketR2 != "" && entry.CategoryPSR2 != "" && entry.CategoryPSDetailR2 != "")
        {
            lbCategoryR2.Text = GeneralFunction.ConvertToCategoryPSDetailFull(entry.CategoryMarketR2, entry.CategoryPSR2, entry.CategoryPSDetailR2);
            ddlCategoryR2.Text = GeneralFunction.ConvertToCategoryPSDetailFull(entry.CategoryMarketR2, entry.CategoryPSR2, entry.CategoryPSDetailR2);
        }


        // Reset any group payment list if g = null
        if (Request.QueryString["g"] == null) GeneralFunction.ResetGroupPaymentCache();

        // If payment group cache contains 10 items, disable the Save and Add More button
        List<Guid> list = GeneralFunction.GetGroupPaymentListCache();
        if (list != null && list.Count >= 10) btnSaveAdd.Visible = false;

        // Is Submission over for registrant
        if (GeneralFunction.IsEntrantSubmissionCutOff())
        {
            btnSaveDraft.Visible = false;
            btnNext.Visible = false;
            btnEdit.Visible = false;
            btnEdit2.Visible = false;
            btnConfirm.Visible = false;
            btnConfirm2.Visible = false;
            btnSaveAdd.Visible = false;
        }

        SustainedSuccessCase();
    }

    private void BindCompanyCreditGrid()
    {
        List<CompanyCredit> list = GeneralFunction.GetCCListCache().OrderBy(x => x.ContactType).ToList();

        gvCC.DataSource = list;
        gvCC.DataBind();

        ShowCCAddButton();

        // Max 8
        if (gvCC.Rows.Count >= 8) phAddCC.Visible = false;
        
        rptIndCredits.DataSource = GeneralFunction.UpdateICFromCCCache(rptIndCredits);
        rptIndCredits.DataBind();
    }

    private void BindAdminRemarks()
    {
        lbAdminRemarks.Text = "";
        EntryRemarksList remarksList = EntryRemarksList.GetEntryRemarksList(entry.Id);
        foreach (EntryRemarks remarks in remarksList)
        {
            string RemarkBy = remarks.DateCreated.ToString("dd/MM/yy hh:mm tt");
            if (remarks.isAdmin)
            {
                try { RemarkBy += " (" + Administrator.GetAdministrator(remarks.CommentatorID).LoginId + ")"; }
                catch { }
            }
            else
            {
                try { RemarkBy += " (" + Registration.GetRegistration(remarks.CommentatorID).Firstname + ")"; }
                catch { }
            }

            lbAdminRemarks.Text += "<tr><td width='200px' style='vertical-align:top'>" + RemarkBy + " </td><td width='5px'> : </td><td>" + remarks.Remarks + "</td></tr>";
        }

        if (lbAdminRemarks.Text == "")
            lbAdminRemarks.Text = "None";
        else
            lbAdminRemarks.Text = "<table width='100%' style='font-size:10px;'>" + lbAdminRemarks.Text + "</table>";
    }

    #region Events

    private void CCSave_Clicked(object sender, EventArgs e)
    {
        Controls_CompanyCredit control = (Controls_CompanyCredit)sender;
        CompanyCredit cc = SaveCCForm(control);

        // validate the control form
        if (!control.ValidateForm()) return;

        // ok all valid
        cc.Status = StatusEntry.Completed;

        if (control.IsNew)
        {           
            if (!GeneralFunction.IsCCInListCache(cc))
                GeneralFunction.AddCCtoListCache(cc);
            else
                GeneralFunction.ReplaceKeyCCtoListCache2(cc);

        }
        else
            GeneralFunction.UpdateCCtoListCache(cc);

        GeneralFunction.OrderCCListCache();
        BindCompanyCreditGrid();
        control.Visible = false;
        phCC.Visible = false;
    }

    private void CCCancel_Clicked(object sender, EventArgs e)
    {
        Controls_CompanyCredit control = (Controls_CompanyCredit)sender;
        control.Visible = false;
        phCC.Visible = false;
        ShowCCAddButton();
    }

    protected void rblCategoryMarket_SelectedIndexChanged(object sender, EventArgs e)
    {
        rblCategoryPS.SelectedIndex = -1;
        PopulateCategoryData();
    }

    protected void rblCategoryPS_SelectedIndexChanged(object sender, EventArgs e)
    {
        PreLoadCategoryDetailList();
        ToggleEffectiveness();
    }

    protected void ddlCategoryPSDetail_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlCategoryPSDetail.SelectedValue == "Real Time Marketing")
        //{
        //    phSocialPlatforms.Visible = true;
        //}
        //else
        //{
        //    phSocialPlatforms.Visible = false;
        //    cblSocialPlatforms.SelectedIndex = -1;
        //    txtSocialPlatformsOthers.Text = "";
        //}

        SustainedSuccessCase();
        
        PopulateDateList();
        ToggleEffectiveness();

    }

    protected void SustainedSuccessCase()
    {
        string msg = "", MsgDate = "", MsgEndDate = "", msgEffort1 = "", msgEffort2 = "";
        DateTime specialCaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["SpecialCaseStartDate"].ToString());
        DateTime CaseStartDateSustainedSuccess = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseStartDateSustainedSuccess"].ToString());
        DateTime CaseEndDateSustainedSuccess = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseEndDateSustainedSuccess"].ToString());
        DateTime caseEndDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseEndDate"].ToString());
        DateTime normalCaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseStartDate"].ToString());
        if (ddlCategoryPSDetail.SelectedValue == "Sustained Success")
        {
            msgEffort1 = ""; msgEffort2 = "";
            CurrentCaseStartDate = CaseStartDateSustainedSuccess;
            lblCaseStartDate.Text = CaseStartDateSustainedSuccess.ToString("MMMM d, yyyy");
            MsgDate = CaseStartDateSustainedSuccess.ToString("MMMM d, yyyy");
            msg = "For Sustained Success category, results must minimally date back to 31 August 2016, with 3 years of data presented.";

            lblCaseEndDate.Text = CaseEndDateSustainedSuccess.ToString("MMMM d, yyyy");
            MsgEndDate = CaseEndDateSustainedSuccess.ToString("MMMM d, yyyy");
        }
        else
        {
            //msgEffort1 = "if your effort launched September 15-30, 2016, you may include data/work from that time period."; msgEffort2 = "";
            //msgEffort2 = "if your effort ended October 1-15, 2017, you may include data/work from that time period.";
            msgEffort1 = ""; msgEffort2 = "";
            CurrentCaseStartDate = normalCaseStartDate;
            lblCaseStartDate.Text = normalCaseStartDate.ToString("MMMM d, yyyy");
            MsgDate = normalCaseStartDate.ToString("MMMM d, yyyy");
            msg = "";

            lblCaseEndDate.Text = caseEndDate.ToString("MMMM d, yyyy");
            MsgEndDate = caseEndDate.ToString("MMMM d, yyyy");
        }
        string jsFunc = "LabelMsg('" + lblSpecialCaseEndDate.ClientID + "','" + msg + "')";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call1", jsFunc, true);

        jsFunc = "LabelMsg('" + lblCaseStartDate.ClientID + "','" + MsgDate + "')";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call2", jsFunc, true);

        jsFunc = "LabelMsg('" + lblCaseEndDate.ClientID + "','" + MsgEndDate + "')";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call5", jsFunc, true);

        jsFunc = "LabelMsg('" + liteffort1.ClientID + "','" + msgEffort1 + "')";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call3", jsFunc, true);

        jsFunc = "LabelMsg('" + liteffort2.ClientID + "','" + msgEffort2 + "')";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call4", jsFunc, true);

    }

    protected void btnAddCC_Click(object sender, EventArgs e)
    {
        cc1.ResetForm();
        cc1.ContactType = "";
        cc1.RecordId = Guid.NewGuid();
        cc1.No = GeneralFunction.NumberOfCCInListCache() + 1;
        cc1.EntryId = entry.Id;
        cc1.DateCreated = DateTime.Now;
        cc1.IsNew = true;
        cc1.PopulateForm();
        cc1.Visible = true;
        phCC.Visible = true;

        phAddCC.Visible = false;
    }

    protected void gvCC_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        CompanyCredit cc = (CompanyCredit)e.Row.DataItem;
        if (cc != null)
        {
            LinkButton lnkView = (LinkButton)e.Row.FindControl("lnkView");
            if (lnkView != null)
            {
                lnkView.CommandArgument = cc.Id.ToString();
            }
            LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
            if (lnkEdit != null)
            {
                lnkEdit.CommandArgument = cc.Id.ToString();

                // if draft, change it to Add
                if (cc.Status == StatusEntry.Draft)
                {
                    lnkEdit.Text = "Add";
                    lnkEdit.CommandName = "add";
                }
            }
            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            if (lnkDelete != null)
            {
                lnkDelete.CommandArgument = cc.Id.ToString();
                lnkDelete.Attributes.Add("onclick", "return confirm('Confirm to delete?');");

                if (GeneralFunction.NumberOfClientInListCache() > 1)
                {
                    if (cc.No <= 3) lnkDelete.Visible = false;

                    if (cc.No == 2) lnkDelete.Visible = true;
                }
                else
                {
                    if (cc.No <= 2) lnkDelete.Visible = false; // 1st 2 cc no delete
                }
            }

            // overwrite the company name to Required if its draft
            if (cc.Status == StatusEntry.Draft)
            {
                e.Row.Cells[1].Text = "Required";
                e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void gvCC_OnRowCommand(object source, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "add")
        {
            // Get the cc object from the session
            CompanyCredit cc = GeneralFunction.GetCCFromCache(new Guid(e.CommandArgument.ToString()));
            if (cc != null)
            {
                phCC.Visible = true;
                cc1.Visible = true;
                PopulateCCForm(cc1, cc);
                cc1.PopulateForm();
                cc1.EnableEditMode();
            }
        }
        if (e.CommandName == "view")
        {
            // Get the cc object from the session
            CompanyCredit cc = GeneralFunction.GetCCFromCache(new Guid(e.CommandArgument.ToString()));
            if (cc != null)
            {
                phCC.Visible = true;
                cc1.Visible = true;
                PopulateCCForm(cc1, cc);
                cc1.PopulateForm();
                cc1.EnableViewMode();
            }
        }
        if (e.CommandName == "modify")
        {
            // Get the cc object from the session
            CompanyCredit cc = GeneralFunction.GetCCFromCache(new Guid(e.CommandArgument.ToString()));
            if (cc != null)
            {
                phCC.Visible = true;
                cc1.Visible = true;
                PopulateCCForm(cc1, cc);
                cc1.PopulateForm();
                cc1.EnableEditMode();
            }
        }
        if (e.CommandName == "remove")
        {
            GeneralFunction.DeleteCCFromListCache(new Guid(e.CommandArgument.ToString()));
            CompanyCredit.DeleteCompanyCredit(new Guid(e.CommandArgument.ToString()));

            GeneralFunction.OrderCCListCache();
            BindCompanyCreditGrid();
        }
    }

    protected void cblTargetAudience_SelectedIndexChanged(object sender, EventArgs e)
    { 
        foreach (ListItem item in cblTargetAudience.Items)
        {
            if (item.Value == "Others")
            {
                if (item.Selected)
                    phcblTargetAudienceOther.Visible = true;
                else
                    phcblTargetAudienceOther.Visible = false;
            }
        }

        PopulateDropdownlist("cblTargetAudience");
    }

    protected void cblObjective_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        PopulateDropdownlist("cblObjective");
    }
    

    protected void cblSDGOption2_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        PopulateDropdownlist("cblSDGOption2");
    }

    protected void cblResearch_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        pnlResearchOther.Visible = false;

        try
        {
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            int index = int.Parse(checkedBox[checkedBox.Length - 1]);

            if (cblResearch.Items[index].Value != "Not Applicable" && cblResearch.Items[index].Selected)
            {
                foreach (ListItem lstItem in cblResearch.Items)
                {
                    if (lstItem.Value == "Not Applicable")
                    {
                        lstItem.Selected = false;
                    }
                }
            }
        }
        catch { }

        foreach (ListItem lstItem in cblResearch.Items)
        {
            if (lstItem.Value == "Not Applicable")
            {
                if (lstItem.Selected)
                {
                    foreach (ListItem lstItem2 in cblResearch.Items)
                    {
                        if (lstItem2.Value != "Not Applicable")
                        {
                            lstItem2.Selected = false;
                        }
                    }
                }
            }
            else if (lstItem.Value == "Others")
            {
                if (lstItem.Selected)
                {
                    pnlResearchOther.Visible = true;
                }
            }
        }

        PopulateDropdownlist("cblResearch");

        foreach (ListItem lstItem in cblResearch.Items)
        {
            if (lstItem.Value == "Not Applicable")
            {
                if (lstItem.Selected)
                {
                    ddlResearchImp.SelectedValue = "Not Applicable";
                }
            }
        }
    }

    protected void ddlProductClassification_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProductClassification.SelectedValue == "Others")
            phddlProductClassificationOther.Visible = true;
        else
            phddlProductClassificationOther.Visible = false;
    }


    protected void ddlTargetAudiencePri_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTargetAudiencePri.SelectedValue == "Others")
            phddlTargetAudiencePriOther.Visible = true;
        else
            phddlTargetAudiencePriOther.Visible = false;
    }


    protected void ddlHeroTouchPoint_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHeroTouchPoint.SelectedValue.Contains("Others"))
            phHeroTouchPointOther.Visible = true;
        else
        {
            phHeroTouchPointOther.Visible = false;
            txtHeroTouchPointOther.Text = "";
        }
    }

    protected void ddlHeroTouchPoint2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHeroTouchPoint2.SelectedValue.Contains("Others"))
            phHeroTouchPointOther2.Visible = true;
        else
        {
            phHeroTouchPointOther2.Visible = false;
            txtHeroTouchPointOther2.Text = "";
        }
    }

    protected void ddlHeroTouchPoint3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHeroTouchPoint3.SelectedValue.Contains("Others"))
            phHeroTouchPointOther3.Visible = true;
        else
        {
            phHeroTouchPointOther3.Visible = false;
            txtHeroTouchPointOther3.Text = "";
        }
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    for (int i = 0; i < CheckBoxList1.Items.Count; i++)
    //    {
    //        if (someCondition)
    //            CheckBoxList1.Items[i].Selected = true;
    //    }
    //}

    protected void ddlEffectiveness_SelectedIndexChanged(object sender, EventArgs e)
    {

        //for (int i = 0; i < cblCaseData.Items.Count; i++)
        //{
        //    if (cblCaseData.Items[i].Value == ddlEffectiveness.SelectedValue)
        //        cblCaseData.Items[i].Selected = true;
        //}
        //PopulateDropdownlist("cblSDGOption2");
        //int count = -1;
        //foreach (ListItem aListItem in cblCaseData.Items)
        //{
        //    count++;
        //    if (ddlEffectiveness.SelectedValue == aListItem.Value)
        //    {
        //        cblCaseData.Items[count].Selected = true;
        //        break;
        //    }
        //}
    }

    protected void cblSocialPlatforms_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (ListItem item in cblSocialPlatforms.Items)
        {
            if (item.Value.Contains("Others") && item.Selected)
                phSocialPlatformsOthers.Visible = true;
            else
                phSocialPlatformsOthers.Visible = false;
        }
    }

    protected void lnkClear_OnClick(object source, EventArgs e)
    {
        LinkButton lnkClear = (LinkButton)source;
        if (lnkClear != null)
        {
            string arg = lnkClear.CommandArgument;

            TextBox txtICName = (TextBox)phIC.FindControl("txtICName" + arg);
            TextBox txtICTitle = (TextBox)phIC.FindControl("txtICTitle" + arg);
            TextBox txtICEmail = (TextBox)phIC.FindControl("txtICEmail" + arg);
            DropDownList ddlICCompany = (DropDownList)phIC.FindControl("ddlICCompany" + arg);

            txtICName.Text = "";
            txtICTitle.Text = "";
            txtICEmail.Text = "";
            ddlICCompany.SelectedValue = "";
        }
    }

    protected void rptIndCredits_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            IndCredit ic = (IndCredit)e.Item.DataItem;

            if (ic != null)
            {
                Label lblCounter = (Label)e.Item.FindControl("lblCounter");
                lblCounter.Text = (e.Item.ItemIndex + 1).ToString().PadLeft(2, '0');
               
                TextBox txtICName = (TextBox)e.Item.FindControl("txtICName");
                TextBox txtICTitle = (TextBox)e.Item.FindControl("txtICTitle");
                TextBox txtICEmail = (TextBox)e.Item.FindControl("txtICEmail");
                DropDownList ddlICCompany = (DropDownList)e.Item.FindControl("ddlICCompany");

                // Get the list of company names from the CC List
                List<CompanyCredit> list = GeneralFunction.GetCompanyListFromCCCache();
                List<CompanyCredit> flist = new List<CompanyCredit>();
                foreach (CompanyCredit companyCC in list)
                {
                    if (!String.IsNullOrEmpty(companyCC.Company))
                        flist.Add(companyCC);
                }
                
                ddlICCompany.DataSource = flist;
                ddlICCompany.DataValueField = "Id";
                ddlICCompany.DataTextField = "Company";
                ddlICCompany.DataBind();

                ddlICCompany.Items.Insert(0, new ListItem("Select",Guid.Empty.ToString()));
                
                txtICName.Text = ic.ContactName;
                txtICTitle.Text = ic.Title;
                txtICEmail.Text = ic.Email;
                ddlICCompany.SelectedValue = ic.UserData1;
            }
        }
    }

    protected void rptIndCredits_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("clear"))
        {
            TextBox txtICName = (TextBox)e.Item.FindControl("txtICName");
            TextBox txtICTitle = (TextBox)e.Item.FindControl("txtICTitle");
            TextBox txtICEmail = (TextBox)e.Item.FindControl("txtICEmail");
            DropDownList ddlICCompany = (DropDownList)e.Item.FindControl("ddlICCompany");

            txtICName.Text = txtICTitle.Text = txtICEmail.Text  = string.Empty;
            ddlICCompany.SelectedValue = Guid.Empty.ToString();
        }
    }

    protected void btnSaveDraft_Click(object sender, EventArgs e)
    {
        if (ValidateForm(true))
        {
            if (SaveForm(StatusEntry.Draft))
                Response.Redirect("Dashboard.aspx");
        }
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (ValidateForm(false))
        {
            GeneralFunction.OrderIC(rptIndCredits); // order the ICs according to the CCs
            SaveForm(StatusEntry.Ready);
            EnableReviewMode();

            //save the entry Id
            ViewState["entryId"] = entry.Id;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        GeneralFunction.ResetCCCache();
        GeneralFunction.ResetGroupPaymentCache();

        Response.Redirect("../Main/Dashboard.aspx");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        EnableEditMode();

        btnCancel.Visible = true;
        btnNext.Visible = true;
        btnSaveDraft.Visible = true;

        phButtonsReviewMode.Visible = false;
        phButtonsReviewDashboardMode.Visible = false;

        lbError.Text = "";
        lbConfirmation.Text = "";
        lbConfirmation2.Text = "";
        ltInstructions.Visible = true;
        ltMainTitle.Text = "Add New";


    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        //SaveForm(StatusEntry.Ready);
        //GeneralFunction.AddIdToGroupPaymentCache(entry.Id);

        Guid entryId = entry.Id;
        
        Entry entryUpdate = Entry.GetEntry(entryId);
        entryUpdate.DateSubmittedString = DateTime.Now.ToString();
        entryUpdate.Save();

        if (ViewState["entryId"] != null) entryId = (Guid)ViewState["entryId"];

        GeneralFunction.AddIdToGroupPaymentCache(entryId);
        Response.Redirect("Summary.aspx");
    }

    protected void btnSaveAdd_Click(object sender, EventArgs e)
    {
        //SaveForm(StatusEntry.Ready);

        Guid entryId = entry.Id;
        if (ViewState["entryId"] != null) entryId = (Guid)ViewState["entryId"];

        GeneralFunction.AddIdToGroupPaymentCache(entryId);
        Response.Redirect("Entry.aspx?g=1"); // inform entry.aspx not to reset group pay cache
    }

    protected void btnAdminSubmitStatus_Click(object sender, EventArgs e)
    {
        // save the wd status
        entry.WithdrawnStatus = ddlWithdraw.SelectedValue;
        if (ddlStatusEntry.Enabled) entry.Status = ddlStatusEntry.SelectedValue;

        if (ddlStatusEntry.SelectedValue == StatusEntry.UploadCompleted && entry.ProcessingStatus != StatusEntry.UploadCompleted)
        {
            Administrator admin = Security.GetAdminLoginSession();
            entry.ProcessingStatus = StatusEntry.Reopened;
            entry.ReopenedBy = admin.Id.ToString();
        }

        // save the round2
        entry.IsRound2 = false;
        if (ddlRound2.SelectedValue == "Yes") entry.IsRound2 = true;

        // save the round2 category
        string market = "", categoryps = "", categorypsdetail = "";
        GeneralFunction.ConvertToCategoryPSDetail(ddlCategoryR2.SelectedValue, ref market, ref categoryps, ref categorypsdetail);
        entry.CategoryMarketR2 = market;
        entry.CategoryPSR2 = categoryps;
        entry.CategoryPSDetailR2 = categorypsdetail;


        entry.Save();
        lbError2.Text = "Status updated.";

    }

    protected void btnAdminSubmitRemarks_Click(object sender, EventArgs e)
    {
        Administrator admin = Security.GetAdminLoginSession();
        Registration reg = Security.GetLoginSessionUser();
        if (txtAdminRemarks.Text.Trim() != "")
        {
            EntryRemarks remarks = EntryRemarks.NewEntryRemarks();
            remarks.Remarks = txtAdminRemarks.Text;
            remarks.EntryId = entry.Id;
            remarks.DateCreatedString = DateTime.Now.ToString();
            if (admin != null)
            {
                remarks.isAdmin = true;
                remarks.CommentatorID = admin.Id;
            }
            else
            {
                remarks.isAdmin = false;
                remarks.CommentatorID = reg.Id;
            }

            remarks.Save();
            txtAdminRemarks.Text = "";
            BindAdminRemarks();
        }
    }

    protected void btnCancelAdmin_Click(object sender, EventArgs e)
    {
        Response.Redirect(GeneralFunction.GetRedirect("../Admin/EntryList.aspx"));
    }

    protected void btnSubmitAdmin_Click(object sender, EventArgs e)
    {
        if (ValidateForm(false))
        {
            SaveForm(""); //do not update the status, leave it as is.
            Response.Redirect(GeneralFunction.GetRedirect("../Admin/EntryList.aspx"));
        }
    }

    #endregion

    #region Helper
    
    private void PopulateIndCredits()
    {
        IndCreditList creditlist = IndCreditList.GetIndCreditList(entry.Id);

        int totalNoOfIC = 10;

        List<IndCredit> newICList = new List<IndCredit>();
        newICList.AddRange(creditlist);

        for (int i = (creditlist.Count + 1); i <= totalNoOfIC; i++)
        {
            IndCredit icCredit = IndCredit.NewIndCredit();
            icCredit.EntryId = entry.Id;
            icCredit.No = i;

            newICList.Add(icCredit);
        }

        rptIndCredits.DataSource = newICList;
        rptIndCredits.DataBind();
    }

    private void PreLoadCategoryDetailList()
    {
        List<string> items = new List<string>();
        DataTable dt = null;
        DivCategoryPSDetail.Visible = true;
        // Single 
        if (rblCategoryMarket.SelectedValue == "SM")
        {
            litProductService.Visible = true;
            // Product/Service cat
            if (rblCategoryPS.SelectedValue == "PSC")
            {
                dt = Category.GetSubcategories("01");
            }

            // Single and Speciality cat
            if (rblCategoryPS.SelectedValue == "SC")
            {
                dt = Category.GetSubcategories("02");
            }
        }

        // Multi 
        if (rblCategoryMarket.SelectedValue == "MM")
        {
            DivCategoryPSDetail.Visible = false;
            litProductService.Visible = false;
            if (rblCategoryPS.SelectedValue == "") {
                entry.CategoryPS = "PSC";
                rblCategoryPS.SelectedValue = entry.CategoryPS;
            }

            // Product/Service cat
            if (rblCategoryPS.SelectedValue == "PSC")
            {
                dt = Category.GetSubcategories("03");
            }

            // Single and Speciality cat
            if (rblCategoryPS.SelectedValue == "SC")
            {
                dt = Category.GetSubcategories("04");
            }
        }

        if (dt != null)
            foreach (DataRow dr in dt.Rows)
                items.Add(dr["Name"].ToString());


        ddlCategoryPSDetail.DataSource = items.OrderBy(m => m).ToList();                
        ddlCategoryPSDetail.DataBind();

        ddlCategoryPSDetail.Items.Insert(0, new ListItem("Select", ""));
    }

    private CompanyCredit SaveCCForm(Controls_CompanyCredit control)
    {
        CompanyCredit cc = null;
        if (control.IsNew)
            cc = CompanyCredit.NewCompanyCredit();
        else
            cc = CompanyCredit.GetCompanyCredit(control.RecordId);

        cc.Id = control.RecordId;
        cc.No = control.No;
        cc.EntryId = control.EntryId;
        cc.ContactType = control.ContactType;
        cc.Company = control.Company;
        cc.Address1 = control.Address1;
        cc.Address2 = control.Address2;
        cc.City = control.City;
        cc.Postal = control.Postal;
        cc.Country = control.Country;
        cc.Salutation = control.Salutation;
        cc.Fullname = control.Fullname;
        cc.Job = control.Job;
        cc.Contact = control.Contact;
        cc.Email = control.Email;
        cc.CompanyType = control.CompanyType;
        cc.CompanyTypeOther = control.CompanyTypeOther;
        cc.ClientCompanyNetwork = control.ClientCompanyNetwork;
        cc.ClientCompanyNetworkOthers = control.ClientCompanyNetworkOthers;
        cc.Network = control.Network;
        cc.NetworkOthers = control.NetworkOther;
        cc.HoldingCompany = control.HoldingCompany;
        cc.HoldingCompanyOthers = control.HoldingCompanyOther;
        cc.DateCreatedString = control.DateCreated.ToString();
        cc.DateModifiedString = control.DateModified.ToString();

        return cc;
    }

    private void PopulateCCForm(Controls_CompanyCredit control, CompanyCredit cc)
    {
        control.RecordId = cc.Id;
        control.No = cc.No;
        control.IsNew = cc.IsNew;
        control.ContactType = cc.ContactType;
        control.Company = cc.Company;
        control.Address1 = cc.Address1;
        control.Address2 = cc.Address2;
        control.City = cc.City;
        control.Postal = cc.Postal;
        control.Country = cc.Country;
        control.Salutation = cc.Salutation;
        control.Fullname = cc.Fullname;
        control.Job = cc.Job;
        control.Contact = cc.Contact;
        control.Email = cc.Email;
        control.CompanyType = cc.CompanyType;
        control.CompanyTypeOther = cc.CompanyTypeOther;
        control.ClientCompanyNetwork = cc.ClientCompanyNetwork;
        control.ClientCompanyNetworkOthers = cc.ClientCompanyNetworkOthers;
        control.Network = cc.Network;
        control.NetworkOther = cc.NetworkOthers;
        control.HoldingCompany = cc.HoldingCompany;
        control.HoldingCompanyOther = cc.HoldingCompanyOthers;
        control.DateCreated = cc.DateCreated;
        control.DateModified = cc.DateModified;
    }

    protected void ValidateDateCase()
    {
        #region Old Validation
        //if ((chkIsCampaignOngoing.Checked == false) &&
        //    (ddlEndDay.SelectedValue.Trim().Equals("") || ddlEndMonth.SelectedValue.Trim().Equals("") || ddlEndYear.SelectedValue.Trim().Equals("") ||
        //    ddlStartDay.SelectedValue.Trim().Equals("") || ddlStartMonth.SelectedValue.Trim().Equals("") || ddlStartYear.SelectedValue.Trim().Equals("")))
        //{
        //    if (ddlEndDay.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlEndDay);

        //    if (ddlEndMonth.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlEndMonth);

        //    if (ddlEndYear.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlEndYear);

        //    if (ddlStartDay.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlStartDay);

        //    if (ddlStartMonth.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlStartMonth);

        //    if (ddlStartYear.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlStartYear);

        //    lbError.Text += "Campaign dates are incomplete.<br>";
        //}
        //else if ((chkIsCampaignOngoing.Checked == true) &&
        //    ddlStartDay.SelectedValue.Trim().Equals("") || ddlStartMonth.SelectedValue.Trim().Equals("") || ddlStartYear.SelectedValue.Trim().Equals(""))
        //{
        //    if (ddlStartDay.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlStartDay);

        //    if (ddlStartMonth.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlStartMonth);

        //    if (ddlStartYear.SelectedValue.Trim().Equals(""))
        //        GeneralFunction.HighlightControl(ddlStartYear);

        //    lbError.Text += "Campaign dates are incomplete.<br>";
        //}

        //try
        //{
        //    if (!chkIsCampaignOngoing.Checked)
        //    {
        //        DateTime ddlCaseStartDate = DateTime.Parse(ddlStartMonth.SelectedValue + "/" + ddlStartDay.SelectedValue + "/" + ddlStartYear.SelectedValue + "  08:59:59 PM");
        //        DateTime ddlCaseEndDate = DateTime.Parse(ddlEndMonth.SelectedValue + "/" + ddlEndDay.SelectedValue + "/" + ddlEndYear.SelectedValue + "  12:00:00 AM");

        //        if ((chkIsCampaignOngoing.Checked == false) && ((ddlCaseStartDate > ddlCaseEndDate) ||
        //       ((ddlCategoryPSDetail.SelectedValue == Category.GetCateries().FirstOrDefault(x => x.Prefix == "SS-SS" && x.ColumnId == "02").Name) && ddlCaseStartDate >= caseEndDate.AddYears(-2)) ||
        //       ((ddlCaseEndDate < normalCaseStartDate) || (ddlCaseEndDate > caseEndDate)) || ((ddlCaseStartDate < normalCaseStartDate) || (ddlCaseStartDate > caseEndDate))))
        //        {
        //            if (ddlCaseStartDate < normalCaseStartDate)
        //            {
        //                lbError.Text += "Case Start Date cannot be before " + normalCaseStartDate.ToString("MMMM dd, yyyy") + ".<br/>";
        //                {
        //                    GeneralFunction.HighlightControl(ddlStartDay);
        //                    GeneralFunction.HighlightControl(ddlStartMonth);
        //                    GeneralFunction.HighlightControl(ddlStartYear);
        //                }
        //            }

        //            if (ddlCaseStartDate > caseEndDate)
        //            {
        //                lbError.Text += "Case Start Date cannot be after " + caseEndDate.ToString("MMMM dd, yyyy") + ".<br/>";
        //                {
        //                    GeneralFunction.HighlightControl(ddlStartDay);
        //                    GeneralFunction.HighlightControl(ddlStartMonth);
        //                    GeneralFunction.HighlightControl(ddlStartYear);
        //                }
        //            }

        //            if (ddlCaseEndDate < normalCaseStartDate)
        //            {
        //                lbError.Text += "Case End Date cannot be before " + normalCaseStartDate.ToString("MMMM dd, yyyy") + ".<br/>";
        //                {
        //                    GeneralFunction.HighlightControl(ddlEndDay);
        //                    GeneralFunction.HighlightControl(ddlEndMonth);
        //                    GeneralFunction.HighlightControl(ddlEndYear);
        //                }
        //            }

        //            if (ddlCaseEndDate > caseEndDate)
        //            {
        //                lbError.Text += "Case End Date cannot be after " + caseEndDate.ToString("MMMM dd, yyyy") + ".<br/>";
        //                lbError.Text += "Please Select Ongoing Campaign.<br>";
        //                {
        //                    GeneralFunction.HighlightControl(ddlEndDay);
        //                    GeneralFunction.HighlightControl(ddlEndMonth);
        //                    GeneralFunction.HighlightControl(ddlEndYear);
        //                }
        //            }

        //            //lbError.Text += "Outside of qualifying period.<br>";
        //        }
        //    }
        //    else
        //    {
        //        DateTime ddlCaseStartDate = DateTime.Parse(ddlStartMonth.SelectedValue + "/" + ddlStartDay.SelectedValue + "/" + ddlStartYear.SelectedValue + "  08:59:59 PM");

        //        if ((chkIsCampaignOngoing.Checked == false) ||
        //       ((ddlCategoryPSDetail.SelectedValue == Category.GetCateries().FirstOrDefault(x => x.Prefix == "SS-SS" && x.ColumnId == "02").Name) && ddlCaseStartDate >= caseEndDate.AddYears(-2)) ||
        //       (((ddlCaseStartDate < normalCaseStartDate) || (ddlCaseStartDate > caseEndDate))))
        //        {
        //            if (ddlCaseStartDate < normalCaseStartDate)
        //            {
        //                lbError.Text += "Case Start Date cannot be before " + normalCaseStartDate.ToString("MMMM dd, yyyy") + ".<br/>";
        //                {
        //                    GeneralFunction.HighlightControl(ddlStartDay);
        //                    GeneralFunction.HighlightControl(ddlStartMonth);
        //                    GeneralFunction.HighlightControl(ddlStartYear);
        //                }
        //            }

        //            if (ddlCaseStartDate > caseEndDate)
        //            {
        //                lbError.Text += "Case Start Date cannot be after " + caseEndDate.ToString("MMMM dd, yyyy") + ".<br/>";
        //                {
        //                    GeneralFunction.HighlightControl(ddlStartDay);
        //                    GeneralFunction.HighlightControl(ddlStartMonth);
        //                    GeneralFunction.HighlightControl(ddlStartYear);
        //                }
        //            }
        //        }
        //    }

        //}
        //catch (Exception ex)
        //{
        //    string LogError = ex.Message;
        //}
        #endregion

        if (ddlEndDay.SelectedValue.Trim().Equals("") || ddlEndMonth.SelectedValue.Trim().Equals("") || ddlEndYear.SelectedValue.Trim().Equals("") ||
        ddlStartDay.SelectedValue.Trim().Equals("") || ddlStartMonth.SelectedValue.Trim().Equals("") || ddlStartYear.SelectedValue.Trim().Equals(""))
        {
            if (ddlEndDay.SelectedValue.Trim().Equals(""))
                GeneralFunction.HighlightControl(ddlEndDay);

            if (ddlEndMonth.SelectedValue.Trim().Equals(""))
                GeneralFunction.HighlightControl(ddlEndMonth);

            if (ddlEndYear.SelectedValue.Trim().Equals(""))
                GeneralFunction.HighlightControl(ddlEndYear);

            if (ddlStartDay.SelectedValue.Trim().Equals(""))
                GeneralFunction.HighlightControl(ddlStartDay);

            if (ddlStartMonth.SelectedValue.Trim().Equals(""))
                GeneralFunction.HighlightControl(ddlStartMonth);

            if (ddlStartYear.SelectedValue.Trim().Equals(""))
                GeneralFunction.HighlightControl(ddlStartYear);

            lbError.Text += "Campaign dates are incomplete.<br>";
        }
        else
        {
            try
            {
                DateTime ddlCaseStartDate = DateTime.Parse(ddlStartMonth.SelectedValue + "/" + ddlStartDay.SelectedValue + "/" + ddlStartYear.SelectedValue + "  12:00:00 AM");

                try
                {
                    DateTime ddlCaseEndDate = DateTime.Parse(ddlEndMonth.SelectedValue + "/" + ddlEndDay.SelectedValue + "/" + ddlEndYear.SelectedValue + "  12:00:00 AM");
                }
                catch (Exception ex)
                {
                    GeneralFunction.HighlightControl(ddlEndDay);
                    GeneralFunction.HighlightControl(ddlEndMonth);
                    GeneralFunction.HighlightControl(ddlEndYear);

                    lbError.Text += "Date does not exist<br/>";
                }
            }
            catch (Exception ex)
            {
                GeneralFunction.HighlightControl(ddlStartDay);
                GeneralFunction.HighlightControl(ddlStartMonth);
                GeneralFunction.HighlightControl(ddlStartYear);

                lbError.Text += "Date does not exist<br/>";
            }
        }

        #region Comment this for a While 

        //if (ddlCategoryPSDetail.SelectedValue == "Sustained Success")
        //{
        //    try
        //    {
        //        DateTime ddlCaseStartDate = DateTime.Parse(ddlStartMonth.SelectedValue + "/" + ddlStartDay.SelectedValue + "/" + ddlStartYear.SelectedValue + "  12:00:00 AM");
        //        DateTime ddlCaseEndDate = DateTime.Parse(ddlEndMonth.SelectedValue + "/" + ddlEndDay.SelectedValue + "/" + ddlEndYear.SelectedValue + "  12:00:00 AM");

        //        if (ddlCaseStartDate > ddlCaseEndDate)
        //        {
        //            GeneralFunction.HighlightControl(ddlStartDay);
        //            GeneralFunction.HighlightControl(ddlStartMonth);
        //            GeneralFunction.HighlightControl(ddlStartYear);

        //            GeneralFunction.HighlightControl(ddlEndDay);
        //            GeneralFunction.HighlightControl(ddlEndMonth);
        //            GeneralFunction.HighlightControl(ddlEndYear);

        //            lbError.Text += "Case start date cannot be before end date.<br/>";
        //        }
        //        else if (ddlCaseStartDate > DateTime.Parse("9/30/2015  12:00:00 AM"))
        //        {
        //            GeneralFunction.HighlightControl(ddlStartDay);
        //            GeneralFunction.HighlightControl(ddlStartMonth);
        //            GeneralFunction.HighlightControl(ddlStartYear);

        //            lbError.Text += "Case start date cannot be after 30 Sept 2015.<br/>";
        //        }
        //        else if (ddlCaseEndDate < DateTime.Parse("7/1/2016  12:00:00 AM") /*|| ddlCaseEndDate > DateTime.Parse("9/30/2017  12:00:00 AM")*/)
        //        {
        //            GeneralFunction.HighlightControl(ddlEndDay);
        //            GeneralFunction.HighlightControl(ddlEndMonth);
        //            GeneralFunction.HighlightControl(ddlEndYear);

        //            lbError.Text += "Case end date must be within the Qualifying Period.<br/>";
        //        }
        //        else if (ddlCaseStartDate.Year + 2 > ddlCaseEndDate.Year)
        //        {
        //            GeneralFunction.HighlightControl(ddlStartDay);
        //            GeneralFunction.HighlightControl(ddlStartMonth);
        //            GeneralFunction.HighlightControl(ddlStartYear);

        //            GeneralFunction.HighlightControl(ddlEndDay);
        //            GeneralFunction.HighlightControl(ddlEndMonth);
        //            GeneralFunction.HighlightControl(ddlEndYear);

        //            lbError.Text += "Requires 3 years of data.<br/>";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string LogError = ex.Message;
        //    }
        //}
        //else
        #endregion

        {
            try
            {
                DateTime ddlCaseStartDate = DateTime.Parse(ddlStartMonth.SelectedValue + "/" + ddlStartDay.SelectedValue + "/" + ddlStartYear.SelectedValue + "  12:00:00 AM");
                DateTime ddlCaseEndDate = DateTime.Parse(ddlEndMonth.SelectedValue + "/" + ddlEndDay.SelectedValue + "/" + ddlEndYear.SelectedValue + "  12:00:00 AM");
                DateTime specialCaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["SpecialCaseStartDate"].ToString());
                DateTime caseEndDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseEndDate"].ToString());
                DateTime CaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseStartDate"].ToString());

                if (ddlCaseStartDate > ddlCaseEndDate)
                {
                    GeneralFunction.HighlightControl(ddlStartDay);
                    GeneralFunction.HighlightControl(ddlStartMonth);
                    GeneralFunction.HighlightControl(ddlStartYear);

                    GeneralFunction.HighlightControl(ddlEndDay);
                    GeneralFunction.HighlightControl(ddlEndMonth);
                    GeneralFunction.HighlightControl(ddlEndYear);
                    lbError.Text += "Case start date cannot be after end date.<br/>";
                }
                if (ddlCaseStartDate > caseEndDate)
                {
                    GeneralFunction.HighlightControl(ddlStartDay);
                    GeneralFunction.HighlightControl(ddlStartDay);
                    GeneralFunction.HighlightControl(ddlStartDay);

                    lbError.Text += "Case start date cannot be after eligibility period.<br/>";
                }
                if (ddlCaseEndDate < CaseStartDate)
                {
                    GeneralFunction.HighlightControl(ddlEndDay);
                    GeneralFunction.HighlightControl(ddlEndMonth);
                    GeneralFunction.HighlightControl(ddlEndYear);

                    lbError.Text += "Case end date cannot be before eligibility period.<br/>";
                }
                #region Comment This For A While
                //else if (ddlCaseStartDate < normalCaseStartDate)
                //{
                //    GeneralFunction.HighlightControl(ddlStartDay);
                //    GeneralFunction.HighlightControl(ddlStartMonth);
                //    GeneralFunction.HighlightControl(ddlStartYear);

                //    lbError.Text += "Case ran must be within the Qualifying Period.<br/>";
                //}
                //else if (ddlCaseEndDate > caseEndDate /*DateTime.Parse("7/1/2016  12:00:00 AM")*/ /*|| ddlCaseEndDate > DateTime.Parse("9/30/2017  12:00:00 AM")*/)
                //{
                //    GeneralFunction.HighlightControl(ddlEndDay);
                //    GeneralFunction.HighlightControl(ddlEndMonth);
                //    GeneralFunction.HighlightControl(ddlEndYear);

                //    lbError.Text += "Case end date must be within the Qualifying Period.<br/>";
                //}
                #endregion
            }
            catch (Exception ex)
            {
                string LogError = ex.Message;
            }
        }

    }


    private bool ValidateForm(bool isDraft)
    {
        lbError.Text = "";
        lbError2.Text = "";
        GeneralFunction.RemoveHighlightControls(this);

        lbError.Text += GeneralFunction.ValidateTextBox("Campaign Title", txtCampaign, true, "string");
        

        DateTime normalCaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseStartDate"].ToString());
        DateTime specialCaseStartDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["SpecialCaseStartDate"].ToString());
        DateTime caseEndDate = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseEndDate"].ToString());

        // Advance validation against other entries of the same reg
        string adv = GeneralFunction.ValidateNewEntryAgainstPrevious(entry.Id, Security.GetLoginSessionUser().Id, txtCampaign.Text.Trim(), rblCategoryMarket.SelectedValue, rblCategoryPS.SelectedValue, ddlCategoryPSDetail.SelectedValue, txtBrand.Text.Trim());
        if (txtCampaign.Text.Trim() != "" && adv != "")
        {
            lbError.Text += adv + "<br />";
            GeneralFunction.HighlightControl(txtCampaign);
            GeneralFunction.HighlightControl(rblCategoryPS);
            GeneralFunction.HighlightControl(ddlCategoryPSDetail);
        }

        if (!isDraft)
        {
            ValidateDateCase(); //DATE VALIDATE

            // case summary word count
            try
            {
                int pcount = int.Parse(hldProfileCount.Value);
            }
            catch
            {
                lbError.Text += "Case Summary has exceeded the 90-word maximum.<br />";
            }


            // Basic validation

            lbError.Text += GeneralFunction.ValidateTextBox("Client", txtClient, true, "string");
            lbError.Text += GeneralFunction.ValidateTextBox("Brand", txtBrand, true, "string");

            if (rblCategoryMarket.SelectedValue == "MM")
            {
                lbError.Text += GeneralFunction.ValidateRadioButtonList("Market Category", rblCategoryMarket, true);
            }
            else
            {
                lbError.Text += GeneralFunction.ValidateRadioButtonList("Products & Services Category or Specialty Category", rblCategoryMarket, true);
            }

            
            lbError.Text += GeneralFunction.ValidateRadioButtonList("Speciality Category", rblCategoryPS, true);

            if(DivCategoryPSDetail.Visible == true)
                lbError.Text += GeneralFunction.ValidateDropDownList("Category", ddlCategoryPSDetail, true, "");


            if (rblCategoryMarket.SelectedValue == "SM")
            {
                    lbError.Text += GeneralFunction.ValidateDropDownList("Market Effectiveness", ddlEffectiveness, true, "");
            }

            if (rblCategoryMarket.SelectedValue == "MM")
            {
                lbError.Text += GeneralFunction.ValidateCheckBoxList("Market Effectiveness", cblEffectiveness, true);
                int counter = 0;
                foreach (ListItem item in cblEffectiveness.Items)
                {
                    if (item.Selected) counter++;
                }
                if (counter < 2)
                {
                    lbError.Text += "Minimum of 2 markets must be selected for Market Effectiveness.<br/>";
                    GeneralFunction.HighlightControl(cblEffectiveness);
                }
                if (counter > 3)
                {
                    lbError.Text += "Maximum of 3 markets can be selected for Market Effectiveness.<br/>";
                    GeneralFunction.HighlightControl(cblEffectiveness);
                }
            }


            lbError.Text += GeneralFunction.ValidateDropDownList("Representative Salutation", ddlRepSalutation, true, "");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Firstname", txtRepFirstname, true, "string");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Lastname", txtRepLastname, true, "string");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Job", txtRepJob, true, "string");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Company", txtRepCompany, true, "string");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Contact Country Code", txtRepContactCountry, false, "phoneNumber");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Contact Area Code", txtRepContactArea, false, "phoneNumber");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Contact Number", txtRepContactNumber, true, "phoneNumber");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Mobile Country Code", txtRepMobileCountry, false, "phoneNumber");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Mobile Area Code", txtRepMobileArea, false, "phoneNumber");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Mobile Number", txtRepMobileNumber, false, "phoneNumber");
            lbError.Text += GeneralFunction.ValidateTextBox("Representative Email", txtRepEmail, true, "EmailSingle");

            lbError.Text += GeneralFunction.ValidateTextBox("Case Summary", txtSummary, true, "string");


            // CC
            if (GeneralFunction.GetCCListCache() == null)
                lbError.Text += "At least 2 Company Credits are required.<br/>";
            else
            {
                List<CompanyCredit> cclist = GeneralFunction.GetCCListCache();
                int ccount = 0;
                foreach (CompanyCredit cc in cclist)
                {
                    if (cc.Company.Trim() != "") ccount++;
                }
                if (ccount < 2) lbError.Text += "At least 2 Company Credits are required.<br/>";
            }

            // IC
            lbError.Text += ValidateIndCredits();
            
            // IC Full Data
            int ic_count = ValidateIndCreditsFullData();
            
            if (ic_count < 2)
            {
                lbError.Text += "A minimum of 2 Individual Credits must be entered.<br/>";
                GeneralFunction.HighlightControl(rptIndCredits.Items[0].FindControl("txtICName"));
                GeneralFunction.HighlightControl(rptIndCredits.Items[0].FindControl("txtICTitle"));
                GeneralFunction.HighlightControl(rptIndCredits.Items[0].FindControl("txtICEmail"));
                GeneralFunction.HighlightControl(rptIndCredits.Items[0].FindControl("ddlICCompany"));
            }

            lbError.Text += GeneralFunction.ValidateDropDownList("Product Classification", ddlProductClassification, true, "");
            lbError.Text += GeneralFunction.ValidateCheckBoxList("Case Objective", cblObjective, true);
            lbError.Text += GeneralFunction.ValidateDropDownList("Overarching Case Objective", ddlObjective2, true, "");

            lbError.Text += GeneralFunction.ValidateCheckBoxList("Target Audience", cblTargetAudience, true);
            lbError.Text += GeneralFunction.ValidateDropDownList("Primary Target Audience", ddlTargetAudiencePri, true, "");
            foreach (ListItem item in cblTargetAudience.Items)
            {
                if (item.Value == "Others" && item.Selected)
                {
                    lbError.Text += GeneralFunction.ValidateTextBox("Target Audience Others", txtTargetAudienceOther, true, "string");
                    break;
                }
            }
            if (ddlTargetAudiencePri.SelectedValue == "Others") lbError.Text += GeneralFunction.ValidateTextBox("Primary Target Audience Others", txtTargetAudiencePriOther, true, "string");


            // Hero touchpoints
            lbError.Text += GeneralFunction.ValidateDropDownList("Hero Touch Point A", ddlHeroTouchPoint, true, "");
            if (ddlHeroTouchPoint.SelectedValue.Contains("Others")) lbError.Text += GeneralFunction.ValidateTextBox("Hero Touch Point A Others", txtHeroTouchPointOther, true, "string");

            lbError.Text += GeneralFunction.ValidateDropDownList("Hero Touch Point B", ddlHeroTouchPoint2, true, "");
            if (ddlHeroTouchPoint2.SelectedValue.Contains("Others")) lbError.Text += GeneralFunction.ValidateTextBox("Hero Touch Point B Others", txtHeroTouchPointOther2, true, "string");

            lbError.Text += GeneralFunction.ValidateDropDownList("Hero Touch Point C", ddlHeroTouchPoint3, true, "");
            if (ddlHeroTouchPoint3.SelectedValue.Contains("Others")) lbError.Text += GeneralFunction.ValidateTextBox("Hero Touch Point C Others", txtHeroTouchPointOther3, true, "string");


            // social platforms
            lbError.Text += GeneralFunction.ValidateCheckBoxList("Social Platforms", cblSocialPlatforms, true);
            foreach (ListItem item in cblSocialPlatforms.Items)
            {
                if (item.Value.Contains("Others") && item.Selected)
                {
                    lbError.Text += GeneralFunction.ValidateTextBox("Social Platforms Others", txtSocialPlatformsOthers, true, "string");
                    break;
                }
            }

            // research
            lbError.Text += GeneralFunction.ValidateCheckBoxList("Research", cblResearch, true);
            lbError.Text += GeneralFunction.ValidateDropDownList("Important Research", ddlResearchImp, true, "");
            
            lbError.Text += GeneralFunction.ValidateCheckBoxList("Countr(ies) in which case ran", cblCaseData, true);

            lbError.Text += GeneralFunction.ValidateDropDownList("The goal most closely aligned with your entered case", ddlSDGOption1, true, "");
            lbError.Text += GeneralFunction.ValidateCheckBoxList("Tag your case against all SDGs", cblSDGOption2, true);

            lbError.Text += GeneralFunction.ValidateRadioButtonList("Publishing Policy and Permission", rblPermission, true);
            //lbError.Text += GeneralFunction.ValidateTextBox("Name", txtName, true, "string");
            //lbError.Text += GeneralFunction.ValidateTextBox("Title", txtTitle, true, "string");
            //lbError.Text += GeneralFunction.ValidateTextBox("Company", txtCompany, true, "string");

            if (!chkAgree.Checked)
            {
                lbError.Text += "Please agree to the competition rules.<br/>";
                GeneralFunction.HighlightControl(chkAgree);
            }
        }


        ///////////////////////////////////////////////////

        try
        {
            if (ddlStartYear.SelectedValue != "" && ddlStartMonth.SelectedValue != "" && ddlStartDay.SelectedValue != "")
            {
                entry.DateCampaignStartString = ddlStartMonth.SelectedValue + "/" + ddlStartDay.SelectedValue + "/" + ddlStartYear.SelectedValue;
            }
            else
                entry.DateCampaignStartString = "1/1/2000";
        }
        catch
        {
            GeneralFunction.HighlightControl(ddlStartYear);
            GeneralFunction.HighlightControl(ddlStartMonth);
            GeneralFunction.HighlightControl(ddlStartDay);
            lbError.Text += "Invalid start date.<br/>";
        }

        try
        {
            if (ddlEndYear.SelectedValue != "" && ddlEndMonth.SelectedValue != "" && ddlEndDay.SelectedValue != "")
            {
                entry.DateCampaignEndString = ddlEndMonth.SelectedValue + "/" + ddlEndDay.SelectedValue + "/" + ddlEndYear.SelectedValue;
            }
            else
                entry.DateCampaignEndString = "1/1/2000";
        }
        catch
        {
            GeneralFunction.HighlightControl(ddlEndYear);
            GeneralFunction.HighlightControl(ddlEndMonth);
            GeneralFunction.HighlightControl(ddlEndDay);
            lbError.Text += "Invalid end date.<br/>";
        }

        ToggleEffectiveness();
        lbError2.Text = lbError.Text; // duplicate for the top error messages

        return (lbError.Text == "");
    }
    

    private string ValidateIndCredits()
    {
        string error = string.Empty;

        foreach (RepeaterItem item in rptIndCredits.Items)
        {
            Label lblCounter = (Label)item.FindControl("lblCounter");
            TextBox txtICName = (TextBox)item.FindControl("txtICName");
            TextBox txtICTitle = (TextBox)item.FindControl("txtICTitle");
            TextBox txtICEmail = (TextBox)item.FindControl("txtICEmail");
            DropDownList ddlICCompany = (DropDownList)item.FindControl("ddlICCompany");

            Guid indCCId = Guid.Empty;
            Guid.TryParse(ddlICCompany.SelectedValue, out indCCId);

            if (!String.IsNullOrEmpty(txtICName.Text.Trim()) || !String.IsNullOrEmpty(txtICTitle.Text.Trim()) || !String.IsNullOrEmpty(txtICEmail.Text.Trim()) || indCCId != Guid.Empty)
            {
                error += GeneralFunction.ValidateTextBox(lblCounter.Text + ". Individual Credits - Contact Name", txtICName, true, "string");
                error += GeneralFunction.ValidateTextBox(lblCounter.Text + ". Individual Credits - Title", txtICTitle, true, "string");
                error += GeneralFunction.ValidateTextBox(lblCounter.Text + ". Individual Credits - Email", txtICEmail, true, "EmailSingle");
                error += GeneralFunction.ValidateDropDownList(lblCounter.Text + ". Individual Credits - Company", ddlICCompany, true, Guid.Empty.ToString());
            }
        }

        return error;
    }

    private int ValidateIndCreditsFullData()
    {       
        int indCount = 0;

        foreach (RepeaterItem item in rptIndCredits.Items)
        {
            TextBox txtICName = (TextBox)item.FindControl("txtICName");
            TextBox txtICTitle = (TextBox)item.FindControl("txtICTitle");
            TextBox txtICEmail = (TextBox)item.FindControl("txtICEmail");
            DropDownList ddlICCompany = (DropDownList)item.FindControl("ddlICCompany");

            if (!String.IsNullOrEmpty(txtICName.Text.Trim()) && !String.IsNullOrEmpty(txtICTitle.Text.Trim()) && !String.IsNullOrEmpty(txtICEmail.Text.Trim()) && !String.IsNullOrEmpty(ddlICCompany.SelectedValue))
                indCount++;
        }

        return indCount;
    }

    private bool SaveForm(string status)
    {
        if (!Security.IsUserAdminSpoof()) entry.RegistrationId = Security.GetLoginSessionUser().Id; // update if not admin spoof mode
        entry.Campaign = txtCampaign.Text.Trim();
        entry.Client = txtClient.Text.Trim();
        entry.Brand = txtBrand.Text.Trim();
        entry.CategoryMarket = rblCategoryMarket.SelectedValue;
        entry.CategoryPS = rblCategoryPS.SelectedValue;
        entry.CategoryPSDetail = ddlCategoryPSDetail.SelectedValue;
        if (DivCategoryPSDetail.Visible == false)
            entry.CategoryPSDetail = Category.GetCateries().FirstOrDefault(x => x.Prefix == "MP-PS").Name;

        if (ddlStartYear.SelectedValue != "" && ddlStartMonth.SelectedValue != "" && ddlStartDay.SelectedValue != "")
        {
            entry.DateCampaignStartString = ddlStartMonth.SelectedValue + "/" + ddlStartDay.SelectedValue + "/" + ddlStartYear.SelectedValue;
        }
        else
            entry.DateCampaignStartString = "1/1/2000";

        if (ddlEndYear.SelectedValue != "" && ddlEndMonth.SelectedValue != "" && ddlEndDay.SelectedValue != "")
        {
            entry.DateCampaignEndString = ddlEndMonth.SelectedValue + "/" + ddlEndDay.SelectedValue + "/" + ddlEndYear.SelectedValue;
        }
        else
            entry.DateCampaignEndString = "1/1/2000";

        //if entry IsCampaignOngoing
        entry.IsCampaignOngoing = chkIsCampaignOngoing.Checked;
        //if (chkIsCampaignOngoing.Checked)
        //    entry.DateCampaignEndString = "1/1/2000";

        entry.Effectiveness = ddlEffectiveness.SelectedValue;
        if (rblCategoryMarket.SelectedValue == "MM") entry.Effectiveness = GeneralFunction.GetValueCheckBoxList(cblEffectiveness, "|");

        entry.RepSalutation = ddlRepSalutation.SelectedValue;
        entry.RepFirstname = txtRepFirstname.Text.Trim();
        entry.RepLastname = txtRepLastname.Text.Trim();
        entry.RepJob = txtRepJob.Text.Trim();
        entry.RepCompany = txtRepCompany.Text.Trim();
        entry.RepContact = GeneralFunction.CreateContact(txtRepContactCountry.Text.Trim(), txtRepContactArea.Text.Trim(), txtRepContactNumber.Text.Trim());
        entry.RepMobile = GeneralFunction.CreateContact(txtRepMobileCountry.Text.Trim(), txtRepMobileArea.Text.Trim(), txtRepMobileNumber.Text.Trim());
        entry.RepEmail = txtRepEmail.Text.Trim();

        entry.Summary = txtSummary.Text.Trim();


        // Wipe out all company credits
        //CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(entry.Id);
        //foreach (CompanyCredit cc in cclist)
        //    CompanyCredit.DeleteCompanyCredit(cc.Id);

        SaveCompanyCredits();



        // Wipe of all ind credits belonging to thsi entry
        IndCreditList creditlist = IndCreditList.GetIndCreditList(entry.Id);
        foreach (IndCredit credit in creditlist)
            IndCredit.DeleteIndCredit(credit.Id);

        SaveIndCredits(entry.Id);

        entry.ProductClassificationOthers = txtProductClassificationOther.Text;
        entry.ProductClassification = ddlProductClassification.SelectedValue;
        entry.EntryObjective = GeneralFunction.GetValueCheckBoxList(cblObjective, "|");
        entry.EntryObjective2 = ddlObjective2.SelectedValue;

        entry.TargetAudience = GeneralFunction.GetValueCheckBoxList(cblTargetAudience, "|");
        entry.TargetAudiencePri = ddlTargetAudiencePri.SelectedValue;
        entry.TargetAudienceOthers = txtTargetAudienceOther.Text.Trim();
        entry.TargetAudiencePriOthers = txtTargetAudiencePriOther.Text.Trim();


        entry.HeroTouchPoint = ddlHeroTouchPoint.SelectedValue;
        entry.HeroTouchPointOthers = txtHeroTouchPointOther.Text.Trim();

        entry.HeroTouchPoint2 = ddlHeroTouchPoint2.SelectedValue;
        entry.HeroTouchPointOthers2 = txtHeroTouchPointOther2.Text.Trim();

        entry.HeroTouchPoint3 = ddlHeroTouchPoint3.SelectedValue;
        entry.HeroTouchPointOthers3 = txtHeroTouchPointOther3.Text.Trim();

        entry.SocialPlatforms = GeneralFunction.GetValueCheckBoxList(cblSocialPlatforms, "|");
        entry.SocialPlatformsOthers = txtSocialPlatformsOthers.Text.Trim();


        entry.Research = GeneralFunction.GetValueCheckBoxList(cblResearch, "|");
        entry.ResearchImp = ddlResearchImp.SelectedValue;
        entry.ResearchOther = txtResearchOther.Text;

        entry.CaseData = GeneralFunction.GetValueCheckBoxList(cblCaseData, "|");

        entry.SDGData1 = ddlSDGOption1.SelectedValue;
        entry.SDGData2 = GeneralFunction.GetValueCheckBoxList(cblSDGOption2, "|");

        entry.Permission = rblPermission.SelectedValue;

        entry.Name = txtName.Text.Trim();
        entry.Title = txtTitle.Text.Trim();
        entry.Company = txtCompany.Text.Trim();

        entry.DateModifiedString = DateTime.Now.ToString();
        if (entry.IsNew) entry.DateCreatedString = DateTime.Now.ToString();

        if (status != "") entry.Status = status;
        entry.WithdrawnStatus = ddlWithdraw.SelectedValue;
        if (status != "") entry.DateSubmittedString = DateTime.Now.ToString();

        if (entry.IsValid)
        {
            Entry OldEntry = null;

            try {
                OldEntry = Entry.GetEntry(entry.Id);
            } catch { }
            
            entry.Save();

            if (OldEntry != null)
                ResetEntryForm(OldEntry, entry);

            // hackish
            // need to ensure that if the user clicks on save draft or next again, it doesnt create a new object into the db
            ViewState["Effie.Entry.SavedPrevious"] = entry.Id;

            return true;
        }
        else
            lbError.Text = entry.BrokenRulesCollection.ToString();

        return false;
    }


    protected void ResetEntryForm(Entry OldEntry, Entry NewEntry)
    {
        try
        {
            EntryForm entryForm = EntryForm.GetEntryForm(Guid.Empty, entry.Id);
            bool isCategoryChanged = (GeneralFunctionEntryForm.GetEntryCategory(NewEntry) != GeneralFunctionEntryForm.GetEntryCategory(OldEntry));
            if (isCategoryChanged)
            {
                NewEntry.Status = StatusEntry.UploadPending;
                NewEntry.Save();
                try
                {
                    string PathEntryForm = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "\\EntryForm\\" + "\\" + entry.Id + "\\";
                    List<ImagesUpload> imageupload = GeneralFunction.GetImageGallery(0, entry, "", false);
                    foreach (ImagesUpload item in imageupload)
                    {
                        string Fileimage = item.path;
                        if (File.Exists(PathEntryForm + Fileimage))
                            File.Delete(PathEntryForm + Fileimage);
                    }

                    EntryFormGallery.DeleteEntryFormGallery(Guid.Empty, entry.Id, "");
                    EntryForm.DeleteEntryForm(entryForm.Id, entryForm.IdEntry);
                }
                catch
                { }
            }
        }
        catch { }
    }

    private void SaveIndCredits(Guid entryId)
    {        
        foreach (RepeaterItem item in rptIndCredits.Items)
        {
            HiddenField hdfCCID = (HiddenField)item.FindControl("hdfCCID");
            Label lblCounter = (Label)item.FindControl("lblCounter");
            TextBox txtICName = (TextBox)item.FindControl("txtICName");
            TextBox txtICTitle = (TextBox)item.FindControl("txtICTitle");
            TextBox txtICEmail = (TextBox)item.FindControl("txtICEmail");
            DropDownList ddlICCompany = (DropDownList)item.FindControl("ddlICCompany");

            // create only if name is present, the rest should have already been validated
            if (!String.IsNullOrEmpty(txtICName.Text))
            {
                IndCredit nc = IndCredit.NewIndCredit();
                nc.EntryId = entryId;
                nc.No = Convert.ToInt32(lblCounter.Text);
                nc.ContactName = txtICName.Text.Trim();
                nc.Title = txtICTitle.Text.Trim();
                nc.Email = txtICEmail.Text.Trim();
                nc.Company = ddlICCompany.SelectedItem.Text;
                nc.DateCreatedString = DateTime.Now.ToString();
                nc.DateModifiedString = DateTime.Now.ToString();
                nc.UserData1 = ddlICCompany.SelectedValue.ToString();

                nc.Save();
            }
        }
    }    

    private void SaveCompanyCredits()
    {
        List<CompanyCredit> cclist = GeneralFunction.GetCCListCache();
        if (cclist != null)
        {
            foreach (CompanyCredit cc in cclist)
            {
                // force all the cc to set the entry id to this current entry id
                cc.EntryId = entry.Id;
                //cc.Status = StatusEntry.Completed;
                if (cc.IsNew) cc.DateCreatedString = DateTime.Now.ToString();
                cc.DateModifiedString = DateTime.Now.ToString();
                cc.Save();
            }            
        }        
    }

    private void EnableViewMode()
    {
        ViewState["Mode"] = "view";
        GeneralFunction.ChangeStateControls(this, false);

        // Hide the Edit and Delete linkbuttons in the CC grid
        foreach (GridViewRow row in gvCC.Rows)
        {
            LinkButton lnkView = (LinkButton)row.FindControl("lnkView");
            LinkButton lnkEdit = (LinkButton)row.FindControl("lnkEdit");
            LinkButton lnkDelete = (LinkButton)row.FindControl("lnkDelete");

            if (lnkView != null && lnkEdit != null && lnkDelete != null)
            {
                lnkView.Enabled = true;
                lnkView.Visible = true;
                lnkEdit.Visible = false;
                lnkDelete.Visible = false;
            }
        }
        phAddCC.Visible = false;

    }

    private void EnableEditMode()
    {
        ViewState["Mode"] = "edit";

        // Hide the view linkbuttons in the CC grid
        int counter = 1;
        foreach (GridViewRow row in gvCC.Rows)
        {
            LinkButton lnkView = (LinkButton)row.FindControl("lnkView");
            LinkButton lnkEdit = (LinkButton)row.FindControl("lnkEdit");
            LinkButton lnkDelete = (LinkButton)row.FindControl("lnkDelete");

            if (lnkView != null && lnkEdit != null && lnkDelete != null)
            {
                lnkView.Visible = false;
                lnkEdit.Visible = true;
                lnkDelete.Visible = true;

                //if (row.Cells[0].Text == "Client" || row.Cells[0].Text == "Lead Agency") lnkDelete.Visible = false;

                if (counter <= 2) lnkDelete.Visible = false;
                counter++;
            }
        }

        GeneralFunction.ChangeStateControls(this, true);
        ToggleEffectiveness();
        ShowCCAddButton();
    }

    private void ShowCCAddButton()
    {
        List<CompanyCredit> list = GeneralFunction.GetCCListCache();

        // if any of the cc is draft then hide the add button
        bool isOK = true;
        if (list.Count > 0 && list[0].Status == StatusEntry.Draft) isOK = isOK & false;
        if (list.Count > 1 && list[1].Status == StatusEntry.Draft) isOK = isOK & false;

        phAddCC.Visible = isOK;

        // Max 8
        if (gvCC.Rows.Count >= 8) phAddCC.Visible = false;

        // if view mode
        if (ViewState["Mode"] != null && ViewState["Mode"].ToString() == "view") phAddCC.Visible = false;
    }

    private void EnableReviewMode()
    {
        EnableViewMode();

        btnCancel.Visible = false;
        btnNext.Visible = false;
        btnSaveDraft.Visible = false;

        //btnEdit.Visible = true;
        //btnConfirm.Visible = true;
        //btnSaveAdd.Visible = true;
        phButtonsReviewMode.Visible = true;

        lbConfirmation.Text = "<p>Please review all fields carefully and make sure all details are correct. Once entries are submitted, any requests to make changes are permitted at the discretion of the Organiser and an admin fee of SGD$200 applies.</p><br/><p>Tip: If you have multiple entries, please return to the Entry Overview page and submit them altogether. This consolidates them into a single invoice and helps to save on the admin charges.</p>";
        lbConfirmation2.Text = lbConfirmation.Text;
        
        ltInstructions.Visible = false;
        ltMainTitle.Text = "Review";

        // special case coming from dashboard
        if (Request.QueryString["db"] == "1")
        {
            phButtonsReviewMode.Visible = false;
            phButtonsReviewDashboardMode.Visible = true;
        }
    }

    private void EnableAdminMode()
    {

    }

    private void PopulateCategoryData()
    {
        //test
        //System.Threading.Thread.Sleep(5000);

        // reset the category selection
        //rblCategoryPS.SelectedIndex = -1;


        PreLoadCategoryDetailList();

        ToggleEffectiveness();
    }

    private void ToggleEffectiveness()
    {
        // toggle effectiveness
        ddlEffectiveness.Visible = false;
        cblEffectiveness.Visible = false;
        lbEffectivenessSM.Visible = false;
        lbEffectivenessMM.Visible = false;
        if (rblCategoryMarket.SelectedValue == "SM")
        {
            ddlEffectiveness.Visible = true;
            lbEffectivenessSM.Visible = true;

            //phCategorySelection.Visible = true;
            rblCategoryPS.Items[1].Enabled = true;
            rblCategoryPS.Items[1].Attributes.Add("style", "display:block");

            // clear the categories
            //ddlCategoryPSDetail.Items.Clear();
        }
        if (rblCategoryMarket.SelectedValue == "MM")
        {
            cblEffectiveness.Visible = true;
            lbEffectivenessMM.Visible = true;

            //phCategorySelection.Visible = false;
            rblCategoryPS.Items[1].Enabled = false;
            rblCategoryPS.Items[1].Attributes.Add("style", "display:none");

            ddlCategoryPSDetail.SelectedIndex = 1;

            //ddlCategoryPSDetail.SelectedIndex = -1;

            // clear the categories
            //ddlCategoryPSDetail.Items.Clear();

        }
    }

    #endregion

    protected void chkIsCampaignOngoing_CheckedChanged(object sender, EventArgs e)
    {
        PopulateDateEnableDisable();
    }

    private void PopulateDateEnableDisable()
    {
        //No need to disable anymore
        //ddlEndDay.Enabled = true;
        //ddlEndMonth.Enabled = true;
        //ddlEndYear.Enabled = true;

        //if (chkIsCampaignOngoing.Checked)
        //{
        //    ddlEndDay.SelectedValue = "";
        //    ddlEndMonth.SelectedValue = "";
        //    ddlEndYear.SelectedValue = "";

        //    ddlEndDay.Enabled = false;
        //    ddlEndMonth.Enabled = false;
        //    ddlEndYear.Enabled = false;
        //}
    }

    private void PopulateDropdownlist(string cbltype)
    {
        List<string> strlist = new List<string>();
        string ddlValue = "";
        //////////////////////////////////////////////////////////////////////////
        if (cbltype == "cblSDGOption2" || cbltype == "ALL")
        {
            foreach (ListItem aListItem in cblSDGOption2.Items)
            {
                if (aListItem.Selected)
                {
                    strlist.Add(aListItem.Text);
                }
            }

            strlist = strlist.OrderBy(q => q).ToList();

            ddlValue = ddlSDGOption1.SelectedValue;
            ddlSDGOption1.Items.Clear();
            ddlSDGOption1.Items.Add(new ListItem("Select", ""));
            foreach (string str in strlist)
            {
                if (ddlSDGOption1.Items.FindByValue(str) == null)
                    ddlSDGOption1.Items.Add(str);
            }
            try
            {
                if (ddlSDGOption1.Items.Count == 2)
                    ddlSDGOption1.SelectedValue = ddlSDGOption1.Items[1].Value;
                else
                    ddlSDGOption1.SelectedValue = "";
            }
            catch { }
        }
        //////////////////////////////////////////////////////////////////////////
        if (cbltype == "cblObjective" || cbltype == "ALL")
        {
            strlist.Clear();
            foreach (ListItem aListItem in cblObjective.Items)
            {
                if (aListItem.Selected)
                {
                    strlist.Add(aListItem.Text);
                }
            }

            strlist = strlist.OrderBy(q => q).ToList();

            ddlValue = ddlObjective2.SelectedValue;
            ddlObjective2.Items.Clear();
            ddlObjective2.Items.Add(new ListItem("Select", ""));
            foreach (string str in strlist)
            {
                if (ddlObjective2.Items.FindByValue(str) == null)
                    ddlObjective2.Items.Add(str);
            }
            try
            {
                if (ddlObjective2.Items.Count == 2)
                    ddlObjective2.SelectedValue = ddlObjective2.Items[1].Value;
                else
                    ddlObjective2.SelectedValue = "";
            }
            catch { }
        }


        //////////////////////////////////////////////////////////////////////////
        if (cbltype == "cblResearch" || cbltype == "ALL")
        {
            strlist.Clear();
            foreach (ListItem aListItem in cblResearch.Items)
            {
                if (aListItem.Selected)
                {
                    strlist.Add(aListItem.Text);
                }
            }

            strlist = strlist.OrderBy(q => q).ToList();
            ddlValue = ddlResearchImp.SelectedValue;
            ddlResearchImp.Items.Clear();
            ddlResearchImp.Items.Add(new ListItem("Select", ""));
            foreach (string str in strlist)
            {
                if (ddlResearchImp.Items.FindByValue(str) == null)
                    ddlResearchImp.Items.Add(str);
            }
            try
            {
                if (ddlResearchImp.Items.Count == 2)
                    ddlResearchImp.SelectedValue = ddlResearchImp.Items[1].Value;
                else
                    ddlResearchImp.SelectedValue = "";
            }
            catch { }
        }
        //////////////////////////////////////////////////////////////////////////
        if (cbltype == "cblTargetAudience" || cbltype == "ALL")
        {
            strlist.Clear();
            foreach (ListItem item in cblTargetAudience.Items)
            {
                if (item.Selected)
                {
                    strlist.Add(item.Text);
                }
            }

            strlist = strlist.OrderBy(q => q).ToList();

            ddlValue = ddlTargetAudiencePri.SelectedValue;
            ddlTargetAudiencePri.Items.Clear();
            ddlTargetAudiencePri.Items.Add(new ListItem("Select", ""));
            foreach (string str in strlist)
            {
                if (ddlTargetAudiencePri.Items.FindByValue(str) == null)
                    ddlTargetAudiencePri.Items.Add(str);
            }
            try
            {
                if (ddlTargetAudiencePri.Items.Count == 2)
                    ddlTargetAudiencePri.SelectedValue = ddlTargetAudiencePri.Items[1].Value;
                else
                    ddlTargetAudiencePri.SelectedValue = "";
            }
            catch { }
        }
        //////////////////////////////////////////////////////////////////////////
    }

    protected void ddlSDGOption1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlSDGOption1.SelectedValue == "Not applicable")
        //{
        //    foreach(ListItem lstItem in cblSDGOption2.Items)
        //    {
        //        if (lstItem.Value == "Not applicable")
        //            lstItem.Selected = true;
        //        else
        //            lstItem.Selected = false;
        //    }
        //}
    }
	

    protected void CheckOngoing_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlEndDay.SelectedValue.Trim().Equals("") && !ddlEndMonth.SelectedValue.Trim().Equals("") && !ddlEndYear.SelectedValue.Trim().Equals(""))
        {
            try
            {
                DateTime ddlCaseEndDate = DateTime.Parse(ddlEndMonth.SelectedValue + "/" + ddlEndDay.SelectedValue + "/" + ddlEndYear.SelectedValue + "  12:00:00 AM");
				//DateTime dateOngoing = DateTime.Parse("09/30/2017  12:00:00 AM");
                DateTime dateOngoing = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["CaseEndDateSustainedSuccess"].ToString());
                if (ddlCaseEndDate > dateOngoing)
                    chkIsCampaignOngoing.Checked = true;
                else
                    chkIsCampaignOngoing.Checked = false;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
