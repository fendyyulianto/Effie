using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ClosedXML.Excel;
using EffieJuryManagementApp;
using System.Text;

public partial class Main_ImportJury : PageSecurity_Admin
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
        ucGen_UploadFileForImport.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "ImportJury\\";
        ucGen_UploadFileForImport.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "ImportJury/" ;
    }

    private void PopulateForm()
    {
        if (Security.IsRoleReadOnlyAdmin())
        {
            btnSubmit.Visible = false;
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        XLWorkbook workBook;
        lbError.Text = "";

        lbError.Text += ucGen_UploadFileForImport.ValidateForm();

        if (String.IsNullOrEmpty(lbError.Text))
        {
            string fileName = DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_ss")+ "_ImportedList";

            ucGen_UploadFileForImport.SaveAndUpdateFile(fileName);

            if (ucGen_UploadFileForImport.GetValue("").ToLower() != (fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName)).ToLower())
            {
                File.Copy(ucGen_UploadFileForImport.saveDirectory + ucGen_UploadFileForImport.GetValue(fileName), ucGen_UploadFileForImport.saveDirectory + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName), true);

                if (File.Exists(ucGen_UploadFileForImport.saveDirectory + ucGen_UploadFileForImport.GetValue(fileName)))
                    GeneralFunction.DeleteFile(ucGen_UploadFileForImport.saveDirectory + ucGen_UploadFileForImport.GetValue(fileName), false);
            }

            //ucGen_UploadFileForImport.SetValue(fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName));

            workBook = new XLWorkbook(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"]+ "ImportJury\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName).ToLower());

            lbError.Text = ValidateExcelData(workBook);

            if (String.IsNullOrEmpty(lbError.Text))
            {
                ReadExcelData(workBook);
                Response.Redirect("../Main/JuryList.aspx");
            }
            else
                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "ImportJury\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName)))
                {
                    GeneralFunction.DeleteFile(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "ImportJury\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName), true);
                    //ucGen_UploadFileForImport.SetValue(string.Empty);
                }
        }
    }

    public string ValidateExcelData(XLWorkbook workBook)
    {
        string error = string.Empty;
        bool isErrorOccured = false;

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
                                Jury jury = Jury.NewJury();
                               
                                jury.Type = RemoveControlChars(row.Cell(3).GetString());
                                jury.Salutation = RemoveControlChars(row.Cell(4).GetString());
                                jury.FirstName = RemoveControlChars(row.Cell(5).GetString());
                                jury.LastName = RemoveControlChars(row.Cell(6).GetString());
                                jury.Designation = RemoveControlChars(row.Cell(8).GetString());
                                jury.Company = RemoveControlChars(row.Cell(9).GetString());
                                //jury.CompanyType = RemoveControlChars(row.Cell(9).GetString());
                                jury.Network = RemoveControlChars(row.Cell(10).GetString());
                                jury.HoldingCompany = RemoveControlChars(row.Cell(11).GetString());
                                jury.Email = RemoveControlChars(row.Cell(12).GetString());
                                jury.Contact = RemoveControlChars(row.Cell(13).GetString());                               
                                jury.Mobile = RemoveControlChars(row.Cell(14).GetString());
                                jury.PAName = RemoveControlChars(row.Cell(15).GetString());
                                jury.PAEmail = RemoveControlChars(row.Cell(16).GetString());
                                jury.PATel = RemoveControlChars(row.Cell(17).GetString());                                
                                jury.Address1 = RemoveControlChars(row.Cell(18).GetString());
                                jury.Address2 = RemoveControlChars(row.Cell(19).GetString());
                                jury.City = RemoveControlChars(row.Cell(20).GetString());
                                jury.Postal = RemoveControlChars(row.Cell(21).GetString());
                                jury.Country = RemoveControlChars(row.Cell(22).GetString());
                                //jury.MarketExp = RemoveControlChars(row.Cell(23).GetString());
                                //jury.IndustryExp = RemoveControlChars(row.Cell(24).GetString());
                                jury.EffieExp = RemoveControlChars(row.Cell(25).GetString());
                                //jury.OtherJudgingExp = RemoveControlChars(row.Cell(28).GetString());
                                jury.Remarks = RemoveControlChars(row.Cell(26).GetString());
                                jury.Reference = RemoveControlChars(row.Cell(27).GetString());
                                jury.Source = RemoveControlChars(row.Cell(28).GetString());
                                jury.LastUpdate = RemoveControlChars(row.Cell(29).GetString()); 

                                jury.DateModifiedString = DateTime.Now.ToString();

                                                                                                                               
                                if (!jury.IsValid)
                                {
                                    error += "Data is not valid in row: " + (row.RowNumber() -1).ToString() + "[" + jury.BrokenRulesCollection.ToString() + "]";
                                    isErrorOccured = true;
                                    break;
                                }

                            }
                            else
                            {
                                error += "Data is not valid in row: " + (row.RowNumber() - 1).ToString();
                                isErrorOccured = true;
                                break;
                            }
                        }

                        skiprows++;


                    }
                }
                sheetNo++;

                if (isErrorOccured)
                    break;
            }            
        }
        else
            error += "Sheet contains no data.<br/>";

        return error;
    }

    public void ReadExcelData(XLWorkbook workBook)
    {        
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
                                Jury jury = Jury.NewJury();
                               
                                jury.Type = RemoveControlChars(row.Cell(3).GetString());
                                jury.Salutation = RemoveControlChars(row.Cell(4).GetString());
                                jury.FirstName = RemoveControlChars(row.Cell(5).GetString());
                                jury.LastName = RemoveControlChars(row.Cell(6).GetString());
                                jury.Designation = RemoveControlChars(row.Cell(8).GetString());
                                jury.Company = RemoveControlChars(row.Cell(9).GetString());
                                //jury.CompanyType = RemoveControlChars(row.Cell(9).GetString());
                                jury.Network = RemoveControlChars(row.Cell(10).GetString());
                                jury.HoldingCompany = RemoveControlChars(row.Cell(11).GetString());
                                jury.Email = RemoveControlChars(row.Cell(12).GetString());
                                jury.Contact = RemoveControlChars(row.Cell(13).GetString());
                                jury.Mobile = RemoveControlChars(row.Cell(14).GetString());
                                jury.PAName = RemoveControlChars(row.Cell(15).GetString());
                                jury.PAEmail = RemoveControlChars(row.Cell(16).GetString());
                                jury.PATel = RemoveControlChars(row.Cell(17).GetString());
                                jury.Address1 = RemoveControlChars(row.Cell(18).GetString());
                                jury.Address2 = RemoveControlChars(row.Cell(19).GetString());
                                jury.City = RemoveControlChars(row.Cell(20).GetString());
                                jury.Postal = RemoveControlChars(row.Cell(21).GetString());
                                jury.Country = RemoveControlChars(row.Cell(22).GetString());
                                
                                string effieExpereinceYears = string.Empty;

                                effieExpereinceYears += "#" + RemoveControlChars(row.Cell(25).GetString()) + "|";
                                effieExpereinceYears += "#" + RemoveControlChars(row.Cell(24).GetString()) + "|";
                                effieExpereinceYears += "#" + RemoveControlChars(row.Cell(23).GetString()) + "|";                                

                                jury.EffieExpYear = effieExpereinceYears;

                                jury.EffieExp = RemoveControlChars(row.Cell(26).GetString());
                                
                                jury.Remarks = RemoveControlChars(row.Cell(27).GetString());
                                jury.Reference = RemoveControlChars(row.Cell(28).GetString());
                                jury.Source = RemoveControlChars(row.Cell(29).GetString());
                                jury.LastUpdate = RemoveControlChars(row.Cell(30).GetString()); 

                                if (jury.IsNew)
                                {
                                    jury.SerialNo = GeneralFunction.GetNewJuryId();
                                    jury.DateCreatedString = DateTime.Now.ToString();
                                    jury.Password = Guid.NewGuid().ToString().Substring(0, 6);
                                }

                                if (jury.IsValid)
                                {
                                    jury = jury.Save();
                                }
                                
                            }
                            //else
                            //    return;
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
        if (String.IsNullOrEmpty(CurrentRow.Cell(3).GetString()) || String.IsNullOrEmpty(CurrentRow.Cell(5).GetString()) || String.IsNullOrEmpty(CurrentRow.Cell(12).GetString())
            || String.IsNullOrEmpty(CurrentRow.Cell(22).GetString()))
        {
            return false;
        }
        else
        {
            if (EffieJuryManagementApp.JuryList.CheckIfRecordExists(CurrentRow.Cell(5).GetString(), CurrentRow.Cell(6).GetString(), CurrentRow.Cell(12).GetString(),false,Guid.Empty))
                return false;
            else
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
}