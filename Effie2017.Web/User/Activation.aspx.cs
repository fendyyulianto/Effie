using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
public partial class User_Activation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userid = Request.QueryString["Id"];
            //string userid = "55958867-A381-4891-B3D6-F0FB56C75A4D";
            if (userid != null)
            {
                try
                {
                    Registration registration = Registration.GetRegistration(new Guid(userid));
                    if (registration.Status == StatusRegistration.InActive)
                    {
                        registration.Status = StatusRegistration.OK;
                        if (registration.IsValid)
                        {
                            registration.Save();
                            lbMesssage.Text = "Your account is now activated.Kindly login by clicking the Login button below.";
                            //if (registration.Status == StatusRegistration.OK)
                            // Email.SendNewRegistrationEmail(registration);

                        }
                    }
                    else if (registration.Status == StatusRegistration.OK)
                    {                        
                       lbMesssage.Text = "Your account already activated. ";
                    }
                }
                catch { lbMesssage.Text = "Invaild User"; }
            }
        }
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
}