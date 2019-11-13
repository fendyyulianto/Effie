using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Effie2017.App;
using Telerik.Web.UI;
using System.Data; 
using ClosedXML.Excel;
using System.IO;

public partial class Admin_AdhocInvoice : PageSecurity_Admin
{   
    Registration reg;
    string regId = string.Empty;
    Entry entrySelected = null;
    Guid regGuidId = Guid.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        regId = Request.QueryString["regId"];
        if (regId != null && !String.IsNullOrEmpty(regId))
        {
            regGuidId = new Guid(GeneralFunction.StringDecryption(regId));
        }

        if (!string.IsNullOrEmpty(Request.QueryString["EntryId"]))
        {
            try
            {
                entrySelected = Entry.GetEntry(new Guid(GeneralFunction.StringDecryption(Request.QueryString["EntryId"])));
            }
            catch
            {
                entrySelected = null;
            } 
        }

        reg = Registration.GetRegistration(regGuidId);

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm(reg);
        }
    }
    private void PopulateForm(Registration reg)
    {
        if (reg != null)
        {
            BindGrid(reg, true);
        }
    }
    private void LoadForm()
    {
        foreach(GridDataItem item in radGridEntry.Items)
        {
            DropDownList ddlInvoiceType = (DropDownList)item.FindControl("ddlInvoiceType");

            ddlInvoiceType.Items.Clear();

            ddlInvoiceType.Items.Add(new ListItem(GeneralFunction.GetInvoiceType(AdhocInvoiceType.ChangeReq), AdhocInvoiceType.ChangeReq));
            ddlInvoiceType.Items.Add(new ListItem(GeneralFunction.GetInvoiceType(AdhocInvoiceType.ExtDeadLine), AdhocInvoiceType.ExtDeadLine));
            ddlInvoiceType.Items.Add(new ListItem(GeneralFunction.GetInvoiceType(AdhocInvoiceType.ReOpen), AdhocInvoiceType.ReOpen));
            ddlInvoiceType.Items.Add(new ListItem(GeneralFunction.GetInvoiceType(AdhocInvoiceType.Custom), AdhocInvoiceType.Custom));

            ddlInvoiceType.Items.Insert(0, new ListItem("-", string.Empty));
        }

        radGridEntry.MasterTableView.GetColumn("ProcessingStatus").Visible = true;

        //Administrator admin = Security.GetAdminLoginSession();
        //if (admin.Access == "SA" && entrySelected != null)
        //{
        //    radGridEntry.MasterTableView.GetColumn("ProcessingStatus").Visible = true;
        //}
        //else
        //{
        //    radGridEntry.MasterTableView.GetColumn("ProcessingStatus").Visible = false;
        //}

    }

    private void BindGrid(Registration reg, bool needRebind)
    {
        EntryList list = GeneralFunction.GetAllEntryCache(false);

        // filter off the draft and ready
        List<Entry> slist = new List<Entry>();
        foreach (Entry entry in list)
        {
            if (reg.Id == entry.RegistrationId && (entry.Status != StatusEntry.Draft && entry.Status != StatusEntry.Ready))
                slist.Add(entry);
        }

        var sort = from entry in slist orderby entry.DateCreated descending select entry;
        if (entrySelected != null)
        {
            slist = slist.Where(x => !string.IsNullOrEmpty(x.ProcessingStatus)).ToList();
            //slist = slist.Where(x => ((x.Status == StatusEntry.Completed || x.Status == StatusEntry.UploadPending || x.Status == StatusEntry.UploadCompleted) ||
            //           (x.ProcessingStatus == StatusEntry.PendingVerification))).ToList();
        }

        radGridEntry.DataSource = slist;
        if (needRebind) radGridEntry.Rebind();
    }

    private bool ValidateForm()
    {
        lblError.Text = string.Empty;

        GeneralFunction.RemoveHighlightControls(this);

        bool isAnySelected = false;
        int counter = 1;
        string OldddlInvoiceType = "";
        bool IsSame = true;
        foreach (GridDataItem item in radGridEntry.Items)
        {
            DropDownList ddlInvoiceType = (DropDownList)item.FindControl("ddlInvoiceType");
            TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
            TextBox txtInvoiceCustom = (TextBox)item.FindControl("txtInvoiceCustom");

            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");

            if (chkbox.Checked)
            {
                string error = string.Empty;

                if (string.IsNullOrEmpty(OldddlInvoiceType))
                {
                    OldddlInvoiceType = ddlInvoiceType.SelectedValue;
                }
                else if (ddlInvoiceType.SelectedValue != OldddlInvoiceType)
                {
                    IsSame = false;
                }
                error = GeneralFunction.ValidateDropDownList("Invoice Type", ddlInvoiceType, true, string.Empty);
                error += GeneralFunction.ValidateTextBox("Amount", txtAmount, true, "decimal");
                error += GeneralFunction.ValidateTextBox("Description", txtAmount, ddlInvoiceType.SelectedValue.Equals(AdhocInvoiceType.Custom), "string");

                if (!String.IsNullOrEmpty(error))
                    lblError.Text += counter + "<br/>" + error;

                isAnySelected = true;
               
            }
            counter++;
        }

        if (!isAnySelected)
            lblError.Text = "Please choose atleast one entry.<br/>";

        if (!IsSame)
            lblError.Text = "Cannot mix between reopen and other case invoice<br/>";

        return string.IsNullOrEmpty(lblError.Text);

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

            // Edit button
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            Security.SecureControlByHiding(lnkBtn);

            //((GridDataItem)e.Item)["Id"].Text = entry.Id.ToString();

            HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");
            hdfId.Value = entry.Id.ToString();

            // market
            if (entry.CategoryMarket == "SM")
                ((GridDataItem)e.Item)["CategoryMarket"].Text = "Single Market";
            else if (entry.CategoryMarket == "MM")
                ((GridDataItem)e.Item)["CategoryMarket"].Text = "Multi Market";
            ((GridDataItem)e.Item)["CategoryMarket"].Text = entry.CategoryPSDetail;


            ((GridDataItem)e.Item)["Status"].Text = GeneralFunction.GetEntryStatusForAdmin(entry.Status);

            string ProcessingStatus = entry.ProcessingStatus;
            if (entry.ProcessingStatus == StatusEntry.Completed)
                ProcessingStatus = "Completed";
            else
                ProcessingStatus = GeneralFunction.GetEntryStatusForAdmin(entry.ProcessingStatus);

            ((GridDataItem)e.Item)["ProcessingStatus"].Text = "<span style='font-weight: bold;'>" + ProcessingStatus + "</span>";


            // Status
            if (entry.Status == StatusEntry.Completed)
                ((GridDataItem)e.Item)["Status"].Text = "<span style=\"font-weight:bold\">" + GeneralFunction.GetEntryStatus(entry.Status) + "</span>";
            if (entry.WithdrawnStatus != "")
                ((GridDataItem)e.Item)["Status"].Text += "<br/><span style=\"color:Red;\">" + GeneralFunction.GetWithdrawnStatus(entry.WithdrawnStatus) + "</span>";
            
            Administrator administrator = AdministratorList.GetAdministratorList().Where(x => x.Id == entry.AdminidAssignedto).FirstOrDefault();
            try
            {
                ((GridDataItem)e.Item)["AdminidAssignedto"].Text = administrator.LoginId;;
            }
            catch
            {
                ((GridDataItem)e.Item)["AdminidAssignedto"].Text = "";
            }


            // submitted by
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnBuSubmittedBy");
            Registration reg = GeneralFunction.GetRegistrationFromEntry(entry);
            lnkBtn.Text = "-";
            if (reg != null)
            {
                lnkBtn.Text = GeneralFunction.GetRegistrationFromEntry(entry).Company;
                lnkBtn.CommandArgument = reg.Id.ToString();

                // Changes by Shaik for adding new columns on 19 Oct 2015
                ((GridDataItem)e.Item)["Firstname"].Text = reg.Firstname;
                ((GridDataItem)e.Item)["Lastname"].Text = reg.Lastname;
            }


            // submitted details
            lbl = (Label)e.Item.FindControl("lblSubmittedDetails");
            lbl.Text = reg.Firstname + " " + reg.Lastname + "<br/>" + reg.Job + "<br/>" + GeneralFunction.ShowFriendlyContact(reg.Contact) + "<br/>" + reg.Email;
            
            // chkboxes
            CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            Security.SecureControlByHiding(chkbox);

            //Edit By Rico, Nov 28 2013, Extra Collumn for Countr and Date Reminder
            ((GridDataItem)e.Item)["Country"].Text = reg.Country;


            CheckBox checkbox = (CheckBox)e.Item.FindControl("chkbox");
            if (checkbox != null) checkbox.Visible = true;
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
        
    }
    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        regId = Request.QueryString["regId"];

        if (regId != null && !String.IsNullOrEmpty(regId))
        {
            regGuidId = new Guid(GeneralFunction.StringDecryption(regId));
        }

        reg = Registration.GetRegistration(regGuidId);

        BindGrid(reg, false);

    }

    protected void ddlInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlInvoiceType = (DropDownList)sender;
        GridDataItem item = (GridDataItem)ddlInvoiceType.NamingContainer;


        TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
        TextBox txtInvoiceCustom = (TextBox)item.FindControl("txtInvoiceCustom");

        if (!String.IsNullOrEmpty(ddlInvoiceType.SelectedValue))
        {
            if (ddlInvoiceType.SelectedValue.Equals(AdhocInvoiceType.ReOpen))
            {
                txtAmount.Text = "200";
            }
            else if (ddlInvoiceType.SelectedValue.Equals(AdhocInvoiceType.ChangeReq))
            {
                txtAmount.Text = "200";
            }
            else if (ddlInvoiceType.SelectedValue.Equals(AdhocInvoiceType.ExtDeadLine))
            {
                txtAmount.Text = "";
            }
            else if (ddlInvoiceType.SelectedValue.Equals(AdhocInvoiceType.Custom))
            {
                txtAmount.Text = "";               
            }

            txtAmount.Visible = true;
            txtInvoiceCustom.Visible = ddlInvoiceType.SelectedValue.Equals(AdhocInvoiceType.Custom);
        }
        else
        {
            txtInvoiceCustom.Visible = txtAmount.Visible = false;
            txtInvoiceCustom.Text = txtAmount.Text = string.Empty;
        }
    }

    protected void chkbox_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkbox = (CheckBox)sender;
        GridDataItem item = (GridDataItem)chkbox.NamingContainer;

        DropDownList ddlInvoiceType = (DropDownList)item.FindControl("ddlInvoiceType");
        TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
        TextBox txtInvoiceCustom = (TextBox)item.FindControl("txtInvoiceCustom");

        ddlInvoiceType.Enabled = txtAmount.Enabled = txtInvoiceCustom.Enabled = chkbox.Checked;
        

        if (!chkbox.Checked)
        {
            ddlInvoiceType.SelectedIndex = -1;
            txtAmount.Text = txtInvoiceCustom.Text = string.Empty;
            txtAmount.Visible = txtInvoiceCustom.Visible = false;
        }
    }

    protected void btnGenerateInvoice_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            regId = Request.QueryString["regId"];

            if (regId != null && !String.IsNullOrEmpty(regId))
            {
                regGuidId = new Guid(GeneralFunction.StringDecryption(regId));
            }

            reg = Registration.GetRegistration(regGuidId);

            Guid payGroupId = Guid.NewGuid();

            decimal totalAmount = 0;

            AdhocInvoice adInvoice = AdhocInvoice.NewAdhocInvoice();
            adInvoice.PayGroupId = payGroupId;
            adInvoice.RegistrationId = reg.Id;

            foreach (GridDataItem item in radGridEntry.Items)
            {
                DropDownList ddlInvoiceType = (DropDownList)item.FindControl("ddlInvoiceType");
                TextBox txtAmount = (TextBox)item.FindControl("txtAmount");
                TextBox txtInvoiceCustom = (TextBox)item.FindControl("txtInvoiceCustom");
                HiddenField hdfId = (HiddenField)item.FindControl("hdfId");

                CheckBox chkbox = (CheckBox)item.FindControl("chkbox");

                if (chkbox.Checked)
                {
                    AdhocInvoiceItem adInvoiceItem = AdhocInvoiceItem.NewAdhocInvoiceItem();

                    adInvoiceItem.Amount = Decimal.Parse(txtAmount.Text.Trim());

                    adInvoiceItem.AdhocInvoiceId = adInvoice.Id;
                    adInvoiceItem.EntryId = new Guid(hdfId.Value.ToString());
                    adInvoiceItem.InvoiceType = ddlInvoiceType.SelectedValue;
                    adInvoiceItem.InvoiceTypeOthers = txtInvoiceCustom.Text.Trim();

                    adInvoiceItem.PayGroupId = payGroupId;

                    adInvoiceItem.DateCreatedString = DateTime.Now.ToString();
                    adInvoiceItem.DateModifiedString = DateTime.Now.ToString();

                    if (adInvoiceItem.IsValid)
                        adInvoiceItem.Save();
                }

            }

            adInvoice.DateCreatedString = DateTime.Now.ToString();
            adInvoice.DateModifiedString = DateTime.Now.ToString();

            if (adInvoice.IsValid)
                adInvoice.Save();

            string InvoiceUrl = "AdhocInvoiceSummary.aspx?adm=e&pgId=" + GeneralFunction.StringEncryption(payGroupId.ToString());
            if (entrySelected != null)
            {
                InvoiceUrl = "AdhocInvoiceSummary.aspx?adm=e&pgId=" + GeneralFunction.StringEncryption(payGroupId.ToString()) + "&EntryId=" + GeneralFunction.StringEncryption(entrySelected.Id.ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
                {
                    InvoiceUrl += "&Page=Management";
                }
            }

            GeneralFunction.SetRedirect(string.Empty);
            Response.Redirect(InvoiceUrl);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (entrySelected != null)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Page"]))
            {
                Response.Redirect("EntryProcessing.aspx?Page=Management");
            }
            else
            {
                Response.Redirect("EntryProcessing.aspx");
            }
        }
        else
        {
            Response.Redirect("../Admin/EntryList.aspx");
        }
        
    }

    #endregion

    #region Helper



    #endregion


    
}