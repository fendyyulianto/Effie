using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
public partial class User_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ((Common_MasterPage)Page.Master).ShowUser();
        ((Common_MasterPage)Page.Master).ShowNav();

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
    }
    private string ValidateForm()
    {
        string err = "";

        if (txtOldPassword.Text.Trim() == "" || txtPwdNew1.Text.Trim() == "" || txtPwdNew2.Text.Trim() == "") err += "All fields are required.<br/>";
        if (txtPwdNew1.Text.Trim() != txtPwdNew2.Text.Trim()) err += "New passwords must match.<br/>";
        if (txtOldPassword.Text.Trim() == txtPwdNew1.Text.Trim() && txtOldPassword.Text.Trim() != "") err += "Old and new passwords cannot be the same.<br/>";
        if (!GeneralFunction.PasswordCheck(txtPwdNew1.Text.Trim(), 8)) err += "Password must have a minimum of 8 characters with at least one of the following: Capital letter; lower case letter; and one number or special character.<br/>";
        return err;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string err = ValidateForm();
        if (err == "")
        {
            Registration reg;
            reg = Registration.NewRegistration();
            // Logged in?

            if (Security.IsUserLogin()) reg = Security.GetLoginSessionUser();

            if (reg != null)
            {
                if (reg.Password.Trim() == GeneralFunction.CreateMD5(txtOldPassword.Text.Trim()))
                {
                    var isExist = reg.LogPassword.Contains(GeneralFunction.CreateMD5(txtPwdNew1.Text.Trim()));

                    if (isExist)
                    {
                        lbError.Text = "Cannot allow repeat password.";
                    }
                    else if (reg.IsValid)
                    {
                        GeneralFunction.LogPasswordRegistration(ref reg, txtPwdNew1.Text.Trim());
                        Response.Redirect("~/Main/Dashboard.aspx");
                    }
                }
                else
                    lbError.Text = "Incorrect old password.";
            }
            else
                Response.Redirect("Login.aspx");
        }
        else
            lbError.Text = err;

        
    }
}
