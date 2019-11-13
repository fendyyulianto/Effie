using System;

using System.IO;
using Effie2017.App;

public partial class Main_PaymentPdfView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Effie2017.App.Registration registration = (Effie2017.App.Registration)Session["registrationForPdfView"];
        //Effie2017.App.Entry entry = (Effie2017.App.Entry)Session["entryForPdfView"];

        if (Request["id"] == null)
            return;

        Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(GeneralFunction.GetValueGuid(Request["id"].ToString(), true));
        Registration registration = GeneralFunction.GetRegistrationFromEntry(entry);

        if (registration == null || entry == null)
            return;

        Session["registrationForPdfView"] = null;
        Session["entryForPdfView"] = null;

        MemoryStream memoryStream = null;

        memoryStream = RegAttachment.GenerateGroupReceipt(registration, entry.PayGroupId);

        Response.ContentType = "application/pdf";
        //Response.AddHeader("Content-Disposition", "attachment; filename=Invoice.pdf");
        memoryStream.WriteTo(Response.OutputStream);
        memoryStream.Close();
    }
}