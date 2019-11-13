using EffieJuryManagementApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Admin : PageSecurity_Admin
{
    static Administrator admin = null;
    string userIdString = string.Empty;
    Guid userId = Guid.Empty;
    public static bool isNew = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            admin = Administrator.NewAdministrator();
            string userIdString = Request.QueryString["id"];
            if ((userIdString != null && Guid.TryParse(userIdString, out userId)))
            {
                isNew = false;
                admin = Administrator.GetAdministrator(userId);
            }

            LoadForm();
            PopulateForm(admin);
        }
    }

    private void PopulateForm(Administrator admin)
    {
        txtName.Text = admin.Name;
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            //ddlAdminType.Enabled = false;
            //txtLoginId.Enabled = false;
        }
        txtLoginId.Text = admin.LoginId;
        //txtPassword.Text = admin.Password;
        ddlAdminType.SelectedValue = admin.Access;
        cbisActive.Checked = admin.IsActive;
    }
    private void LoadForm()
    {
        //ddlAdminType.Items.Add(new ListItem("Support Staff", "ST"));
        ddlAdminType.Items.Add(new ListItem("Read Only", "RO"));
        //ddlAdminType.Items.Add(new ListItem("Admin No Jury", "AD2"));
        //ddlAdminType.Items.Add(new ListItem("Admin Jury Only", "AD3"));
        ddlAdminType.Items.Add(new ListItem("Admin", "AD"));
        //ddlAdminType.Items.Add(new ListItem("Super Admin (Finance)", "SF"));
        ddlAdminType.Items.Add(new ListItem("Super Admin", "SA"));
        ddlAdminType.Items.Insert(0, new ListItem("Select access type", ""));
    }

    private bool ValidateForm()
    {
        lblError.Text = "";
        GeneralFunction.RemoveHighlightControls(this);
        lblError.Text += GeneralFunction.ValidateTextBox("Name", txtName, false, "string");
        lblError.Text += GeneralFunction.ValidateTextBox("Login Id", txtLoginId, true, "string");
        lblError.Text += GeneralFunction.ValidateDropDownList("Admin type", ddlAdminType, true, "");

        string ErrorPass = "Password must have a minimum of 8 characters with at least one of the following: Capital letter; lower case letter; and one number or special character.<br/>";

        bool isPasswordEmpty = !string.IsNullOrWhiteSpace(GeneralFunction.ValidateTextBox("Password", txtPassword, true, "string")) || 
                               !string.IsNullOrWhiteSpace(GeneralFunction.ValidateTextBox("Confirm Password", txtPasswordConfirm, true, "string"));

        if (!string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            lblError.Text += GeneralFunction.ValidateTextBox("Confirm Password", txtPasswordConfirm, true, "");
        }

        if (isNew)
        {
            lblError.Text += GeneralFunction.ValidateTextBox("Password", txtPassword, true, "");
            lblError.Text += GeneralFunction.ValidateTextBox("Confirm Password", txtPasswordConfirm, true, "");
        }

        if ((txtPassword.Text != txtPasswordConfirm.Text && isNew) || // FOR NEW
            (txtPassword.Text != txtPasswordConfirm.Text && !isNew && !isPasswordEmpty)) // FOR EDIT
        {
            lblError.Text += "Password does not match the confirm password.";
            GeneralFunction.HighlightControl(txtPassword);
            GeneralFunction.HighlightControl(txtPasswordConfirm);
        }
        else if (((!GeneralFunction.PasswordCheck(txtPassword.Text.Trim(), 8) && !isPasswordEmpty) && !isNew) ||
            ((!GeneralFunction.PasswordCheck(txtPassword.Text.Trim(), 8) && !isPasswordEmpty) && isNew))
        {
            lblError.Text += ErrorPass;
            GeneralFunction.HighlightControl(txtPassword);
            GeneralFunction.HighlightControl(txtPasswordConfirm);
        }

        List<Administrator> administrator = AdministratorList.GetAdministratorList().Where(x => x.LoginId == txtLoginId.Text).ToList();
        if (administrator.Any() && isNew)
        {
            lblError.Text += "Login Id already exists.<br>";
            GeneralFunction.HighlightControl(txtLoginId);
        }
        else
        {
            administrator = administrator.Where(x => x.Id != admin.Id).ToList();
            if (administrator.Any())
            {
                lblError.Text += "Login Id already exists.<br>";
                GeneralFunction.HighlightControl(txtLoginId);
            }
        }

        return string.IsNullOrEmpty(lblError.Text);
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;
        else
        {
            if (Request.QueryString["id"] == null)
            {
                admin = Administrator.NewAdministrator();
                admin.DateCreatedString = DateTime.Now.ToString();
            }
            else
            {
                admin = Administrator.GetAdministrator(new Guid(Request.QueryString["id"]));
                admin.DateModifiedString = DateTime.Now.ToString();
            }
            admin.Name = txtName.Text;
            admin.LoginId = txtLoginId.Text;
            bool isPasswordEmpty = !string.IsNullOrWhiteSpace(GeneralFunction.ValidateTextBox("Password", txtPassword, true, "string"));
            if(!isPasswordEmpty)
            {
                //admin.Password = GeneralFunction.CreateMD5(txtPassword.Text);
                admin.Password = txtPassword.Text;
                admin.LastChangePasswordString = DateTime.Now.ToString();
            }
            admin.Access = ddlAdminType.SelectedValue;
            admin.IsActive = cbisActive.Checked;

            if (admin.IsValid)
            {
                admin.Save();
                Response.Redirect("AdminList.aspx");
            }
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("AdminList.aspx");
    }
}