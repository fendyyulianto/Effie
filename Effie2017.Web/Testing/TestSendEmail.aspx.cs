using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class Test1 {
    public string id;
    public string dataType;
    public string textString;
}
public partial class Testing_CheckBox : System.Web.UI.Page
{
    string[,] testr = { 
                        { "001", "head", "TV" },
                        { "003", "opt", "- Branded Content" },
                        { "002", "opt", "- Spots" }
                      };

    List<Test1> TestList = new List<Test1>();
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        string Body = string.IsNullOrWhiteSpace(EmailBodyId.Text) ? "Test Send Email" : EmailBodyId.Text;
        Email.SendMail(EmailId.Text, System.Configuration.ConfigurationSettings.AppSettings["AdminEmail"], EmailId.Text, "", "", "TEST SEND EMAIL", Body, true, null, null);
    }
}