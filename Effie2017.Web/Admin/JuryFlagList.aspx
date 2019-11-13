<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master" AutoEventWireup="true" CodeFile="JuryFlagList.aspx.cs" Inherits="Admin_JuryFlagList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

   <h2>Flag Report (Round <asp:Literal ID="ltRound" runat="server" />)</h2>



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
                        <asp:ListItem Value="juryId" Text="Jury Id" />
                        <asp:ListItem Value="juryname" Text="Jury Name" />
                        <asp:ListItem Value="jurycompany" Text="Jury Company" />
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
                                <asp:ListItem Value="Not Applicable">Not Applicable</asp:ListItem>
                                <asp:ListItem Value="Aegis Media">Aegis Media</asp:ListItem>
                                <asp:ListItem Value="BBDO Worldwide">BBDO Worldwide</asp:ListItem>
                                <asp:ListItem Value="DDB Worldwide">DDB Worldwide</asp:ListItem>
                                <asp:ListItem Value="Dentsu">Dentsu</asp:ListItem>
                                <asp:ListItem Value="Digitas">Digitas</asp:ListItem>
                                <asp:ListItem Value="DraftFCB">DraftFCB</asp:ListItem>
                                <asp:ListItem Value="Euro RSCG Worldwide">Euro RSCG Worldwide</asp:ListItem>
                                <asp:ListItem Value="Grey Group">Grey Group</asp:ListItem>
                                <asp:ListItem Value="Group M">Group M</asp:ListItem>
                                <asp:ListItem Value="JWT">JWT</asp:ListItem>
                                <asp:ListItem Value="Leo Burnett Worldwide">Leo Burnett Worldwide</asp:ListItem>
                                <asp:ListItem Value="McCann Worldgroup">McCann Worldgroup</asp:ListItem>
                                <asp:ListItem Value="MediaBrands">MediaBrands</asp:ListItem>
                                <asp:ListItem Value="MediaCom">MediaCom</asp:ListItem>
                                <asp:ListItem Value="Mindshare Worldwide">Mindshare Worldwide</asp:ListItem>
                                <asp:ListItem Value="MPG">MPG</asp:ListItem>
                                <asp:ListItem Value="MS & L Group">MS&L Group</asp:ListItem>
                                <asp:ListItem Value="Mullen Lowe Group">Mullen Lowe Group</asp:ListItem>
                                <asp:ListItem Value="Ogilvy & Mather">Ogilvy & Mather</asp:ListItem>
                                <asp:ListItem Value="Omnicom Media Group">Omnicom Media Group</asp:ListItem>
                                <asp:ListItem Value="Publicis">Publicis</asp:ListItem>
                                <asp:ListItem Value="Saatchi & Saatchi">Saatchi & Saatchi</asp:ListItem>
                                <asp:ListItem Value="Starcom MediaVest Group">Starcom MediaVest Group</asp:ListItem>
                                <asp:ListItem Value="TBWA\Worldwide">TBWA\Worldwide</asp:ListItem>
                                <asp:ListItem Value="United Group">United Group</asp:ListItem>
                                <asp:ListItem Value="WPP Digital">WPP Digital</asp:ListItem>
                                <asp:ListItem Value="Young & Rubicam">Young & Rubicam</asp:ListItem>
                                <asp:ListItem Value="ZenithOptimedia">ZenithOptimedia</asp:ListItem>
                                <asp:ListItem Value="Independent">Independent</asp:ListItem>
                                <asp:ListItem Value="Others">Others</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 20%;">
                    <label>
                        <span>Jury Flag</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlJuryFlag" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="DQ" Text="DQ" />
                        <asp:ListItem Value="Wrong category" Text="Wrong category" />
                        <asp:ListItem Value="JuryRecusal" Text="Jury Recusal" />
                        
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
                                <asp:ListItem Value="Not Applicable">Not Applicable</asp:ListItem>
                                <asp:ListItem Value="Aegis Group">Aegis Group</asp:ListItem>
                                <asp:ListItem Value="Dentsu">Dentsu</asp:ListItem>
                                <asp:ListItem Value="Hakuhodo">Hakuhodo</asp:ListItem>
                                <asp:ListItem Value="Havas Advertising">Havas Advertising</asp:ListItem>
                                <asp:ListItem Value="Interpublic (IPG)">Interpublic (IPG)</asp:ListItem>
                                <asp:ListItem Value="MDC Partners">MDC Partners</asp:ListItem>
                                <asp:ListItem Value="Omnicom">Omnicom</asp:ListItem>
                                <asp:ListItem Value="Publicis Groupe">Publicis Groupe</asp:ListItem>
                                <asp:ListItem Value="WPP Group">WPP Group</asp:ListItem>
                                <asp:ListItem Value="Independent">Independent</asp:ListItem>
                                <asp:ListItem Value="Others">Others</asp:ListItem>
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
                <!--
                <td>
                    <label>
                        <span>Recuse Flag</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlRecuse" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="1" Text="Yes" />
                        <asp:ListItem Value="0" Text="No" />
                    </asp:DropDownList>
                </td>
                -->
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
                <telerik:GridTemplateColumn HeaderText="Category" ItemStyle-Width="200px" SortExpression="CategoryMarket">
                    <ItemTemplate>
                        <asp:Label ID="lbCategory" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Entry Title" ItemStyle-Width="200px" SortExpression="Campaign">
                    <ItemTemplate>
                        <asp:Label ID="lbTitle" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn HeaderText="Client" ItemStyle-Width="200px" SortExpression="Client">
                    <ItemTemplate>
                        <asp:Label ID="lbClient" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Entrant" ItemStyle-Width="100px" SortExpression="Entrant">
                    <ItemTemplate>
                        <asp:Label ID="lnkBtnBuSubmittedBy" runat="server" Text="" />
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


                <telerik:GridTemplateColumn HeaderText="JuryId" ItemStyle-Width="100px" SortExpression="JuryID">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryId" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jury Name" ItemStyle-Width="100px" SortExpression="JuryName">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryName" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Title" ItemStyle-Width="100px" SortExpression="JuryTitle">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryTitle" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Company" ItemStyle-Width="100px" SortExpression="JuryCompany">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryCompany" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
<%--                <telerik:GridTemplateColumn HeaderText="Network" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryNetwork" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Holding Company" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryHoldingCompany" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <telerik:GridTemplateColumn HeaderText="Country" ItemStyle-Width="100px" SortExpression="JuryCountry">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryCountry" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>


                <telerik:GridTemplateColumn HeaderText="Jury Flag" ItemStyle-Width="100px" SortExpression="JuryFlag">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryFlag" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Flag Reason" ItemStyle-Width="100px" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryFlagReason" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
<%--                <telerik:GridTemplateColumn HeaderText="Recuse Flag" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryRecuseFlag" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <telerik:GridTemplateColumn HeaderText="Recuse Reason" ItemStyle-Width="100px" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lbJuryRecuseReason" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                
                <telerik:GridTemplateColumn HeaderText="Date" ItemStyle-Width="100px" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lbDateSubmitted" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkScore" runat="server" Text="View Score" CssClass="tblLinkBlack" CommandName="Score" Visible="false"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns></MasterTableView>
        </telerik:RadGrid>

              <br />
        <asp:Button ID="btnExport" runat="server" Text="Export" onclick="btnExport_Click" />&nbsp;

        <br />
        <div class="errorDiv"><asp:Label ID="lblError" runat="server"></asp:Label></div>




</asp:Content>

