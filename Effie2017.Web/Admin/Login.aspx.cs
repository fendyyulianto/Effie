using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();

        if (!IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                ViewState["Previuos_URL"] = Request.UrlReferrer.GetLeftPart(UriPartial.Query).ToString();
            }
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        AdministratorList adminList = AdministratorList.GetAdministratorList();
        lblError.Text = "";
        foreach (Administrator admin in adminList)
        {
            if ((admin.LoginId.Trim().ToUpper() == txtUserId.Text.Trim().ToUpper()) && admin.IsActive &&
                //(admin.Password.Trim() == GeneralFunction.CreateMD5(txtPassword.Text.Trim()))
                (admin.Password.Trim() == txtPassword.Text.Trim())
                )
            {
                try {
                    if (DateTime.Now > admin.LastChangePassword.AddMonths(3))
                    {
                        lblError.Text = "Login Expired";
                        return;
                    }
                } catch { }
                
                {
                    //save the last login
                    Security.SetLastLoginCache(admin.DateLastLogin);
                    admin.DateLastLoginString = DateTime.Now.ToString();
                    admin.Save();

                    string IPAddress = Page.Request.ServerVariables["REMOTE_ADDR"];
                    GeneralFunction.CreateLogPassword(admin, "Administrator", IPAddress);

                    Security.SetAdminLoginSession(admin);

                    //if (ViewState["Previuos_URL"] != null)
                    //{
                    //    object refUrl = ViewState["Previuos_URL"];
                    //    if (refUrl != null)
                    //    {                        
                    //        Response.Redirect((string)refUrl);
                    //    }
                    //}
                    //else
                    //    Response.Redirect("../Admin/EntryList.aspx");

                    Response.Redirect("../Admin/EntryList.aspx");
                }
            }
        }



        //if (txtUserId.Text.Trim() == System.Configuration.ConfigurationManager.AppSettings["AdminUserId"] && txtPassword.Text == System.Configuration.ConfigurationManager.AppSettings["AdminPassword"])
        //{
        //    Security.SetAdminLoginSession(System.Configuration.ConfigurationManager.AppSettings["AdminUserId"]);
        //    Response.Redirect("../Admin/EntryList.aspx");
        //}
    }
}