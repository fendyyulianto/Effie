using Effie2017.App;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_GenerateEF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        string storagePhysicalPath = System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"];
        List<Entry> dbentryList = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "").ToList();
        List<EntryForm> dbentryFormList = EntryFormList.GetEntryFormList().Where(x => x.Status == StatusEntry.Completed).ToList();
        foreach (EntryForm entryForm in dbentryFormList)
        {
            Entry entry = dbentryList.FirstOrDefault(x => x.Id == entryForm.IdEntry);
            MemoryStream memoryStream = new System.IO.MemoryStream();
            string path = storagePhysicalPath + "EntryUpload/EntryForm";
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
}