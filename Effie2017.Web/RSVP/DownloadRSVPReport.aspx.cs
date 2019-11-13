using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;

public partial class RSVP_DownloadRSVPReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    public void LoadForm()
    {

    }

    public void PopulateForm()
    {

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        lblError.Text = string.Empty;

        lblError.Text += IptechLib.Validation.ValidateTextBox("Security Token", txtSecurityCode, true, IptechLib.ValidationType.String);

        if (String.IsNullOrEmpty(lblError.Text))
        {
            if (!IptechLib.Validation.ValidateGuid(txtSecurityCode.Text))
                lblError.Text += "Please enter valid Security Token.<br/>";
            else
            {
                Guid SecurityToken = Guid.Parse(txtSecurityCode.Text);

                if (SecurityToken.Equals(GeneralFunction.GetSecurityCodeForRSVPReport()))
                {
                    // lbError.Text = "";
                    XLWorkbook workbook = new XLWorkbook();
                    MemoryStream memoryStream = new MemoryStream();

                    // Downloading WorkBook        
                    RSVPReport(workbook).SaveAs(memoryStream);

                    ResetForm();
                    
                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=Effie_RSVP.xlsx");

                    memoryStream.WriteTo(Response.OutputStream);
                    Response.End();
                }
                else
                {
                    lblError.Text += "Please enter valid Security Token.<br/>";
                }
            }
        }
    }

    #region Helper

    public void ResetForm()
    {
        txtSecurityCode.Text = string.Empty;
    }

    protected XLWorkbook RSVPReport(XLWorkbook xlWorkBook)
    {
        List<string> sheetTypes = new List<string>();
        sheetTypes.Add("RSVP - Welcome Dinner");
        sheetTypes.Add("RSVP - Gala Dinner");

        foreach (string sheetType in sheetTypes)
        {
            int x = 1;
            int y = 1;
            string sheetName = sheetType;
            xlWorkBook.Worksheets.Add(sheetName);
            x = 1;

            bool isGalaDinner = sheetType.Equals("RSVP - Gala Dinner");

            if (isGalaDinner)
            {
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Type"); x++;
            }

            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Salutation"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("First Name"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Last Name"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Job Title"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company Name"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Respond Status"); x++;

            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Round 1 Panel ID"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Round 2 Panel ID"); x++;

            if (isGalaDinner)
            {                                
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Is Attending Gala Dinner"); x++;
            }
            else
            {
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Jury Cocktail"); x++;
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Welcome Dinner"); x++;
            }

            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Meal Restrictions"); x++;
            xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Date Responded"); x++;

            y++;

            var rSVPList = Effie2017.App.RSVPList.GetRSVPList().ToList();

            rSVPList = rSVPList.Where(m => isGalaDinner ? m.IsInvitingGalaDinner : !m.IsInvitingGalaDinner).ToList();

            foreach (Effie2017.App.RSVP rSVP in rSVPList)
            {
                x = 1;

                DateTime dateResponded = DateTime.MinValue;

                DateTime.TryParse(rSVP.DateModified.ToString(), out dateResponded);

                if (isGalaDinner)
                {
                    xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.Type); x++;
                }

                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.Salutation); x++;
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.FirstName); x++;
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.LastName); x++;
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.UserData1); x++;
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.Company); x++;
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.Email); x++;

                if (rSVP.WorkflowStatus == "01")
                    xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Not Sent");
                else if (rSVP.WorkflowStatus == "02")
                    xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Sent");
                else if (rSVP.WorkflowStatus == "03")
                    xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Read");
                else if (rSVP.WorkflowStatus == "04")
                    xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Responded");

                x++;

                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.UserData3); x++;
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.Round1PanelID); x++;
                xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.Round2PanelID); x++;

                if (rSVP.Respond)
                {
                    if (isGalaDinner)
                    {                        
                        xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.IsGalaDinner ? "Yes" : "No"); x++;
                    }
                    else
                    {
                        xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.IsJuryCocktail ? "Yes" : "No"); x++;
                        xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.IsWelcomeDinner ? "Yes" : "No"); x++;
                    }

                    xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(rSVP.Dietary); x++;
                    xlWorkBook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(dateResponded.Equals(DateTime.MinValue) ? string.Empty : dateResponded.ToString("dd/MM/yyyy hh:mm")); x++;
                }                


                y++;
            }
        }

        return StyleReport(xlWorkBook);        
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
}