using System;
using System.Web;
using Effie2017.App;

public partial class CallBack_NotifyAdhoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string raw = Request.Form.ToString();
            //string discountCode = string.Empty;
            //string raw = "mc_gross=0.01&protection_eligibility=Ineligible&address_status=unconfirmed&payer_id=75RQAEDZYCD6A&tax=0.00&address_street=test&payment_date=22%3a18%3a17+Oct+22%2c+2015+PDT&payment_status=Completed&charset=gb2312&address_zip=34&first_name=shanice&mc_fee=0.01&address_country_code=SG&address_name=soh+shanice&notify_version=3.8&custom=d693b35c-3db3-4e99-8141-c1626e719165&payer_status=unverified&business=af%40ifektiv.com&address_country=Singapore&address_city=&quantity=1&verify_sign=AMIxIb-fq-bQLc45DZlYXp5uIWZJAzSFbammHFZ7khJL2HUKacwcLPGQ&payer_email=shanice%40ifektiv.com&txn_id=15C87088M38848415&payment_type=instant&last_name=soh&address_state=&receiver_email=af%40ifektiv.com&payment_fee=&receiver_id=MU2A9FC6CKEYE&txn_type=web_accept&item_name=Payment+for+APAC+Effie+Entry&mc_currency=SGD&item_number=D693B35C&residence_country=SG&receipt_id=3610-4808-7977-6573&handling_amount=0.00&transaction_subject=d693b35c-3db3-4e99-8141-c1626e719165&payment_gross=&shipping=0.00&ipn_track_id=721492c2a4faa";

            RawLog rawlog = RawLog.NewRawLog();
            rawlog.Type = 1; //response
            rawlog.Data = raw;
            rawlog.DateString = DateTime.Now.ToString();

            if (rawlog.IsValid) rawlog.Save();

            TransactionLog Tlog = TransactionLog.NewTransactionLog();
            //Tlog.RegistrationID = Guid.NewGuid(); //WRONG

            string[] args = raw.Split('&');

            for (int i = 0; i < args.Length; i++)
            {
                string[] arg = args[i].Split('=');

                switch (arg[0])
                {
                    case "mc_gross": Tlog.Gross = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
                    case "invoice": Tlog.TransactionID = HttpUtility.HtmlDecode(arg[1]); break;
                    case "protection_eligibility": Tlog.Protection = HttpUtility.HtmlDecode(arg[1]); break;
                    case "payer_id": Tlog.PayerID = HttpUtility.HtmlDecode(arg[1]); break;
                    case "tax": Tlog.Tax = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
                    case "payment_date": Tlog.PaymentDate = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
                    case "payment_status": Tlog.PaymentStatus = HttpUtility.HtmlDecode(arg[1]); break;
                    case "charset": break;
                    case "first_name": Tlog.FirstName = HttpUtility.HtmlDecode(arg[1]); break;
                    case "mc_fee": Tlog.TransactionFee = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
                    case "ify_version": break;
                    case "payer_status": break;
                    case "business": Tlog.Business = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
                    case "quantity": break;
                    case "payer_email": Tlog.PayerEmail = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
                    case "verify_sign": Tlog.VerifySign = HttpUtility.HtmlDecode(arg[1]); break;
                    case "txn_id": Tlog.PaypalID = HttpUtility.HtmlDecode(arg[1]); break;
                    case "payment_type": Tlog.PaymentType = HttpUtility.HtmlDecode(arg[1]); break;
                    case "last_name": Tlog.LastName = HttpUtility.HtmlDecode(arg[1]); break;
                    case "receiver_email": Tlog.ReceiverEmail = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
                    case "payment_fee": break;
                    case "receiver_id": Tlog.ReceiverID = HttpUtility.HtmlDecode(arg[1]); break;
                    case "txn_type": break;
                    case "item_name": Tlog.ItemName = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); Tlog.Subject = Server.UrlDecode(HttpUtility.HtmlDecode(arg[1])); break;
                    case "mc_currency": Tlog.Currency = HttpUtility.HtmlDecode(arg[1]); break;
                    case "item_number": break;
                    case "residence_country": Tlog.Country = HttpUtility.HtmlDecode(arg[1]); break;
                    case "handling_amount": Tlog.Handling = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
                    case "transaction_subject": break;
                    case "payment_gross": break;
                    case "shipping": Tlog.Shipping = float.Parse(HttpUtility.HtmlDecode(arg[1])); break;
                    case "merchant_return_link": break;
                    case "custom": Tlog.RegistrationID = new Guid(HttpUtility.HtmlDecode(arg[1])); break;
                    //string[] custom = HttpUtility.HtmlDecode(arg[1]).Split(new string[] { "DCode" }, StringSplitOptions.None);
                    //for (int y = 0; y < custom.Length; y++)
                    //{
                    //    if (y.Equals(0))
                    //        Tlog.RegistrationID = new Guid(custom[y]);

                    //    if (y.Equals(1))
                    //        discountCode = custom[y];
                    //}
                    //break;
                }
            }

            if (Tlog.PaymentStatus.Contains("Completed"))
            {
                try
                {
                    GeneralFunction.CompletePaymentAdhocPayPal(Tlog.RegistrationID);
                }
                catch (Exception exp)
                {
                    RawLog log = RawLog.NewRawLog();
                    log.Type = 3; // Process payment error
                    log.Data = "[" + exp.Message + "]";
                    log.DateString = DateTime.Now.ToString();

                    if (log.IsValid) log.Save();
                }
            }

            if (Tlog.PaymentStatus.Contains("Pending"))
            {
                // not so good
            }

            if (Tlog.PaymentStatus.Contains("Denied"))
            {
                // bad!
            }

            if (Tlog.IsValid)
                Tlog.Save();
            else
            {
                RawLog log = RawLog.NewRawLog();
                log.Type = 2; //error
                log.Data = Tlog.BrokenRulesCollection.ToString();
                log.DateString = DateTime.Now.ToString();

                if (log.IsValid) log.Save();
            }
        }
        catch (Exception ex)
        {
            RawLog log = RawLog.NewRawLog();
            log.Type = 4; // Overall payment error
            log.Data = "[" + ex.Message + "]";
            log.DateString = DateTime.Now.ToString();

            if (log.IsValid) log.Save();
        }
    }
}