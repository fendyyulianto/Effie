using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_ResetPassword : System.Web.UI.Page
{
    Guid RegistrationId = Guid.Empty;
    Registration registration;
    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            string RegistrationIdstr = IptechLib.Crypto.StringDecryption(Request.QueryString["Id"].ToString());
            RegistrationId = new Guid(RegistrationIdstr.ToString());
            registration = Registration.GetRegistration(RegistrationId);
        } catch { }
    }

    private string ValidateForm()
    {
        string err = "";

        if (txtPwdNew1.Text.Trim() == "" || txtPwdNew2.Text.Trim() == "") err += "All fields are required.<br/>";
        if (txtPwdNew1.Text.Trim() != txtPwdNew2.Text.Trim()) err += "New passwords must match.<br/>";
        if (registration != null)
            if (GeneralFunction.CreateMD5(txtPwdNew1.Text.Trim()) == registration.Password) err += "Old and new passwords cannot be the same.<br/>";
        if (!GeneralFunction.PasswordCheck(txtPwdNew1.Text.Trim(), 8)) err += "Password must have a minimum of 8 characters with at least one of the following: Capital letter; lower case letter; and one number or special character.<br/>";
        return err;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string err = ValidateForm();
        if (err == "")
        {
            if (registration != null)
            {
                registration.Password = GeneralFunction.CreateMD5((txtPwdNew1.Text.Trim()));
                registration.DateModifiedString = DateTime.Now.ToString();
                
                //if (passwordHistories.Any())
                //{
                //    lbError.Text = "Cannot allow repeat password.";
                //}
                //else
                if (registration.IsValid)
                {
                    registration.Save();
                    Response.Redirect("Login.aspx");
                }
            }
            else
                Response.Redirect("Login.aspx");
        }
        else
            lbError.Text = err;
    }
}