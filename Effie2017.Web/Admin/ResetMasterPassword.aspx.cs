using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Admin : PageSecurity_Admin
{
    string userIdString = string.Empty;
    Guid userId = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    private void PopulateForm()
    {
       
    }
    private void LoadForm()
    {
       
    }

    private bool ValidateForm()
    {
        lblError.Text = "";
        GeneralFunction.RemoveHighlightControls(this);
        string ErrorPass = "Password must have a minimum of 8 characters with at least one of the following: Capital letter; lower case letter; and one number or special character.<br/>";

        bool isPasswordEmpty = !string.IsNullOrWhiteSpace(GeneralFunction.ValidateTextBox("Password", txtPassword, true, "string")) || 
                               !string.IsNullOrWhiteSpace(GeneralFunction.ValidateTextBox("Password", txtPasswordConfirm, true, "string"));

        lblError.Text += GeneralFunction.ValidateTextBox("Password", txtPassword, true, "string");
        lblError.Text += GeneralFunction.ValidateTextBox("Confirm Password", txtPasswordConfirm, true, "string");

        if ((txtPassword.Text != txtPasswordConfirm.Text && !isPasswordEmpty)) // FOR EDIT
        {
            lblError.Text += "Password does not match the confirm password.";
            GeneralFunction.HighlightControl(txtPassword);
            GeneralFunction.HighlightControl(txtPasswordConfirm);
        }
        else if ((!GeneralFunction.PasswordCheck(txtPassword.Text.Trim(), 8) && !isPasswordEmpty))
        {
            lblError.Text += ErrorPass;
            GeneralFunction.HighlightControl(txtPassword);
            GeneralFunction.HighlightControl(txtPasswordConfirm);
        }
        return string.IsNullOrEmpty(lblError.Text);
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;
        else
        {
            var MasterKey = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("MasterKeyUser")[0];
            MasterKey.Value = GeneralFunction.CreateMD5(txtPassword.Text);
            if (MasterKey.IsValid)
            {
                MasterKey.Save();
                Response.Redirect("EntryList.aspx");
            }
        }
    }
}