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

public class SustainedSuccessPDF
{
    public static Entry entry = null;
    public static EntryForm entryForm = null;
    public static Guid EntryId = Guid.Empty;
    public static List<Data.CollectData> CTPList = new List<Data.CollectData>();
    public static List<Data.ExplainListOtherMarketingViewmodel> explainOtherMarketingList = new List<Data.ExplainListOtherMarketingViewmodel>();
    public static string[] Delimiter = { "#&&##&&#", "&&&$$&&&", "************" };
    public static string CurrentCategory = Data.DocumentCategories.SustainedSuccess.ToString();
    public static string WebURL = System.Configuration.ConfigurationSettings.AppSettings["WebPhysicalPath"];

    public SustainedSuccessPDF()
    {

    }

    public static MemoryStream memStream(EntryForm entf, Entry ent)
    {
        entryForm = entf;
        entry = ent;

        MemoryStream memoryStream = new System.IO.MemoryStream();
        memoryStream = GeneralFunctionEntryForm.memStream(GetDocument());

        return memoryStream;
    }


    public static string GetDocument()
    {
        string body = "";
        string RootTamplate = System.Configuration.ConfigurationSettings.AppSettings["EFTamplate"] + "\\";
        string Path = RootTamplate + "EF" + CurrentCategory + ".html";
        if (File.Exists(Path))
        {
            body = GeneralFunction.ReadTxtFile(Path);
            body = PopulateRow(body);
        }

        if (string.IsNullOrEmpty(body))
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
        TypeImage.Add("txtAnything");
        TypeImage.Add("txtBringingIdea");
        TypeImage.Add("txtExplainWorkedA");
        TypeImage.Add("txtExplainWorkedB");
        TypeImage.Add("txtListAndExplainOtherMarketingText");
        TypeImage.Add("txtExplainListOtherMarketing");
        TypeImage.Add("txtOwnedMedia");
        TypeImage.Add("txtSponsorship");

        body = GeneralFunction.GenerateImagesPDF(entry, body, TypeImage);
        return body;
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

        /////////////////////////////////
        body = body.Replace("###ComparedOtherCompetitorsCheck###", GetMoreLess(entryForm.ComparedOtherCompetitorsCheck));
        body = body.Replace("###ComparedOverallSpendCheck###", GetMoreLess(entryForm.ComparedOverallSpendCheck));
        /////////////////////////////////

        body = body.Replace("###SectionD###", GeneralFunctionEntryForm.CheckTextfield(entryForm.ListAndExplainOtherMarketingText));
        body = body.Replace("###ListAndExplainOtherMarketingText###", GeneralFunctionEntryForm.CheckTextfield(entryForm.ListAndExplainOtherMarketingText));

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


        //if (entryForm.PaidMediaExpendituresCheck != "" && entryForm.PaidMediaExpendituresCheck != null)
        //{
        //    string[] PME = entryForm.PaidMediaExpendituresCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
        //    if (PME.Length != 0)
        //    {
        //        try
        //        {
        //            body = body.Replace("###CurrentYear###", PME[0].Replace("  ", ""));
        //            body = body.Replace("###YearPrior###", PME[1].Replace("  ", ""));
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}

        if (entryForm.ComparedOverallSpendCheck != "" && entryForm.ComparedOverallSpendCheck != null)
        {
            string[] ComparedOverallSpendCheck = entryForm.ComparedOverallSpendCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ComparedOverallSpendCheck.Length != 0)
            {
                try
                {
                    body = body.Replace("###CompareBTitleA###", ((ComparedOverallSpendCheck[0].Replace(" ", "") == "True") ? check("center") : "" + " "));
                    body = body.Replace("###CompareBTitleB###", ((ComparedOverallSpendCheck[1].Replace(" ", "") == "True") ? check("center") : "" + " "));
                    body = body.Replace("###CompareBTitleC###", ((ComparedOverallSpendCheck[2].Replace(" ", "") == "True") ? check("center") : "" + " "));
                    body = body.Replace("###CompareBTitleD###", ((ComparedOverallSpendCheck[3].Replace(" ", "") == "True") ? check("center") : "" + " "));
                }
                catch { }
            }
        }

        body = body.Replace("###EntryTitle###", entry.Brand);
        body = body.Replace("###EntryID###", entry.Serial);

        body = body.Replace("###YouEnteringInto###", Data.GetCategoryMarket(entry.CategoryMarket));
        body = body.Replace("###EntryCategory###", entry.CategoryPSDetail);
        body = body.Replace("###CountryHeader###", entry.Effectiveness);
        body = body.Replace("###StartDate###", entry.DateCampaignStart.ToString("dd MMM yyyy"));
        body = body.Replace("###EndDate###", entry.DateCampaignEnd.ToString("dd MMM yyyy"));

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

        body = body.Replace("###ExecutiveSummary###", GeneralFunctionEntryForm.CheckTextfield(entryForm.ExecutiveSummary));
        body = body.Replace("###DescribeMarketBackground###", GeneralFunctionEntryForm.CheckTextfield(entryForm.DescribeMarket));



        if (entryForm.StrategicChallengeObjectives != "" && entryForm.StrategicChallengeObjectives != null)
        {
            string[] StrategicChallengeObjectivesList = entryForm.StrategicChallengeObjectives.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (StrategicChallengeObjectivesList.Length != 0)
            {
                try
                {
                    body = body.Replace("###MarketCategoryA###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[0]));
                }
                catch
                {
                    body = body.Replace("###MarketCategoryA###", "");
                }

                try
                {
                    body = body.Replace("###MarketCategoryB###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[1]));
                }
                catch
                {
                    body = body.Replace("###MarketCategoryB###", "");
                }

                try
                {
                    body = body.Replace("###MarketCategoryC###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[2]));
                }
                catch
                {
                    body = body.Replace("###MarketCategoryC###", "");
                }

                try
                {
                    body = body.Replace("###MarketCategoryD###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[3]));
                }
                catch
                {
                    body = body.Replace("###MarketCategoryD###", "");
                }

                try
                {
                    body = body.Replace("###MarketCategoryE###", GeneralFunctionEntryForm.CheckTextfield(StrategicChallengeObjectivesList[4]));
                }
                catch
                {
                    body = body.Replace("###MarketCategoryE###", "");
                }
            }
        }

        if (entryForm.Ideas != "" && entryForm.Ideas != null)
        {
            string[] IdeasList = entryForm.Ideas.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (IdeasList.Length != 0)
            {
                try
                {
                    body = body.Replace("###IdeaA###", GeneralFunctionEntryForm.CheckTextfield(IdeasList[0]));
                    body = body.Replace("###IdeaB###", GeneralFunctionEntryForm.CheckTextfield(IdeasList[1]));
                    body = body.Replace("###IdeaC###", GeneralFunctionEntryForm.CheckTextfield(IdeasList[2]));
                }
                catch { }
            }
        }

        body = body.Replace("###BringIdeaA###", GeneralFunctionEntryForm.CheckTextfield(entryForm.BringingIdea));


        body = body.Replace("###ExplainSelected###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Other));
        body = body.Replace("###Elaborate###", GeneralFunctionEntryForm.CheckTextfield(entryForm.PaidMediaExpendituresText));
        body = body.Replace("###OwnedMedia###", GeneralFunctionEntryForm.CheckTextfield(entryForm.OwnedMedia));
        body = body.Replace("###Sponsorship###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Sponsorship));

        if (entryForm.ExplainWorked != "" && entryForm.ExplainWorked != null)
        {
            string[] ExplainWorked = entryForm.ExplainWorked.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ExplainWorked.Length != 0)
            {
                try
                {
                    body = body.Replace("###HowWorkedA###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[0]));
                    body = body.Replace("###HowWorkedB###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[1]));
                }
                catch { }
            }
        }

        body = body.Replace("###Anything###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Anything));

        body = PopulateRowPTC(body);
        body = PopulateRowEOM(body);
        body = PopulateImages(body);
        body = PopulateRowPME(body);

        return body;
    }

    public static string PopulateRowPME(string body)
    {

        if (entryForm.PaidMediaExpendituresCheck != "" && entryForm.PaidMediaExpendituresCheck != null)
        {
            string[] PME = entryForm.PaidMediaExpendituresCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (PME.Length != 0)
            {
                try
                {
                    body = body.Replace("###ddlInitialYearPME###", PME[0].Replace("  ", ""));
                    body = body.Replace("###ddlInterimYearPME###", PME[1].Replace("  ", ""));
                    body = body.Replace("###ddlCurrentYearPME###", PME[2].Replace("  ", ""));

                    body = body.Replace("###txtInitialYearPCP###", PME[3].Replace("  ", ""));
                    body = body.Replace("###txtInterimYearPCP###", PME[4].Replace("  ", ""));
                    body = body.Replace("###txtCurrentYearPCP###", PME[5].Replace("  ", ""));

                    //body = body.Replace("###rblComparedInitial###", GetMoreLess(PME[6].Replace("  ", "")));
                    //body = body.Replace("###rblComparedInterim###", GetMoreLess(PME[7].Replace("  ", "")));
                    body = body.Replace("###rblComparedCurrent###", GetMoreLess(PME[6].Replace("  ", "")));


                    //body = body.Replace("###rblComparedoverallInitial###", GetMoreLess(PME[9].Replace("  ", "")));
                    //body = body.Replace("###rblComparedoverallInterim###", GetMoreLess(PME[10].Replace("  ", "")));
                    body = body.Replace("###rblComparedoverallCurrent###", GetMoreLess(PME[7].Replace("  ", "")));

                    body = body.Replace("###DdlInitialYearPaid###", PME[8].Replace("  ", ""));
                    body = body.Replace("###DdlInterimYearPaid###", PME[9].Replace("  ", ""));
                    body = body.Replace("###DdlCurrentYearPaid###", PME[10].Replace("  ", ""));
                }
                catch
                {

                }
            }
        }

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
        else
        {
            str = "";
        }

        return str;
    }
    public static string PopulateRowPTC(string body)
    {
        CTPList = Data.GetCommunicationTouchPointData();

        if (entryForm != null)
        {
            string[] CTPInitialdb = entryForm.CommunicationTouchPointsInitialYear.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] CTPCurrentdb = entryForm.CommunicationTouchPointsCurrentYear.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] CTPInterimdb = entryForm.CommunicationTouchPointsInterimYear.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);

            if (CTPInitialdb.Count() != 0)
            {
                for (int i = 0; i <= CTPCurrentdb.Length; i++)
                {
                    if (CTPCurrentdb[i].Split(':')[0] == "999")
                    {
                        body = body.Replace("###OtherCTP4###", CTPCurrentdb[i].Split(':')[1]);
                        break;
                    }

                    string[] Initial = CTPInitialdb[i].Split(':');
                    string[] Interim = CTPInterimdb[i].Split(':');
                    string[] Current = CTPCurrentdb[i].Split(':');

                    if (Initial[0] == "000")
                    {
                        body = body.Replace("###InitialYear###", Initial[1]);
                        body = body.Replace("###InterimYear###", Interim[1]);
                        body = body.Replace("###CurrentYear###", Current[1]);
                    }
                    else if (Initial[0] == "998")
                    {
                        body = body.Replace("###OtherCTP1###", Initial[1]);
                        body = body.Replace("###OtherCTP1###", "");
                        body = body.Replace("###OtherCTP2###", Interim[1]);
                        body = body.Replace("###OtherCTP3###", Current[1]);

                    }
                    else
                    {
                        try
                        {
                            Data.CollectData CT = CTPList.FirstOrDefault(x => x.id == Initial[0]);
                            CT.Data1 = Initial[1];
                            CT.Data2 = Interim[1];
                            CT.Data3 = Current[1];

                            body = body.Replace("###CTPTitle" + Initial[0] + "###", CT.Title);
                            body = body.Replace("###CTPDataA" + Initial[0] + "###", CT.Data1);
                            body = body.Replace("###CTPDataB" + Initial[0] + "###", CT.Data2);
                            body = body.Replace("###CTPDataC" + Initial[0] + "###", CT.Data3);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        return body;
    }

    public static string PopulateRowEOM(string body)
    {
        for (int x = 0; x <= Data.ExplainListOtherMarketingData.GetUpperBound(0); x++)
        {
            Data.ExplainListOtherMarketingViewmodel ELOM = new Data.ExplainListOtherMarketingViewmodel();
            ELOM.id = Data.ExplainListOtherMarketingData[x, 0];
            ELOM.MarketingComponents = Data.ExplainListOtherMarketingData[x, 1];
            ELOM.TimePeriod = null;
            explainOtherMarketingList.Add(ELOM);
        }

        if (entryForm != null)
        {
            string[] ELOMG = entryForm.ExplainListOtherMarketing.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);

            if (ELOMG.Count() != 0)
            {
                for (int i = 0; i < ELOMG.Length; i++)
                {
                    string[] Datas = ELOMG[i].Split(':');

                    if (Datas[0] == "998")
                    {
                        body = body.Replace("###OtherMC1###", Datas[1]);
                    }
                    else if (Datas[0] == "999")
                    {
                        body = body.Replace("###OtherMC2###", Datas[1]);
                    }
                    else
                    {
                        try
                        {
                            Data.ExplainListOtherMarketingViewmodel ELOM = explainOtherMarketingList.FirstOrDefault(x => x.id == Datas[0]);
                            ELOM.TimePeriod = Datas[1];

                            body = body.Replace("###MCTitle" + Datas[0] + "###", ELOM.MarketingComponents);
                            body = body.Replace("###MCDataA" + Datas[0] + "###", Datas[1]);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        return body;
    }
}