using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Effie2017.App;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

public partial class Main_EFMultiMarketPDF : System.Web.UI.Page
{
    public static Entry entry = null;
    public static EntryForm entryForm = null;
    public static List<Data.CollectData> CTPList = new List<Data.CollectData>();
    public static List<Data.CountriesList> explainOtherMarketingList = new List<Data.CountriesList>();
    public static List<Data.CountriesList> PaidMediaExpendituresListPart1 = new List<Data.CountriesList>();
    public static List<Data.CountriesList> PaidMediaExpendituresListPart2 = new List<Data.CountriesList>();
    public static string[] Delimiter = { "#&&##&&#", "&&&$$&&&", "************" };
    public static Guid EntryId = Guid.Empty;
    public static string CurrentCategory = Data.DocumentCategories.MultiMarket.ToString();
    public static string WebURL = System.Configuration.ConfigurationSettings.AppSettings["WebPhysicalPath"];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["id"] == null)
            Response.Redirect("./Dashboard.aspx");

        entryForm = null;

        try
        {
            //EntryId = new Guid(GeneralFunction.StringDecryption(Request.QueryString["Id"]));
            EntryId = new Guid(Request.QueryString["Id"]);

            if (Request["temp"] != null)
            {
                if (Session["Entry-" + EntryId.ToString() + Request["skey"]] == null)
                    return;

                entryForm = (EntryForm)Session["Entry-" + EntryId.ToString() + Request["skey"]];
                //HttpContext.Current.Session.Remove("Entry-" + EntryId.ToString() + Request["skey"]);
            }
            else
                entryForm = EntryForm.GetEntryForm(Guid.Empty, EntryId);

                entry = Entry.GetEntry(EntryId);
        }
        catch
        {
            Response.Redirect("./Dashboard.aspx");
        }
        
        MemoryStream memoryStream = new System.IO.MemoryStream();
        StringReader srdr = new StringReader(GetDocument());
        Document pdfDoc = new Document(PageSize.A4, 40, 40, 20, 35);
        HTMLWorker hparse = new HTMLWorker(pdfDoc);
        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);

        //Define the page number
        PageEventHelper pageEventHelper = new PageEventHelper();
        pdfWriter.PageEvent = pageEventHelper;
        // Define the page header
        pageEventHelper.Title = "";
        pageEventHelper.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
        pageEventHelper.HeaderLeft = "";
        pageEventHelper.HeaderRight = "";

        pdfDoc.Open();
        hparse.Parse(srdr);
        
        pdfWriter.CloseStream = false;
        pdfDoc.Close();
        pdfWriter.Close();
        
        Response.ContentType = "application/pdf";
        memoryStream.WriteTo(Response.OutputStream);
        memoryStream.Close();
    }
    
    public static string GetDocument()
    {
        string body = "";
        string RootTamplate = System.Configuration.ConfigurationSettings.AppSettings["EFTamplate"] + "\\";
        string Path = RootTamplate + "EF"+ CurrentCategory + ".html";
        if (File.Exists(Path))
        {
            body = GeneralFunction.ReadTxtFile(Path);
            body = PopulateRow(body);
        }
        
        if(string.IsNullOrEmpty(body))
            body = "<strong> File Not Found! </strong> ";

        return body;
    }
    
    public static string check(string align = "center")
    {
        return "<img src=\"" + WebURL + "images/check.png" + "\" height=\"13\" width=\"13\" align=\"" + align + "\">";
    }


    protected static string PopulateImages(string body)
    {
        List<string> TypeImage = new List<string>();
        TypeImage.Add("txtExecutiveSummary");
        TypeImage.Add("txtStrategicChallengeObjectivesA");
        TypeImage.Add("txtStrategicChallengeObjectivesB");
        TypeImage.Add("txtStrategicChallengeObjectivesC");
        TypeImage.Add("txtStrategicChallengeObjectivesD");
        TypeImage.Add("txtStrategicChallengeObjectivesE");
        TypeImage.Add("txtIdeasA");
        TypeImage.Add("txtIdeasB");
        TypeImage.Add("txtIdeasC");
        TypeImage.Add("txtIdeasD");
        TypeImage.Add("txtBringingIdea");
        TypeImage.Add("txtExplainWorkedA");
        TypeImage.Add("txtExplainWorkedB");
        TypeImage.Add("txtExplainWorkedC");
        TypeImage.Add("txtExplainWorkedD");
        TypeImage.Add("txtSection3");
        TypeImage.Add("txtAnything");
        TypeImage.Add("txtListAndExplainOtherMarketingText");
        TypeImage.Add("txtExplainListOtherMarketing");
        TypeImage.Add("txtBudgetElaboration");
        TypeImage.Add("txtOwnedMedia");
        TypeImage.Add("txtSponsorship");
        TypeImage.Add("txtExplainCriteria");
        TypeImage.Add("txtDescribeMarket");



        body = GeneralFunction.GenerateImagesPDF(entry, body, TypeImage);
        return body;
    }

    public static string GetMoreLess(string str)
    {
        if (str == "Less")
        {
            str = "Less";
        }
        else if (str == "same")
        {
            str = "About the same";
        }
        else if (str == "More")
        {
            str = "More";
        }
        else if (str == "Not")
        {
            str = "NA";
        }
        
        return str;
    }


    public static string PopulateRow(string body)
    {
        body = body.Replace("###Title###", "Entry Form");
        body = body.Replace("###Logo###", WebURL + "images/logo-apaceffie-high.png");
        body = body.Replace("###HeaderLogo###", WebURL + "images/Header-logo-pdf.png");
        body = body.Replace("###IconList###", WebURL + "images/icon-list.png");
        body = body.Replace("###DesTotalOfCountries###", entryForm.DesTotalOfCountries);
        
        body = body.Replace("###Product###", entry.ProductClassification);
        body = body.Replace("###BrandName###", entry.Brand);
        body = body.Replace("###ProductServiceClassification###", entry.Client);
        body = body.Replace("###EntryTitle###", entry.Campaign);
        
        body = body.Replace("###EntryID###", entry.Serial);
        body = body.Replace("###ExecutiveSummary###", GeneralFunctionEntryForm.CheckTextfield(entryForm.ExecutiveSummary) );
        body = body.Replace("###DescribeMarket###", GeneralFunctionEntryForm.CheckTextfield(entryForm.DescribeMarket));
        body = body.Replace("###ExplainListOtherMarketing###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Other));
        body = body.Replace("###PaidMediaExpendituresText###", GeneralFunctionEntryForm.CheckTextfield(entryForm.PaidMediaExpendituresText));

        body = body.Replace("###ExplainCriteria###", GeneralFunctionEntryForm.CheckTextfield(entryForm.ExplainCriteria));
        body = body.Replace("###BrandName###", entry.Campaign);
        body = body.Replace("###ProductServiceClassification###", entry.Client);
        body = body.Replace("###EntryTitle###", entry.Brand);

        body = body.Replace("###YouEnteringInto###", Data.GetCategoryMarket(entry.CategoryMarket));
        body = body.Replace("###EntryCategory###", entry.CategoryPSDetail);
        try
        {
            string countries = "";
            foreach (string country in entry.Effectiveness.Split('|'))
            {
                countries += country + ", ";
            }
            
            body = body.Replace("###CountryHeader###", countries.Substring(0, countries.Length - 4) + ".");

            body = body.Replace("###PMECountryA###", entry.Effectiveness.Split('|')[0].ToString());
            body = body.Replace("###PMECountryB###", entry.Effectiveness.Split('|')[1].ToString());
            body = body.Replace("###PMECountryC###", entry.Effectiveness.Split('|')[2].ToString());

            body = body.Replace("###MCCountryA###", entry.Effectiveness.Split('|')[0].ToString());
            body = body.Replace("###MCCountryB###", entry.Effectiveness.Split('|')[1].ToString());
            body = body.Replace("###MCCountryC###", entry.Effectiveness.Split('|')[2].ToString());

            body = body.Replace("###Country1###", entry.Effectiveness.Split('|')[0].ToString());
            body = body.Replace("###Country2###", entry.Effectiveness.Split('|')[1].ToString());
            body = body.Replace("###Country3###", entry.Effectiveness.Split('|')[2].ToString());

        }
        catch
        {
            body = body.Replace("###CountryHeader###", "No Country Selected.");
        }

        body = body.Replace("###StartDate###", entry.DateCampaignStart.ToString("dd MMM yyyy"));
        body = body.Replace("###EndDate###", entry.DateCampaignEnd.ToString("dd MMM yyyy"));

        body = body.Replace("###Elaborate###", GeneralFunctionEntryForm.CheckTextfield(entryForm.PaidMediaExpendituresText));
        body = body.Replace("###OwnedMedia###", GeneralFunctionEntryForm.CheckTextfield(entryForm.OwnedMedia));
        body = body.Replace("###Sponsorship###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Sponsorship));

        if (entryForm.BringingIdea != "" && entryForm.BringingIdea != null)
        {
            string[] BringingIdea = entryForm.BringingIdea.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (BringingIdea.Length != 0)
            {
                try {
                    body = body.Replace("###BringingIdeaA###", GeneralFunctionEntryForm.CheckTextfield(BringingIdea[0]));
                    body = body.Replace("###BringingIdeaB###", GeneralFunctionEntryForm.CheckTextfield(BringingIdea[1]));
                }
                catch { }
            }
        }
        
        body = body.Replace("###BudgetElaboration###", GeneralFunctionEntryForm.CheckTextfield(entryForm.PaidMediaExpendituresIndicate));

        if (entryForm.PaidMediaExpendituresAstimates != "" && entryForm.PaidMediaExpendituresAstimates != null)
        {
            string[] PaidMediaExpendituresAstimates = entryForm.PaidMediaExpendituresAstimates.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (PaidMediaExpendituresAstimates.Length != 0)
            {
                try {
                    body = body.Replace("###MediaBudgetA###", GeneralFunctionEntryForm.CheckTextfield(PaidMediaExpendituresAstimates[0]));
                    body = body.Replace("###MediaBudgetB###", GeneralFunctionEntryForm.CheckTextfield(PaidMediaExpendituresAstimates[1]));
                    body = body.Replace("###MediaBudgetC###", GeneralFunctionEntryForm.CheckTextfield(PaidMediaExpendituresAstimates[2]));
                }
                catch { }
            }
        }

        if (entryForm.PaidMediaExpendituresAveregeAnnual != "" && entryForm.PaidMediaExpendituresAveregeAnnual != null)
        {
            string[] PaidMediaExpendituresAveregeAnnual = entryForm.PaidMediaExpendituresAveregeAnnual.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (PaidMediaExpendituresAveregeAnnual.Length != 0)
            {
                try {
                    body = body.Replace("###AnualBadgeA###", string.IsNullOrEmpty(PaidMediaExpendituresAveregeAnnual[0]) ? "" : PaidMediaExpendituresAveregeAnnual[0]);
                    body = body.Replace("###AnualBadgeB###", string.IsNullOrEmpty(PaidMediaExpendituresAveregeAnnual[1]) ? "" : PaidMediaExpendituresAveregeAnnual[1]);
                    body = body.Replace("###AnualBadgeC###", string.IsNullOrEmpty(PaidMediaExpendituresAveregeAnnual[2]) ? "" : PaidMediaExpendituresAveregeAnnual[2]);
                }
                catch { }
            }
        }

        if (entryForm.PaidMediaExpendituresTotalBudget != "" && entryForm.PaidMediaExpendituresTotalBudget != null)
        {
            string[] PaidMediaExpendituresTotalBudget = entryForm.PaidMediaExpendituresTotalBudget.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (PaidMediaExpendituresTotalBudget.Length != 0)
            {
                try {
                    body = body.Replace("###ApproximateBudgetA###", GeneralFunctionEntryForm.CheckTextfield(PaidMediaExpendituresTotalBudget[0]));
                    body = body.Replace("###ApproximateBudgetB###", GeneralFunctionEntryForm.CheckTextfield(PaidMediaExpendituresTotalBudget[1]));
                    body = body.Replace("###ApproximateBudgetC###", GeneralFunctionEntryForm.CheckTextfield(PaidMediaExpendituresTotalBudget[2]));
                }
                catch { }
            }
        }
        
        try
        {
            string countries = "";
            foreach (string country in entry.CaseData.Split('|'))
            {
                countries += country + ", ";
            }

            body = body.Replace("###Totalnumber###", (entry.CaseData.Split('|').Count() - 1).ToString());
            body = body.Replace("###CountriesList###", countries.Substring(0, countries.Length - 4) + ".");
        }
        catch
        {
            body = body.Replace("###Totalnumber###", "0");
            body = body.Replace("###CountriesList###", "No Country Selected.");
        }

        if (entryForm.StrategicChallengeObjectives != "" && entryForm.StrategicChallengeObjectives != null)
        {
            string[] StrategicChallengeObjectivesList = entryForm.StrategicChallengeObjectives.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (StrategicChallengeObjectivesList.Length != 0)
            {
                try
                {
                    body = body.Replace("###StrategicChallengeObjectivesA###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[0]));
                    body = body.Replace("###StrategicChallengeObjectivesB###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[1]));
                    body = body.Replace("###StrategicChallengeObjectivesC###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[2]));
                    body = body.Replace("###StrategicChallengeObjectivesD###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[3]));
                }
                catch { }
            }
        }

        if (entryForm.Ideas != "" && entryForm.Ideas != null)
        {
            string[] IdeasList = entryForm.Ideas.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (IdeasList.Length != 0)
            {
                try
                {
                    body = body.Replace("###IdeasA###", GeneralFunctionEntryForm.CheckTextfield(IdeasList[0]));
                    body = body.Replace("###IdeasB###", GeneralFunctionEntryForm.CheckTextfield(IdeasList[1]));
                    body = body.Replace("###IdeasC###", GeneralFunctionEntryForm.CheckTextfield(IdeasList[2]));
                    body = body.Replace("###IdeasD###", GeneralFunctionEntryForm.CheckTextfield(IdeasList[3]));
                }
                catch { }
            }
        }


        if (entryForm.ComparedOtherCompetitorsCheck != "" && entryForm.ComparedOtherCompetitorsCheck != null)
        {
            string[] ComparedOtherCompetitorsCheck = entryForm.ComparedOtherCompetitorsCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ComparedOtherCompetitorsCheck.Length != 0)
            {
                try
                {
                    body = body.Replace("###CompareATitleA###", ((ComparedOtherCompetitorsCheck[0].Replace(" ", "") == "True") ? check() : "" + " "));
                    body = body.Replace("###CompareATitleB###", ((ComparedOtherCompetitorsCheck[1].Replace(" ", "") == "True") ? check() : "" + " "));
                    body = body.Replace("###CompareATitleC###", ((ComparedOtherCompetitorsCheck[2].Replace(" ", "") == "True") ? check("center") : "" + " "));
                    body = body.Replace("###CompareATitleD###", ((ComparedOtherCompetitorsCheck[3].Replace(" ", "") == "True") ? check("center") : "" + " "));
                }
                catch { }

            }
        }

        if (entryForm.ComparedOverallSpendCheck != "" && entryForm.ComparedOverallSpendCheck != null)
        {
            string[] ComparedOverallSpendCheck = entryForm.ComparedOverallSpendCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ComparedOverallSpendCheck.Length != 0)
            {
                try {
                    body = body.Replace("###CompareBTitleA###", ((ComparedOverallSpendCheck[0].Replace(" ", "") == "True") ? check("center") : "" + " "));
                    body = body.Replace("###CompareBTitleB###", ((ComparedOverallSpendCheck[1].Replace(" ", "") == "True") ? check("center") : "" + " "));
                    body = body.Replace("###CompareBTitleC###", ((ComparedOverallSpendCheck[2].Replace(" ", "") == "True") ? check("center") : "" + " "));
                    body = body.Replace("###CompareBTitleD###", ((ComparedOverallSpendCheck[3].Replace(" ", "") == "True") ? check("center") : "" + " "));
                }
                catch { }
            }
        }
        
        if (entryForm.ExplainWorked != "" && entryForm.ExplainWorked != null)
        {
            string[] ExplainWorked = entryForm.ExplainWorked.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ExplainWorked.Length != 0)
            {
                try
                {
                    body = body.Replace("###HowWorkedA###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[0]));
                    body = body.Replace("###HowWorkedB###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[1]));
                    body = body.Replace("###HowWorkedC###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[2]));
                    body = body.Replace("###HowWorkedD###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[3]));
                }
                catch { }
            }
        }

        body = body.Replace("###Anything###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Anything));

        body = PopulateRowPTC(body);
        body = PopulateRowEOM(body);
        body = PopulateRowPME(body);
        body = PopulateImages(body);

        return body;
    }

    public static string PopulateRowPME(string body)
    {
        for (int x = 0; x <= Data.PaidMediaExpendituresPart1.GetUpperBound(0); x++)
        {
            Data.CountriesList PME = new Data.CountriesList();
            PME.id = Data.PaidMediaExpendituresPart1[x, 0];
            PME.Title = Data.PaidMediaExpendituresPart1[x, 1];
            PME.Country1 = null;
            PME.Country2 = null;
            PME.Country3 = null;
            PaidMediaExpendituresListPart1.Add(PME);
        }

        for (int x = 0; x <= Data.PaidMediaExpendituresPart2.GetUpperBound(0); x++)
        {
            Data.CountriesList PME = new Data.CountriesList();
            PME.id = Data.PaidMediaExpendituresPart2[x, 0];
            PME.Title = Data.PaidMediaExpendituresPart2[x, 1];
            PME.Country1 = null;
            PME.Country2 = null;
            PME.Country3 = null;
            PaidMediaExpendituresListPart2.Add(PME);
        }

        if (entryForm != null)
        {
            string[] PMECPart1 = { };
            string[] PMECPart2 = { };
            string[] PMECPart = entryForm.PaidMediaExpendituresCheck.Split(new string[] { Delimiter[2] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (PMECPart.Count() != 0)
            {
                if (PMECPart[0].Count() != 0) PMECPart1 = PMECPart[0].Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
                if (PMECPart[1].Count() != 0) PMECPart2 = PMECPart[1].Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            }

            List<Data.CountriesList> PaidMediaExpendituresListTempPart1 = new List<Data.CountriesList>();
            List<Data.CountriesList> PaidMediaExpendituresListTempPart2 = new List<Data.CountriesList>();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            if (PMECPart1.Count() != 0)
            {
                for (int i = 0; i < PMECPart1.Length; i++)
                {
                    string[] Datas = PMECPart1[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);

                    string[] count1 = Datas[0].Split(':');
                    string[] count2 = Datas[1].Split(':');
                    string[] count3 = Datas[2].Split(':');

                    if (count1[0] == "000")
                    {
                        // body = body.Replace("###PMECountryA###", count1[1]);
                        // body = body.Replace("###PMECountryB###", count2[1]);
                        // body = body.Replace("###PMECountryC###", count3[1]);
                    }
                    else
                    {
                        try
                        {
                            Data.CountriesList PME = PaidMediaExpendituresListPart1.FirstOrDefault(x => x.id == count1[0]);

                            

                            PME.Country1 = Datas[0].Split(':')[1];
                            PME.Country2 = Datas[1].Split(':')[1];
                            PME.Country3 = Datas[2].Split(':')[1];

                            body = body.Replace("###PMETitleAPart1" + count1[0] + "###", PME.Title);

                            body = body.Replace("###PMECountryAPart1" + count1[0] + "###", PME.Country1);
                            body = body.Replace("###PMECountryBPart1" + count1[0] + "###", PME.Country2);
                            body = body.Replace("###PMECountryCPart1" + count1[0] + "###", PME.Country3);

                            PaidMediaExpendituresListTempPart1.Add(PME);
                        }
                        catch
                        {
                            //TODO
                        }
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            if (PMECPart2.Count() != 0)
            {
                for (int i = 0; i < PMECPart2.Length; i++)
                {
                    string[] Datas = PMECPart2[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);

                    string[] count1 = Datas[0].Split(':');
                    string[] count2 = Datas[1].Split(':');
                    string[] count3 = Datas[2].Split(':');

                    if (count1[0] == "000")
                    {
                        //body = body.Replace("###PMECountryPart2A###", count1[1]);
                        //body = body.Replace("###PMECountryPart2B###", count2[1]);
                        //body = body.Replace("###PMECountryPart2C###", count3[1]);
                    }
                    else
                    {
                        try
                        {
                            Data.CountriesList PME = PaidMediaExpendituresListPart2.FirstOrDefault(x => x.id == count1[0]);

                            PME.Country1 = Datas[0].Split(':')[1];
                            PME.Country2 = Datas[1].Split(':')[1];
                            PME.Country3 = Datas[2].Split(':')[1];
                            
                            body = body.Replace("###PMETitleAPart2" + count1[0] + "###", PME.Title);

                            body = body.Replace("###PMECountryAPart2" + count1[0] + "###", GetMoreLess(PME.Country1));
                            body = body.Replace("###PMECountryBPart2" + count1[0] + "###", GetMoreLess(PME.Country2));
                            body = body.Replace("###PMECountryCPart2" + count1[0] + "###", GetMoreLess(PME.Country3));
                        }
                        catch
                        {
                            //TODO
                        }
                    }
                }
            }
        }
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        

        return body;
    }


    public static string PopulateRowPTC(string body)
    {
        CTPList = Data.GetCommunicationTouchPointCountryMulti();

        if (entryForm != null)
        {
            List<Data.CountriesList> CTPListTemp = new List<Data.CountriesList>();
            string[] communicationTouchPointsList = entryForm.CommunicationTouchPointsCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);

            if (communicationTouchPointsList.Count() != 0)
            {
                for (int i = 0; i < communicationTouchPointsList.Length; i++)
                {
                    string[] Datas = communicationTouchPointsList[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);

                    string[] count1 = Datas[0].Split(':');
                    if (count1[0] == "998")
                    {
                        body = body.Replace("###OtherDataCTP2###", (count1[1] == "True") ? check() : "");
                    }
                    if (count1[0] == "999")
                    {
                        body = body.Replace("###OtherDataCTP1###", count1[1]);
                    }

                    else
                    {
                        try
                        {
                            Data.CollectData CT = CTPList.FirstOrDefault(x => x.id == count1[0]);

                            CT.Data1 = Datas[0].Split(':')[1];
                            CT.Data2 = Datas[1].Split(':')[1];
                            CT.Data3 = Datas[2].Split(':')[1];
                            
                            if (CT.AttrType == "Header" || CT.AttrType == "SingleHeader")
                            {
                                body = body.Replace("###CTPTitle" + count1[0] + "###", "<strong>" + CT.Title + "</strong>");
                            }
                            else
                            {
                                body = body.Replace("###CTPTitle" + count1[0] + "###", "- " + CT.Title);
                            }
                            
                            body = body.Replace("###CTPDataA" + count1[0] + "###", (CT.Data1 == "True") ? check() : "");
                            body = body.Replace("###CTPDataB" + count1[0] + "###", (CT.Data2 == "True") ? check() : "");
                            body = body.Replace("###CTPDataC" + count1[0] + "###", (CT.Data3 == "True") ? check() : "");

                        }
                        catch
                        {
                            //TODO
                        }
                    }
                }
            }
        }

        return body;
    }

    public static string PopulateRowEOM(string body)
    {
        for (int x = 0; x <= Data.ExplainListOtherMarketingList.GetUpperBound(0); x++)
        {
            Data.CountriesList ELOM = new Data.CountriesList();
            ELOM.id = Data.ExplainListOtherMarketingList[x, 0];
            ELOM.Title = Data.ExplainListOtherMarketingList[x, 1];
            ELOM.Country1 = null;
            ELOM.Country2 = null;
            ELOM.Country3 = null;
            explainOtherMarketingList.Add(ELOM);
        }


        if (entryForm != null)
        {
            string[] ELOMG = entryForm.ExplainListOtherMarketing.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            List<Data.CountriesList> explainOtherMarketingListTemp = new List<Data.CountriesList>();

            if (ELOMG.Count() != 0)
            {
                for (int i = 0; i < ELOMG.Length; i++)
                {
                    string[] Datas = ELOMG[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);

                    string[] count1 = Datas[0].Split(':');
                    string[] count2 = Datas[1].Split(':');
                    string[] count3 = Datas[2].Split(':');

                    if (count1[0] == "000")
                    {
                        // body = body.Replace("###MCCountryA###", count1[1]);
                        // body = body.Replace("###MCCountryB###", count2[1]);
                        // body = body.Replace("###MCCountryC###", count3[1]);

                    }
                    else if (count1[0] == "998")
                    {
                        body = body.Replace("###OtherMCA###", count1[1]);
                        body = body.Replace("###OtherMCB###", (count2[1] == "True") ? check() : "");
                        body = body.Replace("###OtherMCC###", (count3[1] == "True") ? check() : "");
                        if (Datas[3].Split(':')[0] == "999")
                        {
                            body = body.Replace("###OtherMCD###", (Datas[3].Split(':')[1] == "True") ? check() : "");
                        }
                    }
                    else
                    {
                        try
                        {
                            Data.CountriesList ELOM = explainOtherMarketingList.FirstOrDefault(x => x.id == count1[0]);

                            ELOM.Country1 = Datas[0].Split(':')[1];
                            ELOM.Country2 = Datas[1].Split(':')[1];
                            ELOM.Country3 = Datas[2].Split(':')[1];

                            body = body.Replace("###MCTitle" + count1[0] + "###", ELOM.Title);
                            body = body.Replace("###MCDataA" + count1[0] + "###", (ELOM.Country1 == "True") ? check() : "");
                            body = body.Replace("###MCDataB" + count1[0] + "###", (ELOM.Country2 == "True") ? check() : "");
                            body = body.Replace("###MCDataC" + count1[0] + "###", (ELOM.Country3 == "True") ? check() : "");

                            explainOtherMarketingListTemp.Add(ELOM);
                        }
                        catch
                        {
                            //TODO
                        }
                    }
                }
            }
        }

        return body;
    }

}