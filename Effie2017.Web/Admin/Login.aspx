<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPageLogin.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Admin_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="center">
                <div style="height: 50px">
                </div>
                <div style="width: 500px; border-style: solid; border-width: 1px">
                    <div>
                    </div>
                    <h4><br />
                        Administrator Login</h4><br />
                    <table width="20%" border="0" cellspacing="5" cellpadding="5">
                        <tr>
                            <td align="right">
                                <label>
                                    User ID:</label>
                            </td>
                            <td>
                                <label for="userid">
                                </label>
                                <asp:TextBox ID="txtUserId" runat="server" Width="150px" TabIndex="1" />
                            </td>
                            <td>
                               
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <label>
                                    Password:</label>
                            </td>
                            <td>
                                <label for="pw">
                                </label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150px" TabIndex="2" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr><td colspan="3"></td></tr>
                        <tr><td colspan="3" style="text-align: center; "> <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td></tr>
                        <tr>
                            <td colspan="3" align="center"><asp:Button ID="btnLogin" runat="server" Text="Login" TabIndex="3"   OnClick="btnLogin_Click"/></td>
                        </tr>
                         <tr><td colspan="3"></td></tr>
                    </table>

                </div>
                    <div style="height: 50px">
                </div>
            </td>
        </tr>
    </table>
    
</asp:Content>

