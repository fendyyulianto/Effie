using EffieJuryManagementApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Admin_AdminList : PageSecurity_Admin
{
    Administrator CurrentLoginAdmin = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PopulateForm();
            LaodForm();
        }
        CurrentLoginAdmin = Security.GetAdminLoginSession();
    }

    private void PopulateForm()
    {
        BindGrid(true);
    }
    private void LaodForm()
    {

    }
    public static AdministratorList GetAllAdministratorCache(bool needRefresh)
    {
        AdministratorList list = (AdministratorList)HttpContext.Current.Session["Effie.AdministratorList"];
        if (list == null || needRefresh)
        {
            list = AdministratorList.GetAdministratorList();
            HttpContext.Current.Session["Effie.AdministratorList"] = list;
        }
        return list;
    }
    private void BindGrid(bool isRebind)
    {
        AdministratorList list = GetAllAdministratorCache(isRebind);
        List<Administrator> administratorList = new List<Administrator>();


        string advanceSearch = (string)ViewState["Search"];
        if(advanceSearch == "1")
        {
            foreach (Administrator admin in list)
            {
                if (
                    (ddlActive.SelectedValue == "" || (ddlActive.SelectedValue != "" && ddlActive.SelectedValue == admin.IsActive.ToString())) &&
                    (((txtSearch.Text == "" || txtSearch.Text != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "name") && admin.Name.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    ((txtSearch.Text == "" || txtSearch.Text != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "loginId") && admin.LoginId.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    ((txtSearch.Text == "" || txtSearch.Text != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "access") && admin.Access.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))
                    ))
                {
                    administratorList.Add(admin);
                }
            }
        }
        else
        {
            foreach(Administrator admin in list)
            {
                if (admin.IsActive == true || admin.IsActive == false)
                    administratorList.Add(admin);
            }
        }
        

        rgAdminUser.DataSource = administratorList;
        if(isRebind)
            rgAdminUser.DataBind();
    }
    protected void radGridEntry_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Administrator admin = (Administrator)e.Item.DataItem;
            CheckBox chkAcDec = (CheckBox)e.Item.FindControl("chkAcDec");
            HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");
            hdfId.Value = admin.Id.ToString();
            string userType = string.Empty;

            if (admin.IsActive)
                chkAcDec.Checked = true;
            else
                chkAcDec.Checked = false;


            LinkButton lnkBtnDelete = (LinkButton)e.Item.FindControl("lnkBtnDelete");
            lnkBtnDelete.Attributes.Add("onclick", "return DeleteConfirmation('" + admin.LoginId + "');");
            
            //try {
            //    bool isAny = EntryList.GetEntryList(Guid.Empty, Guid.Empty, "").Where(x => x.AdminidAssignedto == admin.Id).Any();
            //    if (isAny)
            //    {
            //        lnkBtnDelete.Attributes.Clear();
            //        lnkBtnDelete.Attributes.Add("onclick", "return alert(\"This account already tagged to entry(s), can not be removed.\");");
            //        lnkBtnDelete.CommandName = "";
            //    }
            //}
            //catch { }



            if (Security.IsRoleSuperAdmin())
            {
                lnkBtnDelete.Visible = true;
            }
        }
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            //item["Access"].Text = GeneralFunction.GetAdminType(item["Access"].Text); //TODO
        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            //RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            //PageSizeCombo.Items.Clear();
            //PageSizeCombo.Items.Add(new RadComboBoxItem("20", "20"));
            //PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", rgAdminUser.MasterTableView.ClientID);
            //PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            //PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", rgAdminUser.MasterTableView.ClientID);
            //PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            //PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", rgAdminUser.MasterTableView.ClientID);
            //PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            //PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", rgAdminUser.MasterTableView.ClientID);
            //PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            //PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", rgAdminUser.MasterTableView.ClientID);
            //string PageSize = e.Item.OwnerTableView.PageSize.ToString();
            //PageSizeCombo.FindItemByValue(PageSize).Selected = true;
        }
    }
    protected void rgAdminUser_OnPreRender(object sender,EventArgs e)
    {

        if (Security.GetAdminLoginSession().Access == "SA")
        {
            rgAdminUser.MasterTableView.GetColumn("Password").Visible = true;
            rgAdminUser.MasterTableView.GetColumn("AcDec").Visible = false;
            rgAdminUser.MasterTableView.GetColumn("Actions").Visible = true;
        }
        else
        {
            rgAdminUser.MasterTableView.GetColumn("Password").Visible = false;
            rgAdminUser.MasterTableView.GetColumn("AcDec").Visible = false;
            rgAdminUser.MasterTableView.GetColumn("Actions").Visible = false;
        }
    }
    protected void radGridEntry_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false);
    }
    protected void radGridEntry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";
        if(e.CommandName == "EditItem")
        {
            Response.Redirect("Admin.aspx?Id=" + ((GridDataItem)e.Item)["Id"].Text);
        }
        if(e.CommandName == "DeleteItem")
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid AdministratorId = new Guid(item["Id"].Text);
            Administrator.DeleteAdministrator(AdministratorId);
            lblError.Text = item["Name"].Text + " Is deleted<br>";
        }

    }

    protected void chkAcDec_OnCheckedChanged(object sender, EventArgs e)
    {
        foreach(GridDataItem item in rgAdminUser.Items)
        {
            CheckBox chkAcDec = (CheckBox)item.FindControl("chkAcDec");
            HiddenField hdfId = (HiddenField)item.FindControl("hdfId");
            if (hdfId != null)
            {
                try
                {
                    Administrator admin = Administrator.GetAdministrator(new Guid(hdfId.Value));
                    if (admin != null)
                    {
                        if (chkAcDec != null)
                        {
                            if (chkAcDec.Checked)
                                admin.IsActive = true;
                            else
                                admin.IsActive = false;
                            admin.Save();
                        }
                    }
                }
                catch { }
            }

        }
        BindGrid(true);

    }
    protected void lnkBtnAdd_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("Admin.aspx");
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