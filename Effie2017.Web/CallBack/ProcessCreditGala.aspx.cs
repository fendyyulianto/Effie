using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Order_ProcessCreditGala : System.Web.UI.Page
{
    string id;
    Guid orderId = Guid.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
       string id = Request.QueryString["custom"]; // order id
        //id="87E9FCB5-D517-459A-9279-3BF00B346124";
     //  id = Request.QueryString["Id"]; // order id
       
        if (id == null || id == "") return;

        try
        {
            orderId = new Guid(id);
        }
        catch { return; }

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
        else
        {
            //check for session exipration

        }
    }
    private void LoadForm()
    {
    }
    private void PopulateForm()
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
