using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Effie2017.App;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Testing_RevertBackFiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var entryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.Completed).ToList();

        foreach (Entry entry in entryList)
        {
            string multiSingle = "";
            string productSpecialty = "";
            string cat = "";

            if (entry.CategoryMarket == "MM")
                multiSingle = "Multi Market";
            else if (entry.CategoryMarket == "SM")
                multiSingle = "Single Market";

            if (entry.CategoryPS == "PSC")
                productSpecialty = "Product & Services Category";
            else if (entry.CategoryPS == "SC")
                productSpecialty = "Specialty Category";

            cat = entry.CategoryPSDetail.Replace("/", "");

            if (entry.CategoryMarket == "MM")
                multiSingle = "Multi Market";
            else if (entry.CategoryMarket == "SM")
                multiSingle = "Single Market";

            string From = From = System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\";

            CheckFiles(entry);

            //if (Directory.Exists(From))
            //{
            //    //CopyAll(From, entry.Id);
            //}
            //else
            //{
            //    Response.Write("Error : Folder Not Found #### "+ From + " ####;<br>");
            //}                
        }
    }


    protected void CheckFiles(Entry entry)
    {
        string To = "";
        string Default = System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\";

        Response.Write(entry.Serial + "####");

        if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\" + entry.Serial + "_AuthorizationForm_PDF.pdf"))
            Response.Write("1####");
        else
            Response.Write("0####");

        if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CaseImage.jpg"))
            Response.Write("1####");
        else
            Response.Write("0####");

        Response.Write(entry.CreativeUploadType + "####");

        if (File.Exists(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\" + entry.Serial + "_CreativeMaterials_PDF.pdf"))
            Response.Write("1####");
        else
            Response.Write("0####");


        Response.Write("<br>");

    }

    protected void CopyAll(string from, Guid EntryId)
    {
        string To = "";
        string Default = System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\";
        if (Directory.Exists(from))
        {
            string[] allFiles = Directory.GetFiles(from);

            foreach (string file in allFiles)
            {
                try
                {
                    FileInfo fileinfo = new FileInfo(file);

                    To = Default;
                    if (fileinfo.Name.Contains("_AuthorizationForm_PDF"))
                    {
                        To += "Authorisation\\" + EntryId.ToString() + "\\" + fileinfo.Name;
                        File.Copy(file, To);
                    }
                    else if (fileinfo.Name.Contains("_CaseImage"))
                    {
                        To += "Case\\" + EntryId.ToString() + "\\" + fileinfo.Name;
                        File.Copy(file, To);
                    }
                    else if (fileinfo.Name.Contains("_CreativeMaterials_PDF"))
                    {
                        To += "Creative\\" + EntryId.ToString() + "\\" + fileinfo.Name;
                        File.Copy(file, To);
                    }

                    Response.Write(EntryId + " : " + To + "<br>");
                }
                catch (Exception ex)
                {
                    Response.Write("ERROR : " + To + "#####" + ex.Message + "#####<br>");
                }
            }
        }
    }
}