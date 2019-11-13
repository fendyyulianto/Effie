<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master" 
    AutoEventWireup="true" CodeFile="JuryList.aspx.cs" Inherits="Admin_JuryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                    overlay: { closeClick: false} // prevents closing when clicking OUTSIDE fancybox
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

        function CheckAll(id) {
            var masterTable = $find("<%= radGridEntry.ClientID %>").get_masterTableView();
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

            var masterTable = $find("<%= radGridEntry.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == false) {
                var hidden = document.getElementById("hdfCheck");
                var checkBox = document.getElementById(clientId);
                checkBox.checked = false;
            }
        }
    </script>
    <style>
        #popupReminderEmail
        {
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2>
        Jury Listing (Round
        <asp:Literal ID="ltRound" runat="server" />)</h2>
    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>
            Advance Search/Filter</p>
        <table width="100%">
            <tr>
                <td>
                    <label>
                        <span>Search</span> :</label>
                </td>
                <td colspan="3">
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
            </tr>
            <tr>
                <td style="width: 20%;">
                    <label>
                        <span>Network</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlNetwork" runat="server">
                        <asp:ListItem Text="All" Value="" />
                    </asp:DropDownList>
                </td>
                <td style="width: 20%;">
                    <label>
                        <span>Jury Round 1</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlJuryPanel1" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Holding Company</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHoldingCompany" runat="server">
                        <asp:ListItem Text="All" Value="" />
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Jury Round 2</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlJuryPanel2" runat="server">
                        <asp:ListItem Text="All" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label> <span>Country</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Round Assigned</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAssignedRound" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="1" Text="1" />
                        <asp:ListItem Value="2" Text="2" />
                    </asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td>
                    <label> <span>Year</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server">
                    </asp:DropDownList>
                </td>
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
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <%--    <telerik:RadTabStrip runat="server" ID="rtabEntry" CssClass="tabledata" MultiPageID="RadMultiPage1" SelectedIndex="0"
        OnTabClick="rtabEntry_TabClick">
        <Tabs>
            <telerik:RadTab Text="All Entries" Width="150px" Value="">
            </telerik:RadTab>
            <telerik:RadTab Text="Pending Payment" Width="150px" Value="PPN">
            </telerik:RadTab>
            <telerik:RadTab Text="Pending Uploads" Width="150px" Value="UPN">
            </telerik:RadTab>
            <telerik:RadTab Text="Pending Completion" Width="150px" Value="UPC">
            </telerik:RadTab>
            <telerik:RadTab Text="Closed" Width="150px" Value="OK">
            </telerik:RadTab>
            <telerik:RadTab Text="Withdrawn / DQ" Width="150px" Value="WDN">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>--%>
    <div style="padding-left: 91%; padding-bottom: 10px;">
        <asp:Button ID="btnNew" runat="server" Text="Add Judge" OnClick="btnNew_Click" />
    </div>
    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="Windows7" AutoGenerateColumns="false"
        AllowPaging="true" PageSize="50" AllowSorting="true" OnItemDataBound="radGridEntry_ItemDataBound"
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView TableLayout="Auto">
            <Columns>
             <telerik:GridTemplateColumn>
                    <HeaderTemplate>
                        <center>
                            All<br />
                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" /></center>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <center>
                            <asp:CheckBox runat="server" ID="chkbox" /></center>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge Id" ItemStyle-Width="70px" SortExpression="SerialNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnJuryId" runat="server" CommandName="ViewJury"></asp:LinkButton><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge Name" ItemStyle-Width="100px" SortExpression="Firstname">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkJuryName" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--<telerik:GridBoundColumn DataField="Email" HeaderText="Email" ItemStyle-Width="70px"></telerik:GridBoundColumn>--%>
                <telerik:GridBoundColumn DataField="Company" HeaderText="Company" ItemStyle-Width="80px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Designation" HeaderText="Title" ItemStyle-Width="70px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Network" HeaderText="Network" ItemStyle-Width="100px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="HoldingCompany" HeaderText="Holding Company" ItemStyle-Width="80px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Country" HeaderText="Country" ItemStyle-Width="80px">
                </telerik:GridBoundColumn>
                <%--                <telerik:GridTemplateColumn HeaderText="Round1" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label ID="lbRound1" runat="server" Text="" />                    
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <telerik:GridTemplateColumn HeaderText="R1 Jury Panel" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center"
                    SortExpression="Round1PanelId">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryPanelRound1" runat="server" Text="-" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--                <telerik:GridTemplateColumn HeaderText="Round2" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:Label ID="lbRound2" runat="server" Text="" />                    
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <telerik:GridTemplateColumn HeaderText="R2 Jury Panel" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center"
                    SortExpression="Round2PanelId">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryPanelRound2" runat="server" Text="-" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Round1 Categories" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbCategory" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Round2 Categories" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbCategory2" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Scoring Completion" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbScoringCompletion" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText="Scoring Pending" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbScoringPending" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="UNActions" HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="100px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit Profile" CssClass="tblLinkBlack"  CommandName="Edit"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAssign" runat="server" Text="<br>Assign Panel" CssClass="tblLinkBlack" CommandName="Assign"></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtnView" runat="server" Text="<br>View/Recuse" CssClass="tblLinkBlack" CommandName="View"></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtnViewScores" runat="server" Text="<br>View Scores" CssClass="tblLinkBlack" CommandName="ViewScores"></asp:LinkButton>   
                        <asp:HyperLink ID="hlkEmailHistory" runat="server" Text="<br>Email History" CssClass="tblLinkBlack" Target="_blank"></asp:HyperLink>                  
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br /> 
    <span style="color:red;font-weight:bold;font-style:italic">WARNING: Clicking the Edit Panel will over-write previous assignment and result in Results data being purged!   If you like to assign additional jury panel, please use the action button on the right column and assign them individually.<br /></span>
    <asp:Button ID="btnEditPanel" runat="server" Text="Edit Panel" OnClick="btnEditPanel_Click" />&nbsp;<br /><br />
    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />&nbsp;
    <asp:Button ID="btnRemind" runat="server" Text="Send Reminder Email" 
                onclick="btnRemind_Click" />

    
    <asp:Label ID="lbLastReminderDate" runat="server" />
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>

    
    <asp:PlaceHolder ID="phSelectTemplate" runat="server" Visible="false">
        <div id="popupReminderEmail">
            <h2>
                <asp:Label runat="server" ID="lblTitle"></asp:Label></h2>
            <hr />
            <br />
            <table width="100%">
                <tr runat="server" id="roundRow">
                    <td width="30%">
                        Select Rounds :
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRounds">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr runat="server" id="templateRow">
                    <td width="30%">
                        Select Template :
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
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <%--<asp:Button ID="btnSend" runat="server" Text="Send"  OnClientClick="return triggerSend('param1');" ClientIDMode="Static"  />--%>
                        <input type="submit" id="btnSend" value="Send" onclick="return triggerSend('inviteEmail');return false;" />
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
