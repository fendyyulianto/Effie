using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Security.Cryptography;
using System.Text;

public partial class Main_DownloadFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DownloadFile"] == null)
            Response.Redirect("./Dashboard.aspx");

        FileInfo file = new FileInfo(System.Configuration.ConfigurationSettings.AppSettings["storagePhysicalPath"] + Session["DownloadFile"].ToString());
        string lastUpdateDateTime = file.LastWriteTimeUtc.ToString("r");

        string eTag = HttpUtility.UrlEncode(GetHashMD5(file.Name + lastUpdateDateTime, "GXDownloadFileName"));

        //string test = eTag;

        //HttpResponse httpResponse = HttpContext.Current.Response;

        long startBytes = 0;
        long endBytes = file.Length - 1;

        //Response.ClearContent();
        Response.Clear();
        //Response.Buffer = false;
        Response.AddHeader("Accept-Ranges", "bytes");
        Response.AddHeader("ETag", eTag);
        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
        //Response.AddHeader("Content-Length", file.Length.ToString());
        //Response.ContentType = "application/" + file.Extension.ToLower();
        Response.ContentType = "application/octet-stream";

        if (Page.Request.Headers["Range"] != null)
        {
            Response.StatusCode = 206;
            string[] range = Page.Request.Headers["Range"].Split(new char[] { '=', '-' });
            startBytes = Convert.ToInt64(range[1]);
            endBytes = file.Length - startBytes;

            //if (startBytes == file.Length - 1)
            //    endBytes = 1;

            if (startBytes < 0 || startBytes >= file.Length)
            {
                Response.End();
                return;
            }
        }
        else
            endBytes++;

        if (startBytes > 0)
        {
            Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startBytes, endBytes, file.Length));
            Response.AddHeader("Content-Length", (endBytes).ToString());
        }

        Request.ServerVariables.Set("ResponseThrottler-InitialSendSize", System.Configuration.ConfigurationManager.AppSettings["ResponseThrottler-InitialSendSize"]);
        Request.ServerVariables.Set("ResponseThrottler-Rate", System.Configuration.ConfigurationManager.AppSettings["ResponseThrottler-Rate"]);

        Response.TransmitFile(file.FullName, startBytes, endBytes);
        Response.End();
    }

    protected string GetHashMD5(string text, string snipInSentence)
    {
        string rearrangeText = "";

        string sentence = snipInSentence;
        string firstChar = "";
        string thirdChar = "";

        char[] charText = text.ToCharArray();

        //Divided the text into 3 parts Character
        //Get the first Character 
        for (int x = 0; x < charText.Length; x += 3)
        {
            firstChar += charText[x];
        }

        //Get the Third Character 
        for (int x = 2; x < charText.Length; x = x + 3)
        {
            thirdChar += charText[x];
        }

        //put all srting to char variable 
        char[] charSentence = sentence.ToCharArray();
        char[] charFirstChar = firstChar.ToCharArray();
        char[] charThirdChar = thirdChar.ToCharArray();

        //Get max Length
        int maxChar = 0;

        if (charFirstChar.Length <= charThirdChar.Length)
            maxChar = charThirdChar.Length;
        else
            maxChar = charFirstChar.Length;

        //Put everything together. the format is [third Charater] [Sentence] [First Charater]
        for (int x = 0; x < maxChar; x++)
        {
            if (x < charFirstChar.Length && x < charThirdChar.Length && x < charSentence.Length)
                rearrangeText += charThirdChar[x].ToString() + charSentence[x].ToString() + charFirstChar[x].ToString();
            else if (x < charFirstChar.Length && x < charThirdChar.Length && x > charSentence.Length)
                rearrangeText += charThirdChar[x].ToString() + charFirstChar[x].ToString();
            else if (x > charFirstChar.Length && x < charThirdChar.Length && x > charSentence.Length)
                rearrangeText += charThirdChar[x].ToString();
            else if (x < charFirstChar.Length && x > charThirdChar.Length && x > charSentence.Length)
                rearrangeText += charFirstChar[x].ToString();
        }

        //Create MD5
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] tmpSource;
        byte[] tmpHash;

        tmpSource = ASCIIEncoding.ASCII.GetBytes(rearrangeText); // Turn password into byte array
        tmpHash = md5.ComputeHash(tmpSource);

        StringBuilder sOutput = new StringBuilder(tmpHash.Length);
        for (int i = 0; i < tmpHash.Length; i++)
        {
            sOutput.Append(tmpHash[i].ToString("X2").ToLower());  // X2 formats to hexadecimal
        }
        return sOutput.ToString();
    }
}