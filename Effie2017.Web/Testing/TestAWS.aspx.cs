using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_TestAWS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string Serial = "MP-PS001";
        string Serialasdsd = Serial;

        if (GeneralFunction.FileExistsInAmazonS3(System.Configuration.ConfigurationManager.AppSettings["AWSBucket_Small"], Serial + "_CreativeMaterials_Video.mp4"))
        {
            Serialasdsd = "asdsas";
        }

        Serialasdsd = "";
    }
}