using System;
using System.Web;
using Effie2017.App;
using System.Linq;

public partial class Testing_updatePP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GeneralFunction.CompleteNewEntrySubmissionPayPal(new Guid(Request.QueryString["ppId"]));

        //GeneralFunction.CompleteNewGalaOrderPayPal(new Guid(Request.QueryString["ppId"]));

        //lbError.Text = "OK";

        //foreach (Entry entry in EntryList.GetEntryList(Guid.Empty, Guid.Empty, ""))
        //{
        //    foreach (IndCredit ic in IndCreditList.GetIndCreditList(entry.Id))
        //    {
        //        Guid indCCId = Guid.Empty;
        //        Guid.TryParse(ic.UserData1, out indCCId);

        //        if (indCCId == Guid.Empty)
        //        {
        //            foreach (CompanyCredit cc in CompanyCreditList.GetCompanyCreditList(entry.Id))
        //            {
        //                if (ic.Company == cc.Company)
        //                {
        //                    ic.UserData1 = cc.Id.ToString();
        //                    ic.DateCreatedString = ic.DateModifiedString;
        //                    ic.Save();
        //                }
        //            }
        //        }
        //    }
        //}
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //string raw = Request.Form.ToString();
    //        //string discountCode = string.Empty;
    //        string raw = "mc_gross=1144.00&protection_eligibility=Eligible&address_status=confirmed&payer_id=CZ47M7SFTQ2XG&tax=0.00&address_street=1+Emlyn+Road&payment_date=01%3a37%3a09+Dec+14%2c+2016+PST&payment_status=Completed&charset=gb2312&address_zip=W12+9TF&first_name=Will&mc_fee=39.40&address_country_code=GB&address_name=Will+Philipps&notify_version=3.8&custom=fd901804-56e8-4573-b6d9-2e097d0c7eac&payer_status=verified&business=af%40ifektiv.com&address_country=United+Kingdom&address_city=London&quantity=1&verify_sign=A2D4DKJFlDsBtCK3KOnr2fCSH4sUARWyev57dyPowWHLO8oWiCU01L3S&payer_email=willphilipps%40btinternet.com&txn_id=83J71625FR2201601&payment_type=instant&last_name=Philipps&address_state=London&receiver_email=af%40ifektiv.com&payment_fee=&shipping_discount=0.00&insurance_amount=0.00&receiver_id=MU2A9FC6CKEYE&txn_type=web_accept&item_name=Payment+for+APAC+Effie+Entry&discount=0.00&mc_currency=SGD&item_number=FD901804&residence_country=GB&shipping_method=Default&handling_amount=0.00&transaction_subject=&payment_gross=&shipping=0.00&ipn_track_id=RFALS";

    //        RawLog rawlog = RawLog.NewRawLog();
    //        rawlog.Type = 1; //response
    //        rawlog.Data = raw;
    //        rawlog.DateString = DateTime.Now.ToString();

    //        if (rawlog.IsValid) rawlog.Save();

    //        TransactionLog Tlog = TransactionLog.NewTransactionLog();
    //        //Tlog.RegistrationID = Guid.NewGuid(); //WRONG

    //        string[] args = raw.Split('&');

    //        for (int i = 0; i < args.Length; i++)
    //        {
    //            string[] arg = args[i].Split('=');

    //            switch (arg[0])
    //            {
    //                case "mc_gross": Tlog.Gross = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "invoice": Tlog.TransactionID = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "protection_eligibility": Tlog.Protection = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "payer_id": Tlog.PayerID = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "tax": Tlog.Tax = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "payment_date": Tlog.PaymentDate = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "payment_status": Tlog.PaymentStatus = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "charset": break;
    //                case "first_name": Tlog.FirstName = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "mc_fee": Tlog.TransactionFee = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "ify_version": break;
    //                case "payer_status": break;
    //                case "business": Tlog.Business = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "quantity": break;
    //                case "payer_email": Tlog.PayerEmail = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "verify_sign": Tlog.VerifySign = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "txn_id": Tlog.PaypalID = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "payment_type": Tlog.PaymentType = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "last_name": Tlog.LastName = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "receiver_email": Tlog.ReceiverEmail = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "payment_fee": break;
    //                case "receiver_id": Tlog.ReceiverID = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "txn_type": break;
    //                case "item_name": Tlog.ItemName = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); Tlog.Subject = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "mc_currency": Tlog.Currency = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "item_number": break;
    //                case "residence_country": Tlog.Country = HttpUtility.HtmlDecode(arg[1]); break;
    //                case "handling_amount": Tlog.Handling = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "transaction_subject": break;
    //                case "payment_gross": break;
    //                case "shipping": Tlog.Shipping = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
    //                case "merchant_return_link": break;
    //                case "custom": Tlog.RegistrationID = new Guid(HttpUtility.HtmlDecode(arg[1])); break;
    //                //string[] custom = HttpUtility.HtmlDecode(arg[1]).Split(new string[] { "DCode" }, StringSplitOptions.None);
    //                //for (int y = 0; y < custom.Length; y++)
    //                //{
    //                //    if (y.Equals(0))
    //                //        Tlog.RegistrationID = new Guid(custom[y]);

    //                //    if (y.Equals(1))
    //                //        discountCode = custom[y];
    //                //}
    //                //break;
    //            }
    //        }

    //        if (Tlog.PaymentStatus.Contains("Completed"))
    //        {
    //            try
    //            {
    //                GeneralFunction.CompleteNewEntrySubmissionPayPal(Tlog.RegistrationID);
    //            }
    //            catch (Exception exp)
    //            {
    //                RawLog log = RawLog.NewRawLog();
    //                log.Type = 3; // Process payment error
    //                log.Data = "[" + exp.Message + "]";
    //                log.DateString = DateTime.Now.ToString();

    //                if (log.IsValid) log.Save();
    //            }
    //        }

    //        if (Tlog.PaymentStatus.Contains("Pending"))
    //        {
    //            // not so good
    //        }

    //        if (Tlog.PaymentStatus.Contains("Denied"))
    //        {
    //            // bad!
    //        }

    //        if (Tlog.IsValid)
    //            Tlog.Save();
    //        else
    //        {
    //            RawLog log = RawLog.NewRawLog();
    //            log.Type = 2; //error
    //            log.Data = Tlog.BrokenRulesCollection.ToString();
    //            log.DateString = DateTime.Now.ToString();

    //            if (log.IsValid) log.Save();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        RawLog log = RawLog.NewRawLog();
    //        log.Type = 4; // Overall payment error
    //        log.Data = "[" + ex.Message + "]";
    //        log.DateString = DateTime.Now.ToString();

    //        if (log.IsValid) log.Save();
    //    }
    //}
}