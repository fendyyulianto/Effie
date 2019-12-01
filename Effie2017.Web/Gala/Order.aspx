<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="Order.aspx.cs" Inherits="Gala_Order" %>

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
            margin-left: -16px;
        }
        .rdbBlock td
        {
            padding-bottom: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>
        Awards Gala Order Form</h1>
    <p>
        *required fields</p>
    <p>
        &nbsp;</p>
    <br />
    <asp:Panel ID="pnlConfirmation" runat="server" Visible="false">
        <p>
            Please review all fields carefully and make sure all details are correct.
        </p>
    </asp:Panel>
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <p>
        <h2>
            Choose your Ticket type*:</h2>
    </p>
    <br />
    <div class="leftContainer">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td colspan="2" style="padding-bottom: 10px;">
                        Awards Gala Dinner - 1 table 10 pax (S$4,000.00 per table):&nbsp;
                        <asp:DropDownList ID="ddlTableCount" runat="server">
                            <asp:ListItem Value="" Text="0" />
                            <asp:ListItem Value="1" Text="1" />
                            <asp:ListItem Value="2" Text="2" />
                            <asp:ListItem Value="3" Text="3" />
                            <asp:ListItem Value="4" Text="4" />
                            <asp:ListItem Value="5" Text="5" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-bottom: 10px;">
                        Awards Gala Dinner - 1 single seat for 1 pax (S$450.00 per pax):&nbsp;
                        <asp:DropDownList ID="ddlSingleCount" runat="server">
                            <asp:ListItem Value="" Text="0" />
                            <asp:ListItem Value="1" Text="1" />
                            <asp:ListItem Value="2" Text="2" />
                            <asp:ListItem Value="3" Text="3" />
                            <asp:ListItem Value="4" Text="4" />
                            <asp:ListItem Value="5" Text="5" />
                            <asp:ListItem Value="6" Text="6" />
                            <asp:ListItem Value="7" Text="7" />
                            <asp:ListItem Value="8" Text="8" />
                            <asp:ListItem Value="9" Text="9" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 200px">
                        Shipping:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:RadioButtonList ID="rblShipping" runat="server" CssClass="rdbBlock">
                            <asp:ListItem Value="collect_office" Text="Self Collection from Organiser's office." />
                            <asp:ListItem Value="collect_onsite" Text="Self Collection on-site." />
                            <asp:ListItem Value="courier" Text="Courier to my contact address below. Shipping Fee S$15.00 per delivery. <br/>FOR LOCAL (SINGAPORE) ADDRESS ONLY" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-bottom: 30px;">
                        * A 7% Goods & Services Tax (GST) is applicable to all Singapore based companies.
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <br />
    <p>
        <h2>
            Payment Information</h2>
    </p>
    <br />
    <div class="leftContainer">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
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
            <tr>
                <td style="padding-bottom: 10px">
                    Email*:
                </td>
                <td style="padding-bottom: 10px">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px">
                    Company Name*:
                </td>
                <td style="padding-bottom: 10px">
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
                    <asp:DropDownList ID="ddlCountry" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <br />
    <p>
        <h2>
            Payment Method</h2>
    </p>
    <br />
    <div class="leftContainer">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">
                    Please select a payment option*:
                </td>
                <td style="padding-bottom: 10px;">
                    <asp:RadioButtonList ID="rblPayment" runat="server" CssClass="pm">
                        <asp:ListItem Value="PP">Credit Card via PayPal<br />
                <div style="margin-left:20px; font-size:12px;">An admin fee of 4.5% applies.</div></asp:ListItem>
                        <asp:ListItem Value="BTX">Bank Transfer<br />
                <div style="margin-left:20px; font-size:12px;">An admin fee of SGD$20 per transaction applies. Payment should be made in SGD and bank charges are on payor’s account.</div></asp:ListItem>
                        <asp:ListItem Value="CHQ">Cheque (only for local payments in SGD)</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </div>
    <div style="clear: both">
    </div>
    <br />
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
            Singapore 068914
            </p>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError2" runat="server"></asp:Label></div>
    <br />
    <div style="clear: both">
    </div>
    <div style="margin: 0 auto; width: 30%">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="33%" align="right">
                    <span style="padding-bottom: 10px">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="130px" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="130px" OnClick="btnEdit_Click"
                            Visible="false" />
                        <asp:Button ID="btnBack" runat="server" Text="Back" Width="130px" OnClick="btnBack_Click"
                            Visible="false" />
                    </span>
                </td>
                <td width="33%" align="right">
                    <span style="padding-bottom: 10px">
                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" Width="130px" OnClick="btnConfirm_Click"
                            Visible="false" />
                    </span>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
