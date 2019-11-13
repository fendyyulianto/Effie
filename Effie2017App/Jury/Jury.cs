using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{
    [Serializable()]
    public class Jury : Csla.BusinessBase<Jury>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _juryId = string.Empty;
        private string _round1PanelId = string.Empty;
        private string _round2PanelId = string.Empty;
        private string _password = string.Empty;
        private string _salutation = string.Empty;
        private string _firstname = string.Empty;
        private string _lastname = string.Empty;
        private string _job = string.Empty;
        private string _contact = string.Empty;
        private string _mobile = string.Empty;
        private string _email = string.Empty;
        private string _company = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private string _city = string.Empty;
        private string _postal = string.Empty;
        private string _country = string.Empty;
        private string _website = string.Empty;
        private string _companyType = string.Empty;
        private string _companyTypeOther = string.Empty;
        private string _network = string.Empty;
        private string _networkOthers = string.Empty;
        private string _holdingCompany = string.Empty;
        private string _holdingCompanyOthers = string.Empty;
        private string _marketExp = string.Empty;
        private string _marketExpOthers = string.Empty;
        private string _industryExp = string.Empty;
        private string _industryExpOthers = string.Empty;
        private string _effieExp = string.Empty;
        private string _effieExpProgram = string.Empty;
        private string _effieExpYear = string.Empty;
        private string _otherExp = string.Empty;
        private string _skills = string.Empty;
        private string _skillsOthers = string.Empty;
        private string _revelantExp = string.Empty;
        private bool _isFirstTimeLogin = true;
        private SmartDate _dateLastReminded = new SmartDate(false);
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public string JuryId
        {
            get
            {
                CanReadProperty("JuryId", true);
                return _juryId;
            }
            set
            {
                CanWriteProperty("JuryId", true);
                if (value == null) value = string.Empty;
                if (!_juryId.Equals(value))
                {
                    _juryId = value;
                    PropertyHasChanged("JuryId");
                }
            }
        }

        public string Round1PanelId
        {
            get
            {
                CanReadProperty("Round1PanelId", true);
                return _round1PanelId;
            }
            set
            {
                CanWriteProperty("Round1PanelId", true);
                if (value == null) value = string.Empty;
                if (!_round1PanelId.Equals(value))
                {
                    _round1PanelId = value;
                    PropertyHasChanged("Round1PanelId");
                }
            }
        }

        public string Round2PanelId
        {
            get
            {
                CanReadProperty("Round2PanelId", true);
                return _round2PanelId;
            }
            set
            {
                CanWriteProperty("Round2PanelId", true);
                if (value == null) value = string.Empty;
                if (!_round2PanelId.Equals(value))
                {
                    _round2PanelId = value;
                    PropertyHasChanged("Round2PanelId");
                }
            }
        }

        public string Password
        {
            get
            {
                CanReadProperty("Password", true);
                return _password;
            }
            set
            {
                CanWriteProperty("Password", true);
                if (value == null) value = string.Empty;
                if (!_password.Equals(value))
                {
                    _password = value;
                    PropertyHasChanged("Password");
                }
            }
        }

        public string Salutation
        {
            get
            {
                CanReadProperty("Salutation", true);
                return _salutation;
            }
            set
            {
                CanWriteProperty("Salutation", true);
                if (value == null) value = string.Empty;
                if (!_salutation.Equals(value))
                {
                    _salutation = value;
                    PropertyHasChanged("Salutation");
                }
            }
        }

        public string Firstname
        {
            get
            {
                CanReadProperty("Firstname", true);
                return _firstname;
            }
            set
            {
                CanWriteProperty("Firstname", true);
                if (value == null) value = string.Empty;
                if (!_firstname.Equals(value))
                {
                    _firstname = value;
                    PropertyHasChanged("Firstname");
                }
            }
        }

        public string Lastname
        {
            get
            {
                CanReadProperty("Lastname", true);
                return _lastname;
            }
            set
            {
                CanWriteProperty("Lastname", true);
                if (value == null) value = string.Empty;
                if (!_lastname.Equals(value))
                {
                    _lastname = value;
                    PropertyHasChanged("Lastname");
                }
            }
        }

        public string Job
        {
            get
            {
                CanReadProperty("Job", true);
                return _job;
            }
            set
            {
                CanWriteProperty("Job", true);
                if (value == null) value = string.Empty;
                if (!_job.Equals(value))
                {
                    _job = value;
                    PropertyHasChanged("Job");
                }
            }
        }

        public string Contact
        {
            get
            {
                CanReadProperty("Contact", true);
                return _contact;
            }
            set
            {
                CanWriteProperty("Contact", true);
                if (value == null) value = string.Empty;
                if (!_contact.Equals(value))
                {
                    _contact = value;
                    PropertyHasChanged("Contact");
                }
            }
        }

        public string Mobile
        {
            get
            {
                CanReadProperty("Mobile", true);
                return _mobile;
            }
            set
            {
                CanWriteProperty("Mobile", true);
                if (value == null) value = string.Empty;
                if (!_mobile.Equals(value))
                {
                    _mobile = value;
                    PropertyHasChanged("Mobile");
                }
            }
        }

        public string Email
        {
            get
            {
                CanReadProperty("Email", true);
                return _email;
            }
            set
            {
                CanWriteProperty("Email", true);
                if (value == null) value = string.Empty;
                if (!_email.Equals(value))
                {
                    _email = value;
                    PropertyHasChanged("Email");
                }
            }
        }

        public string Company
        {
            get
            {
                CanReadProperty("Company", true);
                return _company;
            }
            set
            {
                CanWriteProperty("Company", true);
                if (value == null) value = string.Empty;
                if (!_company.Equals(value))
                {
                    _company = value;
                    PropertyHasChanged("Company");
                }
            }
        }

        public string Address1
        {
            get
            {
                CanReadProperty("Address1", true);
                return _address1;
            }
            set
            {
                CanWriteProperty("Address1", true);
                if (value == null) value = string.Empty;
                if (!_address1.Equals(value))
                {
                    _address1 = value;
                    PropertyHasChanged("Address1");
                }
            }
        }

        public string Address2
        {
            get
            {
                CanReadProperty("Address2", true);
                return _address2;
            }
            set
            {
                CanWriteProperty("Address2", true);
                if (value == null) value = string.Empty;
                if (!_address2.Equals(value))
                {
                    _address2 = value;
                    PropertyHasChanged("Address2");
                }
            }
        }

        public string City
        {
            get
            {
                CanReadProperty("City", true);
                return _city;
            }
            set
            {
                CanWriteProperty("City", true);
                if (value == null) value = string.Empty;
                if (!_city.Equals(value))
                {
                    _city = value;
                    PropertyHasChanged("City");
                }
            }
        }

        public string Postal
        {
            get
            {
                CanReadProperty("Postal", true);
                return _postal;
            }
            set
            {
                CanWriteProperty("Postal", true);
                if (value == null) value = string.Empty;
                if (!_postal.Equals(value))
                {
                    _postal = value;
                    PropertyHasChanged("Postal");
                }
            }
        }

        public string Country
        {
            get
            {
                CanReadProperty("Country", true);
                return _country;
            }
            set
            {
                CanWriteProperty("Country", true);
                if (value == null) value = string.Empty;
                if (!_country.Equals(value))
                {
                    _country = value;
                    PropertyHasChanged("Country");
                }
            }
        }

        public string Website
        {
            get
            {
                CanReadProperty("Website", true);
                return _website;
            }
            set
            {
                CanWriteProperty("Website", true);
                if (value == null) value = string.Empty;
                if (!_website.Equals(value))
                {
                    _website = value;
                    PropertyHasChanged("Website");
                }
            }
        }

        public string CompanyType
        {
            get
            {
                CanReadProperty("CompanyType", true);
                return _companyType;
            }
            set
            {
                CanWriteProperty("CompanyType", true);
                if (value == null) value = string.Empty;
                if (!_companyType.Equals(value))
                {
                    _companyType = value;
                    PropertyHasChanged("CompanyType");
                }
            }
        }

        public string CompanyTypeOther
        {
            get
            {
                CanReadProperty("CompanyTypeOther", true);
                return _companyTypeOther;
            }
            set
            {
                CanWriteProperty("CompanyTypeOther", true);
                if (value == null) value = string.Empty;
                if (!_companyTypeOther.Equals(value))
                {
                    _companyTypeOther = value;
                    PropertyHasChanged("CompanyTypeOther");
                }
            }
        }

        public string Network
        {
            get
            {
                CanReadProperty("Network", true);
                return _network;
            }
            set
            {
                CanWriteProperty("Network", true);
                if (value == null) value = string.Empty;
                if (!_network.Equals(value))
                {
                    _network = value;
                    PropertyHasChanged("Network");
                }
            }
        }

        public string NetworkOthers
        {
            get
            {
                CanReadProperty("NetworkOthers", true);
                return _networkOthers;
            }
            set
            {
                CanWriteProperty("NetworkOthers", true);
                if (value == null) value = string.Empty;
                if (!_networkOthers.Equals(value))
                {
                    _networkOthers = value;
                    PropertyHasChanged("NetworkOthers");
                }
            }
        }

        public string HoldingCompany
        {
            get
            {
                CanReadProperty("HoldingCompany", true);
                return _holdingCompany;
            }
            set
            {
                CanWriteProperty("HoldingCompany", true);
                if (value == null) value = string.Empty;
                if (!_holdingCompany.Equals(value))
                {
                    _holdingCompany = value;
                    PropertyHasChanged("HoldingCompany");
                }
            }
        }

        public string HoldingCompanyOthers
        {
            get
            {
                CanReadProperty("HoldingCompanyOthers", true);
                return _holdingCompanyOthers;
            }
            set
            {
                CanWriteProperty("HoldingCompanyOthers", true);
                if (value == null) value = string.Empty;
                if (!_holdingCompanyOthers.Equals(value))
                {
                    _holdingCompanyOthers = value;
                    PropertyHasChanged("HoldingCompanyOthers");
                }
            }
        }

        public string MarketExp
        {
            get
            {
                CanReadProperty("MarketExp", true);
                return _marketExp;
            }
            set
            {
                CanWriteProperty("MarketExp", true);
                if (value == null) value = string.Empty;
                if (!_marketExp.Equals(value))
                {
                    _marketExp = value;
                    PropertyHasChanged("MarketExp");
                }
            }
        }

        public string MarketExpOthers
        {
            get
            {
                CanReadProperty("MarketExpOthers", true);
                return _marketExpOthers;
            }
            set
            {
                CanWriteProperty("MarketExpOthers", true);
                if (value == null) value = string.Empty;
                if (!_marketExpOthers.Equals(value))
                {
                    _marketExpOthers = value;
                    PropertyHasChanged("MarketExpOthers");
                }
            }
        }

        public string IndustryExp
        {
            get
            {
                CanReadProperty("IndustryExp", true);
                return _industryExp;
            }
            set
            {
                CanWriteProperty("IndustryExp", true);
                if (value == null) value = string.Empty;
                if (!_industryExp.Equals(value))
                {
                    _industryExp = value;
                    PropertyHasChanged("IndustryExp");
                }
            }
        }

        public string IndustryExpOthers
        {
            get
            {
                CanReadProperty("IndustryExpOthers", true);
                return _industryExpOthers;
            }
            set
            {
                CanWriteProperty("IndustryExpOthers", true);
                if (value == null) value = string.Empty;
                if (!_industryExpOthers.Equals(value))
                {
                    _industryExpOthers = value;
                    PropertyHasChanged("IndustryExpOthers");
                }
            }
        }

        public string EffieExp
        {
            get
            {
                CanReadProperty("EffieExp", true);
                return _effieExp;
            }
            set
            {
                CanWriteProperty("EffieExp", true);
                if (value == null) value = string.Empty;
                if (!_effieExp.Equals(value))
                {
                    _effieExp = value;
                    PropertyHasChanged("EffieExp");
                }
            }
        }

        public string EffieExpProgram
        {
            get
            {
                CanReadProperty("EffieExpProgram", true);
                return _effieExpProgram;
            }
            set
            {
                CanWriteProperty("EffieExpProgram", true);
                if (value == null) value = string.Empty;
                if (!_effieExpProgram.Equals(value))
                {
                    _effieExpProgram = value;
                    PropertyHasChanged("EffieExpProgram");
                }
            }
        }

        public string EffieExpYear
        {
            get
            {
                CanReadProperty("EffieExpYear", true);
                return _effieExpYear;
            }
            set
            {
                CanWriteProperty("EffieExpYear", true);
                if (value == null) value = string.Empty;
                if (!_effieExpYear.Equals(value))
                {
                    _effieExpYear = value;
                    PropertyHasChanged("EffieExpYear");
                }
            }
        }

        public string OtherExp
        {
            get
            {
                CanReadProperty("OtherExp", true);
                return _otherExp;
            }
            set
            {
                CanWriteProperty("OtherExp", true);
                if (value == null) value = string.Empty;
                if (!_otherExp.Equals(value))
                {
                    _otherExp = value;
                    PropertyHasChanged("OtherExp");
                }
            }
        }

        public string Skills
        {
            get
            {
                CanReadProperty("Skills", true);
                return _skills;
            }
            set
            {
                CanWriteProperty("Skills", true);
                if (value == null) value = string.Empty;
                if (!_skills.Equals(value))
                {
                    _skills = value;
                    PropertyHasChanged("Skills");
                }
            }
        }

        public string SkillsOthers
        {
            get
            {
                CanReadProperty("SkillsOthers", true);
                return _skillsOthers;
            }
            set
            {
                CanWriteProperty("SkillsOthers", true);
                if (value == null) value = string.Empty;
                if (!_skillsOthers.Equals(value))
                {
                    _skillsOthers = value;
                    PropertyHasChanged("SkillsOthers");
                }
            }
        }

        public string RevelantExp
        {
            get
            {
                CanReadProperty("RevelantExp", true);
                return _revelantExp;
            }
            set
            {
                CanWriteProperty("RevelantExp", true);
                if (value == null) value = string.Empty;
                if (!_revelantExp.Equals(value))
                {
                    _revelantExp = value;
                    PropertyHasChanged("RevelantExp");
                }
            }
        }

        public bool IsFirstTimeLogin
        {
            get
            {
                CanReadProperty("IsFirstTimeLogin", true);
                return _isFirstTimeLogin;
            }
            set
            {
                CanWriteProperty("IsFirstTimeLogin", true);
                if (value == null) value = false;
                if (!_isFirstTimeLogin.Equals(value))
                {
                    _isFirstTimeLogin = value;
                    PropertyHasChanged("IsFirstTimeLogin");
                }
            }
        }

        public DateTime DateLastReminded
        {
            get
            {
                CanReadProperty("DateLastReminded", true);
                return _dateLastReminded.Date;
            }
        }

        public string DateLastRemindedString
        {
            get
            {
                CanReadProperty("DateLastReminded", true);
                return _dateLastReminded.Text;
            }
            set
            {
                CanWriteProperty("DateLastReminded", true);
                if (value == null) value = string.Empty;
                if (!_dateLastReminded.Equals(value))
                {
                    _dateLastReminded.Text = value;
                    PropertyHasChanged("DateLastReminded");
                }
            }
        }

        public DateTime DateCreated
        {
            get
            {
                CanReadProperty("DateCreated", true);
                return _dateCreated.Date;
            }
        }

        public string DateCreatedString
        {
            get
            {
                CanReadProperty("DateCreated", true);
                return _dateCreated.Text;
            }
            set
            {
                CanWriteProperty("DateCreated", true);
                if (value == null) value = string.Empty;
                if (!_dateCreated.Equals(value))
                {
                    _dateCreated.Text = value;
                    PropertyHasChanged("DateCreated");
                }
            }
        }

        public DateTime DateModified
        {
            get
            {
                CanReadProperty("DateModified", true);
                return _dateModified.Date;
            }
        }

        public string DateModifiedString
        {
            get
            {
                CanReadProperty("DateModified", true);
                return _dateModified.Text;
            }
            set
            {
                CanWriteProperty("DateModified", true);
                if (value == null) value = string.Empty;
                if (!_dateModified.Equals(value))
                {
                    _dateModified.Text = value;
                    PropertyHasChanged("DateModified");
                }
            }
        }

        protected override object GetIdValue()
        {
            return _id;
        }

        #endregion //Business Properties and Methods

        #region Validation Rules
        private void AddCustomRules()
        {
            //add custom/non-generated rules here...
        }

        private void AddCommonRules()
        {
            //
            // Id
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("JuryId", 10));
            //
            // Round1PanelId
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Round1PanelId", 200));
            //
            // Round2PanelId
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Round2PanelId", 200));
            //
            // Password
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Password", 10));
            //
            // Salutation
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Salutation", 10));
            //
            // Firstname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Firstname", 100));
            //
            // Lastname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Lastname", 100));
            //
            // Job
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Job", 100));
            //
            // Contact
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Contact", 50));
            //
            // Mobile
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Mobile", 50));
            //
            // Email
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Email", 50));
            //
            // Company
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Company", 100));
            //
            // Address1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address1", 100));
            //
            // Address2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address2", 100));
            //
            // City
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("City", 100));
            //
            // Postal
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Postal", 50));
            //
            // Country
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Country", 100));
            //
            // Website
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Website", 100));
            //
            // CompanyType
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CompanyType", 100));
            //
            // CompanyTypeOther
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CompanyTypeOther", 100));
            //
            // Network
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Network", 100));
            //
            // NetworkOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("NetworkOthers", 100));
            //
            // HoldingCompany
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HoldingCompany", 100));
            //
            // HoldingCompanyOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HoldingCompanyOthers", 100));
            //
            // MarketExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MarketExp", 1000));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MarketExpOthers", 1000));
            //
            // IndustryExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IndustryExp", 1000));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IndustryExpOthers", 1000));
            //
            // EffieExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EffieExp", 1000));
            //
            // EffieExpProgram
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EffieExpProgram", 100));
            //
            // EffieExpYear
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EffieExpYear", 50));
            //
            // OtherExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OtherExp", 1000));
            //
            // Skills
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Skills", 500));


            // Revelant Exp
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RevelantExp", 1000));

            //
            // SkillsOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SkillsOthers", 100));
        }

        protected override void AddBusinessRules()
        {
            AddCommonRules();
            AddCustomRules();
        }
        #endregion //Validation Rules

        #region Authorization Rules
        protected override void AddAuthorizationRules()
        {
            //TODO: Define authorization rules in Jury
            //AuthorizationRules.AllowRead("Id", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Id", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Round1PanelId", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Round2PanelId", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Password", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Salutation", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Firstname", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Lastname", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Job", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Contact", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Mobile", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Email", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Company", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Address1", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Address2", "JuryReadGroup");
            //AuthorizationRules.AllowRead("City", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Postal", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Country", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Website", "JuryReadGroup");
            //AuthorizationRules.AllowRead("CompanyType", "JuryReadGroup");
            //AuthorizationRules.AllowRead("CompanyTypeOther", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Network", "JuryReadGroup");
            //AuthorizationRules.AllowRead("NetworkOthers", "JuryReadGroup");
            //AuthorizationRules.AllowRead("HoldingCompany", "JuryReadGroup");
            //AuthorizationRules.AllowRead("HoldingCompanyOthers", "JuryReadGroup");
            //AuthorizationRules.AllowRead("MarketExp", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IndustryExp", "JuryReadGroup");
            //AuthorizationRules.AllowRead("EffieExp", "JuryReadGroup");
            //AuthorizationRules.AllowRead("EffieExpProgram", "JuryReadGroup");
            //AuthorizationRules.AllowRead("EffieExpYear", "JuryReadGroup");
            //AuthorizationRules.AllowRead("OtherExp", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Skills", "JuryReadGroup");
            //AuthorizationRules.AllowRead("SkillsOthers", "JuryReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "JuryReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "JuryReadGroup");

            //AuthorizationRules.AllowWrite("Id", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Round1PanelId", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Round2PanelId", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Password", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Salutation", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Firstname", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Lastname", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Job", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Contact", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Mobile", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Email", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Company", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Address1", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Address2", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("City", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Postal", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Country", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Website", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("CompanyType", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("CompanyTypeOther", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Network", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("NetworkOthers", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("HoldingCompany", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("HoldingCompanyOthers", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("MarketExp", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IndustryExp", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("EffieExp", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("EffieExpProgram", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("EffieExpYear", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("OtherExp", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Skills", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("SkillsOthers", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "JuryWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Jury
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryViewGroup"))
            //    return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Jury
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryAddGroup"))
            //    return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Jury
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryEditGroup"))
            //    return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Jury
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryDeleteGroup"))
            //    return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Jury()
        { /* require use of factory method */ }
        private Jury(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }
        public static Jury NewJury()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Jury");
            return DataPortal.Create<Jury>();
        }

        public static Jury GetJury(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Jury");
            return DataPortal.Fetch<Jury>(new Criteria(id));
        }
        public static Jury GetJury(SafeDataReader dr)
        {
            return new Jury(dr);

        }
        public static void DeleteJury(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Jury");
            DataPortal.Delete(new Criteria(id));
        }

        public override Jury Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Jury");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Jury");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a Jury");

            return base.Save();
        }

        #endregion //Factory Methods

        #region Data Access

        #region Criteria

        [Serializable()]
        private class Criteria
        {
            public Guid Id;

            public Criteria(Guid id)
            {
                this.Id = id;
            }
        }

        #endregion //Criteria

        #region Data Access - Create
        [RunLocal]
        protected override void DataPortal_Create(object criteria)
        {
            ValidationRules.CheckRules();
        }

        #endregion //Data Access - Create

        #region Data Access - Fetch
        private void DataPortal_Fetch(Criteria criteria)
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
            {
                cn.Open();

                ExecuteFetch(cn, criteria);
            }//using
        }

        private void ExecuteFetch(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "GetJury";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    dr.Read();
                    FetchObject(dr);
                    ValidationRules.CheckRules();

                    //load child object(s)
                    FetchChildren(dr);
                }
            }//using
        }

        private void FetchObject(SafeDataReader dr)
        {
            
            _id = dr.GetGuid("Id");
            _juryId = dr.GetString("JuryId");
            _round1PanelId = dr.GetString("Round1PanelId");
            _round2PanelId = dr.GetString("Round2PanelId");
            _password = dr.GetString("Password");
            _salutation = dr.GetString("Salutation");
            _firstname = dr.GetString("Firstname");
            _lastname = dr.GetString("Lastname");
            _job = dr.GetString("Job");
            _contact = dr.GetString("Contact");
            _mobile = dr.GetString("Mobile");
            _email = dr.GetString("Email");
            _company = dr.GetString("Company");
            _address1 = dr.GetString("Address1");
            _address2 = dr.GetString("Address2");
            _city = dr.GetString("City");
            _postal = dr.GetString("Postal");
            _country = dr.GetString("Country");
            _website = dr.GetString("Website");
            _companyType = dr.GetString("CompanyType");
            _companyTypeOther = dr.GetString("CompanyTypeOther");
            _network = dr.GetString("Network");
            _networkOthers = dr.GetString("NetworkOthers");
            _holdingCompany = dr.GetString("HoldingCompany");
            _holdingCompanyOthers = dr.GetString("HoldingCompanyOthers");
            _marketExp = dr.GetString("MarketExp");
            _marketExpOthers = dr.GetString("MarketExpOthers");
            _industryExp = dr.GetString("IndustryExp");
            _industryExpOthers = dr.GetString("IndustryExpOthers");
            _effieExp = dr.GetString("EffieExp");
            _effieExpProgram = dr.GetString("EffieExpProgram");
            _effieExpYear = dr.GetString("EffieExpYear");
            _otherExp = dr.GetString("OtherExp");
            _skills = dr.GetString("Skills");
            _skillsOthers = dr.GetString("SkillsOthers");
            _revelantExp = dr.GetString("RevelantExp");
            _isFirstTimeLogin = dr.GetBoolean("IsFirstTimeLogin");
            _dateLastReminded = dr.GetSmartDate("DateLastReminded", _dateLastReminded.EmptyIsMin);
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
        }

        private void FetchChildren(SafeDataReader dr)
        {
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert
        protected override void DataPortal_Insert()
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
            {
                cn.Open();

                ExecuteInsert(cn);

                //update child object(s)
                UpdateChildren(cn);
            }//using

        }

        private void ExecuteInsert(SqlConnection cn)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "AddJury";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

               // _id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            
            cm.Parameters.AddWithValue("@JuryId", _juryId);                        
            cm.Parameters.AddWithValue("@Round1PanelId", _round1PanelId);                      
            cm.Parameters.AddWithValue("@Round2PanelId", _round2PanelId);           
            cm.Parameters.AddWithValue("@Password", _password);           
            cm.Parameters.AddWithValue("@Salutation", _salutation);           
            cm.Parameters.AddWithValue("@Firstname", _firstname);           
            cm.Parameters.AddWithValue("@Lastname", _lastname);            
            cm.Parameters.AddWithValue("@Job", _job);            
            cm.Parameters.AddWithValue("@Contact", _contact);           
            cm.Parameters.AddWithValue("@Mobile", _mobile);            
            cm.Parameters.AddWithValue("@Email", _email);           
            cm.Parameters.AddWithValue("@Company", _company);           
            cm.Parameters.AddWithValue("@Address1", _address1);            
            cm.Parameters.AddWithValue("@Address2", _address2);           
            cm.Parameters.AddWithValue("@City", _city);           
            cm.Parameters.AddWithValue("@Postal", _postal);           
            cm.Parameters.AddWithValue("@Country", _country);           
            cm.Parameters.AddWithValue("@Website", _website);           
            cm.Parameters.AddWithValue("@CompanyType", _companyType);           
            cm.Parameters.AddWithValue("@CompanyTypeOther", _companyTypeOther);           
            cm.Parameters.AddWithValue("@Network", _network);           
            cm.Parameters.AddWithValue("@NetworkOthers", _networkOthers);           
            cm.Parameters.AddWithValue("@HoldingCompany", _holdingCompany);            
            cm.Parameters.AddWithValue("@HoldingCompanyOthers", _holdingCompanyOthers);
            cm.Parameters.AddWithValue("@MarketExp", _marketExp);
            cm.Parameters.AddWithValue("@MarketExpOthers", _marketExpOthers);
            cm.Parameters.AddWithValue("@IndustryExp", _industryExp);
            cm.Parameters.AddWithValue("@IndustryExpOthers", _industryExpOthers);
            cm.Parameters.AddWithValue("@EffieExp", _effieExp);           
            cm.Parameters.AddWithValue("@EffieExpProgram", _effieExpProgram);           
            cm.Parameters.AddWithValue("@EffieExpYear", _effieExpYear);           
            cm.Parameters.AddWithValue("@OtherExp", _otherExp);            
            cm.Parameters.AddWithValue("@Skills", _skills);          
            cm.Parameters.AddWithValue("@SkillsOthers", _skillsOthers);
            cm.Parameters.AddWithValue("@RevelantExp", _revelantExp);
            cm.Parameters.AddWithValue("@IsFirstTimeLogin", _isFirstTimeLogin);
            cm.Parameters.AddWithValue("@DateLastReminded", _dateLastReminded.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
           // cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
        }
        #endregion //Data Access - Insert

        #region Data Access - Update
        protected override void DataPortal_Update()
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
            {
                cn.Open();

                if (base.IsDirty)
                {
                    ExecuteUpdate(cn);
                }

                //update child object(s)
                UpdateChildren(cn);
            }//using

        }

        private void ExecuteUpdate(SqlConnection cn)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "UpdateJury";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);            
            cm.Parameters.AddWithValue("@JuryId", _juryId);           
            cm.Parameters.AddWithValue("@Round1PanelId", _round1PanelId);           
            cm.Parameters.AddWithValue("@Round2PanelId", _round2PanelId);           
            cm.Parameters.AddWithValue("@Password", _password);           
            cm.Parameters.AddWithValue("@Salutation", _salutation);           
            cm.Parameters.AddWithValue("@Firstname", _firstname);           
            cm.Parameters.AddWithValue("@Lastname", _lastname);           
            cm.Parameters.AddWithValue("@Job", _job);           
            cm.Parameters.AddWithValue("@Contact", _contact);          
            cm.Parameters.AddWithValue("@Mobile", _mobile);          
            cm.Parameters.AddWithValue("@Email", _email);          
            cm.Parameters.AddWithValue("@Company", _company);           
            cm.Parameters.AddWithValue("@Address1", _address1);           
            cm.Parameters.AddWithValue("@Address2", _address2);          
            cm.Parameters.AddWithValue("@City", _city);            
            cm.Parameters.AddWithValue("@Postal", _postal);            
            cm.Parameters.AddWithValue("@Country", _country);           
            cm.Parameters.AddWithValue("@Website", _website);           
            cm.Parameters.AddWithValue("@CompanyType", _companyType);           
            cm.Parameters.AddWithValue("@CompanyTypeOther", _companyTypeOther);            
            cm.Parameters.AddWithValue("@Network", _network);           
            cm.Parameters.AddWithValue("@NetworkOthers", _networkOthers);            
            cm.Parameters.AddWithValue("@HoldingCompany", _holdingCompany);            
            cm.Parameters.AddWithValue("@HoldingCompanyOthers", _holdingCompanyOthers);            
            cm.Parameters.AddWithValue("@MarketExp", _marketExp);
            cm.Parameters.AddWithValue("@MarketExpOthers", _marketExpOthers);
            cm.Parameters.AddWithValue("@IndustryExp", _industryExp);
            cm.Parameters.AddWithValue("@IndustryExpOthers", _industryExpOthers);
            cm.Parameters.AddWithValue("@EffieExp", _effieExp);           
            cm.Parameters.AddWithValue("@EffieExpProgram", _effieExpProgram);            
            cm.Parameters.AddWithValue("@EffieExpYear", _effieExpYear);          
            cm.Parameters.AddWithValue("@OtherExp", _otherExp);           
            cm.Parameters.AddWithValue("@Skills", _skills);            
            cm.Parameters.AddWithValue("@SkillsOthers", _skillsOthers);
            cm.Parameters.AddWithValue("@RevelantExp", _revelantExp);
            cm.Parameters.AddWithValue("@IsFirstTimeLogin", _isFirstTimeLogin);
            cm.Parameters.AddWithValue("@DateLastReminded", _dateLastReminded.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
        }

        private void UpdateChildren(SqlConnection cn)
        {
        }
        #endregion //Data Access - Update

        #region Data Access - Delete
        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(new Criteria(_id));
        }

        private void DataPortal_Delete(Criteria criteria)
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("Effie")))
            {
                cn.Open();

                ExecuteDelete(cn, criteria);

            }//using

        }

        private void ExecuteDelete(SqlConnection cn, Criteria criteria)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "DeleteJury";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}