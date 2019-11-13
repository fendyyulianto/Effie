using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Effie2017.App;
using Telerik.Web.UI;
using ClosedXML.Excel;
using System.IO;
public partial class Admin_InvoiceList : PageSecurity_Admin
{
    int counter;
    RegistrationList regList = RegistrationList.GetRegistrationList(string.Empty, string.Empty, string.Empty);
    public static string BodyTamplateMail = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        up1.Save_Clicked += new EventHandler(PaySave_Clicked);
        up1.Cancel_Clicked += new EventHandler(PayCancel_Clicked);


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

        if (GeneralFunction.GetFilterPageId() == "InvoiceList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            ddlPaymentStatus.SelectedValue = GeneralFunction.GetFilterF3();
            ddlCountry.SelectedValue = GeneralFunction.GetFilterF4();
            ViewState["AdvanceSearch"] = "1";
        }
        ViewState["TabFilterValue"] = "";

        BindGrid(true,string.Empty,string.Empty);

        // Readonly Admin
        if (Security.IsReadOnlyAdmin())
        {
            radGridEntry.MasterTableView.GetColumn("SelectALL").Visible = false;
            btnEmailReminder.Visible = false;
        }
    }

    private void LoadForm()
    {
        // Payment Status
        ddlPaymentStatus.Items.Add(new ListItem("All", ""));
        ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentEntryStatus(StatusPaymentEntry.Paid), StatusPaymentEntry.Paid));
        ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentEntryStatus(StatusPaymentEntry.NotPaid), StatusPaymentEntry.NotPaid));

        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryList(false);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));


        Security.SecureControlByHiding(btnExport, "EXPORT");

        try
        {
            ddlAssignedTo.Items.Add(new ListItem("All", ""));
            AdministratorList administratorList = AdministratorList.GetAdministratorList();
            foreach (Administrator admin in administratorList)
            {
                //ddlAssignTo.Items.Add(new ListItem(admin.LoginId, admin.Id.ToString()));
                ddlAssignedTo.Items.Add(new ListItem(admin.LoginId, admin.Id.ToString()));
            }
        }
        catch { }
    }

    private void BindGrid(bool needRebind, string sortExpression, string sortOrder)
    {
        EntryList list = GeneralFunction.GetAllEntryCache(false);

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


            List<Guid> entryIdList = GeneralFunction.GetFilteredEntryListFromRegCountry(ddlCountry.SelectedValue, true);
            List<Guid> entryIdList2 = GeneralFunction.GetFilteredEntryListFromRegCompany(txtSearch.Text.Trim(), true);

            // Changes by Shaik for adding new columns on 19 Oct 2015
            List<Guid> entryIdList3 = GeneralFunction.GetFilteredEntryListFromRegFirstName(txtSearch.Text.Trim(), true);
            List<Guid> entryIdList4 = GeneralFunction.GetFilteredEntryListFromRegLastName(txtSearch.Text.Trim(), true);

            foreach (Entry item in slist)
            {
                if (
                     (ddlAssignedTo.SelectedValue == "" || (ddlAssignedTo.SelectedValue != "" && item.AdminidAssignedto == (new Guid(ddlAssignedTo.SelectedValue)))) &&
                    (ddlDeadline.SelectedValue == "" || (ddlDeadline.SelectedValue != "" && item.Deadline == ddlDeadline.SelectedValue)) &&
                    (ddlPaymentStatus.SelectedValue == "" || (ddlPaymentStatus.SelectedValue != "" && item.PayStatus == ddlPaymentStatus.SelectedValue)) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&
                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "invoiceno") && item.Invoice.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && entryIdList2.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "firstname") && entryIdList3.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "lastname") && entryIdList4.Contains(item.Id)))
                    )
                   )
                    flist.Add(item);
            }
        }
        else
        {
            // tab filtering
            foreach (Entry item in slist)
                if (status == "" || (status != "" && item.PayStatus == status)) flist.Add(item);
        }

        // group by same paygroupid
        // first sort by invoice number
        flist.Sort((x, y) => string.Compare(x.Invoice, y.Invoice));

        // second group
        List<Entry> glist = new List<Entry>();
        string invoice = "";
        decimal amount = 0;
        decimal fees = 0;
        decimal tax = 0;
        decimal grandamount = 0;
        decimal amountreceived = 0;
        Entry newinv = null;
        foreach (Entry entry in flist)
        {
            if (entry.Invoice != invoice)
            {
                // change in invoice so add to list
                // we use the entry object as a pseudo invoice object because it contains almost the same properties

                if (newinv != null)
                {
                    newinv.Amount = amount;
                    newinv.Fee = fees;
                    newinv.Tax = tax;
                    newinv.GrandAmount = grandamount;
                    newinv.AmountReceived = amountreceived;
                    glist.Add(newinv);
                    newinv = null;
                }


                amount = 0;
                fees = 0;
                tax = 0;
                grandamount = 0;
                amountreceived = 0;

                Entry inv = Entry.NewEntry();
                inv.Id = entry.Id;
                inv.DateSubmittedString = entry.DateSubmitted.ToString();
                inv.Invoice = entry.Invoice;
                inv.RegistrationId = entry.RegistrationId;
                inv.Amount = inv.Amount;
                inv.Fee = entry.Fee;
                inv.AmountReceived = entry.AmountReceived;
                inv.Status = entry.Status;
                inv.PayStatus = entry.PayStatus;
                inv.PayGroupId = entry.PayGroupId;
                inv.PaymentMethod = entry.PaymentMethod;
                
                
                // payment contact info
                inv.PayFirstname = entry.PayFirstname;
                inv.PayLastname = entry.PayLastname;
                inv.PayContact = entry.PayContact;
                inv.PayCompany = entry.PayCompany;
                inv.PayAddress1 = entry.PayAddress1;
                inv.PayAddress2 = entry.PayAddress2;
                inv.PayCity = entry.PayCity;
                inv.PayPostal = entry.PayPostal;
                inv.PayCountry = entry.PayCountry;
                inv.LastSendPaymentReminderEmailDateString = GeneralFunction.CleanDateTimeToString(entry.LastSendPaymentReminderEmailDate, "MM/dd/yyyy");


                amount = entry.Amount;
                fees = entry.Fee;
                tax = entry.Tax;
                grandamount = entry.GrandAmount;
                amountreceived = entry.AmountReceived;

                newinv = inv;
                
            }
            else
            {
                // no change in invoice, so sum the amounts, fees and amountreceived
                amount += entry.Amount;
                fees += entry.Fee;
                tax += entry.Tax;
                grandamount += entry.GrandAmount;
                amountreceived += entry.AmountReceived;
            }
            invoice = entry.Invoice;
        }

        // clear the last entry
        if (newinv != null)
        {
            newinv.Amount = amount;
            newinv.Fee = fees;
            newinv.Tax = tax;
            newinv.GrandAmount = grandamount;
            newinv.AmountReceived = amountreceived;
            glist.Add(newinv);
            newinv = null;
        }



        counter = 1;
        List<Entry> sortedList = glist.OrderByDescending(p => p.DateSubmitted).ThenByDescending(p => p.Invoice).ToList();

        #region CustomSort
        if (!String.IsNullOrEmpty(sortExpression))
        {
            SetSortExpression(sortExpression);
            SetSortOrder(sortOrder);

            if (sortExpression.Equals("Firstname"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from filteredentry in sortedList
                                     join reg in regList
                                     on filteredentry.RegistrationId equals reg.Id
                                     orderby reg.Firstname ascending
                                     select filteredentry;

                    sortedList = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from filteredentry in sortedList
                                     join reg in regList
                                     on filteredentry.RegistrationId equals reg.Id
                                     orderby reg.Firstname descending
                                     select filteredentry;

                    sortedList = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("Entrytatus"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(x => x.Status).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(x => x.Status).ToList();
                }
            }
            else if (sortExpression.Equals("Lastname"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from filteredentry in sortedList
                                     join reg in regList
                                     on filteredentry.RegistrationId equals reg.Id
                                     orderby reg.Lastname ascending
                                     select filteredentry;

                    sortedList = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from filteredentry in sortedList
                                     join reg in regList
                                     on filteredentry.RegistrationId equals reg.Id
                                     orderby reg.Lastname descending
                                     select filteredentry;

                    sortedList = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("DateSubmitted"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    sortedList = sortedList.OrderBy(m => m.DateSubmitted).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    sortedList = sortedList.OrderByDescending(m => m.DateSubmitted).ToList();
                }
            }
            else if (sortExpression.Equals("Invoice"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    sortedList = sortedList.OrderBy(m => m.Invoice).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    sortedList = sortedList.OrderByDescending(m => m.Invoice).ToList();
                }
            }
            else if (sortExpression.Equals("GrandAmount"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    sortedList = sortedList.OrderBy(m => m.GrandAmount).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    sortedList = sortedList.OrderByDescending(m => m.GrandAmount).ToList();
                }
            }
            else if (sortExpression.Equals("AmountReceived"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    sortedList = sortedList.OrderBy(m => m.AmountReceived).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    sortedList = sortedList.OrderByDescending(m => m.AmountReceived).ToList();
                }
            }
            else if (sortExpression.Equals("AmountBalance2"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    sortedList = sortedList.OrderBy(m => (m.GrandAmount - m.AmountReceived)).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    sortedList = sortedList.OrderByDescending(m => (m.GrandAmount - m.AmountReceived)).ToList();
                }
            }
            else if (sortExpression.Equals("PaymentMethod"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    sortedList = sortedList.OrderBy(m => m.PaymentMethod).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    sortedList = sortedList.OrderByDescending(m => m.PaymentMethod).ToList();
                }
            }
            else if (sortExpression.Equals("PayStatus"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    sortedList = sortedList.OrderBy(m => m.PayStatus).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    sortedList = sortedList.OrderByDescending(m => m.PayStatus).ToList();
                }
            }
            else if (sortExpression.Equals("DateReminder"))
            {
                //x.DateReminder(x.Id, EmailTypeEnum.Invoice_PendingPayment.ToString())
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    sortedList = flist.OrderBy(x => x.LastSendSubmissionReminderEmailDate).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    sortedList = flist.OrderByDescending(x => x.LastSendSubmissionReminderEmailDate).ToList();
                }
            }
        }
        #endregion

        radGridEntry.DataSource = sortedList; 
        if (needRebind) radGridEntry.DataBind();

        GeneralFunction.SetReportDataCache(sortedList);



        // hide show checkboxes for certain status
        //if (status == StatusPaymentEntry.NotPaid)
        //{
        //    foreach (GridDataItem item in radGridEntry.Items)
        //    {
        //        CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
        //        if (chkbox != null) chkbox.Visible = true;
        //    }
        //}
    }

    #region Events

    private void PaySave_Clicked(object sender, EventArgs e)
    {
        //phPay.Visible = false;
        //BindGrid(true);
        Response.Redirect("InvoiceList.aspx"); // if not redirect, this will cause caching and also cause double posting of the payment form if f5 by user
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

            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnDelete");

            //// No
            //lbl = (Label)e.Item.FindControl("lblNo");
            //lbl.Text = counter.ToString();

            // Status
            ((GridDataItem)e.Item)["PayStatus"].Text = GeneralFunction.GetPaymentEntryStatus(entry.PayStatus);
            ((GridDataItem)e.Item)["Entrytatus"].Text = GeneralFunction.GetEntryStatusForAdmin(entry.Status);
            ((GridDataItem)e.Item)["Deadline"].Text = entry.Deadline;

            // Status
            //if (entry.Status == StatusEntry.Completed)
            //    ((GridDataItem)e.Item)["Status"].Text = "<span style=\"font-weight:bold\">" + GeneralFunction.GetEntryStatus(entry.Status) + "</span>";
            //if (entry.WithdrawnStatus != "")
            //    ((GridDataItem)e.Item)["Status"].Text += "<br/><span style=\"color:Red;\">" + GeneralFunction.GetWithdrawnStatus(entry.WithdrawnStatus) + "</span>";


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
                
                if (string.IsNullOrEmpty(entry.PayCountry))
                {
                    ((GridDataItem)e.Item)["Firstname"].Text = reg.Firstname;
                    ((GridDataItem)e.Item)["Lastname"].Text = reg.Lastname;
                    ((GridDataItem)e.Item)["Country"].Text = reg.Country;
                }
                else
                {
                    ((GridDataItem)e.Item)["Firstname"].Text = entry.PayFirstname;
                    ((GridDataItem)e.Item)["Lastname"].Text = entry.PayLastname;
                    ((GridDataItem)e.Item)["Country"].Text = entry.PayCountry;
                }
            }

            
            // submitted details
            lbl = (Label)e.Item.FindControl("lblSubmittedDetails");
            lbl.Text = reg.Firstname + " " + reg.Lastname + "<br/>" + reg.Job + "<br/>" + GeneralFunction.ShowFriendlyContact(reg.Contact) + "<br/>" + reg.Email;


            // Payment Mode
            lbl = (Label)e.Item.FindControl("lbPaymentMode");
            lbl.Text = GeneralFunction.GetPaymentType(entry.PaymentMethod);


            // amount
            //((GridDataItem)e.Item)["Amount"].Text = (entry.Amount + entry.Fee).ToString();

            // invoice
            lnk = (HyperLink)e.Item.FindControl("lnkInvoice");
            lnk.NavigateUrl = "./PaymentPdfView.aspx?id=" + GeneralFunction.StringEncryption(entry.Id.ToString());

            //edit
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            lnkBtn.CommandArgument = entry.Id.ToString();
            Security.SecureControlByHiding(lnkBtn);

            // update payment
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
            lnkBtn.CommandArgument = entry.Id.ToString();
            
            // hide update payment if PP
            if (entry.PaymentMethod ==  PaymentType.PayPal) lnkBtn.Visible = false;
            Security.SecureControlByHiding(lnkBtn);

            // Resend
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnResend");
            lnkBtn.CommandArgument = entry.Id.ToString();
            Security.SecureControlByHiding(lnkBtn);


            // History
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnHistory");
            lnkBtn.CommandArgument = entry.PayGroupId.ToString();
           

            // chkboxes
            CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            Security.SecureControlReadOnlyByHiding(chkbox);


            string DateReminder = "";
            if (entry.DateReminder(entry.Id, EmailTypeEnum.Invoice_PendingPayment.ToString()) != DateTime.MinValue)
                DateReminder = entry.DateReminder(entry.Id, EmailTypeEnum.Invoice_PendingPayment.ToString()).ToString("dd/MM/yy H:mm");
            
            ((GridDataItem)e.Item)["DateReminder"].Text = (entry.LastSendSubmissionReminderEmailDate == DateTime.MaxValue) ? "" : entry.LastSendSubmissionReminderEmailDate.ToString("dd/MM/yy H:mm"); ;
            
            //((GridDataItem)e.Item)["DateReminder"].Text = GeneralFunction.CleanDateTimeToString(entry.LastSendPaymentReminderEmailDate , "dd/MM/yy hh:mm:ss tt");
            string status = (string)ViewState["TabFilterValue"];
            //if (status == StatusPaymentEntry.NotPaid)
            //{
            //    //foreach (GridDataItem item in radGridEntry.Items)
            //    //{
            //        CheckBox checkbox = (CheckBox)e.Item.FindControl("chkbox");
            //        if (checkbox != null) checkbox.Visible = true;
            //   // }
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
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
        }
    }

    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";


        if (e.CommandName == "Payment")
        {
            GeneralFunction.SetFilter("InvoiceList", txtSearch.Text, ddlSearch.SelectedValue, ddlPaymentStatus.SelectedValue, ddlCountry.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty);
            phPay.Visible = true;
            up1.EntryId = new Guid(e.CommandArgument.ToString());
            up1.PopulateForm();
        }
        else if (e.CommandName == "User")
        {
            GeneralFunction.SetFilter("InvoiceList", txtSearch.Text, ddlSearch.SelectedValue, ddlPaymentStatus.SelectedValue, ddlCountry.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty);
            Registration reg = Registration.GetRegistration(new Guid(e.CommandArgument.ToString()));
            GeneralFunction.SetRedirect("../Admin/InvoiceList.aspx");  // to return from whereever
            Response.Redirect("../Admin/Profile.aspx?Id=" + reg.Id.ToString());
        }
        else if (e.CommandName == "Edit")
        {
            GeneralFunction.SetFilter("InvoiceList", txtSearch.Text, ddlSearch.SelectedValue, ddlPaymentStatus.SelectedValue, ddlCountry.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty);
            Entry en = Entry.GetEntry(new Guid(e.CommandArgument.ToString()));
            Registration reg = Registration.GetRegistration(en.RegistrationId);
            Security.SetLoginSessionUser(reg);

            // get the list of entry ids for this paygorup id
            GeneralFunction.ResetGroupPaymentCache();
            EntryList elist = EntryList.GetEntryList(en.PayGroupId, Guid.Empty, "");
            foreach (Entry ent in elist)
                GeneralFunction.AddIdToGroupPaymentCache(ent.Id);

            GeneralFunction.SetRedirect("../Admin/InvoiceList.aspx");  // to return from whereever
            Response.Redirect("../Main/Summary.aspx?adm=e&pgId=" + elist[0].PayGroupId.ToString());
        }
        else if (e.CommandName == "Resend")
        {
            Entry entry = Entry.GetEntry(new Guid(e.CommandArgument.ToString()));
            Registration reg = Registration.GetRegistration(entry.RegistrationId);

            if (entry.PaymentMethod == PaymentType.PayPal)
            {
                // for PP this is the only email
                Email.SendCompletedPaymentEmailPayPal(reg, entry.PayGroupId, entry.Invoice);
            }
            else
            {
                // Always send the pending email regardless of status

                //if (entry.PayStatus == StatusPaymentEntry.NotPaid)
                //{
                    if (entry.PaymentMethod == PaymentType.BankTransfer) Email.SendNewEntryPendingPaymentBankTransferEmail(reg, entry.PayGroupId, entry.Invoice);
                    if (entry.PaymentMethod == PaymentType.Cheque) Email.SendNewEntryPendingPaymentChequeEmail(reg, entry.PayGroupId, entry.Invoice);
                //}
                //else
                //{
                //    Email.SendCompletedPaymentEmailOthers(reg, entry.PayGroupId, entry.Invoice);
                //}
            }
            lblError.Text = "Sent.";
        }
        else if (e.CommandName == "History")
        {
            GeneralFunction.SetFilter("InvoiceList", txtSearch.Text, ddlSearch.SelectedValue, ddlPaymentStatus.SelectedValue, ddlCountry.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty);
            GeneralFunction.SetRedirect("../Admin/InvoiceList.aspx");  // to return from whereever
            Response.Redirect("../Admin/AmountReceived.aspx?pgId=" + e.CommandArgument.ToString());
        }

        else if (e.CommandName == "adhoc")
        {
            GeneralFunction.SetFilter("InvoiceList", txtSearch.Text, ddlSearch.SelectedValue, ddlPaymentStatus.SelectedValue, ddlCountry.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty);
            GeneralFunction.SetRedirect("../Admin/AdhocInvoiceList.aspx");  // to return from whereever
            Response.Redirect("../Admin/AdhocInvoice.aspx?regId=" + GeneralFunction.StringEncryption(e.CommandArgument.ToString()));
        }
    }

    protected void radGridEntry_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        BindGrid(true, e.SortExpression, e.NewSortOrder.ToString());
    }

    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
        {
            foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
            {
                BindGrid(false, expr.FieldName, expr.SortOrder.ToString());
            }
        }
        else
        {
            BindGrid(false, string.Empty, GridSortOrder.None.ToString());
        } 
    }



    protected void rtabEntry_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        Telerik.Web.UI.RadTab TabClicked = e.Tab;       
        string tabvalue = TabClicked.Value;

        ViewState["TabFilterValue"] = tabvalue;
        ViewState["AdvanceSearch"] = "";
        ViewState["SortExpression"] = "";
        ViewState["SortOrder"] = "";
        radGridEntry.MasterTableView.SortExpressions.Clear();
        BindGrid(true, string.Empty, string.Empty);

        // Readonly Admin
        Security.SecureControlByHiding(btnEmailReminder);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rtabEntry.Visible = false;
        //btnEmailReminder.Visible = false;
        lblError.Text = "";

        GeneralFunction.SetFilter("InvoiceList", txtSearch.Text, ddlSearch.SelectedValue, ddlPaymentStatus.SelectedValue, ddlCountry.SelectedValue, string.Empty, string.Empty, string.Empty, string.Empty);
        ViewState["AdvanceSearch"] = "1";
        BindGrid(true, string.Empty, string.Empty);

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlPaymentStatus.SelectedValue = "";
        ddlCountry.SelectedValue = "";

        rtabEntry.Visible = true;
        //btnEmailReminder.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";

        ViewState["SortExpression"] = "";
        ViewState["SortOrder"] = "";

        GeneralFunction.ResetFilter();
        GeneralFunction.ResetReportDataCache();


        rtabEntry.SelectedIndex = 0;
        BindGrid(true, string.Empty, string.Empty);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Entry> flist = (List<Entry>)data;

            // report to sort by invoice number
            flist = flist.OrderBy(f => f.Invoice).ToList();


            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Invoices";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DL"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DateInvoice"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Invoice"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TotalAmount"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AmountPaid"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("BalanceDue"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Status"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryAmount"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FeeAmount"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("GST"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PaymentMethod"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayCompany"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayAddress1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayAddress2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayCity"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayPostal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayCountry"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayFirstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayLastname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PayContact"); x++;

            //Edit By Rico, Nov 28 2013, Extra Collumn for report
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Sal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("First Name"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Last Name"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Job Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Contact"); x++;
            //END Edit By Rico


            ////////////////////////
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assign"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Reminder"); x++;
            ////////////////////////

            y++;

            foreach (Entry ent in flist)
            {
                x = 1;

                //decimal balance = ent.Amount + ent.Fee - ent.AmountReceived;
                decimal balance = ent.GrandAmount - ent.AmountReceived;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Deadline); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(ent.DateSubmitted, "dd/MM/yy")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Invoice); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((ent.Amount + ent.Fee)); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.GrandAmount); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.AmountReceived); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(balance); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetPaymentEntryStatus(ent.PayStatus)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Amount); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Fee); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Tax); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetPaymentType(ent.PaymentMethod)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayCompany); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayAddress1); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayAddress2); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayCity); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayPostal); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayCountry); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayFirstname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.PayLastname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(ent.PayContact)); x++;

                //Edit By Rico, Nov 28 2013, Extra Collumn for report
                Registration reg = GeneralFunction.GetRegistrationFromEntry(ent);
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Firstname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Lastname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Email); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Job); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Contact); x++;
                //END Edit By Rico

                ////////////////////////////////////////////
                Administrator Assign = null;
                try
                {
                    Assign = AdministratorList.GetAdministratorList().Where(h => h.Id == ent.AdminidAssignedto).FirstOrDefault();
                }
                catch { }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Assign != null ? Assign.LoginId : ""); x++;

                string DateReminder = "";
                if (ent.DateReminder(ent.Id, EmailTypeEnum.Invoice_PendingPayment.ToString()) != DateTime.MinValue)
                    DateReminder = ent.DateReminder(ent.Id, EmailTypeEnum.Invoice_PendingPayment.ToString()).ToString("dd/MM/yy H:mm");

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((ent.LastSendSubmissionReminderEmailDate == DateTime.MaxValue) ? "" : ent.LastSendSubmissionReminderEmailDate.ToString("dd/MM/yy H:mm")); x++;
                ////////////////////////////////////////////

                y++;
            }



            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Invoices.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    #endregion

    #region Helper

    public string GetSortExpression()
    {
        if (ViewState["SortExpression"] == null)
            return string.Empty;
        else
            return ViewState["SortExpression"].ToString();
    }

    public void SetSortExpression(string sortExp)
    {
        ViewState["SortExpression"] = sortExp;
    }

    public string GetSortOrder()
    {
        if (ViewState["SortOrder"] == null)
            return string.Empty;
        else
            return ViewState["SortOrder"].ToString();
    }

    public void SetSortOrder(string sortOrder)
    {
        ViewState["SortOrder"] = sortOrder;
    }

    #endregion
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
             .Where(x => x.EmailType == EmailType.Invoice.ToString() && x.IsActive && !x.IsDelete).ToList();

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
            //EmailTemplate emtemp = SaveForm();
            //GenerateEmails(emtemp.TemplateId);
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
                Email.SendReminderEmailTemplatelReg(reg2, GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true), txtTemplateSubject.Text, EntryList, EmailTypeEnum.Invoice_PendingPayment.ToString());
                foreach (Guid entrylistItem in EntryList)
                {
                    GeneralFunction.SaveEmailSentLogReg(reg2, templateId, evetnYear, EmailTypeEnum.Invoice_PendingPayment.ToString(), entrylistItem);
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
                Email.SendReminderEmailTemplatelReg(reg, GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true), txtTemplateSubject.Text, EntryList, EmailTypeEnum.Invoice_PendingPayment.ToString());
                foreach (Guid entrylistItem in EntryList)
                {
                    GeneralFunction.SaveEmailSentLogReg(reg, templateId, evetnYear, EmailTypeEnum.Invoice_PendingPayment.ToString(), entrylistItem);
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
        radGridEntry.Rebind();
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
