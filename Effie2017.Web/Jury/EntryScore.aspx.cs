using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using System.IO;

public partial class Jury_EntryScore : PageSecurity_Jury
{
    Entry entry;
    Score score;
    EffieJuryManagementApp.Jury jury;
    string round;
    bool isAdminSpoof;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["eId"] != null && Request.QueryString["eId"] != "")
            entry = Entry.GetEntry(new Guid(Request.QueryString["eId"]));
        else
            Response.Redirect("../Jury/Dashboard.aspx");


        if (Request.QueryString["a"] != null && Request.QueryString["a"] != "")
            isAdminSpoof = true;
        else
            isAdminSpoof = false;

        GeneralFunction.GetAllEntryCache(true); // refresh entry cache
        jury = Security.GetLoginSessionJury();
        round = Request.QueryString["r"];
        //score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id);


        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "OnChangeddlRecom" + 
           DateTime.Now.ToString(), "OnChangeddlRecom('" + AdvancementFlag.ClientID + "','" + ddlRecommendation.ClientID + "', '" + round + "');", true);
    }
    private void LoadForm()
    {
        lbWeightSC.Text = GeneralFunction.GetScoreWeightageSC().ToString("N") + "%";
        lbWeightID.Text = GeneralFunction.GetScoreWeightageID().ToString("N") + "%";
        lbWeightIL.Text = GeneralFunction.GetScoreWeightageIL().ToString("N") + "%";
        lbWeightRE.Text = GeneralFunction.GetScoreWeightageRE().ToString("N") + "%";
        
        // round2
        if (round == "2")
        {
            //lbRecommendation.Text = "Recommendation for metals*:";
            //phFlag.Visible = false;
            phCreativeMaterials.Visible = false;
            //AdvancementFlag.Visible = true;
            //trRecommendation.Visible = false;
            //ddlNomination.Visible = true;
        }
        else
        {
            //AdvancementFlag.Visible = false;
            //trRecommendation.Visible = true;
            //ddlNomination.Visible = false;
        }
    }
    private void PopulateForm()
    {
        GeneralFunction.GetAllScoreCache(true);
        score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);
        GeneralFunction.SetddlNomination(ref ddlNomination);
        
        // Jury Info
        lbJuryId.Text = jury.SerialNo;
        lbJuryName.Text = jury.FirstName + " " + jury.LastName;
        lbCompany.Text = jury.Company;
        lbRound.Text = round;

        //string panel = "";
        //if (round == "1") panel = jury.Round1PanelId;
        //if (round == "2") panel = jury.Round2PanelId;
        //lbPanel.Text = panel;

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

        for (int i = 0; i < jurypanelinfoitem.Length; i++)
        {
            if (GeneralFunction.IsInList(jurypanelinfoitem[i], activepanels, '|'))
                lbPanel.Text += jurypanelinfoitem[i] + "&nbsp;";
        }



        lbScoreCompletion.Text = "Pending";
        if (score != null && score.IsSubmitted) lbScoreCompletion.Text = "Completed";


        // Entry Info
        lbEntryId.Text = entry.Serial;
        lbTitle.Text = entry.Campaign;
        lbCategory.Text = GeneralFunction.GetEntryMarket(entry.CategoryMarketFromRound(round)) + "<br/>" + entry.CategoryPSDetailFromRound(round);
        lbBrand.Text = entry.Brand;
        lbClient.Text = entry.Client;

        // uploads - entry form
        #region old method
        //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_PDF.pdf"))
        //{
        //    lnkEntryFormPDF.Visible = true;
        //    lnkEntryFormPDF.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_PDF.pdf?" + DateTime.Now.Ticks.ToString();
        //}
        #endregion
        #region new method
        try
        {
            EntryForm entryForm = EntryForm.GetEntryForm(Guid.Empty, entry.Id);
            if (entryForm.Status != StatusEntry.Draft)
            {
                string url = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?Id=" + entry.Id;

                lnkEntryFormPDF.Visible = true;
                lnkEntryFormPDF.NavigateUrl = url;
            }
        }
        catch
        {
        }
        #endregion

        // entry form word is not shown to jury
        //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_Word.doc"))
        //{
        //    lnkEntryFormDOC.Visible = true;
        //    lnkEntryFormDOC.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_Word.docx";
        //}
        //else if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_Word.docx"))
        //{
        //    lnkEntryFormDOC.Visible = true;
        //    lnkEntryFormDOC.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_Word.docx";
        //}


        //// uploads - authorization form
        //// not shown to jury, aspx commented out
        //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\" + entry.Serial + "_AuthorizationForm_PDF.pdf"))
        //{
        //    lnkAutPDF.Visible = true;
        //    lnkAutPDF.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Authorisation/" + entry.Id.ToString() + "/" + entry.Serial + "_AuthorizationForm_PDF.pdf";
        //}


        //// uploads - case image
        //// not shown to jury, aspx commented out
        //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpg"))
        //{
        //    lnkCaseImage.Visible = true;
        //    lnkCaseImage.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpeg";
        //}
        //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpeg"))
        //{
        //    lnkCaseImage.Visible = true;
        //    lnkCaseImage.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpeg";
        //}


        // uploads - creative materials
        if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterials_PDF.pdf"))
        {
            lnkCreativePDF.Visible = true;
            lnkCreativePDF.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterials_PDF.pdf?" + DateTime.Now.Ticks.ToString();
        }
        if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterialsTranslate_PDF.pdf"))
        {
            lnkCreativePDFTranslate.Visible = true;
            lnkCreativePDFTranslate.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterialsTranslate_PDF.pdf?" + DateTime.Now.Ticks.ToString();
        }
        //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\"  + entry.Serial + "_CreativeMaterials_Video.mp4"))
        //{
        //    //btnCreativeVid.Visible = true;
        //    //btnCreativeVid.CommandArgument = "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterials_Video.mp4";
        //    lnkCreativeVid.Visible = true;
        //    lnkCreativeVid.NavigateUrl = "../Video/DownloadMedia.aspx?filePath=" + System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + "&MediaID=" + entry.Serial + "_CreativeMaterials_Video.mp4";
        //}
        if (GeneralFunction.FileExistsInAmazonS3(System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"], entry.Serial + "_CreativeMaterials_Video.mp4"))
        {
            //btnCreativeVid.Visible = true;
            //btnCreativeVid.CommandArgument = "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterials_Video.mp4";
            lnkCreativeVid.Visible = true;
            lnkCreativeVid.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["AWSS3WebURL"] + System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"] + "/" + entry.Serial + "_CreativeMaterials_Video.mp4?" + DateTime.Now.Ticks.ToString();
        }
        else 
        if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4"))
        {
            lnkCreativeVid.Visible = true;
            lnkCreativeVid.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4";
        }

        // scores
        if (score != null)
        {
            txtSC.Text = score.ScoreSC.ToString();
            txtID.Text = score.ScoreID.ToString();
            txtIL.Text = score.ScoreIL.ToString();
            txtRE.Text = score.ScoreRE.ToString();
            lbCScoreTotal.Text = score.ScoreComposite.ToString();
            
            // calculations
            lbCScoreSC.Text = GeneralFunction.CalculateSC(score.ScoreSC).ToString("N");
            lbCScoreID.Text = GeneralFunction.CalculateID(score.ScoreID).ToString("N");
            lbCScoreIL.Text = GeneralFunction.CalculateIL(score.ScoreIL).ToString("N");
            lbCScoreRE.Text = GeneralFunction.CalculateRE(score.ScoreRE).ToString("N");
            lbCScoreTotal.Text = GeneralFunction.CalculateCompositeScore(score.ScoreSC, score.ScoreID, score.ScoreIL, score.ScoreRE).ToString("N");
            
            txtFeedbackStrong.Text = score.FeedbackStrong;
            txtFeedbackWeak.Text = score.FeedbackWeak;
            ddlFlag.SelectedValue = score.Flag;
            txtFlagOthers.Text = score.FlagOthers;
            ddlRecuse.SelectedValue = "No";
            if (score.IsRecuse) ddlRecuse.SelectedValue = "Yes";
            txtRecuseRemarks.Text = score.RecuseRemarks;
            txtAdditionalComments.Text = score.AdditionalComments;
            ddlRecommendation.SelectedValue = (score.IsAdvancement) ? "Yes" : "No";
            ddlNomination.SelectedValue = score.Nomination;
        }

        ddlRecommendation.Attributes.Add("onchange", "OnChangeddlRecom('" + AdvancementFlag.ClientID + "','" + ddlRecommendation.ClientID + "', '" + round + "');");
        
       
        // enable or disable
        ddlRecuse_SelectedIndexChanged(null, null);


        // admin spoof to disable the form fields and submission
        //btnSave.Visible = !isAdminSpoof;
        //btnSubmit.Visible = !isAdminSpoof;
        if (isAdminSpoof)
            GeneralFunction.ChangeStateControls(this, false);



        if (score != null && score.IsSubmitted)
        {
            btnSave.Visible = false;
            btnSaveDraft.Visible = false;

            GeneralFunction.ChangeStateControls(this, false);
        }



    }
    private bool ValidateForm(bool isDraft)
    {
        lbError.Text = "";
        GeneralFunction.RemoveHighlightControls(this);


        // Validate scores only if not recused
        if (ddlRecuse.SelectedValue == "No")
        {

            lbError.Text += GeneralFunction.ValidateTextBox("Score for Strategic Challenge & Objectives", txtSC, true, "int");
            lbError.Text += GeneralFunction.ValidateTextBox("Score for Idea", txtID, true, "int");
            lbError.Text += GeneralFunction.ValidateTextBox("Score for Bringing Idea to Life", txtIL, true, "int");
            lbError.Text += GeneralFunction.ValidateTextBox("Score for Results", txtRE, true, "int");


            try
            {
                int sc = int.Parse(txtSC.Text);
                if (sc < 10 || sc > 100) throw(new Exception()) ; //throw error for catching
            }
            catch
            {
                lbError.Text += "SC Score must be a valid number between 10 to 100<br/>";
                GeneralFunction.HighlightControl(txtSC);
            }

            try
            {
                int id = int.Parse(txtID.Text);
                if (id < 10 || id > 100) throw (new Exception()); //throw error for catching
            }
            catch
            {
                lbError.Text += "ID Score must be a valid number between 10 to 100<br/>";
                GeneralFunction.HighlightControl(txtID);
            }

            try
            {
                int il = int.Parse(txtIL.Text);
                if (il < 10 || il > 100) throw (new Exception()); //throw error for catching
            }
            catch
            {
                lbError.Text += "IL Score must be a valid number between 10 to 100<br/>";
                GeneralFunction.HighlightControl(txtIL);
            }

            try
            {
                int re = int.Parse(txtRE.Text);
                if (re < 10 || re > 100) throw (new Exception()); //throw error for catching
            }
            catch
            {
                lbError.Text += "RE Score must be a valid number between 10 to 100<br/>";
                GeneralFunction.HighlightControl(txtRE);
            }

            //if (round == "1")
            {
                string v_advancement = GeneralFunction.ValidateDropDownList("Recommendation for advancement", ddlRecommendation, true, "");
                if (v_advancement != "")
                {
                    lbError.Text += v_advancement;
                    GeneralFunction.HighlightControl(ddlRecommendation);
                }
            }
            
        }
        else
        {
            txtSC.Text = "0";
            txtID.Text = "0";
            txtIL.Text = "0";
            txtRE.Text = "0";

            if (!isDraft) lbError.Text += GeneralFunction.ValidateTextBox("Recuse reason", txtRecuseRemarks, true, "string");
        }


        if (ddlFlag.SelectedValue != "" && !isDraft)
        {
            lbError.Text += GeneralFunction.ValidateTextBox("Flag reason", txtFlagOthers, true, "string");
        }

        if (txtFeedbackStrong.Text.Trim().Length > 1000)
        {
            lbError.Text += "Strongest element of the case cannot exceed 1000 characters.";
            GeneralFunction.HighlightControl(txtFeedbackStrong);
        }

        if (txtFeedbackWeak.Text.Trim().Length > 1000)
        {
            lbError.Text += "Weakest element of the case cannot exceed 1000 characters.";
            GeneralFunction.HighlightControl(txtFeedbackWeak);
        }

        if (txtAdditionalComments.Text.Trim().Length > 1000)
        {
            lbError.Text += "Advice and additional comments cannot exceed 1000 characters.";
            GeneralFunction.HighlightControl(txtAdditionalComments);
        }


        // Is this entry scored by this judege before?
        List<Score> scores = ScoreList.GetScoreList(entry.Id, jury.Id).ToList();
        //if (scores.Count != 0 && scores[0].IsSubmitted)
        if (scores.Count != 0 && scores[0].IsSubmitted && scores[0].Round == round)
        {
            lbError.Text += "Scored before. Score not saved.<br/>";
        }

        if (round == "2" && ddlRecommendation.SelectedValue == "Yes")
        {
            lbError.Text += GeneralFunction.ValidateDropDownList("Nomination", ddlNomination, true, "");
        }

        return (lbError.Text == "");
    }
    private bool SaveForm(bool isDraft)
    {
        GeneralFunction.GetAllScoreCache(true);
        score = GeneralFunction.GetScoreFromMatchingEntryCache(entry, jury.Id, round);


        if (score == null) score = Score.NewScore();

        score.ScoreSC = int.Parse(txtSC.Text.Trim());
        score.ScoreID = int.Parse(txtID.Text.Trim());
        score.ScoreIL = int.Parse(txtIL.Text.Trim());
        score.ScoreRE = int.Parse(txtRE.Text.Trim());
        var TotalScore = GeneralFunction.CalculateCompositeScore(score.ScoreSC, score.ScoreID, score.ScoreIL, score.ScoreRE);
        score.ScoreComposite = double.Parse(TotalScore.ToString());

        score.FeedbackStrong = txtFeedbackStrong.Text.Trim();
        score.FeedbackWeak = txtFeedbackWeak.Text.Trim();
        score.Flag = ddlFlag.SelectedValue;
        score.FlagOthers = txtFlagOthers.Text.Trim();
        score.IsRecuse = (ddlRecuse.SelectedValue == "Yes");
        score.RecuseRemarks = txtRecuseRemarks.Text;
        score.AdditionalComments = txtAdditionalComments.Text;
        score.IsAdvancement = (ddlRecommendation.SelectedValue == "Yes");
        score.Nomination = (ddlRecommendation.SelectedValue == "Yes") ? ddlNomination.SelectedValue : "";
        
        score.EntryId = entry.Id;
        score.Juryid = jury.Id;
        score.IsSubmitted = !isDraft;
        score.Round = round;

        // if recused status will be overrided with Not submitted
        if (score.IsRecuse) score.IsSubmitted = false;

        /*if (score.IsSubmitted) */score.DateSubmittedString = DateTime.Now.ToString();
        if (score.IsNew) score.DateCreatedString = DateTime.Now.ToString();

        if (score.IsValid)
        {
            score.Save();
            return true;
        }

        lbError.Text = score.BrokenRulesCollection.ToString();
        return false;
    }

    #region Events
    //protected void btnCreativeVid_Click(object sender, ImageClickEventArgs e)
    //{
    //    //Session["DownloadFile"] = ((ImageButton)sender).CommandArgument;
    //    //Response.Redirect("../Main/DownloadFile.aspx");
    //    //Response.Redirect("../Main/DownloadFileTypeStream.aspx");
    //}
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //Response.Redirect("../Jury/Dashboard.aspx");
        Response.Redirect(GeneralFunction.GetRedirect("../Jury/Dashboard.aspx"));
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {        
        if (ValidateForm(false))
        {
            if (SaveForm(false))
                Response.Redirect("Dashboard.aspx");
        }
    }
    
    protected void btnSaveDraft_Click(object sender, EventArgs e)
    {
        if (ValidateForm(true))
        {
            if (SaveForm(true))
                Response.Redirect("Dashboard.aspx");
        }
    }
    protected void ddlRecuse_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRecuse.SelectedValue == "Yes")
        {
            DisableScoring();
        }

        if (ddlRecuse.SelectedValue == "No")
        {
            EnableScoring();
        }

    }
    #endregion

    #region Helper
    private void DisableScoring()
    {
        txtSC.Enabled = false;
        txtID.Enabled = false;
        txtIL.Enabled = false;
        txtRE.Enabled = false;
        txtFeedbackStrong.Enabled = false;
        txtFeedbackWeak.Enabled = false;
        txtAdditionalComments.Enabled = false;
        ddlFlag.Enabled = false;
        txtFlagOthers.Enabled = false;
        ddlRecommendation.Enabled = false;
        btnSaveDraft.Visible = false;
        btnSave.Visible = true;
    }
    private void EnableScoring()
    {
        txtSC.Enabled = true;
        txtID.Enabled = true;
        txtIL.Enabled = true;
        txtRE.Enabled = true;
        txtFeedbackStrong.Enabled = true;
        txtFeedbackWeak.Enabled = true;
        txtAdditionalComments.Enabled = true;
        ddlFlag.Enabled = true;
        txtFlagOthers.Enabled = true;
        ddlRecommendation.Enabled = true;
        btnSaveDraft.Visible = true;
        btnSave.Visible = false;
    }
    #endregion



}