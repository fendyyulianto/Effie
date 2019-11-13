using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Effie2017.App;
using System.Configuration;
using System.Text;
using System.Linq;

public partial class Admin_AdhocInvoiceSummary : PageSecurity_Admin
{
    List<Guid> list = null;
    int counter;
    Guid payGroupId;
    bool isAdminEdit = false;
    static Entry entrySelected = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        isAdminEdit = (Request.QueryString["adm"] == "e");
        payGroupId = new Guid(GeneralFunction.StringDecryption(Request.QueryString["pgId"]));
        if (!string.IsNullOrEmpty(Request.QueryString["EntryId"]))
        {
            try
            {
                entrySelected = Entry.GetEntry(new Guid(GeneralFunction.StringDecryption(Request.QueryString["EntryId"])));
            }
            catch
            {
                entrySelected = null;
            }
        }
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
            //Security.SecureControlByHiding(btnSubmit);
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

        //if (entrySelected != null)
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

            if (rblPayment.SelectedValue != PaymentType.PayPal)
                adInv.PaymentMethod = rblPayment.SelectedValue;

            if(!string.IsNullOrEmpty(adInv.PaymentMethod))
                adInv.InvoiceDateString = DateTime.Now.ToString();

            adInv.Amount = 0;
            adInv.Tax = 0;
            adInv.Fee = 0;


            foreach (AdhocInvoiceItem adInvoiceItem in adInvoiceItemList)
            {               
                adInvoiceItem.GrandAmount = adInvoiceItem.Amount;

                adInvoiceItem.Save();

                adInv.Amount += adInvoiceItem.Amount;
            }

            decimal subtotal = adInv.Amount;

            if (rblPayment.SelectedValue == PaymentType.PayPal) adInv.Fee = GeneralFunction.CalculateCreditFees(subtotal);
            if (rblPayment.SelectedValue == PaymentType.BankTransfer) adInv.Fee = GeneralFunction.CalculateBankTransferFees();
            //entry.Tax = 0;
            if (ddlCountry.SelectedValue.ToLower() == "singapore") adInv.Tax = GeneralFunction.CalculateTax(subtotal + adInv.Fee);

            adInv.GrandAmount = adInv.Amount + adInv.Fee + adInv.Tax;

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
            Label lbSerialNo = (Label)e.Item.FindControl("lbSerialNo");
            if (lbSerialNo != null)
            {
                lbSerialNo.Text = entry.Serial;
            }
            Label lblTitle = (Label)e.Item.FindControl("lblTitle");
            if (lblTitle != null)
            {
                lblTitle.Text = entry.Campaign;
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

            if (adInv != null)
            {
                GeneralFunction.CompletePendingPaymentAdhoc(adInv.RegistrationId, adInv.PayGroupId, adInv.PaymentMethod, lbTotalFees.Text, false);

                //if (entrySelected != null)
                {
                    Administrator admin = Security.GetAdminLoginSession();
                    AdhocInvoiceItemList adhocInvItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(payGroupId, adInv.Id);
                    EntryList entries = EntryList.GetEntryList(Guid.Empty, adInv.RegistrationId, "");
                    foreach (AdhocInvoiceItem adhocInvItem in adhocInvItemList)
                    {
                        try {
                            //FILTER For REOPEN ENTRY PROCESSING 
                            Entry entry = entries.FirstOrDefault(x => x.Id == adhocInvItem.EntryId &&
                            ((x.Status == StatusEntry.Completed) && (x.ProcessingStatus == StatusEntry.PendingVerification)));

                            if (adhocInvItem.InvoiceType == "ReOpen")
                            {
                                //To set to Upload Complete if Payment Method is selected
                                if (adInv.Invoice == "" && adInv.PaymentMethod != "")
                                {
                                    entry.Status = StatusEntry.UploadCompleted;
                                    entry.ReopenedBy = admin.Id.ToString();
                                    entry.ProcessingStatus = StatusEntry.Reopened;
                                    entry.IDAdhocInvoice = adhocInvItem.AdhocInvoiceId.ToString();
                                }
                                else
                                {
                                    entry.ReopenedBy = admin.Id.ToString();
                                    entry.ProcessingStatus = StatusEntry.PendingReopen;
                                    entry.IDAdhocInvoice = adhocInvItem.AdhocInvoiceId.ToString();
                                }
                                
                                entry.Save();
                            }
                        }
                        catch { }
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["Page"]) && (entrySelected != null))
                    {
                        Response.Redirect("EntryProcessing.aspx?Page=Management");
                    }
                    else if (entrySelected != null)
                    {
                        Response.Redirect("EntryProcessing.aspx");
                    }
                    else
                    {
                        Response.Redirect("../Admin/AdhocInvoiceList.aspx");
                    }
                }
            }
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        AdhocInvoiceList adInvList = AdhocInvoiceList.GetAdhocInvoiceList(Guid.Empty, payGroupId);

        if (String.IsNullOrEmpty(GeneralFunction.GetRedirect("")))
        {
            if(entrySelected != null && adInvList.Count > 0)
                Response.Redirect("../Admin/AdhocInvoice.aspx?regId=" + GeneralFunction.StringEncryption(adInvList[0].RegistrationId.ToString()) + "&EntryId=" + GeneralFunction.StringEncryption(entrySelected.Id.ToString()));
            else if (adInvList.Count > 0)
                Response.Redirect("../Admin/AdhocInvoice.aspx?regId=" + GeneralFunction.StringEncryption(adInvList[0].RegistrationId.ToString()));
            else
                Response.Redirect(GeneralFunction.GetRedirect("../Admin/AdhocInvoiceList.aspx"));
        }
        else
            Response.Redirect(GeneralFunction.GetRedirect("../Admin/AdhocInvoiceList.aspx"));
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
}