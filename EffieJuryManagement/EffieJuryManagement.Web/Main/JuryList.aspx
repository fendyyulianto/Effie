<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JuryList.aspx.cs" Inherits="Main_JuryList"
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

        function AlertJuryActive(judgeId,judgeGuid) {           
            if (window.confirm('Jury account (' + judgeId + ') is active. Are you sure want to delete ?') == false) {
                return false;
            } else {
                var radGridJuryMasterView = $find("<%= radGridJury.ClientID %>").get_masterTableView();
                radGridJuryMasterView.fireCommand("DelateConfirm", judgeGuid);  
            }
        }
    </script>
    <style>
        #popup
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
        .overlay
        {
            background-color: #000;
            position: fixed;
            top: 0;
            right: 0;
            min-height: 935px;
            min-width: 1800px;
            opacity: 0.5;
            filter: alpha(opacity=50);
        }
        a.tooltip span
        {
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <!-- InstanceBeginEditable name="content" -->
    <h2>
        Master List</h2>
    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>
            Advance Search/Filter</p>
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
                        <span>Network</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNetwork" runat="server">
                        <asp:ListItem Text="All" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Country</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Holding Company</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHoldingCompany" runat="server">
                        <asp:ListItem Text="All" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDelete" runat="server">
                        <asp:ListItem Text="All" Value="" />
                        <asp:ListItem Text="Cur" Value="False" />
                        <asp:ListItem Text="Del" Value="True" />
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
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
     <hr />
    <br />
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>  
    <br /> 
    <telerik:RadTabStrip runat="server" ID="rtabEntry" CssClass="tabledata" MultiPageID="RadMultiPage1"
        SelectedIndex="1" OnTabClick="rtabEntry_TabClick">
        <Tabs>
            <telerik:RadTab Text="All" Width="150px" Value="All">
            </telerik:RadTab>
            <telerik:RadTab Text="Current" Width="150px" Value="Current">
            </telerik:RadTab>
            <telerik:RadTab Text="Deleted" Width="150px" Value="Deleted">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <br />
    <telerik:RadGrid ID="radGridJury" runat="server" Skin="Windows7" AutoGenerateColumns="false"
        PageSize="50" AllowPaging="true" AllowSorting="true" OnItemDataBound="radGridJury_ItemDataBound"
        OnNeedDataSource="radGridJury_NeedDataSource" OnItemCommand="radGridJury_ItemCommand">
        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
        <MasterTableView>
            <Columns>
                <telerik:GridTemplateColumn>
                    <HeaderTemplate>
                        <center>
                            All<br>
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
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Type" HeaderText="Type" ItemStyle-Width="70px">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Judge Id" ItemStyle-Width="70px" SortExpression="SerialNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnJuryId" runat="server" CommandName="ViewJury"></asp:LinkButton><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge Name" ItemStyle-Width="80px" SortExpression="Firstname">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkJuryName" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Email" HeaderText="Email" ItemStyle-Width="70px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Company" HeaderText="Company" ItemStyle-Width="100px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Designation" HeaderText="Title" ItemStyle-Width="70px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Network" HeaderText="Network" ItemStyle-Width="100px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="HoldingCompany" HeaderText="Holding Company"
                    ItemStyle-Width="100px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Country" HeaderText="Country" ItemStyle-Width="80px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" HeaderText="Status" ItemStyle-Width="80px" AllowSorting="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="100px">
                    <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit Profile" CssClass="tblLinkBlack"
                            CommandName="Edit"></asp:LinkButton><br />
                            <asp:HyperLink ID="hlkEmailHistory" runat="server" Text="Email History" CssClass="tblLinkBlack"
                            Target="_blank"></asp:HyperLink><br />
                             <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CssClass="tblLinkBlack" OnClientClick="return confirm('Are you sure want to delete ?');"  Visible="false"
                            CommandName="delete"></asp:LinkButton>
                             <asp:LinkButton ID="lnkRestore" runat="server" Text="Restore" CssClass="tblLinkBlack" OnClientClick="return confirm('Are you sure want to restore ?');" Visible="false"
                            CommandName="restore"></asp:LinkButton>
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
                <input name="newJury" type="submit" id="newJury" runat="server" value="Add Jury" onclick="window.location.href = './Jury.aspx'; return false;" />
                <asp:Button ID="btnSendEmail" runat="server" Text="Send Email Invitation" CommandName="send" OnClick="btnSendEmail_Click" />&nbsp;
                <asp:Button ID="btnAddRound" runat="server" Text="Add Invitation" CommandName="add" OnClick="btnAddRound_Click" />&nbsp;

                <%-- <asp:Button ID="btnRound1and2Local" runat="server" Text="Send Invitation (R1 & R2) - Local"
                    OnClick="btnRound1and2Local_Click" OnClientClick="return confirm('Are you sure want to send Round 1 & 2 Invitation - Local ?');" />&nbsp;
                <asp:Button ID="btnRound1and2Overseas" runat="server" Text="Send Invitation (R1 & R2) - Overseas"
                    OnClick="btnRound1and2Overseas_Click" OnClientClick="return confirm('Are you sure want to send Round 1 & 2 Invitation - Overseas ?');" />--%>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />
            </td>
            <td align="right">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
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
