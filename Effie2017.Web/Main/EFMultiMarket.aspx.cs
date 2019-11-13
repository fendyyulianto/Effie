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


public partial class Main_EFMultiMarket : System.Web.UI.Page
{
    #region Global Variable
    Entry entry = null;
    List<Data.CollectData> CTPList = new List<Data.CollectData>();
    List<Data.CountriesList> explainOtherMarketingList = new List<Data.CountriesList>();
    List<Data.CountriesList> PaidMediaExpendituresListPart1 = new List<Data.CountriesList>();
    List<Data.CountriesList> PaidMediaExpendituresListPart2 = new List<Data.CountriesList>();

    string[] Delimiter = { "#&&##&&#", "&&&$$&&&", "************" };
    Guid EntryId = Guid.Empty;
    EntryForm entryForm = null;
    string  CurrentCategory = Data.DocumentCategories.MultiMarket.ToString();
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
        
        List<string> items = GeneralFunction.GetEffectivenessItems();
        CollectExplainListOtherMarketing();
        CollectPaidMediaExpenditures();

        string Msg = "Any changes will not be saved. Confirm to Proceed ?";
        btnCancel.OnClientClick = "return MsgAlert(\"" + Msg + "\");";

    }

    protected void initJavascript()
    {
        List<TextBoxKeyup> textBoxKeyup = new List<TextBoxKeyup>();

        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExecutiveSummary, LimitText = 100, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtDescribeMarket, LimitText = 250, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesA, LimitText = 450, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesB, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesC, LimitText = 175, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasA, LimitText = 300, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasB, LimitText = 25, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasC, LimitText = 150, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtBringingIdea, LimitText = 600, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedA, LimitText = 600, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedB, LimitText = 250, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainCriteria, LimitText = 100, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedC, LimitText = 20, isLimitActive = true });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtBudgetElaboration, LimitText = 100, isLimitActive = true });
        
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtStrategicChallengeObjectivesD, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtIdeasD, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtSection3, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtExplainWorkedD, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtOwnedMedia, LimitText = 0, isLimitActive = false });
        textBoxKeyup.Add(new TextBoxKeyup { textbox = txtSponsorship, LimitText = 0, isLimitActive = false });

        GeneralFunctionEntryForm.InitTextbox(textBoxKeyup, this, entry, ViewState["IsPreview"].ToString());
    }

    public void CollectCommunicationTouchPoints() {

        CTPList = Data.GetCommunicationTouchPointCountryMulti();

        if (entryForm != null)
        {

            List<Data.CountriesList> CTPListTemp = new List<Data.CountriesList>();
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
                    if (count1[0] == "999")
                    {
                        txtOtherCTP2.Text = count1[1];
                    }

                    else
                    {
                        try {

                            string[] count2 = Datas[1].Split(':');
                            string[] count3 = Datas[2].Split(':');
                            Data.CollectData CT = CTPList.FirstOrDefault(x => x.id == count1[0]);

                            CT.Data1 = Datas[0].Split(':')[1];
                            CT.Data2 = Datas[1].Split(':')[1];
                            CT.Data3 = Datas[2].Split(':')[1];
                        } catch {
                            //TODO
                        }
                    }
                }
            }
        }

        rptCTP.DataSource = CTPList;
        rptCTP.DataBind();

    }

    public void CollectExplainListOtherMarketing() {
		explainOtherMarketingList = Data.GetExplainListOtherMarketingList();
        
		if (entryForm != null)
        {
            string[] ELOMG = entryForm.ExplainListOtherMarketing.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            List<Data.CountriesList> explainOtherMarketingListTemp = new List<Data.CountriesList>();

            if (ELOMG.Count() != 0)
            {
                for (int i = 0; i < ELOMG.Length; i++)
                {
                    string[] Datas = ELOMG[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);

                    try {
                        string[] count1 = Datas[0].Split(':');
                        string[] count2 = Datas[1].Split(':');
                        string[] count3 = Datas[2].Split(':');

                        if (count1[0] == "000")
                        {
                            //DdlCountryEx1.SelectedValue = count1[1];
                            //DdlCountryEx2.SelectedValue = count2[1];
                            //DdlCountryEx3.SelectedValue = count3[1];
                        }
                        else if (count1[0] == "998")
                        {
                            txtOtherExplainOtherMarket1.Text = count1[1];
                            cbOtherExplainOtherMarket2.Checked = (count2[1] == "True") ? true : false;
                            cbOtherExplainOtherMarket3.Checked = (count3[1] == "True") ? true : false;
                            if (Datas[3].Split(':')[0] == "999")
                            {
                                cbOtherExplainOtherMarket4.Checked = (Datas[3].Split(':')[1] == "True") ? true : false;
                            }
                        }
                        else
                        {
                            try
                            {
                                Data.CountriesList ELOM = explainOtherMarketingList.FirstOrDefault(x => x.id == count1[0]);

                                ELOM.Country1 = Datas[0].Split(':')[1];
                                ELOM.Country2 = Datas[1].Split(':')[1];
                                ELOM.Country3 = Datas[2].Split(':')[1];
                            }
                            catch
                            {
                                //TODO
                            }
                        }
                    }
                    catch { }
                }
            }
        }

		RPTExplainOtherMarket.DataSource = explainOtherMarketingList;
        RPTExplainOtherMarket.DataBind();

    }

    protected void PopulateHeaderForm()
    {
        if (entry != null)
        {
            txtClassification.Text = entry.ProductClassification;
            
            try
            {
                string countries = "";
                foreach (string country in entry.Effectiveness.Split('|'))
                {
                    countries += country + ", ";
                }

                TxtCountry1.Text =  countries.Substring(0, countries.Length - 4) + ".";

                lblCountryEx1.Text = "[" + entry.Effectiveness.Split('|')[0].ToString() + "]";
                lblCountryEx2.Text = "[" + entry.Effectiveness.Split('|')[1].ToString() + "]";
                lblCountryEx3.Text = "[" + entry.Effectiveness.Split('|')[2].ToString() + "]";

                lblCountryPaid1.Text = "[" + entry.Effectiveness.Split('|')[0].ToString() + "]";
                lblCountryPaid2.Text = "[" + entry.Effectiveness.Split('|')[1].ToString() + "]";
                lblCountryPaid3.Text = "[" + entry.Effectiveness.Split('|')[2].ToString() + "]";

                lbl11ACountry1.Text = "[" + entry.Effectiveness.Split('|')[0].ToString() + "]";
                lbl11ACountry2.Text = "[" + entry.Effectiveness.Split('|')[1].ToString() + "]";
                lbl11ACountry3.Text = "[" + entry.Effectiveness.Split('|')[2].ToString() + "]";

            }
            catch
            {
                TxtCountry1.Text = "";
            }

            lblEntryID.Text = entry.Serial;
            txtCampaign.Text = entry.Campaign;
            txtBrand.Text = entry.Brand;
            lblCategoryMarket.Text = Data.GetCategoryMarket(entry.CategoryMarket);
            txtEntryCategory.Text = entry.CategoryPSDetail;
            txtStartdate1.Text = entry.DateCampaignStart.ToString("dd MMM yyyy");
            txtEnddate1.Text = entry.DateCampaignEnd.ToString("dd MMM yyyy");
        }
    }
	
    public void CollectPaidMediaExpenditures()
    {
        for (int x = 0; x <= Data.PaidMediaExpendituresPart1.GetUpperBound(0); x++)
        {
            Data.CountriesList PME = new Data.CountriesList();
            PME.id = Data.PaidMediaExpendituresPart1[x, 0];
            PME.Title = Data.PaidMediaExpendituresPart1[x, 1];
            PME.Country1 = null;
            PME.Country2 = null;
            PME.Country3 = null;
            PaidMediaExpendituresListPart1.Add(PME);
        }

        for (int x = 0; x <= Data.PaidMediaExpendituresPart2.GetUpperBound(0); x++)
        {
            Data.CountriesList PME = new Data.CountriesList();
            PME.id = Data.PaidMediaExpendituresPart2[x, 0];
            PME.Title = Data.PaidMediaExpendituresPart2[x, 1];
            PME.Country1 = null;
            PME.Country2 = null;
            PME.Country3 = null;
            PaidMediaExpendituresListPart2.Add(PME);
        }

        if (entryForm != null)
        {
            string[] PMECPart1 = { };
            string[] PMECPart2 = { };
            string[] PMECPart = entryForm.PaidMediaExpendituresCheck.Split(new string[] { Delimiter[2] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (PMECPart.Count() != 0)
            {
                if (PMECPart[0].Count() != 0) PMECPart1 = PMECPart[0].Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (PMECPart[1].Count() != 0) PMECPart2 = PMECPart[1].Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            }

            List<Data.CountriesList> PaidMediaExpendituresListTempPart1 = new List<Data.CountriesList>();
            List<Data.CountriesList> PaidMediaExpendituresListTempPart2 = new List<Data.CountriesList>();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
           

            if (PMECPart1.Count() != 0)
            {
                for (int i = 0; i < PMECPart1.Length; i++)
                {
                    string[] Datas = PMECPart1[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);

                    string[] count1 = Datas[0].Split(':');
                    string[] count2 = Datas[1].Split(':');
                    string[] count3 = Datas[2].Split(':');

                    if (count1[0] == "000")
                    {
                        //ddlCountryPaid1.SelectedValue = count1[1];
                        //ddlCountryPaid2.SelectedValue = count2[1];
                        //ddlCountryPaid3.SelectedValue = count3[1];
                    }
                    else
                    {
                        try
                        {
                            Data.CountriesList PME = PaidMediaExpendituresListPart1.FirstOrDefault(x => x.id == count1[0]);

                            PME.Country1 = Datas[0].Split(':')[1];
                            PME.Country2 = Datas[1].Split(':')[1];
                            PME.Country3 = Datas[2].Split(':')[1];

                            PaidMediaExpendituresListTempPart1.Add(PME);
                        }
                        catch
                        {
                            //TODO
                        }
                    }
                }
                rptCountryPaidPart1.DataSource = PaidMediaExpendituresListTempPart1;
            }
            else
            {
                rptCountryPaidPart1.DataSource = PaidMediaExpendituresListPart1;
            }
            rptCountryPaidPart1.DataBind();
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            

            if (PMECPart2.Count() != 0)
            {
                for (int i = 0; i < PMECPart2.Length; i++)
                {
                    string[] Datas = PMECPart2[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);

                    string[] count1 = Datas[0].Split(':');
                    string[] count2 = Datas[1].Split(':');
                    string[] count3 = Datas[2].Split(':');

                    if (count1[0] == "000")
                    {

                    }
                    else
                    {
                        try
                        {
                            Data.CountriesList PME = PaidMediaExpendituresListPart2.FirstOrDefault(x => x.id == count1[0]);

                            PME.Country1 = Datas[0].Split(':')[1];
                            PME.Country2 = Datas[1].Split(':')[1];
                            PME.Country3 = Datas[2].Split(':')[1];

                            PaidMediaExpendituresListTempPart2.Add(PME);
                        }
                        catch
                        {
                            //TODO
                        }
                    }
                }
                rptCountryPaidPart2.DataSource = PaidMediaExpendituresListTempPart2;
            }
            else
            {
                rptCountryPaidPart2.DataSource = PaidMediaExpendituresListPart2;
            }
        }
        else
        {
            rptCountryPaidPart1.DataSource = PaidMediaExpendituresListPart1;
            rptCountryPaidPart2.DataSource = PaidMediaExpendituresListPart2;
        }

        rptCountryPaidPart2.DataBind();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainCriteria, true, "string", "Please answer Question 1");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtDescribeMarket, true, "string", "Please answer Question 1A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesA, true, "string", "Please answer Question 1B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesB, true, "string", "Please answer Question 1C");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesC, true, "string", "Please answer Question 1D"); 
        lbError.Text += GeneralFunction.ValidateTextBox("", txtStrategicChallengeObjectivesD, true, "string", "Please provide sourcing for Section 1. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasA, true, "string", "Please answer Question 2A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasB, true, "string", "Please answer Question 2B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasC, true, "string", "Please answer Question 2C");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtIdeasD, true, "string", "Please provide sourcing for Section 2. Indicate NA if Not Applicable.");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtBringingIdea, true, "string", "Please answer Question 3");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtSection3, true, "string", "Please provide sourcing for Section 3. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedA, true, "string", "Please answer Question 4A");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedB, true, "string", "Please answer question 4B");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedC, true, "string", "Please answer Question 4C");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtExplainWorkedD, true, "string", "Please provide sourcing for Section 4. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtBudgetElaboration, true, "string", "Elaborate on Budget. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtOwnedMedia, true, "string", "Elaborate on Owned Media. Indicate NA if Not Applicable.  ");
        lbError.Text += GeneralFunction.ValidateTextBox("", txtSponsorship, true, "string", "Elaborate on Sponsorships. Indicate NA if Not Applicable.");
        
        //lbError.Text += GeneralFunction.ValidateDropDownList("", DdlCountryEx1, true, "", "Marketing Components - Please indicate Country for Country 1.");
        //lbError.Text += GeneralFunction.ValidateDropDownList("", DdlCountryEx2, true, "", "Marketing Components - Please indicate Country for Country 2.");
        //lbError.Text += GeneralFunction.ValidateDropDownList("", DdlCountryEx3, true, "", "Marketing Components - Please indicate Country for Country 3.");
        
        //lbError.Text += GeneralFunction.ValidateDropDownList("", ddl11ACountry1, true, "", "Communication Touchpoint - Please indicate Country for Country 1.");
        //lbError.Text += GeneralFunction.ValidateDropDownList("", ddl11ACountry2, true, "", "Communication Touchpoint - Please indicate Country for Country 2.");
        //lbError.Text += GeneralFunction.ValidateDropDownList("", ddl11ACountry3, true, "", "Communication Touchpoint - Please indicate Country for Country 3.");

        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlMediaBudget1, true, "", "Paid Media Expenditures - Please indicate Budget for Country 1.");
        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlMediaBudget2, true, "", "Paid Media Expenditures - Please indicate Budget for Country 2.");
        lbError.Text += GeneralFunction.ValidateDropDownList("", ddlMediaBudget3, true, "", "Paid Media Expenditures - Please indicate Budget for Country 3.");

        //lbError.Text += GeneralFunction.ValidateDropDownList("", ddlCountryPaid1, true, "", "Paid Media Expenditure - Please indicate Country for Country 1.");
        //lbError.Text += GeneralFunction.ValidateDropDownList("", ddlCountryPaid2, true, "", "Paid Media Expenditure - Please indicate Country for Country 2.");
        //lbError.Text += GeneralFunction.ValidateDropDownList("", ddlCountryPaid3, true, "", "Paid Media Expenditure - Please indicate Country for Country 3.");

        if (ddlMediaBudget1.SelectedValue.IndexOf("Not Applicable") != -1 || ddlMediaBudget1.SelectedValue.IndexOf("NA (Elaboration Required)") != -1)
            lbError.Text += GeneralFunction.ValidateTextBox("Total Budget Country 1", txtAnualBadge1, true, "string");
        else
            lbError.Text += GeneralFunction.ValidateTextBox("Total Budget Country 1", txtAnualBadge1, false, "string");

        if (ddlMediaBudget2.SelectedValue.IndexOf("Not Applicable") != -1 || ddlMediaBudget2.SelectedValue.IndexOf("NA (Elaboration Required)") != -1)
            lbError.Text += GeneralFunction.ValidateTextBox("Total Budget Country 2", txtAnualBadge2, true, "string");
        else
            lbError.Text += GeneralFunction.ValidateTextBox("Total Budget Country 2", txtAnualBadge2, false, "string");

        if (ddlMediaBudget3.SelectedValue.IndexOf("Not Applicable") != -1 || ddlMediaBudget3.SelectedValue.IndexOf("NA (Elaboration Required)") != -1)
            lbError.Text += GeneralFunction.ValidateTextBox("Total Budget Country 3", txtAnualBadge3, true, "string");
        else
            lbError.Text += GeneralFunction.ValidateTextBox("Total Budget Country 3", txtAnualBadge3, false, "string");

        lbError.Text += GeneralFunction.ValidateTextBox("Indicate the approximate % country 1", txtApproximateBudget1, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Indicate the approximate % country 2", txtApproximateBudget2, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Indicate the approximate % country 3", txtApproximateBudget3, true, "string");

        if (GetCTP().IndexOf("True") == -1)
        {
            divrptCTP.Attributes.Add("class", "TableRequired");
            lbError.Text += "Please tick the checkbox for Communications Touchpoints<br>";
        }
        else
        {
            divrptCTP.Attributes.Add("class", "TableFix");
        }
        if (cbOtherCTP1.Checked)
        {
            lbError.Text += GeneralFunction.ValidateTextBox("", txtOtherCTP2, true, "string", "Please answer the textbox for Communications Touchpoints (other)<br>");
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

        GetPMEPart2();

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
        try
        {
            entryForm.PaidMediaExpendituresCheck = GetPMEPart1() + GetPMEPart2();
            entryForm.PaidMediaExpendituresAstimates = ddlMediaBudget1.SelectedValue + "  " + Delimiter[0] + "  " + ddlMediaBudget2.SelectedValue + "  " + Delimiter[0] + "  " + ddlMediaBudget3.SelectedValue + "  " + Delimiter[0];
            entryForm.PaidMediaExpendituresAveregeAnnual = txtAnualBadge1.Text + "  " + Delimiter[0] + "  " + txtAnualBadge2.Text + "  " + Delimiter[0] + "  " + txtAnualBadge3.Text + "  " + Delimiter[0];
            entryForm.PaidMediaExpendituresTotalBudget = txtApproximateBudget1.Text + "  " + Delimiter[0] + "  " + txtApproximateBudget2.Text + "  " + Delimiter[0] + "  " + txtApproximateBudget3.Text + "  " + Delimiter[0];
        }
        catch { }

        entryForm.ExplainListOtherMarketing = GetEOM();
        entryForm.CommunicationTouchPointsCheck = GetCTP();
		entryForm.DesTotalOfCountries = ddlCountryNumber.SelectedValue;
        entryForm.IdEntry = EntryId;
        entryForm.ExecutiveSummary = txtExecutiveSummary.Text;
        entryForm.ExplainCriteria = txtExplainCriteria.Text;
        entryForm.DescribeMarket = txtDescribeMarket.Text;
        entryForm.StrategicChallengeObjectives = txtStrategicChallengeObjectivesA.Text + "  " + Delimiter[0] + "  " + txtStrategicChallengeObjectivesB.Text + "  " + Delimiter[0] + "  " +
                                                 txtStrategicChallengeObjectivesC.Text + "  " + Delimiter[0] + "  " + txtStrategicChallengeObjectivesD.Text + "  " + Delimiter[0];

        entryForm.Ideas = txtIdeasA.Text + "  " + Delimiter[0] + "  " +
                          txtIdeasB.Text + "  " + Delimiter[0] + "  " +
                          txtIdeasC.Text + "  " + Delimiter[0] + "  " +
                          txtIdeasD.Text + "  " + Delimiter[0];

        entryForm.BringingIdea = txtBringingIdea.Text + "  " + Delimiter[0] + "  " + txtSection3.Text + "  " + Delimiter[0];

        entryForm.ExplainWorked = txtExplainWorkedA.Text + "  " + Delimiter[0] + "  " + txtExplainWorkedB.Text + "  " + Delimiter[0] + "  " +
                                  txtExplainWorkedC.Text + "  " + Delimiter[0] + "  " + txtExplainWorkedD.Text + "  " + Delimiter[0];

        entryForm.OwnedMedia = txtOwnedMedia.Text;
        
        entryForm.CountryList = lbl11ACountry1.Text + "  " + Delimiter[0] + "  " +
                                lbl11ACountry2.Text + "  " + Delimiter[0] + "  " +
                                lbl11ACountry3.Text + "  " + Delimiter[0];

        entryForm.PaidMediaExpendituresIndicate = txtBudgetElaboration.Text;

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

    protected string GetCTP()
    {
        string Country1 = "";
        string Country2 = "";
        string Country3 = "";
        string Temp = "";
        
        {
            foreach (RepeaterItem rpt in rptCTP.Items)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                CheckBox cbCountry1 = (CheckBox)rpt.FindControl("cbCountry1");
                CheckBox cbCountry2 = (CheckBox)rpt.FindControl("cbCountry2");
                CheckBox cbCountry3 = (CheckBox)rpt.FindControl("cbCountry3");
                Country1 = hd.Value + ":" + cbCountry1.Checked;
                Country2 = hd.Value + ":" + cbCountry2.Checked;
                Country3 = hd.Value + ":" + cbCountry3.Checked;
                Temp += Country1 + Delimiter[1] + Country2 + Delimiter[1] + Country3 + Delimiter[0];
            }
            try
            {
                Temp += "998" + ":" + cbOtherCTP1.Checked + Delimiter[0];
                Temp += "999" + ":" + txtOtherCTP2.Text + Delimiter[0];

            }
            catch { }
        }

        return Temp;
    }

    protected string GetEOM()
    {
        string Country1 = "";
        string Country2 = "";
        string Country3 = "";
        string Temp = "";

        {
            Temp = "";
            Country1 = "000" + ":" + lblCountryEx1.Text;
            Country2 = "000" + ":" + lblCountryEx2.Text;
            Country3 = "000" + ":" + lblCountryEx3.Text;

            Temp += Country1 + Delimiter[1] + Country2 + Delimiter[1] + Country3 + Delimiter[0];
            foreach (RepeaterItem rpt in RPTExplainOtherMarket.Items)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                CheckBox cbCountry1 = (CheckBox)rpt.FindControl("cbCountry1");
                CheckBox cbCountry2 = (CheckBox)rpt.FindControl("cbCountry2");
                CheckBox cbCountry3 = (CheckBox)rpt.FindControl("cbCountry3");
                Country1 = hd.Value + ":" + cbCountry1.Checked;
                Country2 = hd.Value + ":" + cbCountry2.Checked;
                Country3 = hd.Value + ":" + cbCountry3.Checked;
                Temp += Country1 + Delimiter[1] + Country2 + Delimiter[1] + Country3 + Delimiter[0];
            }
            try
            {
                Country1 = "998" + ":" + txtOtherExplainOtherMarket1.Text;
                Country2 = "998" + ":" + cbOtherExplainOtherMarket2.Checked;
                Country3 = "998" + ":" + cbOtherExplainOtherMarket3.Checked;
                Temp += Country1 + Delimiter[1] + Country2 + Delimiter[1] + Country3 + Delimiter[1] + "999" + ":" + cbOtherExplainOtherMarket4.Checked + Delimiter[0];
            }
            catch { }
        }
        return Temp;
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
	
    protected string GetPMEPart1()
    {
        string Country1 = "";
        string Country2 = "";
        string Country3 = "";
        string Temp = "";

        {
            Temp = "";
            Country1 = "000" + ":" + lblCountryPaid1.Text;
            Country2 = "000" + ":" + lblCountryPaid2.Text;
            Country3 = "000" + ":" + lblCountryPaid3.Text;

            Temp += Country1 + Delimiter[1] + Country2 + Delimiter[1] + Country3 + Delimiter[0];
            foreach (RepeaterItem rpt in rptCountryPaidPart1.Items)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                TextBox cbCountry1 = (TextBox)rpt.FindControl("txtcountry1");
                TextBox cbCountry2 = (TextBox)rpt.FindControl("txtcountry2");
                TextBox cbCountry3 = (TextBox)rpt.FindControl("txtcountry3");
                Country1 = hd.Value + ":" + cbCountry1.Text;
                Country2 = hd.Value + ":" + cbCountry2.Text;
                Country3 = hd.Value + ":" + cbCountry3.Text;
                Temp += Country1 + Delimiter[1] + Country2 + Delimiter[1] + Country3 + Delimiter[0];
            }
        }
        return Temp + Delimiter[2];
    }

    protected string GetPMEPart2()
    {
        string Country1 = "";
        string Country2 = "";
        string Country3 = "";
        string Temp = "";
        {
            Temp = "";
            foreach (RepeaterItem rpt in rptCountryPaidPart2.Items)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                Label Title = (Label)rpt.FindControl("Title");
                RadioButtonList radioButtonList1 = (RadioButtonList)rpt.FindControl("rbnCountry1");
                RadioButtonList radioButtonList2 = (RadioButtonList)rpt.FindControl("rbnCountry2");
                RadioButtonList radioButtonList3 = (RadioButtonList)rpt.FindControl("rbnCountry3");
                Country1 = hd.Value + ":" + radioButtonList1.SelectedValue;
                Country2 = hd.Value + ":" + radioButtonList2.SelectedValue;
                Country3 = hd.Value + ":" + radioButtonList3.SelectedValue;
                Temp += Country1 + Delimiter[1] + Country2 + Delimiter[1] + Country3 + Delimiter[0];
                radioButtonList1.CssClass = "rdbBlock";
                radioButtonList2.CssClass = "rdbBlock";
                radioButtonList3.CssClass = "rdbBlock";

                if (Title.Text.Replace("font-weight:bold", "").IndexOf("Compared to other competitors in this cate") != -1)
                {
                    lbError.Text += GeneralFunction.ValidateRadioButtonList("", radioButtonList1, true, "Paid Media Expenditure - Indicate Budget as compared to other competitors for Country 1");
                    lbError.Text += GeneralFunction.ValidateRadioButtonList("", radioButtonList2, true, "Paid Media Expenditure - Indicate Budget as compared to other competitors for Country 2");
                    lbError.Text += GeneralFunction.ValidateRadioButtonList("", radioButtonList3, true, "Paid Media Expenditure - Indicate Budget as compared to other competitors for Country 3");
                }
                else
                {
                    lbError.Text += GeneralFunction.ValidateRadioButtonList("", radioButtonList1, true, "Paid Media Expenditure - Indicate Budget as compared to overall spend of the brand for Country 1");
                    lbError.Text += GeneralFunction.ValidateRadioButtonList("", radioButtonList2, true, "Paid Media Expenditure - Indicate Budget as compared to overall spend of the brand for Country 2");
                    lbError.Text += GeneralFunction.ValidateRadioButtonList("", radioButtonList3, true, "Paid Media Expenditure - Indicate Budget as compared to overall spend of the brand for Country 3");
                    
                }
            }
        }
        return Temp;
    }

    private void PopulateForm()
    {
        if (entry != null)
        {

            lblEntryID.Text = entry.Serial;
            txtCampaign.Text = entry.Campaign;
            txtBrand.Text = entry.Brand;
            txtEntryCategory.Text = entry.CategoryPSDetail;

            ddlMediaBudget1.Items.Clear();
            ddlMediaBudget2.Items.Clear();
            ddlMediaBudget3.Items.Clear();
            
            ddlMediaBudget1.Items.Add(new ListItem("Select Budget", ""));
            ddlMediaBudget2.Items.Add(new ListItem("Select Budget", ""));
            ddlMediaBudget3.Items.Add(new ListItem("Select Budget", ""));
            ddlCountryNumber.SelectedValue = (entry.CaseData.Split('|').Count() - 1).ToString();
            foreach (Data.CollectData str in Data.GetPaidMediaExpendituresData())
            {
                ddlMediaBudget1.Items.Add(str.Data2);
                ddlMediaBudget2.Items.Add(str.Data2);
                ddlMediaBudget3.Items.Add(str.Data2);
            }

            try
            {
                string countries = "";
                foreach (string country in entry.CaseData.Split('|'))
                {
                    countries += country + ", ";
                }
            }
            catch
            {

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
                
            }
            catch
            {
                litListCountries.Text = "No Country Selected.";
                lblTotalCountries.Text = "0";
            }
        }
        
        if (entryForm != null)
        {
            ddlCountryNumber.SelectedValue = string.IsNullOrWhiteSpace(entryForm.DesTotalOfCountries) ? ddlCountryNumber.SelectedValue : entryForm.DesTotalOfCountries;

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
                        txtExplainWorkedD.Text = ExplainWorked[3].Replace("  ", "");
                    }
                    catch { }
                }
            }
            

            txtSponsorship.Text = entryForm.Sponsorship;
            txtOwnedMedia.Text = entryForm.OwnedMedia;
            txtBudgetElaboration.Text = entryForm.PaidMediaExpendituresIndicate;
            if (entryForm.BringingIdea !="" && entryForm.BringingIdea != null)
            {
                string[] BringingIdea = entryForm.BringingIdea.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if(BringingIdea.Length != 0)
                {
                    try
                    {
                        txtBringingIdea.Text = BringingIdea[0].Replace("  ", "");
                        txtSection3.Text = BringingIdea[1].Replace("  ", "");
                    }
                    catch { }
                }
            }
            txtExplainCriteria.Text = entryForm.ExplainCriteria;

            if (entryForm.Ideas != "" && entryForm.Ideas != null)
            {
                string[] IdeasList = entryForm.Ideas.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (IdeasList.Length != 0)
                {
                    try {
                        txtIdeasA.Text = IdeasList[0].Replace("  ", ""); ;
                        txtIdeasB.Text = IdeasList[1].Replace("  ", ""); ;
                        txtIdeasC.Text = IdeasList[2].Replace("  ", ""); ;
                        txtIdeasD.Text = IdeasList[3].Replace("  ", "");
                    }
                    catch { }
                }
            }

            if (entryForm.StrategicChallengeObjectives != "" && entryForm.StrategicChallengeObjectives != null)
            {
                string[] StrategicChallengeObjectivesList = entryForm.StrategicChallengeObjectives.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (StrategicChallengeObjectivesList.Length != 0)
                {
                    try {
                        txtStrategicChallengeObjectivesA.Text = StrategicChallengeObjectivesList[0].Replace("  ", "");
                        txtStrategicChallengeObjectivesB.Text = StrategicChallengeObjectivesList[1].Replace("  ", "");
                        txtStrategicChallengeObjectivesC.Text = StrategicChallengeObjectivesList[2].Replace("  ", "");
                        txtStrategicChallengeObjectivesD.Text = StrategicChallengeObjectivesList[3].Replace("  ", "");
                    }
                    catch { }
                }
            }

            if (entryForm.PaidMediaExpendituresAveregeAnnual != "" && entryForm.PaidMediaExpendituresAveregeAnnual != null)
            {
                string[] PaidMediaExpendituresAveregeAnnual = entryForm.PaidMediaExpendituresAveregeAnnual.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (PaidMediaExpendituresAveregeAnnual.Length != 0)
                {
                    try {
                        txtAnualBadge1.Text = PaidMediaExpendituresAveregeAnnual[0].Replace("  ", "");
                        txtAnualBadge2.Text = PaidMediaExpendituresAveregeAnnual[1].Replace("  ", "");
                        txtAnualBadge3.Text = PaidMediaExpendituresAveregeAnnual[2].Replace("  ", "");
                    }
                    catch { }
                }
            }
            if (entryForm.PaidMediaExpendituresTotalBudget != "" && entryForm.PaidMediaExpendituresTotalBudget != null)
            {
                string[] PaidMediaExpendituresTotalBudget = entryForm.PaidMediaExpendituresTotalBudget.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (PaidMediaExpendituresTotalBudget.Length != 0)
                {
                    try {
                        txtApproximateBudget1.Text = PaidMediaExpendituresTotalBudget[0].Replace("  ", "");
                        txtApproximateBudget2.Text = PaidMediaExpendituresTotalBudget[1].Replace("  ", "");
                        txtApproximateBudget3.Text = PaidMediaExpendituresTotalBudget[2].Replace("  ", "");
                    }
                    catch { }
                }
            }
            if(entryForm.PaidMediaExpendituresAstimates !="" && entryForm.PaidMediaExpendituresAstimates != null)
            {
                string[] PaidMediaExpendituresAstimates = entryForm.PaidMediaExpendituresAstimates.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (PaidMediaExpendituresAstimates.Length != 0)
                {
                    try
                    {
                        ddlMediaBudget1.Text = PaidMediaExpendituresAstimates[0].Replace("  ", "");
                        ddlMediaBudget2.Text = PaidMediaExpendituresAstimates[1].Replace("  ", "");
                        ddlMediaBudget3.Text = PaidMediaExpendituresAstimates[2].Replace("  ", "");
                    }
                    catch { }
                }
            }

            txtDescribeMarket.Text = entryForm.DescribeMarket;
            txtExecutiveSummary.Text = entryForm.ExecutiveSummary;
            
           
        }
    }
    
    protected void rptCTP_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.CollectData item = (Data.CollectData)e.Item.DataItem;

            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            Label title = (Label)e.Item.FindControl("Title");
            CheckBox cbCountry1 = (CheckBox)e.Item.FindControl("cbCountry1");
            CheckBox cbCountry2 = (CheckBox)e.Item.FindControl("cbCountry2");
            CheckBox cbCountry3 = (CheckBox)e.Item.FindControl("cbCountry3");
            hd.Value = item.id;
            title.Text = item.Title;
            cbCountry1.Checked = (item.Data1 == "True") ? true : false;
            cbCountry2.Checked = (item.Data2 == "True") ? true : false;
            cbCountry3.Checked = (item.Data3 == "True") ? true : false;

            title.Text = "<b>" + item.Title + "</b>";
            if (item.AttrType == "Header")
            {
                cbCountry1.Visible = false;
                cbCountry2.Visible = false;
                cbCountry3.Visible = false;
                title.Font.Bold = true;
            }
            if (item.AttrType == "SingleHeader")
            {
                title.Font.Bold = true;
            }
        }
    }

    protected void RPTExplainOtherMarket_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.CountriesList item = (Data.CountriesList)e.Item.DataItem;

            Label title = (Label)e.Item.FindControl("Title");
            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            CheckBox country1 = (CheckBox)e.Item.FindControl("cbCountry1");
            CheckBox country2 = (CheckBox)e.Item.FindControl("cbCountry2");
            CheckBox country3 = (CheckBox)e.Item.FindControl("cbCountry3");
            hd.Value = item.id;
            title.Text = item.Title;
            country1.Checked = (item.Country1 == "True") ? true : false;
            country2.Checked = (item.Country2 == "True") ? true : false;
            country3.Checked = (item.Country3 == "True") ? true : false;
        }
    }


    protected void rptrptCountryPaidPart1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.CountriesList item = (Data.CountriesList)e.Item.DataItem;

            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            hd.Value = item.id;

            if (item.id == "000")
            {
                //ddlCountryPaid1.SelectedValue = item.Country1;
                //ddlCountryPaid2.SelectedValue = item.Country2;
                //ddlCountryPaid3.SelectedValue = item.Country3;
            }
            else
            {
                Label title = (Label)e.Item.FindControl("Title");
                TextBox country1 = (TextBox)e.Item.FindControl("txtcountry1");
                TextBox country2 = (TextBox)e.Item.FindControl("txtcountry2");
                TextBox country3 = (TextBox)e.Item.FindControl("txtcountry3");
                title.Text = item.Title;
                country1.Text = item.Country1;
                country2.Text = item.Country2;
                country3.Text = item.Country3;
            }
        }
    }


    protected void rptrptCountryPaidPart2_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.CountriesList item = (Data.CountriesList)e.Item.DataItem;

            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            hd.Value = item.id;
            if (item.id == "000")
            {
                //ddlCountryPaid1.SelectedValue = item.Country1;
                //ddlCountryPaid2.SelectedValue = item.Country2;
                //ddlCountryPaid3.SelectedValue = item.Country3;
            }
            else
            {
                Label title = (Label)e.Item.FindControl("Title");
                RadioButtonList country1 = (RadioButtonList)e.Item.FindControl("rbnCountry1");
                RadioButtonList country2 = (RadioButtonList)e.Item.FindControl("rbnCountry2");
                RadioButtonList country3 = (RadioButtonList)e.Item.FindControl("rbnCountry3");
                
                title.Text = "<span style='font-weight:bold'>" + item.Title + "</span>";
                country1.SelectedValue = item.Country1;
                country2.SelectedValue = item.Country2;
                country3.SelectedValue = item.Country3;

            }

        }
    }
}

