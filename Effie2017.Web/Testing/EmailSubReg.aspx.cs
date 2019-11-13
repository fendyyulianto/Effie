using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_EmailSubReg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Guid PayGroupId = new Guid("ff8488d6-9b30-44cd-be47-3ac3bf80b917");
        Guid PayGroupId = new Guid("ff8488d6-9b30-44cd-be47-3ac3bf80b917");
        var list = EntryList.GetEntryList(PayGroupId, Guid.Empty, "").ToList();
        foreach (var entry in list)
        {
            EntryPayment entryPayment = null;
            entryPayment = EntryPaymentList.GetEntryPaymentList().FirstOrDefault(x => x.EntryId == entry.Id);
            if (entryPayment == null)
            {
                entryPayment = EntryPayment.NewEntryPayment();
                entryPayment.EntryId = entry.Id;
            }
            
            entryPayment.PayCompany = entry.PayCompany;
            entryPayment.PayAddress1 = entry.PayAddress1;
            entryPayment.PayAddress2 = entry.PayAddress2;
            entryPayment.PayCity = entry.PayCity;
            entryPayment.PayPostal = entry.PayPostal;
            entryPayment.PayCountry = entry.PayCountry;
            entryPayment.PayFirstname = entry.PayFirstname;
            entryPayment.PayLastname = entry.PayLastname;
            entryPayment.PayContact = entry.PayContact;
            entryPayment.PaymentMethod = entry.PaymentMethod;
            entryPayment.PayGroupId = entry.PayGroupId;
            entryPayment.Amount = entry.Amount;
            entryPayment.Fee = entry.Fee;
            entryPayment.Tax = entry.Tax;
            entryPayment.GrandAmount = entry.GrandAmount;
            entryPayment.AmountReceived = entry.AmountReceived;
            entryPayment.Invoice = entry.Invoice;
            entryPayment.Save();
        }

        //Registration reg = Registration.GetRegistration(new Guid("a48596b5-513e-4a1a-aa19-24160144db5b"));
        GeneralFunction.CompleteNewEntrySubmissionPayPal(PayGroupId);//ff8488d6-9b30-44cd-be47-3ac3bf80b917 
    }
}