using System;
using System.IO;
using ClosedXML.Excel;
using EffieJuryManagementApp;
using System.Text;
using System.Linq;

public partial class Test_editprofile : System.Web.UI.Page
{
    JuryList juryList = JuryList.GetJuryList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    private void LoadForm()
    {
        lblTitle.Text = "";
        lblDesc.Text = "Please uplaod the List in Excel format.";

        ucGen_UploadFileForImport.fieldName = "Upload Excel *";
        ucGen_UploadFileForImport.uploadFileMsg = "Excel format (max file size 5mb)";
        ucGen_UploadFileForImport.fileType = "Excel";
        ucGen_UploadFileForImport.maxSize = 5242880;
        ucGen_UploadFileForImport.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "ImportInvitation\\";
        ucGen_UploadFileForImport.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "ImportInvitation/";
    }

    private void PopulateForm()
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        XLWorkbook workBook;
        lbError.Text = "";

        lbError.Text += ucGen_UploadFileForImport.ValidateForm();

        if (String.IsNullOrEmpty(lbError.Text))
        {
            string fileName = DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_ss") + "_ImportedList";

            ucGen_UploadFileForImport.SaveAndUpdateFile(fileName);

            if (ucGen_UploadFileForImport.GetValue("").ToLower() != (fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName)).ToLower())
            {
                File.Copy(ucGen_UploadFileForImport.saveDirectory + ucGen_UploadFileForImport.GetValue(fileName), ucGen_UploadFileForImport.saveDirectory + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName), true);

                if (File.Exists(ucGen_UploadFileForImport.saveDirectory + ucGen_UploadFileForImport.GetValue(fileName)))
                    GeneralFunction.DeleteFile(ucGen_UploadFileForImport.saveDirectory + ucGen_UploadFileForImport.GetValue(fileName), false);
            }

            //ucGen_UploadFileForImport.SetValue(fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName));

            workBook = new XLWorkbook(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "ImportInvitation\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName).ToLower());            

            if (String.IsNullOrEmpty(lbError.Text))
            {
                ReadExcelData(workBook);
                Response.Redirect("../Main/JuryList.aspx");
            }
            else
                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "ImportInvitation\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName)))
                {
                    GeneralFunction.DeleteFile(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "ImportInvitation\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName), true);
                    //ucGen_UploadFileForImport.SetValue(string.Empty);
                }
        }
    }
   
    public void ReadExcelData(XLWorkbook workBook)
    {
        JuryList jurylsit = JuryList.GetJuryList();

        if (workBook.Worksheets.Count > 0)
        {
            int sheetNo = 1;

            foreach (IXLWorksheet sh in workBook.Worksheets)
            {
                var dataRange = sh.RangeUsed();
                int skiprows = 1;

                if (dataRange != null)
                {

                    foreach (var row in dataRange.Rows())
                    {
                        if (skiprows >= 2 && sheetNo == 1)
                        {
                            if (ValidateRow(row))
                            {
                                Jury jury = null;

                                string serialNo = RemoveControlChars(row.Cell(3).GetString());

                                try
                                {
                                    jury = jurylsit.Where(m => m.SerialNo.Equals(serialNo)).Single();
                                }
                                catch { }

                                if (jury == null)
                                    jury = Jury.NewJury();

                                if (jury.IsNew)
                                {
                                    jury.Type = RemoveControlChars(row.Cell(2).GetString());
                                    jury.Salutation = RemoveControlChars(row.Cell(4).GetString());
                                    jury.FirstName = RemoveControlChars(row.Cell(5).GetString());
                                    jury.LastName = RemoveControlChars(row.Cell(6).GetString());
                                    jury.Designation = RemoveControlChars(row.Cell(7).GetString());
                                    jury.Company = RemoveControlChars(row.Cell(8).GetString());
                                    jury.Network = RemoveControlChars(row.Cell(9).GetString());
                                    jury.HoldingCompany = RemoveControlChars(row.Cell(10).GetString());
                                    jury.Email = RemoveControlChars(row.Cell(11).GetString());
                                    jury.Contact = RemoveControlChars(row.Cell(12).GetString());
                                    jury.Mobile = RemoveControlChars(row.Cell(13).GetString());
                                    jury.PAName = RemoveControlChars(row.Cell(14).GetString());
                                    jury.PAEmail = RemoveControlChars(row.Cell(16).GetString());
                                    jury.PATel = RemoveControlChars(row.Cell(15).GetString());
                                    jury.Country = RemoveControlChars(row.Cell(17).GetString());
                                    jury.EffieExpProgram = RemoveControlChars(row.Cell(18).GetString());
                                    jury.Source = RemoveControlChars(row.Cell(19).GetString());

                                    string effieExpereinceYears = string.Empty;
                                    
                                    effieExpereinceYears += ((RemoveControlChars(row.Cell(25).GetString()).Equals("1") || RemoveControlChars(row.Cell(26).GetString()).Equals("1")) ? "2014" : "") + "#" + RemoveControlChars(row.Cell(27).GetString()) + "|";
                                    effieExpereinceYears += "|";
                                    effieExpereinceYears += "|";

                                    jury.EffieExpYear = effieExpereinceYears;
                                    jury.Remarks = RemoveControlChars(row.Cell(28).GetString());

                                    jury.SerialNo = GeneralFunction.GetNewJuryId();
                                    jury.DateCreatedString = DateTime.Now.ToString();
                                    jury.Password = Guid.NewGuid().ToString().Substring(0, 6);


                                }
                                else
                                {
                                    bool is2014 = jury.EffieExpYear.IndexOf("2014") != -1;
                                    bool is2015 = jury.EffieExpYear.IndexOf("2015") != -1;
                                    bool is2016 = jury.EffieExpYear.IndexOf("2016") != -1;

                                    string remarks2014 = string.Empty;
                                    string remarks2015 = string.Empty;
                                    string remarks2016 = string.Empty;

                                    try
                                    {
                                        remarks2014 = jury.EffieExpYear.Split('|')[0].Split('#')[1];                                        
                                    }
                                    catch { }

                                    try
                                    {
                                        remarks2015 = jury.EffieExpYear.Split('|')[1].Split('#')[1];
                                    }
                                    catch { }

                                    try
                                    {
                                        remarks2016 = jury.EffieExpYear.Split('|')[2].Split('#')[1];
                                    }
                                    catch { }

                                    string effieExpereinceYears = string.Empty;

                                    effieExpereinceYears += ((RemoveControlChars(row.Cell(25).GetString()).Equals("1") || RemoveControlChars(row.Cell(26).GetString()).Equals("1")) ? "2014" : "") + "#" + RemoveControlChars(row.Cell(27).GetString()) + "|";
                                    effieExpereinceYears += (is2015 ? "2015" : "") + "#" + remarks2015 + "|";
                                    effieExpereinceYears += (is2016 ? "2016" : "") + "#" + remarks2016 + "|";                                    

                                    jury.EffieExpYear = effieExpereinceYears;
                                }

                                if (jury.IsValid)
                                {
                                    jury = jury.Save();
                                }

                                Invitation juryInv = Invitation.NewInvitation();
                                juryInv.JuryId = jury.Id;
                                juryInv.IsRound1Invited = RemoveControlChars(row.Cell(20).GetString()).Equals("1");
                                juryInv.IsRound2Invited = RemoveControlChars(row.Cell(21).GetString()).Equals("1");
                                juryInv.IsDeclined = RemoveControlChars(row.Cell(22).GetString()).Equals("1");
                                juryInv.IsRound1Accepted = RemoveControlChars(row.Cell(23).GetString()).Equals("1");
                                juryInv.IsRound2Accepted = RemoveControlChars(row.Cell(24).GetString()).Equals("1");
                                juryInv.IsRound1Assigned = RemoveControlChars(row.Cell(25).GetString()).Equals("1");
                                juryInv.IsRound2Assigned = RemoveControlChars(row.Cell(26).GetString()).Equals("1");

                                juryInv.EventCode = "2014";

                                if (juryInv.IsValid)
                                    juryInv.Save();

                                if (!IsSameCompanyDetails(jury, row))
                                    SaveCompanyHistory(jury, row);

                            }
                        }

                        skiprows++;
                    }
                }
                sheetNo++;
            }
        }
        else
        {
            lbError.Text += "The Sheet has invalid/not enough data.<br/>Please check your file";
        }
    }

    public bool ValidateRow(ClosedXML.Excel.IXLRangeRow CurrentRow)
    {
        if (String.IsNullOrEmpty(CurrentRow.Cell(1).GetString()) || String.IsNullOrEmpty(CurrentRow.Cell(2).GetString()) || String.IsNullOrEmpty(CurrentRow.Cell(4).GetString())
            || String.IsNullOrEmpty(CurrentRow.Cell(5).GetString()))
        {
            return false;
        }
        else
        {
            return true;               
        }
    }

    //public bool CheckIfRecordExists(ClosedXML.Excel.IXLRangeRow CurrentRow)
    //{
    //    string firstName = CurrentRow.Cell(5).GetString();
    //    string lastName = CurrentRow.Cell(6).GetString();
    //    string email = CurrentRow.Cell(12).GetString();

    //    var duplicateRecords = juryList.Where(m => (m.FirstName.Trim().ToUpper().Equals(firstName.Trim().ToUpper()) && m.LastName.Trim().ToUpper().Equals(lastName.Trim().ToUpper())) || m.Email.Trim().ToUpper().Equals(email.Trim().ToUpper()));

    //    return (duplicateRecords.Count() > 0);
    //}

    public static string RemoveControlChars(string data)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in data.Trim())
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ',' || c == '&' || c == '.' || c == '_' || c == ' ' || c == '#' || c == '-' || (c >= 33 && c <= 255) || (c == 304 || c == 305 || c == 214 || c == 246 || c == 220 || c == 252 || c == 199 || c == 231 || c == 286 || c == 287 || c == 350 || c == 351 || c == 8356))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public bool IsSameCompanyDetails(Jury jury, ClosedXML.Excel.IXLRangeRow CurrentRow)
    {
        if ((jury.Type != RemoveControlChars(CurrentRow.Cell(2).GetString())) || (!jury.Designation.Trim().ToUpper().Equals(RemoveControlChars(CurrentRow.Cell(7).GetString()).ToUpper())) || (!jury.Company.Trim().ToUpper().Equals(RemoveControlChars(CurrentRow.Cell(8).GetString()).ToUpper()))
            || (!jury.Country.Trim().ToUpper().Equals(RemoveControlChars(CurrentRow.Cell(17).GetString()).Trim().ToUpper())) || (!jury.Network.Trim().ToUpper().Equals(RemoveControlChars(CurrentRow.Cell(9).GetString()).Trim().ToUpper()))
            || (!jury.HoldingCompany.Trim().ToUpper().Equals(RemoveControlChars(CurrentRow.Cell(10).GetString()).Trim().ToUpper())))
        {
            return false;
        }


        return true;
    }

    public void SaveCompanyHistory(Jury jury, ClosedXML.Excel.IXLRangeRow CurrentRow)
    {
        CompanyHistory compnyHistroy = CompanyHistory.NewCompanyHistory();

        compnyHistroy.JuryId = jury.Id;
        compnyHistroy.Type = RemoveControlChars(CurrentRow.Cell(2).GetString());
        compnyHistroy.Designation = RemoveControlChars(CurrentRow.Cell(7).GetString());
        compnyHistroy.Company =RemoveControlChars(CurrentRow.Cell(8).GetString());

        compnyHistroy.Country = RemoveControlChars(CurrentRow.Cell(17).GetString());        
        compnyHistroy.DateModifiedString = DateTime.Now.ToString();       
        compnyHistroy.Network = RemoveControlChars(CurrentRow.Cell(9).GetString());
        compnyHistroy.HoldingCompany = RemoveControlChars(CurrentRow.Cell(10).GetString());        

        if (compnyHistroy.IsNew)
            compnyHistroy.DateCreatedString = DateTime.Now.AddYears(-3).ToString();

        if (compnyHistroy.IsValid)
            compnyHistroy.Save();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        foreach (Jury jury in JuryList.GetJuryList())
        {           
            bool is2014 = isAttendedEvent(jury,"2014");
            bool is2015 = isAttendedEvent(jury, "2015");
            bool is2016 = isAttendedEvent(jury, "2016");
            bool is2017 = isAttendedEvent(jury, "2017");

            string remarks2014 = string.Empty;
            string remarks2015 = string.Empty;
            string remarks2016 = string.Empty;
            string remarks2017 = string.Empty;

            try
            {
                remarks2014 = jury.EffieExpYear.Split('|')[0].Split('#')[1];
            }
            catch { }

            try
            {
                remarks2015 = jury.EffieExpYear.Split('|')[1].Split('#')[1];
            }
            catch { }

            try
            {
                remarks2016 = jury.EffieExpYear.Split('|')[2].Split('#')[1];
            }
            catch { }

            try
            {
                remarks2017 = jury.EffieExpYear.Split('|')[3].Split('#')[1];
            }
            catch { }

            string effieExpereinceYears = string.Empty;

            effieExpereinceYears += (is2014 ? "2014" : "") + "#" + remarks2014 + "|";
            effieExpereinceYears += (is2015 ? "2015" : "") + "#" + remarks2015 + "|";
            effieExpereinceYears += (is2016 ? "2016" : "") + "#" + remarks2016 + "|";
            effieExpereinceYears += (is2017 ? "2017" : "") + "#" + remarks2017 + "|";

            jury.EffieExpYear = effieExpereinceYears;

            if (jury.IsValid)
            {
               jury.Save();
            }
        }        
    }

    public static bool isAttendedEvent(Jury jury,string eventCode)
    {
        bool isAttended = false;

        Invitation inv = null;
        try
        {
            inv = InvitationList.GetInvitationList(jury.Id, eventCode).Single();
        }
        catch { }

        if (inv != null)
        {
            isAttended = inv.IsRound1Assigned || inv.IsRound2Assigned;
        }

        return isAttended;
    }
}