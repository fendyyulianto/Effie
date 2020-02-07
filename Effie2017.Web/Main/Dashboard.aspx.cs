using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Telerik.Web.UI;
using System.IO;
using System.Configuration;
using Effie2017.App;


public partial class Main_Dashboard : System.Web.UI.Page
{
    public static int Number;
    public static bool IsPostBackDes = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        ((Common_MasterPage)Page.Master).ShowUser();
        ((Common_MasterPage)Page.Master).ShowNav();
        ltrJs.Text = "";
        Number = 1;
        if (!Page.IsPostBack || !IsPostBackDes)
        {
            IsPostBackDes = Page.IsPostBack;
            LoadForm();
            PopulateForm();
        }
    }
    

    protected void LoadForm()
    {
        if (Request.UserAgent.Contains("; MSIE 7.") || Request.UserAgent.Contains("; MSIE 8."))
            ((Common_MasterPage)Page.Master).SetCompatibility();

        if (GeneralFunction.IsEntrantSubmissionCutOff())
        {
            phSubmit.Visible = false;
        }

        if (GeneralFunction.IsAddNewEntryCutOff())
        {
            phAddNewEntry.Visible = false;
        }


        if (DateTime.Now < DateTime.Parse(ConfigurationManager.AppSettings["EffieTipsAdd1"]))
        {
            EffieTipsAdd1.Visible = true;
        }
        else
        {
            EffieTipsAdd1.Visible = false;
        }

        if (DateTime.Now > DateTime.Parse(ConfigurationManager.AppSettings["EffieTipsAdd1"]))
        {
            EffieTipsAdd2.Visible = true;
        }
        else
        {
            EffieTipsAdd2.Visible = false;
        }
    }

    protected void PopulateForm()
    {
        BindEntry();
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect("./Entry.aspx");
    }

    protected void BindEntry()
    {
        Effie2017.App.Registration registration = Security.GetLoginSessionUser();

        Effie2017.App.EntryList entryList = Effie2017.App.EntryList.GetEntryList(Guid.Empty, registration.Id, "", StatusEntry.PaymentPending + "|" + StatusEntry.UploadPending + "|" + StatusEntry.UploadCompleted + "|" + StatusEntry.Completed + "|");

        bool neeReminder = false;

        foreach (Effie2017.App.Entry entry in entryList)
        {
            if (entry.Status == StatusEntry.UploadCompleted && entry.IsReminded == 0)
            {
                Effie2017.App.Entry entryUpdate = Effie2017.App.Entry.GetEntry(entry.Id);

                entryUpdate.IsReminded = 1;
                entryUpdate.Save();

                neeReminder = true;
            }
        }

        if (neeReminder)
            ltrJs.Text = "<script type=\"text/javascript\"> $( document ).ready(function() { alert('You have successfully uploaded all the files. Please click confirm to complete your submission.'); }); </script>";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "none", "<script type=\"text/javascript\"> alert('You have successfully uploaded all the files. Please click confirm to complete your submission.'); </script>", false);

        radGridEntry.DataSource = entryList;
        radGridEntry.DataBind();

        #region radGridEntryPending
        entryList = Effie2017.App.EntryList.GetEntryList(Guid.Empty, registration.Id, "", StatusEntry.Draft + "|" + StatusEntry.Ready + "|");
        var GroupIds = entryList.GroupBy(x => x.Id);
        List<Entry> entryTeamp = new List<Entry>();
        foreach (var id in GroupIds)
        {
            try
            {
                entryTeamp.Add(entryList.FirstOrDefault(x => x.Id == id.Key));
            }
            catch { }
        }
        radGridEntryPending.DataSource = entryTeamp.OrderByDescending(x => x.DateModified);
        #endregion

        if (Security.IsUserLogin())
        {
            Registration reg = Security.GetLoginSessionUser();
            var LogHostories = LoginHistoryList.GetLoginHistoryList().Where(x => x.UserId == reg.Id);
            radLoginHistories.DataSource = LogHostories.OrderByDescending(x => x.DateModified);
        }
        
    }



    protected void radLoginHistories_ItemCommand(object sender, GridCommandEventArgs e)
    {
       //TODO
    }
    
    protected void radLoginHistories_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.LoginHistory loginHistory = (Effie2017.App.LoginHistory)e.Item.DataItem;
            
            try
            {
                Label lblno = (Label)e.Item.FindControl("lblno");
                lblno.Text = (e.Item.ItemIndex + 1).ToString();
                Number++;
            }
            catch { }
            
            ((GridDataItem)e.Item)["Date"].Text = loginHistory.DateCreated.ToString("dd/MM/yy");
            ((GridDataItem)e.Item)["Time"].Text = loginHistory.DateCreated.ToString("HH:MM:ss");
        }
    }

    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.Entry entry = (Effie2017.App.Entry)e.Item.DataItem;

            LinkButton lnkBtn = null;

            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnDelete");
            //lnkBtn.Attributes.Add("onclick", "return DeleteConfirmation('" + ((GridDataItem)e.Item)["Serial"].Text + "');");
            lnkBtn.Attributes.Add("onclick", "return DeleteConfirmation('Entry');");

            try {
                Label lblno = (Label)e.Item.FindControl("lblno");
                //lblno.Text = Number.ToString();
                lblno.Text = (e.Item.ItemIndex + 1).ToString();
                Number++;
            }
            catch {

            }

            CheckBox chk = null;
            HyperLink lnk = null;
            
            ((GridDataItem)e.Item)["DateSubmitted"].Text = entry.DateSubmitted.ToString("dd/MM/yy");
            ((GridDataItem)e.Item)["DateModified"].Text = entry.DateModified.ToString("dd/MM/yy");

            if (entry.CategoryMarket == "SM")
                ((GridDataItem)e.Item)["CategoryMarket"].Text = "Single Market";
            else if (entry.CategoryMarket == "MM")
                ((GridDataItem)e.Item)["CategoryMarket"].Text = "Multi Market";
            ((GridDataItem)e.Item)["CategoryMarket"].Text = entry.CategoryPSDetail; //+= "<br>" +

            if (entry.PayStatus == "NOT")
                ((GridDataItem)e.Item)["PayStatus"].Text = "<a href='../Admin/PaymentPdfView.aspx?id=" + GeneralFunction.StringEncryption(entry.Id.ToString()) + "' target='_blank' class='fontRed'>" + GeneralFunction.GetPaymentEntryStatus(entry.PayStatus) + "</a>";
            else
                ((GridDataItem)e.Item)["PayStatus"].Text = "<a href='../Admin/PaymentPdfView.aspx?id=" + GeneralFunction.StringEncryption(entry.Id.ToString()) + "' target='_blank'>" + GeneralFunction.GetPaymentEntryStatus(entry.PayStatus) + "</a>";

            ((GridDataItem)e.Item)["Status"].Text = GeneralFunction.GetEntryStatus(entry.Status);

            if (entry.Status == StatusEntry.Draft || GeneralFunction.IsEntrantSubmissionCutOff())
            {
                try
                {
                    lnkBtn = (LinkButton)e.Item.FindControl("LinkCloning");
                    lnkBtn.Visible = false;
                }
                catch { }
            }

            if (GeneralFunction.IsEntrantSubmissionCutOff() || !GeneralFunction.IsAllowClone())
            {
                try
                {
                    lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnCloning");
                    lnkBtn.Visible = false;
                }
                catch { }
            }

            if (entry.Status == StatusEntry.Completed)
            ((GridDataItem)e.Item)["Status"].Text = "<span style=\"font-weight:bold\">" + GeneralFunction.GetEntryStatus(entry.Status) + "</span>";

            if (entry.WithdrawnStatus != "")
                ((GridDataItem)e.Item)["Status"].Text += "<br/><span style=\"color:Red;\">" + GeneralFunction.GetWithdrawnStatus(entry.WithdrawnStatus) + "</span>";

            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnViewCampaign");
            if (lnkBtn.Text == "")
                lnkBtn.Text = entry.Campaign;

            #region Condition for radGridEntry
            //if (entry.PayStatus == StatusPaymentEntry.Paid)
            if (true)
            {
                if (entry.WithdrawnStatus == "" && (entry.Status == StatusEntry.UploadPending || entry.Status == StatusEntry.UploadCompleted || entry.Status == StatusEntry.Completed))
                {
                    //Guid EntryId = new Guid(((GridDataItem)e.Item)["Id"].Text);
                    if (entry != null)
                    {
                        string url = "./" + GeneralFunctionEntryForm.GetEntryCategory(entry) + ".aspx?Id=" + entry.Id;
                        string urlPdf = "./" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?Id=" + entry.Id;
                        string StatusEntryForm = "";
                        HyperLink Edit = (HyperLink)e.Item.FindControl("lnkEntry1");
                        HyperLink ViewPDF = (HyperLink)e.Item.FindControl("lnkEntry2");
                        HyperLink hlEntryForm = (HyperLink)e.Item.FindControl("lnkEntryForm");
                        hlEntryForm.Enabled = true;
                        hlEntryForm.Visible = true;
                        hlEntryForm.NavigateUrl = url;
                        hlEntryForm.CssClass = "tblLinkRed";
                        try
                        {
                            EntryForm entryForm = EntryForm.GetEntryForm(Guid.Empty, entry.Id);
                            if (entry.Status == StatusEntry.Completed || entryForm.Status == StatusEntry.Completed)
                            {
                                StatusEntryForm = "Completed";
                                hlEntryForm.Visible = false;

                                Edit.Enabled = true;
                                Edit.Visible = true;
                                Edit.NavigateUrl = url;
                                Edit.Text = "Edit";
                                Edit.CssClass = "tblLinkBlack";

                                ViewPDF.Enabled = true;
                                ViewPDF.Visible = true;
                                ViewPDF.NavigateUrl = urlPdf;
                                ViewPDF.Text = "<br>View PDF";
                                ViewPDF.CssClass = "tblLinkBlack";
                            }
                            else
                            {
                                StatusEntryForm = "Draft";
                            }

                            if (entry.Status == StatusEntry.Completed)
                            {
                                Edit.Visible = false;
                                hlEntryForm.Visible = false;
                            }

                            hlEntryForm.Text = "Entry Form (" + StatusEntryForm + ")";
                        }
                        catch { }
                    }

                    //////lnk = (HyperLink)e.Item.FindControl("lnkUploadEntry");
                    //////lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                    //////lnk.NavigateUrl = "./UploadForAll.aspx?md=UE&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                    //////lnk.Enabled = true;
                    //////lnk.Visible = true;
                    lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                    lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                    lnk.NavigateUrl = "./UploadForAll.aspx?md=UA&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                    lnk.Enabled = true;
                    lnk.Visible = true;
                    lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                    lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                    lnk.NavigateUrl = "./UploadForAll.aspx?md=UC&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                    lnk.Enabled = true;
                    lnk.Visible = true;
                    lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                    lnk.CssClass = "fancybox2 fancybox.iframe tblLinkRed";
                    lnk.NavigateUrl = "./UploadForCr.aspx?md=UCr&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                    lnk.Enabled = true;
                    lnk.Visible = true;

                    if (entry.Status == StatusEntry.Completed)
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkEntryForm");
                        lnk.Visible = false;
                        //////lnk = (HyperLink)e.Item.FindControl("lnkUploadEntry");
                        //////lnk.Visible = false;
                        lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                        lnk.Visible = false;
                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                        lnk.Visible = false;
                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                        lnk.Visible = false;
                    }

                    //-------
                    //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_PDF.pdf"))
                    //{
                    //    lnk = (HyperLink)e.Item.FindControl("lnkEntry1");
                    //    lnk.Visible = true;
                    //    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                    //    //////lnk = (HyperLink)e.Item.FindControl("lnkUploadEntry");
                    //    //////lnk.Text = "Edit";
                    //    //////lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";

                    //    lnk = (HyperLink)e.Item.FindControl("lnkEntryForm");
                    //    lnk.Text = "Edit";
                    //    lnk.CssClass = "tblLinkBlack"; //fancybox fancybox.iframe 
                    //}

                    //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_Word.doc"))
                    //{
                    //    lnk = (HyperLink)e.Item.FindControl("lnkEntry2");
                    //    lnk.Visible = true;
                    //    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_Word.doc?" + DateTime.Now.Ticks.ToString();
                    //}
                    //else if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_Word.docx"))
                    //{
                    //    lnk = (HyperLink)e.Item.FindControl("lnkEntry2");
                    //    lnk.Visible = true;
                    //    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_Word.docx?" + DateTime.Now.Ticks.ToString();
                    //}

                    //-------
                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\" + entry.Serial + "_AuthorizationForm_PDF.pdf"))
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkAuthorisation1");
                        lnk.Visible = true;
                        lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Authorisation/" + entry.Id.ToString() + "/" + entry.Serial + "_AuthorizationForm_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                    }

                    //-------
                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpg"))
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkCase1");
                        lnk.Visible = true;
                        lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpg?" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                    }
                    else if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpeg"))
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkCase1");
                        lnk.Visible = true;
                        lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpeg?" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                    }

                    //-------
                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterials_PDF.pdf"))
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkCreative1");
                        lnk.Visible = true;
                        lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterials_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox2 fancybox.iframe tblLinkBlack";
                    }

                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4"))
                    {
                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnCreative2");
                        lnkBtn.Visible = true;
                        //lnkBtn.CommandArgument = "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4";
                        lnkBtn.CommandArgument = "../Video/DownloadMedia.aspx?filePath=" + ConfigurationManager.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + "&MediaID=" + entry.Serial + "_CreativeMaterials_Video.mp4&t" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox2 fancybox.iframe tblLinkBlack";
                    }
                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterialsTranslate_PDF.pdf"))
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkCreative3");
                        lnk.Visible = true;
                        lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterialsTranslate_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox2 fancybox.iframe tblLinkBlack";
                    }
                    //if (GeneralFunction.FileExistsInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Original"], entry.Serial + "_CreativeMaterials_Video.mp4"))
                    //{
                    //    if (GeneralFunction.FileExistsInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Small"], entry.Serial + "_CreativeMaterials_Video.mp4"))
                    //    {
                    //        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnCreative2");
                    //        lnkBtn.Visible = true;
                    //        lnkBtn.CommandArgument = "../Video/DownloadMedia.aspx?BucketID=" + ConfigurationManager.AppSettings["AWSBucket_Small"] + "&MediaID=" + entry.Serial + "_CreativeMaterials_Video.mp4";

                    //        lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                    //        lnk.Text = "Edit";
                    //        lnk.CssClass = "fancybox2 fancybox.iframe tblLinkBlack";
                    //    }
                    //    else
                    //    {
                    //        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnCreative2");
                    //        lnkBtn.Visible = true;
                    //        lnkBtn.Text = "Processing";
                    //        lnkBtn.Enabled = false;
                    //        lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                    //        lnk.Text = "Edit";
                    //        lnk.CssClass = "fancybox2 fancybox.iframe tblLinkBlack";
                    //        lnk.Visible = false;
                    //    }
                    //}
                }
            }
            
            if (entry.Status == StatusEntry.PaymentPending || entry.WithdrawnStatus != "")
            {
                lnk = (HyperLink)e.Item.FindControl("lnkEntryForm");
                lnk.Enabled = false;
                lnk.Visible = true;

                //////lnk = (HyperLink)e.Item.FindControl("lnkUploadEntry");
                //////lnk.Enabled = false;
                //////lnk.Visible = true;

                lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                lnk.Enabled = false;
                lnk.Visible = true;
                lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                lnk.Enabled = false;
                lnk.Visible = true;
                lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                lnk.Enabled = false;
                lnk.Visible = true;
            }

            lnk = (HyperLink)e.Item.FindControl("lnkConfirm");
            if (lnk != null)
            {
                //if (entry.PayStatus == StatusPaymentEntry.Paid && entry.Status == StatusEntry.UploadCompleted)
                if (entry.Status == StatusEntry.UploadCompleted)
                {
                    lnk.CssClass = "fancybox3 fancybox.iframe tblLinkBlack";
                    lnk.ForeColor = System.Drawing.Color.Red;
                    lnk.NavigateUrl = "./UploadConfirm.aspx?md=UCr&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                    lnk.Visible = true;
                }
                else if (entry.Status == StatusEntry.Completed)
                {
                    lnk.Visible = false;
                }
                else
                {
                    lnk.CssClass = "tblLinkDisable";
                    lnk.Enabled = false;
                }
            }
            #endregion

            #region Condition for radGridEntryPending
            if (entry.Status == StatusEntry.Ready)
            {
                chk = (CheckBox)e.Item.FindControl("chkSubmit");
                chk.Enabled = true;
                lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnView");
                lnkBtn.Visible = true;
            }
            //else
            //{
            //    lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            //    if (lnkBtn != null)
            //        lnkBtn.Visible = true;
            //}
            #endregion


            //if (GeneralFunction.IsEntrantSubmissionCutOff())
            //{
            //    lnk = (HyperLink)e.Item.FindControl("lnkConfirm");
            //    if (lnk != null) lnk.Visible = false;
            //}



            #region Allow Upload Files
            if (GeneralFunction.IsAllowUploadFiles())
            {
                try
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                    //lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                    lnk.NavigateUrl = "./UploadForAll.aspx?md=UA&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                    lnk.Enabled = true;
                    lnk.Visible = true;
                    lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                    //lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                    lnk.NavigateUrl = "./UploadForAll.aspx?md=UC&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                    lnk.Enabled = true;
                    lnk.Visible = true;

                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\" + entry.Serial + "_AuthorizationForm_PDF.pdf"))
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkAuthorisation1");
                        lnk.Visible = true;
                        lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Authorisation/" + entry.Id.ToString() + "/" + entry.Serial + "_AuthorizationForm_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                    }

                    //-------
                    if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpg"))
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkCase1");
                        lnk.Visible = true;
                        lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpg?" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                    }
                    else if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpeg"))
                    {
                        lnk = (HyperLink)e.Item.FindControl("lnkCase1");
                        lnk.Visible = true;
                        lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpeg?" + DateTime.Now.Ticks.ToString();

                        lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                        lnk.Text = "Edit";
                        lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                    }
                }
                catch { }
            }
            #endregion
        }
    }

    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "Edit")
        {
            //Response.Redirect("./Entry.aspx?id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text));
            Response.Redirect("./Entry.aspx?db=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Delete")
        {
            Effie2017.App.Entry.CleanDeleteEntry(new Guid(((GridDataItem)e.Item)["Id"].Text));

            //lblError.Text += ((GridDataItem)e.Item)["Serial"].Text + " has been deleted.<br>";
            lblError.Text += "Entry has been deleted.<br>";
            BindEntry();
        }
        else if (e.CommandName == "Cloning")
        {
            string Id = ((GridDataItem)e.Item)["Id"].Text;
            bool isSuccess = OnCloningData(Id);
            if (isSuccess)
            {
                BindEntry();
                radGridEntryPending.Rebind();
            }
        }
        else if (e.CommandName == "View")
        {
            Response.Redirect("./Entry.aspx?db=1&v=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Confirm")
        {
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(new Guid(((GridDataItem)e.Item)["Id"].Text));

            entry.Status = StatusEntry.Completed;
            entry.DateModifiedString = DateTime.Now.ToString();
            entry.Save();

            lblError.Text += "Entry has been confirmed.<br>";
            BindEntry();
        }
        else if (e.CommandName == "lnkBtnCreative2")
        {
            Session["DownloadFile"] = e.CommandArgument.ToString();
            //ltrJs.Text = "<script type=\"text/javascript\"> window.open(\"./DownloadFile.aspx\"); </script>";
             //Response.Redirect("./DownloadFile.aspx");
            Response.Redirect(e.CommandArgument.ToString());
        }
    }

    protected bool OnCloningData(string Id)
    {
        Entry entry_Old = Entry.GetEntry(new Guid(Id));
        Entry entry_New = Entry.NewEntry();
        if (entry_Old != null)
        {
            entry_New.Id = Guid.NewGuid();
            entry_New.Campaign = entry_Old.Campaign;
            entry_New.Client = entry_Old.Client;
            entry_New.Brand = entry_Old.Brand;

            entry_New.CategoryMarket = "";//entry_Old.CategoryMarket;
            entry_New.CategoryPS = "";//entry_Old.CategoryPS;
            entry_New.CategoryPSDetail = "";//entry_Old.CategoryPSDetail;


            //entry_New.CategoryMarket = entry_Old.CategoryMarket;
            //entry_New.CategoryPS = entry_Old.CategoryPS;
            //entry_New.CategoryPSDetail = entry_Old.CategoryPSDetail;

            entry_New.DateCampaignStartString = entry_Old.DateCampaignStartString;
            entry_New.DateCampaignEndString = entry_Old.DateCampaignEndString;
            entry_New.IsCampaignOngoing = entry_Old.IsCampaignOngoing;
            entry_New.RepSalutation = entry_Old.RepSalutation;
            entry_New.RepFirstname = entry_Old.RepFirstname;
            entry_New.RepLastname = entry_Old.RepLastname;
            entry_New.RepJob = entry_Old.RepJob;
            entry_New.RepCompany = entry_Old.RepCompany;
            entry_New.RepContact = entry_Old.RepContact;
            entry_New.RepMobile = entry_Old.RepMobile;
            entry_New.RepEmail = entry_Old.RepEmail;
            entry_New.Effectiveness = entry_Old.Effectiveness;
            entry_New.Summary = entry_Old.Summary;
            entry_New.ProductClassification = entry_Old.ProductClassification;
            entry_New.EntryObjective = entry_Old.EntryObjective;
            entry_New.EntryObjective2 = entry_Old.EntryObjective2;
            entry_New.TargetAudience = entry_Old.TargetAudience;
            entry_New.TargetAudiencePri = entry_Old.TargetAudiencePri;
            entry_New.TargetAudienceOthers = entry_Old.TargetAudienceOthers;
            entry_New.TargetAudiencePriOthers = entry_Old.TargetAudiencePriOthers;
            entry_New.HeroTouchPoint = entry_Old.HeroTouchPoint;
            entry_New.HeroTouchPointOthers = entry_Old.HeroTouchPointOthers;
            entry_New.HeroTouchPoint2 = entry_Old.HeroTouchPoint2;
            entry_New.HeroTouchPointOthers2 = entry_Old.HeroTouchPointOthers2;
            entry_New.HeroTouchPoint3 = entry_Old.HeroTouchPoint3;
            entry_New.HeroTouchPointOthers3 = entry_Old.HeroTouchPointOthers3;
            entry_New.SocialPlatforms = entry_Old.SocialPlatforms;
            entry_New.SocialPlatformsOthers = entry_Old.SocialPlatformsOthers;
            entry_New.Research = entry_Old.Research;
            entry_New.ResearchImp = entry_Old.ResearchImp;
            entry_New.CaseData = entry_Old.CaseData;
            entry_New.SDGData1 = entry_Old.SDGData1;
            entry_New.SDGData2 = entry_Old.SDGData2;
            entry_New.Permission = entry_Old.Permission;
            entry_New.Name = entry_Old.Name;
            entry_New.Title = entry_Old.Title;
            entry_New.Company = entry_Old.Company;
            entry_New.DateModifiedString = DateTime.Now.ToString();
            entry_New.DateCreatedString = DateTime.Now.ToString();
            entry_New.Status = StatusEntry.Draft;//entry_Old.Status;

            //entry_New.WithdrawnStatus = entry_Old.WithdrawnStatus;
            entry_New.WithdrawnStatus = "";

            //entry_New.DateSubmittedString = entry_Old.DateSubmittedString;
            entry_New.RegistrationId = entry_Old.RegistrationId;
            entry_New.PayGroupId = Guid.Empty;
            //entry_New.ProcessingStatus = entry_Old.ProcessingStatus;
            //entry_New.AdminidAssignedto = entry_Old.AdminidAssignedto;
            //entry_New.MaterialsSubmitted = entry_Old.MaterialsSubmitted;
            //entry_New.DQFlag = entry_Old.DQFlag;
            //entry_New.NotificationSentDate = entry_Old.NotificationSentDate;
            //entry_New.ReopeningDate = entry_Old.ReopeningDate;
            //entry_New.ReopeningDeadline = entry_Old.ReopeningDeadline;
            //entry_New.FlagReason = entry_Old.FlagReason;
            //entry_New.FlagDQDescription = entry_Old.FlagDQDescription;
            //entry_New.ReopenedBy = entry_Old.ReopenedBy;
            //entry_New.OtherRemarks = entry_Old.OtherRemarks;
            //entry_New.DateVerified = entry_Old.DateVerified;
            //entry_New.ReasonFeeWaiver = entry_Old.ReasonFeeWaiver;

            entry_New.Save();

            CompanyCreditList CompanyCreditOld = CompanyCreditList.GetCompanyCreditList(entry_Old.Id);
            foreach (CompanyCredit item in CompanyCreditOld)
            {
                CompanyCredit CompanyCreditNew = CompanyCredit.NewCompanyCredit();

                CompanyCreditNew.ContactType = item.ContactType;
                CompanyCreditNew.Company = item.Company;
                CompanyCreditNew.Address1 = item.Address1;
                CompanyCreditNew.Address2 = item.Address2;
                CompanyCreditNew.City = item.City;
                CompanyCreditNew.Postal = item.Postal;
                CompanyCreditNew.Country = item.Country;
                CompanyCreditNew.Salutation = item.Salutation;
                CompanyCreditNew.Fullname = item.Fullname;
                CompanyCreditNew.Job = item.Job;
                CompanyCreditNew.Contact = item.Contact;
                CompanyCreditNew.Email = item.Email;
                CompanyCreditNew.ClientCompanyNetwork = item.ClientCompanyNetwork;
                CompanyCreditNew.ClientCompanyNetworkOthers = item.ClientCompanyNetworkOthers;
                CompanyCreditNew.Network = item.Network;
                CompanyCreditNew.NetworkOthers = item.NetworkOthers;
                CompanyCreditNew.HoldingCompany = item.HoldingCompany;
                CompanyCreditNew.HoldingCompanyOthers = item.HoldingCompanyOthers;
                CompanyCreditNew.CompanyType = item.CompanyType;
                CompanyCreditNew.CompanyTypeOther = item.CompanyTypeOther;
                CompanyCreditNew.Status = item.Status;
                CompanyCreditNew.EntryId = entry_New.Id;
                CompanyCreditNew.Save();
            }

            return true;
        }
        return false;
    }

    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        BindEntry();
    }

    protected void radLoginHistories_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        BindEntry();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        GeneralFunction.ResetGroupPaymentCache();

        int x = 0;
        foreach (GridDataItem gdi in radGridEntryPending.Items)
        {
            CheckBox chk = (CheckBox)gdi.FindControl("chkSubmit");

            if (chk.Checked)
            {
                Entry entry = Entry.GetEntry(new Guid(gdi["Id"].Text));
                entry.DateSubmittedString = DateTime.Now.ToString();
                entry.Save();

                GeneralFunction.AddIdToGroupPaymentCache(new Guid(gdi["Id"].Text));
                x++;
            }
        }

        if (x == 0)
        {
            lblError2.Text = "Please select at least 1 entry to submit.<br>";
            GeneralFunction.ResetGroupPaymentCache();
        }
        //else if (x > 9)
        //{
        //    lblError2.Text = "Group submission cannot be more than 9.<br>";
        //    GeneralFunction.ResetGroupPaymentCache();
        //}
        else
            Response.Redirect("./Summary.aspx");
    }

    protected void btnRebind_Click(object sender, EventArgs e)
    {
        PopulateForm();
        radGridEntry.Rebind();
    }
}