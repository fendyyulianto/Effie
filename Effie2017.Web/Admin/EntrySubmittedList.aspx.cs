using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.IO;
using Effie2017.App;
using System.Data;
using iTextSharp;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
public partial class Admin_EntrySubmittedList : PageSecurity_Admin
{
    int counter;
    public static string BodyTamplateMail = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ltrJs.Text = "";

        if (!Page.IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }


        string parameter = Request["__EVENTARGUMENT"];
        if (parameter == "inviteEmail")
            btnSend_Click(sender, e);
    }

    protected void LoadForm()
    {
        //if (Request.UserAgent.Contains("; MSIE 7.") || Request.UserAgent.Contains("; MSIE 8."))
        //    ((Common_MasterPage)Page.Master).SetCompatibility();

        // Entry Status
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

        //ddlCategory.Items.Add(new ListItem("All", ""));

        //ddlCategory.Items.Add(SeparatorItem());
        //ddlCategory.Items.Add(new ListItem("Single - Products and Services", "01"));
        //ddlCategory.Items.Add(SeparatorItem());
        //foreach (DataRow dr in dt1.Rows)
        //    ddlCategory.Items.Add(dr["Name"].ToString());

        //ddlCategory.Items.Add(SeparatorItem());
        //ddlCategory.Items.Add(new ListItem("Single - Specialty", "02"));
        //ddlCategory.Items.Add(SeparatorItem());
        //foreach (DataRow dr in dt2.Rows)
        //    ddlCategory.Items.Add(dr["Name"].ToString());

        //ddlCategory.Items.Add(SeparatorItem());
        //ddlCategory.Items.Add(new ListItem("Multi - Products and Services", "03"));
        //ddlCategory.Items.Add(SeparatorItem());
        //foreach (DataRow dr in dt3.Rows)
        //    ddlCategory.Items.Add(dr["Name"].ToString());

        //ddlCategory.Items.Add(SeparatorItem());
        //ddlCategory.Items.Add(new ListItem("Multi - Specialty", "04"));
        //ddlCategory.Items.Add(SeparatorItem());
        //foreach (DataRow dr in dt4.Rows)
        //    ddlCategory.Items.Add(dr["Name"].ToString());



        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryList(false);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));

        Security.SecureControlByHiding(btnExport, "EXPORT");

        try
        {
            ddlAssignedTo.Items.Add(new ListItem("All", ""));
            AdministratorList administratorList = AdministratorList.GetAdministratorList();
            foreach (Administrator admin in administratorList)
            {
                //ddlAssignTo.Items.Add(new ListItem(admin.LoginId, admin.Id.ToString()));
                ddlAssignedTo.Items.Add(new ListItem(admin.LoginId, admin.Id.ToString()));
            }
        }
        catch { }
    }

    protected void PopulateForm()
    {
        if (GeneralFunction.GetFilterPageId() == "MaterialsSubmittedList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            ddlEntryStatus.SelectedValue = GeneralFunction.GetFilterF3();
            ddlMarket.SelectedValue = GeneralFunction.GetFilterF4();
            ddlCountry.SelectedValue = GeneralFunction.GetFilterF5();
            ddlCategory.SelectedValue = GeneralFunction.GetFilterF6();
            ddlIsVerified.SelectedValue = GeneralFunction.GetFilterF7();

            ViewState["AdvanceSearch"] = "1";
            rtabEntry.Visible = false;
        }
        else
        {
            ViewState["AdvanceSearch"] = "";
            ViewState["TabFilterValue"] = "";
            rtabEntry.SelectedIndex = 0;
        }
        
        BindEntry(false, string.Empty, GridSortOrder.None, true);

        // Readonly Admin
        if (Security.IsReadOnlyAdmin() || Security.IsRoleST())
        {
            btnEmailReminder.Visible = false;
            btnDownload.Visible = false;
            btnVerify.Visible = false;
            radGridEntry.MasterTableView.GetColumn("SelectALL").Visible = false;
        }
        
        if (!Security.IsRoleSuperAdmin() && !Security.IsRoleSuperAdminFinance())
        {
            btnExport.Visible = false;
        }
    }

    protected void BindEntry(bool isCustomSort, string sortExpression, GridSortOrder order, bool needRebind)
    {
        Effie2017.App.EntryList entryList = Effie2017.App.EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.UploadPending + "|" + StatusEntry.UploadCompleted + "|" + StatusEntry.Completed + "|");
       
        // filter
        string status = (string)ViewState["TabFilterValue"];
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
                     (ddlAssignedTo.SelectedValue == "" || (ddlAssignedTo.SelectedValue != "" && item.AdminidAssignedto == (new Guid(ddlAssignedTo.SelectedValue)))) &&
                    (ddlDeadline.SelectedValue == "" || (ddlDeadline.SelectedValue != "" && item.Deadline == ddlDeadline.SelectedValue)) &&
                    (ddlEntryStatus.SelectedValue == "" || (ddlEntryStatus.SelectedValue != "" && item.Status == ddlEntryStatus.SelectedValue)) &&
                    (ddlMarket.SelectedValue == "" || (ddlMarket.SelectedValue != "" && item.CategoryMarket == ddlMarket.SelectedValue)) &&
                    //(ddlCategory.SelectedValue == "" || (ddlCategory.SelectedValue != "" && (item.CategoryPSDetail == ddlCategory.SelectedValue || GeneralFunction.IsCategoryInCategoryGroup(ddlCategory.SelectedValue, item.CategoryPSDetail)))) &&
                    (category == "" || (category != "" && (item.CategoryPSDetailFromRound("") == category || GeneralFunction.IsCategoryInCategoryGroup(category, item.CategoryPSDetailFromRound(""))))) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&
                    (ddlIsVerified.SelectedValue == "" || (ddlIsVerified.SelectedValue != "" && item.IsMaterialsVerified == bool.Parse(ddlIsVerified.SelectedValue))) &&



                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "entryId") && item.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "client") && item.Client.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && entryIdList2.Contains(item.Id))) ||
                     (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "firstname") && entryIdList3.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "lastname") && entryIdList4.Contains(item.Id)))
                    )



                   )
                    flist.Add(item);
            }



        }
        else
        {
            // tab filtering
            if (status == "WDN")
            {
                foreach (Entry item in entryList)
                    if (item.WithdrawnStatus != "") flist.Add(item);
            }
            else
            {
                foreach (Entry item in entryList)
                    if (status == "" || (status != "" && item.Status == status)) flist.Add(item);
            }
        }



        counter = 1;


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
            else if (sortExpression.Equals("ProcessingStatus"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => (x.ProcessingStatus == StatusEntry.Completed) ? "Completed" : GeneralFunction.GetEntryStatusForAdmin(x.ProcessingStatus)).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => (x.ProcessingStatus == StatusEntry.Completed) ? "Completed" : GeneralFunction.GetEntryStatusForAdmin(x.ProcessingStatus)).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => (x.ProcessingStatus == StatusEntry.Completed) ? "Completed" : GeneralFunction.GetEntryStatusForAdmin(x.ProcessingStatus)).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Country"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby reg.Country select entry).ToList();
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
            else if (sortExpression.Equals("IsVerified"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.IsMaterialsVerified select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.IsMaterialsVerified descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.IsMaterialsVerified select entry).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("DateReminder"))
            {
                //x.DateReminder(x.Id, EmailTypeEnum.EntrySubmittedList_AllEntries.ToString())
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.LastSendSubmissionReminderEmailDate).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.LastSendSubmissionReminderEmailDate).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.LastSendSubmissionReminderEmailDate).ToList();
                        break;
                }
            }
        }
        else
            flist = (from entry in flist orderby entry.Invoice descending, entry.DateCreated descending select entry).ToList();

        #endregion

        radGridEntry.DataSource = flist;

        if (needRebind) radGridEntry.DataBind();

        GeneralFunction.SetReportDataCache(flist);
    }

    #region Events

    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.Entry entry = (Effie2017.App.Entry)e.Item.DataItem;

            LinkButton lnkBtn = null;

            CheckBox chk = null;
            HyperLink lnk = null;
            Label lbl = null;

            // No
            //lbl = (Label)e.Item.FindControl("lblNo");
            //lbl.Text = counter.ToString();
            //counter++;

            //((GridDataItem)e.Item)["DateSubmitted"].Text = entry.DateSubmitted.ToString("MM/dd/yy hh:mm:ss tt");
            ((GridDataItem)e.Item)["Deadline"].Text = entry.Deadline;
            if (entry.CategoryMarket == "SM")
                ((GridDataItem)e.Item)["CategoryMarket"].Text = "Single Market";
            else if (entry.CategoryMarket == "MM")
                ((GridDataItem)e.Item)["CategoryMarket"].Text = "Multi Market";
            ((GridDataItem)e.Item)["CategoryMarket"].Text = entry.CategoryPSDetail.Replace("-", " - ").Replace("/", " / ");

            #region ProcessingStatus
            string ProcessingStatus = entry.ProcessingStatus;
            if (entry.ProcessingStatus == StatusEntry.Completed)
                ProcessingStatus = "Completed";
            else
                ProcessingStatus = GeneralFunction.GetEntryStatusForAdmin(entry.ProcessingStatus);
            ((GridDataItem)e.Item)["ProcessingStatus"].Text = ProcessingStatus;
            #endregion


            //((GridDataItem)e.Item)["PayStatus"].Text = GeneralFunction.GetPaymentEntryStatus(entry.PayStatus);
            ((GridDataItem)e.Item)["Status"].Text = GeneralFunction.GetEntryStatusForAdmin(entry.Status);

            if (entry.Status == StatusEntry.Completed)
                ((GridDataItem)e.Item)["Status"].Text = "<span style=\"font-weight:bold\">" + GeneralFunction.GetEntryStatusForAdmin(entry.Status) + "</span>";

            if (entry.WithdrawnStatus != "")
                ((GridDataItem)e.Item)["Status"].Text += "<br/><span style=\"color:Red;\">" + GeneralFunction.GetWithdrawnStatus(entry.WithdrawnStatus) + "</span>";


            Administrator administrator = AdministratorList.GetAdministratorList().Where(x => x.Id == entry.AdminidAssignedto).FirstOrDefault();
            try
            {
                ((GridDataItem)e.Item)["AdminidAssignedto"].Text = administrator.LoginId;
            }
            catch
            {
                ((GridDataItem)e.Item)["AdminidAssignedto"].Text = "";
            }






           













            // submitted by
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnBuSubmittedBy");
            Registration reg = Registration.GetRegistration(entry.RegistrationId);
            lnkBtn.Text = "-";
            if (reg != null)
            {
                lnkBtn.Text = reg.Company;
                lnkBtn.CommandArgument = reg.Id.ToString();

                // Changes by Shaik for adding new columns on 19 Oct 2015
                ((GridDataItem)e.Item)["Firstname"].Text = reg.Firstname;
                ((GridDataItem)e.Item)["Lastname"].Text = reg.Lastname;
                ((GridDataItem)e.Item)["Country"].Text = reg.Country;
            }


            lnk = (HyperLink)e.Item.FindControl("hlkEmailHistory");
            lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
            lnk.NavigateUrl = "./RegistrationEmailSentHistory.aspx?regId=" + reg.Id.ToString() + "&EntryId=" + entry.Id.ToString();


            // submitted details
            lbl = (Label)e.Item.FindControl("lblSubmittedDetails");
            lbl.Text = reg.Firstname + " " + reg.Lastname + "<br/>" + reg.Job + "<br/>" + GeneralFunction.ShowFriendlyContact(reg.Contact) + "<br/>" + reg.Email;

            string DateReminder = "";
            if (entry.DateReminder(entry.Id, EmailTypeEnum.EntrySubmittedList_AllEntries.ToString()) != DateTime.MinValue)
                DateReminder = entry.DateReminder(entry.Id, EmailTypeEnum.EntrySubmittedList_AllEntries.ToString()).ToString("dd/MM/yy H:mm");

            ((GridDataItem)e.Item)["DateReminder"].Text = (entry.LastSendSubmissionReminderEmailDate == DateTime.MaxValue) ? "" : entry.LastSendSubmissionReminderEmailDate.ToString("dd/MM/yy H:mm"); ;
            
            #region Condition for radGridEntry
            if (entry.PayStatus == StatusPaymentEntry.Paid)
            {

            }
            else if (entry.Status == StatusEntry.PaymentPending)
            {
                //lnk = (HyperLink)e.Item.FindControl("lnkUploadEntry");
                //lnk.Enabled = false;
                //lnk.Visible = true;
                //lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                //lnk.Enabled = false;
                //lnk.Visible = true;
                //lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                //lnk.Enabled = false;
                //lnk.Visible = true;
                //lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                //lnk.Enabled = false;
                //lnk.Visible = true;
            }


            if (entry.Status == StatusEntry.UploadPending || entry.Status == StatusEntry.UploadCompleted || entry.Status == StatusEntry.Completed)
            {
                //lnk = (HyperLink)e.Item.FindControl("lnkUploadEntry");
                //lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                //lnk.NavigateUrl = "../Main/UploadForAll.aspx?md=UE&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                //lnk.Enabled = true;
                //lnk.Visible = true;
                //lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                //lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                //lnk.NavigateUrl = "../Main/UploadForAll.aspx?md=UA&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                //lnk.Enabled = true;
                //lnk.Visible = true;
                //lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                //lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                //lnk.NavigateUrl = "../Main/UploadForAll.aspx?md=UC&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                //lnk.Enabled = true;
                //lnk.Visible = true;
                //lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                //lnk.CssClass = "fancybox2 fancybox.iframe tblLinkRed";
                //lnk.NavigateUrl = "../Main/UploadForCr.aspx?md=UCr&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
                //lnk.Enabled = true;
                //lnk.Visible = true;

                if (entry.Status == StatusEntry.Completed)
                {
                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadEntry");
                    //lnk.Visible = false;
                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                    //lnk.Visible = false;
                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                    //lnk.Visible = false;
                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                    //lnk.Visible = false;
                }

                //------- ENTRY FORM START-------
                /*------- DEPRECATED -------*
                 * if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_PDF.pdf"))
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkEntry1");
                    lnk.Visible = true;
                    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadEntry");
                    //lnk.Text = "Edit";
                    //lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                }
                
                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_Word.doc"))
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkEntry2");
                    lnk.Visible = true;
                    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_Word.doc?" + DateTime.Now.Ticks.ToString();
                }
                else if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\" + entry.Serial + "_EntryForm_Word.docx"))
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkEntry2");
                    lnk.Visible = true;
                    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Entry/" + entry.Id.ToString() + "/" + entry.Serial + "_EntryForm_Word.docx?" + DateTime.Now.Ticks.ToString();
                }
                else
                    ((Label)e.Item.FindControl("lbPending1")).Visible = true;

                 **------- DEPRECATED -------*/

                try
                {
                    EntryForm entryForm = EntryForm.GetEntryForm(Guid.Empty, entry.Id);
                    if (entryForm.Status != StatusEntry.Draft)
                    {
                        //string url = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + ".aspx?Id=" + GeneralFunction.StringEncryption(entry.Id.ToString());
                        string url = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?Id=" + entry.Id;
                        lnk = (HyperLink)e.Item.FindControl("lnkEntry1");
                        lnk.Visible = true;
                        lnk.NavigateUrl = url;
                    }
                    else
                    {
                        ((Label)e.Item.FindControl("lbPending1")).Visible = true;
                    }
                }
                catch
                {
                    ((Label)e.Item.FindControl("lbPending1")).Visible = true;
                }

                //------- ENTRY FORM END -------
                
                //-------
                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\" + entry.Serial + "_AuthorizationForm_PDF.pdf"))
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkAuthorisation1");
                    lnk.Visible = true;
                    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Authorisation/" + entry.Id.ToString() + "/" + entry.Serial + "_AuthorizationForm_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadAuthorisation");
                    //lnk.Text = "Edit";
                    //lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                }
                else
                {
                    ((Label)e.Item.FindControl("lbPending2")).Visible = true;
                }



                //-------
                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpg"))
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkCase1");
                    lnk.Visible = true;
                    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpg?" + DateTime.Now.Ticks.ToString();

                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                    //lnk.Text = "Edit";
                    //lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                }
                else if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpeg"))
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkCase1");
                    lnk.Visible = true;
                    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Case/" + entry.Id.ToString() + "/" + entry.Serial + "_CaseImage.jpeg?" + DateTime.Now.Ticks.ToString();

                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadCase");
                    //lnk.Text = "Edit";
                    //lnk.CssClass = "fancybox fancybox.iframe tblLinkBlack";
                }
                else
                    ((Label)e.Item.FindControl("lbPending3")).Visible = true;

                //-------
                bool ispending4 = true;
                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterials_PDF.pdf"))
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkCreative1");
                    lnk.Visible = true;
                    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterials_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                    ispending4 = ispending4 & false;
                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                    //lnk.Text = "Edit";
                    //lnk.CssClass = "fancybox2 fancybox.iframe tblLinkBlack";
                }


                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4"))
                {
                    lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnCreative2");
                    lnkBtn.Visible = true;
                    lnkBtn.CommandArgument = "../Video/DownloadMedia.aspx?filePath=" + System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + "&MediaID=" + entry.Serial + "_CreativeMaterials_Video.mp4&t=" + DateTime.Now.Ticks.ToString();

                    ispending4 = ispending4 & false;
                    //lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                    //lnk.Text = "Edit";
                    //lnk.CssClass = "fancybox2 fancybox.iframe tblLinkBlack";
                }
                
                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterialsTranslate_PDF.pdf"))
                {
                    lnk = (HyperLink)e.Item.FindControl("lnkCreative3");
                    lnk.Visible = true;
                    lnk.NavigateUrl = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "EntryUpload/Creative/" + entry.Id.ToString() + "/" + entry.Serial + "_CreativeMaterialsTranslate_PDF.pdf?" + DateTime.Now.Ticks.ToString();

                    ispending4 = ispending4 & false;
                }

                //if (GeneralFunction.FileExistsInAmazonS3(System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"], entry.Serial + "_CreativeMaterials_Video.mp4"))
                //{
                //    lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnCreative2");
                //    lnkBtn.Visible = true;
                //    lnkBtn.CommandArgument = "../Video/DownloadMedia.aspx?BucketID=" + System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"] + "&MediaID=" + entry.Serial + "_CreativeMaterials_Video.mp4";

                //    ispending4 = ispending4 & false;
                //    //lnk = (HyperLink)e.Item.FindControl("lnkUploadCreative");
                //    //lnk.Text = "Edit";
                //    //lnk.CssClass = "fancybox2 fancybox.iframe tblLinkBlack";
                //}

                if (ispending4) ((Label)e.Item.FindControl("lbPending4")).Visible = true;

            }




            //lnk = (HyperLink)e.Item.FindControl("lnkConfirm");
            //if (lnk != null)
            //{
            //    if (entry.PayStatus == StatusPaymentEntry.Paid && entry.Status == StatusEntry.UploadCompleted)
            //    {
            //        lnk.CssClass = "fancybox3 fancybox.iframe tblLinkBlack";
            //        lnk.ForeColor = System.Drawing.Color.Red;
            //        lnk.NavigateUrl = "./UploadConfirm.aspx?md=UCr&id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text);
            //        lnk.Visible = true;
            //    }
            //    else if (entry.Status == StatusEntry.Completed)
            //    {
            //        lnk.Visible = false;
            //    }
            //    else
            //    {
            //        lnk.CssClass = "tblLinkDisable";
            //        lnk.Enabled = false;
            //    }
            //}
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


            // is verified
            lbl = (Label)e.Item.FindControl("lbIsVerified");
            lbl.Text = entry.IsMaterialsVerified ? "Yes" : "No";

            //// chkbox verification
            //chk = (CheckBox)e.Item.FindControl("chkbox");
            //if (entry.Status == StatusEntry.Completed)
            //    chk.Enabled = true;
            //else
            //    chk.Enabled = false;
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

        if (e.CommandName == "Edit")
        {
            GeneralFunction.SetFilter("MaterialsSubmittedList", txtSearch.Text, ddlSearch.SelectedValue, ddlEntryStatus.SelectedValue, ddlMarket.SelectedValue,
                                  ddlCountry.SelectedValue, ddlCategory.SelectedValue, ddlIsVerified.SelectedValue, string.Empty);

            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/EntrySubmittedList.aspx");  // to return from whereever
            Response.Redirect("./Entry.aspx?db=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Delete")
        {
            Effie2017.App.Entry.CleanDeleteEntry(new Guid(((GridDataItem)e.Item)["Id"].Text));

            //lblError.Text += ((GridDataItem)e.Item)["Serial"].Text + " has been deleted.<br>";
            lblError.Text += "Entry has been deleted.<br>";

            if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
            {
                foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
                {
                    BindEntry(false, expr.FieldName, expr.SortOrder, true);
                }
            }
            else
            {
                BindEntry(false, string.Empty, GridSortOrder.None, true);
            } 
        }
        else if (e.CommandName == "View")
        {
            GeneralFunction.SetFilter("MaterialsSubmittedList", txtSearch.Text, ddlSearch.SelectedValue, ddlEntryStatus.SelectedValue, ddlMarket.SelectedValue,
                                  ddlCountry.SelectedValue, ddlCategory.SelectedValue, ddlIsVerified.SelectedValue, string.Empty);

            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/EntrySubmittedList.aspx");  // to return from whereever

            Response.Redirect("./Entry.aspx?db=1&v=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Confirm")
        {
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(new Guid(((GridDataItem)e.Item)["Id"].Text));

            entry.Status = StatusEntry.Completed;
            entry.DateModifiedString = DateTime.Now.ToString();
            entry.Save();

            lblError.Text += "Entry has been confirmed.<br>";

            if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
            {
                foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
                {
                    BindEntry(false, expr.FieldName, expr.SortOrder, true);
                }
            }
            else
            {
                BindEntry(false, string.Empty, GridSortOrder.None, true);
            }   
        }
        else if (e.CommandName == "lnkBtnCreative2")
        {
            Session["DownloadFile"] = e.CommandArgument.ToString();
            //ltrJs.Text = "<script type=\"text/javascript\"> window.open(\"./DownloadFile.aspx\"); </script>";
            Response.Redirect(e.CommandArgument.ToString());
        }
        else if (e.CommandName == "User")
        {
            GeneralFunction.SetFilter("MaterialsSubmittedList", txtSearch.Text, ddlSearch.SelectedValue, ddlEntryStatus.SelectedValue, ddlMarket.SelectedValue,
                                  ddlCountry.SelectedValue, ddlCategory.SelectedValue, ddlIsVerified.SelectedValue, string.Empty);

            Registration reg = Registration.GetRegistration(new Guid(e.CommandArgument.ToString()));
            Security.SetLoginSessionUser(reg);
            GeneralFunction.SetRedirect("../Admin/EntrySubmittedList.aspx");  // to return from whereever
            Response.Redirect("../Admin/Profile.aspx?Id=" + reg.Id.ToString());
        }
    }

    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
        {
            foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
            {
                BindEntry(true, expr.FieldName, expr.SortOrder, false);
            }
        }
        else
        {
            BindEntry(false, string.Empty, GridSortOrder.None, false);
        }     
    }
    
    protected void radGridEntry_SortCommand(object Sender, GridSortCommandEventArgs e)
    {
        if (e.CommandArgument == "Country")
        {
            BindEntry(true, e.CommandArgument.ToString(), e.NewSortOrder, true);
        }
    }

    protected void rtabEntry_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        Telerik.Web.UI.RadTab TabClicked = e.Tab;
        string tabvalue = TabClicked.Value;
        radGridEntry.MasterTableView.SortExpressions.Clear();

        ViewState["TabFilterValue"] = tabvalue;
        ViewState["AdvanceSearch"] = "";
        BindEntry(false, string.Empty, GridSortOrder.None, true);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rtabEntry.Visible = false;


        // preselect the market if the category was selected
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
        BindEntry(false, string.Empty, GridSortOrder.None, true);

        GeneralFunction.SetFilter("MaterialsSubmittedList", txtSearch.Text, ddlSearch.SelectedValue, ddlEntryStatus.SelectedValue, ddlMarket.SelectedValue,
                                  ddlCountry.SelectedValue, ddlCategory.SelectedValue, ddlIsVerified.SelectedValue, string.Empty);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlEntryStatus.SelectedValue = "";
        ddlCategory.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        ddlMarket.SelectedValue = "";
        ddlIsVerified.SelectedValue = "";

        rtabEntry.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        rtabEntry.SelectedIndex = 0;
        BindEntry(false, string.Empty, GridSortOrder.None, true);

        GeneralFunction.ResetFilter();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExportReport();
    }


    protected void btnDownload_Click(object sender, EventArgs e)
    {
        if (Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip"))
            Directory.Delete(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip", true);

        Effie2017.App.EntryList entryList = Effie2017.App.EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.Completed);

        foreach (Effie2017.App.Entry entry in entryList)
        {
            if (entry.IsRound2) // for round 2
            {
                string multiSingle = "";
                string productSpecialty = "";
                string cat = "";

                if (entry.CategoryMarket == "MM")
                    multiSingle = "Multi Market";
                else if (entry.CategoryMarket == "SM")
                    multiSingle = "Single Market";

                if (entry.CategoryPS == "PSC")
                    productSpecialty = "Product & Services Category";
                else if (entry.CategoryPS == "SC")
                    productSpecialty = "Specialty Category";

                cat = entry.CategoryPSDetail.Replace("/", "");

                CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");
                CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");
                CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");
                CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");
            }
        }

        //preparing zipping program (7-zip)
        System.Diagnostics.Process Proc = new System.Diagnostics.Process();

        Proc.StartInfo.WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "CompressorProgram\\";
        Proc.StartInfo.Arguments = "a \"" + System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export.zip\" \"" + System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\*\"";
        Proc.StartInfo.FileName = "7za.exe";

        //zipping files
        try
        {
            Proc.Start();
            Proc.WaitForExit();

            if (Proc.ExitCode == 0) //SUCCESSED
            {
                //cleaning temp
                if (Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export"))
                    Directory.Delete(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export", true);

                //Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["storageVirtualPath"].ToString() + "TempZip/Files Export.zip", true);
                ltrJs.Text = "<script type=\"text/javascript\"> window.open(\"" + System.Configuration.ConfigurationManager.AppSettings["storageVirtualPath"].ToString() + "TempZip/Files Export.zip\"); </script>"; ;
            }
            else
            {
                lblError.Text = Proc.ExitCode.ToString();
                return;
            }

            Proc.Dispose();
        }
        catch (Exception exp)
        {
            lblError.Text = exp.Message;
            return;
        }
    }

    protected void CopyAll(string from, string to)
    {
        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);

        string[] allDirectories = Directory.GetDirectories(from);

        foreach (string directory in allDirectories)
        {
            CopyAll(directory, to + directory.Substring(directory.LastIndexOf("\\")));
        }

        string[] allFiles = Directory.GetFiles(from);

        foreach (string file in allFiles)
        {
            File.Copy(file, to + file.Substring(file.LastIndexOf("\\")));
        }
    }

    protected void btnVerify_Click(object sender, EventArgs e)
    {
        bool isSelected = false;
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            if (chkbox.Enabled && chkbox.Checked)
            {
                Entry entry = Entry.GetEntry(new Guid(item["Id"].Text));
                if (chkbox.Checked && entry.Status == StatusEntry.Completed)
                {
                    entry.IsMaterialsVerified = chkbox.Checked;
                    entry.DateVerified = DateTime.Now.ToString();
                }
                entry.Save();

                //chkbox.Enabled = false; // disable it
                //chkbox.Checked = false; // unchecked it
                isSelected = true;
            }
        }

        // clear the reg cache
        //GeneralFunction.GetAllRegistrationCache(true);

        if (radGridEntry.MasterTableView.SortExpressions.Count > 0)
        {
            foreach (GridSortExpression expr in radGridEntry.MasterTableView.SortExpressions)
            {
                BindEntry(true, expr.FieldName, expr.SortOrder, false);
            }
        }
        else
        {
            BindEntry(false, string.Empty, GridSortOrder.None, false);
        } 

        if (isSelected)
            lblError.Text = "Verification status updated.";
        else
            lblError.Text = "Select at least 1 entry to verify.";
    }

    #endregion

    #region Helper


    protected void btnEmailReminder_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = true;
        phPopupEmailReminder.Attributes.Add("class", "ModalPopUpSmall");
        divEditTamplate.Visible = false;
        int isChecked = 0;
        if (sender != null)
        {
            phPopupEmailReminder.Attributes.Add("class", "ModalPopUpSmall");
            divEditTamplate.Visible = false;
            if (sender != null)
            {
                Button btnCliced = (Button)sender;

                foreach (GridDataItem item in radGridEntry.MasterTableView.Items)
                {
                    CheckBox CheckBox1 = item.FindControl("chkbox") as CheckBox;
                    if (CheckBox1 != null && CheckBox1.Checked)
                    {
                        isChecked += 1;
                    }
                }
                if (isChecked == 0)
                {
                    lblError.Text = "Plese select record to send email.";
                }
                else if (isChecked >= 1)
                {
                    PopulateTemplatePanel(btnCliced);
                }
            }
        }
    }


    public void PopulateTemplatePanel(Button pressedButton)
    {
        lblTempError.Text = string.Empty;
        lblError.Text = string.Empty;

        IptechLib.Forms.RemoveHighlightControls(phSelectTemplate);

        ddlRounds.Items.Clear();
        ddlRounds.Items.Add(new ListItem("Round 1", RoundsType.Round1));
        ddlRounds.Items.Add(new ListItem("Round 2", RoundsType.Round2));
        ddlRounds.Items.Add(new ListItem("Round 1 & 2", RoundsType.BothRounds));

        if (!pressedButton.CommandName.Equals("add"))
            ddlRounds.Items.Add(new ListItem("N/A", RoundsType.NotApplicable));

        ddlRounds.Items.Insert(0, new ListItem("Please Select", string.Empty.ToString()));
        
        List<EmailTemplate> defaultEmailTempalteList = EmailTemplateList.GetEmailTemplateList(Guid.Empty)
             .Where(x => x.EmailType == EmailType.Entry.ToString() && x.IsActive && !x.IsDelete).ToList();
        
        if (defaultEmailTempalteList.Count == 0)
            lblError.Text = "No Email Template found.<br/>";

        roundRow.Visible = pressedButton.CommandName.Equals("add");
        templateRow.Visible = !pressedButton.CommandName.Equals("add");

        phSelectTemplate.Visible = (defaultEmailTempalteList.Count != 0);

        if (defaultEmailTempalteList.Count != 0)
        {
            ddlTemplateList.DataSource = defaultEmailTempalteList;
            ddlTemplateList.DataTextField = "Title";
            ddlTemplateList.DataValueField = "Id";
            ddlTemplateList.DataBind();

            ddlTemplateList.Items.Insert(0, new ListItem("Please Select", Guid.Empty.ToString()));

            hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());

            lblTitle.Text = pressedButton.Text;
        }
    }

    protected void ddlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString()))
        {
            hlkPreview.CssClass = "fancybox fancybox.iframe tblLinkRed";
            hlkPreview.NavigateUrl = "./EmailPreview.aspx?etmId=" + ddlTemplateList.SelectedValue.ToString();

            /////////////////////////////////////////////////////////////////////
            EmailTemplate emailTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));
           
            txtTemplateName.Text = emailTemplate.Title;
            txtTemplateSubject.Text = emailTemplate.Subject;
            rEditorBody.Content = GeneralFunction.CheckPlaceHolders(emailTemplate.Body, true);
            BodyTamplateMail = emailTemplate.Body;
            chkInvitation.Checked = emailTemplate.IsInvitation;
            hdfRounds.Value = emailTemplate.UserData1;
            hdfEmailCategory.Value = emailTemplate.UserData2;
            phPopupEmailReminder.Attributes.Add("class", "ModalPopUpBig");
            divEditTamplate.Visible = true;
            /////////////////////////////////////////////////////////////////////
        }

        hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (phSelectTemplate.Visible)
        {
            //EmailTemplate emailTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));

            //if (emailTemplate.Subject != txtTemplateSubject.Text || !(GeneralFunction.CheckPlaceHolders(rEditorBody.Content, false).ToString().Equals(GeneralFunction.CheckPlaceHolders(emailTemplate.Body, false).ToString())))
            //{
            //    EmailTemplate emtemp = SaveEmailTemplate();
            //    GenerateEmails(emtemp.TemplateId);
            //}
            //else
            //{
            GenerateEmails(new Guid(ddlTemplateList.SelectedValue));
            //}
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "none", "<script>ClearDataOnLoad();</script>", false);
        }
        phSelectTemplate.Visible = false;
    }

    public bool ValidateForm()
    {
        lblError.Text = string.Empty;

        //lblError.Text += IptechLib.Validation.ValidateDropDownList("Default Template", ddlTemplateList, emailTemplate.IsNew, Guid.Empty.ToString());
        lblError.Text += IptechLib.Validation.ValidateTextBox("Template Name", txtTemplateName, true, IptechLib.ValidationType.String);
        lblError.Text += IptechLib.Validation.ValidateTextBox("Subject", txtTemplateSubject, true, IptechLib.ValidationType.String);

        if (String.IsNullOrEmpty(rEditorBody.Text))
            lblError.Text += "Body required.<br/>";

        return String.IsNullOrEmpty(lblError.Text);
    }

    public EmailTemplate SaveEmailTemplate()
    {
        if (ValidateForm())
        {

            EmailTemplate emailTemplate = EmailTemplate.NewEmailTemplate();
            emailTemplate.Subject = txtTemplateSubject.Text;
            if (txtTemplateName.Text.Contains("Update"))
            {
                var Pos = txtTemplateName.Text.LastIndexOf("Update");
                var substring = txtTemplateName.Text.Substring(0, Pos + 6);
                emailTemplate.Title = substring + " " + DateTime.Now.ToString();
            }
            else if (txtTemplateName.Text.Contains("RSVP"))
            {
                var Pos = txtTemplateName.Text.LastIndexOf("RSVP");
                var substring = txtTemplateName.Text.Substring(0, Pos + 4);
                emailTemplate.Title = substring + " " + DateTime.Now.ToString();
            }
            else
                emailTemplate.Title = txtTemplateName.Text + " - " + DateTime.Now.ToString();
            emailTemplate.Body = GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true);
            emailTemplate.IsActive = true;
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

    protected void btnClose_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = false;
    }
    public void GenerateEmails(Guid templateId)
    {
        string evetnYear = string.Empty;
        try
        {
            evetnYear = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value;
        }
        catch { }

        lblError.Text = string.Empty;
        int counter = 0;
        int counter2 = 0;
        List<Guid> regIdList = new List<Guid>();
        List<Guid> EntryList = new List<Guid>();
        Registration reg2 = null;
        Entry entry = null;
        Registration reg = null;
        List<RegEntry> oRegentry = new List<RegEntry>();
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            if (chkbox.Checked)
            {
                entry = Entry.GetEntry(new Guid(item["Id"].Text));
                reg = Registration.GetRegistration(entry.RegistrationId);
                oRegentry.Add(new RegEntry (entry.Id, entry.RegistrationId));
            }
            chkbox.Checked = false;
        }
        oRegentry = oRegentry.OrderBy(x => x.regid).ToList();
        foreach (RegEntry RegEntryItem in oRegentry)
        {
            entry = Entry.GetEntry(RegEntryItem.entryid);
            reg = Registration.GetRegistration(RegEntryItem.regid);
            if (!regIdList.Contains(reg.Id) && counter2 != 0)
            {
                Email.SendReminderEmailTemplatelReg(reg2, GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true), txtTemplateSubject.Text, EntryList, "MaterialSubmitted");
                foreach (Guid entrylistItem in EntryList)
                {
                    GeneralFunction.SaveEmailSentLogReg(reg2, templateId, evetnYear, "EntrySubmittedList_AllEntries", entrylistItem);
                    Entry entry2 = Entry.GetEntry(entrylistItem);
                    entry2.LastSendSubmissionReminderEmailDateString = DateTime.Now.ToString();
                    entry2.Save();
                }
                EntryList.Clear();
                counter++;
            }
            if (counter2 == oRegentry.Count - 1)
            {
                EntryList.Add(entry.Id);
                Email.SendReminderEmailTemplatelReg(reg, GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true), txtTemplateSubject.Text, EntryList, "MaterialSubmitted");
                foreach (Guid entrylistItem in EntryList)
                {
                    GeneralFunction.SaveEmailSentLogReg(reg, templateId, evetnYear, "EntrySubmittedList_AllEntries", entrylistItem);
                    Entry entry2 = Entry.GetEntry(entrylistItem);
                    entry2.LastSendSubmissionReminderEmailDateString = DateTime.Now.ToString();
                    entry2.Save();
                }
                EntryList.Clear();
                counter++;
            }

            regIdList.Add(reg.Id);
            EntryList.Add(entry.Id);
            reg2 = reg;
            counter2++;
        }
        
        oRegentry.Clear();

        if (counter == 0)
            lblError.Text = "Please select atleast one registration to send email.<br/>";
        else
        {
            lblError.Text = "Email sent " + (counter).ToString() + " .<br/>";
        }
        phSelectTemplate.Visible = false;
        radGridEntry.Rebind();
    }

    private ListItem SeparatorItem()
    {
        ListItem separator = new ListItem("-------------------------------", "");
        separator.Attributes.Add("disabled", "disabled");

        return separator;
    }

    protected void ExportReport()
    {
        object data = GeneralFunction.GetReportDataCache();
        if (data != null)
        {
            List<Entry> flist = (List<Entry>)data;
            // lbError.Text = "";
            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();

            int x = 1;
            int y = 1;

            string sheetName = "sheet1";
            workbook.Worksheets.Add(sheetName);
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DL"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry ID"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Status"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Submitted By"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryFormPDF"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("EntryFormWord"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AuthorizationForm"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CaseImage"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CreativeMaterials1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CreativeMaterials2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CreativeMaterials3"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Materials Verified"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Sal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("First Name"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Last Name"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Job Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Contact"); x++;


            ////////////////////////
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assign"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Reminder"); x++;
            ////////////////////////
            y++;
            foreach (Entry ent in flist)
            {
                string Category = null;

                x = 1;

                if (ent.CategoryMarket == "SM")
                    Category = "Single Market";
                else if (ent.CategoryMarket == "MM")
                    Category = "Multi Market";


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Deadline); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.DateSubmitted); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Campaign); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Client); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.CategoryPSDetail); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryStatusForAdmin(ent.Status)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Registration.GetRegistration(ent.RegistrationId).Company); x++;

                //EntryForm Change to only form
                #region old method
                //if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + ent.Id.ToString() + "\\" + ent.Serial + "_EntryForm_PDF.pdf"))
                //    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_EntryForm_PDF.pdf");
                //x++;
                #endregion
                #region new method
                try
                {
                    EntryForm entryForm = EntryForm.GetEntryForm(Guid.Empty, ent.Id);
                    if (entryForm.Status != StatusEntry.Draft)
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunctionEntryForm.GetEntryCategory(ent) + "PDF.aspx?Id=" + ent.Id);
                }
                catch
                {
                }
                x++;
                #endregion

                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Entry\\" + ent.Id.ToString() + "\\" + ent.Serial + "_EntryForm_Word.doc"))
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_EntryForm_Word.doc");
                x++;

                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + ent.Id.ToString() + "\\" + ent.Serial + "_AuthorizationForm_PDF.pdf"))
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_AuthorizationForm_PDF.pdf");
                x++;

                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + ent.Id.ToString() + "\\" + ent.Serial + "_CaseImage.jpg"))
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CaseImage.jpg");
                else if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + ent.Id.ToString() + "\\" + ent.Serial + "_CaseImage.jpeg"))
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CaseImage.jpeg");
                x++;

                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + ent.Id.ToString() + "\\" + ent.Serial + "_CreativeMaterials_PDF.pdf"))
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CreativeMaterials_PDF.pdf");
                x++;


                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + ent.Serial + "_CreativeMaterials_Video.mp4"))
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CreativeMaterials_Video.mp4");

                }
                //if (GeneralFunction.FileExistsInAmazonS3(System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Original"], ent.Serial + "_CreativeMaterials_Video.mp4"))
                //{
                //    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CreativeMaterials_Video.mp4");                    
                //}
                x++;
                
                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + ent.Id.ToString() + "\\" + ent.Serial + "_CreativeMaterialsTranslate_PDF.pdf"))
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ent.Serial + "_CreativeMaterialsTranslate_PDF.pdf");
                x++;



                string verified = ent.IsMaterialsVerified ? "Yes" : "No";
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(verified); x++;


                Registration reg = Registration.GetRegistration(ent.RegistrationId);
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Firstname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Lastname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Email); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Job); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Contact); x++;
                ////////////////////////////////////////////
                Administrator Assign = null;
                try
                {
                    Assign = AdministratorList.GetAdministratorList().Where(h => h.Id == ent.AdminidAssignedto).FirstOrDefault();
                }
                catch { }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Assign != null ? Assign.LoginId : ""); x++;

                string DateReminder = "";
                if (ent.DateReminder(ent.Id, EmailTypeEnum.EntrySubmittedList_AllEntries.ToString()) != DateTime.MinValue)
                    DateReminder = ent.DateReminder(ent.Id, EmailTypeEnum.EntrySubmittedList_AllEntries.ToString()).ToString("dd/MM/yy H:mm");

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((ent.LastSendSubmissionReminderEmailDate == DateTime.MaxValue) ? "" : ent.LastSendSubmissionReminderEmailDate.ToString("dd/MM/yy H:mm")); x++;
                ////////////////////////////////////////////
                y++;
            }
            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Submitted.xlsx");
            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    #endregion

    public class RegEntry
    {
        public Guid entryid;
        public Guid regid;
        public RegEntry(Guid entryid, Guid regid)
        {
            this.entryid = entryid;
            this.regid = regid;
        }

    }
}
 