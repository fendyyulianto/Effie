using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;

public partial class Order_NotifyCreditGala : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           string raw = Request.Form.ToString();
           
           
            //string raw = "mc_gross=0.01&invoice=&protection_eligibility=Ineligible&address_status=unconfirmed&payer_id=UR49ET2KZXEW8&tax=0.00&address_street=1&payment_date=04%3a14%3a17+Jul+06%2c+2011+PDT&payment_status=Completed&charset=UTF-8&address_zip=123&first_name=Ryan&mc_fee=0.01&address_country_code=SG&address_name=Lin+Ryan&notify_version=3.1&custom=741d39e4-59a0-45ea-80cb-f8ab4a9ae516&payer_status=unverified&business=support%40iptech.com.sg&address_country=Singapore&address_city=Singapore&quantity=1&verify_sign=ApBHX6qbpxJW-Ll3oP22LSbo0WeuAQORcWZMIuElqANQ07mCXbMxElR6&payer_email=ryan.lin%40iptech.com.sg&txn_id=14778729V9946272U&payment_type=instant&last_name=Lin&address_state=&receiver_email=alan%40u-win.com.sg&payment_fee=&receiver_id=SFV2QPZN3N34S&txn_type=web_accept&item_name=Payment+for+the+Conference(s)+and+Forum(s)+purchase&mc_currency=SGD&item_number=WEBNEW2000014&residence_country=SG&receipt_id=0296-7872-5878-7642&handling_amount=0.00&transaction_subject=FFF8A3B8-3CB6-4C76-ACBE-AFFA5DB0D61EDCode42A0D59A&payment_gross=&shipping=0.00&ipn_track_id=D4iefWBu-inyL2a-VXfFwg";
           
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
                    GeneralFunction.CompleteNewGalaOrderPayPal(Tlog.RegistrationID);
                }
                catch(Exception exp) 
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