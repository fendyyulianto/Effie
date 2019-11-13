using System;
using System.Web.UI.WebControls;
using System.Linq;
using Telerik.Web.UI;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;
using Effie2017.App;

public partial class Admin_FullPageEmailSentHistory : System.Web.UI.Page
{
    Guid RegistrationId = Guid.Empty;
    string RegistrationIdString = string.Empty;
    string templateMode = string.Empty;
    string eventYear = string.Empty;
    string EmailType = string.Empty;
    string EntryId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {       
        RegistrationIdString = Request.QueryString["regId"];
        templateMode = Request.QueryString["tempMode"];
        eventYear = Request.QueryString["eventYear"];
        EmailType = Request.QueryString["EmailType"];
        EntryId = Request.QueryString["EntryId"];

        if (RegistrationIdString != null)
        {
            RegistrationId = IptechLib.Validation.GetValueGuid(RegistrationIdString, false);
        }
        else
            RegistrationId = Guid.Empty;

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    public void LoadForm()
    {
        btnBack.Visible = !RegistrationId.Equals(Guid.Empty);
    }

    public void PopulateForm()
    {        
        BindGrid(true);        
    }

    private void BindGrid(bool needRebind)
    {
        List<RegistrationEmailSent> emailSentList = new List<RegistrationEmailSent>();
        List<RegistrationEmailSent> filteredList = new List<RegistrationEmailSent>();

        if (RegistrationId == Guid.Empty)
            emailSentList = RegistrationEmailSentList.GetRegistrationEmailSentList().ToList();
        else
        {
            filteredList = RegistrationEmailSentList.GetRegistrationEmailSentList(Guid.Empty, RegistrationId).ToList();

            foreach (RegistrationEmailSent emailsent in filteredList)
            {
                EmailTemplate emailtemp = null;

                try
                {
                    emailtemp = EmailTemplate.GetEmailTemplate(emailsent.TemplateId);
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

        if (!string.IsNullOrEmpty(EmailType))
        {
            emailSentList = emailSentList.Where(x => x.EntryType == EmailType).ToList();
        }
        
        radGridTemplateHistory.DataSource = emailSentList.OrderByDescending(m => m.DateCreated).ToList();
        if (needRebind) radGridTemplateHistory.Rebind();
    }

    #region Events

    protected void radGridTemplateHistory_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            RegistrationEmailSent registrationEmailSent = (RegistrationEmailSent)e.Item.DataItem;

            if (registrationEmailSent != null)
            {
                Registration registration = Registration.GetRegistration(registrationEmailSent.RegistrationId);

               
                if (registration != null)
                {
                    Label lblType = (Label)e.Item.FindControl("lblType");
                    lblType.Text = registration.Status;

                    //LinkButton lnkBtnRegistrationId = (LinkButton)e.Item.FindControl("lnkBtnRegistrationId");
                    //lnkBtnRegistrationId.Text = registration.Id.ToString();
                    //lnkBtnRegistrationId.CommandArgument = registration.Id.ToString();

                    HyperLink lnk = (HyperLink)e.Item.FindControl("lnkRegistrationName");
                    lnk.Text = registration.Firstname + " " + registration.Lastname;
                    lnk.NavigateUrl = "mailto:" + registration.Email;
               
                }
                
                Label lblEmailTemplate = (Label)e.Item.FindControl("lblEmailTemplate");
                lblEmailTemplate.Text = registrationEmailSent.TemplateName;
            }
        }
    }

    protected void radGridTemplateHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdhocInvoiceList.aspx");
    }

    protected void radGridTemplateHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lbError.Text = string.Empty;

        //if (e.CommandName == "ViewRegistration")
        //{
        //    //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
        //    //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
        //    Response.Redirect("../Main/Registration.aspx?RegistrationId=" + e.CommandArgument.ToString());
        //}
    }

    #endregion
}