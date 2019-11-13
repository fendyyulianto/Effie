<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master"  AutoEventWireup="true" CodeFile="AdminList.aspx.cs" Inherits="Admin_AdminList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <telerik:RadScriptBlock runat="server" ID="radScriptBlcok1">
        <script type="text/javascript">
            function DeleteConfirmation(itemName) {
                return confirm('Are you sure you want to delete ' + itemName + '?');
            }
        </script>
    </telerik:RadScriptBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>Admin User</h2>
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>Advance Search/Filter</p>
        <table width="100%">
            <tr>
                <td>
                    <label>
                        <span>Search</span> :</label>
                </td>
                <td colspan="3" style="">
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox> on

                    <asp:DropDownList ID="ddlSearch" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="name" Text="Name" />
                        <asp:ListItem Value="loginId" Text="Login Id" />
                        <asp:ListItem Value="access" Text="Access Type" />
                    </asp:DropDownList>                    
                </td>
            </tr>
            <tr style="display:none;">
                <td style="width: 10%;" >
                    <label>
                        <span>Active</span>:</label>
                </td>
                <td style="width: 40%;">
                    <asp:DropDownList ID="ddlActive" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="True" Text="Active" />
                        <asp:ListItem Value="False" Text="Inactive" />
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
    <telerik:RadGrid ID="rgAdminUser" runat="server" Skin="Windows7" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" PageSize="20"
            OnItemDataBound="radGridEntry_ItemDataBound" OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand" OnPreRender="rgAdminUser_OnPreRender">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView>
                <Columns>
                    <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Name" HeaderText="Name" ItemStyle-Width="70px" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="LoginId" HeaderText="Login ID" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Password" HeaderText="Password" ItemStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Access" HeaderText="Access Type" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DateLastLogin" HeaderText="Date Last Login" ItemStyle-Width="70px" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="DateCreated" HeaderText="Date Created" ItemStyle-Width="70px" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn  HeaderText="Is Acctive" UniqueName="AcDec" HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="60px" Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfId" runat="server" />
                            <asp:CheckBox ID="chkAcDec" runat="server" AutoPostBack="true" OnCheckedChanged="chkAcDec_OnCheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Actions" UniqueName="Actions"  HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="20px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CssClass="tblLinkBlack" CommandName="EditItem"></asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="lnkBtnDelete" runat="server" Text="Delete" CssClass="tblLinkBlack" CommandName="DeleteItem" Visible="false"></asp:LinkButton>&nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
    </telerik:RadGrid>
    <br />
    <asp:Button ID="lnkBtnAdd" runat="server" Text="Add User" OnClick="lnkBtnAdd_OnClick"></asp:Button>
    <div class="errorDiv"><asp:Label ID="lblError" runat="server"></asp:Label></div>
</asp:Content>

