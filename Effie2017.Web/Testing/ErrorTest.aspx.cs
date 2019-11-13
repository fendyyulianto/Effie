using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_ErrorTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Effie2017.App.Gen_GeneralUseValue test = Effie2017.App.Gen_GeneralUseValue.GetGen_GeneralUseValue(new Guid("d3be68f0-bb7a-4bd3-89a6-3914b4d902fb"));
        test.Value = "3";
        test.Save();
    }
}