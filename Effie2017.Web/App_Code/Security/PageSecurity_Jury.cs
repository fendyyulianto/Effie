using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PageSecurity_User
/// </summary>
public class PageSecurity_Jury : System.Web.UI.Page
{
    public PageSecurity_Jury()
	{
	}
    protected override void OnPreInit(EventArgs e)
    {
        if (!Security.IsJuryLogin())
        {
            Response.Redirect("../Jury/Login.aspx");
        }

        base.OnPreInit(e);
    }
}