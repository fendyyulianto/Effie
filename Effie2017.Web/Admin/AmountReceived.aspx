<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="AmountReceived.aspx.cs" Inherits="Admin_AmountReceived" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h2>Amount Received History</h2>

    <telerik:RadGrid ID="radGridAmountReceived" runat="server" Skin="" MasterTableView-CssClass="tabledata"
                AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                OnItemDataBound="radGridAmountReceived_ItemDataBound" OnNeedDataSource="radGridAmountReceived_NeedDataSource">
                <PagerStyle Mode="NextPrevAndNumeric" Visible="false" />
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView TableLayout="Auto">
                    <Columns>
                        <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DateReceived" HeaderText="Date Received" DataFormatString="{0:dd MMM yyyy}" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DateReceived" HeaderText="Time" DataFormatString="{0:hh:mm}" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="User" HeaderText="User" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Amount" HeaderText="Amount(SGD)" DataFormatString="{0:N}" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="IsSetPaid" HeaderText="Set to Paid" ItemStyle-Width="50px"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Remarks" HeaderText="Remarks" ItemStyle-Width="690px"></telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
    </telerik:RadGrid>

    <input type="submit" value="Back" onclick="history.go(-1);return false;" />

</asp:Content>

