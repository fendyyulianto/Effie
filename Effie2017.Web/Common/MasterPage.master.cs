using System;
using System.Web;
using System.Web.UI;

public partial class Common_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        string CurrentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
        if (Security.IsAdminUserLogin())
        {
        }
        else if ((Request.Url.ToString().ToLower().Contains("/main/efmultimarketpdf.aspx") ||
            Request.Url.ToString().ToLower().Contains("/main/efpositivechangeenvironmentalpdf.aspx") ||
            Request.Url.ToString().ToLower().Contains("/main/efshoppermarketingpdf.aspx") ||
            Request.Url.ToString().ToLower().Contains("/main/efsinglemarketpdf.aspx") ||
            Request.Url.ToString().ToLower().Contains("/main/efsustainedsuccesspdf.aspx")) && Security.IsJuryLogin())
        {
        }
        else if (Request.Url.ToString().ToLower().Contains("/main/") && !Security.IsUserLogin())
        {
            Response.Redirect("../User/Login.aspx?rd=" + IptechLib.Crypto.StringEncryption(CurrentUrl));
        }
    }

    protected void LoadForm()
    {
        Effie2017.App.Registration registration = Security.GetLoginSessionUser();
        if (registration != null)
        {
            lblFirstName.Text = registration.Firstname;
            lblLastName.Text = registration.Lastname;

            if (registration.LastSignIn.ToString("yyyyMMdd") != "99991231")
                lblSign.Text = registration.LastSignIn.ToString("dd MMM yyyy hh:mm tt");
        }
    }

    protected void PopulateForm()
    {
    }

    protected void lnkBtnLogout_Click(object sender, EventArgs e)
    {
        bool isAdminSpoof = Security.IsUserAdminSpoof();

        Security.ResetAllSessions();

        if (Request.Cookies["wdduw2013UserLogin"] != null)
            Response.Cookies["wdduw2013UserLogin"].Expires = DateTime.Now.AddMinutes(-1);

        if (isAdminSpoof)
            Response.Redirect("../Admin/Login.aspx");
        else
            Response.Redirect("../User/Login.aspx");
    }

    public void ShowUser()
    {
        tblUser.Visible = true;
    }

    public void ShowNav()
    {
        ulNav.Visible = true;
    }

    public void SetConfirmLogout()
    {
        //lnkBtnLogout.Attributes.Add("onclick", "return confirm('Please save before logging out. Confirm to logout?');");
        lnkBtnLogout.OnClientClick = "return confirm('Please save before logging out. Confirm to logout?');";
    }

    public void SetCompatibility()
    {
        ltrForCompatibility.Text = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=7\">";
    }
}
