using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Configuration;

public partial class Controls_gen_UploadFile : System.Web.UI.UserControl
{
    public bool isRequired { get; set; }
    public bool createDirectory { get; set; }
    public bool replaceFile { get; set; }

    public string fieldName
    {
        get
        {
            return (string)ViewState["fieldName"];
        }
        set
        {
            ViewState["fieldName"] = value;
        }
    }

    public string fileType
    {
        get
        {
            return (string)ViewState["fileType"];
        }
        set
        {
            ViewState["fileType"] = value;
        }
    }

    public int maxSize
    {
        get
        {
            return (int)ViewState["maxSize"];
        }
        set
        {
            ViewState["maxSize"] = value;
        }
    }

    public string uploadFileMsg
    {
        get
        {
            return (string)ViewState["uploadFileMsg"];
        }
        set
        {
            ViewState["uploadFileMsg"] = value;
        }
    }

    public string saveDirectory
    {
        get
        {
            return (string)ViewState["saveDirectory"];
        }
        set
        {
            ViewState["saveDirectory"] = value;
        }
    }

    public string virtualDirectory
    {
        get
        {
            return (string)ViewState["virtualDirectory"];
        }
        set
        {
            ViewState["virtualDirectory"] = value;
        }
    }

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
        lblfu.Text = fieldName;
        lblfuMsg.Text = uploadFileMsg;
    }

    protected void PopulateForm()
    {
    }

    public string ValidateForm()
    {
        string error = "";

        error += GeneralFunction.ValidateFileUpload(fieldName, fu, isRequired, fileType, maxSize);

        if (error.Contains("required"))
            error = fieldName+" " + error;

        return error;
    }

    public bool HasFile()
    {
        return fu.HasFile;
    }

    public void SaveAndUpdateFile(string dbFileName)
    {
        if (ViewState["deleteFile"] != null && (bool)ViewState["deleteFile"] == true)
            GeneralFunction.DeleteFile(saveDirectory + dbFileName, false);

        if (HasFile())
        {
            GeneralFunction.DeleteFile(saveDirectory + dbFileName, false);
            GeneralFunction.UploadFile(fu, saveDirectory, createDirectory, replaceFile);
        }
    }

   

    public string GetValue(string dbFileName)
    {
        if (HasFile())
            return fu.FileName;
        else if (ViewState["deleteFile"] != null && (bool)ViewState["deleteFile"] == true)
            return "";
        else
            return dbFileName;
    }

    public string GetValueExtension(string dbFileName)
    {
        if (HasFile())
            return fu.FileName.Substring(fu.FileName.LastIndexOf(".") + 1);
        else if (ViewState["deleteFile"] != null && (bool)ViewState["deleteFile"] == true)
            return "";
        else
            return dbFileName.Substring(dbFileName.LastIndexOf(".") + 1);
    }

    public void SetValue(string dbFileName)
    {
        if (dbFileName != "" && File.Exists(saveDirectory + dbFileName))
        {
            //if (dbFileName.ToLower().EndsWith(".jpg") || dbFileName.ToLower().EndsWith(".jpeg") || dbFileName.ToLower().EndsWith(".gif"))
            //{
            //    imgFileViewer.ImageUrl = virtualDirectory + dbFileName;
            //    pnlFileViewer.Visible = true;
            //}
            //else
            //{
            //    lnkFileDownload.Text = dbFileName;
            //    lnkFileDownload.NavigateUrl = virtualDirectory + dbFileName;
            //    pnlFileDownload.Visible = true;
            //}

            //if (!isRequired)
            //    pnlDelete.Visible = true;

            fu.Visible = false;
            lnkFileDownload.Text = dbFileName;
            lnkFileDownload.NavigateUrl = virtualDirectory + dbFileName;
            pnlFileDownload.Visible = true;
            pnlDelete.Visible = true;
        }

        
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ViewState["deleteFile"] = true;
        pnlFileViewer.Visible = false;
        pnlFileDownload.Visible = false;
        pnlDelete.Visible = false;
        fu.Visible = true;
    }
}