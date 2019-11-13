using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Effie2017.App;

public partial class Admin_Controls_PaymentAdhoc : System.Web.UI.UserControl
{
    public event EventHandler Save_Clicked;
    public event EventHandler Cancel_Clicked;

    public Guid PayGroupId { get; set; }
    AdhocInvoice adInv = null;
    decimal total = 0;
    decimal received = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Access Control
        Security.SecureControlByHiding(btnSubmit);
    }
    private void LoadForm()
    {
    }
    public void PopulateForm()
    {
        ResetForm();

        // save the entryid
        hldEntryId.Value = PayGroupId.ToString();

        AdhocInvoiceList adInvList = AdhocInvoiceList.GetAdhocInvoiceList(Guid.Empty,PayGroupId);

        adInv = AdhocInvoice.GetAdhocInvoice(adInvList[0].Id);


        lblInvoice.Text = adInv.Invoice;

        BindGrid();
        lblAmount.Text = adInv.GrandAmount.ToString("N");
        //lblAmount.Text = entry.GrandAmount.ToString("N");
        lblPrevReceived.Text = adInv.AmountReceived.ToString("N");
        lnkAmountReceived.NavigateUrl = "../AdhocAmountReceived.aspx?pgId=" + adInv.PayGroupId.ToString();

        chkPaid.Checked = (adInv.PayStatus == StatusPaymentEntry.Paid);
        chkPaid.Enabled = !chkPaid.Checked;

        //chkAllowUpload.Checked = (entry.Status == StatusEntry.UploadPending);
        //chkAllowUpload.Enabled = !chkPaid.Checked;

        // if the amt is less than invoice amt, enable back the check boxes
        if (!IsAmountFullyPaidPrevious())
        {
            chkPaid.Enabled = true;
            //chkAllowUpload.Enabled = true;
        }


        string lastdatesent = GeneralFunction.CleanDateTimeToString(adInv.LastSendPaidEmailDate, "dd/MM/yy HH:mm tt");
        if (lastdatesent == "") lastdatesent = "-";
        lbLastSendPaidEmailDate.Text = "(Last Sent:" + lastdatesent + ")";



        // Dedfault date
        dpDateReceived.SelectedDate = DateTime.Now;
    }
    public void ResetForm()
    {
        lbError.Text = "";
        lbError2.Text = "";
        txtAmountRecieved.Text = "";
        txtRemarks.Text = "";
        dpDateReceived.Clear();
        dpDateReceived.DateInput.Clear();
        chkPaid.Checked = false;
        //chkSendPaidEmail.Checked = false;
    }
    private void BindGrid()
    {
        AdhocInvoiceItemList list = AdhocInvoiceItemList.GetAdhocInvoiceItemList(adInv.PayGroupId, adInv.Id);

       
        radGridEntry.DataSource = list;
        radGridEntry.Rebind();
    }
    private bool ValidateForm()
    {
        lbError.Text = "";
        lbError2.Text = "";
        GeneralFunction.RemoveHighlightControls(this);

        lbError.Text += GeneralFunction.ValidateTextBox("Amount Received", txtAmountRecieved, true, "decimal");
        lbError.Text += GeneralFunction.ValidateDatePick("Date Received", dpDateReceived, true);

        // paid
        //if (chkSendPaidEmail.Checked && !chkPaid.Checked)
        //{
        //    GeneralFunction.HighlightControl(chkSendPaidEmail);
        //    GeneralFunction.HighlightControl(chkPaid);
        //    lbError.Text += "Paid must be selected in order to send paid email.<br/>";
        //} 

        lbError2.Text = lbError.Text;
        return (lbError.Text == "");
    }

    #region Events
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            Guid paygroupId = new Guid(hldEntryId.Value);
            string InvoiceType = "";
            AdhocInvoiceList adInvList = AdhocInvoiceList.GetAdhocInvoiceList(Guid.Empty, paygroupId);
            adInv = AdhocInvoice.GetAdhocInvoice(adInvList[0].Id);
            Registration reg = null;
            string invoice = "";
            try
            {
                AdhocInvoiceItem adhocInvoiceItem = AdhocInvoiceItemList.GetAdhocInvoiceItemList(adInvList[0].PayGroupId, adInvList[0].Id)[0];
                InvoiceType = adhocInvoiceItem.InvoiceType;
                reg = Registration.GetRegistration(adInvList[0].RegistrationId);
                invoice = adInvList[0].Invoice;
            }
            catch { }

            // save the amount to the first entry or this default entry
            adInv.AmountReceived += decimal.Parse(txtAmountRecieved.Text);
            adInv.Save();


            // paid?
            bool isPaid = false;
            Guid paymentGroupId = adInv.PayGroupId;
            string paidstatus_org = adInv.PayStatus;

            if (chkPaid.Checked && paidstatus_org == StatusPaymentEntry.NotPaid)
            {
                adInv.PayStatus = StatusPaymentEntry.Paid;     
                isPaid = true;
                adInv.Save();

                if (!string.IsNullOrEmpty(invoice))// AD-HOC INVOICE – CONFIRMATION OF PAYMENT
                {
                    if (InvoiceType == AdhocInvoiceType.ReOpen)
                        Email.SendAdhocReOpenPaymentEmailConfirm(reg, paygroupId, invoice);
                    else if (InvoiceType == AdhocInvoiceType.ChangeReq || InvoiceType == AdhocInvoiceType.Custom || InvoiceType == AdhocInvoiceType.ExtDeadLine)
                        Email.SendAdhocOtherRequestPaymentEmailConfirm(reg, paygroupId, invoice);

                    adInv.LastSendPaidEmailDateString = DateTime.Now.ToString();
                }
            }
            if (!chkPaid.Checked && paidstatus_org == StatusPaymentEntry.Paid)
            {
                adInv.PayStatus = StatusPaymentEntry.NotPaid;
            }

            adInv.Save();

            


            // send email? if sent before do not send any more
            string lastdatesent = GeneralFunction.CleanDateTimeToString(adInv.LastSendPaidEmailDate, "dd/MM/yy HH:mm tt");
            //if (lastdatesent == "")
            //{
            //    if (adInv.PaymentMethod != PaymentType.PayPal)
            //    {
            //        if (IsAmountFullyPaidIncludeThisAmount())
            //            // Full payment
            //            GeneralFunction.CompleteNewEntrySubmissionOthers(paymentGroupId);
            //        else
            //        {
            //            // Partial payment
            //            Email.SendAllowUploadEmailOthers(Registration.GetRegistration(list[0].RegistrationId), paymentGroupId, "");
            //            GeneralFunction.UpdateEntryLastSendPaidEmailDate(paymentGroupId);
            //        }
            //    }
            //}


            Administrator admin = Security.GetAdminLoginSession();
            // history
            AdhocInvoiceAmountReceived amt = AdhocInvoiceAmountReceived.NewAdhocInvoiceAmountReceived();
            amt.Amount = decimal.Parse(txtAmountRecieved.Text);
            amt.DateReceivedString = dpDateReceived.DateInput.SelectedDate.ToString();
            amt.PaygroupId = adInv.PayGroupId;
            amt.Invoice = adInv.Invoice;
            amt.Remarks = txtRemarks.Text;
            amt.IsSetPaid = isPaid;
            amt.DateCreatedString = DateTime.Now.ToString();
            if (admin != null)
            {
                amt.isAdmin = true;
                amt.CommentatorID = admin.Id;
            }
            else
            {
                amt.isAdmin = false;
                amt.CommentatorID = reg.Id;
            }
            amt.Save();




            if (Save_Clicked != null) Save_Clicked(this, EventArgs.Empty);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        GeneralFunction.RemoveHighlightControls(this);

        if (Cancel_Clicked != null) Cancel_Clicked(this, EventArgs.Empty);
    }
    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.AdhocInvoiceItem adInvItem = (Effie2017.App.AdhocInvoiceItem)e.Item.DataItem;
            Entry entry = Entry.GetEntry(adInvItem.EntryId);


            Label lbl = null;

            // serial
            lbl = (Label)e.Item.FindControl("lblSerial");
            lbl.Text = entry.Serial;

            // desc
            lbl = (Label)e.Item.FindControl("lblDesc");
            lbl.Text = adInvItem.InvoiceType.Equals(AdhocInvoiceType.Custom) ? adInvItem.InvoiceTypeOthers : GeneralFunction.GetInvoiceType(adInvItem.InvoiceType);


            // submitted by
            lbl = (Label)e.Item.FindControl("lblSubmittedBy");
            Registration reg = Registration.GetRegistration(entry.RegistrationId);
            lbl.Text = "-";
            if (reg != null) lbl.Text = GeneralFunction.GetRegistrationFromEntry(entry).Company;

        }
    }
    protected void txtAmountRecieved_OnTextChanged(object sender, EventArgs e)
    {
        GeneralFunction.RemoveHighlightControls(this);
        lbError.Text = GeneralFunction.ValidateTextBox("Amount Received", txtAmountRecieved, true, "decimal");

        if (lbError.Text == "")
        {
            if (IsAmountFullyPaidIncludeThisAmount())
            {
                chkPaid.Enabled = false;
                //chkAllowUpload.Enabled = false;

                chkPaid.Checked = true;
                //chkAllowUpload.Checked = true;
            }
            else
            {
                chkPaid.Enabled = true;
                chkPaid.Checked = false;
                //chkAllowUpload.Enabled = true;
            }
        }
    }
    private bool IsAmountFullyPaidIncludeThisAmount()
    {
        decimal invamt = Decimal.Parse(lblAmount.Text);
        decimal prevamt = Decimal.Parse(lblPrevReceived.Text);
        decimal thisamt = Decimal.Parse(txtAmountRecieved.Text);

        if (prevamt + thisamt >= invamt) return true;

        return false;
    }
    private bool IsAmountFullyPaidPrevious()
    {
        decimal invamt = Decimal.Parse(lblAmount.Text);
        decimal prevamt = Decimal.Parse(lblPrevReceived.Text);

        if (prevamt >= invamt) return true;

        return false;
    }
    #endregion
}