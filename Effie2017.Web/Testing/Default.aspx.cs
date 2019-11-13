using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Entry entry = EntryList.GetEntryList(new Guid("B0696F18-3F16-419B-8A98-7A433C888F8D"), Guid.Empty, "").FirstOrDefault();
        string Dateasdas = entry.DateSubmitted.ToString("dd MMMM yyyy");
        DateTime asdasd = entry.DateSubmitted;
        int asdasdas = 2;
    }
}