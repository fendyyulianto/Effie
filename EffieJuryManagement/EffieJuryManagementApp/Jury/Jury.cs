using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace EffieJuryManagementApp
{
    [Serializable()]
    public class Jury : Csla.BusinessBase<Jury>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _type = string.Empty;
        private string _serialNo = string.Empty;
        private string _password = string.Empty;
        private string _salutation = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _designation = string.Empty;
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
        private string _pAName = string.Empty;
        private string _pATel = string.Empty;
        private string _pAAddress1 = string.Empty;
        private string _pAAddress2 = string.Empty;
        private string _pAEmail = string.Empty;
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
        private string _effieExpProgramOthers = string.Empty;
        private string _effieExpYear = string.Empty;
        private string _otherEffieExp = string.Empty;
        private string _otherJudgingExp = string.Empty;
        private string _skills = string.Empty;
        private string _skillsOthers = string.Empty;
        private string _revelantExp = string.Empty;
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private string _remarks = string.Empty;
        private string _source = string.Empty;
        private string _lastUpdate = string.Empty;
        private bool _isRound1 = false;
        private bool _isRound2 = false;
        private string _userdata1 = string.Empty;
        private string _userdata2 = string.Empty;
        private string _userdata3 = string.Empty;
        private string _profile = string.Empty;
        private string _reference = string.Empty;
        private bool _isReceiveUpdate = false;
        private bool _isProfileUpdated = false;
        private bool _isActive = false;
        private string _round1PanelId = string.Empty;
        private string _round2PanelId = string.Empty;
        private string _SpecialistType = string.Empty;
        private string _CurrentIndustrySector = string.Empty;
        
        private SmartDate _dateLastReminded = new SmartDate(false);
        private bool _isFirstTimeLogin = false;
        private bool _isToDelete = false;
        private SmartDate _dateJuryModified = new SmartDate(false);

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
        }

        public string Type
        {
            get
            {
                CanReadProperty("Type", true);
                return _type;
            }
            set
            {
                CanWriteProperty("Type", true);
                if (value == null) value = string.Empty;
                if (!_type.Equals(value))
                {
                    _type = value;
                    PropertyHasChanged("Type");
                }
            }
        }

        public string SerialNo
        {
            get
            {
                CanReadProperty("SerialNo", true);
                return _serialNo;
            }
            set
            {
                CanWriteProperty("SerialNo", true);
                if (value == null) value = string.Empty;
                if (!_serialNo.Equals(value))
                {
                    _serialNo = value;
                    PropertyHasChanged("SerialNo");
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

        public string FirstName
        {
            get
            {
                CanReadProperty("FirstName", true);
                return _firstName;
            }
            set
            {
                CanWriteProperty("FirstName", true);
                if (value == null) value = string.Empty;
                if (!_firstName.Equals(value))
                {
                    _firstName = value;
                    PropertyHasChanged("FirstName");
                }
            }
        }

        public string LastName
        {
            get
            {
                CanReadProperty("LastName", true);
                return _lastName;
            }
            set
            {
                CanWriteProperty("LastName", true);
                if (value == null) value = string.Empty;
                if (!_lastName.Equals(value))
                {
                    _lastName = value;
                    PropertyHasChanged("LastName");
                }
            }
        }

        public string Designation
        {
            get
            {
                CanReadProperty("Designation", true);
                return _designation;
            }
            set
            {
                CanWriteProperty("Designation", true);
                if (value == null) value = string.Empty;
                if (!_designation.Equals(value))
                {
                    _designation = value;
                    PropertyHasChanged("Designation");
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

        public string PAName
        {
            get
            {
                CanReadProperty("PAName", true);
                return _pAName;
            }
            set
            {
                CanWriteProperty("PAName", true);
                if (value == null) value = string.Empty;
                if (!_pAName.Equals(value))
                {
                    _pAName = value;
                    PropertyHasChanged("PAName");
                }
            }
        }

        public string PATel
        {
            get
            {
                CanReadProperty("PATel", true);
                return _pATel;
            }
            set
            {
                CanWriteProperty("PATel", true);
                if (value == null) value = string.Empty;
                if (!_pATel.Equals(value))
                {
                    _pATel = value;
                    PropertyHasChanged("PATel");
                }
            }
        }

        public string PAAddress1
        {
            get
            {
                CanReadProperty("PAAddress1", true);
                return _pAAddress1;
            }
            set
            {
                CanWriteProperty("PAAddress1", true);
                if (value == null) value = string.Empty;
                if (!_pAAddress1.Equals(value))
                {
                    _pAAddress1 = value;
                    PropertyHasChanged("PAAddress1");
                }
            }
        }

        public string PAAddress2
        {
            get
            {
                CanReadProperty("PAAddress2", true);
                return _pAAddress2;
            }
            set
            {
                CanWriteProperty("PAAddress2", true);
                if (value == null) value = string.Empty;
                if (!_pAAddress2.Equals(value))
                {
                    _pAAddress2 = value;
                    PropertyHasChanged("PAAddress2");
                }
            }
        }

        public string PAEmail
        {
            get
            {
                CanReadProperty("PAEmail", true);
                return _pAEmail;
            }
            set
            {
                CanWriteProperty("PAEmail", true);
                if (value == null) value = string.Empty;
                if (!_pAEmail.Equals(value))
                {
                    _pAEmail = value;
                    PropertyHasChanged("PAEmail");
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

        public string EffieExpProgramOthers
        {
            get
            {
                CanReadProperty("EffieExpProgramOthers", true);
                return _effieExpProgramOthers;
            }
            set
            {
                CanWriteProperty("EffieExpProgramOthers", true);
                if (value == null) value = string.Empty;
                if (!_effieExpProgramOthers.Equals(value))
                {
                    _effieExpProgramOthers = value;
                    PropertyHasChanged("EffieExpProgramOthers");
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

        public string OtherEffieExp
        {
            get
            {
                CanReadProperty("OtherEffieExp", true);
                return _otherEffieExp;
            }
            set
            {
                CanWriteProperty("OtherEffieExp", true);
                if (value == null) value = string.Empty;
                if (!_otherEffieExp.Equals(value))
                {
                    _otherEffieExp = value;
                    PropertyHasChanged("OtherEffieExp");
                }
            }
        }

        public string OtherJudgingExp
        {
            get
            {
                CanReadProperty("OtherJudgingExp", true);
                return _otherJudgingExp;
            }
            set
            {
                CanWriteProperty("OtherJudgingExp", true);
                if (value == null) value = string.Empty;
                if (!_otherJudgingExp.Equals(value))
                {
                    _otherJudgingExp = value;
                    PropertyHasChanged("OtherJudgingExp");
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

        public string Remarks
        {
            get
            {
                CanReadProperty("Remarks", true);
                return _remarks;
            }
            set
            {
                CanWriteProperty("Remarks", true);
                if (value == null) value = string.Empty;
                if (!_remarks.Equals(value))
                {
                    _remarks = value;
                    PropertyHasChanged("Remarks");
                }
            }
        }

        public string Source
        {
            get
            {
                CanReadProperty("Source", true);
                return _source;
            }
            set
            {
                CanWriteProperty("Source", true);
                if (value == null) value = string.Empty;
                if (!_source.Equals(value))
                {
                    _source = value;
                    PropertyHasChanged("Source");
                }
            }
        }

        public string LastUpdate
        {
            get
            {
                CanReadProperty("LastUpdate", true);
                return _lastUpdate;
            }
            set
            {
                CanWriteProperty("LastUpdate", true);
                if (value == null) value = string.Empty;
                if (!_lastUpdate.Equals(value))
                {
                    _lastUpdate = value;
                    PropertyHasChanged("LastUpdate");
                }
            }
        }

        public bool IsRound1
        {
            get
            {
                CanReadProperty("IsRound1", true);
                return _isRound1;
            }
            set
            {
                CanWriteProperty("IsRound1", true);
                if (!_isRound1.Equals(value))
                {
                    _isRound1 = value;
                    PropertyHasChanged("IsRound1");
                }
            }
        }

        public bool IsRound2
        {
            get
            {
                CanReadProperty("IsRound2", true);
                return _isRound2;
            }
            set
            {
                CanWriteProperty("IsRound2", true);
                if (!_isRound2.Equals(value))
                {
                    _isRound2 = value;
                    PropertyHasChanged("IsRound2");
                }
            }
        }

        public string Userdata1
        {
            get
            {
                CanReadProperty("Userdata1", true);
                return _userdata1;
            }
            set
            {
                CanWriteProperty("Userdata1", true);
                if (value == null) value = string.Empty;
                if (!_userdata1.Equals(value))
                {
                    _userdata1 = value;
                    PropertyHasChanged("Userdata1");
                }
            }
        }

        public string Userdata2
        {
            get
            {
                CanReadProperty("Userdata2", true);
                return _userdata2;
            }
            set
            {
                CanWriteProperty("Userdata2", true);
                if (value == null) value = string.Empty;
                if (!_userdata2.Equals(value))
                {
                    _userdata2 = value;
                    PropertyHasChanged("Userdata2");
                }
            }
        }

        public string Userdata3
        {
            get
            {
                CanReadProperty("Userdata3", true);
                return _userdata3;
            }
            set
            {
                CanWriteProperty("Userdata3", true);
                if (value == null) value = string.Empty;
                if (!_userdata3.Equals(value))
                {
                    _userdata3 = value;
                    PropertyHasChanged("Userdata3");
                }
            }
        }

        public string Profile
        {
            get
            {
                CanReadProperty("Profile", true);
                return _profile;
            }
            set
            {
                CanWriteProperty("Profile", true);
                if (value == null) value = string.Empty;
                if (!_profile.Equals(value))
                {
                    _profile = value;
                    PropertyHasChanged("Profile");
                }
            }
        }

        public string Reference
        {
            get
            {
                CanReadProperty("Reference", true);
                return _reference;
            }
            set
            {
                CanWriteProperty("Reference", true);
                if (value == null) value = string.Empty;
                if (!_reference.Equals(value))
                {
                    _reference = value;
                    PropertyHasChanged("Reference");
                }
            }
        }

        public bool IsReceiveUpdate
        {
            get
            {
                CanReadProperty("IsReceiveUpdate", true);
                return _isReceiveUpdate;
            }
            set
            {
                CanWriteProperty("IsReceiveUpdate", true);
                if (!_isReceiveUpdate.Equals(value))
                {
                    _isReceiveUpdate = value;
                    PropertyHasChanged("IsReceiveUpdate");
                }
            }
        }

        public bool IsProfileUpdated
        {
            get
            {
                CanReadProperty("IsProfileUpdated", true);
                return _isProfileUpdated;
            }
            set
            {
                CanWriteProperty("IsProfileUpdated", true);
                if (!_isProfileUpdated.Equals(value))
                {
                    _isProfileUpdated = value;
                    PropertyHasChanged("IsProfileUpdated");
                }
            }
        }

        public bool IsActive
        {
            get
            {
                CanReadProperty("IsActive", true);
                return _isActive;
            }
            set
            {
                CanWriteProperty("IsActive", true);
                if (!_isActive.Equals(value))
                {
                    _isActive = value;
                    PropertyHasChanged("IsActive");
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

        public string CurrentIndustrySector
        {
            get
            {
                CanReadProperty("CurrentIndustrySector", true);
                return _CurrentIndustrySector;
            }
            set
            {
                CanWriteProperty("CurrentIndustrySector", true);
                if (value == null) value = string.Empty;
                if (!_CurrentIndustrySector.Equals(value))
                {
                    _CurrentIndustrySector = value;
                    PropertyHasChanged("CurrentIndustrySector");
                }
            }
        }
        

        public string SpecialistType
        {
            get
            {
                CanReadProperty("SpecialistType", true);
                return _SpecialistType;
            }
            set
            {
                CanWriteProperty("SpecialistType", true);
                if (value == null) value = string.Empty;
                if (!_SpecialistType.Equals(value))
                {
                    _SpecialistType = value;
                    PropertyHasChanged("SpecialistType");
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
                if (!_isFirstTimeLogin.Equals(value))
                {
                    _isFirstTimeLogin = value;
                    PropertyHasChanged("IsFirstTimeLogin");
                }
            }
        }

        public bool IsToDelete
        {
            get
            {
                CanReadProperty("IsToDelete", true);
                return _isToDelete;
            }
            set
            {
                CanWriteProperty("IsToDelete", true);
                if (!_isToDelete.Equals(value))
                {
                    _isToDelete = value;
                    PropertyHasChanged("IsToDelete");
                }
            }
        }

        public DateTime DateJuryModified
        {
            get
            {
                CanReadProperty("DateJuryModified", true);
                return _dateJuryModified.Date;
            }
        }

        public string DateJuryModifiedString
        {
            get
            {
                CanReadProperty("DateJuryModified", true);
                return _dateJuryModified.Text;
            }
            set
            {
                CanWriteProperty("DateJuryModified", true);
                if (value == null) value = string.Empty;
                if (!_dateJuryModified.Equals(value))
                {
                    _dateJuryModified.Text = value;
                    PropertyHasChanged("DateJuryModified");
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
            // Type
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Type", 100));
            //
            // SerialNo
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SerialNo", 20));
            //
            // Password
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Password", 20));
            //
            // Salutation
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Salutation", 10));
            //
            // FirstName
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("FirstName", 100));
            //
            // LastName
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LastName", 100));
            //
            // Designation
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Designation", 100));
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
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Email", 100));
            //
            // Company
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Company", 200));
            //
            // Address1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address1", 200));
            //
            // Address2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Address2", 200));
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
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Website", 200));
            //
            // PAName
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PAName", 100));
            //
            // PATel
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PATel", 50));
            //
            // PAAddress1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PAAddress1", 200));
            //
            // PAAddress2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PAAddress2", 200));
            //
            // PAEmail
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PAEmail", 100));
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
            //
            // MarketExpOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MarketExpOthers", 1000));
            //
            // IndustryExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IndustryExp", 1000));
            //
            // IndustryExpOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("IndustryExpOthers", 1000));
            //
            // EffieExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EffieExp", 1000));
            //
            // EffieExpProgram
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EffieExpProgram", 1000));
            //
            // EffieExpProgramOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EffieExpProgramOthers", 100));
            //
            // EffieExpYear
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EffieExpYear", 4000));
            //
            // OtherEffieExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OtherEffieExp", 1000));
            //
            // OtherJudgingExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OtherJudgingExp", 1000));
            //
            // Skills
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Skills", 500));
            //
            // SkillsOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SkillsOthers", 100));
            //
            // RevelantExp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RevelantExp", 1000));
            //
            // Remarks
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Remarks", 1000));
            //
            // Source
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Source", 500));
            //
            // LastUpdate
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("LastUpdate", 100));
            //
            // Userdata1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Userdata1", 4000));
            //
            // Userdata2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Userdata2", 4000));
            //
            // Userdata3
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Userdata3", 4000));
            //
            // Profile
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Profile", 4000));
            //
            // Reference
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Reference", 500));
            //
            // Round1PanelId
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Round1PanelId", 200));
            //
            // Round2PanelId
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Round2PanelId", 200));
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
            //AuthorizationRules.AllowRead("Salutation", "JuryReadGroup");
            //AuthorizationRules.AllowRead("FirstName", "JuryReadGroup");
            //AuthorizationRules.AllowRead("LastName", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Designation", "JuryReadGroup");
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
            //AuthorizationRules.AllowRead("MarketExpOthers", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IndustryExp", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IndustryExpOthers", "JuryReadGroup");
            //AuthorizationRules.AllowRead("EffieExp", "JuryReadGroup");
            //AuthorizationRules.AllowRead("EffieExpProgram", "JuryReadGroup");
            //AuthorizationRules.AllowRead("EffieExpYear", "JuryReadGroup");
            //AuthorizationRules.AllowRead("OtherExp", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Skills", "JuryReadGroup");
            //AuthorizationRules.AllowRead("SkillsOthers", "JuryReadGroup");
            //AuthorizationRules.AllowRead("RevelantExp", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IsRound1Invited", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IsRound2Invited", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IsRound1Accepted", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IsRound2Accepted", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IsRound1Assigned", "JuryReadGroup");
            //AuthorizationRules.AllowRead("IsRound2Assigned", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Date1stEmailSent", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Date2ndEmailSent", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Date3rdEmailSent", "JuryReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "JuryReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Userdata1", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Userdata2", "JuryReadGroup");
            //AuthorizationRules.AllowRead("Userdata3", "JuryReadGroup");

            //AuthorizationRules.AllowWrite("Id", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Salutation", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("FirstName", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("LastName", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Designation", "JuryWriteGroup");
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
            //AuthorizationRules.AllowWrite("MarketExpOthers", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IndustryExp", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IndustryExpOthers", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("EffieExp", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("EffieExpProgram", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("EffieExpYear", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("OtherExp", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Skills", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("SkillsOthers", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("RevelantExp", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound1Invited", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound2Invited", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound1Accepted", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound2Accepted", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound1Assigned", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("IsRound2Assigned", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Date1stEmailSent", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Date2ndEmailSent", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Date3rdEmailSent", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Userdata1", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Userdata2", "JuryWriteGroup");
            //AuthorizationRules.AllowWrite("Userdata3", "JuryWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Jury
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Jury
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Jury
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Jury
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("JuryDeleteGroup"))
            //	return true;
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
            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
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
            _type = dr.GetString("Type");
            _serialNo = dr.GetString("SerialNo");
            _password = dr.GetString("Password");
            _salutation = dr.GetString("Salutation");
            _firstName = dr.GetString("FirstName");
            _lastName = dr.GetString("LastName");
            _designation = dr.GetString("Designation");
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
            _pAName = dr.GetString("PAName");
            _pATel = dr.GetString("PATel");
            _pAAddress1 = dr.GetString("PAAddress1");
            _pAAddress2 = dr.GetString("PAAddress2");
            _pAEmail = dr.GetString("PAEmail");
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
            _effieExpProgramOthers = dr.GetString("EffieExpProgramOthers");
            _effieExpYear = dr.GetString("EffieExpYear");
            _otherEffieExp = dr.GetString("OtherEffieExp");
            _otherJudgingExp = dr.GetString("OtherJudgingExp");
            _skills = dr.GetString("Skills");
            _skillsOthers = dr.GetString("SkillsOthers");
            _revelantExp = dr.GetString("RevelantExp");
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _remarks = dr.GetString("Remarks");
            _source = dr.GetString("Source");
            _lastUpdate = dr.GetString("LastUpdate");
            _isRound1 = dr.GetBoolean("IsRound1");
            _isRound2 = dr.GetBoolean("IsRound2");
            _userdata1 = dr.GetString("Userdata1");
            _userdata2 = dr.GetString("Userdata2");
            _userdata3 = dr.GetString("Userdata3");
            _profile = dr.GetString("Profile");
            _reference = dr.GetString("Reference");
            _isReceiveUpdate = dr.GetBoolean("IsReceiveUpdate");
            _isProfileUpdated = dr.GetBoolean("IsProfileUpdated");
            _isActive = dr.GetBoolean("IsActive");
            _round1PanelId = dr.GetString("Round1PanelId");
            _round2PanelId = dr.GetString("Round2PanelId");
            _SpecialistType = dr.GetString("SpecialistType");
            _CurrentIndustrySector = dr.GetString("CurrentIndustrySector");
            
            _dateLastReminded = dr.GetSmartDate("DateLastReminded", _dateLastReminded.EmptyIsMin);
            _isFirstTimeLogin = dr.GetBoolean("IsFirstTimeLogin");
            _isToDelete = dr.GetBoolean("IsToDelete");
            _dateJuryModified = dr.GetSmartDate("DateJuryModified", _dateJuryModified.EmptyIsMin);
        }

        private void FetchChildren(SafeDataReader dr)
        {
        }
        #endregion //Data Access - Fetch

        #region Data Access - Insert
        protected override void DataPortal_Insert()
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
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

                _id = (Guid)cm.Parameters["@Id"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Type", _type);
            cm.Parameters.AddWithValue("@SerialNo", _serialNo);
            cm.Parameters.AddWithValue("@Password", _password);
            cm.Parameters.AddWithValue("@Salutation", _salutation);
            cm.Parameters.AddWithValue("@FirstName", _firstName);
            cm.Parameters.AddWithValue("@LastName", _lastName);
            cm.Parameters.AddWithValue("@Designation", _designation);
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
            cm.Parameters.AddWithValue("@PAName", _pAName);
            cm.Parameters.AddWithValue("@PATel", _pATel);
            cm.Parameters.AddWithValue("@PAAddress1", _pAAddress1);
            cm.Parameters.AddWithValue("@PAAddress2", _pAAddress2);
            cm.Parameters.AddWithValue("@PAEmail", _pAEmail);
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
            cm.Parameters.AddWithValue("@EffieExpProgramOthers", _effieExpProgramOthers);
            cm.Parameters.AddWithValue("@EffieExpYear", _effieExpYear);
            cm.Parameters.AddWithValue("@OtherEffieExp", _otherEffieExp);
            cm.Parameters.AddWithValue("@OtherJudgingExp", _otherJudgingExp);
            cm.Parameters.AddWithValue("@Skills", _skills);
            cm.Parameters.AddWithValue("@SkillsOthers", _skillsOthers);
            cm.Parameters.AddWithValue("@RevelantExp", _revelantExp);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Remarks", _remarks);
            cm.Parameters.AddWithValue("@Source", _source);
            cm.Parameters.AddWithValue("@LastUpdate", _lastUpdate);
            cm.Parameters.AddWithValue("@IsRound1", _isRound1);
            cm.Parameters.AddWithValue("@IsRound2", _isRound2);
            cm.Parameters.AddWithValue("@Userdata1", _userdata1);
            cm.Parameters.AddWithValue("@Userdata2", _userdata2);
            cm.Parameters.AddWithValue("@Userdata3", _userdata3);
            cm.Parameters.AddWithValue("@Reference", _reference);
            cm.Parameters.AddWithValue("@IsReceiveUpdate", _isReceiveUpdate);
            cm.Parameters.AddWithValue("@Profile", _profile);
            cm.Parameters.AddWithValue("@IsProfileUpdated", _isProfileUpdated);
            cm.Parameters.AddWithValue("@IsActive", _isActive);
            cm.Parameters.AddWithValue("@Round1PanelId", _round1PanelId);
            cm.Parameters.AddWithValue("@Round2PanelId", _round2PanelId);
            cm.Parameters.AddWithValue("@SpecialistType", _SpecialistType);
            cm.Parameters.AddWithValue("@CurrentIndustrySector", _CurrentIndustrySector);
            cm.Parameters.AddWithValue("@DateLastReminded", _dateLastReminded.DBValue);
            cm.Parameters.AddWithValue("@IsFirstTimeLogin", _isFirstTimeLogin);
            cm.Parameters.AddWithValue("@IsToDelete", _isToDelete);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@DateJuryModified", _dateJuryModified.DBValue);
        }
        #endregion //Data Access - Insert

        #region Data Access - Update
        protected override void DataPortal_Update()
        {
            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
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
            cm.Parameters.AddWithValue("@Type", _type);
            cm.Parameters.AddWithValue("@SerialNo", _serialNo);
            cm.Parameters.AddWithValue("@Password", _password);
            cm.Parameters.AddWithValue("@Salutation", _salutation);
            cm.Parameters.AddWithValue("@FirstName", _firstName);
            cm.Parameters.AddWithValue("@LastName", _lastName);
            cm.Parameters.AddWithValue("@Designation", _designation);
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
            cm.Parameters.AddWithValue("@PAName", _pAName);
            cm.Parameters.AddWithValue("@PATel", _pATel);
            cm.Parameters.AddWithValue("@PAAddress1", _pAAddress1);
            cm.Parameters.AddWithValue("@PAAddress2", _pAAddress2);
            cm.Parameters.AddWithValue("@PAEmail", _pAEmail);
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
            cm.Parameters.AddWithValue("@EffieExpProgramOthers", _effieExpProgramOthers);
            cm.Parameters.AddWithValue("@EffieExpYear", _effieExpYear);
            cm.Parameters.AddWithValue("@OtherEffieExp", _otherEffieExp);
            cm.Parameters.AddWithValue("@OtherJudgingExp", _otherJudgingExp);
            cm.Parameters.AddWithValue("@Skills", _skills);
            cm.Parameters.AddWithValue("@SkillsOthers", _skillsOthers);
            cm.Parameters.AddWithValue("@RevelantExp", _revelantExp);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@Remarks", _remarks);
            cm.Parameters.AddWithValue("@Source", _source);
            cm.Parameters.AddWithValue("@LastUpdate", _lastUpdate);
            cm.Parameters.AddWithValue("@IsRound1", _isRound1);
            cm.Parameters.AddWithValue("@IsRound2", _isRound2);
            cm.Parameters.AddWithValue("@Userdata1", _userdata1);
            cm.Parameters.AddWithValue("@Userdata2", _userdata2);
            cm.Parameters.AddWithValue("@Userdata3", _userdata3);
            cm.Parameters.AddWithValue("@Reference", _reference);
            cm.Parameters.AddWithValue("@IsReceiveUpdate", _isReceiveUpdate);
            cm.Parameters.AddWithValue("@Profile", _profile);
            cm.Parameters.AddWithValue("@IsProfileUpdated", _isProfileUpdated);
            cm.Parameters.AddWithValue("@IsActive", _isActive);
            cm.Parameters.AddWithValue("@Round1PanelId", _round1PanelId);
            cm.Parameters.AddWithValue("@Round2PanelId", _round2PanelId);
            cm.Parameters.AddWithValue("@SpecialistType", _SpecialistType);
            cm.Parameters.AddWithValue("@CurrentIndustrySector", _CurrentIndustrySector);
            cm.Parameters.AddWithValue("@DateLastReminded", _dateLastReminded.DBValue);
            cm.Parameters.AddWithValue("@IsFirstTimeLogin", _isFirstTimeLogin);
            cm.Parameters.AddWithValue("@IsToDelete", _isToDelete);
            cm.Parameters.AddWithValue("@DateJuryModified", _dateJuryModified.DBValue);
            cm.Parameters.AddWithValue("@Id", _id);
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
            using (SqlConnection cn = new SqlConnection(Database.DB("EffieJuryMgmt")))
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

        #region Custom

        public static string GetEffieExperienceYears(Jury jury,Invitation juryInv)
        {
            string effieExpereinceYears = string.Empty;

            // Updating Jury Effie Experience Remarks (Note: For each year have to update the remarks)
            bool is2014 = jury.EffieExpYear.IndexOf("2014") != -1;
            bool is2015 = jury.EffieExpYear.IndexOf("2015") != -1;
            bool is2016 = jury.EffieExpYear.IndexOf("2016") != -1;
            bool is2017 = (juryInv.IsRound1Assigned || juryInv.IsRound2Assigned);

            string remarks2014 = string.Empty;
            string remarks2015 = string.Empty;
            string remarks2016 = string.Empty;
            string remarks2017 = string.Empty;

            try
            {
                remarks2014 = jury.EffieExpYear.Split('|')[0].Split('#')[1];
            }
            catch { }

            try
            {
                remarks2015 = jury.EffieExpYear.Split('|')[1].Split('#')[1];
            }
            catch { }

            try
            {
                remarks2016 = jury.EffieExpYear.Split('|')[2].Split('#')[1];
            }
            catch { }

            try
            {
                remarks2017 = jury.EffieExpYear.Split('|')[3].Split('#')[1];
            }
            catch { }
            

            effieExpereinceYears += (is2014 ? "2014" : "") + "#" + remarks2014 + "|";
            effieExpereinceYears += (is2015 ? "2015" : "") + "#" + remarks2015 + "|";
            effieExpereinceYears += (is2016 ? "2016" : "") + "#" + remarks2016 + "|";
            effieExpereinceYears += (is2017 ? "2017" : "") + "#" + remarks2017 + "|";

            return effieExpereinceYears;
        }

        #endregion
    }
}
