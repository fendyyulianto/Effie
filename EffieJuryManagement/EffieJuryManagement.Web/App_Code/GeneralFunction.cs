using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.IO;
using Telerik.Web.UI;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using EffieJuryManagementApp;
using ClosedXML.Excel;


/// <summary>
/// Summary description for GeneralFunction
/// </summary>
/// 


#region Enumerators
public static class RoundsType
{
    public const string Round1 = "R1";
    public const string Round2 = "R2";
    public const string BothRounds = "R1R2";
    public const string NotApplicable = "NA";
}

public static class EmailCategory
{
    public const string Normal = "NOR";
    public const string Invitation = "INV";    
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


    #region Form Related Function
    //Form Related Function
    public static string ValidateTextBox(string fieldName, TextBox txt, bool isRequired, string type)
    {
        string ret = "";

        if (isRequired && txt.Text.Trim().Equals(""))
            ret = fieldName + " is required.<br>";

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
                        if (chr == ' ')
                            ret = fieldName + " should not have spaces.<br>";
                        else
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
        ddl.Items.Add("Dentsu");
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

    public static string ValidateDatePick(string fieldName, Telerik.Web.UI.RadDatePicker RadDatePick, bool isRequired)
    {
        if (isRequired && RadDatePick.SelectedDate == null)
        {
            HighlightControl(RadDatePick);
            return fieldName + " is required.<br>";
        }


        return "";
    }

    public static string ValidateRadioButtonList(string fieldName, RadioButtonList rdLst, bool isRequired)
    {
        if (isRequired && rdLst.SelectedValue.Trim().Equals(""))
        {
            HighlightControl(rdLst);
            return fieldName + " is required.<br>";
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

    public static string ValidateDropDownList(string fieldName, DropDownList ddl, bool isRequired, string emptyValue)
    {
        string ret = "";
        if (isRequired && ddl.SelectedValue.Trim().Equals(emptyValue))
        {
            ret = fieldName + " is required.<br>";
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
            if (fileType == "Excel")
            {
                if (!fu.FileName.ToLower().EndsWith(".xls") && !fu.FileName.ToLower().EndsWith(".xlsx"))
                    return fieldName + " Upload xls and xlsx file only.<br>";
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
    public static void DisableAllAction(Control parent, bool Enabled)
    {
        foreach (Control c in parent.Controls)
        {
            //if (c.GetType() == typeof(Button)) ((Button)c).Enabled = Enabled;
            if (c.GetType() == typeof(DropDownList)) ((DropDownList)c).Enabled = Enabled;
            if (c.GetType() == typeof(TextBox)) ((TextBox)c).Enabled = Enabled;
            if (c.GetType() == typeof(CheckBox)) ((CheckBox)c).Enabled = Enabled;
            if (c.GetType() == typeof(RadioButton)) ((RadioButton)c).Enabled = Enabled;
            if (c.GetType() == typeof(RadioButtonList)) ((RadioButtonList)c).Enabled = Enabled;
            if (c.GetType() == typeof(CheckBoxList)) ((CheckBoxList)c).Enabled = Enabled;

            DisableAllAction(c, Enabled);
        }
    }
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

    public static void SaveEmailSentLog(Jury jury, Guid templateId,string eventYear)
    {   
      EmailTemplate templateSent = null;

        // Recording Email sent for each Juries
        EmailSent emailsent = EmailSent.NewEmailSent();
        emailsent.JuryId = jury.Id;
        emailsent.TemplateId = templateId;

        templateSent = EmailTemplate.GetEmailTemplate(templateId);
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
    public static List<string> GetFilteredCountryListFromJury(bool needRefresh)
    {
        List<string> countryList = (List<string>)HttpContext.Current.Session["Effie.JuryFilteredCountryList"];


        if (countryList == null || needRefresh)
        {
            countryList = new List<string>();

            JuryList juryList = GetAllJuryCache(false);
            foreach (Jury jury in juryList)
            {
                if (!countryList.Contains(jury.Country) && jury.Country != "") countryList.Add(jury.Country);
            }
            countryList.Sort((x, y) => string.Compare(x.ToString(), y.ToString()));
            HttpContext.Current.Session["Effie.JuryFilteredCountryList"] = countryList;
        }

        if (countryList == null) countryList = new List<string>();

        return countryList;
    }
    public static void ResetCCCache()
    {
        HttpContext.Current.Session.Remove("Effie.CCList");
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
    public static JuryList GetAllJuryCache(bool needRefresh)
    {
        JuryList list = (JuryList)HttpContext.Current.Session["Effie.JuryList"];
        if (list == null || needRefresh)
        {
            list = JuryList.GetJuryList("", "");
            HttpContext.Current.Session["Effie.JuryList"] = list;
        }
        return list;
    }
    public static Jury GetJuryFromIDCache(Guid juryId)
    {
        JuryList list = GetAllJuryCache(false);
        foreach (Jury j in list)
            if (j.Id == juryId) return j;
        return null;
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
    public static bool IsProfileUpdateDateCutOff()
    {
        if (DateTime.Now > Convert.ToDateTime(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("UpdateProfileDate")[0].Value))
        {
            return true;
        }
        return false;
    }
    public static bool IsInvitationDateCutOff()
    {
        if (DateTime.Now > Convert.ToDateTime(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("InvitationDeadline")[0].Value))
        {
            return true;
        }
        return false;
    }
   

    #endregion

    #region Filter Retainer
    public static void ResetFilter()
    {
        HttpContext.Current.Session.Remove("Effie.JuryManagement.Admin.Page.Filter");
    }
    public static void SetFilter(string pageId, string f1, string f2, string f3, string f4, string f5, string f6, string f7, string f8, string f9,string f10,string f11,string pageNo)
    {
        string data = pageId + "|" + f1 + "|" + f2 + "|" + f3 + "|" + f4 + "|" + f5 + "|" + f6 + "|" + f7 + "|" + f8 + "|" + f9 + "|" + f10 + "|" + f11 + "|" + pageNo;
        HttpContext.Current.Session["Effie.JuryManagement.Admin.Page.Filter"] = data;
    }
    public static string GetFilter()
    {
        if (HttpContext.Current.Session["Effie.JuryManagement.Admin.Page.Filter"] != null)
        {
            string data = (string)HttpContext.Current.Session["Effie.JuryManagement.Admin.Page.Filter"];
            return data;
        }
        else
            return "||||||||||||";
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
    public static string GetFilterF9()
    {
        return GetFilter().Split('|')[9];
    }
    public static string GetFilterF10()
    {
        return GetFilter().Split('|')[10];
    }
    public static string GetFilterF11()
    {
        return GetFilter().Split('|')[11];
    }
    public static string GetFilterPageNo()
    {
        if (!String.IsNullOrEmpty(GetFilter().Split('|')[12]))
            return GetFilter().Split('|')[12];
        else
            return "0";
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
}
