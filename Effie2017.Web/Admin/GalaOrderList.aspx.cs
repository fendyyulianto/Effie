using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Effie2017.App;
using Telerik.Web.UI;
using System.IO;

public partial class Admin_GalaOrderList : PageSecurity_Admin
{
    int counter;
    string round;

    protected void Page_Load(object sender, EventArgs e)
    {
        up1.Save_Clicked += new EventHandler(PaySave_Clicked);
        up1.Cancel_Clicked += new EventHandler(PayCancel_Clicked);


        round = Request.QueryString["r"];

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }
    private void PopulateForm()
    {
        // Refresh the cache
        //GeneralFunction.ResetReportDataCache();
        //GeneralFunction.GetAllJuryCache(true);
        //GeneralFunction.GetAllJuryPanelCategoryCache(true);

        ViewState["TabFilterValue"] = "";

        // preload saved filters
        //if (GeneralFunction.GetFilterPageId() == "JuryList")
        //{
        //    txtSearch.Text = GeneralFunction.GetFilterF1();
        //    ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
        //    ddlNetwork.SelectedValue = GeneralFunction.GetFilterF3();
        //    ddlJuryPanel1.SelectedValue = GeneralFunction.GetFilterF4();
        //    ddlHoldingCompany.SelectedValue = GeneralFunction.GetFilterF5();
        //    ddlJuryPanel2.SelectedValue = GeneralFunction.GetFilterF6();
        //    ddlCountry.SelectedValue = GeneralFunction.GetFilterF7();
        //    ddlAssignedRound.SelectedValue = GeneralFunction.GetFilterF8();
        //    ViewState["AdvanceSearch"] = "1";
        //}
        BindGrid(true);
    }
    private void LoadForm()
    {
        // payment method
        ddlPaymentMethod.Items.Add(new ListItem(GeneralFunction.GetPaymentType(PaymentType.PayPal), PaymentType.PayPal));
        ddlPaymentMethod.Items.Add(new ListItem(GeneralFunction.GetPaymentType(PaymentType.BankTransfer), PaymentType.BankTransfer));
        ddlPaymentMethod.Items.Add(new ListItem(GeneralFunction.GetPaymentType(PaymentType.Cheque), PaymentType.Cheque));
        ddlPaymentMethod.Items.Insert(0, new ListItem("All", ""));

        // payment status
        ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentGalaStatus(StatuspaymentGalaOrder.Paid), StatuspaymentGalaOrder.Paid));
        ddlPaymentStatus.Items.Add(new ListItem(GeneralFunction.GetPaymentGalaStatus(StatuspaymentGalaOrder.NotPaid), StatuspaymentGalaOrder.NotPaid));
        ddlPaymentStatus.Items.Insert(0, new ListItem("All", ""));

        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryListFromJury(false,round);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));


        Security.SecureControlByHiding(btnExport, "EXPORT");
    }
    private void BindGrid(bool needRebind)
    {
        // Any PreCache - put here

        // Data
        GalaOrderList list = GalaOrderList.GetGalaOrderList();

        // filter off the draft and ready
        //List<Entry> slist = new List<Entry>();
        //foreach (Entry entry in list)
        //{
        //    if (entry.Status != StatusEntry.Draft && entry.Status != StatusEntry.Ready)
        //        slist.Add(entry);
        //}

        // filter
        string status = (string)ViewState["TabFilterValue"];
        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<GalaOrder> flist = new List<GalaOrder>();

        if (advanceSearch == "1")
        {
            // Advanced search 
            // basic fields

            foreach (GalaOrder item in list)
            {
                if (
                    (ddlPaymentMethod.SelectedValue == "" || (ddlPaymentMethod.SelectedValue != "" && item.PaymentMethod == ddlPaymentMethod.SelectedValue)) &&
                    (ddlPaymentStatus.SelectedValue == "" || (ddlPaymentStatus.SelectedValue != "" && item.PayStatus == ddlPaymentStatus.SelectedValue)) &&
                    (ddlShipping.SelectedValue == "" || (ddlShipping.SelectedValue != "" && item.Shipping == ddlShipping.SelectedValue)) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && item.PayCountry == ddlCountry.SelectedValue)) &&
                    (status == "" || (ddlCountry.SelectedValue != "" && item.PayCountry == ddlCountry.SelectedValue)) &&
                    (
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "invoice") && item.Invoice.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "name") && (item.PayFirstname.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1 || item.PayLastname.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "email") && item.PayEmail.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))
                    )                    
                   )
                    flist.Add(item);
            }
        }
        else
        {
            if (status == "OOK")
            {
                foreach (GalaOrder item in list)
                    if (!String.IsNullOrEmpty(item.Invoice)) flist.Add(item);
            }
            else if (status == "PEN")
            {
                foreach (GalaOrder item in list)
                    if (String.IsNullOrEmpty(item.Invoice)) flist.Add(item);
            }
            else
                flist = list.ToList();
        }


        // Sort
        //flist = flist.OrderBy(p => p.JuryId).ToList();

        counter = 1;
        radGridEntry.DataSource = flist.OrderByDescending(m => m.DateCreated).OrderByDescending(m => m.Invoice).ToList();
        if (needRebind) radGridEntry.Rebind();

        GeneralFunction.SetReportDataCache(flist);       
    }

    #region Events
    private void PaySave_Clicked(object sender, EventArgs e)
    {
        //phPay.Visible = false;
        //BindGrid(true);
        Response.Redirect("GalaOrderList.aspx"); // if not redirect, this will cause caching and also cause double posting of the payment form if f5 by user
    }
    private void PayCancel_Clicked(object sender, EventArgs e)
    {
        phPay.Visible = false;
    }
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GalaOrder go = (GalaOrder)e.Item.DataItem;

            LinkButton lnkBtn = null;
            Label lbl = null;
            HyperLink lnk = null;

             // Shippng
            lbl = (Label)e.Item.FindControl("lbShipping");
            if (go.Shipping == "collect_office") lbl.Text = "Office";
            if (go.Shipping == "collect_onsite") lbl.Text = "Onsite";
            if (go.Shipping == "courier") lbl.Text = "Courier";

            // Payment Method
            lbl = (Label)e.Item.FindControl("lbPaymentMethod");
            lbl.Text = GeneralFunction.GetPaymentType(go.PaymentMethod);

            // Payment Status
            lbl = (Label)e.Item.FindControl("lbPaymentStatus");
            lbl.Text = GeneralFunction.GetPaymentGalaStatus(go.PayStatus);
            if (go.PayStatus == StatuspaymentGalaOrder.Paid) lbl.ForeColor = System.Drawing.Color.Green;
            if (go.PayStatus == StatuspaymentGalaOrder.NotPaid) lbl.ForeColor = System.Drawing.Color.Red;

            // update payment
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnUpdatePayment");
            lnkBtn.CommandArgument = go.Id.ToString();
            if (go.PaymentMethod == PaymentType.PayPal) lnkBtn.Visible = false; // dont show for PP


            //// chkboxes
            //CheckBox chkbox = (CheckBox)e.Item.FindControl("chkbox");
            //Security.SecureControlByHiding(chkbox);

            counter++;
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
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            //GeneralFunction.SetRedirect("../Admin/EntryList.aspx");  // to return from whereever
            //Response.Redirect("../Admin/Jury.aspx?juryId=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "View")
        {
            Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Admin/GalaOrderList.aspx");  // to return from whereever
            Response.Redirect("../Gala/Order.aspx?Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        else if (e.CommandName == "Payment")
        {
            phPay.Visible = true;
            up1.GalaOrderId = new Guid(e.CommandArgument.ToString());
            up1.PopulateForm();
        }
    }
    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
    }
    protected void rtabEntry_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
    {
        Telerik.Web.UI.RadTab TabClicked = e.Tab;
        string tabvalue = TabClicked.Value;

        ViewState["TabFilterValue"] = tabvalue;
        ViewState["AdvanceSearch"] = "";
        BindGrid(true);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rtabEntry.Visible = false;

        ViewState["AdvanceSearch"] = "1";
        BindGrid(true);
        lblError.Text = "";

        //GeneralFunction.SetFilter("JuryList", txtSearch.Text, ddlSearch.SelectedValue, ddlNetwork.SelectedValue, ddlJuryPanel1.SelectedValue,
        //                          ddlHoldingCompany.SelectedValue, ddlJuryPanel2.SelectedValue, ddlCountry.SelectedValue, ddlAssignedRound.SelectedValue);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlPaymentMethod.SelectedValue = "";
        ddlPaymentStatus.SelectedValue = "";
        ddlCountry.SelectedValue = "";
        rtabEntry.Visible = true;

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";
        //rtabEntry.SelectedIndex = 0;
        BindGrid(true);
        lblError.Text = "";

        GeneralFunction.ResetFilter();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<GalaOrder> flist = (List<GalaOrder>)data;

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Gala Order Listing";
            workbook.Worksheets.Add(sheetName);
            x = 1;


            #region Basic Gala Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 3; x++;


            // basic details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Invoice"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 18; x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Contact"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address1"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address2"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 20; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("City"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Postal"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Table Count"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Seat Count"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Shipping"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Payment Method"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Payment Status"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 15; x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Amount"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Shipping Fee"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Fees"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Tax"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Paid"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 10; x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Remarks"); workbook.Worksheets.Worksheet(sheetName).Column(x).Width = 30; x++;

            #endregion

            y++;

            foreach (GalaOrder go in flist)
            {
                x = 1;


                #region Basic Gala DataRows

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;

                // basic details
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.Invoice); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(go.DateCreated, "dd/MM/yy hh:mm tt")); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayFirstname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayLastname); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayEmail); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.ShowFriendlyContact(go.PayContact)); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayCompany); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayAddress1); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayAddress2); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayCity); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayPostal); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.PayCountry); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.TableCount); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.SeatCount); x++;

                string shipping = "";
                if (go.Shipping == "collect_office") shipping = "Office";
                if (go.Shipping == "collect_onsite") shipping = "Onsite";
                if (go.Shipping == "courier") shipping = "Courier";
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(shipping); x++;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetPaymentType(go.PaymentMethod)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.GetPaymentGalaStatus(go.PayStatus)); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.Amount); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.FeeShipping); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.Fee); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.Tax); workbook.Worksheets.Worksheet(sheetName).Cell(y, x).Style.NumberFormat.Format = "#,##0.00"; x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(GeneralFunction.CleanDateTimeToString(go.DatePaid, "dd/MM/yy hh:mm tt")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(go.RemarksPayment); x++;

                

                #endregion


                y++;
            }



            workbook.SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Gala_Order.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    #endregion

}