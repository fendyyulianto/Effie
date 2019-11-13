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
using Telerik.Web.UI;
public partial class Main_EFShopperMarketing : System.Web.UI.Page
{
    Entry entry = null;
    List<Data.CollectData> CTPList = new List<Data.CollectData>();
    List<Data.ExplainListOtherMarketingViewmodel> explainOtherMarketingList = new List<Data.ExplainListOtherMarketingViewmodel>();
    List<Data.CollectData> PaidMediaExpendituresList = new List<Data.CollectData>();
    string[] Delimiter = { "#&&##&&#", "&&&$$&&&" };
    Guid EntryId = Guid.Empty;
    EntryForm entryForm = null;
    string CurrentCategory = Data.DocumentCategories.ShopperMarketing.ToString();
    EntryForm CurrentEntryForm = null;
    public static string CurrentTypeImageGallery = "";
    public static int CurrentCountImageGallery = 3;
    public static string PDFURL;
    public static string MsgAlert;
    public static string ActionAlert;
    //public static bool IsPreview = false;
    public static EntryFormGallery entryFormGallery = null;
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
            }
            {
                txtOtherSingleMarket.Attributes.Add("class", "TextCtmOther");
                cbOtherSingleMarket.Attributes.Add("class", "Cusinput");
            }
        }
    }
    protected void initJavascript()
    {
        List<TextBoxKeyup> textBoxKeyup = new List<TextBoxKeyup>();
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExecutiveSummary, LimitText = 100, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesA, LimitText = 125, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesB, LimitText = 275, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesC, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesD, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesE, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasA, LimitText = 225, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasB, LimitText = 25, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtAnything, LimitText = 475, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedA, LimitText = 400, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedB, LimitText = 150, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainListOtherMarketing, LimitText = 100, isLimitActive = true });

        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtOwnedMedia, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtSponsorship, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedC, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtBringingIdea, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasC, LimitText = 0, isLimitActive = false });

        GeneralFunctionEntryForm.InitTextbox(textBoxKeyup, this, entry, ViewState["IsPreview"].ToString());
        
    }
    
    public void CollectCommunicationTouchPoints()
    {
        List<Data.CollectData> CTPList = Data.GetCTPMulti();

        if (entryForm != null)
        {
            string[] communicationTouchPointsList = entryForm.CommunicationTouchPointsCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);

            if (communicationTouchPointsList.Count() != 0)
            {
                for (int i = 0; i < communicationTouchPointsList.Length; i++)
                {
                    string[] Datas = communicationTouchPointsList[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);
                    string[] Data1 = null;
                    string[] Data2 = null;
                    string[] Data3 = null;
                    string[] Data4 = null;
                    if (Datas.Length >= 3)
                    {

                        Data1 = Datas[0].Split(':');
                        Data2 = Datas[1].Split(':');
                        Data3 = Datas[2].Split(':');
                        Data4 = Datas[3].Split(':');


                        if (Data1[0] == "998")
                        {
                            string[] Data5 = Datas[3].Split(':');
                            txtOtherCTP2.Text = Data1[1];
                            OtherPreDuringPost1.Checked = (Data2[1] == "True") ? true : false;
                            OtherPreDuringPost2.Checked = (Data3[1] == "True") ? true : false;
                            OtherPreDuringPost3.Checked = (Data4[1] == "True") ? true : false;
                        }
                        else
                        {
                            try
                            {
                                Data.CollectData CT = CTPList.FirstOrDefault(x => x.id == Data1[0]);
                                CT.Data1 = Data1[1];
                                CT.Data2 = Data2[1];
                                CT.Data3 = Data3[1];
                                CT.Data4 = Data4[1];
                            }
                            catch
                            {
                                //TODO
                            }
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
                        }
                        catch
                        {
                            //TODO
                        }
                    }
                }
            }
        }

        RPTExplainOtherMarket.DataSource = explainOtherMarketingList;

        RPTExplainOtherMarket.DataBind();

    }

    private void PopulateForm()
    {
        List<string> items = GeneralFunction.GetEffectivenessItems();
        if (entry != null)
        {
            ddlCountryNumber.SelectedValue = (entry.CaseData.Split('|').Count() - 1).ToString();
            ddlPaidMediaExpendituresCurrent.Items.Clear();
            ddlPaidMediaExpendituresPrior.Items.Clear();

            ddlPaidMediaExpendituresCurrent.Items.Add(new ListItem("Select Budget", ""));
            ddlPaidMediaExpendituresPrior.Items.Add(new ListItem("Select Budget", ""));

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
            txtAnything.Text = entryForm.Anything;

            if (entryForm.ExplainWorked != "" && entryForm.ExplainWorked != null)
            {
                string[] ExplainWorked = entryForm.ExplainWorked.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (ExplainWorked.Length != 0)
                {
                    try
                    {
                        txtExplainWorkedA.Text = ExplainWorked[0].Replace("  ", "");
                        txtExplainWorkedB.Text = ExplainWorked[1].Replace("  ", "");
                        txtExplainWorkedC.Text = ExplainWorked[2].Replace("  ", "");
                    }
                    catch { }
                }
            }

            ddlCountryNumber.SelectedValue = string.IsNullOrWhiteSpace(entryForm.DesTotalOfCountries) ? ddlCountryNumber.SelectedValue : entryForm.DesTotalOfCountries;
            txtSponsorship.Text = entryForm.Sponsorship;
            txtOwnedMedia.Text = entryForm.OwnedMedia;
            txtExplainListOtherMarketing.Text = entryForm.Other;
            txtBringingIdea.Text = entryForm.BringingIdea;
            rblComparedOtherCompetitorsCheck.SelectedValue = entryForm.ComparedOtherCompetitorsCheck;
            rblComparedOverallSpendCheck.SelectedValue = entryForm.ComparedOverallSpendCheck;
            if (entryForm.PaidMediaExpendituresCheck != "" && entryForm.PaidMediaExpendituresCheck != null)
            {
                string[] PME = entryForm.PaidMediaExpendituresCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (PME.Length != 0)
                {
                    try
                    {
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

            if (entryForm.Ideas != "" && entryForm.Ideas != null)
            {
                string[] IdeasList = entryForm.Ideas.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (IdeasList.Length != 0)
                {
                    try
                    {
                        txtIdeasA.Text = IdeasList[0].Replace("  ", "");
                        txtIdeasB.Text = IdeasList[1].Replace("  ", "");
                        txtIdeasC.Text = IdeasList[2].Replace("  ", "");
                    }
                    catch { }
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
                    catch { }
                }
            }

            txtExecutiveSummary.Text = entryForm.ExecutiveSummary;
        }
    }

    protected void rptFileUpload_ItemCommand(object sender, GridCommandEventArgs e)
    {

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
            HiddenField hdAttrType = (HiddenField)e.Item.FindControl("hdAttrType");
            Label title = (Label)e.Item.FindControl("Title");
            CheckBox cbItem = (CheckBox)e.Item.FindControl("cbItem");
            CheckBox PreDuringPost1 = (CheckBox)e.Item.FindControl("PreDuringPost1");
            CheckBox PreDuringPost2 = (CheckBox)e.Item.FindControl("PreDuringPost2");
            CheckBox PreDuringPost3 = (CheckBox)e.Item.FindControl("PreDuringPost3");

            hdAttrType.Value = item.AttrType;
            hd.Value = item.id;
            title.Text = item.Title;
            cbItem.Checked = (item.Data1 == "True") ? true : false;

            PreDuringPost1.Checked = (item.Data2 == "True") ? true : false;
            PreDuringPost2.Checked = (item.Data3 == "True") ? true : false;
            PreDuringPost3.Checked = (item.Data4 == "True") ? true : false;

            title.Text = item.Title;

            if (item.AttrType == "Header" || item.AttrType == "SingleHeader")
            {

                title.Text = "<span  style=\" font-size: 14px; \">" + item.Title + "</span>";
                title.Font.Bold = true;
                cbItem.Visible = false;
            }
            else if (item.AttrType == "Body")
            {
                PreDuringPost1.Visible = false;
                PreDuringPost2.Visible = false;
                PreDuringPost3.Visible = false;
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
        string url = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "\\EntryForm\\" + "\\" + entry.Id + "\\";
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
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesE, true, "string", "Please provide sourcing for Section 1. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasA, true, "string", "Please answer Question 2A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasB, true, "string", "Please answer Question 2B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasC, true, "string", "Please provide sourcing for Section 2. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtAnything, true, "string", "Please answer Question 3");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtBringingIdea, true, "string", "Please provide sourcing for Section 3. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedA, true, "string", "Please answer Question 4A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedB, true, "string", "Please answer question 4B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedC, true, "string", "Please provide sourcing for Section 4. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtOwnedMedia, true, "string", "Elaborate on Owned Media. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainListOtherMarketing, true, "string", "Elaborate on Budget. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtSponsorship, true, "string", "Elaborate on Sponsorships. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedOtherCompetitorsCheck, true, "Paid Media Expenditure - Indicate Budget as compared to other competitors");
        lbError.Text += GeneralFunction.ValidateRadioButtonList("", rblComparedOverallSpendCheck, true, "Paid Media Expenditure - Indicate Budget as compared to overall spend of the brand");

        lbError.Text += GeneralFunction.ValidateTextBox("", ddlCurrentYear, true, "", "Paid Media Expenditure - Please indicate Current Year");
        lbError.Text += GeneralFunction.ValidateTextBox("", YearPrior, true, "", "Paid Media Expenditure - Please indicate Prior Year");

        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlPaidMediaExpendituresCurrent, true, "", "Paid Media Expenditure - Please indicate Budget for Current Year");
        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlPaidMediaExpendituresPrior, true, "", "Paid Media Expenditure - Please indicate Budget for Prior Year");


        if (GetCTP().IndexOf("True") == -1)
        {
            divrptCTP.Attributes.Add("class", "TableRequired");
            lbError.Text += "Please tick the checkbox for Communications Touchpoints<br>";
        }
        else
        {
            divrptCTP.Attributes.Add("class", "TableFix");
        }

        if (GetEOM().IndexOf("True") == -1)
        {
            TableRPTExplainOtherMarket.Attributes.Add("class", "TableRequired");
            lbError.Text += "Please tick the checkbox for Question 4B<br>";
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
        entryForm.DesTotalOfCountries = ddlCountryNumber.SelectedValue;
        entryForm.CommunicationTouchPointsCheck = GetCTP();
        entryForm.ExplainListOtherMarketing = GetEOM();
        entryForm.ComparedOtherCompetitorsCheck = rblComparedOtherCompetitorsCheck.SelectedValue;
        entryForm.ComparedOverallSpendCheck = rblComparedOverallSpendCheck.SelectedValue;

        entryForm.PaidMediaExpendituresCheck = ddlCurrentYear.Text + "  " + Delimiter[0] + "  " + YearPrior.Text
            + "  " + Delimiter[0] + "  " + ddlPaidMediaExpendituresCurrent.SelectedValue + "  " + Delimiter[0] + "  " + ddlPaidMediaExpendituresPrior.SelectedValue;
        entryForm.IdEntry = EntryId;
        entryForm.ExplainWorked = txtExplainWorkedA.Text + "  " + Delimiter[0] + "  " + txtExplainWorkedB.Text + Delimiter[0] + "  " + txtExplainWorkedC.Text + Delimiter[0]; //TODO
        entryForm.OwnedMedia = txtOwnedMedia.Text;
        entryForm.Other = txtExplainListOtherMarketing.Text;
        entryForm.BringingIdea = txtBringingIdea.Text;
        entryForm.Anything = txtAnything.Text;
        entryForm.Ideas = txtIdeasA.Text + "  " + Delimiter[0] + "  " + txtIdeasB.Text + "  " + Delimiter[0] + "  " + txtIdeasC.Text + "  " + Delimiter[0];
        entryForm.StrategicChallengeObjectives = txtStrategicChallengeObjectivesA.Text + "  " + Delimiter[0] + "  " + txtStrategicChallengeObjectivesB.Text + "  " + Delimiter[0] + "  " +
                                                 txtStrategicChallengeObjectivesC.Text + "  " + Delimiter[0] + "  " + txtStrategicChallengeObjectivesD.Text + "  " + Delimiter[0] + "  " +
                                                 txtStrategicChallengeObjectivesE.Text + "  " + Delimiter[0];

        entryForm.ExecutiveSummary = txtExecutiveSummary.Text;
        entryForm.Sponsorship = txtSponsorship.Text;
        entryForm.Startdate1 = txtStartdate1.Text.ToString();
        entryForm.Enddate1 = txtEnddate1.Text.ToString();
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
        PDFURL = "../Main/EFShopperMarketingPDF.aspx?temp=Preview&Id=" + entry.Id + "&skey=" + (string)ViewState["SessionKey"];

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
        CheckBox HeaderPreDuringPost1 = null;
        CheckBox HeaderPreDuringPost2 = null;
        CheckBox HeaderPreDuringPost3 = null;
        string errorHeader = "";
        string errorBody = "";
        string CurHeaderTitle = "";
        int count = 0;
        string BodyCheck = "";
        List<CheckBox> CheckBoxList = new List<CheckBox>();
        List<CheckBox> CheckBoxListNotSelected = new List<CheckBox>();
        string msgErrorHeaderEmpty = "Please indicate if the header touchpoint(s) ran pre-shop, during or post-shop<br>";
        string msgErrorBodyEmpty = "Please indicate the specific type of touchpoint(s) used<br>";
        {
            foreach (RepeaterItem rpt in rptCTP.Items)
            {
                count++;
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                CheckBox cbItem = (CheckBox)rpt.FindControl("cbItem");
                Label Title = (Label)rpt.FindControl("Title");

                HiddenField hdAttrType = (HiddenField)rpt.FindControl("hdAttrType");
                CheckBox PreDuringPost1 = (CheckBox)rpt.FindControl("PreDuringPost1");
                CheckBox PreDuringPost2 = (CheckBox)rpt.FindControl("PreDuringPost2");
                CheckBox PreDuringPost3 = (CheckBox)rpt.FindControl("PreDuringPost3");

                Item = hd.Value + ":" + cbItem.Checked + Delimiter[1] +
                       hd.Value + ":" + PreDuringPost1.Checked + Delimiter[1] +
                       hd.Value + ":" + PreDuringPost2.Checked + Delimiter[1] +
                       hd.Value + ":" + PreDuringPost3.Checked + Delimiter[1];
                
                Temp += Item + Delimiter[0];
                
                if (hdAttrType.Value == "Body")
                {
                    CheckBoxList.Add(cbItem);
                    cbItem.Visible = true;
                    cbItem.CssClass = "Cusinput";
                    BodyCheck += cbItem.Checked + "|";
                }

                if (hdAttrType.Value == "Header" || rptCTP.Items.Count == count)
                {
                    if (string.IsNullOrEmpty(CurHeaderTitle))
                        CurHeaderTitle = Title.Text;

                    if (HeaderPreDuringPost1 == null)
                        HeaderPreDuringPost1 = PreDuringPost1;

                    if (HeaderPreDuringPost2 == null)
                        HeaderPreDuringPost2 = PreDuringPost2;

                    if (HeaderPreDuringPost3 == null)
                        HeaderPreDuringPost3 = PreDuringPost3;


                    if (CurHeaderTitle != Title.Text)
                    {
                        bool isExist = (BodyCheck.IndexOf("True") != -1);
                        if ((!HeaderPreDuringPost1.Checked && !HeaderPreDuringPost2.Checked && !HeaderPreDuringPost3.Checked) && isExist)
                        {
                            if (!HeaderPreDuringPost1.Checked)
                                HeaderPreDuringPost1.CssClass = "CusinputHeaderError";

                            if (!HeaderPreDuringPost2.Checked)
                                HeaderPreDuringPost2.CssClass = "CusinputHeaderError";

                            if (!HeaderPreDuringPost3.Checked)
                                HeaderPreDuringPost3.CssClass = "CusinputHeaderError";
                            
                            HeaderPreDuringPost1 = PreDuringPost1;
                            HeaderPreDuringPost2 = PreDuringPost2;
                            HeaderPreDuringPost3 = PreDuringPost3;

                            errorHeader = msgErrorHeaderEmpty;
                            CheckBoxList.Clear();
                        }
                        else if ((HeaderPreDuringPost1.Checked || HeaderPreDuringPost2.Checked || HeaderPreDuringPost3.Checked) && !isExist)
                        {
                            foreach (CheckBox CheckBoxitem in CheckBoxList)
                            {
                                if (!CheckBoxitem.Checked)
                                    CheckBoxListNotSelected.Add(CheckBoxitem);
                            }

                            if (CheckBoxList.Count() == CheckBoxListNotSelected.Count())
                            {
                                foreach (CheckBox CheckBoxitem in CheckBoxListNotSelected)
                                {
                                    CheckBoxitem.CssClass = "CusinputError";
                                    errorBody = msgErrorBodyEmpty;
                                }
                            }
                            
                            CheckBoxList.Clear();
                            CheckBoxListNotSelected.Clear();
                        }
                        else
                        {
                            bool isLast = (cbItem.Checked && rptCTP.Items.Count == count && hdAttrType.Value == "Body");
                            if ((!HeaderPreDuringPost1.Checked && !HeaderPreDuringPost2.Checked && !HeaderPreDuringPost3.Checked) && isLast)
                            {
                                if (!HeaderPreDuringPost1.Checked)
                                    HeaderPreDuringPost1.CssClass = "CusinputHeaderError";

                                if (!HeaderPreDuringPost2.Checked)
                                    HeaderPreDuringPost2.CssClass = "CusinputHeaderError";

                                if (!HeaderPreDuringPost3.Checked)
                                    HeaderPreDuringPost3.CssClass = "CusinputHeaderError";

                                errorHeader = msgErrorHeaderEmpty;
                            }
                            else if ((HeaderPreDuringPost1.Checked || HeaderPreDuringPost2.Checked || HeaderPreDuringPost3.Checked) && isLast)
                            {
                                foreach (CheckBox CheckBoxitem in CheckBoxList)
                                {
                                    if (!CheckBoxitem.Checked)
                                        CheckBoxListNotSelected.Add(CheckBoxitem);
                                }

                                if (CheckBoxList.Count() == CheckBoxListNotSelected.Count())
                                {
                                    foreach (CheckBox CheckBoxitem in CheckBoxListNotSelected)
                                    {
                                        CheckBoxitem.CssClass = "CusinputError";
                                        errorBody = msgErrorBodyEmpty;
                                    }
                                }

                                CheckBoxList.Clear();
                                CheckBoxListNotSelected.Clear();
                            }

                            HeaderPreDuringPost1 = PreDuringPost1;
                            HeaderPreDuringPost2 = PreDuringPost2;
                            HeaderPreDuringPost3 = PreDuringPost3;
                        }
                        CurHeaderTitle = Title.Text;
						////////
                        HeaderPreDuringPost1 = PreDuringPost1;
                        HeaderPreDuringPost2 = PreDuringPost2;
                        HeaderPreDuringPost3 = PreDuringPost3;
                        CheckBoxList.Clear();
                        CheckBoxListNotSelected.Clear();
						////////
                    }

                    BodyCheck = "";
                    if (hdAttrType.Value != "Body")
                        cbItem.Visible = false;
                }
                if (hdAttrType.Value == "SingleHeader")
                {
                    bool isExist = (BodyCheck.IndexOf("True") != -1);
                    try {
                        if ((HeaderPreDuringPost1.Checked || HeaderPreDuringPost2.Checked || HeaderPreDuringPost3.Checked) && !isExist)
                        {
                            foreach (CheckBox CheckBoxitem in CheckBoxList)
                            {
                                if (!CheckBoxitem.Checked)
                                    CheckBoxListNotSelected.Add(CheckBoxitem);
                            }

                            if (CheckBoxList.Count() == CheckBoxListNotSelected.Count())
                            {
                                foreach (CheckBox CheckBoxitem in CheckBoxListNotSelected)
                                {
                                    CheckBoxitem.CssClass = "CusinputError";
                                    errorBody = msgErrorBodyEmpty;
                                }
                            }

                            CheckBoxListNotSelected.Clear();
                        }
                    }
                    catch { }
                    
                    CheckBoxList.Clear();
                    cbItem.CssClass = "Cusinput";
                }

            }

            Item = "998" + ":" + txtOtherCTP2.Text + Delimiter[1] +
                   "998" + ":" + OtherPreDuringPost1.Checked + Delimiter[1] +
                   "998" + ":" + OtherPreDuringPost2.Checked + Delimiter[1] +
                   "998" + ":" + OtherPreDuringPost3.Checked + Delimiter[1];

            Temp += Item + Delimiter[0];
        }

        lbError.Text += errorHeader + errorBody;

        return Temp;
    }

    protected string  CheckHeaderCTP()
    {
        

        return "";
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