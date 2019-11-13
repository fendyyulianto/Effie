using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Jury_JuryTC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!chkOK.Checked)
        {
            lbError.Text = "Please tick the checkbox before proceeding.";
            return;
        }

        // update jury
        EffieJuryManagementApp.Jury jury = Security.GetLoginSessionJury();
        if (jury != null)
        {
            jury.IsFirstTimeLogin = false;
            jury.Save();

            Response.Redirect("Dashboard.aspx");
        }
    }
}