using System;
using System.Web.UI.WebControls;
using System.Linq;
using EffieJuryManagementApp;
using Telerik.Web.UI;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;
 
public partial class Main_JuryList : PageSecurity_Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }

        string parameter = Request["__EVENTARGUMENT"];
        if (parameter == "inviteEmail")
            btnSend_Click(sender, e);
    }

    private void PopulateForm()
    {
        // Refresh the cache
        GeneralFunction.ResetReportDataCache();
        GeneralFunction.GetAllJuryCache(true);

        string tabValue = Request.QueryString["tab"];
        if (tabValue != null)
        {
            if (tabValue.Equals("0"))
            {
                rtabEntry.SelectedIndex = 1;
                ViewState["TabFilterValue"] = rtabEntry.SelectedTab.Value;
            }
            else if (tabValue.Equals("1"))
            {
                rtabEntry.SelectedIndex = 2;
                ViewState["TabFilterValue"] = rtabEntry.SelectedTab.Value;
            }

        }
        else
        {
            rtabEntry.SelectedIndex = 1;
            ViewState["TabFilterValue"] = rtabEntry.SelectedTab.Value;
        }

        if (GeneralFunction.GetFilterPageId() == "JuryList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            
            //ddlNetwork.SelectedValue = GeneralFunction.GetFilterF3();
            try { ddlNetwork.SelectedValue = GeneralFunction.GetFilterF3(); }
            catch { ddlNetwork.SelectedValue = ""; }

            //ddlHoldingCompany.SelectedValue = GeneralFunction.GetFilterF4();
            try { ddlHoldingCompany.SelectedValue = GeneralFunction.GetFilterF4(); }
            catch { ddlHoldingCompany.SelectedValue = ""; }

            ddlCountry.SelectedValue = GeneralFunction.GetFilterF5();
            ddlType.SelectedValue = GeneralFunction.GetFilterF6();
            radGridJury.CurrentPageIndex = Convert.ToInt32(GeneralFunction.GetFilterPageNo());
            ViewState["AdvanceSearch"] = "1";            
        }                    

        BindGrid(true);


        if (Security.IsRoleReadOnlyAdmin())
        {
            btnSendEmail.Visible = false;
            newJury.Visible = false;
            btnAddRound.Visible = false;
        }
    }

    private void LoadForm()
    {
        btnExport.Visible = Security.IsRoleSuperAdmin();

        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryListFromJury(true);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));

        // Network
        GeneralFunction.LoadddlNetwork(ddlNetwork, false, false);
        ddlNetwork.Items.Insert(0, new ListItem("All", ""));

        // Holding COmpany
        GeneralFunction.LoadddlHoldingCompany(ddlHoldingCompany, false, false);
        ddlHoldingCompany.Items.Insert(0, new ListItem("All", ""));

        Security.SecureControlByHiding(btnExport, "EXPORT");
    }

    private void BindGrid(bool needRebind)
    {
        JuryList list = JuryList.GetJuryList();

        string status = (string)ViewState["TabFilterValue"];
        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Jury> flist = new List<Jury>();

        if (advanceSearch == "1")
        {
            foreach (Jury item in list)
            {
                if (
                     (ddlDelete.SelectedValue == "" || (ddlDelete.SelectedValue != "" && item.IsToDelete.ToString() == ddlDelete.SelectedValue)) &&
                    (ddlType.SelectedValue == "" || (ddlType.SelectedValue != "" && item.Type == ddlType.SelectedValue)) &&
                    (ddlNetwork.SelectedValue == "" || (ddlNetwork.SelectedValue != "" && item.Network == ddlNetwork.SelectedValue)) &&
                    (ddlHoldingCompany.SelectedValue == "" || (ddlHoldingCompany.SelectedValue != "" && item.HoldingCompany == ddlHoldingCompany.SelectedValue)) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && item.Country == ddlCountry.SelectedValue)) &&
                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "judgeId") && item.SerialNo.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "name") && (item.FirstName.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1 || item.LastName.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Designation.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && item.Company.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))
                    )
                   )
                    flist.Add(item);
            }            
        }
        else
        {
            if (status != null)
            {
                if (status.Equals("Current"))
                    flist = list.Where(m => !m.IsToDelete).ToList();
                else if (status.Equals("Deleted"))
                    flist = list.Where(m => m.IsToDelete).ToList();
                else
                    flist = list.ToList();
            }
        }

        // Sort
        flist = flist.OrderBy(p => p.SerialNo).ToList();

        radGridJury.DataSource = flist;
        if (needRebind) radGridJury.Rebind();      

        GeneralFunction.SetReportDataCache(flist);        
    }

    public void PopulateTemplatePanel(Button pressedButton)
    {
        lblTempError.Text = string.Empty;
        lblError.Text = string.Empty;

        IptechLib.Forms.RemoveHighlightControls(phSelectTemplate);

        ddlRounds.Items.Clear();
        ddlRounds.Items.Add(new ListItem("Round 1", RoundsType.Round1));
        ddlRounds.Items.Add(new ListItem("Round 2", RoundsType.Round2));
        ddlRounds.Items.Add(new ListItem("Round 1 & 2", RoundsType.BothRounds));

        if (!pressedButton.CommandName.Equals("add"))
            ddlRounds.Items.Add(new ListItem("N/A", RoundsType.NotApplicable));

        ddlRounds.Items.Insert(0, new ListItem("Please Select", string.Empty.ToString()));

        List<EmailTemplate> defaultEmailTempalteList = EmailTemplateList.GetEmailTemplateList(Guid.Empty).Where(m => m.TemplateId != new Guid(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultTemplateId")[0].Value)
            && m.IsActive && m.UserData2.Equals(EmailCategory.Invitation) && !m.IsDelete).ToList();

        if (defaultEmailTempalteList.Count == 0)
            lblError.Text = "No Email Template found.<br/>";

        roundRow.Visible = pressedButton.CommandName.Equals("add");
        templateRow.Visible = !pressedButton.CommandName.Equals("add");

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

    #region Events
   
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
            
            ((GridDataItem)e.Item)["Status"].Text = jury.IsToDelete ? "Del" : "Cur";
            
            // Jury Name
            lnk = (HyperLink)e.Item.FindControl("lnkJuryName");
            lnk.Text = jury.FirstName + " " + jury.LastName;
            lnk.NavigateUrl = "mailto:" + jury.Email;

            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            lnkBtn.CommandArgument = jury.Id.ToString();

            HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");
            hdfId.Value = jury.Id.ToString();
           
            lnk = (HyperLink)e.Item.FindControl("hlkEmailHistory");
            lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
            lnk.NavigateUrl = "./EmailSentHistory.aspx?juryId=" + jury.Id.ToString();

            CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");

            try {
                InvitationList invList = InvitationList.GetInvitationList(jury.Id, Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value);
                if (invList.Count > 0)
                {
                    chkbox.Enabled = false;
                }
            }
            catch { }


            lnk = (HyperLink)e.Item.FindControl("hlkEmailHistory");
            lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
            lnk.NavigateUrl = "./EmailSentHistory.aspx?juryId=" + jury.Id.ToString();

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
            lnkDelete.CommandArgument = jury.Id.ToString();
            LinkButton lnkRestore = (LinkButton)e.Item.FindControl("lnkRestore");
            lnkRestore.CommandArgument = jury.Id.ToString();

            CheckBox chkBox = (CheckBox)e.Item.FindControl("chkbox");

            switch ((string)ViewState["TabFilterValue"])
            {
                case "All":
                    lnkRestore.Visible = false;
                    lnkDelete.Visible = true;
                    chkBox.Visible = true;
                    break;
                case "Deleted":
                    lnkDelete.Visible = false;
                    lnkRestore.Visible = true;
                    chkBox.Visible = false;
                    break;
                default:
                    lnkDelete.Visible = true;
                    chkBox.Visible = true;
                    break;

            }
            
            if (Security.IsRoleReadOnlyAdmin())
            {
                lnkDelete.Visible = false;
            }

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
            GeneralFunction.SetFilter("JuryList", txtSearch.Text, ddlSearch.SelectedValue, ddlNetwork.SelectedValue, ddlHoldingCompany.SelectedValue, ddlCountry.SelectedValue, ddlType.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, radGridJury.CurrentPageIndex.ToString());
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Main/JuryList.aspx");  // to return from whereever
            Response.Redirect("../Main/Jury.aspx?juryId=" + e.CommandArgument.ToString());
        }
        else if (e.CommandName == "ViewJury")
        {
            GeneralFunction.SetFilter("JuryList", txtSearch.Text, ddlSearch.SelectedValue, ddlNetwork.SelectedValue, ddlHoldingCompany.SelectedValue, ddlCountry.SelectedValue, ddlType.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, radGridJury.CurrentPageIndex.ToString());
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Main/JuryList.aspx");  // to return from whereever
            Response.Redirect("../Main/Jury.aspx?v=1&juryId=" + e.CommandArgument.ToString());
        }
        else if (e.CommandName == "delete")
        {
            Jury jury = Jury.GetJury(new Guid (e.CommandArgument.ToString()));
            if (jury != null)
            {
                if (jury.IsActive)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", "<script>function f(){AlertJuryActive('" + jury.SerialNo + "','" + jury.Id.ToString() + "');Sys.Application.remove_load(f) ;}; Sys.Application.add_load(f)  ;</script>");
                }
                else
                {
                    jury.IsToDelete = true;
                    jury.IsActive = false;
                    jury.DateModifiedString = DateTime.Now.ToString();
                    jury.Save();

                    Response.Redirect("../Main/JuryList.aspx?tab=0");
                }
            }            
        }
        else if (e.CommandName == "restore")
        {
            Jury jury = Jury.GetJury(new Guid(e.CommandArgument.ToString()));
            if (jury != null)
            {
                jury.IsToDelete = false;
                jury.DateModifiedString = DateTime.Now.ToString();
                jury.Save();
            }
            Response.Redirect("../Main/JuryList.aspx?tab=1");
        }
        else if (e.CommandName == "DelateConfirm")
        {
            Jury jury = Jury.GetJury(new Guid(e.CommandArgument.ToString()));
            if (jury != null)
            {
                jury.IsToDelete = true;
                jury.IsActive = false;
                jury.DateModifiedString = DateTime.Now.ToString();
                jury.Save();
            }
            Response.Redirect("../Main/JuryList.aspx?tab=0");
        }
    }

    protected void radGridJury_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rtabEntry.Visible = false;
        GeneralFunction.SetFilter("JuryList", txtSearch.Text, ddlSearch.SelectedValue, ddlNetwork.SelectedValue, ddlHoldingCompany.SelectedValue, ddlCountry.SelectedValue, ddlType.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "0");
        ViewState["AdvanceSearch"] = "1";
        ViewState["TabFilterValue"] = "";
        BindGrid(true);
        lblError.Text = "";


    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        rtabEntry.Visible = true;
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlType.SelectedValue = "";
        ddlNetwork.SelectedValue = "";
        ddlHoldingCompany.SelectedValue = "";
        ddlCountry.SelectedValue = "";

        rtabEntry.SelectedIndex = 1;

        GeneralFunction.ResetFilter();
        GeneralFunction.ResetReportDataCache();

        radGridJury.MasterTableView.SortExpressions.Clear();

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        //rtabEntry.SelectedIndex = 0;
        BindGrid(true);
        lblError.Text = "";
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Jury.aspx");
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<string> effieExpYears = GeneralFunction.GetEffieExperienceYears();

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
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Password"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Salutation"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Contact"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Mobile"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("City"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Postal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Name"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Tel"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Address1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Address2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Email"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Bio"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Type of Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Network"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Holding Company"); x++;

            #endregion


            #region Market Exp Headers

            List<string> marketExpItems = GeneralFunction.GetMarketExperienceItems();
            foreach (string marketExp in marketExpItems)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(marketExp); x++;
            }
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Others"); x++;

            #endregion


            #region Industry Exp Headers

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AG"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AU"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("BW"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("BA"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CE"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CR"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FP"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FM"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FD"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("GV"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HC"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HS"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IT"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ME"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RE"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RS"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RT"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TT"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Others"); x++;

            #endregion


            #region Misc Headers

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Specialist Skills"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Specialist Skills - Others"); x++;

            foreach (string effieExpYear in effieExpYears)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("APAC Effie Exp - " + effieExpYear); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("APAC Effie Exp Remarks - " + effieExpYear); x++;
            }

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Effie Programs"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Effie Programs - Others"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Judging Experience"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Relevant Information"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Remarks"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Source"); x++;            
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reference"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Receive Updates from APAC Effie"); x++;            


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
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Password); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.FirstName); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.LastName); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Designation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(IptechLib.Text.ShowFriendlyContact(jury.Contact)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(IptechLib.Text.ShowFriendlyContact(jury.Mobile)); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Company); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Address1); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Address2); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.City); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Postal); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Country); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAName); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(IptechLib.Text.ShowFriendlyContact(jury.PATel)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAAddress1); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAAddress2); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAEmail); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Profile); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.CompanyType); x++;
                string network = jury.Network;
                if (jury.NetworkOthers != "") network += " - " + jury.NetworkOthers;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(network); x++;
                string holdingcompany = jury.HoldingCompany;
                if (jury.HoldingCompanyOthers != "") holdingcompany += " - " + jury.HoldingCompanyOthers;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(holdingcompany); x++;


                #endregion


                #region Market Exp

                foreach (string marketExp in marketExpItems)
                {
                    if (jury.MarketExp.IndexOf(marketExp) != -1)
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                    x++;
                }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.MarketExpOthers); x++;

                #endregion


                #region Industry Exp

                if (jury.IndustryExp.IndexOf("Agricultural & Industrial") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Automotive") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Beauty & Wellness") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Beverages") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Consumer Electronics and Durables") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Corporate Reputation/Professional Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Financial Products & Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("FMCG") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Food") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Government / Institutional") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Healthcare") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Household Supplies & Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("IT /Telco") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Media, Entertainment & Leisure") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Real Estate") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Restaurants") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Retail") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Travel / Tourism") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.IndustryExpOthers); x++;

                #endregion


                #region Misc

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Skills); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SkillsOthers); x++;

                int effieExpCounter = 0;
                foreach (string effieExpYear in effieExpYears)
                {
                    string yearString = string.Empty;
                    string year = string.Empty;
                    string yearRemark = string.Empty;

                    try
                    {
                        yearString = jury.EffieExpYear.Split('|')[effieExpCounter];
                        year = yearString.Split('#')[0];
                        yearRemark = yearString.Split('#')[1];
                    }
                    catch { }

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(effieExpYear.Equals(year) ? "Yes" : "No"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(yearRemark); x++;

                    effieExpCounter++;
                }

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.EffieExpProgram); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.EffieExpProgramOthers); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.OtherJudgingExp); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.RevelantExp); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Remarks); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Source); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Reference); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.IsReceiveUpdate ? "Yes" : "No"); x++;     

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

    protected void ddlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString()))
        {
            hlkPreview.CssClass = "fancybox fancybox.iframe tblLinkRed";
            hlkPreview.NavigateUrl = "./EmailPreview.aspx?etmId=" + ddlTemplateList.SelectedValue.ToString();
        }

        hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());        
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        lblTempError.Text = string.Empty;

        lblTempError.Text += IptechLib.Validation.ValidateDropDownList("Select Rounds", ddlRounds, roundRow.Visible, string.Empty.ToString());
        lblTempError.Text += IptechLib.Validation.ValidateDropDownList("Select Template", ddlTemplateList, templateRow.Visible, Guid.Empty.ToString());

        if (String.IsNullOrEmpty(lblTempError.Text))
        {
            SendInvitationCriteria invCriteria = new SendInvitationCriteria();

            if (roundRow.Visible)
            {
                invCriteria.isRound1 = ddlRounds.SelectedValue.Equals(RoundsType.Round1) || ddlRounds.SelectedValue.Equals(RoundsType.BothRounds);
                invCriteria.isRound2 = ddlRounds.SelectedValue.Equals(RoundsType.Round2) || ddlRounds.SelectedValue.Equals(RoundsType.BothRounds);
                invCriteria.isSend = false;

                GenerateInvitation(invCriteria, new Guid(ddlTemplateList.SelectedValue));
            }
            else
            {
                EmailTemplate choosenTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));

                if (choosenTemplate != null)
                {
                    invCriteria.isRound1 = choosenTemplate.UserData1.Equals(RoundsType.Round1) || choosenTemplate.UserData1.Equals(RoundsType.BothRounds);
                    invCriteria.isRound2 = choosenTemplate.UserData1.Equals(RoundsType.Round2) || choosenTemplate.UserData1.Equals(RoundsType.BothRounds);
                    invCriteria.isSend = true;

                    if (choosenTemplate.IsInvitation)
                        GenerateInvitation(invCriteria, new Guid(ddlTemplateList.SelectedValue));
                    else
                        GenerateEmails(new Guid(ddlTemplateList.SelectedValue));
                }
            }            

            phSelectTemplate.Visible = false;

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "none", "<script>ClearDataOnLoad();</script>", false);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = false;
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        if (sender != null)
        {
            Button btnCliced = (Button)sender;

            PopulateTemplatePanel(btnCliced);
        }
    }

    protected void btnAddRound_Click(object sender, EventArgs e)
    {
        Button btnCliced = (Button)sender;

        PopulateTemplatePanel(btnCliced);
    }

    protected void rtabEntry_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        Telerik.Web.UI.RadTab TabClicked = e.Tab;
        string tabvalue = TabClicked.Value;

        ViewState["TabFilterValue"] = tabvalue;
        ViewState["AdvanceSearch"] = "";
        lblError.Text = "";
        BindGrid(true);

        // Display the reminder buttons conditionally
        btnAddRound.Visible = false;
        btnSendEmail.Visible = false;
        switch (tabvalue)
        {
            case "All":
                btnAddRound.Visible = true;
                btnSendEmail.Visible = true;
                break;
            case "Deleted":
                btnAddRound.Visible = false;
                btnSendEmail.Visible = false;
                break;
            case "Current":
                btnAddRound.Visible = false;
                btnSendEmail.Visible = false;
                break;
        }
    }

    #endregion

    #region Helper

    public void GenerateInvitation(SendInvitationCriteria invCriteria, Guid templateId)
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
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

            if (chkbox.Checked)
            {
                Jury jury = Jury.GetJury(new Guid(hdfId.Value.ToString()));

                InvitationList invList = InvitationList.GetInvitationList(jury.Id, Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value);

                Invitation inv = null;
                
                if (invList.Count > 0)
                {
                    inv = invList[0];
                }
                else
                {
                    inv = Invitation.NewInvitation();
                }

                inv.EventCode = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value.ToString();
                inv.JuryId = jury.Id;

                if (invCriteria.isRound1)
                {
                    inv.IsRound1Invited = true;
                    inv.DateRound1EmailSentString = DateTime.Now.ToString();
                }
                if (invCriteria.isRound2)
                {
                    inv.IsRound2Invited = true;
                    inv.DateRound2EmailSentString = DateTime.Now.ToString();
                }

                if (inv.IsNew)
                    inv.DateCreatedString = DateTime.Now.ToString();

                inv.DateModifiedString = DateTime.Now.ToString();
               
                if (invCriteria.isSend)
                {
                    Email.SendInvitationTemplateEmail(inv, templateId);
                    GeneralFunction.SaveEmailSentLog(jury, templateId, evetnYear);
                }
                else
                    inv.IsLocked = true;

                if (inv.IsValid)
                    inv.Save();

                chkbox.Checked = false;
                counter++;
            }
        }

        if (counter == 0)
            lblError.Text = "Please select atleat one jury to send Invitation.<br/>";
        else
        {
            if (invCriteria.isSend)
                lblError.Text = "Email sent to " + (counter).ToString() + " Jury(s).<br/>";
            else
                lblError.Text = "Invitation added for " + (counter).ToString() + " Jury(s).<br/>";
        }
    }

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
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

            if (chkbox.Checked)
            {
                Jury jury = Jury.GetJury(new Guid(hdfId.Value.ToString()));
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
    
    #endregion
}

[Serializable]
public class SendInvitationCriteria
{
    public bool isRound1 { get; set; }
    public bool isRound2 { get; set; }
    public bool isSend { get; set; }
    public string title { get; set; }
    public Guid templateId { get; set; }
}