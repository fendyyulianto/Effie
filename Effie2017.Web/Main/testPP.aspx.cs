using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Main_testPP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid paygroupId = new Guid(Request.QueryString["pgId"]);
        string m = Request.QueryString["m"];

        if (m == "0") GeneralFunction.CompleteNewEntrySubmissionPayPal(paygroupId);

        if (m == "1") GeneralFunction.CompleteNewEntrySubmissionOthers(paygroupId);

        //string x = GeneralFunction.GetNewInvoiceNumber();
        //x = x;

    }
}