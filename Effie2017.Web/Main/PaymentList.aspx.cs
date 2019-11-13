using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Telerik.Web.UI;

public partial class Main_PaymentList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ((Common_MasterPage)Page.Master).ShowUser();
        ((Common_MasterPage)Page.Master).ShowNav();
        ltrJs.Text = "";
        GeneralFunction.ResetGroupPaymentCache();
        if (!Page.IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    decimal amountInvoiced = 0;
    decimal amountPaid = 0;
    decimal balanceDue = 0;

    decimal AdhocamountInvoiced = 0;
    decimal AdhocamountPaid = 0;
    decimal AdhocbalanceDue = 0;

    protected void LoadForm()
    {
    }

    protected void PopulateForm()
    {
        BindEntry();
        radGridEntry.Rebind();

        lblAmountInvoiced.Text = amountInvoiced.ToString("0.00");
        lblAmountPaid.Text = amountPaid.ToString("0.00");
        lblBalanceDue.Text = balanceDue.ToString("0.00");
    }

    protected void BindEntry()
    {
        Effie2017.App.Registration registration = Security.GetLoginSessionUser();
        List<Effie2017.App.Entry> entryList = new List<Effie2017.App.Entry>();
        string guidList = "";

        Effie2017.App.EntryList entryList2 = Effie2017.App.EntryList.GetEntryList(Guid.Empty, registration.Id, "", StatusEntry.PaymentPending + "|" + StatusEntry.UploadPending + "|" + StatusEntry.UploadCompleted + "|" + StatusEntry.Completed + "|");

        foreach (Effie2017.App.Entry entry in entryList2)
        {
            //Try 3
            bool isFound = false;
            foreach (Effie2017.App.Entry entryCheck in entryList)
            {
                if (entryCheck.Invoice == entry.Invoice)
                {
                    isFound = true;
                    entryCheck.Amount += entry.Amount;
                    entryCheck.Fee += entry.Fee;
                    entryCheck.AmountReceived += entry.AmountReceived;
                    entryCheck.GrandAmount += entry.GrandAmount;
                }
            }

            if (!isFound)
                    entryList.Add(entry);
            //END Try 3
        }

        radGridEntry.DataSource = entryList;
        List<Effie2017.App.AdhocInvoiceItem> adhocInvoiceList = new List<Effie2017.App.AdhocInvoiceItem>();

        foreach (var entryItem in entryList) {
            AdhocInvoiceItemList AdhocInvoiceLists = Effie2017.App.AdhocInvoiceItemList.GetAdhocInvoiceItemList(entryItem.Id);
            foreach (var AdhocInvoiceItem in AdhocInvoiceLists)
            {
                int count = adhocInvoiceList.Where(x => x.Invoice == AdhocInvoiceItem.Invoice).ToList().Count();
                if (count == 0)
                {
                    adhocInvoiceList.Add(AdhocInvoiceItem);
                }
            }
        }

        radGridEntryAdhoc.DataSource = adhocInvoiceList.Where(x => !string.IsNullOrEmpty(x.Invoice));
    }

    protected void radGridEntryAdhoc_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Effie2017.App.AdhocInvoiceItem adhocInvoiceItem = (Effie2017.App.AdhocInvoiceItem)e.Item.DataItem;
            Effie2017.App.AdhocInvoice adhocInvoice = Effie2017.App.AdhocInvoice.GetAdhocInvoice(adhocInvoiceItem.AdhocInvoiceId);
            LinkButton lnkBtn = null;

            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnDelete");
            lnkBtn.Attributes.Add("onclick", "return DeleteConfirmation('Entry');");
            
            LinkButton EditButton = (LinkButton)e.Item.FindControl("lnkBtnEditAdhoc");
            EditButton.CommandArgument = adhocInvoice.PayGroupId.ToString();
            Security.SecureControlByHiding(lnkBtn);
            EditButton.Visible = true;
            
            HyperLink View = (HyperLink)e.Item.FindControl("lnkInvoiceAdhoc");
            View.NavigateUrl = "../Admin/AdhocPaymentPdfView.aspx?regId=" + GeneralFunction.StringEncryption(adhocInvoice.RegistrationId.ToString()) + "&adId=" + GeneralFunction.StringEncryption(adhocInvoice.Id.ToString());
            
            ((GridDataItem)e.Item)["DateModified"].Text = adhocInvoiceItem.DateModified.ToString("dd/MM/yy");
            ((GridDataItem)e.Item)["AmountPaid"].Text = adhocInvoice.AmountReceived.ToString("0.00");
            
            ((GridDataItem)e.Item)["PaymentMethod"].Text = GeneralFunction.GetPaymentType(adhocInvoice.PaymentMethod);
            string PaymentMethod = ((GridDataItem)e.Item)["PaymentMethod"].Text;
            if (!string.IsNullOrEmpty(PaymentMethod))
            {
                decimal Grant = adhocInvoice.GrandAmount;
                if (Grant == 0)
                    Grant = Convert.ToDecimal(((GridDataItem)e.Item)["GrandAmount"].Text);
                  
                ((GridDataItem)e.Item)["GrandAmount"].Text = Grant.ToString("0.00");
                ((GridDataItem)e.Item)["BalanceDue"].Text = (Convert.ToDecimal(((GridDataItem)e.Item)["GrandAmount"].Text) - Convert.ToDecimal(((GridDataItem)e.Item)["AmountPaid"].Text)).ToString("0.00");

                AdhocamountInvoiced += Grant;
                AdhocamountPaid += Convert.ToDecimal(((GridDataItem)e.Item)["AmountPaid"].Text);
                AdhocbalanceDue += Convert.ToDecimal(((GridDataItem)e.Item)["BalanceDue"].Text);
                EditButton.Text = "Edit";
            }
            else
            {
                View.Visible = false;
                EditButton.Text = "Choose payment method";
                ((GridDataItem)e.Item)["GrandAmount"].Text = "0.00";
                ((GridDataItem)e.Item)["BalanceDue"].Text = "0.00";
                ((GridDataItem)e.Item)["PaymentMethod"].Text = "<span style='font-weight: bold;'>please choose your payment method</span>";
            }
            
            lblAdhocAmountInvoiced.Text = AdhocamountInvoiced.ToString("0.00");
            lblAdhocAmountPaid.Text = AdhocamountPaid.ToString("0.00");
            lblAdhocBalanceDue.Text = AdhocbalanceDue.ToString("0.00");

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

            ((GridDataItem)e.Item)["GrandAmount"].Text = (Convert.ToDecimal(((GridDataItem)e.Item)["GrandAmount"].Text)).ToString("0.00");
            ((GridDataItem)e.Item)["DateSubmitted"].Text = entry.DateSubmitted.ToString("dd/MM/yy");

            //if (entry.PayStatus == StatusPaymentEntry.Paid)
            //    ((GridDataItem)e.Item)["AmountPaid"].Text = (Convert.ToDecimal(((GridDataItem)e.Item)["AmountPlusFee"].Text)).ToString("0.00");
            //else
            //    ((GridDataItem)e.Item)["AmountPaid"].Text = "0.00";
            ((GridDataItem)e.Item)["AmountPaid"].Text = entry.AmountReceived.ToString("0.00");

            ((GridDataItem)e.Item)["BalanceDue"].Text = (Convert.ToDecimal(((GridDataItem)e.Item)["GrandAmount"].Text) - Convert.ToDecimal(((GridDataItem)e.Item)["AmountPaid"].Text)).ToString("0.00");
            ((GridDataItem)e.Item)["PaymentMethod"].Text = GeneralFunction.GetPaymentType(entry.PaymentMethod);

            amountInvoiced += Convert.ToDecimal(((GridDataItem)e.Item)["GrandAmount"].Text);
            amountPaid += Convert.ToDecimal(((GridDataItem)e.Item)["AmountPaid"].Text);
            balanceDue += Convert.ToDecimal(((GridDataItem)e.Item)["BalanceDue"].Text);

            HyperLink lnk = (HyperLink)e.Item.FindControl("lnkInvoice");
            lnk.NavigateUrl = "./PaymentPdfView.aspx?id=" + GeneralFunction.StringEncryption(entry.Id.ToString());
            
            lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
            lnkBtn.CommandArgument = entry.PayGroupId.ToString();
            Security.SecureControlByHiding(lnkBtn);

            
            if (entry.AmountReceived > 0)
                lnkBtn.Visible = false;
            else
                lnkBtn.Visible = true;
        }
    }

    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "Edit")
        {
            //Response.Redirect("./Entry.aspx?id=" + GeneralFunction.StringEncryption(((GridDataItem)e.Item)["Id"].Text));
            //Response.Redirect("./Summary.aspx?Id=" + ((GridDataItem)e.Item)["Id"].Text);

            EntryList entryList = EntryList.GetEntryList(new Guid(e.CommandArgument.ToString()), Guid.Empty, "");
            foreach (Entry entry in entryList)
            {
                GeneralFunction.AddIdToGroupPaymentCache(entry.Id);
            }
            //Response.Redirect("./Summary.aspx");
            Response.Redirect("./Summary.aspx?pgId=" + GeneralFunction.StringEncryption(e.CommandArgument.ToString()));
        }
        else if (e.CommandName == "Delete")
        {
            Effie2017.App.Entry.CleanDeleteEntry(new Guid(((GridDataItem)e.Item)["Id"].Text));

            //lblError.Text += ((GridDataItem)e.Item)["Serial"].Text + " has been deleted.<br>";
            lblError.Text += "Entry has been deleted.<br>";
            BindEntry();
        }
        else if (e.CommandName == "Invoice")
        {
            Effie2017.App.Registration registration = Security.GetLoginSessionUser();
            Effie2017.App.Entry entry = Effie2017.App.Entry.GetEntry(new Guid(((GridDataItem)e.Item)["Id"].Text));

            Session["registrationForPdfView"] = registration;
            Session["entryForPdfView"] = entry;

            ltrJs.Text = "<script type=\"text/javascript\"> window.open('PaymentPdfView.aspx'); </script>";
        }
    }

    protected void radGridEntry_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        BindEntry();
    }


    protected void radGridEntryAdhoc_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";
        if (e.CommandName == "Edit")
        {
            Response.Redirect("AdhocInvoiceSummary.aspx?pgId=" + GeneralFunction.StringEncryption(e.CommandArgument.ToString()));
        }
    }

    protected void radGridEntryAdhoc_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        BindEntry();
    }
}