using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EffieJuryManagementApp;

public partial class EmailTracking : System.Web.UI.Page
{
    private static byte[] _imgbytes =
             Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");

    override protected void OnInit(EventArgs e)
    {
        this.Load += new System.EventHandler(this.Page_Load);
    }

    private void Page_Load(object sender, System.EventArgs e)
    {
        // check if-modified-since header to determine if we 
        // should log again or send back a not modified result
        if (useCached(this.Context.Request))
        {
            Response.StatusCode = 304;
            Response.SuppressContent = true;
        }
        else
        {
            // add code to log visit here
            // such as write to a database

            Response.ContentType = "image/gif";
            Response.AppendHeader("Content-Length", _imgbytes.Length.ToString());
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.BinaryWrite(_imgbytes);


            string invId = Request.QueryString["invId"];

            if (invId != null)
            {
                Invitation inv = Invitation.GetInvitation(IptechLib.Validation.GetValueGuid(invId, true));

                if (inv != null)
                {
                    inv.IsRead = true;

                    if (inv.IsValid)
                        inv.Save();
                }
            }                
        }
    }

    private bool useCached(HttpRequest req)
    {
        string ifmod = req.Headers["If-Modified-Since"];
        return ifmod == null ? false : DateTime.Parse(ifmod).AddHours(24) >= DateTime.Now;
    }
}