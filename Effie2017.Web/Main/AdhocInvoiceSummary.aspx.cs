using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Main_AdhocInvoiceSummary : System.Web.UI.Page
{
    List<Guid> list = null;
    int counter;
    Guid payGroupId;
    bool isAdminEdit = false;
    Entry entry = null;
    Guid SelectedId = Guid.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //list = GeneralFunction.GetGroupPaymentListCache();
        //if ((Request.QueryString["Id"] != null && Request.QueryString["Id"] != ""))
        //    SelectedId = new Guid(Request.QueryString["Id"]);
        //else if (list == null)
        //    return;

        payGroupId = new Guid(GeneralFunction.StringDecryption(Request.QueryString["pgId"]));

        counter = 1;
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
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
        AdhocInvoiceItemList adInvoiceList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(payGroupId,Guid.Empty);

        if (adInvoiceList.Count > 0)
        {
            AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(adInvoiceList[0].AdhocInvoiceId);

            //set Payment Method
            rblPayment.SelectedValue = adInv.PaymentMethod;

            rptEntry.DataSource = adInvoiceList;
            rptEntry.DataBind();


            // Prepop the form
            Registration thisuser = Registration.GetRegistration(adInv.RegistrationId);
            if (string.IsNullOrEmpty(adInv.PayCountry))
            {
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
                txtCompany.Text = adInv.PayCompany;
                txtFirstname.Text = adInv.PayFirstname;
                txtLastname.Text = adInv.PayLastname;
                txtAddress1.Text = adInv.PayAddress1;
                txtAddress2.Text = adInv.PayAddress2;
                txtCity.Text = adInv.PayCity;
                txtPostal.Text = adInv.PayPostal;
                ddlCountry.SelectedValue = adInv.PayCountry;
                txtContactCountryCode.Text = GeneralFunction.GetCountryCodeFromContactNumber(adInv.PayContact);
                txtContactAreaCode.Text = GeneralFunction.GetAreaCodeFromContactNumber(adInv.PayContact);
                txtContactNumber.Text = GeneralFunction.GetNumberFromContactNumber(adInv.PayContact);
            }

            DisplayPriceAmounts();
        }

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

    private AdhocInvoice SaveForm()
    {        
        AdhocInvoiceItemList adInvoiceItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(payGroupId,Guid.Empty);
        List<AdhocInvoiceItem> adhocInvoiceItemList = new List<AdhocInvoiceItem>();
        if (adInvoiceItemList.Count > 0)
        {
            AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(adInvoiceItemList[0].AdhocInvoiceId);
            // cal the grand total
            
            adInv.PayCompany = txtCompany.Text.Trim();
            adInv.PayAddress1 = txtAddress1.Text.Trim();
            adInv.PayAddress2 = txtAddress2.Text.Trim();
            adInv.PayCity = txtCity.Text.Trim();
            adInv.PayPostal = txtPostal.Text.Trim();
            adInv.PayCountry = ddlCountry.SelectedValue;
            adInv.PayFirstname = txtFirstname.Text.Trim();
            adInv.PayLastname = txtLastname.Text.Trim();
            adInv.PayContact = GeneralFunction.CreateContact(txtContactCountryCode.Text.Trim(), txtContactAreaCode.Text.Trim(), txtContactNumber.Text.Trim());
            adInv.PaymentMethod = rblPayment.SelectedValue;

            if (!string.IsNullOrEmpty(adInv.PaymentMethod))
                adInv.InvoiceDateString = DateTime.Now.ToString();

            adInv.Amount = 0;
            adInv.Tax = 0;
            adInv.Fee = 0;
            
            foreach (AdhocInvoiceItem adInvoiceItem in adInvoiceItemList)
            {               
                adInvoiceItem.GrandAmount = adInvoiceItem.Amount;
                adInvoiceItem.Save();

                adhocInvoiceItemList.Add(adInvoiceItem);

                adInv.Amount += adInvoiceItem.Amount;
            }

            decimal subtotal = adInv.Amount;

            if (rblPayment.SelectedValue == PaymentType.PayPal) adInv.Fee = GeneralFunction.CalculateCreditFees(subtotal);
            if (rblPayment.SelectedValue == PaymentType.BankTransfer) adInv.Fee = GeneralFunction.CalculateBankTransferFees();
            //entry.Tax = 0;
            if (ddlCountry.SelectedValue.ToLower() == "singapore") adInv.Tax = GeneralFunction.CalculateTax(subtotal + adInv.Fee);

            adInv.GrandAmount = adInv.Amount + adInv.Fee + adInv.Tax;

            AdhocInvoiceModel adhocInvoices = new AdhocInvoiceModel(adInv, adhocInvoiceItemList);
            Session["AdhocInvoiceModel-" + payGroupId] = adhocInvoices;

            if (adInv.IsValid)
                adInv.Save();

            return adInv;
        }
        return null;
    }

    #region Events
    protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        DisplayPriceAmounts();
    }
    protected void rptEntry_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        AdhocInvoiceItem adhocInv = (AdhocInvoiceItem)e.Item.DataItem;
        if (adhocInv != null)
        {
            Entry entry = Entry.GetEntry(adhocInv.EntryId);

            Label lbNo = (Label)e.Item.FindControl("lbNo");
            if (lbNo != null)
            {
                lbNo.Text = counter.ToString();
            }
            Label lbEntryID = (Label)e.Item.FindControl("lbEntryID");
            if (lbEntryID != null)
            {
                lbEntryID.Text = entry.Serial;
            }
            Label lbTitle = (Label)e.Item.FindControl("lbTitle");
            if (lbTitle != null)
            {
                lbTitle.Text = entry.Campaign;
            }
            Label lbCategory = (Label)e.Item.FindControl("lblCategory");
            if (lbCategory != null)
            {
                lbCategory.Text = Data.GetCategoryMarket(entry.CategoryMarket) + " <br> " + entry.CategoryPSDetail;
            }
            Label lbDesc = (Label)e.Item.FindControl("lbDesc");
            if (lbDesc != null)
            {
                if (!adhocInv.InvoiceType.Equals(AdhocInvoiceType.Custom))
                    lbDesc.Text = GeneralFunction.GetInvoiceType(adhocInv.InvoiceType);
                else
                    lbDesc.Text = adhocInv.InvoiceTypeOthers;
            }
            Label lbFees = (Label)e.Item.FindControl("lbFees");
            if (lbFees != null)
            {               
                lbFees.Text = adhocInv.Amount.ToString("N");
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
            AdhocInvoice adInv = SaveForm();

            // id admin mode, go back to admin page
            if (isAdminEdit) Response.Redirect(GeneralFunction.GetRedirect("../Admin/InvoiceList.aspx"));

            //To set to Upload Complete if Payment Method is selected
            //Administrator admin = Security.GetAdminLoginSession();
            AdhocInvoiceItemList adhocInvItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(payGroupId, adInv.Id);
            EntryList entries = EntryList.GetEntryList(Guid.Empty, adInv.RegistrationId, "");
            foreach (AdhocInvoiceItem adhocInvItem in adhocInvItemList)
            {
                try
                {
                    //FILTER For REOPEN ENTRY PROCESSING 
                    Entry entry = entries.FirstOrDefault(x => x.Id == adhocInvItem.EntryId &&
                    ((x.Status == StatusEntry.Completed) && (x.ProcessingStatus == StatusEntry.PendingReopen)));

                    if (adhocInvItem.InvoiceType == "ReOpen")
                    {
                        //To set to Upload Complete if Payment Method is selected
                        if (adInv.Invoice == "" && adInv.PaymentMethod != "")
                        {
                            entry.Status = StatusEntry.UploadCompleted;
                            entry.ReopeningDate = DateTime.Now.ToString();//
                            entry.ProcessingStatus = StatusEntry.Reopened;//
                            entry.IDAdhocInvoice = adhocInvItem.AdhocInvoiceId.ToString();//
                        }

                        entry.Save();
                    }
                }
                catch { }
            }

            GeneralFunction.CompletePendingPaymentAdhoc(adInv.RegistrationId, adInv.PayGroupId, adInv.PaymentMethod, lbTotalFees.Text, true);
            
            if (rblPayment.SelectedValue == PaymentType.PayPal)
            {
                // get the string of serial numbers
                string serials = "";
                EntryList list = EntryList.GetEntryList(adInv.PayGroupId, Guid.Empty, ""); // contains the pay group id
                foreach (Entry entry in list)
                {
                    serials += entry.Serial + ",";
                }
                if (serials != "") serials = serials.Substring(0, serials.Length - 1);

                PayPal(serials);
            }
            else
            {
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
        if (txtAdminRemarks.Text.Trim() != "")
        {
            InvoiceRemarks remarks = InvoiceRemarks.NewInvoiceRemarks();
            remarks.Remarks = txtAdminRemarks.Text;
            remarks.PayGroupId = payGroupId;
            remarks.DateTimeCreatedString = DateTime.Now.ToString();
            remarks.Save();

            txtAdminRemarks.Text = "";
            BindAdminRemarks();
        }
    }
    #endregion

    private void DisplayPriceAmounts()
    {
        AdhocInvoiceItemList adInvoiceItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(payGroupId,Guid.Empty);

        decimal subtotal = adInvoiceItemList.Sum(m => m.Amount) ;
        decimal fees = 0;

        if (rblPayment.SelectedValue == PaymentType.Cheque)
        {
            fees = 0;
            lbTotalFees.Text = subtotal.ToString("N");
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
        string url = ConfigurationManager.AppSettings["WebURL"] + "AdhocGateway.aspx?pgId=" + GeneralFunction.StringEncryption(payGroupId.ToString());

        Response.Redirect(url);

        //StringBuilder url = new StringBuilder();

        //string m_sPaypalBase = System.Configuration.ConfigurationManager.AppSettings["paypalBase"];
        //string business = System.Configuration.ConfigurationManager.AppSettings["paypalEmail"];
        //string cancelUrl = System.Configuration.ConfigurationManager.AppSettings["cancelPaymentUrl"];
        //string returnUrl = System.Configuration.ConfigurationManager.AppSettings["successPaymentUrl"];
        //string notifyUrl = System.Configuration.ConfigurationManager.AppSettings["notifyUrl"];

        //url.Append(m_sPaypalBase);
        //url.AppendFormat("&business={0}", HttpUtility.UrlEncode(business));
        //url.AppendFormat("&item_name={0}", HttpUtility.UrlEncode("Payment for APAC Effie Ad Hoc"));  // for: " + serials));
        //url.AppendFormat("&item_number={0}", HttpUtility.UrlEncode(payGroupId.ToString().Substring(0, 8).ToUpper())); // suppose to serial num, but there is possiblity of multiple entries per order????

        ////decimal amount = GeneralFunction.CalculateGroupTotalPriceFromCache();
        ////amount += GeneralFunction.CalculateCreditFees(amount); // Add fees for PP payment
        //decimal amount = GeneralFunction.TotalEntryGrandAmount(payGroupId);
        //url.AppendFormat("&amount={0}", HttpUtility.UrlEncode(amount.ToString("0.00")));
        ////For testing of $0.01 
        ////url.AppendFormat("&amount={0}", HttpUtility.UrlEncode("0.01"));
        //url.AppendFormat("&currency_code={0}", HttpUtility.UrlEncode("SGD"));

        //url.AppendFormat("&shipping={0}", HttpUtility.UrlEncode("0"));
        //url.AppendFormat("&no_shipping={0}", HttpUtility.UrlEncode("0"));
        //url.AppendFormat("&invoice={0}", "");

        //string custom = payGroupId.ToString();
        //url.AppendFormat("&custom={0}", HttpUtility.UrlEncode(custom));
        //url.AppendFormat("&cancel_return={0}", HttpUtility.UrlEncode(cancelUrl));
        //url.AppendFormat("&notify_url={0}", HttpUtility.UrlEncode(notifyUrl));
        //url.AppendFormat("&return={0}", HttpUtility.UrlEncode(returnUrl + "?custom=" + payGroupId.ToString()));

        //RawLog rawlog = RawLog.NewRawLog();
        //rawlog.Type = 0; //send
        //rawlog.Data = url.ToString();
        //rawlog.DateString = DateTime.Now.ToString();

        //if (rawlog.IsValid) rawlog.Save();


        //Response.Redirect(url.ToString());
    }


}