using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;

public partial class Testing_Webrequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebRequest request = WebRequest.Create("http://localhost/Effie2017.Web/Testing/ErrorTest.aspx");
        WebResponse responseItem = request.GetResponse();
    }
}