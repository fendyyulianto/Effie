<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPageLogin.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="User_ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Reset Password</h1>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <hr />
    <div class="leftContainer">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="padding-bottom: 10px">
                    <label>New Password *</label>
                </td>
                <td style="padding-bottom: 10px">
                    <asp:TextBox ID="txtPwdNew1" runat="server" MaxLength="50" Width="300px"
                        TextMode="Password" />
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px">
                    <label>Confirm Password *</label>
                </td>
                <td style="padding-bottom: 10px">
                    <asp:TextBox ID="txtPwdNew2" runat="server" MaxLength="50" Width="300px"
                        TextMode="Password" />
                </td>
            </tr>
        </table>
    </div>
    <div style="text-align: center">
        <table width="100%">
            <tr>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></td>
            </tr>
        </table>
    </div>
</asp:Content>


