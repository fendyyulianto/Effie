using System;
using System.Linq;
using Effie2017.App;
using System.Collections.Generic;


public class GeneralFunctionEffie2017App
{
    public GeneralFunctionEffie2017App()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static Guid GetAdminidAssignedto(Guid PayGroupId, Guid Id)
    {
        Guid AdminidAssignedto = Guid.Empty;
        try
        {
            AdhocInvoiceItem adhocInvoiceItem = AdhocInvoiceItemList.GetAdhocInvoiceItemList(PayGroupId, Id).FirstOrDefault();
            Entry entry = Entry.GetEntry(adhocInvoiceItem.EntryId);

            Administrator administrator = AdministratorList.GetAdministratorList().Where(x => x.Id == entry.AdminidAssignedto).FirstOrDefault();
            AdminidAssignedto = administrator.Id;
        }
        catch { }

        return AdminidAssignedto;
    }


    public static DateTime GetDateReminder(Guid ID, string type)
    {
        DateTime DateString = DateTime.MinValue;
        List<RegistrationEmailSent> registrationEmailSentList = RegistrationEmailSentList.GetRegistrationEmailSentList()
                                                                 .Where(x => x.EntryId == ID && x.EntryType == type)
                                                                 .OrderByDescending(y => y.DateCreated).ToList();

        if (registrationEmailSentList.Count() > 0)
        {
            RegistrationEmailSent registrationEmailSent = registrationEmailSentList.FirstOrDefault();
          
            if (!(registrationEmailSent.DateCreated == DateTime.MaxValue) && !(registrationEmailSent.DateCreated == DateTime.MinValue))
                DateString = registrationEmailSent.DateCreated;
        }

        return DateString;
    }


    public static SystemData GetSystemData()
    {
        SystemData data = SystemData.GetSystemData(new Guid("91ddb71e-f77e-4d7f-b728-49304737762d"));
        return data;
    }

    public static string GetPanelId(Guid EntryId, string categoryPSDetail, string round)
    {
        string PanelId = "";
        SystemData data = GetSystemData();
        if (string.IsNullOrWhiteSpace(round))
        {
            round = data.ActiveRound;
        }
        var jurypanel = JuryPanelCategoryList.GetJuryPanelCategoryList(string.Empty, string.Empty)
            .FirstOrDefault(x => x.CategoryPSDetail.Contains(categoryPSDetail) && x.Round == round);

        if (jurypanel != null) PanelId = jurypanel.PanelId;
        
        return PanelId;
    }

    public static string GetDateDepentent(Guid paymentGroupId, Guid EntryId, DateTime DateSubmitted, string Type = "")
    {
        string DateFormat = "dd MMMM yyyy";
        DateTime OnTimeCutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["OnTimeCutOff"].ToString());
        DateTime ExtendedCutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ExtendedCutOff"].ToString());
        DateTime Extended_2_CutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Extended_2_CutOff"].ToString());
        DateTime Extended_3_CutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Extended_3_CutOff"].ToString());

        if (paymentGroupId != Guid.Empty && paymentGroupId != null)
        {
            Entry entry = EntryList.GetEntryList(paymentGroupId, Guid.Empty, "").FirstOrDefault();
            DateSubmitted = entry.DateSubmitted;
        }

        string DateDependent = DateSubmitted.ToString(DateFormat);
        if (DateSubmitted == DateTime.MaxValue)
        {
            return "";
        }
        if (DateSubmitted < OnTimeCutOff)
        {
            if (Type == "D_String")
                DateDependent = "D1";
            else
                DateDependent = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["DueDate1"].ToString()).ToString(DateFormat);
        }
        else if (DateSubmitted < ExtendedCutOff)
        {
            if (Type == "D_String")
                DateDependent = "D2";
            else
                DateDependent = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["DueDate2"].ToString()).ToString(DateFormat);
        }
        else if (DateSubmitted < Extended_2_CutOff)
        {
            if (Type == "D_String")
                DateDependent = "D3";
            else
                DateDependent = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["DueDate3"].ToString()).ToString(DateFormat);
        }
        else if (DateSubmitted < Extended_3_CutOff)
        {
            if (Type == "D_String")
                DateDependent = "D4";
            else
                DateDependent = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["DueDate4"].ToString()).ToString(DateFormat);
        }
        else
        {
            if (Type == "D_String")
                DateDependent = "D4";
            else
                DateDependent = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["DueDate5"].ToString()).ToString(DateFormat);
        }
        return DateDependent;
    }
}
