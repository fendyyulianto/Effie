using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Admin_MasterPageAdmin : System.Web.UI.MasterPage
{
    public static Administrator admin;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
        admin = Security.GetAdminLoginSession();
        if (admin.Access == "SA")
        {
            liEntryList.Visible = true;
            liEntryPendingList.Visible = true;
            liInvoiceList.Visible = true;
            liAdhocInvoiceList.Visible = true;
            liEntrySubmittedList.Visible = true;
            liDownloadMediaList.Visible = true;
            liEmailTemplateList.Visible = true;
            liUserList.Visible = true;
            liAdminList.Visible = true;
            liResetMasterPasswordList.Visible = true;
            liEntryProcessing.Visible = true;
            liFlagReasonCMSlist.Visible = true;
            liJuryList.Visible = true;
            liGalaOrderList.Visible = true;
        }
        else if (admin.Access == "SF")
        {
            liEntryList.Visible = true;
            liEntryPendingList.Visible = true;
            liInvoiceList.Visible = true;
            liAdhocInvoiceList.Visible = true;
            //liEntrySubmittedList.Visible = true;
            //liDownloadMediaList.Visible = true;
            //liEmailTemplateList.Visible = true;
            //liUserList.Visible = true;
            //liAdminList.Visible = true;
            //liEntryProcessing.Visible = true;
            //liFlagReasonCMSlist.Visible = true;
            //liJuryList.Visible = true;
            liGalaOrderList.Visible = true;
        }
        else if (admin.Access == "AD")
        {
            liEntryList.Visible = true;
            liEntryPendingList.Visible = true;
            liInvoiceList.Visible = true;
            liAdhocInvoiceList.Visible = true;
            liEntrySubmittedList.Visible = true;
            liDownloadMediaList.Visible = true;
            liEmailTemplateList.Visible = true;
            liUserList.Visible = true;
            //liAdminList.Visible = true;
            liEntryProcessing.Visible = true;
            liFlagReasonCMSlist.Visible = true;
            liJuryList.Visible = true;
            liGalaOrderList.Visible = true;
        }
        else if (admin.Access == "AD2")
        {
            liEntryList.Visible = true;
            liEntryPendingList.Visible = true;
            liInvoiceList.Visible = true;
            liAdhocInvoiceList.Visible = true;
            liEntrySubmittedList.Visible = true;
            liDownloadMediaList.Visible = true;
            liEmailTemplateList.Visible = true;
            liUserList.Visible = true;
            //liAdminList.Visible = true;
            liEntryProcessing.Visible = true;
            liFlagReasonCMSlist.Visible = true;
            //liJuryList.Visible = true;
            liGalaOrderList.Visible = true;
        }
        else if (admin.Access == "RO")
        {
            liEntryList.Visible = true;
            liEntryPendingList.Visible = true;
            liInvoiceList.Visible = true;
            liAdhocInvoiceList.Visible = true;
            liEntrySubmittedList.Visible = true;
            liDownloadMediaList.Visible = true;
            liEmailTemplateList.Visible = true;
            liUserList.Visible = true;
            //liAdminList.Visible = true;
            liEntryProcessing.Visible = true;
            liFlagReasonCMSlist.Visible = true;
            liJuryList.Visible = true;
            liGalaOrderList.Visible = true;
        }
        else if (admin.Access == "ST")
        {
            //liEntryList.Visible = true;
            //liEntryPendingList.Visible = true;
            //liInvoiceList.Visible = true;
            //liAdhocInvoiceList.Visible = true;
            //liEntrySubmittedList.Visible = true;
            //liDownloadMediaList.Visible = true;
            //liEmailTemplateList.Visible = true;
            //liUserList.Visible = true;
            ////liAdminList.Visible = true;
            //liEntryProcessing.Visible = true;
            //liFlagReasonCMSlist.Visible = true;
            //liJuryList.Visible = true;
            //liGalaOrderList.Visible = true;

            //liEntryList.Visible = true;
            //liEntryPendingList.Visible = true;
            //liInvoiceList.Visible = true;
            //liAdhocInvoiceList.Visible = true;
            liEntrySubmittedList.Visible = true;
            //liDownloadMediaList.Visible = true;
            //liEmailTemplateList.Visible = true;
            //liUserList.Visible = true;
            //liAdminList.Visible = true;
            liEntryProcessing.Visible = true;
            //liFlagReasonCMSlist.Visible = true;
            //liJuryList.Visible = true;
            //liGalaOrderList.Visible = true;
        }
    }
    protected void LoadForm()
    {
    }
    protected void PopulateForm()
    {
        Administrator admin = Security.GetAdminLoginSession();
        if (admin != null)
        {
            lblName.Text = admin.LoginId;
        }

        DateTime lastlogin = Security.GetLastLoginCache();
        if (lastlogin != DateTime.MaxValue)
        {
            lblSign.Text = lastlogin.ToString("dd/MM/yy hh:mm tt");
        }
    }
    protected void lnkBtnLogout_Click(object sender, EventArgs e)
    {
        Security.ResetAllSessions();

        //if (Request.Cookies["wdduw2013UserLogin"] != null)
        //    Response.Cookies["wdduw2013UserLogin"].Expires = DateTime.Now.AddMinutes(-1);

        Response.Redirect("../Admin/Login.aspx");
    }
}
