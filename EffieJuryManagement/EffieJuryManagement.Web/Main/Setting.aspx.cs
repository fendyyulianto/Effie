using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;

public partial class Main_Setting : PageSecurity_Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    public void LoadForm()
    {

    }

    public void PopulateForm()
    {
        radUpdateProfileDate.SelectedDate = Convert.ToDateTime(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("UpdateProfileDate")[0].Value);
        radInvitationDate.SelectedDate = Convert.ToDateTime(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("InvitationDeadline")[0].Value);
        txtEventCode.Text = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value;

        if (Security.IsRoleReadOnlyAdmin())
        {
            btnSubmit.Visible = false;
        }
    }

    public bool ValidateForm()
    {
        lbError.Text = string.Empty;

        if (radUpdateProfileDate.SelectedDate == null)
            lbError.Text += "Update Profile Deadline is required.<br/>";

        if (radInvitationDate.SelectedDate == null)
            lbError.Text += "Invitation Deadline is required.<br/>";

        lbError.Text += IptechLib.Validation.ValidateTextBox("Event Code", txtEventCode, true, IptechLib.ValidationType.Number);

        int eventYear;

        int.TryParse(txtEventCode.Text,out eventYear);

        if (eventYear != DateTime.Now.Year && eventYear != DateTime.Now.AddYears(1).Year)
            lbError.Text += "Event Code is not valid.<br/>";

        return String.IsNullOrEmpty(lbError.Text);
    }

    public void SaveForm()
    {
        if (ValidateForm())
        {
            Gen_GeneralUseValueList genValueUpdateProfileLst = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("UpdateProfileDate");
            Gen_GeneralUseValue genValueUpdateProfile = Gen_GeneralUseValue.GetGen_GeneralUseValue(genValueUpdateProfileLst[0].Id);

            if (genValueUpdateProfile != null)
            {
                DateTime updateProfile = (DateTime)radUpdateProfileDate.SelectedDate;
                genValueUpdateProfile.Value = updateProfile.AddDays(1).AddSeconds(-1).ToString();
                genValueUpdateProfile.DateModifiedString = DateTime.Now.ToString();
                genValueUpdateProfile.Save();
            }

            Gen_GeneralUseValueList genValueInvitationLst = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("InvitationDeadline");
            Gen_GeneralUseValue genValueUpdateInvitation = Gen_GeneralUseValue.GetGen_GeneralUseValue(genValueInvitationLst[0].Id);

            if (genValueUpdateInvitation != null)
            {
                DateTime invitation = (DateTime)radInvitationDate.SelectedDate;
                genValueUpdateInvitation.Value = invitation.AddDays(1).AddSeconds(-1).ToString();
                genValueUpdateInvitation.DateModifiedString = DateTime.Now.ToString();
                genValueUpdateInvitation.Save();
            }

            Gen_GeneralUseValueList genValueEventCodeLst = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode");
            Gen_GeneralUseValue genValueUpdateEventCode = Gen_GeneralUseValue.GetGen_GeneralUseValue(genValueEventCodeLst[0].Id);
            genValueUpdateEventCode.Value = txtEventCode.Text;
            genValueUpdateEventCode.Save();

            lbError.Text = "Setting saved successfully.";
        }
    }

    #region Events

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveForm();
    }

    #endregion
    
}