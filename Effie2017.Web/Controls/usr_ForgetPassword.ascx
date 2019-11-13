<%@ Control Language="C#" AutoEventWireup="true" CodeFile="usr_ForgetPassword.ascx.cs" Inherits="Controls_usr_ForgetPassword" %>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td style="padding-bottom:10px;width:70px;"><asp:Label ID="lblLoginId" runat="server" Text="Email"></asp:Label>:</td>
        <td style="padding-bottom:10px"><asp:TextBox ID="txtLoginId" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td>&nbsp;</td>
        <td><div class="errorDiv"><asp:Label ID="lblMsg" runat="server"></asp:Label></div><asp:Button ID="btnSendPassword" runat="server" Text="Send Password" onclick="btnSendPassword_Click" /> <asp:HyperLink ID="lnkBack" runat="server" Text="Back" Visible="false" style="vertical-align:middle"></asp:HyperLink></td>
        </tr>
    </table>
