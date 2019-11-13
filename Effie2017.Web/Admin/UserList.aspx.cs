using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using Telerik.Web.UI;
using ClosedXML.Excel;
using System.IO;

public partial class Admin_UserList : PageSecurity_Admin
{
    int counter;
    public static string BodyTamplateMail = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //up1.Save_Clicked += new EventHandler(PaySave_Clicked);
        //up1.Cancel_Clicked += new EventHandler(PayCancel_Clicked);


        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }


        //string parameter = Request["__EVENTARGUMENT"];
        //if (parameter == "inviteEmail")
        //    btnSend_Click(sender, e);
    }
    private void PopulateForm()
    {
        // Refresh the cache
        GeneralFunction.ResetReportDataCache();
        GeneralFunction.GetAllEntryCache(true);
        GeneralFunction.GetAllRegistrationCache(true);

        ViewState["TabFilterValue"] = ""; // default
        BindGrid(true);
        
        // Readonly Admin
        btnEmailReminder.Visible = !Security.IsReadOnlyAdmin();
        btnVerify.Visible = !Security.IsReadOnlyAdmin();
    }
    private void LoadForm()
    {
        // Payment Status
        ddlPaymentStatus.Items.Add(new ListItem("All", "All"));
        ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentEntryStatus(StatusPaymentEntry.Paid), StatusPaymentEntry.Paid));
        ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentEntryStatus(StatusPaymentEntry.NotPaid), StatusPaymentEntry.NotPaid));

        // Entry Status
        ddlEntryStatus.Items.Add(new ListItem("All", ""));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadPending), StatusEntry.UploadPending));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadCompleted), StatusEntry.UploadCompleted));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.Completed), StatusEntry.Completed));


        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryList(false);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));



        Security.SecureControlByHiding(btnExport, "EXPORT");
    }


    protected void radGridEntry_SortCommand(object Sender, GridSortCommandEventArgs e)
    {
        //if (e.CommandArgument == "Country")  //This Line Made ALL sorting functions of the title/labels are NOT working
        {
            BindGrid(true, e.CommandArgument.ToString(), e.NewSortOrder, true);
        }
    }

    private void BindGrid(bool needRebind, string sortExpression = "", GridSortOrder order = GridSortOrder.None, bool isCustomSort = false)
    {
        RegistrationList list = GeneralFunction.GetAllRegistrationCache(needRebind);
        EntryList elist = GeneralFunction.GetAllEntryCache(false);

        // filter
        string status = (string)ViewState["TabFilterValue"];
        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Registration> flist = new List<Registration>();


        if (advanceSearch == "1")
        {
            // Advanced search 
            //string payStatus = ddlPaymentStatus.SelectedValue;



            foreach (Registration item in list)
            {
                if (
                    (ddlPaymentStatus.SelectedValue == "All" || (ddlPaymentStatus.SelectedValue != "All" && GeneralFunction.IsRegistrationInList(GeneralFunction.GetRegIdWithEntryPayStatus(ddlPaymentStatus.SelectedValue), item))) &&
                    (ddlEntryStatus.SelectedValue == "" || (ddlEntryStatus.SelectedValue != "" && GeneralFunction.IsRegistrationInList(GeneralFunction.GetRegIdWithEntryStatus(ddlEntryStatus.SelectedValue), item))) &&
                    (ddlActive.SelectedValue == "" || (ddlActive.SelectedValue != "" && GeneralFunction.IsRegistrationInList(GeneralFunction.GetRegIdWithSubmittedEntries(ddlActive.SelectedValue), item))) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && item.Country == ddlCountry.SelectedValue)) &&
                    (ddlIsVerified.SelectedValue == "" || (ddlIsVerified.SelectedValue != "" && item.IsVerified == bool.Parse(ddlIsVerified.SelectedValue))) &&

                    (cblMember.SelectedValue == "" || (
                        (cblMember.SelectedValue != "" && cblMember.Items[0].Selected && item.IsCAAAA) ||
                        (cblMember.SelectedValue != "" && cblMember.Items[1].Selected && item.IsAPEP) ||
                        (cblMember.SelectedValue != "" && cblMember.Items[2].Selected && item.IsEProg))
                    ) &&



                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && item.Company.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "email") && item.Email.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "firstname") && item.Firstname.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "lastname") && item.Lastname.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "contact") && item.Contact.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))
                    )



                   )
                    flist.Add(item);
            }

        }
        else
        {
            // tab filtering
            if (status == "" || status == "DIS")
            {
                foreach (Registration item in list)
                    if (status == "" || (status != "" && item.Status == status)) flist.Add(item);
            }
            else if (status == "XXX")
            {
                foreach (Registration item in list)
                    if (status == "" || (status != "" && !item.IsActive)) flist.Add(item);
            }
            else
            {
                flist = GeneralFunction.GetRegIdWithSubmittedEntries(status);
            }

        }

        if (isCustomSort)
        {
            List<Registration> regList = RegistrationList.GetRegistrationList(string.Empty, string.Empty, string.Empty).ToList();

            if (sortExpression.Equals("Country"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Country).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Country).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Country).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("DateCreated"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.DateCreated).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.DateCreated).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.DateCreated).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Contact"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Contact).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Contact).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Contact).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Job"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Job).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Job).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Job).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("DateReminder"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.DateReminder(x.Id, EmailTypeEnum.UserList.ToString())).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.DateReminder(x.Id, EmailTypeEnum.UserList.ToString())).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.DateReminder(x.Id, EmailTypeEnum.UserList.ToString())).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Firstname"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Firstname).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Firstname).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Firstname).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Lastname"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Lastname).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Lastname).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Lastname).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Company"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Company).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Company).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Company).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Email"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Email).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Email).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Email).ToList();
                        break;
                }
            }
        }

        counter = 1;
        radGridUser.DataSource = flist;
        if (needRebind) radGridUser.DataBind();


        GeneralFunction.SetReportDataCache(flist);
    }

    #region Events
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.Registration reg = (Effie2017.App.Registration)e.Item.DataItem;

            LinkButton lnkBtn = null;
            Label lbl = null;
            HyperLink lnk = null;
            CheckBox chkbox = null;

            // No
            //lbl = (Label)e.Item.FindControl("lblNo");
            //lbl.Text = counter.ToString();


            // submitted by
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnSubmittedBy");
            lnkBtn.Text = reg.Company;
            lnkBtn.CommandArgument = reg.Id.ToString();

            // Email
            lnk = (HyperLink)e.Item.FindControl("lnkEmail");
            lnk.Text = reg.Email;
            lnk.NavigateUrl = "mailto:" + reg.Email;

            // Contact
            ((GridDataItem)e.Item)["Contact"].Text = GeneralFunction.ShowFriendlyContact(reg.Contact);


            try {
                string DateReminder = "";
                if (reg.DateReminder(reg.Id, EmailTypeEnum.UserList.ToString()) != DateTime.MinValue)
                    DateReminder = reg.DateReminder(reg.Id, EmailTypeEnum.UserList.ToString()).ToString("dd/MM/yy H:mm");

                ((GridDataItem)e.Item)["DateReminder"].Text = DateReminder;
            }
            catch {
                ((GridDataItem)e.Item)["DateReminder"].Text = "";
            }

            // CAAAA
            lbl = (Label)e.Item.FindControl("lbCAAAA");
            lbl.Text = GeneralFunction.ExtractBracketValue(reg.Caaaa);



            // APEP
            lbl = (Label)e.Item.FindControl("lbAPEP");
            lbl.Text = GeneralFunction.ExtractBracketValue(reg.Apep);



            // view
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnView");
            lnkBtn.CommandArgument = reg.Id.ToString();

            // edit
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            lnkBtn.CommandArgument = reg.Id.ToString();
            Security.SecureControlByHiding(lnkBtn);

            // is verified
            lbl = (Label)e.Item.FindControl("lbIsVerified");
            lbl.Text = reg.IsVerified ? "Yes" : "No";

            // chkbox verification
            //chkbox = (CheckBox)e.Item.FindControl("chkbox");
            //if (reg.Status == StatusRegistration.OK && !reg.IsVerified)
            //    chkbox.Enabled = true;
            //else
            //    chkbox.Enabled = false;


            lnk = (HyperLink)e.Item.FindControl("hlkEmailHistory");
            lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
            lnk.NavigateUrl = "./RegistrationEmailSentHistory.aspx?regId=" + reg.Id.ToString();


            counter++;
        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            PageSizeCombo.Items.Clear();
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", radGridUser.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", radGridUser.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", radGridUser.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", radGridUser.MasterTableView.ClientID);
            PageSizeCombo.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
        }
    }



    protected void btnEmailReminder_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = true;
        phPopupEmailReminder.Attributes.Add("class", "ModalPopUpSmall");
        divEditTamplate.Visible = false;
        int isChecked = 0;
        if (sender != null)
        {
            phPopupEmailReminder.Attributes.Add("class", "ModalPopUpSmall");
            divEditTamplate.Visible = false;
            if (sender != null)
            {
                Button btnCliced = (Button)sender;

                foreach (GridDataItem item in radGridUser.MasterTableView.Items)
                {
                    CheckBox CheckBox1 = item.FindControl("chkbox") as CheckBox;
                    if (CheckBox1 != null && CheckBox1.Checked)
                    {
                        isChecked += 1;
                    }
                }
                if (isChecked == 0)
                {
                    lblError.Text = "Plese select record to send email.";
                }
                else if (isChecked >= 1)
                {
                    PopulateTemplatePanel(btnCliced);
                }
            }
        }
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
        
        List<EmailTemplate> defaultEmailTempalteList = EmailTemplateList.GetEmailTemplateList(Guid.Empty)
             .Where(x => x.EmailType == EmailType.Entry.ToString() && x.IsActive && !x.IsDelete).ToList();

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

    protected void ddlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString()))
        {
            hlkPreview.CssClass = "fancybox fancybox.iframe tblLinkRed";
            hlkPreview.NavigateUrl = "./EmailPreview.aspx?etmId=" + ddlTemplateList.SelectedValue.ToString();

            /////////////////////////////////////////////////////////////////////
            EmailTemplate emailTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));
            
            txtTemplateName.Text = emailTemplate.Title;
            txtTemplateSubject.Text = emailTemplate.Subject;
            rEditorBody.Content = GeneralFunction.CheckPlaceHolders(emailTemplate.Body, true);
            BodyTamplateMail = emailTemplate.Body;
            chkInvitation.Checked = emailTemplate.IsInvitation;
            hdfRounds.Value = emailTemplate.UserData1;
            hdfEmailCategory.Value = emailTemplate.UserData2;
            phPopupEmailReminder.Attributes.Add("class", "ModalPopUpBig");
            divEditTamplate.Visible = true;
            /////////////////////////////////////////////////////////////////////
        }

        hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (phSelectTemplate.Visible)
        {
            //EmailTemplate emailTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));
            //if (emailTemplate.Subject != txtTemplateSubject.Text || !(GeneralFunction.CheckPlaceHolders(rEditorBody.Content, false).ToString().Equals(GeneralFunction.CheckPlaceHolders(emailTemplate.Body, false).ToString())))
            //{
            //    EmailTemplate emtemp = SaveEmailTemplate();
            //    GenerateEmails(emtemp.TemplateId);
            //}
            //else
            //{
            GenerateEmails(new Guid(ddlTemplateList.SelectedValue));
            //}
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "none", "<script>ClearDataOnLoad();</script>", false);
        }
        phSelectTemplate.Visible = false;
    }

    public bool ValidateForm()
    {
        lblError.Text = string.Empty;

        //lblError.Text += IptechLib.Validation.ValidateDropDownList("Default Template", ddlTemplateList, emailTemplate.IsNew, Guid.Empty.ToString());
        lblError.Text += IptechLib.Validation.ValidateTextBox("Template Name", txtTemplateName, true, IptechLib.ValidationType.String);
        lblError.Text += IptechLib.Validation.ValidateTextBox("Subject", txtTemplateSubject, true, IptechLib.ValidationType.String);

        if (String.IsNullOrEmpty(rEditorBody.Text))
            lblError.Text += "Body required.<br/>";

        return String.IsNullOrEmpty(lblError.Text);
    }

    public EmailTemplate SaveEmailTemplate()
    {
        if (ValidateForm())
        {

            EmailTemplate emailTemplate = EmailTemplate.NewEmailTemplate();
            emailTemplate.Subject = txtTemplateSubject.Text;
            if (txtTemplateName.Text.Contains("Update"))
            {
                var Pos = txtTemplateName.Text.LastIndexOf("Update");
                var substring = txtTemplateName.Text.Substring(0, Pos + 6);
                emailTemplate.Title = substring + " " + DateTime.Now.ToString();
            }
            else if (txtTemplateName.Text.Contains("RSVP"))
            {
                var Pos = txtTemplateName.Text.LastIndexOf("RSVP");
                var substring = txtTemplateName.Text.Substring(0, Pos + 4);
                emailTemplate.Title = substring + " " + DateTime.Now.ToString();
            }
            else
                emailTemplate.Title = txtTemplateName.Text + " - " + DateTime.Now.ToString();
            emailTemplate.Body = GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true);
            emailTemplate.IsActive = true;
            emailTemplate.IsInvitation = chkInvitation.Checked;
            emailTemplate.UserData1 = hdfRounds.Value.ToString();
            emailTemplate.UserData2 = hdfEmailCategory.Value.ToString();

            if (emailTemplate.IsNew)
            {
                emailTemplate.DateCreatedString = DateTime.Now.ToString();

                emailTemplate.TemplateId = new Guid(ddlTemplateList.SelectedValue.ToString());
            }

            emailTemplate.DateModifiedString = DateTime.Now.ToString();

            if (emailTemplate.IsValid)
                emailTemplate.Save();
            else
                return null;

            return emailTemplate;
        }
        else
            return null;
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = false;
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
        List<RegEntry> regentrylist = new List<RegEntry>();
        foreach (GridDataItem item in radGridUser.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            if (chkbox.Checked)
            {
                List<Entry> entryList = EntryList.GetEntryList(Guid.Empty, new Guid(item["Id"].Text), "").ToList();
                Registration reg = Registration.GetRegistration(new Guid(item["Id"].Text));
                regentrylist.Add(new RegEntry { entrylist = entryList, registration = reg });
            }
            chkbox.Checked = false;
        }

        if (regentrylist.Count() == 0)
            lblError.Text = "Please select atleast one registration to send email.<br/>";
        else
        {
            foreach (RegEntry regEntry in regentrylist.ToList())
            {
                GeneralFunction.SaveEmailSentLogReg(regEntry.registration, templateId, evetnYear, "UserList", regEntry.registration.Id);
                List<Guid> IDList = new List<Guid>();
                if (regEntry.entrylist.Count() == 0)
                {
                    //GeneralFunction.SaveEmailSentLogReg(regEntry.registration, templateId, evetnYear, "UserList", regEntry.registration.Id);
                }
                else
                {
                    foreach (Entry entry in regEntry.entrylist)
                    {
                        IDList.Add(entry.Id);
                        //GeneralFunction.SaveEmailSentLogReg(regEntry.registration, templateId, evetnYear, "UserList", entry.Id);
                        entry.LastSendSubmissionReminderEmailDateString = DateTime.Now.ToString();
                        entry.Save();
                    }
                }

                Email.SendReminderEmailTemplatelReg(regEntry.registration, GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true), txtTemplateSubject.Text, IDList, "UserList");
                //Email.SendReminderEmailTemplatelReg(regEntry.registration, templateId, IDList, "UserList");
            }

            lblError.Text = "Email sent " + regentrylist.Count() + " .<br/>";
        }

        regentrylist.Clear();

        phSelectTemplate.Visible = false;
        radGridUser.Rebind();
    }

    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "Edit")
        {
            Registration reg = Registration.GetRegistration(new Guid(e.CommandArgument.ToString()));
            Security.SetLoginSessionUser(reg);
            GeneralFunction.SetRedirect("../Admin/UserList.aspx");  // to return from whereever
            Response.Redirect("../User/Profile.aspx?am=e&md=edit&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "View")
        {
            //Security.SetLoginSessionUser(reg);
            GeneralFunction.SetRedirect("../Admin/UserList.aspx");  // to return from whereever
            Response.Redirect("../Admin/Profile.aspx?Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
    }
    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    { 
        BindGrid(false);
    }

    protected void rtabUser_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        Telerik.Web.UI.RadTab TabClicked = e.Tab;
        string tabvalue = TabClicked.Value;

        radGridUser.MasterTableView.SortExpressions.Clear();

        ViewState["TabFilterValue"] = tabvalue;
        ViewState["AdvanceSearch"] = "";
        BindGrid(true);


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rtabUser.Visible = false;

        ViewState["AdvanceSearch"] = "1";
        BindGrid(true);

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlPaymentStatus.SelectedValue = "All";
        ddlEntryStatus.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlIsVerified.SelectedValue = "";

        rtabUser.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        rtabUser.SelectedIndex = 0;
        BindGrid(true);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Registration> flist = (List<Registration>)data;

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "User Master";
            workbook.Worksheets.Add(sheetName);
            x = 1;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Salutation"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Password"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Job"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Contact"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Mobile"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Fax"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Website"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("City"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;

            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CAAAA"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("APEP"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EProg"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EProgCampaign"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IsEmailUpdate"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IsPromotion1"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Status"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IsActive"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateCreated"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateModified"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("LastSignedIn2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Verified"); x++;
            y++;

            foreach (Registration reg in flist)
            {
                x = 1;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Email); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Firstname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Lastname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Password); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Job); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(reg.Contact)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(reg.Mobile)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(reg.Fax)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Website); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Address1); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Address2); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.City); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;

                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ExtractBracketValue(reg.Caaaa)); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ExtractBracketValue(reg.Apep)); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Eprog); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.EProgCampaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.IsEmailUpdate); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.IsPromo1); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetRegistrationStatus(reg.Status)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.IsActive); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(reg.DateCreated, "dd/MM/yy")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(reg.DateModified, "dd/MM/yy")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(reg.LastSignIn2, "dd/MM/yy")); x++;

                string verified = reg.IsVerified ? "Yes" : "No";
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(verified); x++;

                y++;
            }



            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_User_Master.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        bool isSelected = false;
        foreach (GridDataItem item in radGridUser.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            if (chkbox.Enabled && chkbox.Checked)
            {
                Registration reg = Registration.GetRegistration(new Guid(item["Id"].Text));
                reg.IsVerified = chkbox.Checked;
                reg.Save();

                //chkbox.Enabled = false; // disable it
                //chkbox.Checked = false; // unchecked it
                isSelected = true;
            }
        }

        // clear the reg cache
        GeneralFunction.GetAllRegistrationCache(true);

        BindGrid(true);
        if (isSelected)
            lblError.Text = "Verification status updated.";
        else
            lblError.Text = "Select at least 1 user to verify.";
    }
    #endregion

    public class RegEntry
    {
        public List<Entry> entrylist;
        public Registration registration;
    }
}