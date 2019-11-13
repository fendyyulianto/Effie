using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Effie2017.App
{
    [Serializable()]
    public class Registration : Csla.BusinessBase<Registration>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _salutation = string.Empty;
        private string _firstname = string.Empty;
        private string _lastname = string.Empty;
        private string _job = string.Empty;
        private string _contact = string.Empty;
        private string _mobile = string.Empty;
        private string _fax = string.Empty;
        private string _website = string.Empty;
        private string _company = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private string _city = string.Empty;
        private string _postal = string.Empty;
        private string _country = string.Empty;
        private bool _isCAAAA = false;
        private bool _isAPEP = false;
        private bool _isAFAA = false;
        private bool _isEProg = false;
        private bool _isPromo1 = false;
        private bool _isPromo2 = false;
        private bool _isPromo3 = false;
        private string _caaaa = string.Empty;
        private string _apep = string.Empty;
        private string _afaa = string.Empty;
        private string _eprog = string.Empty;
        private string _eProgCampaign = string.Empty;
        private bool _isEmailUpdate = false;
        private bool _isActive = true;
        private string _status = string.Empty ;
        private SmartDate _lastSignIn = new SmartDate(false);
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private SmartDate _lastSignIn2 = new SmartDate(false);
        private bool _isVerified = false;
        private bool _IsExpired = false;
        private bool _IsLooked = false;
        private string _LogPassword = string.Empty;

        [System.ComponentModel.DataObjectField(true, true)]
        public Guid Id
        {
            get
            {
                CanReadProperty("Id", true);
                return _id;
            }
            set
            {
                CanWriteProperty("Id", true);
                if (value == null) value = Guid.Empty;
                if (!_id.Equals(value))
                {
                    _id = value;
                    PropertyHasChanged("Id");
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

        public string Fax
        {
            get
            {
                CanReadProperty("Fax", true);
                return _fax;
            }
            set
            {
                CanWriteProperty("Fax", true);
                if (value == null) value = string.Empty;
                if (!_fax.Equals(value))
                {
                    _fax = value;
                    PropertyHasChanged("Fax");
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

        public bool IsCAAAA
        {
            get
            {
                CanReadProperty("IsCAAAA", true);
                return _isCAAAA;
            }
            set
            {
                CanWriteProperty("IsCAAAA", true);
                if (!_isCAAAA.Equals(value))
                {
                    _isCAAAA = value;
                    PropertyHasChanged("IsCAAAA");
                }
            }
        }

        public bool IsAPEP
        {
            get
            {
                CanReadProperty("IsAPEP", true);
                return _isAPEP;
            }
            set
            {
                CanWriteProperty("IsAPEP", true);
                if (!_isAPEP.Equals(value))
                {
                    _isAPEP = value;
                    PropertyHasChanged("IsAPEP");
                }
            }
        }

        public bool IsAFAA
        {
            get
            {
                CanReadProperty("IsAFAA", true);
                return _isAFAA;
            }
            set
            {
                CanWriteProperty("IsAFAA", true);
                if (!_isAFAA.Equals(value))
                {
                    _isAFAA = value;
                    PropertyHasChanged("IsAFAA");
                }
            }
        }

        public bool IsEProg
        {
            get
            {
                CanReadProperty("IsEProg", true);
                return _isEProg;
            }
            set
            {
                CanWriteProperty("IsEProg", true);
                if (!_isEProg.Equals(value))
                {
                    _isEProg = value;
                    PropertyHasChanged("IsEProg");
                }
            }
        }

        public bool IsPromo1
        {
            get
            {
                CanReadProperty("IsPromo1", true);
                return _isPromo1;
            }
            set
            {
                CanWriteProperty("IsPromo1", true);
                if (!_isPromo1.Equals(value))
                {
                    _isPromo1 = value;
                    PropertyHasChanged("IsPromo1");
                }
            }
        }

        public bool IsPromo2
        {
            get
            {
                CanReadProperty("IsPromo2", true);
                return _isPromo2;
            }
            set
            {
                CanWriteProperty("IsPromo2", true);
                if (!_isPromo2.Equals(value))
                {
                    _isPromo2 = value;
                    PropertyHasChanged("IsPromo2");
                }
            }
        }

        public bool IsPromo3
        {
            get
            {
                CanReadProperty("IsPromo3", true);
                return _isPromo3;
            }
            set
            {
                CanWriteProperty("IsPromo3", true);
                if (!_isPromo3.Equals(value))
                {
                    _isPromo3 = value;
                    PropertyHasChanged("IsPromo3");
                }
            }
        }

        public string Caaaa
        {
            get
            {
                CanReadProperty("Caaaa", true);
                return _caaaa;
            }
            set
            {
                CanWriteProperty("Caaaa", true);
                if (value == null) value = string.Empty;
                if (!_caaaa.Equals(value))
                {
                    _caaaa = value;
                    PropertyHasChanged("Caaaa");
                }
            }
        }

        public string Apep
        {
            get
            {
                CanReadProperty("Apep", true);
                return _apep;
            }
            set
            {
                CanWriteProperty("Apep", true);
                if (value == null) value = string.Empty;
                if (!_apep.Equals(value))
                {
                    _apep = value;
                    PropertyHasChanged("Apep");
                }
            }
        }

        public string Afaa
        {
            get
            {
                CanReadProperty("Afaa", true);
                return _afaa;
            }
            set
            {
                CanWriteProperty("Afaa", true);
                if (value == null) value = string.Empty;
                if (!_afaa.Equals(value))
                {
                    _afaa = value;
                    PropertyHasChanged("Afaa");
                }
            }
        }

        public string Eprog
        {
            get
            {
                CanReadProperty("Eprog", true);
                return _eprog;
            }
            set
            {
                CanWriteProperty("Eprog", true);
                if (value == null) value = string.Empty;
                if (!_eprog.Equals(value))
                {
                    _eprog = value;
                    PropertyHasChanged("Eprog");
                }
            }
        }
        

        public string LogPassword
        {
            get
            {
                CanReadProperty("LogPassword", true);
                return _LogPassword;
            }
            set
            {
                CanWriteProperty("LogPassword", true);
                if (value == null) value = string.Empty;
                if (!_LogPassword.Equals(value))
                {
                    _LogPassword = value;
                    PropertyHasChanged("LogPassword");
                }
            }
        }

        public string EProgCampaign
        {
            get
            {
                CanReadProperty("EProgCampaign", true);
                return _eProgCampaign;
            }
            set
            {
                CanWriteProperty("EProgCampaign", true);
                if (value == null) value = string.Empty;
                if (!_eProgCampaign.Equals(value))
                {
                    _eProgCampaign = value;
                    PropertyHasChanged("EProgCampaign");
                }
            }
        }

        public bool IsEmailUpdate
        {
            get
            {
                CanReadProperty("IsEmailUpdate", true);
                return _isEmailUpdate;
            }
            set
            {
                CanWriteProperty("IsEmailUpdate", true); 
                if (!_isEmailUpdate.Equals(value))
                {
                    _isEmailUpdate = value;
                    PropertyHasChanged("IsEmailUpdate");
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

        public string Status
        {
            get
            {
                CanReadProperty("Status", true);
                return _status;
            }
            set
            {
                CanWriteProperty("Status", true);
                if (value == null) value = string.Empty;
                if (!_status.Equals(value))
                {
                    _status = value;
                    PropertyHasChanged("Status");
                }
            }
        }

        public DateTime LastSignIn
        {
            get
            {
                CanReadProperty("LastSignIn", true);
                return _lastSignIn.Date;
            }
        }

        public string LastSignInString
        {
            get
            {
                CanReadProperty("LastSignIn", true);
                return _lastSignIn.Text;
            }
            set
            {
                CanWriteProperty("LastSignIn", true);
                if (value == null) value = string.Empty;
                if (!_lastSignIn.Equals(value))
                {
                    _lastSignIn.Text = value;
                    PropertyHasChanged("LastSignIn");
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

        public DateTime LastSignIn2
        {
            get
            {
                CanReadProperty("LastSignIn2", true);
                return _lastSignIn2.Date;
            }
        }

        public string LastSignIn2String
        {
            get
            {
                CanReadProperty("LastSignIn2", true);
                return _lastSignIn2.Text;
            }
            set
            {
                CanWriteProperty("LastSignIn2", true);
                if (value == null) value = string.Empty;
                if (!_lastSignIn2.Equals(value))
                {
                    _lastSignIn2.Text = value;
                    PropertyHasChanged("LastSignIn2");
                }
            }
        }

        public bool IsVerified
        {
            get
            {
                CanReadProperty("IsVerified", true);
                return _isVerified;
            }
            set
            {
                CanWriteProperty("IsVerified", true);
                if (!_isVerified.Equals(value))
                {
                    _isVerified = value;
                    PropertyHasChanged("IsVerified");
                }
            }
        }

        public bool IsLooked
        {
            get
            {
                CanReadProperty("IsLooked", true);
                return _IsLooked;
            }
            set
            {
                CanWriteProperty("IsLooked", true);
                if (!_IsLooked.Equals(value))
                {
                    _IsLooked = value;
                    PropertyHasChanged("IsLooked");
                }
            }
        }
        
        public bool IsExpired
        {
            get
            {
                CanReadProperty("IsExpired", true);
                return _IsExpired;
            }
            set
            {
                CanWriteProperty("IsExpired", true);
                if (!_IsExpired.Equals(value))
                {
                    _IsExpired = value;
                    PropertyHasChanged("IsExpired");
                }
            }
        }

        public DateTime DateReminder(Guid ID, string type)
        {
            DateTime datereminder = DateTime.MinValue;
            try
            {
                List<RegistrationEmailSent> registrationEmailSentList = RegistrationEmailSentList.GetRegistrationEmailSentList()
                                                                             .Where(x => x.EntryType == type)
                                                                             .OrderByDescending(y => y.DateCreated).ToList();
                List<Registration> registrationList = RegistrationList.GetRegistrationList("", "", "").Where(x => x.Id == ID).ToList();
                List<Entry> entryList = EntryList.GetEntryList(Guid.Empty, ID, "").ToList();
                RegistrationEmailSent registrationEmailSent = (from entry in entryList
                                                               join res in registrationEmailSentList on entry.Id equals res.EntryId
                                                               orderby res.DateCreated descending
                                                               select res).FirstOrDefault();

                if (!(registrationEmailSent.DateCreated == DateTime.MaxValue) && !(registrationEmailSent.DateCreated == DateTime.MinValue))
                    datereminder = registrationEmailSent.DateCreated;
            }
            catch { datereminder = GeneralFunctionEffie2017App.GetDateReminder(ID, type); }
            
            return datereminder;
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
            // Email
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Email", 50));
            //
            // Password
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Password", 50));
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
            // Fax
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Fax", 50));
            //
            // Website
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Website", 100));
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
            // Caaaa
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Caaaa", 100));
            //
            // Apep
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Apep", 100));
            //
            // Afaa
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Afaa", 100));
            //
            // Eprog
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Eprog", 100));
            //
            // EProgCampaign
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EProgCampaign", 100));
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
            //TODO: Define authorization rules in Registration
            //AuthorizationRules.AllowRead("Id", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Email", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Password", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Salutation", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Firstname", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Lastname", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Job", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Contact", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Mobile", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Fax", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Website", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Company", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Address1", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Address2", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("City", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Postal", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Country", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("IsCAAAA", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("IsAPEP", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("IsAFAA", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("IsEProg", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Caaaa", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Apep", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Afaa", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Eprog", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("EProgCampaign", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("IsEmailUpdate", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("Status", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "RegistrationReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "RegistrationReadGroup");

            //AuthorizationRules.AllowWrite("Email", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Password", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Salutation", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Firstname", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Lastname", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Job", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Contact", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Mobile", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Fax", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Website", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Company", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Address1", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Address2", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("City", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Postal", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Country", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("IsCAAAA", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("IsAPEP", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("IsAFAA", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("IsEProg", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Caaaa", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Apep", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Afaa", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Eprog", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("EProgCampaign", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("IsEmailUpdate", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("Status", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "RegistrationWriteGroup");
            //AuthorizationRules.AllowWrite("LastSignIn2", "EntryWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Registration
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Registration
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Registration
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Registration
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("RegistrationDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Registration()
        { /* require use of factory method */ }

        private Registration(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static Registration NewRegistration()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Registration");
            return DataPortal.Create<Registration>();
        }

        public static Registration GetRegistration(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Registration");
            return DataPortal.Fetch<Registration>(new Criteria(id));
        }
        public static Registration GetRegistration(SafeDataReader dr)
        {
            return new Registration(dr);
        }
        public static void DeleteRegistration(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Registration");
            DataPortal.Delete(new Criteria(id));
        }

        public override Registration Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Registration");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Registration");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a Registration");

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
                cm.CommandText = "GetRegistration";

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
            _email = dr.GetString("Email");
            _password = dr.GetString("Password");
            _salutation = dr.GetString("Salutation");
            _firstname = dr.GetString("Firstname");
            _lastname = dr.GetString("Lastname");
            _job = dr.GetString("Job");
            _contact = dr.GetString("Contact");
            _mobile = dr.GetString("Mobile");
            _fax = dr.GetString("Fax");
            _website = dr.GetString("Website");
            _company = dr.GetString("Company");
            _address1 = dr.GetString("Address1");
            _address2 = dr.GetString("Address2");
            _city = dr.GetString("City");
            _postal = dr.GetString("Postal");
            _country = dr.GetString("Country");
            _isCAAAA = dr.GetBoolean("IsCAAAA");
            _isAPEP = dr.GetBoolean("IsAPEP");
            _isAFAA = dr.GetBoolean("IsAFAA");
            _isEProg = dr.GetBoolean("IsEProg");
            _isPromo1 = dr.GetBoolean("IsPromo1");
            _isPromo2 = dr.GetBoolean("IsPromo2");
            _isPromo3 = dr.GetBoolean("IsPromo3");
            _caaaa = dr.GetString("CAAAA");
            _apep = dr.GetString("APEP");
            _afaa = dr.GetString("AFAA");
            _eprog = dr.GetString("Eprog");
            _eProgCampaign = dr.GetString("EProgCampaign");
            _LogPassword = dr.GetString("LogPassword");
            _isEmailUpdate = dr.GetBoolean("IsEmailUpdate");
            _isActive = dr.GetBoolean("IsActive");
            _status = dr.GetString("Status");
            _lastSignIn = dr.GetSmartDate("LastSignIn", _lastSignIn.EmptyIsMin);
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _lastSignIn2 = dr.GetSmartDate("LastSignIn2", _lastSignIn2.EmptyIsMin);
            _isVerified = dr.GetBoolean("IsVerified");
            _IsExpired = dr.GetBoolean("IsExpired");
            _IsLooked = dr.GetBoolean("IsLooked");
            
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
                cm.CommandText = "AddRegistration";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

               // _id = (Guid)cm.Parameters["@NewId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {
            
                cm.Parameters.AddWithValue("@Email", _email);           
                cm.Parameters.AddWithValue("@Password", _password);            
                cm.Parameters.AddWithValue("@Salutation", _salutation);           
                cm.Parameters.AddWithValue("@Firstname", _firstname);           
                cm.Parameters.AddWithValue("@Lastname", _lastname);            
                cm.Parameters.AddWithValue("@Job", _job);           
                cm.Parameters.AddWithValue("@Contact", _contact);           
                cm.Parameters.AddWithValue("@Mobile", _mobile);           
                cm.Parameters.AddWithValue("@Fax", _fax);           
                cm.Parameters.AddWithValue("@Website", _website);           
                cm.Parameters.AddWithValue("@Company", _company);            
                cm.Parameters.AddWithValue("@Address1", _address1);           
                cm.Parameters.AddWithValue("@Address2", _address2);            
                cm.Parameters.AddWithValue("@City", _city);           
                cm.Parameters.AddWithValue("@Postal", _postal);           
                cm.Parameters.AddWithValue("@Country", _country);            
                cm.Parameters.AddWithValue("@IsCAAAA", _isCAAAA);          
                cm.Parameters.AddWithValue("@IsAPEP", _isAPEP);           
                cm.Parameters.AddWithValue("@IsAFAA", _isAFAA);            
                cm.Parameters.AddWithValue("@IsEProg", _isEProg);           
                cm.Parameters.AddWithValue("@IsPromo1", _isPromo1);           
                cm.Parameters.AddWithValue("@IsPromo2", _isPromo2);           
                cm.Parameters.AddWithValue("@IsPromo3", _isPromo3);           
                cm.Parameters.AddWithValue("@CAAAA", _caaaa);           
                cm.Parameters.AddWithValue("@APEP", _apep);           
                cm.Parameters.AddWithValue("@AFAA", _afaa);           
                cm.Parameters.AddWithValue("@Eprog", _eprog);
                cm.Parameters.AddWithValue("@LogPassword", _LogPassword);
                cm.Parameters.AddWithValue("@EProgCampaign", _eProgCampaign);
                cm.Parameters.AddWithValue("@IsEmailUpdate", _isEmailUpdate);
                cm.Parameters.AddWithValue("@IsActive", _isActive);
                cm.Parameters.AddWithValue("@Status", _status);
                cm.Parameters.AddWithValue("@LastSignIn", _lastSignIn.DBValue);
                cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
                cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
                cm.Parameters.AddWithValue("@LastSignIn2", _lastSignIn2.DBValue);
                cm.Parameters.AddWithValue("@IsVerified", _isVerified);
                cm.Parameters.AddWithValue("@IsExpired", _IsExpired);
                cm.Parameters.AddWithValue("@IsLooked", _IsLooked);


            cm.Parameters.AddWithValue("@Id", _id);
                //cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
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
                cm.CommandText = "UpdateRegistration";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);          
            cm.Parameters.AddWithValue("@Email", _email);           
            cm.Parameters.AddWithValue("@Password", _password);           
            cm.Parameters.AddWithValue("@Salutation", _salutation);          
            cm.Parameters.AddWithValue("@Firstname", _firstname);           
            cm.Parameters.AddWithValue("@Lastname", _lastname);          
            cm.Parameters.AddWithValue("@Job", _job);            
            cm.Parameters.AddWithValue("@Contact", _contact);          
            cm.Parameters.AddWithValue("@Mobile", _mobile);          
            cm.Parameters.AddWithValue("@Fax", _fax);        
            cm.Parameters.AddWithValue("@Website", _website);           
            cm.Parameters.AddWithValue("@Company", _company);           
            cm.Parameters.AddWithValue("@Address1", _address1);         
            cm.Parameters.AddWithValue("@Address2", _address2);           
            cm.Parameters.AddWithValue("@City", _city);          
            cm.Parameters.AddWithValue("@Postal", _postal);            
            cm.Parameters.AddWithValue("@Country", _country);            
            cm.Parameters.AddWithValue("@IsCAAAA", _isCAAAA);           
            cm.Parameters.AddWithValue("@IsAPEP", _isAPEP);           
            cm.Parameters.AddWithValue("@IsAFAA", _isAFAA);           
            cm.Parameters.AddWithValue("@IsEProg", _isEProg);
            cm.Parameters.AddWithValue("@IsPromo1", _isPromo1);
            cm.Parameters.AddWithValue("@IsPromo2", _isPromo2);
            cm.Parameters.AddWithValue("@IsPromo3", _isPromo3);           
            cm.Parameters.AddWithValue("@CAAAA", _caaaa);            
            cm.Parameters.AddWithValue("@APEP", _apep);            
            cm.Parameters.AddWithValue("@AFAA", _afaa);         
            cm.Parameters.AddWithValue("@Eprog", _eprog);
            cm.Parameters.AddWithValue("@EProgCampaign", _eProgCampaign);
            cm.Parameters.AddWithValue("@LogPassword", _LogPassword);
            cm.Parameters.AddWithValue("@IsEmailUpdate", _isEmailUpdate);
            cm.Parameters.AddWithValue("@IsActive", _isActive);    
            cm.Parameters.AddWithValue("@Status", _status);
            cm.Parameters.AddWithValue("@LastSignIn", _lastSignIn.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@LastSignIn2", _lastSignIn2.DBValue);
            cm.Parameters.AddWithValue("@IsVerified", _isVerified);
            cm.Parameters.AddWithValue("@IsExpired", _IsExpired);
            cm.Parameters.AddWithValue("@IsLooked", _IsLooked);
            
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
                cm.CommandText = "DeleteRegistration";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }
        #endregion //Data Access - Delete
        #endregion //Data Access
    }
}
