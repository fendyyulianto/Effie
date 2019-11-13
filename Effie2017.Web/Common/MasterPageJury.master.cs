using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Common_MasterPageJury : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.Url.ToString().ToLower().Contains("/jury/") && !Security.IsJuryLogin())
            Response.Redirect("../Jury/Login.aspx");
    }

    protected void LoadForm()
    {
        EffieJuryManagementApp.Jury jury = Security.GetLoginSessionJury();
        if (jury != null)
        {
            lblJuryName.Text = jury.FirstName + " " + jury.LastName;
            

            //if (registration.LastSignIn.ToString("yyyyMMdd") != "99991231")
            //    lblSign.Text = registration.LastSignIn.ToString("dd MMM yyyy hh:mm tt");
        }
    }

    protected void PopulateForm()
    {
    }

    protected void lnkBtnLogout_Click(object sender, EventArgs e)
    {
        //bool isAdminSpoof = Security.IsUserAdminSpoof();

        Security.ResetAllSessions();

        if (Request.Cookies["wdduw2013JuryLogin"] != null)
            Response.Cookies["wdduw2013JuryLogin"].Expires = DateTime.Now.AddMinutes(-1);

        //if (isAdminSpoof)
        //    Response.Redirect("../Admin/Login.aspx");
        //else
            Response.Redirect("../Jury/Login.aspx");
    }

    public void ShowUser()
    {
        tblUser.Visible = true;
    }

    public void ShowNav()
    {
        ulNav.Visible = true;
    }

    public void SetConfirmLogout()
    {
        //lnkBtnLogout.Attributes.Add("onclick", "return confirm('Please save before logging out. Confirm to logout?');");
        lnkBtnLogout.OnClientClick = "return confirm('Please save before logging out. Confirm to logout?');";
    }

    public void SetCompatibility()
    {
        ltrForCompatibility.Text = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=7\">";
    }
}
