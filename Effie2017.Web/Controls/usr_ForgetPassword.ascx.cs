using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_usr_ForgetPassword : System.Web.UI.UserControl
{
    public bool backVisible { get; set; }
    public string backRedirection { get; set; }

    private string _loginAppName = "unknown";

    public string loginAppName
    {
        get
        {
            return _loginAppName;
        }
        set
        {
            _loginAppName = value;
        }
    }

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
        if (backVisible)
        {
            lnkBack.Visible = true;
            lnkBack.NavigateUrl = backRedirection;
        }
    }

    protected void PopulateForm()
    {
    }

    protected void btnSendPassword_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";

        if (!txtLoginId.Text.Trim().Equals(""))
        {
            Effie2017.App.RegistrationList registrationList = Effie2017.App.RegistrationList.GetRegistrationList(txtLoginId.Text, "", StatusRegistration.OK);

            if (registrationList.Count == 1)
            {
                //string host = System.Configuration.ConfigurationManager.AppSettings["emailHost"].ToString();
                //int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["emailport"].ToString());
                //string emailFromName = System.Configuration.ConfigurationManager.AppSettings["emailFromName"].ToString();
                //string emailFrom = System.Configuration.ConfigurationManager.AppSettings["emailFrom"].ToString();
                //string emailFromLogin = System.Configuration.ConfigurationManager.AppSettings["emailFromLogin"].ToString();
                //string emailFromPassword = System.Configuration.ConfigurationManager.AppSettings["emailFromPassword"].ToString();
                //string emailTo = "";
                //string emailCc = System.Configuration.ConfigurationManager.AppSettings["emailCc"].ToString();
                //string emailBcc = System.Configuration.ConfigurationManager.AppSettings["emailBcc"].ToString();
                //string emailSubject = System.Configuration.ConfigurationManager.AppSettings["emailSubjectForgetPassword"].ToString();
                //bool isHTML = true;
                //string emailMessage = "";

                //emailTo = registrationList[0].Email;

                //emailMessage = GeneralFunction.ReadTxtFile(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "emailTemplate\\UserPassword.htm");

                //emailMessage = emailMessage.Replace("##LoginId##", registrationList[0].Email);
                //emailMessage = emailMessage.Replace("##Password##", registrationList[0].Password);

                //GeneralFunction.SendEmail(host, port, emailFromName, emailFrom, emailFromLogin, emailFromPassword, emailTo, emailCc, emailBcc, emailSubject, isHTML, emailMessage);

                Registration reg = registrationList[0];
                string password = GeneralFunction.CreatePassword(8);
                GeneralFunction.LogPasswordRegistration(ref reg, password);
                Email.SendForgotPasswordEmail(reg, password, false);

                lblMsg.Text = "Password Sent.<br>";
            }
            else
                lblMsg.Text = "Login is invalid.<br>";
        }
        else
            lblMsg.Text = "login is required";
    }
}