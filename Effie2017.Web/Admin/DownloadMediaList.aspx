<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownloadMediaList.aspx.cs"
    Inherits="Admin_DownloadMediaList" MasterPageFile="~/Admin/MasterPageAdmin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">
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
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2>
        Download Media</h2>
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
                        <asp:ListItem Value="entryId" Text="Entry Id" />
                        <asp:ListItem Value="title" Text="Title" />
                        <asp:ListItem Value="client" Text="Client" />
                        <asp:ListItem Value="company" Text="Company" />
                        <asp:ListItem Value="firstname" Text="First Name" />
                        <asp:ListItem Value="lastname" Text="Last Name" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    <label>
                        <span>Entry Status</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlEntryStatus" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="width: 20%;">
                    <label>
                        <span>Market</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlMarket" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="SM" Text="Single Market" />
                        <asp:ListItem Value="MM" Text="Multi Market" />
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
                        <span>Category</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td>
                    <label>
                        <span>DL</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDeadline" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="D1" Text="D1" />
                        <asp:ListItem Value="D2" Text="D2" />
                        <asp:ListItem Value="D3" Text="D3" />
                        <asp:ListItem Value="D4" Text="D4" />
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
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
    <br />
    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="Windows7" AutoGenerateColumns="false" OnSortCommand="radGridEntry_SortCommand"
        AllowPaging="true" AllowSorting="true" OnItemDataBound="radGridEntry_ItemDataBound" PageSize="50"
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView AllowCustomSorting="true">
            <Columns>
                <telerik:GridTemplateColumn UniqueName="SelectALL"  HeaderText="" ItemStyle-Width="60px">
                 <HeaderTemplate>
                        <center>
                            All<br />
                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" /></center>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <center>
                        <asp:CheckBox ID="chkbox" runat="server" /></center>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="10px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Deadline" UniqueName="Deadline" HeaderText="DL" HeaderStyle-Width="80px" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Serial" HeaderText="Entry ID" ItemStyle-Width="70px" SortExpression="Serial">
                </telerik:GridBoundColumn>
                 <telerik:GridBoundColumn DataField="DateSubmitted" HeaderText="Date" HeaderStyle-Width="60" SortExpression="DateSubmitted" DataFormatString="{0:dd/MM/yy H:mm}">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Campaign" HeaderText="Title" HeaderStyle-Width="145" SortExpression="Campaign">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Client" HeaderText="Client" HeaderStyle-Width="85" SortExpression="Client">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CategoryMarket" HeaderText="Category" HeaderStyle-Width="90px" SortExpression="CategoryMarket">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" HeaderText="Entry Status" ItemStyle-Width="80px" SortExpression="Status">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="IsVideoDownloaded" HeaderText="Media Downloaded" SortExpression="IsVideoDownloaded"
                    ItemStyle-Width="80px">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Submitted By" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnBuSubmittedBy" runat="server" Text="View" CssClass="tblLinkBlack"
                            CommandName="User"></asp:LinkButton>
                        <a href="#" class="tooltip">
                            <img src="../images/icon-info.png" width="15" height="15" /><asp:Label ID="lblSubmittedDetails"
                                runat="server" /></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridBoundColumn DataField="Firstname" HeaderText="First Name" ItemStyle-Width="100px"
                    AllowSorting="true">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Lastname" HeaderText="Last Name" ItemStyle-Width="100px"
                    AllowSorting="true">
                </telerik:GridBoundColumn><telerik:GridBoundColumn DataField="Country" HeaderText="Country" ItemStyle-Width="100px" AllowSorting="true"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="270px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnView" runat="server" Text="View" CssClass="tblLinkBlack"
                            CommandName="View"></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkDownloadVideo" runat="server" Text="Download Video" CssClass="tblLinkBlack"
                            CommandName="video"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <asp:Button ID="btnDownload" runat="server" Text="Download (Zip)" OnClick="btnDownload_Click" />&nbsp;
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
</asp:Content>
