using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
public partial class User_Profile : System.Web.UI.Page
{
    Registration reg;
    bool isAdminViewMode = false;
    bool isAdminEditMode = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["am"] == "v") isAdminViewMode = true;
        if (Request.QueryString["am"] == "e") isAdminEditMode = true;



        if (Security.IsUserLogin()) reg = Security.GetLoginSessionUser();
        else
        {
            if (Request.QueryString["md"] == "edit")
                Response.Redirect("Login.aspx");
            else
                reg = Registration.NewRegistration();
        }


        // New reg cutoff?
        if (GeneralFunction.IsRegisterNewUserCutOff() && reg.IsNew)
        {
            Response.Redirect("../User/Login.aspx");
        }


        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
            
        }
      
        ddlCountry.Items[43].Attributes.Add("disabled", "disabled");

    }
    private void LoadForm()
    {
        // Country

        ddlCountry.Items.Add(new ListItem("Select Country", ""));
        ddlCountry.Items.Add(new ListItem("Australia", "Australia"));
        ddlCountry.Items.Add(new ListItem("Bangladesh", "Bangladesh"));
        ddlCountry.Items.Add(new ListItem("Bhutan", "Bhutan"));
        ddlCountry.Items.Add(new ListItem("Cambodia", "Cambodia"));
        ddlCountry.Items.Add(new ListItem("China", "China"));
        ddlCountry.Items.Add(new ListItem("Cook Islands", "Cook Islands"));
        ddlCountry.Items.Add(new ListItem("Fiji", "Fiji"));
        ddlCountry.Items.Add(new ListItem("French Polynesia", "French Polynesia"));
        ddlCountry.Items.Add(new ListItem("Hong Kong", "Hong Kong"));
        ddlCountry.Items.Add(new ListItem("India", "India"));
        ddlCountry.Items.Add(new ListItem("Indonesia", "Indonesia"));
        ddlCountry.Items.Add(new ListItem("Japan", "Japan"));
        ddlCountry.Items.Add(new ListItem("Kazakhstan", "Kazakhstan"));
        ddlCountry.Items.Add(new ListItem("Kiribati", "Kiribati"));
        ddlCountry.Items.Add(new ListItem("Korea", "Korea"));
        ddlCountry.Items.Add(new ListItem("Laos", "Laos"));
        ddlCountry.Items.Add(new ListItem("Malaysia", "Malaysia"));
        ddlCountry.Items.Add(new ListItem("Maldives", "Maldives"));
        ddlCountry.Items.Add(new ListItem("Marshall Islands", "Marshall Islands"));
        ddlCountry.Items.Add(new ListItem("Micronesia", "Micronesia"));
        ddlCountry.Items.Add(new ListItem("Mongolia", "Mongolia"));
        ddlCountry.Items.Add(new ListItem("Myanmar", "Myanmar"));
        ddlCountry.Items.Add(new ListItem("Nauru", "Nauru"));
        ddlCountry.Items.Add(new ListItem("Nepal", "Nepal"));
        ddlCountry.Items.Add(new ListItem("New Caledonia", "New Caledonia"));
        ddlCountry.Items.Add(new ListItem("New Zealand", "New Zealand"));
        ddlCountry.Items.Add(new ListItem("Niue", "Niue"));
        ddlCountry.Items.Add(new ListItem("Pakistan", "Pakistan"));
        ddlCountry.Items.Add(new ListItem("Palau", "Palau"));
        ddlCountry.Items.Add(new ListItem("Philippines", "Philippines"));
        ddlCountry.Items.Add(new ListItem("Samoa", "Samoa"));
        ddlCountry.Items.Add(new ListItem("Singapore", "Singapore"));
        ddlCountry.Items.Add(new ListItem("Solomon Islands", "Solomon Islands"));
        ddlCountry.Items.Add(new ListItem("Sri Lanka", "Sri Lanka"));
        ddlCountry.Items.Add(new ListItem("Taiwan", "Taiwan"));
        ddlCountry.Items.Add(new ListItem("Thailand", "Thailand"));
        ddlCountry.Items.Add(new ListItem("Timor-Leste", "Timor-Leste"));
        ddlCountry.Items.Add(new ListItem("Tonga", "Tonga"));
        ddlCountry.Items.Add(new ListItem("Tuvalu", "Tuvalu"));
        ddlCountry.Items.Add(new ListItem("Uzbekistan", "Uzbekistan"));
        ddlCountry.Items.Add(new ListItem("Vanautu", "Vanautu"));
        ddlCountry.Items.Add(new ListItem("Vietnam", "Vietnam"));
        ddlCountry.Items.Add(new ListItem("-------Rest of the Standard Countries---------", ""));
        GeneralFunction.LoadDropDownListCountry(ddlCountry);
       // ddlCountry.Items.Insert(23, new ListItem("-------Rest of the Standard Countries---------", ""));
        ddlCountry.Items[43].Attributes.Add("disabled", "disabled");
    }
    private void PopulateForm()
    {
        //string userIdString = Request.QueryString["uId"];
        //if (userIdString == null) userIdString = "";
        if (reg != null)
        {
            //if (userIdString != "")
            //{
            if (!reg.IsNew)
            {
                ddlSalutation.SelectedValue = reg.Salutation;
                txtEmail.Text = reg.Email;
                txtConfirmEmail.Text = reg.Email;
                txtPassword.Text = reg.Password;
                txtConfirmPassword.Text = reg.Password;
                txtFirstName.Text = reg.Firstname;
                txtLastName.Text = reg.Lastname;
                txtJobTitle.Text = reg.Job;
                txtWebsite.Text = reg.Website;
                txtCompanyName.Text = reg.Company;
                txtAddress1.Text = reg.Address1;
                txtAddress2.Text = reg.Address2;
                txtCity.Text = reg.City;
                txtPostalCode.Text = reg.Postal;
                


                txtMobileCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(reg.Mobile);
                txtMobileArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(reg.Mobile);
                txtMobile.Text = GeneralFunction.GetNumberFromContactNumber(reg.Mobile);

                txtContactConutry.Text = GeneralFunction.GetCountryCodeFromContactNumber(reg.Contact);
                txtContactArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(reg.Contact);
                txtContact.Text = GeneralFunction.GetNumberFromContactNumber(reg.Contact);

                txtFaxCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(reg.Fax);
                txtFaxArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(reg.Fax);
                txtFax.Text = GeneralFunction.GetNumberFromContactNumber(reg.Fax);

                ddlCountry.SelectedValue = reg.Country;
                if (Security.IsRoleSuperAdmin())
                {
                    LookedTableRowId.Visible = true;
                }
                else
                {
                    LookedTableRowId.Visible = false;
                }
                
                cbLooked.Enabled = false;
                cbLooked.Checked = reg.IsLooked;

                chkDisable.Checked = !reg.IsActive;
                chkVerified.Checked = reg.IsVerified;



                // if edit user, precheck and disable the TC
                if (!reg.IsNew)
                {
                    chkTC.Checked = true;
                    chkTC.Enabled = false;
                }

            }
            UIShow();
        }
    }
    private void BindAdminRemarks()
    {
        lbAdminRemarks.Text = "";
        RegistrationRemarksList remarksList = RegistrationRemarksList.GetRegistrationRemarksList(reg.Id);
        foreach (RegistrationRemarks remarks in remarksList)
            lbAdminRemarks.Text += "<tr><td width='100px' style='vertical-align:top'>" + remarks.DateCreated.ToString("dd/MM/yy hh:mm tt") + " : </td><td>" + remarks.Remarks + "</td></tr>";
        if (lbAdminRemarks.Text == "")
            lbAdminRemarks.Text = "None";
        else
            lbAdminRemarks.Text = "<table width='100%' style='font-size:10px;'>" + lbAdminRemarks.Text + "</table>";
    }
   
    private bool ValidateForm()
    {
        lbError.Text = "";
        GeneralFunction.RemoveHighlightControls(this);
        RegistrationList registrationList = RegistrationList.GetRegistrationList("", "", "");
        foreach (Registration registration in registrationList)
        {
            if (registration.Email.Trim() == txtEmail.Text.Trim())
            {
                if (registration.Id != reg.Id)
                {
                    lbError.Text += "The mail address is already registered.<br/>";
                    GeneralFunction.HighlightControl(txtEmail);
                    break;
                }
            }
           

        }

        lbError.Text += GeneralFunction.ValidateTextBox("Email", txtEmail, true, "EmailSingle");
        if (Request.QueryString["md"] != "edit")
        {
            lbError.Text += GeneralFunction.ValidateTextBox("Re-confirm Email", txtConfirmEmail, true, "EmailSingle");
            if (txtEmail.Text.Trim() != txtConfirmEmail.Text.Trim())
            {
                lbError.Text += "Emails do not match.<br/>";
                GeneralFunction.HighlightControl(txtEmail);
                GeneralFunction.HighlightControl(txtConfirmEmail);
            }

            if (((txtPassword.Text.Length < 8) || (txtConfirmPassword.Text.Length < 8)))
            {
                lbError.Text += "Password must be 8 characters.<br/>";
                GeneralFunction.HighlightControl(txtPassword);
                GeneralFunction.HighlightControl(txtConfirmPassword);
            }
            lbError.Text += GeneralFunction.ValidateTextBox("Password", txtPassword, true, "string");
            lbError.Text += GeneralFunction.ValidateTextBox("Confirm Password", txtConfirmPassword, true, "string");
            if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                lbError.Text += "Password do not match.<br/>";
                GeneralFunction.HighlightControl(txtPassword);
                GeneralFunction.HighlightControl(txtConfirmPassword);
            }

            if (!GeneralFunction.PasswordCheck(txtPassword.Text.Trim(), 8))
            {
                lbError.Text += "Password must have a minimum of 8 characters with at least one of the following: Capital letter; lower case letter; and one number or special character.<br/>";
                GeneralFunction.HighlightControl(txtPassword);
                GeneralFunction.HighlightControl(txtConfirmPassword);
            }
        }
        
        lbError.Text += GeneralFunction.ValidateDropDownList("Salutation", ddlSalutation, true, "");
        lbError.Text += GeneralFunction.ValidateTextBox("First Name", txtFirstName, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Last Name", txtLastName , true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Job Title", txtJobTitle , true, "string");

        lbError.Text += GeneralFunction.ValidateTextBox("Contact Country Code", txtContactConutry, false, "number");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Area Code", txtContactArea, false, "number");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact", txtContact, true, "number");

        lbError.Text += GeneralFunction.ValidateTextBox("Mobile Country Code", txtMobileCountry, false, "number");
        lbError.Text += GeneralFunction.ValidateTextBox("Mobile Area Code", txtMobileArea, false, "number");
        lbError.Text += GeneralFunction.ValidateTextBox("Mobile", txtMobile, false, "number");

        lbError.Text += GeneralFunction.ValidateTextBox("Fax Country Code", txtFaxCountry, false, "number");
        lbError.Text += GeneralFunction.ValidateTextBox("Fax Area Code", txtFaxArea, false, "number");
        lbError.Text += GeneralFunction.ValidateTextBox("Fax", txtFax, false, "number");

        lbError.Text += GeneralFunction.ValidateTextBox("Company Name", txtCompanyName, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Address 1", txtAddress1, true, "string");
      //  lbError.Text += GeneralFunction.ValidateTextBox("Address 2", txtAddress2, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("City", txtCity, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Zip code", txtPostalCode, true, "string");
        lbError.Text += GeneralFunction.ValidateDropDownList("Country", ddlCountry, true, "");
        
        if (!chkTC.Checked) lbError.Text += "You must agree to recieve updates, the Terms of Use and Privacy Policy.<br/>";
        return (lbError.Text == "");
    }
    private void SaveForm()
    {
        reg.Email = txtEmail.Text;
        if (Request.QueryString["md"] != "edit")
        {
            GeneralFunction.LogPasswordRegistration(ref reg, txtPassword.Text.Trim());
        }
        reg.Salutation = ddlSalutation.SelectedValue;
        reg.Firstname = txtFirstName.Text;
        reg.Lastname = txtLastName.Text;
        reg.Job = txtJobTitle.Text;
        reg.Mobile = txtMobileCountry.Text.Trim() + "|" + txtMobileArea.Text.Trim() + "|" + txtMobile.Text.Trim();
        reg.Contact = txtContactConutry.Text.Trim() + "|" + txtContactArea.Text.Trim() + "|" + txtContact.Text.Trim();
        reg.Fax = txtFaxCountry.Text.Trim() + "|" + txtFaxArea.Text.Trim() + "|" + txtFax.Text.Trim();
        reg.Website = txtWebsite.Text;
        reg.Company = txtCompanyName.Text;
        reg.Address1 = txtAddress1.Text;
        reg.Address2 = txtAddress2.Text;
        reg.City = txtCity.Text;
        reg.Postal = txtPostalCode.Text;
        reg.Country = ddlCountry.SelectedValue;
        reg.IsLooked = cbLooked.Checked;
        //reg.DateCreatedString = DateTime.Now.ToString();

        reg.IsCAAAA = false;
        reg.IsAFAA = false;
        reg.IsAPEP = false;
        reg.IsEProg = false;

        reg.Caaaa = "";
        reg.Afaa = "";
        reg.Apep = "";

        reg.EProgCampaign = "";
        reg.Eprog = "";

        // overwrite - it must always be true
        reg.IsEmailUpdate = true;

        if (reg.IsNew)
        {
            reg.DateCreatedString = DateTime.Now.ToString();
            reg.Status = StatusRegistration.InActive;
        }
        if (reg.IsValid)
            reg.Save();
        
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            SaveForm();

            if (isAdminViewMode || isAdminEditMode) Response.Redirect(GeneralFunction.GetRedirect("~/Admin/UserList.aspx"));

            if (Security.IsUserLogin())
            {
                Response.Redirect("~/Main/Dashboard.aspx");
            }
            else
            {
                if (reg.Status == StatusRegistration.InActive)
                    Email.SendActivateRegistrationEmail(reg);
                string userid = GeneralFunction.StringEncryption(reg.Id.ToString());
                Response.Redirect("ThankYou.aspx?Id=" + userid);
            }
        }
    }
    private void UIShow()
    {
        lbSignUpTitle.Visible = true;

        //string userIdString = Request.QueryString["uId"];
        //if (userIdString == null) userIdString = "";
        if (reg != null)
        {
            if (!reg.IsNew)
            {
                phLoginDetails.Visible = false;
                lbSignUpTitle.Visible = false;
                lbEditTitle.Visible = true;
                txtEmail.Enabled = false;
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtCompanyName.Enabled = false;
            }
        }

        if (Security.IsRoleSuperAdmin())
        {
            LookedTableRowId.Visible = true;
        }
        else
        {
            LookedTableRowId.Visible = false;
        }

        // Admin Mode
        if (isAdminViewMode || isAdminEditMode)
        {
            if (isAdminViewMode)
            {
                btnSubmit.Visible = false;
                GeneralFunction.ChangeStateControls(this, false);
            }

            if (isAdminEditMode)
            {
                // top status
                phStatus.Visible = true;
                phAdminRemarks.Visible = true;
                phAdminRemarksEdit.Visible = true;

                // admin remarks
                BindAdminRemarks();

                GeneralFunction.ChangeStateControls(this, true);

                txtEmail.Enabled = false;
            }

            lbEditUserTitle.Visible = false;
            lbEditTitle.Visible = false;
            lbEditUserTitle.Visible = true;



            // Access Control for administrators
            Security.SecureControlByHiding(btnAdminSubmitStatus);
            Security.SecureControlByHiding(btnAdminSubmitRemarks);
            Security.SecureControlByHiding(btnSubmit);

        }

    }
    protected void rbEffieProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        UIShow();
    }

    protected void rbCAAAA_SelectedIndexChanged(object sender, EventArgs e)
    {
        UIShow();
    }
    protected void rbAPEP_SelectedIndexChanged(object sender, EventArgs e)
    {
        UIShow();
    }
    protected void rbAFAA_SelectedIndexChanged(object sender, EventArgs e)
    {
        UIShow();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (isAdminViewMode || isAdminEditMode) Response.Redirect(GeneralFunction.GetRedirect("~/Admin/UserList.aspx"));

        if (Request.QueryString["md"] == "edit")
            Response.Redirect("~/Main/Dashboard.aspx");
        else
            Response.Redirect("Login.aspx");
    }

    protected void btnAdminSubmitStatus_Click(object sender, EventArgs e)
    {
        reg.IsActive = !chkDisable.Checked;
        reg.IsVerified = chkVerified.Checked;
        reg.Save();
        lbError.Text = "Updated.";
        GeneralFunction.GetAllRegistrationCache(true); // forces the cache to refresh
    }
    protected void btnAdminSubmitRemarks_Click(object sender, EventArgs e)
    {
        if (txtAdminRemarks.Text.Trim() != "")
        {
            RegistrationRemarks remarks = RegistrationRemarks.NewRegistrationRemarks();
            remarks.Remarks = txtAdminRemarks.Text;
            remarks.RegistrationId = reg.Id;
            remarks.DateCreatedString = DateTime.Now.ToString();
            remarks.Save();

            txtAdminRemarks.Text = "";
            BindAdminRemarks();
        }
    }
}
