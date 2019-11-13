using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using Telerik.Web.UI;
using System.IO;
using System.Data;
using Ionic.Zip;

public partial class Admin_DownloadMediaList : PageSecurity_Admin
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
        if (GeneralFunction.GetFilterPageId() == "DownloadMediaList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            ddlEntryStatus.SelectedValue = GeneralFunction.GetFilterF3();
            ddlMarket.SelectedValue = GeneralFunction.GetFilterF4();
            ddlCountry.SelectedValue = GeneralFunction.GetFilterF5();
            ddlCategory.SelectedValue = GeneralFunction.GetFilterF6();
           
            ViewState["AdvanceSearch"] = "1";
        }

        BindGrid(false, string.Empty, GridSortOrder.None, true);

        // Readonly Admin
        btnDownload.Visible = !Security.IsReadOnlyAdmin();


        if ((!Security.IsRoleSuperAdmin()) && (!Security.IsRoleAdmin())) // REFERENCE FROM SecureControlByHiding
        {
            radGridEntry.MasterTableView.GetColumn("SelectALL").Visible = false;
        }
    }

    private void LoadForm()
    {
        ddlEntryStatus.Items.Add(new ListItem("All", ""));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadPending), StatusEntry.UploadPending));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.UploadCompleted), StatusEntry.UploadCompleted));
        ddlEntryStatus.Items.Add(new ListItem(GeneralFunction.GetEntryStatusForAdmin(StatusEntry.Completed), StatusEntry.Completed));

        // Category
        DataTable dt1 = Category.GetSubcategories("01");
        DataTable dt2 = Category.GetSubcategories("02");
        DataTable dt3 = Category.GetSubcategories("03");
        DataTable dt4 = Category.GetSubcategories("04");

        ddlCategory.Items.Add(new ListItem("All", ""));

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Single - Products and Services", "01"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt1.Rows)
            ddlCategory.Items.Add("SP-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Single - Specialty", "02"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt2.Rows)
            ddlCategory.Items.Add("SS-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Multi - Products and Services", "03"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt3.Rows)
            ddlCategory.Items.Add("MP-" + dr["Name"].ToString());

        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(new ListItem("Multi - Specialty", "04"));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt4.Rows)
            ddlCategory.Items.Add("MS-" + dr["Name"].ToString());

        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryList(false);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));
    }

    private void BindGrid(bool isCustomSort, string sortExpression, GridSortOrder order, bool needRebind)
    {
        Effie2017.App.EntryList entryList = Effie2017.App.EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.UploadPending + "|" + StatusEntry.UploadCompleted + "|" + StatusEntry.Completed + "|");

        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Entry> flist = new List<Entry>();

        if (advanceSearch == "1")
        {
            // Advanced search 
            // basic fields
            // user's country
            List<Guid> entryIdList = GeneralFunction.GetFilteredEntryListFromRegCountry(ddlCountry.SelectedValue, true);
            List<Guid> entryIdList2 = GeneralFunction.GetFilteredEntryListFromRegCompany(txtSearch.Text.Trim(), true);

            // Changes by Shaik for adding new columns on 19 Oct 2015
            List<Guid> entryIdList3 = GeneralFunction.GetFilteredEntryListFromRegFirstName(txtSearch.Text.Trim(), true);
            List<Guid> entryIdList4 = GeneralFunction.GetFilteredEntryListFromRegLastName(txtSearch.Text.Trim(), true);

            foreach (Entry item in entryList)
            {
                // category strip out the prefix
                string category = ddlCategory.SelectedValue;
                if (category.Length > 3) category = category.Substring(3, category.Length - 3);

                if (
                    (ddlDeadline.SelectedValue == "" || (ddlDeadline.SelectedValue != "" && item.Deadline == ddlDeadline.SelectedValue)) &&
                    (ddlEntryStatus.SelectedValue == "" || (ddlEntryStatus.SelectedValue != "" && item.Status == ddlEntryStatus.SelectedValue)) &&
                    (ddlMarket.SelectedValue == "" || (ddlMarket.SelectedValue != "" && item.CategoryMarket == ddlMarket.SelectedValue)) &&
                    //(ddlCategory.SelectedValue == "" || (ddlCategory.SelectedValue != "" && (item.CategoryPSDetail == ddlCategory.SelectedValue || GeneralFunction.IsCategoryInCategoryGroup(ddlCategory.SelectedValue, item.CategoryPSDetail)))) &&
                    (category == "" || (category != "" && (item.CategoryPSDetailFromRound("") == category || GeneralFunction.IsCategoryInCategoryGroup(category, item.CategoryPSDetailFromRound(""))))) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&
                    
                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entryId") && item.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "client") && item.Client.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && entryIdList2.Contains(item.Id))) ||
                     (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "firstname") && entryIdList3.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "lastname") && entryIdList4.Contains(item.Id)))
                    )

                    &&
                    (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + item.Serial + "_CreativeMaterials_Video.mp4"))
                   )
                    flist.Add(item);
            }



        }
        else
        {
            foreach (Entry item in entryList)
            {
                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + item.Serial + "_CreativeMaterials_Video.mp4"))
                {
                    flist.Add(item);
                }
            }
        }

        #region CustomSorting

        if (isCustomSort)
        {
            List<Registration> regList = RegistrationList.GetRegistrationList(string.Empty, string.Empty, string.Empty).ToList();

            if (sortExpression.Equals("Serial"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Serial select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Serial descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Serial select entry).ToList();
                        break;
                }
            }            
            else if (sortExpression.Equals("DateSubmitted"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateSubmitted select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Campaign"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Campaign select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Campaign descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Campaign select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Client"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Client select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Client descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Client select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("CategoryMarket"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.CategoryMarket orderby entry.CategoryPSDetail select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.CategoryMarket descending orderby entry.CategoryPSDetail descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.CategoryMarket orderby entry.CategoryPSDetail select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Status"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Status select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Status descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.Status select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Firstname"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Firstname select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Firstname descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Firstname select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Lastname"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Lastname select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Lastname descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Lastname select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("IsVideoDownloaded"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.IsVideoDownloaded select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.IsVideoDownloaded descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.IsVideoDownloaded select entry).ToList();
                        break;
                }
            }
        }
        else
            flist = (from entry in flist orderby entry.Invoice descending, entry.DateCreated descending select entry).ToList();

        #endregion

        radGridEntry.DataSource = flist;
        if (needRebind) radGridEntry.DataBind();
    }

    #region Events

    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.Entry entry = (Effie2017.App.Entry)e.Item.DataItem;

            LinkButton lnkBtn = null;
            Label lbl = null;
            HyperLink lnk = null;

            //((GridDataItem)e.Item)["DateSubmitted"].Text = entry.DateSubmitted.ToString("MM/dd/yy hh:mm:ss tt");

            if (entry.CategoryMarket == "SM")
                ((GridDataItem)e.Item)["CategoryMarket"].Text = "Single Market";
            else if (entry.CategoryMarket == "MM")
                ((GridDataItem)e.Item)["CategoryMarket"].Text = "Multi Market";
            ((GridDataItem)e.Item)["CategoryMarket"].Text = entry.CategoryPSDetail;

            ((GridDataItem)e.Item)["Status"].Text = GeneralFunction.GetEntryStatusForAdmin(entry.Status);
            ((GridDataItem)e.Item)["Deadline"].Text = entry.Deadline;

            // Status
            if (entry.Status == StatusEntry.Completed)
                ((GridDataItem)e.Item)["Status"].Text = "<span style=\"font-weight:bold\">" + GeneralFunction.GetEntryStatus(entry.Status) + "</span>";
            if (entry.WithdrawnStatus != "")
                ((GridDataItem)e.Item)["Status"].Text += "<br/><span style=\"color:Red;\">" + GeneralFunction.GetWithdrawnStatus(entry.WithdrawnStatus) + "</span>";

            // submitted by
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnBuSubmittedBy");
            Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
            lnkBtn.Text = "-";
            if (reg != null)
            {
                lnkBtn.Text = GeneralFunction.GetRegistrationFromEntry(entry).Company;
                lnkBtn.CommandArgument = reg.Id.ToString();               
            }


            // submitted details
            lbl = (Label)e.Item.FindControl("lblSubmittedDetails");
            lbl.Text = reg.Firstname + " " + reg.Lastname + "<br/>" + reg.Job + "<br/>" + GeneralFunction.ShowFriendlyContact(reg.Contact) + "<br/>" + reg.Email;
            
            if (reg != null)
            {
                lnkBtn.Text = GeneralFunction.GetRegistrationFromEntry(entry).Company;
                lnkBtn.CommandArgument = reg.Id.ToString();

                // Changes by Shaik for adding new columns on 19 Oct 2015
                ((GridDataItem)e.Item)["Firstname"].Text = reg.Firstname;
                ((GridDataItem)e.Item)["Lastname"].Text = reg.Lastname;
                ((GridDataItem)e.Item)["Country"].Text = reg.Country;
            }

            if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4"))
            {
                lnkBtn = (LinkButton)e.Item.FindControl("lnkDownloadVideo");
                lnkBtn.Visible = true;
                lnkBtn.CommandArgument = entry.Id.ToString();
            }
            

            ((GridDataItem)e.Item)["IsVideoDownloaded"].Text = entry.IsVideoDownloaded ? "Yes" : "No";
        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            PageSizeCombo.Items.Clear();
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", radGridEntry.MasterTableView.ClientID);
            PageSizeCombo.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
        }
    }

    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "video")
        {
            Entry entry = Entry.GetEntry(new Guid(e.CommandArgument.ToString()));
            entry.IsVideoDownloaded = true;
            entry.Save();
            Response.Redirect("../Video/DownloadMedia.aspx?filePath=" + System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + "&MediaID=" + entry.Serial + "_CreativeMaterials_Video.mp4&t=" + DateTime.Now.Ticks.ToString());        
            
        }
        else if (e.CommandName == "View")
        {
            GeneralFunction.SetFilter("DownloadMediaList", txtSearch.Text, ddlSearch.SelectedValue, ddlEntryStatus.SelectedValue, ddlMarket.SelectedValue,
                                 ddlCountry.SelectedValue, ddlCategory.SelectedValue, string.Empty, string.Empty);

            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/DownloadMediaList.aspx");  // to return from whereever
            Response.Redirect("../Main/Entry.aspx?db=1&v=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "User")
        {
            GeneralFunction.SetFilter("DownloadMediaList", txtSearch.Text, ddlSearch.SelectedValue, ddlEntryStatus.SelectedValue, ddlMarket.SelectedValue,
                                 ddlCountry.SelectedValue, ddlCategory.SelectedValue, string.Empty, string.Empty);

            Registration reg = Registration.GetRegistration(new Guid(e.CommandArgument.ToString()));
            Security.SetLoginSessionUser(reg);
            GeneralFunction.SetRedirect("../Admin/DownloadMediaList.aspx");  // to return from whereever
            Response.Redirect("../Admin/Profile.aspx?Id=" + reg.Id.ToString());
        }
    }

    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
        {
            foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
            {
                BindGrid(false, expr.FieldName, expr.SortOrder, false);
            }
        }
        else
        {
            BindGrid(false, string.Empty, GridSortOrder.None, false);
        }     
    }

    protected void radGridEntry_SortCommand(object Sender, GridSortCommandEventArgs e)
    {
        if (e.CommandArgument == "Country")
        {
            BindGrid(true, e.CommandArgument.ToString(), e.NewSortOrder, true);
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        int counter = 0;
        bool isSelected = false;

        List<string> filesToInclude = new List<string>();

        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            if (chkbox.Checked)
            {
                Entry entry = Entry.GetEntry(new Guid(item["Id"].Text));                
                entry.IsVideoDownloaded = true;
                entry.Save();

                filesToInclude.Add(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4");
                
                isSelected = true;
                counter++;
            }
            chkbox.Checked = false; // unchecked it
        }

        if (!isSelected)
            lblError.Text = "Select at least 1 entry to downlaod video.";
        else
        {
            string archiveName = String.Format("CreativeVideos-{0}.zip", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + archiveName + "\"");
            Response.ContentType = "application/zip";
            Response.BufferOutput = true;

            using (ZipFile zip = new ZipFile())
            {                                
                // filesToInclude is a string[] or List<String>
                zip.AddFiles(filesToInclude,"files");
                
                zip.Save(Response.OutputStream);
            }
                       
            lblError.Text = "Downloaded video for " + counter + " entry(s).";
            
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path).ToString());            
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {       
        string cat = ddlCategory.SelectedValue;
        if (cat.Length > 3)
        {
            // First 2 chars is SP / SS / MP / MS
            // convert to SM / MM
            if (cat.Substring(0, 1) == "S") ddlMarket.SelectedValue = "SM";
            if (cat.Substring(0, 1) == "M") ddlMarket.SelectedValue = "MM";
        }
        if (cat.Length == 2)
        {
            // also for selection of category grouping
            if (cat == "01" || cat == "02") ddlMarket.SelectedValue = "SM";
            if (cat == "03" || cat == "04") ddlMarket.SelectedValue = "MM";
        }

        ViewState["AdvanceSearch"] = "1";
        BindGrid(false, string.Empty, GridSortOrder.None, true);

        GeneralFunction.SetFilter("DownloadMediaList", txtSearch.Text, ddlSearch.SelectedValue, ddlEntryStatus.SelectedValue, ddlMarket.SelectedValue,
                                  ddlCountry.SelectedValue, ddlCategory.SelectedValue, string.Empty,string.Empty);

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlEntryStatus.SelectedValue = "";
        ddlCategory.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlMarket.SelectedValue = "";
        

        ViewState["AdvanceSearch"] = "";

        BindGrid(false, string.Empty, GridSortOrder.None, false);

        GeneralFunction.ResetFilter();
    }

    //END Edit By Rico
    #endregion

    #region Helper

    private ListItem SeparatorItem()
    {
        ListItem separator = new ListItem("-------------------------------", "");
        separator.Attributes.Add("disabled", "disabled");

        return separator;
    }

    #endregion
}