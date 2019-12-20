using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Effie2017.App;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

/// <summary>
/// Summary description for Email
/// </summary>


public class Email
{
    #region Gala
    
    public static int SendTemplateEmailReopenEntry( Entry entry)
    {
        Registration registration = Registration.GetRegistration(entry.RegistrationId);
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "ForgotPasswordEmail.htm");
        emailformat = emailformat.Replace("#NAME#", registration.Firstname + " " + registration.Lastname);
        emailformat = emailformat.Replace("#USERNAME#", registration.Email);
        emailformat = emailformat.Replace("#PASSWORD#", registration.Password);
        emailformat = emailformat.Replace("#URL#", System.Configuration.ConfigurationSettings.AppSettings["WebUrl"]);
        string subject = "APAC Effie "+ GeneralFunction.EffieEventYear() + " Login Credentials";
        rtnValue = SendMail(registration.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", "", subject, emailformat, true, null, null);

        return rtnValue;
    }

    public static int SendTemplateEmailJury(EffieJuryManagementApp.Jury jury, Guid tempalteId)
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

                //if (emailformat.IndexOf("#PROFILELINK#") != -1)
                //    jury.IsProfileUpdated = false;

                jury.Save();

                string updateProfileLink = ConfigurationSettings.AppSettings["WebURL"] + "Jury/Profile.aspx?juryId=" + IptechLib.Crypto.StringEncryption(jury.Id.ToString());

                emailformat = emailformat.Replace("#PROFILELINK#", "<a href='" + updateProfileLink + "'>this link</a>");


                if (!String.IsNullOrEmpty(jury.Email.Trim()))
                {
                    string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                    Match match = Regex.Match(jury.Email.Trim(), pattern, RegexOptions.IgnoreCase);

                    if (match.Success)
                        emailCC = jury.Email;
                }

                rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", emailCC, "", emailtempalte.Subject, emailformat, true, null, null);
            }
        }
        return rtnValue;
    }



    public static int SendTemplateEmailEntryProcessing(Registration registration, Guid tempalteId, String txtFlagReasons)
    {

        int rtnValue = 0;
        EmailTemplate emailtempalte = EmailTemplate.GetEmailTemplate(tempalteId);

        if (emailtempalte != null)
        {
            string emailformat = emailtempalte.Body;
            string emailCC = string.Empty;

            if (registration != null)
            {
                emailformat = emailformat.Replace("#FIRSTNAME#", registration.Firstname);
                emailformat = emailformat.Replace("#LASTNAME#", registration.Lastname);

                emailformat = emailformat.Replace("#ENTRYID#", "");
                emailformat = emailformat.Replace("#INVOICE#", "");

                emailformat = emailformat.Replace("#FLAGREASON#", txtFlagReasons);

                if (!String.IsNullOrEmpty(registration.Email.Trim()))
                {
                    string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                    Match match = Regex.Match(registration.Email.Trim(), pattern, RegexOptions.IgnoreCase);

                    if (match.Success)
                        emailCC = registration.Email;
                }

                rtnValue = SendMail(registration.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", emailCC, "", emailtempalte.Subject, emailformat, true, null, null);
            }
        }
        return rtnValue;
    }


    public static int SendReminderEmailTemplatelReg(Registration reg, string emailformat, string emailsubject, List<Guid> IDList, string CurrentPage)
    {
        string entryID = "";
        string invoice = "";
        string ENTRYDATA = "";
        List<Entry> entryList = new List<Entry>();
        List<Guid> paymentGroupIdList = new List<Guid>();
        //EmailTemplate emailtempalte = EmailTemplate.GetEmailTemplate(tempalteId);
        //string emailformat = emailtempalte.Body;
        int rtnValue = 0;
        try
        {
            if (CurrentPage == EmailTypeEnum.AdhocInvoice_PendingPayment.ToString())
            {
                string AdhocInvoiceDataList = "";
                foreach (Guid ID in IDList)
                {
                    try
                    {
                        AdhocInvoiceItem adhocInvoiceItem = AdhocInvoiceItem.GetAdhocInvoiceItem(ID);

                        try
                        {
                            Entry entry = Entry.GetEntry(adhocInvoiceItem.EntryId);
                            if (!string.IsNullOrEmpty(entry.Serial))
                                entryID += entry.Serial + ", ";
                        }
                        catch { }

                        if (!paymentGroupIdList.Contains(adhocInvoiceItem.PayGroupId))
                        {
                            
                            AdhocInvoiceItemList adhocInvoiceItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(adhocInvoiceItem.PayGroupId, Guid.Empty);
                            foreach (AdhocInvoiceItem AdhocInvItem in adhocInvoiceItemList)
                            {
                                if (!string.IsNullOrEmpty(AdhocInvItem.Invoice) && !invoice.Contains(AdhocInvItem.Invoice))
                                {
                                    invoice += "<tr><td style=\"font-size:13px\">" + AdhocInvItem.DateCreated.ToString("dd MMM yyyy") + "</td><td style=\"font-size:13px\">" + AdhocInvItem.Invoice + "</td></tr>";
                                }
                            }
                            AdhocInvoiceDataList += GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(adhocInvoiceItem.PayGroupId) + "<br>";
                            paymentGroupIdList.Add(adhocInvoiceItem.PayGroupId);
                        }
                    }
                    catch { }
                }

                if (!string.IsNullOrEmpty(entryID))
                    entryID = entryID.Substring(0, entryID.Length - 2).ToString();

                ENTRYDATA = AdhocInvoiceDataList;
                invoice = GenerateHTMLTable(invoice);
                
            }
            else if (CurrentPage == EmailTypeEnum.Invoice_PendingPayment.ToString())
            {
                string EntryDataList = "";
                foreach (Guid ID in IDList)
                {
                    Entry EntryItem = Entry.GetEntry(ID);
                    
                    if (!paymentGroupIdList.Contains(EntryItem.PayGroupId))
                    {
                        entryList = EntryList.GetEntryList(EntryItem.PayGroupId, Guid.Empty, "").ToList();
                        foreach (Entry entry in entryList)
                        {
                            if (!string.IsNullOrEmpty(entry.Serial))
                                entryID += entry.Serial + ", ";

                            if (!string.IsNullOrEmpty(entry.Invoice) && !invoice.Contains(entry.Invoice))
                            {
                                invoice += "<tr><td style=\"font-size:13px\">" + entry.DateCreated.ToString("dd MMM yyyy") + "</td><td style=\"font-size:13px\">" + entry.Invoice + "</td></tr>";
                            }
                        }
                        EntryDataList += GeneralFunction.GetHTMLTableFromPaymentGroup(EntryItem.PayGroupId) + "<br>";
                        paymentGroupIdList.Add(EntryItem.PayGroupId);
                    }
                }

                if (!string.IsNullOrEmpty(entryID))
                    entryID = entryID.Substring(0, entryID.Length - 2).ToString();

                ENTRYDATA = EntryDataList;
                invoice = GenerateHTMLTable(invoice);
            }
            else
            {
                foreach (Guid ID in IDList)
                {
                    Entry EntryItem = Entry.GetEntry(ID);

                    if (!string.IsNullOrEmpty(EntryItem.Serial))
                        entryID += EntryItem.Serial + ", ";
                    if (!string.IsNullOrEmpty(EntryItem.Invoice) && !invoice.Contains(EntryItem.Invoice))
                    {
                        invoice += "<tr><td style=\"font-size:13px\">" + EntryItem.DateCreated.ToString("dd MMM yyyy") + "</td><td style=\"font-size:13px\">" + EntryItem.Invoice + "</td></tr>";
                    }

                    entryList.Add(EntryItem);
                }

                if (!string.IsNullOrEmpty(entryID))
                    entryID = entryID.Substring(0, entryID.Length - 2).ToString();

                ENTRYDATA = GeneralFunction.GetHTMLTableFromPaymentGroupEntryList(entryList);
                invoice = GenerateHTMLTable(invoice);
            }

            //emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));

            emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
            emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
            emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
            emailformat = emailformat.Replace("#FLAGREASON#", "");
            emailformat = emailformat.Replace("#INVOICE#", invoice);
            emailformat = emailformat.Replace("#ENTRYDATA#", ENTRYDATA);
            emailformat = emailformat.Replace("#ENTRYID#", entryID);
            rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", "", "", emailsubject, emailformat, true, null, null);
        }
        catch { rtnValue = 0; }
       
        return rtnValue;
    }

    public static int SendTemplateEmailReg(Registration registration, Guid tempalteId, Guid ID, string type)
    {
        int rtnValue = 0;
        EmailTemplate emailtempalte = EmailTemplate.GetEmailTemplate(tempalteId);
        if (emailtempalte != null)
        {
            string emailformat = emailtempalte.Body;
            string emailCC = string.Empty;
            
            if (registration != null)
            {
                emailformat = emailformat.Replace("#FIRSTNAME#", registration.Firstname);
                emailformat = emailformat.Replace("#LASTNAME#", registration.Lastname);
                emailformat = emailformat.Replace("#FLAGREASON#", "");

                //emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
                emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
                //emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

                if (type == "Entry")
                {
                    Entry entry = Entry.GetEntry(ID);
                    emailformat = emailformat.Replace("#ENTRYID#", entry.Serial);
                    emailformat = emailformat.Replace("#INVOICE#", entry.Invoice);
                }
                else if(type == "Adhoc")
                {
                    AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(ID);
                    AdhocInvoiceItemList adInvItem = AdhocInvoiceItemList.GetAdhocInvoiceItemList(adInv.PayGroupId,ID);
                    Entry entry = Entry.GetEntry(adInvItem[0].EntryId);
                    emailformat = emailformat.Replace("#ENTRYID#", entry.Serial);
                    emailformat = emailformat.Replace("#INVOICE#", adInv.Invoice);
                    adInv.LastSendPaymentReminderEmailDateString = DateTime.Now.ToString();
                    adInv.Save();
                }
                if (!String.IsNullOrEmpty(registration.Email.Trim()))
                {
                    string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
                    Match match = Regex.Match(registration.Email.Trim(), pattern, RegexOptions.IgnoreCase);

                    if (match.Success)
                        emailCC = registration.Email;
                }

                rtnValue = SendMail(registration.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", emailCC, "", emailtempalte.Subject, emailformat, true, null, null);
            }
        }
        return rtnValue;
    }
    
    public static int SendInvitationTemplateEmail(EffieJuryManagementApp.Invitation inv, Guid tempalteId)
    {
        //TODO MAN

        int rtnValue = 0;
        //string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EmailTemplate\\InvitationEmailRound1.htm");

        EmailTemplate emailtempalte = EmailTemplate.GetEmailTemplate(tempalteId);

        if (emailtempalte != null)
        {
            string emailformat = emailtempalte.Body;
            string emailCC = string.Empty;

            EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(inv.JuryId);

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
                
                rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"],"", emailCC, "", emailtempalte.Subject, emailformat, true, null, null);
            }
        }
        return rtnValue;
    }

    public static int SendCompletedGalaPaymentEmailPayPal(GalaOrder go)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "GalaEmailPaidPayPal.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", go.PayFirstname);
        emailformat = emailformat.Replace("#LASTNAME#", go.PayLastname);
        emailformat = emailformat.Replace("#INVOICE#", go.Invoice);

        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]);

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGalaReceipt(go);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");
        attachmentCollection.Add(attachment);

        attachments = attachmentCollection;
        rtnValue = SendMail(go.PayEmail, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], "Order for APAC Effie Awards Gala Tickets - Invoice No " + go.Invoice, emailformat, true, attachments, null);

        return rtnValue;
    }

    public static int SendPendingGalaPaymentEmailOthers(GalaOrder go)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "GalaEmailPendingOthers.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", go.PayFirstname);
        emailformat = emailformat.Replace("#LASTNAME#", go.PayLastname);
        emailformat = emailformat.Replace("#INVOICE#", go.Invoice);

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGalaReceipt(go);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");
        attachmentCollection.Add(attachment);

        attachments = attachmentCollection;
        rtnValue = SendMail(go.PayEmail, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], "Order for APAC Effie Awards Gala Tickets - Invoice No " + go.Invoice, emailformat, true, attachments, null);

        return rtnValue;
    }

    public static int SendCompletedGalaPaymentEmailOthers(GalaOrder go)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "GalaEmailPaidPayPal.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", go.PayFirstname);
        emailformat = emailformat.Replace("#LASTNAME#", go.PayLastname);
        emailformat = emailformat.Replace("#INVOICE#", go.Invoice);

        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]);

        // Attachment
        AttachmentCollection attachments = null;
        //AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        //// invoice
        //MemoryStream ms = RegAttachment.GenerateGalaReceipt(go);
        //Attachment attachment = new Attachment(ms, "Invoice.pdf");
        //attachmentCollection.Add(attachment);

        //attachments = attachmentCollection;
        rtnValue = SendMail(go.PayEmail, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], "Order for APAC Effie Awards Gala Tickets - Invoice No " + go.Invoice, emailformat, true, attachments, null);

        return rtnValue;
    }

    #endregion

    #region Entry Related Emails

    public static int SendJuryReminderRound1Email(EffieJuryManagementApp.Jury jury)
    {
        int rtnValue = 0;
        string emailCC = string.Empty;

        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "JuryReminderRound1Email.htm");
        emailformat = emailformat.Replace("#FIRSTNAME#", jury.FirstName);
        emailformat = emailformat.Replace("#LASTNAME#", jury.LastName);
        emailformat = emailformat.Replace("#LOGINID#", jury.SerialNo);
        emailformat = emailformat.Replace("#PASSWORD#", jury.Password);
        emailformat = emailformat.Replace("#WebURL#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"]);

        if (!String.IsNullOrEmpty(jury.PAEmail.Trim()))
        {
            string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match match = Regex.Match(jury.PAEmail.Trim(), pattern, RegexOptions.IgnoreCase);

            if (match.Success)
                emailCC = jury.PAEmail;
        }

        rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], emailCC, "", "Judging Round 1 Reminder", emailformat, true, null, null);
        //rtnValue = SendMail("support@iptech.com", System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", "", "Judging Round 1 Reminder", emailformat, true, null, null);

        return rtnValue;
    }

    public static int SendJuryReminderRound2Email(EffieJuryManagementApp.Jury jury)
    {
        int rtnValue = 0; 
        string emailCC = string.Empty;

        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "JuryReminderRound2Email.htm");
        emailformat = emailformat.Replace("#FIRSTNAME#", jury.FirstName);
        emailformat = emailformat.Replace("#LASTNAME#", jury.LastName);
        emailformat = emailformat.Replace("#LOGINID#", jury.SerialNo);
        emailformat = emailformat.Replace("#PASSWORD#", jury.Password);
        emailformat = emailformat.Replace("#WebURL#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"]);

        if (!String.IsNullOrEmpty(jury.PAEmail.Trim()))
        {
            string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match match = Regex.Match(jury.PAEmail.Trim(), pattern, RegexOptions.IgnoreCase);

            if (match.Success)
                emailCC = jury.PAEmail;
        }

        rtnValue = SendMail(jury.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], emailCC, "", "Judging Round 2 Reminder", emailformat, true, null, null);
        //rtnValue = SendMail("support@iptech.com", System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], "", "", "Judging Round 2 Reminder", emailformat, true, null, null);

        return rtnValue;
    }

    public static int SendActivateRegistrationEmail(Registration reg)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "ActivateRegisrationEmail.htm");
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#ID#", reg.Id.ToString());
        emailformat = emailformat.Replace("#WebURL#", System.Configuration.ConfigurationSettings.AppSettings["WebURL"]);
        //emailformat = emailformat.Replace("#PARENT#", parent.Firstname + " " + parent.Lastname);
        //emailformat = emailformat.Replace("#EMAIL#", parent.Email);
        //emailformat = emailformat.Replace("#CONTACT#", parent.Mobile);
        //emailformat = emailformat.Replace("#CONTACTMODE#", parent.Contactmode);
        //emailformat = emailformat.Replace("#FEEDBACK#", Util.ShowHTMLBreaks(feedbackmessage));
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Activate Account";
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", "", subject, emailformat, true, null, null);

        return rtnValue;
    }

    public static int SendCompletedPaymentEmailPayPal(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "NewEntryEmailPaidPayPal.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");
        attachmentCollection.Add(attachment);

        // Removed details attachment
        //// details 
        //MemoryStream ms2 = RegAttachment.GenerateEntryDetailsSummary(reg, paymentGroupId);
        //Attachment attachment2 = new Attachment(ms2, "EntryDetails.pdf");
        //attachmentCollection.Add(attachment2);
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Entry Submission/Invoice " + invoicenum;
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);

        return rtnValue;
    }

    public static int SendCompletedPaymentEmailPayPalChange(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "NewEntryEmailPaidPayPalChange.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");
        attachmentCollection.Add(attachment);

        // Removed details attachment
        //// details 
        //MemoryStream ms2 = RegAttachment.GenerateEntryDetailsSummary(reg, paymentGroupId);
        //Attachment attachment2 = new Attachment(ms2, "EntryDetails.pdf");
        //attachmentCollection.Add(attachment2);
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Entry Submission/Invoice " + invoicenum;
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);

        return rtnValue;
    }

    public static int SendCompletedPaymentEmailOthers(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "NewEntryEmailPaidOthers.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");

        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Payment Confirmation/Upload of Entry Materials";
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);

        return rtnValue;
    }
    
    //public static int SendAllowUploadEmailOthers(Registration reg, Guid paymentGroupId, string invoicenum)
    //{
    //    int rtnValue = 0;
    //    string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "NewEntryEmailPaidOthers.htm");

    //    emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
    //    emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
    //    emailformat = emailformat.Replace("#INVOICE#", invoicenum);
    //    emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
    //    emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
    //    emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

    //    // Attachment
    //    AttachmentCollection attachments = null;
    //    AttachmentCollection attachmentCollection = new MailMessage().Attachments;

    //    // invoice
    //    MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
    //    Attachment attachment = new Attachment(ms, "Invoice.pdf");
    //    attachmentCollection.Add(attachment);

    //    string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Upload of Entry Materials";
    //    attachments = attachmentCollection;
    //    rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);

    //    return rtnValue;
    //}
    
    //public static int SendPaymentEmailPaypal(Registration reg, Guid paymentGroupId, string invoicenum)
    //{
    //    int rtnValue = 0;
    //    string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailPaypal.htm");

    //    emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
    //    emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
    //    emailformat = emailformat.Replace("#INVOICE#", invoicenum);
    //    emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
    //    //emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
    //    //emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

    //    // Attachment
    //    AttachmentCollection attachments = null;
    //    AttachmentCollection attachmentCollection = new MailMessage().Attachments;

    //    // invoice
    //    MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
    //    Attachment attachment = new Attachment(ms, "Invoice.pdf");
    //    attachmentCollection.Add(attachment);
    //    string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Entry Submission/Invoice " + invoicenum;
    //    attachments = attachmentCollection;
    //    rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
    //    return rtnValue;
    //}

    public static int SendNewEntryPendingPaymentChequeEmail(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "NewEntryEmailCheque.htm");
       
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
        
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");
        attachmentCollection.Add(attachment);
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Entry Submission/Invoice " + invoicenum;
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }

    public static int SendNewEntryPendingPaymentBankTransferEmail(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "NewEntryEmailBankTransfer.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");
        attachmentCollection.Add(attachment);
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Entry Submission/Invoice " + invoicenum;
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }



    public static int SendNewEntryPendingPaymentChangeToChequeEmail(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "EntryEmailChangeToCheque.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);

        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");
        attachmentCollection.Add(attachment);
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Change of Payment Mode / Invoice " + invoicenum;
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }

    public static int SendNewEntryPendingPaymentChangeToBankTransferEmail(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "EntryEmailChangeToBankTransfer.htm");

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ENTRYDATA#", GeneralFunction.GetHTMLTableFromPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetDateDepentent(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateGroupReceipt(reg, paymentGroupId);
        Attachment attachment = new Attachment(ms, "Invoice.pdf");
        attachmentCollection.Add(attachment);
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Change of Payment Mode / Invoice  " + invoicenum;
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }
    public static int SendAdhocReOpenPaymentEmailConfirm(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhoc.htm");

        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Reopening Request / Invoice " + invoicenum;

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetAdhocDateDepentent(paymentGroupId));

        emailformat = emailformat.Replace("#URL#", ConfigurationSettings.AppSettings["WebURL"]); //HEREE
        emailformat = emailformat.Replace("#PayURL#", ConfigurationManager.AppSettings["WebURL"] + "AdhocGateway.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString()));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }
    
    public static int SendAdhocReOpenPaymentEmail(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        string ToPage = "";
        string Link = "";
        int rtnValue = 0;
        AdhocInvoiceItemList adInvoiceList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(paymentGroupId, Guid.Empty);
        if (adInvoiceList.Count > 0)
        {
            AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(adInvoiceList[0].AdhocInvoiceId);
            ToPage = "AdhocInvoiceSummary.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString());
            Link = ConfigurationSettings.AppSettings["WebURL"] + "Main/" + ToPage;
        }
        
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "ReopenEmailAdhoc.htm");
    
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Reopening Request ";

        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetAdhocDateDepentent(paymentGroupId));
        emailformat = emailformat.Replace("#LINK#", Link);
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#PayURL#", ConfigurationManager.AppSettings["WebURL"] + "AdhocGateway.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString()));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            //attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }
    public static int SendAdhocOtherRequestPaymentEmailConfirm(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocConfirm.htm");

        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Entry Request ";

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));

        emailformat = emailformat.Replace("#PayURL#", ConfigurationManager.AppSettings["WebURL"] + "AdhocGateway.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString()));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }
    
    public static int SendAdhocOtherRequestPaymentEmail(Registration reg, Guid paymentGroupId, string invoicenum)
    {
        string ToPage = "";
        string Link = "";
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherRequestEmailAdhoc.htm");

        AdhocInvoiceItemList adInvoiceList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(paymentGroupId, Guid.Empty);
        if (adInvoiceList.Count > 0)
        {
            AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(adInvoiceList[0].AdhocInvoiceId);
            ToPage = "AdhocInvoiceSummary.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString());
            Link = ConfigurationSettings.AppSettings["WebURL"] + "Main/" + ToPage;
        }

        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Entry Request ";

        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#LINK#", Link);
        emailformat = emailformat.Replace("#PayURL#", ConfigurationManager.AppSettings["WebURL"] + "AdhocGateway.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString()));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            //attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }
    

    public static int SendAdhocPendingPaymentEmail(Guid paymentGroupId, string TotalFees)
    {
        int rtnValue = 0;
        string SendMailTo = "";
        string ToPage = "";
        
        AdhocInvoiceItemList adInvoiceList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(paymentGroupId, Guid.Empty);
        if (adInvoiceList.Count > 0)
        {
            AdhocInvoice adInv = AdhocInvoice.GetAdhocInvoice(adInvoiceList[0].AdhocInvoiceId);
            Registration reg = Registration.GetRegistration(adInv.RegistrationId);
            SendMailTo = reg.Email;
            ToPage = "AdhocInvoiceSummary.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString());
        }
        else
        {
            EntryList entryList = EntryList.GetEntryList(paymentGroupId, Guid.Empty, "");
            if (entryList.Count == 0) return -1;
            Registration reg = Registration.GetRegistration(entryList[0].RegistrationId);
            SendMailTo = reg.Email;
            ToPage = "Summary.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString());
        }

        string Link = ConfigurationSettings.AppSettings["WebURL"] + "Main/" + ToPage ;
        string emailformat = ReadEmailTemplate(ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "AdhocPendingPayment.htm");
        emailformat = emailformat.Replace("#TOTALFEE#", TotalFees);
        emailformat = emailformat.Replace("#LINK#", Link);

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = null;
        Attachment attachment = null;
        string Subject = "AdHoc Pending payment Method";
        attachments = attachmentCollection;
        
        rtnValue = SendMail(SendMailTo, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], Subject, emailformat, true, attachments, null);
        
        return rtnValue;
    }

    public static int SendForgotPasswordEmail(Registration reg, string password, bool IncludeBCC)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "ForgotPasswordEmail.htm");
        emailformat = emailformat.Replace("#NAME#", reg.Firstname + " " + reg.Lastname);
        emailformat = emailformat.Replace("#USERNAME#", reg.Email);
        emailformat = emailformat.Replace("#PASSWORD#", password);
        emailformat = emailformat.Replace("#URL#", System.Configuration.ConfigurationSettings.AppSettings["WebUrl"]);
        string subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " Account Reset Password";
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", "", subject, emailformat, true, null, null, IncludeBCC);

        return rtnValue;
    }
    
    public static int SendPendingPaymentAdhocChequeEmailConfirm(Registration reg, Guid paymentGroupId, string invoicenum, bool isReminder)
    {
        int rtnValue = 0;

        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocCheque.htm");
        //if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
        //    emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherPaymentEmailAdhocCheque.htm");
        string subject = string.Empty;
        Administrator admin = null;

        try
        {
            admin = Security.GetAdminLoginSession();
            if (admin != null) //ADMIN SELECTS THE PAYMENT MODE.
            {
                emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocChequeAdmin.htm");
                if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                    emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherPaymentEmailAdhocChequeConfirm.htm");
            }
        }
        catch { }

        if (isReminder)
        {
            subject = "Payment Reminder for Admin Charges - " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", "<p>This is a notification that we have not received your payment for:</p>");
        }
        else
        {
            subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Reopening Request / Invoice " + invoicenum;
            if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Entry Request / Invoice " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", string.Empty);
        }

        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetAdhocDateDepentent(paymentGroupId));
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }
    

    public static int SendPendingPaymentAdhocChequeEmail(Registration reg, Guid paymentGroupId, string invoicenum,bool isReminder)
    {
        int rtnValue = 0;
        string subject = string.Empty;

        string emailformat = ""; 
        if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
            emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherPaymentEmailAdhocChequeConfirm.htm");
        else
            emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "ReopenPaymentEmailAdhocChequeConfirm.htm");

        Administrator admin = null;
        try
        {
            admin = Security.GetAdminLoginSession();
            if (admin != null)
            {
                emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocChequeAdmin.htm");
                //if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                //    emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherPaymentEmailAdhocChequeAdmin.htm");
            }
        }
        catch { }
        if (isReminder)
        {
            subject = "Payment Reminder for Admin Charges - " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", "<p>This is a notification that we have not received your payment for:</p>");
        }
        else
        {
            subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Reopening Request / Invoice " + invoicenum;
            if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Entry Request / Invoice " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", string.Empty);
        }

        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetAdhocDateDepentent(paymentGroupId));
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }

    public static int SendPendingPaymentAdhocBankTransferEmailConfirm(Registration reg, Guid paymentGroupId, string invoicenum, bool isReminder)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocBankTransferAdmin.htm"); //NOT USE
        //if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
        //    emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherPaymentEmailAdhocBankTransfer.htm");

        string subject = string.Empty;
        Administrator admin = null;
        try
        {
            admin = Security.GetAdminLoginSession();
            if (admin != null) //ADMIN SELECTS THE PAYMENT MODE.
            {
                emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocBankTransferConfirmAdmin.htm");
                if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                    emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherPaymentEmailAdhocBankTransferConfirm.htm");
            }
        }
        catch { }
        if (isReminder)
        {
            subject = "Payment Reminder for Admin Charges - " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", "<p>This is a notification that we have not received your payment for:</p>");
        }
        else
        {
            subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Reopening Request / Invoice  " + invoicenum;
            if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Entry Request / Invoice " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", string.Empty);
        }
        
        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]); //HEREEEE
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetAdhocDateDepentent(paymentGroupId));
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }
    
    public static int SendPendingPaymentAdhocBankTransferEmail(Registration reg, Guid paymentGroupId, string invoicenum, bool isReminder)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocBankTransferAdmin.htm");
        if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
            emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherPaymentEmailAdhocBankTransferConfirm.htm");

        Administrator admin = null;
        try
        {
            admin = Security.GetAdminLoginSession();
            if (admin != null)
            {
                emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocBankTransferAdmin.htm");
                //if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                //    emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OtherPaymentEmailAdhocBankTransferAdmin.htm");
            }
        }
        catch { }

        string subject = string.Empty;

        if (isReminder)
        {
            subject = "Payment Reminder for Admin Charges - " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", "<p>This is a notification that we have not received your payment for:</p>");
        }
        else
        {
            subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Reopening Request / Invoice " + invoicenum;
            if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Entry Request / Invoice " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", string.Empty);
        }

        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetAdhocDateDepentent(paymentGroupId));
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }


    public static int SendPendingPaymentAdhocPaypalEmailConfirm(Registration reg, Guid paymentGroupId, string invoicenum, bool isReminder)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhoc.htm");

        string subject = string.Empty;

        Administrator admin = null;
        try
        {
            admin = Security.GetAdminLoginSession();
            if (admin != null) //ADMIN SELECTS THE PAYMENT MODE.
            {
                emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocPaypalConfirmAdmin.htm");

            }
        }
        catch { }

        if (isReminder)
        {
            subject = "Payment Reminder for Admin Charges - " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", "<p>This is a notification that we have not received your payment for:</p>");
        }
        else
        {
            subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Reopening Request / Invoice " + invoicenum;
            if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Entry Request / Invoice " + invoicenum;

            emailformat = emailformat.Replace("#ISREMINDER#", string.Empty);
        }

        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetAdhocDateDepentent(paymentGroupId));
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));
        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]); //HEREEEE
        emailformat = emailformat.Replace("#PayURL#", ConfigurationManager.AppSettings["WebURL"] + "AdhocGateway.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString()));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            attachmentCollection.Add(attachment);
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }

    public static int SendPendingPaymentAdhocPaypalEmail(Registration reg, Guid paymentGroupId, string invoicenum, bool isReminder)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocConfirm.htm");

        Administrator admin = null;
        //try {
        //    admin = Security.GetAdminLoginSession();
        //    if (admin != null)
        //    {
        //        emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "PaymentEmailAdhocPaypalAdmin.htm");
        //    }
        //}
        //catch { }
        
        string subject = string.Empty;

        if (isReminder)
        {
            subject = "Payment Reminder for Admin Charges - " + invoicenum;
            emailformat = emailformat.Replace("#ISREMINDER#", "<p>This is a notification that we have not received your payment for:</p>");
        }
        else
        {
            subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Reopening Request / Invoice " + invoicenum;
            if (!GeneralFunction.IsAdhocReOpen(reg.Id, paymentGroupId))
                subject = "APAC Effie " + GeneralFunction.EffieEventYear() + " - Entry Request" + invoicenum;

            emailformat = emailformat.Replace("#ISREMINDER#", string.Empty);
        }

        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]);
        emailformat = emailformat.Replace("#DateDependent#", GeneralFunction.GetAdhocDateDepentent(paymentGroupId));
        emailformat = emailformat.Replace("#FIRSTNAME#", reg.Firstname);
        emailformat = emailformat.Replace("#LASTNAME#", reg.Lastname);
        emailformat = emailformat.Replace("#INVOICE#", invoicenum);
        emailformat = emailformat.Replace("#ADHOCDATA#", GeneralFunction.GetHTMLTableFromAdhocPaymentGroup(paymentGroupId));

        emailformat = emailformat.Replace("#PayURL#", ConfigurationManager.AppSettings["WebURL"] + "AdhocGateway.aspx?pgId=" + GeneralFunction.StringEncryption(paymentGroupId.ToString()));

        // Attachment
        AttachmentCollection attachments = null;
        AttachmentCollection attachmentCollection = new MailMessage().Attachments;

        // invoice
        MemoryStream ms = RegAttachment.GenerateAdhocReceipt(reg, paymentGroupId);
        if (ms != null)
        {
            Attachment attachment = new Attachment(ms, "Invoice.pdf");
            if (admin != null)
            {
                attachmentCollection.Add(attachment);
            }
        }
        attachments = attachmentCollection;
        rtnValue = SendMail(reg.Email, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], ConfigurationSettings.AppSettings["AdminEmailName"], "", System.Configuration.ConfigurationSettings.AppSettings["BccFinance"], subject, emailformat, true, attachments, null);
        return rtnValue;
    }

    #endregion

    #region RSVP related Emails

    public static int SendRSVPEmailLocal(RSVP rSVP)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "LocalRSVP.htm");

        string emailBcc = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailBcc"].ToString();
        string emailSubject = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailSubject"].ToString();
        string emailSenderName = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailName"].ToString();

        string rsvpURL = System.Configuration.ConfigurationManager.AppSettings["WebURL"] + "RSVP/RSVPRequest.aspx?rsvpId=" + IptechLib.Crypto.StringEncryption(rSVP.Id.ToString());
        string emailTrackURL = System.Configuration.ConfigurationManager.AppSettings["WebURL"] + "RSVP/RSVPRead.aspx?rsvpId=" + IptechLib.Crypto.StringEncryption(rSVP.Id.ToString());

        emailformat = emailformat.Replace("#Name#", rSVP.FirstName);
        emailformat = emailformat.Replace("#RSVPURL#", rsvpURL);
        emailformat = emailformat.Replace("#EMAILTRACKER#", emailTrackURL);

        rtnValue = SendRSVPMail(rSVP.Email, System.Configuration.ConfigurationSettings.AppSettings["RSVPEmail"], emailSenderName, "", emailBcc, emailSubject, emailformat, true, null, null);

        return rtnValue;
    }

    public static int SendRSVPEmailOverseas(RSVP rSVP)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "OverseasRSVP.htm");

        string emailBcc = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailBcc"].ToString();
        string emailSubject = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailSubject"].ToString();
        string emailSenderName = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailName"].ToString();

        string rsvpURL = System.Configuration.ConfigurationManager.AppSettings["WebURL"] + "RSVP/RSVPRequest.aspx?rsvpId=" + IptechLib.Crypto.StringEncryption(rSVP.Id.ToString());        
        string emailTrackURL = System.Configuration.ConfigurationManager.AppSettings["WebURL"] + "RSVP/RSVPRead.aspx?rsvpId=" + IptechLib.Crypto.StringEncryption(rSVP.Id.ToString());

        emailformat = emailformat.Replace("#Name#", rSVP.FirstName);
        emailformat = emailformat.Replace("#RSVPURL#", rsvpURL);
        emailformat = emailformat.Replace("#EMAILTRACKER#", emailTrackURL);

        rtnValue = SendRSVPMail(rSVP.Email, System.Configuration.ConfigurationSettings.AppSettings["RSVPEmail"], emailSenderName, "", emailBcc, emailSubject, emailformat, true, null, null);

        return rtnValue;
    }

    public static int SendRSVPGalaDinnerEmail(RSVP rSVP)
    {
        int rtnValue = 0;
        string emailformat = ReadEmailTemplate(System.Configuration.ConfigurationSettings.AppSettings["EmailTemplateLocation"] + "GalaDinnerRSVP.htm");

        string emailBcc = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailBcc"].ToString();
        string emailSubject = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailSubject"].ToString();
        string emailSenderName = System.Configuration.ConfigurationManager.AppSettings["RSVPEmailName"].ToString();

        string rsvpURL = System.Configuration.ConfigurationManager.AppSettings["WebURL"] + "RSVP/RSVPGalaRequest.aspx?rsvpId=" + IptechLib.Crypto.StringEncryption(rSVP.Id.ToString());
        string emailTrackURL = System.Configuration.ConfigurationManager.AppSettings["WebURL"] + "RSVP/RSVPRead.aspx?rsvpId=" + IptechLib.Crypto.StringEncryption(rSVP.Id.ToString());

        emailformat = emailformat.Replace("#URL#", ConfigurationManager.AppSettings["WebURL"]);

        emailformat = emailformat.Replace("#Name#", rSVP.FirstName);
        emailformat = emailformat.Replace("#RSVPURL#", rsvpURL);        

        emailformat = emailformat.Replace("#EMAILTRACKER#", emailTrackURL);

        rtnValue = SendRSVPMail(rSVP.Email, System.Configuration.ConfigurationSettings.AppSettings["RSVPEmail"], emailSenderName, "", emailBcc, emailSubject, emailformat, true, null, null);

        return rtnValue;
    }

    #endregion

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

    public static int SendMail(string mailTo, string mailFrom, string mailFromName, string mailCC, string mailBcc, 
        string subject, string body, bool IsHTML, AttachmentCollection attachmentCollection, AlternateView alternateview, bool IncludeBCC = true)
    {
        int rtnValue = 0;
        
        string mailUsername = System.Configuration.ConfigurationSettings.AppSettings["MailUsername"];
        string mailPassword = System.Configuration.ConfigurationSettings.AppSettings["MailPassword"];
        string ExtendSubject = System.Configuration.ConfigurationManager.AppSettings["ExtendSubject"];

        if (mailTo.Trim() == "") return -1;

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress(mailFrom, mailFromName);
        try { msg.To.Add(new MailAddress(mailTo)); }
        catch { return -1; }

        #region BCC
        if (mailCC != null && mailCC != "" && IncludeBCC)
        {
            for (int i = 0; i < mailCC.Split(',').Length; i++)
                msg.CC.Add(new MailAddress(mailCC.Split(',')[i]));
        }

        // standard Bcc
        string bcc = "";
        bcc = System.Configuration.ConfigurationSettings.AppSettings["Bcc"];
        if (bcc != null && bcc != "" && IncludeBCC)
        {
            for (int i = 0; i < bcc.Split(',').Length; i++)
                msg.Bcc.Add(new MailAddress(bcc.Split(',')[i]));
        }

        // Custom Bcc
        if (mailBcc != null && mailBcc != "" && IncludeBCC)
        {
            for (int i = 0; i < mailBcc.Split(',').Length; i++)
                msg.Bcc.Add(new MailAddress(mailBcc.Split(',')[i]));
        }

        #endregion


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

    public static int SendRSVPMail(string mailTo, string mailFrom, string mailFromName, string mailCC, string mailBcc, string subject, string body, bool IsHTML, AttachmentCollection attachmentCollection, AlternateView alternateview)
    {
        int rtnValue = 0;

        string mailUsername = System.Configuration.ConfigurationSettings.AppSettings["MailUsername"];
        string mailPassword = System.Configuration.ConfigurationSettings.AppSettings["MailPassword"];
        string ExtendSubject = System.Configuration.ConfigurationManager.AppSettings["ExtendSubject"];

        if (mailTo.Trim() == "") return -1;

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress(mailFrom, mailFromName);
        try { msg.To.Add(new MailAddress(mailTo)); }
        catch { return -1; }

        if (mailCC != null && mailCC != "")
        {
            for (int i = 0; i < mailCC.Split(',').Length; i++)
                msg.CC.Add(new MailAddress(mailCC.Split(',')[i]));
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

    public static string GenerateHTMLTable(string content)
    {
        if (content == "")
            return "";

        return "<table width='400px'>" + content + "</table>";
    }
}
