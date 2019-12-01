using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Effie2017.App;
using System.Web.UI.WebControls;
using System.Web.UI;

/// <summary>
/// Summary description for Security
/// </summary>
/// 
#region Enumerators
public static class AdminAccessType
{
    public const string SuperAdmin = "SA";
    public const string SuperAdminFinance = "SF";
    public const string AdminFinance = "AF";
    public const string Admin = "AD";
    public const string Admin2 = "AD2"; //ADMIN NO JURY
    public const string ReadOnlyAdmin = "RO";
    public const string ST = "ST";
}
#endregion


public class Security
{
	public Security()
	{ 	}

    public static void ResetAllSessions()
    {
        HttpContext.Current.Session.Clear();
    }

    public static void SetAdminLoginSession(Administrator admin)
    {
        HttpContext.Current.Session["Effie.Security.Login.Administrator"] = admin;
    }
    public static Administrator GetAdminLoginSession()
    {
        if (HttpContext.Current.Session["Effie.Security.Login.Administrator"] != null)
            return (Administrator)HttpContext.Current.Session["Effie.Security.Login.Administrator"];
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
    public static bool IsRoleAdminFinance()
    {
        Administrator admin = GetAdminLoginSession();
        if (admin != null && admin.Access == AdminAccessType.AdminFinance)
        {
            return true;
        }
        return false;
    }
    public static bool IsRoleSuperAdminFinance()
    {
        Administrator admin = GetAdminLoginSession();
        if (admin != null && admin.Access == AdminAccessType.SuperAdminFinance)
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
    public static bool IsReadOnlyAdmin()
    {
        Administrator admin = GetAdminLoginSession();
        if (admin != null && admin.Access == AdminAccessType.ReadOnlyAdmin)
        {
            return true;
        }
        return false;
    }
    
    public static bool IsRoleAdmin2() //ADMIN NO JURY
    {
        Administrator admin = GetAdminLoginSession();
        if (admin != null && admin.Access == AdminAccessType.Admin2)
        {
            return true;
        }
        return false;
    }
    public static bool IsRoleST()
    {
        Administrator admin = GetAdminLoginSession();
        if (admin != null && admin.Access == AdminAccessType.ST)
        {
            return true;
        }
        return false;
    }

    public static void SetLastLoginCache(DateTime datetime)
    {
        HttpContext.Current.Session["Effie.Security.Login.LastLogin"] = datetime;
    }
    public static DateTime GetLastLoginCache()
    {
        if (HttpContext.Current.Session["Effie.Security.Login.LastLogin"] != null)
            return (DateTime)HttpContext.Current.Session["Effie.Security.Login.LastLogin"];
        else
            return DateTime.MaxValue;
    }

    public static void SetLoginSessionUser(Registration reg)
    {
        HttpContext.Current.Session["Effie.Security.Login.Registration"] = reg;
    }
    public static Registration GetLoginSessionUser()
    {
        if (HttpContext.Current.Session["Effie.Security.Login.Registration"] != null)
            return (Registration)HttpContext.Current.Session["Effie.Security.Login.Registration"];
        
        return null;
    }
    public static bool IsUserLogin()
    {
        if (GetLoginSessionUser() != null) return true;
        return false;
    }
    public static bool IsUserAdminSpoof()
    {
        if (GetLoginSessionUser() != null)
        {
            if (GetLoginSessionUser().Status == StatusRegistration.Admin) return true;
        }
        return false;
    }
    public static string GenerateSecurityURLToken()
    {
        return GeneralFunction.StringEncryption(DateTime.Now.Ticks.ToString());
    }
    public static bool IsSecurityURLTokenValid(string token)
    {
        try
        {
            string dec = GeneralFunction.StringDecryption(token);
            DateTime datetime = new DateTime(long.Parse(dec));
            if (DateTime.Now.Subtract(datetime).Minutes < 60)
                return true;
        }
        catch 
        {
            return false;
        }
        return false;
    }


    public static void SetLoginSessionJury(EffieJuryManagementApp.Jury jury)
    {
        HttpContext.Current.Session["Effie.Security.Login.Jury"] = jury;        
    }
    public static EffieJuryManagementApp.Jury GetLoginSessionJury()
    {
        if (HttpContext.Current.Session["Effie.Security.Login.Jury"] != null)
            return (EffieJuryManagementApp.Jury)HttpContext.Current.Session["Effie.Security.Login.Jury"];

        return null;
    }
    public static bool IsJuryLogin()
    {
        if (GetLoginSessionJury() != null) return true;
        return false;
    }

    public static void SecureControlByHiding(WebControl wc)
    {
        if (IsRoleSuperAdmin()) return;
        if (IsRoleSuperAdminFinance()) return;
        if (IsRoleAdminFinance()) return;
        if (IsRoleAdmin()) return;
        if (IsRoleAdmin2()) return;

        if (wc != null)
        {
            wc.Visible = false;
            wc.Attributes.Clear();
        }
    }


    public static void SecureControlReadOnlyByHiding(WebControl wc)
    {
        if (IsReadOnlyAdmin())
        {
            wc.Visible = false;
            wc.Attributes.Clear();
        }
    }

    public static void SecureControlByHiding(WebControl wc, string function)
    {
        if (IsRoleSuperAdmin()) return;
        if ((IsRoleSuperAdminFinance() || IsRoleAdminFinance()) && function == "EXPORT") return;
        if (IsRoleAdmin() && (function != "EXPORT" && function != "FlagReasonAdd")) return;
        if (IsRoleAdmin2() && (function != "EXPORT" && function != "FlagReasonAdd" && function != "AddJudge")) return;

        if (wc != null)
        {
            wc.Visible = false;
            wc.Attributes.Clear();
        }
    }

}