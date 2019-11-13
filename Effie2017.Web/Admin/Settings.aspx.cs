using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Admin_Settings : PageSecurity_Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }
    private void LoadForm()
    {
        cblRound1.DataSource = GeneralFunction.GetJuryPanelList("1");
        cblRound1.DataBind();


        cblRound2.DataSource = GeneralFunction.GetJuryPanelList("2");
        cblRound2.DataBind();


    }
    private void PopulateForm()
    {
        SystemData sys = GeneralFunction.GetSystemData();

        GeneralFunction.AssignValueCheckBoxList(cblRound1, sys.ActivePanelsRound1, "|");
        GeneralFunction.AssignValueCheckBoxList(cblRound2, sys.ActivePanelsRound2, "|");

        if (sys.ActiveRound == "1")
        {
            rbRound1.Checked = true;
            rbRound1_CheckedChanged(null, null);
        }
        if (sys.ActiveRound == "2")
        {
            rbRound2.Checked = true;
            rbRound2_CheckedChanged(null, null);
        }


        if (Security.IsReadOnlyAdmin())
        {
            GeneralFunctionEntryForm.DisableAllAction(this, false);
            btnSubmit.Visible = !Security.IsReadOnlyAdmin();
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SystemData sys = GeneralFunction.GetSystemData();

        sys.ActivePanelsRound1 = GeneralFunction.GetValueCheckBoxList(cblRound1, "|");
        sys.ActivePanelsRound2 = GeneralFunction.GetValueCheckBoxList(cblRound2, "|");
        if (rbRound1.Checked) sys.ActiveRound = "1";
        if (rbRound2.Checked) sys.ActiveRound = "2";

        sys.Save();

        lbError.Text = "Settings updated.";
        lbError2.Text = lbError.Text;
    }
    protected void rbRound1_CheckedChanged(object sender, EventArgs e)
    {
        cblRound1.Enabled = true;
        cblRound2.Enabled = false;
    }
    protected void rbRound2_CheckedChanged(object sender, EventArgs e)
    {
        cblRound1.Enabled = false;
        cblRound2.Enabled = true;
    }

}