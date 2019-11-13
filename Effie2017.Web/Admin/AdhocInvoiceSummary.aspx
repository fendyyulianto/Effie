<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdhocInvoiceSummary.aspx.cs"
    Inherits="Admin_AdhocInvoiceSummary" MasterPageFile="~/Admin/MasterPageAdmin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .rdb td
        {
            width: 300px;
        }
        .cbl td
        {
            width: 10%;
            height: 30px;
        }
        .txt62 input
        {
            width: 62px;
        }
        .txt90 input
        {
            width: 90px;
        }
        .rdbBlock
        {
            margin-left: 30px;
            width: 100%;
        }
        .rdbBlock input
        {
            margin-left: -20px;
        }
        .rdbBlock td
        {
            padding-bottom: 10px;
        }
        
        .pm input
        {
            margin-left: -30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <!-- InstanceBeginEditable name="content" -->
    <div style="float: left">
        <h1>
            Payment</h1>
    </div>
    <asp:PlaceHolder ID="phAdminRemarks" runat="server" Visible="false">
        <div style="margin-left: 400px;">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="vertical-align: top;">
                        Remarks:
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbAdminRemarks" runat="server" />
                    </td>
                </tr>
                <asp:PlaceHolder ID="phAdminRemarksEdit" runat="server">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtAdminRemarks" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAdminSubmitRemarks" runat="server" Text="Submit Remarks" OnClick="btnAdminSubmitRemarks_Click" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </table>
        </div>
    </asp:PlaceHolder>
    <div style="clear: both">
    </div>
    <h2>
        Campaigns for Submission</h2>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabledata"
        style="font-size: 14px">
        <tr>
            <th width="28">
                No.
            </th>
            <th width="170">
                Entry ID
            </th>
             <th width="150">
               Title
            </th>
             <th width="350">
               Category
            </th>
            <th width="350">
               Description
            </th>
            <th width="150">
                Amount<br />
                (SGD)
            </th>
        </tr>
        <asp:Repeater ID="rptEntry" runat="server" OnItemDataBound="rptEntry_OnItemDataBound">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="lbNo" runat="server"></asp:Label>.
                    </td>
                    <td>
                        <asp:Label ID="lbSerialNo" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTitle" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCategory" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbDesc" runat="server"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lbFees" runat="server"></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:PlaceHolder ID="phAdminFees" runat="server" Visible="false">
            <tr>
                <td colspan="5" align="right">
                    Admin Fee
                </td>
                <td align="right">
                    <asp:Label ID="lbAdminFees" runat="server" />
                </td>
            </tr>
        </asp:PlaceHolder>
        <tr>
            <td colspan="5" align="right">
                Sub-total
            </td>
            <td align="right">
                <asp:Label ID="lbSubTotal" runat="server" />
            </td>
        </tr>
        <tr style="display:none;">
            <td colspan="5" align="right">
                Add GST @
                <asp:Label ID="lbGSTRate" runat="server" />%
            </td>
            <td align="right">
                <asp:Label ID="lbGST" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="5" align="right">
                Total Amount Payable
            </td>
            <td align="right">
                <asp:Label ID="lbTotalFees" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <%--* A 7% Goods & Services Tax (GST) is applicable to all Singapore based companies.--%>
    <%--<br />
    <br />--%>
    <br />
    <h2>
        Payment Method</h2>
    <div style="margin-left: 30px;">
        <asp:RadioButtonList ID="rblPayment" runat="server" CssClass="pm" AutoPostBack="True"
            OnSelectedIndexChanged="rblPayment_SelectedIndexChanged">
            <asp:ListItem Value="PP">Credit Card via PayPal<br />
                <span style="font-size:12px;">An admin fee of 4.5% applies.</span></asp:ListItem>
            <asp:ListItem Value="BTX">Bank Transfer<br />
                <span style="font-size:12px;">An admin fee of SGD$20 per transaction applies. Payment should be made in SGD and all bank charges must be covered by the entrant.</span></asp:ListItem>
            <asp:ListItem Value="CHQ">Cheque (only for local payments in SGD)</asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <br />
    <p>
          For Bank Transfer, payment to be made to: <br />
          Name of Account: Ifektiv Pte Ltd <br />
          Bank: OCBC Bank (Oversea-Chinese Banking Corporation Limited) <br />
          Bank Address: 65 Chulia Street, OCBC Centre, Singapore 049513 <br />
          Bank Code: 7339 <br />
          A/C No.: 687-703702-001 <br />
          Swift Code: OCBCSGSG
            </p>

            <br />
            <p>
            Cheque should be made payable to: <span style="font-weight:bold">Ifektiv Pte Ltd </span><br />
            Mail the cheque, attaching a copy of the payment invoice to the following address: <br />
            APAC Effie Awards <br />
            c/o Ifektiv Pte Ltd <br />
            160 Robinson Road, #25-12 SBF Center<br/>
            Singapore 188613
            </p>
    <br />
    <br />
    <h2>
        Payment Information</h2>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="17%" style="padding-bottom: 10px">
                Company Name*:
            </td>
            <td width="83%" style="padding-bottom: 10px">
                <asp:TextBox ID="txtCompany" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Address 1*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtAddress1" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Address 2:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtAddress2" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                City*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtCity" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Postal Code*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtPostal" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Country*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                First Name*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtFirstname" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Last Name*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtLastname" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Contact Number*:
            </td>
            <td>
                <span class="txt62">
                    <asp:TextBox ID="txtContactCountryCode" runat="server" MaxLength="100"></asp:TextBox></span>
                <span class="txt62">
                    <asp:TextBox ID="txtContactAreaCode" runat="server" MaxLength="100"></asp:TextBox></span>
                <span class="txt90">
                    <asp:TextBox ID="txtContactNumber" runat="server" MaxLength="100"></asp:TextBox></span>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                &nbsp;
            </td>
            <td style="padding-bottom: 10px; font-size: 10px">
                Country Code Area Code&nbsp;&nbsp;&nbsp;Number
            </td>
        </tr>
    </table>
    <br />
    <h2>
        Reminders:</h2>
    <ol style="margin-left: 20px;">
        <li>Please ensure that all payment details are correct.</li>
       <%-- <li>Entrants have the onus to make accurate declarations of the Company Profile which
            determines the Entry Fees. Incorrect fees charged due to inaccurate declarations
            will be accounted for by entrants.</li>
        <li>Entries which are not accompanied with the correct entry fee payment will not be
            accepted.</li>--%>
        <li>Full payment must be received before entrants can upload the supporting documents
            for submission.</li>
        <li>Refund Policy:
            <br />
            a. The Organiser issues refunds only in cases where entrants have overpaid or been
            incorrectly charged.<br />
            b. No refunds will be made for any withdrawals, disqualifications or incomplete
            submissions.<br />
        </li>
    </ol>
    <div class="errorDiv">
        <asp:Label ID="lbError2" runat="server"></asp:Label></div>
    <br />
    <div style="clear: both">
    </div>
    <div style="margin: 0 auto; width: 30%">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="12%">
                    <span style="padding-bottom: 10px">
                        <asp:Button ID="btnBack" runat="server" Text="Cancel" Width="130px" OnClick="btnBack_Click" />
                    </span>
                </td>
                <td width="20%" align="right">
                    <span style="padding-bottom: 10px">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="130px" OnClick="btnSubmit_Click" />
                    </span>
                </td>
            </tr>
        </table>
    </div>
    <!-- InstanceEndEditable -->
</asp:Content>
