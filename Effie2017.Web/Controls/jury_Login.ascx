<%@ Control Language="C#" AutoEventWireup="true" CodeFile="jury_Login.ascx.cs" Inherits="Controls_usr_Login" %>

<table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
            <td style="padding-bottom:10px;width:70px;"><asp:Label ID="lblLoginId" runat="server" Text="Jury Id"></asp:Label>:</td>
            <td style="padding-bottom:10px"><asp:TextBox ID="txtLoginId" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
            <td style="padding-bottom:10px"><asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>:</td>
            <td style="padding-bottom:10px"><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
            <td style="padding-bottom:10px">&nbsp;</td>
            <td style="padding-bottom:10px"><asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember me" /></td>
            </tr>
            <tr>
            <td>&nbsp;</td>
            <td>
            
                <%--Login is disabled for a short while for maintainance. Sorry for the inconvience.--%>

            <div class="errorDiv"><asp:Label ID="lblMsg" runat="server"></asp:Label></div><asp:Button ID="btnLogin" runat="server" Text="Login" onclick="btnLogin_Click" Visible="true" /> <asp:HyperLink ID="lnkForgetPass" runat="server" Text="Forgot your Password?" Visible="false" style="vertical-align:middle"></asp:HyperLink></td>
            </tr>
        </table>
