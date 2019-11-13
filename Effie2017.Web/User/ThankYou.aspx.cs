using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
public partial class User_ThankYou : System.Web.UI.Page
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
       Registration reg = null;
     
        string EncrypuserIdString = Request.QueryString["Id"];
        if (EncrypuserIdString != "")
        {
            string userIdString = GeneralFunction.StringDecryption(EncrypuserIdString);
            Guid userId = Guid.Empty;
            if ((userIdString != null && Guid.TryParse(userIdString, out userId)))
                reg = Registration.GetRegistration(userId);
        }
        if (!IsPostBack)
        {

            LoadForm();
            PopulateForm(reg);
        }
    }
    private void PopulateForm(Registration reg)
    {
        if (reg != null)
        {
            hlEmail.Text = reg.Email;
        }
    }
    private void LoadForm()
    {
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
}