using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_SendMail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string emailformat = Email.ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "ForgotPasswordEmail.htm");
        emailformat = emailformat.Replace("#NAME#", "Fendy Yulianto");
        emailformat = emailformat.Replace("#USERNAME#", "FendyYulianto@Yuhu.com");
        emailformat = emailformat.Replace("#PASSWORD#", "MyPassword");
        emailformat = emailformat.Replace("#URL#", System.Configuration.ConfigurationSettings.AppSettings["WebUrl"]);

        string host = "Localhost";
        int port = 25;
        string emailFromName = System.Configuration.ConfigurationSettings.AppSettings["AdminEmailName"];
        string emailFrom = System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"];
        string emailFromLogin = "user1@localmail.com";
        string emailFromPassword = "";
        string emailTo = "user1@localmail.com";
        string emailCc = "user1@localmail.com";
        string emailBcc = "user1@localmail.com";
        string emailSubject = "APAC Effie "+ GeneralFunction.EffieEventYear() + " Login Credentials";
        bool isHTML = true;
        string emailMessage = emailformat;
        AttachmentCollection attachmentCollection = null;
        AlternateView alternativeView = null;
        
        SendEmail(host, port, emailFromName, emailFrom, emailFromLogin, emailFromPassword, emailTo,
            emailCc, emailBcc, emailSubject, isHTML, emailMessage, attachmentCollection, alternativeView);
    }

    public static void SendEmail(string host, int port, string emailFromName, string emailFrom,
        string emailFromLogin, string emailFromPassword, string emailTo, string emailCc, string emailBcc,
        string emailSubject, bool isHTML, string emailMessage, AttachmentCollection attachmentCollection = null,
        AlternateView alternativeView = null)
    {
        SmtpClient smtpClient = new SmtpClient();
        NetworkCredential networkCredential = null;
        MailMessage email = new MailMessage();

        smtpClient.Host = host;
        smtpClient.Port = port;

        if (!emailFromPassword.Trim().Equals(""))
        {
            networkCredential = new NetworkCredential(emailFromLogin, emailFromPassword);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredential;
        }

        MailAddress fromAddress = new MailAddress(emailFrom, emailFromName);
        email.From = fromAddress;

        if (!emailCc.Equals(""))
            email.CC.Add(emailCc);
        if (!emailBcc.Equals(""))
        {
            string[] emailBccAddList = emailBcc.Split(',');
            foreach (string emailBccAdd in emailBccAddList)
            {
                if (!emailBccAdd.Trim().Equals(""))
                    email.Bcc.Add(emailBccAdd);
            }
        }

        email.To.Add(emailTo);
        email.Subject = emailSubject;
        email.IsBodyHtml = isHTML;
        email.Body = emailMessage;

        if (attachmentCollection != null)
        {
            foreach (Attachment attachment in attachmentCollection)
            {
                email.Attachments.Add(attachment);
            }
        }

        if (alternativeView != null)
        {
            email.AlternateViews.Add(alternativeView);
        }
        smtpClient.Send(email);
    }
}
