using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Video_DownloadMedia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["BucketID"] != null && Request.QueryString["MediaID"] != null)
            {
                if (GeneralFunction.FileExistsInAmazonS3(Request.QueryString["BucketID"], Request.QueryString["MediaID"]))
                {
                    GetStream(Request.QueryString["BucketID"], Request.QueryString["MediaID"]);
                }
            }
            else if (Request.QueryString["filePath"] != null && Request.QueryString["MediaID"] != null)
            {
                if (File.Exists(Request.QueryString["filePath"] + Request.QueryString["MediaID"]))
                {
                    GetStreamFromLocal(Request.QueryString["filePath"], Request.QueryString["MediaID"]);
                }
            }
        }
        catch
        {
        }
    }

    public void GetStream(string bucketName,string dbFileName)
    {
        //Create and populate a memorystream with the contents of the
        //database table
        MemoryStream mstream = GeneralFunction.GetFileFromAmazonS3(bucketName, dbFileName);
        if (mstream != null)
        {
            //Convert the memorystream to an array of bytes.
            byte[] byteArray = mstream.ToArray();
            //Clean up the memory stream
            mstream.Flush();
            mstream.Close();
            // Clear all content output from the buffer stream
            Response.Clear();
            // Add a HTTP header to the output stream that specifies the default filename
            // for the browser's download dialog
            Response.AddHeader("Content-Disposition", "attachment; filename=" + dbFileName + "");
            // Add a HTTP header to the output stream that contains the 
            // content length(File Size). This lets the browser know how much data is being transfered
            Response.AddHeader("Content-Length", byteArray.Length.ToString());
            // Set the HTTP MIME type of the output stream
            Response.ContentType = "application/octet-stream";
            // Write the data out to the client.
            Response.BinaryWrite(byteArray);
        }
    }

    public void GetStreamFromLocal(string filePath,string dbFileName)
    {
        MemoryStream mstream = new MemoryStream() ;
        FileStream file = new FileStream(filePath + dbFileName, FileMode.Open, FileAccess.Read);
        //Create and populate a memorystream with the contents of the
        //database table
        if(file != null)
        {
            byte[] bytes = new byte[file.Length];
            file.Read(bytes, 0, (int)file.Length);
            mstream.Write(bytes,0,(int)file.Length);
        }

        
        if (mstream != null)
        {
            //Convert the memorystream to an array of bytes.
            byte[] byteArray = mstream.ToArray();
            //Clean up the memory stream
            mstream.Flush();
            mstream.Close();
            // Clear all content output from the buffer stream
            Response.Clear();
            // Add a HTTP header to the output stream that specifies the default filename
            // for the browser's download dialog
            Response.AddHeader("Content-Disposition", "attachment; filename=" + dbFileName + "");
            // Add a HTTP header to the output stream that contains the 
            // content length(File Size). This lets the browser know how much data is being transfered
            Response.AddHeader("Content-Length", byteArray.Length.ToString());
            // Set the HTTP MIME type of the output stream
            Response.ContentType = "application/octet-stream";
            // Write the data out to the client.
            Response.BinaryWrite(byteArray);
        }
    }
}