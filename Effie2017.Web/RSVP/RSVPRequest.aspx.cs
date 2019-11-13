using System;
using Effie2017.App;

public partial class RSVP_RSVPRequest : System.Web.UI.Page
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
            pnlLocal.Visible = rsvp.Location.Equals("Local");
            pnlOverseas.Visible = rsvp.Location.Equals("Overseas");
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
            if (rsvp.Location.Equals("Local"))
            {
                rsvp.IsJuryCocktail = rblstCocktailLocal.SelectedValue.Equals("Yes");
            }
            else if (rsvp.Location.Equals("Overseas"))
            {
                rsvp.IsJuryCocktail = rblstCocktailOverseas.SelectedValue.Equals("Yes");
                rsvp.IsWelcomeDinner = rblstGalaOverseas.SelectedValue.Equals("Yes");
            }

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

        lblError.Text += IptechLib.Validation.ValidateRadioButtonList("RSVP for Cocktail", rblstCocktailLocal, pnlLocal.Visible);
        lblError.Text += IptechLib.Validation.ValidateRadioButtonList("RSVP for Cocktail", rblstCocktailOverseas, pnlOverseas.Visible);
        lblError.Text += IptechLib.Validation.ValidateRadioButtonList("RSVP for Welcome Dinner", rblstGalaOverseas, pnlOverseas.Visible);

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