using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Admin_Profile : PageSecurity_Admin
{
    Registration reg = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateForm();
        }
    }
    private void PopulateForm()
    {
        reg = Registration.GetRegistration(new Guid(Request.QueryString["Id"].ToString()));


        lnkEmail.Text = reg.Email;
        lnkEmail.NavigateUrl = "mailto:" + reg.Email;
        //lbPassword.Text = reg.Password;

        lbSalutation.Text = reg.Salutation;
        lbFirstname.Text = reg.Firstname;
        lbLastname.Text = reg.Lastname;
        lbJobTitle.Text = reg.Job;
        lbWebsite.Text = reg.Website;
        lbMobile.Text = GeneralFunction.ShowFriendlyContact(reg.Mobile);
        lbContact.Text = GeneralFunction.ShowFriendlyContact(reg.Contact);
        lbFax.Text = GeneralFunction.ShowFriendlyContact(reg.Fax);

        lbCompany.Text = reg.Company;
        lbAddress1.Text = reg.Address1;
        lbAddress2.Text = reg.Address2;
        lbCity.Text = reg.City;
        lbPostal.Text = reg.Postal;
        lbCountry.Text = reg.Country;

        if (reg.IsCAAAA)
        {
            lbCAAAA.Text = "Yes";
            lbCAAAADetails.Text = reg.Caaaa;
        }

        //if (reg.IsAFAA)
        //{
        //    rbAFAA.SelectedValue = "Yes";
        //    ddlAFAA.SelectedValue = reg.Afaa;
        //}
        //else
        //{
        //    rbAFAA.SelectedValue = "No";
        //    ddlAFAA.SelectedValue = "";
        //}
        if (reg.IsAPEP)
        {
            lbAPEP.Text = "Yes";
            lbAPEPDetails.Text = reg.Apep;
        }


        if (reg.IsEProg)
        {
            lbEffieProgram.Text = "Yes";
            lbNationalEffie.Text = reg.Eprog;
            lbCampaignName.Text = reg.EProgCampaign;
        }


        if (reg.IsEmailUpdate)
            lbEmailupdates.Text = "Yes";


        if (reg.IsPromo1)
            lbPromo1.Text = "Yes";


        // IsVerified
        lblVerified.Text = "No";
        lblVerified.ForeColor = System.Drawing.Color.Red;
        if (reg.IsVerified)
        {
            lblVerified.Text = "Yes";
            lblVerified.ForeColor = System.Drawing.Color.Green;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(GeneralFunction.GetRedirect("../Admin/UserList.aspx"));
    }
}