<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPageJuryLogin.master"
    AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Jury_Login" %>

<%@ Register TagPrefix="uc" TagName="jury_Login" Src="~/Controls/jury_Login.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="signin">
        <h2>
            Judge Sign In</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <asp:PlaceHolder ID="phLogin" runat="server">
                <tr>
                    <td width="58%" style="padding-bottom: 10px; vertical-align: top">
                        <uc:jury_Login ID="usr_Login" runat="server" loginSuccessRedirection="../Jury/Dashboard.aspx"
                            firstTimeLoginSuccessRedirection="../Jury/JuryTC.aspx" forgetPasswordVisible="false"
                            forgetPasswordRedirection="" />
                    </td>
                    <td width="5%" style="padding-bottom: 10px; border-left: 1px solid #ccc">
                        &nbsp;
                    </td>
                    <td width="37%" style="padding-bottom: 10px; vertical-align: top">
                    </td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phCutoff" runat="server" Visible="false">
                <tr style="height: 150px; text-align: center">
                    <td colspan="3">
                        You are unable to login as judging has ended.
                    </td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td colspan="3" align="center">
                    <br />
                    Best viewed in Chrome, Firefox, IE 9+ and Safari 6+.
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
