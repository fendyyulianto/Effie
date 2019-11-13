<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Entry.ascx.cs" Inherits="Admin_Controls_Entry" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadGrid ID="rgEntryList" runat="server"  OnItemDataBound="rgEntryList_OnItemDataBound" AutoGenerateColumns="false"      PageSize="10" AllowPaging="true">
    <MasterTableView>
        <Columns>
        <telerik:GridTemplateColumn HeaderText="Submitted By">
                <ItemTemplate>
                    <asp:Label ID="lbSubmittedBy" runat="server" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
              <telerik:GridTemplateColumn HeaderText="Invoice Amount">
                <ItemTemplate>
                    <asp:Label ID="lbInvoiceAmount" runat="server" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Amount Paid">
                <ItemTemplate>
                    <asp:Label ID="lbAmountPaid" runat="server" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn HeaderText="Balance Due">
                <ItemTemplate>
                    <asp:Label ID="lbBalanceDue" runat="server" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
              <telerik:GridTemplateColumn HeaderText="Status">
                <ItemTemplate>
                    <asp:Label ID="lbStatus" runat="server" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
        </Columns>
        </MasterTableView>
        </telerik:RadGrid>