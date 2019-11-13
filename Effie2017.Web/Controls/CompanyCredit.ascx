<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompanyCredit.ascx.cs"
    Inherits="Controls_CompanyCredit" %>
<asp:HiddenField ID="hldEntryId" runat="server" />
<asp:HiddenField ID="hldRecordId" runat="server" />
<asp:HiddenField ID="hldNo" runat="server" />
<asp:HiddenField ID="hldIsNew" runat="server" />
<asp:HiddenField ID="hldDateCreated" runat="server" />
<asp:HiddenField ID="hldDateModified" runat="server" />

<div class="errorDiv"><asp:Label ID="lbError2" runat="server" /></div>

<asp:PlaceHolder ID="phAgencyInfo" runat="server" Visible="false">
    <div style="float: left; width: 800px; margin-right: 20px; margin-bottom:20px;">
        All company credits will be used to tally the Effie Effectiveness Index results, APAC Effie Agency of the Year and Agency Network of the Year Awards.
        To ensure that the correct agency office is recognised for the purpose of the results, trophies, certificates, Awards Annual, etc., we will merge the Agency Name and its City as entered in the fields below, for example, “ABC Agency Auckland”, therefore, please ensure that these fields are entered correctly.
    </div>
</asp:PlaceHolder>

<div style="float: left; width: 460px; margin-right: 20px">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td style="padding-bottom: 10px">
                Type*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:Label ID="lbType" runat="server" Text="" Visible="False"></asp:Label>
                <asp:DropDownList ID="ddlContactType" runat="server" Visible="False" AutoPostBack="true" OnSelectedIndexChanged="ddlContactType_OnSelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Company*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtCompany" runat="server" MaxLength="35"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Address 1*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtAddress1" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Address 2:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtAddress2" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                City*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtCity" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Postal Code*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtPostal" runat="server" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Country*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:DropDownList ID="ddlCountry" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>
<div style="float: left; width: 460px">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td style="padding-bottom: 10px">
                Salutation*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:DropDownList ID="ddlSalutation" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Full Name*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtFullname" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Job Title*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtJob" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Contact Number*:
            </td>
            <td style="padding-bottom: 10px">
                <span class="txt62">
                    <asp:TextBox ID="txtContactCountry" runat="server" MaxLength="5"></asp:TextBox></span>
                <span class="txt62">
                    <asp:TextBox ID="txtContactArea" runat="server" MaxLength="5"></asp:TextBox></span>
                <span class="txt90">
                    <asp:TextBox ID="txtContactNumber" runat="server" MaxLength="20"></asp:TextBox></span>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                &nbsp;
            </td>
            <td style="padding-bottom: 10px; font-size: 10px">
                Country Code Area Code&nbsp;&nbsp;&nbsp;Number
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Email*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>

<hr style="border-bottom:1px solid #eeeeee; clear: both" />

<%--<div style="float: left; width: 800px">--%>
<div style="width: 800px">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <asp:PlaceHolder ID="phCompanyType" runat="server">
        <tr>
            <td style="padding-bottom: 10px">
               Type of Company*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:DropDownList ID="ddlCompanyType" runat="server">
                    <asp:ListItem Text="Select" Value="" />
                    <asp:ListItem Value="Full-service Ad Agency">Full-service Ad Agency</asp:ListItem>
                    <asp:ListItem Value="Brand Identity Firm">Brand Identity Firm</asp:ListItem>
                    <asp:ListItem Value="Client">Client</asp:ListItem>
                    <asp:ListItem Value="Design Firm">Design Firm</asp:ListItem>
                    <asp:ListItem Value="Digital/Interactive Agency">Digital/Interactive Agency</asp:ListItem>
                    <asp:ListItem Value="Direct Marketing Agency">Direct Marketing Agency</asp:ListItem>
                    <asp:ListItem Value="Event Marketing Agency">Event Marketing Agency</asp:ListItem>
                    <asp:ListItem Value="Experiential Agency">Experiential Agency</asp:ListItem>
                    <asp:ListItem Value="Guerrilla Agency">Guerrilla Agency</asp:ListItem>
                    <asp:ListItem Value="Media Agency">Media Agency</asp:ListItem>
                    <asp:ListItem Value="Media Company">Media Company</asp:ListItem>
                    <asp:ListItem Value="Multicultural Agency">Multicultural Agency</asp:ListItem>
                    <asp:ListItem Value="Promotional Agency">Promotional Agency</asp:ListItem>
                    <asp:ListItem Value="Production Company">Production Company</asp:ListItem>
                    <asp:ListItem Value="Public Relations Firm">Public Relations Firm</asp:ListItem>
                    <asp:ListItem Value="Research Company">Research Company</asp:ListItem>
                    <asp:ListItem Value="Others">Other</asp:ListItem>
                </asp:DropDownList>
                &nbsp;
                <asp:PlaceHolder ID="phCompanyTypeOther" runat="server" Visible="false">Other: <asp:TextBox ID="txtCompanyTypeOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
            </td>
        </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phCCNetwork" runat="server">
            <tr>
                <td style="padding-bottom: 10px">
                    Client Company Network*:&nbsp;&nbsp;<a href="#" class="tooltip"><img src="../images/icon-info.png" width="15" height="15" /><span>The Client Network is used to determine the Most Effective Marketer rankings in the Effie Effectiveness Index. State the network company that the client belongs to (for eg, Procter & Gamble, Unilever, etc). If not applicable, please enter the client name.</span></a>
                </td>
                <td style="padding-bottom: 10px">
                    
                    <asp:DropDownList ID="ddlClientCompanyNetwork" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClientCompanyNetwork_OnSelectedIndexChange">
                    </asp:DropDownList>
                    <br /><br />
                    <asp:PlaceHolder ID="phClientCompanyNetworkOther" runat="server" Visible="false">Others: <asp:TextBox ID="txtClientCompanyNetworkOthers" runat="server" MaxLength="100" /></asp:PlaceHolder>
                </td>
            </tr>
        </asp:PlaceHolder>
         <asp:PlaceHolder ID="phNetwork" runat="server">
        <tr>
            <td style="padding-bottom: 10px">
                Network*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:DropDownList ID="ddlNetwork" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNetwork_OnSelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;
                <asp:PlaceHolder ID="phNetworkOther" runat="server" Visible="false">Others: <asp:TextBox ID="txtNetworkOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 10px">
                Holding Company*:
            </td>
            <td style="padding-bottom: 10px">
                <asp:DropDownList ID="ddlHoldingCompany" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHoldingCompany_OnSelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;
                <asp:PlaceHolder ID="phHoldingCompanyOther" runat="server" Visible="false">Others: <asp:TextBox ID="txtHoldingCompanyOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
            </td>
        </tr>
        </asp:PlaceHolder>
    </table>
    <div style="font-size: 12px; ">
        <span style="font-weight: bold; ">Important Note:</span>
        <div style="margin-top: 5px; ">
            <span style=" font-weight: bold; ">Network: </span>
            <div style=" margin-left: 15px; ">
                <ol>
                    <li>If an agency does not have a network, select "Non-network". </li>
                    <li>If the agency has a network that is not listed, select "OTHER" and specify.</li>
                </ol>
            </div>
        </div>
        <br/>
        <div>
            <span style=" font-weight: bold; ">Holding Company: </span>
            <div style=" margin-left: 15px; ">
                <ol>
                    <li>If an agency does not have a holding company, select "Independent".  </li>
                    <li>If the agency has a holding company and it is not listed, select "OTHER" and specify.</li>
                </ol>
            </div>
        </div>
    </div>
</div>

<%--<div style="clear: both" />--%>
<div class="errorDiv"><asp:Label ID="lbError" runat="server" /></div>
<div ><asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>


<br /><br />

<%--<div style="clear: both" />
<hr style="clear: both" />--%>