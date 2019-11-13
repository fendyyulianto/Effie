using System.Web;
using EffieJuryManagementApp;
using System;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Security
/// </summary>
/// 
#region Enumerators
public static class AdminAccessType
{
    public const string SuperAdmin = "SA";
    public const string Admin = "AD";
    public const string ReadOnlyAdmin = "RO";
}
#endregion


public class Security : System.Web.UI.Page
{
	public Security()
	{ 	}

    public static void ResetAllSessions()
    {
        HttpContext.Current.Session.Clear();
    }

    public static void SetAdminLoginSession(Administrator admin)
    {
        HttpContext.Current.Session["EffieJuryManagement.Security.Login.Administrator"] = admin;
    }
    public static Administrator GetAdminLoginSession()
    {
        if (HttpContext.Current.Session["EffieJuryManagement.Security.Login.Administrator"] != null)
            return (Administrator)HttpContext.Current.Session["EffieJuryManagement.Security.Login.Administrator"];
        return null;
    }
    public static bool IsAdminUserLogin()
    {
        if (GetAdminLoginSession() != null) return true;
        return false;
    }

    public static bool IsRoleSuperAdmin()
    {
        Administrator admin = GetAdminLoginSession();
        if (admin != null && admin.Access == AdminAccessType.SuperAdmin)
        {
            return true;
        }
        return false;
    }
    public static bool IsRoleAdmin()
    {
        Administrator admin = GetAdminLoginSession();
        if (admin != null && admin.Access == AdminAccessType.Admin)
        {
            return true;
        }
        return false;
    }
    public static bool IsRoleReadOnlyAdmin()
    {
        Administrator admin = GetAdminLoginSession();
        if (admin != null && admin.Access == AdminAccessType.ReadOnlyAdmin)
        {
            return true;
        }
        return false;
    }

    public static void SetLastLoginCache(DateTime datetime)
    {
        HttpContext.Current.Session["EffieJuryManagement.Security.Login.LastLogin"] = datetime;
    }
    public static DateTime GetLastLoginCache()
    {
        if (HttpContext.Current.Session["EffieJuryManagement.Security.Login.LastLogin"] != null)
            return (DateTime)HttpContext.Current.Session["EffieJuryManagement.Security.Login.LastLogin"];
        else
            return DateTime.MaxValue;
    }

    public static void RedirectToErrorPage()
    {
        HttpContext.Current.Response.Redirect("../Error/Error.aspx");
    }

    public static void RedirectToAccessDeniedPage()
    {
        HttpContext.Current.Response.Redirect("../Error/AccessDenied.aspx");
    }

    public static void SecureControlByHiding(WebControl wc)
    {
        if (IsRoleSuperAdmin()) return;
        if (IsRoleAdmin()) return;

        if (wc != null)
        {
            wc.Visible = false;
            wc.Attributes.Clear();
        }
    }
    public static void SecureControlByHiding(WebControl wc, string function)
    {
        if (IsRoleSuperAdmin()) return;
        if (IsRoleAdmin() && function == "EXPORT") return;

        if (wc != null)
        {
            wc.Visible = false;
            wc.Attributes.Clear();
        }
    }
}