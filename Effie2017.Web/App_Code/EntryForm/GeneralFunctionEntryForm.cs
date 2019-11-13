using System;
using System.Data;
using System.Configuration;
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
using Effie2017.App;
using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.ElasticTranscoder;
using Amazon.ElasticTranscoder.Model;
using Amazon.S3.Model;
using Amazon.S3.IO;
using ClosedXML.Excel;
using HtmlAgilityPack;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;

/// <summary>
/// Summary description for GeneralFunctionEntryForm
/// </summary>
/// 

public class TextBoxKeyup
{
    public TextBox textbox { get; set; }
    public int LimitText { get; set; }
    public bool isLimitActive { get; set; }
}
public class ImageGalleryResult
{
    public string Filename { get; set; }
    public string FileLocation { get; set; }
    public string Error { get; set; }
}
public class GeneralFunctionEntryForm
{

    public static string WebURL = System.Configuration.ConfigurationSettings.AppSettings["WebPhysicalPath"];

    public GeneralFunctionEntryForm()
    {
        
    }
    
    public static MemoryStream memStream(string Doc)
    {
        MemoryStream memoryStream = new System.IO.MemoryStream();
        StringReader srdr = new StringReader(Doc);
        Document pdfDoc = new Document(PageSize.A4, 40, 40, 20, 35);
        HTMLWorker hparse = new HTMLWorker(pdfDoc);
        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);

        //Define the page number
        PageEventHelper pageEventHelper = new PageEventHelper();
        pdfWriter.PageEvent = pageEventHelper;
        // Define the page header
        pageEventHelper.Title = "";
        pageEventHelper.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
        pageEventHelper.HeaderLeft = "";
        pageEventHelper.HeaderRight = "";

        pdfDoc.Open();
        hparse.Parse(srdr);

        pdfWriter.CloseStream = false;
        pdfDoc.Close();
        pdfWriter.Close();

        return memoryStream;
    }

    public static MemoryStream Classification(EntryForm entf, Entry ent)
    {
        MemoryStream memoryStream = new System.IO.MemoryStream();
        
        if (ent != null)
        {
            if (ent.CategoryPSDetail.IndexOf("Positive Change Environmental") != -1)
            {
                //Cat = "EFPositiveChangeEnvironmental";
                memoryStream = PositiveChangeEnvironmentalPDF.memStream(entf, ent);
            }
            else if (ent.CategoryPSDetail.IndexOf("Sustained Success") != -1)
            {
                //Cat = "EFSustainedSuccess";
                memoryStream = SustainedSuccessPDF.memStream(entf, ent);
            }
            else if (ent.CategoryPSDetail.IndexOf("Shopper") != -1)
            {
                //Cat = "EFShopperMarketing";
                memoryStream = ShopperMarketingPDF.memStream(entf, ent);
            }
            else if (ent.CategoryMarket == "SM")
            {
                //Cat = "EFSingleMarket";
                memoryStream = SingleMarketPDF.memStream(entf, ent);
            }
            else if (ent.CategoryMarket == "MM")
            {
                //Cat = "EFMultiMarket";
                memoryStream = MultiMarketPDF.memStream(entf, ent);
            }
        }
        
        return memoryStream;
    }

    public static string CheckTextfield(string str)
    {
        string Min = "### imagesMIN ###";
        string plus = "### imagesPLUS ###";
        string Minimg = "<img src=\"" + WebURL + "images/IconMin.png" + "\" height=\"6\" width=\"14\">";
        string plusimg = "<img src=\"" + WebURL + "images/IconPlus.png" + "\" height=\"6\" width=\"14\">";
        str = str.Replace("  ", "");
        str = str.Replace("<", Min);
        str = str.Replace(">", plus);
        str = str.Replace(Min, Minimg);
        str = str.Replace(plus, plusimg);
        str = str.Replace("\n", "<br />");
        return str;
    }

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

    public static string GetEntryCategory(Entry entry)
    {
        string Cat = "";
        if (entry != null)
        {
            if (entry.CategoryPSDetail.IndexOf("Positive Change Environmental") != -1)
            {
                Cat = "EFPositiveChangeEnvironmental";
            }
            else if (entry.CategoryPSDetail.IndexOf("Sustained Success") != -1)
            {
                Cat = "EFSustainedSuccess";
            }
            else if (entry.CategoryPSDetail.IndexOf("Shopper") != -1)
            {
                Cat = "EFShopperMarketing";
            }
            else if (entry.CategoryMarket == "SM")
            {
                Cat = "EFSingleMarket";
            }
            else if (entry.CategoryMarket == "MM")
            {
                Cat = "EFMultiMarket";
            }
        }

        return Cat;
    }

    public static void  InitTextbox(List<TextBoxKeyup> textBoxKeyupList, Page page, Entry entry, string IsPreview)
    {
        foreach (TextBoxKeyup textBoxKey in textBoxKeyupList)
        {
            #region Stop Typing Textbox
            ///*len.length >= wordLen ||       wordsLeft <= 0  */
            ////parseInt($('#show_remaining_words" + textBoxKey.textbox.ClientID + "').val()) <= 0 
            ////"   		console.log('wordsLeft => '+wordsLeft);                                                                 " +
            ////"   		console.log('len.length => '+len.length);                                                                 " +
            ////"   		console.log('Count TXT => '+parseInt($('#show_remaining_words" + textBoxKey.textbox.ClientID + "').text())); " +
            ////"   		var len = $('#" + textBoxKey.textbox.ClientID + "').val().split(/[\\s]+/);              " +
            ////"   		console.log('wordList.length => '+wordList.length);                                                                 " +
            //if (textBoxKey.LimitText == null || textBoxKey.LimitText == 0)
            //    textBoxKey.LimitText = 9999;

            //String scriptText = "";
            //scriptText += "   $(document).ready(function () {" +
            //                "   $('#" + textBoxKey.textbox.ClientID + "').keydown(function(event) {	                            " +
            //                "   		var wordLen = " + textBoxKey.LimitText + ";                                             " +
            //                "   		var word = $('#" + textBoxKey.textbox.ClientID + "').val().replace(\"  \",\" \").replace(\" \", \" \").replace(\"   \", \" \").replace(/\\n/g, \" \");                                             " +
            //                "   		var wordList = word.split(\" \");" +
            //                "           var WordCount = 0;                                                          " +
            //                "           for (var i = 0, len = wordList.length; i < len; i++) {                           " +
            //                "               if (/^[a-zA-Z0-9]/.test(wordList[i]))                                   " +
            //                "           	{                                                                       " +
            //                "           		WordCount++;                                                            " +
            //                "           	}                                                                           " +
            //                "           }                                                                                       " +
            //                "   		var wordsLeft = (wordLen - WordCount) - 1;                                                   " +
            //                "   		console.log('wordsLeft.length => '+wordsLeft);                                                                 " +
            //                "   		if (wordsLeft <= 0  && word.slice(-1) == ' ') { " +
            //                "   			if ( event.keyCode == 46 || event.keyCode == 8 ) {                                  " +
            //                "   			} else if (event.keyCode < 48 || event.keyCode > 57 ) {                             " +
            //                "   				event.preventDefault();                                                         " +
            //                "   			}                                                                                   " +
            //                "   		}                                                                                       " +
            //                "   	});  });                                                                                    ";
            //page.ClientScript.RegisterClientScriptBlock(page.GetType(), "'" + textBoxKey.textbox.ID.ToString() + "keydown'", scriptText, true);
            #endregion
            
            List<ImagesUpload> imageuploadList = GeneralFunction.GetImageGallery(0, entry, textBoxKey.textbox.ID.ToString(), false);
            textBoxKey.textbox.Attributes.Add("onkeyup", "LimitText(\"" + textBoxKey.textbox.ClientID + "\", " + textBoxKey.LimitText + ", \"" + textBoxKey.isLimitActive + "\");");
            page.ClientScript.RegisterStartupScript(page.GetType(), "'" + textBoxKey.textbox.ID.ToString() + "'", "LimitText(\"" + textBoxKey.textbox.ClientID + "\", " + textBoxKey.LimitText + ", \"" + textBoxKey.isLimitActive + "\");", true);
            int count = 1;
            string Images = "";
            string FileLoc = "";
            try
            {
                string url = System.Configuration.ConfigurationSettings.AppSettings["storageVirtualPath"] + "\\EntryForm\\" + "\\" + entry.Id + "\\";
                foreach (ImagesUpload imagesUpload in imageuploadList)
                {
                    if (imagesUpload != null)
                    {
                        string Imagepath = imagesUpload.path;
                        if (!string.IsNullOrEmpty(Imagepath))
                        {
                            FileLoc = url + Imagepath;
                            FileLoc = FileLoc.Replace("\\", "/").Replace("\\/", "/");
                            Images += "<a href='" + FileLoc + "' style='margin-left: 10px;margin-right: 10px;vertical-align: middle;' target='_blank'> Chart " + count /*imagesUpload.FileName*/ + "</a>";

                        }
                    }
                    count++;
                }
            }
            catch
            {
            }

            if (string.IsNullOrEmpty(Images))
            {
                Images = "<a></a>";
            }

            page.ClientScript.RegisterStartupScript(page.GetType(), "'PreviewImagesEntryForm-" + textBoxKey.textbox.ID.ToString() + IsPreview + "'", "PreviewImagesEntryForm(\"" + textBoxKey.textbox.ID + "\", \"" + IsPreview + "\", \"" + Images + "\");", true);
        }
    }
}