using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;

public partial class Thankyou : System.Web.UI.Page
{
    public string juryIdString = string.Empty;
    public string rounds = string.Empty;
    public string requestString = string.Empty;
    Guid juryId = Guid.Empty;
    Jury jury;

    protected void Page_Load(object sender, EventArgs e)
    {
        juryIdString = Request.QueryString["jId"];
        rounds = Request.QueryString["rounds"];
        requestString = Request.QueryString["request"];

        if (GeneralFunction.IsInvitationDateCutOff())
            pnlDeadline.Visible = true;
        else
        {
            if (juryIdString != null && rounds != null && requestString != null)
            {
                if (GeneralFunction.ValidateGuid(GeneralFunction.GetValueGuid(juryIdString, true).ToString()))
                {
                    juryId = GeneralFunction.GetValueGuid(juryIdString, true);

                    jury = Jury.GetJury(juryId);

                    if (!IsPostBack)
                    {
                        LoadForm();
                        PopulateForm(jury);
                    }
                }
                else
                    return;
            }
            else
                return;
        }
    }

    private void LoadForm()
    {

    }

    private void PopulateForm(Jury jury)
    {
        if (jury != null)
        {
            InvitationList inv = InvitationList.GetInvitationList(jury.Id, Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value);

            if (inv.Count > 0)
            {
                if (!inv[0].IsLocked)
                {
                    string[] roundsArray = null;
                    try
                    {
                        roundsArray = GeneralFunction.StringDecryption(rounds).ToString().Trim().Split('|').ToArray();
                    }
                    catch
                    {

                    }

                    string evetnYear = string.Empty;
                    try
                    {
                        evetnYear = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value;
                    }
                    catch { }

                    if (roundsArray != null)
                    {
                        if (GeneralFunction.StringDecryption(requestString).Trim().ToLower().Equals("yes"))
                        {
                            foreach (string round in roundsArray)
                            {
                                if (!String.IsNullOrEmpty(round))
                                {
                                    if (Convert.ToInt32(round) == 1)
                                    {
                                        inv[0].IsRound1Accepted = true;
                                    }
                                    if (Convert.ToInt32(round) == 2)
                                    {
                                        inv[0].IsRound2Accepted = true;
                                    }                                    
                                }
                            }

                            EmailTemplate updateProfileTemplate = EmailTemplate.GetEmailTemplate(new Guid(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultUpdateProfileTemplateId")[0].Value));
                            if (updateProfileTemplate != null)
                            {
                                Email.SendTemplateEmail(jury, updateProfileTemplate.Id);
                                GeneralFunction.SaveEmailSentLog(jury, updateProfileTemplate.Id, evetnYear);
                            }

                            pnlSuccess.Visible = true;
                        }
                        else
                        {
                            foreach (string round in roundsArray)
                            {
                                if (!String.IsNullOrEmpty(round))
                                {
                                    if (Convert.ToInt32(round) == 1)
                                    {
                                        inv[0].IsRound1Accepted = false;
                                    }
                                    if (Convert.ToInt32(round) == 2)
                                    {
                                        inv[0].IsRound2Accepted = false;
                                    }                                    
                                }
                            }

                            pnlReject.Visible = true;
                        }

                        inv[0].IsLocked = true;   //Only one time Jury can give his response
                        inv[0].IsDeclined = GeneralFunction.StringDecryption(requestString).Trim().ToLower().Equals("no");

                        inv[0].Save();
                    }
                }
                else
                {
                    pnlLock.Visible = true;
                }
            }
        }
    }
}