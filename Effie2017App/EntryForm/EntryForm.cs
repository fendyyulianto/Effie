using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using Csla.Validation;

namespace Effie2017.App
{ 
	[Serializable()] 
	public class EntryForm : Csla.BusinessBase<EntryForm>
	{
		#region Business Properties and Methods

		//declare members
		private Guid _id = Guid.NewGuid();
		private Guid _idEntry = Guid.Empty;
		private SmartDate _dateCreated = new SmartDate(false);
		private SmartDate _dateModified = new SmartDate(false);
		private string _entryCategory = string.Empty;
		private string _desCountriesTime = string.Empty;
		private string _desTotalOfCountries = string.Empty;
		private string _country1 = string.Empty;
		private string _startdate1 = string.Empty;
		private string _enddate1 = string.Empty;
		private string _country2 = string.Empty;
		private string _startdate2 = string.Empty;
		private string _enddate2 = string.Empty;
		private string _country3 = string.Empty;
		private string _startdate3 = string.Empty;
		private string _enddate3 = string.Empty;
		private string _executiveSummary = string.Empty;
		private string _describeMarket = string.Empty;
		private string _strategicChallengeObjectives = string.Empty;
		private string _ideas = string.Empty;
		private string _bringingIdea = string.Empty;
		private string _communicationTouchPointsInitialYear = string.Empty;
		private string _communicationTouchPointsInterimYear = string.Empty;
		private string _communicationTouchPointsCurrentYear = string.Empty;
		private string _listAndExplainOtherMarketingText = string.Empty;
		private string _listAndExplainOtherMarketingCheck = string.Empty;
		private string _paidMediaExpendituresText = string.Empty;
		private string _paidMediaExpendituresPercent = string.Empty;
		private string _ownedMedia = string.Empty;
		private string _sponsorship = string.Empty;
		private string _explainWorked = string.Empty;
		private string _anything = string.Empty;
		private string _communicationTouchPointsCheck = string.Empty;
		private string _selectAllOtherMarketing = string.Empty;
		private string _other = string.Empty;
		private string _explainListOtherMarketing = string.Empty;
		private string _paidMediaExpendituresCheck = string.Empty;
		private string _paidMediaExpendituresTotalBudget = string.Empty;
		private string _paidMediaExpendituresAveregeAnnual = string.Empty;
		private string _paidMediaExpendituresIndicate = string.Empty;
		private string _paidMediaExpendituresAstimates = string.Empty;
		private string _paidMediaExpendituresCompared = string.Empty;
		private string _explainCriteria = string.Empty;
		private string _countryList = string.Empty;
		private string _explainIdea = string.Empty;
		private string _comparedOtherCompetitorsCheck = string.Empty;
		private string _comparedOverallSpendCheck = string.Empty;
		private string _typeCategoryOriginal = string.Empty;
		private string _typeCategory = string.Empty;
        private string _status = string.Empty;
        
        [System.ComponentModel.DataObjectField(true, false)]
		public Guid Id
		{
			get
			{
				CanReadProperty("Id", true);
				return _id;
			}
		}

		public Guid IdEntry
		{
			get
			{
				CanReadProperty("IdEntry", true);
				return _idEntry;
			}
			set
			{
				CanWriteProperty("IdEntry", true);
				if (!_idEntry.Equals(value))
				{
					_idEntry = value;
					PropertyHasChanged("IdEntry");
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
				CanReadProperty("DateCreatedString", true);
				return _dateCreated.Text;
			}
			set
			{
				CanWriteProperty("DateCreatedString", true);
				if (value == null) value = string.Empty;
				if (!_dateCreated.Equals(value))
				{
					_dateCreated.Text = value;
					PropertyHasChanged("DateCreatedString");
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
				CanReadProperty("DateModifiedString", true);
				return _dateModified.Text;
			}
			set
			{
				CanWriteProperty("DateModifiedString", true);
				if (value == null) value = string.Empty;
				if (!_dateModified.Equals(value))
				{
					_dateModified.Text = value;
					PropertyHasChanged("DateModifiedString");
				}
			}
		}

		public string EntryCategory
		{
			get
			{
				CanReadProperty("EntryCategory", true);
				return _entryCategory;
			}
			set
			{
				CanWriteProperty("EntryCategory", true);
				if (value == null) value = string.Empty;
				if (!_entryCategory.Equals(value))
				{
					_entryCategory = value;
					PropertyHasChanged("EntryCategory");
				}
			}
		}

		public string DesCountriesTime
		{
			get
			{
				CanReadProperty("DesCountriesTime", true);
				return _desCountriesTime;
			}
			set
			{
				CanWriteProperty("DesCountriesTime", true);
				if (value == null) value = string.Empty;
				if (!_desCountriesTime.Equals(value))
				{
					_desCountriesTime = value;
					PropertyHasChanged("DesCountriesTime");
				}
			}
		}

		public string DesTotalOfCountries
		{
			get
			{
				CanReadProperty("DesTotalOfCountries", true);
				return _desTotalOfCountries;
			}
			set
			{
				CanWriteProperty("DesTotalOfCountries", true);
				if (value == null) value = string.Empty;
				if (!_desTotalOfCountries.Equals(value))
				{
					_desTotalOfCountries = value;
					PropertyHasChanged("DesTotalOfCountries");
				}
			}
		}

		public string Country1
		{
			get
			{
				CanReadProperty("Country1", true);
				return _country1;
			}
			set
			{
				CanWriteProperty("Country1", true);
				if (value == null) value = string.Empty;
				if (!_country1.Equals(value))
				{
					_country1 = value;
					PropertyHasChanged("Country1");
				}
			}
		}

		public string Startdate1
		{
			get
			{
				CanReadProperty("Startdate1", true);
				return _startdate1;
			}
			set
			{
				CanWriteProperty("Startdate1", true);
				if (value == null) value = string.Empty;
				if (!_startdate1.Equals(value))
				{
					_startdate1 = value;
					PropertyHasChanged("Startdate1");
				}
			}
		}

		public string Enddate1
		{
			get
			{
				CanReadProperty("Enddate1", true);
				return _enddate1;
			}
			set
			{
				CanWriteProperty("Enddate1", true);
				if (value == null) value = string.Empty;
				if (!_enddate1.Equals(value))
				{
					_enddate1 = value;
					PropertyHasChanged("Enddate1");
				}
			}
		}

		public string Country2
		{
			get
			{
				CanReadProperty("Country2", true);
				return _country2;
			}
			set
			{
				CanWriteProperty("Country2", true);
				if (value == null) value = string.Empty;
				if (!_country2.Equals(value))
				{
					_country2 = value;
					PropertyHasChanged("Country2");
				}
			}
		}

		public string Startdate2
		{
			get
			{
				CanReadProperty("Startdate2", true);
				return _startdate2;
			}
			set
			{
				CanWriteProperty("Startdate2", true);
				if (value == null) value = string.Empty;
				if (!_startdate2.Equals(value))
				{
					_startdate2 = value;
					PropertyHasChanged("Startdate2");
				}
			}
		}

		public string Enddate2
		{
			get
			{
				CanReadProperty("Enddate2", true);
				return _enddate2;
			}
			set
			{
				CanWriteProperty("Enddate2", true);
				if (value == null) value = string.Empty;
				if (!_enddate2.Equals(value))
				{
					_enddate2 = value;
					PropertyHasChanged("Enddate2");
				}
			}
		}

		public string Country3
		{
			get
			{
				CanReadProperty("Country3", true);
				return _country3;
			}
			set
			{
				CanWriteProperty("Country3", true);
				if (value == null) value = string.Empty;
				if (!_country3.Equals(value))
				{
					_country3 = value;
					PropertyHasChanged("Country3");
				}
			}
		}

		public string Startdate3
		{
			get
			{
				CanReadProperty("Startdate3", true);
				return _startdate3;
			}
			set
			{
				CanWriteProperty("Startdate3", true);
				if (value == null) value = string.Empty;
				if (!_startdate3.Equals(value))
				{
					_startdate3 = value;
					PropertyHasChanged("Startdate3");
				}
			}
		}

		public string Enddate3
		{
			get
			{
				CanReadProperty("Enddate3", true);
				return _enddate3;
			}
			set
			{
				CanWriteProperty("Enddate3", true);
				if (value == null) value = string.Empty;
				if (!_enddate3.Equals(value))
				{
					_enddate3 = value;
					PropertyHasChanged("Enddate3");
				}
			}
		}

		public string ExecutiveSummary
		{
			get
			{
				CanReadProperty("ExecutiveSummary", true);
				return _executiveSummary;
			}
			set
			{
				CanWriteProperty("ExecutiveSummary", true);
				if (value == null) value = string.Empty;
				if (!_executiveSummary.Equals(value))
				{
					_executiveSummary = value;
					PropertyHasChanged("ExecutiveSummary");
				}
			}
		}

		public string DescribeMarket
		{
			get
			{
				CanReadProperty("DescribeMarket", true);
				return _describeMarket;
			}
			set
			{
				CanWriteProperty("DescribeMarket", true);
				if (value == null) value = string.Empty;
				if (!_describeMarket.Equals(value))
				{
					_describeMarket = value;
					PropertyHasChanged("DescribeMarket");
				}
			}
		}

		public string StrategicChallengeObjectives
		{
			get
			{
				CanReadProperty("StrategicChallengeObjectives", true);
				return _strategicChallengeObjectives;
			}
			set
			{
				CanWriteProperty("StrategicChallengeObjectives", true);
				if (value == null) value = string.Empty;
				if (!_strategicChallengeObjectives.Equals(value))
				{
					_strategicChallengeObjectives = value;
					PropertyHasChanged("StrategicChallengeObjectives");
				}
			}
		}

		public string Ideas
		{
			get
			{
				CanReadProperty("Ideas", true);
				return _ideas;
			}
			set
			{
				CanWriteProperty("Ideas", true);
				if (value == null) value = string.Empty;
				if (!_ideas.Equals(value))
				{
					_ideas = value;
					PropertyHasChanged("Ideas");
				}
			}
		}

		public string BringingIdea
		{
			get
			{
				CanReadProperty("BringingIdea", true);
				return _bringingIdea;
			}
			set
			{
				CanWriteProperty("BringingIdea", true);
				if (value == null) value = string.Empty;
				if (!_bringingIdea.Equals(value))
				{
					_bringingIdea = value;
					PropertyHasChanged("BringingIdea");
				}
			}
		}

		public string CommunicationTouchPointsInitialYear
		{
			get
			{
				CanReadProperty("CommunicationTouchPointsInitialYear", true);
				return _communicationTouchPointsInitialYear;
			}
			set
			{
				CanWriteProperty("CommunicationTouchPointsInitialYear", true);
				if (value == null) value = string.Empty;
				if (!_communicationTouchPointsInitialYear.Equals(value))
				{
					_communicationTouchPointsInitialYear = value;
					PropertyHasChanged("CommunicationTouchPointsInitialYear");
				}
			}
		}

		public string CommunicationTouchPointsInterimYear
		{
			get
			{
				CanReadProperty("CommunicationTouchPointsInterimYear", true);
				return _communicationTouchPointsInterimYear;
			}
			set
			{
				CanWriteProperty("CommunicationTouchPointsInterimYear", true);
				if (value == null) value = string.Empty;
				if (!_communicationTouchPointsInterimYear.Equals(value))
				{
					_communicationTouchPointsInterimYear = value;
					PropertyHasChanged("CommunicationTouchPointsInterimYear");
				}
			}
		}

		public string CommunicationTouchPointsCurrentYear
		{
			get
			{
				CanReadProperty("CommunicationTouchPointsCurrentYear", true);
				return _communicationTouchPointsCurrentYear;
			}
			set
			{
				CanWriteProperty("CommunicationTouchPointsCurrentYear", true);
				if (value == null) value = string.Empty;
				if (!_communicationTouchPointsCurrentYear.Equals(value))
				{
					_communicationTouchPointsCurrentYear = value;
					PropertyHasChanged("CommunicationTouchPointsCurrentYear");
				}
			}
		}

		public string ListAndExplainOtherMarketingText
		{
			get
			{
				CanReadProperty("ListAndExplainOtherMarketingText", true);
				return _listAndExplainOtherMarketingText;
			}
			set
			{
				CanWriteProperty("ListAndExplainOtherMarketingText", true);
				if (value == null) value = string.Empty;
				if (!_listAndExplainOtherMarketingText.Equals(value))
				{
					_listAndExplainOtherMarketingText = value;
					PropertyHasChanged("ListAndExplainOtherMarketingText");
				}
			}
		}

		public string ListAndExplainOtherMarketingCheck
		{
			get
			{
				CanReadProperty("ListAndExplainOtherMarketingCheck", true);
				return _listAndExplainOtherMarketingCheck;
			}
			set
			{
				CanWriteProperty("ListAndExplainOtherMarketingCheck", true);
				if (value == null) value = string.Empty;
				if (!_listAndExplainOtherMarketingCheck.Equals(value))
				{
					_listAndExplainOtherMarketingCheck = value;
					PropertyHasChanged("ListAndExplainOtherMarketingCheck");
				}
			}
		}

		public string PaidMediaExpendituresText
		{
			get
			{
				CanReadProperty("PaidMediaExpendituresText", true);
				return _paidMediaExpendituresText;
			}
			set
			{
				CanWriteProperty("PaidMediaExpendituresText", true);
				if (value == null) value = string.Empty;
				if (!_paidMediaExpendituresText.Equals(value))
				{
					_paidMediaExpendituresText = value;
					PropertyHasChanged("PaidMediaExpendituresText");
				}
			}
		}

		public string PaidMediaExpendituresPercent
		{
			get
			{
				CanReadProperty("PaidMediaExpendituresPercent", true);
				return _paidMediaExpendituresPercent;
			}
			set
			{
				CanWriteProperty("PaidMediaExpendituresPercent", true);
				if (value == null) value = string.Empty;
				if (!_paidMediaExpendituresPercent.Equals(value))
				{
					_paidMediaExpendituresPercent = value;
					PropertyHasChanged("PaidMediaExpendituresPercent");
				}
			}
		}

		public string OwnedMedia
		{
			get
			{
				CanReadProperty("OwnedMedia", true);
				return _ownedMedia;
			}
			set
			{
				CanWriteProperty("OwnedMedia", true);
				if (value == null) value = string.Empty;
				if (!_ownedMedia.Equals(value))
				{
					_ownedMedia = value;
					PropertyHasChanged("OwnedMedia");
				}
			}
		}

		public string Sponsorship
		{
			get
			{
				CanReadProperty("Sponsorship", true);
				return _sponsorship;
			}
			set
			{
				CanWriteProperty("Sponsorship", true);
				if (value == null) value = string.Empty;
				if (!_sponsorship.Equals(value))
				{
					_sponsorship = value;
					PropertyHasChanged("Sponsorship");
				}
			}
		}

		public string ExplainWorked
		{
			get
			{
				CanReadProperty("ExplainWorked", true);
				return _explainWorked;
			}
			set
			{
				CanWriteProperty("ExplainWorked", true);
				if (value == null) value = string.Empty;
				if (!_explainWorked.Equals(value))
				{
					_explainWorked = value;
					PropertyHasChanged("ExplainWorked");
				}
			}
		}

		public string Anything
		{
			get
			{
				CanReadProperty("Anything", true);
				return _anything;
			}
			set
			{
				CanWriteProperty("Anything", true);
				if (value == null) value = string.Empty;
				if (!_anything.Equals(value))
				{
					_anything = value;
					PropertyHasChanged("Anything");
				}
			}
		}

		public string CommunicationTouchPointsCheck
		{
			get
			{
				CanReadProperty("CommunicationTouchPointsCheck", true);
				return _communicationTouchPointsCheck;
			}
			set
			{
				CanWriteProperty("CommunicationTouchPointsCheck", true);
				if (value == null) value = string.Empty;
				if (!_communicationTouchPointsCheck.Equals(value))
				{
					_communicationTouchPointsCheck = value;
					PropertyHasChanged("CommunicationTouchPointsCheck");
				}
			}
		}

		public string SelectAllOtherMarketing
		{
			get
			{
				CanReadProperty("SelectAllOtherMarketing", true);
				return _selectAllOtherMarketing;
			}
			set
			{
				CanWriteProperty("SelectAllOtherMarketing", true);
				if (value == null) value = string.Empty;
				if (!_selectAllOtherMarketing.Equals(value))
				{
					_selectAllOtherMarketing = value;
					PropertyHasChanged("SelectAllOtherMarketing");
				}
			}
		}

		public string Other
		{
			get
			{
				CanReadProperty("Other", true);
				return _other;
			}
			set
			{
				CanWriteProperty("Other", true);
				if (value == null) value = string.Empty;
				if (!_other.Equals(value))
				{
					_other = value;
					PropertyHasChanged("Other");
				}
			}
		}

		public string ExplainListOtherMarketing
		{
			get
			{
				CanReadProperty("ExplainListOtherMarketing", true);
				return _explainListOtherMarketing;
			}
			set
			{
				CanWriteProperty("ExplainListOtherMarketing", true);
				if (value == null) value = string.Empty;
				if (!_explainListOtherMarketing.Equals(value))
				{
					_explainListOtherMarketing = value;
					PropertyHasChanged("ExplainListOtherMarketing");
				}
			}
		}

		public string PaidMediaExpendituresCheck
		{
			get
			{
				CanReadProperty("PaidMediaExpendituresCheck", true);
				return _paidMediaExpendituresCheck;
			}
			set
			{
				CanWriteProperty("PaidMediaExpendituresCheck", true);
				if (value == null) value = string.Empty;
				if (!_paidMediaExpendituresCheck.Equals(value))
				{
					_paidMediaExpendituresCheck = value;
					PropertyHasChanged("PaidMediaExpendituresCheck");
				}
			}
		}

		public string PaidMediaExpendituresTotalBudget
		{
			get
			{
				CanReadProperty("PaidMediaExpendituresTotalBudget", true);
				return _paidMediaExpendituresTotalBudget;
			}
			set
			{
				CanWriteProperty("PaidMediaExpendituresTotalBudget", true);
				if (value == null) value = string.Empty;
				if (!_paidMediaExpendituresTotalBudget.Equals(value))
				{
					_paidMediaExpendituresTotalBudget = value;
					PropertyHasChanged("PaidMediaExpendituresTotalBudget");
				}
			}
		}

		public string PaidMediaExpendituresAveregeAnnual
		{
			get
			{
				CanReadProperty("PaidMediaExpendituresAveregeAnnual", true);
				return _paidMediaExpendituresAveregeAnnual;
			}
			set
			{
				CanWriteProperty("PaidMediaExpendituresAveregeAnnual", true);
				if (value == null) value = string.Empty;
				if (!_paidMediaExpendituresAveregeAnnual.Equals(value))
				{
					_paidMediaExpendituresAveregeAnnual = value;
					PropertyHasChanged("PaidMediaExpendituresAveregeAnnual");
				}
			}
		}

		public string PaidMediaExpendituresIndicate
		{
			get
			{
				CanReadProperty("PaidMediaExpendituresIndicate", true);
				return _paidMediaExpendituresIndicate;
			}
			set
			{
				CanWriteProperty("PaidMediaExpendituresIndicate", true);
				if (value == null) value = string.Empty;
				if (!_paidMediaExpendituresIndicate.Equals(value))
				{
					_paidMediaExpendituresIndicate = value;
					PropertyHasChanged("PaidMediaExpendituresIndicate");
				}
			}
		}

		public string PaidMediaExpendituresAstimates
		{
			get
			{
				CanReadProperty("PaidMediaExpendituresAstimates", true);
				return _paidMediaExpendituresAstimates;
			}
			set
			{
				CanWriteProperty("PaidMediaExpendituresAstimates", true);
				if (value == null) value = string.Empty;
				if (!_paidMediaExpendituresAstimates.Equals(value))
				{
					_paidMediaExpendituresAstimates = value;
					PropertyHasChanged("PaidMediaExpendituresAstimates");
				}
			}
		}

		public string PaidMediaExpendituresCompared
		{
			get
			{
				CanReadProperty("PaidMediaExpendituresCompared", true);
				return _paidMediaExpendituresCompared;
			}
			set
			{
				CanWriteProperty("PaidMediaExpendituresCompared", true);
				if (value == null) value = string.Empty;
				if (!_paidMediaExpendituresCompared.Equals(value))
				{
					_paidMediaExpendituresCompared = value;
					PropertyHasChanged("PaidMediaExpendituresCompared");
				}
			}
		}

		public string ExplainCriteria
		{
			get
			{
				CanReadProperty("ExplainCriteria", true);
				return _explainCriteria;
			}
			set
			{
				CanWriteProperty("ExplainCriteria", true);
				if (value == null) value = string.Empty;
				if (!_explainCriteria.Equals(value))
				{
					_explainCriteria = value;
					PropertyHasChanged("ExplainCriteria");
				}
			}
		}

		public string CountryList
		{
			get
			{
				CanReadProperty("CountryList", true);
				return _countryList;
			}
			set
			{
				CanWriteProperty("CountryList", true);
				if (value == null) value = string.Empty;
				if (!_countryList.Equals(value))
				{
					_countryList = value;
					PropertyHasChanged("CountryList");
				}
			}
		}

		public string ExplainIdea
		{
			get
			{
				CanReadProperty("ExplainIdea", true);
				return _explainIdea;
			}
			set
			{
				CanWriteProperty("ExplainIdea", true);
				if (value == null) value = string.Empty;
				if (!_explainIdea.Equals(value))
				{
					_explainIdea = value;
					PropertyHasChanged("ExplainIdea");
				}
			}
		}

		public string ComparedOtherCompetitorsCheck
		{
			get
			{
				CanReadProperty("ComparedOtherCompetitorsCheck", true);
				return _comparedOtherCompetitorsCheck;
			}
			set
			{
				CanWriteProperty("ComparedOtherCompetitorsCheck", true);
				if (value == null) value = string.Empty;
				if (!_comparedOtherCompetitorsCheck.Equals(value))
				{
					_comparedOtherCompetitorsCheck = value;
					PropertyHasChanged("ComparedOtherCompetitorsCheck");
				}
			}
		}

		public string ComparedOverallSpendCheck
		{
			get
			{
				CanReadProperty("ComparedOverallSpendCheck", true);
				return _comparedOverallSpendCheck;
			}
			set
			{
				CanWriteProperty("ComparedOverallSpendCheck", true);
				if (value == null) value = string.Empty;
				if (!_comparedOverallSpendCheck.Equals(value))
				{
					_comparedOverallSpendCheck = value;
					PropertyHasChanged("ComparedOverallSpendCheck");
				}
			}
		}

		public string TypeCategoryOriginal
		{
			get
			{
				CanReadProperty("TypeCategoryOriginal", true);
				return _typeCategoryOriginal;
			}
			set
			{
				CanWriteProperty("TypeCategoryOriginal", true);
				if (value == null) value = string.Empty;
				if (!_typeCategoryOriginal.Equals(value))
				{
					_typeCategoryOriginal = value;
					PropertyHasChanged("TypeCategoryOriginal");
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

        
        public string TypeCategory
		{
			get
			{
				CanReadProperty("TypeCategory", true);
				return _typeCategory;
			}
			set
			{
				CanWriteProperty("TypeCategory", true);
				if (value == null) value = string.Empty;
				if (!_typeCategory.Equals(value))
				{
					_typeCategory = value;
					PropertyHasChanged("TypeCategory");
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
			//TODO: Define authorization rules in EntryForm
			//AuthorizationRules.AllowRead("Id", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("IdEntry", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("DateCreated", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("DateCreatedString", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("DateModified", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("DateModifiedString", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("EntryCategory", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("DesCountriesTime", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("DesTotalOfCountries", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Country1", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Startdate1", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Enddate1", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Country2", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Startdate2", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Enddate2", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Country3", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Startdate3", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Enddate3", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ExecutiveSummary", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("DescribeMarket", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("StrategicChallengeObjectives", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Ideas", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("BringingIdea", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("CommunicationTouchPointsInitialYear", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("CommunicationTouchPointsInterimYear", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("CommunicationTouchPointsCurrentYear", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ListAndExplainOtherMarketingText", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ListAndExplainOtherMarketingCheck", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("PaidMediaExpendituresText", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("PaidMediaExpendituresPercent", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("OwnedMedia", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Sponsorship", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ExplainWorked", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Anything", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("CommunicationTouchPointsCheck", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("SelectAllOtherMarketing", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("Other", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ExplainListOtherMarketing", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("PaidMediaExpendituresCheck", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("PaidMediaExpendituresTotalBudget", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("PaidMediaExpendituresAveregeAnnual", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("PaidMediaExpendituresIndicate", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("PaidMediaExpendituresAstimates", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("PaidMediaExpendituresCompared", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ExplainCriteria", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("CountryList", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ExplainIdea", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ComparedOtherCompetitorsCheck", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("ComparedOverallSpendCheck", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("TypeCategoryOriginal", "EntryFormReadGroup");
			//AuthorizationRules.AllowRead("TypeCategory", "EntryFormReadGroup");

			//AuthorizationRules.AllowWrite("IdEntry", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("DateCreatedString", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("DateModifiedString", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("EntryCategory", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("DesCountriesTime", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("DesTotalOfCountries", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Country1", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Startdate1", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Enddate1", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Country2", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Startdate2", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Enddate2", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Country3", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Startdate3", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Enddate3", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ExecutiveSummary", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("DescribeMarket", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("StrategicChallengeObjectives", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Ideas", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("BringingIdea", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("CommunicationTouchPointsInitialYear", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("CommunicationTouchPointsInterimYear", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("CommunicationTouchPointsCurrentYear", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ListAndExplainOtherMarketingText", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ListAndExplainOtherMarketingCheck", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("PaidMediaExpendituresText", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("PaidMediaExpendituresPercent", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("OwnedMedia", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Sponsorship", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ExplainWorked", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Anything", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("CommunicationTouchPointsCheck", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("SelectAllOtherMarketing", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("Other", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ExplainListOtherMarketing", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("PaidMediaExpendituresCheck", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("PaidMediaExpendituresTotalBudget", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("PaidMediaExpendituresAveregeAnnual", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("PaidMediaExpendituresIndicate", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("PaidMediaExpendituresAstimates", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("PaidMediaExpendituresCompared", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ExplainCriteria", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("CountryList", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ExplainIdea", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ComparedOtherCompetitorsCheck", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("ComparedOverallSpendCheck", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("TypeCategoryOriginal", "EntryFormWriteGroup");
			//AuthorizationRules.AllowWrite("TypeCategory", "EntryFormWriteGroup");
		}


		public static bool CanGetObject()
		{
			//TODO: Define CanGetObject permission in EntryForm
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("EntryFormViewGroup"))
			//	return true;
			//return false;
		}

		public static bool CanAddObject()
		{
			//TODO: Define CanAddObject permission in EntryForm
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("EntryFormAddGroup"))
			//	return true;
			//return false;
		}

		public static bool CanEditObject()
		{
			//TODO: Define CanEditObject permission in EntryForm
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("EntryFormEditGroup"))
			//	return true;
			//return false;
		}

		public static bool CanDeleteObject()
		{
			//TODO: Define CanDeleteObject permission in EntryForm
			return true;
			//if (Csla.ApplicationContext.User.IsInRole("EntryFormDeleteGroup"))
			//	return true;
			//return false;
		}
		#endregion //Authorization Rules

		#region Factory Methods
		private EntryForm()
		{ /* require use of factory method */ }

		public static EntryForm NewEntryForm()
		{
			if (!CanAddObject())
				throw new System.Security.SecurityException("User not authorized to add a EntryForm");
			return DataPortal.Create<EntryForm>();
		}

        public static EntryForm GetEntryForm(Guid id, Guid entryid)
        {
            if (!CanGetObject())
                throw new System.Security.SecurityException("User not authorized to view a EntryForm");
            return DataPortal.Fetch<EntryForm>(new Criteria(id, entryid));
        }

        public static void DeleteEntryForm(Guid id, Guid entryid)
        {
            if (!CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EntryForm");
            DataPortal.Delete(new Criteria(id, entryid));
        }

        public override EntryForm Save()
        {
            if (IsDeleted && !CanDeleteObject())
                throw new System.Security.SecurityException("User not authorized to remove a EntryForm");
            else if (IsNew && !CanAddObject())
                throw new System.Security.SecurityException("User not authorized to add a EntryForm");
            else if (!CanEditObject())
                throw new System.Security.SecurityException("User not authorized to update a EntryForm");

            return base.Save();
        }

        #endregion //Factory Methods

        #region Data Access

        #region Criteria

        [Serializable()]
        private class Criteria
        {
            public Guid Id;
            public Guid IdEntry;
            public Criteria(Guid id, Guid entryid)
            {
                this.Id = id;
                this.IdEntry = entryid;
            }

        }

        private EntryForm(SafeDataReader dr)
        {
            FetchObject(dr);
            MarkOld();
        }

        public static EntryForm GetEntryForm(SafeDataReader dr)
        {
            return new EntryForm(dr);
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
                cm.CommandText = "GetEntryForm";

                cm.Parameters.AddWithValue("@Id", criteria.Id);
                cm.Parameters.AddWithValue("@IdEntry", criteria.IdEntry);

                using (SafeDataReader dr = new SafeDataReader(cm.ExecuteReader()))
                {
                    dr.Read();
                    FetchObject(dr);

                    //load child object(s)
                    FetchChildren(dr);
                }
                ValidationRules.CheckRules();
            }//using
        }

        private void FetchObject(SafeDataReader dr)
        {
			_id = dr.GetGuid("Id");
			_idEntry = dr.GetGuid("IdEntry");
			_dateCreated = dr.GetSmartDate("DateCreated", _dateCreated.EmptyIsMin);
			_dateModified = dr.GetSmartDate("DateModified", _dateModified.EmptyIsMin);
			_entryCategory = dr.GetString("EntryCategory");
			_desCountriesTime = dr.GetString("DesCountriesTime");
			_desTotalOfCountries = dr.GetString("DesTotalOfCountries");
			_country1 = dr.GetString("Country1");
			_startdate1 = dr.GetString("Startdate1");
			_enddate1 = dr.GetString("Enddate1");
			_country2 = dr.GetString("Country2");
			_startdate2 = dr.GetString("Startdate2");
			_enddate2 = dr.GetString("Enddate2");
			_country3 = dr.GetString("Country3");
			_startdate3 = dr.GetString("Startdate3");
			_enddate3 = dr.GetString("Enddate3");
			_executiveSummary = dr.GetString("ExecutiveSummary");
			_describeMarket = dr.GetString("DescribeMarket");
			_strategicChallengeObjectives = dr.GetString("StrategicChallengeObjectives");
			_ideas = dr.GetString("Ideas");
			_bringingIdea = dr.GetString("BringingIdea");
			_communicationTouchPointsInitialYear = dr.GetString("CommunicationTouchPointsInitialYear");
			_communicationTouchPointsInterimYear = dr.GetString("CommunicationTouchPointsInterimYear");
			_communicationTouchPointsCurrentYear = dr.GetString("CommunicationTouchPointsCurrentYear");
			_listAndExplainOtherMarketingText = dr.GetString("ListAndExplainOtherMarketingText");
			_listAndExplainOtherMarketingCheck = dr.GetString("ListAndExplainOtherMarketingCheck");
			_paidMediaExpendituresText = dr.GetString("PaidMediaExpendituresText");
			_paidMediaExpendituresPercent = dr.GetString("PaidMediaExpendituresPercent");
			_ownedMedia = dr.GetString("OwnedMedia");
			_sponsorship = dr.GetString("Sponsorship");
			_explainWorked = dr.GetString("ExplainWorked");
			_anything = dr.GetString("Anything");
			_communicationTouchPointsCheck = dr.GetString("CommunicationTouchPointsCheck");
			_selectAllOtherMarketing = dr.GetString("SelectAllOtherMarketing");
			_other = dr.GetString("Other");
			_explainListOtherMarketing = dr.GetString("ExplainListOtherMarketing");
			_paidMediaExpendituresCheck = dr.GetString("PaidMediaExpendituresCheck");
			_paidMediaExpendituresTotalBudget = dr.GetString("PaidMediaExpendituresTotalBudget");
			_paidMediaExpendituresAveregeAnnual = dr.GetString("PaidMediaExpendituresAveregeAnnual");
			_paidMediaExpendituresIndicate = dr.GetString("PaidMediaExpendituresIndicate");
			_paidMediaExpendituresAstimates = dr.GetString("PaidMediaExpendituresAstimates");
			_paidMediaExpendituresCompared = dr.GetString("PaidMediaExpendituresCompared");
			_explainCriteria = dr.GetString("ExplainCriteria");
			_countryList = dr.GetString("CountryList");
			_explainIdea = dr.GetString("ExplainIdea");
			_comparedOtherCompetitorsCheck = dr.GetString("ComparedOtherCompetitorsCheck");
			_comparedOverallSpendCheck = dr.GetString("ComparedOverallSpendCheck");
			_typeCategoryOriginal = dr.GetString("TypeCategoryOriginal");
            _status = dr.GetString("Status");
			_typeCategory = dr.GetString("TypeCategory");
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
				cm.CommandText = "AddEntryForm";

				AddInsertParameters(cm);

				cm.ExecuteNonQuery();

			}//using
		}

		private void AddInsertParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@Id", _id);
			if (_idEntry != Guid.Empty)
				cm.Parameters.AddWithValue("@IdEntry", _idEntry);
			else
				cm.Parameters.AddWithValue("@IdEntry", DBNull.Value);
			cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
			cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
			if (_entryCategory.Length > 0)
				cm.Parameters.AddWithValue("@EntryCategory", _entryCategory);
			else
				cm.Parameters.AddWithValue("@EntryCategory", DBNull.Value);
			if (_desCountriesTime.Length > 0)
				cm.Parameters.AddWithValue("@DesCountriesTime", _desCountriesTime);
			else
				cm.Parameters.AddWithValue("@DesCountriesTime", DBNull.Value);
			if (_desTotalOfCountries.Length > 0)
				cm.Parameters.AddWithValue("@DesTotalOfCountries", _desTotalOfCountries);
			else
				cm.Parameters.AddWithValue("@DesTotalOfCountries", DBNull.Value);
			if (_country1.Length > 0)
				cm.Parameters.AddWithValue("@Country1", _country1);
			else
				cm.Parameters.AddWithValue("@Country1", DBNull.Value);
			if (_startdate1.Length > 0)
				cm.Parameters.AddWithValue("@Startdate1", _startdate1);
			else
				cm.Parameters.AddWithValue("@Startdate1", DBNull.Value);
			if (_enddate1.Length > 0)
				cm.Parameters.AddWithValue("@Enddate1", _enddate1);
			else
				cm.Parameters.AddWithValue("@Enddate1", DBNull.Value);
			if (_country2.Length > 0)
				cm.Parameters.AddWithValue("@Country2", _country2);
			else
				cm.Parameters.AddWithValue("@Country2", DBNull.Value);
			if (_startdate2.Length > 0)
				cm.Parameters.AddWithValue("@Startdate2", _startdate2);
			else
				cm.Parameters.AddWithValue("@Startdate2", DBNull.Value);
			if (_enddate2.Length > 0)
				cm.Parameters.AddWithValue("@Enddate2", _enddate2);
			else
				cm.Parameters.AddWithValue("@Enddate2", DBNull.Value);
			if (_country3.Length > 0)
				cm.Parameters.AddWithValue("@Country3", _country3);
			else
				cm.Parameters.AddWithValue("@Country3", DBNull.Value);
			if (_startdate3.Length > 0)
				cm.Parameters.AddWithValue("@Startdate3", _startdate3);
			else
				cm.Parameters.AddWithValue("@Startdate3", DBNull.Value);
			if (_enddate3.Length > 0)
				cm.Parameters.AddWithValue("@Enddate3", _enddate3);
			else
				cm.Parameters.AddWithValue("@Enddate3", DBNull.Value);
			if (_executiveSummary.Length > 0)
				cm.Parameters.AddWithValue("@ExecutiveSummary", _executiveSummary);
			else
				cm.Parameters.AddWithValue("@ExecutiveSummary", DBNull.Value);
			if (_describeMarket.Length > 0)
				cm.Parameters.AddWithValue("@DescribeMarket", _describeMarket);
			else
				cm.Parameters.AddWithValue("@DescribeMarket", DBNull.Value);
			if (_strategicChallengeObjectives.Length > 0)
				cm.Parameters.AddWithValue("@StrategicChallengeObjectives", _strategicChallengeObjectives);
			else
				cm.Parameters.AddWithValue("@StrategicChallengeObjectives", DBNull.Value);
			if (_ideas.Length > 0)
				cm.Parameters.AddWithValue("@Ideas", _ideas);
			else
				cm.Parameters.AddWithValue("@Ideas", DBNull.Value);
			if (_bringingIdea.Length > 0)
				cm.Parameters.AddWithValue("@BringingIdea", _bringingIdea);
			else
				cm.Parameters.AddWithValue("@BringingIdea", DBNull.Value);
			if (_communicationTouchPointsInitialYear.Length > 0)
				cm.Parameters.AddWithValue("@CommunicationTouchPointsInitialYear", _communicationTouchPointsInitialYear);
			else
				cm.Parameters.AddWithValue("@CommunicationTouchPointsInitialYear", DBNull.Value);
			if (_communicationTouchPointsInterimYear.Length > 0)
				cm.Parameters.AddWithValue("@CommunicationTouchPointsInterimYear", _communicationTouchPointsInterimYear);
			else
				cm.Parameters.AddWithValue("@CommunicationTouchPointsInterimYear", DBNull.Value);
			if (_communicationTouchPointsCurrentYear.Length > 0)
				cm.Parameters.AddWithValue("@CommunicationTouchPointsCurrentYear", _communicationTouchPointsCurrentYear);
			else
				cm.Parameters.AddWithValue("@CommunicationTouchPointsCurrentYear", DBNull.Value);
			if (_listAndExplainOtherMarketingText.Length > 0)
				cm.Parameters.AddWithValue("@ListAndExplainOtherMarketingText", _listAndExplainOtherMarketingText);
			else
				cm.Parameters.AddWithValue("@ListAndExplainOtherMarketingText", DBNull.Value);
			if (_listAndExplainOtherMarketingCheck.Length > 0)
				cm.Parameters.AddWithValue("@ListAndExplainOtherMarketingCheck", _listAndExplainOtherMarketingCheck);
			else
				cm.Parameters.AddWithValue("@ListAndExplainOtherMarketingCheck", DBNull.Value);
			if (_paidMediaExpendituresText.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresText", _paidMediaExpendituresText);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresText", DBNull.Value);
			if (_paidMediaExpendituresPercent.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresPercent", _paidMediaExpendituresPercent);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresPercent", DBNull.Value);
			if (_ownedMedia.Length > 0)
				cm.Parameters.AddWithValue("@OwnedMedia", _ownedMedia);
			else
				cm.Parameters.AddWithValue("@OwnedMedia", DBNull.Value);
			if (_sponsorship.Length > 0)
				cm.Parameters.AddWithValue("@Sponsorship", _sponsorship);
			else
				cm.Parameters.AddWithValue("@Sponsorship", DBNull.Value);
			if (_explainWorked.Length > 0)
				cm.Parameters.AddWithValue("@ExplainWorked", _explainWorked);
			else
				cm.Parameters.AddWithValue("@ExplainWorked", DBNull.Value);
			if (_anything.Length > 0)
				cm.Parameters.AddWithValue("@Anything", _anything);
			else
				cm.Parameters.AddWithValue("@Anything", DBNull.Value);
			if (_communicationTouchPointsCheck.Length > 0)
				cm.Parameters.AddWithValue("@CommunicationTouchPointsCheck", _communicationTouchPointsCheck);
			else
				cm.Parameters.AddWithValue("@CommunicationTouchPointsCheck", DBNull.Value);
			if (_selectAllOtherMarketing.Length > 0)
				cm.Parameters.AddWithValue("@SelectAllOtherMarketing", _selectAllOtherMarketing);
			else
				cm.Parameters.AddWithValue("@SelectAllOtherMarketing", DBNull.Value);
			if (_other.Length > 0)
				cm.Parameters.AddWithValue("@Other", _other);
			else
				cm.Parameters.AddWithValue("@Other", DBNull.Value);
			if (_explainListOtherMarketing.Length > 0)
				cm.Parameters.AddWithValue("@ExplainListOtherMarketing", _explainListOtherMarketing);
			else
				cm.Parameters.AddWithValue("@ExplainListOtherMarketing", DBNull.Value);
			if (_paidMediaExpendituresCheck.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresCheck", _paidMediaExpendituresCheck);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresCheck", DBNull.Value);
			if (_paidMediaExpendituresTotalBudget.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresTotalBudget", _paidMediaExpendituresTotalBudget);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresTotalBudget", DBNull.Value);
			if (_paidMediaExpendituresAveregeAnnual.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresAveregeAnnual", _paidMediaExpendituresAveregeAnnual);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresAveregeAnnual", DBNull.Value);
			if (_paidMediaExpendituresIndicate.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresIndicate", _paidMediaExpendituresIndicate);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresIndicate", DBNull.Value);
			if (_paidMediaExpendituresAstimates.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresAstimates", _paidMediaExpendituresAstimates);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresAstimates", DBNull.Value);
			if (_paidMediaExpendituresCompared.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresCompared", _paidMediaExpendituresCompared);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresCompared", DBNull.Value);
			if (_explainCriteria.Length > 0)
				cm.Parameters.AddWithValue("@ExplainCriteria", _explainCriteria);
			else
				cm.Parameters.AddWithValue("@ExplainCriteria", DBNull.Value);
			if (_countryList.Length > 0)
				cm.Parameters.AddWithValue("@CountryList", _countryList);
			else
				cm.Parameters.AddWithValue("@CountryList", DBNull.Value);
			if (_explainIdea.Length > 0)
				cm.Parameters.AddWithValue("@ExplainIdea", _explainIdea);
			else
				cm.Parameters.AddWithValue("@ExplainIdea", DBNull.Value);
			if (_comparedOtherCompetitorsCheck.Length > 0)
				cm.Parameters.AddWithValue("@ComparedOtherCompetitorsCheck", _comparedOtherCompetitorsCheck);
			else
				cm.Parameters.AddWithValue("@ComparedOtherCompetitorsCheck", DBNull.Value);
			if (_comparedOverallSpendCheck.Length > 0)
				cm.Parameters.AddWithValue("@ComparedOverallSpendCheck", _comparedOverallSpendCheck);
			else
				cm.Parameters.AddWithValue("@ComparedOverallSpendCheck", DBNull.Value);
			if (_typeCategoryOriginal.Length > 0)
				cm.Parameters.AddWithValue("@TypeCategoryOriginal", _typeCategoryOriginal);
			else
				cm.Parameters.AddWithValue("@TypeCategoryOriginal", DBNull.Value);
			if (_typeCategory.Length > 0)
				cm.Parameters.AddWithValue("@TypeCategory", _typeCategory);
			else
				cm.Parameters.AddWithValue("@TypeCategory", DBNull.Value);
            if (_status.Length > 0)
                cm.Parameters.AddWithValue("@Status", _status);
            else
                cm.Parameters.AddWithValue("@Status", DBNull.Value);
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
                cm.CommandText = "UpdateEntryForm";

                AddUpdateParameters(cm);

                cm.ExecuteNonQuery();

            }//using
        }

		private void AddUpdateParameters(SqlCommand cm)
		{
			cm.Parameters.AddWithValue("@Id", _id);
			if (_idEntry != Guid.Empty)
				cm.Parameters.AddWithValue("@IdEntry", _idEntry);
			else
				cm.Parameters.AddWithValue("@IdEntry", DBNull.Value);
			cm.Parameters.AddWithValue("@DateCreated", _dateCreated.DBValue);
			cm.Parameters.AddWithValue("@DateModified", _dateModified.DBValue);
			if (_entryCategory.Length > 0)
				cm.Parameters.AddWithValue("@EntryCategory", _entryCategory);
			else
				cm.Parameters.AddWithValue("@EntryCategory", DBNull.Value);
			if (_desCountriesTime.Length > 0)
				cm.Parameters.AddWithValue("@DesCountriesTime", _desCountriesTime);
			else
				cm.Parameters.AddWithValue("@DesCountriesTime", DBNull.Value);
			if (_desTotalOfCountries.Length > 0)
				cm.Parameters.AddWithValue("@DesTotalOfCountries", _desTotalOfCountries);
			else
				cm.Parameters.AddWithValue("@DesTotalOfCountries", DBNull.Value);
			if (_country1.Length > 0)
				cm.Parameters.AddWithValue("@Country1", _country1);
			else
				cm.Parameters.AddWithValue("@Country1", DBNull.Value);
			if (_startdate1.Length > 0)
				cm.Parameters.AddWithValue("@Startdate1", _startdate1);
			else
				cm.Parameters.AddWithValue("@Startdate1", DBNull.Value);
			if (_enddate1.Length > 0)
				cm.Parameters.AddWithValue("@Enddate1", _enddate1);
			else
				cm.Parameters.AddWithValue("@Enddate1", DBNull.Value);
			if (_country2.Length > 0)
				cm.Parameters.AddWithValue("@Country2", _country2);
			else
				cm.Parameters.AddWithValue("@Country2", DBNull.Value);
			if (_startdate2.Length > 0)
				cm.Parameters.AddWithValue("@Startdate2", _startdate2);
			else
				cm.Parameters.AddWithValue("@Startdate2", DBNull.Value);
			if (_enddate2.Length > 0)
				cm.Parameters.AddWithValue("@Enddate2", _enddate2);
			else
				cm.Parameters.AddWithValue("@Enddate2", DBNull.Value);
			if (_country3.Length > 0)
				cm.Parameters.AddWithValue("@Country3", _country3);
			else
				cm.Parameters.AddWithValue("@Country3", DBNull.Value);
			if (_startdate3.Length > 0)
				cm.Parameters.AddWithValue("@Startdate3", _startdate3);
			else
				cm.Parameters.AddWithValue("@Startdate3", DBNull.Value);
			if (_enddate3.Length > 0)
				cm.Parameters.AddWithValue("@Enddate3", _enddate3);
			else
				cm.Parameters.AddWithValue("@Enddate3", DBNull.Value);
			if (_executiveSummary.Length > 0)
				cm.Parameters.AddWithValue("@ExecutiveSummary", _executiveSummary);
			else
				cm.Parameters.AddWithValue("@ExecutiveSummary", DBNull.Value);
			if (_describeMarket.Length > 0)
				cm.Parameters.AddWithValue("@DescribeMarket", _describeMarket);
			else
				cm.Parameters.AddWithValue("@DescribeMarket", DBNull.Value);
			if (_strategicChallengeObjectives.Length > 0)
				cm.Parameters.AddWithValue("@StrategicChallengeObjectives", _strategicChallengeObjectives);
			else
				cm.Parameters.AddWithValue("@StrategicChallengeObjectives", DBNull.Value);
			if (_ideas.Length > 0)
				cm.Parameters.AddWithValue("@Ideas", _ideas);
			else
				cm.Parameters.AddWithValue("@Ideas", DBNull.Value);
			if (_bringingIdea.Length > 0)
				cm.Parameters.AddWithValue("@BringingIdea", _bringingIdea);
			else
				cm.Parameters.AddWithValue("@BringingIdea", DBNull.Value);
			if (_communicationTouchPointsInitialYear.Length > 0)
				cm.Parameters.AddWithValue("@CommunicationTouchPointsInitialYear", _communicationTouchPointsInitialYear);
			else
				cm.Parameters.AddWithValue("@CommunicationTouchPointsInitialYear", DBNull.Value);
			if (_communicationTouchPointsInterimYear.Length > 0)
				cm.Parameters.AddWithValue("@CommunicationTouchPointsInterimYear", _communicationTouchPointsInterimYear);
			else
				cm.Parameters.AddWithValue("@CommunicationTouchPointsInterimYear", DBNull.Value);
			if (_communicationTouchPointsCurrentYear.Length > 0)
				cm.Parameters.AddWithValue("@CommunicationTouchPointsCurrentYear", _communicationTouchPointsCurrentYear);
			else
				cm.Parameters.AddWithValue("@CommunicationTouchPointsCurrentYear", DBNull.Value);
			if (_listAndExplainOtherMarketingText.Length > 0)
				cm.Parameters.AddWithValue("@ListAndExplainOtherMarketingText", _listAndExplainOtherMarketingText);
			else
				cm.Parameters.AddWithValue("@ListAndExplainOtherMarketingText", DBNull.Value);
			if (_listAndExplainOtherMarketingCheck.Length > 0)
				cm.Parameters.AddWithValue("@ListAndExplainOtherMarketingCheck", _listAndExplainOtherMarketingCheck);
			else
				cm.Parameters.AddWithValue("@ListAndExplainOtherMarketingCheck", DBNull.Value);
			if (_paidMediaExpendituresText.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresText", _paidMediaExpendituresText);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresText", DBNull.Value);
			if (_paidMediaExpendituresPercent.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresPercent", _paidMediaExpendituresPercent);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresPercent", DBNull.Value);
			if (_ownedMedia.Length > 0)
				cm.Parameters.AddWithValue("@OwnedMedia", _ownedMedia);
			else
				cm.Parameters.AddWithValue("@OwnedMedia", DBNull.Value);
			if (_sponsorship.Length > 0)
				cm.Parameters.AddWithValue("@Sponsorship", _sponsorship);
			else
				cm.Parameters.AddWithValue("@Sponsorship", DBNull.Value);
			if (_explainWorked.Length > 0)
				cm.Parameters.AddWithValue("@ExplainWorked", _explainWorked);
			else
				cm.Parameters.AddWithValue("@ExplainWorked", DBNull.Value);
			if (_anything.Length > 0)
				cm.Parameters.AddWithValue("@Anything", _anything);
			else
				cm.Parameters.AddWithValue("@Anything", DBNull.Value);
			if (_communicationTouchPointsCheck.Length > 0)
				cm.Parameters.AddWithValue("@CommunicationTouchPointsCheck", _communicationTouchPointsCheck);
			else
				cm.Parameters.AddWithValue("@CommunicationTouchPointsCheck", DBNull.Value);
			if (_selectAllOtherMarketing.Length > 0)
				cm.Parameters.AddWithValue("@SelectAllOtherMarketing", _selectAllOtherMarketing);
			else
				cm.Parameters.AddWithValue("@SelectAllOtherMarketing", DBNull.Value);
			if (_other.Length > 0)
				cm.Parameters.AddWithValue("@Other", _other);
			else
				cm.Parameters.AddWithValue("@Other", DBNull.Value);
			if (_explainListOtherMarketing.Length > 0)
				cm.Parameters.AddWithValue("@ExplainListOtherMarketing", _explainListOtherMarketing);
			else
				cm.Parameters.AddWithValue("@ExplainListOtherMarketing", DBNull.Value);
			if (_paidMediaExpendituresCheck.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresCheck", _paidMediaExpendituresCheck);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresCheck", DBNull.Value);
			if (_paidMediaExpendituresTotalBudget.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresTotalBudget", _paidMediaExpendituresTotalBudget);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresTotalBudget", DBNull.Value);
			if (_paidMediaExpendituresAveregeAnnual.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresAveregeAnnual", _paidMediaExpendituresAveregeAnnual);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresAveregeAnnual", DBNull.Value);
			if (_paidMediaExpendituresIndicate.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresIndicate", _paidMediaExpendituresIndicate);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresIndicate", DBNull.Value);
			if (_paidMediaExpendituresAstimates.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresAstimates", _paidMediaExpendituresAstimates);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresAstimates", DBNull.Value);
			if (_paidMediaExpendituresCompared.Length > 0)
				cm.Parameters.AddWithValue("@PaidMediaExpendituresCompared", _paidMediaExpendituresCompared);
			else
				cm.Parameters.AddWithValue("@PaidMediaExpendituresCompared", DBNull.Value);
			if (_explainCriteria.Length > 0)
				cm.Parameters.AddWithValue("@ExplainCriteria", _explainCriteria);
			else
				cm.Parameters.AddWithValue("@ExplainCriteria", DBNull.Value);
			if (_countryList.Length > 0)
				cm.Parameters.AddWithValue("@CountryList", _countryList);
			else
				cm.Parameters.AddWithValue("@CountryList", DBNull.Value);
			if (_explainIdea.Length > 0)
				cm.Parameters.AddWithValue("@ExplainIdea", _explainIdea);
			else
				cm.Parameters.AddWithValue("@ExplainIdea", DBNull.Value);
			if (_comparedOtherCompetitorsCheck.Length > 0)
				cm.Parameters.AddWithValue("@ComparedOtherCompetitorsCheck", _comparedOtherCompetitorsCheck);
			else
				cm.Parameters.AddWithValue("@ComparedOtherCompetitorsCheck", DBNull.Value);
			if (_comparedOverallSpendCheck.Length > 0)
				cm.Parameters.AddWithValue("@ComparedOverallSpendCheck", _comparedOverallSpendCheck);
			else
				cm.Parameters.AddWithValue("@ComparedOverallSpendCheck", DBNull.Value);
			if (_typeCategoryOriginal.Length > 0)
				cm.Parameters.AddWithValue("@TypeCategoryOriginal", _typeCategoryOriginal);
			else
				cm.Parameters.AddWithValue("@TypeCategoryOriginal", DBNull.Value);
			if (_typeCategory.Length > 0)
				cm.Parameters.AddWithValue("@TypeCategory", _typeCategory);
			else
				cm.Parameters.AddWithValue("@TypeCategory", DBNull.Value);
            if (_status.Length > 0)
                cm.Parameters.AddWithValue("@Status", _status);
            else
                cm.Parameters.AddWithValue("@Status", DBNull.Value);
        }

		private void UpdateChildren(SqlConnection cn)
		{
		}
		#endregion //Data Access - Update

        #region Data Access - Delete
        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(new Criteria(_id, _idEntry));
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
				cm.CommandText = "DeleteEntryForm";

				cm.Parameters.AddWithValue("@Id", criteria.Id);

				cm.ExecuteNonQuery();
			}//using
		}
		#endregion //Data Access - Delete
		#endregion //Data Access
	}
}
