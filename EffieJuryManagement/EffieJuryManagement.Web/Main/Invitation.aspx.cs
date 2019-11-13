using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;
using System.Data;
using System.Configuration;
using System.IO;

public partial class Main_Invitation : PageSecurity_Admin
{
    Invitation inv;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["invId"] != null && Request.QueryString["invId"] != "")
            inv = Invitation.GetInvitation(new Guid(Request.QueryString["invId"]));
        else
            inv = Invitation.NewInvitation();

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
        if (Security.IsRoleReadOnlyAdmin())
        {
            btnSubmit.Visible = false;
        }
    }

    public void LoadForm()
    {

    }

    public void PopulateForm()
    {
        if (inv != null)
        {
            lblEventYear.Text = inv.EventCode;

            Jury jury = Jury.GetJury(inv.JuryId);

            lblJuryId.Text = jury.SerialNo;
            lblName.Text = jury.FirstName + " " + jury.LastName;
            lblCompany.Text = jury.Company;
            lblEmail.Text = jury.Email;
            lblCountry.Text = jury.Country;
            
            chkInvRound1.Checked = inv.IsRound1Invited;
            chkInvRound2.Checked = inv.IsRound2Invited;

            //if (inv.IsLocked)
            {
                chkAccptRound1.Checked = inv.IsRound1Accepted;
                chkAccptRound2.Checked = inv.IsRound2Accepted;

                chkDecline.Checked = inv.IsDeclined;
            }

            chkShortListedRound1.Checked = inv.IsRound1Shortlisted;
            chkShortListedRound2.Checked = inv.IsRound2Shortlisted;

            chkAssignRound1.Checked = inv.IsRound1Assigned;
            chkAssignRound2.Checked = inv.IsRound2Assigned;
        }
    }

    public Invitation SaveForm()
    {
        if (ValidateForm())
        {
            inv.IsDeclined = chkDecline.Checked;

            inv.IsRound1Accepted = chkAccptRound1.Checked;
            inv.IsRound2Accepted = chkAccptRound2.Checked;

            inv.IsRound1Shortlisted = chkShortListedRound1.Checked;
            inv.IsRound2Shortlisted = chkShortListedRound2.Checked;

            inv.IsRound1Assigned = chkAssignRound1.Checked;
            inv.IsRound2Assigned = chkAssignRound2.Checked;

            inv.DateModifiedString = DateTime.Now.ToString();

            if (inv.IsValid)
                inv.Save();

            return inv;
        }
        else
            return null;
    }

    public bool ValidateForm()
    {
        lblError.Text = string.Empty;

        return String.IsNullOrEmpty(lblError.Text);
    }

    #region Events

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(GeneralFunction.GetRedirect("../Main/InvitationList.aspx"));
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Invitation inv = SaveForm();

        if (inv != null)
        {
            Jury jury = Jury.GetJury(inv.JuryId);

            if (jury != null)
            {
                jury.EffieExpYear = Jury.GetEffieExperienceYears(jury, inv);
                jury.DateModifiedString = DateTime.Now.ToString();
                jury.Save();
            }

            Response.Redirect(GeneralFunction.GetRedirect("../Main/InvitationList.aspx"));
        }
    }

    

    #endregion

    
}