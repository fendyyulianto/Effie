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

        for (int x = 0; x < 3; x++)
        {
            Test1 Testin = new Test1();
            Testin.id = testr[x, 0];
            Testin.dataType = testr[x, 1];
            Testin.textString = testr[x, 2];

            TestList.Add(Testin);
        }


        if (!IsPostBack)
        {
            rptIndCredits.DataSource = TestList;
            rptIndCredits.DataBind();

            CheckBox chk = new CheckBox();
            chk.ID = "test";
            //plc.add(chk);
        }
    }

    protected void rptIndCredits_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Test1 item = (Test1)e.Item.DataItem;

            HiddenField hd = (HiddenField)e.Item.FindControl("hdItemId");
            hd.Value= item.id;

            if (item.dataType == "head")
            {
                CheckBox chk = (CheckBox)e.Item.FindControl("chk1");
                chk.Visible = false;
                chk = (CheckBox)e.Item.FindControl("chk2");
                chk.Visible = false;
                chk = (CheckBox)e.Item.FindControl("chk3");
                chk.Visible = false;


                Label lbl = (Label)e.Item.FindControl("head");
                lbl.Text = item.textString;
            }
            if (item.dataType == "opt")
            {
                Label lbl = (Label)e.Item.FindControl("head");
                lbl.Text = item.textString;
            }
            //CheckBox chk = (CheckBox)e.Item.FindControl("chk");

            //chk.Text = item;
        }
    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        if (chk.Checked)
        {
            Response.Write(chk.Text);
        }
        
        foreach (RepeaterItem rpt in rptIndCredits.Items)
        {
            CheckBox chk2 = (CheckBox)rpt.FindControl("chk1");

            if (chk2.Checked)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                Response.Write(hd.Value + "|");
            }
        }

        foreach (RepeaterItem rpt in rptIndCredits.Items)
        {
            CheckBox chk2 = (CheckBox)rpt.FindControl("chk2");

            if (chk2.Checked)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                Response.Write(hd.Value + "|");
            }
        }

        foreach (RepeaterItem rpt in rptIndCredits.Items)
        {
            CheckBox chk2 = (CheckBox)rpt.FindControl("chk3");

            if (chk2.Checked)
            {
                HiddenField hd = (HiddenField)rpt.FindControl("hdItemId");
                Response.Write(hd.Value + "|");
            }
        }
    }
}