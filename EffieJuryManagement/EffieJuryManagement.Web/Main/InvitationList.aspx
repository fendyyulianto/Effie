<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvitationList.aspx.cs" Inherits="Main_InvitationList"
    MasterPageFile="~/Common/MasterPage.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- Add mousewheel plugin (this is optional) -->
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/lib/jquery.mousewheel-3.0.6.pack.js"></script>
    <!-- Add fancyBox main JS and CSS files -->
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/jquery.fancybox.js?v=2.1.5"></script>
    <link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/jquery.fancybox.css?v=2.1.5"
        media="screen" />
    <!-- Add Button helper (this is optional) -->
    <link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-buttons.css?v=1.0.5" />
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-buttons.js?v=1.0.5"></script>
    <!-- Add Thumbnail helper (this is optional) -->
    <link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-thumbs.css?v=1.0.7" />
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-thumbs.js?v=1.0.7"></script>
    <!-- Add Media helper (this is optional) -->
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-media.js?v=1.0.6"></script>
    <script type="text/javascript">
        function CheckAll(id) {
            var masterTable = $find("<%= radGridJury.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    var checkBox = masterTable.get_dataItems()[i].findElement("chkbox");
                    if (checkBox != null)
                        checkBox.checked = true;  // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    var checkBox = masterTable.get_dataItems()[i].findElement("chkbox");
                    if (checkBox != null)
                        checkBox.checked = false; // for unchecking the checkboxes
                }
            }
        }
        function unCheckHeader(id, clientId) {
            var masterTable = $find("<%= radGridJury.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == false) {
                var hidden = document.getElementById("hdfCheck");
                var checkBox = document.getElementById(clientId);
                checkBox.checked = false;
            }
        }

        $(document).ready(function () {
            $('.fancybox').fancybox({
                beforeClose: function () {
                    $('body').fadeOut({ duration: 700, complete: function () { location.reload(); } });
                    return;
                },
                autoSize: false,
                height: 700,
                width: 800,
                closeClick: false, // prevents closing when clicking INSIDE fancybox
                helpers: {
                    overlay: { closeClick: false } // prevents closing when clicking OUTSIDE fancybox
                }
            });


        });

        function CloseFancyBox() {
            $("a[title='Close']").click();
        }

        function triggerSend(params) {
            if (window.confirm('Are you sure to continue ?') == false) {
                return false;
            } else {
                __doPostBack('btnSend', params);
            }
        }

        function ClearDataOnLoad() {

            theForm.__EVENTTARGET.value = "";
            theForm.__EVENTARGUMENT.value = "";
            theForm.submit();
        }
    </script>
    <style>
        #popup {
            opacity: 1;
            width: 500px;
            height: 190px;
            padding: 25px;
            margin-left: 115px;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            margin: auto;
            position: fixed;
            border: 6px solid #bbbbbb;
            background-color: #fff;
            z-index: 1002;
            overflow-y: scroll;
        }

        .overlay {
            background-color: #000;
            position: fixed;
            top: 0;
            right: 0;
            min-height: 935px;
            min-width: 1800px;
            opacity: 0.5;
            filter: alpha(opacity=50);
        }

        a.tooltip span {
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <!-- InstanceBeginEditable name="content" -->
    <h2>Invitation List</h2>
    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>
            Advance Search/Filter
        </p>
        <table width="100%">
            <tr>
                <td>
                    <label>
                        <span>Search</span> :</label>
                </td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                    on
                    <asp:DropDownList ID="ddlSearch" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="judgeId" Text="Judge Id" />
                        <asp:ListItem Value="name" Text="Name" />
                        <asp:ListItem Value="company" Text="Company" />
                        <asp:ListItem Value="title" Text="Title" />
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Invitation Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlInvitation" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="R1" Text="Round 1" />
                        <asp:ListItem Value="R2" Text="Round 2" />
                        <asp:ListItem Value="NOT" Text="Decline" />
                        <asp:ListItem Value="NORes" Text="No Response" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Type</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="Agency" Text="Agency" />
                        <asp:ListItem Value="Client" Text="Client" />
                        <asp:ListItem Value="Media Company" Text="Media Company" />
                        <asp:ListItem Value="Others" Text="Others" />
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Read Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRead" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="1" Text="Yes" />
                        <asp:ListItem Value="0" Text="No" />
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td>
                    <label>
                        <span>Specialist Type </span>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSpecialistType" runat="server" Style="width: 234px">
                            <asp:ListItem Value="" Text="All" />
                            <asp:ListItem Value="Creative" Text="Creative" />
                            <asp:ListItem Value="Strategist/Planner" Text="Strategist/Planner" />
                            <asp:ListItem Value="Suit" Text="Suit" />
                            <asp:ListItem Value="Digital Specialist" Text="Digital Specialist" />
                            <asp:ListItem Value="Data/Technology Specialist" Text="Data/Technology Specialist" />
                            <asp:ListItem Value="Others" Text="Others" />
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Accepted Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAccepted" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="R1" Text="Round 1" />
                        <asp:ListItem Value="R2" Text="Round 2" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Network </span>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNetwork" runat="server">
                            <asp:ListItem Text="All" Value="" />
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Shortlisted Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlShortlisted" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="R1" Text="Round 1" />
                        <asp:ListItem Value="R2" Text="Round 2" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Holding Company </span>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHoldingCompany" Style="width: 234px" runat="server" >
                            <asp:ListItem Text="All" Value="" />
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Assigned Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAssigned" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="R1" Text="Round 1" />
                        <asp:ListItem Value="R2" Text="Round 2" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Country </span>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server">
                            <asp:ListItem Text="All" Value="" />
                    </asp:DropDownList>
                </td>

                <td>
                    <label>
                        <span>Account Activated Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlActive" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="1" Text="Yes" />
                        <asp:ListItem Value="0" Text="No" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="display:none;">
                <td>
                    <label>
                        <span>Last Profile Update </span>:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlLastProfileUpdate" runat="server">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td></td>
            </tr>
            
            <tr>
                <td colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <div style="clear: both">
    </div>
    <hr />
    <div class="yearColumn">
        <span>Filter by Effie Year : &nbsp;&nbsp;</span>
        <asp:Repeater runat="server" ID="rptEffieYears" OnItemDataBound="rptEffieYears_ItemDataBound" OnItemCommand="rptEffieYears_ItemCommand">
            <HeaderTemplate>
                <ul class="yearTab">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:LinkButton runat="server" ID="lnkYear" CommandName="year"></asp:LinkButton></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div style="clear: both">
    </div>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    <telerik:RadGrid ID="radGridJury" runat="server" Skin="Windows7" AutoGenerateColumns="false"
        PageSize="50" AllowPaging="true" AllowSorting="true" OnItemDataBound="radGridJury_ItemDataBound"
        OnSortCommand="radGridJury_SortCommand" OnNeedDataSource="radGridJury_NeedDataSource"
        OnItemCommand="radGridJury_ItemCommand">
        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
        <MasterTableView TableLayout="Auto" AllowCustomSorting="true">
            <Columns>
                <telerik:GridTemplateColumn ItemStyle-Width="100px">
                    <HeaderTemplate>
                        <center>
                            Select All
                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" /></center>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <center>
                            <asp:CheckBox runat="server" ID="chkbox" /></center>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Id" Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdfId" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Id" Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdfJuryId" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge Id" ItemStyle-Width="65px" SortExpression="SerialNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnJuryId" runat="server" CommandName="ViewJury"></asp:LinkButton><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Type" ItemStyle-Width="60px" SortExpression="Type">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblType"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge Name" ItemStyle-Width="80px" SortExpression="Name">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkJuryName" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Company" ItemStyle-Width="100px" SortExpression="Company">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCompany"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Title" ItemStyle-Width="150px" SortExpression="Title">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTitle"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Country" ItemStyle-Width="150px" SortExpression="Country">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCountry"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Invite R1" SortExpression="IsRound1Invited"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkInvRound1" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Invite R2" SortExpression="IsRound2Invited"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkInvRound2" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Decline" ItemStyle-Width="30px" SortExpression="IsDeclined">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkDecline" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Accept R1" SortExpression="IsRound1Accepted"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkAccptRound1" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Accept R2" SortExpression="IsRound2Accepted"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkAccptRound2" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Shortlist R1" SortExpression="IsRound1Shortlisted"
                    Visible="false" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkShortListedRound1" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Shortlist R2" SortExpression="IsRound2Shortlisted"
                    Visible="false" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkShortListedRound2" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Assign R1" SortExpression="IsRound1Assigned"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkAssignRound1" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Assign R2" SortExpression="IsRound2Assigned"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkAssignRound2" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="R1 Email Sent Date" SortExpression="DateRound1EmailSent"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblRound1EmailSent"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="R2 Email Sent Date" SortExpression="DateRound2EmailSent"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblRound2EmailSent"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Read" SortExpression="IsRead" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkRead" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Account<br/>Activated" SortExpression="IsActive"
                    ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkActive" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                
                <telerik:GridTemplateColumn HeaderText="Jury<br/>Update" SortExpression="JuryUpdate" ItemStyle-Width="30px">
                    <ItemTemplate>
                         <asp:Label runat="server" ID="lblJuryUpdate"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>


                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="180px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit Status" CssClass="tblLinkBlack"
                            CommandName="Edit"></asp:LinkButton><br />
                        <asp:HyperLink ID="hlkEmailHistory" runat="server" Text="View Email History" CssClass="tblLinkBlack"
                            Target="_blank"></asp:HyperLink><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" OnClick="btnSendEmail_Click" />&nbsp; 
                <asp:Button ID="btnEnableJury" runat="server" Text="Activate" OnClick="btnEnableJury_Click"
                    OnClientClick="return confirm('Are you sure want to Activate all the selected Jury(s) ?');" />&nbsp;
                <asp:Button ID="btnDisableJury" runat="server" Text="Deactivate" OnClick="btnDisableJury_Click"
                    OnClientClick="return confirm('Are you sure want to Deactivate all the selected Jury(s) ?');" />&nbsp;
                <%-- <asp:Button ID="btnRound1and2Local" runat="server" Text="Send Invitation (R1 & R2) - Local"
                    OnClick="btnRound1and2Local_Click" OnClientClick="return confirm('Are you sure want to send Round 1 & 2 Invitation - Local ?');" />&nbsp;
                <asp:Button ID="btnRound1and2Overseas" runat="server" Text="Send Invitation (R1 & R2) - Overseas"
                    OnClick="btnRound1and2Overseas_Click" OnClientClick="return confirm('Are you sure want to send Round 1 & 2 Invitation - Overseas ?');" />--%>
            </td>
            <td align="right"></td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />&nbsp; 
                <asp:Button ID="btnSummaryReport" runat="server" Text="Summary Report" OnClick="btnbtnSummaryReport_Click" />
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phSelectTemplate" runat="server" Visible="false">
        <div id="popup">
            <h2>
                <asp:Label runat="server" ID="lblTitle"></asp:Label></h2>
            <hr />
            <br />
            <table width="100%">
                <tr>
                    <td width="30%">Select Template :
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTemplateList" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateList_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:HyperLink runat="server" ID="hlkPreview" Visible="false" Text="Preview"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" OnClientClick="return confirm('Are you sure to continue ?');" />--%>
                        <input type="submit" id="btnSend" value="Send" onclick="return triggerSend('genericEmail'); return false;" />
                        &nbsp;
                        <asp:Button runat="server" ID="btnClose" Text="Close" OnClick="btnClose_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblTempError" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="overlay">
        </div>
    </asp:PlaceHolder>
</asp:Content>
