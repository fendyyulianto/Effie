<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="FlagReasonCMSList.aspx.cs" Inherits="Admin_FlagReasonCMSList" %>

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
    <h2>Flag Reason</h2>

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
                        <asp:ListItem Value="bodyname" Text="Body Name" />
                        <asp:ListItem Value="description" Text="Description" />
                    </asp:DropDownList>                    
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr style="display:none">
                <td style="width: 10%;">
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
    <telerik:RadGrid ID="rgFlagReason" runat="server" Skin="Windows7" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
            OnItemDataBound="rgFlagReason_ItemDataBound" OnNeedDataSource="rgFlagReason_NeedDataSource" OnItemCommand="rgFlagReason_ItemCommand" OnPreRender="rgFlagReason_OnPreRender"
            PageSize="50">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView>
                <Columns>
                    <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Bodyname" HeaderText="Flag Reason" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Description" HeaderText="Flag Description" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn  HeaderText="Is Has Other" UniqueName="isHasOther" HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="60px"  Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfIdOther" runat="server" />
                            <asp:CheckBox ID="chkIsHasOther" runat="server" AutoPostBack="true" OnCheckedChanged="chkIsHasOther_OnCheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn  HeaderText="Is Active" UniqueName="AcDec" HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="60px" Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfId" runat="server" />
                            <asp:CheckBox ID="chkAcDec" runat="server" AutoPostBack="true" OnCheckedChanged="chkAcDec_OnCheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Actions" UniqueName="Actions"  HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CssClass="tblLinkBlack" CommandName="EditItem"></asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="lnkBtnDelete" runat="server" Text="Delete" CssClass="tblLinkBlack" CommandName="DeleteItem"></asp:LinkButton>&nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
    </telerik:RadGrid>
    <br />
    <asp:Button ID="lnkBtnAdd" runat="server" Text="Add Flag Reason" OnClick="lnkBtnAdd_OnClick"></asp:Button>
    <div class="errorDiv"><asp:Label ID="lblError" runat="server"></asp:Label></div>
</asp:Content>

