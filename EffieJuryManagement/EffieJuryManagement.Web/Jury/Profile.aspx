<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/MasterPageLogin.master"
    MaintainScrollPositionOnPostback="true" CodeFile="Profile.aspx.cs" Inherits="Jury_Profile" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="gen_UploadFile" Src="~/Controls/gen_UploadFile.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

        
        .TableCustom {
            font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

        .TableCustom td, #customers th {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: center;
        }

        .TableCustom tr:nth-child(even){background-color: #f1f5fb;}

        /*.TableCustom tr:hover {background-color: #ddd;}*/

        .TableCustom th {
            border: 1px solid #ddd;
            padding-top: 10px;
            padding-bottom: 10px;
            text-align: left;
            background-color: #f1f5fb;
            color: #4c607a;;
            text-align: center;
        }
    </style>
    <script language="javascript">
        function cnt() {

            var maxwords = 350;
            w = document.getElementById('<%= txtProfile.ClientID %>');
            x = document.getElementById('show_remaining_words');
            var hld = document.getElementById('<%= hldProfileCount.ClientID %>');

            var y = w.value;
            var r = 0;
            a = y.replace(/\s/g, ' ');
            a = a.split(' ');
            for (z = 0; z < a.length; z++) { if (a[z].length > 0) r++; }
            x.innerHTML = maxwords - r;

            if (maxwords < r) x.innerHTML += ' (Exceeded)';

            hld.value = x.innerHTML;
            //alert('[' + hld.value + ']');
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <!-- InstanceBeginEditable name="content" -->
    <h2>
        <div style="float: left">
            <h1>
                <asp:Label runat="server" ID="lbTitle" Text=""></asp:Label></h1>
        </div>
    </h2>
    <div style="clear: both">
    </div>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <hr />
    <p>
        *required fields</p>
    <br />
    <div class="leftContainer">
        <h2>
            Personal Information</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 281px;">
                        Type*:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlType" runat="server" Style="width: 234px">
                            <asp:ListItem Value="" Text="Please select" />
                            <asp:ListItem Value="Agency" Text="Agency" />
                            <asp:ListItem Value="Client" Text="Client" />
                            <asp:ListItem Value="Media Company" Text="Media Company" />
                            <asp:ListItem Value="Others" Text="Others" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Judge Id:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbJuryId" runat="server" Text="Auto generated upon creation" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">
                        Salutation*:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlSalutation" runat="server" Style="width: 234px">
                            <asp:ListItem Value="" Text="Please select" />
                            <asp:ListItem Value="Dr" Text="Dr." />
                            <asp:ListItem Value="Mr" Text="Mr." />
                            <asp:ListItem Value="Ms" Text="Ms." />
                            <asp:ListItem Value="Mrs" Text="Mrs." />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        First Name*:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtFirstName" MaxLength="100" runat="server" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Last Name*:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtLastName" MaxLength="100" runat="server" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Job Title:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtJobTitle" MaxLength="100" runat="server" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <%-- <tr>
                         <td style="padding-bottom: 10px">
                             Company*:
                         </td>
                         <td style="padding-bottom: 10px">
                             <input name="textfield" type="text" id="textfield">
                         </td>
                     </tr>--%>
                <tr>
                    <td>
                        Contact Number:
                    </td>
                    <td>
                        <span class="txt62">
                            <asp:TextBox ID="txtContactConutry" runat="server" MaxLength="5" Width="84px"></asp:TextBox></span>
                        <span class="txt62">
                            <asp:TextBox ID="txtContactArea" runat="server" MaxLength="5" CssClass="txt62" Width="100px"></asp:TextBox></span>
                        <span class="txt90">
                            <asp:TextBox ID="txtContact" runat="server" MaxLength="20" CssClass="txt90" Width="150px"></asp:TextBox></span>
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
                    <td>
                        Mobile Number:
                    </td>
                    <td>
                        <span class="txt62">
                            <asp:TextBox ID="txtMobileCountry" runat="server" MaxLength="5" Width="84px"></asp:TextBox></span>
                        <span class="txt62">
                            <asp:TextBox ID="txtMobileArea" runat="server" MaxLength="5" CssClass="txt62" Width="100px"></asp:TextBox></span>
                        <span class="txt90">
                            <asp:TextBox ID="txtMobile" runat="server" MaxLength="20" CssClass="txt90" Width="150px"></asp:TextBox></span>
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
                        Email* :
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtEmail" MaxLength="100" runat="server" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        PA Name :
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtPAName" MaxLength="50" runat="server" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        PA Email :
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtPAEmail" MaxLength="100" runat="server" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        PA Tel :
                    </td>
                    <td>
                        <span class="txt62">
                            <asp:TextBox ID="txtPATelCountry" runat="server" MaxLength="5" Width="84px"></asp:TextBox></span>
                        <span class="txt62">
                            <asp:TextBox ID="txtPATelArea" runat="server" MaxLength="5" CssClass="txt62" Width="100px"></asp:TextBox></span>
                        <span class="txt90">
                            <asp:TextBox ID="txtPATel" runat="server" MaxLength="20" CssClass="txt90" Width="150px"></asp:TextBox></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="padding-bottom: 10px">
                        Password*:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:Label ID="lbPassword" runat="server" Text="Auto generated upon creation" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Photo:
                    </td>
                    <td>
                        <asp:Image ID="imgPhoto" runat="server" Visible="false" Width="350px" /><br />
                        <br />
                        <asp:FileUpload ID="filePhoto" runat="server" /><br />
                        <span style="font-size: 10px;">JPEG fomat (max file size 1mb)</span>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    </div>
    <div style="clear: both">
    </div>
    <div>
        <table width="90%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    Bio (in attachment):
                </td>
                <td style="padding-top: 3%;">
                    <uc:gen_UploadFile ID="ucGen_UploadFileForProfile" runat="server" isRequired="false"
                        replaceFile="true" createDirectory="false" />
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td width="24%">
                    Bio (in words):<br />
                    <span style="font-style: italic; font-size: 11px">Max 200 words</span><br />
                    <span style="font-style: italic; font-size: 11px">This will be used on website</span>
                </td>
                <td>
                    <asp:TextBox ID="txtProfile" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <%--<p style="text-align: right; font-size: 12px">
                        Remaining word count : <font color="red"><span id="show_remaining_words">350</span></font>
                        (max 350)</p>--%>
                    <asp:HiddenField ID="hldProfileCount" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <div class="leftContainer">
        <h2>
            Company Information</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">
                        Company Name:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtCompanyName" MaxLength="100" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Address 1:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtAddress1" MaxLength="100" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Address 2:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtAddress2" MaxLength="100" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        City:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtCity" MaxLength="100" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Postal Code:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtPostalCode" MaxLength="50" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Country:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlCountry" Style="width: 234px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Type of Company:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlCompanyType" Style="width: 234px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCompanyType_SelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="" />
                            <asp:ListItem Value="Full-service Ad Agency">Full-service Ad Agency</asp:ListItem>
                            <asp:ListItem Value="Brand Identity Firm">Brand Identity Firm</asp:ListItem>
                            <asp:ListItem Value="Client">Client</asp:ListItem>
                            <asp:ListItem Value="Creative Agency">Creative Agency</asp:ListItem>
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
                        <asp:PlaceHolder ID="phCompanyTypeOther" runat="server" Visible="false">Other:
                            <asp:TextBox ID="txtCompanyTypeOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Agency Network:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlNetwork" Style="width: 234px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlNetwork_OnSelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:PlaceHolder ID="phNetworkOther" runat="server" Visible="false">Others:
                            <asp:TextBox ID="txtNetworkOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px">
                        Holding Company:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlHoldingCompany" Style="width: 234px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlHoldingCompany_OnSelectedIndexChanged">
                            <asp:ListItem Text="Select" Value="" />
                            <asp:ListItem Value="Not Applicable">Not Applicable</asp:ListItem>
                            <asp:ListItem Value="Aegis Group">Aegis Group</asp:ListItem>
                            <asp:ListItem Value="Dentsu">Dentsu</asp:ListItem>
                            <asp:ListItem Value="Hakuhodo">Hakuhodo</asp:ListItem>
                            <asp:ListItem Value="Havas Advertising">Havas Advertising</asp:ListItem>
                            <asp:ListItem Value="Interpublic (IPG)">Interpublic (IPG)</asp:ListItem>
                            <asp:ListItem Value="MDC Partners">MDC Partners</asp:ListItem>
                            <asp:ListItem Value="Omnicom">Omnicom</asp:ListItem>
                            <asp:ListItem Value="Publicis Groupe">Publicis Groupe</asp:ListItem>
                            <asp:ListItem Value="WPP Group">WPP Group</asp:ListItem>
                            <asp:ListItem Value="Independent">Independent</asp:ListItem>
                            <asp:ListItem Value="Others">Others - pls specify</asp:ListItem>
                        </asp:DropDownList>
                        <asp:PlaceHolder ID="phHoldingCompanyOther" runat="server" Visible="false">Others:
                            <asp:TextBox ID="txtHoldingCompanyOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <div class="leftContainer" style="width: 900px; display:none;">
        <h2> Profile History</h2>
        <br />
        <telerik:RadGrid ID="radCompanyList" runat="server" OnItemDataBound="radCompanyList_ItemDataBound"
            Skin="Windows7" AutoGenerateColumns="false" 
			OnNeedDataSource="radCompanyList_NeedDataSource">
            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
            <MasterTableView>
                <Columns>
                    <telerik:GridTemplateColumn Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hdfId" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No">
                        <ItemTemplate>
                            <%# Container.DataSetIndex+1 %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="Type" HeaderText="Type" SortExpression="Type" />
                    <telerik:GridBoundColumn DataField="Designation" HeaderText="Job Title" SortExpression="Designation" />
                    <telerik:GridBoundColumn DataField="Company" HeaderText="Company" SortExpression="Company" />
                    <telerik:GridBoundColumn DataField="Country" HeaderText="Country" SortExpression="Country" />
                    <telerik:GridBoundColumn DataField="CompanyType" HeaderText="Company Type" SortExpression="CompanyType" />
                    <telerik:GridBoundColumn DataField="Network" HeaderText="Network" SortExpression="Network" />
                    <telerik:GridBoundColumn DataField="HoldingCompany" HeaderText="Holding Company"
                        SortExpression="HoldingCompany" />
                    <telerik:GridBoundColumn DataField="DateCreated" HeaderText="Updated On" SortExpression="DateCreated"
                        DataFormatString="{0 : dd/MMM/yyyy}" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <div style="float: right">
            <br />
            <asp:LinkButton runat="server" ID="lnkSHowLess" Visible="false" OnClick="lnkSHowMore_Click"
                Text="show less (-)" CommandArgument="sub"></asp:LinkButton>&nbsp;
            <asp:LinkButton runat="server" ID="lnkSHowMore" Visible="false" OnClick="lnkSHowMore_Click"
                Text="show more (+)" CommandArgument="add"></asp:LinkButton>
        </div>
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <div class="leftContainer" style="width: 900px">
        <h2>
            Professional Experience</h2>
        <br />
        <table width="900px" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">
                        Market Experience:
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:CheckBoxList ID="cblMarketExperience" runat="server" Width="100%" CssClass="cbl"
                            RepeatColumns="6" RepeatDirection="Vertical">
                        </asp:CheckBoxList>
                        <asp:PlaceHolder ID="phMarketExperienceOthers" runat="server" Visible="true">Others:
                            <asp:TextBox ID="txtMarketExperienceOthers" runat="server" TextMode="MultiLine" style=" height: 80px; "/></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; padding-top: 10px; width: 271px">
                        Industry Experience:
                    </td>
                    <td style="padding-bottom: 10px">
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:CheckBoxList ID="cblIndusty" runat="server" Width="100%" CssClass="cbl" RepeatColumns="3"
                            RepeatDirection="Vertical">
                        </asp:CheckBoxList>
                        <asp:PlaceHolder ID="phIndustryOthers" runat="server" Visible="true">Others:
                            <asp:TextBox ID="txtIndustryExperienceOthers" runat="server" TextMode="MultiLine"  style=" height: 80px; "/></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px" colspan="2">
                        Specialist Skills:
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px" colspan="2">
                        <asp:CheckBoxList ID="cblSkills" runat="server" Width="100%" CssClass="cbl2" RepeatColumns="2"
                            RepeatDirection="Vertical">
                            <asp:ListItem Value="Account Services" Text="Account Services" />
                            <asp:ListItem Value="Branding" Text="Branding" />
                            <asp:ListItem Value="Creative" Text="Creative" />
                            <asp:ListItem Value="Digital" Text="Digital" />
                            <asp:ListItem Value="Head of Company/Senior Management" Text="Head of Company/Senior Management" />
                            <asp:ListItem Value="Marketing & Marketing Communications" Text="Marketing & Marketing Communications" />
                            <asp:ListItem Value="Media Services" Text="Media Services" />
                            <asp:ListItem Value="Public Relations" Text="Public Relations" />
                            <asp:ListItem Value="Research/Analytics" Text="Research/Analytics" />
                            <asp:ListItem Value="Sales/Business Development" Text="Sales / Business Development" />
                            <asp:ListItem Value="Strategic Planning" Text="Strategic Planning" />
                            <asp:ListItem Value="Social Media" Text="Social Media" />
                        </asp:CheckBoxList>
                        <asp:PlaceHolder ID="phSkillsCompanyOther" runat="server" Visible="true">Others:
                            <asp:TextBox ID="txtSkillsOthers" runat="server" TextMode="MultiLine"  style=" height: 80px; "/></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">
                        APAC Effie Experience:
                    </td>
                    <td style="padding-bottom: 10px; display:none;">
                       <asp:Repeater runat="server" ID="rptEffieExp" OnItemDataBound="rptEffieExp_OnItemDataBound">
                            <HeaderTemplate>
                                <table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkYear"  Enabled="false" />
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtYearRemarks" Enabled="false" placeholder="Enter Remarks" Visible="false" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td colspan="99">

                        <table class="TableCustom" border="1">
                            <thead>
                                <tr>
                                    <th>
                                        Years
                                    </th>
                                    <th>
                                        Invite R1
                                    </th>
                                    <th>
                                        Invite R2
                                    </th>
                                    <th>
                                        Decline
                                    </th>
                                    <th>
                                        Accept R1
                                    </th>
                                    <th>
                                        Accept R2
                                    </th>
                                    <th>
                                        Assign R1
                                    </th>
                                    <th>
                                        Assign R2
                                    </th>
                                    <th>
                                        Remarks
                                    </th>

                                </tr>
                            </thead>
                            <asp:Repeater runat="server" ID="rtpJuryYear" OnItemDataBound="rtpJuryYear_OnItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblYear" />
                                    </td>
                                    <td>
                                          <asp:CheckBox runat="server" ID="InviteR1" />
                                    </td>
                                    <td>
                                          <asp:CheckBox runat="server" ID="InviteR2" />
                                    </td>
                                    <td>
                                          <asp:CheckBox runat="server" ID="Decline" />
                                    </td>
                                    <td>
                                          <asp:CheckBox runat="server" ID="AcceptR1" />
                                    </td>
                                    <td>
                                          <asp:CheckBox runat="server" ID="AcceptR2" />
                                    </td>
                                    <td>
                                          <asp:CheckBox runat="server" ID="AssignR1" />
                                    </td>
                                    <td>
                                          <asp:CheckBox runat="server" ID="AssignR2" />
                                    </td>
                                    <td>
                                         <asp:TextBox runat="server" ID="txtYearJuryRemarks" placeholder="Enter Remarks" Enabled="false" Visible="false" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">
                        Other Effie Programs:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:CheckBoxList runat="server" ID="cblstOtherEffieExp" Width="100%" CssClass="cbl2"
                            RepeatColumns="3" RepeatDirection="Vertical">
                        </asp:CheckBoxList>
                        <asp:PlaceHolder ID="phOtherEffieExpOther" runat="server" Visible="true">Others:
                            <asp:TextBox ID="txtOtherEffieExpOthers" runat="server" MaxLength="100" /></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">
                        Other Judging Experience:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtOtherExp" runat="server" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-bottom: 10px; width: 271px">
                        Other Relevant Information:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtRevelantExp" runat="server" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="padding-bottom: 10px; width: 271px">
                        Remarks:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="padding-bottom: 10px; width: 271px">
                        Source:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:TextBox runat="server" ID="txtSource"></asp:TextBox>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="padding-bottom: 10px; width: 271px">
                        Reference:
                    </td>
                    <td style="padding-bottom: 10px">
                        <%-- <asp:HyperLink runat="server" ID="hlkReference" Target="_blank"></asp:HyperLink>--%>
                        <asp:TextBox runat="server" ID="txtReference"></asp:TextBox>
                    </td>
                </tr>
                
                <tr style="display: none;">
                    <td style="padding-bottom: 10px">
                        Specialist Type:
                    </td>
                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlSpecialistType" runat="server" Style="width: 234px">
                            <asp:ListItem Value="" Text="Please select" />
                            <asp:ListItem Value="Creative" Text="Creative" />
                            <asp:ListItem Value="Strategist/Planner" Text="Strategist/Planner" />
                            <asp:ListItem Value="Suit" Text="Suit" />
                            <asp:ListItem Value="Digital Specialist" Text="Digital Specialist" />
                            <asp:ListItem Value="Data/Technology Specialist" Text="Data/Technology Specialist" />
                            <asp:ListItem Value="Others" Text="Others" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="padding-bottom: 10px">
                        Current Industry Sector:
                    </td>

                    <td style="padding-bottom: 10px">
                        <asp:DropDownList ID="ddlCurrentIndustrySector" runat="server" Style="width: 234px">
                            <asp:ListItem Value="" Text="Please select" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <div class="leftContainer" style="width: 900px">
        <asp:CheckBox runat="server" ID="chkRecieveUpdate" Text="I would like to receive updates from APAC Effie" />
    </div>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError2" runat="server"></asp:Label></div>
    <div style="text-align: center">
        <table width="100%">
            <tr>
                <td style="text-align: center" colspan="3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Update" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">        cnt();</script>
</asp:Content>
