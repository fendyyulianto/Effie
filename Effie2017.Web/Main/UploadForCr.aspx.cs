using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Configuration;

public partial class Main_UploadForCr : System.Web.UI.Page
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
        
        Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(GeneralFunction.GetValueGuid(Request["id"].ToString(), true));
        string Minutes = "4";
        bool isSustained = (entry.CategoryPSDetail.IndexOf("Sustained Success") != -1);
        if (isSustained)
        {
            Minutes = "4";
        }
        else
        {
            Minutes = "3";
        }

        if (Request["md"].ToString() == "UE")
        {
            lblTitle.Text = "Entry Form File Uploads";
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
            lblTitle.Text = "Authorization Form File Upload";
            lblDesc.Text = "Please submit the Authorization Form in PDF format. The file must not exceed 1MB in size.";

            ucGen_UploadFileForAll1.fieldName = "Upload PDF *";
            ucGen_UploadFileForAll1.uploadFileMsg = "PDF format (max file size 1mb)";
            ucGen_UploadFileForAll1.fileType = "PDF";
            ucGen_UploadFileForAll1.maxSize = 1048576;
            ucGen_UploadFileForAll1.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll1.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Authorisation/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll2.Visible = false;

            lblRequired.Text = "* file is required";
        }
        else if (Request["md"].ToString() == "UC")
        {
            lblTitle.Text = "Case File Upload";
            lblDesc.Text = "Upload an image that best represents your case. The case image will be used for promotional purposes (eg. Awards Journal, Winners Showcase, etc). Image file must be in JPG format, not exceeding 1MB in size.";

            ucGen_UploadFileForAll1.fieldName = "Upload JPG *";
            ucGen_UploadFileForAll1.uploadFileMsg = "JPG format (max file size 1mb)";
            ucGen_UploadFileForAll1.fileType = "JPG";
            ucGen_UploadFileForAll1.maxSize = 1048576;
            ucGen_UploadFileForAll1.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll1.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll2.Visible = false;

            lblRequired.Text = "* file is required";
        }
        else if (Request["md"].ToString() == "UCr")
        {
            lblTitle.Text = "Creative Materials";
            lblDesc.Text = "You may select one of the following options for the submission of Creative Materials, depending on the requirements and nature of your case. The Creative Materials should showcase the integral communication touch points mentioned in the written case. If you are including non-English work, you must include translations either as subtitles in the creative work or a one-page PDF of the translations.<br>";

            lblTitle1.Text = "(Recommended for entries <span style=\"font-weight:bold;text-decoration:underline\">without</span> a TVC/video/audio component as part of the campaign.) Submit a maximum 12-slide powerpoint file, converted into PDF format and not exceeding 5MB in size.";
            ucGen_UploadFileForAll1.fieldName = "Upload PDF *";
            ucGen_UploadFileForAll1.uploadFileMsg = "PDF format (max file size 5mb)";
            ucGen_UploadFileForAll1.fileType = "PDF";
            ucGen_UploadFileForAll1.maxSize = 5242880;
            ucGen_UploadFileForAll1.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll1.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll1Trans.fieldName = "Upload Translation in PDF (optional)";
            ucGen_UploadFileForAll1Trans.uploadFileMsg = "PDF format (max file size 1mb)";
            ucGen_UploadFileForAll1Trans.fileType = "PDF";
            ucGen_UploadFileForAll1Trans.maxSize = 1048576;
            ucGen_UploadFileForAll1Trans.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll1Trans.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/";

            lblTitle2.Text = "(Recommended for entries with a TVC/video/audio component in its campaign.) Submit a Creative Video, no longer than " + Minutes + " minutes. Video files should be in .mp4 format and the file size for online upload must be limited to 200MB.";
            ucGen_UploadFileForAll2.fieldName = "Upload Video *";
            ucGen_UploadFileForAll2.uploadFileMsg = "mp4 format (max file size 200mb)";
            ucGen_UploadFileForAll2.fileType = "mp4";
            ucGen_UploadFileForAll2.createDirectory = false;
            ucGen_UploadFileForAll2.maxSize = 210658918;
            ucGen_UploadFileForAll2.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\";
            ucGen_UploadFileForAll2.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/CreativeVideo/";

            ucGen_UploadFileForAll2Trans.fieldName = "Upload Translation in PDF (optional)";
            ucGen_UploadFileForAll2Trans.uploadFileMsg = "PDF format (max file size 1mb)";
            ucGen_UploadFileForAll2Trans.fileType = "PDF";
            ucGen_UploadFileForAll2Trans.createDirectory = false;
            ucGen_UploadFileForAll2Trans.maxSize = 1048576;
            ucGen_UploadFileForAll2Trans.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll2Trans.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/";

            lblTitle3.Text = "Submit a Creative Video, no longer than "+ Minutes + " minutes. Video files should be in .mp4 format, with a max file size of 200MB. <br>In addition, supplement your video with still images of examples of work featured on your video. Images must be compiled into a powerpoint and uploaded as a single file in PDF format. File size must not exceed 2MB.";
            ucGen_UploadFileForAll3.fieldName = "Upload PDF *";
            ucGen_UploadFileForAll3.uploadFileMsg = "PDF format (max file size 2mb)";
            ucGen_UploadFileForAll3.fileType = "PDF";
            ucGen_UploadFileForAll3.maxSize = 2097152;
            ucGen_UploadFileForAll3.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll3.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll3Trans.fieldName = "Upload Translation in PDF (optional)";
            ucGen_UploadFileForAll3Trans.uploadFileMsg = "PDF format (max file size 1mb)";
            ucGen_UploadFileForAll3Trans.fileType = "PDF";
            ucGen_UploadFileForAll3Trans.maxSize = 1048576;
            ucGen_UploadFileForAll3Trans.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\";
            ucGen_UploadFileForAll3Trans.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/";

            ucGen_UploadFileForAll4.fieldName = "Upload Video *";
            ucGen_UploadFileForAll4.uploadFileMsg = "mp4 format (max file size 200mb)";
            ucGen_UploadFileForAll4.fileType = "mp4";
            ucGen_UploadFileForAll4.createDirectory = false;
            ucGen_UploadFileForAll4.maxSize = 210658918;
            ucGen_UploadFileForAll4.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\";
            ucGen_UploadFileForAll4.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/CreativeVideo/" ;

            lblRequired.Text = "* both files are required";
        }

        rdCreativeUploadType1.Text = Minutes + " min Creative Video only";
        rdCreativeUploadType2.Text = Minutes + " min Creative Video AND PDF with additional still images from Creative Video";
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

        //UCr Only
        if (Request["md"].ToString() == "UCr")
        {
            if (!rdCreativeUploadType0.Checked && !rdCreativeUploadType1.Checked && !rdCreativeUploadType2.Checked)
            {
                lblError.Text += "Selection is required.<br>";
            }

            if (rdCreativeUploadType0.Checked)
            {
                lblError.Text += ucGen_UploadFileForAll1.ValidateForm();
                lblError.Text += ucGen_UploadFileForAll1Trans.ValidateForm();
            }
            else if (rdCreativeUploadType1.Checked)
            {
                lblError.Text += ucGen_UploadFileForAll2.ValidateForm();
                lblError.Text += ucGen_UploadFileForAll2Trans.ValidateForm();
            }
            else if (rdCreativeUploadType2.Checked)
            {
                lblError.Text += ucGen_UploadFileForAll3.ValidateForm();

                if (lblError.Text.Contains("required") && ucGen_UploadFileForAll4.ValidateForm().Contains("required"))
                {
                }
                else
                    lblError.Text += ucGen_UploadFileForAll4.ValidateForm();

                lblError.Text += ucGen_UploadFileForAll3Trans.ValidateForm();
            }
        }

        if (Page.Request["id"] == null)
        {
        }

        if (lblError.Text != "")
            lblError.Text += "Please upload the correct file type.";

        if (lblError.Text == "")
        {
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(GeneralFunction.GetValueGuid(Request["id"].ToString(), true));

            if (Request["md"].ToString() == "UCr")
            {
                GeneralFunction.DeleteDirectory(ucGen_UploadFileForAll1.saveDirectory, false);

                while (Directory.Exists(ucGen_UploadFileForAll1.saveDirectory))
                {
                    System.Threading.Thread.Sleep(300);
                }

                if (rdCreativeUploadType0.Checked)
                {
                    ucGen_UploadFileForAll1.SaveAndUpdateFile(entry.Serial + "_CreativeMaterials_PDF");
                    ucGen_UploadFileForAll1Trans.SaveAndUpdateFile(entry.Serial + "_CreativeMaterialsTranslate_PDF");

                    if (ucGen_UploadFileForAll1.GetValue("").ToLower() != (entry.Serial + "_CreativeMaterials_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_CreativeMaterials_PDF")).ToLower())
                    {
                        File.Copy(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_CreativeMaterials_PDF"), ucGen_UploadFileForAll1.saveDirectory + entry.Serial + "_CreativeMaterials_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_CreativeMaterials_PDF"), true);
                        GeneralFunction.DeleteFile(ucGen_UploadFileForAll1.saveDirectory + ucGen_UploadFileForAll1.GetValue(entry.Serial + "_CreativeMaterials_PDF"), false);
                    }
                    if (ucGen_UploadFileForAll1Trans.HasFile() && ucGen_UploadFileForAll1Trans.GetValue("").ToLower() != (entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll1Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF")).ToLower())
                    {
                        File.Copy(ucGen_UploadFileForAll1Trans.saveDirectory + ucGen_UploadFileForAll1Trans.GetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF"), ucGen_UploadFileForAll1Trans.saveDirectory + entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll1Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF"), true);
                        GeneralFunction.DeleteFile(ucGen_UploadFileForAll1Trans.saveDirectory + ucGen_UploadFileForAll1Trans.GetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF"), false);
                    }

                    ucGen_UploadFileForAll1.SetValue(entry.Serial + "_CreativeMaterials_PDF." + ucGen_UploadFileForAll1.GetValueExtension("_CreativeMaterials_PDF"));
                    ucGen_UploadFileForAll1Trans.SetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll1Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF"));

                    entry.CreativeUploadType = "0";

                    //if (GeneralFunction.FileExistsInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Original"], entry.Serial + "_CreativeMaterials_Video.mp4"))
                    //{
                    //    GeneralFunction.DeleteFileInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Original"], entry.Serial + "_CreativeMaterials_Video.mp4");

                    //    if (GeneralFunction.FileExistsInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Small"], entry.Serial + "_CreativeMaterials_Video.mp4"))
                    //    {
                    //        GeneralFunction.DeleteFileInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Small"], entry.Serial + "_CreativeMaterials_Video.mp4");
                    //    }
                    //}
                    if (File.Exists(ConfigurationManager.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4"))
                    {
                        GeneralFunction.DeleteFile(ConfigurationManager.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4", true);
                    }
                }
                else if (rdCreativeUploadType1.Checked)
                {
                    ucGen_UploadFileForAll2.createDirectory = false;
                    ucGen_UploadFileForAll2.SaveAndUpdateFile(entry.Serial + "_CreativeMaterials_Video");
                    ucGen_UploadFileForAll2Trans.SaveAndUpdateFile(entry.Serial + "_CreativeMaterialsTranslate_PDF");

                    if (ucGen_UploadFileForAll2.GetValue("").ToLower() != (entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll2.GetValueExtension("_CreativeMaterials_Video")).ToLower())
                    {
                        File.Copy(ucGen_UploadFileForAll2.saveDirectory + ucGen_UploadFileForAll2.GetValue(entry.Serial + "_CreativeMaterials_Video"), ucGen_UploadFileForAll2.saveDirectory + entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll2.GetValueExtension("_CreativeMaterials_Video"), true);
                        GeneralFunction.DeleteFile(ucGen_UploadFileForAll2.saveDirectory + ucGen_UploadFileForAll2.GetValue(entry.Serial + "_CreativeMaterials_Video"), false);
                    }
                    if (ucGen_UploadFileForAll2Trans.HasFile() && ucGen_UploadFileForAll2Trans.GetValue("").ToLower() != (entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll2Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF")).ToLower())
                    {
                        File.Copy(ucGen_UploadFileForAll2Trans.saveDirectory + ucGen_UploadFileForAll2Trans.GetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF"), ucGen_UploadFileForAll2Trans.saveDirectory + entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll2Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF"), true);
                        GeneralFunction.DeleteFile(ucGen_UploadFileForAll2Trans.saveDirectory + ucGen_UploadFileForAll2Trans.GetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF"), false);
                    }

                    ucGen_UploadFileForAll2.SetValue(entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll2.GetValueExtension("_CreativeMaterials_Video"));
                    ucGen_UploadFileForAll2Trans.SetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll2Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF"));
                    //ucGen_UploadFileForAll2.SetValue(ucGen_UploadFileForAll2.SaveAndUpdateFileToAmazonS3(entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll2.GetValueExtension("_CreativeMaterials_Video")));

                    entry.CreativeUploadType = "1";
                }
                else if (rdCreativeUploadType2.Checked)
                {
                    ucGen_UploadFileForAll3.SaveAndUpdateFile(entry.Serial + "_CreativeMaterials_PDF");
                    ucGen_UploadFileForAll4.SaveAndUpdateFile(entry.Serial + "_CreativeMaterials_Video");
                    ucGen_UploadFileForAll3Trans.SaveAndUpdateFile(entry.Serial + "_CreativeMaterialsTranslate_PDF");


                    if (ucGen_UploadFileForAll3.GetValue("").ToLower() != (entry.Serial + "_CreativeMaterials_PDF." + ucGen_UploadFileForAll3.GetValueExtension("_CreativeMaterials_PDF")).ToLower())
                    {
                        File.Copy(ucGen_UploadFileForAll3.saveDirectory + ucGen_UploadFileForAll3.GetValue(entry.Serial + "_CreativeMaterials_PDF"), ucGen_UploadFileForAll3.saveDirectory + entry.Serial + "_CreativeMaterials_PDF." + ucGen_UploadFileForAll3.GetValueExtension("_CreativeMaterials_PDF"), true);
                        GeneralFunction.DeleteFile(ucGen_UploadFileForAll3.saveDirectory + ucGen_UploadFileForAll3.GetValue(entry.Serial + "_CreativeMaterials_PDF"), false);
                    }
                    if (ucGen_UploadFileForAll4.GetValue("").ToLower() != (entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll4.GetValueExtension("_CreativeMaterials_Video")).ToLower())
                    {
                        File.Copy(ucGen_UploadFileForAll4.saveDirectory + ucGen_UploadFileForAll4.GetValue(entry.Serial + "_CreativeMaterials_Video"), ucGen_UploadFileForAll4.saveDirectory + entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll4.GetValueExtension("_CreativeMaterials_Video"), true);
                        GeneralFunction.DeleteFile(ucGen_UploadFileForAll4.saveDirectory + ucGen_UploadFileForAll4.GetValue(entry.Serial + "_CreativeMaterials_Video"), false);
                    }
                    if (ucGen_UploadFileForAll3Trans.HasFile() && ucGen_UploadFileForAll3Trans.GetValue("").ToLower() != (entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll3Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF")).ToLower())
                    {
                        File.Copy(ucGen_UploadFileForAll3Trans.saveDirectory + ucGen_UploadFileForAll3Trans.GetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF"), ucGen_UploadFileForAll3Trans.saveDirectory + entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll3Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF"), true);
                        GeneralFunction.DeleteFile(ucGen_UploadFileForAll3Trans.saveDirectory + ucGen_UploadFileForAll3Trans.GetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF"), false);
                    }

                    ucGen_UploadFileForAll3.SetValue(entry.Serial + "_CreativeMaterials_PDF." + ucGen_UploadFileForAll3.GetValueExtension("_CreativeMaterials_PDF"));
                    ucGen_UploadFileForAll4.SetValue(entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll4.GetValueExtension("_CreativeMaterials_Video"));
                    ucGen_UploadFileForAll3Trans.SetValue(entry.Serial + "_CreativeMaterialsTranslate_PDF." + ucGen_UploadFileForAll3Trans.GetValueExtension("_CreativeMaterialsTranslate_PDF"));
                    //ucGen_UploadFileForAll4.SetValue(ucGen_UploadFileForAll4.SaveAndUpdateFileToAmazonS3(entry.Serial + "_CreativeMaterials_Video." + ucGen_UploadFileForAll4.GetValueExtension("_CreativeMaterials_Video")));

                    entry.CreativeUploadType = "2";
                }

                rdCreativeUploadType0.Enabled = false;
                rdCreativeUploadType1.Enabled = false;
                rdCreativeUploadType2.Enabled = false;

                entry.Save();
            }

            lblDoneMsg.Text = "Your file(s) has been uploaded.";
            btnSubmitDummy.Visible = false;
            btnCloseDummy.Visible = true;

            if (GeneralFunction.isAllMaterialUploaded(entry))
            {
                entry.Status = StatusEntry.UploadCompleted;
                entry.DateModifiedString = DateTime.Now.ToString();
                entry.Save();
            }
        }
    }
}