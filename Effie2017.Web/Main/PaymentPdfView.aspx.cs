using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

public partial class Main_PaymentPdfView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        //Effie2017.App.Registration registration = (Effie2017.App.Registration)Session["registrationForPdfView"];
        //Effie2017.App.Entry entry = (Effie2017.App.Entry)Session["entryForPdfView"];

        if (Request["id"] == null)
            Response.Redirect("./PaymentList.aspx");

        Effie2017.App.Registration registration = Security.GetLoginSessionUser();
        Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(GeneralFunction.GetValueGuid(Request["id"].ToString(), true));

        if (registration == null || entry == null)
            Response.Redirect("./PaymentList.aspx");

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