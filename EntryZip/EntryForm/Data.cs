using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Data
/// </summary>
/// 

public class Data
{
   
    public Data()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    
    public static List<CollectData> GetFlagReasons()
    {
        List<CollectData> datas = new List<CollectData>();
        foreach (FlagReasons FlagReason in FlagReasonsList.GetFlagReasonsList().OrderBy(x => x.Bodyname))
        {
            if (FlagReason.IsActive && FlagReason.Bodyname.ToLower() != "other")
            {
                Data.CollectData data = new Data.CollectData();
                data.id = FlagReason.Id.ToString();
                data.AttrType = FlagReason.Type;
                data.Title = FlagReason.Bodyname;
                data.Desc = FlagReason.Description;
                data.isHasOther = FlagReason.isHasOther;
                data.Data1 = "";
                data.Data2 = "False";
                datas.Add(data);
            }
        }

        foreach (FlagReasons FlagReason in FlagReasonsList.GetFlagReasonsList())
        {
            if (FlagReason.IsActive && FlagReason.Bodyname.ToLower() == "other")
            {
                Data.CollectData data = new Data.CollectData();
                data.id = FlagReason.Id.ToString();
                data.AttrType = FlagReason.Type;
                data.Title = FlagReason.Bodyname;
                data.Desc = FlagReason.Description;
                data.isHasOther = FlagReason.isHasOther;
                data.Data1 = "";
                data.Data2 = "False";
                datas.Add(data);
            }
        }

        return datas;
    }

    public static List<CollectData> GetCTPSingle() {

        List<CollectData> datas = new List<CollectData>();
        for (int x = 0; x <= Data.CTPSingle.GetUpperBound(0); x++)
        {
            Data.CollectData data = new Data.CollectData();
            data.id = Data.CTPSingle[x, 0];
            data.AttrType = Data.CTPSingle[x, 1];
            data.Title = Data.CTPSingle[x, 2];
            datas.Add(data);
        }
        return datas;
    }

    public static List<CountriesList> GetExplainListOtherMarketingList() {

        List<CountriesList> datas = new List<CountriesList>();
        for (int x = 0; x <= Data.ExplainListOtherMarketingList.GetUpperBound(0); x++)
        {
            Data.CountriesList ELOM = new Data.CountriesList();
            ELOM.id = Data.ExplainListOtherMarketingList[x, 0];
            ELOM.Title = Data.ExplainListOtherMarketingList[x, 1];
            ELOM.Country1 = null;
            ELOM.Country2 = null;
            ELOM.Country3 = null;
            datas.Add(ELOM);
        }

        return datas;
    }

    public static List<CollectData> GetCTPPCE()
    {

        List<CollectData> datas = new List<CollectData>();
        for (int x = 0; x <= Data.CTPPCE.GetUpperBound(0); x++)
        {
            Data.CollectData data = new Data.CollectData();
            data.id = Data.CTPPCE[x, 0];
            data.AttrType = Data.CTPPCE[x, 1];
            data.Title = Data.CTPPCE[x, 2];
            datas.Add(data);
        }
        return datas;
    }

    public static List<CollectData> GetCommunicationTouchPointCountryMulti()
    {
        List<CollectData> datas = new List<CollectData>();
        for (int x = 0; x <= Data.CommunicationTouchPointCountryMulti.GetUpperBound(0); x++)
        {
            Data.CollectData data = new Data.CollectData();
            data.id = Data.CommunicationTouchPointCountryMulti[x, 0];
            data.AttrType = Data.CommunicationTouchPointCountryMulti[x, 1];
            data.Title = Data.CommunicationTouchPointCountryMulti[x, 2];
            datas.Add(data);
        }
        return datas;
    }
    public static List<CollectData> GetCTPMulti()
    {

        List<CollectData> datas = new List<CollectData>();
        for (int x = 0; x <= Data.CTPMulti.GetUpperBound(0); x++)
        {
            Data.CollectData data = new Data.CollectData();
            data.id = Data.CTPMulti[x, 0];
            data.AttrType = Data.CTPMulti[x, 1];
            data.Title = Data.CTPMulti[x, 2];
            datas.Add(data);
        }
        return datas;
    }

    public static List<CollectData> GetPaidMediaExpendituresData()
    {

        List<CollectData> datas = new List<CollectData>();
        for (int x = 0; x <= Data.PaidMediaExpendituresData.GetUpperBound(0); x++)
        {
            Data.CollectData ELOM = new Data.CollectData();
            ELOM.id = Data.PaidMediaExpendituresData[x, 0];
            ELOM.Data1 = Data.PaidMediaExpendituresData[x, 1];
            ELOM.Data2 = Data.PaidMediaExpendituresData[x, 2];
            ELOM.value = Data.PaidMediaExpendituresData[x, 3];
            datas.Add(ELOM);
        }

        return datas;
    }

    public static List<ExplainListOtherMarketingViewmodel> GetListExplainPoitAllOthers()
    {
        List<ExplainListOtherMarketingViewmodel> datas = new List<ExplainListOtherMarketingViewmodel>();
        for (int x = 0; x <= Data.ListExplainPoitAllOthers.GetUpperBound(0); x++)
        {
            Data.ExplainListOtherMarketingViewmodel ELOM = new Data.ExplainListOtherMarketingViewmodel();
            ELOM.id = Data.ListExplainPoitAllOthers[x, 0];
            ELOM.MarketingComponents = Data.ListExplainPoitAllOthers[x, 1];
            ELOM.TimePeriod = null;
            datas.Add(ELOM);
        }
        return datas;
    }
    

    public static List<ExplainListOtherMarketingViewmodel> GetDataListExplainAllOthers()
    {
        List<ExplainListOtherMarketingViewmodel> datas = new List<ExplainListOtherMarketingViewmodel>();
        for (int x = 0; x <= Data.ListExplainAllOthers.GetUpperBound(0); x++)
        {
            Data.ExplainListOtherMarketingViewmodel ELOM = new Data.ExplainListOtherMarketingViewmodel();
            ELOM.id = Data.ListExplainAllOthers[x, 0];
            ELOM.MarketingComponents = Data.ListExplainAllOthers[x, 1];
            ELOM.TimePeriod = null;
            datas.Add(ELOM);
        }
        return datas;
    }

    public static List<CollectData> GetCommunicationTouchPointData()
    {
        List<CollectData> datas = new List<CollectData>();
        for (int x = 0; x <= Data.CommunicationTouchPointData.GetUpperBound(0); x++)
        {
            Data.CollectData ELOM = new Data.CollectData();
            ELOM.id = Data.CommunicationTouchPointData[x, 0];
            ELOM.Title = Data.CommunicationTouchPointData[x, 1];
            datas.Add(ELOM);
        }
        return datas;
    }

    

    public static string GetCategoryMarket(string Category)
    {
        if (Category == "MM") return "Multi Market";
        if (Category == "SM") return "Single Market";
        else return "";
    }


    public static string GetCategoryPS(string Category)
    {
        if (Category == "PSC") return "Product & Services Category";
        else if (Category == "SC") return "Specialty Category";
        else return "";
    }

    public enum DocumentCategories
    {
        SustainedSuccess,
        MultiMarket,
        PositiveChangeEnvironmental,
        ShopperMarketing,
        SingleMarket
    };
    
    public class ExplainListOtherMarketingViewmodel
    {
        public string id;
        public string MarketingComponents;
        public string TimePeriod;
    }

    public class CollectData
    {
        public string id;
        public string Title;
        public string Desc;
        public bool isHasOther;
        public string AttrType;
        public string Data1;
        public string Data2;
        public string Data3;
        public string Data4;
        public string Data5;
        public string value;
        public Entry Entry;
        public Registration Registration;
    }

    public class CountriesList
    {
        public string id;
        public string Title;
        public string Country1;
        public string Country2;
        public string Country3;
        public string AttrType;
    }

    protected static string[,] CommunicationTouchPointData = {
                { "001", "<span style='font-weight: bold;'>Branded Content</span>", "", "" },
                { "002", "<span style='font-weight: bold;'>Cinema</span>", "", "" },
                { "003", "<span style='font-weight: bold;'>Consumer Involvement</span> (Consumer generated, viral, WOM)", "", "" },
                { "004", "<span style='font-weight: bold;'>Direct</span> (Mail, Email)", "", "" },
                { "005", "<span style='font-weight: bold;'>Ecommerce</span>", "", "" },
                { "006", "<span style='font-weight: bold;'>Events</span>", "", "" },
                { "007", "<span style='font-weight: bold;'>Guerrilla</span> (Ambient media, buzz marketing, sampling/trial, street teams, tagging, wraps)", "", "" },
                { "008", "<span style='font-weight: bold;'>Interactive/Online</span> (display ads, branded website/microsite, mobile/tablet optimized website, digital video, video skins/bugs, podcasts, gaming, contests, geo-based ads, other)", "", "" },
                { "009", "<span style='font-weight: bold;'>Internal Marketing</span>", "", "" },
                { "010", "<span style='font-weight: bold;'>Mobile/Tablet</span> (app, display ad, in-app or in-game ad, location-based communications/real time marketing, messaging/editorial/content, other)", "", "" },
                { "011", "<span style='font-weight: bold;'>OOH</span> (airport, billboard, place based, transit, other)", "", "" },
                { "012", "<span style='font-weight: bold;'>Packaging</span>", "", "" },
                { "013", "<span style='font-weight: bold;'>Point of Care (POC)</span> (brochures, coverwraps, electronic check-in, wallboards, video – HAN & Accent Health, etc.)", "", "" },
                { "014", "<span style='font-weight: bold;'>PR</span>", "", "" },
                { "015", "<span style='font-weight: bold;'>Print</span> (custom publication, magazine-print or digital, newspaper-print or digital, trade/professional)", "", "" },
                { "016", "<span style='font-weight: bold;'>Product Design</span>", "", "" },
                { "017", "<span style='font-weight: bold;'>Professional Engagement</span> (closed loop marketing (CLM), congresses, continuing engagement, detail/e-detail/ interactive visual aids (IVAs), informational/documentary video, in-office)", "", "" },
                { "018", "<span style='font-weight: bold;'>Radio</span> (merchandising, program/content, spots)", "", "" },
                { "019", "<span style='font-weight: bold;'>Retail Experience</span> (in-store merchandising, in-store video, pharmacy, POP, retailtainment, store within a store, other)", "", "" },
                { "020", "<span style='font-weight: bold;'>Sales Promotion</span>", "", "" },
                { "021", "<span style='font-weight: bold;'>Search Engine Marketing (SEM/SEO)</span>", "", "" },
                { "022", "<span style='font-weight: bold;'>Social Media</span>", "", "" },
                { "023", "<span style='font-weight: bold;'>Sponsorship</span>", "", "" },
                { "024", "<span style='font-weight: bold;'>Trade Shows</span>", "", "" },
                { "025", "<span style='font-weight: bold;'>TV</span> (spots, branded content, sponsorship, product placement, interactive TV/Video on Demand)", "", "" }
    };
    
    public static string[,] ExplainListOtherMarketingData = {
                        { "001", "None", "" },
                        { "002", "Couponing", "" },
                        { "003", "CRM / Loyalty Programs", "" },
                        { "005", "Giveaways / Sampling", "" },
                        { "006", "Leveraging Distribution", "" },
                        { "007", "Other marketing for the brand running at the same time as the entered effort/campaign", "" },
                        { "008", "Pricing Changes", "" }
                      };
    
    protected static string[,] FlagReason = {
                        {"001", "Header", "ENTRY DETAILS"},
                        {"002", "Body", "Entry No. missing"},
                        {"003", "Body", "Entry No. incorrect"},
                        {"004", "Body", "Entry Title missing"},
                        {"005", "Body", "Entry Title incorrect"},
                        {"006", "Body", "Category missing"},
                        {"007", "Body", "Category incorrect"},
                        {"008", "Body", "Brand Name missing"},
                        {"009", "Body", "Brand Name incorrect"},
                        {"010", "Body", "Markets (Effectiveness) missing"},
                        {"011", "Body", "Markets (Effectiveness) incorrect"},
                        {"012", "Body", "Start & End Date missing"},
                        {"013", "Body", "Start & End Date incorrect"},
                        {"014", "Header", "AUTHORISATION FORM"},
                        {"015", "Body", "Incorrect form"},
                        {"016", "Body", "Signatures Incomplete/Missing"},
                        {"017", "Body", "Entry ID Incorrect"},
                        {"018", "Body", "Entry ID Incomplete/Missing"},
                        {"019", "Body", "Category Incorrect"},
                        {"020", "Body", "Category Incomplete/Missing"},
                        {"021", "Header", "ENTRY FORM"},
                        {"022", "Body", "Incorrect Entry Form"},
                        {"023", "Body", "Has logos, pictorial elements, screengrabs"},
                        {"024", "Body", "Has image of creative work"},
                        {"025", "Body", "Has image of competitor works"},
                        {"026", "Body", "Font size less than 10 point"},
                        {"027", "Body", "Colour Font present"},
                        {"028", "Body", "Exceeds the page limit for selected category [refer to Entry Processing Checklist]"},
                        {"029", "Body", "Cover page, all questions & instructions. option checkboxes are not intact"},
                        {"030", "Body", "Agency name present"},
                        {"031", "Body", "Results stated after 30/9/2017"},
                        {"032", "Body", "Results section has incomplete or no sourcing"},
                        {"033", "Body", "Sections other than results has incomplete or no sourcing"},
                        {"034", "Body", "Questions section is not completed"},
                        {"035", "Header", "Applicable ONLY for Small Budget:"},
                        {"036", "Body", " Entry is over qualifying budget amount"},
                        {"037", "Header", "CREATIVE MATERIALS:"},
                        {"038", "SubHeader", "Option A: Creative Showcase (PDF):"},
                        {"039", "Body", " Agency name present"},
                        {"040", "Body", " PDF more than 12 pages"},
                        {"041", "Body", " Results present"},
                        {"042", "Body", " Competitive work or logos present"},
                        {"043", "SubHeader", "Option B: Creative Video:"},
                        {"044", "Body", " Agency Name Present"},
                        {"045", "Body", " Video more than 4 mins"},
                        {"046", "Body", " Results on video"},
                        {"047", "Body", " Video does not have sound"},
                        {"048", "Body", " Competitive work or logos present"},
                        {"049", "SubHeader", "Option C: Creative Video + Still Images (PDF)"},
                        {"050", "Body", " Agency name present on video and/or PDF"},
                        {"051", "Body", " Results on video and/or PDF"},
                        {"052", "Body", " Competitive work or logos present on video and/or PDF"},
                        {"053", "Body", " Video more than 4 mins"},
                        {"054", "Body", " Video does not have sound"},
                        {"055", "Body", " PDF is more than 2 slides"}
                };

    protected static string[,] PaidMediaExpendituresData = {
                        { "001", "Under $100K", "Under $100K", "1" },
                        { "002", "$100K – under $250K", "$100K – under $250K", "2" },
                        { "003", "$250K – under $500K", "$250K – under $500K", "3" },
                        { "004", "$500K – under $1M", "$500K – under $1M", "4" },
                        { "005", "$1M – under $5M", "$1M – under $5M", "5" },
                        { "006", "$5M – under $10M", "$5M – under $10M", "6" },
                        { "007", "$10M – under $20M", "$10M – under $20M", "7" },
                        { "008", "$20M and over", "$20M and over", "8" },
                        { "009", "Not Applicable", "NA (Elaboration Required)", "9" },
                      };

    public static string[,] ExplainListOtherMarketingList = {
                        { "001", "None", "", "" },
                        { "002", "Couponing", "", "" },
                        { "003", "CRM / Loyalty Programmes", "", "" },
                        { "004", "Giveaways / Sampling", "", "" },
                        { "005", "Leveraging Distribution", "", "" },
                        { "006", "Other marketing for the brand running at the same time as the entered effort/campaign", "", "" },
                        { "007", "Pricing Changes", "", "" }
                    };

    public static string[,] PaidMediaExpendituresPart1 = {
                        { "001", "Total Budget Range for this case from  in the current year","",""},
                        { "002", "Average annual budget for this case in the prior year (enter NA if not applicable)","",""},
                        { "003", "Indicate the approximate % of the case's total media budget spent in each market.  E.g. If your total media budget was X for the case over 10 markets this should = 100%.  What % out of 100% was spent in each of the markets you selected?","",""}
                      };
    
    public static string[,] PaidMediaExpendituresPart2 = {
                        { "001", "Compared to other competitors in this category, this budget is: ","",""},
                        { "002", "Compared to overall spend on the brand in the prior year, the brand’s overall budget this year:","",""}
                      };

    protected static string[,] ListExplainAllOthers = {
                        { "002", "Couponing","",""},
                        { "003", "CRM/Loyalty Programs","",""},
                        { "004", "Economic Factors","",""},
                        { "005", "Giveaways/Sampling","",""},
                        { "006", "Leveraging Distribution","",""},
                        { "007", "Other marketing for the brand, running at the same time this effort","",""},
                        { "008", "Pricing Changes","",""},
                        { "009", "Weather","",""},
                        { "001", "None","",""},
                      };

    protected static string[,] ListExplainPoitAllOthers = {
                        { "002", "Couponing","",""},
                        { "003", "CRM/Loyalty Programs","",""},
                        { "004", "Economic Factors","",""},
                        { "005", "Giveaways/Sampling","",""},
                        { "006", "Leveraging Distribution","",""},
                        { "007", "Other marketing for the brand, running at the same time as this effort","",""},
                        { "008", "Pricing Changes","",""},
                        { "009", "Weather","",""},
                        { "001", "None","",""},
                      };

    protected static string[,] CommunicationTouchPointCountryMulti = {
                        { "001", "SingleHeader", "Branded Content", "" },
                        { "002", "SingleHeader", "Cinema", "" },
                        { "003", "Header", "Consumer Involvement/User Generated", "" },
                        { "004", "Body", "Consumer Generated", "" },
                        { "005", "Body", "Viral", "" },
                        { "006", "Body", "WOM", "" },
                        { "007", "Header", "Direct", "" },
                        { "008", "Body", "Email", "" },
                        { "009", "Body", "Mail", "" },
                        { "010", "SingleHeader", "E-Commerce", "" },
                        { "011", "SingleHeader", "Events", "" },
                        { "012", "Header", "Guerrilla", "" },
                        { "013", "Body", "Ambient Media", "" },
                        { "014", "Body", "Buzz Marketing", "" },
                        { "015", "Body", "Sampling/Trial", "" },
                        { "016", "Body", "Street Teams", "" },
                        { "017", "Body", "Tagging", "" },
                        { "018", "Body", "Wraps", "" },
                        { "019", "Header", "Interactive/Online", "" },
                        { "020", "Body", "Brand Website/ Microsite", "" },
                        { "021", "Body", "Contests", "" },
                        { "022", "Body", "Digital Video", "" },
                        { "023", "Body", "Display Ads", "" },
                        { "024", "Body", "Gaming", "" },
                        { "025", "Body", "Geo-based Ads", "" },
                        { "026", "Body", "Mobile/Tablet Optimized Website", "" },
                        { "027", "Body", "Podcasts", "" },
                        { "028", "Body", "Video Skins/Bugs", "" },
                        { "029", "Body", "Other", "" },
                        { "030", "SingleHeader", "Internal Marketing", "" },
                        { "031", "Header", "Mobile/Tablet", "" },
                        { "032", "Body", "App", "" },
                        { "033", "Body", "Display Ad", "" },
                        { "034", "Body", "In-App or In-Game Ad", "" },
                        { "035", "Body", "Location-based Communications/Real Time Marketing", "" },
                        { "036", "Body", "Messaging/Editorial/ Content", "" },
                        { "037", "Body", "Other", "" },
                        { "038", "Header", "OOH", "" },
                        { "039", "Body", "Airport", "" },
                        { "040", "Body", "Billboard", "" },
                        { "041", "Body", "Place-Based", "" },
                        { "042", "Body", "Transit", "" },
                        { "043", "Body", "Other", "" },
                        { "044", "SingleHeader", "Packaging", "" },
                        { "045", "Header", "Point of Care (POC)", "" },
                        { "046", "Body", "Brochures", "" },
                        { "047", "Body", "Coverwraps", "" },
                        { "048", "Body", "Electronic Check-In", "" },
                        { "049", "Body", "Video (HAN, Accent Health)", "" },
                        { "050", "Body", "Wallboards", "" },
                        { "051", "Body", "Other", "" },
                        { "052", "SingleHeader", "PR", "" },
                        { "053", "Header", "Print", "" },
                        { "054", "Body", "Custom Publication", "" },
                        { "055", "Body", "Magazine (Digital)", "" },
                        { "056", "Body", "Magazine (Print)", "" },
                        { "057", "Body", "Newspaper (Digital)", "" },
                        { "058", "Body", "Newspaper (Print)", "" },
                        { "059", "Body", "Trade/Professional", "" },
                        { "060", "SingleHeader", "Product Design", "" },
                        { "061", "Header", "Professional Engagement", "" },
                        { "062", "Body", "Closed Loop Marketing (CLM)", "" },
                        { "063", "Body", "Congresses", "" },
                        { "064", "Body", "Continuing Engagement", "" },
                        { "065", "Body", "Detail/E-Detail/Interactive Visual Aids (IVAs)", "" },
                        { "066", "Body", "Informational/ Documentary Video", "" },
                        { "067", "Body", "In-Office", "" },
                        { "068", "Header", "Radio", "" },
                        { "069", "Body", "Merchandising", "" },
                        { "070", "Body", "Program/Content", "" },
                        { "071", "Body", "Spots", "" },
                        { "072", "Header", "Retail Experience", "" },
                        { "073", "Body", "In-Store Merchandising", "" },
                        { "074", "Body", "In-Store Video", "" },
                        { "075", "Body", "Pharmacy", "" },
                        { "076", "Body", "POP", "" },
                        { "077", "Body", "Retailtainment", "" },
                        { "078", "Body", "Store within a Store", "" },
                        { "079", "Body", "Other", "" },
                        { "080", "SingleHeader", "Sales Promotion", "" },
                        { "081", "SingleHeader", "Search Engine Marketing (SEM/SEO)", "" },
                        { "082", "SingleHeader", "Social Media", "" },
                        { "083", "SingleHeader", "Sponsorship", "" },
                        { "084", "SingleHeader", "Trade Shows", "" },
                        { "085", "Header", "TV", "" },
                        { "086", "Body", "Branded Content", "" },
                        { "087", "Body", "Interactive TV/ Video on Demand", "" },
                        { "088", "Body", "Product Placement", "" },
                        { "089", "Body", "Sponsorship", "" },
                        { "090", "Body", "Spots", "" },
                      };


    protected static string[,] CTPMulti = {
                        { "001", "SingleHeader", "Branded Content", "" },
                        { "002", "Header", "Digital/Interactive", "" },
                        { "003", "Body", "Developed Retailer Site Content", "" },
                        { "004", "Body", "Digital Video", "" },
                        { "005", "Body", "Display Ads", "" },
                        { "006", "Body", "Gaming", "" },
                        { "007", "Body", "MFR/Retailer Website", "" },
                        { "008", "Body", "Other", "" },
                        { "009", "Header", "Direct", "" },
                        { "011", "Body", "Email", "" },
                        { "010", "Body", "Mail", "" },
                        { "012", "Body", "Retailer Specific", "" },
                        { "013", "SingleHeader", "Distribution Changes", "" },
                        { "014", "SingleHeader", "Ecommerce", "" },
                        { "015", "SingleHeader", "Events", "" },
                        { "016", "Header", "Guerrilla", "" },
                        { "017", "Body", "Ambient Media", "" },
                        { "018", "Body", "Buzz Marketing", "" },
                        { "019", "Body", "Street Teams", "" },
                        { "020", "Body", "Tagging", "" },
                        { "021", "Body", "Wraps", "" },
                        { "022", "Header", "Mobile/Tablet", "" },
                        { "023", "Body", "App", "" },
                        { "024", "Body", "Display Ad", "" },
                        { "025", "Body", "In-App or In-Game Ad", "" },
                        { "026", "Body", "Location-based Communications/ Real Time Marketing", "" },
                        { "027", "Body", "Messaging/Editorial/Content", "" },
                        { "028", "Body", "Other", "" },
                        { "029", "Header", "OOH", "" },
                        { "030", "Body", "Airport", "" },
                        { "031", "Body", "Billboard", "" },
                        { "032", "Body", "Place Based", "" },
                        { "033", "Body", "Transit", "" },
                        { "034", "Body", "Other", "" },
                        { "035", "SingleHeader", "Packaging", "" },
                        { "036", "SingleHeader", "PR", "" },
                        { "037", "Header", "Pricing", "" },
                        { "038", "Body", "Couponing", "" },
                        { "039", "Body", "Trade", "" },
                        { "040", "Header", "Print", "" },
                        { "041", "Body", "Custom Publication ", "" },
                        { "042", "Body", "Magazine – Digital ", "" },
                        { "043", "Body", "Magazine – Print", "" },
                        { "044", "Body", "Newspaper – Digital", "" },
                        { "045", "Body", "Newspaper – Print", "" },
                        { "046", "Body", "Retailer Specific Publication", "" },
                        { "047", "Header", "Product Design", "" },
                        { "048", "Body", "Account Specific", "" },
                        { "049", "Body", "Promo Specific", "" },
                        { "050", "Header", "Radio", "" },
                        { "051", "Body", "Promo/Endorsements", "" },
                        { "052", "Body", "Program/Content", "" },
                        { "053", "Body", "Spots", "" },
                        { "054", "Header", "Retail Experience", "" },
                        { "055", "Body", "In-Store Merchandising", "" },
                        { "056", "Body", "In-Store Video/Kiosk", "" },
                        { "057", "Body", "POP", "" },
                        { "058", "Body", "Retailtainment", "" },
                        { "059", "Body", "Sales Promotion", "" },
                        { "060", "Body", "Store within a Store", "" },
                        { "061", "Header", "Sampling", "" },
                        { "062", "Body", "Direct Mail", "" },
                        { "063", "Body", "In-Store", "" },
                        { "064", "Body", "OOH (event)", "" },
                        { "065", "SingleHeader", "Search Engine Marketing (SEM/SEO)", "" },
                        { "066", "Header", "Shopper Involvement", "" },
                        { "067", "Body", "Consumer Generated", "" },
                        { "068", "Body", "Viral", "" },
                        { "069", "Body", "WOM", "" },
                        { "070", "SingleHeader", "Social Media", "" },
                        { "071", "SingleHeader", "Sponsorship", "" },
                        { "072", "SingleHeader", "Trade Communications/ Promo", "" },
                        { "073", "SingleHeader", "Trade Shows", "" },
                        { "074", "Header", "TV", "" },
                        { "075", "Body", "Branded Content ", "" },
                        { "076", "Body", "Co-op", "" },
                        { "078", "Body", "National Tagged Spots", "" },
                        { "079", "Body", "Product Placement", "" },
                        { "080", "Body", "Sponsorship", "" },
                        { "077", "Body", "Spots", "" },
                        //{ "081", "Body", "Spots", "" }
                    };

    protected static string[,] CTPSingle = {
                        { "001", "SingleHeader", "Branded Content", "" },
                        { "002", "SingleHeader", "Cinema", "" },
                        { "003", "Header", "Consumer Involvement/User Generated", "" },
                        { "004", "Body", "Consumer Generated", "" },
                        { "005", "Body", "Viral", "" },
                        { "006", "Body", "WOM", "" },
                        { "007", "Header", "Direct", "" },
                        { "009", "Body", "Email", "" },
                        { "008", "Body", "Mail", "" },
                        { "010", "SingleHeader", "Ecommerce", "" },
                        { "011", "SingleHeader", "Events", "" },
                        { "012", "Header", "Guerrilla", "" },
                        { "013", "Body", "Ambient Media", "" },
                        { "014", "Body", "Buzz Marketing", "" },
                        { "015", "Body", "Sampling/Trial", "" },
                        { "016", "Body", "Street Teams", "" },
                        { "017", "Body", "Tagging", "" },
                        { "018", "Body", "Wraps", "" },
                        { "019", "Header", "Interactive/Online", "" },
                        { "020", "Body", "Brand Website/ Microsite", "" },
                        { "021", "Body", "Contests", "" },
                        { "022", "Body", "Digital Video", "" },
                        { "023", "Body", "Display Ads", "" },
                        { "024", "Body", "Gaming", "" },
                        { "025", "Body", "Geo-based Ads", "" },
                        { "026", "Body", "Mobile/Tablet Optimized Website", "" },
                        { "027", "Body", "Podcasts", "" },
                        { "028", "Body", "Video Skins/Bugs", "" },
                        { "029", "Body", "Other", "" },
                        { "030", "SingleHeader", "Internal Marketing", "" },
                        { "031", "Header", "Mobile/Tablet", "" },
                        { "032", "Body", "App", "" },
                        { "033", "Body", "Display Ad", "" },
                        { "034", "Body", "In-App or In-Game Ad", "" },
                        { "035", "Body", "Location-based Communications/Real Time Marketing", "" },
                        { "036", "Body", "Messaging/Editorial/Content", "" },
                        { "037", "Body", "Other", "" },
                        { "038", "Header", "OOH", "" },
                        { "039", "Body", "Airport", "" },
                        { "040", "Body", "Billboard", "" },
                        { "041", "Body", "Place-Based", "" },
                        { "042", "Body", "Transit", "" },
                        { "043", "Body", "Other", "" },
                        { "044", "SingleHeader", "Packaging", "" },
                        { "045", "Header", "Point of Care (POC)", "" },
                        { "046", "Body", "Brochures", "" },
                        { "047", "Body", "Coverwraps", "" },
                        { "048", "Body", "Electronic Check-In", "" },
                        { "049", "Body", "Video (HAN, Accent Health)", "" },
                        { "050", "Body", "Wallboards", "" },
                        { "051", "Body", "Other", "" },
                        { "052", "SingleHeader", "PR", "" },
                        { "053", "Header", "Print", "" },
                        { "054", "Body", "Custom Publication", "" },
                        { "055", "Body", "Magazine - Digital", "" },
                        { "056", "Body", "Magazine – Print", "" },
                        { "057", "Body", "Newspaper - Digital", "" },
                        { "058", "Body", "Newspaper – Print", "" },
                        { "059", "Body", "Trade/Professional", "" },
                        { "060", "SingleHeader", "Product Design", "" },
                        { "061", "Header", "Professional Engagement", "" },
                        { "062", "Body", "Closed Loop Marketing (CLM)", "" },
                        { "063", "Body", "Congresses", "" },
                        { "064", "Body", "Continuing Engagement", "" },
                        { "065", "Body", "Detail/E-Detail/Interactive Visual Aids (IVAs)", "" },
                        { "066", "Body", "Informational/ Documentary Video", "" },
                        { "067", "Body", "In-Office", "" },
                        { "068", "Header", "Radio", "" },
                        { "069", "Body", "merchandising", "" },
                        { "070", "Body", "Program/Content", "" },
                        { "071", "Body", "Spots", "" },
                        { "072", "Header", "Retail Experience", "" },
                        { "073", "Body", "In-Store Merchandising", "" },
                        { "074", "Body", "In-Store Video", "" },
                        { "075", "Body", "Pharmacy", "" },
                        { "076", "Body", "POP", "" },
                        { "077", "Body", "Retailtainment", "" },
                        { "078", "Body", "Store within a Store", "" },
                        { "079", "Body", "Other", "" },
                        { "080", "SingleHeader", "Sales Promotion", "" },
                        { "081", "SingleHeader", "Search Engine Marketing (SEM/SEO)", "" },
                        { "082", "SingleHeader", "Social Media", "" },
                        { "083", "SingleHeader", "Sponsorship", "" },
                        { "084", "SingleHeader", "Trade Shows", "" },
                        { "085", "Header", "TV", "" },
                        { "086", "Body", "Branded Content", "" },
                        { "087", "Body", "Interactive TV/ Video on Demand", "" },
                        { "088", "Body", "Product Placement", "" },
                        { "089", "Body", "Sponsorship", "" },
                        { "090", "Body", "Spots", "" }
                    };
    protected static string[,] CTPPCE = {
                        { "001", "SingleHeader", "Branded Content", "" },
                        { "002", "SingleHeader", "Cinema", "" },
                        { "003", "Header", "Consumer Involvement/User Generated", "" },
                        { "004", "Body", "Consumer Generated", "" },
                        { "005", "Body", "Viral", "" },
                        { "006", "Body", "WOM", "" },
                        { "007", "Header", "Direct", "" },
                        { "009", "Body", "Email", "" },
                        { "008", "Body", "Mail", "" },
                        { "010", "SingleHeader", "Ecommerce", "" },
                        { "011", "SingleHeader", "Events", "" },
                        { "012", "Header", "Guerrilla", "" },
                        { "013", "Body", "Ambient Media", "" },
                        { "014", "Body", "Buzz Marketing", "" },
                        { "015", "Body", "Sampling/Trial", "" },
                        { "016", "Body", "Street Teams", "" },
                        { "017", "Body", "Tagging", "" },
                        { "018", "Body", "Wraps", "" },
                        { "019", "Header", "Interactive/Online", "" },
                        { "020", "Body", "Brand Website/ Microsite", "" },
                        { "021", "Body", "Contests", "" },
                        { "022", "Body", "Digital Video", "" },
                        { "023", "Body", "Display Ads", "" },
                        { "024", "Body", "Gaming", "" },
                        { "025", "Body", "Geo-based Ads", "" },
                        { "026", "Body", "Mobile/Tablet Optimized Website", "" },
                        { "027", "Body", "Podcasts", "" },
                        { "028", "Body", "Video Skins/Bugs", "" },
                        { "029", "Body", "Other", "" },
                        { "030", "SingleHeader", "Internal Marketing", "" },
                        { "031", "Header", "Mobile/Tablet", "" },
                        { "032", "Body", "App", "" },
                        { "033", "Body", "Display Ad", "" },
                        { "034", "Body", "In-App or In-Game Ad", "" },
                        { "035", "Body", "Location-based Communications/Real Time Marketing", "" },
                        { "036", "Body", "Messaging/Editorial/Content", "" },
                        { "037", "Body", "Other", "" },
                        { "038", "Header", "OOH", "" },
                        { "039", "Body", "Airport", "" },
                        { "040", "Body", "Billboard", "" },
                        { "041", "Body", "Place-Based", "" },
                        { "042", "Body", "Transit", "" },
                        { "043", "Body", "Other", "" },
                        { "044", "SingleHeader", "Packaging", "" },
                        { "045", "Header", "Point of Care (POC)", "" },
                        { "046", "Body", "Brochures", "" },
                        { "047", "Body", "Coverwraps", "" },
                        { "048", "Body", "Electronic Check-In", "" },
                        { "049", "Body", "Video (HAN, Accent Health)", "" },
                        { "050", "Body", "Wallboards", "" },
                        { "051", "Body", "Other", "" },
                        { "052", "SingleHeader", "PR", "" },
                        { "053", "Header", "Print", "" },
                        { "054", "Body", "Custom Publication", "" },
                        { "055", "Body", "Magazine - Digital", "" },
                        { "056", "Body", "Magazine – Print", "" },
                        { "057", "Body", "Newspaper - Digital", "" },
                        { "058", "Body", "Newspaper – Print", "" },
                        { "059", "Body", "Trade/Professional", "" },
                        { "060", "SingleHeader", "Product Design", "" },
                        { "061", "Header", "Professional Engagement", "" },
                        { "062", "Body", "Closed Loop Marketing (CLM)", "" },
                        { "063", "Body", "Congresses", "" },
                        { "064", "Body", "Continuing Engagement", "" },
                        { "065", "Body", "Detail/E-Detail/Interactive Visual Aids (IVAs)", "" },
                        { "066", "Body", "Informational/ Documentary Video", "" },
                        { "067", "Body", "In-Office", "" },
                        { "068", "Header", "Radio", "" },
                        { "069", "Body", "merchandising", "" },
                        { "070", "Body", "Program/Content", "" },
                        { "071", "Body", "Spots", "" },
                        { "072", "Header", "Retail Experience", "" },
                        { "073", "Body", "In-Store Merchandising", "" },
                        { "074", "Body", "In-Store Video", "" },
                        { "075", "Body", "Pharmacy", "" },
                        { "076", "Body", "POP", "" },
                        { "077", "Body", "Retailtainment", "" },
                        { "078", "Body", "Store within a Store", "" },
                        { "079", "Body", "Other", "" },
                        { "080", "SingleHeader", "Sales Promotion", "" },
                        { "081", "SingleHeader", "Search Engine Marketing (SEM/SEO)", "" },
                        { "082", "SingleHeader", "Social Media", "" },
                        { "083", "SingleHeader", "Sponsorship", "" },
                        { "084", "SingleHeader", "Trade Shows", "" },
                        { "085", "Header", "TV", "" },
                        { "086", "Body", "Branded Content", "" },
                        { "087", "Body", "Interactive TV/ Video on Demand", "" },
                        { "088", "Body", "Product Placement", "" },
                        { "089", "Body", "Sponsorship", "" },
                        { "090", "Body", "Spots", "" }
                    };
    
    
}