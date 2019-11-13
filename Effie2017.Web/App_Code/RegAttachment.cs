using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text.pdf;
using Effie2017.App;
using iTextSharp.text;

/// <summary>
/// Summary description for Invoice
/// </summary>
public class RegAttachment
{
	public RegAttachment()
	{
	}
    public static MemoryStream GenerateGalaReceipt(GalaOrder go)
    {
        PdfReader pdfReader = null;
        MemoryStream memoryStreamPdfStamper = null;
        PdfStamper pdfStamper = null;
        AcroFields pdfFormFields = null;

        pdfReader = new PdfReader(System.Configuration.ConfigurationSettings.AppSettings["PdfTemplateLocation"] + "Invoice Template Gala.pdf");
        memoryStreamPdfStamper = new MemoryStream();
        pdfStamper = new PdfStamper(pdfReader, memoryStreamPdfStamper);
        pdfFormFields = pdfStamper.AcroFields;


        // Form filling
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Basic information
        string customerinfo = "";
        customerinfo += go.PayFirstname + " " + go.PayLastname + "\r\n";
        customerinfo += go.PayCompany + "\r\n";
        customerinfo += go.PayAddress1 + "\r\n";
        customerinfo += go.PayAddress2 + "\r\n";
        customerinfo += go.PayCity;
        if (go.PayCity.Trim() != "") customerinfo += " ";
        customerinfo += go.PayPostal + "\r\n";
        customerinfo += go.PayCountry + "\r\n";

        pdfFormFields.SetField("customer", customerinfo);

        bool isPP = false;
        bool isBank = false;
        int rowcounter = 1;
        decimal total = 0;
        string invno = "";
        //foreach (Entry entry in entries)
        //{
        string name = "Gala Order, Tables: " + go.TableCount.ToString() + " Seats: " + go.SeatCount.ToString();
        if (go.SeatCount == 0)
        {
            name = "Gala Order, Tables: " + go.TableCount.ToString();
        }
        else if (go.TableCount == 0)
        {
            name = "Gala Order, Seats: " + go.SeatCount.ToString();
        }
        PopulateFeeRow(rowcounter, pdfFormFields, name, go.Amount, 0, 1, false, ""); 
        rowcounter++;

        if (go.FeeShipping > 0)
        {
            PopulateFeeRow(rowcounter, pdfFormFields, "Shipping Fee", go.FeeShipping, 0, 1, false, ""); 
            rowcounter++;
        }

        invno = go.Invoice;

        pdfFormFields.SetField("date", go.DateCreated.ToString("dd MMM yyyy"));
        //}

        pdfFormFields.SetField("invno", invno);

        decimal fees = go.Fee;
        pdfFormFields.SetField("st2", "S$ " + fees.ToString("N"));

        total = go.Amount + go.FeeShipping + fees;
        pdfFormFields.SetField("st1", "S$ " + total.ToString("N"));


        decimal tax = go.Tax;
        pdfFormFields.SetField("st4", "S$ " + tax.ToString("N"));

        decimal grandtotal = total;
        grandtotal += tax;
        pdfFormFields.SetField("st3", "S$ " + grandtotal.ToString("N"));

        // dummy
        pdfFormFields.SetField("blank", " ");


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





        pdfStamper.FormFlattening = true;
        pdfStamper.Writer.CloseStream = false;
        pdfStamper.Close();

        // test
        memoryStreamPdfStamper.Flush();
        memoryStreamPdfStamper.Seek(0, SeekOrigin.Begin);

        return memoryStreamPdfStamper;
    }

    public static MemoryStream GenerateGroupReceipt(Registration reg, Guid paymentGroupId)
    {
        EntryList entries = EntryList.GetEntryList(paymentGroupId, reg.Id, "");
        PdfReader pdfReader = null;
        PdfStamper pdfStamper = null;
        AcroFields pdfFormFields = null;
        List<MemoryStream> memoryStreamPdfStamperList = new List<MemoryStream>();
        MemoryStream memoryStreamPdfStamper = null;

        bool isPP = false;
        bool isBank = false;
        int rowcounter = 1;
        decimal total = 0;
        decimal fees = 0;
        decimal tax = 0;
        decimal grandtotal = 0;
        int CountEntry = 1;
        int Pages = 1;
        int MuchRow = 22;
        string TamplatePDF = "";
        //isPP = (entry.PaymentMethod == PaymentType.PayPal);
        //isBank = (entry.PaymentMethod == PaymentType.BankTransfer);
        // Form filling
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        foreach (Entry entry in entries)
        {
            if ((CountEntry % (MuchRow + 1)) == 0 || CountEntry == 1)
            {
                if ((entries.Count <= MuchRow) || ((entries.Count > MuchRow) && ((entries.Count - CountEntry) <= MuchRow)))
                {
                    TamplatePDF = "Invoice Template 2.pdf";
                    
                }
                else if (entries.Count > MuchRow)
                {
                    TamplatePDF = "Invoice Template 1.pdf";
                }

                pdfReader = new PdfReader(System.Configuration.ConfigurationSettings.AppSettings["PdfTemplateLocation"] + TamplatePDF);
                memoryStreamPdfStamper = null;
                memoryStreamPdfStamper = new MemoryStream();
                pdfStamper = new PdfStamper(pdfReader, memoryStreamPdfStamper);
                pdfFormFields = pdfStamper.AcroFields;

                rowcounter = 1;

                string customerinfo = "";
                customerinfo += reg.Salutation + " " + reg.Firstname + " " + reg.Lastname + "\r\n";
                customerinfo += reg.Job + "\r\n";
                customerinfo += reg.Address1 + "\r\n";
                customerinfo += reg.Address2 + "\r\n";
                customerinfo += reg.City;
                if (reg.City.Trim() != "") customerinfo += " ";
                customerinfo += reg.Postal + "\r\n";
                customerinfo += reg.Country + "\r\n";

                if (entries.Count > 0)
                {
                    customerinfo = "";
                    customerinfo += entries[0].PayFirstname + " " + entries[0].PayLastname + "\r\n";
                    customerinfo += entries[0].PayCompany + "\r\n";
                    customerinfo += entries[0].PayAddress1 + "\r\n";
                    if (entries[0].PayAddress2.Trim() != "") customerinfo += entries[0].PayAddress2 + "\r\n";
                    customerinfo += entries[0].PayCity;
                    if (entries[0].PayCity.Trim() != "") customerinfo += " ";
                    customerinfo += entries[0].PayPostal + "\r\n";
                    customerinfo += entries[0].PayCountry + "\r\n";
                }

                { //HEADER
                    pdfFormFields.SetField("invno", entry.Invoice);
                    pdfFormFields.SetField("date", entry.DateSubmitted.ToString("dd MMM yyyy"));
                    pdfFormFields.SetField("customer", customerinfo);
                }
            }

            /////////////////////////////////////////////////////////////////////////////////////

            PopulateRow(rowcounter, pdfFormFields, entry.Serial, entry.Amount, entry.Campaign);
            
            total += entry.Amount;
            fees += entry.Fee;
            tax += entry.Tax;
            grandtotal += entry.GrandAmount;
            
            rowcounter++;

            /////////////////////////////////////////////////////////////////////////////////////
            
            { //FOTTER
                pdfFormFields.SetField("st1", "S$ " + (total + fees).ToString("N"));
                pdfFormFields.SetField("st2", "S$ " + fees.ToString("N"));
                pdfFormFields.SetField("st3", "S$ " + grandtotal.ToString("N"));
                pdfFormFields.SetField("st4", "S$ " + tax.ToString("N"));
                pdfFormFields.SetField("page", Pages.ToString());
                pdfFormFields.SetField("blank", " ");
            }

            if (((CountEntry % 22) == 0) || (CountEntry == entries.Count))
            {
                pdfStamper.FormFlattening = true;
                pdfStamper.Writer.CloseStream = false;
                pdfStamper.Close();
                memoryStreamPdfStamperList.Add(memoryStreamPdfStamper);
                Pages++;
            }

            CountEntry++;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////


        MemoryStream docstream = CreatePdfDoc(memoryStreamPdfStamperList);

        foreach (MemoryStream memoryStreamPdfStamper1 in memoryStreamPdfStamperList)
            memoryStreamPdfStamper1.Dispose();

        docstream.Position = 0;
        return docstream;
    }

    public static MemoryStream GenerateEntryDetailsSummary(Registration reg, Guid paymentGroupId)
    {
        EntryList entries = EntryList.GetEntryList(paymentGroupId, reg.Id, "");

        PdfReader pdfReader = null;
        
        PdfStamper pdfStamper = null;
        AcroFields pdfFormFields = null;

        List<MemoryStream> memoryStreamPdfStamperList = new List<MemoryStream>();

        int rowcounter = 0;
        foreach (Entry entry in entries)
        {
            MemoryStream memoryStreamPdfStamper = null;
            pdfReader = new PdfReader(System.Configuration.ConfigurationSettings.AppSettings["PdfTemplateLocation"] + "ENTRY DETAILS.pdf");
            memoryStreamPdfStamper = new MemoryStream();
            pdfStamper = new PdfStamper(pdfReader, memoryStreamPdfStamper);
            pdfFormFields = pdfStamper.AcroFields;


            // Form filling
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            PopulateFeeRow(rowcounter, pdfFormFields, entry.Campaign, entry.Amount, 0, 1, false, "");

            pdfFormFields.SetField("ENTRY ID", entry.Serial);
            pdfFormFields.SetField("CAMPAIGN TITLE", entry.Campaign);
            pdfFormFields.SetField("CLIENT NAME", entry.Client);
            pdfFormFields.SetField("BRAND NAME", entry.Brand);
            pdfFormFields.SetField("CATEGORY", entry.CategoryPSDetail);
            
            rowcounter++;

            pdfStamper.FormFlattening = true;
            pdfStamper.Writer.CloseStream = false;
            pdfStamper.Close();

            memoryStreamPdfStamperList.Add(memoryStreamPdfStamper);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        MemoryStream docstream = CreatePdfDoc(memoryStreamPdfStamperList);
        
        foreach (MemoryStream memoryStreamPdfStamper in memoryStreamPdfStamperList)
            memoryStreamPdfStamper.Dispose();

        docstream.Position = 0;
        return docstream;

    }


    private static AcroFields PopulateRow(int rowcounter, AcroFields pdfFormFields, string Serial, decimal Amount, string Campaign)
    {
        string fieldsno = rowcounter.ToString() + "a";
        string fieldSerial = rowcounter.ToString() + "b";
        string fieldCampaign = rowcounter.ToString() + "c";
        string fieldtotal = rowcounter.ToString() + "d";
        decimal feetotal = Amount;

        pdfFormFields.SetField(fieldtotal, "S$ " + feetotal.ToString("N"));
        pdfFormFields.SetField(fieldsno, rowcounter.ToString());
        pdfFormFields.SetField(fieldSerial, Serial);
        pdfFormFields.SetField(fieldCampaign, Campaign);
        
        return pdfFormFields;
    }

    private static decimal PopulateFeeRow(int counter, AcroFields pdfFormFields, string name, decimal price, decimal priceEB, int feecount, bool isearlybird, string priceoverride)
    {
        string fieldsno = "no" + counter.ToString();
        string fieldname = "des" + counter.ToString();
        //string fieldqty = "qty" + counter.ToString();
        //string fieldunit = "unit" + counter.ToString();
        string fieldtotal = "amt" + counter.ToString();

        pdfFormFields.SetField(fieldsno, counter.ToString());
        pdfFormFields.SetField(fieldname, GeneralFunction.ConvertHTMLBreaksToBreaks(name));
        //pdfFormFields.SetField(fieldqty, feecount.ToString());
        //pdfFormFields.SetField(fieldunit, "S$ " + price.ToString("N"));
        //if (isearlybird) pdfFormFields.SetField(fieldunit, "S$ " + priceEB.ToString("N"));
        decimal feetotal = feecount * price;
        if (isearlybird) feetotal = feecount * priceEB;
        pdfFormFields.SetField(fieldtotal, "S$ " + feetotal.ToString("N"));

        if (priceoverride.Trim() != "")
        {
            pdfFormFields.SetField(fieldtotal, priceoverride);
            return 0; // return 0 because its paid or complimentary
        }
            

        return feetotal;
    }
    private static MemoryStream CreatePdfDoc(MemoryStream memoryStreamPdfStamper)
    {
        iTextSharp.text.Document document = new iTextSharp.text.Document();
        MemoryStream memoryStreamDocument = new MemoryStream();
        PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStreamDocument);
        document.Open();
        pdfWriter.CloseStream = false;

        PdfContentByte pdfContentByte = pdfWriter.DirectContent;
        document.NewPage();
        PdfImportedPage pdfImportedPage = pdfWriter.GetImportedPage(new PdfReader(memoryStreamPdfStamper.GetBuffer()), 1);
        pdfContentByte.AddTemplate(pdfImportedPage, 0, 0);
        memoryStreamPdfStamper.Dispose();

        document.Close();

        return memoryStreamDocument;
    }
    public static MemoryStream CreatePdfDoc(List<MemoryStream> memoryStreamPdfStamperList)
    {
        iTextSharp.text.Document document = new iTextSharp.text.Document();
        MemoryStream memoryStreamDocument = new MemoryStream();
        PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStreamDocument);
        document.Open();
        pdfWriter.CloseStream = false;

        foreach (MemoryStream memoryStreamPdfStamper in memoryStreamPdfStamperList)
        {
            PdfContentByte pdfContentByte = pdfWriter.DirectContent;
            document.NewPage();
            PdfImportedPage pdfImportedPage = pdfWriter.GetImportedPage(new PdfReader(memoryStreamPdfStamper.GetBuffer()), 1);
            pdfContentByte.AddTemplate(pdfImportedPage, 0, 0);
            memoryStreamPdfStamper.Dispose();
        }

        document.Close();

        return memoryStreamDocument;
    }

    #region Old Method
    //public static MemoryStream GenerateAdhocReceipt(Registration reg, Guid paymentGroupId)
    //{
    //    PdfReader pdfReader = null;
    //    MemoryStream memoryStreamPdfStamper = null;
    //    PdfStamper pdfStamper = null;
    //    AcroFields pdfFormFields = null;

    //    AdhocInvoiceList adhocInvList = AdhocInvoiceList.GetAdhocInvoiceList(reg.Id,paymentGroupId);
    //    if (adhocInvList.Count > 0)
    //    {
    //        AdhocInvoiceItemList adhocInvItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(paymentGroupId, adhocInvList[0].Id);

    //        EntryList entries = EntryList.GetEntryList(paymentGroupId, reg.Id, "");



    //        pdfReader = new PdfReader(System.Configuration.ConfigurationSettings.AppSettings["PdfTemplateLocation"] + "Adhoc Invoice Template.pdf");
    //        memoryStreamPdfStamper = new MemoryStream();
    //        pdfStamper = new PdfStamper(pdfReader, memoryStreamPdfStamper);
    //        pdfFormFields = pdfStamper.AcroFields;


    //        // Form filling
    //        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //        // Basic information
    //        string customerinfo = "";
    //        customerinfo += adhocInvList[0].PayFirstname + " " + adhocInvList[0].PayLastname + "\r\n";
    //        customerinfo += adhocInvList[0].PayCompany + "\r\n";
    //        customerinfo += adhocInvList[0].PayAddress1 + "\r\n";
    //        if (adhocInvList[0].PayAddress2.Trim() != "") customerinfo += adhocInvList[0].PayAddress2 + "\r\n";
    //        customerinfo += adhocInvList[0].PayCity;
    //        if (adhocInvList[0].PayCity.Trim() != "") customerinfo += " ";
    //        customerinfo += adhocInvList[0].PayPostal + "\r\n";
    //        customerinfo += adhocInvList[0].PayCountry + "\r\n";


    //        pdfFormFields.SetField("customer", customerinfo);

    //        int rowcounter = 1;
    //        decimal total = 0;
    //        decimal fees = 0;
    //        decimal tax = 0;
    //        decimal grandtotal = 0;
    //        string invno = "";
    //        foreach (AdhocInvoiceItem adhocInvItem in adhocInvItemList)
    //        {
    //            Entry entry = Entry.GetEntry(adhocInvItem.EntryId);

    //            PopulateFeeRowAdhoc(rowcounter, pdfFormFields, entry.Serial + " - " + (adhocInvItem.InvoiceType.Equals(AdhocInvoiceType.Custom) ? adhocInvItem.InvoiceTypeOthers : GeneralFunction.GetInvoiceType(adhocInvItem.InvoiceType)) + "<br/>"+entry.Campaign, adhocInvItem.Amount);

    //            rowcounter++;

    //            string DateInvoice = "";
    //            if (adhocInvList[0].InvoiceDate != DateTime.MaxValue && adhocInvList[0].InvoiceDate != DateTime.MinValue)
    //                DateInvoice = adhocInvList[0].InvoiceDate.ToString("dd MMM yyyy");

    //            pdfFormFields.SetField("date", DateInvoice);
    //        }

    //        total = adhocInvList[0].Amount;
    //        fees = adhocInvList[0].Fee;
    //        tax = adhocInvList[0].Tax;
    //        grandtotal = adhocInvList[0].GrandAmount;

    //        invno = adhocInvList[0].Invoice;

    //        pdfFormFields.SetField("invno", invno);

    //        pdfFormFields.SetField("st1", "S$ " + (total + fees).ToString("N"));

    //        // Tax
    //        pdfFormFields.SetField("st4", "S$ " + tax.ToString("N"));

    //        //if (isPP) 
    //        pdfFormFields.SetField("st2", "S$ " + fees.ToString("N"));
    //        //if (fees != 0) pdfFormFields.SetField("admin", "Admin Fees");


    //        //if (isPP) 
    //        pdfFormFields.SetField("st3", "S$ " + grandtotal.ToString("N"));

    //        // dummy
    //        pdfFormFields.SetField("blank", " ");

    //        pdfStamper.FormFlattening = true;
    //        pdfStamper.Writer.CloseStream = false;
    //        pdfStamper.Close();

    //        // test
    //        memoryStreamPdfStamper.Flush();
    //        memoryStreamPdfStamper.Seek(0, SeekOrigin.Begin);

    //    }

    //    return memoryStreamPdfStamper;
    //}
    #endregion

    #region New Method
    public static MemoryStream GenerateAdhocReceipt(Registration reg, Guid paymentGroupId)
    {
        Document document = new Document(PageSize.A4);
        MemoryStream memoryStreamDocument = new MemoryStream();
        PdfWriter pdfWriter = PdfWriter.GetInstance(document, memoryStreamDocument);
        pdfWriter.CloseStream = false;

        PdfReader pdfReader = null;
        MemoryStream memoryStreamPdfStamper = null;
        PdfStamper pdfStamper = null;
        AcroFields pdfFormFields = null;

        AdhocInvoiceList adhocInvList = AdhocInvoiceList.GetAdhocInvoiceList(reg.Id, paymentGroupId);
        if (adhocInvList.Count > 0)
        {
            document.Open();
            AdhocInvoiceItemList adhocInvItemList = AdhocInvoiceItemList.GetAdhocInvoiceItemList(paymentGroupId, adhocInvList[0].Id);

            int itemCounter = 1;
            int rowcounter = 1;
            decimal total = 0;
            decimal fees = 0;
            decimal tax = 0;
            decimal grandtotal = 0;
            string invno = "";

            int pageCounter = 1;
            decimal totalPage = Math.Ceiling((decimal)adhocInvItemList.Count / 9);

            foreach (AdhocInvoiceItem adhocInvItem in adhocInvItemList)
            {
                if (rowcounter == 1)
                {
                    if (pageCounter < totalPage)
                        pdfReader = new PdfReader(System.Configuration.ConfigurationSettings.AppSettings["PdfTemplateLocation"] + "Adhoc Invoice Template 1.pdf");
                    else
                        pdfReader = new PdfReader(System.Configuration.ConfigurationSettings.AppSettings["PdfTemplateLocation"] + "Adhoc Invoice Template 2.pdf");

                    memoryStreamPdfStamper = new MemoryStream();
                    pdfStamper = new PdfStamper(pdfReader, memoryStreamPdfStamper);
                    pdfFormFields = pdfStamper.AcroFields;


                    // Form filling
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    // Basic information
                    string customerinfo = "";
                    customerinfo += adhocInvList[0].PayFirstname + " " + adhocInvList[0].PayLastname + "\r\n";
                    customerinfo += adhocInvList[0].PayCompany + "\r\n";
                    customerinfo += adhocInvList[0].PayAddress1 + "\r\n";
                    if (adhocInvList[0].PayAddress2.Trim() != "") customerinfo += adhocInvList[0].PayAddress2 + "\r\n";
                    customerinfo += adhocInvList[0].PayCity;
                    if (adhocInvList[0].PayCity.Trim() != "") customerinfo += " ";
                    customerinfo += adhocInvList[0].PayPostal + "\r\n";
                    customerinfo += adhocInvList[0].PayCountry + "\r\n";


                    pdfFormFields.SetField("customer", customerinfo);
                }
                
                //foreach (AdhocInvoiceItem adhocInvItem in adhocInvItemList)
                //{
                    Entry entry = Entry.GetEntry(adhocInvItem.EntryId);

                    PopulateFeeRowAdhoc(rowcounter, pdfFormFields, 
                        entry.Serial + " - " + entry.Campaign + "<br/>" + (adhocInvItem.InvoiceType.Equals(AdhocInvoiceType.Custom) ? adhocInvItem.InvoiceTypeOthers : GeneralFunction.GetInvoiceType(adhocInvItem.InvoiceType)),
                        adhocInvItem.Amount, itemCounter);

                    rowcounter++;
                    itemCounter++;

                    string DateInvoice = "";
                    if (adhocInvList[0].InvoiceDate != DateTime.MaxValue && adhocInvList[0].InvoiceDate != DateTime.MinValue)
                        DateInvoice = adhocInvList[0].InvoiceDate.ToString("dd MMM yyyy");

                    pdfFormFields.SetField("date", DateInvoice);
                //}

                total = adhocInvList[0].Amount;
                fees = adhocInvList[0].Fee;
                tax = adhocInvList[0].Tax;
                grandtotal = adhocInvList[0].GrandAmount;

                invno = adhocInvList[0].Invoice;

                pdfFormFields.SetField("invno", invno);
                
                //GeneralFunction.GetInvoiceType(adhocInv.InvoiceType);

                if (pageCounter == totalPage)
                {
                    pdfFormFields.SetField("st1", "S$ " + (total + fees).ToString("N"));

                    // Tax
                    pdfFormFields.SetField("st4", "S$ " + tax.ToString("N"));

                    //if (isPP) 
                    pdfFormFields.SetField("st2", "S$ " + fees.ToString("N"));
                    //if (fees != 0) pdfFormFields.SetField("admin", "Admin Fees");


                    //if (isPP) 
                    pdfFormFields.SetField("st3", "S$ " + grandtotal.ToString("N"));
                }

                // dummy
                pdfFormFields.SetField("blank", " ");

                if (rowcounter == 10 || itemCounter == adhocInvItemList.Count + 1)
                {
                    pdfStamper.FormFlattening = true;
                    pdfStamper.Writer.CloseStream = false;
                    pdfStamper.Close();
                    
                    PdfContentByte pdfContentByte = pdfWriter.DirectContent;
                    document.NewPage();
                    PdfImportedPage pdfImportedPage = pdfWriter.GetImportedPage(new PdfReader(memoryStreamPdfStamper.GetBuffer()), 1);
                    pdfContentByte.AddTemplate(pdfImportedPage, 0, 0);

                    rowcounter = 1;
                    pageCounter++;
                }
            }

            document.Close();
        }

        // test
        memoryStreamDocument.Flush();
        //memoryStreamDocument.Seek(0, SeekOrigin.Begin);
        memoryStreamDocument.Position = 0;

        return memoryStreamDocument;
    }
    #endregion

    private static decimal PopulateFeeRowAdhoc(int counter, AcroFields pdfFormFields, string name, decimal price, int itemCounter)
    {
        string fieldsno = "no" + counter.ToString();
        string fieldname = "des" + counter.ToString();
        //string fieldqty = "qty" + counter.ToString();
        //string fieldunit = "unit" + counter.ToString();
        string fieldtotal = "amt" + counter.ToString();

        pdfFormFields.SetField(fieldsno, itemCounter.ToString());
        pdfFormFields.SetField(fieldname, GeneralFunction.ConvertHTMLBreaksToBreaks(name));
        //pdfFormFields.SetField(fieldqty, feecount.ToString());
        //pdfFormFields.SetField(fieldunit, "S$ " + price.ToString("N"));
        //if (isearlybird) pdfFormFields.SetField(fieldunit, "S$ " + priceEB.ToString("N"));
        decimal feetotal = price;
        
        pdfFormFields.SetField(fieldtotal, "S$ " + feetotal.ToString("N"));

        
        return feetotal;
    }

}