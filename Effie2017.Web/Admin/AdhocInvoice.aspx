<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdhocInvoice.aspx.cs" Inherits="Admin_AdhocInvoice"
    MaintainScrollPositionOnPostback="true" MasterPageFile="~/Admin/MasterPageAdmin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        a.tooltip span 
        {
            width: 200px;
        }
    </style>
    <h2>
        Generate Ad-hoc Invoice</h2>
    <br />
    <br />
    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="Windows7" AutoGenerateColumns="false" PageSize="50"
        AllowPaging="true" AllowSorting="true" OnItemDataBound="radGridEntry_ItemDataBound"
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkbox" runat="server" Visible="false" OnCheckedChanged="chkbox_OnCheckedChanged"
                            AutoPostBack="true" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Id" Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdfId" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="10px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Serial" HeaderText="Entry ID" ItemStyle-Width="70px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateSubmitted" HeaderText="Date" DataFormatString="{0:dd/MM/yy H:mm}"
                    ItemStyle-Width="60px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Campaign" HeaderText="Title" ItemStyle-Width="150px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Client" HeaderText="Client" ItemStyle-Width="90px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CategoryMarket" HeaderText="Category" ItemStyle-Width="150px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" HeaderText="Entry Status" ItemStyle-Width="80px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ProcessingStatus" UniqueName="ProcessingStatus" HeaderText="Processing Status" ItemStyle-Width="100px" AllowSorting="false"></telerik:GridBoundColumn>
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
                    AllowSorting="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Lastname" HeaderText="Last Name" ItemStyle-Width="100px"
                    AllowSorting="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Country" HeaderText="Country" ItemStyle-Width="100px"
                    AllowSorting="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn Visible="false" DataField="AdminidAssignedto" UniqueName="AdminidAssignedto" HeaderText="Assign" ItemStyle-Width="100px" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="350px">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlInvoiceType" AutoPostBack="true" Enabled="false"
                            OnSelectedIndexChanged="ddlInvoiceType_SelectedIndexChanged">
                            <asp:ListItem Text="-" Value=""></asp:ListItem>
                            <asp:ListItem Text="REOPENING OF ENTRY" Value="ReOpen"></asp:ListItem>
                            <asp:ListItem Text="CHANGE REQUEST" Value="ChangeReq"></asp:ListItem>
                            <asp:ListItem Text="EXTENSION OF DEADLINE" Value="ExtDeadLine"></asp:ListItem>
                            <asp:ListItem Text="CUSTOM" Value="Custom"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <asp:TextBox runat="server" ID="txtAmount" Width="40px" Visible="false" MaxLength="10"
                            Enabled="false"></asp:TextBox>
                        <br />
                        <br />
                        <asp:TextBox runat="server" ID="txtInvoiceCustom" Visible="false" MaxLength="200"
                            placeholder="Description for Custom Invoice" Enabled="false"></asp:TextBox>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Button ID="btnBack" runat="server" Text="Back" onclick="btnBack_Click" />
            </td>
            <td align="right">
                <asp:Button ID="btnGenerateInvoice" runat="server" Text="Generate Request" OnClientClick="return confirm('Confirm to generate invoice for checked entries?');"
                    OnClick="btnGenerateInvoice_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <div class="errorDiv">
                    <asp:Label ID="lblError" runat="server"></asp:Label></div>
            </td>
        </tr>
    </table>
</asp:Content>
