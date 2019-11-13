using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;
using Telerik.Web.UI;
using ClosedXML.Excel;
using System.IO;
using System.Configuration;
using Ionic.Zip;

public partial class Main_JuryDocumentsList : PageSecurity_Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }

        string parameter = Request["__EVENTARGUMENT"];
        if (parameter == "genericEmail")
            btnSend_Click(sender, e);  
    }

    private void PopulateForm()
    {
        // Refresh the cache
        GeneralFunction.ResetReportDataCache();
        GeneralFunction.GetAllJuryCache(true);

        if (GeneralFunction.GetFilterPageId() == "JuryDocumentsList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            ddlType.SelectedValue = GeneralFunction.GetFilterF3();
            ddlUploadPhoto.SelectedValue = GeneralFunction.GetFilterF4();
            ddlUploadBio.SelectedValue = GeneralFunction.GetFilterF5();                                      

            radGridJury.CurrentPageIndex = Convert.ToInt32(GeneralFunction.GetFilterPageNo());
            ViewState["AdvanceSearch"] = "1";

            HighLightYearTab(GeneralFunction.GetFilterF10());      
        }

        BindGrid(true);
    }

    private void LoadForm()
    {
        btnExport.Visible = Security.IsRoleSuperAdmin();

        // Experience Years
        List<string> effieExpYears = GeneralFunction.GetEffieExperienceYears();
        rptEffieYears.DataSource = effieExpYears;
        rptEffieYears.DataBind();

        Security.SecureControlByHiding(btnExport, "EXPORT");
        Security.SecureControlByHiding(btnDownload, "DOWNLOAD");
    }

    private void BindGrid(bool needRebind)
    {
        string invitationYear = !String.IsNullOrEmpty(GetHighLightedYearTab()) ? GetHighLightedYearTab() : GeneralFunction.GetEffieExperienceYears().Last().ToString();  // By Default, get latest year invitation list when page loads
        HighLightYearTab(invitationYear);

        InvitationList invList = InvitationList.GetInvitationList(Guid.Empty, invitationYear);        

        JuryList juryList = JuryList.GetJuryList();

        var filteredJuryList = (from jury in juryList
                                join inv in invList on jury.Id equals inv.JuryId
                                where inv.IsDeclined == false
                                select jury).ToList();
 

        List<Jury> list = new List<Jury>();

        //foreach (Jury jury in filteredJuryList)
        //{
        //    string photo = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id.ToString() + ".jpg";
        //    string profile = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo.ToString() + ".doc";
        //    string profile1 = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo.ToString() + ".docx";

        //    if (File.Exists(photo) || File.Exists(profile) || File.Exists(profile1))
        //        list.Add(jury);
        //}

        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Jury> flist = new List<Jury>();

        if (advanceSearch == "1")
        {
            foreach (Jury item in filteredJuryList)
            {
                if ((ddlType.SelectedValue == "" || (ddlType.SelectedValue != "" && item.Type == ddlType.SelectedValue)) && (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "judgeId") && item.SerialNo.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "name") && (item.FirstName.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1 || item.LastName.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Designation.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && item.Company.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)))
                   )
                {
                    bool isFilterPhoto = ddlUploadPhoto.SelectedValue.Equals("1");
                    bool isFilterBio = ddlUploadBio.SelectedValue.Equals("1");

                    bool isFilterPhotoNo = ddlUploadPhoto.SelectedValue.Equals("0");
                    bool isFilterBioNo = ddlUploadBio.SelectedValue.Equals("0"); 

                    string photo = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + item.Id.ToString() + ".jpg";
                    string profile = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + item.SerialNo.ToString() + ".doc";
                    string profile1 = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + item.SerialNo.ToString() + ".docx";

                    bool isToAdd = false;

                    if (isFilterPhoto || isFilterBio)
                    {                        
                        if (isFilterPhoto)
                        {
                            if (File.Exists(photo))
                                isToAdd = true;
                        }

                        if (isFilterBio)
                        {
                            if (File.Exists(profile) || File.Exists(profile1))
                                isToAdd = true;
                        }                        
                    }
                    else if (isFilterPhotoNo || isFilterBioNo)
                    {
                        if (isFilterPhotoNo)
                        {
                            if (!File.Exists(photo))
                                isToAdd = true;
                        }

                        if (isFilterBioNo)
                        {
                            if (!File.Exists(profile) && !File.Exists(profile1))
                                isToAdd = true;
                        }   
                    }
                    else
                    {
                        isToAdd = false;
                        flist.Add(item);
                    }

                    if (isToAdd)
                        flist.Add(item);
                }
            }

        }
        else
        {
            foreach (Jury jury in filteredJuryList)
            {
                flist.Add(jury);
            }
        }

        // Sort
        flist = flist.OrderBy(p => p.SerialNo).ToList();

        radGridJury.DataSource = flist;
        if (needRebind) radGridJury.Rebind();

        GeneralFunction.SetReportDataCache(flist);
    }

    #region Events

    protected void rptEffieYears_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string year = (string)e.Item.DataItem;
            if (year != null)
            {
                LinkButton lnkYear = (LinkButton)e.Item.FindControl("lnkYear");
                lnkYear.Text = year.ToString();
                lnkYear.CommandArgument = year.ToString();
            }
        }
    }

    protected void rptEffieYears_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("year"))
        {
            radGridJury.MasterTableView.SortExpressions.Clear();

            GeneralFunction.SetFilter("JuryDocumentsList", txtSearch.Text, ddlSearch.SelectedValue, ddlType.SelectedValue, ddlUploadPhoto.SelectedValue, ddlUploadBio.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty, e.CommandArgument.ToString(), string.Empty, radGridJury.CurrentPageIndex.ToString());
            ViewState["AdvanceSearch"] = "1";
            HighLightYearTab(e.CommandArgument.ToString());
            BindGrid(true);
            lblError.Text = "";
        }
    }

    protected void radGridJury_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Jury jury = (Jury)e.Item.DataItem;

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


            lnk = (HyperLink)e.Item.FindControl("lnkPhoto");
            
            string photo = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id.ToString() + ".jpg";
            if (File.Exists(photo))
            {
                lnk.NavigateUrl = "../Main/PhotoPreview.aspx?juryId=" + IptechLib.Crypto.StringEncryption(jury.Id.ToString());
                lnk.Text = "View";
            }

            lnk = (HyperLink)e.Item.FindControl("lnkProfile");
            string profile = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo.ToString() + ".doc";
            if (File.Exists(profile))
            {
                lnk.NavigateUrl = ConfigurationManager.AppSettings["storageVirtualPath"] + "JuryProfile//" + jury.SerialNo.ToString() + ".doc";
                lnk.Text = "View";
            }
            string profile1 = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo.ToString() + ".docx";
            if (File.Exists(profile1))
            {
                lnk.NavigateUrl = ConfigurationManager.AppSettings["storageVirtualPath"] + "JuryProfile//" + jury.SerialNo.ToString() + ".docx";
                lnk.Text = "View";
            }

            lbl = (Label)e.Item.FindControl("lblJuryUpdateJudge");
            lbl.Text = jury.DateJuryModified.Equals(DateTime.MaxValue) ? string.Empty : jury.DateJuryModified.ToString("dd/MM/yy");

            lbl = (Label)e.Item.FindControl("lblJuryUpdateAdmin");
            lbl.Text = jury.DateModified.Equals(DateTime.MaxValue) ? string.Empty : jury.DateModified.ToString("dd/MM/yy");
        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            PageSizeCombo.Items.Clear();
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
        }
    }

    protected void radGridJury_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "Edit")
        {
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Main/JuryDocumentsList.aspx");  // to return from whereever
            Response.Redirect("../Main/Jury.aspx?juryId=" + e.CommandArgument.ToString());
        }
        if (e.CommandName == "ViewJury")
        {
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Main/JuryDocumentsList.aspx");  // to return from whereever
            Response.Redirect("../Main/Jury.aspx?v=1&juryId=" + e.CommandArgument.ToString());
        }
    }

    protected void radGridJury_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //rtabEntry.Visible = false;
        GeneralFunction.SetFilter("JuryDocumentsList", txtSearch.Text, ddlSearch.SelectedValue, ddlType.SelectedValue, ddlUploadPhoto.SelectedValue, ddlUploadBio.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, radGridJury.CurrentPageIndex.ToString());
        ViewState["AdvanceSearch"] = "1";
        BindGrid(true);
        lblError.Text = "";


    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlType.SelectedValue = "";
        ddlUploadPhoto.SelectedValue = "";
        ddlUploadBio.SelectedValue = "";

        ResetYearTabs();

        GeneralFunction.ResetFilter();
        GeneralFunction.ResetReportDataCache();

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        //rtabEntry.SelectedIndex = 0;
        BindGrid(true);
        lblError.Text = "";


    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        Button btnCliced = (Button)sender;

        PopulateTemplatePanel(btnCliced);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Jury> flist = (List<Jury>)data;

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
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Salutation"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;


            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;


            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Type of Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Network"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Holding Company"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Photo"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Bio Attachment"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Jury Update By Judge"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Jury Update By Admin"); x++;

            #endregion

            y++;

            foreach (Jury jury in flist)
            {
                x = 1;

                #region Basic Jury DataRows

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Type); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SerialNo); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Email); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.FirstName); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.LastName); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Designation); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Company); x++;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.CompanyType); x++;
                string network = jury.Network;
                if (jury.NetworkOthers != "") network += " - " + jury.NetworkOthers;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(network); x++;
                string holdingcompany = jury.HoldingCompany;
                if (jury.HoldingCompanyOthers != "") holdingcompany += " - " + jury.HoldingCompanyOthers;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(holdingcompany); x++;

                string photo = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id.ToString() + ".jpg";
                if (File.Exists(photo))
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Value = jury.Id.ToString();
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Hyperlink = new XLHyperlink(ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "JuryPhoto\\" + jury.Id.ToString() + ".jpg"); x++;
                }
                else
                    x++;

                string profile = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo.ToString() + ".doc";
                string profile1 = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo.ToString() + ".docx";
                if (File.Exists(profile))
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Value = jury.SerialNo.ToString();
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Hyperlink = new XLHyperlink(ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "JuryProfile\\" + jury.SerialNo.ToString() + ".doc"); x++;
                }
                else if (File.Exists(profile1))
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Value = jury.SerialNo.ToString();
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Hyperlink = new XLHyperlink(ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "JuryProfile\\" + jury.SerialNo.ToString() + ".docx"); x++;
                }
                else
                    x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.DateJuryModified.Equals(DateTime.MaxValue) ? string.Empty : jury.DateJuryModified.ToString("dd/MM/yy")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.DateModified.Equals(DateTime.MaxValue) ? string.Empty : jury.DateModified.ToString("dd/MM/yy")); x++;

                #endregion

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

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        int counter = 0;
        bool isSelected = false;
        lblError.Text = string.Empty;

        List<MediaFileInfo> filesToInclude = new List<MediaFileInfo>();
        
        foreach (GridDataItem item in radGridJury.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            if (chkbox.Checked)
            {
                Jury jury = Jury.GetJury(new Guid(item["Id"].Text));

                string storageFileName = jury.FirstName + " " + jury.LastName + " - " + jury.SerialNo;

                string photo = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id.ToString() + ".jpg";
                string profile = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo.ToString() + ".doc";
                string profile1 = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo.ToString() + ".docx";

                if (File.Exists(photo))
                {                    
                    AddNewMediaFile(filesToInclude,storageFileName,photo,"Photo",".jpeg");
                }
                if (File.Exists(profile))
                {
                    AddNewMediaFile(filesToInclude,storageFileName,profile,"Bio",".doc");
                }
                if (File.Exists(profile1))
                {
                    AddNewMediaFile(filesToInclude,storageFileName,profile1,"Bio",".docx");
                }

                isSelected = true;
                counter++;
            }
            chkbox.Checked = false; // unchecked it
        }

        if (!isSelected)
            lblError.Text = "Select at least 1 jury to downlaod jury updates.";
        else
        {            
            string archiveName = String.Format("JuryUpdates-{0}.zip", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + archiveName + "\"");
            Response.ContentType = "application/zip";
            Response.BufferOutput = false;            

            using (ZipFile zip = new ZipFile())
            {                
                foreach(MediaFileInfo info in filesToInclude)
                {
                    zip.AddEntry(info.fileDirectory + "\\" + info.storageFileName + info.fileExtension, File.ReadAllBytes(info.fielPath));
                }                
                zip.Save(Response.OutputStream);
            }
            Response.End();
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        lblTempError.Text = string.Empty;

        lblTempError.Text += IptechLib.Validation.ValidateDropDownList("Select Template", ddlTemplateList, true, Guid.Empty.ToString());

        if (String.IsNullOrEmpty(lblTempError.Text))
        {
            GenerateEmails(new Guid(ddlTemplateList.SelectedValue));
            phSelectTemplate.Visible = false;

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "none", "<script>ClearDataOnLoad();</script>", false);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = false;
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

    #endregion

    #region Helper

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
        foreach (GridDataItem item in radGridJury.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            
            if (chkbox.Checked)
            {
                Jury jury = Jury.GetJury(new Guid(item["Id"].Text));
                Email.SendTemplateEmail(jury, templateId);
                GeneralFunction.SaveEmailSentLog(jury, templateId, evetnYear);

                chkbox.Checked = false;
                counter++;
            }
        }

        if (counter == 0)
            lblError.Text = "Please select atleat one jury to send email.<br/>";
        else
        {
            lblError.Text = "Email sent to " + (counter).ToString() + " Jury(s).<br/>";
        }
    }

    public void PopulateTemplatePanel(Button pressedButton)
    {
        lblTempError.Text = string.Empty;
        lblError.Text = string.Empty;

        IptechLib.Forms.RemoveHighlightControls(phSelectTemplate);

        List<EmailTemplate> defaultEmailTempalteList = EmailTemplateList.GetEmailTemplateList(Guid.Empty).Where(m => m.TemplateId != new Guid(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultTemplateId")[0].Value)
            && m.IsActive && !m.IsInvitation).ToList();

        if (defaultEmailTempalteList.Count == 0)
            lblError.Text = "No Email Template found.<br/>";

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

    public void HighLightYearTab(string selectedYear)
    {
        foreach (RepeaterItem item in rptEffieYears.Items)
        {
            LinkButton lnkYear = (LinkButton)item.FindControl("lnkYear");

            if (lnkYear.CommandArgument.ToString().Equals(selectedYear))
            {
                lnkYear.CssClass = "highlightTab";
            }
            else
                lnkYear.CssClass = "";
        }
    }

    public string GetHighLightedYearTab()
    {
        foreach (RepeaterItem item in rptEffieYears.Items)
        {
            LinkButton lnkYear = (LinkButton)item.FindControl("lnkYear");

            if (lnkYear.CssClass == "highlightTab")
                return lnkYear.CommandArgument.ToString();
        }
        return string.Empty;
    }

    public void ResetYearTabs()
    {
        foreach (RepeaterItem item in rptEffieYears.Items)
        {
            LinkButton lnkYear = (LinkButton)item.FindControl("lnkYear");

            lnkYear.CssClass = string.Empty;
        }
    }

    public void AddNewMediaFile(List<MediaFileInfo> filesToInclude, string storageFileName, string fileName, string directory, string fileExtension)
    {
        MediaFileInfo mediaInfo = new MediaFileInfo();
        mediaInfo.storageFileName = storageFileName;
        mediaInfo.fielPath = fileName;
        mediaInfo.fileDirectory = directory;
        mediaInfo.fileExtension = fileExtension;
        filesToInclude.Add(mediaInfo);
    }

    #endregion
    
}

public class MediaFileInfo
{
    public string storageFileName { get; set; }
    public string fielPath { get; set; }
    public string fileDirectory { get; set; }
    public string fileExtension { get; set; }
}