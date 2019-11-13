using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using Telerik.Web.UI;
using System.Data;
using ClosedXML.Excel;
using System.IO;

public partial class Admin_EntryProcessing : PageSecurity_Admin
{
    #region GlobalVariable
    string[] Delimiter = { "#&&##&&#", "&&&$$&&&", "************", "||" };
    public static List<Entry> CurrentEntries = null;
    public static Entry EntrySelected = null;
    public static EmailTemplate CurEmailTemplate;
    public static Administrator admin;
    public static string PageTitle;
    public const string DefaultPage = StatusEntry.PendingVerification;//"ALLP";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        doPostBack(sender, e);

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }
    
    protected void doPostBack(object sender, EventArgs e)
    {
        admin = Security.GetAdminLoginSession();
        
        //ViewState["CurrentPage"] = "";
        PageTitle = "Entry Processing";
        if (Security.IsRoleSuperAdmin())
        {
            //ddlAssignedTo.Enabled = true;
            //ViewState["CurrentPage"] = Request.QueryString["Page"];
            //PageTitle = "Entry Processing Management";
            //if (admin.Access != "SA")
            //    Response.Redirect("./EntryProcessing.aspx");
        }
        else
        {
            //ddlAssignedTo.Enabled = false;
        }

        up1.Save_Clicked += new EventHandler(PaySave_Clicked);
        up1.Cancel_Clicked += new EventHandler(PayCancel_Clicked);

        //try
        //{
        //    string parameter = Request["__EVENTARGUMENT"];
        //    if (parameter == "inviteEmail")
        //        btnSendMail_Click(sender, e);
        //}
        //catch { }
    }

    private void PopulateForm(string CurrentTab = DefaultPage)
    {
        ViewState["TabFilterValue"] = CurrentTab;
        BindGrid(false, string.Empty, GridSortOrder.None, true);
    }

    protected void initddlupdateProcessingStatus(string State = "") {
        ddlupdateProcessingStatus.Items.Clear();
        //try
        {
            if (State == StatusEntry.Reopened)
            {
                ddlupdateProcessingStatus.Items.Add(new ListItem("Completed", StatusEntry.Completed));
            }
            else
            {
                ddlupdateProcessingStatus.Items.Add(new ListItem("Pending Upload", StatusEntry.UploadPending));
                ddlupdateProcessingStatus.Items.Add(new ListItem("Completed", StatusEntry.Completed));
                ddlupdateProcessingStatus.Items.Add(new ListItem("Pending Verification", StatusEntry.PendingVerification));
                ddlupdateProcessingStatus.Items.Add(new ListItem("Reopened", StatusEntry.Reopened));
            }
        }
        //catch { }
    }

    private void LoadForm()
    {
        rptFlagReason.DataSource = Data.GetFlagReasons();
        rptFlagReason.DataBind();
        divDQFlagNotification.Attributes.Add("class", "ModalPopUpSmall");
        divEditTamplate.Visible = false;
        initddlupdateProcessingStatus();
        
        foreach (FlagReasons flagReasons in FlagReasonsList.GetFlagReasonsList())
        {
            cblFlagReason.Items.Add(new ListItem(flagReasons.Bodyname, flagReasons.Id.ToString()));
        }
        cblFlagReason.DataBind();

        try
        {
            ddlAssignedTo.Items.Add(new ListItem("All", ""));
            AdministratorList administratorList = AdministratorList.GetAdministratorList();
            foreach (Administrator admin in administratorList) //.Where(x => x.Access == "ST")
            {
                ddlAssignTo.Items.Add(new ListItem(admin.LoginId, admin.Id.ToString()));
                ddlAssignedTo.Items.Add(new ListItem(admin.LoginId, admin.Id.ToString()));
            }
        }
        catch { }
        // Country
        List<string> Countries = GeneralFunction.GetFilteredCountryList(false).ToList(); //.Where(x => x != "South Korea").ToList();
        ddlCountry.DataSource = Countries;
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));
        // Category
        LoadCategories();

        SetColumn();
    }

    protected void LoadCategories()
    {
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
    }

    protected void rptFlagReason_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Data.CollectData item = (Data.CollectData)e.Item.DataItem;

            HiddenField hdID = (HiddenField)e.Item.FindControl("hdItemId");
            Literal title = (Literal)e.Item.FindControl("Title");
            HiddenField hdAttrType = (HiddenField)e.Item.FindControl("hdAttrType");
            CheckBox cbItem = (CheckBox)e.Item.FindControl("cbItem");
            TextBox txtOtherDQ = (TextBox)e.Item.FindControl("txtOtherDQ");
            CheckBox isHasOther = (CheckBox)e.Item.FindControl("isHasOther");

            hdID.Value = item.id.ToString();
            hdAttrType.Value = item.AttrType;
            title.Text = item.Title;
            txtOtherDQ.Visible = false;
            isHasOther.Checked = item.isHasOther;

            if (item.isHasOther)
            {
                txtOtherDQ.Visible = true;
                txtOtherDQ.Text = item.Data1;

                System.Web.UI.HtmlControls.HtmlTableRow bc = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trOther");
                bc.Visible = true;
            }

            cbItem.Checked = (item.Data2 == "True") ? true : false;
        }
    }

    protected List<Data.CollectData> GetSelectedFlagReasons(Registration reg, Entry entry)
    {
        List<Data.CollectData> CollectData = new List<Data.CollectData>();
        
        if (!string.IsNullOrEmpty(entry.FlagReason))
        {
            string[] FlagReasonList = entry.FlagReason.Split(new string[] { Delimiter[3] }, System.StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string item in FlagReasonList)
            {
                string ID = "", data = "";
                try
                {
                    ID = item.Split(':')[0];
                    data = item.Split(':')[1];

                    Data.CollectData Col = Data.GetFlagReasons().FirstOrDefault(x => x.id == ID);
                    Col.Data1 = data;
                    Col.Entry = entry;
                    Col.Registration = reg;
                    if (data == "True")
                        CollectData.Add(Col);
                }

                catch { ID = ""; data = ""; }
            }
        }
        return CollectData;
    }


    protected XLWorkbook PopulateExcelRow(ref XLWorkbook workbook, ref int y, ref int x, ref string sheetName, Entry entry)
    {
        x = 1;
        string TempDate = "";
        Registration reg = Registration.GetRegistration(entry.RegistrationId);
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Deadline); x++; 
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Email); x++; 
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Salutation); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Firstname); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Lastname); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Country); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Job); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Company); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.PayCity); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Serial); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Campaign); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Client); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Brand); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Data.GetCategoryMarket(entry.CategoryMarket)); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Data.GetCategoryPS(entry.CategoryPS)); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.CategoryPSDetail); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.WithdrawnStatus); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.DateSubmitted.ToString("dd MMM yyyy")); x++;

        Administrator Assign = null;
        try
        {
            Assign = AdministratorList.GetAdministratorList().Where(h => h.Id == entry.AdminidAssignedto).FirstOrDefault();
        }
        catch { }

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(Assign != null ? Assign.LoginId : ""); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryStatusForAdmin(entry.Status)); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetPaymentEntryStatus(entry.PayStatus)); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetEntryStatusForAdmin(entry.ProcessingStatus)); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.DQFlag); x++;
        
        string SelectedFlagReason = GeneralFunction.GetSelectedFlagReasons(entry.FlagReason, true).Replace("||", ", ");
        if (!string.IsNullOrEmpty(SelectedFlagReason))
        {
            SelectedFlagReason = SelectedFlagReason.Substring(0, SelectedFlagReason.Length - 2) + ".";
        }
       
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(SelectedFlagReason); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.FlagDQDescription); x++;
        
        try { TempDate = Convert.ToDateTime(entry.NotificationSentDate).ToString("dd MMM yyyy"); } catch { TempDate = ""; }
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(TempDate); x++;
        
        Administrator ReopenedBy = null;
        try {
            ReopenedBy = AdministratorList.GetAdministratorList().FirstOrDefault(h => h.Id == new Guid(string.IsNullOrEmpty(entry.ReopenedBy) ? Guid.Empty.ToString() : entry.ReopenedBy));
        }
        catch { }
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ReopenedBy != null ? ReopenedBy.LoginId : ""); x++;
        string ReopeningFee = "";
        try {
            AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(new Guid(entry.IDAdhocInvoice));
            ReopeningFee = adInv.Amount.ToString("0.00");
        }
        catch {
            ReopeningFee = "0.00";
        }
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ReopeningFee); x++; 
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.ReasonFeeWaiver); x++; //TODO

        try { TempDate = Convert.ToDateTime(entry.ReopeningDate).ToString("dd MMM yyyy"); } catch { TempDate = ""; }
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(TempDate); x++;

        //try { TempDate = Convert.ToDateTime(entry.ReopeningDeadline).ToString("dd MMM yyyy"); } catch { TempDate = ""; }
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(TempDate); x++;

        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.OtherRemarks); x++;
        y++;
        return workbook;
    }


    protected void btnReport_Click(object sender, EventArgs e)
    {
        int x = 1;
        int y = 1;
        string sheetName = "Entry Processing Report Final";
        XLWorkbook workbook = new XLWorkbook();
        MemoryStream memoryStream = new MemoryStream();
        workbook.Worksheets.Add(sheetName);

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("DL"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Sal"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Job"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("City"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Id"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Title"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Client"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Brand"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category Market"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category PS"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Category PSDetail"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Withdrawn Status"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Submitted"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assign"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry Status"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Payment Status"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Processing Status"); x++;

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Flag"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Flag Reason(s)"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Flag Description"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Notify Date"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reopened By"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reopening Fee"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reason for Fee Waiver"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Reopen"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reopening Deadline"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Remarks"); x++;

        y++;

        //if (isAnySelected())
        //{
        //    foreach (GridDataItem item in radGridEntry.Items)
        //    {
        //        CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
        //        HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
        //        Guid EntryId = new Guid(hdfId.Value);
        //        if (chkbox.Checked)
        //        {
        //            chkbox.Checked = false;
        //            Entry entry = Entry.GetEntry(EntryId);
        //            PopulateExcelRow(ref workbook, ref y, ref x, ref sheetName, entry);
        //        }
        //    }
        //}
        //else
        {
            foreach (Entry entry in CurrentEntries)
            {
                PopulateExcelRow(ref workbook, ref y, ref x, ref sheetName, entry);
            }
        }

        workbook.SaveAs(memoryStream);

        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=Effie Entry Processing Report Final.xlsx");

        memoryStream.WriteTo(Response.OutputStream);
        Response.End();
    }
    

    private void BindGrid(bool isCustomSort, string sortExpression, GridSortOrder order, bool needRebind)
    {
        // filter
        string status = (string)ViewState["TabFilterValue"];
        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Entry> list = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "").ToList();
        if (CurrentEntries != null && advanceSearch == "1")
            list = CurrentEntries;

        if (status == "ALL" || status == "ALLP" /*|| status == DefaultPage*/)
        {
            if (status == "ALL")
            {
                list = list.Where(x => (x.Status == StatusEntry.PaymentPending || x.Status == StatusEntry.Completed || x.Status == StatusEntry.UploadPending || x.Status == StatusEntry.UploadCompleted)).ToList();
            }
            if (status == "ALLP")
            {
                list = list.Where(x => ((x.Status == StatusEntry.Completed || x.Status == StatusEntry.UploadPending || x.Status == StatusEntry.UploadCompleted) &&
                       !string.IsNullOrEmpty(x.ProcessingStatus))).ToList();
            }
            status = "";
        }
        
        List<Entry> flist = new List<Entry>();
        List<Entry> slist = new List<Entry>();
        foreach (Entry entry in list)
        {
            slist.Add(entry);
        }

        if (advanceSearch == "1")
        {
            List<Guid> entryIdList2 = GeneralFunction.GetFilteredEntryListFromRegCompany(txtSearch.Text.Trim(), true);
            List<Guid> entryIdList = GeneralFunction.GetFilteredEntryListFromRegCountry(ddlCountry.SelectedValue, true);
            List<Guid> entryIdList3 = GeneralFunction.GetFilteredEntryListFromRegFirstName(txtSearch.Text.Trim(), true);
            List<Guid> entryIdList4 = GeneralFunction.GetFilteredEntryListFromRegLastName(txtSearch.Text.Trim(), true);

            foreach (Entry item in slist)
            {
                // category strip out the prefix
                string category = ddlCategory.SelectedValue;
                if (category.Length > 3) category = category.Substring(3, category.Length - 3);

                if (
                    (ddlDeadline.SelectedValue == "" || (ddlDeadline.SelectedValue != "" && item.Deadline == ddlDeadline.SelectedValue)) &&
                    (ddlMarket.SelectedValue == "" || (ddlMarket.SelectedValue != "" && item.CategoryMarket == ddlMarket.SelectedValue)) &&
                    (ddlAssignedTo.SelectedValue == "" || (ddlAssignedTo.SelectedValue != "" && item.AdminidAssignedto == (new Guid(ddlAssignedTo.SelectedValue)))) &&
                    (category == "" || (category != "" && (item.CategoryPSDetail == category || GeneralFunction.IsCategoryInCategoryGroup(category, item.CategoryPSDetail)))) &&
                    
                    (ddlProcessingStatus.SelectedValue == "" || (ddlProcessingStatus.SelectedValue != "" && item.ProcessingStatus == ddlProcessingStatus.SelectedValue)) &&
                    (ddlEntryStatus.SelectedValue == "" || (ddlEntryStatus.SelectedValue != "" && item.Status == ddlEntryStatus.SelectedValue)) &&
                    
                     (ddlDQFlag.SelectedValue == "" || (ddlDQFlag.SelectedValue != "" && item.DQFlag == ddlDQFlag.SelectedValue)) &&
                     (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && entryIdList.Contains(item.Id))) &&

                    ((txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && item.Campaign.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "EntryID") && item.Serial.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && entryIdList2.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "firstname") && entryIdList3.Contains(item.Id))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "lastname") && entryIdList4.Contains(item.Id)))))

                    flist.Add(item);
            }
        }
        else
        {
            foreach (Entry item in slist)
                if (status == "" || (status != "" && item.ProcessingStatus == status)) flist.Add(item);
        }       
        
        #region CustomSorting

        if (isCustomSort)
        {
            List<Registration> regList = RegistrationList.GetRegistrationList(string.Empty, string.Empty, string.Empty).ToList();

            if (sortExpression.Equals("DateCreated"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateCreated select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateCreated descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateCreated select entry).ToList();
                        break;
                }
            }
            if (sortExpression.Equals("DateCreated"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateCreated select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateCreated descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.DateCreated select entry).ToList();
                        break;
                }
            }
            if (sortExpression.Equals("Campaign"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Campaign).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Campaign).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Campaign).ToList();
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
            else if (sortExpression.Equals("MaterialsSubmitted"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.MaterialsSubmitted).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.MaterialsSubmitted).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.MaterialsSubmitted).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("ReopeningDate"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.ReopeningDate).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.ReopeningDate).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.ReopeningDate).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("ReopeningDeadline"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.ReopeningDeadline).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.ReopeningDeadline).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.ReopeningDate).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("NotificationSentDate"))
            {
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
            else if (sortExpression.Equals("AdminidAssignedto"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => (AdministratorList.GetAdministratorList().Where(y => y.Id == x.AdminidAssignedto).FirstOrDefault() != null) ?
                                                    AdministratorList.GetAdministratorList().Where(y => y.Id == x.AdminidAssignedto).FirstOrDefault().LoginId : "").ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => (AdministratorList.GetAdministratorList().Where(y => y.Id == x.AdminidAssignedto).FirstOrDefault() != null) ?
                                                    AdministratorList.GetAdministratorList().Where(y => y.Id == x.AdminidAssignedto).FirstOrDefault().LoginId : "").ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => (AdministratorList.GetAdministratorList().Where(y => y.Id == x.AdminidAssignedto).FirstOrDefault() != null) ?
                                                    AdministratorList.GetAdministratorList().Where(y => y.Id == x.AdminidAssignedto).FirstOrDefault().LoginId : "").ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("DQFlag"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => !string.IsNullOrEmpty(x.FlagReason) && (!string.IsNullOrEmpty(x.DQFlag) || x.DQFlag == "None")).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => !string.IsNullOrEmpty(x.FlagReason) && (!string.IsNullOrEmpty(x.DQFlag) || x.DQFlag == "None")).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => !string.IsNullOrEmpty(x.FlagReason) && (!string.IsNullOrEmpty(x.DQFlag) || x.DQFlag == "None")).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Deadline"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.Deadline).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.Deadline).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.Deadline).ToList();
                        break;
                }
            }

            else if (sortExpression.Equals("SubmittedBy"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => GeneralFunction.GetRegistrationFromEntry(x).Company).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => GeneralFunction.GetRegistrationFromEntry(x).Company).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => GeneralFunction.GetRegistrationFromEntry(x).Company).ToList();
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
            else if (sortExpression.Equals("DateVerified"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = flist.OrderBy(x => x.DateVerified).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = flist.OrderByDescending(x => x.DateVerified).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = flist.OrderBy(x => x.DateVerified).ToList();
                        break;
                }
            }
            else if (sortExpression.Equals("Serial"))
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
            else if (sortExpression.Equals("DateReminder"))
            {
                switch (order)
                {
                    case GridSortOrder.Ascending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.LastSendUploadReminderEmailDate select entry).ToList();
                        break;
                    case GridSortOrder.Descending:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.LastSendUploadReminderEmailDate descending select entry).ToList();
                        break;
                    case GridSortOrder.None:
                        flist = (from entry in flist join reg in regList on entry.RegistrationId equals reg.Id orderby entry.LastSendUploadReminderEmailDate select entry).ToList();
                        break;
                }
            }           
        }
        else
            flist = (from entry in flist orderby entry.Invoice descending, entry.DateCreated descending select entry).ToList();

        #endregion
        
        if (Security.IsReadOnlyAdmin()) //Security.IsRoleAdmin2() && ViewState["TabFilterValue"].ToString() != "ALL"
        {
            flist = flist.Where(x => x.AdminidAssignedto == admin.Id).ToList();
        }

        if(!(advanceSearch == "1"))
            CurrentEntries = flist;

        radGridEntry.DataSource = flist;
        if (needRebind)
            radGridEntry.DataBind();
        
    }

    #region Events
    private void PaySave_Clicked(object sender, EventArgs e)
    {
        phPay.Visible = false;
        BindGrid(false, string.Empty, GridSortOrder.None, true);
    }

    private void PayCancel_Clicked(object sender, EventArgs e)
    {
        phPay.Visible = false;
    }

    protected AdhocInvoiceItem GetAdhocInvoiceItem(Registration reg, Entry entry)
    {
        AdhocInvoiceList invoiceList = AdhocInvoiceList.GetAdhocInvoiceList(Guid.Empty, Guid.Empty);
        List<AdhocInvoice> adhocInvoiceList = invoiceList.Where(m => !String.IsNullOrEmpty(m.Invoice) && m.RegistrationId == reg.Id).ToList();
        List<AdhocInvoiceItem> adhocInvoiceItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(Guid.Empty, Guid.Empty).Where(x => x.InvoiceType == "ReOpen" && x.EntryId == entry.Id).ToList();
        AdhocInvoiceItem adhocInvoiceItem = (from AI in adhocInvoiceList
                                             join AII in adhocInvoiceItemList on AI.Id equals AII.AdhocInvoiceId
                                             orderby AII.DateCreated descending
                                             select AII).FirstOrDefault();
        return adhocInvoiceItem;
    }

    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Entry entry = (Entry)e.Item.DataItem;
            try
            {
                if (CurrentEntries != null && ViewState["AdvanceSearch"].ToString() == "1")
                    entry = Entry.GetEntry(entry.Id);
            }
            catch { }

            LinkButton lnkBtn = null;
            Label lbl = null;
            HyperLink lnk = null;

            HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");
            hdfId.Value = entry.Id.ToString();

            ((GridDataItem)e.Item)["Client"].Text = entry.Client.Replace(")", ") ");
            
            ((GridDataItem)e.Item)["CategoryMarket"].Text = /*Data.GetCategoryMarket(entry.CategoryMarket) + "<br>" + */entry.CategoryPSDetail.Replace("/"," / "); 
            ((GridDataItem)e.Item)["Status"].Text = GeneralFunction.GetEntryStatusForAdmin(entry.Status);
            ((GridDataItem)e.Item)["Deadline"].Text = entry.Deadline;
            string ProcessingStatus = entry.ProcessingStatus;
            if (entry.ProcessingStatus == StatusEntry.Completed)
                ProcessingStatus = "Completed";
            else
                ProcessingStatus = GeneralFunction.GetEntryStatusForAdmin(entry.ProcessingStatus);
            
            ((GridDataItem)e.Item)["ProcessingStatus"].Text = "<span style='font-weight: bold;'>" + ProcessingStatus + "</span>";
            
            string DQFlag = string.IsNullOrEmpty(entry.DQFlag) ? "None" : entry.DQFlag;
            if (!string.IsNullOrEmpty(entry.FlagReason))
            {
                string Text = GeneralFunction.GetSelectedFlagReasons(entry.FlagReason).Replace("||", "<br>");
                string Tooltips = "<a href=\"#\" class=\"tooltip\"><img src = \"../images/icon-info.png\" width = \"15\" height = \"15\" /><span style=\"margin-left: -430px; width: 400px; \"> " + Text + "</span ></a> ";
                DQFlag += " " + Tooltips;
            }

            ((GridDataItem)e.Item)["DQFlag"].Text = DQFlag;

            ((GridDataItem)e.Item)["MaterialsSubmitted"].Text = GetDateDB(entry.MaterialsSubmitted);
            ((GridDataItem)e.Item)["ReopeningDate"].Text = GetDateDB(entry.ReopeningDate);
            ((GridDataItem)e.Item)["ReopeningDeadline"].Text = GetDateDB(entry.ReopeningDeadline);
            ((GridDataItem)e.Item)["NotificationSentDate"].Text = GetDateDB(entry.NotificationSentDate);
            ((GridDataItem)e.Item)["DateVerified"].Text = GetDateDB(entry.DateVerified);
            
            Registration reg = Registration.GetRegistration(entry.RegistrationId);

            if (reg != null)
            {
                string url = "<a href=\""+ "../Admin/Profile.aspx?Id=" + reg.Id.ToString() + "\">"+ reg.Company + "</a>";
                string Tips = reg.Firstname + " " + reg.Lastname + "<br/>" + reg.Job + "<br/>" + GeneralFunction.ShowFriendlyContact(reg.Contact) + "<br/>" + reg.Email;
                
                ((GridDataItem)e.Item)["SubmittedBy"].Text = url + " <a href=\"#\" class=\"tooltip\"><img src = \"../images/icon-info.png\" width = \"15\" height = \"15\" /><span> " + Tips + "</span ></a> ";

                // Changes by Shaik for adding new columns on 19 Oct 2015
                ((GridDataItem)e.Item)["Firstname"].Text = reg.Firstname;
                ((GridDataItem)e.Item)["Lastname"].Text = reg.Lastname;
                ((GridDataItem)e.Item)["Country"].Text = reg.Country;
            }
            
            try {
                if (!string.IsNullOrEmpty(entry.IDAdhocInvoice))
                {
                    AdhocInvoice adhocinvoice = AdhocInvoice.GetAdhocInvoice(new Guid(entry.IDAdhocInvoice));
                    string AdhocInvoiceUrl = "./AdhocPaymentPdfView.aspx?regId=" + GeneralFunction.StringEncryption(reg.Id.ToString()) + "&adId=" + GeneralFunction.StringEncryption(entry.IDAdhocInvoice);
                    lnk = (HyperLink)e.Item.FindControl("lnkInvoice");
                    lnk.NavigateUrl = AdhocInvoiceUrl;
                    lnk.Text = adhocinvoice.Invoice;
                    if (string.IsNullOrEmpty(adhocinvoice.Invoice))
                    {
                        string InvoiceUrl = "AdhocInvoiceSummary.aspx?adm=e&pgId=" + GeneralFunction.StringEncryption(adhocinvoice.PayGroupId.ToString()) + "&EntryId=" + GeneralFunction.StringEncryption(entry.Id.ToString());
                        lnk.NavigateUrl = InvoiceUrl;
                        lnk.Text = "Update Payment";
                    }
                }
            }
            catch { }
            
            // Adhoc Invoice
            lnkBtn = (LinkButton)e.Item.FindControl("lbkAdhocInvoice");
            lnkBtn.CommandArgument = entry.RegistrationId.ToString();
            
            // update payment
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
            lnkBtn.CommandArgument = entry.Id.ToString();
            
            Administrator administrator = AdministratorList.GetAdministratorList().Where(x => x.Id == entry.AdminidAssignedto).FirstOrDefault();
            try {
                ((GridDataItem)e.Item)["AdminidAssignedto"].Text = administrator.LoginId;
            }
            catch {
                ((GridDataItem)e.Item)["AdminidAssignedto"].Text = "";
            }

            try {
                lnk = (HyperLink)e.Item.FindControl("hlkEmailHistory");
                lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                lnk.NavigateUrl = "./RegistrationEmailSentHistory.aspx?regId=" + reg.Id.ToString() + "&EntryId=" + entry.Id.ToString();
            }
            catch { }

            try {
                EntryForm entryForm = EntryForm.GetEntryForm(Guid.Empty, entry.Id);
                string url = "../Main/" + GeneralFunctionEntryForm.GetEntryCategory(entry) + "PDF.aspx?Id=" + entry.Id;
                lnk = (HyperLink)e.Item.FindControl("lnkBtnView");
                lnk.Text = "View Entry";
                lnk.Visible = true;
                lnk.NavigateUrl = url;
            }
            catch {
                lnk = (HyperLink)e.Item.FindControl("lnkBtnView");
                lnk.Text = "View Entry";
                lnk.Visible = true;
                lnk.Enabled = false;
            }


            { // ACTIONS BUTTON
                switch (ViewState["TabFilterValue"].ToString())
                {
                    case StatusEntry.PendingVerification:
                        lnk = (HyperLink)e.Item.FindControl("lnkBtnView");
                        lnk.Text = "View Entry";

                        //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnView");
                        //lnkBtn.Text = "View Entry";

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnViewDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateStatus");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkCompleteProcessing");

                        if(Security.IsRoleST() || Security.IsReadOnlyAdmin())
                            lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lbkAdhocInvoice");
                        if (Security.IsRoleST() || Security.IsReadOnlyAdmin())
                            lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateDQI");
                        if (Security.IsRoleST() || Security.IsReadOnlyAdmin())
                            lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnReopen");
                        if(Security.IsRoleST() || Security.IsReadOnlyAdmin())
                            lnkBtn.Visible = false;

                        break;
                    case StatusEntry.PendingReopen:
                        lnk = (HyperLink)e.Item.FindControl("lnkBtnView");
                        lnk.Text = "View Entry";

                        //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnView");
                        //lnkBtn.Text = "View Entry";

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnViewDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateStatus");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkCompleteProcessing");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lbkAdhocInvoice");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnReopen");
                        lnkBtn.Visible = false;

                        break;
                    case StatusEntry.Reopened:
                        lnk = (HyperLink)e.Item.FindControl("lnkBtnView");
                        lnk.Text = "View Entry";

                        //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnView");
                        //lnkBtn.Text = "View Entry";

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnReopen");
                        lnkBtn.Visible = true;
                        if (entry.Status != StatusEntry.Completed)
                            lnkBtn.Visible = false;


                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnViewDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateStatus");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lbkAdhocInvoice");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkCompleteProcessing");
                        if (Security.IsRoleST() || Security.IsReadOnlyAdmin())
                            lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateDQI");
                        if (Security.IsRoleST() || Security.IsReadOnlyAdmin())
                            lnkBtn.Visible = false;

                        break;
                    case StatusEntry.Completed:
                        lnk = (HyperLink)e.Item.FindControl("lnkBtnView");
                        lnk.Text = "View Entry";

                        //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnView");
                        //lnkBtn.Text = "View Entry";

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnViewDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnReopen");
                        lnkBtn.Visible = false;
                        
                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateStatus");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lbkAdhocInvoice");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkCompleteProcessing");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkResetStatus");
                        if (Security.IsRoleST() || Security.IsReadOnlyAdmin())
                            lnkBtn.Visible = false;

                        break;
                    case "ALLP":
                        lnk = (HyperLink)e.Item.FindControl("lnkBtnView");
                        lnk.Text = "View Entry";

                        //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnView");
                        //lnkBtn.Text = "View";

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnViewDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateStatus");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnReopen");
                        lnkBtn.Visible = false;
                        
                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
                        lnkBtn.Visible = false;
                        
                        lnkBtn = (LinkButton)e.Item.FindControl("lbkAdhocInvoice");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkCompleteProcessing");
                        if (Security.IsRoleST() || Security.IsReadOnlyAdmin())
                            lnkBtn.Visible = false;

                        break;
                    case "ALL":
                        lnk = (HyperLink)e.Item.FindControl("lnkBtnView");
                        lnk.Text = "View Entry";

                        //lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnView");
                        //lnkBtn.Text = "View";

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnViewDQI");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateStatus");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnReopen");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkCompleteProcessing");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lbkAdhocInvoice");
                        lnkBtn.Visible = false;

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
                        lnkBtn.Visible = false;
                        break;
                }

                lnkBtn = (LinkButton)e.Item.FindControl("lnkbtnUpdateDQI");
                if (entry.Status != StatusEntry.Completed)
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

    protected static string GetDateDB(string stsdate)
    {
        string TempDate = "";
        try { TempDate = Convert.ToDateTime(stsdate).ToString("dd/MM/yy H:mm"); } catch { TempDate = ""; }
        return TempDate;
    }

    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";
        lblError2.Text = "";
        Entry entry = null;
        try
        {
            entry = Entry.GetEntry(new Guid(((GridDataItem)e.Item)["Id"].Text));
        }
        catch {}

        if (e.CommandName == "Edit")
        {
            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/EntryPendingList.aspx");  // to return from whereever
            Response.Redirect("../Main/Entry.aspx?db=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "View")
        {
            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/EntryPendingList.aspx");  // to return from whereever
            Response.Redirect("../Main/Entry.aspx?db=1&v=1&Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "User")
        {
            Registration reg = Registration.GetRegistration(new Guid(e.CommandArgument.ToString()));
            GeneralFunction.SetRedirect("../Admin/EntryPendingList.aspx");  // to return from whereever
            Response.Redirect("../Admin/Profile.aspx?Id=" + reg.Id.ToString());
        }
        else if (e.CommandName == "adhoc")
        {
            GeneralFunction.SetRedirect("../Admin/AdhocInvoiceList.aspx");  // to return from whereever
            string url = "../Admin/AdhocInvoice.aspx?regId=" + GeneralFunction.StringEncryption(e.CommandArgument.ToString()) + "&EntryId=" + GeneralFunction.StringEncryption(entry.Id.ToString());
            //if (!string.IsNullOrEmpty(ViewState["CurrentPage"].ToString()))
            //{
            //    url += "&Page=Management";
            //}
            Response.Redirect(url);
        }
        else if (e.CommandName == "Payment")
        {
            phPay.Visible = true;
            up1.EntryId = new Guid(e.CommandArgument.ToString());
            up1.PopulateForm();
        }
        else if (e.CommandName == "UpdateDQI")
        {
            EntrySelected = entry;
            InitrptFlagReason(entry, true);
        }
        else if (e.CommandName == "ViewDQI")
        {
            InitrptFlagReason(entry, false);
        }
        else if (e.CommandName == "UpdateStatus")
        {
            EntrySelected = entry;
            phUpdateStatus.Visible = true;
        }
        else if (e.CommandName == "Reopen")
        {
            UpdateStatusProcessing(entry, StatusEntry.Reopened);
            PopulateForm(ViewState["TabFilterValue"].ToString());
        }
        else if (e.CommandName == "ResetStatus")
        {
            UpdateStatusProcessing(entry, StatusEntry.PendingVerification);
            PopulateForm(ViewState["TabFilterValue"].ToString());
        }
        
        else if (e.CommandName == "EmailHistory")
        {
            //TODO

            //phEmailHistory.Visible = true;
            //try
            //{
            //    string url = "./RegistrationEmailSentHistory.aspx?regId=" + entry.RegistrationId.ToString() + "&EntryId=" + entry.Id.ToString();
            //    iframeEmailHistory.Src = url;
            //}
            //catch { }

            //UpdateStatusProcessing(entry, StatusEntry.PendingVerification);
            //PopulateForm(ViewState["TabFilterValue"].ToString());
        }
        
        else if (e.CommandName == "CompleteProcessing")
        {
            if (entry.Status != StatusEntry.Completed)
            {
                lblError.Text = "One or more entries are not Closed.<br/>";
                lblError2.Text = lblError.Text;
                return;
            }
            UpdateStatusProcessing(entry, StatusEntry.Completed);
            PopulateForm(ViewState["TabFilterValue"].ToString());
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
        BindGrid(true, e.CommandArgument.ToString(), e.NewSortOrder, true);
    }

    protected void InitrptFlagReason(Entry entry, bool Editable){
        List<Data.CollectData> CollectDataList = Data.GetFlagReasons();
        string[] FlagReasonSplit = entry.FlagReason.Split(new string[] { Delimiter[3] }, System.StringSplitOptions.RemoveEmptyEntries);

        if (FlagReasonSplit.Count() != 0)
        {
            for (int i = 0; i < FlagReasonSplit.Length; i++)
            {
                try
                {
                    string[] Datas = FlagReasonSplit[i].Split(':');
                    string ID = Datas[0];
                    string Other = Datas[1];
                    Data.CollectData CollectData = CollectDataList.FirstOrDefault(x => x.id == ID);
                    CollectData.Data1 = Other;
                    CollectData.Data2 = "True";
                }
                catch { }
            }
        }

        rptFlagReason.DataSource = CollectDataList;
        rptFlagReason.DataBind();

        PHUpdateDQ.Visible = true;
    }

    public string ValidateEmailForm()
    {
        string Error = string.Empty;        
        Error += IptechLib.Validation.ValidateTextBox("Subject", txtTemplateSubject, true, IptechLib.ValidationType.String);

        if (String.IsNullOrEmpty(rEditorBody.Text))
            Error += "Body required.<br/>";

        return Error;
    }

    public void GenerateEmails(Guid templateId)
    {
        lblError.Text = string.Empty;
        List<Guid> ListIDReg = new List<Guid>();
        List<Entry> EntryList = new List<Entry>();
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

            if (chkbox.Checked)
            {

                Entry entry = Entry.GetEntry(new Guid(item["Id"].Text));
                if (!ListIDReg.Where(ID => ID == entry.RegistrationId).Any())
                {
                    ListIDReg.Add(entry.RegistrationId);
                }

                EntryList.Add(entry);
                chkbox.Checked = false;
            }
        }
        
        if (ListIDReg.Count() <= 0)
            lblError.Text = "Please select at leat one registration to send email.<br/>";
        else
        { 
            string DQFalgReason = GetFlagReasons(EntryList, ListIDReg);
            lblError.Text = "Email sent " + ListIDReg.Count() + " .<br/>";
            string EmailBody = GeneralFunction.CheckPlaceHolders(rEditorBody.Content, true);

            //if (!GeneralFunction.CheckPlaceHolders(CurEmailTemplate.Body, true).Contains("#FLAGREASON#"))
            if (btnDQFlagNotification.Text != "Send Email")
            {
                EmailBody = EmailBody.Replace("#FLAGREASON#", DQFalgReason);
            }
            
            SendTamplateMail(EntryList, ListIDReg, templateId, EmailBody);
            ListIDReg.Clear();
            EntryList.Clear();
        }

        phDQFlagNotification.Visible = false;
        divDQFlagNotification.Attributes.Add("class", "ModalPopUpSmall");
        divEditTamplate.Visible = false;
        PopulateForm(ViewState["TabFilterValue"].ToString());
    }

    protected string GetFlagReasons(List<Entry> EntryList, List<Guid> ListIDReg)
    {
        string emailBody = "";
        List<FlagReasons> flagReasonsList = FlagReasonsList.GetFlagReasonsList().Where(i => i.IsActive == true).ToList();
        foreach (Guid IDreg in ListIDReg)
        {
            string evetnYear = string.Empty;
            try
            {
                evetnYear = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value;
            }
            catch { }

            #region New Method
            Registration register = Registration.GetRegistration(IDreg);
            List<Entry> entrylist = EntryList.Where(x => x.RegistrationId == IDreg).OrderBy(y => y.FlagReason).ToList();

            #region Group FlagReason
            List<string> flagReasonEntryList = new List<string>();

            foreach (Effie2017.App.Entry flagReasonEntry in entrylist)
            {
                if (!flagReasonEntryList.Where(i => i == flagReasonEntry.FlagReason).Any())
                {
                    flagReasonEntryList.Add(flagReasonEntry.FlagReason);
                }
            }

            foreach (string flagReasonEntryItem in flagReasonEntryList)
            {
                List<Entry> entrylistForFlagReason = entrylist.Where(x => x.FlagReason == flagReasonEntryItem).OrderBy(y => y.Campaign).ToList();

                List<string> campaignEntryList = new List<string>();

                foreach (Effie2017.App.Entry campaignEntry in entrylistForFlagReason)
                {
                    if (!campaignEntryList.Where(i => i == campaignEntry.Campaign).Any())
                    {
                        campaignEntryList.Add(campaignEntry.Campaign);
                    }
                }

                foreach (string campaignEntryItem in campaignEntryList)
                {
                    List<Entry> entrylistForCampaign = entrylistForFlagReason.Where(x => x.Campaign == campaignEntryItem).OrderBy(y => y.Serial).ToList();

                    if (entrylistForCampaign.Count != 0)
                    {
                        string Head = "";
                        foreach (Effie2017.App.Entry entryItem in entrylistForCampaign)
                        {
                            Head += entryItem.Serial + ", ";
                        }
                        Head = Head.Substring(0, Head.LastIndexOf(", ")) + " [" + entrylistForCampaign[0].Campaign + "]<br>";

                        string DQList = "";
                        foreach (string flagReasonId in entrylistForCampaign[0].FlagReason.Split(new string[] { "||" }, StringSplitOptions.None))
                        {
                            if (flagReasonId != "")
                            {
                                foreach (FlagReasons flagReasonsItem in flagReasonsList)
                                {
                                    if (flagReasonsItem.Id.ToString().ToLower() == flagReasonId.ToLower().Substring(0, flagReasonId.LastIndexOf(':')))
                                    {
                                        if (flagReasonsItem.Bodyname.ToLower() != "others")
                                            DQList += "<li>" + flagReasonsItem.Description + "</li>";
                                        else
                                            DQList += "<li>" + flagReasonId.Substring(flagReasonId.LastIndexOf(':') + 1) + "</li>";
                                        break;
                                    }
                                }
                            }
                        }

                        emailBody += "<span><strong style=\"font-size:13px\">" + Head + "</strong></span>" + "<ol style=\"font-size:13px\">" + DQList + "</ol>";
                    }
                }
            }
            #endregion
            #endregion
        }
        return emailBody;
    }

    protected void SendTamplateMail(List<Entry> EntryList, List<Guid> ListIDReg, Guid templateId, string emailTamplate)
    {
        foreach (Guid IDreg in ListIDReg)
        {
            string evetnYear = string.Empty;
            try
            {
                evetnYear = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value;
            }
            catch { }
            
            Registration register = Registration.GetRegistration(IDreg);
            List<Entry> entrylist = EntryList.Where(x => x.RegistrationId == IDreg).OrderBy(y => y.FlagReason).ToList();
            
            List<Guid> GuidIDEntryList = new List<Guid>();
            foreach (Entry entry in entrylist)
            {
                GeneralFunction.SaveEmailSentLogReg(register, templateId, evetnYear, "EntryProcessing", entry.Id);
                GuidIDEntryList.Add(entry.Id);
                if (btnDQFlagNotification.Text != "Send Email")
                {
                    entry.NotificationSentDate = DateTime.Now.ToString();
                }
                else
                {
                    entry.LastSendSubmissionReminderEmailDateString = DateTime.Now.ToString();
                }
                

                entry.Save();
            }

            Email.SendReminderEmailTemplatelReg(register, emailTamplate, txtTemplateSubject.Text, GuidIDEntryList, "EntryProcessing");
            GuidIDEntryList.Clear();
            //Email.SendTemplateEmailEntryProcessing(register, templateId, emailBody);
        }
    }

    protected class TitleFlag
    {
        public string Title { get; set; }
        public List<string> Serial { get; set; }
        public List<string> Flag { get; set; }

    }

    protected void rtabEntry_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        Telerik.Web.UI.RadTab TabClicked = e.Tab;       
        string tabvalue = TabClicked.Value;

        radGridEntry.MasterTableView.SortExpressions.Clear();
         
        ViewState["TabFilterValue"] = tabvalue;
        ViewState["AdvanceSearch"] = "";
        BindGrid(false, string.Empty, GridSortOrder.None, true);
        SetColumn(tabvalue);
        lblError.Text = "";
        lblError2.Text = "";
    }

    protected void SetColumn(string tabvalue = DefaultPage)
    {
        btnDQFlagNotification.Text = "Send Notification";
        List<ColumnEnum> Columns = new List<ColumnEnum>();
        switch (tabvalue)
        {
            case StatusEntry.PendingVerification:
                btnAssignToST.Visible = false;
                btnDeadlineReminder.Visible = false;
                btnUpdateStatus.Visible = false;
                btnUpdateDQIssue.Visible = true;
                btnReport.Visible = true;
                btnDQFlagNotification.Visible = true;
                btnComplete.Visible = true;

                Columns.Add(ColumnEnum.DQFlag);
                Columns.Add(ColumnEnum.SelectColumn);
                Columns.Add(ColumnEnum.EntryID);
                //Columns.Add(ColumnEnum.DateCreated);
                Columns.Add(ColumnEnum.Campaign);
                Columns.Add(ColumnEnum.Client);
                Columns.Add(ColumnEnum.CategoryMarket);
                Columns.Add(ColumnEnum.Status);
                Columns.Add(ColumnEnum.SubmittedBy);
                Columns.Add(ColumnEnum.Firstname);
                Columns.Add(ColumnEnum.Lastname);
                Columns.Add(ColumnEnum.MaterialsSubmitted);
                Columns.Add(ColumnEnum.ProcessingStatus);
                Columns.Add(ColumnEnum.AdminidAssignedto);
                Columns.Add(ColumnEnum.ReopeningDate);
                Columns.Add(ColumnEnum.NotificationSentDate);
                ColumnHandler(Columns);
                Columns.Clear();
                break;
            case StatusEntry.Reopened:
                btnAssignToST.Visible = false;
                btnDeadlineReminder.Visible = false;
                btnUpdateStatus.Visible = false;
                btnUpdateDQIssue.Visible = true;
                btnReport.Visible = true;
                btnDQFlagNotification.Visible = true;
                btnComplete.Visible = true;

                Columns.Add(ColumnEnum.DQFlag);
                Columns.Add(ColumnEnum.SelectColumn);
                Columns.Add(ColumnEnum.EntryID);
                //Columns.Add(ColumnEnum.DateCreated);
                Columns.Add(ColumnEnum.Campaign);
                Columns.Add(ColumnEnum.Client);
                Columns.Add(ColumnEnum.CategoryMarket);
                Columns.Add(ColumnEnum.Status);
                Columns.Add(ColumnEnum.SubmittedBy);
                Columns.Add(ColumnEnum.Firstname);
                Columns.Add(ColumnEnum.Lastname);
                Columns.Add(ColumnEnum.MaterialsSubmitted);
                Columns.Add(ColumnEnum.ProcessingStatus);
                Columns.Add(ColumnEnum.AdminidAssignedto);
                Columns.Add(ColumnEnum.ReopeningDate);
                Columns.Add(ColumnEnum.NotificationSentDate);
                ColumnHandler(Columns);
                Columns.Clear();
                break;
            case StatusEntry.PendingReopen:
                btnDQFlagNotification.Text = "Send Email";
                btnDQFlagNotification.Visible = true;
                btnAssignToST.Visible = false;
                btnDeadlineReminder.Visible = false;
                btnUpdateStatus.Visible = false;
                btnUpdateDQIssue.Visible = false;
                btnReport.Visible = false;
                btnComplete.Visible = false;

                Columns.Add(ColumnEnum.DQFlag);
                Columns.Add(ColumnEnum.SelectColumn);
                Columns.Add(ColumnEnum.EntryID);
                //Columns.Add(ColumnEnum.DateCreated);
                Columns.Add(ColumnEnum.Campaign);
                Columns.Add(ColumnEnum.Client);
                Columns.Add(ColumnEnum.CategoryMarket);
                Columns.Add(ColumnEnum.Status);
                Columns.Add(ColumnEnum.SubmittedBy);
                Columns.Add(ColumnEnum.Firstname);
                Columns.Add(ColumnEnum.Lastname);
                Columns.Add(ColumnEnum.MaterialsSubmitted);
                Columns.Add(ColumnEnum.ProcessingStatus);
                Columns.Add(ColumnEnum.AdminidAssignedto);
                Columns.Add(ColumnEnum.ReopeningDate);
                Columns.Add(ColumnEnum.NotificationSentDate);
                ColumnHandler(Columns);
                Columns.Clear();
                break;
            case StatusEntry.Completed:
                btnAssignToST.Visible = false;
                btnDeadlineReminder.Visible = false;
                btnUpdateStatus.Visible = false;
                btnUpdateDQIssue.Visible = false;
                btnReport.Visible = true;
                btnDQFlagNotification.Visible = false;
                btnComplete.Visible = false;

                Columns.Add(ColumnEnum.DQFlag);
                Columns.Add(ColumnEnum.SelectColumn);
                Columns.Add(ColumnEnum.EntryID);
                //Columns.Add(ColumnEnum.DateCreated);
                Columns.Add(ColumnEnum.Campaign);
                Columns.Add(ColumnEnum.Client);
                Columns.Add(ColumnEnum.CategoryMarket);
                Columns.Add(ColumnEnum.Status);
                Columns.Add(ColumnEnum.SubmittedBy);
                Columns.Add(ColumnEnum.Firstname);
                Columns.Add(ColumnEnum.Lastname);
                Columns.Add(ColumnEnum.MaterialsSubmitted);
                Columns.Add(ColumnEnum.ProcessingStatus);
                Columns.Add(ColumnEnum.AdminidAssignedto);
                Columns.Add(ColumnEnum.ReopeningDate);
                Columns.Add(ColumnEnum.NotificationSentDate);
                ColumnHandler(Columns);
                Columns.Clear();

                break;
            case "ALLP":
                btnAssignToST.Visible = true;
                btnDeadlineReminder.Visible = false;
                btnUpdateStatus.Visible = false;
                btnUpdateDQIssue.Visible = false;
                btnReport.Visible = true;
                btnDQFlagNotification.Visible = true;
                btnComplete.Visible = true;

                Columns.Add(ColumnEnum.DQFlag);
                Columns.Add(ColumnEnum.SelectColumn);
                Columns.Add(ColumnEnum.EntryID);
                //Columns.Add(ColumnEnum.DateCreated);
                Columns.Add(ColumnEnum.Campaign);
                Columns.Add(ColumnEnum.Client);
                Columns.Add(ColumnEnum.CategoryMarket);
                Columns.Add(ColumnEnum.Status);
                Columns.Add(ColumnEnum.SubmittedBy);
                Columns.Add(ColumnEnum.Firstname);
                Columns.Add(ColumnEnum.Lastname);
                Columns.Add(ColumnEnum.MaterialsSubmitted);
                Columns.Add(ColumnEnum.ProcessingStatus);
                Columns.Add(ColumnEnum.AdminidAssignedto);
                Columns.Add(ColumnEnum.ReopeningDate);
                Columns.Add(ColumnEnum.NotificationSentDate);
                ColumnHandler(Columns);
                Columns.Clear();
                break;
            case "ALL":
                btnAssignToST.Visible = true;
                btnDeadlineReminder.Visible = false;
                btnUpdateStatus.Visible = false;
                btnUpdateDQIssue.Visible = false;
                btnReport.Visible = false;
                btnDQFlagNotification.Visible = true;
                btnComplete.Visible = false;

                //Columns.Add(ColumnEnum.DQFlag);
                Columns.Add(ColumnEnum.SelectColumn);
                Columns.Add(ColumnEnum.EntryID);
                //Columns.Add(ColumnEnum.DateCreated);
                Columns.Add(ColumnEnum.Campaign);
                Columns.Add(ColumnEnum.Client);
                Columns.Add(ColumnEnum.CategoryMarket);
                Columns.Add(ColumnEnum.Status);
                Columns.Add(ColumnEnum.SubmittedBy);
                Columns.Add(ColumnEnum.Firstname);
                Columns.Add(ColumnEnum.Lastname);
                Columns.Add(ColumnEnum.MaterialsSubmitted);
                //Columns.Add(ColumnEnum.ProcessingStatus);
                Columns.Add(ColumnEnum.AdminidAssignedto);
                ColumnHandler(Columns);
                Columns.Clear();
                break;
        }

        //if (admin.Access != "SA")
        {
            btnAssignToST.Visible = false;
        }

        if (!Security.IsRoleSuperAdmin() && !Security.IsRoleSuperAdminFinance())
        {
            btnReport.Visible = false;
        }
        
        if(Security.IsReadOnlyAdmin() || Security.IsRoleST())
        {
            btnAssignToST.Visible = false;
            btnDeadlineReminder.Visible = false;
            btnUpdateStatus.Visible = false;
            btnUpdateDQIssue.Visible = false;
            btnReport.Visible = false;
            btnDQFlagNotification.Visible = false;
            btnComplete.Visible = false;
            radGridEntry.MasterTableView.GetColumn("SelectColumn").Visible = false;
        }
        
        initddlupdateProcessingStatus(tabvalue);
    }

    protected void ColumnHandler(List<ColumnEnum> Columns, bool Visible = true) {
        List<ColumnEnum> ColumnList = new List<ColumnEnum>();
        ColumnList.Add(ColumnEnum.SelectColumn);
        ColumnList.Add(ColumnEnum.EntryID);
        ColumnList.Add(ColumnEnum.No);
        ColumnList.Add(ColumnEnum.DateCreated);
        ColumnList.Add(ColumnEnum.Campaign);
        ColumnList.Add(ColumnEnum.Client);
        ColumnList.Add(ColumnEnum.CategoryMarket);
        ColumnList.Add(ColumnEnum.Status);
        ColumnList.Add(ColumnEnum.SubmittedBy);
        ColumnList.Add(ColumnEnum.Firstname);
        ColumnList.Add(ColumnEnum.Lastname);
        ColumnList.Add(ColumnEnum.MaterialsSubmitted);
        ColumnList.Add(ColumnEnum.ProcessingStatus);
        ColumnList.Add(ColumnEnum.ReopeningDate);
        ColumnList.Add(ColumnEnum.AdminidAssignedto);
        ColumnList.Add(ColumnEnum.ReopeningDeadline);
        ColumnList.Add(ColumnEnum.ReopeningFee);
        ColumnList.Add(ColumnEnum.DQFlag);
        ColumnList.Add(ColumnEnum.NotificationSentDate);
        ColumnList.Add(ColumnEnum.DateVerified);

        //HIDE ALL COLUMN
        foreach (ColumnEnum Column in ColumnList)
        {
            radGridEntry.MasterTableView.GetColumn(Column.ToString()).Visible = !Visible;
        }

        foreach (ColumnEnum Column in Columns)
        {
            radGridEntry.MasterTableView.GetColumn(Column.ToString()).Visible = Visible;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rtabEntry.Visible = false;
        lblError.Text = "";
        lblError2.Text = "";
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
        BindGrid(false, string.Empty, GridSortOrder.None, true);

    }


    public void PopulateTemplatePanel()
    {
        lblTempError.Text = string.Empty;
        lblError.Text = string.Empty;

        List<EmailTemplate> defaultEmailTempalteList = EmailTemplateList.GetEmailTemplateList(Guid.Empty).ToList();
        if (btnDQFlagNotification.Text == "Send Email")
            defaultEmailTempalteList = defaultEmailTempalteList.Where(x => x.EmailType == EmailType.Entry.ToString() && x.IsActive && !x.IsDelete).ToList();
        else
            defaultEmailTempalteList = defaultEmailTempalteList.Where(x => x.EmailType == EmailType.DQ.ToString() && x.IsActive && !x.IsDelete).ToList();

        if (defaultEmailTempalteList.Count == 0)
            lblError.Text = "No Email Template found.<br/>";

        phDQFlagNotification.Visible = (defaultEmailTempalteList.Count != 0);
        if (defaultEmailTempalteList.Count != 0)
        {
            ddlTemplateList.DataSource = defaultEmailTempalteList;
            ddlTemplateList.DataTextField = "Title";
            ddlTemplateList.DataValueField = "Id";
            ddlTemplateList.DataBind();

            ddlTemplateList.Items.Insert(0, new ListItem("Please Select", Guid.Empty.ToString()));

            hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());
            
        }
    }

    protected void ddlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString()))
        {
            hlkPreview.CssClass = "fancybox fancybox.iframe tblLinkRed";
            hlkPreview.NavigateUrl = "./EmailPreview.aspx?etmId=" + ddlTemplateList.SelectedValue.ToString();

            /////////////////////////////////////////////////////////////////////
            CurEmailTemplate = EmailTemplate.GetEmailTemplate(new Guid(ddlTemplateList.SelectedValue.ToString()));
            
            lblError.Text = string.Empty;
            List<Guid> ListIDReg = new List<Guid>();
            List<Entry> EntryList = new List<Entry>();
            foreach (GridDataItem item in radGridEntry.Items)
            {
                CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
                HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

                if (chkbox.Checked)
                {

                    Entry entry = Entry.GetEntry(new Guid(item["Id"].Text));
                    if (!ListIDReg.Where(ID => ID == entry.RegistrationId).Any())
                    {
                        ListIDReg.Add(entry.RegistrationId);
                    }

                    EntryList.Add(entry);
                }
            }

            txtTemplateName.Text = CurEmailTemplate.Title;
            txtTemplateSubject.Text = CurEmailTemplate.Subject;

            string DQFalgReason = GetFlagReasons(EntryList, ListIDReg);
            if (btnDQFlagNotification.Text != "Send Email")
                rEditorBody.Content = GeneralFunction.CheckPlaceHolders(CurEmailTemplate.Body, true).Replace("#FLAGREASON#", DQFalgReason);
            else
                rEditorBody.Content = GeneralFunction.CheckPlaceHolders(CurEmailTemplate.Body, true);
            
            //rEditorBody.Content = GeneralFunction.CheckPlaceHolders(CurEmailTemplate.Body, true);
            divDQFlagNotification.Attributes.Add("class", "ModalPopUpBig");
            divEditTamplate.Visible = true;
            /////////////////////////////////////////////////////////////////////

        }

        hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        PHUpdateDQ.Visible = false;
        phDQFlagNotification.Visible = false;
        phUpdateStatus.Visible = false;
        phAssignTo.Visible = false;
        
        divDQFlagNotification.Attributes.Add("class", "ModalPopUpSmall");
        divEditTamplate.Visible = false;
    }
    
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlProcessingStatus.SelectedValue = "";
        ddlCategory.SelectedValue = "";
        ddlDQFlag.SelectedValue = "";
        ddlAssignedTo.SelectedValue = "";
        ddlMarket.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        rtabEntry.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = DefaultPage;
        rtabEntry.SelectedIndex = 1;
        BindGrid(false, string.Empty, GridSortOrder.None, true);
    }
    #endregion

    #region Helper
    private ListItem SeparatorItem()
    {
        ListItem separator = new ListItem("-------------------------------", "");
        separator.Attributes.Add("disabled", "disabled");

        return separator;
    }
    #endregion

    public void btnSendMail_Click(object sender, EventArgs e)
    {
        if (phDQFlagNotification.Visible) 
        {
            GenerateEmails(new Guid(ddlTemplateList.SelectedValue));
        }
        phDQFlagNotification.Visible = false;
    }

    protected void btnAssignToST_Click(object sender, EventArgs e)
    {
        if (isAnySelected())
        {
            phAssignTo.Visible = true;
            lblError.Text = "";
            lblError2.Text = "";
        }
        else
        {
            lblError.Text = "Please select at least one Entry to Update Status.<br/>";
            lblError2.Text = lblError.Text;
        }
    }

    protected void btnSubmitAssignTo_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
            Guid EntryId = new Guid(hdfId.Value);
            if (chkbox.Checked)
            {
                Entry entry = Entry.GetEntry(EntryId);
                Registration reg = Registration.GetRegistration(entry.RegistrationId);
                entry.AdminidAssignedto = new Guid(ddlAssignTo.SelectedValue);
                entry.Save();
                chkbox.Checked = false;
                phAssignTo.Visible = false;
            }
        }
        PopulateForm(ViewState["TabFilterValue"].ToString());
    }
    
    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        if (isAnySelected())
        {
            phUpdateStatus.Visible = true; 
            lblError.Text = "";
            lblError2.Text = "";
        }
        else
        {
            lblError.Text = "Please select at least one Entry to Update Status.<br/>";
            lblError2.Text = lblError.Text;
        }
    }

    protected bool isAnySelected() {
        bool isAny = false;
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
            if (chkbox.Checked)
            {
                isAny = true;
            }
        }
        
        return isAny;
    }

    protected void btnUpdateDQIssue_Click(object sender, EventArgs e)
    {
        bool issame = true;
        bool isStatusNotComplete = false;
        Entry OldEntry = null, NewEntry = null;

        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
            Guid EntryId = new Guid(hdfId.Value);
            if (chkbox.Checked && issame)
            {
                if(OldEntry == null)
                    OldEntry = Entry.GetEntry(EntryId);

                if (NewEntry == null)
                    NewEntry = Entry.GetEntry(EntryId);

                if (OldEntry.FlagReason == NewEntry.FlagReason)
                {
                    OldEntry = NewEntry;
                    NewEntry = null;
                }
                else
                {
                    issame = false;
                }

                if (OldEntry.Status != StatusEntry.Completed && !isStatusNotComplete)
                {
                    isStatusNotComplete = true;
                }

            }
        }

        if (isAnySelected() && issame && !isStatusNotComplete)
        {
            InitrptFlagReason(OldEntry, true);
            lblError.Text = "";
            lblError2.Text = "";
        }
        else if (!issame)
        {
            lblError.Text = "DQ Reasons are not the same.<br/>";
            lblError2.Text = lblError.Text;
        }
        else if (isStatusNotComplete)
        {
            lblError.Text = "Please select Entry(s) status Completed only.<br/>";
            lblError2.Text = lblError.Text;
        }
        else
        {
            lblError.Text = "Please select at least one Entry to Update DQ.<br/>";
            lblError2.Text = lblError.Text;
        }
    }
    
    protected void btnSubmitPHUpdateDQ_Click(object sender, EventArgs e)
    {
        if (EntrySelected != null)
        {
            UpdateDQ(EntrySelected);
            EntrySelected = null;
            PHUpdateDQ.Visible = false;
        }
        else
        {
            foreach (GridDataItem item in radGridEntry.Items)
            {
                CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
                HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
                Guid EntryId = new Guid(hdfId.Value);
                if (chkbox.Checked)
                {
                    Entry entry = Entry.GetEntry(EntryId);
                    UpdateDQ(entry);
                    chkbox.Checked = false;
                    PHUpdateDQ.Visible = false;
                }
            }
        }

        PopulateForm(ViewState["TabFilterValue"].ToString());

        radGridEntry.Rebind();
        rptFlagReason.DataSource = Data.GetFlagReasons();
        rptFlagReason.DataBind();
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError2.Text = "";

        if (isAnySelected())
        {
            foreach (GridDataItem item in radGridEntry.Items)
            {
                CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
                HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
                Guid EntryId = new Guid(hdfId.Value);
                if (chkbox.Checked)
                {
                    Entry entry = Entry.GetEntry(EntryId);
                    if (entry.Status != StatusEntry.Completed)
                    {
                        lblError.Text = "One or more entries are not Closed.<br/>";
                        break;
                    }
                }
            }

            if (lblError.Text == "")
            {
                foreach (GridDataItem item in radGridEntry.Items)
                {
                    CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
                    HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
                    Guid EntryId = new Guid(hdfId.Value);
                    if (chkbox.Checked)
                    {
                        Entry entry = Entry.GetEntry(EntryId);
                        UpdateStatusProcessing(entry, StatusEntry.Completed);
                    }
                }
            }
        }
        else
        {
            lblError.Text = "Please select at least one Entry to Update Status.<br/>";
            lblError2.Text = lblError.Text;
        }
        
        PopulateForm(ViewState["TabFilterValue"].ToString());
    }
    

    protected void UpdateDQ(Entry entry)
    {
        string FlagReasons = "";
        foreach (RepeaterItem rpt in rptFlagReason.Items)
        {
            HiddenField hdID = (HiddenField)rpt.FindControl("hdItemId");
            Literal title = (Literal)rpt.FindControl("Title");
            HiddenField hdAttrType = (HiddenField)rpt.FindControl("hdAttrType");
            CheckBox isHasOther = (CheckBox)rpt.FindControl("isHasOther");
            CheckBox cbItem = (CheckBox)rpt.FindControl("cbItem");
            TextBox txtOtherDQ = (TextBox)rpt.FindControl("txtOtherDQ");

            if (cbItem.Checked)
            {
                if (isHasOther.Checked)
                {
                    FlagReasons += hdID.Value + ":" + txtOtherDQ .Text + Delimiter[3];
                }
                else
                {
                    FlagReasons += hdID.Value + ":" + Delimiter[3];
                }
            }
        }

        entry.DateCreatedString = DateTime.Now.ToString();
        entry.FlagReason = FlagReasons;
        if (string.IsNullOrEmpty(FlagReasons))
            entry.DQFlag = "None";
        else
            entry.DQFlag = "DQ";
        
        entry.FlagDQDescription = txtFlagDescription.Text;
        entry.Save();
        
    }

    protected void btnphUpdateStatus_Click(object sender, EventArgs e)
    {
        bool isNeedBind = false;
        if (EntrySelected != null)
        {
            UpdateStatusProcessing(EntrySelected, ddlupdateProcessingStatus.SelectedValue);
            phUpdateStatus.Visible = false;
            isNeedBind = true;
            EntrySelected = null;
        }
        else
        {
            foreach (GridDataItem item in radGridEntry.Items)
            {
                CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
                HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
                Guid EntryId = new Guid(hdfId.Value);
                if (chkbox.Checked)
                {
                    Entry entry = Entry.GetEntry(EntryId);
                    Registration reg = Registration.GetRegistration(entry.RegistrationId);
                    UpdateStatusProcessing(entry, ddlupdateProcessingStatus.SelectedValue);
                    chkbox.Checked = false;
                    phUpdateStatus.Visible = false;
                    isNeedBind = true;
                }
            }
        }
        
        if (isNeedBind)
        {
            PopulateForm(ViewState["TabFilterValue"].ToString());
        }
    }

    protected void UpdateStatusProcessing(Entry entry, string status)
    {
        if (status == StatusEntry.Reopened)
        {
            //entry.ReopeningDate = DateTime.Now.ToString();
            entry.ReopenedBy = admin.Id.ToString();
            entry.Status = StatusEntry.UploadCompleted;
        }
        
        //if (status == StatusEntry.PendingVerification && entry.Status == StatusEntry.Completed)
        //{
        //    entry.Status = StatusEntry.UploadCompleted;
        //}

        if (entry.Status != StatusEntry.Completed && status == StatusEntry.Completed)
            lblError.Text = "Please make sure your Entry is Completed.<br/>";
        else
            entry.ProcessingStatus = status;


        entry.Save();
    }

    protected bool isMoreThanOneRegister()
    {
        List<Guid> ListIDReg = new List<Guid>();
        foreach (GridDataItem item in radGridEntry.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkboxSelect");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

            if (chkbox.Checked)
            {
                Entry entry = Entry.GetEntry(new Guid(item["Id"].Text));
                if (!ListIDReg.Where(ID => ID == entry.RegistrationId).Any())
                {
                    ListIDReg.Add(entry.RegistrationId);
                }
            }
        }
        
        return (ListIDReg.Count() > 1);
    }

    protected void btnDQFlagNotification_Click(object sender, EventArgs e)
    {
        if (isMoreThanOneRegister())
        {
            lblError.Text = "You can only select the same user to send notification.<br/>";
        }
        else if (isAnySelected())
        {
            PopulateTemplatePanel();
            lblError.Text = "";
            lblError2.Text = "";
        }
        else
        {
            lblError.Text = "Please select at least one Entry to Update DQ Flag Notification.<br/>";
            lblError2.Text = lblError.Text;
        }
    }
   
    protected bool isFileExist(string path)
    {
        if(File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + path))
            return true;

        return false;
    }

    protected enum ColumnEnum
    {
        SelectColumn,
        EntryID,
        No,
        DateCreated,
        Campaign,
        Client,
        CategoryMarket,
        Status,
        SubmittedBy,
        Firstname,
        Lastname,
        MaterialsSubmitted,
        ProcessingStatus,
        ReopeningDate,
        AdminidAssignedto,
        ReopeningDeadline,
        ReopeningFee,
        DQFlag,
        NotificationSentDate,
        DateVerified
    };
}
