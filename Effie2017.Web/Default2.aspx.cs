using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //List<string> invoiceList = new List<string>();
        ////invoiceList.Add("AE17/13032"); //DEMO
        ////invoiceList.Add("AE17/13118"); //DEMO
        ////invoiceList.Add("AE17/13263"); //DEMO
        ////invoiceList.Add("AE17/13102"); //DEMO

        ////invoiceList.Add("AE18/13101");
        ////invoiceList.Add("AE18/13099");
        ////invoiceList.Add("AE18/13102");
        ////invoiceList.Add("AE18/13100");

        //invoiceList.Add("AE18/13054"); //SendNewEntryPendingPaymentBankTransferEmail
        //invoiceList.Add("AE18/13053"); //SendNewEntryPendingPaymentBankTransferEmail
        //invoiceList.Add("AE18/13051"); //SendNewEntryPendingPaymentBankTransferEmail
        //invoiceList.Add("AE18/13050"); //SendNewEntryPendingPaymentBankTransferEmail
        //invoiceList.Add("AE18/13049"); //SendNewEntryPendingPaymentBankTransferEmail
        //invoiceList.Add("AE18/13048"); //SendNewEntryPendingPaymentBankTransferEmail
        //invoiceList.Add("AE18/13032"); //SendNewEntryPendingPaymentBankTransferEmail
        //invoiceList.Add("AE18/13031"); //SendNewEntryPendingPaymentBankTransferEmail
        
        //foreach (string invoice in invoiceList)
        //{
        //    try
        //    {
        //        Entry entry = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "").Where(x => x.Invoice == invoice).FirstOrDefault();
        //        if (entry != null)
        //        {
        //            Email.SendNewEntryPendingPaymentBankTransferEmail(Registration.GetRegistration(entry.RegistrationId), entry.PayGroupId, invoice);
        //            lblinvoice.Text += invoice + " - SendNewEntryPendingPaymentBankTransferEmail<br>";
        //        }

        //    }
        //    catch { }
        //}
        //lblinvoice.Text += "<br><br><br>";
        //invoiceList.Clear();

        //invoiceList.Add("AE18/13052"); //SendCompletedPaymentEmailPayPal


        //foreach (string invoice in invoiceList)
        //{
        //    try
        //    {
        //        Entry entry = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "").Where(x => x.Invoice == invoice).FirstOrDefault();
        //        if (entry != null)
        //        {
        //            Email.SendCompletedPaymentEmailPayPal(Registration.GetRegistration(entry.RegistrationId), entry.PayGroupId, invoice);
        //            lblinvoice.Text += invoice + " - SendCompletedPaymentEmailPayPal<br>";
        //        }

        //    }
        //    catch { }
        //}

        //lblinvoice.Text += "<br><br><br>";
        //invoiceList.Clear();

        //invoiceList.Add("AE18/13047"); //SendNewEntryPendingPaymentChequeEmail

        //foreach (string invoice in invoiceList)
        //{
        //    try
        //    {
        //        Entry entry = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "").Where(x => x.Invoice == invoice).FirstOrDefault();
        //        if (entry != null)
        //        {
        //            Email.SendNewEntryPendingPaymentChequeEmail(Registration.GetRegistration(entry.RegistrationId), entry.PayGroupId, invoice);
        //            lblinvoice.Text += invoice + " - SendNewEntryPendingPaymentChequeEmail<br>";
        //        }

        //    }
        //    catch { }
        //}

        //lblinvoice.Text += "<br><br><br>";
        //invoiceList.Clear();
    }


    //REPORT
    //AE18/13054 - SendNewEntryPendingPaymentBankTransferEmail
    //AE18/13053 - SendNewEntryPendingPaymentBankTransferEmail
    //AE18/13051 - SendNewEntryPendingPaymentBankTransferEmail
    //AE18/13050 - SendNewEntryPendingPaymentBankTransferEmail
    //AE18/13049 - SendNewEntryPendingPaymentBankTransferEmail
    //AE18/13048 - SendNewEntryPendingPaymentBankTransferEmail
    //AE18/13032 - SendNewEntryPendingPaymentBankTransferEmail
    //AE18/13031 - SendNewEntryPendingPaymentBankTransferEmail
    //AE18/13052 - SendCompletedPaymentEmailPayPal
    //AE18/13047 - SendNewEntryPendingPaymentChequeEmail

}