using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RSVP_RSVPSend : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        var rSVPList = Effie2017.App.RSVPList.GetRSVPList().Where(m => m.IsInvitingGalaDinner && m.WorkflowStatus == "01").ToList();

        foreach (Effie2017.App.RSVP rSVP in rSVPList)
        {
            if (rSVP.WorkflowStatus == "01")
            {
                //if (rSVP.Location.Equals("Local"))
                //{
                //    Email.SendRSVPEmailLocal(rSVP);
                //}
                //else if (rSVP.Location.Equals("Overseas"))
                //{
                //    Email.SendRSVPEmailOverseas(rSVP);
                //}

                Email.SendRSVPGalaDinnerEmail(rSVP);
                rSVP.DateCreatedString = DateTime.Now.ToString();
                rSVP.WorkflowStatus = "02";
                rSVP.Save();
            }
        }
    }
}