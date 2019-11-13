<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentList.aspx.cs" Inherits="Main_PaymentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">
        function DeleteConfirmation(itemName) {
            return confirm('Are you sure you want to delete ' + itemName + '?');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <telerik:RadScriptManager ID="radScrptMgr" runat="server"></telerik:RadScriptManager>

    <!-- InstanceBeginEditable name="content" -->
		<h1>Payments</h1>
        <h2>Entries Submitted</h2>
        <div class="errorDiv"><asp:Label ID="lblError" runat="server"></asp:Label></div>
        <telerik:RadGrid ID="radGridEntry" runat="server" Skin="" MasterTableView-CssClass="tabledata"
            AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true"
            OnItemDataBound="radGridEntry_ItemDataBound" OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView TableLayout="Auto"><Columns>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateSubmitted" HeaderText="Date Submitted" HeaderStyle-Width="85"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Invoice" HeaderText="Invoice No." HeaderStyle-Width="137"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="GrandAmount" HeaderText="Amount Invoiced (SGD)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="137" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="AmountPaid" HeaderText="Amount Paid (SGD)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="137" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="BalanceDue" HeaderText="Balance Due (SGD)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="137" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PaymentMethod" HeaderText="Payment Method" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Invoice" HeaderStyle-CssClass="darkGrey"><ItemTemplate>
                    <%--<asp:LinkButton ID="lnkBtnInvoice" runat="server" Text="View" CommandName="Invoice"></asp:LinkButton>--%>
                    <asp:HyperLink ID="lnkInvoice" runat="server" Text="View" Target="_blank"></asp:HyperLink>
                    <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                </ItemTemplate></telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Edit" Visible="false"><ItemTemplate>
                    <%--<asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>--%>
                </ItemTemplate></telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Delete" Visible="false"><ItemTemplate>
                    <asp:LinkButton ID="lnkBtnDelete" runat="server" Text="Delete" CommandName="Delete"></asp:LinkButton>
                </ItemTemplate></telerik:GridTemplateColumn>
            </Columns></MasterTableView>
        </telerik:RadGrid>

        <table class="tabledata" rules="all" border="1" style="table-layout:auto;empty-cells:show;width:634px">
	        <colgroup>
		        <col style="width:223px" />
		        <col style="width:137px" />
		        <col style="width:137px" />
		        <col style="width:137px" />
	        </colgroup>
            <tbody>
	        <tr>
		        <td>TOTAL</td>
		        <td align="right"><asp:Label ID="lblAmountInvoiced" runat="server"></asp:Label></td>
		        <td align="right"><asp:Label ID="lblAmountPaid" runat="server"></asp:Label></td>
		        <td align="right"><asp:Label ID="lblBalanceDue" runat="server"></asp:Label></td>
	        </tr>
	        </tbody>
        </table>

        <asp:Literal ID="ltrJs" runat="server"></asp:Literal>








    <h1>Other Payments </h1>
    <h2>Entries Submitted</h2>
    <telerik:RadGrid ID="radGridEntryAdhoc" runat="server" Skin="" MasterTableView-CssClass="tabledata"
            AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true"
            OnItemDataBound="radGridEntryAdhoc_ItemDataBound" OnNeedDataSource="radGridEntryAdhoc_NeedDataSource" 
            OnItemCommand="radGridEntryAdhoc_ItemCommand">
            <PagerStyle Mode="NextPrevAndNumeric" />
            <GroupingSettings CaseSensitive="false" />
            <MasterTableView TableLayout="Auto"><Columns>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateModified" HeaderText="Date Modified" HeaderStyle-Width="85"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Invoice" HeaderText="Invoice No." HeaderStyle-Width="137"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="GrandAmount" HeaderText="Amount Invoiced (SGD)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="137" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="AmountPaid" HeaderText="Amount Paid (SGD)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="137" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="BalanceDue" HeaderText="Balance Due (SGD)" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="137" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PaymentMethod" HeaderText="Payment Method" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Invoice" HeaderStyle-CssClass="darkGrey"><ItemTemplate>
                    <asp:HyperLink ID="lnkInvoiceAdhoc" runat="server" Text="View" Target="_blank"></asp:HyperLink>
                    <asp:LinkButton ID="lnkBtnEditAdhoc" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                </ItemTemplate></telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Edit" Visible="false"><ItemTemplate>
                </ItemTemplate></telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Delete" Visible="false"><ItemTemplate>
                    <asp:LinkButton ID="lnkBtnDelete" runat="server" Text="Delete" CommandName="Delete"></asp:LinkButton>
                </ItemTemplate></telerik:GridTemplateColumn>
            </Columns></MasterTableView>
        </telerik:RadGrid>

        <table class="tabledata" rules="all" border="1" style="table-layout:auto;empty-cells:show;width:634px">
	        <colgroup>
		        <col style="width:223px" />
		        <col style="width:137px" />
		        <col style="width:137px" />
		        <col style="width:137px" />
	        </colgroup>
            <tbody>
	        <tr>
		        <td>TOTAL</td>
		        <td align="right"><asp:Label ID="lblAdhocAmountInvoiced" runat="server"></asp:Label></td>
		        <td align="right"><asp:Label ID="lblAdhocAmountPaid" runat="server"></asp:Label></td>
		        <td align="right"><asp:Label ID="lblAdhocBalanceDue" runat="server"></asp:Label></td>
	        </tr>
	        </tbody>
        </table>

        <asp:Literal ID="Literal1" runat="server"></asp:Literal>

</asp:Content>

