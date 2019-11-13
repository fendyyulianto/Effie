using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class User_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (GeneralFunction.IsEntrantLoginCutOff() && !IsBackDoor())
        {
            phLogin.Visible = false;
            phCutoff.Visible = true;
            return;
        } 

        if (GeneralFunction.IsRegisterNewUserCutOff())
        {
            panelRegister.Visible = false;
        }
    }
    private bool IsBackDoor()
    {
        string bd = Request.QueryString["bd"];
        return (bd != null && bd == ConfigurationManager.AppSettings["BackdoorCode"]);
    }
    protected void btnSignup_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("../User/Profile.aspx");
    }
}