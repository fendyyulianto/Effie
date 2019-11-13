using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Admin_MasterPageAdmin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }
    protected void LoadForm()
    {
    }
    protected void PopulateForm()
    {
        Administrator admin = Security.GetAdminLoginSession();
        if (admin != null)
        {
            lblName.Text = admin.LoginId;
        }

        if (admin.Access == "AD3")
            liMenu7.Visible = false; ;

        DateTime lastlogin = Security.GetLastLoginCache();
        if (lastlogin != DateTime.MaxValue)
        {
            lblSign.Text = lastlogin.ToString("dd/MM/yy hh:mm tt");
        }
    }
    protected void lnkBtnLogout_Click(object sender, EventArgs e)
    {
        Security.ResetAllSessions();

        //if (Request.Cookies["wdduw2013UserLogin"] != null)
        //    Response.Cookies["wdduw2013UserLogin"].Expires = DateTime.Now.AddMinutes(-1);

        Response.Redirect("../Admin/Login.aspx");
    }
}
