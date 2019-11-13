using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Effie2017.App;

public partial class Admin_Controls_Payment : System.Web.UI.UserControl
{
    public event EventHandler Save_Clicked;
    public event EventHandler Cancel_Clicked;

    public Guid EntryId { get; set; }
    Entry entry = null;
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
        hldEntryId.Value = EntryId.ToString();

        entry = Entry.GetEntry(EntryId);


        lblInvoice.Text = entry.Invoice;

        BindGrid();
        lblAmount.Text = total.ToString("N");
        //lblAmount.Text = entry.GrandAmount.ToString("N");
        lblPrevReceived.Text = received.ToString("N");
        lnkAmountReceived.NavigateUrl = "../AmountReceived.aspx?pgId=" + entry.PayGroupId.ToString();

        try
        {
            if ((entry.PayStatus == StatusPaymentEntry.Paid))
                rblPayment.SelectedValue = entry.PayStatus;
            else if ((entry.Status == StatusEntry.UploadPending))
                rblPayment.SelectedValue = "AllowUpload";
        }
        catch { }

        //TODOOO
        //////chkPaid.Checked = (entry.PayStatus == StatusPaymentEntry.Paid);
        //////chkPaid.Enabled = !chkPaid.Checked;

        //////chkAllowUpload.Checked = (entry.Status == StatusEntry.UploadPending);
        //////chkAllowUpload.Enabled = !chkPaid.Checked;

        //////// if the amt is less than invoice amt, enable back the check boxes
        //////if (!IsAmountFullyPaidPrevious())
        //////{
        //////    chkPaid.Enabled = true;
        //////    chkAllowUpload.Enabled = true;
        //////}


        string lastdatesent = GeneralFunction.CleanDateTimeToString(entry.LastSendPaidEmailDate, "dd/MM/yy HH:mm tt");
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
        rblPayment.ClearSelection();
    }
    private void BindGrid()
    {
        EntryList list = EntryList.GetEntryList(entry.PayGroupId, Guid.Empty, "");

        // add to cache
        GeneralFunction.ResetGroupPaymentCache();
        foreach (Entry e in list)
        {
            //total += e.AmountPlusFee;
            total += e.GrandAmount;
            received += e.AmountReceived;
        }


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
            Guid entryId = new Guid(hldEntryId.Value);
            entry = Entry.GetEntry(entryId);

            // save the amount to the first entry or this default entry
            entry.AmountReceived += decimal.Parse(txtAmountRecieved.Text);
            entry.Save();


            // paid?
            bool isPaid = false;
            Guid paymentGroupId = entry.PayGroupId;
            EntryList list = EntryList.GetEntryList(paymentGroupId, Guid.Empty, "");
            foreach (Entry ent in list)
            {
                string paidstatus_org = ent.PayStatus;

                // Allow Upload - only if its not aleady pending complete or completed
                if (rblPayment.SelectedValue == "AllowUpload" && ent.Status != StatusEntry.UploadCompleted && ent.Status != StatusEntry.Completed)
                {
                    //if (rblPayment.SelectedValue == "AllowUpload")
                    //{
                        ent.Status = StatusEntry.UploadPending;
                    //}
                    //else
                    //{
                    //    ent.Status = StatusEntry.PaymentPending;
                    //}
                }
                
                // make the change only if the status has changed.
                // if not do not change it
                if (rblPayment.SelectedValue == StatusPaymentEntry.Paid /*chkPaid.Checked*/ && paidstatus_org == StatusPaymentEntry.NotPaid)
                {
                    ent.PayStatus = StatusPaymentEntry.Paid;

                    // set if its not aleady pending complete or completed
                    if (ent.Status != StatusEntry.UploadCompleted && ent.Status != StatusEntry.Completed) ent.Status = StatusEntry.UploadPending;

                    isPaid = true;
                }

                //if (paidstatus_org == StatusPaymentEntry.Paid)
                //{
                //    ent.PayStatus = StatusPaymentEntry.NotPaid;
                //}


                // double check if paid then it must be allow upload
                if (ent.PayStatus == StatusPaymentEntry.Paid && ent.Status != StatusEntry.UploadCompleted && ent.Status != StatusEntry.Completed)
                    ent.Status = StatusEntry.UploadPending;
                
                ent.Save();
            }


            // send email? if sent before do not send any more
            string lastdatesent = GeneralFunction.CleanDateTimeToString(entry.LastSendPaidEmailDate, "dd/MM/yy HH:mm tt");
            //if (chkAllowUpload.Checked && lastdatesent == "")
            if (lastdatesent == "")
            {
                if (entry.PaymentMethod != PaymentType.PayPal)
                {
                    //if (IsAmountFullyPaidIncludeThisAmount())
                    if (rblPayment.SelectedValue == StatusPaymentEntry.Paid || IsAmountFullyPaidIncludeThisAmount())
                        // Full payment
                        GeneralFunction.CompleteNewEntrySubmissionOthers(paymentGroupId);
                    else if (rblPayment.SelectedValue == "AllowUpload")
                    {
                        // Partial payment
                        Email.SendAllowUploadEmailOthers(Registration.GetRegistration(list[0].RegistrationId), paymentGroupId, "");
                        GeneralFunction.UpdateEntryLastSendPaidEmailDate(paymentGroupId);
                    }
                }
            }


            Administrator admin = Security.GetAdminLoginSession();
            Registration reg = Security.GetLoginSessionUser();
            // history
            AmountReceived amt = AmountReceived.NewAmountReceived();
            amt.Amount = decimal.Parse(txtAmountRecieved.Text);
            DateTime datetime = DateTime.Parse(dpDateReceived.DateInput.SelectedDate.ToString());
            string Date = datetime.ToString("MM/dd/yyyy");
            string Time = DateTime.Now.ToString("hh:mm tt");
            DateTime DateReceived = DateTime.Parse(Date + " " + Time);
            amt.DateReceivedString = DateReceived.ToString();
            amt.PaygroupId = entry.PayGroupId;
            amt.Invoice = "";
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
            Effie2017.App.Entry entry = (Effie2017.App.Entry)e.Item.DataItem;

            Label lbl = null;

            // submitted by
            lbl = (Label)e.Item.FindControl("lblSubmittedBy");
            Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
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
                // rblPayment.Enabled = false;
                rblPayment.SelectedValue = StatusPaymentEntry.Paid;
                //chkPaid.Enabled = false;
                //chkAllowUpload.Enabled = false;

                //chkPaid.Checked = true;
                //chkAllowUpload.Checked = true;
            }
            else
            {
                rblPayment.ClearSelection();
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