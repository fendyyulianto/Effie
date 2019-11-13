<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPageLogin.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="User_ForgotPassword" %>

<%@ Register TagPrefix="uc" TagName="usr_ForgetPassword" Src="~/Controls/usr_ForgetPassword.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="signin" style="width: 315px;height: 125px;">
        
      	<h2>Sign In</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
            <td width="58%" style="padding-bottom:10px;vertical-align:top">
                <uc:usr_ForgetPassword ID="usr_ForgetPassword" runat="server"
                    backVisible="true" backRedirection="../User/Login.aspx" />
            </td>
            </tr>
        </table>
        </div>

</asp:Content>

