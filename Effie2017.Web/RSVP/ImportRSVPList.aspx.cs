using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ClosedXML.Excel;
using Effie2017.App;
using System.Text;

public partial class RSVP_ImportRSVPList : System.Web.UI.Page
{
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
        ucGen_UploadFileForImport.saveDirectory = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "RSVPList\\";
        ucGen_UploadFileForImport.virtualDirectory = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "RSVPList/";
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

            workBook = new XLWorkbook(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "RSVPList\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName).ToLower());

            lbError.Text = ValidateExcelData(workBook);

            if (String.IsNullOrEmpty(lbError.Text))
            {
                ReadExcelData(workBook);                
            }
            else
                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "RSVPList\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName)))
                {
                    GeneralFunction.DeleteFile(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"] + "RSVPList\\" + fileName + "." + ucGen_UploadFileForImport.GetValueExtension(fileName), true);
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
                            if (row.Cell(1).GetString().Trim() != "" && ValidateRow(row))
                            {
                                RSVP rsvp = RSVP.NewRSVP();

                                /*rsvp.Salutation = RemoveControlChars(row.Cell(3).GetString());
                                rsvp.FirstName = RemoveControlChars(row.Cell(4).GetString());
                                rsvp.LastName = RemoveControlChars(row.Cell(5).GetString());
                                rsvp.Email = RemoveControlChars(row.Cell(2).GetString());
                                rsvp.Company = RemoveControlChars(row.Cell(9).GetString());
                                rsvp.Location = (RemoveControlChars(row.Cell(8).GetString()).Equals("Singapore") ? "Local" : "Overseas");
                                rsvp.UserData1 = RemoveControlChars(row.Cell(6).GetString());
                                rsvp.UserData2 = RemoveControlChars(row.Cell(1).GetString());
                                rsvp.UserData3 = RemoveControlChars(row.Cell(8).GetString());*/

                                rsvp.Type = RemoveControlChars(row.Cell(1).GetString());
                                //rsvp.Salutation = RemoveControlChars(row.Cell(3).GetString());
                                rsvp.FirstName = RemoveControlChars(row.Cell(2).GetString());
                                rsvp.LastName = RemoveControlChars(row.Cell(3).GetString());
                                rsvp.UserData1 = RemoveControlChars(row.Cell(4).GetString());
                                rsvp.Company = RemoveControlChars(row.Cell(5).GetString());
                                //rsvp.UserData3 = RemoveControlChars(row.Cell(6).GetString());                                                             
                                rsvp.Salutation = RemoveControlChars(row.Cell(6).GetString());
                                rsvp.Email = RemoveControlChars(row.Cell(7).GetString());
                                //rsvp.Round1PanelID = RemoveControlChars(row.Cell(14).GetString());
                                //rsvp.Round2PanelID = RemoveControlChars(row.Cell(15).GetString());

                                if (!rsvp.IsValid)
                                {
                                    //error += "Data is not valid in row: " + (row.RowNumber() - 1).ToString() + "[" + rsvp.BrokenRulesCollection.ToString() + "]";
                                    //isErrorOccured = true;
                                    break;
                                }

                            }
                            else
                            {
                                //error += "Data is not valid in row: " + (row.RowNumber() - 1).ToString();
                                //isErrorOccured = true;
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
                            if (row.Cell(1).GetString().Trim() != "" && ValidateRow(row))
                            {
                                RSVP rsvp = RSVP.NewRSVP();

                                rsvp.Type = RemoveControlChars(row.Cell(1).GetString());
                                //rsvp.Salutation = RemoveControlChars(row.Cell(3).GetString());
                                rsvp.FirstName = RemoveControlChars(row.Cell(2).GetString());
                                rsvp.LastName = RemoveControlChars(row.Cell(3).GetString());
                                rsvp.UserData1 = RemoveControlChars(row.Cell(4).GetString());
                                rsvp.Company = RemoveControlChars(row.Cell(5).GetString());
                                //rsvp.UserData3 = RemoveControlChars(row.Cell(6).GetString());                                                             
                                rsvp.Salutation = RemoveControlChars(row.Cell(6).GetString());
                                rsvp.Email = RemoveControlChars(row.Cell(7).GetString());                                                                
                                //rsvp.Round1PanelID = RemoveControlChars(row.Cell(14).GetString());
                                //rsvp.Round2PanelID = RemoveControlChars(row.Cell(15).GetString());

                                rsvp.IsInvitingGalaDinner = true;

                                rsvp.WorkflowStatus = "01";
                                rsvp.DateCreatedString = DateTime.Now.ToString();

                                if (rsvp.IsValid)
                                {
                                    rsvp = rsvp.Save();
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
        if (String.IsNullOrEmpty(CurrentRow.Cell(1).GetString()) || String.IsNullOrEmpty(CurrentRow.Cell(2).GetString()) || String.IsNullOrEmpty(CurrentRow.Cell(3).GetString()) || String.IsNullOrEmpty(CurrentRow.Cell(7).GetString()))
        {
            return false;
        }
        else
            return true;                   
    }
   
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