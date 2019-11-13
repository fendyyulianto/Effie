using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using Telerik.Web.UI;

public partial class Admin_Controls_PaymentGala : System.Web.UI.UserControl
{
    public event EventHandler Save_Clicked;
    public event EventHandler Cancel_Clicked;

    public Guid GalaOrderId { get; set; }
    GalaOrder go = null;
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
        hldOrderId.Value = GalaOrderId.ToString();

        go = GalaOrder.GetGalaOrder(GalaOrderId);
        total = go.Amount + go.FeeShipping + go.Fee + go.Tax;
        lblInvoice.Text = go.Invoice;
        txtRemarks.Text = go.RemarksPayment;
        if (go.DatePaidString != "")
            dpDateReceived.SelectedDate = go.DatePaid;
        else
            dpDateReceived.SelectedDate = DateTime.Now;

        //BindGrid();
        lblAmount.Text = total.ToString("N");
        lblPrevReceived.Text = received.ToString("N");
        //lnkAmountReceived.NavigateUrl = "../AmountReceived.aspx?pgId=" + entry.PayGroupId.ToString();

        chkPaid.Checked = (go.PayStatus == StatusPaymentEntry.Paid);

        //string lastdatesent = GeneralFunction.CleanDateTimeToString(entry.LastSendPaidEmailDate, "dd/MM/yy HH:mm tt");
        //if (lastdatesent == "") lastdatesent = "-";
        //lbLastSendPaidEmailDate.Text = "(Last Sent:" + lastdatesent + ")";

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
        chkSendPaidEmail.Checked = false;
    }
    private void BindGrid()
    {
        //EntryList list = EntryList.GetEntryList(entry.PayGroupId, Guid.Empty, "");

        //// add to cache
        //GeneralFunction.ResetGroupPaymentCache();
        //foreach (Entry e in list)
        //{
        //    total += e.AmountPlusFee;
        //    received += e.AmountReceived;
        //}


        //radGridEntry.DataSource = list;
        //radGridEntry.Rebind();
    }
    private bool ValidateForm()
    {
        lbError.Text = "";
        lbError2.Text = "";

        //lbError.Text += GeneralFunction.ValidateTextBox("Amount Received", txtAmountRecieved, true, "decimal");
        lbError.Text += GeneralFunction.ValidateDatePick("Date Received", dpDateReceived, true);

        //// paid
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
            Guid goId = new Guid(hldOrderId.Value);
            go = GalaOrder.GetGalaOrder(goId);

            // save the amount to the first entry or this default entry
            //go.AmountReceived += decimal.Parse(txtAmountRecieved.Text);
            //go.Save();

            go.DatePaidString = dpDateReceived.DateInput.SelectedDate.ToString();
            go.RemarksPayment = txtRemarks.Text;

            // paid?
            bool isPaid = false;
            //Guid paymentGroupId = entry.PayGroupId;
            //EntryList list = EntryList.GetEntryList(paymentGroupId, Guid.Empty, "");
            //foreach (Entry ent in list)
            //{
                string paidstatus_org = go.PayStatus;

                // make the change only if the status has changed.
                // if not do not change it
                if (chkPaid.Checked && paidstatus_org == StatusPaymentEntry.NotPaid)
                {
                    go.PayStatus = StatusPaymentEntry.Paid;
                    isPaid = true;
                }
                if (!chkPaid.Checked && paidstatus_org == StatusPaymentEntry.Paid)
                {
                    go.PayStatus = StatusPaymentEntry.NotPaid;
                }
                go.Save();
            //}


            //// send email?
            //if (chkSendPaidEmail.Checked)
            //{
            //    if (go.PaymentMethod == PaymentType.PayPal) // there shouldnt be any PP, its disabled on the list, it can never come here
            //        GeneralFunction.CompleteNewGalaOrderPayPal(go.Id);
            //    else
            //        GeneralFunction.CompleteNewGalaOrderOthers(go.Id);
            //}


            // Always send email if paid
                if (isPaid) GeneralFunction.CompleteNewGalaOrderOthers(go.Id);



            //// history
            //AmountReceived amt = AmountReceived.NewAmountReceived();
            //amt.Amount = decimal.Parse(txtAmountRecieved.Text);
            //amt.DateReceivedString = dpDateReceived.DateInput.SelectedDate.ToString();
            //amt.PaygroupId = entry.PayGroupId;
            //amt.Invoice = "";
            //amt.Remarks = txtRemarks.Text;
            //amt.IsSetPaid = isPaid;
            //amt.DateCreatedString = DateTime.Now.ToString();
            //amt.Save();




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
    #endregion
}