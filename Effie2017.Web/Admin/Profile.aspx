<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="Admin_Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h1>View User</h1>

    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <hr />
    <br />
    <div class="leftContainer">

        <h2>Login Details</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">Email Address:<br>
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:HyperLink ID="lnkEmail" runat="server" /></td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">Verified:<br>
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lblVerified" runat="server" /></td>
                </tr>
            </tbody>
        </table>

        <br />
    </div>
    <div style="clear: both"></div>
    <hr />
    <div class="leftContainer">
        <h2>Personal Information</h2>

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">Salutation:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbSalutation" runat="server" />

                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">First Name:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbFirstname" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Last Name:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbLastname" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Job Title:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbJobTitle" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Contact Number:
                    </td>
                    <td>
                        <asp:Label ID="lbContact" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Mobile Number:
                    </td>
                    <td>
                        <asp:Label ID="lbMobile" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Fax Number:
                    </td>
                    <td>
                        <asp:Label ID="lbFax" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Website:
                    </td>
                    <td>
                        <asp:Label ID="lbWebsite" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    </div>
    <div style="clear: both"></div>
    <hr />
    <div class="leftContainer">
        <h2>Company Information</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">Company Name:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbCompany" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Address 1:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbAddress1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Address 2:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbAddress2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">City:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbCity" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Postal Code:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbPostal" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Country:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbCountry" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="clear: both"></div>
    
    <div style="display:none;">
        <hr />
        <p>Company Profile:</p>
        <br />

        <table width="100%" border="0" cellspacing="5" cellpadding="5">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 60%">Member of:
                    </td>
                    <td style="padding-bottom: 10px; width: 5%">&nbsp;
                    </td>
                    <td style="padding-bottom: 10px; width: 5%">&nbsp;
                    </td>
                    <td style="padding-bottom: 10px; width: 30%">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">The Confederation of Asian Advertising Agency Associations (CAAAA)
                    </td>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:Label ID="lbCAAAA" runat="server" Text="No" />
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbCAAAADetails" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">Effie partners in&nbsp;Asia Pacific
                    </td>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:Label ID="lbAPEP" runat="server" Text="No" />
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbAPEPDetails" runat="server" />
                    </td>
                </tr>
                <!--
                <tr>
                    <td style="padding-bottom: 10px">
                        Has your company submitted entries into a 2013 national Effie Program in Asia Pacific?
                    </td>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:Label ID="lbEffieProgram" runat="server" Text="No" />
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbNationalEffie" runat="server" /><br />
                        <asp:Label ID="lbCampaignName" runat="server" />
                    </td>
                </tr>
                -->
                <tr>
                    <td style="padding-bottom: 10px">Promotion 1
                    </td>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:Label ID="lbPromo1" runat="server" Text="No" />
                    </td>
                    <td style="padding-bottom: 10px">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">I would like to sign up for email updates on APAC Effie Awards.
                    </td>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:Label ID="lbEmailupdates" runat="server" Text="No" />
                    </td>
                    <td style="padding-bottom: 10px">&nbsp;
                    </td>
                </tr>


                <%--<tr style="display:none">
                    <td style="padding-bottom: 10px" >
                        Asian Federation of Advertising Associations (AFAA)
                    </td>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:RadioButtonList ID="rbAFAA" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                            >
                            <asp:ListItem Value="No" Text="No" />
                            <asp:ListItem Value="Yes" Text="Yes" />
                        </asp:RadioButtonList>
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlAFAA" runat="server" Style="width: 190px;" Enabled="False">
                                <asp:ListItem Value="" Text="Select" />
                                <asp:ListItem Value="Advertising Association of Nepal (AAN)" Text="Advertising Association of Nepal (AAN)" />
                                <asp:ListItem Value="Advertising Association of Pakistan (AAP)" Text="Advertising Association of Pakistan (AAP)" />
                                <asp:ListItem Value="Advertising Board of the Philippines (AdBoard Philippines)" Text="Advertising Board of the Philippines (AdBoard Philippines)" />
                                <asp:ListItem Value="Advertising Council of India (ACI)" Text="Advertising Council of India (ACI)" />
                                <asp:ListItem Value="Persatuan Perusahaan Periklanan Indonesia (PPPI)" Text="Persatuan Perusahaan Periklanan Indonesia (PPPI)" />
                                <asp:ListItem Value="International Advertising Association (IAA) UAE Chapter" Text="International Advertising Association (IAA) UAE Chapter" />
                                <asp:ListItem Value="Japan Advertising Federation (JAF)" Text="Japan Advertising Federation (JAF)" />
                                <asp:ListItem Value="Korea Federation of Advertising Associations (KFAA" Text="Korea Federation of Advertising Associations (KFAA" />
                                <asp:ListItem Value="Malaysian Advertisers Association (MAA)" Text="Malaysian Advertisers Association (MAA)" />
                                <asp:ListItem Value="Singapore Advertising and Media Alliance (SAMA)" Text="Singapore Advertising and Media Alliance (SAMA)" />
                                <asp:ListItem Value="Taipei Association of Advertising Agencies (TAAA)" Text="Taipei Association of Advertising Agencies (TAAA)" />
                                <asp:ListItem Value="The Advertising Association of Thailand (AAT)" Text="The Advertising Association of Thailand (AAT)" />
                                <asp:ListItem Value="Vietnam Advertising Association (VAA)" Text="Vietnam Advertising Association (VAA)" />                    
                        </asp:DropDownList>

                    </td>
                </tr>--%>
            </tbody>
        </table>
    </div>

    <br />
    <%--<p>If you have answered ‘No’ to all of the above, you may still enjoy the Special Rates if your company has entered into any of the most recent national Effie Programs in Asia Pacific.</p>--%>
    <br />
    <br />


    <div style="text-align: center">
        <table width="100%">
            <tr>
                <td style="text-align: right">
                    <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                </td>
                <td></td>
                <td style="text-align: left"></td>
            </tr>
        </table>
    </div>



</asp:Content>

