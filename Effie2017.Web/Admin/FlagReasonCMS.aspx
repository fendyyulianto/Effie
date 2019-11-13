<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="FlagReasonCMS.aspx.cs" Inherits="Admin_FlagReasonCMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="leftContainer">
        <h2>Flag Reason</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">
                   Flag Reason:
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:TextBox ID="txtBodyName" runat="server" width="700px"></asp:TextBox>
                </td>
            </tr>
            <tr ID="pnlOther" runat="server">
                <td  style="padding-bottom: 10px; width: 200px;">
                    Flag Description:
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:TextBox ID="txtDescription" runat="server" width="700px"></asp:TextBox>
                </td>
            </tr>
            <tr style="display:none">
                <td  style="padding-bottom: 10px; width: 200px;">
                    Is has other?:
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:CheckBox ID="chkHasOther" runat="server" Text="Select" ></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td  style="padding-bottom: 10px; width: 200px;">
                    Is Active:
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:CheckBox ID="chkIsActive" runat="server" Text="Active" ></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:Button ID="btnSave" runat="server" Text="Submit" OnClick="btnSave_OnClick" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" />
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
        
        <script type="text/javascript">
           $(document).ready(function(){
             $("#<%= chkHasOther.ClientID %>").change(function(){
                     $("#<%= pnlOther.ClientID %>").toggle($(this).is(':checked'));
             });
         });
        </script>
    </div>
</asp:Content>

