<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invitation.aspx.cs" Inherits="Main_Invitation"
    MasterPageFile="~/Common/MasterPage.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <!-- InstanceBeginEditable name="content" -->
    <h2>
        Edit Invitation (Event Year - <asp:Label runat="server" ID="lblEventYear"></asp:Label>)</h2>
    <div style="clear: both">
    </div>
    <hr />
    <br />
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
    <div class="leftContainer">
        <fieldset>
            <legend>Jury Information</legend>
            <table width="100%" class="tabledata">
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Jury ID :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:Label runat="server" ID="lblJuryId"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Name :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:Label runat="server" ID="lblName"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%;">
                        <label>
                            Company :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:Label runat="server" ID="lblCompany"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Email :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:Label runat="server" ID="lblEmail"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Country :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:Label runat="server" ID="lblCountry"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
     <div  style="margin-top: 1%;float: right;margin-right:30%;">
        
            <table width="50%" class="tabledata">
                <tr>
                    <td style="width: 60%;">
                        <label>
                            Invitation R1 :</label>
                    </td>
                    <td style="width: 40%;">
                        <asp:CheckBox runat="server" ID="chkInvRound1" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Invitation R2 :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:CheckBox runat="server" ID="chkInvRound2" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Decline :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:CheckBox runat="server" ID="chkDecline"  />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Accepted R1 :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:CheckBox runat="server" ID="chkAccptRound1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Accepted R2 :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:CheckBox runat="server" ID="chkAccptRound2" />
                    </td>
                </tr>
                <tr style="display:none;">
                    <td style="width: 10%;">
                        <label>
                            Shortlisted R1 :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:CheckBox runat="server" ID="chkShortListedRound1"   />
                    </td>
                </tr>
                <tr style="display:none;">
                    <td style="width: 10%;">
                        <label>
                            Shortlisted R2 :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:CheckBox runat="server" ID="chkShortListedRound2"  />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Assigned R1 :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:CheckBox runat="server" ID="chkAssignRound1" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%;">
                        <label>
                            Assigned R2 :</label>
                    </td>
                    <td style="width: 90%;">
                        <asp:CheckBox runat="server" ID="chkAssignRound2" Enabled="false" />
                    </td>
                </tr>
            </table>
       
    </div>
    <div style="clear: both">
    </div>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnBack" runat="server" Text="Back" onclick="btnBack_Click" />&nbsp;
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                    onclick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
