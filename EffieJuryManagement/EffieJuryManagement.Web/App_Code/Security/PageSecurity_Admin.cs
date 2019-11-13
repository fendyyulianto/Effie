﻿using System;

/// <summary>
/// Summary description for PageSecurity_Admin
/// </summary>
public class PageSecurity_Admin : System.Web.UI.Page
{
	public PageSecurity_Admin()
	{
	}
    protected override void OnPreInit(EventArgs e)
    {
        if (!Security.IsAdminUserLogin())
        {
            Response.Redirect("../Login/Login.aspx");
        }

        base.OnPreInit(e);
    }
}