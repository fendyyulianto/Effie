<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master"
    AutoEventWireup="true" CodeFile="JuryScoreList.aspx.cs" Inherits="Admin_JuryScoreList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2>
        Scores (by Judge)</h2>
    <span style="margin-left: 778px">
        <asp:Button ID="btnEntryList" runat="server" Text="Entries (by Judge)" OnClick="btnEntryList_Click" /></span>
    <div style="float: left">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 200px">
                        Judge Id:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbJuryId" runat="server" Font-Bold="true" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Judge Name:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbJuryName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Company:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbCompany" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Agency Network:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbNetwork" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Holding Company:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbHoldingCompany" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Judging Round:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbRound" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Jury Panel:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbPanel" runat="server" />&nbsp;&nbsp;<asp:HyperLink ID="lnkJury"
                            runat="server" Text="Edit Jury Panel" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Scoring Completion:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbScoreCompletion" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Scoring Pending:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbScorePending" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="margin-left: 430px;">
        <table width="50%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 50px; text-decoration: underline;">
                        Legend
                    </td>
                    <td style="padding-bottom: 10px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 50px">
                        SC
                    </td>
                    <td style="padding-bottom: 10px">
                        Strategic Challenge (23.33%)
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        ID
                    </td>
                    <td style="padding-bottom: 10px">
                        Idea (23.33%)
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        IL
                    </td>
                    <td style="padding-bottom: 10px">
                        Bringing Idea to Life (23.33%)
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        RE
                    </td>
                    <td style="padding-bottom: 10px">
                        Results (30%)
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="clear: both;">
    </div>
    <br />
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
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Category</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Scoring Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlScoreStatus" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="0" Text="Pending" />
                        <asp:ListItem Value="1" Text="Completed" />
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
                        <span>Recused</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRecuse" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="1" Text="Yes" />
                        <asp:ListItem Value="0" Text="No" />
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
                <td style="width: 10%;">
                </td>
                <td style="width: 40%;">
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
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand"
        Width="1350px">
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
                <telerik:GridTemplateColumn HeaderText="Entry Id" ItemStyle-Width="100px" SortExpression="Serial">
                    <ItemTemplate>
                        <asp:Label ID="lbSerial" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Category" ItemStyle-Width="100px" SortExpression="CategoryMarket">
                    <ItemTemplate>
                        <asp:Label ID="lbCategory" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Entry Title" ItemStyle-Width="100px" SortExpression="Campaign">
                    <ItemTemplate>
                        <asp:Label ID="lbCampaign" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
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
                <telerik:GridTemplateColumn HeaderText="Brand" ItemStyle-Width="100px" SortExpression="Brand">
                    <ItemTemplate>
                        <asp:Label ID="lbBrand" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Country" ItemStyle-Width="100px" SortExpression="Country">
                    <ItemTemplate>
                        <asp:Label ID="lbCountry" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--Raw score--%>
                <telerik:GridTemplateColumn HeaderText="SC" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#BBBBBB" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreSCRaw" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="ID" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#BBBBBB" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreIDRaw" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="IL" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#BBBBBB" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreILRaw" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="RE" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#BBBBBB" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreRERaw" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--Weighted score--%>
                <telerik:GridTemplateColumn HeaderText="SC(C)" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#DDDDDD" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreSC" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="ID(C)" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#DDDDDD" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreID" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="IL(C)" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#DDDDDD" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreIL" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="RE(C)" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#DDDDDD" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreRE" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Total Comp. Score" ItemStyle-Width="50px"
                    ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle BackColor="#DDDDDD" />
                    <ItemTemplate>
                        <asp:Label ID="lbScoreComposite" runat="server" Text="NS" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Scoring Status" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="ScoreStatus">
                    <ItemTemplate>
                        <asp:Label ID="lbScoreStatus" runat="server" Text="Pending" ForeColor="Gray" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jury Flag" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="JuryFlag">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryFlag" runat="server" Text="-" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jury Recuse Flag" ItemStyle-Width="50px" 
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryRecuse" runat="server" Text="-" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Admin Recuse Flag" ItemStyle-Width="50px"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbRecuse" runat="server" Text="-" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Advance" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:Label ID="lbAdvancement" runat="server" Text="-" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="200px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkScore" runat="server" Text="View Score" CssClass="tblLinkBlack"
                            CommandName="Score" Visible="false"></asp:LinkButton><br />
                        <asp:LinkButton ID="lnkReset" runat="server" Text="Reset Scoring Status" CssClass="tblLinkBlack"
                            CommandName="Reset" Visible="false" OnClientClick="return confirm('Confirm to reset this score to Pending status?');"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />&nbsp;
    <br />
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
</asp:Content>
