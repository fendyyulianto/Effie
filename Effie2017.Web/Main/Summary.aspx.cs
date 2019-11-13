using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Effie2017.App;
using System.Configuration;
using System.Text;
using System.Linq;

public partial class Main_Summary : System.Web.UI.Page
{
    List<Guid> list = null;
    int counter;
    Guid payGroupId;
    string PgId = "";
    bool isAdminEdit = false;
    Entry entry = null;
    //Guid SelectedId = Guid.Empty;
    string Colspan = "4";
    protected void Page_Load(object sender, EventArgs e)
    {
        list = GeneralFunction.GetGroupPaymentListCache();
        if (Request.QueryString["pgId"] != "" && Request.QueryString["pgId"] != null)
            PgId = Request.QueryString["pgId"];

        isAdminEdit = (Request.QueryString["adm"] == "e");
        if (isAdminEdit)
            payGroupId = new Guid((Request.QueryString["pgId"]));

        counter = 1;
        if (!IsPostBack)
        {
            try {
                LoadForm();
                PopulateForm();
            }
            catch {
                Response.Redirect("../main/Dashboard.aspx");
            }
        }
    }

    private void LoadForm()
    {
        GeneralFunction.LoadDropDownListCountry(ddlCountry);
        ddlCountry.Items.Insert(0, new ListItem("Select", ""));

        lbAdminFees.Text = decimal.Parse("0").ToString("N");

    }

    private void PopulateForm()
    {
        BindGrid();
        DisplayPriceAmounts();

        if (isAdminEdit)
        {       
            phAdminRemarks.Visible = true;
            BindAdminRemarks();

            // Access Control for administrators
            Security.SecureControlByHiding(btnAdminSubmitRemarks);
            Security.SecureControlByHiding(btnSubmit);
        }
    }

    private void BindGrid()
    {
        List<Entry> entries = new List<Entry>();
        foreach (Guid id in list)
        {
            entry = Entry.GetEntry(id);
            entries.Add(entry);
        }
        
        rptEntry.DataSource = entries;
        rptEntry.DataBind();
        
        Entry firstentry = entries[0];

        // admin mode - load from the payment data fields of the entry
        if (string.IsNullOrEmpty(firstentry.PayCountry))
        {
            // Prepop the form
            Registration thisuser = Security.GetLoginSessionUser();
            txtCompany.Text = thisuser.Company;
            txtFirstname.Text = thisuser.Firstname;
            txtLastname.Text = thisuser.Lastname;
            txtAddress1.Text = thisuser.Address1;
            txtAddress2.Text = thisuser.Address2;
            txtCity.Text = thisuser.City;
            txtPostal.Text = thisuser.Postal;
            ddlCountry.SelectedValue = thisuser.Country;
            txtContactCountryCode.Text = GeneralFunction.GetCountryCodeFromContactNumber(thisuser.Contact);
            txtContactAreaCode.Text = GeneralFunction.GetAreaCodeFromContactNumber(thisuser.Contact);
            txtContactNumber.Text = GeneralFunction.GetNumberFromContactNumber(thisuser.Contact);
        }
        else
        {
            txtCompany.Text = firstentry.PayCompany;
            txtFirstname.Text = firstentry.PayFirstname;
            txtLastname.Text = firstentry.PayLastname;
            txtAddress1.Text = firstentry.PayAddress1;
            txtAddress2.Text = firstentry.PayAddress2;
            txtCity.Text = firstentry.PayCity;
            txtPostal.Text = firstentry.PayPostal;
            ddlCountry.SelectedValue = firstentry.PayCountry;
            txtContactCountryCode.Text = GeneralFunction.GetCountryCodeFromContactNumber(firstentry.PayContact);
            txtContactAreaCode.Text = GeneralFunction.GetAreaCodeFromContactNumber(firstentry.PayContact);
            txtContactNumber.Text = GeneralFunction.GetNumberFromContactNumber(firstentry.PayContact);
            rblPayment.SelectedValue = firstentry.PaymentMethod;
        }


        DisplayPriceAmounts();

        // Disable the payment selection
        if (isAdminEdit)
            rblPayment.Enabled = false;

    }

    private void BindAdminRemarks()
    {
        lbAdminRemarks.Text = "";
        InvoiceRemarksList remarksList = InvoiceRemarksList.GetInvoiceRemarksList(payGroupId);
        foreach (InvoiceRemarks remarks in remarksList)
        {
            string RemarkBy = remarks.DateTimeCreated.ToString("dd/MM/yy hh:mm tt");
            if (remarks.isAdmin)
            {
                try { RemarkBy += " (" + Administrator.GetAdministrator(remarks.CommentatorID).LoginId + ")"; }
                catch { }
            }
            else
            {
                try { RemarkBy += " (" + Registration.GetRegistration(remarks.CommentatorID).Firstname + ")"; }
                catch { }
            }

            lbAdminRemarks.Text += "<tr><td width='200px' style='vertical-align:top'>" + RemarkBy + " </td><td width='5px'> : </td><td>" + remarks.Remarks + "</td></tr>";
        }

        if (lbAdminRemarks.Text == "")
            lbAdminRemarks.Text = "None";
        else
            lbAdminRemarks.Text = "<table width='100%' style='font-size:10px;'>" + lbAdminRemarks.Text + "</table>";
    }

    private bool ValidateForm()
    {
        lbError.Text = "";
        lbError2.Text = "";
        GeneralFunction.RemoveHighlightControls(this);

        lbError.Text += GeneralFunction.ValidateRadioButtonList("Payment Method", rblPayment, true);
        lbError.Text += GeneralFunction.ValidateTextBox("Company", txtCompany, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Address1", txtAddress1, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("City", txtCity, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Postal", txtPostal, true, "string");
        lbError.Text += GeneralFunction.ValidateDropDownList("Country", ddlCountry, true, "");
        lbError.Text += GeneralFunction.ValidateTextBox("First name", txtFirstname, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Last name", txtLastname, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Country Code", txtContactCountryCode, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Area Code", txtContactAreaCode, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact", txtContactNumber, true, "phoneNumber");

        lbError2.Text = lbError.Text;
        return (lbError.Text == "");
    }

    private void SaveForm()
    {
        // Save the fields to the group of entries
        List<Entry> entries = new List<Entry>();
        List<Entry> entriesSession = new List<Entry>();
        entriesSession.Clear();
        // new paygroupid
        payGroupId = Guid.NewGuid();
        Guid SessionPayGroup = Guid.Empty;
        // cal the grand total
        decimal subtotal = GeneralFunction.CalculateGroupTotalPriceFromCache();
        // Get the entries
        counter = 0;
        foreach (Guid id in list)
        {
            Entry entry = Entry.GetEntry(id);
            entriesSession.Add(onEntryData(entry, subtotal));
            counter++;
        }

        //Session["EntryList-" + SessionPayGroup] = entriesSession;
    }

    public Entry onEntryData(Entry entry, decimal subtotal)
    {
        entry.PayCompany = txtCompany.Text.Trim();
        entry.PayAddress1 = txtAddress1.Text.Trim();
        entry.PayAddress2 = txtAddress2.Text.Trim();
        entry.PayCity = txtCity.Text.Trim();
        entry.PayPostal = txtPostal.Text.Trim();
        entry.PayCountry = ddlCountry.SelectedValue;
        entry.PayFirstname = txtFirstname.Text.Trim();
        entry.PayLastname = txtLastname.Text.Trim();
        entry.PayContact = GeneralFunction.CreateContact(txtContactCountryCode.Text.Trim(), txtContactAreaCode.Text.Trim(), txtContactNumber.Text.Trim());
        entry.PaymentMethod = rblPayment.SelectedValue;

        // do not adjust amounts or paygroupid if in admin mode
        //if (!isAdminEdit)
        {
            Registration reg = Registration.GetRegistration(entry.RegistrationId);
            entry.Amount = GeneralFunction.CalculateEntryPrice(entry, reg);
            entry.Fee = 0;
            entry.Tax = 0;

            // add the fees and tax to only the first entry in the invoice group
            if (counter == 0)
            {
                if (rblPayment.SelectedValue == PaymentType.PayPal) entry.Fee = GeneralFunction.CalculateCreditFees(subtotal);
                if (rblPayment.SelectedValue == PaymentType.BankTransfer) entry.Fee = GeneralFunction.CalculateBankTransferFees();

                //entry.Tax = 0;
                if (ddlCountry.SelectedValue.ToLower() == "singapore") entry.Tax = GeneralFunction.CalculateTax(subtotal + entry.Fee);
            }


            entry.GrandAmount = entry.Amount + entry.Fee + entry.Tax;


            // To solve pending payment issue from paypal - only if paygroupId is empty will assign new paygroupId or else it will have same paygroupId for the entire process
            // update the pay group id to be the same
            if (entry.PayGroupId == Guid.Empty)
                entry.PayGroupId = payGroupId;
            else
                payGroupId = entry.PayGroupId;
        }

        //entry.PayStatus = StatusPaymentEntry.NotPaid;
        //entry.Status = StatusEntry.PaymentPending;
        
        if(rblPayment.SelectedValue != PaymentType.PayPal)
            entry.Save();

        //If admin then need save
        if (isAdminEdit)
        {
            entry.Save();
        }


        {
            EntryPayment entryPayment = null;
            entryPayment = EntryPaymentList.GetEntryPaymentList().FirstOrDefault(x => x.EntryId == entry.Id);
            if (entryPayment == null)
            {
                entryPayment = EntryPayment.NewEntryPayment();
                entryPayment.EntryId = entry.Id;
            }

            entryPayment.PayCompany = entry.PayCompany;
            entryPayment.PayAddress1 = entry.PayAddress1;
            entryPayment.PayAddress2 = entry.PayAddress2;
            entryPayment.PayCity = entry.PayCity;
            entryPayment.PayPostal = entry.PayPostal;
            entryPayment.PayCountry = entry.PayCountry;
            entryPayment.PayFirstname = entry.PayFirstname;
            entryPayment.PayLastname = entry.PayLastname;
            entryPayment.PayContact = entry.PayContact;
            entryPayment.PaymentMethod = entry.PaymentMethod;
            entryPayment.PayGroupId = entry.PayGroupId;
            entryPayment.Amount = entry.Amount;
            entryPayment.Fee = entry.Fee;
            entryPayment.Invoice = entry.Invoice;
            entryPayment.Tax = entry.Tax;
            entryPayment.GrandAmount = entry.GrandAmount;
            entryPayment.AmountReceived = entry.AmountReceived;
            entryPayment.Save();

        }

        return entry;
    }

    #region Events
    protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        DisplayPriceAmounts();
    }
    protected void rptEntry_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Entry entry = (Entry)e.Item.DataItem;
        if (entry != null)
        {
            string Colspan = "5";
            Label lbNo = (Label)e.Item.FindControl("lbNo");
            if (lbNo!= null)
            {
                lbNo.Text = counter.ToString();
            }
            Label lbSerialNo = (Label)e.Item.FindControl("lbSerialNo");
            System.Web.UI.HtmlControls.HtmlTableCell ThEntryID2 = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("ThEntryID2");
            if (lbSerialNo != null)
            {
                if (string.IsNullOrEmpty(entry.Serial))
                {
                    ThEntryID1.Visible = false;
                    ThEntryID2.Visible = false;
                    Colspan = "4";
                }

                lbSerialNo.Text = entry.Serial;
            }

            Tab1.Attributes.Add("colspan", Colspan);
            Tab2.Attributes.Add("colspan", Colspan);
            Tab3.Attributes.Add("colspan", Colspan);
            Tab4.Attributes.Add("colspan", Colspan);
            Label lbTitle = (Label)e.Item.FindControl("lbTitle");
            if (lbTitle != null)
            {
                lbTitle.Text = entry.Campaign;
            }
            Label lbBrand = (Label)e.Item.FindControl("lbBrand");
            if (lbBrand != null)
            {
                lbBrand.Text = entry.Brand;
            }
            Label lbCategory = (Label)e.Item.FindControl("lbCategory");
            if (lbCategory != null)
            {
                lbCategory.Text = Data.GetCategoryMarket(entry.CategoryMarket) + " <br> " + entry.CategoryPSDetail;
            }
            Label lbFees = (Label)e.Item.FindControl("lbFees");
            if (lbFees != null)
            {
                Registration reg = Registration.GetRegistration(entry.RegistrationId);
                decimal amount = GeneralFunction.CalculateEntryPrice(entry, reg);
                lbFees.Text = amount.ToString("N");
            }
            counter++;
        }
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        DisplayPriceAmounts();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            SaveForm();

            // id admin mode, go back to admin page
            if (isAdminEdit) Response.Redirect(GeneralFunction.GetRedirect("../Admin/InvoiceList.aspx"));

            bool isEditMode = false;
            if (Request["pgId"] != null)
                isEditMode = true;

            GeneralFunction.CompletePendingPaymentEntrySubmission(payGroupId, rblPayment.SelectedValue, isEditMode);

            if (rblPayment.SelectedValue == PaymentType.PayPal)
            {
                // get the string of serial numbers
                string serials = "";
                EntryList list = EntryList.GetEntryList(payGroupId, Guid.Empty, ""); // contains the pay group id
                foreach (Entry entry in list)
                {
                    serials += entry.Serial + ",";
                }
                if (serials != "") serials = serials.Substring(0, serials.Length - 1);

                PayPal(serials);
            }
            else
            {
                //if (rblPayment.Text == "")
                //{
                    //Email.SendAdhocPendingPaymentEmail(payGroupId, lbTotalFees.Text);
                //}
                Response.Redirect("../Main/PendingPayment.aspx");
            }
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        // id admin mode, go back to admin page
        if (isAdminEdit) Response.Redirect(GeneralFunction.GetRedirect("../Admin/InvoiceList.aspx"));

        Response.Redirect("../Main/Dashboard.aspx");
    }
    protected void btnAdminSubmitRemarks_Click(object sender, EventArgs e)
    {
        Administrator admin = Security.GetAdminLoginSession();
        Registration reg = Security.GetLoginSessionUser();
        if (txtAdminRemarks.Text.Trim() != "")
        {
            InvoiceRemarks remarks = InvoiceRemarks.NewInvoiceRemarks();
            remarks.Remarks = txtAdminRemarks.Text;
            remarks.PayGroupId = payGroupId;
            remarks.DateTimeCreatedString = DateTime.Now.ToString();
            if (admin != null)
            {
                remarks.isAdmin = true;
                remarks.CommentatorID = admin.Id;
            }
            else
            {
                remarks.isAdmin = false;
                remarks.CommentatorID = reg.Id;
            }
            remarks.Save();

            txtAdminRemarks.Text = "";
            BindAdminRemarks();
        }
    }
    #endregion

    #region Helper

    private void DisplayPriceAmounts()
    {
        decimal fees = 0;
        decimal subtotal = GeneralFunction.CalculateGroupTotalPriceFromCache();
        if (subtotal < -1)
        {
            subtotal = 0;
            foreach (Guid id in list)
            {
                Entry entry = Entry.GetEntry(id);
                Registration reg = Registration.GetRegistration(entry.RegistrationId);
                subtotal += GeneralFunction.CalculateEntryPrice(entry, reg);
            }
        }
        
        if (rblPayment.SelectedValue == PaymentType.Cheque)
        {
            fees = 0;
            lbTotalFees.Text = GeneralFunction.CalculateGroupTotalPriceFromCache().ToString("N");

            phAdminFees.Visible = false;
        }
        else
        {
            if (rblPayment.SelectedValue == PaymentType.PayPal)
            {
                fees = GeneralFunction.CalculateCreditFees(subtotal);
            }
            else if (rblPayment.SelectedValue == PaymentType.BankTransfer)
            {
                fees = GeneralFunction.CalculateBankTransferFees();
            }

            phAdminFees.Visible = true;
        }
        if (fees == 0) phAdminFees.Visible = false;


        decimal total = subtotal + fees;
        decimal gst = 0;
        if (ddlCountry.SelectedValue.ToLower() == "singapore") gst = GeneralFunction.CalculateTax(total);
        decimal grandtotal = subtotal + fees + gst;

        lbSubTotal.Text = total.ToString("N");
        lbAdminFees.Text = fees.ToString("N");
        lbGST.Text = gst.ToString("N");
        lbTotalFees.Text = grandtotal.ToString("N");

        lbGSTRate.Text = (decimal.Parse(ConfigurationManager.AppSettings["GSTRate"]) * 100).ToString("#");
    }

    private void PayPal(string serials)
    {
        StringBuilder url = new StringBuilder();

        string m_sPaypalBase = System.Configuration.ConfigurationManager.AppSettings["paypalBase"];
        string business = System.Configuration.ConfigurationManager.AppSettings["paypalEmail"];
        string cancelUrl = System.Configuration.ConfigurationManager.AppSettings["cancelPaymentUrl"];
        string returnUrl = System.Configuration.ConfigurationManager.AppSettings["successPaymentUrl"];
        string notifyUrl = System.Configuration.ConfigurationManager.AppSettings["notifyUrl"];

        url.Append(m_sPaypalBase);
        url.AppendFormat("&business={0}", HttpUtility.UrlEncode(business));
        url.AppendFormat("&item_name={0}", HttpUtility.UrlEncode("Payment for APAC Effie Entry"));  // for: " + serials));
        url.AppendFormat("&item_number={0}", HttpUtility.UrlEncode(payGroupId.ToString().Substring(0, 8).ToUpper())); // suppose to serial num, but there is possiblity of multiple entries per order????

        decimal amount = GeneralFunction.CalculateGroupTotalPriceFromCache();
        amount += GeneralFunction.CalculateCreditFees(amount); // Add fees for PP payment // 0.01;
        if (ddlCountry.SelectedValue.ToLower() == "singapore") amount += GeneralFunction.CalculateTax(amount);

        bool IsTestPay = System.Configuration.ConfigurationManager.AppSettings["IsTestPay"].ToString() == "1";
        if (IsTestPay)
        {
            amount = 0.01m;
        }
        url.AppendFormat("&amount={0}", HttpUtility.UrlEncode(amount.ToString("0.00")));
        
        //For testing of $0.01 
        //url.AppendFormat("&amount={0}", HttpUtility.UrlEncode("0.01"));
        
        url.AppendFormat("&currency_code={0}", HttpUtility.UrlEncode("SGD"));

        url.AppendFormat("&shipping={0}", HttpUtility.UrlEncode("0"));
        url.AppendFormat("&no_shipping={0}", HttpUtility.UrlEncode("0"));
        url.AppendFormat("&invoice={0}", "");

        string custom = payGroupId.ToString();
        url.AppendFormat("&custom={0}", HttpUtility.UrlEncode(custom));
        url.AppendFormat("&cancel_return={0}", HttpUtility.UrlEncode(cancelUrl));
        url.AppendFormat("&notify_url={0}", HttpUtility.UrlEncode(notifyUrl));
        url.AppendFormat("&return={0}", HttpUtility.UrlEncode(returnUrl + "?custom=" + payGroupId.ToString()));

        RawLog rawlog = RawLog.NewRawLog();
        rawlog.Type = 0; //send
        rawlog.Data = url.ToString();
        rawlog.DateString = DateTime.Now.ToString();

        if (rawlog.IsValid) rawlog.Save();
        //GeneralFunction.CompleteNewEntrySubmissionPayPal(payGroupId);

        Response.Redirect(url.ToString());
    }

    #endregion
}