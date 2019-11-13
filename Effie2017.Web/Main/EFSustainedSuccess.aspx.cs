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
using System.Web.UI.HtmlControls;
using System.Text;
using Telerik.Web.UI;

public partial class Main_EFSustainedSuccess : System.Web.UI.Page
{
    #region Global Variable
    Entry entry = null;
    List<Data.CollectData> CTPList = new List<Data.CollectData>();
    List<Data.ExplainListOtherMarketingViewmodel> explainOtherMarketingList = new List<Data.ExplainListOtherMarketingViewmodel>();
    string[] Delimiter = { "#&&##&&#", "&&&$$&&&" };
    Guid EntryId = Guid.Empty;
    EntryForm entryForm = null;
    string  CurrentCategory = Data.DocumentCategories.SustainedSuccess.ToString();
    EntryForm CurrentEntryForm = null;
    public static string CurrentTypeImageGallery = "";
    public static int CurrentCountImageGallery = 3;
    public static string PDFURL;
    public static string MsgAlert;
    public static string ActionAlert;
    //public static bool IsPreview = false;
    public static EntryFormGallery entryFormGallery = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
        {
            EntryId = new Guid(Request.QueryString["Id"]);
            //EntryId = new Guid(GeneralFunction.StringDecryption(Request.QueryString["Id"]));
            entry = Entry.GetEntry(EntryId);

            //Check if entry is closed
            if (entry.Status == StatusEntry.Completed)
                Response.Redirect("./Dashboard.aspx");

            PDFURL = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?temp=Preview&Id=" + entry.Id;
            try
            {
                entryForm = EntryForm.GetEntryForm(Guid.Empty, EntryId);
            }
            catch
            {
                entryForm = EntryForm.NewEntryForm();
            }
        }

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
            ViewState["IsPreview"] = false;
            ViewState["SessionKey"] = DateTime.Now.ToString("HHmmss");
        }

        initJavascript();

        PDFURL = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?temp=Preview&Id=" + entry.Id + "&skey=" + (string)ViewState["SessionKey"];
        
        doPostBack(sender, e);
    }

    public void DisableAllAction(Control parent, bool Enabled)
    {
        foreach (Control c in parent.Controls)
        {
            //if (c.GetType() == typeof(Button)) ((Button)c).Enabled = Enabled;
            if (c.GetType() == typeof(DropDownList)) ((DropDownList)c).Enabled = Enabled;
            if (c.GetType() == typeof(TextBox)) ((TextBox)c).Enabled = Enabled;
            if (c.GetType() == typeof(CheckBox)) ((CheckBox)c).Enabled = Enabled;
            if (c.GetType() == typeof(RadioButton)) ((RadioButton)c).Enabled = Enabled;
            if (c.GetType() == typeof(RadioButtonList)) ((RadioButtonList)c).Enabled = Enabled;
            if (c.GetType() == typeof(CheckBoxList)) ((CheckBoxList)c).Enabled = Enabled;

            DisableAllAction(c, Enabled);
        }
    }

    protected void doPostBack(object sender, EventArgs e)
    {
        try
        {
            string parameter = Request["__EVENTARGUMENT"];
            if (parameter == "Preview")
                Preview(true);
            else if (parameter.IndexOf("txt") != -1)
                UploadCharts(parameter);
            else if (parameter == "SaveImage")
                btnSaveImage_Click(sender, e);
            else if (parameter == "Cancel_Alert")
                Cancel_Alert(sender, e);
            else if (parameter == "OK_Alert")
                btnSave_Click(sender, e);
        }
        catch { }
    }

    private void LoadForm()
    {
        CollectCommunicationTouchPoints();
        CollectExplainListOtherMarketing();
        
        // Experience Years
        //List<string> effieExpYears = GeneralFunction.GetEffieExperienceYears();
        //DdlInitialYearPaid.DataSource = effieExpYears; DdlInitialYearPaid.DataBind();
        //DdlInterimYearPaid.DataSource = effieExpYears; DdlInterimYearPaid.DataBind();
        //DdlCurrentYearPaid.DataSource = effieExpYears; DdlCurrentYearPaid.DataBind();

        //DdlInitialYear.DataSource = effieExpYears; DdlInitialYear.DataBind();
        //DdlInterimYear.DataSource = effieExpYears; DdlInterimYear.DataBind();
        //DdlCurrentYear.DataSource = effieExpYears; DdlCurrentYear.DataBind();

        ddlInitialYearPME.Items.Clear();
        ddlInterimYearPME.Items.Clear();
        ddlCurrentYearPME.Items.Clear();

        ddlInitialYearPME.Items.Add(new ListItem("Select Budget", ""));
        ddlInterimYearPME.Items.Add(new ListItem("Select Budget", ""));
        ddlCurrentYearPME.Items.Add(new ListItem("Select Budget", ""));
        foreach (Data.CollectData str in Data.GetPaidMediaExpendituresData())
        {
            ddlInitialYearPME.Items.Add(str.Data2);
            ddlInterimYearPME.Items.Add(str.Data2);
            ddlCurrentYearPME.Items.Add(str.Data2);
        }
		
		TablePreviewImage.Visible = false;
        btnEdit.Visible = false;
        btnSubmit.Visible = false;
        ddlCountryNumber.Items.Add(new ListItem("Select", ""));
        for (int i = 1; i <= 42; i++)
        {
            ddlCountryNumber.Items.Add(i.ToString());
        }
        string Msg = "Any changes will not be saved. Confirm to Proceed ?";
        btnCancel.OnClientClick = "return MsgAlert(\"" + Msg + "\");";
    }

    protected void initJavascript()
    {
        List<TextBoxKeyup> textBoxKeyup = new List<TextBoxKeyup>();
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExecutiveSummary, LimitText = 100, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesA, LimitText = 150, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesB, LimitText = 350, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesC, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesD, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasA, LimitText = 250, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasB, LimitText = 25, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainListOtherMarketing, LimitText = 100, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedA, LimitText = 475, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedB, LimitText = 200, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtAnything, LimitText = 600, isLimitActive = true });
        
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtOwnedMedia, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtSponsorship, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtListAndExplainOtherMarketingText, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtBringingIdea, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasC, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesE, LimitText = 0, isLimitActive = false });
        
        GeneralFunctionEntryForm.InitTextbox(textBoxKeyup, this, entry, ViewState["IsPreview"].ToString());
        
    }
   
    public void CollectCommunicationTouchPoints()
    {
        CTPList = Data.GetCommunicationTouchPointData();

        if (entryForm != null)
        {
            string[] CTPInitialdb = entryForm.CommunicationTouchPointsInitialYear.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] CTPCurrentdb = entryForm.CommunicationTouchPointsCurrentYear.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] CTPInterimdb = entryForm.CommunicationTouchPointsInterimYear.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);

            if (CTPInitialdb.Count() != 0)
            {
                for (int i = 0; i < CTPCurrentdb.Length; i++)
                {
                    if (CTPCurrentdb[i].Split(':')[0] == "999")
                    {
                        cbOtherCTP4.Text = CTPCurrentdb[i].Split(':')[1];
                        break;
                    }

                    string[] Initial = CTPInitialdb[i].Split(':');
                    string[] Interim = CTPInterimdb[i].Split(':');
                    string[] Current = CTPCurrentdb[i].Split(':');
                    
                    if (Initial[0] == "000")
                    {
                        DdlInitialYear.Text = Initial[1];
                        DdlInterimYear.Text = Interim[1];
                        DdlCurrentYear.Text = Current[1];
                    }
                    else if (Initial[0] == "998")
                    {
                        txtOtherCTP1.Text = Initial[1];
                        cbOtherCTP2.Text = Interim[1];
                        cbOtherCTP3.Text = Current[1];
                    }
                    else
                    {
                        try
                        {

                            Data.CollectData CT = CTPList.FirstOrDefault(x => x.id == Initial[0]);
                            CT.Data1 = Initial[1];
                            CT.Data2 = Interim[1];
                            CT.Data3 = Current[1];
                        }
                        catch
                        {
                            rptCTP.DataSource = CTPList;
                        }
                    }
                }
            }
        }

        rptCTP.DataSource = CTPList;
        rptCTP.DataBind();

    }

    public void CollectExplainListOtherMarketing()
    {
        for (int x = 0; x <= Data.ExplainListOtherMarketingData.GetUpperBound(0); x++)
        {
            Data.ExplainListOtherMarketingViewmodel ELOM = new Data.ExplainListOtherMarketingViewmodel();
            ELOM.id = Data.ExplainListOtherMarketingData[x, 0];
            ELOM.MarketingComponents = Data.ExplainListOtherMarketingData[x, 1];
            ELOM.TimePeriod = null;
            explainOtherMarketingList.Add(ELOM);
        }

        if (entryForm != null)
        {
            string[] ELOMG = entryForm.ExplainListOtherMarketing.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            List<Data.ExplainListOtherMarketingViewmodel> explainOtherMarketingListTemp = new List<Data.ExplainListOtherMarketingViewmodel>();

            if (ELOMG.Count() != 0)
            {
                for (int i = 0; i < ELOMG.Length; i++)
                {
                    string[] Datas = ELOMG[i].Split(':');

                    if (Datas[0] == "998")
                    {
                        txtOtherExplainOtherMarket1.Text = Datas[1];
                    }
                    else if (Datas[0] == "999")
                    {
                        txtOtherExplainOtherMarket2.Text = Datas[1];
                    }
                    else
                    {
                        try
                        {
                            Data.ExplainListOtherMarketingViewmodel ELOM = explainOtherMarketingList.FirstOrDefault(x => x.id == Datas[0]);
                            ELOM.TimePeriod = Datas[1];
                            explainOtherMarketingListTemp.Add(ELOM);
                        }
                        catch
                        {
                            //TODO
                        }
                    }
                }
                RPTExplainOtherMarket.DataSource = explainOtherMarketingListTemp;
            }
            else
            {
                RPTExplainOtherMarket.DataSource = explainOtherMarketingList;
            }
        }
        else
        {
            RPTExplainOtherMarket.DataSource = explainOtherMarketingList;
        }
        
        RPTExplainOtherMarket.DataBind();

    }
    
    private void PopulateForm()
    {
        List<string> items = GeneralFunction.GetEffectivenessItems();
        if (entry != null)
        {
            ddlCountryNumber.SelectedValue = (entry.CaseData.Split('|').Count() - 1).ToString();
            txtClassification.Text = entry.ProductClassification;
            TxtCountry1.Text = entry.Effectiveness;
            txtStartdate1.Text = entry.DateCampaignStart.ToString("dd MMM yyyy"); ;
            txtEnddate1.Text = entry.DateCampaignEnd.ToString("dd MMM yyyy"); ;
            lblEntryID.Text = entry.Serial;
            txtCampaign.Text = entry.Campaign;
            txtBrand.Text = entry.Brand;
            lblCategoryMarket.Text = Data.GetCategoryMarket(entry.CategoryMarket);
            txtEntryCategory.Text = entry.CategoryPSDetail;
            this.txtEnddate1.Enabled = false;
            this.txtStartdate1.Enabled = false;
            try
            {
                string countries = "";
                foreach (string country in entry.CaseData.Split('|'))
                {
                    countries += country + ", ";
                }
                litListCountries.Text = countries.Substring(0, countries.Length - 4) + ".";
                lblTotalCountries.Text = (entry.CaseData.Split('|').Count() - 1).ToString();

                //entryForm.DesTotalOfCountries = lblTotalCountries.Text;
            }
            catch
            {
                litListCountries.Text = "No Country Selected.";
                lblTotalCountries.Text = "0";
                //entryForm.DesTotalOfCountries = "0";
            }
        }

        if (entryForm != null)
        {
            if (entryForm.PaidMediaExpendituresCheck != "" && entryForm.PaidMediaExpendituresCheck != null)
            {
                string[] PME = entryForm.PaidMediaExpendituresCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (PME.Length != 0)
                {
                    try
                    {

                        ddlInitialYearPME.SelectedValue = PME[0].Replace("  ", "");
                        ddlInterimYearPME.SelectedValue = PME[1].Replace("  ", "");
                        ddlCurrentYearPME.SelectedValue = PME[2].Replace("  ", "");

                        txtInitialYearPCP.Text = PME[3].Replace("  ", "");
                        txtInterimYearPCP.Text = PME[4].Replace("  ", "");
                        txtCurrentYearPCP.Text = PME[5].Replace("  ", "");

                        //rblComparedInitial.SelectedValue = PME[6].Replace("  ", "");
                        //rblComparedInterim.SelectedValue = PME[7].Replace("  ", "");
                        rblComparedCurrent.SelectedValue = PME[6].Replace("  ", "");

                        //rblComparedoverallInitial.SelectedValue = PME[9].Replace("  ", "");
                        //rblComparedoverallInterim.SelectedValue = PME[10].Replace("  ", "");
                        rblComparedoverallCurrent.SelectedValue = PME[7].Replace("  ", "");
                       
                        DdlInitialYearPaid.Text = PME[8].Replace("  ", "");
                        DdlInterimYearPaid.Text = PME[9].Replace("  ", "");
                        DdlCurrentYearPaid.Text = PME[10].Replace("  ", "");
                    }
                    catch
                    {

                    }
                }
            }

            txtAnything.Text = entryForm.Anything;
            if (entryForm.ExplainWorked != "" && entryForm.ExplainWorked != null)
            {
                string[] ExplainWorked = entryForm.ExplainWorked.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (ExplainWorked.Length != 0)
                {
                    txtExplainWorkedA.Text = ExplainWorked[0].Replace("  ", "");
                    txtExplainWorkedB.Text = ExplainWorked[1].Replace("  ", "");
                }
            }

            txtListAndExplainOtherMarketingText.Text = entryForm.ListAndExplainOtherMarketingText;
            ddlCountryNumber.SelectedValue = string.IsNullOrWhiteSpace(entryForm.DesTotalOfCountries) ? ddlCountryNumber.SelectedValue : entryForm.DesTotalOfCountries;
            txtSponsorship.Text = entryForm.Sponsorship;
            txtOwnedMedia.Text = entryForm.OwnedMedia;
            txtExplainListOtherMarketing.Text = entryForm.Other;
            txtBringingIdea.Text = entryForm.BringingIdea;
            
            if (entryForm.Ideas != "" && entryForm.Ideas != null)
            {
                string[] IdeasList = entryForm.Ideas.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (IdeasList.Length != 0)
                {
                    try {
                        txtIdeasA.Text = IdeasList[0].Replace("  ", "");
                        txtIdeasB.Text = IdeasList[1].Replace("  ", "");
                        txtIdeasC.Text = IdeasList[2].Replace("  ", "");
                    }
                    catch {

                    }
                }
            }

            if (entryForm.StrategicChallengeObjectives != "" && entryForm.StrategicChallengeObjectives != null)
            {
                string[] StrategicChallengeObjectivesList = entryForm.StrategicChallengeObjectives.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (StrategicChallengeObjectivesList.Length != 0)
                {
                    try
                    {
                        txtStrategicChallengeObjectivesA.Text = StrategicChallengeObjectivesList[0].Replace("  ", "");
                        txtStrategicChallengeObjectivesB.Text = StrategicChallengeObjectivesList[1].Replace("  ", "");
                        txtStrategicChallengeObjectivesC.Text = StrategicChallengeObjectivesList[2].Replace("  ", "");
                        txtStrategicChallengeObjectivesD.Text = StrategicChallengeObjectivesList[3].Replace("  ", "");
                        txtStrategicChallengeObjectivesE.Text = StrategicChallengeObjectivesList[4].Replace("  ", "");
                    }
                    catch
                    {

                    }
                }
            }
            
            txtExecutiveSummary.Text = entryForm.ExecutiveSummary;

        }
    }

    protected void rptFileUpload_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("DeleteImage"))
        {
            HiddenField Id = (HiddenField)e.Item.FindControl("ID");
            GeneralFunction.DeleteImagegallery(Id.Value, CurrentCountImageGallery, entry, CurrentTypeImageGallery);
            PreviewImage();

            lblDoneMsg.Visible = true;
            lblDoneMsg.Text = "Delete completed<br>";
        }
    }

    protected void rptFileUpload_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ImagesUpload item = (ImagesUpload)e.Item.DataItem;
            Label Title = (Label)e.Item.FindControl("Title");
            HyperLink PrevImage = (HyperLink)e.Item.FindControl("PrevImage");
            LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
            FileUpload fileupload = (FileUpload)e.Item.FindControl("File");
            HtmlAnchor btnEdit = (HtmlAnchor)e.Item.FindControl("btnEdit");
            HtmlAnchor btnCancel = (HtmlAnchor)e.Item.FindControl("btnCancel");
            
            Label lblMax = (Label)e.Item.FindControl("lblMax");
            HiddenField ID = (HiddenField)e.Item.FindControl("ID");
            
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            PrevImage.Visible = false;
            Title.Text = item.Title;
            ID.Value = item.ID;
            
            if (!string.IsNullOrEmpty(item.path))
            {
                PrevImage.Text = item.FileName;
                lblMax.CssClass = "DisableControl";
                fileupload.CssClass = "DisableControl";
                btnEdit.Attributes.Add("onclick", "EditEntryFormFileUpload(\"" + btnDelete.ClientID + "\",\"" + lblMax.ClientID + "\",\"" + fileupload.ClientID + "\",\"" + btnEdit.ClientID + "\",\"" + PrevImage.ClientID + "\",\"" + btnCancel.ClientID + "\", false)");
                btnCancel.Attributes.Add("onclick", "EditEntryFormFileUpload(\"" + btnDelete.ClientID + "\",\"" + lblMax.ClientID + "\",\"" + fileupload.ClientID + "\",\"" + btnEdit.ClientID + "\",\"" + PrevImage.ClientID + "\",\"" + btnCancel.ClientID + "\", true)");
                btnEdit.Visible = true;
                btnDelete.Visible = true;
                PrevImage.Visible = true;
                PrevImage.NavigateUrl = item.path;
            }
        }
    }

    protected void rptPrevImage_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ImagesUpload item = (ImagesUpload)e.Item.DataItem;
            Literal Space = (Literal)e.Item.FindControl("Space");
            Image PrevImagerptPrevImage = (Image)e.Item.FindControl("PrevImagerptPrevImage");
            HiddenField ID = (HiddenField)e.Item.FindControl("ID");
            PrevImagerptPrevImage.Visible = true;
            string NoImage = System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "images/NoImage.png";
            Space.Text = "&nbsp;";
            PrevImagerptPrevImage.ImageUrl = NoImage;
            ID.Value = item.ID;

            if (int.Parse(item.ID) % 3 == 0)
            {
                Space.Text = "<br/>";
            }
            if (!string.IsNullOrEmpty(item.path))
            {
                PrevImagerptPrevImage.Visible = true;
                PrevImagerptPrevImage.ImageUrl = item.path;
            }
           
        }
    }

    protected void rptCTP_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.CollectData item = (Data.CollectData)e.Item.DataItem;

            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            Literal title = (Literal)e.Item.FindControl("Title");
            TextBox Initial = (TextBox)e.Item.FindControl("txtInitial");
            TextBox Interim = (TextBox)e.Item.FindControl("txtInterim");
            TextBox Current = (TextBox)e.Item.FindControl("txtCurrent");
            hd.Value = item.id;
            title.Text = item.Title;
            Initial.Text = item.Data1;
            Interim.Text = item.Data2;
            Current.Text = item.Data3;
        }
    }

    protected void RPTExplainOtherMarket_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.ExplainListOtherMarketingViewmodel item = (Data.ExplainListOtherMarketingViewmodel)e.Item.DataItem;

            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            Label title = (Label)e.Item.FindControl("MarketingComponents");
            TextBox TimePeriod = (TextBox)e.Item.FindControl("txtTimePeriod");
            hd.Value = item.id;
            title.Text = item.MarketingComponents;
            TimePeriod.Text = item.TimePeriod;
        }
    }
    
    
    protected void UploadCharts(string textbox)
    {
        lblDoneMsg.Visible = false;
        PopupUploadImage.Visible = true;
        CurrentTypeImageGallery = textbox;
        CurrentCountImageGallery = 3;
        if (textbox == "txtExplainWorkedA")
        {
            CurrentCountImageGallery = 5;
        }
        PreviewImage();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ActionAlert = "OK_Alert";
        phAlert.Visible = true;
        MsgAlert = "<br>Are you sure to continue ?";
    }
    
    protected void btnDraft_Click(object sender, EventArgs e)
    {
        SaveForm(true);
        Response.Redirect("../Main/Dashboard.aspx");
    }
    
    protected void PreviewImage()
    {
        string PathEntryForm = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "\\EntryForm\\" + "\\" + entry.Id + "\\";
        string path = System.Configuration.ConfigurationSettings.AppSettings["EntryForm"] + "\\" + entry.Id + "\\";
        string url =  System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "\\EntryForm\\" + "\\" + entry.Id + "\\";
        string NoImage = System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "images/NoImage.png";
        List<ImagesUpload> imagesUpload = GeneralFunction.GetImageGallery(CurrentCountImageGallery, entry, CurrentTypeImageGallery, false);
        
        rptPrevImage.DataSource = GeneralFunction.GetImageGallery(CurrentCountImageGallery, entry, CurrentTypeImageGallery);
        rptPrevImage.DataBind();

        rptFileUpload.DataSource = GeneralFunction.GetImageGallery(CurrentCountImageGallery, entry, CurrentTypeImageGallery);
        rptFileUpload.DataBind();
    }


    protected void btnClosePopupimage_Click(object sender, EventArgs e)
    {
        PopupUploadImage.Visible = false;
    }


    protected void btnSaveImage_Click(object sender, EventArgs e)
    {
        lblDoneMsg.Text = "";
        string ImagesPath = "";
        int counts = 1;
        try
        {
            entryFormGallery = EntryFormGallery.GetEntryFormGallery(Guid.Empty, entry.Id, CurrentTypeImageGallery);
        }
        catch
        {
            entryFormGallery = EntryFormGallery.NewEntryFormGallery();
        }
        List<ImageGalleryResult> ImageGalleryResultList = new List<ImageGalleryResult>();
        foreach (RepeaterItem rpt in rptFileUpload.Items)
        {
            FileUpload FileUpload = (FileUpload)rpt.FindControl("File");
            HiddenField ID = (HiddenField)rpt.FindControl("ID");
            if (FileUpload != null)
            {
                //string Fileimage = GeneralFunction.SaveImageGallery(FileUpload, ID.Value, entry, CurrentTypeImageGallery, this);
                ImageGalleryResult ImageGalleryResult = GeneralFunction.SaveImageGallery(FileUpload, ID.Value, entry, CurrentTypeImageGallery, this);
                ImageGalleryResultList.Add(ImageGalleryResult);
                string path = counts++ + ":" + ImageGalleryResult.FileLocation + "|";
                ImagesPath += path;
            }
        }

        entryFormGallery.EntryId = entry.Id;
        entryFormGallery.Type = CurrentTypeImageGallery;
        entryFormGallery.ImagesPath = ImagesPath;
        entryFormGallery.Save();

        lblDoneMsg.Visible = true;
        foreach (ImageGalleryResult IGR in ImageGalleryResultList)
        {
            if (!string.IsNullOrEmpty(IGR.Error))
                lblDoneMsg.Text = IGR.Error + "<br>";
        }

        if (string.IsNullOrEmpty(lblDoneMsg.Text))
            lblDoneMsg.Text = "Upload completed<br>";

        PreviewImage();
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Main/Dashboard.aspx");
    }

    protected bool ValidationForm()
    {
        phAlert.Visible = false;
        lbError.Text = "";
        lbError2.Text = "";

        GeneralFunction.RemoveHighlightControls(this);
        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlCountryNumber, true, "", "Please indicate the total number of countries in which the case ran or is currently running");
        lbError.Text += GeneralFunction.ValidateTextBox("Executive Summary", txtExecutiveSummary, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesA, true, "string", "Please answer Question 1A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesB, true, "string", "Please answer Question 1B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesC, true, "string", "Please answer Question 1C");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesD, true, "string", "Please answer Question 1D");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesE, true, "string", "Please provide sourcing for Section 1. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasA, true, "string", "Please answer Question 2A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasB, true, "string", "Please answer Question 2B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasC, true, "string", "Please provide sourcing for Section 2. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtAnything, true, "string", "Please answer Question 3");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtBringingIdea, true, "string", "Please provide sourcing for Section 3. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedA, true, "string", "Please answer Question 4A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedB, true, "string", "Please answer question 4B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtListAndExplainOtherMarketingText, true, "string", "Please provide sourcing for Section 4. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtOwnedMedia, true, "string", "Elaborate on Owned Media. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainListOtherMarketing, true, "string", "Elaborate on Budget. Indicate NA if Not Applicable.");

        lbError.Text += GeneralFunction.ValidateTextBox("", txtSponsorship, true, "string", "Elaborate on Sponsorships. Indicate NA if Not Applicable.");

        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlInitialYearPME, true, "", "Paid Media Expenditure - Please indicate Budget for Initial Year.");
        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlInterimYearPME, true, "", "Paid Media Expenditure - Please indicate Budget for Interim Year.");
        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlCurrentYearPME, true, "", "Paid Media Expenditure - Please indicate Budget for Current Year.");
        
        //lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedInitial, true, "Paid Media Expenditure - Indicate Budget as compared to other competitors for Initial Year.");
        //lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedInterim, true, "Paid Media Expenditure - Indicate Budget as compared to other competitors for Interim Year.");
        lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedCurrent, true, "Paid Media Expenditure - Indicate Budget as compared to other competitors for Current Year.");

        //lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedoverallInitial, true, "Paid Media Expenditure - Indicate Budget as compared to overall spend of the brand for Initial Year.");
        //lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedoverallInterim, true, "Paid Media Expenditure - Indicate Budget as compared to overall spend of the brand for Interim Year.");
        lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedoverallCurrent, true, "Paid Media Expenditure - Indicate Budget as compared to overall spend of the brand for Current Year.");
        
        lbError.Text += GeneralFunction.ValidateTextBox("", DdlInitialYearPaid, true, "string", "Paid Media Expenditure - Please indicate Initial Year");
        lbError.Text += GeneralFunction.ValidateTextBox("", DdlInterimYearPaid, true, "string", "Paid Media Expenditure - Please indicate Interim Year");
        lbError.Text += GeneralFunction.ValidateTextBox("", DdlCurrentYearPaid, true, "string", "Paid Media Expenditure - Please indicate Current Year");

        lbError.Text += GeneralFunction.ValidateTextBox("", DdlInitialYear, true, "string", "Consumer Touch Points - Please indicate Initial Year");
        lbError.Text += GeneralFunction.ValidateTextBox("", DdlInterimYear, true, "string", "Consumer Touch Points - Please indicate Interim Year");
        lbError.Text += GeneralFunction.ValidateTextBox("", DdlCurrentYear, true, "string", "Consumer Touch Points - Please indicate Current Year");


        bool CheckCTP = false;
        foreach (RepeaterItem rpt in rptCTP.Items)
        {

            HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
            TextBox Initial = (TextBox)rpt.FindControl("txtInitial");
            TextBox Interim = (TextBox)rpt.FindControl("txtInterim");
            TextBox Current = (TextBox)rpt.FindControl("txtCurrent");
            
            if ((!string.IsNullOrEmpty(Initial.Text) || !string.IsNullOrEmpty(Interim.Text) || !string.IsNullOrEmpty(Current.Text)))
                CheckCTP = true;
        }

        if (!CheckCTP)
        {
            tabledataCTP.Attributes.Add("class", "TableRequired");
            lbError.Text += "Please answer the textbox for Communications Touchpoints.<br>";
        }
        else
        {
            tabledataCTP.Attributes.Add("class", "TableFix");
        }

        if(!string.IsNullOrEmpty(cbOtherCTP2.Text) ||  !string.IsNullOrEmpty(cbOtherCTP3.Text) || !string.IsNullOrEmpty(cbOtherCTP4.Text))
        {
            lbError.Text += GeneralFunction.ValidateTextBox("", txtOtherCTP1, true, "string", "Please answer the textbox  for Communications Touchpoints (Other)");
        }

        bool ChekEOM = false;
        foreach (RepeaterItem rpt in RPTExplainOtherMarket.Items)
        {
            HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
            TextBox TimePeriod = (TextBox)rpt.FindControl("txtTimePeriod");
            if (!string.IsNullOrEmpty(TimePeriod.Text))
                ChekEOM = true;
        }

        if (!ChekEOM)
        {
            divrptEOM.Attributes.Add("class", "TableRequired");
            lbError.Text += "Please tick the checkbox for Question 4B.<br>";
        }
        else
        {
            divrptEOM.Attributes.Add("class", "TableFix");
        }

        if(!string.IsNullOrEmpty(txtOtherExplainOtherMarket2.Text))
        {
            lbError.Text += GeneralFunction.ValidateTextBox("", txtOtherExplainOtherMarket1, true, "string", "Please answer the textbox for Question 4B (other)");
        }

        lbError2.Text = lbError.Text;
        return string.IsNullOrEmpty(lbError.Text);
    }
	
    protected bool SaveForm(bool isDraft) 
	{
        entryForm = SetEntryFormValue();

        if (isDraft)
            entryForm.Status = StatusEntry.Draft;
        else
            entryForm.Status = StatusEntry.Completed;
        
        if (entryForm.IsValid)
            entryForm.Save();

        if (GeneralFunction.isAllMaterialUploaded(entry))
        {
            entry.Status = StatusEntry.UploadCompleted;
        }
        else
        {
            entry.Status = StatusEntry.UploadPending;
        }

        entry.DateModifiedString = DateTime.Now.ToString();
        entry.Save();

        return true;
    }
    
    protected EntryForm SetEntryFormValue()
    {
        entryForm.DesTotalOfCountries = ddlCountryNumber.SelectedValue;
        entryForm.CommunicationTouchPointsCheck = GetCTP();
        entryForm.ExplainListOtherMarketing = GetEOM();
        entryForm.PaidMediaExpendituresCheck =
            ddlInitialYearPME.SelectedValue + "  " + Delimiter[0] + "  " + ddlInterimYearPME.SelectedValue + "  " + Delimiter[0] + "  " + ddlCurrentYearPME.SelectedValue + "  " + Delimiter[0] + "  " +
            txtInitialYearPCP.Text + "  " + Delimiter[0] + "  " + txtInterimYearPCP.Text + "  " + Delimiter[0] + "  " + txtCurrentYearPCP.Text + "  " + Delimiter[0] + "  " +
            rblComparedCurrent.SelectedValue + "  " + Delimiter[0] + "  " +
            rblComparedoverallCurrent.SelectedValue + "  " + Delimiter[0] + "  " +
            DdlInitialYearPaid.Text + "  " + Delimiter[0] + "  " + DdlInterimYearPaid.Text + "  " + Delimiter[0] + "  " + DdlCurrentYearPaid.Text;


        entryForm.IdEntry = EntryId;
        entryForm.ExplainWorked = txtExplainWorkedA.Text + "  " + Delimiter[0] + "  " + txtExplainWorkedB.Text + Delimiter[0];
        entryForm.OwnedMedia = txtOwnedMedia.Text;
        entryForm.Other = txtExplainListOtherMarketing.Text;
        entryForm.BringingIdea = txtBringingIdea.Text;
        entryForm.Anything = txtAnything.Text;
        entryForm.Ideas = txtIdeasA.Text + "  " + Delimiter[0] + "  " + txtIdeasB.Text + "  " + Delimiter[0] + "  " + txtIdeasC.Text + "  " + Delimiter[0];
        entryForm.StrategicChallengeObjectives = txtStrategicChallengeObjectivesA.Text + "  " + Delimiter[0] + "  " + txtStrategicChallengeObjectivesB.Text + "  " + Delimiter[0] +
                                                 txtStrategicChallengeObjectivesC.Text + "  " + Delimiter[0] + "  " + txtStrategicChallengeObjectivesD.Text + "  " + Delimiter[0] +
                                                 txtStrategicChallengeObjectivesE.Text;

        entryForm.ExecutiveSummary = txtExecutiveSummary.Text;
        entryForm.Sponsorship = txtSponsorship.Text;
        entryForm.Startdate1 = Convert.ToDateTime(txtStartdate1.Text).ToString("yyyy/MM/dd");
        entryForm.Enddate1 = Convert.ToDateTime(txtEnddate1.Text).ToString("yyyy/MM/dd");
        entryForm.TypeCategory = CurrentCategory;
        entryForm.ListAndExplainOtherMarketingText = txtListAndExplainOtherMarketingText.Text;

        if (entryForm.TypeCategoryOriginal == "")
            entryForm.TypeCategoryOriginal = CurrentCategory;

        return entryForm;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidationForm()) return;
        
        SaveForm(false);
        Response.Redirect("../Main/Dashboard.aspx");
    }

    protected void DisableUploadImage(bool Visible)
    {
        TableEditImage.Visible = Visible;
        btnSaveImage.Visible = Visible;

        btnDraft.Visible = Visible;
        btnNext.Visible = Visible;

        TablePreviewImage.Visible = !Visible;

        btnEdit.Visible = !Visible;
        ViewState["IsPreview"] = !Visible;
        btnSubmit.Visible = !Visible;
        lblverification1.Visible = !Visible;
        lblverification2.Visible = !Visible;
        initJavascript();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        PopupUploadImage.Visible = false;
        if (!ValidationForm()) return;

        if (!(bool)ViewState["IsPreview"])
        {
            DisableAllAction(this, false);
            DisableUploadImage(false);
            lblverification1.Text = "<p><span style='font-weight: bold; font-size: 19px;'>Please preview the PDF and ensure that the information submitted is correct.</span></p><br>";
            lblverification2.Text = lblverification1.Text;

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "none", "<script type=\"text/javascript\">alert('Please preview the PDF and ensure that the information submitted is correct.');</script>", false);
            hdScrollPos.Value = "";
            SaveForm(true);
            string Msg = "Your Entry has been saved as draft.";
            btnCancel.OnClientClick = "return MsgAlert(\""+ Msg + "\");";
        }
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        DisableUploadImage(true);
        DisableAllAction(this, true);
        hdScrollPos.Value = "";
        string Msg = "Any changes will not be saved. Confirm to Proceed ?";
        btnCancel.OnClientClick = "return MsgAlert(\"" + Msg + "\");";
    }
    
    protected void Cancel_Alert(object sender, EventArgs e)
    {
        phAlert.Visible = false;
    }

    protected void Preview(bool isDraft)
    {
        PopupUploadImage.Visible = false;
        Session["Entry-" + EntryId.ToString() + (string)ViewState["SessionKey"]] = SetEntryFormValue();
        ViewState["SessionKey"] = DateTime.Now.ToString("HHmmss");
        PDFURL = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?temp=Preview&Id=" + entry.Id + "&skey=" + (string)ViewState["SessionKey"];
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string url = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?Id=" + entry.Id;
        Page.RegisterClientScriptBlock("openWindow", "<script>window.open('" + url + "', '_blank');</script>");
    }


    protected string GetCTP()
    {
        string CTPInitial = "";
        string CTPInterim = "";
        string CTPCurrent = "";
        string Temp = "";

        {
            CTPInitial += "000" + ":" + DdlInitialYear.Text + Delimiter[0];
            CTPInterim += "000" + ":" + DdlInterimYear.Text + Delimiter[0];
            CTPCurrent += "000" + ":" + DdlCurrentYear.Text + Delimiter[0];
            foreach (RepeaterItem rpt in rptCTP.Items)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                TextBox Initial = (TextBox)rpt.FindControl("txtInitial");
                TextBox Interim = (TextBox)rpt.FindControl("txtInterim");
                TextBox Current = (TextBox)rpt.FindControl("txtCurrent");
                CTPInitial += hd.Value + ":" + Initial.Text + Delimiter[0];
                CTPInterim += hd.Value + ":" + Interim.Text + Delimiter[0];
                CTPCurrent += hd.Value + ":" + Current.Text + Delimiter[0];
            }
            CTPInitial += "998" + ":" + txtOtherCTP1.Text + Delimiter[0];
            CTPInterim += "998" + ":" + cbOtherCTP2.Text + Delimiter[0];
            CTPCurrent += "998" + ":" + cbOtherCTP3.Text + Delimiter[0];
            CTPCurrent += "999" + ":" + cbOtherCTP4.Text + Delimiter[0];

            entryForm.CommunicationTouchPointsInitialYear = CTPInitial;
            entryForm.CommunicationTouchPointsInterimYear = CTPInterim;
            entryForm.CommunicationTouchPointsCurrentYear = CTPCurrent;
        }
        Temp = CTPInitial + CTPInterim + CTPCurrent;
        return Temp;
    }

    protected string GetEOM()
    {
        string TimePerioddb = "";
        {
            foreach (RepeaterItem rpt in RPTExplainOtherMarket.Items)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                TextBox TimePeriod = (TextBox)rpt.FindControl("txtTimePeriod");
                TimePerioddb += hd.Value + ":" + TimePeriod.Text + Delimiter[0];
            }
            TimePerioddb += "998" + ":" + txtOtherExplainOtherMarket1.Text + Delimiter[0];
            TimePerioddb += "999" + ":" + txtOtherExplainOtherMarket2.Text + Delimiter[0];
        }
        return TimePerioddb;
    }
}
