using System;
using System.Web.UI.WebControls;
using System.Linq;
using Telerik.Web.UI;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;
using Effie2017.App;

public partial class Admin_JuryEmailSentHistory : System.Web.UI.Page
{
    Guid juryId = Guid.Empty;
    string juryIdString = string.Empty;
    string templateMode = string.Empty;
    string eventYear = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {       
        juryIdString = Request.QueryString["JuryId"];
        templateMode = Request.QueryString["tempMode"];
        eventYear = Request.QueryString["eventYear"];

        if (juryIdString != null)
        {
            juryId = IptechLib.Validation.GetValueGuid(juryIdString, false);
        }
        else
            juryId = Guid.Empty;

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    public void LoadForm()
    {
        btnBack.Visible = !juryId.Equals(Guid.Empty);
    }

    public void PopulateForm()
    {        
        BindGrid(true);        
    }

    private void BindGrid(bool needRebind)
    {
        List<EffieJuryManagementApp.EmailSent> emailSentList = new List<EffieJuryManagementApp.EmailSent>();
        List<EffieJuryManagementApp.EmailSent> filteredList = new List<EffieJuryManagementApp.EmailSent>();

        if (juryId == Guid.Empty)
            emailSentList = EffieJuryManagementApp.EmailSentList.GetEmailSentList().ToList();
        else
        {
            filteredList = EffieJuryManagementApp.EmailSentList.GetEmailSentList(Guid.Empty, juryId).ToList();

            foreach (EffieJuryManagementApp.EmailSent emailsent in filteredList)
            {
                EffieJuryManagementApp.EmailTemplate emailtemp = null;

                try
                {
                    emailtemp = EffieJuryManagementApp.EmailTemplate.GetEmailTemplate(emailsent.TemplateId);
                }
                catch { emailtemp = null;}


                if (emailtemp != null)
                {
                    if (templateMode == null)
                    {
                        emailSentList.Add(emailsent);
                    }
                    else
                    {
                        if (templateMode.Equals("INV"))
                        {
                            if (emailtemp.UserData2.Equals(EmailCategory.Invitation))
                            {
                                if (!String.IsNullOrEmpty(eventYear))
                                {
                                    if (emailsent.EventYear.Equals(eventYear))
                                        emailSentList.Add(emailsent);
                                }
                                else
                                    emailSentList.Add(emailsent);
                            }
                        }                        
                    }
                }               
            }
        }

        radGridTemplateHistory.DataSource = emailSentList.OrderByDescending(m => m.DateCreated).ToList();
        if (needRebind) radGridTemplateHistory.Rebind();
    }

    #region Events

    protected void radGridTemplateHistory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            EffieJuryManagementApp.EmailSent emailsent = (EffieJuryManagementApp.EmailSent)e.Item.DataItem;

            if (emailsent != null)
            {
                EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(emailsent.JuryId);
                
                if (jury != null)
                {
                    Label lblType = (Label)e.Item.FindControl("lblType");
                    lblType.Text = jury.Type;

                    LinkButton lnkBtnJuryId = (LinkButton)e.Item.FindControl("lnkBtnJuryId");
                    lnkBtnJuryId.Text = jury.SerialNo;
                    lnkBtnJuryId.CommandArgument = jury.Id.ToString();

                    HyperLink lnk = (HyperLink)e.Item.FindControl("lnkJuryName");
                    lnk.Text = jury.FirstName + " " + jury.LastName;
                    lnk.NavigateUrl = "mailto:" + jury.Email;
               
                }
                
                Label lblEmailTemplate = (Label)e.Item.FindControl("lblEmailTemplate");
                lblEmailTemplate.Text = emailsent.TemplateName;
            }
        }
    }

    protected void radGridTemplateHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
    }

    protected void radGridTemplateHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lbError.Text = string.Empty;

        //if (e.CommandName == "ViewJury")
        //{
        //    //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
        //    //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
        //    Response.Redirect("../Main/Jury.aspx?juryId=" + e.CommandArgument.ToString());
        //}
    }

    #endregion
}