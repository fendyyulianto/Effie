using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Effie2017.App;
using Telerik.Web.UI;

public partial class Admin_EntryList : PageSecurity_Admin
{
    int counter;
    public static string BodyTamplateMail = "";
    public static int PageSizeDefault = 50;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        up1.Save_Clicked += new EventHandler(PaySave_Clicked);
        up1.Cancel_Clicked += new EventHandler(PayCancel_Clicked);

        Administrator admin = Security.GetAdminLoginSession();
        if (admin.Access == "ST")
            Response.Redirect("./EntryProcessing.aspx");
        if (admin.Access == "AD3")
            Response.Redirect("./JuryList.aspx?r=1");

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

        ViewState["TabFilterValue"] = "";
        BindGrid(false, string.Empty, GridSortOrder.None, true);

        // Readonly Admin

        if (Security.IsReadOnlyAdmin())
        {
            radGridEntry.MasterTableView.GetColumn("SelectALL").Visible = false;
            btnEmailReminder.Visible = !Security.IsReadOnlyAdmin();
        }


        if (!Security.IsRoleSuperAdmin() && !Security.IsRoleSuperAdminFinance())
        {
            btnExport.Visible = false;
            btnExport2.Visible = false;
        }

    }

    private void LoadForm()
    {
        try
        {
            ddlAssignedTo.Items.Add(new ListItem("All", ""));
            AdministratorList administratorList = AdministratorList.GetAdministratorList();
            foreach (Administrator admin in administratorList)
            {
                ddlAssignTo.Items.Add(new ListItem(admin.LoginId, admin.Id.ToString()));
                ddlAssignedTo.Items.Add(new ListItem(admin.LoginId, admin.Id.ToString()));
            }
        }
        catch { }

        if (Security.IsRoleSuperAdmin())
        {
            btnAssignToST.Visible = true;
        }

        // Payment Status
        ddlPaymentStatus.Items.Add(new ListItem("All", ""));
        ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentEntryStatus(StatusPaymentEntry.Paid), StatusPaymentEntry.Paid));
        ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentEntryStatus(StatusPaymentEntry.NotPaid), StatusPaymentEntry.NotPaid));

        // Entry Status
        ddlEntryStatus.Items.Add(new ListItem("All", ""));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadPending), StatusEntry.UploadPending));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadCompleted), StatusEntry.UploadCompleted));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.Completed), StatusEntry.Completed));

        // Category
        DataTable dt1 = Category.GetSubcategories("01");
        DataTable dt2 = Category.GetSubcategories("02");
        DataTable dt3 = Category.GetSubcategories("03");
        DataTable dt4 = Category.GetSubcategories("04");

        ddlCategory.Items.Add(new ListItem("All", ""));

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Single - Products and Services", "01"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt1.Rows)
            ddlCategory.Items.Add("SP-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Single - Specialty", "02"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt2.Rows)
            ddlCategory.Items.Add("SS-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Multi - Products and Services", "03"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt3.Rows)
            ddlCategory.Items.Add("MP-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Multi - Specialty", "04"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt4.Rows)
            ddlCategory.Items.Add("MS-" + dr["Name"].ToString());



        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryList(false);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));


        Security.SecureControlByHiding(btnExport, "EXPORT");
        Security.SecureControlByHiding(btnExport2, "EXPORT");
    }
    private void BindGrid(bool isCustomSort, string sortExpression, GridSortOrder order, bool needRebind)
    {
        EntryList list = GeneralFunction.GetAllEntryCache(needRebind);

        // filter off the draft and ready
        List<Entry> slist = new List<Entry>();
        foreach (Entry entry in list)
        {
            if (entry.Status != StatusEntry.Draft && entry.Status != StatusEntry.Ready)
                slist.Add(entry);
        }

        // filter
        string status = (string)ViewState["TabFilterValue"];
        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Entry> flist = new List<Entry>();

        if (advanceSearch == "1")
        {
            // Advanced search 
            // basic fields
            // user's country
            List<Guid> entryIdList = GeneralFunction.GetFilteredEntryListFromRegCountry(ddlCountry.SelectedValue, true);
            List<Guid> entryIdList2 = GeneralFunction.GetFilteredEntryListFromRegCompany(txtSearch.Text.Trim(), true);

            // Changes by Shaik for adding new columns on 19 Oct 2015
            List<Guid> entryIdList3 = GeneralFunction.GetFilteredEntryListFromRegFirstName(txtSearch.Text.Trim(), true);
            List<Guid> entryIdList4 = GeneralFunction.GetFilteredEntryListFromRegLastName(txtSearch.Text.Trim(), true);

            foreach (Entry item in slist)
            {
                // category strip out the prefix
                string category = ddlCategory.SelectedValue;
                if (category.Length > 3) category = category.Substring(3, category.Length - 3);

                if (
                    (ddlAssignedTo.SelectedValue == "" || (ddlAssignedTo.SelectedValue != "" && item.AdminidAssignedto == (new Guid(ddlAssignedTo.SelectedValue)))) &&
                    (ddlPaymentStatus.SelectedValue == "" || (ddlPaymentStatus.SelectedValue != "" && item.PayStatus == ddlPaymentStatus.SelectedValue)) &&
                    (ddlDeadline.SelectedValue == "" || (ddlDeadline.SelectedValue != "" && item.Deadline == ddlDeadline.SelectedValue)) &&
                    (ddlEntryStatus.SelectedValue == "" || (ddlEntryStatus.SelectedValue != "" && item.Status == ddlEntryStatus.SelectedValue)) &&
                    (ddlMarket.SelectedValue == "" || (ddlMarket.SelectedValue != "" && item.CategoryMarket == ddlMarket.SelectedValue)) &&
                    (category == "" || (category != "" && (item.CategoryPSDetail == category || GeneralFunction.IsCategoryInCategoryGroup(category, item.CategoryPSDetail)))) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&
                    ((txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entryId") && item.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "client") && item.Client.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && entryIdList2.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "firstname") && entryIdList3.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "lastname") && entryIdList4.Contains(item.Id)))
                    ))
                    flist.Add(item);
            }
        }
        else
        {
            // tab filtering
            if (status == "WDN")
            {
                foreach (Entry item in slist)
                    if (item.WithdrawnStatus != "") flist.Add(item);
            }
            else
            {
                foreach (Entry item in slist)
                    if (status == "" || (status != "" && item.Status == status)) flist.Add(item);
            }
        }

        counter = 1;

        #region CustomSorting

        if (isCustomSort)
        {
            List<Registration> regList = RegistrationList.GetRegistrationList(string.Empty, string.Empty, string.Empty).ToList();

            if (sortExpression.Equals("Country"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country select entry).ToList();
                        break;
                }
            }

            else if (sortExpression.Equals("ProcessingStatus"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => (x.ProcessingStatus == StatusEntry.Completed) ? "Completed" : GeneralFunction.GetEntryStatusForAdmin(x.ProcessingStatus)).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => (x.ProcessingStatus == StatusEntry.Completed) ? "Completed" : GeneralFunction.GetEntryStatusForAdmin(x.ProcessingStatus)).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => (x.ProcessingStatus == StatusEntry.Completed) ? "Completed" : GeneralFunction.GetEntryStatusForAdmin(x.ProcessingStatus)).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("DateSubmitted"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Campaign"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Campaign select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Campaign descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Campaign select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Client"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Client select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Client descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Client select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("CategoryMarket"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.CategoryMarket orderby entry.CategoryPSDetail select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.CategoryMarket descending orderby entry.CategoryPSDetail descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.CategoryMarket orderby entry.CategoryPSDetail select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("PayStatus"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.PayStatus select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.PayStatus descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.PayStatus select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Status"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Status).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Status).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Status).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Firstname"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Firstname select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Firstname descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Firstname select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Lastname"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Lastname select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Lastname descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Lastname select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("DateReminder"))
            {
                //x.DateReminder(x.Id, EmailTypeEnum.EntryList_PendingUploads.ToString())
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.LastSendSubmissionReminderEmailDate  ).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.LastSendSubmissionReminderEmailDate).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.LastSendSubmissionReminderEmailDate).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Serial"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Serial).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Serial).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Serial).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Invoice"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Invoice select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Invoice descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Invoice select entry).ToList();
                        break;
                }
            }
        }
        else
            flist = (from entry in flist orderby entry.Invoice descending, entry.DateCreated descending select entry).ToList();

        #endregion

        radGridEntry.DataSource = flist;
        if (needRebind)
            radGridEntry.DataBind();

        GeneralFunction.SetReportDataCache(flist);
    }

    #region Events
    private void PaySave_Clicked(object sender, EventArgs e)
    {
        phPay.Visible = false;

        if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
        {
            foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
            {
                BindGrid(false, expr.FieldName, expr.SortOrder, true);
            }
        }
        else
        {
            BindGrid(false, string.Empty, GridSortOrder.None, true);
        }

        //Response.Redirect("EntryList.aspx"); // if not redirect, this will cause caching and also cause double posting of the payment form if f5 by user
    }
    private void PayCancel_Clicked(object sender, EventArgs e)
    {
        phPay.Visible = false;
    }
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.Entry entry = (Effie2017.App.Entry)e.Item.DataItem;

            LinkButton lnkBtn = null;
            Label lbl = null;
            HyperLink lnk = null;

            // Edit button
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            Security.SecureControlReadOnlyByHiding(lnkBtn);


            HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");
            hdfId.Value = entry.Id.ToString();

            //// No
            //lbl = (Label)e.Item.FindControl("lblNo");
            //lbl.Text = counter.ToString();

            // market
            //if (entry.CategoryMarket == "SM")
            //    ((GridDataItem)e.Item)["CategoryMarket"].Text = "Single Market";
            //else if (entry.CategoryMarket == "MM")
            //    ((GridDataItem)e.Item)["CategoryMarket"].Text = "Multi Market";
            ((GridDataItem)e.Item)["CategoryMarket"].Text = entry.CategoryPSDetail;

            ((GridDataItem)e.Item)["PayStatus"].Text = GeneralFunction.GetPaymentEntryStatus(entry.PayStatus);
            ((GridDataItem)e.Item)["Status"].Text = GeneralFunction.GetEntryStatusForAdmin(entry.Status);
            ((GridDataItem)e.Item)["Deadline"].Text = entry.Deadline;

            // Status
            if (entry.Status == StatusEntry.Completed)
                ((GridDataItem)e.Item)["Status"].Text = "<span style=\"font-weight:bold\">" + GeneralFunction.GetEntryStatus(entry.Status) + "</span>";
            if (entry.WithdrawnStatus != "")
                ((GridDataItem)e.Item)["Status"].Text += "<br/><span style=\"color:Red;\">" + GeneralFunction.GetWithdrawnStatus(entry.WithdrawnStatus) + "</span>";


            Administrator administrator = AdministratorList.GetAdministratorList().Where(x => x.Id == entry.AdminidAssignedto).FirstOrDefault();
            try
            {
                ((GridDataItem)e.Item)["AdminidAssignedto"].Text = administrator.LoginId;
            }
            catch
            {
                ((GridDataItem)e.Item)["AdminidAssignedto"].Text = "";
            }


            // submitted by
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnBuSubmittedBy");
            Registration reg = Registration.GetRegistration(entry.RegistrationId);
            lnkBtn.Text = "-";
            if (reg != null)
            {
                lnkBtn.Text = reg.Company;
                lnkBtn.CommandArgument = reg.Id.ToString();

                // Changes by Shaik for adding new columns on 19 Oct 2015
                ((GridDataItem)e.Item)["Firstname"].Text = reg.Firstname;
                ((GridDataItem)e.Item)["Lastname"].Text = reg.Lastname;
            }

            #region ProcessingStatus
            string ProcessingStatus = entry.ProcessingStatus;
            if (entry.ProcessingStatus == StatusEntry.Completed)
                ProcessingStatus = "Completed";
            else
                ProcessingStatus = GeneralFunction.GetEntryStatusForAdmin(entry.ProcessingStatus);
            ((GridDataItem)e.Item)["ProcessingStatus"].Text = ProcessingStatus;
            #endregion

            // submitted details
            lbl = (Label)e.Item.FindControl("lblSubmittedDetails");
            lbl.Text = reg.Firstname + " " + reg.Lastname + "<br/>" + reg.Job + "<br/>" + GeneralFunction.ShowFriendlyContact(reg.Contact) + "<br/>" + reg.Email;


            // invoice
            lnk = (HyperLink)e.Item.FindControl("lnkInvoice");
            lnk.Text = entry.Invoice;
            lnk.NavigateUrl = "./PaymentPdfView.aspx?id=" + GeneralFunction.StringEncryption(entry.Id.ToString());


            // update payment
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
            lnkBtn.CommandArgument = entry.Id.ToString();
            lnkBtn.Visible = false;
            // hide update payment if PP
            //if (entry.PaymentMethod == PaymentType.PayPal) lnkBtn.Visible = false;
            //Security.SecureControlByHiding(lnkBtn);


            //// set to open
            //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnOpen");
            //lnkBtn.CommandArgument = entry.Id.ToString();
            //if (entry.Status == StatusEntry.Completed) lnkBtn.Visible = true;
            //Security.SecureControlByHiding(lnkBtn);

            // Adhoc Invoice
            lnkBtn = (LinkButton)e.Item.FindControl("lbkAdhocInvoice");
            lnkBtn.CommandArgument = entry.RegistrationId.ToString();
            if (Security.IsReadOnlyAdmin())
            {
                lnkBtn.Visible = false;
            }

            // chkboxes
            //CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            //Security.SecureControlReadOnlyByHiding(chkbox);

            //Edit By Rico, Nov 28 2013, Extra Collumn for Countr and Date Reminder
            ((GridDataItem)e.Item)["Country"].Text = reg.Country;

            string DateReminder = "";
            if (entry.DateReminder(entry.Id, EmailTypeEnum.EntryList_PendingUploads.ToString()) != DateTime.MinValue)
                 DateReminder = entry.DateReminder(entry.Id, EmailTypeEnum.EntryList_PendingUploads.ToString()).ToString("dd/MM/yy H:mm");

            ((GridDataItem)e.Item)["DateReminder"].Text = (entry.LastSendSubmissionReminderEmailDate == DateTime.MaxValue) ? "" : entry.LastSendSubmissionReminderEmailDate.ToString("dd/MM/yy H:mm");

            //switch ((string)ViewState["TabFilterValue"])
            //{
            //    case StatusEntry.UploadPending:
            //        ((GridDataItem)e.Item)["DateReminder"].Text = GeneralFunction.CleanDateTimeToString(entry.LastSendUploadReminderEmailDate, "MM/dd/yy hh:mm:ss tt");
            //        break;
            //    case StatusEntry.UploadCompleted:
            //        ((GridDataItem)e.Item)["DateReminder"].Text = GeneralFunction.CleanDateTimeToString(entry.LastSendCompletionReminderEmailDate, "MM/dd/yy hh:mm:ss tt");
            //        break;
            //    default:
            //        ((GridDataItem)e.Item)["DateReminder"].Text = "";
            //        break;
            //}
            //END Edit By Rico
            //string status = (string)ViewState["TabFilterValue"];
            //if (status == StatusEntry.UploadPending || status == StatusEntry.UploadCompleted)
            //{
            //    CheckBox checkbox = (CheckBox)e.Item.FindControl("chkbox");
            //    if (checkbox != null) checkbox.Visible = true;
            //    Security.SecureControlByHiding(checkbox);

            //}
            counter++;


            lnk = (HyperLink)e.Item.FindControl("hlkEmailHistory");
            lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
            lnk.NavigateUrl = "./RegistrationEmailSentHistory.aspx?regId=" + reg.Id.ToString() + "&EntryId=" + entry.Id.ToString();
        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            PageSizeCombo.Items.Clear();
            //PageSizeCombo.Items.Add(new RadComboBoxItem("20", "20"));
            //PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            string PageSize = e.Item.OwnerTableView.PageSize.ToString();
            PageSizeCombo.FindItemByValue(PageSizeDefault.ToString()).Selected = true;
        }
    }
    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "Edit")
        {
            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Main/Entry.aspx?db=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Open")
        {
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(new Guid(e.CommandArgument.ToString()));
            if (entry.Status == StatusEntry.Completed)
            {
                entry.Status = StatusEntry.UploadCompleted;
                entry.Save();
            }
            BindGrid(false, string.Empty, GridSortOrder.None, true);
        }
        else if (e.CommandName == "View")
        {
            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Main/Entry.aspx?db=1&v=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Withdraw")
        {
        }
        else if (e.CommandName == "Payment")
        {
            phPay.Visible = true;
            up1.EntryId = new Guid(e.CommandArgument.ToString());
            up1.PopulateForm();
        }
        else if (e.CommandName == "User")
        {
            Registration reg = Registration.GetRegistration(new Guid(e.CommandArgument.ToString()));
            Security.SetLoginSessionUser(reg);
            GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Admin/Profile.aspx?Id=" + reg.Id.ToString());
        }
        else if (e.CommandName == "adhoc")
        {
            GeneralFunction.SetRedirect("../Admin/AdhocInvoiceList.aspx");  // to return from whereever
            Response.Redirect("../Admin/AdhocInvoice.aspx?regId=" + GeneralFunction.StringEncryption(e.CommandArgument.ToString()));
        }
        //else if (e.CommandName == RadGrid.SortCommandName)
        //{
        //    var sortCommandArgs = (GridSortCommandEventArgs)e;
        //    if (sortCommandArgs.SortExpression == "Firstname" || sortCommandArgs.SortExpression == "Lastname")
        //    {
        //        radGridEntry.MasterTableView.AllowCustomSorting = true;
        //    }
        //}
    }
    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
        {
            foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
            {
                BindGrid(true, expr.FieldName, expr.SortOrder, false);
            }
        }
        else
        {
            BindGrid(false, string.Empty, GridSortOrder.None, false);
        }
    }

    protected void radGridEntry_SortCommand(object Sender, GridSortCommandEventArgs e)
    {
        //if (e.CommandArgument == "Country")  //This Line Made ALL sorting functions of the title/labels are NOT working
        {
            BindGrid(true, e.CommandArgument.ToString(), e.NewSortOrder, true);
        }
    }

    protected void btnEmailReminder_Click(object sender, EventArgs e)
    {
        //List<Guid> paygroupIdList = new List<Guid>();

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

                foreach (GridDataItem item in radGridEntry.MasterTableView.Items)
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

        //bool isSelected = false;
        //foreach (GridDataItem item in radGridEntry.Items)
        //{
        //    CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
        //    if (chkbox.Checked)
        //    {
        //        Entry entry = Entry.GetEntry(new Guid(item["Id"].Text));
        //        Registration reg = Registration.GetRegistration(entry.RegistrationId);
        //        Email.SendReminderPendingUploadCompletionEmail(reg, entry);
        //        isSelected = true;

        //        // save the last sent date
        //        if (((Button)sender).CommandName == "upload") entry.LastSendUploadReminderEmailDateString = DateTime.Now.ToString();
        //        if (((Button)sender).CommandName == "completion") entry.LastSendCompletionReminderEmailDateString = DateTime.Now.ToString();
        //        entry.Save();
        //    }
        //    chkbox.Checked = false; // unchecked it
        //}

        //if (!isSelected)
        //    lblError.Text = "Select at least 1 entry to send email.";
        //else
        //{
        //    lblError.Text = "Email sent.";
        //    BindGrid(false, string.Empty, GridSortOrder.None, true);
        //}

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
    protected void rtabEntry_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        Telerik.Web.UI.RadTab TabClicked = e.Tab;
        string tabvalue = TabClicked.Value;
        radGridEntry.MasterTableView.SortExpressions.Clear();
        ViewState["TabFilterValue"] = tabvalue;
        ViewState["AdvanceSearch"] = "";
        lblError.Text = "";
        BindGrid(false, string.Empty, GridSortOrder.None, true);

        // Display the reminder buttons conditionally
        if (tabvalue == "")
        {
            btnEmailReminder.Visible = true;
        }
        else
        {
            btnEmailReminder.Visible = false;
        }


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rtabEntry.Visible = false;
        //btnEmailReminder.Visible = false;
        lblError.Text = "";
        
        // preselect the market if the category was selected
        string cat = ddlCategory.SelectedValue;
        if (cat.Length > 3)
        {
            // First 2 chars is SP / SS / MP / MS
            // convert to SM / MM
            if (cat.Substring(0, 1) == "S") ddlMarket.SelectedValue = "SM";
            if (cat.Substring(0, 1) == "M") ddlMarket.SelectedValue = "MM";
        }
        if (cat.Length == 2)
        {
            // also for selection of category grouping
            if (cat == "01" || cat == "02") ddlMarket.SelectedValue = "SM";
            if (cat == "03" || cat == "04") ddlMarket.SelectedValue = "MM";
        }

        ViewState["AdvanceSearch"] = "1";
        BindGrid(false, string.Empty, GridSortOrder.None, true);

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlPaymentStatus.SelectedValue = "";
        ddlEntryStatus.SelectedValue = "";
        ddlCategory.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlMarket.SelectedValue = "";
        lblError.Text = "";

        rtabEntry.Visible = true;
        //btnEmailReminder.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        rtabEntry.SelectedIndex = 0;
        BindGrid(false, string.Empty, GridSortOrder.None, true);
    }
    //Edit By Rico, Nov 28 2013, to split full report and summary report
    protected void btnExport_Click(object sender, EventArgs e)
    {
        GenerateReport(1);
    }
    protected void btnExport2_Click(object sender, EventArgs e)
    {
        GenerateReport(2);
    }
    //END Edit By Rico



    protected void btnReport_Click(object sender, EventArgs e)
    {
        object data = GeneralFunction.GetReportDataCache();
        if (data != null)
        {
            List<Entry> flist = (List<Entry>)data;

            int x = 1;
            int y = 1;
            string sheetName = "Entry Processing Report Final";
            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            workbook.Worksheets.Add(sheetName);

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DL"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Sal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Job"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("City"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category Market"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category PS"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category PSDetail"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Withdrawn Status"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Submitted"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assign"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Status"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Payment Status"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Processing Status"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Flag"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Flag Reason(s)"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Flag Description"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Notify Date"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reopened By"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reopening Fee"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reason for Fee Waiver"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Reopen"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reopening Deadline"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Remarks"); x++;

            y++;
            {
                foreach (Entry entry in flist)
                {
                    PopulateExcelRow(ref workbook, ref y, ref x, ref sheetName, entry);
                }
            }

            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Assignment Report.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }

    }


    protected XLWorkbook PopulateExcelRow(ref XLWorkbook workbook, ref int y, ref int x, ref string sheetName, Entry entry)
    {
        x = 1;
        string TempDate = "";
        Registration reg = Registration.GetRegistration(entry.RegistrationId);
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Deadline); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Email); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Salutation); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Firstname); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Lastname); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Job); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.PayCity); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Serial); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Campaign); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Client); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Brand); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Data.GetCategoryMarket(entry.CategoryMarket)); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Data.GetCategoryPS(entry.CategoryPS)); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.CategoryPSDetail); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.WithdrawnStatus); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.DateSubmitted.ToString("dd MMM yyyy")); x++;

        Administrator Assign = null;
        try
        {
            Assign = AdministratorList.GetAdministratorList().Where(h => h.Id == entry.AdminidAssignedto).FirstOrDefault();
        }
        catch { }

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Assign != null ? Assign.LoginId : ""); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryStatusForAdmin(entry.Status)); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetPaymentEntryStatus(entry.PayStatus)); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryStatusForAdmin(entry.ProcessingStatus)); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.DQFlag); x++;

        string SelectedFlagReason = GeneralFunction.GetSelectedFlagReasons(entry.FlagReason, true).Replace("||", ", ");
        if (!string.IsNullOrEmpty(SelectedFlagReason))
        {
            SelectedFlagReason = SelectedFlagReason.Substring(0, SelectedFlagReason.Length - 2) + ".";
        }

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(SelectedFlagReason); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.FlagDQDescription); x++;

        try { TempDate = Convert.ToDateTime(entry.NotificationSentDate).ToString("dd MMM yyyy"); } catch { TempDate = ""; }
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(TempDate); x++;

        Administrator ReopenedBy = null;
        try
        {
            ReopenedBy = AdministratorList.GetAdministratorList().FirstOrDefault(h => h.Id == new Guid(string.IsNullOrEmpty(entry.ReopenedBy) ? Guid.Empty.ToString() : entry.ReopenedBy));
        }
        catch { }
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ReopenedBy != null ? ReopenedBy.LoginId : ""); x++;
        string ReopeningFee = "";
        try
        {
            AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(new Guid(entry.IDAdhocInvoice));
            ReopeningFee = adInv.Amount.ToString("0.00");
        }
        catch
        {
            ReopeningFee = "0.00";
        }
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ReopeningFee); x++; 
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.ReasonFeeWaiver); x++; //TODO

        try { TempDate = Convert.ToDateTime(entry.ReopeningDate).ToString("dd MMM yyyy"); } catch { TempDate = ""; }
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(TempDate); x++;

        //try { TempDate = Convert.ToDateTime(entry.ReopeningDeadline).ToString("dd MMM yyyy"); } catch { TempDate = ""; }
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(TempDate); x++;

        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.OtherRemarks); x++;
        y++;
        return workbook;
    }
    #endregion

    #region Helper
    private ListItem SeparatorItem()
    {
        ListItem separator = new ListItem("-------------------------------", "");
        separator.Attributes.Add("disabled", "disabled");

        return separator;
    }

    //Edit By Rico, Nov 28 2013, to split full report and summary report
    protected void GenerateReport(int reportType) //1 = Full, 2 = Summary
    {
        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Entry> flist = (List<Entry>)data;

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Entry Master";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DL"); x++;

            // Registration details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Salutation"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Password"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Job"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Contact"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Mobile"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Fax"); x++;

            if (reportType == 1)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Website"); x++;
            }

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;

            if (reportType == 1)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address1"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address2"); x++;
            }

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Postal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("City"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CAAAA"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("APEP"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EProg"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EProgCampaign"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Promotion1"); x++;

            if (reportType == 1)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IsEmailUpdate"); x++;
            }

            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Status"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IsActive"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateCreated"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateModified"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("LastSignedIn2"); x++;



            // Entry details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CategoryMarket"); x++;


            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CategoryPS"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CategoryPSDetail"); x++;

            if (reportType == 1)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateCampaignStart"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateCampaignEnd"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Effectiveness"); x++;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RepSalutation"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RepFirstname"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RepLastname"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RepJob"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RepCompany"); x++;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RepContact"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RepMobile"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RepEmail"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Summary"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ProductClassification"); x++;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ProductClassificationOthers"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryObjective"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryObjectiveOthers"); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("OverachingCaseObjective"); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TargetAudience"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TargetAudienceOthers"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TargetAudiencePrimary"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TargetAudiencePrimaryOthers"); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HeroTouchPoint"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HeroTouchPointOthers"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HeroTouchPoint2"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HeroTouchPointOthers2"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HeroTouchPoint3"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HeroTouchPointOthers3"); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("SocialPlatforms"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("SocialPlatformsOthers"); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Research"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ResearchImportant"); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CountriesRanIn"); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("GoalAlignedCaseSDGs"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TagCaseSDGs"); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Permission"); x++;
            }



            // Payment Details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Amount"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Fee"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AmountReceived"); x++;

            if (reportType == 1)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayGroupId"); x++;
            }

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PaymentMethod"); x++;


            if (reportType == 1)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayCompany"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayAddress1"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayAddress2"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayCity"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayPostal"); x++;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayCountry"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayFirstname"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayLastname"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayContact"); x++;
            }

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Invoice"); x++;




            // Misc Details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryStatus"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayStatus"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("WithdrawnStatus"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateSubmitted"); x++;
            ////////////////////////
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assign"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Reminder"); x++;
            ////////////////////////
            if (reportType == 1)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateCreated"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateModified"); x++;
            }

            #endregion


            #region Company Credit Headers

            for (int i = 1; i <= 8; i++)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-ContactType"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Company"); x++;

                if (reportType == 1)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Address1"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Address2"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-City"); x++;


                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Postal"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Country"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Salutation"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Fullname"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Job"); x++;


                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Contact"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Email"); x++;
                }

                // absent for non client types - not the 1st cc
                if (i == 1 || i == 2)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-ClientCompanyNetwork"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-ClientCompanyNetworkOthers"); x++;
                }
                else
                {
                    if (reportType == 1)
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-CompanyType"); x++;
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-CompanyTypeOthers"); x++;
                    }

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-Network"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-NetworkOthers"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-HoldingCompany"); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(i.ToString() + "-HoldingCompanyOthers"); x++;
                }


            }

            #endregion


            if (reportType == 1)
            {
                #region Individual Credit Headers

                for (int i = 1; i <= 10; i++)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ContactName" + i.ToString()); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title" + i.ToString()); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email" + i.ToString()); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company" + i.ToString()); x++;
                }

                #endregion
            }


            if (reportType == 1)
            {
                #region Materials Headers

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryFormPDF"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryFormWord"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AuthorizationForm"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CaseImage"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CreativeMaterials1"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CreativeMaterials2"); x++;


                #endregion
            }


            y++;

            foreach (Entry ent in flist)
            {
                Registration reg = GeneralFunction.GetRegistrationFromEntry(ent);

                x = 1;


                #region Basic Entry DataRows

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Deadline); x++;

                // Registration Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Email); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Firstname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Lastname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Password); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Job); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(reg.Contact)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(reg.Mobile)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(reg.Fax)); x++;

                if (reportType == 1)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Website); x++;
                }

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;

                if (reportType == 1)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Address1); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Address2); x++;
                }

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Postal); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.City); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ExtractBracketValue(reg.Caaaa)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ExtractBracketValue(reg.Apep)); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Eprog); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.EProgCampaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.IsPromo1); x++;

                if (reportType == 1)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.IsEmailUpdate); x++;
                }

                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetRegistrationStatus(reg.Status)); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.IsActive); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.DateCreatedString); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.DateModifiedString); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.LastSignIn2String); x++;




                // Entry Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Campaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Client); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Brand); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryMarket); x++;


                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryPS); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryPSDetail); x++;

                if (reportType == 1)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(ent.DateCampaignStart, "dd/MM/yy")); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(ent.DateCampaignEnd, "dd/MM/yy")); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Effectiveness); x++;


                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.RepSalutation); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.RepFirstname); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.RepLastname); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.RepJob); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.RepCompany); x++;


                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(ent.RepContact)); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(ent.RepMobile)); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.RepEmail); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Summary); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.ProductClassification); x++;


                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.ProductClassificationOthers); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.EntryObjective); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.EntryObjectiveOthers); x++;

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.EntryObjective2); x++;

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.TargetAudience); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.TargetAudienceOthers); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.TargetAudiencePri); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.TargetAudiencePriOthers); x++;

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.HeroTouchPoint); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.HeroTouchPointOthers); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.HeroTouchPoint2); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.HeroTouchPointOthers2); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.HeroTouchPoint3); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.HeroTouchPointOthers3); x++;

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.SocialPlatforms); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.SocialPlatformsOthers); x++;

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Research); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.ResearchImp); x++;

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CaseData); x++;

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.SDGData1); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.SDGData2); x++;

                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Permission); x++;
                }





                // Payment Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Amount.ToString("N")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Fee.ToString("N")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.AmountReceived.ToString("N")); x++;

                if (reportType == 1)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayGroupId); x++;
                }

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetPaymentType(ent.PaymentMethod)); x++;


                if (reportType == 1)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayCompany); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayAddress1); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayAddress2); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayCity); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayPostal); x++;


                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayCountry); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayFirstname); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayLastname); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(ent.PayContact)); x++;
                }

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Invoice); x++;




                // Misc Details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryStatusForAdmin(ent.Status)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetPaymentEntryStatus(ent.PayStatus)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetWithdrawnStatus(ent.WithdrawnStatus)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(ent.DateSubmitted, "dd/MM/yy")); x++;
                
                ////////////////////////////////////////////
                Administrator Assign = null;
                try
                {
                    Assign = AdministratorList.GetAdministratorList().Where(h => h.Id == ent.AdminidAssignedto).FirstOrDefault();
                }
                catch { }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Assign != null ? Assign.LoginId : ""); x++;

                string DateReminder = "";
                if (ent.DateReminder(ent.Id, EmailTypeEnum.EntryList_PendingUploads.ToString()) != DateTime.MinValue)
                    DateReminder = ent.DateReminder(ent.Id, EmailTypeEnum.EntryList_PendingUploads.ToString()).ToString("dd/MM/yy H:mm");

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.LastSendSubmissionReminderEmailDate.ToString("dd/MM/yy H:mm")); x++;
                ////////////////////////////////////////////

                if (reportType == 1)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(ent.DateCreated, "dd/MM/yy")); x++;
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(ent.DateModified, "dd/MM/yy")); x++;
                }

                #endregion



                #region Company Credit DataRows

                CompanyCreditList ccList = CompanyCreditList.GetCompanyCreditList(ent.Id);

                bool isSkipped = false;

                for (int i = 1; i <= 8; i++)
                {
                    CompanyCredit cc = null;
                    try
                    {
                        cc = ccList[i - 1];
                    }
                    catch { }

                    if (cc != null)
                    {
                        if ((i == 1) || (i == 2) && !isSkipped)
                        {
                            if (cc.ContactType.IndexOf("Client") != -1)
                            {
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.ContactType); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Company); x++;

                                if (reportType == 1)
                                {
                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Address1); x++;
                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Address2); x++;
                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.City); x++;


                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Postal); x++;
                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Country); x++;
                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Salutation); x++;
                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Fullname); x++;
                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Job); x++;


                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(cc.Contact)); x++;
                                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Email); x++;
                                }

                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.ClientCompanyNetwork); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.ClientCompanyNetworkOthers); x++;
                            }
                            else
                            {
                                if (reportType == 1)
                                {
                                    x = x + 14;
                                }
                                else
                                {
                                    x = x + 4;
                                }

                                if (i == 2)
                                {
                                    isSkipped = true;
                                    i--;
                                }
                            }
                        }
                        else
                        {
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.ContactType); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Company); x++;

                            if (reportType == 1)
                            {
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Address1); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Address2); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.City); x++;


                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Postal); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Country); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Salutation); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Fullname); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Job); x++;


                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(cc.Contact)); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Email); x++;

                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.CompanyType); x++;
                                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.CompanyTypeOther); x++;
                            }

                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.Network); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.NetworkOthers); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.HoldingCompany); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(cc.HoldingCompanyOthers); x++;
                        }
                    }
                    else
                    {
                        //x = x + 18; // move forward number of columns, assuming we wont hit error on i=1 which has lesser columns

                        if (reportType == 1)
                        {
                            x = x + 18;
                        }
                        else
                        {
                            x = x + 6;
                        }
                    }
                }

                if (isSkipped)
                    x = x - 18;

                #endregion


                if (reportType == 1)
                {
                    #region Individual Credit DataRows

                    IndCreditList icList = IndCreditList.GetIndCreditList(ent.Id);
                    for (int i = 1; i <= 10; i++)
                    {
                        IndCredit ic = null;
                        try
                        {
                            ic = icList[i - 1];
                        }
                        catch
                        { }

                        if (ic != null)
                        {
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ic.ContactName); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ic.Title); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ic.Email); x++;
                            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ic.Company); x++;
                        }
                        else
                            x = x + 4;


                    }

                    #endregion
                }


                if (reportType == 1)
                {
                    #region Materials DataRows


                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + ent.Id.ToString() + "\\" + ent.Serial + "_EntryForm_PDF.pdf"))
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_EntryForm_PDF.pdf");
                    x++;

                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + ent.Id.ToString() + "\\" + ent.Serial + "_EntryForm_Word.doc"))
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_EntryForm_Word.doc");
                    x++;

                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + ent.Id.ToString() + "\\" + ent.Serial + "_AuthorizationForm_PDF.pdf"))
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_AuthorizationForm_PDF.pdf");
                    x++;

                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + ent.Id.ToString() + "\\" + ent.Serial + "_CaseImage.jpg") ||
                        File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + ent.Id.ToString() + "\\" + ent.Serial + "_CaseImage.jpeg"))
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CaseImage.jpg");
                    x++;

                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + ent.Id.ToString() + "\\" + ent.Serial + "_CreativeMaterials_PDF.pdf"))
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CreativeMaterials_PDF.pdf");
                    x++;


                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + ent.Serial + "_CreativeMaterials_Video.mp4"))
                    {
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CreativeMaterials_Video.mp4");
                    }
                    x++;

                    //if (GeneralFunction.FileExistsInAmazonS3(System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Original"], ent.Serial + "_CreativeMaterials_Video.mp4"))
                    //{
                    //    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CreativeMaterials_Video.mp4");                        
                    //}
                    //x++;

                    #endregion
                }



                y++;
            }



            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            if (reportType == 1)
                Response.AddHeader("content-disposition", "attachment;filename=Effie_Entry_Master.xlsx");
            else if (reportType == 2)
                Response.AddHeader("content-disposition", "attachment;filename=Effie_Entry_Summary.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }
    //END Edit By Rico
    #endregion
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
            //    EmailTemplate emtemp = SaveForm();
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
    

    protected void btnClose_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = false;
        phAssignTo.Visible = false;
    }


    protected bool isAnySelected()
    {
        bool isAny = false;
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            if (chkbox.Checked)
            {
                isAny = true;
            }
        }

        return isAny;
    }

    protected void btnAssignToST_Click(object sender, EventArgs e)
    {
        if (isAnySelected())
        {
            phAssignTo.Visible = true;
            lblError.Text = "";
            //lblError2.Text = "";
        }
        else
        {
            lblError.Text = "Please select at least one Entry to Update Status.<br/>";
            //lblError2.Text = lblError.Text;
        }
    }


    protected void btnSubmitAssignTo_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
            Guid EntryId = new Guid(hdfId.Value);
            if (chkbox.Checked)
            {
                Entry entry = Entry.GetEntry(EntryId);
                Registration reg = Registration.GetRegistration(entry.RegistrationId);
                entry.AdminidAssignedto = new Guid(ddlAssignTo.SelectedValue);
                entry.Save();
                chkbox.Checked = false;
                phAssignTo.Visible = false;
                BindGrid(false, string.Empty, GridSortOrder.None, true);
            }
        }
        //PopulateForm(ViewState["TabFilterValue"].ToString());
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
        int counter2 = 0;
        List<Guid> regIdList = new List<Guid>();
        List<Guid> EntryList = new List<Guid>();
        Registration reg2 = null;
        Entry entry = null;
        Registration reg = null;
        List<RegEntry> oRegentry = new List<RegEntry>();
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            if (chkbox.Checked)
            {
                entry = Entry.GetEntry(new Guid(item["Id"].Text));
                reg = Registration.GetRegistration(entry.RegistrationId);
                oRegentry.Add(new RegEntry(new Guid(item["Id"].Text), reg.Id));
            }
            chkbox.Checked = false;
        }
        oRegentry = oRegentry.OrderBy(x => x.regid).ToList();
        foreach (RegEntry RegEntryItem in oRegentry)
        {
            entry = Entry.GetEntry(RegEntryItem.entryid);
            reg = Registration.GetRegistration(RegEntryItem.regid);
            if (!regIdList.Contains(reg.Id) && counter2 != 0)
            {
                Email.SendReminderEmailTemplatelReg(reg2, GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true), txtTemplateSubject.Text, EntryList, "EntryList");
                foreach (Guid entrylistItem in EntryList)
                {
                    GeneralFunction.SaveEmailSentLogReg(reg2, templateId, evetnYear, "EntryList_PendingUploads", entrylistItem);
                    Entry entry2 = Entry.GetEntry(entrylistItem);
                    entry2.LastSendSubmissionReminderEmailDateString = DateTime.Now.ToString();
                    entry2.Save();
                }
                EntryList.Clear();
                counter++;
            }
            if (counter2 == oRegentry.Count - 1)
            {
                EntryList.Add(entry.Id);
                Email.SendReminderEmailTemplatelReg(reg, GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true), txtTemplateSubject.Text, EntryList, "EntryList");
                foreach (Guid entrylistItem in EntryList)
                {
                    GeneralFunction.SaveEmailSentLogReg(reg, templateId, evetnYear, "EntryList_PendingUploads", entrylistItem);
                    Entry entry2 = Entry.GetEntry(entrylistItem);
                    entry2.LastSendSubmissionReminderEmailDateString = DateTime.Now.ToString();
                    entry2.Save();
                }
                EntryList.Clear();
                counter++;
            }

            regIdList.Add(reg.Id);
            EntryList.Add(entry.Id);
            reg2 = reg;
            counter2++;
        }

        oRegentry.Clear();

        if (counter == 0)
            lblError.Text = "Please select atleast one registration to send email.<br/>";
        else
        {
            lblError.Text = "Email sent " + (counter).ToString() + " .<br/>";
        }
        phSelectTemplate.Visible = false;
        PopulateForm();
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

    public EmailTemplate SaveForm()
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
    public class RegEntry
    {
        public Guid entryid;
        public Guid regid;
        public RegEntry(Guid entryid, Guid regid)
        {
            this.entryid = entryid;
            this.regid = regid;
        }

    }
}