using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;

public partial class Login_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();

        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
                ViewState["Previous_URL"] = Request.UrlReferrer.GetLeftPart(UriPartial.Query).ToString();
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        AdministratorList adminList = AdministratorList.GetAdministratorList();

        foreach (Administrator admin in adminList)
        {
            if (admin.LoginId.Trim().ToUpper() == txtUserId.Text.Trim().ToUpper() && admin.IsActive &&
                admin.Password.Trim().ToUpper() == txtPassword.Text.Trim().ToUpper())
            {
                //save the last login
                Security.SetLastLoginCache(admin.DateLastLogin);
                admin.DateLastLoginString = DateTime.Now.ToString();
                admin.Save();


                Security.SetAdminLoginSession(admin);

                if (ViewState["Previous_URL"] != null)
                {
                    string referrerURL = string.Empty;
                    referrerURL = (string)ViewState["Previous_URL"];

                    Response.Redirect(referrerURL);
                }
                else
                    Response.Redirect("../Main/JuryList.aspx");
            }
        }        
    }
}