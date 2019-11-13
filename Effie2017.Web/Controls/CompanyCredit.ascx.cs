using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Controls_CompanyCredit : System.Web.UI.UserControl
{
    public event EventHandler Save_Clicked;
    public event EventHandler Cancel_Clicked;

    public Guid RecordId { get; set; }
    public Guid EntryId { get; set; }
    public int No { get; set; }
    public string ContactType { get; set; }
    public string Company { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string Postal { get; set; }
    public string Country { get; set; }
    public string Salutation { get; set; }
    public string Fullname { get; set; }
    public string Job { get; set; }
    public string Contact { get; set; }
    public string Email { get; set; }
    public string CompanyType { get; set; }
    public string CompanyTypeOther { get; set; }
    public string ClientCompanyNetwork { get; set; }
    public string ClientCompanyNetworkOthers { get; set; }
    public string Network { get; set; }
    public string NetworkOther { get; set; }
    public string HoldingCompany { get; set; }
    public string HoldingCompanyOther { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public bool IsNew { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LoadForm();
            //PopulateForm();
            
        }
    }
    private void LoadForm()
    {
    }
    public void PopulateForm()
    {
        // Salutation
        ddlSalutation.Items.Clear();
        ddlSalutation.Items.Add(new ListItem("Select", ""));
        ddlSalutation.Items.Add("Dr.");
        ddlSalutation.Items.Add("Mr.");
        ddlSalutation.Items.Add("Ms.");
        ddlSalutation.Items.Add("Mrs.");

        // Country - to add APAC countries before the rest
        ddlCountry.Items.Clear();
        ddlCountry.Items.Add(new ListItem("Select", ""));
        ddlCountry.Items.Add(new ListItem("Australia", "Australia"));
        ddlCountry.Items.Add(new ListItem("Bangladesh", "Bangladesh"));
        ddlCountry.Items.Add(new ListItem("Bhutan", "Bhutan"));
        ddlCountry.Items.Add(new ListItem("Cambodia", "Cambodia"));
        ddlCountry.Items.Add(new ListItem("China", "China"));
        ddlCountry.Items.Add(new ListItem("Cook Islands", "Cook Islands"));
        ddlCountry.Items.Add(new ListItem("Fiji", "Fiji"));
        ddlCountry.Items.Add(new ListItem("French Polynesia", "French Polynesia"));
        ddlCountry.Items.Add(new ListItem("Hong Kong", "Hong Kong"));
        ddlCountry.Items.Add(new ListItem("India", "India"));
        ddlCountry.Items.Add(new ListItem("Indonesia", "Indonesia"));
        ddlCountry.Items.Add(new ListItem("Japan", "Japan"));
        ddlCountry.Items.Add(new ListItem("Kazakhstan", "Kazakhstan"));
        ddlCountry.Items.Add(new ListItem("Kiribati", "Kiribati"));
        ddlCountry.Items.Add(new ListItem("Korea", "Korea"));
        ddlCountry.Items.Add(new ListItem("Laos", "Laos"));
        ddlCountry.Items.Add(new ListItem("Malaysia", "Malaysia"));
        ddlCountry.Items.Add(new ListItem("Maldives", "Maldives"));
        ddlCountry.Items.Add(new ListItem("Marshall Islands", "Marshall Islands"));
        ddlCountry.Items.Add(new ListItem("Micronesia", "Micronesia"));
        ddlCountry.Items.Add(new ListItem("Mongolia", "Mongolia"));
        ddlCountry.Items.Add(new ListItem("Myanmar", "Myanmar"));
        ddlCountry.Items.Add(new ListItem("Nauru", "Nauru"));
        ddlCountry.Items.Add(new ListItem("Nepal", "Nepal"));
        ddlCountry.Items.Add(new ListItem("New Caledonia", "New Caledonia"));
        ddlCountry.Items.Add(new ListItem("New Zealand", "New Zealand"));
        ddlCountry.Items.Add(new ListItem("Niue", "Niue"));
        ddlCountry.Items.Add(new ListItem("Pakistan", "Pakistan"));
        ddlCountry.Items.Add(new ListItem("Palau", "Palau"));
        ddlCountry.Items.Add(new ListItem("Philippines", "Philippines"));
        ddlCountry.Items.Add(new ListItem("Samoa", "Samoa"));
        ddlCountry.Items.Add(new ListItem("Singapore", "Singapore"));
        ddlCountry.Items.Add(new ListItem("Solomon Islands", "Solomon Islands"));
        ddlCountry.Items.Add(new ListItem("Sri Lanka", "Sri Lanka"));
        ddlCountry.Items.Add(new ListItem("Taiwan", "Taiwan"));
        ddlCountry.Items.Add(new ListItem("Thailand", "Thailand"));
        ddlCountry.Items.Add(new ListItem("Timor-Leste", "Timor-Leste"));
        ddlCountry.Items.Add(new ListItem("Tonga", "Tonga"));
        ddlCountry.Items.Add(new ListItem("Tuvalu", "Tuvalu"));
        ddlCountry.Items.Add(new ListItem("Uzbekistan", "Uzbekistan"));
        ddlCountry.Items.Add(new ListItem("Vanautu", "Vanautu"));
        ddlCountry.Items.Add(new ListItem("Vietnam", "Vietnam"));



        ddlCountry.Items.Add(new ListItem("-------Rest of the Standard Countries---------", ""));
        GeneralFunction.LoadDropDownListCountry(ddlCountry);


        // Client Company Network
        ddlClientCompanyNetwork.Items.Clear();
        ddlClientCompanyNetwork.Items.Add(new ListItem("Select", ""));
        ddlClientCompanyNetwork.Items.Add("360buy");
        ddlClientCompanyNetwork.Items.Add("361°");
        ddlClientCompanyNetwork.Items.Add("7-Eleven, Inc.");
        ddlClientCompanyNetwork.Items.Add("A.F. Watsons Group");
        ddlClientCompanyNetwork.Items.Add("AB InBev");
        ddlClientCompanyNetwork.Items.Add("Adidas");
        ddlClientCompanyNetwork.Items.Add("Aditya Birla Group");
        ddlClientCompanyNetwork.Items.Add("Alibaba");
        ddlClientCompanyNetwork.Items.Add("Alipay");
        ddlClientCompanyNetwork.Items.Add("Amnesty International");
        ddlClientCompanyNetwork.Items.Add("Amway");
        ddlClientCompanyNetwork.Items.Add("Apaiporn Srisook");
        ddlClientCompanyNetwork.Items.Add("Audi");
        ddlClientCompanyNetwork.Items.Add("AVON");
        ddlClientCompanyNetwork.Items.Add("AXA");
        ddlClientCompanyNetwork.Items.Add("Baidu");
        ddlClientCompanyNetwork.Items.Add("Bank of New Zealand");
        ddlClientCompanyNetwork.Items.Add("Bayer");
        ddlClientCompanyNetwork.Items.Add("Bear Electric");
        ddlClientCompanyNetwork.Items.Add("BenQ");
        ddlClientCompanyNetwork.Items.Add("BMW");
        ddlClientCompanyNetwork.Items.Add("Boundary Road Brewery");
        ddlClientCompanyNetwork.Items.Add("BP");
        ddlClientCompanyNetwork.Items.Add("Brother");
        ddlClientCompanyNetwork.Items.Add("Cadbury");
        ddlClientCompanyNetwork.Items.Add("Campbell Moore");
        ddlClientCompanyNetwork.Items.Add("Canon");
        ddlClientCompanyNetwork.Items.Add("Carlsberg");
        ddlClientCompanyNetwork.Items.Add("Centre for Enabled Living");
        ddlClientCompanyNetwork.Items.Add("Cerebos");
        ddlClientCompanyNetwork.Items.Add("China Telecom");
        ddlClientCompanyNetwork.Items.Add("CHIVAS");
        ddlClientCompanyNetwork.Items.Add("Christchurch & Canterbury Tourism");
        ddlClientCompanyNetwork.Items.Add("Chrysler");
        ddlClientCompanyNetwork.Items.Add("Citibank");
        ddlClientCompanyNetwork.Items.Add("Clarks");
        ddlClientCompanyNetwork.Items.Add("Coach");
        ddlClientCompanyNetwork.Items.Add("Coca-Cola");
        ddlClientCompanyNetwork.Items.Add("Columbia Sportswear");
        ddlClientCompanyNetwork.Items.Add("CSL Limited");
        ddlClientCompanyNetwork.Items.Add("Daimler AG");
        ddlClientCompanyNetwork.Items.Add("Danone");
        ddlClientCompanyNetwork.Items.Add("Danzi");
        ddlClientCompanyNetwork.Items.Add("DBS");
        ddlClientCompanyNetwork.Items.Add("Dell");
        ddlClientCompanyNetwork.Items.Add("Diageo");
        ddlClientCompanyNetwork.Items.Add("Dior");
        ddlClientCompanyNetwork.Items.Add("Disney");
        ddlClientCompanyNetwork.Items.Add("Dongsuh Food");
        ddlClientCompanyNetwork.Items.Add("eLong");
        ddlClientCompanyNetwork.Items.Add("Estee Lauder Companies");
        ddlClientCompanyNetwork.Items.Add("FAW Hainan Motor");
        ddlClientCompanyNetwork.Items.Add("Ferrero");
        ddlClientCompanyNetwork.Items.Add("Fiat");
        ddlClientCompanyNetwork.Items.Add("Fiat Chrysler");
        ddlClientCompanyNetwork.Items.Add("Fonterra Co-operative Group");
        ddlClientCompanyNetwork.Items.Add("Ford Motor Company");
        ddlClientCompanyNetwork.Items.Add("Foton");
        ddlClientCompanyNetwork.Items.Add("Garnier");
        ddlClientCompanyNetwork.Items.Add("Gatorade");
        ddlClientCompanyNetwork.Items.Add("General Electric Company");
        ddlClientCompanyNetwork.Items.Add("General Motors");
        ddlClientCompanyNetwork.Items.Add("GlaxoSmithKline");
        ddlClientCompanyNetwork.Items.Add("Google");
        ddlClientCompanyNetwork.Items.Add("Greenpeace");
        ddlClientCompanyNetwork.Items.Add("Haier");
        ddlClientCompanyNetwork.Items.Add("Health Promotion Agency");
        ddlClientCompanyNetwork.Items.Add("Health Promotion Board");
        ddlClientCompanyNetwork.Items.Add("Heart & Sole");
        ddlClientCompanyNetwork.Items.Add("Heineken");
        ddlClientCompanyNetwork.Items.Add("Hell Pizza");
        ddlClientCompanyNetwork.Items.Add("Hewlett-Packard Company");
        ddlClientCompanyNetwork.Items.Add("Hungry Jack's");
        ddlClientCompanyNetwork.Items.Add("Hyundai Group");
        ddlClientCompanyNetwork.Items.Add("Hyundai Motor Company");
        ddlClientCompanyNetwork.Items.Add("IBM");
        ddlClientCompanyNetwork.Items.Add("ICICI Bank");
        ddlClientCompanyNetwork.Items.Add("IKEA");
        ddlClientCompanyNetwork.Items.Add("Insurance Australia Group");
        ddlClientCompanyNetwork.Items.Add("Intel");
        ddlClientCompanyNetwork.Items.Add("Johnson & Johnson");
        ddlClientCompanyNetwork.Items.Add("Kewal Kiran Clothing Limited");
        ddlClientCompanyNetwork.Items.Add("Kia Motors");
        ddlClientCompanyNetwork.Items.Add("Kimberly-Clark");
        ddlClientCompanyNetwork.Items.Add("Kirin Company, Limited");
        ddlClientCompanyNetwork.Items.Add("Kmart");
        ddlClientCompanyNetwork.Items.Add("Kraft");
        ddlClientCompanyNetwork.Items.Add("Land Rover");
        ddlClientCompanyNetwork.Items.Add("Leaf");
        ddlClientCompanyNetwork.Items.Add("Lego Group");
        ddlClientCompanyNetwork.Items.Add("Lenovo");
        ddlClientCompanyNetwork.Items.Add("Levi Strauss & Co.");
        ddlClientCompanyNetwork.Items.Add("LG Electronics");
        ddlClientCompanyNetwork.Items.Add("Lipton");
        ddlClientCompanyNetwork.Items.Add("L'Oréal");
        ddlClientCompanyNetwork.Items.Add("Luxottica");
        ddlClientCompanyNetwork.Items.Add("LVMH");
        ddlClientCompanyNetwork.Items.Add("Mars");
        ddlClientCompanyNetwork.Items.Add("McDonald's");
        ddlClientCompanyNetwork.Items.Add("MENGNIU");
        ddlClientCompanyNetwork.Items.Add("Mercedes-Benz");
        ddlClientCompanyNetwork.Items.Add("Metersbonwe Group");
        ddlClientCompanyNetwork.Items.Add("Microsoft");
        ddlClientCompanyNetwork.Items.Add("Mitre 10 (NZ) Limited");
        ddlClientCompanyNetwork.Items.Add("Mizuno");
        ddlClientCompanyNetwork.Items.Add("Mondelez International");
        ddlClientCompanyNetwork.Items.Add("Motorola");
        ddlClientCompanyNetwork.Items.Add("MTM");
        ddlClientCompanyNetwork.Items.Add("Nestlé");
        ddlClientCompanyNetwork.Items.Add("NetEase");
        ddlClientCompanyNetwork.Items.Add("Neutrogena");
        ddlClientCompanyNetwork.Items.Add("New Balance");
        ddlClientCompanyNetwork.Items.Add("Nike");
        ddlClientCompanyNetwork.Items.Add("Nikon Corporation");
        ddlClientCompanyNetwork.Items.Add("Nissan");
        ddlClientCompanyNetwork.Items.Add("Nissin");
        ddlClientCompanyNetwork.Items.Add("Nivea");
        ddlClientCompanyNetwork.Items.Add("Nokia");
        ddlClientCompanyNetwork.Items.Add("NSW Rural Fire Service");
        ddlClientCompanyNetwork.Items.Add("ORBIS");
        ddlClientCompanyNetwork.Items.Add("Parmalat");
        ddlClientCompanyNetwork.Items.Add("PepsiCo");
        ddlClientCompanyNetwork.Items.Add("Pernod Ricard");
        ddlClientCompanyNetwork.Items.Add("Peugeot");
        ddlClientCompanyNetwork.Items.Add("Philippine Department of Tourism");
        ddlClientCompanyNetwork.Items.Add("Philips");
        ddlClientCompanyNetwork.Items.Add("Pizza Hut");
        ddlClientCompanyNetwork.Items.Add("Procter & Gamble");
        ddlClientCompanyNetwork.Items.Add("Protoleaf");
        ddlClientCompanyNetwork.Items.Add("Puma");
        ddlClientCompanyNetwork.Items.Add("Reckitt Benckiser");
        ddlClientCompanyNetwork.Items.Add("Red Bull");
        ddlClientCompanyNetwork.Items.Add("Samsung");
        ddlClientCompanyNetwork.Items.Add("sanofi-aventis");
        ddlClientCompanyNetwork.Items.Add("SAP");
        ddlClientCompanyNetwork.Items.Add("Save Our Sons");
        ddlClientCompanyNetwork.Items.Add("Shabondama Soap Co., Ltd.");
        ddlClientCompanyNetwork.Items.Add("Shanghai General Motors");
        ddlClientCompanyNetwork.Items.Add("Shiseido");
        ddlClientCompanyNetwork.Items.Add("Siemens");
        ddlClientCompanyNetwork.Items.Add("Singapore University of Technology and Design");
        ddlClientCompanyNetwork.Items.Add("Sony");
        ddlClientCompanyNetwork.Items.Add("Subaru");
        ddlClientCompanyNetwork.Items.Add("Subway");
        ddlClientCompanyNetwork.Items.Add("Suntory Group");
        ddlClientCompanyNetwork.Items.Add("Taiwan Beer");
        ddlClientCompanyNetwork.Items.Add("Taiwan High Speed Rail");
        ddlClientCompanyNetwork.Items.Add("TESCO");
        ddlClientCompanyNetwork.Items.Add("Thai Health Promotion Foundation");
        ddlClientCompanyNetwork.Items.Add("The Campbell Soup Company");
        ddlClientCompanyNetwork.Items.Add("Toyota");
        ddlClientCompanyNetwork.Items.Add("Unilever");
        ddlClientCompanyNetwork.Items.Add("UNITED ARROWS LTD.");
        ddlClientCompanyNetwork.Items.Add("UPS");
        ddlClientCompanyNetwork.Items.Add("V8 Supercars");
        ddlClientCompanyNetwork.Items.Add("Virgin Mobile Group");
        ddlClientCompanyNetwork.Items.Add("Volkswagen AG");
        ddlClientCompanyNetwork.Items.Add("Wrigley Confectionery (China) Limited");
        ddlClientCompanyNetwork.Items.Add("Wyeth Consumer Healthcare");
        ddlClientCompanyNetwork.Items.Add("Xian Janssen Pharmaceutical");
        ddlClientCompanyNetwork.Items.Add("Yadea");
        ddlClientCompanyNetwork.Items.Add("Yili");
        ddlClientCompanyNetwork.Items.Add("Yuen Foong Yu Group");
        ddlClientCompanyNetwork.Items.Add("Yum! Brands");
        ddlClientCompanyNetwork.Items.Add("Zespri");
        ddlClientCompanyNetwork.Items.Add(new ListItem("Others, pls specify", "Others"));
        
        // Network
        GeneralFunction.LoadddlNetwork(ddlNetwork, true, true);

        // Holding COmpany
        GeneralFunction.LoadddlHoldingCompany(ddlHoldingCompany, true, true);
        
        // pop type
        // check if there is already 2 Leads, then dont pop the lead anymomre
        ddlContactType.Items.Clear();
        ddlContactType.Items.Add(new ListItem("Select", ""));
        if (GeneralFunction.NumberOfClientInListCache() < 2 || ContactType == "Client") ddlContactType.Items.Add(new ListItem("Client", "Client"));
        if (GeneralFunction.NumberOfLeadInListCache() < 2 || ContactType == "Lead Agency") ddlContactType.Items.Add(new ListItem("Lead Agency", "Lead Agency"));
        if (GeneralFunction.NumberOfContributingCompanyInListCache() < 4 || ContactType == "Contributing Agency") ddlContactType.Items.Add(new ListItem("Contributing Agency", "Contributing Agency"));


        
        lbType.Text = ContactType;
        txtCompany.Text = Company;
        txtAddress1.Text = Address1;
        txtAddress2.Text = Address2;
        txtCity.Text = City;
        txtPostal.Text = Postal;
        ddlCountry.SelectedValue = Country;
        ddlSalutation.SelectedValue = Salutation;
        txtFullname.Text = Fullname;
        txtJob.Text = Job;
        txtContactCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(Contact);
        txtContactArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(Contact);
        txtContactNumber.Text = GeneralFunction.GetNumberFromContactNumber(Contact);
        txtEmail.Text = Email;

        ddlCompanyType.SelectedValue = CompanyType;
        txtCompanyTypeOther.Text = CompanyTypeOther;
        ddlClientCompanyNetwork.SelectedValue = ClientCompanyNetwork;
        txtClientCompanyNetworkOthers.Text = ClientCompanyNetworkOthers;
        txtNetworkOther.Text = NetworkOther;

        try { ddlNetwork.SelectedValue = Network; }
        catch { ddlNetwork.SelectedValue = ""; }
        
        try { ddlHoldingCompany.SelectedValue = HoldingCompany; }
        catch { ddlHoldingCompany.SelectedValue = ""; }

        txtHoldingCompanyOther.Text = HoldingCompanyOther;

        hldRecordId.Value = RecordId.ToString();
        hldIsNew.Value = IsNew.ToString();
        hldEntryId.Value = EntryId.ToString();
        hldNo.Value = No.ToString();
        hldDateCreated.Value = DateCreated.ToString();
        hldDateModified.Value = DateModified.ToString();

        // hide the others textboxes by defasult
        phClientCompanyNetworkOther.Visible = false;
        phNetworkOther.Visible = false;
        phHoldingCompanyOther.Visible = false;
        if (txtClientCompanyNetworkOthers.Text.Trim() != "") phClientCompanyNetworkOther.Visible = true;
        if (txtNetworkOther.Text.Trim() != "") phNetworkOther.Visible = true;
        if (txtHoldingCompanyOther.Text.Trim() != "") phHoldingCompanyOther.Visible = true;

        // show up the ddl or label for type
        ddlContactType.Visible = false;
        lbType.Visible = false;
        
        //if (ContactType == "")
        //    ddlContactType.Visible = true;
        //else
        //    lbType.Visible = true;

        // only editable for type for 3rd CC onwards
        ShowCompanyType(No, ContactType);


        // scrolls up to the cc table after postback
        btnSubmit.OnClientClick = "location.hash = 'CCTABLE'; return true;";
        btnCancel.OnClientClick = "location.hash = 'CCTABLE'; return true;";
    }

    private void ShowCompanyType(int No,string ContactType)
    {
        if (No > 2)
        {
            ddlContactType.Visible = true;
            ddlContactType.SelectedValue = ContactType;
        }
        else
            lbType.Visible = true;


        // networking section 
        if (ContactType == "Client")
        {
            phCCNetwork.Visible = true;
            phNetwork.Visible = false;
            phCompanyType.Visible = false;
        }
        else
        {
            phCCNetwork.Visible = false;
            phNetwork.Visible = true;
            phCompanyType.Visible = true;

            // show the agency infor
            phAgencyInfo.Visible = true;
        }
    }

    #region Helper
    public bool ValidateForm()
    {
        lbError.Text = "";
        lbError2.Text = "";
        GeneralFunction.RemoveHighlightControls(this);

        // validate only if its shown, mneaning new CC
        if (ddlContactType.Visible)
            lbError.Text += GeneralFunction.ValidateDropDownList("Type", ddlContactType, true, "");

        lbError.Text += GeneralFunction.ValidateTextBox("Company Name", txtCompany, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Address1", txtAddress1, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("City", txtCity, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Postal Code", txtPostal, true, "string");
        lbError.Text += GeneralFunction.ValidateDropDownList("Country", ddlCountry, true, "");
        lbError.Text += GeneralFunction.ValidateDropDownList("Salutation", ddlSalutation, true, "");
        lbError.Text += GeneralFunction.ValidateTextBox("Fullname", txtFullname, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Job", txtJob, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Country Code", txtContactCountry, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Area Code", txtContactArea, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Number", txtContactNumber, true, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Email", txtEmail, true, "EmailSingle");

       
       
        //if (ddlCompanyType.SelectedValue == "Others")
        //    lbError.Text += GeneralFunction.ValidateTextBox("Type of Company - Others", txtCompanyTypeOther, true, "string");

        if (lbType.Text == "Client" || ddlContactType.SelectedValue == "Client")
        {
            lbError.Text += GeneralFunction.ValidateDropDownList("Client Company Network", ddlClientCompanyNetwork, true, "");
            if (ddlClientCompanyNetwork.SelectedValue == "Others")
                lbError.Text += GeneralFunction.ValidateTextBox("Client Company Network", txtClientCompanyNetworkOthers, true, "string");
        }
        else
        {
            lbError.Text += GeneralFunction.ValidateDropDownList("Type of Company", ddlCompanyType, true, "");

            lbError.Text += GeneralFunction.ValidateDropDownList("Network", ddlNetwork, true, "");
            if (ddlNetwork.SelectedValue == "Others")
                lbError.Text += GeneralFunction.ValidateTextBox("Network - Others", txtNetworkOther, true, "string");

            lbError.Text += GeneralFunction.ValidateDropDownList("Holding Company", ddlHoldingCompany, true, "");
            if (ddlHoldingCompany.SelectedValue == "Others")
                lbError.Text += GeneralFunction.ValidateTextBox("Holding Company - Others", txtHoldingCompanyOther, true, "string");

        }

        lbError2.Text = lbError.Text;
        return (lbError.Text == "");
    }
    public void SaveForm()
    {
        ContactType = lbType.Text.Trim();
        if (ContactType == "" || ddlContactType.SelectedValue != "") ContactType = ddlContactType.SelectedValue;
        Company = txtCompany.Text.Trim();
        Address1 = txtAddress1.Text.Trim();
        Address2 = txtAddress2.Text.Trim();
        City = txtCity.Text.Trim();
        Postal = txtPostal.Text.Trim();
        Country = ddlCountry.SelectedValue;
        Salutation = ddlSalutation.SelectedValue;
        Fullname = txtFullname.Text.Trim();
        Job = txtJob.Text.Trim();
        Contact = txtContactCountry.Text.Trim() + "|" + txtContactArea.Text.Trim() + "|" + txtContactNumber.Text.Trim();
        Email = txtEmail.Text.Trim();

        CompanyType = ddlCompanyType.SelectedValue;
        CompanyTypeOther = txtCompanyTypeOther.Text;
        ClientCompanyNetwork = ddlClientCompanyNetwork.SelectedValue;
        ClientCompanyNetworkOthers = txtClientCompanyNetworkOthers.Text.Trim();
        
        Network = ddlNetwork.SelectedValue;
        NetworkOther = "";
        if (Network == "Others") NetworkOther = txtNetworkOther.Text.Trim();
        
        HoldingCompany = ddlHoldingCompany.SelectedValue;
        HoldingCompanyOther = "";
        if (HoldingCompany == "Others") HoldingCompanyOther = txtHoldingCompanyOther.Text.Trim();
        
        if (IsNew)
            DateCreated = DateTime.Now;
        else
            DateCreated = DateTime.Parse(hldDateCreated.Value);
        DateModified = DateTime.Now;

        EntryId = Guid.Parse(hldEntryId.Value);
        RecordId = Guid.Parse(hldRecordId.Value);
        No = int.Parse(hldNo.Value);
        IsNew = bool.Parse(hldIsNew.Value);
       
    }
    public void ResetForm()
    {
        lbType.Text = "";
        lbType.Visible = false;

        ddlContactType.SelectedValue = "";
        ddlContactType.Visible = false;

        txtCompany.Text = "";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtCity.Text = "";
        txtPostal.Text = "";
        ddlCountry.SelectedValue = "";
        ddlSalutation.SelectedValue = "";
        txtFullname.Text = "";
        txtJob.Text = "";
        txtContactCountry.Text = "";
        txtContactArea.Text = "";
        txtContactNumber.Text = "";
        txtEmail.Text = "";
        ddlClientCompanyNetwork.SelectedValue = "";
        txtClientCompanyNetworkOthers.Text = "";
        ddlNetwork.SelectedValue = "";
        ddlHoldingCompany.SelectedValue = "";
    }
    public void EnableViewMode()
    {
        btnSubmit.Visible = false;
    }
    public void EnableEditMode()
    {
        btnSubmit.Visible = true;
    }
    #endregion

    #region Events
    protected void ddlClientCompanyNetwork_OnSelectedIndexChange(object sender, EventArgs e)
    {
        phClientCompanyNetworkOther.Visible = false;
        if (ddlClientCompanyNetwork.SelectedValue == "Others") phClientCompanyNetworkOther.Visible = true;
    }
    protected void ddlNetwork_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        phNetworkOther.Visible = false;
        if (ddlNetwork.SelectedValue == "Others") phNetworkOther.Visible = true;
    }
    protected void ddlHoldingCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        phHoldingCompanyOther.Visible = false;
        if (ddlHoldingCompany.SelectedValue == "Others") phHoldingCompanyOther.Visible = true;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //if (ValidateForm())
        //{
            SaveForm();

            if (Save_Clicked != null) Save_Clicked(this, EventArgs.Empty);
        //}
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lbError.Text = "";
        lbError2.Text = "";
        GeneralFunction.RemoveHighlightControls(this);

        if (Cancel_Clicked != null) Cancel_Clicked(this, EventArgs.Empty);
    }

    protected void ddlContactType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string contactType = ddlContactType.SelectedValue;
        int No = Convert.ToInt32(hldNo.Value);

        ShowCompanyType(No,contactType);
    }
    #endregion

}