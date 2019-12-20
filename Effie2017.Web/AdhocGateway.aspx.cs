using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Effie2017.App;

public partial class AdhocGateway : System.Web.UI.Page
{
    Guid payGroupId = Guid.Empty;
    string payGroupIdString = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        payGroupIdString = Request.QueryString["pgId"];

        if (payGroupIdString != null && !String.IsNullOrEmpty(payGroupIdString))
            payGroupId = GeneralFunction.GetValueGuid(payGroupIdString,true);
        else
            return;


        StringBuilder url = new StringBuilder();

        string m_sPaypalBase = System.Configuration.ConfigurationManager.AppSettings["paypalBase"];
        string business = System.Configuration.ConfigurationManager.AppSettings["paypalEmail"];
        string cancelUrl = System.Configuration.ConfigurationManager.AppSettings["cancelPaymentUrlAdhoc"];
        string returnUrl = System.Configuration.ConfigurationManager.AppSettings["successPaymentUrlAdhoc"];
        string notifyUrl = System.Configuration.ConfigurationManager.AppSettings["notifyUrlAdhoc"];

        url.Append(m_sPaypalBase);
        url.AppendFormat("&business={0}", HttpUtility.UrlEncode(business));
        url.AppendFormat("&item_name={0}", HttpUtility.UrlEncode("Payment for APAC Effie "+ GeneralFunction.EffieEventYear()));  // for: " + serials));
        url.AppendFormat("&item_number={0}", HttpUtility.UrlEncode(payGroupId.ToString().Substring(0, 8).ToUpper())); // suppose to serial num, but there is possiblity of multiple entries per order????

        //decimal amount = GeneralFunction.CalculateGroupTotalPriceFromCache();
        //amount += GeneralFunction.CalculateCreditFees(amount); // Add fees for PP payment
        decimal amount = GeneralFunction.TotalAdhocGrandAmount(payGroupId);
        
        bool IsTestPay = System.Configuration.ConfigurationManager.AppSettings["IsTestPay"].ToString() == "1";
        if (IsTestPay)
        {
            amount = 0.01m;
        }
        url.AppendFormat("&amount={0}", HttpUtility.UrlEncode(amount.ToString("0.00")));



        // For testing of $0.01 
        //url.AppendFormat("&amount={0}", HttpUtility.UrlEncode("0.01"));




        url.AppendFormat("&currency_code={0}", HttpUtility.UrlEncode("SGD"));

        url.AppendFormat("&shipping={0}", HttpUtility.UrlEncode("0"));
        url.AppendFormat("&no_shipping={0}", HttpUtility.UrlEncode("0"));
        url.AppendFormat("&invoice={0}", "");

        string custom = payGroupId.ToString();
        url.AppendFormat("&custom={0}", HttpUtility.UrlEncode(custom));
        url.AppendFormat("&cancel_return={0}", HttpUtility.UrlEncode(cancelUrl));
        url.AppendFormat("&notify_url={0}", HttpUtility.UrlEncode(notifyUrl));
        url.AppendFormat("&return={0}", HttpUtility.UrlEncode(returnUrl + "?custom=" + payGroupId.ToString()));

        RawLog rawlog = RawLog.NewRawLog();
        rawlog.Type = 0; //send
        rawlog.Data = url.ToString();
        rawlog.DateString = DateTime.Now.ToString();

        if (rawlog.IsValid) rawlog.Save();


        Response.Redirect(url.ToString());
    }
}