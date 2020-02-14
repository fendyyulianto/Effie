using System;
using System.Web.UI.WebControls;
using System.Linq;
using EffieJuryManagementApp;
using Telerik.Web.UI;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;
using System.Text.RegularExpressions;

public partial class Main_InvitationList : PageSecurity_Admin
{
    List<Jury> juryList = JuryList.GetJuryList().ToList();

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            LoadForm();
            PopulateForm();
        }

        string parameter = Request["__EVENTARGUMENT"];
        if (parameter == "genericEmail")
            btnSend_Click(sender, e);        
    }

    private void PopulateForm()
    {
        // Refresh the cache
        GeneralFunction.ResetReportDataCache();
        GeneralFunction.GetAllJuryCache(true);

        if (GeneralFunction.GetFilterPageId() == "InvitationList")
        {
            txtSearch.Text = GeneralFunction.GetFilterF1();
            ddlSearch.SelectedValue = GeneralFunction.GetFilterF2();
            ddlAccepted.SelectedValue = GeneralFunction.GetFilterF3();
            ddlType.SelectedValue = GeneralFunction.GetFilterF4();
            ddlShortlisted.SelectedValue = GeneralFunction.GetFilterF5();
            ddlInvitation.SelectedValue = GeneralFunction.GetFilterF6();
            ddlAssigned.SelectedValue = GeneralFunction.GetFilterF7();
            ddlRead.SelectedValue = GeneralFunction.GetFilterF8();
            ddlActive.SelectedValue = GeneralFunction.GetFilterF9();
            radGridJury.CurrentPageIndex = Convert.ToInt32(GeneralFunction.GetFilterPageNo());
            ViewState["AdvanceSearch"] = "1";

            HighLightYearTab(GeneralFunction.GetFilterF10());            
        }

        BindGrid(true, string.Empty, string.Empty);

        if (Security.IsRoleReadOnlyAdmin())
        {
            btnSendEmail.Visible = false;
            btnDisableJury.Visible = false;
            btnEnableJury.Visible = false;
        }
    }

    private void LoadForm()
    {
        btnExport.Visible = Security.IsRoleSuperAdmin();
        btnSummaryReport.Visible = Security.IsRoleSuperAdmin();

        // Experience Years
        List<string> effieExpYears = GeneralFunction.GetEffieExperienceYears();
        rptEffieYears.DataSource = effieExpYears;
        rptEffieYears.DataBind();

        Security.SecureControlByHiding(btnExport, "EXPORT");
        Security.SecureControlByHiding(btnSummaryReport, "EXPORT");


        // Country
        ddlCountry.DataSource = GeneralFunction.GetFilteredCountryListFromJury(true);
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new ListItem("All", ""));

        // Network
        GeneralFunction.LoadddlNetwork(ddlNetwork, false, false);
        ddlNetwork.Items.Insert(0, new ListItem("All", ""));

        // Holding COmpany
        GeneralFunction.LoadddlHoldingCompany(ddlHoldingCompany, false, false);
        ddlHoldingCompany.Items.Insert(0, new ListItem("All", ""));

    }

    private void BindGrid(bool needRebind, string sortExpression, string sortOrder)
    {
        string invitationYear = !String.IsNullOrEmpty(GetHighLightedYearTab()) ? GetHighLightedYearTab() : GeneralFunction.GetEffieExperienceYears().Last().ToString();  // By Default, get latest year invitation list when page loads
        HighLightYearTab(invitationYear);

        InvitationList list = InvitationList.GetInvitationList(Guid.Empty, invitationYear);

        string advanceSearch = (string)ViewState["AdvanceSearch"];

        List<Invitation> filteredStatusList = new List<Invitation>();
        filteredStatusList = GetAllFilterByStatus(list);

        List<Invitation> flist = new List<Invitation>();

        if (advanceSearch == "1")
        {
            foreach (Invitation item in filteredStatusList)
            {
                Jury jury = Jury.GetJury(item.JuryId);

                if (
                    (ddlInvitation.SelectedValue == "" || (ddlInvitation.SelectedValue != "" && IsddlInvitation(ddlInvitation.SelectedValue, jury))) &&
                    (ddlType.SelectedValue == "" || (ddlType.SelectedValue != "" && jury.Type == ddlType.SelectedValue)) &&
                    (ddlNetwork.SelectedValue == "" || (ddlNetwork.SelectedValue != "" && jury.Network == ddlNetwork.SelectedValue)) &&
                    (ddlHoldingCompany.SelectedValue == "" || (ddlHoldingCompany.SelectedValue != "" && jury.HoldingCompany == ddlHoldingCompany.SelectedValue)) &&
                    (ddlSpecialistType.SelectedValue == "" || (ddlSpecialistType.SelectedValue != "" && jury.HoldingCompany == ddlSpecialistType.SelectedValue)) &&
                    (ddlCountry.SelectedValue == "" || (ddlCountry.SelectedValue != "" && jury.Country == ddlCountry.SelectedValue)) &&
                    (
                     (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "judgeId") && jury.SerialNo.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "name") && (jury.FirstName.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1 || jury.LastName.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "title") && jury.Designation.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1)) ||
                    (txtSearch.Text.Trim() == "" || (txtSearch.Text.Trim() != "" && (ddlSearch.SelectedValue == "" || ddlSearch.SelectedValue == "company") && jury.Company.ToUpper().IndexOf(txtSearch.Text.Trim().ToUpper()) != -1))
                   ))
                {
                    flist.Add(item);
                }
                
                    
            }
        }
        else
        {
            foreach (Invitation jury in filteredStatusList)
            {
                flist.Add(jury);
            }
        }
        
        // Sort
        flist = flist.OrderByDescending(p => p.DateCreated).ToList();

        #region CustomSort
        if (!String.IsNullOrEmpty(sortExpression))
        {
            SetSortExpression(sortExpression);
            SetSortOrder(sortOrder);

            if (sortExpression.Equals("SerialNo"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.SerialNo ascending
                                     select inv;

                    flist = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.SerialNo descending
                                     select inv;

                    flist = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("Type"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.Type ascending
                                     select inv;

                    flist = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.Type descending
                                     select inv;

                    flist = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("Name"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.FirstName ascending
                                     select inv;

                    flist = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.FirstName descending
                                     select inv;

                    flist = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("Company"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.Company ascending
                                     select inv;

                    flist = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.Company descending
                                     select inv;

                    flist = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("JuryUpdate"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby GetJuryUpdate(jury) ascending
                                     select inv;

                    flist = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby GetJuryUpdate(jury) descending
                                     select inv;

                    flist = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("Title"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.Designation ascending
                                     select inv;

                    flist = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.Designation descending
                                     select inv;

                    flist = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("Country"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.Country ascending
                                     select inv;

                    flist = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.Country descending
                                     select inv;

                    flist = resultList.ToList();
                }
            }
            else if (sortExpression.Equals("IsRound1Invited"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRound1Invited).ToList();                   
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRound1Invited).ToList();    
                }
            }
            else if (sortExpression.Equals("IsRound2Invited"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRound2Invited).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRound2Invited).ToList();
                }
            }
            else if (sortExpression.Equals("IsDeclined"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsDeclined).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsDeclined).ToList();
                }
            }
            else if (sortExpression.Equals("IsRound1Accepted"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRound1Accepted).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRound1Accepted).ToList();
                }
            }
            else if (sortExpression.Equals("IsRound2Accepted"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRound2Accepted).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRound2Accepted).ToList();
                }
            }
            else if (sortExpression.Equals("IsRound1Shortlisted"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRound1Shortlisted).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRound1Shortlisted).ToList();
                }
            }
            else if (sortExpression.Equals("IsRound2Shortlisted"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRound2Shortlisted).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRound2Shortlisted).ToList();
                }
            }
            else if (sortExpression.Equals("IsRound1Assigned"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRound1Assigned).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRound1Assigned).ToList();
                }
            }
            else if (sortExpression.Equals("IsRound2Assigned"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRound2Assigned).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRound2Assigned).ToList();
                }
            }
            else if (sortExpression.Equals("DateRound1EmailSent"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.DateRound1EmailSent).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.DateRound1EmailSent).ToList();
                }
            }
            else if (sortExpression.Equals("DateRound2EmailSent"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.DateRound2EmailSent).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.DateRound2EmailSent).ToList();
                }
            }
            else if (sortExpression.Equals("IsRead"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    flist = flist.OrderBy(m => m.IsRead).ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    flist = flist.OrderByDescending(m => m.IsRead).ToList();
                }
            }
            else if (sortExpression.Equals("IsActive"))
            {
                if (sortOrder.Equals(GridSortOrder.Ascending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.IsActive 
                                     select inv;

                    flist = resultList.ToList();
                }
                else if (sortOrder.Equals(GridSortOrder.Descending.ToString()))
                {
                    var resultList = from inv in flist
                                     join jury in juryList
                                     on inv.JuryId equals jury.Id
                                     orderby jury.IsActive descending
                                     select inv;

                    flist = resultList.ToList();
                }
            }
        }
        #endregion

        radGridJury.DataSource = flist;
        if (needRebind) radGridJury.Rebind();

        GeneralFunction.SetReportDataCache(flist);        
    }

    protected bool IsddlInvitation(string ddlInvitation, Jury jury)
    {
        try {
            //GeneralFunction.GetFilterF10()
            string invitationYear = !String.IsNullOrEmpty(GetHighLightedYearTab()) ? GetHighLightedYearTab() : GeneralFunction.GetEffieExperienceYears().Last().ToString();  // By Default, get latest year invitation list when page loads
            Invitation inv = InvitationList.GetInvitationList(jury.Id, invitationYear).FirstOrDefault();

            if (ddlInvitation == "R1" && inv.IsRound1Invited)
            {
                return true;
            }
            else if (ddlInvitation == "R2" && inv.IsRound2Invited)
            {
                return true;
            }
            else if (ddlInvitation == "NOT" && inv.IsDeclined)
            {
                return true;
            }
            else if (ddlInvitation == "NORes" && !inv.IsDeclined && !inv.IsRound1Accepted && !inv.IsRound2Accepted)
            {
                return true;
            }
        }
        catch { }

        return false;
    }

    public void PopulateTemplatePanel(Button pressedButton)
    {
        lblTempError.Text = string.Empty;
        lblError.Text = string.Empty;

        IptechLib.Forms.RemoveHighlightControls(phSelectTemplate);

        List<EmailTemplate> defaultEmailTempalteList = EmailTemplateList.GetEmailTemplateList(Guid.Empty).Where(m => m.TemplateId != new Guid(Gen_GeneralUseValueList.GetGen_GeneralUseValueList("DefaultTemplateId")[0].Value)
            && m.IsActive /*&& !m.IsInvitation*/ && !m.IsDelete).ToList();

        if (defaultEmailTempalteList.Count == 0)
            lblError.Text = "No Email Template found.<br/>";

        phSelectTemplate.Visible = (defaultEmailTempalteList.Count != 0);

        if (defaultEmailTempalteList.Count != 0)
        {
            ddlTemplateList.DataSource = defaultEmailTempalteList;
            ddlTemplateList.DataTextField = "Title";
            ddlTemplateList.DataValueField = "Id";
            ddlTemplateList.DataBind();

            ddlTemplateList.Items.Insert(0, new ListItem("Please Select", Guid.Empty.ToString()));

            hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());

            lblTitle.Text = pressedButton.Text;
        }
    }

    #region Events

    protected void rptEffieYears_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string year = (string)e.Item.DataItem;
            if (year != null)
            {
                LinkButton lnkYear = (LinkButton)e.Item.FindControl("lnkYear");
                lnkYear.Text = year.ToString();
                lnkYear.CommandArgument = year.ToString();
            }
        }
    }

    protected void rptEffieYears_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("year"))
        {
            radGridJury.MasterTableView.SortExpressions.Clear();

            GeneralFunction.SetFilter("InvitationList", txtSearch.Text, ddlSearch.SelectedValue, ddlAccepted.SelectedValue, ddlType.SelectedValue, ddlShortlisted.SelectedValue, ddlInvitation.SelectedValue, ddlAssigned.SelectedValue, ddlRead.SelectedValue, ddlActive.SelectedValue, e.CommandArgument.ToString(), string.Empty, "0");
            ViewState["AdvanceSearch"] = "1";
            HighLightYearTab(e.CommandArgument.ToString());            
            BindGrid(true, string.Empty, string.Empty);
            lblError.Text = "";
        }
    }

    protected void radGridJury_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        BindGrid(true, e.SortExpression, e.NewSortOrder.ToString());
    }

    protected void radGridJury_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            Invitation invitation = (Invitation)e.Item.DataItem;

            if (invitation != null)
            {
                try {
                    Jury jury = Jury.GetJury(invitation.JuryId);

                    if (jury != null)
                    {
                        LinkButton lnkBtn = null;
                        Label lbl = null;
                        CheckBox chkBox = null;
                        HyperLink lnk = null;

                        HiddenField hdfId = (HiddenField)e.Item.FindControl("hdfId");
                        hdfId.Value = invitation.Id.ToString();

                        HiddenField hdfJuryId = (HiddenField)e.Item.FindControl("hdfJuryId");
                        hdfJuryId.Value = jury.Id.ToString();

                        lbl = (Label)e.Item.FindControl("lblType");
                        lbl.Text = jury.Type;

                        // Jury Id
                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnJuryId");
                        lnkBtn.Text = jury.SerialNo;
                        lnkBtn.CommandArgument = jury.Id.ToString();

                        // Jury Name
                        lnk = (HyperLink)e.Item.FindControl("lnkJuryName");
                        lnk.Text = jury.FirstName + " " + jury.LastName;
                        lnk.NavigateUrl = "mailto:" + jury.Email;



                        lbl = (Label)e.Item.FindControl("lblCompany");
                        lbl.Text = jury.Company;

                        lbl = (Label)e.Item.FindControl("lblTitle");
                        lbl.Text = jury.Designation;

                        lbl = (Label)e.Item.FindControl("lblCountry");
                        lbl.Text = jury.Country;

                        lbl = (Label)e.Item.FindControl("lblRound1EmailSent");
                        if (!String.IsNullOrEmpty(invitation.DateRound1EmailSentString))
                            lbl.Text = invitation.DateRound1EmailSent.ToString("dd/MM/yyyy");

                        lbl = (Label)e.Item.FindControl("lblRound2EmailSent");
                        if (!String.IsNullOrEmpty(invitation.DateRound2EmailSentString))
                            lbl.Text = invitation.DateRound2EmailSent.ToString("dd/MM/yyyy");

                        chkBox = (CheckBox)e.Item.FindControl("chkInvRound1");
                        chkBox.Checked = invitation.IsRound1Invited;

                        chkBox = (CheckBox)e.Item.FindControl("chkInvRound2");
                        chkBox.Checked = invitation.IsRound2Invited;

                        chkBox = (CheckBox)e.Item.FindControl("chkDecline");
                        chkBox.Checked = invitation.IsDeclined;

                        chkBox = (CheckBox)e.Item.FindControl("chkAccptRound1");
                        chkBox.Checked = invitation.IsRound1Accepted;

                        chkBox = (CheckBox)e.Item.FindControl("chkAccptRound2");
                        chkBox.Checked = invitation.IsRound2Accepted;

                        chkBox = (CheckBox)e.Item.FindControl("chkShortListedRound1");
                        chkBox.Checked = invitation.IsRound1Shortlisted;

                        chkBox = (CheckBox)e.Item.FindControl("chkShortListedRound2");
                        chkBox.Checked = invitation.IsRound2Shortlisted;

                        chkBox = (CheckBox)e.Item.FindControl("chkAssignRound1");
                        chkBox.Checked = invitation.IsRound1Assigned;

                        chkBox = (CheckBox)e.Item.FindControl("chkAssignRound2");
                        chkBox.Checked = invitation.IsRound2Assigned;

                        chkBox = (CheckBox)e.Item.FindControl("chkRead");
                        chkBox.Checked = invitation.IsRead;

                        chkBox = (CheckBox)e.Item.FindControl("chkActive");
                        chkBox.Checked = jury.IsActive;

                        lbl = (Label)e.Item.FindControl("lblJuryUpdate");
                        lbl.Text = (GetJuryUpdate(jury) == DateTime.MaxValue) ? "" : GetJuryUpdate(jury).ToString("dd/MM/yy");

                        lnkBtn = (LinkButton)e.Item.FindControl("lnkBtnEdit");
                        lnkBtn.CommandArgument = invitation.Id.ToString();

                        lnk = (HyperLink)e.Item.FindControl("hlkEmailHistory");
                        lnk.CssClass = "fancybox fancybox.iframe tblLinkRed";
                        lnk.NavigateUrl = "./EmailSentHistory.aspx?juryId=" + jury.Id.ToString() + "&tempMode=INV&eventYear=" + GetHighLightedYearTab();
                    }
                }
                catch { }
                
            }

        }
        else if (e.Item.ItemType == GridItemType.Pager)
        {
            RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            PageSizeCombo.Items.Clear();
            PageSizeCombo.Items.Add(new RadComboBoxItem("50", "50"));
            PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("100", "100"));
            PageSizeCombo.FindItemByText("100").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("200", "200"));
            PageSizeCombo.FindItemByText("200").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.Items.Add(new RadComboBoxItem("All", "99999"));
            PageSizeCombo.FindItemByText("All").Attributes.Add("ownerTableViewId", radGridJury.MasterTableView.ClientID);
            PageSizeCombo.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
        }
    }

    protected DateTime GetJuryUpdate(Jury jury)
    {
        DateTime JuryUpdate = DateTime.MaxValue;
        if (!jury.DateJuryModified.Equals(DateTime.MaxValue) && !jury.DateModified.Equals(DateTime.MaxValue))
        {
            if (jury.DateJuryModified > jury.DateModified)
            {
                JuryUpdate = jury.DateJuryModified;
            }
            else if (jury.DateModified > jury.DateJuryModified)
            {
                JuryUpdate = jury.DateModified;
            }
        }
        else if (!jury.DateJuryModified.Equals(DateTime.MaxValue))
        {
            JuryUpdate = jury.DateJuryModified;
        }
        else if (!jury.DateModified.Equals(DateTime.MaxValue))
        {
            JuryUpdate = jury.DateModified;
        }

        return JuryUpdate;
    }

    protected void radGridJury_ItemCommand(object sender, GridCommandEventArgs e)
    {
        lblError.Text = "";

        if (e.CommandName == "Edit")
        {
            GeneralFunction.SetFilter("InvitationList", txtSearch.Text, ddlSearch.SelectedValue, ddlAccepted.SelectedValue, ddlType.SelectedValue, ddlShortlisted.SelectedValue, ddlInvitation.SelectedValue, ddlAssigned.SelectedValue, ddlRead.SelectedValue, ddlActive.SelectedValue, GetHighLightedYearTab(), string.Empty, radGridJury.CurrentPageIndex.ToString());
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Main/InvitationList.aspx");  // to return from whereever
            Response.Redirect("../Main/Invitation.aspx?invId=" + e.CommandArgument.ToString());
        }
        if (e.CommandName == "ViewJury")
        {
            GeneralFunction.SetFilter("InvitationList", txtSearch.Text, ddlSearch.SelectedValue, ddlAccepted.SelectedValue, ddlType.SelectedValue, ddlShortlisted.SelectedValue, ddlInvitation.SelectedValue, ddlAssigned.SelectedValue, ddlRead.SelectedValue, ddlActive.SelectedValue, GetHighLightedYearTab(), string.Empty, radGridJury.CurrentPageIndex.ToString());
            //Security.SetLoginSessionUser(GeneralFunction.GetDummyRegistrationForAdminSpoof());
            GeneralFunction.SetRedirect("../Main/InvitationList.aspx");  // to return from whereever
            Response.Redirect("../Main/Jury.aspx?v=1&juryId=" + e.CommandArgument.ToString());
        }
    }

    protected void radGridJury_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGrid(false, GetSortExpression(), GetSortOrder());
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //rtabEntry.Visible = false;
        GeneralFunction.SetFilter("InvitationList", txtSearch.Text, ddlSearch.SelectedValue, ddlAccepted.SelectedValue, ddlType.SelectedValue, ddlShortlisted.SelectedValue, ddlInvitation.SelectedValue, ddlAssigned.SelectedValue, ddlRead.SelectedValue, ddlActive.SelectedValue, GetHighLightedYearTab(), string.Empty, "0");
        ViewState["AdvanceSearch"] = "1";
        BindGrid(true, string.Empty, string.Empty);
        lblError.Text = "";
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSearch.SelectedValue = "";
        ddlType.SelectedValue = "";
        ddlInvitation.SelectedValue = "";
        ddlAccepted.SelectedValue = "";
        ddlShortlisted.SelectedValue = "";
        ddlAssigned.SelectedValue = "";
        ddlRead.SelectedValue = "";
        ddlActive.SelectedValue = "";
        ResetYearTabs();

        ViewState["AdvanceSearch"] = "";
        ViewState["TabFilterValue"] = "";

        ViewState["SortExpression"] = "";
        ViewState["SortOrder"] = "";

        GeneralFunction.ResetFilter();
        GeneralFunction.ResetReportDataCache();

        radGridJury.MasterTableView.SortExpressions.Clear();

        //rtabEntry.SelectedIndex = 0;
        BindGrid(true, string.Empty, string.Empty);
        lblError.Text = "";


    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Jury.aspx");
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Invitation> flist = (List<Invitation>)data;

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Jury Listing";
            workbook.Worksheets.Add(sheetName);
            x = 1;

            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;


            // basic details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Type"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("JudgeId"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Password"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Salutation"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Contact"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Mobile"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Address2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("City"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Postal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Name"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Tel"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Address1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Address2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PA Email"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Bio"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Type of Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Network"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Holding Company"); x++;

            #endregion

            #region Market Exp Headers

            List<string> marketExpItems = GeneralFunction.GetMarketExperienceItems();
            foreach (string marketExp in marketExpItems)
            {
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(marketExp); x++;
            }
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Others"); x++;

            #endregion

            #region Industry Exp Headers

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AG"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("AU"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("BW"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("BA"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CE"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("CR"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FP"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FM"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("FD"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("GV"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HC"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("HS"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("IT"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("ME"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RE"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RS"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("RT"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("TT"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Others"); x++;

            #endregion

            #region Misc Headers

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Specialist Skills"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Specialist Skills - Others"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("APAC Effie Exp"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Effie Programs"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Effie Programs - Others"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Judging Experience"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Other Relevant Information"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Remarks"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Source"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Reference"); x++;


            #endregion

            #region Invitation Response

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Invitation R1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Invitation R2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Declined"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Accepted R1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Accepted R2"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Shortlisted R1"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Shortlisted R2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assigned R1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assigned R2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("R1 Email Sent"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("R2 Email Sent"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Read"); x++;
            

            #endregion

            y++;
            int count = 0;
            foreach (Invitation inv in flist)
            {
                count++;
                //if (count <= 60 || count >= 62) continue;

                x = 1;

                Jury jury = Jury.GetJury(inv.JuryId);

                #region Basic Jury DataRows

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Type); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SerialNo); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Email); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Password); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.FirstName); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.LastName); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Designation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(IptechLib.Text.ShowFriendlyContact(jury.Contact)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(IptechLib.Text.ShowFriendlyContact(jury.Mobile)); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Company); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Address1); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Address2); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.City); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Postal); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Country); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAName); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(IptechLib.Text.ShowFriendlyContact(jury.PATel)); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAAddress1); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAAddress2); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAEmail); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(ReplaceString(jury.Profile)); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.CompanyType); x++;
                string network = jury.Network;
                if (jury.NetworkOthers != "") network += " - " + jury.NetworkOthers;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(network); x++;
                string holdingcompany = jury.HoldingCompany;
                if (jury.HoldingCompanyOthers != "") holdingcompany += " - " + jury.HoldingCompanyOthers;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(holdingcompany); x++;


                #endregion

                #region Market Exp

                foreach (string marketExp in marketExpItems)
                {
                    if (jury.MarketExp.IndexOf(marketExp) != -1)
                        workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                    x++;
                }
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.MarketExpOthers); x++;

                #endregion

                #region Industry Exp

                if (jury.IndustryExp.IndexOf("Agricultural & Industrial") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Automotive") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Beauty & Wellness") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Beverages") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Consumer Electronics and Durables") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Corporate Reputation/Professional Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Financial Products & Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("FMCG") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Food") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Government / Institutional") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Healthcare") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Household Supplies & Services") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("IT /Telco") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Media, Entertainment & Leisure") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Real Estate") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Restaurants") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Retail") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;
                if (jury.IndustryExp.IndexOf("Travel / Tourism") != -1) workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("1");
                x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.IndustryExpOthers); x++;

                #endregion

                #region Misc

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Skills); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SkillsOthers); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.EffieExpYear); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.EffieExpProgram); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.EffieExpProgramOthers); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.OtherJudgingExp); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.RevelantExp); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Remarks); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Source); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Reference); x++;

                #endregion

                #region Invitation Response

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(inv.IsRound1Invited ? "Yes" : "No"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(inv.IsRound2Invited ? "Yes" : "No"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(inv.IsDeclined ? "Yes" : "No"); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((inv.IsRound1Accepted ? "Yes" : "No")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((inv.IsRound2Accepted ? "Yes" : "No")); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(inv.IsLocked ? (inv.IsRound1Shortlisted ? "Yes" : "No") : string.Empty); x++;
                //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(inv.IsLocked ? (inv.IsRound2Shortlisted ? "Yes" : "No") : string.Empty); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((inv.IsRound1Assigned ? "Yes" : "No")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((inv.IsRound2Assigned ? "Yes" : "No")); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(!String.IsNullOrEmpty(inv.DateRound1EmailSentString) ? inv.DateRound1EmailSent.ToString("dd/MM/yyyy") : string.Empty); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(!String.IsNullOrEmpty(inv.DateRound2EmailSentString) ? inv.DateRound2EmailSent.ToString("dd/MM/yyyy") : string.Empty); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(inv.IsRead ? "Yes" : "No"); x++;

                #endregion

                y++;

            }


            GeneralFunction.StyleReport(workbook).SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Jury_InvitationList.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    protected string ReplaceString(string str)
    {
        str = str.Replace("\n", " ");
        str = str.Replace("\r", String.Empty);
        str = str.Replace("\t", String.Empty);
        str = str.Replace("\v", " ");
        str = str.Replace("  ", " ");
        return str;
    }


    protected void btnbtnSummaryReport_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        object data = GeneralFunction.GetReportDataCache();

        if (data != null)
        {
            List<Invitation> flist = (List<Invitation>)data;

            XLWorkbook workbook = new XLWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            int x = 1;
            int y = 1;
            string sheetName = "Jury Listing";
            workbook.Worksheets.Add(sheetName);
            x = 1;

            #region Basic Entry Headers
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No."); x++;


            // basic details
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Type"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("JudgeId"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Sal"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Firstname"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Lastname"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Title"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Company"); x++;

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Network"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Holding Company"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Country"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Specialist Type"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Current Industry Sector"); x++;
            
            #endregion

            #region Invitation Response

            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Invite R1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Invite R2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Decline"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Accept R1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Accept R2"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Shortlisted R1"); x++;
            //workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Shortlisted R2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assign R1"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Assign R2"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("R1 Email Sent"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("R2 Email Sent"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Read"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Email"); x++;
            workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("PAEmail"); x++;


            #endregion

            y++;

            foreach (Invitation inv in flist)
            {
                x = 1;

                Jury jury = Jury.GetJury(inv.JuryId);

                #region Basic Jury DataRows

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue((y - 1).ToString()); x++;
                
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Type); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SerialNo); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Salutation); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.FirstName); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.LastName); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Designation); x++;

                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Company); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Network); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.HoldingCompany); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Country); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.SpecialistType); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.CurrentIndustrySector); x++;

                #endregion

                #region Invitation Response
                //////////////////////////////////////////////////////////////////////////////////////////
                if (inv.IsRound1Invited) //"Invitation R1"
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(1); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }

                //////////////////////////////////////////////////////////////////////////////////////////
                if (inv.IsRound2Invited) //"Invitation R2"
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(1); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }

                //////////////////////////////////////////////////////////////////////////////////////////
                if (inv.IsDeclined) //"Declined"
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(1); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                if (inv.IsRound1Accepted)//"Accepted R1" //inv.IsRound1Invited && 
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(1); x++;
                }
                else if (inv.IsRound1Invited)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(0); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                if (inv.IsRound2Accepted)//"Accepted R2" //inv.IsRound2Invited &&
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(1); x++;
                }
                else if (inv.IsRound2Invited)
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(0); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                if (inv.IsRound1Invited && inv.IsRound1Assigned)//"Assigned R1"
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(1); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                if (inv.IsRound2Invited && inv.IsRound2Assigned)//"Assigned R2"
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(1); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                if (!String.IsNullOrEmpty(inv.DateRound1EmailSentString))//"R1 Email Sent"
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(inv.DateRound1EmailSent.ToString("dd/MM/yyyy")); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                if (!String.IsNullOrEmpty(inv.DateRound2EmailSentString)) // "R2 Email Sent"
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(inv.DateRound2EmailSent.ToString("dd/MM/yyyy")); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(string.Empty); x++;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                if (inv.IsRead)//"READ"
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("Yes"); x++;
                }
                else
                {
                    workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue("No"); x++;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion


                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.Email); x++;
                workbook.Worksheets.Worksheet(sheetName).Cell(y, x).SetValue(jury.PAEmail); x++;

                y++;
            }


            GeneralFunction.StyleReport(workbook).SaveAs(memoryStream);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Effie_Jury_InvitationList.xlsx");

            memoryStream.WriteTo(Response.OutputStream);
            Response.End();
        }
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        Button btnCliced = (Button)sender;

        PopulateTemplatePanel(btnCliced);
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        lblTempError.Text = string.Empty;

        lblTempError.Text += IptechLib.Validation.ValidateDropDownList("Select Template", ddlTemplateList, true, Guid.Empty.ToString());

        if (String.IsNullOrEmpty(lblTempError.Text))
        {
            GenerateEmails(new Guid(ddlTemplateList.SelectedValue));
            phSelectTemplate.Visible = false;

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "none", "<script>ClearDataOnLoad();</script>", false);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        phSelectTemplate.Visible = false;
    }

    protected void ddlTemplateList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString()))
        {
            hlkPreview.CssClass = "fancybox fancybox.iframe tblLinkRed";
            hlkPreview.NavigateUrl = "./EmailPreview.aspx?etmId=" + ddlTemplateList.SelectedValue.ToString();
        }

        hlkPreview.Visible = !ddlTemplateList.SelectedValue.Equals(Guid.Empty.ToString());
    }

    protected void btnEnableJury_Click(object sender, EventArgs e)
    {
        EnableDisableJuryAccounts(true);
    }

    protected void btnDisableJury_Click(object sender, EventArgs e)
    {
        EnableDisableJuryAccounts(false);
    }
    
    #endregion

    #region Helper

    public List<Invitation> GetAllFilterByStatus(InvitationList AllInvList)
    {
        List<Invitation> filteredByStatusList = AllInvList.ToList();

        bool isRound1Invited = ddlInvitation.SelectedValue.Equals("R1");
        bool isRound2Invited = ddlInvitation.SelectedValue.Equals("R2");
        bool isDeclined = ddlInvitation.SelectedValue.Equals("NOT");

        if (isRound1Invited)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRound1Invited).ToList();
        if (isRound2Invited)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRound2Invited).ToList();
        if (isDeclined)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsDeclined).ToList();


        bool isRound1Accepted = ddlAccepted.SelectedValue.Equals("R1");
        bool isRound2Accepted = ddlAccepted.SelectedValue.Equals("R2");

        if (isRound1Accepted)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRound1Accepted).ToList();
        if (isRound2Accepted)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRound2Accepted).ToList();

        bool isRound1Shortlisted = ddlShortlisted.SelectedValue.Equals("R1");
        bool isRound2Shortlisted = ddlShortlisted.SelectedValue.Equals("R2");

        if (isRound1Shortlisted)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRound1Shortlisted).ToList();
        if (isRound2Shortlisted)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRound2Shortlisted).ToList();

        bool isRound1Assigned = ddlAssigned.SelectedValue.Equals("R1");
        bool isRound2Assigned = ddlAssigned.SelectedValue.Equals("R2");

        if (isRound1Assigned)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRound1Assigned).ToList();
        if (isRound2Assigned)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRound2Assigned).ToList();

        bool isRead = ddlRead.SelectedValue.Equals("1");
        bool isNotRead = ddlRead.SelectedValue.Equals("0");

        if (isRead)
            filteredByStatusList = filteredByStatusList.Where(m => m.IsRead).ToList();
        if (isNotRead)
            filteredByStatusList = filteredByStatusList.Where(m => !m.IsRead).ToList();

        bool isActive = ddlActive.SelectedValue.Equals("1");
        bool isNotActive = ddlActive.SelectedValue.Equals("0");

        if (isActive)
        {
            var filteredInvList = from inv in filteredByStatusList
                                  join jury in juryList
                                  on inv.JuryId equals jury.Id
                                  where jury.IsActive
                                  select inv;

            filteredByStatusList = filteredInvList.ToList();
        }
        if (isNotActive)
        {
            var filteredInvList = from inv in filteredByStatusList
                                  join jury in juryList
                                  on inv.JuryId equals jury.Id
                                  where !jury.IsActive
                                  select inv;

            filteredByStatusList = filteredInvList.ToList();
        }

        return filteredByStatusList;
    }

    public void GenerateEmails(Guid templateId)
    {
        string evetnYear = string.Empty;
        try
        {
            evetnYear = Gen_GeneralUseValueList.GetGen_GeneralUseValueList("EventCode")[0].Value;
        }
        catch { }

        lblError.Text = string.Empty;
        int counter = 0;
        foreach (GridDataItem item in radGridJury.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            HiddenField hdfJuryId = (HiddenField)item.FindControl("hdfJuryId");

            if (chkbox.Checked)
            {
                Jury jury = Jury.GetJury(new Guid(hdfJuryId.Value.ToString()));
                Email.SendTemplateEmail(jury, templateId);
                GeneralFunction.SaveEmailSentLog(jury, templateId, evetnYear);

                chkbox.Checked = false;
                counter++;
            }
        }

        if (counter == 0)
            lblError.Text = "Please select atleat one jury to send email.<br/>";
        else
        {
            lblError.Text = "Email sent to " + (counter).ToString() + " Jury(s).<br/>";
        }
    }

    public string GetSortExpression()
    {       
        if (ViewState["SortExpression"] == null)
            return string.Empty;
        else
            return ViewState["SortExpression"].ToString();        
    }

    public void SetSortExpression(string sortExp)
    {
        ViewState["SortExpression"] = sortExp;
    }

    public string GetSortOrder()
    {
        if (ViewState["SortOrder"] == null)
            return string.Empty;
        else
            return ViewState["SortOrder"].ToString();
    }

    public void SetSortOrder(string sortOrder)
    {
        ViewState["SortOrder"] = sortOrder;
    }

    public void EnableDisableJuryAccounts(bool enableAccount)
    {
        lblError.Text = string.Empty;
        int counter = 0;
        foreach (GridDataItem item in radGridJury.Items)
        {
            CheckBox chkbox = (CheckBox)item.FindControl("chkbox");
            HiddenField hdfJuryId = (HiddenField)item.FindControl("hdfJuryId");

            if (chkbox.Checked)
            {
                Jury jury = Jury.GetJury(new Guid(hdfJuryId.Value.ToString()));

                jury.IsActive = enableAccount;
                jury.DateModifiedString = DateTime.Now.ToString();
                jury.Save();

                chkbox.Checked = false;
                counter++;
            }
        }
        BindGrid(true, GetSortExpression(), GetSortOrder());
        if (counter == 0)
            lblError.Text = "Please select atleat one jury to " + (enableAccount ? "activate" : "deactivate") + " account.<br/>";
        else
        {
            lblError.Text = "" + (enableAccount ? "Activated " : "Deactivated ") + (counter).ToString() + " Jury(s).<br/>";
        }
    }

    public void HighLightYearTab(string selectedYear)
    {
        foreach (RepeaterItem item in rptEffieYears.Items)
        {
            LinkButton lnkYear = (LinkButton)item.FindControl("lnkYear");

            if (lnkYear.CommandArgument.ToString().Equals(selectedYear))
            {
                lnkYear.CssClass = "highlightTab";
            }
            else
                lnkYear.CssClass = "";
        }
    }

    public string GetHighLightedYearTab()
    {
        foreach (RepeaterItem item in rptEffieYears.Items)
        {
            LinkButton lnkYear = (LinkButton)item.FindControl("lnkYear");

            if (lnkYear.CssClass == "highlightTab")
                return lnkYear.CommandArgument.ToString();
        }
        return string.Empty;
    }

    public void ResetYearTabs()
    {
        foreach (RepeaterItem item in rptEffieYears.Items)
        {
            LinkButton lnkYear = (LinkButton)item.FindControl("lnkYear");

            lnkYear.CssClass = string.Empty;
        }
    }

    #endregion
    
}