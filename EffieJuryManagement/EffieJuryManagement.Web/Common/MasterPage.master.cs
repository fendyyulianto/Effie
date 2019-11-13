using System;
using System.Web.UI;
using EffieJuryManagementApp;

public partial class Common_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    private void LoadForm()
    {

    }

    private void PopulateForm()
    {
        Administrator admin = Security.GetAdminLoginSession();
        if (admin != null)
        {
            lblName.Text = admin.Name;
            lblSign.Text = admin.DateLastLogin.ToString("dd MMM yyyy hh:mm tt");
        }
    }

    protected void lnkBtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Login/Login.aspx");
    }

  
}
