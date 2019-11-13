<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdminJury.master" AutoEventWireup="true" CodeFile="JuryPanel.aspx.cs" Inherits="Admin_JuryPanel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style type="text/css">
        .rdb td
        {
            width: 300px;
        }
        .cbl td
        {
            width: 10%;
            height: 30px;
        }
        .cbl2 td
        {
            width: 30%;
            height: 30px;
        }
        .cbl3
        {
            margin-left: 16px;
        }
        .txt62 input
        {
            width: 62px;
        }
        .txt90 input
        {
            width: 90px;
        }
        .rdbBlock
        {
            margin-left: 30px;
            width: 100%;
        }
        .rdbBlock input
        {
            margin-left: -20px;
        }
        .rdbBlock td
        {
            padding-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="float: left">
        <h1>
            <asp:Label runat="server" ID="lbTitle" Text=""></asp:Label></h1>
    </div>
    <asp:PlaceHolder ID="phRound" runat="server" Visible="true">
        <div style="width: 100%; text-align: right;">
            <table width="700px" border="0" cellspacing="0" cellpadding="0" style="float: right;">
                <!--
                    <tr><td style="width:70%">&nbsp;</td>
                        <td style="width:30%">
                            <asp:CheckBox ID="chkDisable" runat="server" Text="Disable" />
                        </td>
                    </tr>
                    -->
                <tr>
                    <td style="width: 70%">
                        Round 1 Panel
                    </td>
                    <td style="width: 30%">
                        <asp:DropDownList ID="ddlRound1" runat="server" Visible="false" />
                        <asp:CheckBoxList ID="cblRound1" runat="server" RepeatColumns="4" CssClass="cbl3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">
                        Round 2 Panel
                    </td>
                    <td style="width: 30%">
                        <asp:DropDownList ID="ddlRound2" runat="server" Visible="false" />
                        <asp:CheckBoxList ID="cblRound2" runat="server" RepeatColumns="4" CssClass="cbl3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </asp:PlaceHolder>
    <div style="clear: both">
    </div>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label><br />
        <span style="font-style:italic;font-weight:bold">WARNING: The Assign Panel will over-write previous assignment and resulting in Results data being purged!<br /></span>
    </div>
    <hr />
    <br />
    
    <div style="text-align: center">
        <table width="100%">
            <tr>
                <td style="text-align: right">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                </td>
                <td>
                </td>
                <td style="text-align: left">
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

