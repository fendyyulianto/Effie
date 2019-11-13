<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="ResetMasterPassword.aspx.cs" Inherits="Admin_Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="">
        <h2>Reset Master Password</h2>
        <table width="40%" border="0" cellspacing="0" cellpadding="0">
            <tr style="display:none">
                <td  style="padding-bottom: 10px; width: 200px;">
                    Master Key User:
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">
                    Password:
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:TextBox ID="txtPassword" type="Password" runat="server"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">
                    Confirm Password:
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:TextBox ID="txtPasswordConfirm" type="Password" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:Button ID="btnSave" runat="server" Text="Submit" OnClick="btnSave_OnClick" />
                </td>
            </tr>
            <tr>
                <td colspan="99"></td>
            </tr>
        </table>
    </div>
    <br/>
    <div class="">
         <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <br/> <br/>
    <script>
        function OnChangePass()
        {
            <%--TimeRequest = setInterval(function () {
               if ($("#<%=txtPassword1.ClientID%>").val() != "" && $("#<%=txtPassword1.ClientID%>").val() != null)
                {
                    $("#<%=hdnPasswordChange.ClientID%>").val("true");
                }
                clearTimeout(TimeRequest);
            }, 500);--%>
            
        }

        $(document).ready(function () {
            
        });
    </script>
</asp:Content>

