<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Setting.aspx.cs" Inherits="Main_Setting"
    MasterPageFile="~/Common/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <!-- InstanceBeginEditable name="content" -->
    <h2>
        Setting</h2>
    <div style="clear: both">
    </div>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <hr />
    <br />
    <div class="leftContainer">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td width="25%" valign="top">
                        Update Profile Deadline :
                    </td>
                    <td style="padding-bottom: 10px" valign="top">
                        <telerik:RadDatePicker runat="server" ID="radUpdateProfileDate" MinDate="1/1/1900" Width="180px">
                         <Calendar ShowRowHeaders="false"></Calendar>  
                        </telerik:RadDatePicker>
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td valign="top" width="20%">
                        Invitation Deadline :
                    </td>
                    <td style="padding-bottom: 10px">
                        <telerik:RadDatePicker runat="server" ID="radInvitationDate" MinDate="1/1/1900" Width="180px">
                        <DateInput Width="300px"></DateInput>
                        </telerik:RadDatePicker>
                    </td>
                    <td>
                    </td>
                </tr>
                 <tr style="display:none;">
                    <td valign="top" width="20%">
                        Event Code :
                    </td>
                    <td style="padding-bottom: 10px">
                       <asp:TextBox runat="server" ID="txtEventCode" Width="50px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    </div>
    <div style="text-align: center">
        <table width="100%">
            <tr>
                <td style="text-align: center" colspan="3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
