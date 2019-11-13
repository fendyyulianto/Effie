using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO;
using HtmlAgilityPack;
using System.Text;
using Effie2017.App;

public partial class Admin_EmailTemplate : PageSecurity_Admin
{
    EmailTemplate emailTemplate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["etmId"] != null)
            emailTemplate = EmailTemplate.GetEmailTemplate(new Guid(Request.QueryString["etmId"]));
        else
            emailTemplate = EmailTemplate.NewEmailTemplate();


        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    public void LoadForm()
    {
        EmailTemplateList defaultEmailTempalteList = EmailTemplateList.GetEmailTemplateList(new Guid(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultTemplateId")[0].Value));

        ddlTemplateList.DataSource = defaultEmailTempalteList;
        ddlTemplateList.DataTextField = "Title";
        ddlTemplateList.DataValueField = "Id";
        ddlTemplateList.DataBind();

        ddlTemplateList.Items.Insert(0, new ListItem("Please Select", Guid.Empty.ToString()));

        ddlEmailType.Items.Add(new ListItem("Please Select", ""));
        ddlEmailType.Items.Add(new ListItem(EmailType.DQ.ToString(), EmailType.DQ.ToString()));
        ddlEmailType.Items.Add(new ListItem(EmailType.Entry.ToString(), EmailType.Entry.ToString()));
        ddlEmailType.Items.Add(new ListItem(EmailType.Invoice.ToString(), EmailType.Invoice.ToString()));
    }

    public void PopulateForm()
    {
        lbTitle.Text = "Add Email Template";
        if (!emailTemplate.IsNew)
        {
            lbTitle.Text = "Edit Email Template";            
        }

        templateRow.Visible = emailTemplate.IsNew;

        if (!emailTemplate.IsNew)
        {            
            txtTemplateName.Text = emailTemplate.Title;
            txtTemplateSubject.Text = emailTemplate.Subject;
            rEditorBody.Content = GeneralFunction.CheckPlaceHolders(emailTemplate.Body, true);
            chkActive.Checked = emailTemplate.IsActive;
            chkInvitation.Checked = emailTemplate.IsInvitation;
            hdfRounds.Value = emailTemplate.UserData1;
            hdfEmailCategory.Value = emailTemplate.UserData2;
            ddlEmailType.SelectedValue = emailTemplate.EmailType;
        }
    }

    public bool ValidateForm()
    {
        lbError.Text = string.Empty;

        lbError.Text += IptechLib.Validation.ValidateDropDownList("Default Template", ddlTemplateList, emailTemplate.IsNew, Guid.Empty.ToString());
        lbError.Text += IptechLib.Validation.ValidateTextBox("Template Name", txtTemplateName, true, IptechLib.ValidationType.String);
        lbError.Text += IptechLib.Validation.ValidateTextBox("Subject", txtTemplateSubject, true, IptechLib.ValidationType.String);
        lbError.Text += GeneralFunction.ValidateDropDownList("Email Type", ddlEmailType, true, "");
        
        if (String.IsNullOrEmpty(rEditorBody.Text))
            lbError.Text += "Body required.<br/>";

        return String.IsNullOrEmpty(lbError.Text);
    }

    public EmailTemplate SaveForm()
    {
        if (ValidateForm())
        {
            emailTemplate.Subject = txtTemplateSubject.Text;
            emailTemplate.Title = txtTemplateName.Text;            
            emailTemplate.Body = GeneralFunction.CheckPlaceHolders(rEditorBody.Content,true);
            emailTemplate.IsActive = chkActive.Checked;
            emailTemplate.IsInvitation = chkInvitation.Checked;
            emailTemplate.UserData1 = hdfRounds.Value.ToString();
            emailTemplate.UserData2 = hdfEmailCategory.Value.ToString();
            emailTemplate.EmailType = ddlEmailType.SelectedValue;
            
            if (emailTemplate.IsNew)
            {
                emailTemplate.DateCreatedString = DateTime.Now.ToString();
                emailTemplate.TemplateId = new Guid(ddlTemplateList.SelectedValue.ToString());
            }

            emailTemplate.DateModifiedString = DateTime.Now.ToString();

            if (emailTemplate.IsValid)
                emailTemplate.Save();
            else
                return null;

            return emailTemplate;
        }
        else
            return null;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        EmailTemplate emtemp = SaveForm();

        if (emtemp != null)
        {
            Response.Redirect("../Admin/EmailTemplateList.aspx");
        }
    }
   
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Admin/EmailTemplateList.aspx");
    }

    protected void ddlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString()))
        {
            try
            {
                EmailTemplate standardTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));

                if (standardTemplate != null)
                {
                    rEditorBody.Content = standardTemplate.Body;
                    txtTemplateSubject.Text = standardTemplate.Subject;
                    chkInvitation.Checked = standardTemplate.IsInvitation;
                    hdfRounds.Value = standardTemplate.UserData1;
                    hdfEmailCategory.Value = standardTemplate.UserData2;
                }
            }
            catch { }
        }
    }

}