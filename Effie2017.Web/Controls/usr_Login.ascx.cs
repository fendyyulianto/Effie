using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using Effie2017.App;
using System.Configuration;

public partial class Controls_usr_Login : System.Web.UI.UserControl
{
    public string loginSuccessRedirection { get; set; }
    public bool forgetPasswordVisible { get; set; }
    public string forgetPasswordRedirection { get; set; }
    public static int FailedLoginCount { get; set; }

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
        if (!Page.IsPostBack)
        {
            FailedLoginCount = 0;
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

        if (Request.Cookies["wdduw2013UserLogin"] != null)
        {
            try
            {
                Effie2017.App.Registration registration = Effie2017.App.Registration.GetRegistration(GeneralFunction.GetValueGuid(Request.Cookies["wdduw2013UserLogin"].Value, true));

                if (registration.LastSignIn2.ToString("yyyyMMdd") != "99991231")
                {
                    registration.LastSignInString = registration.LastSignIn2.ToString();
                }

                registration.LastSignIn2String = DateTime.Now.ToString();
                registration.Save();

                Security.SetLoginSessionUser(registration);
                Response.Redirect(loginSuccessRedirection);
            }
            catch
            {
                if (Request.Cookies["wdduw2013UserLogin"] != null)
                    Response.Cookies["wdduw2013UserLogin"].Expires = DateTime.Now.AddMinutes(-1);
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
            string MasterKeyUser = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("MasterKeyUser")[0].Value;
            //string MasterKeyUser = ConfigurationManager.AppSettings["MasterKeyUser"].ToString();
            List<Registration> registrationList = Effie2017.App.RegistrationList.GetRegistrationList(txtLoginId.Text, "", StatusRegistration.OK).ToList();
            bool isBackDoor = (MasterKeyUser == GeneralFunction.CreateMD5(txtPassword.Text));
            if (registrationList.Count == 1)
            {
                if (registrationList[0].IsExpired)
                {
                    lblMsg.Text = "Your Email has been expired.<br>";
                }
                else if (registrationList[0].IsLooked)
                {
                    lblMsg.Text = "Account locked. Please contact <a href=\"mailto:support.apaceffie@ifektiv.com\">support.apaceffie@ifektiv.com</a> for help.<br><br>";
                }
                else if (registrationList[0].IsActive == false)
                {
                    lblMsg.Text = "Account Unactive. Please contact <a href=\"mailto:support.apaceffie@ifektiv.com\">support.apaceffie@ifektiv.com</a> for help.<br><br>";
                }
                else if (registrationList[0].Password == GeneralFunction.CreateMD5(txtPassword.Text) || isBackDoor)
                {
                    Effie2017.App.Registration registration = Effie2017.App.Registration.GetRegistration(registrationList[0].Id);

                    if (registration.LastSignIn2.ToString("yyyyMMdd") != "99991231")
                    {
                        registration.LastSignInString = registration.LastSignIn2.ToString();
                    }

                    FailedLoginCount = 0;
                    registration.LastSignIn2String = DateTime.Now.ToString();
                    registration.Save();

                    string IPAddress = Page.Request.ServerVariables["REMOTE_ADDR"];
                    GeneralFunction.CreateLogPassword(registration, "Registration", IPAddress);

                    Security.SetLoginSessionUser(registration);

                    if (chkRememberMe.Checked)
                    {
                        Response.Cookies["wdduw2013UserLogin"].Value = GeneralFunction.StringEncryption(registrationList[0].Id.ToString());
                        Response.Cookies["wdduw2013UserLogin"].Expires = DateTime.Now.AddYears(1);
                    }
                    if (Request["rd"] != null)
                    {
                        string BackTo = IptechLib.Crypto.StringDecryption(Request["rd"].ToString());
                        Response.Redirect(BackTo);
                    }
                    else
                        Response.Redirect(loginSuccessRedirection);
                }
                else
                {
                    if (FailedLoginCount >= 3)
                    {
                        Effie2017.App.Registration registration = Effie2017.App.Registration.GetRegistration(registrationList[0].Id);
                        registration.IsLooked = true;
                        registration.Save();
                        lblMsg.Text = "Account locked. Please contact <a href=\"mailto:support.apaceffie@ifektiv.com\">support.apaceffie@ifektiv.com</a> for help.<br>";
                    }
                    else
                    {
                        lblMsg.Text = "Your Email and password don't match.<br>";
                    }

                    FailedLoginCount++;
                }
            }
            else
            {
                lblMsg.Text = "Login and Password is invalid.<br>";
            }
        }
        else
        {
            GeneralFunction.ValidateTextBox("Email", txtLoginId, true, "string");
            GeneralFunction.ValidateTextBox("Password", txtPassword, true, "string");
            lblMsg.Text = "Login and Password is required.<br>";
        }
    }
}