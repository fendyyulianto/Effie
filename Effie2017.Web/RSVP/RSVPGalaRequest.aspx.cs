using System;
using Effie2017.App;

public partial class RSVP_RSVPGalaRequest : System.Web.UI.Page
{
    Guid rsvpIdGuid = Guid.Empty;
    string rsvpIdString = string.Empty;
    RSVP rsvp = null;    

    protected void Page_Load(object sender, EventArgs e)
    {
        rsvpIdString = Request.QueryString["rsvpId"];        

        if (rsvpIdString != null)
        {
            rsvpIdGuid = IptechLib.Validation.GetValueGuid(rsvpIdString, true);
        }

        try
        {
            rsvp = RSVP.GetRSVP(rsvpIdGuid);
        }
        catch { }

        if (rsvp != null)
        {
            if (rsvp.Respond)
                Response.Redirect("RSVPResponded.aspx");
        }
        else
            Response.Redirect("RSVPResponded.aspx");
       
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm(rsvp);
        }
    }

    public void LoadForm()
    {

    }

    public void PopulateForm(RSVP rsvp)
    {
        if (rsvp != null)
        {
            //lblName.Text = rsvp.FirstName;
        }
    }

    public RSVP SaveForm()
    {
        rsvpIdString = Request.QueryString["rsvpId"];
        
        if (rsvpIdString != null)
        {
            rsvpIdGuid = IptechLib.Validation.GetValueGuid(rsvpIdString, true);
        }

        try
        {
            rsvp = RSVP.GetRSVP(rsvpIdGuid);
        }
        catch { }

        if (rsvp != null)
        {
            rsvp.IsGalaDinner = rblstGalaDinner.SelectedValue.Equals("Yes");
            rsvp.Dietary = txtDietery.Text.Trim();            
            rsvp.WorkflowStatus = "04";
            rsvp.Respond = true;
            rsvp.DateModifiedString = DateTime.Now.ToString();

            rsvp.Save();
        }

        return rsvp;
    }

    public bool ValidateForm()
    {
        lblError.Text = string.Empty;        
        
        lblError.Text += IptechLib.Validation.ValidateRadioButtonList("RSVP for Gala Dinner", rblstGalaDinner, true);

        IptechLib.Forms.RemoveHighlightControls(this);

        return String.IsNullOrEmpty(lblError.Text);
    }

    #region Events

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            RSVP rsvpSubmit = SaveForm();

            if (rsvpSubmit != null)
            {
                Response.Redirect("Thankyou.aspx");
            }
        }
    }

    #endregion
}