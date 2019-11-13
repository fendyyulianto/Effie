using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using System.Data;
using System.Configuration;
using System.IO;
using Telerik.Web.UI;

public partial class Admin_JuryPanel : PageSecurity_Admin
{
    int MaxJuryPerPanel = 20;
    int itemsToShow { get; set; }
    string round;
    protected void Page_Load(object sender, EventArgs e)
    {
        round = Request.QueryString["r"];
        
        itemsToShow = Convert.ToInt32(EffieJuryManagementApp.Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultListToShow")[0].Value);
        
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    private void LoadForm()
    {
        ViewState["idsForPanel"] = (string)Session["idsForPanel"];

        // Round 1 and 2 panels
        cblRound1.DataSource = GeneralFunction.GetJuryPanelList("1");
        cblRound1.DataBind();
        cblRound2.DataSource = GeneralFunction.GetJuryPanelList("2");
        cblRound2.DataBind();
    }

    private void PopulateForm()
    {
        lbTitle.Text = "Edit Judge Panel";

        //panel info
        //GeneralFunction.AssignValueCheckBoxList(cblRound1, jury.Round1PanelId, "|");
        //GeneralFunction.AssignValueCheckBoxList(cblRound2, jury.Round2PanelId, "|");

        //EffieJuryManagementApp.InvitationList juryInvList = EffieJuryManagementApp.InvitationList.GetInvitationList(jury.Id, EffieJuryManagementApp.Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value);
        //if(juryInvList.Count > 0)
        //{
        //    cblRound1.Enabled = juryInvList[0].IsRound1Accepted;
        //    cblRound2.Enabled = juryInvList[0].IsRound2Accepted;
        //}
    }
    #region Events
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // Validate the max jury per panel
        lbError.Text = "";
        string error1 = "";
        string error2 = "";
        List<string> panels1 = GeneralFunction.GetJuryPanelList("1");
        foreach (string panel in panels1)
        {
            if (/*!GeneralFunction.GetJuryListFromPanel(panel, "1").Contains(jury) &&*/
                GeneralFunction.GetJuryListFromPanel(panel, "1").Count() >= MaxJuryPerPanel &&
                GeneralFunction.GetValueCheckBoxList(cblRound1, "|").Contains(panel))
                error1 += panel + "<br/>";
        }

        List<string> panels2 = GeneralFunction.GetJuryPanelList("2");
        foreach (string panel in panels2)
        {
            if (/*GeneralFunction.GetJuryListFromPanel(panel, "2").Contains(jury) &&*/
                GeneralFunction.GetJuryListFromPanel(panel, "2").Count() >= MaxJuryPerPanel &&
                GeneralFunction.GetValueCheckBoxList(cblRound2, "|").Contains(panel))
                error2 += panel + "<br/>";
        }

        if (error1 != "" || error2 != "")
        {
            if (error1 != "")
                lbError.Text += "Maximum number of judges for the following panels in Round 1:<br/>" + error1;
            if (error2 != "")
                lbError.Text += "Maximum number of judges for the following panels in Round 2:<br/>" + error2;

            return;
        }

        //Save
        string[] ids = ((string)ViewState["idsForPanel"]).Split('|');

        foreach (string id in ids)
        {
            if (id != "")
            {
                EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(new Guid(id));

                EffieJuryManagementApp.InvitationList juryInvList = EffieJuryManagementApp.InvitationList.GetInvitationList(jury.Id, EffieJuryManagementApp.Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value);

                if (juryInvList.Count > 0)
                {
                    juryInvList[0].IsRound1Assigned = !String.IsNullOrEmpty(cblRound1.SelectedValue.Trim());
                    juryInvList[0].IsRound2Assigned = !String.IsNullOrEmpty(cblRound2.SelectedValue.Trim());

                    juryInvList[0].DateModifiedString = DateTime.Now.ToString();
                    juryInvList[0].Save();


                    jury.EffieExpYear = EffieJuryManagementApp.Jury.GetEffieExperienceYears(jury, juryInvList[0]);
                }


                if (!jury.IsNew)
                {
                    // Ok update the panels
                    jury.Round1PanelId = GeneralFunction.GetValueCheckBoxList(cblRound1, "|");
                    jury.Round2PanelId = GeneralFunction.GetValueCheckBoxList(cblRound2, "|");

                    lbError.Text = "Panel assignment updated.";
                }

                jury.Save();
            }
        }
        
        Response.Redirect(GeneralFunction.GetRedirect("../Admin/JuryList.aspx?r=" + round));
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(GeneralFunction.GetRedirect("../Admin/JuryList.aspx?r=" + round));
    }
    #endregion
}