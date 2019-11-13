<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master" AutoEventWireup="true" CodeFile="JuryAllScoreList.aspx.cs" Inherits="Admin_JuryAllScoreList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<h2>Results (Round <asp:Literal ID="ltRound" runat="server" />)</h2>



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
                         <asp:ListItem Value="brand" Text="Brand" />
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

          <telerik:RadGrid ID="radGridEntry" runat="server"  Skin="Windows7"  
            AutoGenerateColumns="false" AllowPaging="true" PageSize="50" AllowSorting="true"
            OnItemDataBound="radGridEntry_ItemDataBound" OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
            <PagerStyle AlwaysVisible="true" />
            <MasterTableView AllowCustomSorting="true">
            <Columns>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
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
                <telerik:GridTemplateColumn HeaderText="Panel" ItemStyle-Width="50px" SortExpression="Panel">
                    <ItemTemplate>
                        <asp:Label ID="lbPanel" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>


                <%--Composite score--%>
                <telerik:GridTemplateColumn HeaderText="J1" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ1" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J2" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ2" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J3" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ3" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J4" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ4" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J5" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ5" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J6" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ6" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J7" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ7" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J8" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ8" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J9" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ9" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J10" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ10" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J11" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ11" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J12" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ12" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>                
                <telerik:GridTemplateColumn HeaderText="J13" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ13" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J14" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ14" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J15" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ15" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="J16" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center"  UniqueName="J16">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ16" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText="J17" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" UniqueName="J17">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ17" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText="J18" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" UniqueName="J18">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ18" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText="J19" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" UniqueName="J19">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ19" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText="J20" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" UniqueName="J20">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbScoreJ20" runat="server" Text="" CommandName="score" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>



                <%--Total Score--%>
                <telerik:GridTemplateColumn HeaderText="Total" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbScoretotal" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="No. of Scores" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbScoreCount" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Avg. Comp. Score" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbAvgScore" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="rank" HeaderText="Ranking" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lbRank" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>



                <%--Advancement--%>
                <telerik:GridTemplateColumn UniqueName="advtotal" HeaderText="Adv" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbAdv" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="advprecent" HeaderText="Adv%" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbAdvPercent" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>




                <%--High Low Scores--%>
                <telerik:GridTemplateColumn HeaderText="Total" ItemStyle-Width="100px" HeaderStyle-BackColor="#CCCCCC" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbHSScoretotal" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="No. of Scores" ItemStyle-Width="50px" HeaderStyle-BackColor="#CCCCCC" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbHSScoreCount" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Avg. Comp. Score" ItemStyle-Width="50px" HeaderStyle-BackColor="#CCCCCC" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbHSAvgScore" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="hsrank"  HeaderText="Ranking" ItemStyle-Width="50px" HeaderStyle-BackColor="#CCCCCC" ItemStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lbHSRank" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>





            </Columns></MasterTableView>
        </telerik:RadGrid>
         <br />
         <asp:Button ID="btnExportSummary" runat="server" Text="Export Summary" onclick="btnExportSummary_Click" />&nbsp;
         <asp:Button ID="btnExportSummaryComments" runat="server" Text="Export Summary Comments" onclick="btnExportSummaryComments_Click" />&nbsp;
         <asp:Button ID="btnExportAllSummary" runat="server" Text="Export Interim Summary" onclick="btnExportAllSummary_Click" />&nbsp;
         <asp:Button ID="btnExportMaster" runat="server" Text="Export Master" onclick="btnExportMaster_Click" />&nbsp;
         <asp:Button ID="btnExportRecuse" runat="server" Text="Export Recuse" onclick="btnExportRecuse_Click" />&nbsp;

        <div class="errorDiv"><asp:Label ID="lblError" runat="server"></asp:Label></div>




</asp:Content>

