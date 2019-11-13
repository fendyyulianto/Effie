using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PageSecurity_User
/// </summary>
public class PageSecurity_User : System.Web.UI.Page
{
	public PageSecurity_User()
	{
	}
    protected override void OnPreInit(EventArgs e)
    {
        if (!Security.IsUserLogin())
        {
            Response.Redirect("../User/Login.aspx");
        }

        base.OnPreInit(e);
    }
}