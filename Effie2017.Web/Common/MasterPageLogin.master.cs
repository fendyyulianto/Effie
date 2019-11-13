﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_MasterPageLogin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (GeneralFunction.IsEntrantSubmissionCutOff())
        {
            //phSubmissionClosed.Visible = true;
            phClosingMessage.Visible = false;
        }

        if (Request.Url.ToString().ToLower().Contains("admin"))
            phClosingMessage.Visible = false;
    }
}
