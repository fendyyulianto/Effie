using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_ResendPaypal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        GeneralFunction.CompleteNewEntrySubmissionPayPal(new Guid(txtGroupId.Text));
    }

    protected void btnGo2_Click(object sender, EventArgs e)
    {
        Guid payGroupId = new Guid(txtGroupId.Text);
        Guid regId = Guid.Empty;
        EntryList list = EntryList.GetEntryList(payGroupId, Guid.Empty, ""); // contains the pay group id
        
        bool isChange = false;
        if (list[0].Invoice != "")
            isChange = true;

        string invoice = "";
        decimal totalamount = 0;
        foreach (Entry entry in list)
        {
            entry.Status = StatusEntry.UploadPending;
            entry.PayStatus = StatusPaymentEntry.Paid;
            if (!isChange)
            {
                if (entry.Serial.Trim() == "") entry.Serial = GeneralFunction.GetNewOrderNumber(entry);
                if (invoice == "") invoice = GeneralFunction.GetNewInvoiceNumber();
            }
            else
            {
                invoice = entry.Invoice;
            }
            entry.Invoice = invoice;
            //entry.DateSubmittedString = DateTime.Now.ToString();
            //entry.Save();
            regId = entry.RegistrationId;
            //totalamount += GetEntryPrice(entry);
        }

        // update again for the amount to the 1st entry
        list[0].AmountReceived = totalamount;
        //list[0].Save();


        // Save a new amount received
        AmountReceived amt = AmountReceived.NewAmountReceived();
        amt.Amount = totalamount;
        amt.DateReceivedString = DateTime.Now.ToString();
        amt.PaygroupId = payGroupId;
        amt.Invoice = invoice;
        amt.Remarks = "Received from Paypal";
        amt.IsSetPaid = true;
        amt.DateCreatedString = DateTime.Now.ToString();
        //amt.Save();



        //Send Email
        Email.SendCompletedPaymentEmailPayPalChange(Registration.GetRegistration(regId), payGroupId, invoice); // payment group id
        
        GeneralFunction.UpdateEntryLastSendPaidEmailDate(payGroupId);
    }
}