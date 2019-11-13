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

public partial class Main_EFSingleMarket : System.Web.UI.Page
{
    #region Global Variable
    Entry entry = null;
    List<Data.ExplainListOtherMarketingViewmodel> explainOtherMarketingList = new List<Data.ExplainListOtherMarketingViewmodel>();
    List<Data.CollectData> PaidMediaExpendituresList = new List<Data.CollectData>();
    string[] Delimiter = { "#&&##&&#", "&&&$$&&&" };
    Guid EntryId = Guid.Empty;
    EntryForm entryForm = null;
    string  CurrentCategory = Data.DocumentCategories.SingleMarket.ToString();
    EntryForm CurrentEntryForm = null;
    public static string CurrentTypeImageGallery = "";
    public static int CurrentCountImageGallery = 3;
    public static string PDFURL;
    public static string MsgAlert;
    public static string ActionAlert;
    //public static string IsPreview;
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
            PopulateHeaderForm();
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
        TablePreviewImage.Visible = false;
        btnEdit.Visible = false;
        btnSubmit.Visible = false;
        ddlCountryNumber.Items.Add(new ListItem("Select", ""));
        for (int i = 1; i <= 42; i++)
        {
            ddlCountryNumber.Items.Add(i.ToString());
        }
        
        CollectCommunicationTouchPoints();
        CollectExplainListOtherMarketing();


        string Msg = "Any changes will not be saved. Confirm to Proceed ?";
        btnCancel.OnClientClick = "return MsgAlert(\"" + Msg + "\");";
    }


    protected void initJavascript()
    {
        List<TextBoxKeyup> textBoxKeyup = new List<TextBoxKeyup>();
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExecutiveSummary, LimitText = 100, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesA, LimitText = 125, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesB, LimitText = 275, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesC, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesD, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasA, LimitText = 225, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasB, LimitText = 25, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainListOtherMarketing, LimitText = 100, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedA, LimitText = 400, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedB, LimitText = 150, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtAnything, LimitText = 475, isLimitActive = true });

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
        List<Data.CollectData> CTPList = Data.GetCTPSingle();

        if (entryForm != null)
        {
            string[] communicationTouchPointsList = entryForm.CommunicationTouchPointsCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);

            if (communicationTouchPointsList.Count() != 0)
            {
                for (int i = 0; i < communicationTouchPointsList.Length; i++)
                {
                    string[] Datas = communicationTouchPointsList[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);
                    string[] count1 = Datas[0].Split(':');
                    
                    if (count1[0] == "998")
                    {
                        cbOtherCTP1.Checked = (count1[1] == "True") ? true : false;
                    }
                    else if (count1[0] == "999")
                    {
                        txtOtherCTP2.Text = count1[1];
                    }
                    else
                    {
                        try
                        {
                            Data.CollectData CT = CTPList.FirstOrDefault(x => x.id == count1[0]);
                            CT.Data1 = Datas[0].Split(':')[1];
                        }
                        catch
                        {
                            //TODO
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
        explainOtherMarketingList = Data.GetListExplainPoitAllOthers();

        if (entryForm != null)
        {
            string[] ELOMG = entryForm.ExplainListOtherMarketing.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            List<Data.ExplainListOtherMarketingViewmodel> explainOtherMarketingListTemp = new List<Data.ExplainListOtherMarketingViewmodel>();

            if (ELOMG.Count() != 0)
            {
                for (int i = 0; i < ELOMG.Length; i++)
                {
                    string[] Datas = ELOMG[i].Split(':');

                    if (Datas[0] == "999")
                    {
                        txtOtherSingleMarket.Text = Datas[1];
                    }
                    else if (Datas[0] == "998")
                    {
                        cbOtherSingleMarket.Checked = (Datas[1] == "True") ? true : false;
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

    protected void PopulateHeaderForm()
    {
        if (entry != null)
        {
            txtClassification.Text = entry.ProductClassification;
            TxtCountry1.Text = entry.Effectiveness;
            txtStartdate1.Text = entry.DateCampaignStart.ToString("dd MMM yyyy");
            txtEnddate1.Text = entry.DateCampaignEnd.ToString("dd MMM yyyy");
            lblEntryID.Text = entry.Serial;
            txtCampaign.Text = entry.Campaign;
            txtBrand.Text = entry.Brand;
            lblCategoryMarket.Text = Data.GetCategoryMarket(entry.CategoryMarket);
            txtEntryCategory.Text = entry.CategoryPSDetail;

            rblComparedOtherCompetitorsCheck.Attributes.Add("class", "rdbBlock");
            rblComparedOverallSpendCheck.Attributes.Add("class", "rdbBlock");
            {
                txtOtherCTP2.Attributes.Add("class", "TextCtmOther");
                cbOtherCTP1.Attributes.Add("class", "Cusinput");
                LabelOtherCTP.Attributes.Add("class", "Cuslabel");
            }
            {
                txtOtherSingleMarket.Attributes.Add("class", "TextCtmOther");
                cbOtherSingleMarket.Attributes.Add("class", "Cusinput");
                LabelEom.Attributes.Add("class", "Cuslabel");
            }
        }
    }

    private void PopulateForm()
    {
        List<string> items = GeneralFunction.GetEffectivenessItems();
        if (entry != null)
        {
            ddlPaidMediaExpendituresCurrent.Items.Clear();
            ddlPaidMediaExpendituresPrior.Items.Clear();

            ddlPaidMediaExpendituresCurrent.Items.Add(new ListItem("Select Budget", ""));
            ddlPaidMediaExpendituresPrior.Items.Add(new ListItem("Select Budget", ""));

            ddlCountryNumber.SelectedValue = (entry.CaseData.Split('|').Count() - 1).ToString();

            foreach (Data.CollectData str in Data.GetPaidMediaExpendituresData())
            {
                ddlPaidMediaExpendituresCurrent.Items.Add(str.Data1);
                ddlPaidMediaExpendituresPrior.Items.Add(str.Data1);
            }
            

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
            txtListAndExplainOtherMarketingText.Text = entryForm.ListAndExplainOtherMarketingText;
            if (entryForm.ExplainWorked != "" && entryForm.ExplainWorked != null)
            {
                string[] ExplainWorked = entryForm.ExplainWorked.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (ExplainWorked.Length != 0)
                {
                    try {
                        txtExplainWorkedA.Text = ExplainWorked[0].Replace("  ", "");
                        txtExplainWorkedB.Text = ExplainWorked[1].Replace("  ", "");
                    }
                    catch { }
                }
            }

            ddlCountryNumber.SelectedValue = string.IsNullOrWhiteSpace(entryForm.DesTotalOfCountries) ? ddlCountryNumber.SelectedValue : entryForm.DesTotalOfCountries;
            txtAnything.Text = entryForm.Anything;
            rblComparedOtherCompetitorsCheck.SelectedValue = entryForm.ComparedOtherCompetitorsCheck;
            rblComparedOverallSpendCheck.SelectedValue = entryForm.ComparedOverallSpendCheck;
            
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

            if (entryForm.PaidMediaExpendituresCheck != "" && entryForm.PaidMediaExpendituresCheck != null)
            {
                string[] PME = entryForm.PaidMediaExpendituresCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (PME.Length != 0)
                {
                    try
                    {
                        //ddlCurrentYear.SelectedValue = PME[0].Replace("  ", "");
                        //YearPrior.SelectedValue = PME[1].Replace("  ", "");
                        ddlCurrentYear.Text = PME[0].Replace("  ", "");
                        YearPrior.Text = PME[1].Replace("  ", "");
                        ddlPaidMediaExpendituresCurrent.SelectedValue = PME[2].Replace("  ", "");
                        ddlPaidMediaExpendituresPrior.SelectedValue = PME[3].Replace("  ", "");
                    }
                    catch
                    {

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
            Label title = (Label)e.Item.FindControl("Title");
            CheckBox cbItem = (CheckBox)e.Item.FindControl("cbItem");
            HiddenField hdAttrType = (HiddenField)e.Item.FindControl("hdAttrType");
            hd.Value = item.id;
            title.Text = item.Title;
            cbItem.Checked = (item.Data1 == "True") ? true : false;
            hdAttrType.Value = item.AttrType;

            if (item.AttrType == "Header")
            {
                cbItem.Visible = false;
                title.Font.Bold = true;
                title.Text = "<span style=\" font-size: 14px; margin-left: 15px;\">" + item.Title + "</span>";
            }
            if (item.AttrType == "SingleHeader")
            {
                title.Font.Bold = true;
                title.Text = "<span  style=\" font-size: 14px; \">" + item.Title + "</span>";
            }
            if (item.AttrType == "Body")
            {
                cbItem.CssClass = "CusinputBody";
                title.Text = "<span style=\"margin-left: 0px;\">" + item.Title + "</span>";
            }
            else
            {
                cbItem.CssClass = "Cusinput";
            }
        }
    }

    protected void RPTExplainOtherMarket_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.ExplainListOtherMarketingViewmodel item = (Data.ExplainListOtherMarketingViewmodel)e.Item.DataItem;

            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            Label title = (Label)e.Item.FindControl("Title");
            CheckBox cbItem = (CheckBox)e.Item.FindControl("cbItem");
            cbItem.Checked = (item.TimePeriod == "True") ? true : false;
            hd.Value = item.id;
            title.Text = item.MarketingComponents;
        }
    }

    protected void RPTPaidMedia_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.CollectData item = (Data.CollectData)e.Item.DataItem;

            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            Label title1 = (Label)e.Item.FindControl("Title1");
            CheckBox cbItem1 = (CheckBox)e.Item.FindControl("cbItem1");
            Label title2 = (Label)e.Item.FindControl("Title2");
            CheckBox cbItem2 = (CheckBox)e.Item.FindControl("cbItem2");
            cbItem1.Checked = (item.Data1 == "True") ? true : false;
            cbItem2.Checked = (item.Data2 == "True") ? true : false;
            hd.Value = item.id;
            title1.Text = item.Title;
            title2.Text = item.Title;
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
    

    protected void ValidateBudget()
    {
        //List<CountriesBudget> CountriesBudgetList = new List<CountriesBudget>();
        //CountriesBudgetList.Add(new CountriesBudget { Countries = "Australia, China, Japan, South Korea", under = "$1M – under $5M" });
        //CountriesBudgetList.Add(new CountriesBudget { Countries = "Hong Kong, India", under = "$500K – under $1M" });
        //CountriesBudgetList.Add(new CountriesBudget { Countries = "Indonesia, Malaysia, New Zealand, Philippines, Singapore, Taiwan, Thailand, Vietnam", under = "$250K – under $500K" });
        //CountriesBudgetList.Add(new CountriesBudget { Countries = "Brunei, Cambodia, East Timor, Laos, Myanmar, Macau, Mongolia, North Korea, American Samoa, French Polynesia, " +
        //    "Pitcairn Islands, Samoa, Tonga, Tuvalu, Hawaii, Wallis and Futuna, Christmas Island, Cocos (Keeling) Islands, Norfolk Island, Cook Islands" + 
        //    ", Niue, Tokelau, Fiji, New Caledonia, Papua New Guinea, Solomon Islands, Vanuatu, Federated States of Micronesia, Guam, Kiribati, Marshall Islands, "+
        //    "Nauru, Northern Mariana Islands, Palau, Wake Island, Bangladesh, Bhutan, British Indian Ocean Territory, Maldives, Nepal, Pakistan, Sri Lanka, ", under = "$100K – under $250K" });

        //if (entry.CategoryPSDetail.IndexOf("Small Budget") != -1)
        //{
        //    foreach (CountriesBudget countriesBudget in CountriesBudgetList)
        //    {
        //        if (countriesBudget.Countries.IndexOf(entry.Effectiveness) != -1 &&
        //            !string.IsNullOrEmpty(ddlPaidMediaExpendituresCurrent.SelectedValue))
        //        {
        //            string valueSelected = Data.GetPaidMediaExpendituresData().FirstOrDefault(x => x.Data1 == ddlPaidMediaExpendituresCurrent.SelectedValue).value;
        //            string valueUnder = Data.GetPaidMediaExpendituresData().FirstOrDefault(x => x.Data1 == countriesBudget.under).value;
        //            if (int.Parse(valueSelected) >= int.Parse(valueUnder))
        //            {
        //                lbError.Text += "Media Expenditure has exceeded the limit for this category Year Must be under " + countriesBudget.under + "<br>";
        //                GeneralFunction.HighlightControl(ddlPaidMediaExpendituresCurrent);
        //            }
        //        }

        //        if (countriesBudget.Countries.IndexOf(entry.Effectiveness) != -1 &&
        //            !string.IsNullOrEmpty(ddlPaidMediaExpendituresPrior.SelectedValue))
        //        {
        //            string valueSelected = Data.GetPaidMediaExpendituresData().FirstOrDefault(x => x.Data1 == ddlPaidMediaExpendituresPrior.SelectedValue).value;
        //            string valueUnder = Data.GetPaidMediaExpendituresData().FirstOrDefault(x => x.Data1 == countriesBudget.under).value;
        //            if (int.Parse(valueSelected) >= int.Parse(valueUnder))
        //            {
        //                lbError.Text += "Paid Media Expenditures Prior Year Must be under " + countriesBudget.under + "<br>";
        //                GeneralFunction.HighlightControl(ddlPaidMediaExpendituresPrior);
        //            }
        //        }
        //    }
        //}   

        if (txtEntryCategory.Text == "Small Budget-Services" || txtEntryCategory.Text == "Small Budget-Products")
        {
            if ("Australia,China,Japan,South Korea".Contains(entry.Effectiveness))
            {
                if (!"Under $100K, $100K – under $250K, $250K – under $500K, $500K – under $1M".Contains(ddlPaidMediaExpendituresCurrent.SelectedValue))
                {
                    lbError.Text += "Media Expenditure has exceeded the limit for this category<br>";
                    GeneralFunction.HighlightControl(ddlPaidMediaExpendituresCurrent);
                }
            }
            else if ("Hong Kong,India".Contains(entry.Effectiveness))
            {
                if (!"Under $100K, $100K – under $250K, $250K – under $500K".Contains(ddlPaidMediaExpendituresCurrent.SelectedValue))
                {
                    lbError.Text += "Media Expenditure has exceeded the limit for this category<br>";
                    GeneralFunction.HighlightControl(ddlPaidMediaExpendituresCurrent);
                }
            }
            else if ("Indonesia, Malaysia, New Zealand, Philippines, Singapore, Taiwan, Thailand, Vietnam".Contains(entry.Effectiveness))
            {
                if (!"Under $100K, $100K – under $250K".Contains(ddlPaidMediaExpendituresCurrent.SelectedValue))
                {
                    lbError.Text += "Media Expenditure has exceeded the limit for this category<br>";
                    GeneralFunction.HighlightControl(ddlPaidMediaExpendituresCurrent);
                }
            }
            else if ("Brunei, Cambodia, East Timor, Laos, Myanmar, Macau, Mongolia, North Korea, American Samoa, French Polynesia, Pitcairn Islands, Samoa, Tonga, Tuvalu, Hawaii, Wallis and Futuna, Christmas Island, Cocos (Keeling) Islands, Norfolk Island, Cook Islands, Niue, Tokelau, Fiji, New Caledonia, Papua New Guinea, Solomon Islands, Vanuatu, Federated States of Micronesia, Guam, Kiribati, Marshall Islands, Nauru, Northern Mariana Islands, Palau, Wake Island, Bangladesh, Bhutan, British Indian Ocean Territory, Maldives, Nepal, Pakistan, Sri Lanka, ".Contains(entry.Effectiveness))
            {
                if (!"Under $100K".Contains(ddlPaidMediaExpendituresCurrent.SelectedValue))
                {
                    lbError.Text += "Media Expenditure has exceeded the limit for this category<br>";
                    GeneralFunction.HighlightControl(ddlPaidMediaExpendituresCurrent);
                }
            }
        }
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
        lbError.Text += GeneralFunction.ValidateTextBox("", txtAnything, true, "string", "Please answer Question 3 ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtBringingIdea, true, "string", "Please provide sourcing for Section 3. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedA, true, "string", "Please answer Question 4A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedB, true, "string", "Please answer question 4B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtOwnedMedia, true, "string", "Elaborate on Owned Media. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainListOtherMarketing, true, "string", "Elaborate on Budget. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtListAndExplainOtherMarketingText, true, "string", "Please provide sourcing for Section 4. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtSponsorship, true, "string", "Elaborate on Sponsorships. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedOtherCompetitorsCheck, true, "Paid Media Expenditure - Indicate Budget as compared to other competitors");
        lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedOverallSpendCheck, true, "Paid Media Expenditure - Indicate Budget as compared to overall spend of the brand");
        
        lbError.Text += GeneralFunction.ValidateTextBox("", ddlCurrentYear, true, "", "Paid Media Expenditure - Please indicate Current Year");
        lbError.Text += GeneralFunction.ValidateTextBox("", YearPrior, true, "", "Paid Media Expenditure - Please indicate Prior Year");
        
        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlPaidMediaExpendituresCurrent, true, "", "Paid Media Expenditure - Please indicate Budget for Current Year");
        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlPaidMediaExpendituresPrior, true, "", "Paid Media Expenditure - Please indicate Budget for Prior Year");

        ValidateBudget();

        if (GetCTP().IndexOf("True") == -1)
        {
            divrptCTP.Attributes.Add("class", "TableRequired");
            lbError.Text += "Please tick the checkbox for Communications Touchpoints<br>";
        }
        else
        {
            divrptCTP.Attributes.Add("class", "TableFix");
        }

        if(cbOtherSingleMarket.Checked)
        {
            lbError.Text += GeneralFunction.ValidateTextBox("", txtOtherSingleMarket, true, "string", "Please answer the textbox for Question 4B (other)");   
        }
        
        if (cbOtherCTP1.Checked)
        {
            lbError.Text += GeneralFunction.ValidateTextBox("Other (describe – Communication Touchpoint) ", txtOtherCTP2, true, "string");
        }

        if (GetEOM().IndexOf("True") == -1)
        {
            TableRPTExplainOtherMarket.Attributes.Add("class", "TableRequired");
            lbError.Text += "Please tick the checkbox for Question 4B <br>";
        }
        else
        {
            TableRPTExplainOtherMarket.Attributes.Add("class", "TableFix");
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
        entryForm.CommunicationTouchPointsCheck = GetCTP();
        entryForm.ExplainListOtherMarketing = GetEOM();
        entryForm.DesTotalOfCountries = ddlCountryNumber.SelectedValue;
        entryForm.PaidMediaExpendituresCheck = ddlCurrentYear.Text + "  " + Delimiter[0] + "  " + YearPrior.Text
                                        + "  " + Delimiter[0] + "  " + ddlPaidMediaExpendituresCurrent.SelectedValue + "  " + Delimiter[0] + "  " + ddlPaidMediaExpendituresPrior.SelectedValue;
        entryForm.IdEntry = EntryId;
        entryForm.ExplainWorked = txtExplainWorkedA.Text + "  " + Delimiter[0] + "  " + txtExplainWorkedB.Text + Delimiter[0];
        entryForm.OwnedMedia = txtOwnedMedia.Text;
        entryForm.Other = txtExplainListOtherMarketing.Text;
        entryForm.BringingIdea = txtBringingIdea.Text;
        entryForm.ListAndExplainOtherMarketingText = txtListAndExplainOtherMarketingText.Text;
        entryForm.Ideas = txtIdeasA.Text + "  " + Delimiter[0] + "  " + txtIdeasB.Text + "  " + Delimiter[0] + "  " + txtIdeasC.Text + "  " + Delimiter[0];
        entryForm.StrategicChallengeObjectives = txtStrategicChallengeObjectivesA.Text + "  " + Delimiter[0] + "  " + txtStrategicChallengeObjectivesB.Text + "  " + Delimiter[0] + "  " +
                                                 txtStrategicChallengeObjectivesC.Text + "  " + Delimiter[0] + "  " + txtStrategicChallengeObjectivesD.Text + "  " + Delimiter[0] + "  " +
                                                 txtStrategicChallengeObjectivesE.Text + "  " + Delimiter[0] + "  ";

        entryForm.ComparedOtherCompetitorsCheck = rblComparedOtherCompetitorsCheck.SelectedValue;
        entryForm.ComparedOverallSpendCheck = rblComparedOverallSpendCheck.SelectedValue;
        entryForm.Anything = txtAnything.Text;
        entryForm.ExecutiveSummary = txtExecutiveSummary.Text;

        entryForm.Sponsorship = txtSponsorship.Text;
        entryForm.TypeCategory = CurrentCategory;

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
        if (!ValidationForm())
            return;

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
            btnCancel.OnClientClick = "return MsgAlert(\"" + Msg + "\");";
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
        string Item = "";
        string Temp = "";
        {
            foreach (RepeaterItem rpt in rptCTP.Items)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                HiddenField hdAttrType = (HiddenField)rpt.FindControl("hdAttrType");
                Label Title = (Label)rpt.FindControl("Title");
                CheckBox cbItem = (CheckBox)rpt.FindControl("cbItem");
                Item = hd.Value + ":" + cbItem.Checked;
                Temp += Item + Delimiter[0];

                if (hdAttrType.Value == "Header")
                {
                    cbItem.Visible = false;
                    Title.Font.Bold = true;
                }
                else if (hdAttrType.Value == "SingleHeader")
                {
                    cbItem.CssClass = "Cusinput";
                    Title.Font.Bold = true;
                }
                else if (hdAttrType.Value == "Body")
                {
                    cbItem.CssClass = "CusinputBody";
                }
                

            }
            Temp += "998" + ":" + cbOtherCTP1.Checked + Delimiter[0];
            Temp += "999" + ":" + txtOtherCTP2.Text + Delimiter[0];
        }
        return Temp;
    }

    protected string GetEOM()
    {
        string TimePerioddb = "";
        {
            foreach (RepeaterItem rpt in RPTExplainOtherMarket.Items)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                CheckBox cbItem = (CheckBox)rpt.FindControl("cbItem");
                Label Title = (Label)rpt.FindControl("Title");
                TimePerioddb += hd.Value + ":" + cbItem.Checked + Delimiter[0];
                cbItem.CssClass = "Cusinput";
                Title.CssClass = "Cuslabel";
            }
            TimePerioddb += "998" + ":" + cbOtherSingleMarket.Checked + Delimiter[0];
            TimePerioddb += "999" + ":" + txtOtherSingleMarket.Text + Delimiter[0];
        }
        return TimePerioddb;
    }
}
