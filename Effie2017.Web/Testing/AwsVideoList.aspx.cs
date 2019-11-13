using ClosedXML.Excel;
using Effie2017.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_AwsVideoList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }



    protected void btnReport_Click(object sender, EventArgs e)
    {
        int x = 1;
        int y = 1;
        string sheetName = "AWS Video";
        XLWorkbook workbook = new XLWorkbook();
        MemoryStream memoryStream = new MemoryStream();
        workbook.Worksheets.Add(sheetName);

        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Entry ID"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("URL"); x++;

        y++;
        {
            List<Entry> Entrylist = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "")
                .Where(o => !string.IsNullOrEmpty(o.Serial)).ToList();
            foreach (Entry entry in Entrylist)
            {
                if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\CreativeVideo\\" + entry.Serial + "_CreativeMaterials_Video.mp4"))
                //(GeneralFunction.FileExistsInAmazonS3(System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"], entry.Serial + "_CreativeMaterials_Video.mp4"))
                {
                    PopulateExcelRow(ref workbook, ref y, ref x, ref sheetName, entry);
                }
            }
        }

        workbook.SaveAs(memoryStream);

        Response.Clear();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=AWS Video Report.xlsx");

        memoryStream.WriteTo(Response.OutputStream);
        Response.End();
    }


    protected XLWorkbook PopulateExcelRow(ref XLWorkbook workbook, ref int y, ref int x, ref string sheetName, Entry entry)
    {
        string url = System.Configuration.ConfigurationManager.AppSettings["AWSS3WebURL"] + System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"] + "/" +
                        entry.Serial + "_CreativeMaterials_Video.mp4?" + DateTime.Now.Ticks.ToString();
        x = 1;
        Registration reg = Registration.GetRegistration(entry.RegistrationId);
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(entry.Serial); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Firstname); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Lastname); x++;
        //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(reg.Email); x++;
        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(url); x++;
        y++;
        return workbook;
    }
}