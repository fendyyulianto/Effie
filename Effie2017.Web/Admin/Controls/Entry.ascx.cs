using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using Effie2017.App;
public partial class Admin_Controls_Entry : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }
    private void LoadForm()
    {
    }
    private void PopulateForm()
    {
        BindGrid();
    }
    protected void rgEntryList_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        Entry entry = (Entry)e.Item.DataItem;
        if (entry != null)
        {
            Label lbStatus = (Label)e.Item.FindControl("lbStatus");
            if (lbStatus != null)
            {

                lbStatus.Text = entry.PayStatus;
            }
        }
    }
    private void BindGrid()
    {
        EntryList entryList = GeneralFunction.GetAllEntryCache(false);
        rgEntryList.DataSource = entryList;
        rgEntryList.DataBind();
    }
}