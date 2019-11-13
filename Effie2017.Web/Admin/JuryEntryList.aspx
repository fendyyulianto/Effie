<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master" AutoEventWireup="true" CodeFile="JuryEntryList.aspx.cs" Inherits="Admin_JuryEntryList" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   <h2>Entries (by Judge)</h2>

        <span style="margin-left: 778px"><asp:Button ID="btnScores" runat="server" 
        Text="Scores (by Judge)" onclick="btnScores_Click" /></span>

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                <td style="padding-bottom:10px; width:200px">Judge Id:</td>
                <td style="padding-bottom:10px"><asp:Label ID="lbJuryId" runat="server" Font-Bold="true"/></td>
                </tr>
                <tr>
                <td style="padding-bottom:10px">Judge Name:</td>
                <td style="padding-bottom:10px"><asp:Label ID="lbJuryName" runat="server"/></td>
                </tr>
                <tr>
                <td style="padding-bottom:10px">Company:</td>
                <td style="padding-bottom:10px"><asp:Label ID="lbCompany" runat="server"/></td>
                </tr>
                <tr>
                <td style="padding-bottom:10px">Agency Network:</td>
                <td style="padding-bottom:10px"><asp:Label ID="lbNetwork" runat="server"/></td>
                </tr>
                <tr>
                <td style="padding-bottom:10px">Holding Company:</td>
                <td style="padding-bottom:10px"><asp:Label ID="lbHoldingCompany" runat="server"/></td>
                </tr>
                <tr>
                <td style="padding-bottom:10px">Judging Round:</td>
                <td style="padding-bottom:10px"><asp:Label ID="lbRound" runat="server"/></td>
                </tr>
                <tr>
                <td style="padding-bottom:10px">Jury Panel:</td>
                <td style="padding-bottom:10px"><asp:Label ID="lbPanel" runat="server"/>&nbsp;&nbsp;<asp:HyperLink ID="lnkJury" runat="server" Text="Edit Jury Panel" /></td>
                </tr>
                <tr>
                <td style="padding-bottom:10px">Scoring Completion:</td>
                <td style="padding-bottom:10px"><asp:Label ID="lbScoreCompletion" runat="server"/></td>
                </tr>
            </tbody>
        </table>

        <br />
    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>Advance Search/Filter</p>
        <table width="100%">
            <tr>
                <td>
                    <label>
                        <span>Search</span> :</label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox> on

                    <asp:DropDownList ID="ddlSearch" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="entryId" Text="Entry Id" />
                        <asp:ListItem Value="title" Text="Title" />
                        <asp:ListItem Value="entrant" Text="Entrant" />
                        <asp:ListItem Value="client" Text="Client" />
                    </asp:DropDownList>                    
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    <label>
                        <span>Category</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlCategory" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="width: 10%;">
                    <label>
                        <span>Panel</span> :</label>
                </td>
                <td style="width: 40%;">
                    <asp:DropDownList ID="ddlPanel" runat="server">
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
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
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
                <td>
                    <label>
                        <span></span> </label>
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

          <telerik:RadGrid ID="radGridEntry" runat="server"  Skin="Windows7" 
            AutoGenerateColumns="false" AllowPaging="true" PageSize="50" AllowSorting="true"
            OnItemDataBound="radGridEntry_ItemDataBound" OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
            <PagerStyle AlwaysVisible="true" />
            <MasterTableView>
            <Columns>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Serial" HeaderText="Entry Id" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Category" ItemStyle-Width="200px">
                    <ItemTemplate>
                        <asp:Label ID="lbCategory" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Campaign" HeaderText="Entry Title" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Entrant" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lnkBtnBuSubmittedBy" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Client" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbClient" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Country" ItemStyle-Width="100px">
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
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="70px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRecuse" runat="server" Text="Recuse" CssClass="tblLinkBlack" CommandName="Recuse"  Visible="false"></asp:LinkButton> <%--OnClientClick="return confirm('Confirm to recuse?');"--%>
                        <asp:LinkButton ID="LnkUnrecuse" runat="server" Text="Unrecuse" CssClass="tblLinkBlack" CommandName="Unrecuse"  Visible="false"></asp:LinkButton> <%--OnClientClick="return confirm('Confirm to unrecuse?');"--%>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns></MasterTableView>
        </telerik:RadGrid>

        <br />

        <asp:Button ID="btnExport" runat="server" Text="Export" onclick="btnExport_Click" />&nbsp;

        <br />
        <div class="errorDiv"><asp:Label ID="lblError" runat="server"></asp:Label></div>




</asp:Content>

