
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;
using System.Data;
using System.Configuration;
using System.IO;
using Telerik.Web.UI;

public partial class Main_Jury : PageSecurity_Admin
{
    Jury jury;
    int itemsToShow { get; set; }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["juryId"] != null && Request.QueryString["juryId"] != "")
            jury = Jury.GetJury(new Guid(Request.QueryString["juryId"]));
        else
            jury = Jury.NewJury();

        itemsToShow = Convert.ToInt32(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultListToShow")[0].Value);

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }

        // view mode
        if (Request.QueryString["v"] != null && Request.QueryString["v"] == "1")
        {
            IptechLib.Forms.ChangeStateControls(this, false);
            
            filePhoto.Enabled = false;

            btnSubmit.Visible = false;
            btnEdit.Visible = true;

            btnEdit.Enabled = true;
            btnBack.Enabled = true;            
        }
        else
        {
            btnEdit.Enabled = true;
            lnkChangePassword.Visible = Security.IsRoleSuperAdmin() && !jury.IsNew;
        }

        if (Security.IsRoleReadOnlyAdmin())
        {
            GeneralFunction.DisableAllAction(this, false);
            btnSubmit.Visible = false;
            btnJurySubmitRemarks.Visible = false;
            filePhoto.Enabled = false;
        }
    }

    private void LoadForm()
    {
        // Experience
        List<string> items = GeneralFunction.GetMarketExperienceItems();
        cblMarketExperience.DataSource = items;
        cblMarketExperience.DataBind();

        // Experience Years
        List<string> effieExpYears = GeneralFunction.GetEffieExperienceYears();
        rptEffieExp.DataSource = effieExpYears;
        rptEffieExp.DataBind();

        GeneralFunction.LoadDDLCurrentIndustrySector(ddlCurrentIndustrySector);

        // Programs
        List<string> programItems = GeneralFunction.GetOtherEffieExperienceItems();
        cblstOtherEffieExp.DataSource = programItems;
        cblstOtherEffieExp.DataBind();

        // Industry - filter to unique from categories
        DataTable dt = Category.GetSubcategories("");
        List<string> cats = new List<string>();
        foreach (DataRow row in dt.Rows)
            if (!cats.Contains(row["Name"].ToString())) cats.Add(row["Name"].ToString());

        // filter off unwanted categories
        List<string> cats2 = new List<string>();
        foreach (String item in cats)
        {
            if (item.Trim() != "Asia Pacific Brands" &&
                item.Trim() != "Beverages – Alcohol" &&
                item.Trim() != "Beverages Non-Alcohol" &&
                item.Trim() != "Brand Experience" &&
                item.Trim() != "Brand Revitalisation" &&
                item.Trim() != "David vs Goliath" &&
                item.Trim() != "GoodWorks - Brand" &&
                item.Trim() != "GoodWorks – Non Profit" &&
                item.Trim() != "Media Innovation" &&
                item.Trim() != "New Product or Service" &&
                item.Trim() != "Other Products & Services" &&
                item.Trim() != "Products" &&
                item.Trim() != "Services" &&
                item.Trim() != "Small Budget" &&
                item.Trim() != "Sustained Success")
                cats2.Add(item);
        }
        cats2.Add("Beverages");
        cats2.Add("FMCG");
        cblIndusty.DataSource = cats2.OrderBy(p => p);
        cblIndusty.DataBind();       

        // Country
        ddlCountry.Items.Add(new ListItem("Select Country", ""));
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
        // ddlCountry.Items.Insert(23, new ListItem("-------Rest of the Standard Countries---------", ""));
        ddlCountry.Items[43].Attributes.Add("disabled", "disabled");
        
        // Network
        GeneralFunction.LoadddlNetwork(ddlNetwork, true, true);

        // Holding COmpany
        GeneralFunction.LoadddlHoldingCompany(ddlHoldingCompany, true, true);

        ucGen_UploadFileForProfile.uploadFileMsg = "MS Word fomat (max file size 1mb)";
        ucGen_UploadFileForProfile.fieldName = "Bio (in attachment)";
        ucGen_UploadFileForProfile.fileType = "Word";
        ucGen_UploadFileForProfile.maxSize = 1048576;
        ucGen_UploadFileForProfile.isRequired = true;
        ucGen_UploadFileForProfile.saveDirectory = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\";
        ucGen_UploadFileForProfile.virtualDirectory = ConfigurationManager.AppSettings["storageVirtualPath"] + "JuryProfile/";

        // word count
        txtProfile.Attributes.Add("onkeyup", "cnt();");
    }


    protected List<JuryAPACEffieExperience> GetJuryAPACEffieExperienceList(EffieJuryManagementApp.Jury jury)
    {
        List<JuryAPACEffieExperience> JAPY = new List<JuryAPACEffieExperience>();
        var EffieExpYears = jury.EffieExpYear.Split('|');
        foreach (var item in EffieExpYears)
        {
            try { JAPY.Add(new JuryAPACEffieExperience { Year = item.Split('#')[0], Remarks = item.Split('#')[1] }); }
            catch { }
        }

        List<JuryAPACEffieExperience> JuryAPACEffieExperiences = new List<JuryAPACEffieExperience>();
        List<string> effieExpYears = GeneralFunction.GetEffieExperienceYears();
        foreach (var year in effieExpYears)
        {
            EffieJuryManagementApp.InvitationList invitations = EffieJuryManagementApp.InvitationList.GetInvitationList(jury.Id, year);
            EffieJuryManagementApp.Invitation invitation = invitations.FirstOrDefault();
            if (invitation != null)
            {
                string Remaks = "";
                try { Remaks = JAPY.FirstOrDefault(x => x.Year == year).Remarks; }
                catch { }
                JuryAPACEffieExperience juryAPACEffieExperience = new JuryAPACEffieExperience();
                juryAPACEffieExperience.Year = year;
                juryAPACEffieExperience.InviteR1 = invitation.IsRound1Invited;
                juryAPACEffieExperience.InviteR2 = invitation.IsRound2Invited;
                juryAPACEffieExperience.Decline = invitation.IsDeclined;
                juryAPACEffieExperience.AcceptR1 = invitation.IsRound1Accepted;
                juryAPACEffieExperience.AcceptR2 = invitation.IsRound2Accepted;
                juryAPACEffieExperience.AssignR1 = invitation.IsRound1Assigned;
                juryAPACEffieExperience.AssignR2 = invitation.IsRound2Assigned;
                juryAPACEffieExperience.Remarks = Remaks;
                JuryAPACEffieExperiences.Add(juryAPACEffieExperience);
            }

        }

        return JuryAPACEffieExperiences;
    }

    private void PopulateForm()
    {
        lbTitle.Text = "Add Judge";
        if (!jury.IsNew)
        {
            lbTitle.Text = "Edit Judge";           
        }
        
        // personal
        if (!jury.IsNew)
        {
            lbJuryId.Text = jury.SerialNo;
            //ddlType.Enabled = false;
        }
        if (jury.SerialNo != "") ddlType.SelectedValue = jury.SerialNo.Substring(0, 2);
        ddlSpecialistType.SelectedValue = jury.SpecialistType;
        ddlCurrentIndustrySector.SelectedValue = jury.CurrentIndustrySector;
        ddlType.SelectedValue = jury.Type;
        ddlSalutation.SelectedValue = jury.Salutation;
        txtEmail.Text = jury.Email;
        txtFirstName.Text = jury.FirstName;
        txtLastName.Text = jury.LastName;
        txtJobTitle.Text = jury.Designation;
        txtMobileCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(jury.Mobile);
        txtMobileArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(jury.Mobile);
        txtMobile.Text = GeneralFunction.GetNumberFromContactNumber(jury.Mobile);
        txtContactConutry.Text = GeneralFunction.GetCountryCodeFromContactNumber(jury.Contact);
        txtContactArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(jury.Contact);
        txtContact.Text = GeneralFunction.GetNumberFromContactNumber(jury.Contact);

        txtPAName.Text = jury.PAName;
        txtPAEmail.Text = jury.PAEmail;
        txtPATelCountry.Text = GeneralFunction.GetCountryCodeFromContactNumber(jury.PATel);
        txtPATelArea.Text = GeneralFunction.GetAreaCodeFromContactNumber(jury.PATel);
        txtPATel.Text = GeneralFunction.GetNumberFromContactNumber(jury.PATel);

        if (!jury.IsNew)
        {
            lbPassword.Text = jury.Password;
        }

        // company
        txtCompanyName.Text = jury.Company;
        txtAddress1.Text = jury.Address1;
        txtAddress2.Text = jury.Address2;
        txtCity.Text = jury.City;
        txtPostalCode.Text = jury.Postal;
        ddlCountry.SelectedValue = jury.Country;
        ddlCompanyType.SelectedValue = jury.CompanyType;

        try { ddlNetwork.SelectedValue = jury.Network; }
        catch { ddlNetwork.SelectedValue = ""; }

        try { ddlHoldingCompany.SelectedValue = jury.HoldingCompany; }
        catch { ddlHoldingCompany.SelectedValue = ""; }
        
        txtCompanyTypeOther.Text = jury.CompanyTypeOther;
        txtNetworkOther.Text = jury.NetworkOthers;
        txtHoldingCompanyOther.Text = jury.HoldingCompanyOthers;

        // Experience Years
        #region Experience Years
        rtpJuryYear.DataSource = GetJuryAPACEffieExperienceList(jury);
        rtpJuryYear.DataBind();

        #endregion


        // experience
        IptechLib.Forms.AssignValueCheckBoxList(cblstOtherEffieExp, jury.EffieExpProgram, "|");
        txtOtherEffieExpOthers.Text = jury.EffieExpProgramOthers;
        IptechLib.Forms.AssignValueCheckBoxList(cblMarketExperience, jury.MarketExp, "|");
        txtMarketExperienceOthers.Text = jury.MarketExpOthers;
        IptechLib.Forms.AssignValueCheckBoxList(cblIndusty, jury.IndustryExp, "|");
        txtIndustryExperienceOthers.Text = jury.IndustryExpOthers;
        //IptechLib.Forms.AssignValueCheckBoxList(cblstEffieExp, jury.EffieExpYear, "|");
        txtOtherExp.Text = jury.OtherJudgingExp;
        IptechLib.Forms.AssignValueCheckBoxList(cblSkills, jury.Skills, "|");
        txtSkillsOthers.Text = jury.SkillsOthers;
        txtRevelantExp.Text = jury.RevelantExp;

        for (int i = 0; i <= rptEffieExp.Items.Count - 1; i++)
        {
            CheckBox chkYear = (CheckBox)rptEffieExp.Items[i].FindControl("chkYear");
            TextBox txtYearRemarks = (TextBox)rptEffieExp.Items[i].FindControl("txtYearRemarks");

            string yearString = string.Empty;
            string year = string.Empty;
            string yearRemark = string.Empty;

            try
            {
                yearString = jury.EffieExpYear.Split('|')[i];
                year = yearString.Split('#')[0];
                yearRemark = yearString.Split('#')[1];
            }
            catch { }

            chkYear.Checked = year.Equals(chkYear.Text);

            txtYearRemarks.Text = yearRemark;

            //txtYearRemarks.Enabled = chkYear.Checked;
        }

        // photo
        string photo = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id.ToString() + ".jpg";
        if (File.Exists(photo))
        {
            imgPhoto.ImageUrl = ConfigurationManager.AppSettings["storageVirtualPath"] + "JuryPhoto//" + jury.Id.ToString() + ".jpg?" + DateTime.Now.Ticks.ToString();
            imgPhoto.Visible = true;
        }

        txtProfile.Text = jury.Profile;

        //other
        txtSource.Text = jury.Source;
        txtRemarks.Text = jury.Remarks;
        txtReference.Text = jury.Reference;


        if (File.Exists(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo + ".doc"))
            ucGen_UploadFileForProfile.SetValue(jury.SerialNo + ".doc");
        else if (File.Exists(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo + ".docx"))
            ucGen_UploadFileForProfile.SetValue(jury.SerialNo + ".docx");

        chkRecieveUpdate.Checked = jury.IsReceiveUpdate;

        //ddlNetwork_OnSelectedIndexChanged(null, null);
        //ddlHoldingCompany_OnSelectedIndexChanged(null, null);
        //CheckCompanyTypeStatus(jury.CompanyType);

        bool isViewMode = false;

        if (Request.QueryString["v"] != null)
        {
            isViewMode = (Request.QueryString["v"] == "1");
        }

        PopulateCompanyInformation(isViewMode);

        BindCompanyHistory(true, itemsToShow);

        // admin remarks
        BindJuryRemarks();
    }

    public void BindCompanyHistory(bool needRebind, int itemsToShow)
    {
        int itemstoDisplay = Convert.ToInt32(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultListToShow")[0].Value);

        CompanyHistoryList companyHistryList = CompanyHistoryList.GetCompanyHistoryList(jury.Id);

        if (itemsToShow > itemstoDisplay)
        {
            radCompanyList.DataSource = companyHistryList.Take(itemsToShow);
        }
        else
            radCompanyList.DataSource = companyHistryList.Take(itemstoDisplay);

        if (needRebind) radCompanyList.Rebind();



        lnkSHowMore.Visible = companyHistryList.Count > itemstoDisplay && companyHistryList.Count > itemsToShow;
        lnkSHowLess.Visible = radCompanyList.Items.Count > itemstoDisplay;



    }

    private bool ValidateForm()
    {
        lbError.Text = "";
        lbError2.Text = "";
        GeneralFunction.RemoveHighlightControls(this);

        lbError.Text += GeneralFunction.ValidateDropDownList("Type", ddlType, true, "");
        lbError.Text += GeneralFunction.ValidateDropDownList("Specialist Type", ddlSpecialistType, false, "");
        lbError.Text += GeneralFunction.ValidateDropDownList("Current Industry Sector", ddlCurrentIndustrySector, false, "");
        
        lbError.Text += GeneralFunction.ValidateDropDownList("Salutation", ddlSalutation, true, "");
        lbError.Text += GeneralFunction.ValidateTextBox("First Name", txtFirstName, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Last Name", txtLastName, true, "string");
        lbError.Text += GeneralFunction.ValidateTextBox("Job Title", txtJobTitle, false, "string");

        lbError.Text += GeneralFunction.ValidateTextBox("Contact Country Code", txtContactConutry, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact Area Code", txtContactArea, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Contact", txtContact, false, "phoneNumber");

        lbError.Text += GeneralFunction.ValidateTextBox("Mobile Country Code", txtMobileCountry, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Mobile Area Code", txtMobileArea, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("Mobile", txtMobile, false, "phoneNumber");

        lbError.Text += GeneralFunction.ValidateTextBox("Email", txtEmail, true, "EmailSingle");

        if (String.IsNullOrEmpty(GeneralFunction.ValidateTextBox("Email", txtEmail, true, "EmailSingle")) && String.IsNullOrEmpty(GeneralFunction.ValidateTextBox("First Name", txtFirstName, true, "string")) &&
            String.IsNullOrEmpty(GeneralFunction.ValidateTextBox("Last Name", txtLastName, true, "string")))
        {
            if (EffieJuryManagementApp.JuryList.CheckIfRecordExists(txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtEmail.Text.Trim(), jury.IsNew, jury.Id))
            {
                lbError.Text += "First Name & Last Name / Email already exists.<br/>";
            }
        }

        //lbError.Text += GeneralFunction.ValidateTextBox("PA Email", txtPAEmail, true, "EmailSingle");
        lbError.Text += GeneralFunction.ValidateTextBox("PA Tel Country Code", txtPATelCountry, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("PA Tel  Area Code", txtPATelArea, false, "phoneNumber");
        lbError.Text += GeneralFunction.ValidateTextBox("PA Tel", txtPATel, false, "phoneNumber");
       
        #region Validating and Saving Photo

        string photo = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id.ToString() + ".jpg";

        lbError.Text += GeneralFunction.ValidateFileUpload("Photo", filePhoto, false, "Image", 1048576);
        if (String.IsNullOrEmpty(GeneralFunction.ValidateFileUpload("Photo", filePhoto, false, "Image", 1048576)) && filePhoto.HasFile)
        {
            if (!jury.IsNew)
            {
                filePhoto.SaveAs(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto//" + jury.Id.ToString() + ".jpg");
                imgPhoto.ImageUrl = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto//" + jury.Id.ToString() + ".jpg?" + DateTime.Now.Ticks.ToString();
                imgPhoto.Visible = true;
            }
            else
            {
                ViewState["JuryPhoto"] = DateTime.Now.Ticks.ToString();

                filePhoto.SaveAs(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + ViewState["JuryPhoto"].ToString() + ".jpg");
                imgPhoto.ImageUrl = ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto//" + ViewState["JuryPhoto"].ToString() + ".jpg?" + DateTime.Now.Ticks.ToString();
                imgPhoto.Visible = true;
            }
        }

        # endregion

        #region Validating and Saving Bio
        ucGen_UploadFileForProfile.isRequired = false;
        lbError.Text += ucGen_UploadFileForProfile.ValidateForm();

        if (String.IsNullOrEmpty(ucGen_UploadFileForProfile.ValidateForm()) && ucGen_UploadFileForProfile.HasFile())
        {
            if (!jury.IsNew)
            {
                ucGen_UploadFileForProfile.SaveAndUpdateFile(jury.SerialNo);

                File.Copy(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(jury.SerialNo), ucGen_UploadFileForProfile.saveDirectory + jury.SerialNo + "." + ucGen_UploadFileForProfile.GetValueExtension(jury.SerialNo), true);

                if (File.Exists(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(jury.SerialNo)))
                    GeneralFunction.DeleteFile(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(jury.SerialNo), false);

                ucGen_UploadFileForProfile.SetValue(jury.SerialNo + "." + ucGen_UploadFileForProfile.GetValueExtension(jury.SerialNo));
            }
            else
            {
                ViewState["JuryProfile"] = DateTime.Now.Ticks.ToString();

                ucGen_UploadFileForProfile.SaveAndUpdateFile(ViewState["JuryProfile"].ToString());

                File.Copy(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(ViewState["JuryProfile"].ToString()), ucGen_UploadFileForProfile.saveDirectory + ViewState["JuryProfile"].ToString() + "." + ucGen_UploadFileForProfile.GetValueExtension(ViewState["JuryProfile"].ToString()), true);

                if (File.Exists(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(ViewState["JuryProfile"].ToString())))
                    GeneralFunction.DeleteFile(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(ViewState["JuryProfile"].ToString()), false);

                ucGen_UploadFileForProfile.SetValue(ViewState["JuryProfile"].ToString() + "." + ucGen_UploadFileForProfile.GetValueExtension(ViewState["JuryProfile"].ToString()));
            }
        }
        # endregion

        lbError.Text += GeneralFunction.ValidateTextBox("Bio (in words)", txtProfile, false, "string");

        // profile word count
        if (!String.IsNullOrEmpty(hldProfileCount.Value))
        {
            try
            {
                int pcount = int.Parse(hldProfileCount.Value);
            }
            catch
            {
                lbError.Text += "Bio (in words) has exceeded the 350-word maximum.<br />";
            }
        }

        lbError.Text += GeneralFunction.ValidateTextBox("Company Name", txtCompanyName, false, "string");

        lbError.Text += GeneralFunction.ValidateDropDownList("Country", ddlCountry, false, "");


        lbError.Text += GeneralFunction.ValidateDropDownList("Type of Company", ddlCompanyType, false, "");
        //lbError.Text += GeneralFunction.ValidateDropDownList("Network", ddlNetwork, !ddlCompanyType.SelectedValue.Equals("Client"), "");
        lbError.Text += GeneralFunction.ValidateDropDownList("Network", ddlNetwork, false, "");
        if (ddlNetwork.SelectedValue == "Others")
            lbError.Text += GeneralFunction.ValidateTextBox("Network - Others", txtNetworkOther, !ddlCompanyType.SelectedValue.Equals("Client"), "string");
        //lbError.Text += GeneralFunction.ValidateDropDownList("Holding Company", ddlHoldingCompany, !ddlCompanyType.SelectedValue.Equals("Client"), "");
        lbError.Text += GeneralFunction.ValidateDropDownList("Holding Company", ddlHoldingCompany, false, "");
        if (ddlHoldingCompany.SelectedValue == "Others")
            lbError.Text += GeneralFunction.ValidateTextBox("Holding Company - Others", txtHoldingCompanyOther, !ddlCompanyType.SelectedValue.Equals("Client"), "string");

        //lbError.Text += GeneralFunction.ValidateCheckBoxList("Effie Experience", cblstEffieExp, false);

        lbError2.Text = lbError.Text;

        PopulateCompanyInformation(false);

        return (lbError.Text == "");
    }

    private bool SaveForm()
    {
        if (!IsSameCompanyDetails() || jury.IsNew)
            SaveCompanyHistory();

        jury.Email = txtEmail.Text;
        jury.Type = ddlType.SelectedValue;
        jury.SpecialistType = ddlSpecialistType.SelectedValue;
        jury.CurrentIndustrySector = ddlCurrentIndustrySector.SelectedValue;
        jury.Salutation = ddlSalutation.SelectedValue;
        jury.FirstName = txtFirstName.Text;
        jury.LastName = txtLastName.Text;
        jury.Designation = txtJobTitle.Text;
        jury.Mobile = txtMobileCountry.Text.Trim() + "|" + txtMobileArea.Text.Trim() + "|" + txtMobile.Text.Trim();
        jury.Contact = txtContactConutry.Text.Trim() + "|" + txtContactArea.Text.Trim() + "|" + txtContact.Text.Trim();
        jury.Company = txtCompanyName.Text;
        jury.Address1 = txtAddress1.Text;
        jury.Address2 = txtAddress2.Text;
        jury.City = txtCity.Text;
        jury.Postal = txtPostalCode.Text;
        jury.Country = ddlCountry.SelectedValue;
        jury.DateModifiedString = DateTime.Now.ToString();

        jury.Profile = txtProfile.Text;

        jury.PAName = txtPAName.Text;
        jury.PAEmail = txtPAEmail.Text;
        jury.PATel = txtPATelCountry.Text.Trim() + "|" + txtPATelArea.Text.Trim() + "|" + txtPATel.Text.Trim();

        jury.CompanyType = ddlCompanyType.SelectedValue;
        jury.CompanyTypeOther = txtCompanyTypeOther.Text;

        jury.Network = ddlNetwork.SelectedValue;
        if (jury.Network == "Others") jury.NetworkOthers = txtNetworkOther.Text.Trim();

        jury.HoldingCompany = ddlHoldingCompany.SelectedValue;
        if (jury.HoldingCompany == "Others") jury.HoldingCompanyOthers = txtHoldingCompanyOther.Text.Trim();



        jury.MarketExp = GeneralFunction.GetValueCheckBoxList(cblMarketExperience, "|");
        jury.MarketExpOthers = txtMarketExperienceOthers.Text.Trim();
        jury.IndustryExp = GeneralFunction.GetValueCheckBoxList(cblIndusty, "|");
        jury.IndustryExpOthers = txtIndustryExperienceOthers.Text.Trim();

        //jury.EffieExpYear = IptechLib.Forms.GetValueCheckBoxList(cblstEffieExp,"|");

        #region Old Effie Exp
        //string effieExpYear = string.Empty;

        //foreach (RepeaterItem item in rptEffieExp.Items)
        //{
        //    CheckBox chkYear = (CheckBox)item.FindControl("chkYear");
        //    TextBox txtYearRemarks = (TextBox)item.FindControl("txtYearRemarks");

        //    if (chkYear.Checked)
        //    {
        //        effieExpYear += chkYear.Text;
        //    }

        //    if (!String.IsNullOrEmpty(txtYearRemarks.Text.Trim()))
        //        effieExpYear += "#" + txtYearRemarks.Text;

        //    effieExpYear += "|";
        //}
        #endregion

        string effieExpYear = string.Empty;

        foreach (RepeaterItem item in rtpJuryYear.Items)
        {
            Label lblYear = (Label)item.FindControl("lblYear");
            TextBox txtYearJuryRemarks = (TextBox)item.FindControl("txtYearJuryRemarks");

            effieExpYear += lblYear.Text;

            if (!String.IsNullOrEmpty(txtYearJuryRemarks.Text.Trim()))
                effieExpYear += "#" + txtYearJuryRemarks.Text;

            effieExpYear += "|";
        }

        jury.EffieExpYear = effieExpYear;

        jury.EffieExpProgram = IptechLib.Forms.GetValueCheckBoxList(cblstOtherEffieExp, "|");
        jury.EffieExpProgramOthers = txtOtherEffieExpOthers.Text;
        jury.OtherJudgingExp = txtOtherExp.Text;
        jury.Skills = GeneralFunction.GetValueCheckBoxList(cblSkills, "|");
        jury.SkillsOthers = txtSkillsOthers.Text;
        jury.RevelantExp = txtRevelantExp.Text;

        jury.Source = txtSource.Text;
        jury.Remarks = txtRemarks.Text;
        jury.Reference = txtReference.Text;

        //ucGen_UploadFileForProfile.SaveAndUpdateFile(jury.SerialNo);

        //if (ucGen_UploadFileForProfile.HasFile())
        //{
        //    File.Copy(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(jury.SerialNo), ucGen_UploadFileForProfile.saveDirectory + jury.SerialNo + "." + ucGen_UploadFileForProfile.GetValueExtension(jury.SerialNo), true);

        //    if (File.Exists(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(jury.SerialNo)))
        //        GeneralFunction.DeleteFile(ucGen_UploadFileForProfile.saveDirectory + ucGen_UploadFileForProfile.GetValue(jury.SerialNo), false);
        //}
        jury.IsReceiveUpdate = chkRecieveUpdate.Checked;

        if (jury.IsNew)
        {
            jury.IsFirstTimeLogin = true;
            jury.IsActive = true;
            if (String.IsNullOrEmpty(jury.SerialNo)) jury.SerialNo = JuryList.GetNewJuryId();
            jury.DateCreatedString = DateTime.Now.ToString();
            if (String.IsNullOrEmpty(jury.Password)) jury.Password = Guid.NewGuid().ToString().Substring(0, 6);

            if (ViewState["JuryPhoto"] != null)
            {
                if (File.Exists(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + ViewState["JuryPhoto"].ToString() + ".jpg"))
                {
                    File.Move(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + ViewState["JuryPhoto"].ToString() + ".jpg", ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id + ".jpg");
                }
            }
            if (ViewState["JuryProfile"] != null)
            {

                if (File.Exists(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + ViewState["JuryProfile"].ToString() + ".doc"))
                {
                    File.Move(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + ViewState["JuryProfile"].ToString() + ".doc", ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo + ".doc");
                }
                else if (File.Exists(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + ViewState["JuryProfile"].ToString() + ".docx"))
                {
                    File.Move(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + ViewState["JuryProfile"].ToString() + ".docx", ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryProfile\\" + jury.SerialNo + ".docx");
                }
            }
        }       

        if (jury.IsValid)
        {
            jury = jury.Save();

            ////save photo
            //if (filePhoto.HasFile && filePhoto.PostedFile.ContentType == "image/jpeg")
            //{
            //    filePhoto.SaveAs(ConfigurationManager.AppSettings["storagePhysicalPath"] + "JuryPhoto\\" + jury.Id.ToString() + ".jpg");
            //}

            return true;
        }
        else
        {
            return false;
        }

    }

    public void SaveCompanyHistory()
    {
        CompanyHistory compnyHistroy = CompanyHistory.NewCompanyHistory();

        compnyHistroy.JuryId = jury.Id;
        compnyHistroy.Type = ddlType.SelectedValue; // jury.Type;
        compnyHistroy.Designation = txtJobTitle.Text;// jury.Designation;
        compnyHistroy.Company = txtCompanyName.Text;// jury.Company;
        compnyHistroy.Address1 = txtAddress1.Text;// jury.Address1;
        compnyHistroy.Address2 = txtAddress2.Text;////jury.Address2;
        compnyHistroy.City = txtCity.Text;// jury.City;
        compnyHistroy.Postal = txtPostalCode.Text;// jury.Postal;
        compnyHistroy.Country = ddlCountry.SelectedValue;// jury.Country;
        compnyHistroy.DateModifiedString = DateTime.Now.ToString();

        compnyHistroy.CompanyType = ddlCompanyType.SelectedValue; //jury.CompanyType;
        compnyHistroy.CompanyTypeOther = txtCompanyTypeOther.Text;// jury.CompanyTypeOther;

        compnyHistroy.Network = ddlNetwork.SelectedValue;// jury.Network;
        if (compnyHistroy.Network == "Others") compnyHistroy.NetworkOthers = txtNetworkOther.Text.Trim();// jury.NetworkOthers.Trim();

        compnyHistroy.HoldingCompany = ddlHoldingCompany.SelectedValue;// jury.HoldingCompany;
        if (compnyHistroy.HoldingCompany == "Others") compnyHistroy.HoldingCompanyOthers = txtHoldingCompanyOther.Text;// jury.HoldingCompanyOthers.Trim();

        if (compnyHistroy.IsNew)
            compnyHistroy.DateCreatedString = DateTime.Now.ToString();

        if (compnyHistroy.IsValid)
            compnyHistroy.Save();
    }

    private void BindJuryRemarks()
    {
        lbJuryRemarks.Text = "";
        JuryRemarksList remarksList = JuryRemarksList.GetJuryRemarksList(jury.Id);
        foreach (JuryRemarks remarks in remarksList)
            lbJuryRemarks.Text += "<tr><td width='100px' style='vertical-align:top'>" + remarks.DateCreated.ToString("dd/MM/yy hh:mm tt") + " : </td><td>" + remarks.Remarks + "</td></tr>";
        if (lbJuryRemarks.Text == "")
            lbJuryRemarks.Text = "None";
        else
            lbJuryRemarks.Text = "<table width='100%' style='font-size:10px;'>" + lbJuryRemarks.Text + "</table>";
    }

    #region Events

    //protected void chkYear_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox chkYear = (CheckBox)sender;

    //    if (chkYear != null)
    //    {
    //        TextBox txtYearRemarks = (TextBox)chkYear.NamingContainer.FindControl("txtYearRemarks");

    //        if (txtYearRemarks != null)
    //        {
    //            //if (!chkYear.Checked)
    //            //    txtYearRemarks.Text = string.Empty;

    //            //txtYearRemarks.Enabled = chkYear.Checked;
    //        }
    //    }
    //}

    protected void rtpJuryYear_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            JuryAPACEffieExperience juryAPACEffieExperiences = (JuryAPACEffieExperience)e.Item.DataItem;
            if (juryAPACEffieExperiences != null)
            {
                Label lblYear = (Label)e.Item.FindControl("lblYear");
                lblYear.Text = juryAPACEffieExperiences.Year.ToString();

                CheckBox InviteR1 = (CheckBox)e.Item.FindControl("InviteR1");
                InviteR1.Checked = bool.Parse(juryAPACEffieExperiences.InviteR1.ToString());
                InviteR1.Enabled = false;

                CheckBox InviteR2 = (CheckBox)e.Item.FindControl("InviteR2");
                InviteR2.Checked = bool.Parse(juryAPACEffieExperiences.InviteR2.ToString());
                InviteR2.Enabled = false;

                CheckBox Decline = (CheckBox)e.Item.FindControl("Decline");
                Decline.Checked = bool.Parse(juryAPACEffieExperiences.Decline.ToString());
                Decline.Enabled = false;

                CheckBox AcceptR1 = (CheckBox)e.Item.FindControl("AcceptR1");
                AcceptR1.Checked = bool.Parse(juryAPACEffieExperiences.AcceptR1.ToString());
                AcceptR1.Enabled = false;

                CheckBox AcceptR2 = (CheckBox)e.Item.FindControl("AcceptR2");
                AcceptR2.Checked = bool.Parse(juryAPACEffieExperiences.AcceptR2.ToString());
                AcceptR2.Enabled = false;

                CheckBox AssignR1 = (CheckBox)e.Item.FindControl("AssignR1");
                AssignR1.Checked = bool.Parse(juryAPACEffieExperiences.AssignR1.ToString());
                AssignR1.Enabled = false;

                CheckBox AssignR2 = (CheckBox)e.Item.FindControl("AssignR2");
                AssignR2.Checked = bool.Parse(juryAPACEffieExperiences.AssignR2.ToString());
                AssignR2.Enabled = false;

                TextBox Remaks = (TextBox)e.Item.FindControl("txtYearJuryRemarks");
                Remaks.Text = juryAPACEffieExperiences.Remarks;
            }
        }
    }
	
    protected void rptEffieExp_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string year = (string)e.Item.DataItem;
            if (year != null)
            {
                CheckBox chkYear = (CheckBox)e.Item.FindControl("chkYear");
                chkYear.Text = year.ToString();

                //TextBox txtYearRemarks = (TextBox)e.Item.FindControl("txtYearRemarks");
                //txtYearRemarks.Enabled = chkYear.Checked;
            }
        }
    }
    
    //protected void ddlNetwork_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    phNetworkOther.Visible = false;
    //    if (ddlNetwork.SelectedValue == "Others") phNetworkOther.Visible = true;
    //}

    //protected void ddlHoldingCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    phHoldingCompanyOther.Visible = false;
    //    if (ddlHoldingCompany.SelectedValue == "Others") phHoldingCompanyOther.Visible = true;
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateForm())
        {
            if (SaveForm())
                Response.Redirect("../Main/JuryList.aspx");
        }
    }    

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(GeneralFunction.GetRedirect("../Main/JuryList.aspx"));
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Jury.aspx?juryId=" + jury.Id.ToString());
    }

    protected void radCompanyList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            CompanyHistory companyHistory = (CompanyHistory)e.Item.DataItem;

            HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");

            hdfId.Value = companyHistory.Id.ToString();

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
            lnkDelete.CommandArgument = companyHistory.Id.ToString();
            
            if (Security.IsRoleReadOnlyAdmin())
            {
                lnkDelete.Enabled = false;
                lnkDelete.Visible = false;
            }
        }
    }

    protected void radCompanyList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("delete"))
        {
            CompanyHistory.DeleteCompanyHistory(new Guid(e.CommandArgument.ToString()));
        }
        BindCompanyHistory(true,radCompanyList.Items.Count);
    }

    protected void radCompanyList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCompanyHistory(false, itemsToShow);
    }

    //protected void ddlCompanyType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    CheckCompanyTypeStatus(ddlCompanyType.SelectedValue);
    //}

    protected void lnkSHowMore_Click(object sender, EventArgs e)
    {
        LinkButton lnkClicked = (LinkButton)sender;

        int itemstoDisplay = Convert.ToInt32(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultListToShow")[0].Value);

        if (lnkClicked.CommandArgument.Equals("add"))
            itemsToShow = radCompanyList.Items.Count + itemstoDisplay;
        else
            itemsToShow = radCompanyList.Items.Count - itemstoDisplay;

        BindCompanyHistory(true, itemsToShow);
    }

    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
        pnlPassword.Visible = false;
        pnlChangePassword.Visible = true;
        lblErrorPwd.Visible = false;
        txtPassword.Text = lbPassword.Text;
    }

    protected void lnkPasswordSubmit_Click(object sender, EventArgs e)
    {
        if (jury != null)
        {
            jury.Password = txtPassword.Text.Trim();
            jury.DateModifiedString = DateTime.Now.ToString();
            jury.Save();

            lblErrorPwd.Text = "Password updated successfully.<br/>";
            lblErrorPwd.Visible = true;

            lbPassword.Text = jury.Password;
        }
        pnlPassword.Visible = true;
        pnlChangePassword.Visible = false;
    }

    protected void lnkCancelPassword_Click(object sender, EventArgs e)
    {
        lblErrorPwd.Visible = false;
        pnlPassword.Visible = true;
        pnlChangePassword.Visible = false;
    }

    protected void btnJurySubmitRemarks_Click(object sender, EventArgs e)
    {
        if (txtRemarks.Text.Trim() != "")
        {
            JuryRemarks remarks = JuryRemarks.NewJuryRemarks();
            remarks.Remarks = txtRemarks.Text;
            remarks.JuryId = jury.Id;
            remarks.DateCreatedString = DateTime.Now.ToString();
            remarks.Save();

            txtRemarks.Text = "";
            BindJuryRemarks();
        }
    }

    #endregion

    #region Helper

    public bool IsSameCompanyDetails()
    {
        //if ((jury.Type != ddlType.SelectedValue) || (!jury.Designation.Trim().ToUpper().Equals(txtJobTitle.Text.Trim().ToUpper())) || (!jury.Company.Trim().ToUpper().Equals(txtCompanyName.Text.Trim().ToUpper()))
        //  || (!jury.Address1.Trim().ToUpper().Equals(txtAddress1.Text.Trim().ToUpper())) || (!jury.Address2.Trim().ToUpper().Equals(txtAddress2.Text.Trim().ToUpper())) || (!jury.City.Trim().ToUpper().Equals(txtCity.Text.Trim().ToUpper()))
        //  || (!jury.Postal.Trim().ToUpper().Equals(txtPostalCode.Text.Trim().ToUpper())) || (!jury.Country.Trim().ToUpper().Equals(ddlCountry.SelectedValue.Trim().ToUpper())) || (!jury.CompanyType.Trim().ToUpper().Equals(ddlCompanyType.SelectedValue.Trim().ToUpper()))
        //  || (!jury.CompanyTypeOther.Trim().ToUpper().Equals(txtCompanyTypeOther.Text.Trim().ToUpper())) || (!jury.Network.Trim().ToUpper().Equals(ddlNetwork.SelectedValue.Trim().ToUpper())) || (!jury.NetworkOthers.Trim().ToUpper().Equals(txtNetworkOther.Text.Trim().ToUpper()))
        //  || (!jury.HoldingCompany.Trim().ToUpper().Equals(ddlHoldingCompany.SelectedValue.Trim().ToUpper())) || (!jury.HoldingCompanyOthers.Trim().ToUpper().Equals(txtHoldingCompanyOther.Text.Trim().ToUpper())))
        

        if ((!jury.Company.Trim().ToUpper().Equals(txtCompanyName.Text.Trim().ToUpper())) ||
            (!jury.Designation.Trim().ToUpper().Equals(txtJobTitle.Text.Trim().ToUpper())) ||
            (!jury.Country.Trim().ToUpper().Equals(ddlCountry.SelectedValue.Trim().ToUpper())))
        {
            return false;
        }


        return true;
    }

    //public void CheckCompanyTypeStatus(string companyType)
    //{
    //    if (companyType.Trim().Equals("Client"))
    //    {
    //        ddlNetwork.SelectedValue = ddlHoldingCompany.SelectedValue = "";

    //        ddlNetwork.Enabled = ddlHoldingCompany.Enabled = false;
    //    }
    //    else
    //        ddlNetwork.Enabled = ddlHoldingCompany.Enabled = true;
    //}

    public void PopulateCompanyInformation(bool viewMode)
    {
        phCompanyTypeOther.Style.Clear();
        phHoldingCompanyOther.Style.Clear();
        phNetworkOther.Style.Clear();

        if (ddlCompanyType.SelectedValue.Equals("Client"))
        {
            phCompanyTypeOther.Style.Add("display", "none");
            ddlNetwork.Enabled = viewMode ? false : true;
            ddlHoldingCompany.Enabled = viewMode ? false : true;
        }
        else if (ddlCompanyType.SelectedValue.Equals("Others"))
        {
            phCompanyTypeOther.Style.Add("display", "inline");
            ddlNetwork.Enabled = viewMode ? false : true;
            ddlHoldingCompany.Enabled = viewMode ? false : true;
        }
        else
        {
            phCompanyTypeOther.Style.Add("display", "none");
            ddlNetwork.Enabled = viewMode ? false : true;
            ddlHoldingCompany.Enabled = viewMode ? false : true;
        }

        if (ddlNetwork.SelectedValue.Equals("Others"))
        {
            phNetworkOther.Style.Add("display", "inline");
        }
        else
        {
            phNetworkOther.Style.Add("display", "none");
        }

        if (ddlHoldingCompany.SelectedValue.Equals("Others"))
        {
            phHoldingCompanyOther.Style.Add("display", "inline");
        }
        else
        {
            phHoldingCompanyOther.Style.Add("display", "none");
        }
    }

    public class JuryAPACEffieExperience
    {
        public string Year { get; set; }
        public bool? InviteR1 { get; set; }
        public bool? InviteR2 { get; set; }
        public bool? Decline { get; set; }
        public bool? AcceptR1 { get; set; }
        public bool? AcceptR2 { get; set; }
        public bool? AssignR1 { get; set; }
        public bool? AssignR2 { get; set; }
        public string Remarks { get; set; }
    }
    #endregion

}