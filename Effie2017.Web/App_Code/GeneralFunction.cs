using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.IO;
using Telerik.Web.UI;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Effie2017.App;
using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.ElasticTranscoder;
using Amazon.ElasticTranscoder.Model;
using Amazon.S3.Model;
using Amazon.S3.IO;
using ClosedXML.Excel;
using HtmlAgilityPack;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Globalization;

/// <summary>
/// Summary description for GeneralFunction
/// </summary>
/// 


#region Enumerators


public class CountriesBudget
{
    public string Countries { get; set; }
    public string under { get; set; }
}

public static class PaymentType
{
    public const string PayPal = "PP";
    public const string Cheque = "CHQ";
    public const string BankTransfer = "BTX";
}

public static class RoundsType
{
    public const string Round1 = "R1";
    public const string Round2 = "R2";
    public const string BothRounds = "R1R2";
    public const string NotApplicable = "NA";
}

public class ImagesUpload
{
    public string Title { get; set; }
    public string path { get; set; }
    public string ID { get; set; }
    public string FileName { get; set; }
}

public class Nomination
{
    public int GrandEffie { get; set; }
    public int Gold { get; set; }
    public int Silver { get; set; }
    public int Bronze { get; set; }
    public int Finalist { get; set; }
}


public enum EnumNomination
{
    GrandEffie,
    Gold,
    Silver,
    Bronze,
    Finalist
};

public static class EmailCategory
{
    public const string Normal = "NOR";
    public const string Invitation = "INV";
}

public static class StatusRegistration
{
    public const string InActive = "DIS";
    public const string OK = "OK";
    public const string Admin = "ADM";
}

public static class StatusEntry
{
    public const string Draft = "DFT"; // Entry Status
    public const string Ready = "RDY"; // Entry Status
    public const string PaymentPending = "PPN";// Entry Status
    public const string UploadPending = "UPN";// Entry Status
    public const string UploadCompleted = "UPC";// Entry Status
    public const string Completed = "OK";// Entry Status && Entry Processing

    public const string Reopened = "REP";// Entry Processing
    public const string PendingReopen = "PREP";// Entry Processing
    public const string PendingVerification = "UPV";// Entry Processing
}

public static class StatusEntryWidthdrawn
{
    public const string None = "";
    public const string Withdrawn = "WDN";
    public const string DQ = "DQ";
}

public static class StatusPaymentEntry
{
    public const string NotPaid = "NOT";
    public const string Paid = "OK";
}

public static class StatusGalaOrder
{
    public const string Draft = "DFT";
    public const string Confirm = "OK";
}

public static class StatuspaymentGalaOrder
{
    public const string NotPaid = "NOT";
    public const string Paid = "OK";
}


public enum EmailType
{
    Entry, Invoice, DQ
};

public enum EmailTypeEnum
{
    EntryList_PendingUploads,
    AdhocInvoice_PendingPayment,
    EntryPendingList_AllEntries,
    EntryProcessing,
    Invoice_PendingPayment,
    UserList,
    EntrySubmittedList_AllEntries
};

public static class AdhocInvoiceType
{
    public const string ReOpen = "ReOpen";
    public const string ChangeReq = "ChangeReq";
    public const string ExtDeadLine = "ExtDeadLine";
    public const string Custom = "Custom";
}


public class FileModel
{
    public string FileName { get; set; }
    public string Type { get; set; }
    public int Size { get; set; }
    public string Path { get; set; }
}

#endregion

#region Custom Events
public class ParameterEventArgs : EventArgs
{
    public string UserData1;
    public string UserData2;
    public string UserData3;
    public string UserData4;
    public string UserData5;
}
#endregion

public class GeneralFunction
{
    public GeneralFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private const string lowers = "abcdefghijklmnopqrstuvwxyz";
    private const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string number = "0123456789";
    private const string specialChar = "!@#$%^&*+-_";


    public static void LogPasswordRegistration(ref Registration reg, string NewPassword)
    {
        string password = "";
        NewPassword = CreateMD5(NewPassword);
        List<string> pass = reg.LogPassword.Split('|').ToList();
        Dictionary<string, int> passwords = new Dictionary<string, int>();
        
        for (int i = 0; i < pass.Count(); i++ )
        {
            if(!string.IsNullOrWhiteSpace(pass[i])) passwords.Add(pass[i], i);
        }

        passwords.Add(NewPassword, pass.Count());
        var ListPassword = passwords.OrderByDescending(x => x.Value).Take(5).ToList();
        ListPassword = ListPassword.OrderBy(x => x.Value).ToList();
        
        foreach (var item in ListPassword)
        {
            password += item.Key + "|";
        }

        reg.LogPassword = password;
        reg.Password = NewPassword;
        reg.DateModifiedString = DateTime.Now.ToString();

        if (!reg.IsNew)
        {
            reg.Save();
        }
    }


    public static void LogPasswordAdmin(Administrator admin, string NewPassword)
    {
        string password = "";
        NewPassword = CreateMD5(NewPassword);
        //List<string> pass = admin.LogPassword.Split('|').ToList();
        //Dictionary<string, int> passwords = new Dictionary<string, int>();

        //for (int i = 0; i < pass.Count(); i++)
        //{
        //    if (!string.IsNullOrWhiteSpace(pass[i])) passwords.Add(pass[i], i);
        //}

        //passwords.Add(NewPassword, pass.Count());
        //var ListPassword = passwords.OrderByDescending(x => x.Value).Take(5).ToList();
        //ListPassword = ListPassword.OrderBy(x => x.Value).ToList();

        //foreach (var item in ListPassword)
        //{
        //    password += item.Key + "|";
        //}

        //admin.LogPassword = password;
        admin.Password = NewPassword;
        admin.DateModifiedString = DateTime.Now.ToString();

        admin.Save();
    }

    public static string CreateMD5(string input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        return "";
    }

    public static bool PasswordCheck(string text, int MinChar)
    {
        bool data = false;
        if (text.IndexOfAny(lowers.ToCharArray()) >= 0)
        {
            if (text.IndexOfAny(uppers.ToCharArray()) >= 0)
            {
                if (text.IndexOfAny(number.ToCharArray()) >= 0)
                {
                    //if (text.IndexOfAny(specialChar.ToCharArray()) >= 0)
                    {
                        if (text.Length >= MinChar)
                            data = true;
                    }
                }
            }
        }
        return data;
    }

    public static string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder res = new StringBuilder();
        Random rnd = new Random();
        while (0 < length--)
        {
            res.Append(valid[rnd.Next(valid.Length)]);
        }
        return res.ToString();
    }

    public static bool CreateLogPassword(object obj, string Type, string IPAddress)
    {
        bool result = false;
        if (Type == "Registration")
        {
            Registration reg = obj as Registration;
            try
            {
                LoginHistory log = LoginHistory.NewLoginHistory();
                log.UserId = reg.Id;
                log.Type = Type;
                log.DateModifiedString = DateTime.Now.ToString();
                log.DateCreatedString = DateTime.Now.ToString();
                log.IPAddress = IPAddress;
                log.Save();
                result = true;
            }
            catch
            {
                result = false;
            }
        }
        else
        {
            Administrator admin = obj as Administrator;
            try
            {
                LoginHistory log = LoginHistory.NewLoginHistory();
                log.UserId = admin.Id;
                log.Type = Type;
                log.DateModifiedString = DateTime.Now.ToString();
                log.DateCreatedString = DateTime.Now.ToString();
                log.IPAddress = IPAddress;
                log.Save();
                result = true;
            }
            catch
            {
                result = false;
            }
        }


        return result;
    }

    public static void SetddlNomination(ref DropDownList ddlNomination)
    {
        ddlNomination.Items.Clear();
        List<EnumNomination> theList = Enum.GetValues(typeof(EnumNomination)).Cast<EnumNomination>().ToList();
        foreach (EnumNomination en in theList)
        {
            ddlNomination.Items.Add(en.ToString());
        }
    }

    public static string GetAdminType(string str)
    {
        if (str == "SA")
            return "Super Admin";
        else if (str == "RO")
            return "Read Only";
        else if (str == "AD")
            return "Admin";
        else if (str == "ST")
            return "Support Staff";
        else if (str == "AD2")
            return "Admin No Jury";
        else if (str == "AD3")
            return "Admin Jury Only";
        else if (str == "SF")
            return "Super Admin (Finance)";

        return str;
    }

    public static string GetEmailType(string emailTypeEnum)
    {
        string EmailType = "";
        if (emailTypeEnum == EmailTypeEnum.AdhocInvoice_PendingPayment.ToString())
            EmailType = "Adhoc Invoices";
        if (emailTypeEnum == EmailTypeEnum.EntryList_PendingUploads.ToString())
            EmailType = "Entries Submitted ";
        if (emailTypeEnum == EmailTypeEnum.EntryPendingList_AllEntries.ToString())
            EmailType = "Entries Pending Submission";
        if (emailTypeEnum == EmailTypeEnum.EntryProcessing.ToString())
            EmailType = "Entry Processing";
        if (emailTypeEnum == EmailTypeEnum.Invoice_PendingPayment.ToString())
            EmailType = "Invoices";
        if (emailTypeEnum == EmailTypeEnum.UserList.ToString())
            EmailType = "User List";
        if (emailTypeEnum == EmailTypeEnum.EntrySubmittedList_AllEntries.ToString())
            EmailType = "Material Submitted";
        return EmailType; 
    }
    

    public static bool isAllMaterialUploaded(Entry entry)
    {
        bool isUploaded = false;
        bool Authorisation = File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\" + entry.Serial + "_AuthorizationForm_PDF.pdf");
        bool Case = (
                    File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpg") ||
                    File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpeg")
                    );
        bool Creative = (
                        File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterials_PDF.pdf") ||
                        File.Exists(ConfigurationManager.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4")
                        );
        try
        {
            EntryForm entryForm = EntryForm.GetEntryForm(Guid.Empty, entry.Id);
            if (Authorisation && Case && Creative && entryForm.Status == StatusEntry.Completed)
            {
                isUploaded = true;
            }
            else
            {
                isUploaded = false;
            }
        }
        catch
        {
            isUploaded = false;
        }

        return isUploaded;
    }


    #region Form Related Function
    public static void SaveEmailSentLogJury(EffieJuryManagementApp.Jury jury, Guid templateId, string eventYear)
    {
        EffieJuryManagementApp.EmailTemplate templateSent = null;

        // Recording Email sent for each Juries
        EffieJuryManagementApp.EmailSent emailsent = EffieJuryManagementApp.EmailSent.NewEmailSent();
        emailsent.JuryId = jury.Id;
        emailsent.TemplateId = templateId;

        templateSent = EffieJuryManagementApp.EmailTemplate.GetEmailTemplate(templateId);
        if (templateSent != null)
            emailsent.TemplateName = templateSent.Title;
        else
            emailsent.TemplateName = string.Empty;

        emailsent.DateCreatedString = DateTime.Now.ToString();
        emailsent.DateModifiedString = DateTime.Now.ToString();

        emailsent.EventYear = eventYear;

        if (emailsent.IsValid)
            emailsent.Save();
    }


    public static void DeleteImagegallery(string ID, int count, Entry entry, string type)
    {
        int counts = 1;
        string ImagesPath = "";
        string PathEntryForm = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "\\EntryForm\\" + "\\" + entry.Id + "\\";
        List<ImagesUpload> imageupload = GeneralFunction.GetImageGallery(count, entry, type, false);
        try
        {
            EntryFormGallery entryFormGallery = EntryFormGallery.GetEntryFormGallery(Guid.Empty, entry.Id, type);

            foreach (ImagesUpload item in imageupload)
            {
                string Fileimage = item.path;
                string path = counts + ":" + Fileimage + "|";

                if (ID == item.ID)
                {
                    path = counts + ":" + "|";
                    if (File.Exists(PathEntryForm + Fileimage))
                        File.Delete(PathEntryForm + Fileimage);
                }

                ImagesPath += path;

                counts++;
            }
            entryFormGallery.ImagesPath = ImagesPath;
            entryFormGallery.Save();
        }
        catch
        { }
    }


    public static ImageGalleryResult SaveImageGallery(FileUpload file, string ID, Entry entry, string type, Page page)
    {
        ImageGalleryResult imageGalleryResult = new ImageGalleryResult();
        string path = System.Configuration.ConfigurationSettings.AppSettings["EntryForm"] + "\\" + entry.Id + "\\" + "Image\\";
        var exists = System.IO.Directory.Exists(path);
        if (!exists)
            System.IO.Directory.CreateDirectory(path);

        string FileLoc = "";
        if ((file.PostedFile != null) && (file.PostedFile.ContentLength > 0))
        {
            string error = GeneralFunction.ValidateFileUpload("", file, true, "ImageChart", 1048576).Replace("<br>", "");
            if (string.IsNullOrEmpty(error))
            {
                string SubName = file.PostedFile.FileName.Substring(0, file.PostedFile.FileName.LastIndexOf("."));
                string Ext = file.PostedFile.FileName.Replace(SubName, "");
                string FileName = SubName + "-" + Guid.NewGuid().ToString().Substring(0, 5) + Ext;
                string SaveLocation = path + System.IO.Path.GetFileName(FileName);

                imageGalleryResult.Filename = FileName;
                imageGalleryResult.Error = "";
                try
                {
                    DeleteImagegallery(ID, 0, entry, type);
                    file.PostedFile.SaveAs(SaveLocation);
                    FileLoc = "\\Image\\" + System.IO.Path.GetFileName(FileName);
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                imageGalleryResult.Filename = file.PostedFile.FileName;
                imageGalleryResult.FileLocation = "";
                imageGalleryResult.Error = "Only jpeg/png format supported";
                ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", @"alert('" + error + "')", true);
            }
        }

        ImagesUpload imageupload = GeneralFunction.GetImageGallery(0, entry, type, false).FirstOrDefault(x => x.ID == ID);
        if (imageupload != null)
        {
            string Imagepath = imageupload.path;
            if (string.IsNullOrEmpty(FileLoc) && !string.IsNullOrEmpty(Imagepath))
            {
                FileLoc = Imagepath;
            }
        }

        imageGalleryResult.FileLocation = FileLoc;
        return imageGalleryResult;
    }

    public static List<ImagesUpload> GetImageGallery(int count, Entry entry, string type, bool WithURL = true)
    {
        string url = "";
        string Text = "Image to Chart/Graph ";
        if (WithURL)
            url = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "\\EntryForm\\" + "\\" + entry.Id + "\\";

        List<ImagesUpload> imagesUpload = new List<ImagesUpload>();
        imagesUpload.Clear();
        try
        {
            EntryFormGallery Gallery = EntryFormGallery.GetEntryFormGallery(Guid.Empty, entry.Id, type);
            string[] ImagesPath = Gallery.ImagesPath.Split('|');
            foreach (string Image in ImagesPath)
            {
                try
                {
                    string[] item = Image.Split(':');
                    string ID = item[0];
                    string fileName = "";
                    if (!string.IsNullOrEmpty(item[1]))
                    {
                        string Path = item[1];
                        fileName = Path.Replace("\\Image\\", "");
                        imagesUpload.Add(new ImagesUpload { path = url + Path, Title = Text + ID + " :", ID = ID, FileName = fileName });
                    }
                    else
                    {
                        imagesUpload.Add(new ImagesUpload { path = "", Title = Text + ID + " :", ID = ID, FileName = fileName });
                    }
                }
                catch { }
            }

            if (ImagesPath.Count() > imagesUpload.Count())
            {
                for (int i = imagesUpload.Count() + 1; i <= count; i++)
                {
                    imagesUpload.Add(new ImagesUpload { path = "", Title = Text + i.ToString() + " :", ID = i.ToString(), FileName = "" });
                }
            }
        }
        catch
        {
            imagesUpload.Clear();
            for (int i = 1; i <= count; i++)
            {
                {
                    imagesUpload.Add(new ImagesUpload { path = "", Title = Text + i + " :", ID = i.ToString(), FileName = "" });
                }

            }
        }
        return imagesUpload;
    }

    public static string GenerateImagesPDF(Entry entry, string body, List<string> TypeImage)
    {
        string WebURL = System.Configuration.ConfigurationSettings.AppSettings["WebPhysicalPath"];
        string HeaderTable = "<table border=\"1\">";
        string FooterTable = "</table><br>";
        string BodyTable = "";
        string Fulltable = "";
        string imgString = "";

        string NoImage = "<img src=\"" + WebURL + "images/Chartblank.png" + "\" width=\"250\" align=\"center\">";
        string path = System.Configuration.ConfigurationSettings.AppSettings["EntryForm"] + "\\" + entry.Id + "\\";
        string url = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "\\EntryForm\\" + "\\" + entry.Id + "\\";
        foreach (string type in TypeImage)
        {
            BodyTable = "";
            string FooterName = "";
            int CountImages = 0;
            List<ImagesUpload> imagesUpload = GeneralFunction.GetImageGallery(0, entry, type, false);
            for (int i = 1; i <= imagesUpload.Count(); i++)
            {
                if (!string.IsNullOrEmpty(imagesUpload[i - 1].path))
                {
                    CountImages++;
                    imgString += "<td><div style=\"color:#7f8182;\"><img src=\"" + url + imagesUpload[i - 1].path + "\" width=\"250\" align=\"center\" /></div></td>";
                    FooterName += "<td align=\"center\" style=\"font-size:10px;line-height:10px;\" > Chart " + i + "</td>";
                }

                if (CountImages % 2 == 0)
                {
                    if (!string.IsNullOrEmpty(imgString))
                        BodyTable += "<tr>" + imgString + "</tr>" + "<tr>" + FooterName + "</tr>";

                    imgString = "";
                    FooterName = "";
                }

                if (imagesUpload.Count() == i)
                {
                    if(!string.IsNullOrEmpty(imgString))
                        BodyTable += "<tr>" + imgString + "<td>" + NoImage + "</td>" + "</tr>" + "<tr>" + FooterName + "<td></td>" + "</tr>";
                    imgString = ""; FooterName = "";
                }

            }

            if(!string.IsNullOrEmpty(BodyTable))
                Fulltable = HeaderTable + BodyTable + FooterTable;
            
            body = body.Replace("###" + type + "###", Fulltable);
            Fulltable = "";
            BodyTable = "";
        }

        return body;
    }


    public static void SaveEmailSentLogReg(Registration registration, Guid templateId, string eventYear, string TypeEntry, Guid ID)
    {
        EmailTemplate templateSent = null;

        // Recording Email sent for each Juries
        RegistrationEmailSent registrationEmailSent = RegistrationEmailSent.NewRegistrationEmailSent();
        registrationEmailSent.RegistrationId = registration.Id;
        registrationEmailSent.TemplateId = templateId;
        registrationEmailSent.EntryType = TypeEntry;

        templateSent = EmailTemplate.GetEmailTemplate(templateId);
        if (templateSent != null)
            registrationEmailSent.TemplateName = templateSent.Title;
        else
            registrationEmailSent.TemplateName = string.Empty;

        registrationEmailSent.EntryId = ID;
        registrationEmailSent.DateCreatedString = DateTime.Now.ToString();
        registrationEmailSent.DateModifiedString = DateTime.Now.ToString();

        registrationEmailSent.EventYear = eventYear;

        if (registrationEmailSent.IsValid)
            registrationEmailSent.Save();
    }

    //Form Related Function
    public static string ValidateTextBox(string fieldName, TextBox txt, bool isRequired, string type, string OverideText = "")
    {
        string ret = "";

        if (isRequired && txt.Text.Trim().Equals(""))
        {
            ret = fieldName + " is required.<br>";
            if (!string.IsNullOrEmpty(OverideText))
                ret = OverideText + "<br>";
        }
        
        if (!txt.Text.Trim().Equals(""))
        {
            if (type == "string")
                ret = "";
            else if (type == "number")
            {
                char[] test = txt.Text.ToCharArray();

                foreach (char chr in test)
                {
                    try
                    {
                        int intTest = 0;
                        if (chr.ToString() != "")
                            intTest = System.Convert.ToInt32(chr.ToString());
                    }
                    catch
                    {
                        ret = fieldName + " should be in numerics.<br>";
                    }
                }
            }
            else if (type == "phoneNumber")
            {
                char[] test = txt.Text.ToCharArray();

                foreach (char chr in test)
                {
                    try
                    {
                        int intTest = 0;
                        if (chr.ToString() != "" && chr.ToString() != "-" && chr.ToString() != "–")
                            intTest = System.Convert.ToInt32(chr.ToString());
                    }
                    catch
                    {
                        ret = fieldName + " should be in numerics.<br>";
                    }
                }
            }
            else if (type == "int")
            {
                try
                {
                    int test = System.Convert.ToInt32(txt.Text);
                    return "";
                }
                catch
                {
                    ret = fieldName + " should be in numerics.<br>";
                }
            }
            else if (type == "double")
            {
                try
                {
                    double test = System.Convert.ToDouble(txt.Text);
                    ret = "";
                }
                catch
                {
                    ret = fieldName + " is number only.<br>";
                }
            }
            else if (type == "decimal")
            {
                try
                {
                    decimal test = System.Convert.ToDecimal(txt.Text);
                    ret = "";
                }
                catch
                {
                    ret = fieldName + " should be in numerics.<br>";
                }
            }
            else if (type == "DateTime")
            {
                try
                {
                    DateTime test = System.Convert.ToDateTime(txt.Text);
                }
                catch
                {
                    ret = fieldName + " is date time only.<br>";
                }
            }
            else if (type == "EmailSingle")
            {
                string[] test = txt.Text.Split('@');

                if (test.Length != 2)
                    ret = fieldName + " is Invalid.<br>";
                else if (test[1].Trim().Equals(""))
                    ret = fieldName + " is Invalid.<br>";
                else
                {
                    test = test[1].Split('.');

                    if (test.Length == 1)
                        ret = fieldName + " is Invalid.<br>";
                    else if (test[test.Length - 1].Trim().Equals(""))
                        ret = fieldName + " is Invalid.<br>";
                }
            }
            else if (type == "EmailMultiple")
            {
                string[] emails = txt.Text.Split(',');

                foreach (string email in emails)
                {
                    string[] test = email.Split('@');

                    if (test.Length != 2)
                        ret = fieldName + " is Invalid.<br>";
                    else if (test[1].Trim().Equals(""))
                        ret = fieldName + " is Invalid.<br>";

                    test = test[1].Split('.');

                    if (test.Length == 1)
                        ret = fieldName + " is Invalid.<br>";
                    else if (test[test.Length - 1].Trim().Equals(""))
                        ret = fieldName + " is Invalid.<br>";
                }
            }
        }

        if (ret != "") HighlightControl(txt);

        return ret;
    }

    public static string ValidateDatePick(string fieldName, Telerik.Web.UI.RadDatePicker RadDatePick, bool isRequired)
    {
        if (isRequired && RadDatePick.SelectedDate == null)
        {
            HighlightControl(RadDatePick);
            return fieldName + " is required.<br>";
        }


        return "";
    }

    public static string ValidateRadioButtonList(string fieldName, RadioButtonList rdLst, bool isRequired, string OverideText = "")
    {
        string ret = "";
        if (isRequired && rdLst.SelectedValue.Trim().Equals(""))
        {
            HighlightControl(rdLst);
            ret = fieldName + " is required.<br>";
            if (!string.IsNullOrEmpty(OverideText))
                ret = OverideText + "<br>";

            return ret;
        }

        return "";
    }

    public static string ValidateCheckBoxList(string fieldName, CheckBoxList chkLst, bool isRequired)
    {
        if (isRequired && chkLst.SelectedValue.Trim().Equals(""))
        {
            HighlightControl(chkLst);
            return fieldName + " is required.<br>";
        }

        return "";
    }

    public static string ValidateDropDownList(string fieldName, DropDownList ddl, bool isRequired, string emptyValue, string OverideText = "")
    {
        string ret = "";
        if (isRequired && ddl.SelectedValue.Trim().Equals(emptyValue))
        {
            if (string.IsNullOrEmpty(OverideText))
            {
                ret = fieldName + " is required.<br>";
            }
            else
            {
                ret = OverideText + "<br>";
            }
            HighlightControl(ddl);
        }

        return ret;
    }

    public static string ValidateComboBoxList(string fieldName, Telerik.Web.UI.RadComboBox radComBox, bool isRequired, string emptyValue)
    {
        if (isRequired && radComBox.SelectedValue.Trim().Equals(emptyValue))
        {
            HighlightControl(radComBox);
            return fieldName + " is required.<br>";
        }

        return "";
    }

    public static bool ValidateDate(string date)
    {
        bool isValid = true;

        try
        {
            DateTime test = System.Convert.ToDateTime(date);
        }
        catch
        {
            isValid = false;
        }

        return isValid;
    }

    public static bool ValidateDate(string year, string month, string day)
    {
        bool isValid = true;

        try
        {
            DateTime test = new DateTime(System.Convert.ToInt32(year), System.Convert.ToInt32(month), System.Convert.ToInt32(day));
        }
        catch
        {
            isValid = false;
        }

        return isValid;
    }

    public static string ValidateFileUpload(string fieldName, FileUpload fu, bool isRequired, string fileType, int maxSizeInByte)
    {
        if (isRequired && fu.FileName.Trim().Equals(""))
            return fieldName + " is required.<br>";

        if (!fu.FileName.Trim().Equals(""))
        {
            if (fileType == "Image")
            {
                if (!fu.FileName.ToLower().EndsWith(".jpg") && !fu.FileName.ToLower().EndsWith(".jpeg") && !fu.FileName.ToLower().EndsWith(".gif"))
                    return fieldName + " Upload jpg, jpeg and gif file only.<br>";
            }
            if (fileType == "ImageChart")
            {

                if (!fu.FileName.ToLower().EndsWith(".jpg") && !fu.FileName.ToLower().EndsWith(".jpeg") && !fu.FileName.ToLower().EndsWith(".gif") && !fu.FileName.ToLower().EndsWith(".png"))
                    return fieldName + " Upload png, jpg, jpeg and gif file only.<br>";
            }
            else if (fileType == "SlideShow")
            {
                if (!fu.FileName.ToLower().EndsWith(".ppt"))
                    return fieldName + " Upload ppt file only.<br>";
            }
            else if (fileType == "ExhOthCat")
            {
                if (!fu.FileName.ToLower().EndsWith(".jpg") && !fu.FileName.ToLower().EndsWith(".jpeg") && !fu.FileName.ToLower().EndsWith(".pdf"))
                    return fieldName + " Upload pdf, jpg and jpeg file only.<br>";
            }
            else if (fileType == "PDF")
            {
                if (!fu.FileName.ToLower().EndsWith(".pdf"))
                    return fieldName + " Upload pdf file only.<br>";
            }
            else if (fileType == "Word")
            {
                if (!fu.FileName.ToLower().EndsWith(".doc") && !fu.FileName.ToLower().EndsWith(".docx"))
                    return fieldName + " Upload doc and docx file only.<br>";
            }
            else if (fileType == "JPG")
            {
                if (!fu.FileName.ToLower().EndsWith(".jpg") && !fu.FileName.ToLower().EndsWith(".jpeg"))
                    return fieldName + " Upload jpg and jpeg file only.<br>";
            }
            else if (fileType == "mp4")
            {
                if (!fu.FileName.ToLower().EndsWith(".mp4") && !fu.FileName.ToLower().EndsWith(".mp4"))
                    return fieldName + " Upload mp4 file only.<br>";
            }
            else if (fileType == "video")
            {
                if (!fu.FileName.ToLower().EndsWith(".mov") && !fu.FileName.ToLower().EndsWith(".avi"))
                    return fieldName + " Upload mov and avi file only.<br>";
            }

            if (maxSizeInByte != 0)
            {
                if (fu.PostedFile.ContentLength > maxSizeInByte)
                {
                    string maxSizeString = "";
                    if (maxSizeInByte < 1024)
                        maxSizeString = maxSizeInByte.ToString() + "b";
                    else if (maxSizeInByte < 1048576)
                        maxSizeString = (maxSizeInByte / 1024).ToString() + "kb";
                    else if (maxSizeInByte < 1073741824)
                        maxSizeString = (maxSizeInByte / 1048576).ToString() + "mb";
                    else
                        maxSizeString = (maxSizeInByte / 1073741824).ToString() + "gb";

                    return fieldName + " max file size is " + maxSizeString + ".<br>";
                }
            }
        }

        return "";
    }


    public static string ValidateHtmlInputFile(string fieldName, HtmlInputFile fu, bool isRequired, string fileType, int maxSizeInByte)
    {
        if (isRequired && fu.PostedFile.FileName.Trim().Equals(""))
            return fieldName + " is required.<br>";
        FileModel filemodel = new FileModel();
        filemodel.Size = fu.PostedFile.ContentLength;
        filemodel.FileName = fu.PostedFile.FileName;
        filemodel.Type = fu.PostedFile.ContentType;
        string result = ValidateFile(fieldName, filemodel, isRequired, fileType, maxSizeInByte);
        return result;
    }


    public static string ValidateFile(string fieldName, FileModel file, bool isRequired, string fileType, int maxSizeInByte)
    {
        if (isRequired && file.FileName.Trim().Equals(""))
            return fieldName + " is required.<br>";

        if (!file.FileName.Trim().Equals(""))
        {
            if (fileType == "Image")
            {
                if (!file.FileName.ToLower().EndsWith(".jpg") && !file.FileName.ToLower().EndsWith(".jpeg") && !file.FileName.ToLower().EndsWith(".gif") && !file.FileName.ToLower().EndsWith(".png"))
                    return fieldName + " Upload png, jpg, jpeg and gif file only.<br>";
            }
            else if (fileType == "SlideShow")
            {
                if (!file.FileName.ToLower().EndsWith(".ppt"))
                    return fieldName + " Upload ppt file only.<br>";
            }
            else if (fileType == "ExhOthCat")
            {
                if (!file.FileName.ToLower().EndsWith(".jpg") && !file.FileName.ToLower().EndsWith(".jpeg") && !file.FileName.ToLower().EndsWith(".pdf"))
                    return fieldName + " Upload pdf, jpg and jpeg file only.<br>";
            }
            else if (fileType == "PDF")
            {
                if (!file.FileName.ToLower().EndsWith(".pdf"))
                    return fieldName + " Upload pdf file only.<br>";
            }
            else if (fileType == "Word")
            {
                if (!file.FileName.ToLower().EndsWith(".doc") && !file.FileName.ToLower().EndsWith(".docx"))
                    return fieldName + " Upload doc and docx file only.<br>";
            }
            else if (fileType == "JPG")
            {
                if (!file.FileName.ToLower().EndsWith(".jpg") && !file.FileName.ToLower().EndsWith(".jpeg"))
                    return fieldName + " Upload jpg and jpeg file only.<br>";
            }
            else if (fileType == "mp4")
            {
                if (!file.FileName.ToLower().EndsWith(".mp4") && !file.FileName.ToLower().EndsWith(".mp4"))
                    return fieldName + " Upload mp4 file only.<br>";
            }
            else if (fileType == "video")
            {
                if (!file.FileName.ToLower().EndsWith(".mov") && !file.FileName.ToLower().EndsWith(".avi"))
                    return fieldName + " Upload mov and avi file only.<br>";
            }

            if (maxSizeInByte != 0)
            {
                if (file.Size > maxSizeInByte)
                {
                    string maxSizeString = "";
                    if (maxSizeInByte < 1024)
                        maxSizeString = maxSizeInByte.ToString() + "b";
                    else if (maxSizeInByte < 1048576)
                        maxSizeString = (maxSizeInByte / 1024).ToString() + "kb";
                    else if (maxSizeInByte < 1073741824)
                        maxSizeString = (maxSizeInByte / 1048576).ToString() + "mb";
                    else
                        maxSizeString = (maxSizeInByte / 1073741824).ToString() + "gb";

                    return fieldName + " Max file size is " + maxSizeString + ".<br>";
                }
            }
        }

        return "";
    }

    public static string ValidateRadUpload(string fieldName, Telerik.Web.UI.RadUpload radUpload, bool isRequired, string fileType, int maxSize)
    {
        if (isRequired && radUpload.UploadedFiles.Count == 0)
            return fieldName + " is required.<br>";

        if (radUpload.UploadedFiles.Count > 0)
        {
            if (fileType == "Image")
            {
                if (radUpload.UploadedFiles[0].GetExtension().ToLower() != ".jpg" && radUpload.UploadedFiles[0].GetExtension().ToLower() != ".jpeg" && radUpload.UploadedFiles[0].GetExtension().ToLower() != ".gif")
                    return fieldName + " upload jpg, jpeg and gif file only.<br>";
            }
            if (fileType == "Pdf")
            {
                if (radUpload.UploadedFiles[0].GetExtension().ToLower() != ".pdf")
                    return fieldName + " upload pdf file only.<br>";
            }

            if (maxSize != 0)
            {
                if (radUpload.UploadedFiles[0].ContentLength > maxSize)
                    return fieldName + " max file size is " + (maxSize / 1024).ToString() + "kb.<br>";
            }
        }

        return "";
    }

    public static string GetValueCheckBoxList(CheckBoxList chkLst, string delimiter)
    {
        string value = "";

        foreach (ListItem li in chkLst.Items)
        {
            if (li.Selected)
                value += li.Value + delimiter;
        }

        return value;
    }

    public static void AssignValueCheckBoxList(CheckBoxList chkLst, string data, string delimiter)
    {
        string[] itemList = data.Split(new string[] { delimiter }, StringSplitOptions.None);

        foreach (string item in itemList)
        {
            foreach (ListItem li in chkLst.Items)
            {
                if (li.Value == item)
                    li.Selected = true;
            }
        }
    }

    public static bool GetValueBoolean(RadioButtonList rdLst)
    {
        if (rdLst.SelectedValue == "true")
            return true;
        else if (rdLst.SelectedValue == "false")
            return false;

        return false;
    }

    public static void AssignValueBoolean(RadioButtonList rdLst, bool item)
    {
        string value = "";

        if (item)
            value = "true";
        else if (!item)
            value = "false";

        rdLst.SelectedValue = value;
    }
    #endregion

    #region Email Related Function
    //Email Related Function
    public static void SendEmail(string host, int port, string emailFromName, string emailFrom, string emailFromLogin, string emailFromPassword, string emailTo, string emailCc, string emailBcc, string emailSubject, bool isHTML, string emailMessage, AttachmentCollection attachmentCollection = null)
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
            email.Bcc.Add(emailBcc);

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

        smtpClient.Send(email);
    }

    public static void SendErrorException(Exception exp)
    {
        string host = System.Configuration.ConfigurationManager.AppSettings["hostError"].ToString();
        int port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["portError"].ToString());
        string emailFromName = System.Configuration.ConfigurationManager.AppSettings["emailFromNameError"].ToString();
        string emailFrom = System.Configuration.ConfigurationManager.AppSettings["emailFromError"].ToString();
        string emailFromLogin = System.Configuration.ConfigurationManager.AppSettings["emailFromLoginError"].ToString();
        string emailFromPassword = System.Configuration.ConfigurationManager.AppSettings["emailFromPasswordError"].ToString();
        string emailTo = System.Configuration.ConfigurationManager.AppSettings["emailToError"].ToString();
        string emailCc = "";
        string emailBcc = "";
        string emailSubject = System.Configuration.ConfigurationManager.AppSettings["emailSubjectError"].ToString();
        bool isHTML = true;
        string emailMessage = "";

        emailMessage = GeneralFunction.ReadTxtFile(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "emailTemplate\\globalAsaxError.htm");
        emailMessage = emailMessage.Replace("##Message##", exp.Message);
        emailMessage = emailMessage.Replace("##GetType##", exp.GetType().ToString());
        emailMessage = emailMessage.Replace("##StackTrace##", exp.StackTrace);

        GeneralFunction.SendEmail(host, port, emailFromName, emailFrom, emailFromLogin, emailFromPassword, emailTo, emailCc, emailBcc, emailSubject, isHTML, emailMessage);
    }
    #endregion

    #region File And Folder Related Function
    //File And Folder Related Function
    public static void UploadFile(FileUpload fu, string uploadTo, bool createDirectory, bool replaceFile)
    {
        if (!fu.FileName.Trim().Equals(""))
        {
            if (createDirectory && !Directory.Exists(uploadTo))
                Directory.CreateDirectory(uploadTo);

            if (!replaceFile)
            {
                if (File.Exists(uploadTo + fu.FileName))
                    throw new System.Exception("File Exsist.");
            }

            fu.PostedFile.SaveAs(uploadTo + fu.FileName);
        }
        else
            throw new System.Exception("File Empty.");
    }

    public static void CopyFile(string fileName, string copyTo, bool createDirectory, bool replaceFile)
    {
        if (File.Exists(fileName))
        {
            if (createDirectory && !Directory.Exists(copyTo))
                Directory.CreateDirectory(copyTo);

            File.Copy(fileName, copyTo + fileName.Substring(fileName.LastIndexOf("\\") + 1), replaceFile);
        }
    }

    public static void CutFile(string fileName, string cutTo, bool createDirectory, bool replaceFile)
    {
        if (File.Exists(fileName))
        {
            CopyFile(fileName, cutTo, createDirectory, replaceFile);
            DeleteFile(fileName, true);
        }
    }

    public static void DeleteFile(string fileName, bool checkFileExist)
    {
        if (File.Exists(fileName))
            File.Delete(fileName);
        else if (checkFileExist)
            throw new SystemException("File does not Exist.");
    }

    public static void CopyDirectory(string directoryName, string copyTo, bool createDirectory, bool replaceDirectory)
    {
        if (Directory.Exists(directoryName))
        {
            string toBeCopieddirectory = directoryName.Substring(0, directoryName.LastIndexOf("\\"));
            toBeCopieddirectory = toBeCopieddirectory.Substring(toBeCopieddirectory.LastIndexOf("\\") + 1) + "\\";

            if (createDirectory && !Directory.Exists(copyTo))
                Directory.CreateDirectory(copyTo);
            else if (!createDirectory && !Directory.Exists(copyTo))
                throw new SystemException(copyTo + " Directory does not Exist.");

            if (!replaceDirectory && Directory.Exists(copyTo + toBeCopieddirectory))
                throw new SystemException(copyTo + toBeCopieddirectory + " Directory already Exist.");

            string[] directories = Directory.GetDirectories(directoryName);

            foreach (string directory in directories)
            {
                CopyDirectory(directory + "\\", copyTo + toBeCopieddirectory, true, true);
            }

            string[] files = Directory.GetFiles(directoryName);

            foreach (string file in files)
            {
                CopyFile(file, copyTo + toBeCopieddirectory, true, true);
            }
        }
    }

    public static void CutDirectory(string directoryName, string cutTo, bool createDirectory, bool replaceDirectory)
    {
        if (Directory.Exists(directoryName))
        {
            CopyDirectory(directoryName, cutTo, createDirectory, replaceDirectory);
            DeleteDirectory(directoryName, true);
        }
    }

    public static void DeleteDirectory(string directoryName, bool checkDirectoryExist)
    {
        if (Directory.Exists(directoryName))
            Directory.Delete(directoryName, true);
        else if (checkDirectoryExist)
            throw new SystemException("Directory does not Exist.");
    }

    public static string ReadTxtFile(string fileName)
    {
        string FileContent = "";

        if (File.Exists(fileName))
        {
            StreamReader streamReader = File.OpenText(fileName);

            FileContent = streamReader.ReadToEnd();

            streamReader.Dispose();
        }
        else
            throw new SystemException("File does not Exist");

        return FileContent;
    }

    public static void WriteTxtFile(string fileName, string textMessage)
    {
        StreamWriter streamWriter = File.AppendText(fileName);

        streamWriter.WriteLine(textMessage);

        streamWriter.Flush();
        streamWriter.Dispose();
    }
    #endregion

    #region Other Function
    public static bool ValidateGuid(string id)
    {
        bool isValid = true;

        try
        {
            Guid test = new Guid(id);
        }
        catch
        {
            isValid = false;
        }

        return isValid;
    }
    public static Guid GetValueGuid(string stringId, bool isEncrypted)
    {
        Guid returnValue = Guid.Empty;

        if (isEncrypted)
            stringId = GeneralFunction.StringDecryption(stringId);

        try
        {
            returnValue = new Guid(stringId);
        }
        catch
        {
        }

        return returnValue;
    }
    public static string StringEncryption(string normalText)
    {
        if (normalText == null)
            return "";

        byte[] normalTextAsByte = System.Text.ASCIIEncoding.ASCII.GetBytes(normalText);

        return System.Convert.ToBase64String(normalTextAsByte);
    }
    public static string StringDecryption(string encryptedText)
    {
        if (encryptedText == null)
            return "";

        byte[] encryptedTextAsBytes = System.Convert.FromBase64String(encryptedText);

        return System.Text.ASCIIEncoding.ASCII.GetString(encryptedTextAsBytes);
    }


    public static void LoadddlHoldingCompany(DropDownList ddl, bool isWithOther, bool isWithSelect)
    {
        ddl.Items.Clear();
        if (isWithSelect)
        {
            ddl.Items.Add(new ListItem("Select", ""));
        }
        ddl.Items.Add("ADK Group");
        ddl.Items.Add("Adknowledge");
        ddl.Items.Add("Astro");
        ddl.Items.Add("Omnicom");
        ddl.Items.Add("Publicis Groupe");
        ddl.Items.Add("BlueFocus");
        ddl.Items.Add("Enero Group");
        ddl.Items.Add("Dentsu Group");
        ddl.Items.Add("Cheil Worldwide");
        ddl.Items.Add("Creative Lions Asia");
        ddl.Items.Add("The Aleph Group");
        ddl.Items.Add("DJE Holdings");
        ddl.Items.Add("Interpublic (IPG)");
        ddl.Items.Add("WPP Group");
        ddl.Items.Add("Hakuhodo");
        ddl.Items.Add("Havas");
        ddl.Items.Add("GIIR Corporation");
        ddl.Items.Add("Insight Group");
        ddl.Items.Add("LEO Group");
        ddl.Items.Add("M&C Saatchi PLC");
        ddl.Items.Add("Tencent");
        ddl.Items.Add("Alibaba");
        ddl.Items.Add("Independent");
        if (isWithOther)
        {
            ddl.Items.Add(new ListItem("Other, pls specify", "Others"));
        }
    }


    public static void LoadddlNetwork(DropDownList ddlNetwork, bool isWithOther, bool isWithSelect)
    {
        ddlNetwork.Items.Clear();
        if (isWithSelect)
        {
            ddlNetwork.Items.Add(new ListItem("Select", ""));
        }
        ddlNetwork.Items.Add("3R Group");
        ddlNetwork.Items.Add("Adfactors PR");
        ddlNetwork.Items.Add("ADK");
        ddlNetwork.Items.Add("AdParlor");
        ddlNetwork.Items.Add("APD Group");
        ddlNetwork.Items.Add("Astro");
        ddlNetwork.Items.Add("Avian Media");
        ddlNetwork.Items.Add("BBDO Worldwide");
        ddlNetwork.Items.Add("BBH");
        ddlNetwork.Items.Add("Blackpaper Limited");
        ddlNetwork.Items.Add("Blue 449");
        ddlNetwork.Items.Add("BlueFocus Digital");
        ddlNetwork.Items.Add("BMF");
        ddlNetwork.Items.Add("Carat Group");
        ddlNetwork.Items.Add("CCG Group");
        ddlNetwork.Items.Add("Cheil Worldwide");
        ddlNetwork.Items.Add("CMRS Group");
        ddlNetwork.Items.Add("Concept Group");
        ddlNetwork.Items.Add("Creative Lions Asia");
        ddlNetwork.Items.Add("Culture Machine");
        ddlNetwork.Items.Add("DDB Worldwide");
        ddlNetwork.Items.Add("Dentsu Aegis Network");
        ddlNetwork.Items.Add("Dentsu Japan Network");
        ddlNetwork.Items.Add("Edelman");
        ddlNetwork.Items.Add("Famous Innovations");
        ddlNetwork.Items.Add("FCB");
        ddlNetwork.Items.Add("Geometry Global");
        ddlNetwork.Items.Add("Grey Group");
        ddlNetwork.Items.Add("Group M");
        ddlNetwork.Items.Add("GTB");
        ddlNetwork.Items.Add("Hakuhodo");
        ddlNetwork.Items.Add("Hansa Cequity");
        ddlNetwork.Items.Add("Havas Media");
        ddlNetwork.Items.Add("Havas Worldwide");
        ddlNetwork.Items.Add("HS Ad");
        ddlNetwork.Items.Add("Hylink");
        ddlNetwork.Items.Add("ibs");
        ddlNetwork.Items.Add("Initiative");
        ddlNetwork.Items.Add("INNOCEAN Worldwide");
        ddlNetwork.Items.Add("Insight Group");
        ddlNetwork.Items.Add("IPG Mediabrands");
        ddlNetwork.Items.Add("Isobar");
        ddlNetwork.Items.Add("J. Walter Thompson");
        ddlNetwork.Items.Add("Jack Morton Worldwide");
        ddlNetwork.Items.Add("Kantar");
        ddlNetwork.Items.Add("Kinetic");
        ddlNetwork.Items.Add("Leo Burnett Worldwide");
        ddlNetwork.Items.Add("LEO Digital Network");
        ddlNetwork.Items.Add("LEO Entertainment Network");
        ddlNetwork.Items.Add("Linksus");
        ddlNetwork.Items.Add("M&C Saatchi");
        ddlNetwork.Items.Add("McCann Worldgroup");
        ddlNetwork.Items.Add("mcgarrybowen");
        ddlNetwork.Items.Add("Mediabrands");
        ddlNetwork.Items.Add("MediaCom");
        ddlNetwork.Items.Add("Mindshare Worldwide");
        ddlNetwork.Items.Add("MSLGroup");
        ddlNetwork.Items.Add("MullenLowe Group");
        ddlNetwork.Items.Add("Oasis Group Asia");
        ddlNetwork.Items.Add("Ogilvy");
        ddlNetwork.Items.Add("OMD");
        ddlNetwork.Items.Add("Omnicom Media Group");
        ddlNetwork.Items.Add("PHD");
        ddlNetwork.Items.Add("Posterscope");
        ddlNetwork.Items.Add("Publicis");
        ddlNetwork.Items.Add("R/GA");
        ddlNetwork.Items.Add("Red Fuse");
        ddlNetwork.Items.Add("Resolution Media");
        ddlNetwork.Items.Add("Saatchi & Saatchi");
        ddlNetwork.Items.Add("SapientRazorfish");
        ddlNetwork.Items.Add("Shape Advertising");
        ddlNetwork.Items.Add("Socialab");
        ddlNetwork.Items.Add("Spark Foundry");
        ddlNetwork.Items.Add("Spark44");
        ddlNetwork.Items.Add("Starcom");
        ddlNetwork.Items.Add("Sunfun Media");
        ddlNetwork.Items.Add("TBWA\\Worldwide");
        ddlNetwork.Items.Add("Tencent");
        ddlNetwork.Items.Add("The Glitch");
        ddlNetwork.Items.Add("TIC Group");
        ddlNetwork.Items.Add("UM");
        ddlNetwork.Items.Add("Vizeum");
        ddlNetwork.Items.Add("Wavemaker");
        ddlNetwork.Items.Add("Wieden+Kennedy");
        ddlNetwork.Items.Add("WPP AUNZ");
        ddlNetwork.Items.Add("Wunderman Thompson");
        ddlNetwork.Items.Add("X Social Group Limited");
        ddlNetwork.Items.Add("Y&R");
        ddlNetwork.Items.Add("Youku Tudou");
        ddlNetwork.Items.Add("Z1Star Digital");
        ddlNetwork.Items.Add("Zenith");
        ddlNetwork.Items.Add("Non-Network");
        if (isWithOther)
        {
            ddlNetwork.Items.Add(new ListItem("Other, pls specify", "Others"));
        }
    }

    public static void LoadDropDownListCountry(DropDownList ddl)
    {
        ddl.Items.Add("Afghanistan");
        ddl.Items.Add("Aland Islands");
        ddl.Items.Add("Albania");
        ddl.Items.Add("Algeria");
        ddl.Items.Add("American Samoa");
        ddl.Items.Add("Andorra");
        ddl.Items.Add("Angola");
        ddl.Items.Add("Anguilla");
        ddl.Items.Add("Antarctica");
        ddl.Items.Add("Antigua and Barbuda");
        ddl.Items.Add("Argentina");
        ddl.Items.Add("Armenia");
        ddl.Items.Add("Aruba");
        ddl.Items.Add("Australia");
        ddl.Items.Add("Austria");
        ddl.Items.Add("Azerbaijan");
        ddl.Items.Add("Bahamas");
        ddl.Items.Add("Bahrain");
        ddl.Items.Add("Bangladesh");
        ddl.Items.Add("Barbados");
        ddl.Items.Add("Belarus");
        ddl.Items.Add("Belgium");
        ddl.Items.Add("Belize");
        ddl.Items.Add("Benin");
        ddl.Items.Add("Bermuda");
        ddl.Items.Add("Bhutan");
        ddl.Items.Add("Bolivia");
        ddl.Items.Add("Bosnia and Herzegovina");
        ddl.Items.Add("Botswana");
        ddl.Items.Add("Bouvet Island");
        ddl.Items.Add("Brazil");
        ddl.Items.Add("British Indian Ocean Territory");
        ddl.Items.Add("Brunei");
        ddl.Items.Add("Bulgaria");
        ddl.Items.Add("Burkina Faso");
        ddl.Items.Add("Burundi");
        ddl.Items.Add("Cambodia");
        ddl.Items.Add("Cameroon");
        ddl.Items.Add("Canada");
        ddl.Items.Add("Cape Verde");
        ddl.Items.Add("Cayman Islands");
        ddl.Items.Add("Central African Republic");
        ddl.Items.Add("Chad");
        ddl.Items.Add("Chile");
        ddl.Items.Add("China");
        ddl.Items.Add("Christmas Island");
        ddl.Items.Add("Cocos Islands");
        ddl.Items.Add("Colombia");
        ddl.Items.Add("Comoros");
        ddl.Items.Add("Congo");
        ddl.Items.Add("Congo, Democratic Republic of the");
        ddl.Items.Add("Cook Islands");
        ddl.Items.Add("Costa Rica");
        ddl.Items.Add("Cote d'Ivoire");
        ddl.Items.Add("Croatia");
        ddl.Items.Add("Cuba");
        ddl.Items.Add("Cyprus");
        ddl.Items.Add("Czech Republic");
        ddl.Items.Add("Denmark");
        ddl.Items.Add("Djibouti");
        ddl.Items.Add("Dominica");
        ddl.Items.Add("Dominican Republic");
        ddl.Items.Add("Ecuador");
        ddl.Items.Add("Egypt");
        ddl.Items.Add("El Salvador");
        ddl.Items.Add("Equatorial Guinea");
        ddl.Items.Add("Eritrea");
        ddl.Items.Add("Estonia");
        ddl.Items.Add("Ethiopia");
        ddl.Items.Add("Falkland Islands");
        ddl.Items.Add("Faroe Islands");
        ddl.Items.Add("Fiji");
        ddl.Items.Add("Finland");
        ddl.Items.Add("France");
        ddl.Items.Add("French Guiana");
        ddl.Items.Add("French Polynesia");
        ddl.Items.Add("French Southern Territories");
        ddl.Items.Add("Gabon");
        ddl.Items.Add("Gambia");
        ddl.Items.Add("Georgia");
        ddl.Items.Add("Germany");
        ddl.Items.Add("Ghana");
        ddl.Items.Add("Gibraltar");
        ddl.Items.Add("Greece");
        ddl.Items.Add("Greenland");
        ddl.Items.Add("Grenada");
        ddl.Items.Add("Guadeloupe");
        ddl.Items.Add("Guam");
        ddl.Items.Add("Guatemala");
        ddl.Items.Add("Guernsey");
        ddl.Items.Add("Guinea");
        ddl.Items.Add("Guinea-Bissau");
        ddl.Items.Add("Guyana");
        ddl.Items.Add("Haiti");
        ddl.Items.Add("Heard Island and McDonald Islands");
        ddl.Items.Add("Honduras");
        ddl.Items.Add("Hong Kong");
        ddl.Items.Add("Hungary");
        ddl.Items.Add("Iceland");
        ddl.Items.Add("India");
        ddl.Items.Add("Indonesia");
        ddl.Items.Add("Iran");
        ddl.Items.Add("Iraq");
        ddl.Items.Add("Ireland");
        ddl.Items.Add("Isle of Man");
        ddl.Items.Add("Israel");
        ddl.Items.Add("Italy");
        ddl.Items.Add("Jamaica");
        ddl.Items.Add("Japan");
        ddl.Items.Add("Jersey");
        ddl.Items.Add("Jordan");
        ddl.Items.Add("Kazakhstan");
        ddl.Items.Add("Kenya");
        ddl.Items.Add("Kiribati");
        ddl.Items.Add("Kuwait");
        ddl.Items.Add("Kyrgyzstan");
        ddl.Items.Add("Laos");
        ddl.Items.Add("Latvia");
        ddl.Items.Add("Lebanon");
        ddl.Items.Add("Lesotho");
        ddl.Items.Add("Liberia");
        ddl.Items.Add("Libya");
        ddl.Items.Add("Liechtenstein");
        ddl.Items.Add("Lithuania");
        ddl.Items.Add("Luxembourg");
        ddl.Items.Add("Macao");
        ddl.Items.Add("Macedonia");
        ddl.Items.Add("Madagascar");
        ddl.Items.Add("Malawi");
        ddl.Items.Add("Malaysia");
        ddl.Items.Add("Maldives");
        ddl.Items.Add("Mali");
        ddl.Items.Add("Malta");
        ddl.Items.Add("Marshall Islands");
        ddl.Items.Add("Martinique");
        ddl.Items.Add("Mauritania");
        ddl.Items.Add("Mauritius");
        ddl.Items.Add("Mayotte");
        ddl.Items.Add("Mexico");
        ddl.Items.Add("Micronesia");
        ddl.Items.Add("Moldova");
        ddl.Items.Add("Monaco");
        ddl.Items.Add("Mongolia");
        ddl.Items.Add("Montenegro");
        ddl.Items.Add("Montserrat");
        ddl.Items.Add("Morocco");
        ddl.Items.Add("Mozambique");
        ddl.Items.Add("Myanmar");
        ddl.Items.Add("Namibia");
        ddl.Items.Add("Nauru");
        ddl.Items.Add("Nepal");
        ddl.Items.Add("Netherlands");
        ddl.Items.Add("Netherlands Antilles");
        ddl.Items.Add("New Caledonia");
        ddl.Items.Add("New Zealand");
        ddl.Items.Add("Nicaragua");
        ddl.Items.Add("Niger");
        ddl.Items.Add("Nigeria");
        ddl.Items.Add("Niue");
        ddl.Items.Add("Norfolk Island");
        ddl.Items.Add("Northern Mariana Islands");
        ddl.Items.Add("North Korea");
        ddl.Items.Add("Norway");
        ddl.Items.Add("Oman");
        ddl.Items.Add("Pakistan");
        ddl.Items.Add("Palau");
        ddl.Items.Add("Palestinian Territories");
        ddl.Items.Add("Panama");
        ddl.Items.Add("Papua New Guinea");
        ddl.Items.Add("Paraguay");
        ddl.Items.Add("Peru");
        ddl.Items.Add("Philippines");
        ddl.Items.Add("Pitcairn");
        ddl.Items.Add("Poland");
        ddl.Items.Add("Portugal");
        ddl.Items.Add("Puerto Rico");
        ddl.Items.Add("Qatar");
        ddl.Items.Add("Reunion");
        ddl.Items.Add("Romania");
        ddl.Items.Add("Russia");
        ddl.Items.Add("Rwanda");
        ddl.Items.Add("Saint Helena");
        ddl.Items.Add("Saint Kitts and Nevis");
        ddl.Items.Add("Saint Lucia");
        ddl.Items.Add("Saint Pierre and Miquelon");
        ddl.Items.Add("Saint Vincent and the Grenadines");
        ddl.Items.Add("Samoa");
        ddl.Items.Add("San Marino");
        ddl.Items.Add("Sao Tome and Principe");
        ddl.Items.Add("Saudi Arabia");
        ddl.Items.Add("Senegal");
        ddl.Items.Add("Serbia");
        ddl.Items.Add("Serbia and Montenegro");
        ddl.Items.Add("Seychelles");
        ddl.Items.Add("Sierra Leone");
        ddl.Items.Add("Singapore");
        ddl.Items.Add("Slovakia");
        ddl.Items.Add("Slovenia");
        ddl.Items.Add("Solomon Islands");
        ddl.Items.Add("Somalia");
        ddl.Items.Add("South Africa");
        ddl.Items.Add("South Georgia and the South Sandwich Islands");
        ddl.Items.Add("South Korea");
        ddl.Items.Add("Spain");
        ddl.Items.Add("Sri Lanka");
        ddl.Items.Add("Sudan");
        ddl.Items.Add("Suriname");
        ddl.Items.Add("Svalbard and Jan Mayen");
        ddl.Items.Add("Swaziland");
        ddl.Items.Add("Sweden");
        ddl.Items.Add("Switzerland");
        ddl.Items.Add("Syria");
        ddl.Items.Add("Taiwan");
        ddl.Items.Add("Tajikistan");
        ddl.Items.Add("Tanzania");
        ddl.Items.Add("Thailand");
        ddl.Items.Add("Timor-Leste");
        ddl.Items.Add("Togo");
        ddl.Items.Add("Tokelau");
        ddl.Items.Add("Tonga");
        ddl.Items.Add("Trinidad and Tobago");
        ddl.Items.Add("Tunisia");
        ddl.Items.Add("Turkey");
        ddl.Items.Add("Turkmenistan");
        ddl.Items.Add("Turks and Caicos Islands");
        ddl.Items.Add("Tuvalu");
        ddl.Items.Add("Uganda");
        ddl.Items.Add("Ukraine");
        ddl.Items.Add("United Arab Emirates");
        ddl.Items.Add("United Kingdom");
        ddl.Items.Add("United States");
        ddl.Items.Add("United States minor outlying islands");
        ddl.Items.Add("Uruguay");
        ddl.Items.Add("Uzbekistan");
        ddl.Items.Add("Vanuatu");
        ddl.Items.Add("Vatican City");
        ddl.Items.Add("Venezuela");
        ddl.Items.Add("Vietnam");
        ddl.Items.Add("Virgin Islands, British");
        ddl.Items.Add("Virgin Islands, U.S.");
        ddl.Items.Add("Wallis and Futuna");
        ddl.Items.Add("Western Sahara");
        ddl.Items.Add("Yemen");
        ddl.Items.Add("Zambia");
        ddl.Items.Add("Zimbabwe");
    }
    public static DateTime GetDateFromddMMyyyy(string ddMMyyyy)
    {
        DateTime date = new DateTime(9999, 12, 31);

        string[] datePartList = ddMMyyyy.Split('/');

        if (datePartList.Count() == 3)
        {
            try
            {
                date = new DateTime(Convert.ToInt32(datePartList[2].Substring(0, 4)), Convert.ToInt32(datePartList[1]), Convert.ToInt32(datePartList[0]));
            }
            catch
            {
            }
        }

        return date;
    }
    public static int GetIndexOfHeader(RadGrid radGrid, string headerText)
    {
        int indexOfHead = 999;

        for (int x = 0; x < radGrid.Columns.Count; x++)
        {
            if (radGrid.Columns[x].HeaderText == headerText)
            {
                indexOfHead = x;
                break;
            }
        }

        return indexOfHead;
    }
    public static string ReconstructRedirect(string goToURL, object obj)
    {
        if (obj != null && !goToURL.Contains("zhm=true"))
        {
            if (goToURL.Contains("?"))
                goToURL += "&zhm=true";
            else
                goToURL += "?zhm=true";
        }

        return goToURL;
    }
    public static string ConvertToRoman(int Value)
    {
        System.Text.StringBuilder sbRN = new System.Text.StringBuilder();


        //Start high, and just replace the huge numbers with letters.
        // 1111 -> M111 -> MC11 ->MCX1 -> MCXI
        sbRN.Append(GenerateNumber(ref Value, 1000, 'M'));
        sbRN.Append(GenerateNumber(ref Value, 500, 'D'));
        sbRN.Append(GenerateNumber(ref Value, 100, 'C'));
        sbRN.Append(GenerateNumber(ref Value, 50, 'L'));
        sbRN.Append(GenerateNumber(ref Value, 10, 'X'));
        sbRN.Append(GenerateNumber(ref Value, 5, 'V'));
        sbRN.Append(GenerateNumber(ref Value, 1, 'I'));

        //let's replace the some substrings like:
        //IIII to IV, VIV to IX, etc.

        sbRN.Replace("IIII", "IV");
        sbRN.Replace("VIV", "IX");
        sbRN.Replace("XXXX", "XL");
        sbRN.Replace("LXL", "XC");
        sbRN.Replace("CCCC", "CD");
        sbRN.Replace("DCD", "CM");
        return (sbRN.ToString());
    }
    private static string GenerateNumber(ref int value, int magnitude, char letter)
    {
        System.Text.StringBuilder sbNumberString = new System.Text.StringBuilder();

        while (value >= magnitude)
        {
            value -= magnitude;
            sbNumberString.Append(letter);
        }
        return (sbNumberString.ToString());
    }
    public static void RemoveHighlightControls(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.GetType() == typeof(DropDownList)) ((DropDownList)c).CssClass = "";
            if (c.GetType() == typeof(TextBox)) ((TextBox)c).CssClass = "";
            if (c.GetType() == typeof(CheckBox)) ((CheckBox)c).CssClass = "";
            if (c.GetType() == typeof(RadioButton)) ((RadioButton)c).CssClass = "";
            if (c.GetType() == typeof(RadioButtonList)) ((RadioButtonList)c).CssClass = "";
            if (c.GetType() == typeof(CheckBoxList)) ((CheckBoxList)c).CssClass = "";

            if (c.GetType() == typeof(Table)) ((Table)c).CssClass = "fees-table";

            RemoveHighlightControls(c);
        }
    }
    public static void HighlightControl(Control control)
    {
        if (control.GetType() == typeof(DropDownList)) ((DropDownList)control).CssClass = "errorControl";
        if (control.GetType() == typeof(TextBox)) ((TextBox)control).CssClass = "errorControl";
        if (control.GetType() == typeof(CheckBox)) ((CheckBox)control).CssClass = "errorControl";
        if (control.GetType() == typeof(RadioButton)) ((RadioButton)control).CssClass = "errorControl";
        if (control.GetType() == typeof(RadioButtonList)) ((RadioButtonList)control).CssClass = "errorControl";
        if (control.GetType() == typeof(CheckBoxList)) ((CheckBoxList)control).CssClass = "errorControl";

        if (control.GetType() == typeof(Table)) ((Table)control).CssClass = "errorControl";

    }
    public static void SetRedirect(string url)
    {
        HttpContext.Current.Session["Redirector.URL"] = url;
    }
    public static string GetRedirect(string url)
    {
        // if the session redirector is not null, then return that, else return back the url
        string redirect = (string)HttpContext.Current.Session["Redirector.URL"];
        if (redirect != null)
        {
            HttpContext.Current.Session.Remove("Redirector.URL");
            return redirect;
        }
        return url;
    }

    private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
    private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
    private static string PASSWORD_CHARS_NUMERIC = "23456789";

    public static string GeneratePassword(int maxLength)
    {
        char[][] charGroups = new char[][]
                {
                    PASSWORD_CHARS_LCASE.ToCharArray(),
                    PASSWORD_CHARS_UCASE.ToCharArray(),
                    PASSWORD_CHARS_NUMERIC.ToCharArray(),
				//					PASSWORD_CHARS_SPECIAL.ToCharArray()
			};

        // Use this array to track the number of unused characters in each
        // character group.
        int[] charsLeftInGroup = new int[charGroups.Length];

        // Initially, all characters in each group are not used.
        for (int i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;

        // Use this array to track (iterate through) unused character groups.
        int[] leftGroupsOrder = new int[charGroups.Length];

        // Initially, all character groups are not used.
        for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;

        // Use a 4-byte array to fill it with random bytes and convert it then
        // to an integer value.
        byte[] randomBytes = new byte[4];

        // Generate 4 random bytes.
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        rng.GetBytes(randomBytes);

        // Convert 4 bytes into a 32-bit integer value.
        int seed = (randomBytes[0] & 0x7f) << 24 |
            randomBytes[1] << 16 |
            randomBytes[2] << 8 |
            randomBytes[3];

        // Now, this is real randomization.
        Random random = new Random(seed);

        // This array will hold password characters.
        char[] password = null;

        // Allocate appropriate memory for the password.
        password = new char[maxLength];

        // Index of the next character to be added to password.
        int nextCharIdx;

        // Index of the next character group to be processed.
        int nextGroupIdx;

        // Index which will be used to track not processed character groups.
        int nextLeftGroupsOrderIdx;

        // Index of the last non-processed character in a group.
        int lastCharIdx;

        // Index of the last non-processed group.
        int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        // Generate password characters one at a time.
        for (int i = 0; i < password.Length; i++)
        {
            // If only one character group remained unprocessed, process it;
            // otherwise, pick a random character group from the unprocessed
            // group list. To allow a special character to appear in the
            // first position, increment the second parameter of the Next
            // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0,
                    lastLeftGroupsOrderIdx);

            // Get the actual index of the character group, from which we will
            // pick the next character.
            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

            // Get the index of the last unprocessed characters in this group.
            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

            // If only one unprocessed character is left, pick it; otherwise,
            // get a random character from the unused character list.
            if (lastCharIdx == 0)
                nextCharIdx = 0;
            else
                nextCharIdx = random.Next(0, lastCharIdx + 1);

            // Add this character to the password.
            password[i] = charGroups[nextGroupIdx][nextCharIdx];

            // If we processed the last character in this group, start over.
            if (lastCharIdx == 0)
                charsLeftInGroup[nextGroupIdx] =
                    charGroups[nextGroupIdx].Length;
            // There are more unprocessed characters left.
            else
            {
                // Swap processed character with the last unprocessed character
                // so that we don't pick it until we process all characters in
                // this group.
                if (lastCharIdx != nextCharIdx)
                {
                    char temp = charGroups[nextGroupIdx][lastCharIdx];
                    charGroups[nextGroupIdx][lastCharIdx] =
                        charGroups[nextGroupIdx][nextCharIdx];
                    charGroups[nextGroupIdx][nextCharIdx] = temp;
                }
                // Decrement the number of unprocessed characters in
                // this group.
                charsLeftInGroup[nextGroupIdx]--;
            }

            // If we processed the last group, start all over.
            if (lastLeftGroupsOrderIdx == 0)
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            // There are more unprocessed groups left.
            else
            {
                // Swap processed group with the last unprocessed group
                // so that we don't pick it until we process all groups.
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                    leftGroupsOrder[lastLeftGroupsOrderIdx] =
                        leftGroupsOrder[nextLeftGroupsOrderIdx];
                    leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                }
                // Decrement the number of unprocessed groups.
                lastLeftGroupsOrderIdx--;
            }
        }

        // Convert password characters into a string and return the result.
        return new string(password);
    }

    public static bool CheckPassword(string pwd)
    {
        // Enforce password complexity
        // * Must be at least 10 characters
        // * Must contain at least one one lower case letter, one upper case letter, one digit and one special character
        // * Valid special characters (which are configurable) are -   @#$%^&+=
        //
        //	string pwdPattern = "^.*(?=.{10,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$";

        // Enforce password validation
        // * Must be at least 8 characters 
        // & Must be alphanumeric
        string pwdPattern = "^.*(?=.{8,})(?=.*\\d)(?=.*[a-zA-Z]).*$";
        Regex passreg = new Regex(pwdPattern);

        Match mRes = passreg.Match(pwd);
        return mRes.Success;
    }

    public static Control FindControlRecursive(Control Root, string Id)
    {

        if (Root.ID == Id)

            return Root;



        foreach (Control Ctl in Root.Controls)
        {

            Control FoundCtl = FindControlRecursive(Ctl, Id);

            if (FoundCtl != null)

                return FoundCtl;

        }



        return null;

    }

    public static void ChangeStateControls(Control parent, bool state)
    {
        foreach (Control c in parent.Controls)
        {
            if (c is DropDownList) ((DropDownList)(c)).Enabled = state;
            if (c is TextBox) ((TextBox)(c)).Enabled = state;
            if (c is CheckBoxList) ((CheckBoxList)(c)).Enabled = state;
            if (c is RadioButtonList) ((RadioButtonList)(c)).Enabled = state;
            if (c is RadioButton) ((RadioButton)(c)).Enabled = state;
            if (c is CheckBox) ((CheckBox)(c)).Enabled = state;
            if (c is LinkButton)
            {
                LinkButton b = (LinkButton)c;
                if (b.CommandArgument != "DO_NOT_DISABLE")
                    ((LinkButton)(c)).Enabled = state;
            }

            //if (c is Button)
            //{
            //    Button b = (Button)c;
            //    if (b.CommandArgument != "DO_NOT_DISABLE")
            //        ((Button)(c)).Enabled = state;
            //}
            ChangeStateControls(c, state);
        }
    }

    public static void LoadDDLCurrentIndustrySector(DropDownList ddl)
    {
        ddl.Items.Add("Business and Industrial");
        ddl.Items.Add("Drink and Beverage");
        ddl.Items.Add("Financial Services");
        ddl.Items.Add("Food");
        ddl.Items.Add("Government and Non-profit");
        ddl.Items.Add("Household and Domestic");
        ddl.Items.Add("Leisure and Entertainment");
        ddl.Items.Add("Media and Publishing");
        ddl.Items.Add("Motor and Auto");
        ddl.Items.Add("Pharmaceutical and Healthcare");
        ddl.Items.Add("Retail");
        ddl.Items.Add("Telecoms");
        ddl.Items.Add("Tobacco");
        ddl.Items.Add("Toiletries and Cosmetics");
        ddl.Items.Add("Travel, Transport and Tourism");
        ddl.Items.Add("Utilities and Services");
        ddl.Items.Add("Wearing Apparel");
    }

    public static string ConvertToHTMLBreaks(string text)
    {
        // [br] to <br/>
        return text.Replace("[br]", "<br/>");
    }
    public static string ConvertToBreaks(string text)
    {
        // [br] to \r\n
        return text.Replace("[br]", "\r\n");
    }
    public static string ConvertHTMLBreaksToBreaks(string text)
    {
        // <br/> to \r\n
        return text.Replace("<br/>", "\r\n");
    }

    public static string ExtractBracketValue(string text)
    {
        int left = text.IndexOf("(");
        int right = text.IndexOf(")");

        if (left != -1 && right != -1)
        {
            return text.Substring(left + 1, right - left - 1);
        }

        return text;
    }
    public static bool IsInList(string value, string list, char delimiter)
    {
        string[] items = list.Split(delimiter);
        if (items.Length > 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Trim().ToUpper() == value.Trim().ToUpper())
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static string CleanDelimiterList(string value, char delimiter, string replacementdelimiter)
    {
        if (value.Length <= 1) return value;

        // if the delimiter is last char, replace it first with empty
        if (value.Substring(value.Length - 1, 1) == delimiter.ToString())
            value = value.Substring(0, value.Length - 1);

        value = value.Replace(delimiter.ToString(), replacementdelimiter);
        return value;
    }
    public static string CleanDateTimeToString(DateTime datetime, string format)
    {
        if (datetime == DateTime.MaxValue || datetime == DateTime.MinValue)
        {
            return "";
        }

        return datetime.ToString(format);
    }

    public static string GetDateReminder(Guid ID, string type)
    {
        string DateFormat = "dd/MM/yy H:mm"; //"MM/dd/yy h:mm:ss tt"
        string DateString = "";
        List<RegistrationEmailSent> registrationEmailSentList = RegistrationEmailSentList.GetRegistrationEmailSentList()
                                                                 .Where(x => x.EntryId == ID && x.EntryType == type)
                                                                 .OrderByDescending(y => y.DateCreated).ToList();

        if (registrationEmailSentList.Count() > 0)
        {
            RegistrationEmailSent registrationEmailSent = registrationEmailSentList.FirstOrDefault();
            DateString = CleanDateTimeToString(registrationEmailSent.DateCreated, DateFormat);
        }
        
        return DateString;
    }

    public static XLWorkbook StyleReport(XLWorkbook workbook)
    {
        foreach (IXLWorksheet sheet in workbook.Worksheets)
        {
            // Sheet Formatting
            IXLCell Registration_Head = workbook.Worksheets.Worksheet(sheet.Name).Row(1).Cell(1);
            IXLCell LastColumnAddress = sheet.LastCellUsed();
            IXLRange isTable = workbook.Worksheets.Worksheet(sheet.Name).Range((IXLCell)Registration_Head, (IXLCell)LastColumnAddress);
            isTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            isTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            isTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            workbook.Worksheets.Worksheet(sheet.Name).Row(1).Style.Font.Bold = true;
        }


        return workbook;
    }

    #endregion

    #region Related
    public static string GetCountryCodeFromContactNumber(string contact)
    {
        try
        {
            if (contact.Split('|')[0] != null)
                return contact.Split('|')[0];
        }
        catch { }
        return "";
    }
    public static string GetAreaCodeFromContactNumber(string contact)
    {
        try
        {
            if (contact.Split('|')[1] != null)
                return contact.Split('|')[1];
        }
        catch { }
        return "";
    }
    public static string GetNumberFromContactNumber(string contact)
    {
        try
        {
            if (contact.Split('|')[2] != null)
                return contact.Split('|')[2];
        }
        catch { }
        return "";
    }
    public static string CreateContact(string cc, string area, string number)
    {
        return cc + "|" + area + "|" + number;
    }
    public static string ShowFriendlyContact(string contact)
    {
        string cc = "";
        string ac = "";
        string no = "";
        string[] parts = contact.Split('|');

        try { if (parts[0] != "") cc = parts[0] + "-"; }
        catch { }

        try { if (parts[1] != "") ac = parts[1] + "-"; }
        catch { }

        try { if (parts[2] != "") no = parts[2]; }
        catch { }




        return cc + ac + no;
    }


    public static string GetDateDepentent(Guid paymentGroupId, string Type = "")
    {
        string DateFormat = "dd MMMM yyyy";
        DateTime OnTimeCutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["OnTimeCutOff"].ToString());
        DateTime ExtendedCutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ExtendedCutOff"].ToString());
        DateTime Extended_2_CutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Extended_2_CutOff"].ToString());
        DateTime Extended_3_CutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Extended_3_CutOff"].ToString());

        Entry entry = EntryList.GetEntryList(paymentGroupId, Guid.Empty, "").FirstOrDefault();
        string DateDependent = entry.DateSubmitted.ToString(DateFormat);
        if (entry.DateSubmitted < OnTimeCutOff)
        {
            if (Type == "D_String")
                DateDependent = "D1";
            else
                DateDependent = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["DueDate1"].ToString()).ToString(DateFormat);
        }
        else if (entry.DateSubmitted < ExtendedCutOff)
        {
            if (Type == "D_String")
                DateDependent = "D2";
            else
                DateDependent = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["DueDate2"].ToString()).ToString(DateFormat);
        }
        else if (entry.DateSubmitted < Extended_2_CutOff)
        {
            if (Type == "D_String")
                DateDependent = "D3";
            else
                DateDependent = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["DueDate3"].ToString()).ToString(DateFormat);
        }
        else if (entry.DateSubmitted < Extended_3_CutOff)
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


    public static string GetAdhocDateDepentent(Guid paymentGroupId, string DateFormat = "dd MMMM yyyy")
    {
        #region Old Method
        //DateTime OnTimeCutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["OnTimeCutOff"].ToString());
        //DateTime ExtendedCutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ExtendedCutOff"].ToString());
        //DateTime Extended_2_CutOff = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Extended_2_CutOff"].ToString());
        //string DateDependent = "";
        //try {
        //    AdhocInvoiceItem adhocInvoiceItem = AdhocInvoiceItemList.GetAdhocInvoiceItemList(paymentGroupId, Guid.Empty).FirstOrDefault();
        //    Entry entry = Entry.GetEntry(adhocInvoiceItem.EntryId);
        //    DateDependent = entry.DateSubmitted.ToString(DateFormat);
        //    if (entry.DateSubmitted < OnTimeCutOff)
        //    {
        //        DateDependent = OnTimeCutOff.ToString(DateFormat);
        //    }
        //    else if (entry.DateSubmitted < ExtendedCutOff)
        //    {
        //        DateDependent = ExtendedCutOff.ToString(DateFormat);
        //    }
        //    else if (entry.DateSubmitted < Extended_2_CutOff)
        //    {
        //        DateDependent = Extended_2_CutOff.ToString(DateFormat);
        //    }
        //}
        //catch { }
        //return DateDependent;
        #endregion

        AdhocInvoiceItem adhocInvoiceItem = AdhocInvoiceItemList.GetAdhocInvoiceItemList(paymentGroupId, Guid.Empty).FirstOrDefault();
        AdhocInvoice adhocInvoice = AdhocInvoice.GetAdhocInvoice(adhocInvoiceItem.AdhocInvoiceId);

        string DateDependent = AddWorkdays(adhocInvoice.DateCreated, 3).ToString(DateFormat); ;

        return DateDependent;
        //return Convert.ToDateTime("23 Jan " + GeneralFunction.EffieEventYear() + "").ToString(DateFormat);
    }

    public static DateTime AddWorkdays(DateTime originalDate, int workDays)
    {
        DateTime tmpDate = originalDate;
        while (workDays > 0)
        {
            tmpDate = tmpDate.AddDays(1);
            if (tmpDate.DayOfWeek < DayOfWeek.Saturday &&
                tmpDate.DayOfWeek > DayOfWeek.Sunday &&
                !IsHoliday(tmpDate))
                workDays--;
        }
        return tmpDate;
    }

    public static bool IsHoliday(DateTime originalDate)
    {
        // INSERT YOUR HOlIDAY-CODE HERE!
        if (originalDate.ToString("yyyyMMdd") == "20180101")
            return true;

        return false;
    }

    public static string GetNewOrderNumber(Entry entry)
    {
        string prefix = "";

        // get the column Id 1=SM,PSC   2=SM,SC   3=MM,PSC    4=MM,SC
        int columnId = 0;
        if (entry.CategoryMarket == "SM")
        {
            if (entry.CategoryPS == "PSC") columnId = 1;
            if (entry.CategoryPS == "SC") columnId = 2;
        }

        if (entry.CategoryMarket == "MM")
        {
            if (entry.CategoryPS == "PSC") columnId = 3;
            if (entry.CategoryPS == "SC") columnId = 4;
        }


        // search the data with this categorypsdetail
        DataTable dt = Category.GetSubcategories("");
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Name"].ToString() == entry.CategoryPSDetail && int.Parse(dr["ColumnId"].ToString()) == columnId)
            {
                prefix = dr["Prefix"].ToString();
                break;
            }
        }

        string ordernumber = "";
        Setting setting = null; // Setting.GetSetting(new Guid());

        SettingList settingList = SettingList.GetSettingList(prefix);
        if (settingList.Count > 0) setting = settingList[0];

        ordernumber = (int.Parse(setting.OrderNo) + 1).ToString();
        setting.OrderNo = ordernumber;
        setting.Save();


        ordernumber = prefix + ordernumber.PadLeft(3, '0');
        return ordernumber;
    }
    public static string GetNewInvoiceNumber()
    {
        string invnumber = "";
        Setting setting = null; // Setting.GetSetting(new Guid());

        SettingList settingList = SettingList.GetSettingList("SP-AG"); // use this category for the invoice number
        if (settingList.Count > 0) setting = settingList[0];

        if (setting != null)
        {
            invnumber = (int.Parse(setting.InvoiceNo) + 1).ToString();
            setting.InvoiceNo = invnumber;
            setting.Save();

            string prefix = System.Configuration.ConfigurationManager.AppSettings["CodePrefixInvoice"];

            invnumber = prefix + invnumber;
        }
        return invnumber;
    }
    public static string GetNewInvoiceNumberGala()
    {
        string invnumber = "";
        Setting setting = null; // Setting.GetSetting(new Guid());

        SettingList settingList = SettingList.GetSettingList("GALA"); // use this category for the invoice number
        if (settingList.Count > 0) setting = settingList[0];

        if (setting != null)
        {
            invnumber = (int.Parse(setting.InvoiceNo) + 1).ToString();
            setting.InvoiceNo = invnumber;
            setting.Save();

            string prefix = System.Configuration.ConfigurationManager.AppSettings["CodePrefixInvoice"];

            invnumber = prefix + invnumber;
        }
        return invnumber;
    }
    public static string GetNewAdhocInvoiceNumber()
    {
        string invnumber = "";
        Setting setting = null; // Setting.GetSetting(new Guid());

        SettingList settingList = SettingList.GetSettingList("ADHOC"); // use this category for the invoice number
        if (settingList.Count > 0) setting = settingList[0];

        if (setting != null)
        {
            invnumber = (int.Parse(setting.InvoiceNo) + 1).ToString();
            setting.InvoiceNo = invnumber;
            setting.Save();

            string prefix = System.Configuration.ConfigurationManager.AppSettings["CodePrefixInvoice"];

            invnumber = prefix + invnumber;
        }
        return invnumber;
    }
    //public static string GetNewInvoiceNumber()
    //{
    //    string invnumber = "";
    //    Setting setting = Setting.GetSetting(new Guid());
    //    invnumber = (int.Parse(setting.InvoiceNo) + 1).ToString();
    //    setting.InvoiceNo = invnumber;
    //    setting.Save();

    //    string prefix = System.Configuration.ConfigurationManager.AppSettings["CodePrefixInvoice"];

    //    invnumber = prefix + invnumber;
    //    return invnumber;
    //}
    public static string GetHTMLTableFromAdhocPaymentGroup(Guid paymentGroupId)
    {
        string html = "";
        html += "<table width='600px'>";
        EntryList list = EntryList.GetEntryList(paymentGroupId, Guid.Empty, "");

        AdhocInvoiceItemList adInvoiceList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(paymentGroupId, Guid.Empty);

        if (adInvoiceList.Count > 0)
        {
            html += "<tr><th align='left'>Entry ID</th><th align='left'>Title</th><th align='left'>Description</th></tr>";
            foreach (AdhocInvoiceItem adhocItem in adInvoiceList)
            {
                try {
                    Entry entry = Entry.GetEntry(adhocItem.EntryId);
                    html += "<tr><td>" + entry.Serial + "</td><td>" + entry.Campaign + "</td><td>" + (adhocItem.InvoiceType.Equals(AdhocInvoiceType.Custom) ? adhocItem.InvoiceTypeOthers : GeneralFunction.GetInvoiceType(adhocItem.InvoiceType)) + "</td></tr>";
                }
                catch { }
            }
        }
        html += "</table>";
        return html;
    }

    public static string GetHTMLTableFromPaymentGroup(Guid paymentGroupId)
    {
        string html = "";
        html += "<table width='400px'>";
        EntryList list = EntryList.GetEntryList(paymentGroupId, Guid.Empty, "");
        foreach (Entry entry in list)
        {
            html += "<tr><td>" + entry.Serial + "</td><td>" + GetEntryMarket(entry.CategoryMarket) + " / " + entry.CategoryPSDetail + " / " + entry.Campaign + "</td></tr>";
        }
        html += "</table>";
        return html;
    }

    public static string GetHTMLTableFromPaymentGroupEntryList(List<Entry> EntryList)
    {
        string html = "";
        html += "<table width='400px'>";
        foreach (Entry entry in EntryList)
        {
            html += "<tr><td>" + entry.Serial + "</td><td>" + GetEntryMarket(entry.CategoryMarket) + " / " + entry.CategoryPSDetail + " / " + entry.Campaign + "</td></tr>";
        }
        html += "</table>";
        return html;
    }

    public static string GetHTMLTableFromEntry(Entry entry)
    {
        string html = "";
        html += "<table width='400px'>";
        html += "<tr><td>" + entry.Serial + "</td><td>" + GetEntryMarket(entry.CategoryMarket) + " / " + entry.CategoryPSDetail + " / " + entry.Campaign + "</td></tr>";
        html += "</table>";
        return html;
    }
    public static List<string> GetEffectivenessItems()
    {
        List<string> list = new List<string>();
        list.Add("Australia");
        list.Add("Bangladesh");
        list.Add("Bhutan");
        list.Add("Cambodia");
        list.Add("China");
        list.Add("Cook Islands");
        list.Add("Fiji");
        list.Add("French Polynesia");
        list.Add("Hong Kong");
        list.Add("India");
        list.Add("Indonesia");
        list.Add("Japan");
        list.Add("Kazakhstan");
        list.Add("Kiribati");
        list.Add("Korea");
        list.Add("Laos");
        list.Add("Macau");
        list.Add("Maldives");
        list.Add("Malaysia");
        list.Add("Marshall Islands");
        list.Add("Micronesia");
        list.Add("Mongolia");
        list.Add("Myanmar");
        list.Add("Nauru");
        list.Add("Nepal");
        list.Add("New Caledonia");
        list.Add("New Zealand");
        list.Add("Niue");
        list.Add("Pakistan");
        list.Add("Palau");
        list.Add("Philippines");
        list.Add("Samoa");
        list.Add("Singapore");
        list.Add("Solomon Islands");
        list.Add("Sri Lanka");
        list.Add("Taiwan");
        list.Add("Thailand");
        list.Add("Timor-Leste");
        list.Add("Tonga");
        list.Add("Tuvalu");
        list.Add("Uzbekistan");
        list.Add("Vanautu");
        list.Add("Vietnam");
        list.Add("Afghanistan");
        list.Add("Brunei Darussalam");
        list.Add("Kyrgyzstan");
        list.Add("Tajikistan");
        list.Add("Turkmenistan");

        list = list.OrderBy(name => name).ToList();

        return list;

    }
    public static List<string> GetMarketExperienceItems()
    {
        List<string> list = new List<string>();
        list.Add("Global");
        list.Add("APAC");
        list.Add("MENA");
        list.Add("Europe");
        list.Add("North America");
        list.Add("South America");
        list.Add("Australia");
        list.Add("Bangladesh");
        list.Add("Cambodia");
        list.Add("China");
        list.Add("Hong Kong");
        list.Add("India");
        list.Add("Indonesia");
        list.Add("Japan");
        list.Add("Korea");
        list.Add("Malaysia");
        list.Add("Myanmar");
        list.Add("New Zealand");
        list.Add("Pakistan");
        list.Add("Philippines");
        list.Add("Singapore");
        list.Add("Taiwan");
        list.Add("Thailand");
        list.Add("Vietnam");
        return list;

    }

    public static List<string> GetOtherEffieExperienceItems()
    {
        List<string> list = new List<string>();
        list.Add("Global");
        list.Add("MENA");
        list.Add("Euro");
        list.Add("North America");
        list.Add("Latin America");
        list.Add("Australia");
        list.Add("China");
        list.Add("Hong Kong");
        list.Add("India");
        list.Add("Korea");
        list.Add("Malaysia");
        //list.Add("Myanmar");
        list.Add("New Zealand");
        list.Add("Singapore");
        list.Add("Sri Lanka");
        return list;

    }

    public static List<string> GetEffieExperienceYears()
    {
        List<string> list = new List<string>();
        list.Add("2014");
        list.Add("2015");
        list.Add("2016");
        list.Add("2017");
        list.Add("2018");
        list.Add("2019");
        list.Add("2020");
        return list;

    }


    public static string CheckPlaceHolders(string body, bool isSave)
    {
        string emailBody = body;

        string round1 = "<table  border='0' cellspacing='0' cellpadding='0' id='emailBodyR1' style='font-size: 15px;text-align: left; color: #ffffff;float:left;margin-right: 1%;'><tr><td width='38%' align='center' style='padding: 5px; background-color: #B5985A'><a href='#LINKAPPROVER1#' style='font-family: Arial, Helvetica, sans-serif; color: black;text-decoration: none; display: block; font-weight: bold'>YES<br />I would like to participate in Round 1 Judging.</a></td></tr></table>";
        string round2 = "<table  border='0' cellspacing='0' cellpadding='0' id='emailBodyR2' style='font-size: 15px;text-align: left; color: #ffffff;float:left;margin-right: 1%;'><tr><td width='38%' align='center' style='padding: 5px; background-color: #B5985A'><a href='#LINKAPPROVER2#' style='font-family: Arial, Helvetica, sans-serif; color: black;text-decoration: none; display: block; font-weight: bold'>YES<br />I would like to participate in Round 2 Judging.</a></td></tr></table>";
        string bothrounds = "<table  border='0' cellspacing='0' cellpadding='0' id='emailBodyR1R2' style='font-size: 15px;text-align: left; color: #ffffff;float:left;margin-right: 1%;'><tr><td width='38%' align='center' style='padding: 5px; background-color: #B5985A'><a href='#LINKAPPROVE#' style='font-family: Arial, Helvetica, sans-serif; color: black;text-decoration: none; display: block; font-weight: bold'>YES<br />I would like to participate in both Rounds.</a></td></tr></table>";
        string reject = "<table border='0' cellspacing='0' cellpadding='0' id='emailBodyNo' style='font-size: 15px;text-align: left; color: #ffffff;float:left;margin-right: 1%;'><tr><td width='38%' align='center' style='padding: 5px; background-color: #B3ACA9'><a href='#LINKREJECT#' style='font-family: Arial, Helvetica, sans-serif; color: black;text-decoration: none; display: block; font-weight: bold'>NO<br />I am unable to participate in Judging.</a></td></tr></table>";


        if (isSave)
        {
            emailBody = emailBody.Replace("#ROUND1#", round1);
            emailBody = emailBody.Replace("#ROUND2#", round2);
            emailBody = emailBody.Replace("#BOTHROUNDS#", bothrounds);
            emailBody = emailBody.Replace("#REJECT#", reject);
        }
        else
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(body);
            HtmlNodeCollection col = doc.DocumentNode.SelectNodes("//table");

            foreach (HtmlNode node in col)
            {
                if (node.Id.Equals("emailBodyR1"))
                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode("#ROUND1#"), node);
                if (node.Id.Equals("emailBodyR2"))
                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode("#ROUND2#"), node);
                if (node.Id.Equals("emailBodyR1R2"))
                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode("#BOTHROUNDS#"), node);
                if (node.Id.Equals("emailBodyNo"))
                    node.ParentNode.ReplaceChild(HtmlNode.CreateNode("#REJECT#"), node);
            }

            emailBody = doc.DocumentNode.OuterHtml;
        }

        return emailBody;
    }

    public static List<string> GetSDGItems()
    {
        List<string> list = new List<string>();
        //list.Add("Affordable and Clean Energy");
        //list.Add("Clean Water and Sanitation");
        //list.Add("Climate Action");
        //list.Add("Decent Work and Economic Growth");
        //list.Add("Gender Equality");
        //list.Add("Good Health and Well-Being");
        //list.Add("Industry, Innovation and Infrastructure");
        //list.Add("Life Below Water");
        //list.Add("Life on Land");
        //list.Add("No Poverty");        
        //list.Add("Partnerships for the Goals");
        //list.Add("Peace, Justice and Strong Institutions");
        //list.Add("Quality Education");
        //list.Add("Reduced Inequalities");
        //list.Add("Responsible Consumption and Production");
        //list.Add("Sustainable Cities and Communities");
        //list.Add("Zero Hunger");
        //list.Add("Not Applicable");

        list.Add("Affordable and Clean Energy");
        list.Add("Clean Water and Sanitation");
        list.Add("Climate Action");
        list.Add("Decent Work and Economic Growth");
        list.Add("Gender Equality");
        list.Add("Good Health and Well-Being");
        list.Add("Industry, Innovation and Infrastructure");
        list.Add("Life Below Water");
        list.Add("Life on Land");
        list.Add("No Poverty");
        list.Add("Partnerships for the Goals");
        list.Add("Peace, Justice and Strong Institutions");
        list.Add("Quality Education");
        list.Add("Reduced Inequalities");
        list.Add("Responsible Consumption and Production");
        list.Add("Sustainable Cities and Communities");
        list.Add("Zero Hunger");
        list.Add("Not applicable");


        return list;

    }

    public static string ConvertToCategoryPSDetailFull(string market, string categoryPS, string categoryPSDetail)
    {
        return market.Substring(0, 1) + categoryPS.Substring(0, 1) + "-" + categoryPSDetail;
    }
    public static void ConvertToCategoryPSDetail(string categoryPSDetailFull, ref string market, ref string categoryPS, ref string categoryPSDetail)
    {
        if (categoryPSDetailFull.Length > 2)
        {
            string code = categoryPSDetailFull.Substring(0, 2);
            switch (code)
            {
                case "SP":
                    market = "SM";
                    categoryPS = "PSC";
                    break;
                case "SS":
                    market = "SM";
                    categoryPS = "SC";
                    break;
                case "MP":
                    market = "MM";
                    categoryPS = "PSC";
                    break;
                case "MS":
                    market = "MM";
                    categoryPS = "SC";
                    break;
            }
            //categoryPSDetail = categoryPSDetailFull.Split('-')[1]; // e.g. SP-Automotive

            categoryPSDetail = categoryPSDetailFull.Substring(3, categoryPSDetailFull.Length - 3); // strip off the first 3 characters
        }
    }
    public static DataTable GetAllCategoryCache(bool needRefresh)
    {
        DataTable dt = (DataTable)HttpContext.Current.Session["Effie.CategoryList"];
        if (dt == null || needRefresh)
        {
            dt = Category.GetSubcategories("");
            HttpContext.Current.Session["Effie.CategoryList"] = dt;
        }
        return dt;
    }
    public static bool IsCategoryInCategoryGroup(string groupcode, string category)
    {
        DataTable dt = GetAllCategoryCache(false);
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["ColumnId"].ToString() == groupcode && dr["Name"].ToString() == category)
            {
                return true;
            }
        }

        return false;
    }

    public static List<string> GetFilteredCountryList(bool needRefresh)
    {
        List<string> countryList = (List<string>)HttpContext.Current.Session["Effie.FilteredCountryList"];


        if (countryList == null || needRefresh)
        {
            countryList = new List<string>();

            RegistrationList regs = GetAllRegistrationCache(false);
            foreach (Registration reg in regs)
            {
                if (!countryList.Contains(reg.Country) && reg.Country != "") countryList.Add(reg.Country);
            }
            countryList.Sort((x, y) => string.Compare(x.ToString(), y.ToString()));
            HttpContext.Current.Session["Effie.FilteredCountryList"] = countryList;
        }

        if (countryList == null) countryList = new List<string>();

        return countryList;
    }
    public static List<string> GetFilteredCountryListFromJury(bool needRefresh, string rounds)
    {
        List<string> countryList = (List<string>)HttpContext.Current.Session["Effie.JuryFilteredCountryList"];


        if (countryList == null || needRefresh)
        {
            countryList = new List<string>();

            List<EffieJuryManagementApp.Jury> juryList = GetAllJuryCache(false, rounds);
            foreach (EffieJuryManagementApp.Jury jury in juryList)
            {
                if (!countryList.Contains(jury.Country) && jury.Country != "") countryList.Add(jury.Country);
            }
            countryList.Sort((x, y) => string.Compare(x.ToString(), y.ToString()));
            HttpContext.Current.Session["Effie.JuryFilteredCountryList"] = countryList;
        }

        if (countryList == null) countryList = new List<string>();

        return countryList;
    }

    public static List<Guid> GetFilteredEntryListFromRegCountry(string country, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredEntryListFromRegCountry"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            RegistrationList regs = GetAllRegistrationCache(false);
            foreach (Registration reg in regs)
            {
                if (reg.Country.Trim().ToUpper() == country.Trim().ToUpper())
                {
                    // Get all Entry belonging to this reg Id
                    EntryList entrys = EntryList.GetEntryList(Guid.Empty, reg.Id, "");
                    foreach (Entry entry in entrys)
                        list.Add(entry.Id);
                }
            }
            HttpContext.Current.Session["Effie.FilteredEntryListFromRegCountry"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredEntryListFromRegCompany(string company, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredEntryListFromRegCompany"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            RegistrationList regs = GetAllRegistrationCache(false);
            foreach (Registration reg in regs)
            {
                if (reg.Company.Trim().ToUpper().IndexOf(company.Trim().ToUpper()) != -1)
                {
                    // Get all Entry belonging to this reg Id
                    EntryList entrys = EntryList.GetEntryList(Guid.Empty, reg.Id, "");
                    foreach (Entry entry in entrys)
                        list.Add(entry.Id);
                }
            }
            HttpContext.Current.Session["Effie.FilteredEntryListFromRegCompany"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredEntryListFromRegFirstName(string firstName, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredEntryListFromRegFirstName"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            var regs = GetAllRegistrationCache(false).ToList();

            foreach (Registration reg in regs)
            {
                if (reg.Firstname.Trim().ToUpper().IndexOf(firstName.Trim().ToUpper()) != -1)
                {
                    // Get all Entry belonging to this reg Id
                    EntryList entrys = EntryList.GetEntryList(Guid.Empty, reg.Id, "");
                    foreach (Entry entry in entrys)
                        list.Add(entry.Id);
                }
            }
            HttpContext.Current.Session["Effie.FilteredEntryListFromRegFirstName"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredEntryListFromRegLastName(string lastName, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredEntryListFromRegLastName"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            RegistrationList regs = GetAllRegistrationCache(false);
            foreach (Registration reg in regs)
            {
                if (reg.Lastname.Trim().ToUpper().IndexOf(lastName.Trim().ToUpper()) != -1)
                {
                    // Get all Entry belonging to this reg Id
                    EntryList entrys = EntryList.GetEntryList(Guid.Empty, reg.Id, "");
                    foreach (Entry entry in entrys)
                        list.Add(entry.Id);
                }
            }
            HttpContext.Current.Session["Effie.FilteredEntryListFromRegLastName"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredEntryListFromClientCC(string client, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredEntryListFromClientCC"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            EntryList entryList = GetAllEntryCache(false);
            foreach (Entry entry in entryList)
            {
                CompanyCreditList ccList = GetAllCCListCache(false);
                foreach (CompanyCredit cc in ccList)
                {
                    if (cc.EntryId == entry.Id && cc.No == 1 && cc.Company.Trim().ToUpper().IndexOf(client.Trim().ToUpper()) != -1)
                    {
                        // Get all CC with no = 1 (client type) that matc
                        list.Add(entry.Id);
                    }
                }
            }

            HttpContext.Current.Session["Effie.FilteredEntryListFromClientCC"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredEntryListFromAgencyCC(string agency, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredEntryListFromAgencyCC"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            EntryList entryList = GetAllEntryCache(false);
            foreach (Entry entry in entryList)
            {
                CompanyCreditList ccList = GetAllCCListCache(false);
                foreach (CompanyCredit cc in ccList)
                {
                    if (cc.EntryId == entry.Id && cc.No != 1 && cc.Company.Trim().ToUpper().IndexOf(agency.Trim().ToUpper()) != -1)
                    {
                        // Get all CC with no != 1 (rest all agency) that matc
                        list.Add(entry.Id);
                    }
                }
            }

            HttpContext.Current.Session["Effie.FilteredEntryListFromAgencyCC"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredEntryListFromJuryAssignment(string juryname, string round, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredEntryListFromJury"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            EntryList entries = GetAllEntryCache(false);
            foreach (Entry entry in entries)
            {
                if (entry.CategoryMarketFromRound(round).Trim() != "" && entry.CategoryPSFromRound(round).Trim() != "" && entry.CategoryPSDetailFromRound(round).Trim() != "")
                {
                    List<EffieJuryManagementApp.Jury> juryList = GetJuryListFromEntryCategory(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);
                    foreach (EffieJuryManagementApp.Jury jury in juryList)
                    {
                        if ((jury.FirstName.IndexOf(juryname) != -1 || jury.LastName.IndexOf(juryname) != -1))
                        {
                            list.Add(entry.Id);
                        }
                    }
                }
            }
            HttpContext.Current.Session["Effie.FilteredEntryListFromJury"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredEntryListFromJuryPanel(string panelId, string round, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredEntryListFromJuryPanel"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            EntryList entries = GetAllEntryCache(false);
            foreach (Entry entry in entries)
            {
                if (entry.CategoryMarket.Trim() != "" && entry.CategoryPS.Trim() != "" && entry.CategoryPSDetail.Trim() != "")
                {

                    List<JuryPanelCategory> jcpList = GetJuryPanelCategoryFromCategoryPS(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);
                    if (jcpList.Count == 1 && jcpList[0].PanelId == panelId)
                    {
                        list.Add(entry.Id);
                    }
                }
            }
            HttpContext.Current.Session["Effie.FilteredEntryListFromJuryPanel"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }



    public static void ResetCCCache()
    {
        HttpContext.Current.Session.Remove("Effie.CCList");
    }
    public static CompanyCreditList GetAllCCListCache(bool needRefresh)
    {
        if (HttpContext.Current.Session["Effie.AllCCList"] == null || needRefresh)
        {
            CompanyCreditList cclist = CompanyCreditList.GetCompanyCreditList(Guid.Empty);
            HttpContext.Current.Session["Effie.AllCCList"] = cclist;
        }
        return (CompanyCreditList)HttpContext.Current.Session["Effie.AllCCList"];
    }
    public static List<CompanyCredit> GetCCListCache()
    {
        return (List<CompanyCredit>)HttpContext.Current.Session["Effie.CCList"];
    }
    public static void AddCCtoListCache(CompanyCredit cc)
    {
        List<CompanyCredit> existinglist = GetCCListCache();

        if (existinglist == null) existinglist = new List<CompanyCredit>();
        existinglist.Add(cc);
        HttpContext.Current.Session["Effie.CCList"] = existinglist;

    }
    public static void DeleteCCFromListCache(Guid id)
    {
        List<CompanyCredit> existinglist = GetCCListCache();

        if (existinglist != null)
        {
            int index = -1;
            foreach (CompanyCredit cc in existinglist)
            {
                index++;
                if (cc.Id == id) break;
            }
            existinglist.RemoveAt(index);
            HttpContext.Current.Session["Effie.CCList"] = existinglist;
        }
    }
    public static void UpdateCCtoListCache(CompanyCredit cc)
    {
        List<CompanyCredit> existinglist = GetCCListCache();

        if (existinglist != null)
        {
            int index = -1;
            int counter = 0;
            foreach (CompanyCredit item in existinglist)
            {
                if (item.Id == cc.Id)
                {
                    index = counter;
                }
                counter++;
            }

            if (index != -1)
            {
                SetDroppedCC(existinglist[index]); // store the dropped cc for later
                existinglist.RemoveAt(index);
                existinglist.Insert(index, cc);
                HttpContext.Current.Session["Effie.CCList"] = existinglist;
            }
        }
    }
    public static void ReplaceKeyCCtoListCache2(CompanyCredit cc)
    {
        // use only for CLient and Lead Agency types
        // other types use UpdateCCtoListCache

        List<CompanyCredit> existinglist = GetCCListCache();

        if (existinglist != null)
        {
            int index = -1;
            int counter = 0;
            foreach (CompanyCredit item in existinglist)
            {
                if (item.Id == cc.Id)
                {
                    index = counter;
                }
                counter++;
            }

            if (index != -1)
            {
                SetDroppedCC(existinglist[index]); // store the dropped cc for later
                existinglist.RemoveAt(index);
                existinglist.Insert(index, cc);
                HttpContext.Current.Session["Effie.CCList"] = existinglist;
            }
        }
    }
    public static bool IsCCInListCache(CompanyCredit cc)
    {
        List<CompanyCredit> existinglist = GetCCListCache();

        if (existinglist != null)
        {
            foreach (CompanyCredit item in existinglist)
            {
                if (item.Id == cc.Id) return true;
            }
        }
        return false;
    }
    public static CompanyCredit GetCCFromCache(Guid Id)
    {
        List<CompanyCredit> existinglist = GetCCListCache();
        if (existinglist != null)
        {
            foreach (CompanyCredit item in existinglist)
            {
                if (item.Id == Id)
                {
                    return item;
                }
            }
        }
        return null;
    }
    public static List<CompanyCredit> GetCompanyListFromCCCache()
    {
        List<CompanyCredit> clist = new List<CompanyCredit>();

        List<CompanyCredit> existinglist = GetCCListCache();
        if (existinglist != null)
        {
            foreach (CompanyCredit cc in existinglist)
            {
                clist.Add(cc);
            }
        }
        return clist;
    }
    public static void SetDroppedCC(CompanyCredit cc)
    {
        HttpContext.Current.Session["Effie.DropCC"] = cc;
    }
    public static CompanyCredit GetDroppedCC()
    {
        return (CompanyCredit)HttpContext.Current.Session["Effie.DropCC"];
    }
    public static List<IndCredit> UpdateICFromCCCache(Repeater rptIndCredits)
    {
        // Get the list of company names from the CC List
        List<CompanyCredit> list = GetCompanyListFromCCCache();
        List<CompanyCredit> flist = new List<CompanyCredit>();
        foreach (CompanyCredit companyCC in list)
        {
            if (!String.IsNullOrEmpty(companyCC.Company))
                flist.Add(companyCC);
        }

        CompanyCredit emptyCC = CompanyCredit.NewCompanyCredit();
        emptyCC.Company = "";
        emptyCC.Id = Guid.Empty;
        flist.Insert(0, emptyCC);

        // Updating and Deleting of ICs
        foreach (RepeaterItem item in rptIndCredits.Items)
        {
            TextBox txtICName = (TextBox)item.FindControl("txtICName");
            TextBox txtICTitle = (TextBox)item.FindControl("txtICTitle");
            TextBox txtICEmail = (TextBox)item.FindControl("txtICEmail");
            DropDownList ddlICCompany = (DropDownList)item.FindControl("ddlICCompany");

            string selectedCompany = ddlICCompany.SelectedValue;

            ddlICCompany.Items.Clear();

            try
            {
                // to remove deleted companycredit from individualcredit items
                ddlICCompany.DataSource = flist;
                ddlICCompany.DataValueField = "Id";
                ddlICCompany.DataTextField = "Company";
                ddlICCompany.DataBind();

                if (ddlICCompany.Items.Cast<ListItem>().Where(m => m.Value == selectedCompany).Count() > 0)
                    ddlICCompany.SelectedValue = selectedCompany;
            }
            catch
            {
                // clear the remaining line of textboxes = Deleting from IC
                txtICName.Text = txtICTitle.Text = txtICEmail.Text = string.Empty;
                ddlICCompany.SelectedValue = Guid.Empty.ToString();
            }
        }

        SetDroppedCC(null);

        // Adding of ICs
        // check all the 1- 10 if this company has been selected in the ddl
        foreach (CompanyCredit cc in GetCCListCache())
        {
            bool isExist = false;

            foreach (RepeaterItem item in rptIndCredits.Items)
            {
                TextBox txtICName = (TextBox)item.FindControl("txtICName");
                TextBox txtICTitle = (TextBox)item.FindControl("txtICTitle");
                TextBox txtICEmail = (TextBox)item.FindControl("txtICEmail");
                DropDownList ddlICCompany = (DropDownList)item.FindControl("ddlICCompany");

                Guid indCCId = Guid.Empty;
                Guid.TryParse(ddlICCompany.SelectedValue, out indCCId);

                if (cc.Id == indCCId)
                {
                    isExist = true;
                    break;
                }
            }

            // if not selected by any of the 10 ddls, then we add new for the next empty row
            if (!isExist)
            {
                foreach (RepeaterItem item in rptIndCredits.Items)
                {
                    TextBox txtICName = (TextBox)item.FindControl("txtICName");
                    TextBox txtICTitle = (TextBox)item.FindControl("txtICTitle");
                    TextBox txtICEmail = (TextBox)item.FindControl("txtICEmail");
                    DropDownList ddlICCompany = (DropDownList)item.FindControl("ddlICCompany");

                    Guid indCCId = Guid.Empty;
                    Guid.TryParse(ddlICCompany.SelectedValue, out indCCId);

                    if (String.IsNullOrEmpty(txtICName.Text.Trim()) && String.IsNullOrEmpty(txtICTitle.Text.Trim()) && String.IsNullOrEmpty(txtICEmail.Text.Trim()) && indCCId == Guid.Empty)
                    {
                        // ok this row is empty
                        if (ddlICCompany.Items.Cast<ListItem>().Where(m => m.Value == cc.Id.ToString()).Count() > 0)
                            ddlICCompany.SelectedValue = cc.Id.ToString();

                        txtICName.Text = cc.Fullname;
                        txtICTitle.Text = cc.Job;
                        txtICEmail.Text = cc.Email;

                        break;
                    }
                }
            }
        }

        return OrderIC(rptIndCredits);
    }
    public static List<IndCredit> OrderIC(Repeater rptIndCredits)
    {
        // Order the IC List according to the company order in the CC List        
        List<IndCredit> sortICList = new List<IndCredit>();

        foreach (RepeaterItem item in rptIndCredits.Items)
        {
            TextBox txtICName = (TextBox)item.FindControl("txtICName");
            TextBox txtICTitle = (TextBox)item.FindControl("txtICTitle");
            TextBox txtICEmail = (TextBox)item.FindControl("txtICEmail");
            DropDownList ddlICCompany = (DropDownList)item.FindControl("ddlICCompany");

            Guid indCCId = Guid.Empty;
            Guid.TryParse(ddlICCompany.SelectedValue, out indCCId);

            CompanyCredit selectedCompanyCredit = null;

            try
            {
                selectedCompanyCredit = GetCCListCache().Where(m => m.Id == indCCId).Single();
            }
            catch { }

            if (selectedCompanyCredit != null)
            {
                IndCredit ic = IndCredit.NewIndCredit();
                ic.Company = ddlICCompany.SelectedValue;
                ic.ContactName = txtICName.Text.Trim();
                ic.Title = txtICTitle.Text.Trim();
                ic.Email = txtICEmail.Text.Trim();
                ic.UserData1 = selectedCompanyCredit.Id.ToString();

                sortICList.Add(ic);
            }
            else
            {
                IndCredit ic = IndCredit.NewIndCredit();
                ic.Company = ddlICCompany.SelectedValue;
                ic.ContactName = txtICName.Text.Trim();
                ic.Title = txtICTitle.Text.Trim();
                ic.Email = txtICEmail.Text.Trim();
                ic.UserData1 = Guid.Empty.ToString();

                sortICList.Add(ic);
            }
        }

        return sortICList;
    }
    public static int NumberOfLeadInListCache()
    {
        int count = 0;
        List<CompanyCredit> list = GetCCListCache();
        foreach (CompanyCredit c in list)
        {
            if (c.ContactType.IndexOf("Lead Agency") != -1) count++;
        }
        return count;
    }
    public static int NumberOfClientInListCache()
    {
        int count = 0;
        List<CompanyCredit> list = GetCCListCache();
        foreach (CompanyCredit c in list)
        {
            if (c.ContactType.IndexOf("Client") != -1) count++;
        }
        return count;
    }
    public static int NumberOfContributingCompanyInListCache()
    {
        int count = 0;
        List<CompanyCredit> list = GetCCListCache();
        foreach (CompanyCredit c in list)
        {
            if (c.ContactType.IndexOf("Contributing Agency") != -1) count++;
        }
        return count;
    }
    public static int NumberOfCCInListCache()
    {
        int count = 0;
        List<CompanyCredit> list = GetCCListCache();
        if (list != null) return list.Count;
        return count;
    }
    public static void OrderCCListCache()
    {
        List<CompanyCredit> existinglist = GetCCListCache();
        List<CompanyCredit> sortedlist = new List<CompanyCredit>();
        if (existinglist == null) existinglist = new List<CompanyCredit>();
        int counter = 1;

        // get the client
        foreach (CompanyCredit cc in existinglist)
        {
            if (cc.ContactType == "Client")
            {
                cc.No = counter;
                sortedlist.Add(cc);
                counter++;
            }
        }


        // get the lead agencies
        foreach (CompanyCredit cc in existinglist)
        {
            if (cc.ContactType == "Lead Agency")
            {
                cc.No = counter;
                sortedlist.Add(cc);
                counter++;
            }
        }

        // get the contributing
        foreach (CompanyCredit cc in existinglist)
        {
            if (cc.ContactType == "Contributing Agency")
            {
                cc.No = counter;
                sortedlist.Add(cc);
                counter++;
            }
        }

        HttpContext.Current.Session["Effie.CCList"] = sortedlist;
    }

    public static void ResetReportDataCache()
    {
        HttpContext.Current.Session.Remove("Effie.ReportData");
    }
    public static void SetReportDataCache(object data)
    {
        HttpContext.Current.Session["Effie.ReportData"] = data;
    }
    public static object GetReportDataCache()
    {
        return HttpContext.Current.Session["Effie.ReportData"];
    }

    public static bool IsRegistrationInList(List<Registration> list, Registration reg)
    {
        foreach (Registration r in list)
            if (r.Id == reg.Id) return true;
        return false;
    }
    public static Registration GetRegistrationFromEntry(Entry entry)
    {
        RegistrationList list = GetAllRegistrationCache(false);
        foreach (Registration reg in list)
        {
            if (reg.Id == entry.RegistrationId) return reg;
        }

        return null;
    }
    public static Registration GetDummyRegistrationForAdminSpoof()
    {
        Registration reg = Registration.NewRegistration();
        reg.Firstname = "Super";
        reg.Lastname = "Admin";
        reg.Status = StatusRegistration.Admin;
        return reg;
    }
    public static RegistrationList GetAllRegistrationCache(bool needRefresh)
    {
        RegistrationList list = (RegistrationList)HttpContext.Current.Session["Effie.RegistrationList"];
        if (list == null || needRefresh)
        {
            list = RegistrationList.GetRegistrationList("", "", "");
            HttpContext.Current.Session["Effie.RegistrationList"] = list;
        }
        return list;
    }
    public static List<Registration> GetRegIdWithSubmittedEntries(string status)
    {
        // active and non active regs
        // status = ACT or INA
        RegistrationList list = GeneralFunction.GetAllRegistrationCache(false);
        EntryList elist = GeneralFunction.GetAllEntryCache(false);
        List<Registration> flist = new List<Registration>();

        foreach (Registration r in list)
        {
            bool haveEntry = false;
            foreach (Entry e in elist)
            {
                if (e.RegistrationId == r.Id)
                {
                    haveEntry = true;
                    break;
                }
            }

            if ((haveEntry && status == "ACT") || (!haveEntry && status == "INA"))
            {
                flist.Add(r);
            }
        }

        return flist;
    }
    public static List<Registration> GetRegIdWithEntryPayStatus(string status)
    {
        // get regs with entries of that status
        RegistrationList list = GeneralFunction.GetAllRegistrationCache(false);
        EntryList elist = GeneralFunction.GetAllEntryCache(false);
        List<Registration> flist = new List<Registration>();

        foreach (Registration r in list)
        {
            foreach (Entry e in elist)
            {
                if (e.RegistrationId == r.Id && e.PayStatus == status)
                    flist.Add(r);

                // not paid - > 2 status values, NOT or blank
                if (e.PayStatus == "" && status == StatusPaymentEntry.NotPaid)
                    flist.Add(r);

            }
        }
        return flist;
    }
    public static List<Registration> GetRegIdWithEntryStatus(string status)
    {
        // get regs with entries of that status
        RegistrationList list = GeneralFunction.GetAllRegistrationCache(false);
        EntryList elist = GeneralFunction.GetAllEntryCache(false);
        List<Registration> flist = new List<Registration>();

        foreach (Registration r in list)
        {
            foreach (Entry e in elist)
            {
                if (e.RegistrationId == r.Id && e.Status == status)
                    flist.Add(r);


            }
        }
        return flist;
    }

    public static EntryList GetAllEntryCache(bool needRefresh)
    {
        EntryList list = (EntryList)HttpContext.Current.Session["Effie.EntryList"];
        if (list == null || needRefresh)
        {
            list = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "");
            HttpContext.Current.Session["Effie.EntryList"] = list;
        }
        return list;
    }
    public static Entry GetEntryFromIDCache(Guid entryId)
    {
        EntryList list = GetAllEntryCache(true);
        foreach (Entry e in list)
            if (e.Id == entryId) return e;
        return null;
    }
    public static List<Entry> GetEntryListFromCategory(string categoryPSDetailFull, string round)
    {
        // first 2 chars are the market and categoryps
        if (categoryPSDetailFull == "") return new List<Entry>();

        string market = "";
        string categoryPS = "";
        string categoryPSDetail = "";
        ConvertToCategoryPSDetail(categoryPSDetailFull, ref market, ref categoryPS, ref categoryPSDetail);

        return GetEntryListFromCategory(market, categoryPS, categoryPSDetail, round);
    }
    public static List<Entry> GetEntryListFromCategory(string market, string categoryPS, string categoryPSDetail, string round)
    {
        // get all entries matching the categoryPSDetail + status must be completed (OK)
        EntryList list = GetAllEntryCache(false);
        return list.Where(p => p.CategoryMarketFromRound(round) == market &&
                                p.CategoryPSFromRound(round) == categoryPS &&
                                p.CategoryPSDetailFromRound(round) == categoryPSDetail &&
                                p.Status == StatusEntry.Completed &&
                                p.WithdrawnStatus == "" &&
                                (round != "2" || (round == "2" && p.IsRound2))
                                )
                    .OrderBy(p => p.Serial)
                    .ToList();
    }
    public static List<Entry> GetEntryListFromJuryPanel(EffieJuryManagementApp.Jury jury, string round)
    {
        // returns all possible entries that match the jury's panel's category assignment
        List<Entry> list = new List<Entry>();
        string jurypanelId = "";
        if (round == "1") jurypanelId = jury.Round1PanelId;
        if (round == "2") jurypanelId = jury.Round2PanelId;
        if (jurypanelId != "")
        {
            List<JuryPanelCategory> jpcList = GetJuryPanelCategoryFromPanelId(jurypanelId, round);
            foreach (JuryPanelCategory jpc in jpcList)
            {
                List<Entry> thislist = GetEntryListFromCategory(jpc.CategoryPSDetail, round);
                list.AddRange(thislist);
            }
        }
        return list;
    }
    public static List<Entry> GetEntryListValidFromJuryPanel(EffieJuryManagementApp.Jury jury, string round)
    {
        // returns only the non recused entries from the jury panel
        List<Entry> list = GetEntryListFromJuryPanel(jury, round);
        List<Entry> flist = new List<Entry>();
        foreach (Entry entry in list)
        {
            Score score = GetScoreFromMatchingEntryCache(entry, jury.Id, round);
            if (score != null)
            {
                if (!score.IsAdminRecuse && !score.IsRecuse) flist.Add(entry);
            }
            else
                // no corresponding score, hence it cant have been recused, so we add it in
                flist.Add(entry);
        }
        return flist;
    }
    public static int GetEntryCountValidFromJury(EffieJuryManagementApp.Jury jury, List<JuryPanelCategory> jpcList, List<Score> scores, string round)
    {
        List<Entry> flist = new List<Entry>();

        List<Entry> validEntryFromJuryPanel = (from jpc in jpcList
                                               from entry in GetEntryListFromCategory(jpc.CategoryPSDetail, round)
                                               select entry).ToList();

        foreach (Entry ent in validEntryFromJuryPanel)
        {
            Score score = null;

            try
            {
                score = scores.Where(m => m.EntryId.Equals(ent.Id)).Single();
            }
            catch { }

            if (score != null)
            {
                if (!score.IsAdminRecuse && !score.IsRecuse) flist.Add(ent);
            }
            else
                // no corresponding score, hence it cant have been recused, so we add it in
                flist.Add(ent);
        }


        return flist.Count();
    }

    public static int GetPendingEntryCountValidFromJury(EffieJuryManagementApp.Jury jury, List<JuryPanelCategory> jpcList, List<Score> scores, string round)
    {
        List<Entry> flist = new List<Entry>();

        List<Entry> validEntryFromJuryPanel = (from jpc in jpcList
                                               from entry in GetEntryListFromCategory(jpc.CategoryPSDetail, round)
                                               select entry).ToList();

        foreach (Entry ent in validEntryFromJuryPanel)
        {
            Score score = null;

            try
            {
                score = scores.Where(m => m.EntryId.Equals(ent.Id)).Single();
            }
            catch { }

            if (score != null)
            {
                if (!score.IsAdminRecuse && !score.IsRecuse && !score.IsSubmitted) flist.Add(ent);
            }
            else
                // no corresponding score, hence it cant have been recused, so we add it in
                flist.Add(ent);
        }


        return flist.Count();
    }

    public static void ResetGroupPaymentCache()
    {
        HttpContext.Current.Session.Remove("Effie.GPList");
    }
    public static void AddIdToGroupPaymentCache(Guid id)
    {
        List<Guid> existinglist = GetGroupPaymentListCache();

        if (existinglist == null) existinglist = new List<Guid>();
        existinglist.Add(id);
        HttpContext.Current.Session["Effie.GPList"] = existinglist;
    }
    public static List<Guid> GetGroupPaymentListCache()
    {
        return (List<Guid>)HttpContext.Current.Session["Effie.GPList"];
    }
    public static Guid GetPayGroupIdFromGroupListCache()
    {
        Guid id = Guid.Empty;
        List<Guid> list = GetGroupPaymentListCache();
        if (list != null)
        {
            Entry entry = Entry.GetEntry(list[0]);
            return entry.PayGroupId;
        }
        return id;
    }

    public static void AcknowledgeDelegateReg(Guid groupId)
    {
    }

    public static void AcknowledgeVisitorReg(Guid userId)
    {
    }


    public static Entry onEntryData(EntryPayment entryPayment, Entry entry)
    {
        try {
            entry.PayCompany = entryPayment.PayCompany;
            entry.PayAddress1 = entryPayment.PayAddress1;
            entry.PayAddress2 = entryPayment.PayAddress2;
            entry.PayCity = entryPayment.PayCity;
            entry.PayGroupId = entryPayment.PayGroupId;
            entry.PayPostal = entryPayment.PayPostal;
            entry.PayCountry = entryPayment.PayCountry;
            entry.PayFirstname = entryPayment.PayFirstname;
            entry.PayLastname = entryPayment.PayLastname;
            entry.PayContact = entryPayment.PayContact;
            entry.PaymentMethod = entryPayment.PaymentMethod;
            entry.Amount = entryPayment.Amount;
            entry.Fee = entryPayment.Fee;
            entry.Tax = entryPayment.Tax;
            entry.GrandAmount = entryPayment.GrandAmount;
            entry.Save();
        }
        catch { }

        return entry;
    }


    public static void CompleteNewEntrySubmissionPayPal(Guid payGroupId)
    {
        string ErrorLog = "";
        try {

            ErrorLog += "(Check payGroupId =>" + payGroupId + ")";
            // ok 
            Guid regId = Guid.Empty;

            List<EntryPayment> list = EntryPaymentList.GetEntryPaymentList().Where(x => x.PayGroupId == payGroupId).ToList();
            
            ErrorLog += "(List =>" + list + "+" + list.Count + ")";

            // validate if the entries already have an invoice
            //if (list.Count > 0 && list[0].Invoice != "") return;
            if (list.Count > 0 && list[0].AmountReceived != 0) return;

            bool isChange = false;
            if (list[0].Invoice != "")
                isChange = true;

            ErrorLog += "(Check Invoice =>" + list + "+" + list.Count + ")";

            string invoice = "";
            decimal totalamount = 0;

            ErrorLog += "(===============  LOOP  ================)";
            int count = 0;
            foreach (EntryPayment entryPayment in list)
            {
                Entry entry = Entry.GetEntry(entryPayment.EntryId);
                entry.Status = StatusEntry.UploadPending;
                entry.PayStatus = StatusPaymentEntry.Paid;

                if (!isChange)
                {
                    if (entry.Serial.Trim() == "") entry.Serial = GeneralFunction.GetNewOrderNumber(entry);

                    ErrorLog += "(Check Serial =>" + entry.Serial.Trim() + ")";

                    if (invoice == "") invoice = GeneralFunction.GetNewInvoiceNumber();

                    ErrorLog += "(Check invoice =>" + invoice + ")";
                }
                else
                {
                    invoice = entry.Invoice;
                    ErrorLog += "(Check Else invoice =>" + entry.Invoice + ")";
                }
                entry.Invoice = invoice;
                //entry.DateSubmittedString = DateTime.Now.ToString();
                entryPayment.Invoice = invoice;
                ErrorLog += "(Check SAVE)";
                regId = entry.RegistrationId;
                ErrorLog += "(Check Reg " + regId + ")";
                entry = onEntryData(entryPayment, entry);
                totalamount += GetEntryPrice(entry);
                ErrorLog += "(Check totalamount " + totalamount + ")";
                ErrorLog += "(Check Try Set to DB )";

                if (count == (list.Count - 1))
                {
                    entry.AmountReceived = totalamount;
                }
                entry.Save();
                entryPayment.Save();
                count++;
            }

            ErrorLog += "(===============  LOOP  ================)";

            // update again for the amount to the 1st entry
            ErrorLog += "(Check totalamount " + totalamount + " )";
            list[0].AmountReceived = totalamount;
            list[0].Save();
            ErrorLog += "(Check list[0].Save() )";

            // Save a new amount received
            AmountReceived amt = AmountReceived.NewAmountReceived();
            amt.Amount = totalamount;
            amt.DateReceivedString = DateTime.Now.ToString();
            amt.PaygroupId = payGroupId;
            amt.Invoice = invoice;
            amt.Remarks = "Received from Paypal";
            amt.IsSetPaid = true;
            amt.DateCreatedString = DateTime.Now.ToString();
            amt.Save();


            ErrorLog += "(Check AmountReceived )";

            //Send Email
            if (isChange)
                Email.SendCompletedPaymentEmailPayPalChange(Registration.GetRegistration(regId), payGroupId, invoice); // payment group id
            else
                Email.SendCompletedPaymentEmailPayPal(Registration.GetRegistration(regId), payGroupId, invoice); // payment group id


            ErrorLog += "(Check Email)";

            UpdateEntryLastSendPaidEmailDate(payGroupId);

            ErrorLog += "(Check UpdateEntryLastSendPaidEmailDate)";

        }
        catch
        {
            RawLog log = RawLog.NewRawLog();
            log.Type = 5; // Process payment error
            log.Data = "[" + ErrorLog + "]";
            log.DateString = DateTime.Now.ToString();

            if (log.IsValid) log.Save();
        }

    }

    public static void CompleteNewEntrySubmissionOthers(Guid payGroupId)
    {
        // ok 
        Guid regId = Guid.Empty;
        EntryList list = EntryList.GetEntryList(payGroupId, Guid.Empty, ""); // contains the pay group id
        string invoice = "";
        foreach (Entry entry in list)
        {
            if (entry.Status != StatusEntry.UploadCompleted && entry.Status != StatusEntry.Completed) entry.Status = StatusEntry.UploadPending;
            entry.PayStatus = StatusPaymentEntry.Paid;
            //entry.DateSubmittedString = DateTime.Now.ToString();
            entry.Save();
            regId = entry.RegistrationId;
            invoice = entry.Invoice;
        }

        //Send Email
        Email.SendCompletedPaymentEmailOthers(Registration.GetRegistration(regId), payGroupId, invoice); // payment group id
        UpdateEntryLastSendPaidEmailDate(payGroupId);
    }
    public static void CompletePendingPaymentEntrySubmission(Guid payGroupId, string payMethod, bool isEditMode)
    {
        Guid regId = Guid.Empty;
        EntryList list = EntryList.GetEntryList(payGroupId, Guid.Empty, ""); // contains the pay group id

        // validate if the entries already have an invoice
        if (list.Count > 0 && list[0].Invoice != "")
        {
            if (isEditMode)
            {
                if (payMethod == PaymentType.Cheque) Email.SendNewEntryPendingPaymentChangeToChequeEmail(Registration.GetRegistration(list[0].RegistrationId), payGroupId, list[0].Invoice); // payment group id
                if (payMethod == PaymentType.BankTransfer) Email.SendNewEntryPendingPaymentChangeToBankTransferEmail(Registration.GetRegistration(list[0].RegistrationId), payGroupId, list[0].Invoice); // payment group id
            }

            return;
        }


        string invoice = "";
        foreach (Entry entry in list)
        {
            // do not set to pending payment if its paypal
            if (payMethod != PaymentType.PayPal)
            {
                entry.Status = StatusEntry.PaymentPending;
                if (entry.Serial.Trim() == "") entry.Serial = GeneralFunction.GetNewOrderNumber(entry);
                if (invoice == "") invoice = GeneralFunction.GetNewInvoiceNumber();
                entry.Invoice = invoice;

            }
            entry.PayStatus = StatusPaymentEntry.NotPaid;
            entry.DateSubmittedString = DateTime.Now.ToString();

            entry.Save();
            regId = entry.RegistrationId;
        }

        //Send Change to email if editing
        //Send ack Email only if its not PP
        if (payMethod == PaymentType.Cheque) Email.SendNewEntryPendingPaymentChequeEmail(Registration.GetRegistration(regId), payGroupId, invoice); // payment group id
        if (payMethod == PaymentType.BankTransfer) Email.SendNewEntryPendingPaymentBankTransferEmail(Registration.GetRegistration(regId), payGroupId, invoice); // payment group id
    }

    public static void CompletePendingPaymentAdhoc(Guid regId, Guid payGroupId, string payMethod, string TotalFees = "", bool isConfirm = false)
    {
        AdhocInvoiceList adhocInvList = AdhocInvoiceList.GetAdhocInvoiceList(regId, payGroupId);
        
        Administrator admin = Security.GetAdminLoginSession();

        if (adhocInvList.Count > 0)
        {
            string invoice = "";

            adhocInvList[0].PayStatus = StatusPaymentEntry.NotPaid;

            if (String.IsNullOrEmpty(adhocInvList[0].Invoice))
            {
                if (payMethod != "")
                    adhocInvList[0].Invoice = GeneralFunction.GetNewAdhocInvoiceNumber();
                else
                    adhocInvList[0].Invoice = "";
            }

            invoice = adhocInvList[0].Invoice;

            adhocInvList[0].LastSendPaymentReminderEmailDateString = DateTime.Now.ToString();

            if (adhocInvList[0].IsValid)
                adhocInvList[0].Save();
            
            AdhocInvoiceItemList adhocInvItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(payGroupId, adhocInvList[0].Id);
            string InvoiceType = "";
            foreach (AdhocInvoiceItem adhocInvItem in adhocInvItemList)
            {
                if (String.IsNullOrEmpty(adhocInvItem.Invoice))
                {
                    adhocInvItem.Invoice = invoice;
                }
                
                InvoiceType = adhocInvItem.InvoiceType;
                adhocInvItem.LastSendPaymentReminderEmailDateString = DateTime.Now.ToString();

                adhocInvItem.Save();
            }
            
            if (isConfirm) //Main/AdhocInvoiceSummary.aspx -> From Email
            {
                { // (CONFIRMATION EMAIL AFTER ENTRANT SELECTS THE PAYMENT MODE)
                    //if (payMethod == PaymentType.PayPal) Email.SendPendingPaymentAdhocPaypalEmail(Registration.GetRegistration(regId), payGroupId, invoice, false); //MOVE TO ProcessCredit.aspx.cs
                    if (payMethod == PaymentType.Cheque) Email.SendPendingPaymentAdhocChequeEmail(Registration.GetRegistration(regId), payGroupId, invoice, false); // payment group id
                    if (payMethod == PaymentType.BankTransfer) Email.SendPendingPaymentAdhocBankTransferEmail(Registration.GetRegistration(regId), payGroupId, invoice, false); // payment group id
                }
            }
            else //Admin/AdhocInvoiceSummary.aspx
            {
                //if (payMethod == "") //EMAIL FOR ENTRANTS TO SELECT THEIR PAYMENT MODE 
                //{ 
                //    if (InvoiceType == AdhocInvoiceType.ReOpen) //REOPENING
                //        Email.SendAdhocReOpenPaymentEmail(Registration.GetRegistration(regId), payGroupId, invoice);
                //    else if (InvoiceType == AdhocInvoiceType.ChangeReq || InvoiceType == AdhocInvoiceType.Custom || InvoiceType == AdhocInvoiceType.ExtDeadLine) //OTHER Invoice Type
                //        Email.SendAdhocOtherRequestPaymentEmail(Registration.GetRegistration(regId), payGroupId, TotalFees);
                //}

                {
                    //if (payMethod == "") Email.SendAdhocPendingPaymentEmail(payGroupId, TotalFees); 
                    if (payMethod == PaymentType.PayPal) Email.SendPendingPaymentAdhocPaypalEmailConfirm(Registration.GetRegistration(regId), payGroupId, invoice, false); // payment group id
                    if (payMethod == PaymentType.Cheque) Email.SendPendingPaymentAdhocChequeEmailConfirm(Registration.GetRegistration(regId), payGroupId, invoice, false); // payment group id
                    if (payMethod == PaymentType.BankTransfer) Email.SendPendingPaymentAdhocBankTransferEmailConfirm(Registration.GetRegistration(regId), payGroupId, invoice, false); // payment group id
                }
            }
        }
    }

    public static void CompletePaymentAdhocPayPal(Guid payGroupId)
    {
        // ok 
        Guid regId = Guid.Empty;

        AdhocInvoiceList adhocInvList = AdhocInvoiceList.GetAdhocInvoiceList(Guid.Empty, payGroupId);

        decimal totalamount = 0;

        if (adhocInvList.Count > 0)
        {
            AdhocInvoiceItemList adInvoiceItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(payGroupId, adhocInvList[0].Id);

            adhocInvList[0].PayStatus = StatusPaymentEntry.Paid;
            regId = adhocInvList[0].RegistrationId;

            totalamount = adhocInvList[0].GrandAmount;

            // update again for the amount to the 1st entry
            adhocInvList[0].PaymentMethod = "PP";
            adhocInvList[0].AmountReceived = totalamount;
            adhocInvList[0].Save();

            foreach (AdhocInvoiceItem adhocInvItem in adInvoiceItemList)
            {
                adhocInvItem.LastSendPaidEmailDateString = DateTime.Now.ToString();
                adhocInvItem.Save();
            }
            
            // Save a new amount received
            AdhocInvoiceAmountReceived amt = AdhocInvoiceAmountReceived.NewAdhocInvoiceAmountReceived();
            amt.Amount = totalamount;
            amt.DateReceivedString = DateTime.Now.ToString();
            amt.PaygroupId = payGroupId;
            amt.Invoice = adInvoiceItemList[0].Invoice;
            amt.Remarks = "Received from Paypal";
            amt.IsSetPaid = true;
            amt.DateCreatedString = DateTime.Now.ToString();
            amt.Save();
        }

        //Send Email
        string InvoiceType = "";
        Registration reg = null;

        //try
        //{
            AdhocInvoiceItem adhocInvoiceItem = AdhocInvoiceItemList.GetAdhocInvoiceItemList(adhocInvList[0].PayGroupId, adhocInvList[0].Id)[0];
            reg = Registration.GetRegistration(adhocInvList[0].RegistrationId);
            InvoiceType = adhocInvoiceItem.InvoiceType;

            if (InvoiceType == AdhocInvoiceType.ReOpen)
                Email.SendAdhocReOpenPaymentEmailConfirm(reg, payGroupId, adhocInvList[0].Invoice);
            else if (InvoiceType == AdhocInvoiceType.ChangeReq || InvoiceType == AdhocInvoiceType.Custom || InvoiceType == AdhocInvoiceType.ExtDeadLine)
                Email.SendAdhocOtherRequestPaymentEmailConfirm(reg, payGroupId, adhocInvList[0].Invoice);
        //}
        //catch { }
    }

    public static void UpdateEntryLastSendPaidEmailDate(Guid payGroupId)
    {
        EntryList list = EntryList.GetEntryList(payGroupId, Guid.Empty, ""); // contains the pay group id
        foreach (Entry entry in list)
        {
            entry.LastSendPaidEmailDateString = DateTime.Now.ToString();
            entry.Save();
        }
    }

    public static string ValidateNewEntryAgainstPrevious(Guid entryId, Guid regId, string campaign, string categoryMarket, string categoryPS, string CategoryPSDetail, string brand)
    {
        /// Logic : for no draft only, if same campaign name
        /// SM or MM
        /// if products and services, cannot choose anymore if already choosen 1.
        /// if speciality, cannot choose the same one as previous

        string ret = "";
        if (campaign != "" && categoryPS != "" && CategoryPSDetail != "" && brand != "")
        {
            // Get all entries belonging to this registration user
            //EntryList entries = EntryList.GetEntryList(Guid.Empty, regId, "");

            // Get all entries
            EntryList entries = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "");

            //EntryList entries = EntryList.GetEntryList(Guid.Empty, new Guid("e5e7dc17-f4f6-4953-a1c3-416f05f74a1d"), "");

            var FEntries = entries.Where(x => x.Id != entryId && campaign.ToUpper().Trim() == x.Campaign.ToUpper().Trim() && brand.ToUpper().Trim() == x.Brand.ToUpper().Trim() && x.RegistrationId == regId).ToList();
            var SCentriesCount = FEntries.Where(x => x.CategoryMarket == categoryMarket && x.CategoryPS == "SC").Count();
            var PSCentriesCount = FEntries.Where(x => x.CategoryMarket == categoryMarket && x.CategoryPS == "PSC").Count();
            if (PSCentriesCount == 1 && SCentriesCount >= 3 && (categoryPS == "PSC" || categoryPS == "SC"))
            {
                ret = "You are not permitted to submit the same case into more than <span style=\"text-decoration:underline;\">three</span> Specialty Category and <span style=\"text-decoration:underline;\">one</span> Products & Services Category. ";
            }
            else if (SCentriesCount >= 4 && categoryPS == "SC")
            {
                ret = "You are not permitted to submit the same case into more than <span style=\"text-decoration:underline;\">four</span> Specialty Category.";
            }

            foreach (Entry preventry in entries)
            {
                if (entryId != preventry.Id /*&& preventry.Status != StatusEntry.Draft*/)
                {
                    if (campaign.ToUpper().Trim() == preventry.Campaign.ToUpper().Trim() && brand.ToUpper().Trim() == preventry.Brand.ToUpper().Trim())
                    {
                        //if (categoryMarket == preventry.CategoryMarket &&
                        //    preventry.CategoryPSDetail == "New Product or Service")
                        //        ret = "You are not permitted to submit a duplicate case in New Product or Service.";
                        /*else*/
                        if (categoryMarket == preventry.CategoryMarket &&
                   categoryPS == "PSC" &&
                   preventry.CategoryPS == "PSC")
                        {
                            if (preventry.RegistrationId == regId)
                            {
                                if (preventry.CategoryPSDetail == CategoryPSDetail)
                                    ret = "You are not permitted to submit a duplicate case.";
                                else
                                    ret = "You are not permitted to submit the same case into more than <span style=\"text-decoration:underline;\">one</span> Products & Services Category.";
                            }
                            else
                                ret = "This case has already been submitted by another entrant. Please check with your partners and agree to submit once only.";
                        }

                        else if (categoryMarket == preventry.CategoryMarket &&
                            categoryPS == preventry.CategoryPS &&
                            CategoryPSDetail == preventry.CategoryPSDetail)
                        {
                            ret = categoryMarket + "=" + preventry.CategoryMarket + "##" + categoryPS + "=" + preventry.CategoryPS + "##" + CategoryPSDetail + "=" + preventry.CategoryPSDetail;
                            if (preventry.RegistrationId == regId)
                            {
                                //if (preventry.CategoryPSDetail == CategoryPSDetail)
                                ret = "You are not permitted to submit a duplicate case.";
                                //else
                                //    ret = "You are not permitted to submit more than <span style=\"text-decoration:underline;\">one</span> case in the Specialty Category.";
                            }
                            else
                                ret = "This campaign has been submitted by another Entrant in the same category. Select another category to enter.";
                        }




                        //// allow SM and MM separately
                        //if (categoryMarket == preventry.CategoryMarket && 
                        //    preventry.CategoryPS == "PSC" &&                // prev is PSC
                        //    CategoryPSDetail == "New Product or Service")   // now is New Product or Service which means also its SC
                        //    ret = "You are not permitted to submit the same case in a Specialty Category - New Product or Service.";



                        //if (categoryMarket == preventry.CategoryMarket && 
                        //    preventry.CategoryPS == "SC" &&                 // prev is SC
                        //    preventry.CategoryPSDetail == "New Product or Service" && // and prev is a New Product or Service
                        //    categoryPS == "PSC")                            // now is PSC- now does matter which detail for PSC
                        //    ret = "You are not permitted to submit the same case in a Products & Services Category.";



                        //if (categoryMarket == preventry.CategoryMarket && 
                        //    preventry.CategoryPS == "PSC" &&
                        //    preventry.CategoryPS == categoryPS)             // both prev and now PSC - so not allowed
                        //    ret = "You are not permitted to submit the same case in another Products & Services Category.";



                        //if (CategoryPSDetail == preventry.CategoryPSDetail && 
                        //    categoryPS == preventry.CategoryPS && 
                        //    preventry.CategoryPS == "SC")
                        //    ret = "You are not permitted to submit the same case in a Specialty Category - New Product or Service.";

                        if (ret != "") break;
                    }
                }
            }
        }


        return ret;
    }

    public static decimal TotalAdhocGrandAmount(Guid payGroupId)
    {
        decimal grandtotal = 0;
        AdhocInvoiceList list = AdhocInvoiceList.GetAdhocInvoiceList(Guid.Empty, payGroupId); // contains the pay group id
        foreach (AdhocInvoice entry in list)
        {
            grandtotal += entry.GrandAmount;
        }
        return grandtotal;
    }
    public static decimal TotalEntryGrandAmount(Guid payGroupId)
    {
        decimal grandtotal = 0;
        EntryList list = EntryList.GetEntryList(payGroupId, Guid.Empty, ""); // contains the pay group id
        foreach (Entry entry in list)
        {
            grandtotal += entry.GrandAmount;
        }
        return grandtotal;
    }
    public static decimal CalculateGroupTotalPriceFromCache()
    {
        decimal totalprice = 0;
        List<Guid> list = GeneralFunction.GetGroupPaymentListCache();
        if (list == null) return -999999;

        // Get the entries
        foreach (Guid id in list)
        {
            Entry entry = Entry.GetEntry(id);
            totalprice += CalculateEntryPrice(entry, Security.GetLoginSessionUser());
        }
        return totalprice;
    }
    public static decimal CalculateEntryPrice(Entry entry, Registration reg)
    {
        decimal price = -999999;
        bool isPreferential = false;
        bool isSpecial = false;

        //if (reg.IsCAAAA || reg.IsAPEP) isPreferential = true;
        //if (reg.IsAFAA || reg.IsEProg) isSpecial = true;

        if (reg.IsCAAAA || reg.IsAPEP || reg.IsEProg) isPreferential = true;
        //if (reg.IsAFAA) isSpecial = true;

    // Preferential is cheaper so calculate the special first
    //if (isSpecial)
    //{
    //    if (IsOnTimeNow()) 
    //        price = 99999;
    //    else if (IsExtendedNow()) 
    //        price = 99999;
    //    else
    //        price = 99999;
    //}

    //if (isPreferential)
    //{
    //    if (IsOnTimeNow()) 
    //        price = 790;
    //    else if (IsExtendedNow()) 
    //        price = 890;
    //    else co        //        price = 990;

    //    return price;
    //}

    //if (reg.IsPromo1)
    //{
    //    if (IsOnTimeNow())
    //        price = 930;
    //    else if (IsExtendedNow())
    //        price = 930;
    //    else
    //        price = 1030;

    //    return price;
    //}
    
        // Normal
        if (IsOnTimeNow(entry.DateSubmitted))
            price = decimal.Parse(ConfigurationSettings.AppSettings["Price1"]);
        else if (IsExtendedNow(entry.DateSubmitted))
            price = decimal.Parse(ConfigurationSettings.AppSettings["Price2"]);
        else if (IsExtended_2_Now(entry.DateSubmitted))
            price = decimal.Parse(ConfigurationSettings.AppSettings["Price3"]);
        else if (IsExtended_3_Now(entry.DateSubmitted))
            price = decimal.Parse(ConfigurationSettings.AppSettings["Price4"]);
        else
            price = decimal.Parse(ConfigurationSettings.AppSettings["Price5"]);

        return price;
    }
    public static decimal CalculateCreditFees(decimal total)
    {
        decimal fee = decimal.Parse(ConfigurationSettings.AppSettings["PPFee"]);
        return fee * total;
    }
    public static decimal CalculateTax(decimal total)
    {
        return decimal.Parse(ConfigurationSettings.AppSettings["GSTRate"]) * total;
    }
    public static decimal CalculateBankTransferFees()
    {
        return decimal.Parse(ConfigurationSettings.AppSettings["BankTransferFee"].ToString());
    }
    public static decimal GetEntryPrice(Entry entry)
    {
        return entry.Amount + entry.Fee + entry.Tax;
    }
    public static bool IsOnTimeNow(DateTime dateTimeCheck)
    {
        return (dateTimeCheck < DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["OnTimeCutOff"]));
    }
    public static bool IsExtendedNow(DateTime dateTimeCheck)
    {
        return (dateTimeCheck > DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["OnTimeCutOff"]) &&
                dateTimeCheck < DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ExtendedCutOff"]));
    }
    public static bool IsExtended_2_Now(DateTime dateTimeCheck)
    {
        return (dateTimeCheck > DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ExtendedCutOff"]) &&
                dateTimeCheck < DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Extended_2_CutOff"]));
    }
    public static bool IsExtended_3_Now(DateTime dateTimeCheck)
    {
        return (dateTimeCheck > DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Extended_2_CutOff"]) &&
                dateTimeCheck < DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Extended_3_CutOff"]));
    }


    public static List<EffieJuryManagementApp.Jury> GetAllJuryCache(bool needRefresh, string rounds)
    {
        List<EffieJuryManagementApp.Jury> filteredJury = new List<EffieJuryManagementApp.Jury>();
        List<EffieJuryManagementApp.Jury> list = (List<EffieJuryManagementApp.Jury>)HttpContext.Current.Session["Effie.JuryList"];
        if (list == null || needRefresh)
        {
            List<EffieJuryManagementApp.Invitation> invList = EffieJuryManagementApp.InvitationList.GetInvitationList(Guid.Empty, EffieJuryManagementApp.Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value).ToList();
            list = EffieJuryManagementApp.JuryList.GetJuryList("", "").ToList();

            filteredJury = (from jury in list
                            join inv in invList
                            on jury.Id equals inv.JuryId
                            orderby jury.SerialNo
                            select jury).ToList();

            if (rounds != null || !String.IsNullOrEmpty(rounds))
            {
                if (rounds.Trim().ToLower().Contains("1"))
                {
                    filteredJury = (from jury in list
                                    join inv in invList
                                    on jury.Id equals inv.JuryId
                                    where inv.IsRound1Accepted
                                    orderby jury.SerialNo
                                    select jury).ToList();
                }
                else if (rounds.Trim().ToLower().Contains("2"))
                {
                    filteredJury = (from jury in list
                                    join inv in invList
                                    on jury.Id equals inv.JuryId
                                    where inv.IsRound2Accepted
                                    orderby jury.SerialNo
                                    select jury).ToList();
                }
            }



            HttpContext.Current.Session["Effie.JuryList"] = filteredJury.ToList();
        }
        return filteredJury;
    }
    public static EffieJuryManagementApp.Jury GetJuryFromIDCache(Guid juryId, string rounds)
    {
        EffieJuryManagementApp.Jury jury = null;

        List<EffieJuryManagementApp.Jury> list = GetAllJuryCache(true, rounds);

        try
        {
            jury = list.Where(m => m.Id == juryId).Single();
        }
        catch { }

        //foreach (EffieJuryManagementApp.Jury j in list)
        //    if (j.Id == juryId) return j;

        return jury;
    }
    public static JuryPanelCategoryList GetAllJuryPanelCategoryCache(bool needRefresh)
    {
        JuryPanelCategoryList list = (JuryPanelCategoryList)HttpContext.Current.Session["Effie.JuryPanelCategoryList"];
        if (list == null || needRefresh)
        {
            list = JuryPanelCategoryList.GetJuryPanelCategoryList("", "");
            HttpContext.Current.Session["Effie.JuryPanelCategoryList"] = list;
        }
        return list;
    }
    public static List<EffieJuryManagementApp.Jury> GetJuryListFromPanel(string jurypanel, string round)
    {
        List<EffieJuryManagementApp.Jury> list = GetAllJuryCache(true, round);
        List<EffieJuryManagementApp.Jury> jlist = new List<EffieJuryManagementApp.Jury>();

        if (round == "1")
            //jlist = list.Where(p => p.Round1PanelId == jurypanel).ToList();
            jlist = list.Where(p => GeneralFunction.IsInList(jurypanel, p.Round1PanelId, '|')).ToList();
        if (round == "2")
            //jlist = list.Where(p => p.Round2PanelId == jurypanel).ToList();
            jlist = list.Where(p => GeneralFunction.IsInList(jurypanel, p.Round2PanelId, '|')).ToList();

        return jlist;
    }
    public static List<EffieJuryManagementApp.Jury> GetJuryListFromEntryCategory(string market, string categoryPS, string categoryPSDetail, string round)
    {
        JuryPanelCategoryList jpclist = GetAllJuryPanelCategoryCache(false);
        string panelId = "";

        try
        {
            panelId = jpclist.Where(jcp => jcp.CategoryPSDetail == ConvertToCategoryPSDetailFull(market, categoryPS, categoryPSDetail) && jcp.Round == round).Single().PanelId;
        }
        catch { }

        //foreach (JuryPanelCategory jcp in jpclist)
        //{
        //    if (jcp.CategoryPSDetail == ConvertToCategoryPSDetailFull(market, categoryPS, categoryPSDetail) && jcp.Round == round)
        //    {
        //        panelId = jcp.PanelId;
        //        break;
        //    }
        //}

        List<EffieJuryManagementApp.Jury> jlist = GetAllJuryCache(true, round);

        if (!String.IsNullOrEmpty(panelId))
        {
            if (round == "1")
                return jlist.Where(p => GeneralFunction.IsInList(panelId, p.Round1PanelId, '|')) //jlist.Where(p => p.Round1PanelId == panelId)
                            .OrderBy(p => p.Id.ToString())
                            .ToList();

            if (round == "2")
                return jlist.Where(p => GeneralFunction.IsInList(panelId, p.Round2PanelId, '|')) //jlist.Where(p => p.Round2PanelId == panelId)
                            .OrderBy(p => p.Id.ToString())
                            .ToList();
        }

        return new List<EffieJuryManagementApp.Jury>();
    }

    public static List<EffieJuryManagementApp.Jury> GetJuryListFromEntryCategory(Entry entry, string market, string categoryPS, string categoryPSDetail, string round)
    {
        JuryPanelCategoryList jpclist = GetAllJuryPanelCategoryCache(false);
        string panelId = "";

        try
        {
            panelId = jpclist.Where(jcp => jcp.CategoryPSDetail == ConvertToCategoryPSDetailFull(market, categoryPS, categoryPSDetail) && jcp.Round == round).Single().PanelId;
        }
        catch { }

        //foreach (JuryPanelCategory jcp in jpclist)
        //{
        //    if (jcp.CategoryPSDetail == ConvertToCategoryPSDetailFull(market, categoryPS, categoryPSDetail) && jcp.Round == round)
        //    {
        //        panelId = jcp.PanelId;
        //        break;
        //    }
        //}

        List<EffieJuryManagementApp.Jury> jlist = GetAllJuryCache(true, round);

        if (!String.IsNullOrEmpty(panelId))
        {
            if (round == "1")
                return jlist.Where(p => GeneralFunction.IsInList(panelId, p.Round1PanelId, '|') && !IsRecuse(entry, p.Id, round)) //jlist.Where(p => p.Round1PanelId == panelId)
                            .OrderBy(p => p.Id.ToString())
                            .ToList();

            if (round == "2")
                return jlist.Where(p => GeneralFunction.IsInList(panelId, p.Round2PanelId, '|') && !IsRecuse(entry, p.Id, round)) //jlist.Where(p => p.Round2PanelId == panelId)
                            .OrderBy(p => p.Id.ToString())
                            .ToList();
        }

        return new List<EffieJuryManagementApp.Jury>();
    }

    public static bool IsRecuse(Entry entry, Guid juryId, string round)
    {
        bool isRecuse = false;

        Score scoreResult = null;
        List<Score> scores = GetAllScoreCache(false).ToList();

        try
        {
            scoreResult = scores.Where(p => p.EntryId == entry.Id && p.Juryid == juryId && p.Round == round).Single();

            if (scoreResult.IsAdminRecuse || scoreResult.IsRecuse)
                isRecuse = true;
        }
        catch { }

        return isRecuse;
    }

    public static List<string> GetJuryPanelList(string round)
    {
        List<string> list = new List<string>();
        if (round == "1")
        {
            list.Add("01");
            list.Add("02");
            list.Add("03");
            list.Add("04");
            list.Add("05");
            list.Add("06");
            list.Add("07");
            list.Add("08");
            list.Add("09");
            list.Add("10");
            list.Add("11");
            list.Add("12");
            list.Add("13");
            list.Add("14");
            list.Add("15");
            list.Add("16");
            list.Add("17");
            list.Add("18");
            list.Add("19");
            list.Add("20");
            list.Add("21");
            list.Add("22");
            list.Add("23");
            list.Add("24");
        }
        if (round == "2")
        {
            list.Add("01");
            list.Add("02");
            list.Add("03");
            list.Add("04");
            list.Add("05");
            list.Add("06");
            list.Add("07");
            list.Add("08");
            list.Add("09");
            list.Add("10");
            list.Add("11");
            list.Add("12");
            list.Add("13");
            list.Add("14");
            list.Add("15");
            list.Add("16");
            list.Add("17");
            list.Add("18");
            list.Add("19");
            list.Add("20");
            list.Add("21");
            list.Add("22");
            list.Add("23");
            list.Add("24");
        }
        return list;
    }
    public static List<JuryPanelCategory> GetJuryPanelCategoryFromPanelId(string panelId, string round)
    {
        JuryPanelCategoryList list = GetAllJuryPanelCategoryCache(false);
        //return list.Where(p => p.PanelId == panelId )
        return list.Where(p => GeneralFunction.IsInList(p.PanelId, panelId, '|') && p.Round == round)
                    .OrderBy(p => p.OrderNo)
                    .ToList();
    }
    public static List<JuryPanelCategory> GetJuryPanelCategoryFromCategoryPS(string market, string categoryPS, string categoryPSDetail, string round)
    {
        JuryPanelCategoryList list = GetAllJuryPanelCategoryCache(false);
        return list.Where(p => p.CategoryPSDetail == ConvertToCategoryPSDetailFull(market, categoryPS, categoryPSDetail) && p.Round == round)
                    .OrderBy(p => p.OrderNo)
                    .ToList();
    }
    public static string GetNewJuryId()
    {
        string prefix = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("JurySerialNoPrefix")[0].Value;
        string juryId = "";
        Gen_GeneralUseValueList gen_GeneralUseValueList = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("JurySerialNo");
        Gen_GeneralUseValue gen_GeneralUseValue = Gen_GeneralUseValue.GetGen_GeneralUseValue(gen_GeneralUseValueList[0].Id);

        if (gen_GeneralUseValue != null)
        {
            juryId = gen_GeneralUseValue.Value;
            juryId = prefix + juryId;

            gen_GeneralUseValue.Value = (Convert.ToInt32(gen_GeneralUseValue.Value) + 1).ToString();
            gen_GeneralUseValue.DateModifiedString = DateTime.Now.ToString();
            gen_GeneralUseValue.Save();
        }
        return juryId;
    }
    public static EffieJuryManagementApp.Jury GetDummyJuryForAdminSpoof(Guid juryId)
    {
        EffieJuryManagementApp.Jury jury = EffieJuryManagementApp.Jury.GetJury(juryId);
        return jury;
    }


    public static ScoreList GetAllScoreCache(bool needRefresh)
    {
        ScoreList list = (ScoreList)HttpContext.Current.Session["Effie.ScoreList"];
        if (list == null || needRefresh)
        {
            list = ScoreList.GetScoreList(Guid.Empty, Guid.Empty);
            HttpContext.Current.Session["Effie.ScoreList"] = list;
        }
        return list;
    }
    public static List<Score> GetScoreListFromJuryCache(Guid juryId, string round)
    {
        ScoreList scores = GetAllScoreCache(false);
        return scores.Where(p => p.Juryid == juryId && p.Round == round).ToList();
    }
    public static List<Score> GetAllScoreListFromEntryCache(Entry entry)
    {
        ScoreList scores = GetAllScoreCache(false);
        return scores.Where(p => p.EntryId == entry.Id)
                            .OrderBy(p => p.Juryid)
                            .ToList();
    }
    public static Score GetScoreFromMatchingEntryCache(Entry entry, Guid juryId, string round)
    {
        Score scoreResult = null;

        List<Score> scores = GetAllScoreCache(false).ToList();

        try
        {
            scoreResult = scores.Where(p => p.EntryId == entry.Id && p.Juryid == juryId && p.Round == round).Single();
        }
        catch { }

        return scoreResult;
    }
    public static List<Guid> GetFilteredScoreListFromRegCompany(string company, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredScoreListFromRegCompany"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            List<Score> scores = GetAllScoreCache(false).ToList();
            List<Registration> regs = GetAllRegistrationCache(false).ToList();

            foreach (Registration reg in regs)
            {
                if (reg.Company.Trim().ToUpper().IndexOf(company.Trim().ToUpper()) != -1)
                {
                    // Get all Entry belonging to this reg Id
                    EntryList entrys = EntryList.GetEntryList(Guid.Empty, reg.Id, "");
                    foreach (Entry entry in entrys)
                    {
                        foreach (Score score in scores)
                        {
                            if (score.EntryId == entry.Id) list.Add(score.Id);
                        }
                    }

                }
            }
            HttpContext.Current.Session["Effie.FilteredScoreListFromRegCompany"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredScoreListFromJuryAssignment(string juryname, string juryId, string jurycompany, string round, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredScoreListFromJury"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            ScoreList scores = GetAllScoreCache(true);
            foreach (Score score in scores)
            {
                EffieJuryManagementApp.Jury jury = GetJuryFromIDCache(score.Juryid, round);
                try {
                    if (
                      (jury.SerialNo.IndexOf(juryId) != -1 && juryId.Trim() != "") ||
                      ((jury.FirstName.IndexOf(juryname) != -1 || jury.FirstName.IndexOf(juryname) != -1) && juryname.Trim() != "") ||
                      (jury.Company.IndexOf(jurycompany) != -1 && jurycompany.Trim() != "")
                      )
                      {
                          list.Add(score.Id);
                      }
                }
                catch { }
            }
            HttpContext.Current.Session["Effie.FilteredScoreListFromJury"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }
    public static List<Guid> GetFilteredScoreListFromJuryAssignment(string jurynetwork, string juryholdingcompany, string round, bool needRefresh)
    {
        List<Guid> list = (List<Guid>)HttpContext.Current.Session["Effie.FilteredScoreListFromJury2"];

        if (list == null || needRefresh)
        {
            list = new List<Guid>();

            ScoreList scores = GetAllScoreCache(true);
            foreach (Score score in scores)
            {
                EffieJuryManagementApp.Jury jury = GetJuryFromIDCache(score.Juryid, round);
                try {
                    if ((jury.Network.IndexOf(jurynetwork) != -1 && jurynetwork.Trim() != "") ||
                       (jury.HoldingCompany.IndexOf(juryholdingcompany) != -1 && juryholdingcompany.Trim() != "") )
                    {
                        list.Add(score.Id);
                    }
                }
                catch { }
            }
            HttpContext.Current.Session["Effie.FilteredScoreListFromJury2"] = list;
        }

        if (list == null) list = new List<Guid>();
        return list;
    }


    public static List<Score> FilterScoreListFromActivePanels(List<Score> scoreList, string round)
    {
        // Pre-requite
        // Refresh cache for 
        // Entries, Scores, JCPList

        SystemData sys = GetSystemData();
        List<Score> flist = new List<Score>();
        foreach (Score score in scoreList)
        {
            Entry entry = GetEntryFromIDCache(score.EntryId);
            if (IsEntryInActivePanels(entry, sys, round))
                flist.Add(score);
        }
        return flist;
    }
    public static List<Entry> FilterEntryListFromActivePanels(List<Entry> entryList, string round)
    {
        // Pre-requite
        // Refresh cache for 
        // Entries, JCPList

        SystemData sys = GetSystemData();
        List<Entry> flist = new List<Entry>();
        foreach (Entry entry in entryList)
        {
            if (IsEntryInActivePanels(entry, sys, round))
                flist.Add(entry);
        }
        return flist;
    }
    public static bool IsEntryInActivePanels(Entry entry, SystemData sys, string round)
    {
        List<JuryPanelCategory> jcpList = GetJuryPanelCategoryFromCategoryPS(entry.CategoryMarketFromRound(round), entry.CategoryPSFromRound(round), entry.CategoryPSDetailFromRound(round), round);

        foreach (JuryPanelCategory jcp in jcpList)
        {
            string activepanels = sys.ActivePanelsRound1;
            if (round == "2") activepanels = sys.ActivePanelsRound2;
            if (IsInList(jcp.PanelId, activepanels, '|'))
                return true;
        }
        return false;
    }


    public static decimal GetScoreWeightageSC()
    {
        return (decimal)70 / 3;
    }
    public static decimal GetScoreWeightageID()
    {
        return (decimal)70 / 3;
    }
    public static decimal GetScoreWeightageIL()
    {
        return (decimal)70 / 3;
    }
    public static decimal GetScoreWeightageRE()
    {
        return (decimal)30;
    }
    public static decimal CalculateSC(int sc)
    {
        return (sc * GetScoreWeightageSC() / 100);
    }
    public static decimal CalculateID(int id)
    {
        return (id * GetScoreWeightageID() / 100);
    }
    public static decimal CalculateIL(int il)
    {
        return (il * GetScoreWeightageIL() / 100);
    }
    public static decimal CalculateRE(int re)
    {
        return (re * GetScoreWeightageRE() / 100);
    }
    public static decimal CalculateCompositeScore(int sc, int id, int il, int re)
    {
        return CalculateSC(sc) + CalculateID(id) + CalculateIL(il) + CalculateRE(re);
    }

    public static SystemData GetSystemData()
    {
        SystemData data = SystemData.GetSystemData(new Guid("91ddb71e-f77e-4d7f-b728-49304737762d"));
        return data;
    }

    public static Guid GetSecurityCodeForRSVPReport()
    {
        Guid securityCode = Guid.Empty;

        SystemData data = SystemData.GetSystemData(new Guid("56ff6a99-6247-43de-aecc-7b727fa076ff"));

        if (data != null)
        {
            Guid.TryParse(data.ActivePanelsRound1, out securityCode);
        }

        return securityCode;
    }

    public static void CompleteNewGalaOrderPayPal(Guid goId)
    {
        // ok 
        GalaOrder go = GalaOrder.GetGalaOrder(goId);

        // validate if the entries already have an invoice
        if (go.Invoice != "") return;


        go.PayStatus = StatuspaymentGalaOrder.Paid;
        go.Invoice = GeneralFunction.GetNewInvoiceNumberGala();
        go.DatePaidString = DateTime.Now.ToString();
        go.Save();

        //// Save a new amount received
        //AmountReceived amt = AmountReceived.NewAmountReceived();
        //amt.Amount = totalamount;
        //amt.DateReceivedString = DateTime.Now.ToString();
        //amt.PaygroupId = payGroupId;
        //amt.Invoice = invoice;
        //amt.Remarks = "Received from Paypal";
        //amt.IsSetPaid = true;
        //amt.DateCreatedString = DateTime.Now.ToString();
        //amt.Save();



        //Send Email
        Email.SendCompletedGalaPaymentEmailPayPal(go);
        //UpdateEntryLastSendPaidEmailDate(payGroupId);
    }
    public static void CompleteNewGalaOrderOthers(Guid goId)
    {
        // ok
        GalaOrder go = GalaOrder.GetGalaOrder(goId);

        // validate if the entries already have an invoice
        //if (go.Invoice != "") return;


        go.PayStatus = StatuspaymentGalaOrder.Paid;
        go.DatePaidString = DateTime.Now.ToString();
        if (go.Invoice.Trim() == "") go.Invoice = GeneralFunction.GetNewInvoiceNumberGala();
        go.Save();

        //Send Email
        Email.SendCompletedGalaPaymentEmailOthers(go);
    }
    public static void CompletePendingPaymentGalaOthers(Guid goId)
    {
        // pending
        GalaOrder go = GalaOrder.GetGalaOrder(goId);

        // validate if the entries already have an invoice
        //if (go.Invoice != "") return;


        go.PayStatus = StatuspaymentGalaOrder.NotPaid;
        if (go.Invoice.Trim() == "") go.Invoice = GeneralFunction.GetNewInvoiceNumberGala();
        go.Save();

        //Send Email
        Email.SendPendingGalaPaymentEmailOthers(go);
    }



    public static bool IsJuryLoginCutOff()
    {
        return (DateTime.Now > DateTime.Parse(ConfigurationManager.AppSettings["JuryLoginCutOff"]));
    }
    public static bool IsEntrantLoginCutOff()
    {
        return (DateTime.Now > DateTime.Parse(ConfigurationManager.AppSettings["EntrantLoginCutOff"]));
    }
    public static bool IsAllowClone()
    {
        return ("1" == ConfigurationManager.AppSettings["IsAllowClone"].ToString());
    }
    
    public static bool IsEntrantSubmissionCutOff()
    {
        return (DateTime.Now > DateTime.Parse(ConfigurationManager.AppSettings["EntrantSubmissionCutOff"]));
    }
    public static bool IsAddNewEntryCutOff()
    {
        return (DateTime.Now > DateTime.Parse(ConfigurationManager.AppSettings["AddNewEntryCutOff"]));
    }
    public static bool IsAllowUploadFiles()
    {
        return ConfigurationManager.AppSettings["AllowUploadFiles"].ToString() == "1";
    }
    public static bool IsRegisterNewUserCutOff()
    {
        return (DateTime.Now > DateTime.Parse(ConfigurationManager.AppSettings["RegisterNewUserCutOff"]));
    }
    public static string EffieEventYear()
    {
        return ConfigurationManager.AppSettings["EffieEventYear"].ToString();
    }
    
    #endregion


    #region Filter Retainer
    public static void ResetFilter()
    {
        HttpContext.Current.Session.Remove("Effie.Admin.Page.Filter");
    }
    public static void SetFilter(string pageId, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8)
    {
        string data = pageId + "|" + f1 + "|" + f2 + "|" + f3 + "|" + f4 + "|" + f5 + "|" + f6 + "|" + f7 + "|" + f8;
        HttpContext.Current.Session["Effie.Admin.Page.Filter"] = data;
    }
    public static string GetFilter()
    {
        if (HttpContext.Current.Session["Effie.Admin.Page.Filter"] != null)
        {
            string data = (string)HttpContext.Current.Session["Effie.Admin.Page.Filter"];
            return data;
        }
        else
            return "||||||||";
    }
    public static string GetFilterPageId()
    {
        return GetFilter().Split('|')[0];
    }
    public static string GetFilterF1()
    {
        return GetFilter().Split('|')[1];
    }
    public static string GetFilterF2()
    {
        return GetFilter().Split('|')[2];
    }
    public static string GetFilterF3()
    {
        return GetFilter().Split('|')[3];
    }
    public static string GetFilterF4()
    {
        return GetFilter().Split('|')[4];
    }
    public static string GetFilterF5()
    {
        return GetFilter().Split('|')[5];
    }
    public static string GetFilterF6()
    {
        return GetFilter().Split('|')[6];
    }
    public static string GetFilterF7()
    {
        return GetFilter().Split('|')[7];
    }
    public static string GetFilterF8()
    {
        return GetFilter().Split('|')[8];
    }

    #endregion


    #region BarcodeImage
    //public static System.IO.MemoryStream GetBarcodeImage(Registration reg)
    //{        
    //    BarcodeLib.Barcode.Linear.Linear barcode = new BarcodeLib.Barcode.Linear.Linear();
    //    {
    //        barcode.Data = "*" + reg. + "*";
    //        barcode.Type = BarcodeType.CODE39;
    //        barcode.BarHeight = 30;
    //        barcode.AddCheckSum = false;
    //        barcode.Format = System.Drawing.Imaging.ImageFormat.Jpeg;
    //    }
    //    MemoryStream ms = new MemoryStream();      
    //    barcode.drawBarcode(ms);  
    //    ms.Position = 0;
    //    return ms;
    //}

    #endregion

    #region Lookup
    public static string GetPaymentEntryStatus(string entrystatus)
    {
        switch (entrystatus)
        {
            case StatusPaymentEntry.NotPaid:
                return "Not Paid";
                break;
            case StatusPaymentEntry.Paid:
                return "Paid";
                break;
        }
        return "";
    }
    public static string GetEntryStatus(string entrystatus)
    {
        switch (entrystatus)
        {
            case StatusEntry.Draft:
                return "Draft";
                break;
            case StatusEntry.Ready:
                return "Ready For Submission";
                break;
            case StatusEntry.PaymentPending:
                return "Open";
                break;
            case StatusEntry.UploadPending:
                return "Open";
                break;
            case StatusEntry.UploadCompleted:
                return "Open";
                break;
            case StatusEntry.Completed:
                return "Closed";
                break;
        }
        return "";
    }
    public static string GetEntryStatusForAdmin(string entrystatus)
    {
        switch (entrystatus)
        {
            case StatusEntry.Draft:
                return "Draft";
                break;
            case StatusEntry.Ready:
                return "Ready For Submission";
                break;
            case StatusEntry.PaymentPending:
                return "Pending Payment";
                break;
            case StatusEntry.UploadPending:
                return "Pending Upload";
                break;
            case StatusEntry.UploadCompleted:
                return "Pending Completion";
                break;
            case StatusEntry.Completed:
                return "Closed";
                break;
            case StatusEntry.Reopened:
                return "Reopened";
                break;
            case StatusEntry.PendingVerification:
                return "Pending Verification";
                break;
            case StatusEntry.PendingReopen:
                return "Pending Reopen";
                break;

        }
        return "";
    }
    public static string GetRegistrationStatus(string registrationstatus)
    {
        switch (registrationstatus)
        {
            case StatusRegistration.InActive:
                return "Not Authenticated";
                break;
            case StatusRegistration.OK:
                return "Authenticated";
                break;

        }
        return "";
    }
    public static string GetWithdrawnStatus(string withdrawnstatus)
    {
        switch (withdrawnstatus)
        {
            case StatusEntryWidthdrawn.None:
                return "";
                break;
            case StatusEntryWidthdrawn.Withdrawn:
                return "Withdrawn";
                break;
            case StatusEntryWidthdrawn.DQ:
                return "DQ";
                break;
        }
        return "";
    }
    public static string GetPaymentType(string paymenttype)
    {
        switch (paymenttype)
        {
            case PaymentType.PayPal:
                return "PayPal";
                break;
            case PaymentType.Cheque:
                return "Cheque";
                break;
            case PaymentType.BankTransfer:
                return "Bank Transfer";
                break;

        }
        return "";
    }
    public static string GetEntryMarket(string entrymarket)
    {
        switch (entrymarket)
        {
            case "SM":
                return "Single Market";
                break;
            case "MM":
                return "Multi Market";
                break;
        }
        return "";
    }
    public static string GetPaymentGalaStatus(string status)
    {
        switch (status)
        {
            case StatuspaymentGalaOrder.NotPaid:
                return "Not Paid";
                break;
            case StatuspaymentGalaOrder.Paid:
                return "Paid";
                break;
        }
        return "";
    }
    public static string GetInvoiceType(string invType)
    {
        switch (invType)
        {
            case AdhocInvoiceType.ReOpen:
                return "REOPENING OF ENTRY";
                break;
            case AdhocInvoiceType.ChangeReq:
                return "CHANGE REQUEST";
                break;
            case AdhocInvoiceType.ExtDeadLine:
                return "EXTENSION OF DEADLINE";
                break;
            case AdhocInvoiceType.Custom:
                return "CUSTOM";
                break;
        }
        return "";
    }
    //public static string GetGalaShipping(string shippingcode)
    //{
    //    switch (shippingcode)
    //    {
    //        case "collect_office":
    //            return "Single Market";
    //            break;
    //        case "MM":
    //            return "Multi Market";
    //            break;
    //    }
    //    return "";
    //}
    //public static string GetUserApprovalStatus(string approvalstatus)
    //{
    //    switch (approvalstatus)
    //    {
    //        case StatusApproval.Approved:
    //            return "Approve";
    //            break;
    //        case StatusApproval.NotApproved:
    //            return "Not Approved";
    //            break;
    //        case StatusApproval.Rejected :
    //            return "Reject";
    //            break;
    //    }
    //    return "";
    //}
    #endregion

    #region Amazon Cloud

    // Function to upload
    public static void UploadFileToAmazonS3(FileUpload fu, string bucketName, string fileName)
    {
        //if (!fu.FileName.Trim().Equals(""))
        //{
        //    TransferUtility utility = new TransferUtility();
        //    utility.Upload(fu.PostedFile.InputStream, bucketName, fileName);

        //    if (FileExistsInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Small"], fileName))
        //    {
        //        DeleteFileInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Small"], fileName);
        //    }

        //    ConvertHDtoLD_Video(fileName, ConfigurationManager.AppSettings["AWSPipeLineID"], ConfigurationManager.AppSettings["AWSPresetID"]);            
        //}
        //else
        //    throw new System.Exception("File Empty.");

        if (!fu.FileName.Trim().Equals(""))
        {
            if (FileExistsInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Small"], fileName))
            {
                DeleteFileInAmazonS3(ConfigurationManager.AppSettings["AWSBucket_Small"], fileName);
            }

            Amazon.S3.AmazonS3Client s3Client = new Amazon.S3.AmazonS3Client();

            PutObjectRequest request = new PutObjectRequest();
            request.InputStream = fu.PostedFile.InputStream;
            request.BucketName = bucketName;
            request.CannedACL = S3CannedACL.PublicRead;
            request.Key = fileName;
            s3Client.PutObject(request);

            //ConvertHDtoLD_Video(fileName, ConfigurationManager.AppSettings["AWSPipeLineID"], ConfigurationManager.AppSettings["AWSPresetID"]); 
        }
        else
            throw new System.Exception("File Empty.");
    }

    public static void ConvertHDtoLD_Video(string fileName, string pipeLineID, string presetID)
    {
        AmazonElasticTranscoderClient etsClient = new AmazonElasticTranscoderClient();
        var response = etsClient.CreateJob(new CreateJobRequest()
        {
            PipelineId = pipeLineID, //pipeline.Id,
            Input = new JobInput()
            {
                AspectRatio = "auto",
                Container = "mp4",
                FrameRate = "auto",
                Interlaced = "auto",
                Resolution = "auto",
                Key = fileName
            },
            Output = new CreateJobOutput()
            {
                ThumbnailPattern = "",
                Rotate = "0",
                PresetId = presetID,
                Key = fileName
            }
        });
    }


    public static string GetSelectedFlagReasons(string FlagReason, bool isDes = false, bool isNeedNumbering = false)
    {
        string[] Delimiter = { "#&&##&&#", "&&&$$&&&", "************", "||" };
        string SelectedFlagReason = "";
        int count = 0;
        if (!string.IsNullOrEmpty(FlagReason))
        {
            List<Data.CollectData> CollectDataList = Data.GetFlagReasons();
            string[] FlagReasonSplit = FlagReason.Split(new string[] { Delimiter[3] }, System.StringSplitOptions.RemoveEmptyEntries);

            if (FlagReasonSplit.Count() != 0)
            {
                for (int i = 0; i < FlagReasonSplit.Length; i++)
                {
                    count++;
                    string[] Datas = FlagReasonSplit[i].Split(':');
                    string ID = Datas[0];
                    string Others = Datas[1];
                    string Num = "";

                    if (isNeedNumbering)
                    {
                        Num = count + ". ";
                    }

                    try
                    {

                        Data.CollectData CollectData = CollectDataList.FirstOrDefault(x => x.id == ID);
                        if (isDes)
                        {
                            if (CollectData.isHasOther)
                            {
                                SelectedFlagReason += Num + Others + "||";
                            }
                            else
                            {
                                SelectedFlagReason += Num + CollectData.Desc + "||";
                            }
                        }
                        else
                        {
                            SelectedFlagReason += Num + CollectData.Title + "||";
                        }
                    }
                    catch { }
                }
            }
        }
        else
        {
            SelectedFlagReason = "";
        }
        return SelectedFlagReason;
    }

    public static MemoryStream GetFileFromAmazonS3(string bucketName, string fileName)
    {
        Amazon.S3.AmazonS3Client s3Client = new Amazon.S3.AmazonS3Client();

        MemoryStream file = new MemoryStream();

        using (s3Client)
        {

            try
            {
                GetObjectResponse r = s3Client.GetObject(new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = fileName
                });
                try
                {
                    BufferedStream stream2 = new BufferedStream(r.ResponseStream);
                    byte[] buffer = new byte[0x2000];
                    int count = 0;
                    while ((count = stream2.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, count);
                    }
                }
                catch (AmazonS3Exception)
                {
                    //Show exception
                }

            }
            catch (AmazonS3Exception)
            {
                //Show exception
            }
        }

        return file;
    }

    public static bool FileExistsInAmazonS3(string bucketName, string fileName)
    {
        try {
            using (AmazonS3Client client = new AmazonS3Client())
            {
                S3FileInfo s3FileInfo = new S3FileInfo(client, bucketName, fileName);
                if (s3FileInfo.Exists)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch { return false; }
    }

    public static void DeleteFileInAmazonS3(string bucketName, string fileName)
    {
        using (AmazonS3Client client = new AmazonS3Client())
        {
            S3FileInfo s3FileInfo = new S3FileInfo(client, bucketName, fileName);
            if (s3FileInfo.Exists)
            {
                s3FileInfo.Delete();
            }

        }
    }

    public static bool IsAdhocReOpen(Guid regId, Guid payGroupId)
    {
        AdhocInvoiceList adhocInvList = AdhocInvoiceList.GetAdhocInvoiceList(regId, payGroupId);
        AdhocInvoiceItem adhocInvoiceItem = AdhocInvoiceItemList.GetAdhocInvoiceItemList(adhocInvList[0].PayGroupId, adhocInvList[0].Id)[0];
        Registration reg = Registration.GetRegistration(adhocInvList[0].RegistrationId);
        string InvoiceType = adhocInvoiceItem.InvoiceType;

        bool isAdhocReOpen = true;

        if (InvoiceType == AdhocInvoiceType.ReOpen)
            isAdhocReOpen = true;
        else if (InvoiceType == AdhocInvoiceType.ChangeReq || InvoiceType == AdhocInvoiceType.Custom || InvoiceType == AdhocInvoiceType.ExtDeadLine)
            isAdhocReOpen = false;

        return isAdhocReOpen;
    }

    #endregion
}
