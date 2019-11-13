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

public partial class Testing_FileZip : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DeleteFolder("TempZip");
        DeleteFolder("EntryUpload/EntryForm/");
        
        var entryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "", StatusEntry.Completed).ToList();

        foreach (Entry entry in entryList)
        {
            GenerateEF(entry);
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

            CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\Authorisation\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");
            CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\Case\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");
            CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\Creative\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");
            //CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\Entry\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");
            CopyAll(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\EntryForm\\" + entry.Id.ToString() + "\\", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\");

            CopySingle(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "EntryUpload\\CreativeVideo\\" + entry.Serial.ToString() + "_CreativeMaterials_Video.mp4", System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\" + multiSingle + "\\" + productSpecialty + "\\" + cat + "\\" + entry.Serial + "\\" + entry.Serial.ToString() + "_CreativeMaterials_Video.mp4");
        }

        //preparing zipping program (7-zip)
        System.Diagnostics.Process Proc = new System.Diagnostics.Process();

        Proc.StartInfo.WorkingDirectory = System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "CompressorProgram\\";
        Proc.StartInfo.Arguments = "a \"" + System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export.zip\" \"" + System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export\\*\"";
        Proc.StartInfo.FileName = "7za.exe";

        //zipping files
        try
        {
            Proc.Start();
            Proc.WaitForExit();

            if (Proc.ExitCode == 0) //SUCCESSED
            {
                //cleaning temp
                if (Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export"))
                    Directory.Delete(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + "TempZip\\Files Export", true);

                //Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["storageVirtualPath"].ToString() + "TempZip/Files Export.zip", true);
            }
            else
            {
                Response.Write(Proc.ExitCode.ToString());
                return;
            }

            Proc.Dispose();
        }
        catch (Exception exp)
        {
            Response.Write(exp.Message);
            return;
        }

        //DeleteFolder("EntryUpload/EntryForm/");
    }

    protected void DeleteFolder(string path)
    {
        if (Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + path))
            Directory.Delete(System.Configuration.ConfigurationManager.AppSettings["storagePhysicalPath"].ToString() + path, true);
    }

    protected void GenerateEF(Entry entry)
    {
        string storagePhysicalPath = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"];
        EntryForm entryForm = EntryFormList.GetEntryFormList().FirstOrDefault(x => x.IdEntry == entry.Id);
        if(entryForm != null)
        {
            MemoryStream memoryStream = new System.IO.MemoryStream();
            string path = storagePhysicalPath + "EntryUpload/EntryForm/"+ entry.Id;
            string Filename = entry.Serial + "_" + GeneralFunctionEntryForm.GetEntryCategory(entry) + ".pdf";
            string Fullpath = Path.Combine(path, Filename);
            try
            {
                using (memoryStream = GeneralFunctionEntryForm.Classification(entryForm, entry))
                {
                    Document myDocument = new Document();
                    PdfWriter myPDFWriter = PdfWriter.GetInstance(myDocument, memoryStream);
                    myDocument.Open();

                    byte[] content = memoryStream.ToArray();

                    var exists = Directory.Exists(path);
                    if (!exists)
                        System.IO.Directory.CreateDirectory(path);

                    // Write out PDF from memory stream.
                    using (FileStream fs = File.Create(Fullpath))
                    {
                        fs.Write(content, 0, (int)content.Length);
                    }
                }
            }
            catch
            {
                Response.Write("ERROR ==>  (entryForm.Id:" + entryForm.Id + ") - " + Fullpath + "<br>");
            }
        }
    }

    protected void CopySingle(string from, string to)
    {
        if (File.Exists(from))
        {
            File.Copy(from,to);
        }
    }

    protected void CopyAll(string from, string to)
    {
        if (!Directory.Exists(to))
            Directory.CreateDirectory(to);

        if (Directory.Exists(from))
        {
            string[] allDirectories = Directory.GetDirectories(from);

            foreach (string directory in allDirectories)
            {
                CopyAll(directory, to + directory.Substring(directory.LastIndexOf("\\")));
            }

            string[] allFiles = Directory.GetFiles(from);

            foreach (string file in allFiles)
            {
                File.Copy(file, to + file.Substring(file.LastIndexOf("\\")));
            }
        }
    }
}