using Effie2017.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Admin_FlagReasonCMSList : PageSecurity_Admin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateForm();
            LoadForm();
        }
        
    }

    private void PopulateForm()
    {
        BindGrid(false);
    }
    private void LoadForm()
    {
        Security.SecureControlByHiding(lnkBtnAdd, "FlagReasonAdd");
    }

    private void BindGrid(bool isRebind)
    {
        FlagReasonsList list = FlagReasonsList.GetFlagReasonsList();
        List<FlagReasons> flagReasonsList = new List<FlagReasons>();
        string advanceSearch = (string)ViewState["Search"];

        if (advanceSearch == "1")
        {
            foreach (FlagReasons f in list)
            {
                if (
                    (ddlActive.SelectedValue == "" || (ddlActive.SelectedValue != "" && ddlActive.SelectedValue == f.IsActive.ToString())) &&
                    (((txtSearch.Text == "" || txtSearch.Text != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "description") && f.Description.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    ((txtSearch.Text == "" || txtSearch.Text != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "bodyname") && f.Bodyname.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))
                    ))
                {
                    flagReasonsList.Add(f);
                }
            }
        }
        else
        {
            foreach(FlagReasons f in list)
            {
                if (f.IsActive == true || f.IsActive == false)
                    flagReasonsList.Add(f);
            }
        }


        rgFlagReason.DataSource = flagReasonsList;
        if (isRebind)
            rgFlagReason.DataBind();
    }
    protected void rgFlagReason_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            FlagReasons f = (FlagReasons)e.Item.DataItem;
            CheckBox chkAcDec = (CheckBox)e.Item.FindControl("chkAcDec");
            HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");
            hdfId.Value = f.Id.ToString();
            CheckBox chkIsHasOther = (CheckBox)e.Item.FindControl("chkIsHasOther");
            HiddenField hdfIdOther = (HiddenField)e.Item.FindControl("hdfIdOther");
            hdfIdOther.Value = f.Id.ToString();


            if (f.IsActive)
                chkAcDec.Checked = true;
            else
                chkAcDec.Checked = false;
            if (f.isHasOther)
                chkIsHasOther.Checked = true;
            else
                chkIsHasOther.Checked = false;


            LinkButton lnkBtnDelete = (LinkButton)e.Item.FindControl("lnkBtnDelete");
            lnkBtnDelete.Attributes.Add("onclick", "return DeleteConfirmation('" + ((GridDataItem)e.Item)["Bodyname"].Text + "');");

        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            PageSizeCombo.Items.Clear();
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", rgFlagReason.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", rgFlagReason.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", rgFlagReason.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", rgFlagReason.MasterTableView.ClientID);
            PageSizeCombo.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
        }
    }

    protected void rgFlagReason_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
    }
    protected void rgFlagReason_OnPreRender(object sender, EventArgs e)
    {

        if (Security.GetAdminLoginSession().Access == "SA")
        {
            rgFlagReason.MasterTableView.GetColumn("AcDec").Visible = false;
            rgFlagReason.MasterTableView.GetColumn("Actions").Visible = true;
        }
        else
        {
            rgFlagReason.MasterTableView.GetColumn("AcDec").Visible = false;
            rgFlagReason.MasterTableView.GetColumn("Actions").Visible = false;
        }
    }
    protected void rgFlagReason_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";
        if(e.CommandName == "EditItem")
        {
            Response.Redirect("FlagReasonCMS.aspx?Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        if(e.CommandName == "DeleteItem")
        {
            GridDataItem item = (GridDataItem)e.Item;
            FlagReasons.DeleteFlagReasons(new Guid(item["Id"].Text));
            lblError.Text = item["Bodyname"].Text + " Is deleted<br>";
        }

    }

    protected void chkAcDec_OnCheckedChanged(object sender, EventArgs e)
    {
        foreach(GridDataItem item in rgFlagReason.Items)
        {
            CheckBox chkAcDec = (CheckBox)item.FindControl("chkAcDec");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
            if (hdfId != null)
            {
                try
                {
                    FlagReasons f = FlagReasons.GetFlagReasons(new Guid(hdfId.Value));
                    if (f != null)
                    {
                        if (chkAcDec != null)
                        {
                            if (chkAcDec.Checked)
                                f.IsActive = true;
                            else
                                f.IsActive = false;
                            f.Save();
                        }
                    }
                }
                catch { }
            }

        }
        BindGrid(true);

    }
	
    protected void chkIsHasOther_OnCheckedChanged(object sender, EventArgs e)
    {
        foreach (GridDataItem item in rgFlagReason.Items)
        {
            CheckBox chkIsHasOther = (CheckBox)item.FindControl("chkIsHasOther");
            HiddenField hdfIdOther = (HiddenField)item.FindControl("hdfIdOther");
            if (hdfIdOther != null)
            {
                try
                {
                    FlagReasons f = FlagReasons.GetFlagReasons(new Guid(hdfIdOther.Value));
                    if (f != null)
                    {
                        if (chkIsHasOther.Checked)
                        {
                            f.isHasOther = true;
                            //f.Description = string.Empty;
                        }
                        else
                        {
                            f.isHasOther = false;
                        }

                        f.Save();
                    }
                }
                catch { }
            }

        }
        BindGrid(true);

    }
    protected void lnkBtnAdd_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("FlagReasonCMS.aspx");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["Search"] = "1";
        BindGrid(true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ViewState["Search"] = "";
        BindGrid(true);
    }

}