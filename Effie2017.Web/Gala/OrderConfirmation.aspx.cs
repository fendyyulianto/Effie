using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Gala_OrderConfirmation : System.Web.UI.Page
{
    GalaOrder go = null;

    protected void Page_Load(object sender, EventArgs e)
    {
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

    }
    private void PopulateForm()
    {
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
        lbError.Text += GeneralFunction.ValidateTextBox("Email", txtEmail, true, "string");

        lbError.Text += GeneralFunction.ValidateRadioButtonList("Payment Method", rblPayment, true);


        lbError2.Text = lbError.Text;
        return (lbError.Text == "");
    }
    private void SaveForm()
    {
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
        go.Amount = (go.TableCount * 1500) + (go.SeatCount * 180);
        go.Fee = 0;
        if (rblPayment.SelectedValue == PaymentType.BankTransfer) go.Fee = GeneralFunction.CalculateBankTransferFees();
        if (rblPayment.SelectedValue == PaymentType.PayPal) go.Fee = GeneralFunction.CalculateCreditFees(go.Amount);


        // Misc
        go.Status = StatusGalaOrder.Draft;
        go.PayStatus = StatuspaymentGalaOrder.NotPaid;
        go.IsReminded = 0;
        go.DateCreatedString = DateTime.Now.ToString();


        if (go.IsValid)
        {
            go.Save();

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
            SaveForm();


            //if (rblPayment.SelectedValue == PaymentType.PayPal)
            //{
            //    // get the string of serial numbers
            //    string serials = "";
            //    EntryList list = EntryList.GetEntryList(payGroupId, Guid.Empty, ""); // contains the pay group id
            //    foreach (Entry entry in list)
            //    {
            //        serials += entry.Serial + ",";
            //    }
            //    if (serials != "") serials = serials.Substring(0, serials.Length - 1);

            //    PayPal(serials);
            //}
            //else
            //{
            //    // temp force to paid, to be removed
            //    //GeneralFunction.CompleteNewEntrySubmissionOthers(payGroupId);


            //    Response.Redirect("../Main/PendingPayment.aspx");
            //}

            Response.Redirect("../Gala/OrderConfirmation.aspx");
        }
    }

    #endregion
}