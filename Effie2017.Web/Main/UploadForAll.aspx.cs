using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

public partial class Main_UploadForAll : System.Web.UI.Page
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

        radProgArea.ProgressIndicators = Telerik.Web.UI.Upload.ProgressIndicators.TotalProgressBar |
                                            Telerik.Web.UI.Upload.ProgressIndicators.TotalProgress |
                                            Telerik.Web.UI.Upload.ProgressIndicators.TotalProgressPercent |
                                            Telerik.Web.UI.Upload.ProgressIndicators.RequestSize |
                                            Telerik.Web.UI.Upload.ProgressIndicators.FilesCountBar |
                                            Telerik.Web.UI.Upload.ProgressIndicators.FilesCount |
                                            Telerik.Web.UI.Upload.ProgressIndicators.FilesCountPercent |
                                            Telerik.Web.UI.Upload.ProgressIndicators.SelectedFilesCount |
                                            Telerik.Web.UI.Upload.ProgressIndicators.CurrentFileName |
                                            Telerik.Web.UI.Upload.ProgressIndicators.TimeElapsed |
                                            Telerik.Web.UI.Upload.ProgressIndicators.TimeEstimated |
                                            Telerik.Web.UI.Upload.ProgressIndicators.TransferSpeed;
        
        Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(GeneralFunction.GetValueGuid(Request["id"].ToString(), true));

        if (Request["md"].ToString() == "UE")
        {
            lblTitle.Text = "Entry Forms";
            lblDesc.Text = "Please submit the Entry Form in both PDF and MS Word format. Each file must not exceed 1MB in size.";

            ucGen_UploadFileForAll1.fieldName = "Upload PDF *";
            ucGen_UploadFileForAll1.uploadFileMsg = "PDF format (max file size 1mb)";
            ucGen_UploadFileForAll1.fileType = "PDF";
            ucGen_UploadFileForAll1.maxSize = 1048576;
            ucGen_UploadFileForAll1.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll1.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll2.fieldName = "Upload Word *";
            ucGen_UploadFileForAll2.uploadFileMsg = "MS Word fomat (max file size 1 mb)";
            ucGen_UploadFileForAll2.fileType = "Word";
            ucGen_UploadFileForAll2.maxSize = 1048576;
            ucGen_UploadFileForAll2.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll2.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/";

            lblRequired.Text = "* both files are required";
        }
        else if (Request["md"].ToString() == "UA")
        {
            lblTitle.Text = "Authorisation Form";
            lblDesc.Text = "Please submit the Authorization Form in PDF format. The file must not exceed 1MB in size.";

            ucGen_UploadFileForAll1.fieldName = "Upload PDF *";
            ucGen_UploadFileForAll1.uploadFileMsg = "PDF format (max file size 1mb)";
            ucGen_UploadFileForAll1.fileType = "PDF";
            ucGen_UploadFileForAll1.maxSize = 1048576;
            ucGen_UploadFileForAll1.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll1.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Authorisation/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll2.Visible = false;

            lblRequired.Text = "* file is required";

            radProgArea.ProgressIndicators = Telerik.Web.UI.Upload.ProgressIndicators.TotalProgressBar |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TotalProgress |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TotalProgressPercent |
                                                Telerik.Web.UI.Upload.ProgressIndicators.RequestSize |
                                                Telerik.Web.UI.Upload.ProgressIndicators.CurrentFileName |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TimeElapsed |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TimeEstimated |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TransferSpeed;
        }
        else if (Request["md"].ToString() == "UC")
        {
            lblTitle.Text = "Case Image";
            lblDesc.Text = "Upload an image that best represents your case. The case image will be used for promotional purposes (eg. Awards Journal, Winners Showcase, etc). Image file must be in JPG format, not exceeding 1MB in size.";

            ucGen_UploadFileForAll1.fieldName = "Upload JPG *";
            ucGen_UploadFileForAll1.uploadFileMsg = "JPG format (max file size 1mb)";
            ucGen_UploadFileForAll1.fileType = "JPG";
            ucGen_UploadFileForAll1.maxSize = 1048576;
            ucGen_UploadFileForAll1.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll1.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll2.Visible = false;

            lblRequired.Text = "* file is required";

            radProgArea.ProgressIndicators = Telerik.Web.UI.Upload.ProgressIndicators.TotalProgressBar |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TotalProgress |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TotalProgressPercent |
                                                Telerik.Web.UI.Upload.ProgressIndicators.RequestSize |
                                                Telerik.Web.UI.Upload.ProgressIndicators.CurrentFileName |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TimeElapsed |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TimeEstimated |
                                                Telerik.Web.UI.Upload.ProgressIndicators.TransferSpeed;
        }
        else if (Request["md"].ToString() == "UCr")
        {
            lblTitle.Text = "Creative Materials";
            lblDesc.Text = "You may select one of the following options for the submission of Creative Materials, depending on the requirements and nature of your case. The Creative Materials should showcase the integral communication touch points mentioned in the written case.";

            ucGen_UploadFileForAll1.fieldName = "Upload PDF *";
            ucGen_UploadFileForAll1.uploadFileMsg = "PDF format (max file size 5mb)";
            ucGen_UploadFileForAll1.fileType = "PDF";
            ucGen_UploadFileForAll1.maxSize = 41943040;
            ucGen_UploadFileForAll1.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll1.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll2.fieldName = "Upload Video *";
            ucGen_UploadFileForAll2.uploadFileMsg = "mp4 format (max file size 200mb)";
            ucGen_UploadFileForAll2.fileType = "mp4";
            ucGen_UploadFileForAll2.maxSize = 208666624;
            ucGen_UploadFileForAll2.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll2.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/";

            lblRequired.Text = "* both files are required";
        }
    }

    protected void PopulateForm()
    {
        if (Page.Request["id"] != null)
        {
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        lblError.Text += ucGen_UploadFileForAll1.ValidateForm();

        if (Request["md"].ToString() == "UE" || Request["md"].ToString() == "UCr")
        {
            if (lblError.Text.Contains("required") && ucGen_UploadFileForAll2.ValidateForm().Contains("required"))
            {
            }
            else
                lblError.Text += ucGen_UploadFileForAll2.ValidateForm();
        }

        if (Page.Request["id"] == null)
        {
        }

        if (lblError.Text != "")
            lblError.Text += "Please upload the correct file type.";

        if (lblError.Text == "")
        {
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(GeneralFunction.GetValueGuid(Request["id"].ToString(), true));

            if (Request["md"].ToString() == "UE")
            {
                GeneralFunction.DeleteDirectory(ucGen_UploadFileForAll1.saveDirectory, false);

                while (Directory.Exists(ucGen_UploadFileForAll1.saveDirectory))
                {
                    System.Threading.Thread.Sleep(300);
                }

                ucGen_UploadFileForAll1.SaveAndUpdateFile(entry.Serial + "_EntryForm_PDF");
                ucGen_UploadFileForAll2.SaveAndUpdateFile(entry.Serial + "_EntryForm_Word");

                if (ucGen_UploadFileForAll1.GetValue("").ToLower() != (entry.Serial + "_EntryForm_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_EntryForm_PDF")).ToLower())
                {
                    File.Copy(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_EntryForm_PDF"), ucGen_UploadFileForAll1.saveDirectory + entry.Serial + "_EntryForm_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_EntryForm_PDF"), true);
                    GeneralFunction.DeleteFile(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_EntryForm_PDF"), false);
                }
                if (ucGen_UploadFileForAll2.GetValue("").ToLower() != (entry.Serial + "_EntryForm_Word." + ucGen_UploadFileForAll2.GetValueExtension("_EntryForm_Word")).ToLower())
                {
                    File.Copy(ucGen_UploadFileForAll2.saveDirectory + ucGen_UploadFileForAll2.GetValue(entry.Serial + "_EntryForm_Word"), ucGen_UploadFileForAll2.saveDirectory + entry.Serial + "_EntryForm_Word." + ucGen_UploadFileForAll2.GetValueExtension("_EntryForm_Word"), true);
                    GeneralFunction.DeleteFile(ucGen_UploadFileForAll2.saveDirectory + ucGen_UploadFileForAll2.GetValue(entry.Serial + "_EntryForm_Word"), false);
                }

                ucGen_UploadFileForAll1.SetValue(entry.Serial + "_EntryForm_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_EntryForm_PDF"));
                ucGen_UploadFileForAll2.SetValue(entry.Serial + "_EntryForm_Word." + ucGen_UploadFileForAll2.GetValueExtension("_EntryForm_Word"));
            }
            else if (Request["md"].ToString() == "UA")
            {
                GeneralFunction.DeleteDirectory(ucGen_UploadFileForAll1.saveDirectory, false);

                while (Directory.Exists(ucGen_UploadFileForAll1.saveDirectory))
                {
                    System.Threading.Thread.Sleep(300);
                }

                ucGen_UploadFileForAll1.SaveAndUpdateFile(entry.Serial + "_AuthorizationForm_PDF");

                if (ucGen_UploadFileForAll1.GetValue("").ToLower() != (entry.Serial + "_AuthorizationForm_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_AuthorizationForm_PDF")).ToLower())
                {
                    File.Copy(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_AuthorizationForm_PDF"), ucGen_UploadFileForAll1.saveDirectory + entry.Serial + "_AuthorizationForm_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_AuthorizationForm_PDF"), true);
                    GeneralFunction.DeleteFile(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_AuthorizationForm_PDF"), false);
                }

                ucGen_UploadFileForAll1.SetValue(entry.Serial + "_AuthorizationForm_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_AuthorizationForm_PDF"));
            }
            else if (Request["md"].ToString() == "UC")
            {
                GeneralFunction.DeleteDirectory(ucGen_UploadFileForAll1.saveDirectory, false);

                while (Directory.Exists(ucGen_UploadFileForAll1.saveDirectory))
                {
                    System.Threading.Thread.Sleep(300);
                }

                ucGen_UploadFileForAll1.SaveAndUpdateFile(entry.Serial + "_CaseImage");

                if (ucGen_UploadFileForAll1.GetValue("").ToLower() != (entry.Serial + "_CaseImage." + ucGen_UploadFileForAll1.GetValueExtension("_CaseImage")).ToLower())
                {
                    File.Copy(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_CaseImage"), ucGen_UploadFileForAll1.saveDirectory + entry.Serial + "_CaseImage." + ucGen_UploadFileForAll1.GetValueExtension("_CaseImage"), true);
                    GeneralFunction.DeleteFile(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_CaseImage"), false);
                }

                ucGen_UploadFileForAll1.SetValue(entry.Serial + "_CaseImage." + ucGen_UploadFileForAll1.GetValueExtension("_CaseImage"));
            }
            else if (Request["md"].ToString() == "UCr")
            {
                ucGen_UploadFileForAll1.SaveAndUpdateFile(entry.Serial + "_CreativeMaterials_PDF");
                ucGen_UploadFileForAll2.SaveAndUpdateFile(entry.Serial + "_CreativeMaterials_Video");

                File.Copy(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_CreativeMaterials_PDF"), ucGen_UploadFileForAll1.saveDirectory + entry.Serial + "_CreativeMaterials_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_CreativeMaterials_PDF"), true);
                GeneralFunction.DeleteFile(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_CreativeMaterials_PDF"), false);
                File.Copy(ucGen_UploadFileForAll2.saveDirectory + ucGen_UploadFileForAll2.GetValue(entry.Serial + "_CreativeMaterials_Video"), ucGen_UploadFileForAll2.saveDirectory + entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll2.GetValueExtension("_CreativeMaterials_Video"), true);
                GeneralFunction.DeleteFile(ucGen_UploadFileForAll2.saveDirectory + ucGen_UploadFileForAll2.GetValue(entry.Serial + "_CreativeMaterials_Video"), false);

                ucGen_UploadFileForAll1.SetValue(entry.Serial + "_CreativeMaterials_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_CreativeMaterials_PDF"));
                ucGen_UploadFileForAll2.SetValue(entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll2.GetValueExtension("_CreativeMaterials_Video"));
            }

            lblDoneMsg.Text = "Upload completed.";
            btnSubmitDummy.Visible = false;
            btnCloseDummy.Visible = true;

            if (!GeneralFunction.IsAllowUploadFiles())
            {
                if (GeneralFunction.isAllMaterialUploaded(entry))
                {
                    entry.Status = StatusEntry.UploadCompleted;
                    entry.DateModifiedString = DateTime.Now.ToString();
                    entry.Save();
                }
            }
        }
    }
}