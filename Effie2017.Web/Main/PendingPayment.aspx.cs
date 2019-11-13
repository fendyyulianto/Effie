using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Main_PendingPayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnAddEntry_Click(object sender, EventArgs e)
    {
        Response.Redirect("../main/Entry.aspx");
    }
    protected void btnDashboard_Click(object sender, EventArgs e)
    {
        Response.Redirect("../main/Dashboard.aspx");
    }
    
}