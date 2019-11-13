using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using Effie2017.App;

public partial class Main_UploadConfirm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    protected void LoadForm()
    {
        if (Request.UserAgent.Contains("; MSIE 7.") || Request.UserAgent.Contains("; MSIE 8."))
            ltrForCompatibility.Text = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=7\">";

        //Write Your Item Binding Code Here
        if (Request["md"] == null)
            Response.Redirect("../User/Login.aspx");
    }

    protected void PopulateForm()
    {
        if (Page.Request["id"] != null)
        {
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(GeneralFunction.GetValueGuid(Request["id"].ToString(), true));

            //-------
            try {
                EntryForm entryForm = EntryForm.GetEntryForm(Guid.Empty, entry.Id);
                //string url = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + ".aspx?Id=" + GeneralFunction.StringEncryption(entry.Id.ToString());
                string url = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?Id=" + entry.Id;
                lnkFileDownload0.NavigateUrl = url;
            }
            catch { }
            
            //-------
            if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\" + entry.Serial + "_AuthorizationForm_PDF.pdf"))
                lnkFileDownload2.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Authorisation/" + entry.Id.ToString() + "/" + entry.Serial + "_AuthorizationForm_PDF.pdf";

            //-------
            if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpg"))
                lnkFileDownload3.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpg";
            else if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpeg"))
                lnkFileDownload3.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpeg";

            //-------
            if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterials_PDF.pdf"))
            {
                tblFile4.Visible = true;
                lnkFileDownload4.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterials_PDF.pdf";
            }

            if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\"  + entry.Serial + "_CreativeMaterials_Video.mp4"))
            {
                tblFile5.Visible = true;
                //lnkFileDownload5.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/CreativeVideo/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterials_Video.mp4";

                lnkFileDownload5.NavigateUrl = "../Video/DownloadMedia.aspx?filePath=" + System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + "&MediaID=" + entry.Serial + "_CreativeMaterials_Video.mp4";
            }
            //if (GeneralFunction.FileExistsInAmazonS3(System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"], entry.Serial + "_CreativeMaterials_Video.mp4"))
            //{
            //    tblFile5.Visible = true;
            //    lnkFileDownload5.NavigateUrl = "../Video/DownloadMedia.aspx?BucketID=" + System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"] + "&MediaID=" + entry.Serial + "_CreativeMaterials_Video.mp4";
            //}
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        if (!chkFileViewed0.Checked ||  !chkFileViewed2.Checked || !chkFileViewed3.Checked)
            lblError.Text = "Please preview the files before confirm.<br>";

        if (lblError.Text == "")
        {
            if ((tblFile4.Visible && !chkFileViewed4.Checked) || (tblFile5.Visible && !chkFileViewed5.Checked))
                lblError.Text = "Please preview the files before confirm.<br>";
        }

        if (Page.Request["id"] == null)
        {
        }

        if (lblError.Text == "")
        {
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(GeneralFunction.GetValueGuid(Request["id"].ToString(), true));

            if (string.IsNullOrEmpty(entry.ProcessingStatus))
                entry.ProcessingStatus = StatusEntry.PendingVerification;
            //else if (entry.ProcessingStatus == StatusEntry.Reopened)
            //    entry.ProcessingStatus = StatusEntry.Completed;

            entry.Status = StatusEntry.Completed;
            entry.DateModifiedString = DateTime.Now.ToString();
            entry.MaterialsSubmitted = DateTime.Now.ToString();
            entry.Save();

            ltrJs.Text = "<script type=\"text/javascript\"> FancyClose(); </script>";
        }
    }
}