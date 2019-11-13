using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_FlagReasonCMS : PageSecurity_Admin
{
    FlagReasons f = null;
    string flagIdString = string.Empty;
    Guid flagId = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            f = FlagReasons.NewFlagReasons();
            flagIdString = Request.QueryString["id"];
            if ((flagIdString != null && Guid.TryParse(flagIdString, out flagId)))
                f = FlagReasons.GetFlagReasons(flagId);

            LoadForm();
            PopulateForm(f);
        }
    }
    private void PopulateForm(FlagReasons f)
    {
        txtDescription.Text = f.Description;
        txtBodyName.Text = f.Bodyname;
        chkHasOther.Checked = f.isHasOther;
        if (chkHasOther.Checked)
            pnlOther.Visible = false;
        else
            pnlOther.Visible = true;
        chkIsActive.Checked = f.IsActive;
    }
    private void LoadForm()
    {
    }

    private bool ValidateForm()
    {
        lblError.Text = "";
        GeneralFunction.RemoveHighlightControls(this);
        if(chkHasOther.Checked)
            lblError.Text += GeneralFunction.ValidateTextBox("Description", txtDescription, false, "string");
        else
            lblError.Text += GeneralFunction.ValidateTextBox("Description", txtDescription, true, "string");
        lblError.Text += GeneralFunction.ValidateTextBox("Body name", txtBodyName, true, "string");
        return string.IsNullOrEmpty(lblError.Text);
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (!ValidateForm())
            return;
        else
        {
            if (Request.QueryString["id"] == null)
            {
                f = FlagReasons.NewFlagReasons();
                f.DateCreatedString = DateTime.Now.ToString();
            }
            else
            {
                f = FlagReasons.GetFlagReasons(new Guid(Request.QueryString["id"]));
                f.DateModifiedString = DateTime.Now.ToString();
            }
            f.Bodyname = txtBodyName.Text;
            f.IsActive = chkIsActive.Checked;
            if (chkHasOther.Checked)
            {
                f.isHasOther = true;
                f.Description = "";
            }
            else if(!chkHasOther.Checked)
            {
                f.isHasOther = false;
                f.Description = txtDescription.Text;
            }

            if (f.IsValid)
            {
                f.Save();
                Response.Redirect("FlagReasonCMSList.aspx");
            }
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("FlagReasonCMSList.aspx");
    }
}