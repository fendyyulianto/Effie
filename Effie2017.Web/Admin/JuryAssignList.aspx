<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master"
    AutoEventWireup="true" CodeFile="JuryAssignList.aspx.cs" Inherits="Admin_JuryAssignList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2>
        Jury Assignment
        <asp:Literal ID="ltRound" runat="server" /></h2>
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
                        <asp:ListItem Value="entrant" Text="Entrant" />
                        <asp:ListItem Value="client" Text="Client" />
                        <asp:ListItem Value="agency" Text="Agency" />
                        <asp:ListItem Value="juryname" Text="Jury Name" />
                        <%--<asp:ListItem Value="panel" Text="Panel" />--%>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    <label>
                        <span>Panel</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlPanel" runat="server">
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
                <td colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <br />
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
    <br />
    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="Windows7" AutoGenerateColumns="false"
        AllowPaging="true" PageSize="50" AllowSorting="true" OnItemDataBound="radGridEntry_ItemDataBound"
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView AllowCustomSorting="true">
            <Columns>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Serial" HeaderText="Entry Id" ItemStyle-Width="70px" SortExpression="Serial">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Category" ItemStyle-Width="200px" SortExpression="CategoryMarket">
                    <ItemTemplate>
                        <asp:Label ID="lbCategory" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Campaign" HeaderText="Entry Title" ItemStyle-Width="70px" SortExpression="Campaign">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Entrant" ItemStyle-Width="100px" SortExpression="Entrant">
                    <ItemTemplate>
                        <asp:Label ID="lnkBtnBuSubmittedBy" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Client" ItemStyle-Width="100px" SortExpression="Client">
                    <ItemTemplate>
                        <asp:Label ID="lbClient" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Country" ItemStyle-Width="100px" SortExpression="Country">
                    <ItemTemplate>
                        <asp:Label ID="lbCountry" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Agency1" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbAgency1" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Agency2" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbAgency2" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Agency3" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbAgency3" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Agency4" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbAgency4" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Agency5" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbAgency5" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jury Panel" ItemStyle-Width="100px" SortExpression="Panel">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryPanel" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--                <telerik:GridTemplateColumn HeaderText="Round2 Jury Panel" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryPanel2" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <telerik:GridTemplateColumn HeaderText="Judge1" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury1" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge2" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury2" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge3" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury3" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge4" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury4" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge5" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury5" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge6" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury6" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge7" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury7" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge8" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury8" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge9" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury9" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge10" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury10" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge11" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury11" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge12" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury12" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge13" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury13" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge14" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury14" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge15" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury15" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge16" ItemStyle-Width="100px" UniqueName="Judge16" >
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury16" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge17" ItemStyle-Width="100px" UniqueName="Judge17" >
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury17" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge18" ItemStyle-Width="100px" UniqueName="Judge18" >
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury18" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge19" ItemStyle-Width="100px" UniqueName="Judge19" >
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury19" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge20" ItemStyle-Width="100px" UniqueName="Judge20" >
                    <ItemTemplate>
                        <asp:HyperLink ID="lbJury20" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />&nbsp;
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
</asp:Content>
