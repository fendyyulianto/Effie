using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Testing_sendPPemail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid payGroupId = new Guid(Request.QueryString["ppId"]);



        // ok 
        Guid regId = Guid.Empty;
        EntryList list = EntryList.GetEntryList(payGroupId, Guid.Empty, ""); // contains the pay group id

        // validate if the entries already have an invoice
        if (list.Count == 0)
        {
            lbError.Text = "ERROR: 1";
            return;
        }


        string invoice = "";
        foreach (Entry entry in list)
        {
            invoice = entry.Invoice;
            regId = entry.RegistrationId;
        }


        //Send Email
        int rtn = Email.SendCompletedPaymentEmailPayPal(Registration.GetRegistration(regId), payGroupId, invoice); // payment group id
        GeneralFunction.UpdateEntryLastSendPaidEmailDate(payGroupId);

        lbError.Text = "OK. email code [" + rtn.ToString() + "]";
    }
}