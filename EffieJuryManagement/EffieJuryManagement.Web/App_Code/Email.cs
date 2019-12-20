using System;

using System.Net.Mail;
using System.IO;
using System.Configuration;
using EffieJuryManagementApp;
using System.Text.RegularExpressions;
using System.Net;
/// <summary>
/// Summary description for Email
/// </summary>


public class Email
{
    public static int SendInvitationTemplateEmail(Invitation inv, Guid tempalteId)
    {
        int rtnValue = 0;
        //string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EmailTemplate\\InvitationEmailRound1.htm");

        EmailTemplate emailtempalte = EmailTemplate.GetEmailTemplate(tempalteId);

        if (emailtempalte != null)
        {
            string emailformat = emailtempalte.Body;
            string emailCC = string.Empty;

            Jury jury = Jury.GetJury(inv.JuryId);

            if (jury != null)
            {
                emailformat = emailformat.Replace("#NAME#", jury.FirstName);

                emailformat = emailformat.Replace("#LINKAPPROVE#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|2") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
                emailformat = emailformat.Replace("#LINKAPPROVER1#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
                emailformat = emailformat.Replace("#LINKAPPROVER2#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("2|") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
                emailformat = emailformat.Replace("#LINKREJECT#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|2") + "&request=" + GeneralFunction.StringEncryption("no") + "");

                if (emailformat.IndexOf("#PROFILELINK#") != -1)
                    jury.IsProfileUpdated = false;

                jury.Save();

                emailformat = emailformat.Replace("#PROFILELINK#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Jury/Profile.aspx?juryId=" + IptechLib.Crypto.StringEncryption(jury.Id.ToString()));
                
                emailformat = emailformat.Replace("#EMAILTRACKER#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "EmailTracking.aspx?invId=" + IptechLib.Crypto.StringEncryption(inv.Id.ToString()));

                if (!String.IsNullOrEmpty(jury.PAEmail.Trim()))
                {
                    string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                    Match match = Regex.Match(jury.PAEmail.Trim(), pattern, RegexOptions.IgnoreCase);

                    if (match.Success)
                        emailCC = jury.PAEmail;
                } 
                
                rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], emailCC, "", emailtempalte.Subject, emailformat, true, null, null);
            }
        }
        return rtnValue;
    }

    public static int SendTemplateEmail(Jury jury, Guid tempalteId)
    {
        int rtnValue = 0;
        //string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EmailTemplate\\InvitationEmailRound1.htm");

        EmailTemplate emailtempalte = EmailTemplate.GetEmailTemplate(tempalteId);

        if (emailtempalte != null)
        {
            string emailformat = emailtempalte.Body;
            string emailCC = string.Empty;
           

            if (jury != null)
            {
                emailformat = emailformat.Replace("#NAME#", jury.FirstName);

                emailformat = emailformat.Replace("#LINKAPPROVE#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|2") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
                emailformat = emailformat.Replace("#LINKAPPROVER1#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
                emailformat = emailformat.Replace("#LINKAPPROVER2#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("2|") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
                emailformat = emailformat.Replace("#LINKREJECT#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|2") + "&request=" + GeneralFunction.StringEncryption("no") + "");

                emailformat = emailformat.Replace("#JURYLOGINURL#", System.Configuration.ConfigurationSettings.AppSettings["WebURLEffie"] + "Jury/Login.aspx");

                emailformat = emailformat.Replace("#LOGINID#", jury.SerialNo);
                emailformat = emailformat.Replace("#PASSWORD#", jury.Password);

                if (emailformat.IndexOf("#PROFILELINK#") != -1)
                    jury.IsProfileUpdated = false;
               
                jury.Save();

                string updateProfileLink = ConfigurationSettings.AppSettings["WebURL"] + "Jury/Profile.aspx?juryId=" + IptechLib.Crypto.StringEncryption(jury.Id.ToString());

                emailformat = emailformat.Replace("#PROFILELINK#", "<a href='" + updateProfileLink + "'>this link</a>");


                if (!String.IsNullOrEmpty(jury.PAEmail.Trim()))
                {
                    string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                    Match match = Regex.Match(jury.PAEmail.Trim(), pattern, RegexOptions.IgnoreCase);

                    if (match.Success && tempalteId != new Guid("714b1944-3fab-41c7-b353-cedeec28924f") && tempalteId != new Guid("e0cc09ee-30e1-44b7-83b1-48834a6fbbb9"))
                        emailCC = jury.PAEmail;
                }

                //Disable PAEmail
                emailCC = "";

                rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], emailCC, "", emailtempalte.Subject, emailformat, true, null, null);
            }
        }
        return rtnValue;
    }

    //public static int SendJuryRound1andRound2EmailLocal(Invitation inv, Guid tempalteId)
    //{
    //    int rtnValue = 0;
    //    //string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EmailTemplate\\InvitationEmailRound1andRound2-Local.htm");

    //    EmailTemplate emailtempalte = EmailTemplate.GetEmailTemplate(tempalteId);

    //    if (emailtempalte != null)
    //    {
    //        string emailformat = emailtempalte.Body;
    //        Jury jury = Jury.GetJury(inv.JuryId);

    //        if (jury != null)
    //        {

    //            emailformat = emailformat.Replace("#NAME#", jury.FirstName);

    //            emailformat = emailformat.Replace("#LINKAPPROVE#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|2") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
    //            emailformat = emailformat.Replace("#LINKAPPROVER1#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
    //            emailformat = emailformat.Replace("#LINKAPPROVER2#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("2|") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
    //            emailformat = emailformat.Replace("#LINKREJECT#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|2") + "&request=" + GeneralFunction.StringEncryption("no") + "");

    //            emailformat = emailformat.Replace("#EMAILTRACKER#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "EmailTracking.aspx?invId=" + IptechLib.Crypto.StringEncryption(inv.Id.ToString()));

    //            //rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", "", "Invitation to be on APAC Effie 2016 Jury", emailformat, true, null, null);
    //            rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", "", emailtempalte.Subject, emailformat, true, null, null);
    //        }
    //    }
    //    return rtnValue;
    //}

    //public static int SendJuryRound1andRound2EmailOverseas(Invitation inv, Guid tempalteId)
    //{
    //    int rtnValue = 0;
    //    //string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EmailTemplate\\InvitationEmailRound1andRound2-Overseas.htm");

    //    EmailTemplate emailtempalte = EmailTemplate.GetEmailTemplate(tempalteId);

    //    if (emailtempalte != null)
    //    {
    //        string emailformat = emailtempalte.Body;
    //        Jury jury = Jury.GetJury(inv.JuryId);

    //        if (jury != null)
    //        {

    //            emailformat = emailformat.Replace("#NAME#", jury.FirstName);

    //            emailformat = emailformat.Replace("#LINKAPPROVE#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|2") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
    //            emailformat = emailformat.Replace("#LINKAPPROVER1#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
    //            emailformat = emailformat.Replace("#LINKAPPROVER2#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("2|") + "&request=" + GeneralFunction.StringEncryption("yes") + "");
    //            emailformat = emailformat.Replace("#LINKREJECT#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "Thankyou.aspx?jId=" + GeneralFunction.StringEncryption(jury.Id.ToString()) + "&rounds=" + GeneralFunction.StringEncryption("1|2") + "&request=" + GeneralFunction.StringEncryption("no") + "");

    //            emailformat = emailformat.Replace("#EMAILTRACKER#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"] + "EmailTracking.aspx?invId=" + IptechLib.Crypto.StringEncryption(inv.Id.ToString()));

    //            //rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", "", "Invitation to be on APAC Effie 2016 Jury", emailformat, true, null, null);
    //            rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", "", emailtempalte.Subject, emailformat, true, null, null);
    //        }
    //    }

    //    return rtnValue;
    //}
    
    public static string ReadEmailTemplate(string filePath)
    {
        // Read the file as one string.
        string _fileContent = "";

        try
        {
            System.IO.StreamReader _file = new System.IO.StreamReader(filePath);
            _fileContent = _file.ReadToEnd();

            _file.Close();
        }
        catch (Exception err)
        {
            _fileContent = err.Message;
        }

        return (_fileContent);
    }

    public static int SendMail(string mailTo, string mailFrom, string mailCC, string mailBcc, string subject, string body, bool IsHTML, AttachmentCollection attachmentCollection, AlternateView alternateview)
    {
        int rtnValue = 0;

        string mailUsername = System.Configuration.ConfigurationSettings.AppSettings["MailUsername"];
        string mailPassword = System.Configuration.ConfigurationSettings.AppSettings["MailPassword"];
        string ExtendSubject = System.Configuration.ConfigurationManager.AppSettings["ExtendSubject"];

        if (mailTo.Trim() == "") return -1;

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress(mailFrom, ConfigurationSettings.AppSettings["AdminEmailName"]);
        try { msg.To.Add(new MailAddress(mailTo)); }
        catch { return -1; }

        if (mailCC != null && mailCC != "")
        {
            for (int i = 0; i < mailCC.Split(',').Length; i++)
                msg.CC.Add(new MailAddress(mailCC.Split(',')[i]));
        }

        // standard Bcc
        string bcc = "";
        bcc = System.Configuration.ConfigurationSettings.AppSettings["Bcc"];
        if (bcc != null && bcc != "")
        {
            for (int i = 0; i < bcc.Split(',').Length; i++)
                msg.Bcc.Add(new MailAddress(bcc.Split(',')[i]));
        }

        // Custom Bcc
        if (mailBcc != null && mailBcc != "")
        {
            for (int i = 0; i < mailBcc.Split(',').Length; i++)
                msg.Bcc.Add(new MailAddress(mailBcc.Split(',')[i]));
        }


        msg.Subject = ExtendSubject + subject;
        msg.Body = body;
        if (IsHTML == true)
            msg.IsBodyHtml = true;
        else
            msg.IsBodyHtml = false;

        if (alternateview != null)
        {
            msg.AlternateViews.Add(alternateview);
        }

        if (attachmentCollection != null)
        {
            foreach (Attachment attachment in attachmentCollection)
            {
                msg.Attachments.Add(attachment);
            }
        }

        try
        {
            SmtpClient client = new SmtpClient();
            //client.Host = host;
            //client.Port = port;
            if (!String.IsNullOrEmpty(mailUsername) && !String.IsNullOrEmpty(mailPassword))
            {
                var networkCredential = new NetworkCredential(mailUsername, mailPassword);
                client.UseDefaultCredentials = false;
                client.Credentials = networkCredential;
            }

            //client.Credentials
            client.Send(msg);
        }
        catch (Exception e)
        {
            rtnValue = -1;
        }

        return rtnValue;
    }

}
