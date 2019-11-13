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
public class ShopperMarketingPDF
{
    public static Entry entry = null;
    public static EntryForm entryForm = null;
    public static List<Data.CollectData> CTPList = new List<Data.CollectData>();
    public static List<Data.ExplainListOtherMarketingViewmodel> explainOtherMarketingList = new List<Data.ExplainListOtherMarketingViewmodel>();
    public static List<Data.CollectData> PaidMediaExpendituresList = new List<Data.CollectData>();
    public static string[] Delimiter = { "#&&##&&#", "&&&$$&&&", "************" };
    public static Guid EntryId = Guid.Empty;
    public static string CurrentCategory = Data.DocumentCategories.ShopperMarketing.ToString();
    public static string WebURL = System.Configuration.ConfigurationSettings.AppSettings["WebPhysicalPath"];

    public ShopperMarketingPDF()
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
        TypeImage.Add("txtExplainWorkedC");
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
        body = body.Replace("###EntryID###", entry.Serial);

        body = body.Replace("###DesTotalOfCountries###", entryForm.DesTotalOfCountries);
        body = body.Replace("###Product###", entry.ProductClassification);
        body = body.Replace("###BrandName###", entry.Brand);
        body = body.Replace("###ProductServiceClassification###", entry.Client);
        body = body.Replace("###EntryTitle###", entry.Campaign);

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

        if (entryForm.PaidMediaExpendituresCheck != "" && entryForm.PaidMediaExpendituresCheck != null)
        {
            string[] PME = entryForm.PaidMediaExpendituresCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (PME.Length != 0)
            {
                try
                {
                    body = body.Replace("###CurrentYear###", PME[0].Replace("  ", ""));
                    body = body.Replace("###YearPrior###", PME[1].Replace("  ", ""));

                    body = body.Replace("###PaidMediaExpendituresCurrent###", PME[2].Replace("  ", ""));
                    body = body.Replace("###PaidMediaExpendituresPrior###", PME[3].Replace("  ", ""));
                }
                catch
                {

                }
            }
        }

        body = body.Replace("###BringIdeaA###", GeneralFunctionEntryForm.CheckTextfield(entryForm.BringingIdea));
        body = body.Replace("###ComparedOtherCompetitorsCheck###", GetMoreLess(entryForm.ComparedOtherCompetitorsCheck));
        body = body.Replace("###ComparedOverallSpendCheck###", GetMoreLess(entryForm.ComparedOverallSpendCheck));


        body = body.Replace("###ExplainSelected###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Other));
        body = body.Replace("###Elaborate###", GeneralFunctionEntryForm.CheckTextfield(entryForm.PaidMediaExpendituresText));
        body = body.Replace("###OwnedMedia###", GeneralFunctionEntryForm.CheckTextfield(entryForm.OwnedMedia));
        body = body.Replace("###Sponsorship###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Sponsorship));

        if (entryForm.ExplainWorked != "" && entryForm.ExplainWorked != null)
        {
            string[] ExplainWorked = entryForm.ExplainWorked.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);
            if (ExplainWorked.Length != 0)
            {
                body = body.Replace("###HowWorkedA###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[0]));
                body = body.Replace("###HowWorkedB###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[1]));
                body = body.Replace("###HowWorkedC###", GeneralFunctionEntryForm.CheckTextfield(ExplainWorked[2]));
            }
        }

        body = body.Replace("###Anything###", GeneralFunctionEntryForm.CheckTextfield(entryForm.Anything));

        body = PopulateRowPTC(body);
        body = PopulateRowEOM(body);
        body = PopulateImages(body);

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
        List<Data.CollectData> CTPList = Data.GetCTPMulti();

        if (entryForm != null)
        {
            List<Data.CollectData> CTPListTemp = new List<Data.CollectData>();
            string[] communicationTouchPointsList = entryForm.CommunicationTouchPointsCheck.Split(new string[] { Delimiter[0] }, System.StringSplitOptions.RemoveEmptyEntries);

            if (communicationTouchPointsList.Count() != 0)
            {
                for (int i = 0; i < communicationTouchPointsList.Length; i++)
                {
                    string[] Datas = communicationTouchPointsList[i].Split(new string[] { Delimiter[1] }, System.StringSplitOptions.RemoveEmptyEntries);

                    string[] Data1 = Datas[0].Split(':');
                    string[] Data2 = Datas[1].Split(':');
                    string[] Data3 = Datas[2].Split(':');
                    string[] Data4 = Datas[3].Split(':');

                    if (Data1[0] == "998")
                    {
                        string[] Data5 = Datas[3].Split(':');
                        body = body.Replace("###OtherCTPD1###", Data1[1]);

                        body = body.Replace("###OtherCTPA1###", (Data2[1] == "True") ? check() : "");
                        body = body.Replace("###OtherCTPB1###", (Data3[1] == "True") ? check() : "");
                        body = body.Replace("###OtherCTPC1###", (Data4[1] == "True") ? check() : "");
                    }
                    else
                    {
                        try
                        {
                            Data.CollectData CT = CTPList.FirstOrDefault(x => x.id == Data1[0]);

                            CT.Data1 = Data1[1];
                            CT.Data2 = Data2[1];
                            CT.Data3 = Data3[1];
                            CT.Data4 = Data4[1];

                            if (CT.AttrType == "Header" || CT.AttrType == "SingleHeader")
                            {
                                body = body.Replace("###CTPTitle" + Data1[0] + "###", "<strong>" + CT.Title + "</strong>");
                            }
                            else
                            {
                                body = body.Replace("###CTPTitle" + Data1[0] + "###", "" + CT.Title);
                            }

                            body = body.Replace("###CTPDataA" + Data1[0] + "###", (CT.Data2 == "True") ? check() : "");
                            body = body.Replace("###CTPDataB" + Data1[0] + "###", (CT.Data3 == "True") ? check() : "");
                            body = body.Replace("###CTPDataC" + Data1[0] + "###", (CT.Data4 == "True") ? check() : "");

                            body = body.Replace("###CTPDataD" + Data1[0] + "###", (CT.Data1 == "True") ? check() : "");

                            CTPListTemp.Add(CT);
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
        explainOtherMarketingList = Data.GetListExplainPoitAllOthers();

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
                        body = body.Replace("###OtherMC1###", (Datas[1] == "True") ? check() : "");
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

                            body = body.Replace("###MCTitleA" + Datas[0] + "###", ELOM.MarketingComponents);
                            body = body.Replace("###MCDataA" + Datas[0] + "###", (Datas[1] == "True") ? check() : "");
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