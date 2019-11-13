using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
using System.Text;

public partial class Gala_Order : System.Web.UI.Page
{
    GalaOrder go = null;
    string id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["Id"];
        if (id != null && id != "")
        {
            go = GalaOrder.GetGalaOrder(new Guid(id));
        }
        else
            go = GalaOrder.NewGalaOrder();

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }
    private void LoadForm()
    {
        GeneralFunction.LoadDropDownListCountry(ddlCountry);
        ddlCountry.Items.Insert(0, new ListItem("Select", ""));


        if (Security.IsUserAdminSpoof())
        {
            btnBack.Visible = true;
            btnEdit.Visible = false;
            btnSubmit.Visible = false;
            btnConfirm.Visible = false;

            GeneralFunction.ChangeStateControls(this, false);
        }
    }
    private void PopulateForm()
    {
        if (go.TableCount != 0) ddlTableCount.SelectedValue = go.TableCount.ToString();
        if (go.SeatCount != 0) ddlSingleCount.SelectedValue = go.SeatCount.ToString();
        rblShipping.SelectedValue = go.Shipping;

        txtFirstname.Text = go.PayFirstname;
        txtLastname.Text = go.PayLastname;
        txtContactCountryCode.Text = GeneralFunction.GetCountryCodeFromContactNumber(go.PayContact);
        txtContactAreaCode.Text = GeneralFunction.GetAreaCodeFromContactNumber(go.PayContact);
        txtContactNumber.Text = GeneralFunction.GetNumberFromContactNumber(go.PayContact);
        txtEmail.Text = go.PayEmail;
        txtCompany.Text = go.PayCompany;
        txtAddress1.Text = go.PayAddress1;
        txtAddress2.Text = go.PayAddress2;
        txtCity.Text = go.PayCity;
        txtPostal.Text = go.PayPostal;
        ddlCountry.SelectedValue = go.PayCountry;

        rblPayment.SelectedValue = go.PaymentMethod;

    }
    private bool ValidateForm()
    {
        lbError.Text = "";
        lbError2.Text = "";
        GeneralFunction.RemoveHighlightControls(this);

        if (ddlTableCount.SelectedValue == "" && ddlSingleCount.SelectedValue == "")
        {
            lbError.Text += GeneralFunction.ValidateDropDownList("Table count", ddlTableCount, true, "");
            lbError.Text += GeneralFunction.ValidateDropDownList("Single count", ddlSingleCount, true, "");
        }


        lbError.Text += GeneralFunction.ValidateRadioButtonList("Shipping", rblShipping, true);
        
        lbError.Text += GeneralFunction.ValidateTextBox("Company", txtCompany, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Address1", txtAddress1, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("City", txtCity, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Postal", txtPostal, true, "string");
        lbError.Text += GeneralFunction.ValidateDropDownList("Country", ddlCountry, true, "");
        lbError.Text += GeneralFunction.ValidateTextBox("First name", txtFirstname, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Last name", txtLastname, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Country Code", txtContactCountryCode, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Area Code", txtContactAreaCode, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact", txtContactNumber, true, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Email", txtEmail, true, "EmailSingle");

        lbError.Text += GeneralFunction.ValidateRadioButtonList("Payment Method", rblPayment, true);


        lbError2.Text = lbError.Text;
        return (lbError.Text == "");
    }
    private void SaveForm()
    {
        if (go.IsNew)
            go = GalaOrder.NewGalaOrder();

        go.TableCount = 0;
        go.SeatCount = 0;

        if (ddlTableCount.SelectedValue != "") go.TableCount = int.Parse(ddlTableCount.SelectedValue);
        if (ddlSingleCount.SelectedValue != "") go.SeatCount = int.Parse(ddlSingleCount.SelectedValue);
        go.Shipping = rblShipping.SelectedValue;

        go.PayFirstname = txtFirstname.Text.Trim();
        go.PayLastname = txtLastname.Text.Trim();
        go.PayContact = GeneralFunction.CreateContact(txtContactCountryCode.Text.Trim(), txtContactAreaCode.Text.Trim(), txtContactNumber.Text.Trim());
        go.PayEmail = txtEmail.Text.Trim();

        go.PayCompany = txtCompany.Text.Trim();
        go.PayAddress1 = txtAddress1.Text.Trim();
        go.PayAddress2 = txtAddress2.Text.Trim();
        go.PayCity = txtCity.Text.Trim();
        go.PayPostal = txtPostal.Text.Trim();
        go.PayCountry = ddlCountry.SelectedValue;

        go.PaymentMethod = rblPayment.SelectedValue;


        // Calculation
        go.Amount = (go.TableCount * 4000) + (go.SeatCount * 450);
        go.Fee = 0;
        if (go.Shipping == "courier") go.FeeShipping = 15;
        if (rblPayment.SelectedValue == PaymentType.BankTransfer) go.Fee = GeneralFunction.CalculateBankTransferFees();
        if (rblPayment.SelectedValue == PaymentType.PayPal) go.Fee = GeneralFunction.CalculateCreditFees(go.Amount + go.FeeShipping);

        // Gst
        go.Tax = 0;
        if (ddlCountry.SelectedValue.ToLower() == "singapore") go.Tax = GeneralFunction.CalculateTax(go.Amount + go.Fee + go.FeeShipping);


        // Misc
        go.Status = StatusGalaOrder.Confirm; //StatusGalaOrder.Draft;
        go.PayStatus = StatuspaymentGalaOrder.NotPaid;
        go.IsReminded = 0;
        go.DateCreatedString = DateTime.Now.ToString();
        

        if (go.IsValid)
        {
            go = go.Save();

        }
        else
        {
            lbError.Text = go.BrokenRulesCollection.ToString();
            lbError2.Text = go.BrokenRulesCollection.ToString();
        }

    }

    #region Events
    protected void rblPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            GeneralFunction.ChangeStateControls(this, false);

            btnSubmit.Visible = false;
            btnEdit.Visible = true;
            btnConfirm.Visible = true;
            pnlConfirmation.Visible = true;
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        GeneralFunction.ChangeStateControls(this, true);

        btnSubmit.Visible = true;
        btnEdit.Visible = false;
        btnConfirm.Visible = false;
        pnlConfirmation.Visible = false;
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        SaveForm();

        if (go.PaymentMethod == PaymentType.PayPal)
            PayPal(go.Id);
        else
        {
            GeneralFunction.CompletePendingPaymentGalaOthers(go.Id);
            Response.Redirect("../Gala/PendingPayment.aspx?Id=" + go.Id.ToString());
        }

    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(GeneralFunction.GetRedirect("../Admin/GalaOrderList.aspx"));
    }
    #endregion


    private void PayPal(Guid goId)
    {
        StringBuilder url = new StringBuilder();

        string m_sPaypalBase = System.Configuration.ConfigurationManager.AppSettings["paypalBase"];
        string business = System.Configuration.ConfigurationManager.AppSettings["paypalEmail"];
        string cancelUrl = System.Configuration.ConfigurationManager.AppSettings["cancelPaymentUrlGala"] + "?id=" + goId.ToString();
        string returnUrl = System.Configuration.ConfigurationManager.AppSettings["successPaymentUrlGala"];
        string notifyUrl = System.Configuration.ConfigurationManager.AppSettings["notifyUrlGala"];        

        url.Append(m_sPaypalBase);
        url.AppendFormat("&business={0}", HttpUtility.UrlEncode(business));
        url.AppendFormat("&item_name={0}", HttpUtility.UrlEncode("Payment for Gala Order"));  // for: " + serials));
        url.AppendFormat("&item_number={0}", HttpUtility.UrlEncode(goId.ToString().Substring(0, 8).ToUpper())); // suppose to serial num, but there is possiblity of multiple entries per order????

        decimal amount = go.Amount;
        amount += go.Fee; // Add fees for PP payment
        amount += go.FeeShipping;
        amount += go.Tax;
        url.AppendFormat("&amount={0}", HttpUtility.UrlEncode(amount.ToString("0.00")));



        // //For testing of $0.01 
        //url.AppendFormat("&amount={0}", HttpUtility.UrlEncode("0.01"));




        url.AppendFormat("&currency_code={0}", HttpUtility.UrlEncode("SGD"));

        url.AppendFormat("&shipping={0}", HttpUtility.UrlEncode("0"));
        url.AppendFormat("&no_shipping={0}", HttpUtility.UrlEncode("0"));
        url.AppendFormat("&invoice={0}", "");

        string custom = goId.ToString();
        url.AppendFormat("&custom={0}", HttpUtility.UrlEncode(custom));
        url.AppendFormat("&cancel_return={0}", HttpUtility.UrlEncode(cancelUrl));
        url.AppendFormat("&notify_url={0}", HttpUtility.UrlEncode(notifyUrl));
        url.AppendFormat("&return={0}", HttpUtility.UrlEncode(returnUrl + "?custom=" + goId.ToString()));

        RawLog rawlog = RawLog.NewRawLog();
        rawlog.Type = 0; //send
        rawlog.Data = url.ToString();
        rawlog.DateString = DateTime.Now.ToString();

        if (rawlog.IsValid) rawlog.Save();


        Response.Redirect(url.ToString());
    }
}