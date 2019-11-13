<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="GalaOrderList.aspx.cs" Inherits="Admin_GalaOrderList" %>

<%@ Register Src="~/Admin/Controls/PaymentGala.ascx" TagName="Payment" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        #popup
        {
            opacity: 1;
            width: 800px;
            height: 550px;
            padding: 25px;
            margin-left: 115px;
            top: 0;
            margin-top: 0px;
            position: fixed;
            border: 6px solid #bbbbbb;
            background-color: #fff;
            z-index: 100;
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
    <h2>
        Gala Orders</h2>
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
                        <asp:ListItem Value="invoice" Text="Invoice" />
                        <asp:ListItem Value="name" Text="Name" />
                        <asp:ListItem Value="email" Text="Email" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    <label>
                        <span>Payment Method</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlPaymentMethod" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="width: 20%;">
                    <label>
                        <span>Payment Status</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlPaymentStatus" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Shipping</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlShipping" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="collect_office" Text="Self Collection from Organiser's office" />
                        <asp:ListItem Value="collect_onsite" Text="Self Collection on-site" />
                        <asp:ListItem Value="courier" Text="Courier" />
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <!--
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
                </td>
                <td>
                </td>
            </tr>
            -->
            <tr>
                <td colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <br />
        <telerik:RadTabStrip runat="server" ID="rtabEntry" CssClass="tabledata" MultiPageID="RadMultiPage1" SelectedIndex="0"
        OnTabClick="rtabEntry_TabClick">
        <Tabs>
            <telerik:RadTab Text="All" Width="150px" Value="">
            </telerik:RadTab> 
            <telerik:RadTab Text="Completed" Width="150px" Value="OOK">
            </telerik:RadTab>            
            <telerik:RadTab Text="Draft" Width="150px" Value="PEN">
            </telerik:RadTab>          
        </Tabs>
    </telerik:RadTabStrip>
    <%--    <div style="padding-left: 826px; padding-bottom: 10px;">
    <asp:Button ID="btnNew" runat="server" Text="Add Judge" onclick="btnNew_Click" />
    </div>--%>
    <br />
    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="Windows7" AutoGenerateColumns="false"
        AllowPaging="true" PageSize="50" AllowSorting="true" OnItemDataBound="radGridEntry_ItemDataBound"
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView>
            <Columns>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Invoice" HeaderText="Invoice" ItemStyle-Width="50px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateCreated" HeaderText="Date" ItemStyle-Width="90px"
                    DataFormatString="{0:dd/MM/yy H:mm}">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PayFirstname" HeaderText="First name" ItemStyle-Width="100px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PayLastname" HeaderText="Last name" ItemStyle-Width="100px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PayEmail" HeaderText="Email" ItemStyle-Width="100px">
                </telerik:GridBoundColumn>
                <%--<telerik:GridBoundColumn DataField="PayCompany" HeaderText="Company" ItemStyle-Width="100px"></telerik:GridBoundColumn>--%>
                <%--<telerik:GridBoundColumn DataField="PayCountry" HeaderText="Country" ItemStyle-Width="100px"></telerik:GridBoundColumn>--%>
                <telerik:GridTemplateColumn HeaderText="Payment Method" ItemStyle-Width="70px" SortExpression="PaymentMethod">
                    <ItemTemplate>
                        <asp:Label ID="lbPaymentMethod" runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Payment Status" ItemStyle-Width="50px" SortExpression="PaymentStatus">
                    <ItemTemplate>
                        <asp:Label ID="lbPaymentStatus" runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="TableCount" HeaderText="Tables" ItemStyle-Width="30px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="SeatCount" HeaderText="Seats" ItemStyle-Width="30px">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Shipping" ItemStyle-Width="70px" SortExpression="Shipping">
                    <ItemTemplate>
                        <asp:Label ID="lbShipping" runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CssClass="tblLinkBlack"
                            CommandName="Edit" Visible="false"></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtnView" runat="server" Text="View" CssClass="tblLinkBlack"
                            CommandName="View"></asp:LinkButton><br />
                        <asp:LinkButton ID="lnkBtnUpdatePayment" runat="server" Text="Update Payment" CssClass="tblLinkBlack"
                            CommandName="Payment"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />&nbsp;
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
    <asp:PlaceHolder ID="phPay" runat="server" Visible="false">
        <div id="popup">
            <uc:Payment ID="up1" runat="server" />
        </div>
        <div class="overlay">
        </div>
    </asp:PlaceHolder>
</asp:Content>
