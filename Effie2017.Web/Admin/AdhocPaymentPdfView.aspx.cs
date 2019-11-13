using System;

using System.IO;
using Effie2017.App;


public partial class Admin_AdhocPaymentPdfView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Effie2017.App.Registration registration = (Effie2017.App.Registration)Session["registrationForPdfView"];
        //Effie2017.App.Entry entry = (Effie2017.App.Entry)Session["entryForPdfView"];

        if (Request["regId"] == null && Request["adId"] == null)
            return;

        AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(GeneralFunction.GetValueGuid(Request["adId"].ToString(), true));
        Registration registration = Registration.GetRegistration(GeneralFunction.GetValueGuid(Request["regId"].ToString(), true));

        if (registration == null || adInv == null)
            return;

        
        MemoryStream memoryStream = null;

        // invoice
        memoryStream = RegAttachment.GenerateAdhocReceipt(registration, adInv.PayGroupId);

        Response.ContentType = "application/pdf";
        //Response.AddHeader("Content-Disposition", "attachment; filename=Invoice.pdf");
        memoryStream.WriteTo(Response.OutputStream);
        memoryStream.Close();
    }
}