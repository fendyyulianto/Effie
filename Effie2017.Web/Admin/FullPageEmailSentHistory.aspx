
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FullPageEmailSentHistory.aspx.cs" Inherits="Admin_FullPageEmailSentHistory" MasterPageFile="~/Admin/MasterPageAdmin.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h2> Email Sent History</h2>
    <div style="clear: both">
    </div>
    <hr />
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <telerik:RadGrid  Skin="" ID="radGridTemplateHistory" runat="server" AutoGenerateColumns="false"
        MasterTableView-CssClass="tabledata"
        AllowSorting="true" OnItemDataBound="radGridTemplateHistory_ItemDataBound" 
        OnNeedDataSource="radGridTemplateHistory_NeedDataSource"
        OnItemCommand="radGridTemplateHistory_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView TableLayout="Fixed">
            <Columns>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Type" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblType"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Name" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkRegistrationName" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Email Template" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmailTemplate"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="DateCreated" HeaderText="Date Sent" DataFormatString="{0 : dd/MMM/yyyy}">
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <br />
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click"/>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>