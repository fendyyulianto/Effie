using System;
using Telerik.Web.UI;
using Effie2017.App;

public partial class Admin_AmountReceived : System.Web.UI.Page
{
    string pgId;

    protected void Page_Load(object sender, EventArgs e)
    {
        pgId = Request.QueryString["pgId"];
        if (pgId == null || pgId == "") return;

        if (!IsPostBack)
        {
            BindGrid();
        }
    }
    private void BindGrid()
    {
        AmountReceivedList list = AmountReceivedList.GetAmountReceivedList(new Guid(pgId));

        radGridAmountReceived.DataSource = list;
        radGridAmountReceived.Rebind();
    }
    protected void radGridAmountReceived_NeedDataSource(object Sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }
    protected void radGridAmountReceived_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            AmountReceived amt = (AmountReceived)e.Item.DataItem;
            ((GridDataItem)e.Item)["IsSetPaid"].Text = "No";
            if (amt.IsSetPaid) ((GridDataItem)e.Item)["IsSetPaid"].Text = "Yes";
            try { ((GridDataItem)e.Item)["User"].Text = "" + Administrator.GetAdministrator(amt.CommentatorID).LoginId + ""; }
            catch { }
        }
    }
}