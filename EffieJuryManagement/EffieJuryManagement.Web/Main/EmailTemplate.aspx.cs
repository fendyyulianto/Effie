using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;
using System.Data;
using System.Configuration;
using System.IO;
using HtmlAgilityPack;
using System.Text;


public partial class Main_EmailTemplate : PageSecurity_Admin
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
            rEditorBody.Content = CheckPlaceHolders(emailTemplate.Body, false);
            chkActive.Checked = emailTemplate.IsActive;
            chkInvitation.Checked = emailTemplate.IsInvitation;
            hdfRounds.Value = emailTemplate.UserData1;
            hdfEmailCategory.Value = emailTemplate.UserData2;
        }

        if (Security.IsRoleReadOnlyAdmin())
        {
            btnSubmit.Visible = false;
            GeneralFunction.DisableAllAction(this, false);
        }
    }

    public bool ValidateForm()
    {
        lbError.Text = string.Empty;

        lbError.Text += IptechLib.Validation.ValidateDropDownList("Default Template", ddlTemplateList, emailTemplate.IsNew, Guid.Empty.ToString());
        lbError.Text += IptechLib.Validation.ValidateTextBox("Template Name", txtTemplateName, true, IptechLib.ValidationType.String);
        lbError.Text += IptechLib.Validation.ValidateTextBox("Subject", txtTemplateSubject, true, IptechLib.ValidationType.String);

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
            emailTemplate.Body = CheckPlaceHolders(rEditorBody.Content,true);
            emailTemplate.IsActive = chkActive.Checked;
            emailTemplate.IsInvitation = chkInvitation.Checked;
            emailTemplate.UserData1 = hdfRounds.Value.ToString();
            emailTemplate.UserData2 = hdfEmailCategory.Value.ToString();

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
            Response.Redirect("../Main/EmailTemplateList.aspx");
        }
    }
   
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Main/EmailTemplateList.aspx");
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

    public string CheckPlaceHolders(string body,bool isSave)
    {
        string emailBody = body;

        string round1 = "<table  border='0' cellspacing='0' cellpadding='0' id='emailBodyR1' style='font-size: 15px;text-align: left; color: #ffffff;float:left;margin-right: 1%;'><tr><td width='38%' align='center' style='padding: 5px; background-color: #B5985A'><a href='#LINKAPPROVER1#' style='font-family: Arial, Helvetica, sans-serif; color: black;text-decoration: none; display: block; font-weight: bold'>YES<br />I would like to participate in Round 1 Judging.</a></td></tr></table>";
        string round2 = "<table  border='0' cellspacing='0' cellpadding='0' id='emailBodyR2' style='font-size: 15px;text-align: left; color: #ffffff;float:left;margin-right: 1%;'><tr><td width='38%' align='center' style='padding: 5px; background-color: #B5985A'><a href='#LINKAPPROVER2#' style='font-family: Arial, Helvetica, sans-serif; color: black;text-decoration: none; display: block; font-weight: bold'>YES<br />I would like to participate in Round 2 Judging.</a></td></tr></table>";
        string bothrounds = "<table  border='0' cellspacing='0' cellpadding='0' id='emailBodyR1R2' style='font-size: 15px;text-align: left; color: #ffffff;float:left;margin-right: 1%;'><tr><td width='38%' align='center' style='padding: 5px; background-color: #B5985A'><a href='#LINKAPPROVE#' style='font-family: Arial, Helvetica, sans-serif; color: black;text-decoration: none; display: block; font-weight: bold'>YES<br />I would like to participate in both Rounds.</a></td></tr></table>";
        string reject = "<table border='0' cellspacing='0' cellpadding='0' id='emailBodyNo' style='font-size: 15px;text-align: left; color: #ffffff;float:left;margin-right: 1%;'><tr><td width='38%' align='center' style='padding: 5px; background-color: #B3ACA9'><a href='#LINKREJECT#' style='font-family: Arial, Helvetica, sans-serif; color: black;text-decoration: none; display: block; font-weight: bold'>NO<br />I am unable to participate in Judging.</a></td></tr></table>";
        

        if (isSave)
        {
            emailBody = emailBody.Replace("#ROUND1#", round1);
            emailBody = emailBody.Replace("#ROUND2#", round2);
            emailBody = emailBody.Replace("#BOTHROUNDS#", bothrounds);
            emailBody = emailBody.Replace("#REJECT#", reject);
        }
        else
        {           
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(body);
            HtmlNodeCollection col = doc.DocumentNode.SelectNodes("//table");

            foreach (HtmlNode node in col)
            {
                if (node.Id.Equals("emailBodyR1"))
                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode("#ROUND1#"), node);
                if (node.Id.Equals("emailBodyR2"))
                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode("#ROUND2#"), node);
                if (node.Id.Equals("emailBodyR1R2"))
                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode("#BOTHROUNDS#"), node);
                if (node.Id.Equals("emailBodyNo"))
                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode("#REJECT#"), node);
            }

            emailBody = doc.DocumentNode.OuterHtml;
        }

        return emailBody;
    }
}