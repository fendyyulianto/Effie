using System;
using System.Web.UI.WebControls;
using System.Linq;
using EffieJuryManagementApp;
using Telerik.Web.UI;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;

public partial class Main_EmailTemplateList : PageSecurity_Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    private void PopulateForm()
    {
        BindGrid(true);
        if (Security.IsRoleReadOnlyAdmin())
        {
            newJury.Visible = false;
        }
    }

    private void LoadForm()
    {

    }

    private void BindGrid(bool needRebind)
    {
        EmailTemplateList emailTemplateList = EmailTemplateList.GetEmailTemplateList();

        radGridJury.DataSource = emailTemplateList.Where(m => m.TemplateId != new Guid(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultTemplateId")[0].Value) && !m.IsDelete).OrderByDescending(m => m.DateCreated).ToList();

        if (needRebind) radGridJury.Rebind();
    }

    #region Events

    protected void radGridJury_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            EmailTemplate emailTemplate = (EmailTemplate)e.Item.DataItem;

            if (emailTemplate != null)
            {
                LinkButton lnkBtn = null;
                HyperLink hlk = null;

                EmailTemplate parentTemplate = EmailTemplate.GetEmailTemplate(emailTemplate.TemplateId);

                try
                {
                    if (parentTemplate != null)
                    {
                        Label lblTempalteName = (Label)e.Item.FindControl("lblTempalteName");
                        lblTempalteName.Text = parentTemplate.Title;
                    }
                }
                catch { }


                lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
                lnkBtn.CommandArgument = emailTemplate.Id.ToString();

                lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnDelete");
                lnkBtn.CommandArgument = emailTemplate.Id.ToString();
                if (Security.IsRoleReadOnlyAdmin())
                {
                    lnkBtn.Visible = false;
                }

                hlk = (HyperLink)e.Item.FindControl("hlkPreview");
                hlk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                hlk.NavigateUrl = "./EmailPreview.aspx?etmId=" + emailTemplate.Id.ToString();


                lnkBtn = (LinkButton)e.Item.FindControl("hlCloneTamplate");
                lnkBtn.CommandArgument = emailTemplate.Id.ToString();
                if (Security.IsRoleReadOnlyAdmin())
                {
                    lnkBtn.Visible = false;
                }
            }

        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            PageSizeCombo.Items.Clear();
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
        }
    }

    protected void radGridJury_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lbError.Text = "";

        if (e.CommandName == "edit")
        {
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            Response.Redirect("../Main/EmailTemplate.aspx?etmId=" + e.CommandArgument.ToString());
        }
        if (e.CommandName == "delete")
        {
            EmailTemplate tempToDelete = EmailTemplate.GetEmailTemplate(new Guid(e.CommandArgument.ToString()));

            tempToDelete.IsDelete = true;
            tempToDelete.DateModifiedString = DateTime.Now.ToString();
            tempToDelete.Save();
        }
        if (e.CommandName == "CloneTamplate")
        {
            EmailTemplate emailTemplate = EmailTemplate.GetEmailTemplate(new Guid(e.CommandArgument.ToString()));
            CloneTamplate(emailTemplate);
        }

        PopulateForm();
    }


    protected void CloneTamplate(EmailTemplate emailTemplate)
    {

        int RSIndex = emailTemplate.Title.LastIndexOf(" - RSVP");
        int UpdateIndex = emailTemplate.Title.LastIndexOf(" - Update");
        int Clone = emailTemplate.Title.LastIndexOf(" - Clone");
        string TamplateName = emailTemplate.Title + " - " + DateTime.Now.ToString("dd MMM yyyy HH:mm");
        if (RSIndex != -1)
        {
            TamplateName = emailTemplate.Title.Substring(0, RSIndex) + " - Clone " + DateTime.Now.ToString("dd MMM yyyy HH:mm");
        }
        else if (UpdateIndex != -1)
        {
            TamplateName = emailTemplate.Title.Substring(0, UpdateIndex) + " - Clone " + DateTime.Now.ToString("dd MMM yyyy HH:mm");
        }
        else if (Clone != -1)
        {
            TamplateName = emailTemplate.Title.Substring(0, Clone) + " - Clone " + DateTime.Now.ToString("dd MMM yyyy HH:mm");
        }
        else
        {
            TamplateName = emailTemplate.Title + " - Clone " + DateTime.Now.ToString("dd MMM yyyy HH:mm");
        }

        //EmailTemplate standardTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));
        EmailTemplate NewEmailTemplate = EmailTemplate.NewEmailTemplate();
        NewEmailTemplate.Body = emailTemplate.Body;
        NewEmailTemplate.DateCreatedString = DateTime.Now.ToString();
        NewEmailTemplate.DateModifiedString = DateTime.Now.ToString();
        NewEmailTemplate.UserData1 = emailTemplate.UserData1;
        NewEmailTemplate.UserData2 = emailTemplate.UserData2;
        NewEmailTemplate.UserData3 = emailTemplate.UserData3;
        NewEmailTemplate.Title = TamplateName;
        NewEmailTemplate.TemplateId = emailTemplate.TemplateId;
        NewEmailTemplate.IsActive = emailTemplate.IsActive;
        NewEmailTemplate.IsDelete = emailTemplate.IsDelete;
        NewEmailTemplate.IsInvitation = emailTemplate.IsInvitation;
        NewEmailTemplate.Subject = emailTemplate.Subject;

        if (NewEmailTemplate.IsValid)
            NewEmailTemplate.Save();

        PopulateForm();
    }

    protected void radGridJury_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
    }

    #endregion

    #region Helper



    #endregion
}