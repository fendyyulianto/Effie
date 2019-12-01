using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;
using System.Linq;

namespace Effie2017.App
{
    [Serializable()]
    public class Entry : Csla.BusinessBase<Entry>
    {
        #region Business Properties and Methods

        //declare members
        private Guid _id = Guid.NewGuid();
        private string _serial = string.Empty;
        private Guid _registrationId = Guid.NewGuid();
        private string _campaign = string.Empty;
        private string _client = string.Empty;
        private string _brand = string.Empty;
        private string _categoryMarket = string.Empty;
        private string _categoryPS = string.Empty;
        private string _categoryPSDetail = string.Empty;
        private SmartDate _dateCampaignStart = new SmartDate(true);
        private SmartDate _dateCampaignEnd = new SmartDate(false);
        private string _effectiveness = string.Empty;
        private string _repSalutation = string.Empty;
        private string _repFirstname = string.Empty;
        private string _repLastname = string.Empty;
        private string _repJob = string.Empty;
        private string _repCompany = string.Empty;
        private string _repContact = string.Empty;
        private string _repMobile = string.Empty;
        private string _repEmail = string.Empty;
        private string _summary = string.Empty;
        private string _caseData = string.Empty;
        private string _permission = string.Empty;
        private string _name = string.Empty;
        private string _title = string.Empty;
        private string _company = string.Empty;
        private decimal _amount = 0;
        private decimal _fee = 0;
        private decimal _tax = 0;
        private decimal _grandAmount = 0;
        private Guid _payGroupId = Guid.Empty;
        private string _paymentMethod = string.Empty;
        private string _payCompany = string.Empty;
        private string _payAddress1 = string.Empty;
        private string _payAddress2 = string.Empty;
        private string _payCity = string.Empty;
        private string _payPostal = string.Empty;
        private string _payCountry = string.Empty;
        private string _payFirstname = string.Empty;
        private string _payLastname = string.Empty;
        private string _payContact = string.Empty;
        private bool _isUploadForm = false;
        private bool _isUploadAuthorizationForm = false;
        private bool _isUploadCaseImage = false;
        private bool _isUploadCreativeMaterials = false;
        private string _status = string.Empty;
        private string _payStatus = string.Empty;
        private string _productClassification = string.Empty;
        private string _productClassificationOthers = string.Empty;
        private string _entryObjective = string.Empty;
        private string _entryObjectiveOthers = string.Empty;
        private string _entryObjective2 = string.Empty;
        private string _targetAudience = string.Empty;
        private string _targetAudienceOthers = string.Empty;
        private string _targetAudiencePri = string.Empty;
        private string _targetAudiencePriOthers = string.Empty;

        private string _heroTouchPoint = string.Empty;
        private string _heroTouchPoint2 = string.Empty;
        private string _heroTouchPoint3 = string.Empty;
        private string _heroTouchPointOthers = string.Empty;
        private string _heroTouchPointOthers2 = string.Empty;
        private string _heroTouchPointOthers3 = string.Empty;

        private string _socialPlatforms = string.Empty;
        private string _socialPlatformsOthers = string.Empty;



        private string _research = string.Empty;
        private string _researchImp = string.Empty;
        private string _sDGData1 = string.Empty;
        private string _sDGData2 = string.Empty;
        private string _invoice = string.Empty;
        private string _creativeUploadType = string.Empty;
        private decimal _amountReceived = 0;
        private string _withdrawnStatus = string.Empty;
        private SmartDate _lastSendPaidEmailDate = new SmartDate(false);
        private SmartDate _lastSendUploadReminderEmailDate = new SmartDate(false);
        private SmartDate _lastSendCompletionReminderEmailDate = new SmartDate(false);
        private SmartDate _lastSendPaymentReminderEmailDate = new SmartDate(false);
        private SmartDate _lastSendSubmissionReminderEmailDate = new SmartDate(false);
        private SmartDate _dateSubmitted = new SmartDate(false);
        private SmartDate _dateCreated = new SmartDate(false);
        private SmartDate _dateModified = new SmartDate(false);
        private int _isReminded = 0;
        private bool _isRound2 = false;
        private string _categoryMarketR2 = string.Empty;
        private string _categoryPSR2 = string.Empty;
        private string _categoryPSDetailR2 = string.Empty;
        private bool _isMaterialsVerified = false;
        private bool _isVideoDownloaded = false;
        private bool _isCampaignOngoing = false;
        /////////////////////// NEW /////////////////////////
        private string _processingStatus = string.Empty;
        private Guid _adminidAssignedto = Guid.Empty;
        private string _materialsSubmitted = string.Empty;
        private string _dQFlag = string.Empty;
        private string _notificationSentDate = string.Empty;
        private string _reopeningDate = string.Empty;
        private string _reopeningDeadline = string.Empty;
        private string _flagReason = string.Empty;
        private string _flagDQDescription = string.Empty;
        private string _reopenedBy = string.Empty;
        private string _otherRemarks = string.Empty;
        private string _dateVerified = string.Empty;
        private string _reasonFeeWaiver = string.Empty;
        private string _researchOther = string.Empty;
        private string _iDAdhocInvoice = string.Empty;
        
        /////////////////////// NEW /////////////////////////
        private decimal _amountPlusFee = 0;

        private string _panelId = string.Empty;

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

        public string Serial
        {
            get
            {
                CanReadProperty("Serial", true);
                return _serial;
            }
            set
            {
                CanWriteProperty("Serial", true);
                if (value == null) value = string.Empty;
                if (!_serial.Equals(value))
                {
                    _serial = value;
                    PropertyHasChanged("Serial");
                }
            }
        }

        public Guid RegistrationId
        {
            get
            {
                CanReadProperty("RegistrationId", true);
                return _registrationId;
            }
            set
            {
                CanWriteProperty("RegistrationId", true);
                if (value == null) value = Guid.Empty;
                if (!_registrationId.Equals(value))
                {
                    _registrationId = value;
                    PropertyHasChanged("RegistrationId");
                }
            }
        }

        public string Campaign
        {
            get
            {
                CanReadProperty("Campaign", true);
                return _campaign;
            }
            set
            {
                CanWriteProperty("Campaign", true);
                if (value == null) value = string.Empty;
                if (!_campaign.Equals(value))
                {
                    _campaign = value;
                    PropertyHasChanged("Campaign");
                }
            }
        }

        public string Client
        {
            get
            {
                CanReadProperty("Client", true);
                return _client;
            }
            set
            {
                CanWriteProperty("Client", true);
                if (value == null) value = string.Empty;
                if (!_client.Equals(value))
                {
                    _client = value;
                    PropertyHasChanged("Client");
                }
            }
        }

        public string Brand
        {
            get
            {
                CanReadProperty("Brand", true);
                return _brand;
            }
            set
            {
                CanWriteProperty("Brand", true);
                if (value == null) value = string.Empty;
                if (!_brand.Equals(value))
                {
                    _brand = value;
                    PropertyHasChanged("Brand");
                }
            }
        }

        public string CategoryMarket
        {
            get
            {
                CanReadProperty("CategoryMarket", true);
                return _categoryMarket;
            }
            set
            {
                CanWriteProperty("CategoryMarket", true);
                if (value == null) value = string.Empty;
                if (!_categoryMarket.Equals(value))
                {
                    _categoryMarket = value;
                    PropertyHasChanged("CategoryMarket");
                }
            }
        }

        public string CategoryPS
        {
            get
            {
                CanReadProperty("CategoryPS", true);
                return _categoryPS;
            }
            set
            {
                CanWriteProperty("CategoryPS", true);
                if (value == null) value = string.Empty;
                if (!_categoryPS.Equals(value))
                {
                    _categoryPS = value;
                    PropertyHasChanged("CategoryPS");
                }
            }
        }

        public string CategoryPSDetail
        {
            get
            {
                CanReadProperty("CategoryPSDetail", true);
                return _categoryPSDetail;
            }
            set
            {
                CanWriteProperty("CategoryPSDetail", true);
                if (value == null) value = string.Empty;
                if (!_categoryPSDetail.Equals(value))
                {
                    _categoryPSDetail = value;
                    PropertyHasChanged("CategoryPSDetail");
                }
            }
        }

        public DateTime DateCampaignStart
        {
            get
            {
                CanReadProperty("DateCampaignStart", true);
                return _dateCampaignStart.Date;
            }
        }

        public string DateCampaignStartString
        {
            get
            {
                CanReadProperty("DateCampaignStart", true);
                return _dateCampaignStart.Text;
            }
            set
            {
                CanWriteProperty("DateCampaignStart", true);
                if (value == null) value = string.Empty;
                if (!_dateCampaignStart.Equals(value))
                {
                    _dateCampaignStart.Text = value;
                    PropertyHasChanged("DateCampaignStart");
                }
            }
        }

        public DateTime DateCampaignEnd
        {
            get
            {
                CanReadProperty("DateCampaignEnd", true);
                return _dateCampaignEnd.Date;
            }
        }

        public string DateCampaignEndString
        {
            get
            {
                CanReadProperty("DateCampaignEnd", true);
                return _dateCampaignEnd.Text;
            }
            set
            {
                CanWriteProperty("DateCampaignEnd", true);
                if (value == null) value = string.Empty;
                if (!_dateCampaignEnd.Equals(value))
                {
                    _dateCampaignEnd.Text = value;
                    PropertyHasChanged("DateCampaignEnd");
                }
            }
        }

        public string Effectiveness
        {
            get
            {
                CanReadProperty("Effectiveness", true);
                return _effectiveness;
            }
            set
            {
                CanWriteProperty("Effectiveness", true);
                if (value == null) value = string.Empty;
                if (!_effectiveness.Equals(value))
                {
                    _effectiveness = value;
                    PropertyHasChanged("Effectiveness");
                }
            }
        }

        public string RepSalutation
        {
            get
            {
                CanReadProperty("RepSalutation", true);
                return _repSalutation;
            }
            set
            {
                CanWriteProperty("RepSalutation", true);
                if (value == null) value = string.Empty;
                if (!_repSalutation.Equals(value))
                {
                    _repSalutation = value;
                    PropertyHasChanged("RepSalutation");
                }
            }
        }

        public string RepFirstname
        {
            get
            {
                CanReadProperty("RepFirstname", true);
                return _repFirstname;
            }
            set
            {
                CanWriteProperty("RepFirstname", true);
                if (value == null) value = string.Empty;
                if (!_repFirstname.Equals(value))
                {
                    _repFirstname = value;
                    PropertyHasChanged("RepFirstname");
                }
            }
        }

        public string RepLastname
        {
            get
            {
                CanReadProperty("RepLastname", true);
                return _repLastname;
            }
            set
            {
                CanWriteProperty("RepLastname", true);
                if (value == null) value = string.Empty;
                if (!_repLastname.Equals(value))
                {
                    _repLastname = value;
                    PropertyHasChanged("RepLastname");
                }
            }
        }

        public string RepJob
        {
            get
            {
                CanReadProperty("RepJob", true);
                return _repJob;
            }
            set
            {
                CanWriteProperty("RepJob", true);
                if (value == null) value = string.Empty;
                if (!_repJob.Equals(value))
                {
                    _repJob = value;
                    PropertyHasChanged("RepJob");
                }
            }
        }

        public string RepCompany
        {
            get
            {
                CanReadProperty("RepCompany", true);
                return _repCompany;
            }
            set
            {
                CanWriteProperty("RepCompany", true);
                if (value == null) value = string.Empty;
                if (!_repCompany.Equals(value))
                {
                    _repCompany = value;
                    PropertyHasChanged("RepCompany");
                }
            }
        }

        public string RepContact
        {
            get
            {
                CanReadProperty("RepContact", true);
                return _repContact;
            }
            set
            {
                CanWriteProperty("RepContact", true);
                if (value == null) value = string.Empty;
                if (!_repContact.Equals(value))
                {
                    _repContact = value;
                    PropertyHasChanged("RepContact");
                }
            }
        }

        public string RepMobile
        {
            get
            {
                CanReadProperty("RepMobile", true);
                return _repMobile;
            }
            set
            {
                CanWriteProperty("RepMobile", true);
                if (value == null) value = string.Empty;
                if (!_repMobile.Equals(value))
                {
                    _repMobile = value;
                    PropertyHasChanged("RepMobile");
                }
            }
        }

        public string RepEmail
        {
            get
            {
                CanReadProperty("RepEmail", true);
                return _repEmail;
            }
            set
            {
                CanWriteProperty("RepEmail", true);
                if (value == null) value = string.Empty;
                if (!_repEmail.Equals(value))
                {
                    _repEmail = value;
                    PropertyHasChanged("RepEmail");
                }
            }
        }

        public string Summary
        {
            get
            {
                CanReadProperty("Summary", true);
                return _summary;
            }
            set
            {
                CanWriteProperty("Summary", true);
                if (value == null) value = string.Empty;
                if (!_summary.Equals(value))
                {
                    _summary = value;
                    PropertyHasChanged("Summary");
                }
            }
        }

        public string CaseData
        {
            get
            {
                CanReadProperty("CaseData", true);
                return _caseData;
            }
            set
            {
                CanWriteProperty("CaseData", true);
                if (value == null) value = string.Empty;
                if (!_caseData.Equals(value))
                {
                    _caseData = value;
                    PropertyHasChanged("CaseData");
                }
            }
        }

        public string Permission
        {
            get
            {
                CanReadProperty("Permission", true);
                return _permission;
            }
            set
            {
                CanWriteProperty("Permission", true);
                if (value == null) value = string.Empty;
                if (!_permission.Equals(value))
                {
                    _permission = value;
                    PropertyHasChanged("Permission");
                }
            }
        }

        public string Name
        {
            get
            {
                CanReadProperty("Name", true);
                return _name;
            }
            set
            {
                CanWriteProperty("Name", true);
                if (value == null) value = string.Empty;
                if (!_name.Equals(value))
                {
                    _name = value;
                    PropertyHasChanged("Name");
                }
            }
        }

        public string Title
        {
            get
            {
                CanReadProperty("Title", true);
                return _title;
            }
            set
            {
                CanWriteProperty("Title", true);
                if (value == null) value = string.Empty;
                if (!_title.Equals(value))
                {
                    _title = value;
                    PropertyHasChanged("Title");
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

        public decimal Amount
        {
            get
            {
                CanReadProperty("Amount", true);
                return _amount;
            }
            set
            {
                CanWriteProperty("Amount", true);
                if (!_amount.Equals(value))
                {
                    _amount = value;
                    PropertyHasChanged("Amount");
                }
            }
        }

        public decimal Fee
        {
            get
            {
                CanReadProperty("Fee", true);
                return _fee;
            }
            set
            {
                CanWriteProperty("Fee", true);
                if (!_fee.Equals(value))
                {
                    _fee = value;
                    PropertyHasChanged("Fee");
                }
            }
        }

        public decimal Tax
        {
            get
            {
                CanReadProperty("Tax", true);
                return _tax;
            }
            set
            {
                CanWriteProperty("Tax", true);
                if (!_tax.Equals(value))
                {
                    _tax = value;
                    PropertyHasChanged("Tax");
                }
            }
        }

        public decimal GrandAmount
        {
            get
            {
                CanReadProperty("GrandAmount", true);
                return _grandAmount;
            }
            set
            {
                CanWriteProperty("GrandAmount", true);
                if (!_grandAmount.Equals(value))
                {
                    _grandAmount = value;
                    PropertyHasChanged("GrandAmount");
                }
            }
        }

        public Guid PayGroupId
        {
            get
            {
                CanReadProperty("PayGroupId", true);
                return _payGroupId;
            }
            set
            {
                CanWriteProperty("PayGroupId", true);
                if (value == null) value = Guid.Empty;
                if (!_payGroupId.Equals(value))
                {
                    _payGroupId = value;
                    PropertyHasChanged("PayGroupId");
                }
            }
        }

        public string PaymentMethod
        {
            get
            {
                CanReadProperty("PaymentMethod", true);
                return _paymentMethod;
            }
            set
            {
                CanWriteProperty("PaymentMethod", true);
                if (value == null) value = string.Empty;
                if (!_paymentMethod.Equals(value))
                {
                    _paymentMethod = value;
                    PropertyHasChanged("PaymentMethod");
                }
            }
        }

        public string PayCompany
        {
            get
            {
                CanReadProperty("PayCompany", true);
                return _payCompany;
            }
            set
            {
                CanWriteProperty("PayCompany", true);
                if (value == null) value = string.Empty;
                if (!_payCompany.Equals(value))
                {
                    _payCompany = value;
                    PropertyHasChanged("PayCompany");
                }
            }
        }

        public string PayAddress1
        {
            get
            {
                CanReadProperty("PayAddress1", true);
                return _payAddress1;
            }
            set
            {
                CanWriteProperty("PayAddress1", true);
                if (value == null) value = string.Empty;
                if (!_payAddress1.Equals(value))
                {
                    _payAddress1 = value;
                    PropertyHasChanged("PayAddress1");
                }
            }
        }

        public string PayAddress2
        {
            get
            {
                CanReadProperty("PayAddress2", true);
                return _payAddress2;
            }
            set
            {
                CanWriteProperty("PayAddress2", true);
                if (value == null) value = string.Empty;
                if (!_payAddress2.Equals(value))
                {
                    _payAddress2 = value;
                    PropertyHasChanged("PayAddress2");
                }
            }
        }

        public string PayCity
        {
            get
            {
                CanReadProperty("PayCity", true);
                return _payCity;
            }
            set
            {
                CanWriteProperty("PayCity", true);
                if (value == null) value = string.Empty;
                if (!_payCity.Equals(value))
                {
                    _payCity = value;
                    PropertyHasChanged("PayCity");
                }
            }
        }

        public string PayPostal
        {
            get
            {
                CanReadProperty("PayPostal", true);
                return _payPostal;
            }
            set
            {
                CanWriteProperty("PayPostal", true);
                if (value == null) value = string.Empty;
                if (!_payPostal.Equals(value))
                {
                    _payPostal = value;
                    PropertyHasChanged("PayPostal");
                }
            }
        }

        public string PayCountry
        {
            get
            {
                CanReadProperty("PayCountry", true);
                return _payCountry;
            }
            set
            {
                CanWriteProperty("PayCountry", true);
                if (value == null) value = string.Empty;
                if (!_payCountry.Equals(value))
                {
                    _payCountry = value;
                    PropertyHasChanged("PayCountry");
                }
            }
        }

        public string PayFirstname
        {
            get
            {
                CanReadProperty("PayFirstname", true);
                return _payFirstname;
            }
            set
            {
                CanWriteProperty("PayFirstname", true);
                if (value == null) value = string.Empty;
                if (!_payFirstname.Equals(value))
                {
                    _payFirstname = value;
                    PropertyHasChanged("PayFirstname");
                }
            }
        }

        public string PayLastname
        {
            get
            {
                CanReadProperty("PayLastname", true);
                return _payLastname;
            }
            set
            {
                CanWriteProperty("PayLastname", true);
                if (value == null) value = string.Empty;
                if (!_payLastname.Equals(value))
                {
                    _payLastname = value;
                    PropertyHasChanged("PayLastname");
                }
            }
        }

        public string PayContact
        {
            get
            {
                CanReadProperty("PayContact", true);
                return _payContact;
            }
            set
            {
                CanWriteProperty("PayContact", true);
                if (value == null) value = string.Empty;
                if (!_payContact.Equals(value))
                {
                    _payContact = value;
                    PropertyHasChanged("PayContact");
                }
            }
        }

        public bool IsUploadForm
        {
            get
            {
                CanReadProperty("IsUploadForm", true);
                return _isUploadForm;
            }
            set
            {
                CanWriteProperty("IsUploadForm", true);
                if (!_isUploadForm.Equals(value))
                {
                    _isUploadForm = value;
                    PropertyHasChanged("IsUploadForm");
                }
            }
        }

        public bool IsUploadAuthorizationForm
        {
            get
            {
                CanReadProperty("IsUploadAuthorizationForm", true);
                return _isUploadAuthorizationForm;
            }
            set
            {
                CanWriteProperty("IsUploadAuthorizationForm", true);
                if (!_isUploadAuthorizationForm.Equals(value))
                {
                    _isUploadAuthorizationForm = value;
                    PropertyHasChanged("IsUploadAuthorizationForm");
                }
            }
        }

        public bool IsUploadCaseImage
        {
            get
            {
                CanReadProperty("IsUploadCaseImage", true);
                return _isUploadCaseImage;
            }
            set
            {
                CanWriteProperty("IsUploadCaseImage", true);
                if (!_isUploadCaseImage.Equals(value))
                {
                    _isUploadCaseImage = value;
                    PropertyHasChanged("IsUploadCaseImage");
                }
            }
        }

        public bool IsUploadCreativeMaterials
        {
            get
            {
                CanReadProperty("IsUploadCreativeMaterials", true);
                return _isUploadCreativeMaterials;
            }
            set
            {
                CanWriteProperty("IsUploadCreativeMaterials", true);
                if (!_isUploadCreativeMaterials.Equals(value))
                {
                    _isUploadCreativeMaterials = value;
                    PropertyHasChanged("IsUploadCreativeMaterials");
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

        public string PayStatus
        {
            get
            {
                CanReadProperty("PayStatus", true);
                return _payStatus;
            }
            set
            {
                CanWriteProperty("PayStatus", true);
                if (value == null) value = string.Empty;
                if (!_payStatus.Equals(value))
                {
                    _payStatus = value;
                    PropertyHasChanged("PayStatus");
                }
            }
        }

        public string ProductClassification
        {
            get
            {
                CanReadProperty("ProductClassification", true);
                return _productClassification;
            }
            set
            {
                CanWriteProperty("ProductClassification", true);
                if (value == null) value = string.Empty;
                if (!_productClassification.Equals(value))
                {
                    _productClassification = value;
                    PropertyHasChanged("ProductClassification");
                }
            }
        }
        public string ProductClassificationOthers
        {
            get
            {
                CanReadProperty("ProductClassificationOthers", true);
                return _productClassificationOthers;
            }
            set
            {
                CanWriteProperty("ProductClassificationOthers", true);
                if (value == null) value = string.Empty;
                if (!_productClassificationOthers.Equals(value))
                {
                    _productClassificationOthers = value;
                    PropertyHasChanged("ProductClassificationOthers");
                }
            }
        }
        public string EntryObjective
        {
            get
            {
                CanReadProperty("EntryObjective", true);
                return _entryObjective;
            }
            set
            {
                CanWriteProperty("EntryObjective", true);
                if (value == null) value = string.Empty;
                if (!_entryObjective.Equals(value))
                {
                    _entryObjective = value;
                    PropertyHasChanged("EntryObjective");
                }
            }
        }
        public string EntryObjectiveOthers
        {
            get
            {
                CanReadProperty("EntryObjectiveOthers", true);
                return _entryObjectiveOthers;
            }
            set
            {
                CanWriteProperty("EntryObjectiveOthers", true);
                if (value == null) value = string.Empty;
                if (!_entryObjectiveOthers.Equals(value))
                {
                    _entryObjectiveOthers = value;
                    PropertyHasChanged("EntryObjectiveOthers");
                }
            }
        }

        public string EntryObjective2
        {
            get
            {
                CanReadProperty("EntryObjective2", true);
                return _entryObjective2;
            }
            set
            {
                CanWriteProperty("EntryObjective2", true);
                if (value == null) value = string.Empty;
                if (!_entryObjective2.Equals(value))
                {
                    _entryObjective2 = value;
                    PropertyHasChanged("EntryObjective2");
                }
            }
        }

        public string TargetAudience
        {
            get
            {
                CanReadProperty("TargetAudience", true);
                return _targetAudience;
            }
            set
            {
                CanWriteProperty("TargetAudience", true);
                if (value == null) value = string.Empty;
                if (!_targetAudience.Equals(value))
                {
                    _targetAudience = value;
                    PropertyHasChanged("TargetAudience");
                }
            }
        }

        public string TargetAudienceOthers
        {
            get
            {
                CanReadProperty("TargetAudienceOthers", true);
                return _targetAudienceOthers;
            }
            set
            {
                CanWriteProperty("TargetAudienceOthers", true);
                if (value == null) value = string.Empty;
                if (!_targetAudienceOthers.Equals(value))
                {
                    _targetAudienceOthers = value;
                    PropertyHasChanged("TargetAudienceOthers");
                }
            }
        }

        public string TargetAudiencePri
        {
            get
            {
                CanReadProperty("TargetAudiencePri", true);
                return _targetAudiencePri;
            }
            set
            {
                CanWriteProperty("TargetAudiencePri", true);
                if (value == null) value = string.Empty;
                if (!_targetAudiencePri.Equals(value))
                {
                    _targetAudiencePri = value;
                    PropertyHasChanged("TargetAudiencePri");
                }
            }
        }

        public string TargetAudiencePriOthers
        {
            get
            {
                CanReadProperty("TargetAudiencePriOthers", true);
                return _targetAudiencePriOthers;
            }
            set
            {
                CanWriteProperty("TargetAudiencePriOthers", true);
                if (value == null) value = string.Empty;
                if (!_targetAudiencePriOthers.Equals(value))
                {
                    _targetAudiencePriOthers = value;
                    PropertyHasChanged("TargetAudiencePriOthers");
                }
            }
        }

        public string HeroTouchPoint
        {
            get
            {
                CanReadProperty("HeroTouchPoint", true);
                return _heroTouchPoint;
            }
            set
            {
                CanWriteProperty("HeroTouchPoint", true);
                if (value == null) value = string.Empty;
                if (!_heroTouchPoint.Equals(value))
                {
                    _heroTouchPoint = value;
                    PropertyHasChanged("HeroTouchPoint");
                }
            }
        }

        public string HeroTouchPoint2
        {
            get
            {
                CanReadProperty("HeroTouchPoint2", true);
                return _heroTouchPoint2;
            }
            set
            {
                CanWriteProperty("HeroTouchPoint2", true);
                if (value == null) value = string.Empty;
                if (!_heroTouchPoint2.Equals(value))
                {
                    _heroTouchPoint2 = value;
                    PropertyHasChanged("HeroTouchPoint2");
                }
            }
        }

        public string HeroTouchPoint3
        {
            get
            {
                CanReadProperty("HeroTouchPoint3", true);
                return _heroTouchPoint3;
            }
            set
            {
                CanWriteProperty("HeroTouchPoint3", true);
                if (value == null) value = string.Empty;
                if (!_heroTouchPoint3.Equals(value))
                {
                    _heroTouchPoint3 = value;
                    PropertyHasChanged("HeroTouchPoint3");
                }
            }
        }

        public string HeroTouchPointOthers
        {
            get
            {
                CanReadProperty("HeroTouchPointOthers", true);
                return _heroTouchPointOthers;
            }
            set
            {
                CanWriteProperty("HeroTouchPointOthers", true);
                if (value == null) value = string.Empty;
                if (!_heroTouchPointOthers.Equals(value))
                {
                    _heroTouchPointOthers = value;
                    PropertyHasChanged("HeroTouchPointOthers");
                }
            }
        }

        public string HeroTouchPointOthers2
        {
            get
            {
                CanReadProperty("HeroTouchPointOthers2", true);
                return _heroTouchPointOthers2;
            }
            set
            {
                CanWriteProperty("HeroTouchPointOthers2", true);
                if (value == null) value = string.Empty;
                if (!_heroTouchPointOthers2.Equals(value))
                {
                    _heroTouchPointOthers2 = value;
                    PropertyHasChanged("HeroTouchPointOthers2");
                }
            }
        }

        public string HeroTouchPointOthers3
        {
            get
            {
                CanReadProperty("HeroTouchPointOthers3", true);
                return _heroTouchPointOthers3;
            }
            set
            {
                CanWriteProperty("HeroTouchPointOthers3", true);
                if (value == null) value = string.Empty;
                if (!_heroTouchPointOthers3.Equals(value))
                {
                    _heroTouchPointOthers3 = value;
                    PropertyHasChanged("HeroTouchPointOthers3");
                }
            }
        }

        public string SocialPlatforms
        {
            get
            {
                CanReadProperty("SocialPlatforms", true);
                return _socialPlatforms;
            }
            set
            {
                CanWriteProperty("SocialPlatforms", true);
                if (value == null) value = string.Empty;
                if (!_socialPlatforms.Equals(value))
                {
                    _socialPlatforms = value;
                    PropertyHasChanged("SocialPlatforms");
                }
            }
        }

        public string SocialPlatformsOthers
        {
            get
            {
                CanReadProperty("SocialPlatformsOthers", true);
                return _socialPlatformsOthers;
            }
            set
            {
                CanWriteProperty("SocialPlatformsOthers", true);
                if (value == null) value = string.Empty;
                if (!_socialPlatformsOthers.Equals(value))
                {
                    _socialPlatformsOthers = value;
                    PropertyHasChanged("SocialPlatformsOthers");
                }
            }
        }

        public string Research
        {
            get
            {
                CanReadProperty("Research", true);
                return _research;
            }
            set
            {
                CanWriteProperty("Research", true);
                if (value == null) value = string.Empty;
                if (!_research.Equals(value))
                {
                    _research = value;
                    PropertyHasChanged("Research");
                }
            }
        }

        public string ResearchImp
        {
            get
            {
                CanReadProperty("ResearchImp", true);
                return _researchImp;
            }
            set
            {
                CanWriteProperty("ResearchImp", true);
                if (value == null) value = string.Empty;
                if (!_researchImp.Equals(value))
                {
                    _researchImp = value;
                    PropertyHasChanged("ResearchImp");
                }
            }
        }

        public string SDGData1
        {
            get
            {
                CanReadProperty("SDGData1", true);
                return _sDGData1;
            }
            set
            {
                CanWriteProperty("SDGData1", true);
                if (value == null) value = string.Empty;
                if (!_sDGData1.Equals(value))
                {
                    _sDGData1 = value;
                    PropertyHasChanged("SDGData1");
                }
            }
        }

        public string SDGData2
        {
            get
            {
                CanReadProperty("SDGData2", true);
                return _sDGData2;
            }
            set
            {
                CanWriteProperty("SDGData2", true);
                if (value == null) value = string.Empty;
                if (!_sDGData2.Equals(value))
                {
                    _sDGData2 = value;
                    PropertyHasChanged("SDGData2");
                }
            }
        }

        public string Invoice
        {
            get
            {
                CanReadProperty("Invoice", true);
                return _invoice;
            }
            set
            {
                CanWriteProperty("Invoice", true);
                if (value == null) value = string.Empty;
                if (!_invoice.Equals(value))
                {
                    _invoice = value;
                    PropertyHasChanged("Invoice");
                }
            }
        }

        public string CreativeUploadType
        {
            get
            {
                CanReadProperty("CreativeUploadType", true);
                return _creativeUploadType;
            }
            set
            {
                CanWriteProperty("CreativeUploadType", true);
                if (value == null) value = string.Empty;
                if (!_creativeUploadType.Equals(value))
                {
                    _creativeUploadType = value;
                    PropertyHasChanged("CreativeUploadType");
                }
            }
        }

        public decimal AmountReceived
        {
            get
            {
                CanReadProperty("AmountReceived", true);
                return _amountReceived;
            }
            set
            {
                CanWriteProperty("AmountReceived", true);
                if (!_amountReceived.Equals(value))
                {
                    _amountReceived = value;
                    PropertyHasChanged("AmountReceived");
                }
            }
        }

        public string WithdrawnStatus
        {
            get
            {
                CanReadProperty("WithdrawnStatus", true);
                return _withdrawnStatus;
            }
            set
            {
                CanWriteProperty("WithdrawnStatus", true);
                if (value == null) value = string.Empty;
                if (!_withdrawnStatus.Equals(value))
                {
                    _withdrawnStatus = value;
                    PropertyHasChanged("WithdrawnStatus");
                }
            }
        }

        public DateTime LastSendPaidEmailDate
        {
            get
            {
                CanReadProperty("LastSendPaidEmailDate", true);
                return _lastSendPaidEmailDate.Date;
            }
        }

        public string LastSendPaidEmailDateString
        {
            get
            {
                CanReadProperty("LastSendPaidEmailDate", true);
                return _lastSendPaidEmailDate.Text;
            }
            set
            {
                CanWriteProperty("LastSendPaidEmailDate", true);
                if (value == null) value = string.Empty;
                if (!_lastSendPaidEmailDate.Equals(value))
                {
                    _lastSendPaidEmailDate.Text = value;
                    PropertyHasChanged("LastSendPaidEmailDate");
                }
            }
        }

        public DateTime LastSendUploadReminderEmailDate
        {
            get
            {
                CanReadProperty("LastSendUploadReminderEmailDate", true);
                return _lastSendUploadReminderEmailDate.Date;
            }
        }

        public string LastSendUploadReminderEmailDateString
        {
            get
            {
                CanReadProperty("LastSendUploadReminderEmailDate", true);
                return _lastSendUploadReminderEmailDate.Text;
            }
            set
            {
                CanWriteProperty("LastSendUploadReminderEmailDate", true);
                if (value == null) value = string.Empty;
                if (!_lastSendUploadReminderEmailDate.Equals(value))
                {
                    _lastSendUploadReminderEmailDate.Text = value;
                    PropertyHasChanged("LastSendUploadReminderEmailDate");
                }
            }
        }

        public DateTime LastSendCompletionReminderEmailDate
        {
            get
            {
                CanReadProperty("LastSendCompletionReminderEmailDate", true);
                return _lastSendCompletionReminderEmailDate.Date;
            }
        }

        public string LastSendCompletionReminderEmailDateString
        {
            get
            {
                CanReadProperty("LastSendCompletionReminderEmailDate", true);
                return _lastSendCompletionReminderEmailDate.Text;
            }
            set
            {
                CanWriteProperty("LastSendCompletionReminderEmailDate", true);
                if (value == null) value = string.Empty;
                if (!_lastSendCompletionReminderEmailDate.Equals(value))
                {
                    _lastSendCompletionReminderEmailDate.Text = value;
                    PropertyHasChanged("LastSendCompletionReminderEmailDate");
                }
            }
        }

        public DateTime LastSendPaymentReminderEmailDate
        {
            get
            {
                CanReadProperty("LastSendPaymentReminderEmailDate", true);
                return _lastSendPaymentReminderEmailDate.Date;
            }
        }

        public string LastSendPaymentReminderEmailDateString
        {
            get
            {
                CanReadProperty("LastSendPaymentReminderEmailDate", true);
                return _lastSendPaymentReminderEmailDate.Text;
            }
            set
            {
                CanWriteProperty("LastSendPaymentReminderEmailDate", true);
                if (value == null) value = string.Empty;
                if (!_lastSendPaymentReminderEmailDate.Equals(value))
                {
                    _lastSendPaymentReminderEmailDate.Text = value;
                    PropertyHasChanged("LastSendPaymentReminderEmailDate");
                }
            }
        }

        public DateTime LastSendSubmissionReminderEmailDate
        {
            get
            {
                CanReadProperty("LastSendSubmissionReminderEmailDateString", true);
                return _lastSendSubmissionReminderEmailDate.Date;
            }
        }

        public string LastSendSubmissionReminderEmailDateString
        {
            get
            {
                CanReadProperty("LastSendSubmissionReminderEmailDateString", true);
                return _lastSendSubmissionReminderEmailDate.Text;
            }
            set
            {
                CanWriteProperty("LastSendSubmissionReminderEmailDateString", true);
                if (value == null) value = string.Empty;
                if (!_lastSendSubmissionReminderEmailDate.Equals(value))
                {
                    _lastSendSubmissionReminderEmailDate.Text = value;
                    PropertyHasChanged("LastSendSubmissionReminderEmailDateString");
                }
            }
        }

        public DateTime DateSubmitted
        {
            get
            {
                CanReadProperty("DateSubmitted", true);
                return _dateSubmitted.Date;
            }
        }

        public string DateSubmittedString
        {
            get
            {
                CanReadProperty("DateSubmitted", true);
                return _dateSubmitted.Text;
            }
            set
            {
                CanWriteProperty("DateSubmitted", true);
                if (value == null) value = string.Empty;
                if (!_dateSubmitted.Equals(value))
                {
                    _dateSubmitted.Text = value;
                    PropertyHasChanged("DateSubmitted");
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

        public int IsReminded
        {
            get
            {
                CanReadProperty("IsReminded", true);
                return _isReminded;
            }
            set
            {
                CanWriteProperty("IsReminded", true);
                if (!_isReminded.Equals(value))
                {
                    _isReminded = value;
                    PropertyHasChanged("IsReminded");
                }
            }
        }


        public decimal AmountPlusFee
        {
            get
            {
                return _amount + _fee;
            }
        }

        public decimal AmountBalance
        {
            get
            {
                return _amount + _fee - _amountReceived;
            }
        }

        public decimal AmountBalance2
        {
            get
            {
                return _grandAmount - _amountReceived;
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

        public string CategoryMarketR2
        {
            get
            {
                CanReadProperty("CategoryMarketR2", true);
                return _categoryMarketR2;
            }
            set
            {
                CanWriteProperty("CategoryMarketR2", true);
                if (value == null) value = string.Empty;
                if (!_categoryMarketR2.Equals(value))
                {
                    _categoryMarketR2 = value;
                    PropertyHasChanged("CategoryMarketR2");
                }
            }
        }

        public string CategoryPSR2
        {
            get
            {
                CanReadProperty("CategoryPSR2", true);
                return _categoryPSR2;
            }
            set
            {
                CanWriteProperty("CategoryPSR2", true);
                if (value == null) value = string.Empty;
                if (!_categoryPSR2.Equals(value))
                {
                    _categoryPSR2 = value;
                    PropertyHasChanged("CategoryPSR2");
                }
            }
        }

        public string CategoryPSDetailR2
        {
            get
            {
                CanReadProperty("CategoryPSDetailR2", true);
                return _categoryPSDetailR2;
            }
            set
            {
                CanWriteProperty("CategoryPSDetailR2", true);
                if (value == null) value = string.Empty;
                if (!_categoryPSDetailR2.Equals(value))
                {
                    _categoryPSDetailR2 = value;
                    PropertyHasChanged("CategoryPSDetailR2");
                }
            }
        }

        public bool IsMaterialsVerified
        {
            get
            {
                CanReadProperty("IsMaterialsVerified", true);
                return _isMaterialsVerified;
            }
            set
            {
                CanWriteProperty("IsMaterialsVerified", true);
                if (value == null) value = false;
                if (!_isMaterialsVerified.Equals(value))
                {
                    _isMaterialsVerified = value;
                    PropertyHasChanged("IsMaterialsVerified");
                }
            }
        }

        public bool IsVideoDownloaded
        {
            get
            {
                CanReadProperty("IsVideoDownloaded", true);
                return _isVideoDownloaded;
            }
            set
            {
                CanWriteProperty("IsVideoDownloaded", true);
                if (value == null) value = false;
                if (!_isVideoDownloaded.Equals(value))
                {
                    _isVideoDownloaded = value;
                    PropertyHasChanged("IsVideoDownloaded");
                }
            }
        }

        public bool IsCampaignOngoing
        {
            get
            {
                CanReadProperty("IsCampaignOngoing", true);
                return _isCampaignOngoing;
            }
            set
            {
                CanWriteProperty("IsCampaignOngoing", true);
                if (value == null) value = false;
                if (!_isCampaignOngoing.Equals(value))
                {
                    _isCampaignOngoing = value;
                    PropertyHasChanged("IsCampaignOngoing");
                }
            }
        }
        /////////////////////// NEW /////////////////////////
        public string ProcessingStatus
        {
            get
            {
                CanReadProperty("ProcessingStatus", true);
                return _processingStatus;
            }
            set
            {
                CanWriteProperty("ProcessingStatus", true);
                if (value == null) value = string.Empty;
                if (!_processingStatus.Equals(value))
                {
                    _processingStatus = value;
                    PropertyHasChanged("ProcessingStatus");
                }
            }
        }

        public Guid AdminidAssignedto
        {
            get
            {
                CanReadProperty("AdminidAssignedto", true);
                return _adminidAssignedto;
            }
            set
            {
                CanWriteProperty("AdminidAssignedto", true);
                if (!_adminidAssignedto.Equals(value))
                {
                    _adminidAssignedto = value;
                    PropertyHasChanged("AdminidAssignedto");
                }
            }
        }

        public string MaterialsSubmitted
        {
            get
            {
                CanReadProperty("MaterialsSubmitted", true);
                return _materialsSubmitted;
            }
            set
            {
                CanWriteProperty("MaterialsSubmitted", true);
                if (value == null) value = string.Empty;
                if (!_materialsSubmitted.Equals(value))
                {
                    _materialsSubmitted = value;
                    PropertyHasChanged("MaterialsSubmitted");
                }
            }
        }

        public string DQFlag
        {
            get
            {
                CanReadProperty("DQFlag", true);
                return _dQFlag;
            }
            set
            {
                CanWriteProperty("DQFlag", true);
                if (value == null) value = string.Empty;
                if (!_dQFlag.Equals(value))
                {
                    _dQFlag = value;
                    PropertyHasChanged("DQFlag");
                }
            }
        }

        public string NotificationSentDate
        {
            get
            {
                CanReadProperty("NotificationSentDate", true);
                return _notificationSentDate;
            }
            set
            {
                CanWriteProperty("NotificationSentDate", true);
                if (value == null) value = string.Empty;
                if (!_notificationSentDate.Equals(value))
                {
                    _notificationSentDate = value;
                    PropertyHasChanged("NotificationSentDate");
                }
            }
        }

        public string ReopeningDate
        {
            get
            {
                CanReadProperty("ReopeningDate", true);
                return _reopeningDate;
            }
            set
            {
                CanWriteProperty("ReopeningDate", true);
                if (value == null) value = string.Empty;
                if (!_reopeningDate.Equals(value))
                {
                    _reopeningDate = value;
                    PropertyHasChanged("ReopeningDate");
                }
            }
        }

        public string ReopeningDeadline
        {
            get
            {
                CanReadProperty("ReopeningDeadline", true);
                return _reopeningDeadline;
            }
            set
            {
                CanWriteProperty("ReopeningDeadline", true);
                if (value == null) value = string.Empty;
                if (!_reopeningDeadline.Equals(value))
                {
                    _reopeningDeadline = value;
                    PropertyHasChanged("ReopeningDeadline");
                }
            }
        }

        public string FlagReason
        {
            get
            {
                CanReadProperty("FlagReason", true);
                return _flagReason;
            }
            set
            {
                CanWriteProperty("FlagReason", true);
                if (value == null) value = string.Empty;
                if (!_flagReason.Equals(value))
                {
                    _flagReason = value;
                    PropertyHasChanged("FlagReason");
                }
            }
        }

        public string FlagDQDescription
        {
            get
            {
                CanReadProperty("FlagDQDescription", true);
                return _flagDQDescription;
            }
            set
            {
                CanWriteProperty("FlagDQDescription", true);
                if (value == null) value = string.Empty;
                if (!_flagDQDescription.Equals(value))
                {
                    _flagDQDescription = value;
                    PropertyHasChanged("FlagDQDescription");
                }
            }
        }

        public string ReopenedBy
        {
            get
            {
                CanReadProperty("ReopenedBy", true);
                return _reopenedBy;
            }
            set
            {
                CanWriteProperty("ReopenedBy", true);
                if (value == null) value = string.Empty;
                if (!_reopenedBy.Equals(value))
                {
                    _reopenedBy = value;
                    PropertyHasChanged("ReopenedBy");
                }
            }
        }

        public string OtherRemarks
        {
            get
            {
                CanReadProperty("OtherRemarks", true);
                return _otherRemarks;
            }
            set
            {
                CanWriteProperty("OtherRemarks", true);
                if (value == null) value = string.Empty;
                if (!_otherRemarks.Equals(value))
                {
                    _otherRemarks = value;
                    PropertyHasChanged("OtherRemarks");
                }
            }
        }

        public string DateVerified
        {
            get
            {
                CanReadProperty("DateVerified", true);
                return _dateVerified;
            }
            set
            {
                CanWriteProperty("DateVerified", true);
                if (value == null) value = string.Empty;
                if (!_dateVerified.Equals(value))
                {
                    _dateVerified = value;
                    PropertyHasChanged("DateVerified");
                }
            }
        }

        public string ReasonFeeWaiver
        {
            get
            {
                CanReadProperty("ReasonFeeWaiver", true);
                return _reasonFeeWaiver;
            }
            set
            {
                CanWriteProperty("ReasonFeeWaiver", true);
                if (value == null) value = string.Empty;
                if (!_reasonFeeWaiver.Equals(value))
                {
                    _reasonFeeWaiver = value;
                    PropertyHasChanged("ReasonFeeWaiver");
                }
            }
        }

        public string ResearchOther
        {
            get
            {
                CanReadProperty("ResearchOther", true);
                return _researchOther;
            }
            set
            {
                CanWriteProperty("ResearchOther", true);
                if (value == null) value = string.Empty;
                if (!_researchOther.Equals(value))
                {
                    _researchOther = value;
                    PropertyHasChanged("ResearchOther");
                }
            }
        }

        public string IDAdhocInvoice
        {
            get
            {
                CanReadProperty("IDAdhocInvoice", true);
                return _iDAdhocInvoice;
            }
            set
            {
                CanWriteProperty("IDAdhocInvoice", true);
                if (value == null) value = string.Empty;
                if (!_iDAdhocInvoice.Equals(value))
                {
                    _iDAdhocInvoice = value;
                    PropertyHasChanged("IDAdhocInvoice");
                }
            }
        }

        public string Deadline
        {
            get
            {
                return GeneralFunctionEffie2017App.GetDateDepentent(_payGroupId, _id, _dateSubmitted.Date, "D_String" );
            }
        }
        
        public DateTime DateReminder(Guid ID, string type)
        {
            return GeneralFunctionEffie2017App.GetDateReminder(ID, type);
        }

        /////////////////////// NEW /////////////////////////
        protected override object GetIdValue()
        {
            return _id;
        }


        public string GetPanelId(string round)
        {
            return GeneralFunctionEffie2017App.GetPanelId(_id, _categoryPSDetail, round); ;
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
            // Serial
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Serial", 50));
            //
            // Campaign
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Campaign", 100));
            //
            // Client
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Client", 100));
            //
            // Brand
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Brand", 100));
            //
            // CategoryMarket
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CategoryMarket", 10));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CategoryMarketR2", 10));
            //
            // CategoryPS
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CategoryPS", 10));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CategoryPSR2", 10));
            //
            // CategoryPSDetail
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CategoryPSDetail", 200));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CategoryPSDetailR2", 200));
            //
            // Effectiveness
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Effectiveness", 100));
            //
            // RepSalutation
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RepSalutation", 10));
            //
            // RepFirstname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RepFirstname", 100));
            //
            // RepLastname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RepLastname", 100));
            //
            // RepJob
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RepJob", 100));
            //
            // RepCompany
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RepCompany", 100));
            //
            // RepContact
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RepContact", 50));
            //
            // RepMobile
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RepMobile", 50));
            //
            // RepEmail
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("RepEmail", 100));
            //
            // Summary
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Summary", 4000));
            //
            // CaseData
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CaseData", 1000));
            //
            // Permission
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Permission", 50));
            //
            // Name
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Name", 100));
            //
            // Title
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Title", 100));
            //
            // Company
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Company", 100));
            //
            // PaymentMethod
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PaymentMethod", 10));
            //
            // PayCompany
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayCompany", 100));
            //
            // PayAddress1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayAddress1", 100));
            //
            // PayAddress2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayAddress2", 100));
            //
            // PayCity
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayCity", 100));
            //
            // PayPostal
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayPostal", 50));
            //
            // PayCountry
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayCountry", 100));
            //
            // PayFirstname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayFirstname", 100));
            //
            // PayLastname
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayLastname", 100));
            //
            // PayContact
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("PayContact", 50));

            //
            // ProductClassification
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ProductClassification", 100));
            //
            // ProductClassificationOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ProductClassificationOthers", 100));
            //
            // EntryObjective
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EntryObjective", 1000));
            //
            // EntryObjectiveOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EntryObjectiveOthers", 100));
            //
            // EntryObjective2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("EntryObjective2", 100));
            //
            // TargetAudience
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TargetAudience", 1000));
            //
            // TargetAudienceOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TargetAudienceOthers", 200));
            //
            // TargetAudiencePri
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TargetAudiencePri", 100));
            //
            // TargetAudiencePriOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("TargetAudiencePriOthers", 200));
            //
            // HeroTouchPoint
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HeroTouchPoint", 100));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HeroTouchPoint2", 100));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HeroTouchPoint3", 100));
            //
            // HeroTouchPointOthers
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HeroTouchPointOthers", 200));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HeroTouchPointOthers2", 200));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("HeroTouchPointOthers3", 200));
            // Social Platforms
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SocialPlatforms", 1000));
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SocialPlatformsOthers", 200));
            //
            // Research
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Research", 1000));
            //
            // ResearchImp
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ResearchImp", 100));

            //
            // CreativeUploadType
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("CreativeUploadType", 100));
            //
            // WithdrawnStatus
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("WithdrawnStatus", 3));
            //
            // SDGData1
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SDGData1", 100));
            //
            // SDGData2
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("SDGData2", 1000));
            //
            // Invoice
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("Invoice", 100));
            /////////////////////// NEW /////////////////////////
            //
            // ProcessingStatus
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ProcessingStatus", 200));
            //
            // MaterialsSubmitted
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("MaterialsSubmitted", 500));
            // DQFlag
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DQFlag", 200));
            //
            // NotificationSentDate
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("NotificationSentDate", 500));
            //
            // ReopeningDate
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ReopeningDate", 500));
            //
            // ReopeningDeadline
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ReopeningDeadline", 500));
            //
            // ReopenedBy
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ReopenedBy", 500));
            //
            // OtherRemarks
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("OtherRemarks", 500));
            //
            // DateVerified
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("DateVerified", 500));
            //
            // ReasonFeeWaiver
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ReasonFeeWaiver", 1000));
            //
            // ResearchOther
            //
            ValidationRules.AddRule(CommonRules.StringMaxLength, new CommonRules.MaxLengthRuleArgs("ResearchOther", 1000));
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
            //TODO: Define authorization rules in Entry
            //AuthorizationRules.AllowRead("Id", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Serial", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RegistrationId", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Campaign", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Client", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Brand", "EntryReadGroup");
            //AuthorizationRules.AllowRead("CategoryMarket", "EntryReadGroup");
            //AuthorizationRules.AllowRead("CategoryPS", "EntryReadGroup");
            //AuthorizationRules.AllowRead("CategoryPSDetail", "EntryReadGroup");
            //AuthorizationRules.AllowRead("DateCampaignStart", "EntryReadGroup");
            //AuthorizationRules.AllowRead("DateCampaignEnd", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Effectiveness", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RepSalutation", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RepFirstname", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RepLastname", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RepJob", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RepCompany", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RepContact", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RepMobile", "EntryReadGroup");
            //AuthorizationRules.AllowRead("RepEmail", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Summary", "EntryReadGroup");
            //AuthorizationRules.AllowRead("CaseData", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Permission", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Name", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Title", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Company", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Amount", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Fee", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayGroupId", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PaymentMethod", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayCompany", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayAddress1", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayAddress2", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayCity", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayPostal", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayCountry", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayFirstname", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayLastname", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayContact", "EntryReadGroup");
            //AuthorizationRules.AllowRead("IsUploadForm", "EntryReadGroup");
            //AuthorizationRules.AllowRead("IsUploadAuthorizationForm", "EntryReadGroup");
            //AuthorizationRules.AllowRead("IsUploadCaseImage", "EntryReadGroup");
            //AuthorizationRules.AllowRead("IsUploadCreativeMaterials", "EntryReadGroup");
            //AuthorizationRules.AllowRead("Status", "EntryReadGroup");
            //AuthorizationRules.AllowRead("PayStatus", "EntryReadGroup");
            //AuthorizationRules.AllowRead("DateCreated", "EntryReadGroup");
            //AuthorizationRules.AllowRead("DateModified", "EntryReadGroup");
            //AuthorizationRules.AllowRead("IsReminded", "EntryReadGroup");

            //AuthorizationRules.AllowWrite("Serial", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Campaign", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Client", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Brand", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("CategoryMarket", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("CategoryPS", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("CategoryPSDetail", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("DateCampaignStart", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("DateCampaignEnd", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Effectiveness", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("RepSalutation", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("RepFirstname", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("RepLastname", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("RepJob", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("RepCompany", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("RepContact", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("RepMobile", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("RepEmail", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Summary", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("CaseData", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Permission", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Name", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Title", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Company", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Amount", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Fee", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PaymentMethod", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayCompany", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayAddress1", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayAddress2", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayCity", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayPostal", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayCountry", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayFirstname", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayLastname", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayContact", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("IsUploadForm", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("IsUploadAuthorizationForm", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("IsUploadCaseImage", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("IsUploadCreativeMaterials", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("Status", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("PayStatus", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("DateCreated", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("DateModified", "EntryWriteGroup");
            //AuthorizationRules.AllowWrite("IsReminded", "EntryWriteGroup");
        }


        public static bool CanGetObject()
        {
            //TODO: Define CanGetObject permission in Entry
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryViewGroup"))
            //	return true;
            //return false;
        }

        public static bool CanAddObject()
        {
            //TODO: Define CanAddObject permission in Entry
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryAddGroup"))
            //	return true;
            //return false;
        }

        public static bool CanEditObject()
        {
            //TODO: Define CanEditObject permission in Entry
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryEditGroup"))
            //	return true;
            //return false;
        }

        public static bool CanDeleteObject()
        {
            //TODO: Define CanDeleteObject permission in Entry
            return true;
            //if (Csla.ApplicationContext.User.IsInRole("EntryDeleteGroup"))
            //	return true;
            //return false;
        }
        #endregion //Authorization Rules

        #region Factory Methods
        private Entry()
        { /* require use of factory method */ }

        private Entry(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static Entry NewEntry()
        {
            if (!CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Entry");
            return DataPortal.Create<Entry>();
        }

        public static Entry GetEntry(Guid id)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a Entry");
            return DataPortal.Fetch<Entry>(new Criteria(id));
        }

        public static Entry GetEntry(SafeDataReader dr)
        {
            return new Entry(dr);
        }


        public static void DeleteEntry(Guid id)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Entry");
            DataPortal.Delete(new Criteria(id));
        }

        public override Entry Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a Entry");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a Entry");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a Entry");

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
                cm.CommandText = "GetEntry";

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
            _serial = dr.GetString("Serial");
            _registrationId = dr.GetGuid("RegistrationId");
            _campaign = dr.GetString("Campaign");
            _client = dr.GetString("Client");
            _brand = dr.GetString("Brand");
            _categoryMarket = dr.GetString("CategoryMarket");
            _categoryPS = dr.GetString("CategoryPS");
            _categoryPSDetail = dr.GetString("CategoryPSDetail");
            _dateCampaignStart = dr.GetSmartDate("DateCampaignStart", _dateCampaignStart.EmptyIsMin);
            _dateCampaignEnd = dr.GetSmartDate("DateCampaignEnd", _dateCampaignEnd.EmptyIsMin);
            _effectiveness = dr.GetString("Effectiveness");
            _repSalutation = dr.GetString("RepSalutation");
            _repFirstname = dr.GetString("RepFirstname");
            _repLastname = dr.GetString("RepLastname");
            _repJob = dr.GetString("RepJob");
            _repCompany = dr.GetString("RepCompany");
            _repContact = dr.GetString("RepContact");
            _repMobile = dr.GetString("RepMobile");
            _repEmail = dr.GetString("RepEmail");
            _summary = dr.GetString("Summary");
            _caseData = dr.GetString("CaseData");
            _permission = dr.GetString("Permission");
            _name = dr.GetString("Name");
            _title = dr.GetString("Title");
            _company = dr.GetString("Company");
            _amount = dr.GetDecimal("Amount");
            _fee = dr.GetDecimal("Fee");
            _tax = dr.GetDecimal("Tax");
            _grandAmount = dr.GetDecimal("GrandAmount");
            _payGroupId = dr.GetGuid("PayGroupId");
            _paymentMethod = dr.GetString("PaymentMethod");
            _payCompany = dr.GetString("PayCompany");
            _payAddress1 = dr.GetString("PayAddress1");
            _payAddress2 = dr.GetString("PayAddress2");
            _payCity = dr.GetString("PayCity");
            _payPostal = dr.GetString("PayPostal");
            _payCountry = dr.GetString("PayCountry");
            _payFirstname = dr.GetString("PayFirstname");
            _payLastname = dr.GetString("PayLastname");
            _payContact = dr.GetString("PayContact");
            _isUploadForm = dr.GetBoolean("IsUploadEntryForm");
            _isUploadAuthorizationForm = dr.GetBoolean("IsUploadAuthorizationForm");
            _isUploadCaseImage = dr.GetBoolean("IsUploadCaseImage");
            _isUploadCreativeMaterials = dr.GetBoolean("IsUploadCreativeMaterials");
            _status = dr.GetString("Status");
            _payStatus = dr.GetString("PayStatus");
            _productClassification = dr.GetString("ProductClassification");
            _productClassificationOthers = dr.GetString("ProductClassificationOthers");
            _entryObjective = dr.GetString("EntryObjective");
            _entryObjectiveOthers = dr.GetString("EntryObjectiveOthers");
            _entryObjective2 = dr.GetString("EntryObjective2");
            _targetAudience = dr.GetString("TargetAudience");
            _targetAudienceOthers = dr.GetString("TargetAudienceOthers");
            _targetAudiencePri = dr.GetString("TargetAudiencePri");
            _targetAudiencePriOthers = dr.GetString("TargetAudiencePriOthers");

            _heroTouchPoint = dr.GetString("HeroTouchPoint");
            _heroTouchPoint2 = dr.GetString("HeroTouchPoint2");
            _heroTouchPoint3 = dr.GetString("HeroTouchPoint3");

            _heroTouchPointOthers = dr.GetString("HeroTouchPointOthers");
            _heroTouchPointOthers2 = dr.GetString("HeroTouchPointOthers2");
            _heroTouchPointOthers3 = dr.GetString("HeroTouchPointOthers3");

            _socialPlatforms = dr.GetString("SocialPlatforms");
            _socialPlatformsOthers = dr.GetString("SocialPlatformsOthers");

            _research = dr.GetString("Research");
            _researchImp = dr.GetString("ResearchImp");
            _sDGData1 = dr.GetString("SDGData1");
            _sDGData2 = dr.GetString("SDGData2");
            _invoice = dr.GetString("Invoice");
            _creativeUploadType = dr.GetString("CreativeUploadType");
            _amountReceived = dr.GetDecimal("AmountReceived");
            _withdrawnStatus = dr.GetString("WithdrawnStatus");
            _lastSendPaidEmailDate = dr.GetSmartDate("LastSendPaidEmailDate", _lastSendPaidEmailDate.EmptyIsMin);
            _lastSendUploadReminderEmailDate = dr.GetSmartDate("LastSendUploadReminderEmailDate", _lastSendUploadReminderEmailDate.EmptyIsMin);
            _lastSendCompletionReminderEmailDate = dr.GetSmartDate("LastSendCompletionReminderEmailDate", _lastSendCompletionReminderEmailDate.EmptyIsMin);
            _lastSendPaymentReminderEmailDate = dr.GetSmartDate("LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.EmptyIsMin);
            _lastSendSubmissionReminderEmailDate = dr.GetSmartDate("LastSendSubmissionReminderEmailDate", _lastSendSubmissionReminderEmailDate.EmptyIsMin);
            _dateSubmitted = dr.GetSmartDate("DateSubmitted", _dateSubmitted.EmptyIsMin);
            _dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
            _dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
            _isReminded = dr.GetInt32("IsReminded");
            _isRound2 = dr.GetBoolean("IsRound2");
            _categoryMarketR2 = dr.GetString("CategoryMarketR2");
            _categoryPSR2 = dr.GetString("CategoryPSR2");
            _categoryPSDetailR2 = dr.GetString("CategoryPSDetailR2");
            _isMaterialsVerified = dr.GetBoolean("IsMaterialsVerified");
            _isVideoDownloaded = dr.GetBoolean("IsVideoDownloaded");
            _isCampaignOngoing = dr.GetBoolean("IsCampaignOngoing");
            /////////////////////// NEW /////////////////////////
            _processingStatus = dr.GetString("ProcessingStatus");
            _adminidAssignedto = dr.GetGuid("adminId_AssignedTo");
            _materialsSubmitted = dr.GetString("MaterialsSubmitted");
            _dQFlag = dr.GetString("DQFlag");
            _notificationSentDate = dr.GetString("NotificationSentDate");
            _reopeningDate = dr.GetString("ReopeningDate");
            _reopeningDeadline = dr.GetString("ReopeningDeadline");
            _flagReason = dr.GetString("FlagReason");
            _flagDQDescription = dr.GetString("FlagDQDescription");
            _reopenedBy = dr.GetString("ReopenedBy");
            _otherRemarks = dr.GetString("OtherRemarks");
            _dateVerified = dr.GetString("DateVerified");
            _reasonFeeWaiver = dr.GetString("ReasonFeeWaiver");
            _researchOther = dr.GetString("ResearchOther");
            _iDAdhocInvoice = dr.GetString("IDAdhocInvoice");

            /////////////////////// NEW /////////////////////////
            
            //_panelId = dr.GetString("PanelId");
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
                cm.CommandText = "AddEntry";

                AddInsertParameters(cm);

                cm.ExecuteNonQuery();

                //_id = (Guid)cm.Parameters["@NewId"].Value;
                // _registrationId = (Guid)cm.Parameters["@NewRegistrationId"].Value;
                // _payGroupId = (Guid)cm.Parameters["@NewPayGroupId"].Value;
            }//using
        }

        private void AddInsertParameters(SqlCommand cm)
        {

            cm.Parameters.AddWithValue("@Serial", _serial);
            cm.Parameters.AddWithValue("@Campaign", _campaign);
            cm.Parameters.AddWithValue("@Client", _client);
            cm.Parameters.AddWithValue("@Brand", _brand);
            cm.Parameters.AddWithValue("@CategoryMarket", _categoryMarket);
            cm.Parameters.AddWithValue("@CategoryPS", _categoryPS);
            cm.Parameters.AddWithValue("@CategoryPSDetail", _categoryPSDetail);
            cm.Parameters.AddWithValue("@DateCampaignStart", _dateCampaignStart.DBValue);
            cm.Parameters.AddWithValue("@DateCampaignEnd", _dateCampaignEnd.DBValue);
            cm.Parameters.AddWithValue("@Effectiveness", _effectiveness);
            cm.Parameters.AddWithValue("@RepSalutation", _repSalutation);
            cm.Parameters.AddWithValue("@RepFirstname", _repFirstname);
            cm.Parameters.AddWithValue("@RepLastname", _repLastname);
            cm.Parameters.AddWithValue("@RepJob", _repJob);
            cm.Parameters.AddWithValue("@RepCompany", _repCompany);
            cm.Parameters.AddWithValue("@RepContact", _repContact);
            cm.Parameters.AddWithValue("@RepMobile", _repMobile);
            cm.Parameters.AddWithValue("@RepEmail", _repEmail);
            cm.Parameters.AddWithValue("@Summary", _summary);
            cm.Parameters.AddWithValue("@CaseData", _caseData);
            cm.Parameters.AddWithValue("@Permission", _permission);
            cm.Parameters.AddWithValue("@Name", _name);
            cm.Parameters.AddWithValue("@Title", _title);
            cm.Parameters.AddWithValue("@Company", _company);
            cm.Parameters.AddWithValue("@Amount", _amount);
            cm.Parameters.AddWithValue("@Fee", _fee);
            cm.Parameters.AddWithValue("@Tax", _tax);
            cm.Parameters.AddWithValue("@GrandAmount", _grandAmount);
            cm.Parameters.AddWithValue("@PaymentMethod", _paymentMethod);
            cm.Parameters.AddWithValue("@PayCompany", _payCompany);
            cm.Parameters.AddWithValue("@PayAddress1", _payAddress1);
            cm.Parameters.AddWithValue("@PayAddress2", _payAddress2);
            cm.Parameters.AddWithValue("@PayCity", _payCity);
            cm.Parameters.AddWithValue("@PayPostal", _payPostal);
            cm.Parameters.AddWithValue("@PayCountry", _payCountry);
            cm.Parameters.AddWithValue("@PayFirstname", _payFirstname);
            cm.Parameters.AddWithValue("@PayLastname", _payLastname);
            cm.Parameters.AddWithValue("@PayContact", _payContact);
            cm.Parameters.AddWithValue("@IsUploadEntryForm", _isUploadForm);
            cm.Parameters.AddWithValue("@IsUploadAuthorizationForm", _isUploadAuthorizationForm);
            cm.Parameters.AddWithValue("@IsUploadCaseImage", _isUploadCaseImage);
            cm.Parameters.AddWithValue("@IsUploadCreativeMaterials", _isUploadCreativeMaterials);
            cm.Parameters.AddWithValue("@Status", _status);
            cm.Parameters.AddWithValue("@PayStatus", _payStatus);
            cm.Parameters.AddWithValue("@ProductClassification", _productClassification);
            cm.Parameters.AddWithValue("@ProductClassificationOthers", _productClassificationOthers);
            cm.Parameters.AddWithValue("@EntryObjective", _entryObjective);
            cm.Parameters.AddWithValue("@EntryObjectiveOthers", _entryObjectiveOthers);
            cm.Parameters.AddWithValue("@EntryObjective2", _entryObjective2);
            cm.Parameters.AddWithValue("@TargetAudience", _targetAudience);
            cm.Parameters.AddWithValue("@TargetAudienceOthers", _targetAudienceOthers);
            cm.Parameters.AddWithValue("@TargetAudiencePri", _targetAudiencePri);
            cm.Parameters.AddWithValue("@TargetAudiencePriOthers", _targetAudiencePriOthers);

            cm.Parameters.AddWithValue("@HeroTouchPoint", _heroTouchPoint);
            cm.Parameters.AddWithValue("@HeroTouchPoint2", _heroTouchPoint2);
            cm.Parameters.AddWithValue("@HeroTouchPoint3", _heroTouchPoint3);

            cm.Parameters.AddWithValue("@HeroTouchPointOthers", _heroTouchPointOthers);
            cm.Parameters.AddWithValue("@HeroTouchPointOthers2", _heroTouchPointOthers2);
            cm.Parameters.AddWithValue("@HeroTouchPointOthers3", _heroTouchPointOthers3);

            cm.Parameters.AddWithValue("@SocialPlatforms", _socialPlatforms);
            cm.Parameters.AddWithValue("@SocialPlatformsOthers", _socialPlatformsOthers);

            cm.Parameters.AddWithValue("@Research", _research);
            cm.Parameters.AddWithValue("@ResearchImp", _researchImp);
            cm.Parameters.AddWithValue("@SDGData1", _sDGData1);
            cm.Parameters.AddWithValue("@SDGData2", _sDGData2);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@CreativeUploadType", _creativeUploadType);
            cm.Parameters.AddWithValue("@AmountReceived", _amountReceived);
            cm.Parameters.AddWithValue("@WithdrawnStatus", _withdrawnStatus);
            cm.Parameters.AddWithValue("@LastSendPaidEmailDate", _lastSendPaidEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendUploadReminderEmailDate", _lastSendUploadReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendCompletionReminderEmailDate", _lastSendCompletionReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendSubmissionReminderEmailDate", _lastSendSubmissionReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@DateSubmitted", _dateSubmitted.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@IsReminded", _isReminded);
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@RegistrationId", _registrationId);
            cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
            cm.Parameters.AddWithValue("@IsRound2", _isRound2);
            cm.Parameters.AddWithValue("@CategoryMarketR2", _categoryMarketR2);
            cm.Parameters.AddWithValue("@CategoryPSR2", _categoryPSR2);
            cm.Parameters.AddWithValue("@CategoryPSDetailR2", _categoryPSDetailR2);
            cm.Parameters.AddWithValue("@IsMaterialsVerified", _isMaterialsVerified);
            cm.Parameters.AddWithValue("@IsVideoDownloaded", _isVideoDownloaded);
            cm.Parameters.AddWithValue("@IsCampaignOngoing", _isCampaignOngoing);
            /////////////////////// NEW /////////////////////////
            if (_processingStatus != string.Empty)
                cm.Parameters.AddWithValue("@ProcessingStatus", _processingStatus);
            else
                cm.Parameters.AddWithValue("@ProcessingStatus", DBNull.Value);
            if (_materialsSubmitted != string.Empty)
                cm.Parameters.AddWithValue("@MaterialsSubmitted", _materialsSubmitted);
            else
                cm.Parameters.AddWithValue("@MaterialsSubmitted", DBNull.Value);
            if (_dQFlag != string.Empty)
                cm.Parameters.AddWithValue("@DQFlag", _dQFlag);
            else
                cm.Parameters.AddWithValue("@DQFlag", DBNull.Value);
            if (_notificationSentDate != string.Empty)
                cm.Parameters.AddWithValue("@NotificationSentDate", _notificationSentDate);
            else
                cm.Parameters.AddWithValue("@NotificationSentDate", DBNull.Value);
            if (_reopeningDate != string.Empty)
                cm.Parameters.AddWithValue("@ReopeningDate", _reopeningDate);
            else
                cm.Parameters.AddWithValue("@ReopeningDate", DBNull.Value);
            if (_reopeningDeadline != string.Empty)
                cm.Parameters.AddWithValue("@ReopeningDeadline", _reopeningDeadline);
            else
                cm.Parameters.AddWithValue("@ReopeningDeadline", DBNull.Value);
            if (_flagReason != string.Empty)
                cm.Parameters.AddWithValue("@FlagReason", _flagReason);
            else
                cm.Parameters.AddWithValue("@FlagReason", DBNull.Value);
            if (_flagDQDescription != string.Empty)
                cm.Parameters.AddWithValue("@FlagDQDescription", _flagDQDescription);
            else
                cm.Parameters.AddWithValue("@FlagDQDescription", DBNull.Value);
            if (_reopenedBy != string.Empty)
                cm.Parameters.AddWithValue("@ReopenedBy", _reopenedBy);
            else
                cm.Parameters.AddWithValue("@ReopenedBy", DBNull.Value);
            if (_otherRemarks != string.Empty)
                cm.Parameters.AddWithValue("@OtherRemarks", _otherRemarks);
            else
                cm.Parameters.AddWithValue("@OtherRemarks", DBNull.Value);
            if (_dateVerified != string.Empty)
                cm.Parameters.AddWithValue("@DateVerified", _dateVerified);
            else
                cm.Parameters.AddWithValue("@DateVerified", DBNull.Value);
            if (_reasonFeeWaiver != string.Empty)
                cm.Parameters.AddWithValue("@ReasonFeeWaiver", _reasonFeeWaiver);
            else
                cm.Parameters.AddWithValue("@ReasonFeeWaiver", DBNull.Value);
            if (_researchOther != string.Empty)
                cm.Parameters.AddWithValue("@ResearchOther", _researchOther);
            else
                cm.Parameters.AddWithValue("@ResearchOther", DBNull.Value);
            if (_iDAdhocInvoice != string.Empty)
                cm.Parameters.AddWithValue("@IDAdhocInvoice", _iDAdhocInvoice);
            else
                cm.Parameters.AddWithValue("@IDAdhocInvoice", DBNull.Value);

            
            cm.Parameters.AddWithValue("@adminId_AssignedTo", _adminidAssignedto);
            //cm.Parameters.AddWithValue("@NewId", _id);
            //cm.Parameters["@NewId"].Direction = ParameterDirection.Output;
            //cm.Parameters.AddWithValue("@NewRegistrationId", _registrationId);
            //cm.Parameters["@NewRegistrationId"].Direction = ParameterDirection.Output;
            //cm.Parameters.AddWithValue("@NewPayGroupId", _payGroupId);
            //cm.Parameters["@NewPayGroupId"].Direction = ParameterDirection.Output;
            //cm.Parameters.AddWithValue("@NewadminId_AssignedTo", _adminidAssignedto);
            //cm.Parameters["@NewadminId_AssignedTo"].Direction = ParameterDirection.Output;

            /////////////////////// NEW /////////////////////////

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
                cm.CommandText = "UpdateEntry";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

        private void AddUpdateParameters(SqlCommand cm)
        {
            cm.Parameters.AddWithValue("@Id", _id);
            cm.Parameters.AddWithValue("@Serial", _serial);
            cm.Parameters.AddWithValue("@RegistrationId", _registrationId);
            cm.Parameters.AddWithValue("@Campaign", _campaign);
            cm.Parameters.AddWithValue("@Client", _client);
            cm.Parameters.AddWithValue("@Brand", _brand);
            cm.Parameters.AddWithValue("@CategoryMarket", _categoryMarket);
            cm.Parameters.AddWithValue("@CategoryPS", _categoryPS);
            cm.Parameters.AddWithValue("@CategoryPSDetail", _categoryPSDetail);
            cm.Parameters.AddWithValue("@DateCampaignStart", _dateCampaignStart.DBValue);
            cm.Parameters.AddWithValue("@DateCampaignEnd", _dateCampaignEnd.DBValue);
            cm.Parameters.AddWithValue("@Effectiveness", _effectiveness);
            cm.Parameters.AddWithValue("@RepSalutation", _repSalutation);
            cm.Parameters.AddWithValue("@RepFirstname", _repFirstname);
            cm.Parameters.AddWithValue("@RepLastname", _repLastname);
            cm.Parameters.AddWithValue("@RepJob", _repJob);
            cm.Parameters.AddWithValue("@RepCompany", _repCompany);
            cm.Parameters.AddWithValue("@RepContact", _repContact);
            cm.Parameters.AddWithValue("@RepMobile", _repMobile);
            cm.Parameters.AddWithValue("@RepEmail", _repEmail);
            cm.Parameters.AddWithValue("@Summary", _summary);
            cm.Parameters.AddWithValue("@CaseData", _caseData);
            cm.Parameters.AddWithValue("@Permission", _permission);
            cm.Parameters.AddWithValue("@Name", _name);
            cm.Parameters.AddWithValue("@Title", _title);
            cm.Parameters.AddWithValue("@Company", _company);
            cm.Parameters.AddWithValue("@Amount", _amount);
            cm.Parameters.AddWithValue("@Fee", _fee);
            cm.Parameters.AddWithValue("@Tax", _tax);
            cm.Parameters.AddWithValue("@GrandAmount", _grandAmount);
            cm.Parameters.AddWithValue("@PayGroupId", _payGroupId);
            cm.Parameters.AddWithValue("@PaymentMethod", _paymentMethod);
            cm.Parameters.AddWithValue("@PayCompany", _payCompany);
            cm.Parameters.AddWithValue("@PayAddress1", _payAddress1);
            cm.Parameters.AddWithValue("@PayAddress2", _payAddress2);
            cm.Parameters.AddWithValue("@PayCity", _payCity);
            cm.Parameters.AddWithValue("@PayPostal", _payPostal);
            cm.Parameters.AddWithValue("@PayCountry", _payCountry);
            cm.Parameters.AddWithValue("@PayFirstname", _payFirstname);
            cm.Parameters.AddWithValue("@PayLastname", _payLastname);
            cm.Parameters.AddWithValue("@PayContact", _payContact);
            cm.Parameters.AddWithValue("@IsUploadEntryForm", _isUploadForm);
            cm.Parameters.AddWithValue("@IsUploadAuthorizationForm", _isUploadAuthorizationForm);
            cm.Parameters.AddWithValue("@IsUploadCaseImage", _isUploadCaseImage);
            cm.Parameters.AddWithValue("@IsUploadCreativeMaterials", _isUploadCreativeMaterials);
            cm.Parameters.AddWithValue("@Status", _status);
            cm.Parameters.AddWithValue("@PayStatus", _payStatus);
            cm.Parameters.AddWithValue("@ProductClassification", _productClassification);
            cm.Parameters.AddWithValue("@ProductClassificationOthers", _productClassificationOthers);
            cm.Parameters.AddWithValue("@EntryObjective", _entryObjective);
            cm.Parameters.AddWithValue("@EntryObjectiveOthers", _entryObjectiveOthers);
            cm.Parameters.AddWithValue("@EntryObjective2", _entryObjective2);
            cm.Parameters.AddWithValue("@TargetAudience", _targetAudience);
            cm.Parameters.AddWithValue("@TargetAudienceOthers", _targetAudienceOthers);
            cm.Parameters.AddWithValue("@TargetAudiencePri", _targetAudiencePri);
            cm.Parameters.AddWithValue("@TargetAudiencePriOthers", _targetAudiencePriOthers);

            cm.Parameters.AddWithValue("@HeroTouchPoint", _heroTouchPoint);
            cm.Parameters.AddWithValue("@HeroTouchPoint2", _heroTouchPoint2);
            cm.Parameters.AddWithValue("@HeroTouchPoint3", _heroTouchPoint3);

            cm.Parameters.AddWithValue("@HeroTouchPointOthers", _heroTouchPointOthers);
            cm.Parameters.AddWithValue("@HeroTouchPointOthers2", _heroTouchPointOthers2);
            cm.Parameters.AddWithValue("@HeroTouchPointOthers3", _heroTouchPointOthers3);

            cm.Parameters.AddWithValue("@SocialPlatforms", _socialPlatforms);
            cm.Parameters.AddWithValue("@SocialPlatformsOthers", _socialPlatformsOthers); cm.Parameters.AddWithValue("@Research", _research);
            cm.Parameters.AddWithValue("@ResearchImp", _researchImp);
            cm.Parameters.AddWithValue("@SDGData1", _sDGData1);
            cm.Parameters.AddWithValue("@SDGData2", _sDGData2);
            cm.Parameters.AddWithValue("@Invoice", _invoice);
            cm.Parameters.AddWithValue("@CreativeUploadType", _creativeUploadType);
            cm.Parameters.AddWithValue("@AmountReceived", _amountReceived);
            cm.Parameters.AddWithValue("@WithdrawnStatus", _withdrawnStatus);
            cm.Parameters.AddWithValue("@LastSendPaidEmailDate", _lastSendPaidEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendUploadReminderEmailDate", _lastSendUploadReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendCompletionReminderEmailDate", _lastSendCompletionReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendPaymentReminderEmailDate", _lastSendPaymentReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@LastSendSubmissionReminderEmailDate", _lastSendSubmissionReminderEmailDate.DBValue);
            cm.Parameters.AddWithValue("@DateSubmitted", _dateSubmitted.DBValue);
            cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
            cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
            cm.Parameters.AddWithValue("@IsReminded", _isReminded);
            cm.Parameters.AddWithValue("@IsRound2", _isRound2);
            cm.Parameters.AddWithValue("@CategoryMarketR2", _categoryMarketR2);
            cm.Parameters.AddWithValue("@CategoryPSR2", _categoryPSR2);
            cm.Parameters.AddWithValue("@CategoryPSDetailR2", _categoryPSDetailR2);
            cm.Parameters.AddWithValue("@IsMaterialsVerified", _isMaterialsVerified);
            cm.Parameters.AddWithValue("@IsVideoDownloaded", _isVideoDownloaded);
            cm.Parameters.AddWithValue("@IsCampaignOngoing", _isCampaignOngoing);
            /////////////////////// NEW /////////////////////////
            if (_processingStatus != string.Empty)
                cm.Parameters.AddWithValue("@ProcessingStatus", _processingStatus);
            else
                cm.Parameters.AddWithValue("@ProcessingStatus", DBNull.Value);
            cm.Parameters.AddWithValue("@adminId_AssignedTo", _adminidAssignedto);
            if (_materialsSubmitted != string.Empty)
                cm.Parameters.AddWithValue("@MaterialsSubmitted", _materialsSubmitted);
            else
                cm.Parameters.AddWithValue("@MaterialsSubmitted", DBNull.Value);
            if (_dQFlag != string.Empty)
                cm.Parameters.AddWithValue("@DQFlag", _dQFlag);
            else
                cm.Parameters.AddWithValue("@DQFlag", DBNull.Value);
            if (_notificationSentDate != string.Empty)
                cm.Parameters.AddWithValue("@NotificationSentDate", _notificationSentDate);
            else
                cm.Parameters.AddWithValue("@NotificationSentDate", DBNull.Value);
            if (_reopeningDate != string.Empty)
                cm.Parameters.AddWithValue("@ReopeningDate", _reopeningDate);
            else
                cm.Parameters.AddWithValue("@ReopeningDate", DBNull.Value);
            if (_reopeningDeadline != string.Empty)
                cm.Parameters.AddWithValue("@ReopeningDeadline", _reopeningDeadline);
            else
                cm.Parameters.AddWithValue("@ReopeningDeadline", DBNull.Value);
            if (_flagReason != string.Empty)
                cm.Parameters.AddWithValue("@FlagReason", _flagReason);
            else
                cm.Parameters.AddWithValue("@FlagReason", DBNull.Value);
            if (_flagDQDescription != string.Empty)
                cm.Parameters.AddWithValue("@FlagDQDescription", _flagDQDescription);
            else
                cm.Parameters.AddWithValue("@FlagDQDescription", DBNull.Value);
            if (_reopenedBy != string.Empty)
                cm.Parameters.AddWithValue("@ReopenedBy", _reopenedBy);
            else
                cm.Parameters.AddWithValue("@ReopenedBy", DBNull.Value);
            if (_otherRemarks != string.Empty)
                cm.Parameters.AddWithValue("@OtherRemarks", _otherRemarks);
            else
                cm.Parameters.AddWithValue("@OtherRemarks", DBNull.Value);
            if (_dateVerified != string.Empty)
                cm.Parameters.AddWithValue("@DateVerified", _dateVerified);
            else
                cm.Parameters.AddWithValue("@DateVerified", DBNull.Value);
            if (_reasonFeeWaiver != string.Empty)
                cm.Parameters.AddWithValue("@ReasonFeeWaiver", _reasonFeeWaiver);
            else
                cm.Parameters.AddWithValue("@ReasonFeeWaiver", DBNull.Value);
            if (_researchOther != string.Empty)
                cm.Parameters.AddWithValue("@ResearchOther", _researchOther);
            else
                cm.Parameters.AddWithValue("@ResearchOther", DBNull.Value);
            if (_iDAdhocInvoice != string.Empty)
                cm.Parameters.AddWithValue("@IDAdhocInvoice", _iDAdhocInvoice);
            else
                cm.Parameters.AddWithValue("@IDAdhocInvoice", DBNull.Value);

            
            /////////////////////// NEW /////////////////////////
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
                cm.CommandText = "DeleteEntry";

                cm.Parameters.AddWithValue("@Id", criteria.Id);

                cm.ExecuteNonQuery();
            }//using
        }


        #endregion //Data Access - Delete
        #endregion //Data Access

        #region Other Methods
        public string CategoryMarketFromRound(string round)
        {
            if (round == "1") return _categoryMarket;
            if (round == "2" && _categoryMarketR2.Trim() != "") return _categoryMarketR2;
            return _categoryMarket;
        }
        public string CategoryPSFromRound(string round)
        {
            if (round == "1") return _categoryPS;
            if (round == "2" && _categoryPSR2.Trim() != "") return _categoryPSR2;
            return _categoryPS;
        }
        public string CategoryPSDetailFromRound(string round)
        {
            if (round == "1") return _categoryPSDetail;
            if (round == "2" && _categoryPSDetailR2.Trim() != "") return _categoryPSDetailR2;
            return _categoryPSDetail;
        }
        public static void CleanDeleteEntry(Guid id)
        {
            Effie2017.App.CompanyCreditList companyCreditList = Effie2017.App.CompanyCreditList.GetCompanyCreditList(id);
            foreach (Effie2017.App.CompanyCredit companyCredit in companyCreditList)
            {
                Effie2017.App.CompanyCredit.DeleteCompanyCredit(companyCredit.Id);
            }

            Effie2017.App.IndCreditList indCreditList = Effie2017.App.IndCreditList.GetIndCreditList(id);
            foreach (Effie2017.App.IndCredit indCredit in indCreditList)
            {
                Effie2017.App.IndCredit.DeleteIndCredit(indCredit.Id);
            }

            Effie2017.App.Entry.DeleteEntry(id);
        }
        #endregion
    }
}
