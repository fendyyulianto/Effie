using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using Effie2017.App;

public partial class Controls_usr_Login : System.Web.UI.UserControl
{
    public string loginSuccessRedirection { get; set; }
    public string firstTimeLoginSuccessRedirection { get; set; }
    public bool forgetPasswordVisible { get; set; }
    public string forgetPasswordRedirection { get; set; }

    public string loginAppName
    {
        get
        {
            return ViewState["loginAppName"].ToString();
        }
        set
        {
            ViewState["loginAppName"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // temp diabled
        //if (Request.QueryString["a"] == "1") btnLogin.Visible = true;


        if (!Page.IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    protected void LoadForm()
    {
        if (forgetPasswordVisible)
        {
            lnkForgetPass.Visible = true;
            lnkForgetPass.NavigateUrl = forgetPasswordRedirection;
        }

        if (Request.Cookies["wdduw2013JuryLogin"] != null)
        {
            try
            {
                EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(GeneralFunction.GetValueGuid(Request.Cookies["wdduw2013JuryLogin"].Value, true));

                if (jury.IsActive)
                {
                    Security.SetLoginSessionJury(jury);                    

                    Response.Redirect(loginSuccessRedirection);
                }
            }
            catch
            {
                if (Request.Cookies["wdduw2013JuryLogin"] != null)
                    Response.Cookies["wdduw2013JuryLogin"].Expires = DateTime.Now.AddMinutes(-1);
            }
        }
    }

    protected void PopulateForm()
    {
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        if (!txtLoginId.Text.Trim().Equals("") && !txtPassword.Text.Trim().Equals(""))
        {
            EffieJuryManagementApp.JuryList juryList = EffieJuryManagementApp.JuryList.GetJuryListLogin(txtLoginId.Text.Trim(), txtPassword.Text.Trim());

            if (juryList.Count == 1)
            {
                if (juryList[0].IsActive)
                {
                    Security.SetLoginSessionJury(juryList[0]);

                    if (chkRememberMe.Checked)
                    {
                        Response.Cookies["wdduw2013JuryLogin"].Value = GeneralFunction.StringEncryption(juryList[0].Id.ToString());
                        Response.Cookies["wdduw2013JuryLogin"].Expires = DateTime.Now.AddYears(1);
                    }

                    // Enable this for Round 1 Jury
                    if (juryList[0].IsFirstTimeLogin)
                        Response.Redirect(firstTimeLoginSuccessRedirection);

                    Response.Redirect(loginSuccessRedirection);
                }
                else
                    lblMsg.Text = "Your account is disabled.<br>";
            }
            else
                lblMsg.Text = "Login and Password is invalid.<br>";
        }
        else
        {
            GeneralFunction.ValidateTextBox("Jury Id", txtLoginId, true, "string");
            GeneralFunction.ValidateTextBox("Password", txtPassword, true, "string");
            lblMsg.Text = "Login and Password is required.<br>";
        }
    }
}