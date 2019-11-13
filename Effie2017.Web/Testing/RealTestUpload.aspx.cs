using System;
using System.Web.UI;

using System.IO;
using System.Configuration;


public partial class Testing_RealTestUpload : System.Web.UI.Page
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


        lblTitle.Text = "Creative Materials";
        lblDesc.Text = "You may select one of the following options for the submission of Creative Materials, depending on the requirements and nature of your case. The Creative Materials should showcase the integral communication touch points mentioned in the written case.<br>";



        lblTitle2.Text = "(Recommended for entries with a TVC/video/audio component in its campaign.) Submit a Creative Video, no longer than 3 minutes. Video files should be in .mp4 format and the file size for online upload must be limited to 200MB.";
        ucGen_UploadFileForAll2.fieldName = "Upload Video *";
        ucGen_UploadFileForAll2.uploadFileMsg = "mp4 format (max file size 200mb)";
        ucGen_UploadFileForAll2.fileType = "mp4";
        ucGen_UploadFileForAll2.createDirectory = false;
        ucGen_UploadFileForAll2.maxSize = 208666624;
        ucGen_UploadFileForAll2.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\";
        ucGen_UploadFileForAll2.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/CreativeVideo/";



    }

    protected void PopulateForm()
    {
        ucGen_UploadFileForAll2.SetValue("testing.mp4");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblError.Text = "";


        if (!rdCreativeUploadType1.Checked)
        {
            lblError.Text += "Selection is required.<br>";
        }

        if (rdCreativeUploadType1.Checked)
            lblError.Text += ucGen_UploadFileForAll2.ValidateForm();
       
        if (lblError.Text != "")
            lblError.Text += "Please upload the correct file type.";

        if (lblError.Text == "")
        {
            if (rdCreativeUploadType1.Checked)
            {
                ucGen_UploadFileForAll2.createDirectory = false;
                //ucGen_UploadFileForAll2.SaveAndUpdateFile("testing");

                //if (ucGen_UploadFileForAll2.GetValue("").ToLower() != ("testing." + ucGen_UploadFileForAll2.GetValueExtension("testing")).ToLower())
                //{
                //    File.Copy(ucGen_UploadFileForAll2.saveDirectory + ucGen_UploadFileForAll2.GetValue("testing"), ucGen_UploadFileForAll2.saveDirectory + "testing." + ucGen_UploadFileForAll2.GetValueExtension("testing"), true);
                //    GeneralFunction.DeleteFile(ucGen_UploadFileForAll2.saveDirectory + ucGen_UploadFileForAll2.GetValue("testing"), false);
                //}
                //ucGen_UploadFileForAll2.SetValue("testing." + ucGen_UploadFileForAll2.GetValueExtension("testing"));
                ucGen_UploadFileForAll2.SetValue(ucGen_UploadFileForAll2.SaveAndUpdateFileToAmazonS3("testing." + ucGen_UploadFileForAll2.GetValueExtension("testing")));

            }

            rdCreativeUploadType1.Enabled = false;           
        }
    }
}