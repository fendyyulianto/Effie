<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Payment.ascx.cs" Inherits="Admin_Controls_Payment" %>
<asp:HiddenField ID="hldEntryId" runat="server" />
<h2>
    Update Payment</h2>
<div class="errorDiv">
    <asp:Label ID="lbError2" runat="server" /></div>
<div style="float: left; width: 660px; margin-right: 20px">
    <div style="text-align: right">
        <span>Invoice: </span>
        <asp:Label ID="lblInvoice" runat="server"></asp:Label></div>
    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="" MasterTableView-CssClass="tabledata"
        AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false" OnItemDataBound="radGridEntry_ItemDataBound"
        OnNeedDataSource="radGridEntry_NeedDataSource">
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Auto">
            <Columns>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Serial" HeaderText="Entry ID" ItemStyle-Width="70px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Client" HeaderText="Client" ItemStyle-Width="290px">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Submitted By" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lblSubmittedBy" runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td style="padding-bottom: 10px; width: 60%;">
                Invoice Amount(SGD):
            </td>
            <td style="padding-bottom: 10px">
                <asp:Label ID="lblAmount" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px; width: 60%;">
                Amount Previously Received(SGD):
            </td>
            <td style="padding-bottom: 10px">
                <asp:Label ID="lblPrevReceived" runat="server" />&nbsp;&nbsp;<asp:HyperLink ID="lnkAmountReceived"
                    runat="server" Text="View History" />
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px; width: 60%;">
                Amount Received(SGD)*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtAmountRecieved" runat="server" MaxLength="10" OnTextChanged="txtAmountRecieved_OnTextChanged"
                    AutoPostBack="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px; width: 60%;">
                Date Received*:
            </td>
            <td style="padding-bottom: 10px">
                <telerik:RadDatePicker ID="dpDateReceived" runat="server" DateInput-DisplayDateFormat="dd MMM yyyy" />
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10p; width: 60%;">
                Remarks:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px" colspan="2">

                <asp:RadioButtonList ID="rblPayment" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Value="OK">Paid</asp:ListItem>
                    <asp:ListItem Value="AllowUpload">Allow Upload</asp:ListItem>
                </asp:RadioButtonList>  &nbsp;&nbsp;&nbsp;

                <%--<asp:CheckBox ID="chkPaid" runat="server" />Paid &nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkAllowUpload" runat="server" />Allow Upload &nbsp;&nbsp;&nbsp;--%>
                <!-- <asp:CheckBox ID="chkSendPaidEmail" runat="server" />Send Paid Email &nbsp;&nbsp;&nbsp; -->
                <asp:Label ID="lbLastSendPaidEmailDate" runat="server" />
            </td>
        </tr>
    </table>
</div>
<hr style="border-bottom: 1px solid #eeeeee; clear: both" />
<div class="errorDiv">
    <asp:Label ID="lbError" runat="server" /></div>
<div>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />&nbsp;
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
</div>
<br />
<br />
