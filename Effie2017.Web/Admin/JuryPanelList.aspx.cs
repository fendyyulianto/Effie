using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Effie2017.App;
using System.Data;

public partial class Admin_JuryPanelList : PageSecurity_Admin
{
    string round = "";
    int numberOfCats = 8;

    protected void Page_Load(object sender, EventArgs e)
    {
        round = Request.QueryString["r"];

        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }
    }
    private void PopulateForm()
    {
        // Pop the jpc data
        JuryPanelCategoryList jpcList = JuryPanelCategoryList.GetJuryPanelCategoryList("", "");
        
        DropDownList firstDdl = null;
        DropDownList secDdl = null;
        int rptCount = 0;
        int rptJPC = 0;

        foreach (RepeaterItem item in rptPanel.Items)        
        {
            string thispanelno = ((Label)item.FindControl("lbPanelNo")).Text;
                // loop all the ddls in the repeater for a match

            List<JuryPanelCategory> filteredJPCList = jpcList.Where(m => m.PanelId.Equals(thispanelno) && m.Round == round).OrderBy(m => m.OrderNo).ToList();

            

            foreach (JuryPanelCategory jpc in filteredJPCList)
            {
                //if (jpc.Round == round)
                //{                    
                    for (int i = 1; i <= numberOfCats; i++)
                    {
                        DropDownList ddlCategory = (DropDownList)item.FindControl("ddlCategory" + i.ToString());
                        if (thispanelno == jpc.PanelId && i == jpc.OrderNo)
                        {
                            if (rptJPC == 0 && rptCount == 0) firstDdl = ddlCategory;
                            if (rptJPC == 1 && rptCount == 0) secDdl = ddlCategory;

                            ddlCategory.SelectedValue = jpc.CategoryPSDetail;
                            //PopulateAllDDL(ddlCategory);                            
                            PopulateNumberEntries(ddlCategory);                            
                        }


                    }
                //}
                rptJPC++;
            }                        
            rptCount++;
        }

        PopulateAllDDL(firstDdl);
        PopulateAllDDL(secDdl);
        

        // Hide the save as draft if all confirmed
        if (jpcList.Count > 0)
        {
            btnSave.Visible = !jpcList[0].IsActive;
            if (!jpcList[0].IsActive)
                btnConfirm.Attributes.Add("onclick", "return confirm('Confirm to submit?');");
        }
        else
            btnConfirm.Attributes.Add("onclick", "return confirm('Confirm to submit?');");

        if (Security.IsReadOnlyAdmin())
        {
            GeneralFunctionEntryForm.DisableAllAction(this, false);
            btnConfirm.Visible = !Security.IsReadOnlyAdmin();
        }
    }
    private void LoadForm()
    {
        // Refresh a copy of all entries
        GeneralFunction.GetAllEntryCache(true);

        ltRound.Text = Request.QueryString["r"];

        BindPanels();        
        PopulateAllDDL(null);        
    }
    private void BindPanels()
    {
        rptPanel.DataSource = GeneralFunction.GetJuryPanelList(round);
        rptPanel.DataBind();
    }

    #region Events
    protected void rptPanel_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string panel = (string)e.Item.DataItem;
        if (panel != null)
        {
            // number of panel
            Label lbPanelNo = (Label)e.Item.FindControl("lbPanelNo");
            if (lbPanelNo != null)
            {
                lbPanelNo.Text = panel;
            }

            // list of jury
            int jurycount = 0;
            RadGrid radGridJury = (RadGrid)e.Item.FindControl("radGridJury");
            if (radGridJury != null)
            {
                List<EffieJuryManagementApp.Jury> jlist = GeneralFunction.GetJuryListFromPanel(panel, round);
                radGridJury.DataSource = jlist;
                jurycount = jlist.Count;
            }

            // number of jury
            Label lbJuryCount = (Label)e.Item.FindControl("lbJuryCount");
            if (lbJuryCount != null)
            {
                lbJuryCount.Text = jurycount.ToString();
            }
        }
    }
    protected void radGridJury_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            EffieJuryManagementApp.Jury jury = (EffieJuryManagementApp.Jury)e.Item.DataItem;

            LinkButton lnkBtn = null;
            Label lbl = null;
            HyperLink lnk = null;

            // Jury Name
            lnkBtn = (LinkButton)e.Item.FindControl("btnJuryName");
            lnkBtn.Text = jury.FirstName + " " + jury.LastName;
            lnkBtn.CommandArgument = jury.Id.ToString();

        }
    }
    protected void radGridJury_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "ViewJury")
        {
            GeneralFunction.SetRedirect("../Admin/JuryPanelList.aspx?r=" + round);  // to return from whereever
            Response.Redirect("../Admin/Jury.aspx?juryId=" + e.CommandArgument.ToString() + "&v=1");
        }
    }
    protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lbError.Text = "";
        DropDownList ddl = (DropDownList)sender;
        PopulateAllDDL(ddl);
        PopulateNumberEntries(ddl);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Save(false);
        lbError.Text = "Draft panels saved.";
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        Save(true);
        lbError.Text = "Panels submitted.";
        btnSave.Visible = false;
    }
    #endregion

    #region Helper
    private void BindCategoryData(DropDownList ddlCategory, List<string> excludevalues, string prevselectedvalue)
    {
        // Category
        DataTable dt1 = Category.GetSubcategories("01");
        DataTable dt2 = Category.GetSubcategories("02");
        DataTable dt3 = Category.GetSubcategories("03");
        DataTable dt4 = Category.GetSubcategories("04");

        ddlCategory.Items.Add(new ListItem("Select One", ""));

        BindCategoryData2(ddlCategory, dt1, "Single - Products and Services", "SP", excludevalues, prevselectedvalue);
        BindCategoryData2(ddlCategory, dt2, "Single - Specialty", "SS", excludevalues, prevselectedvalue);
        BindCategoryData2(ddlCategory, dt3, "Multi - Products and Services", "MP", excludevalues, prevselectedvalue);
        BindCategoryData2(ddlCategory, dt4, "Multi - Specialty", "MS", excludevalues, prevselectedvalue);
    }
    private void BindCategoryData2(DropDownList ddlCategory, DataTable dt, string title, string code, List<string> excludevalues, string prevselectedvalue)
    {
        ddlCategory.Items.Add(SeparatorItem());
        ddlCategory.Items.Add(SeparatorItemWithText(title));
        ddlCategory.Items.Add(SeparatorItem());
        foreach (DataRow dr in dt.Rows)
        {
            string newdatavalue = code + "-" + dr["Name"].ToString();
            string newtextvalue = newdatavalue;
            List<Entry> elist = GeneralFunction.GetEntryListFromCategory(newdatavalue, round);
            newtextvalue += " (" + elist.Count.ToString() + ")";
            if (!excludevalues.Contains(newdatavalue) || prevselectedvalue == newdatavalue)
                ddlCategory.Items.Add(new ListItem(newtextvalue, newdatavalue));
        }
    }
    private void PopulateAllDDL(DropDownList excludeddl)
    {
        //DataTable dt = GeneralFunction.GetAllCategoryCache(false);

        // get all exluded the values
        List<string> allexluded = new List<string>();
        foreach (RepeaterItem item in rptPanel.Items)
        {
            for (int i = 1; i <= numberOfCats; i++)
            {
                DropDownList ddlCategory = (DropDownList)item.FindControl("ddlCategory" + i.ToString());
                if (ddlCategory.SelectedValue != "") 
                    allexluded.Add(ddlCategory.SelectedValue);
            }
        }

        // get the panel number
        string panelno = "";
        if (excludeddl != null)
        {
            RepeaterItem ritem = (RepeaterItem)excludeddl.Parent;
            panelno = ((Label)ritem.FindControl("lbPanelNo")).Text;
        }

        // bind all the ddl 
        foreach (RepeaterItem item in rptPanel.Items)
        {
            string thispanelno = ((Label)item.FindControl("lbPanelNo")).Text;

            for (int i = 1; i <= numberOfCats; i++)
            {
                DropDownList ddlCategory = (DropDownList)item.FindControl("ddlCategory" + i.ToString());


                if (excludeddl == null || 
                    (excludeddl != null && 
                     (excludeddl.ID != "ddlCategory" + i.ToString() ||
                     (excludeddl.ID == "ddlCategory" + i.ToString() && thispanelno != panelno))
                     )
                   ) 
                {
                    string prevselectedvalue = ddlCategory.SelectedValue;

                    ddlCategory.Items.Clear();
                    ddlCategory.SelectedIndex = -1;
                    ddlCategory.SelectedValue = null;
                    ddlCategory.ClearSelection();
                    BindCategoryData(ddlCategory, allexluded, prevselectedvalue);

                    // insert it only if its autopostback (ddl not null) and prev value selected is not empty
                    if (excludeddl != null && prevselectedvalue != "")
                    {
                        ddlCategory.SelectedValue = prevselectedvalue;
                    }
                }
            }
        }
    }
    private void PopulateNumberEntries(DropDownList ddl)
    {
        RepeaterItem ritem = (RepeaterItem)ddl.Parent;
        string panelno = ((Label)ritem.FindControl("lbPanelNo")).Text;
        string ddlname = ddl.ID;
        string number = ddlname.Substring(ddlname.Length - 1, 1);

        Label lbEntriesCount = (Label)ritem.FindControl("lbEntriesCount" + number);
        List<Entry> elist = new List<Entry>();
        if (ddl.SelectedValue != "") 
            elist = GeneralFunction.GetEntryListFromCategory(ddl.SelectedValue, round);
        lbEntriesCount.Text = elist.Count.ToString();

        // total 
        CalTotalEntries(ddl);
    }
    private void CalTotalEntries(DropDownList ddl)
    {
        RepeaterItem ritem = (RepeaterItem)ddl.Parent;
        string panelno = ((Label)ritem.FindControl("lbPanelNo")).Text;

        int total = 0;

        for (int i = 1; i <= numberOfCats; i++)
        {
            Label lbEntriesCount = (Label)ritem.FindControl("lbEntriesCount" + i.ToString());
            total += int.Parse(lbEntriesCount.Text);
        }

        Label lbEntriesCountTotal = (Label)ritem.FindControl("lbEntriesCountTotal");
        lbEntriesCountTotal.Text = total.ToString();
    }
    private void Save(bool isActive)
    {
        // Delete all old records
        JuryPanelCategoryList jpcList = JuryPanelCategoryList.GetJuryPanelCategoryList("", "");
        foreach (JuryPanelCategory jpc in jpcList)
        {
            if (jpc.Round == round)
                JuryPanelCategory.DeleteJuryPanelCategory(jpc.Id);
        }

        // save all ddls
        foreach (RepeaterItem item in rptPanel.Items)
        {
            string thispanelno = ((Label)item.FindControl("lbPanelNo")).Text;
            for (int i = 1; i <= numberOfCats; i++)
            {
                DropDownList ddlCategory = (DropDownList)item.FindControl("ddlCategory" + i.ToString());


                JuryPanelCategory jpc = JuryPanelCategory.NewJuryPanelCategory();
                jpc.PanelId = thispanelno;
                jpc.CategoryPSDetail = ddlCategory.SelectedValue;
                jpc.OrderNo = i;
                jpc.IsActive = isActive;
                jpc.Round = round;
                jpc.Save();
            }
        }

    }
    private ListItem SeparatorItem()
    {
        ListItem separator = new ListItem("-------------------------------", "");
        separator.Attributes.Add("disabled", "disabled");

        return separator;
    }
    private ListItem SeparatorItemWithText(string text)
    {
        ListItem separator = new ListItem(text, "");
        separator.Attributes.Add("disabled", "disabled");

        return separator;
    }
    #endregion

}