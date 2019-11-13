<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="Entry.aspx.cs" Inherits="Main_Entry" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Src="../Controls/CompanyCredit.ascx" TagName="CompanyCredit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .rdb td {
            width: 300px;
        }
        
        .cbl td {
            width: 10%;
            height: 30px;
        }

        .GroupcblCaseData td {
            width: 10%;
            height: 30px;
        }

        .txt62 input {
            width: 62px;
        }

        .txt90 input {
            width: 90px;
        }

        .rdbBlock {
            margin-left: 0px;
            width: 100%;
        }

            .rdbBlock input {
                margin-left: -30px;
            }

            .rdbBlock label {
                margin-left: 10px;
            }

            .rdbBlock td {
                padding-left: 30px;
                padding-bottom: 20px;
            }




        .cdbBlock {
            margin-left: 0px;
            width: 100%;
        }

            .cdbBlock input {
                margin-left: -15px;
            }

            .cdbBlock label {
                margin-left: 0px;
            }

            .cdbBlock td {
                padding-left: 20px;
            }





        #popup {
            opacity: 1;
            width: 800px;
            height: 550px;
            padding: 25px;
            margin-left: 115px;
            top: 0;
            margin-top: 0px;
            position: fixed;
            border: 6px solid #bbbbbb;
            background-color: #fff;
            z-index: 100;
            overflow-y: scroll;
        }

        .overlay {
            background-color: #000;
            position: fixed;
            top: 0;
            right: 0;
            min-height: 935px;
            min-width: 1800px;
            opacity: 0.5;
            filter: alpha(opacity=50);
        }
    </style>
    <script language="javascript">

        function cnt() {
            var maxwords = 90;
            w = document.getElementById('<%= txtSummary.ClientID %>');
            x = document.getElementById('show_remaining_words');
            var hld = document.getElementById('<%= hldProfileCount.ClientID %>');

            var y = w.value;
            var r = 0;
            a = y.replace(/\s/g, ' ');
            a = a.split(' ');
            for (z = 0; z < a.length; z++) { if (a[z].length > 0) r++; }
            x.innerHTML = maxwords - r;

            if (maxwords < r) {
                var charCount = 0;
                var spaceCount = 0;
                for (var i = 0, len = w.value.length; i < len; i++) {
                    charCount++;
                    if (w.value[i] == " ")
                        spaceCount++;

                    if (spaceCount == maxwords - 1) {
                        charCount = charCount - 1;
                        break;
                    }
                }

                w.value = w.value.substr(0, charCount);
            }

            hld.value = x.innerHTML;
        }

        function AlertPayment() {
            return confirm('Please review all fields carefully and make sure all details are correct. Once entries are submitted, any requests to make changes are permitted at the discretion of the Organiser and an admin fee of SGD$200 applies.\n\nTip: If you have multiple entries, please return to the Entry Overview page and submit them altogether. This consolidates them into a single invoice by and helps to save on the admin charges.');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rblCategoryPS">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phCategory" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlCategoryPSDetail">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phDateSelection" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chkIsCampaignOngoing">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phDateSelection" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rblCategoryMarket">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phEffectiveness" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--Changing the categorypsdetail to realtime marketing makes the social platforms appear--%>
            <telerik:AjaxSetting AjaxControlID="ddlCategoryPSDetail">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phSocialPlatforms" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlEffectiveness">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phEffectiveness" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>



            <%-- Hero touchpoints to appear the others textboxes  --%>
            <telerik:AjaxSetting AjaxControlID="ddlHeroTouchPoint">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phHeroTouchpoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlHeroTouchPoint2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phHeroTouchpoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlHeroTouchPoint3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phHeroTouchpoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlEndDay">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phHeroTouchpoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlEndMonth">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phHeroTouchpoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlEndYear">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phHeroTouchpoint" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <!-- InstanceBeginEditable name="content" -->
    <div style="float: left">
        <h1>
            <asp:Literal ID="ltMainTitle" runat="server" Text="Add New" />
            Entry</h1>
    </div>
    <asp:PlaceHolder ID="phStatus" runat="server" Visible="false">
        <div style="width: 100%; text-align: right;">
            <table width="700px" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="width: 70%">Submitted on:
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lbSubmittedOn" runat="server" Font-Bold="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">Payment Status:
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lbStatusPayment" runat="server" Font-Bold="true" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">Entry Status:
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lbStatusEntry" runat="server" Font-Bold="true" Visible="false" /><asp:DropDownList
                            ID="ddlStatusEntry" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">Withdraw/DQ Status:
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lbWithdrawStatusEntry" runat="server" Font-Bold="true" Text="-" />
                        <asp:DropDownList ID="ddlWithdraw" runat="server" Visible="false">
                            <asp:ListItem Value="" Text="-" />
                            <asp:ListItem Value="WDN" Text="Withdrawn" />
                            <asp:ListItem Value="DQ" Text="DQ" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">Round 2:
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lbRound2" runat="server" Font-Bold="true" Visible="false" /><asp:DropDownList
                            ID="ddlRound2" runat="server" Visible="false">
                            <asp:ListItem Value="No" Text="No" />
                            <asp:ListItem Value="Yes" Text="Yes" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">Changed Round 2 Category:<br />
                        (if any)
                    </td>
                    <td style="width: 30%">
                        <asp:Label ID="lbCategoryR2" runat="server" Font-Bold="true" Visible="false" /><asp:DropDownList
                            ID="ddlCategoryR2" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%">&nbsp;
                    </td>
                    <td style="width: 30%">
                        <asp:Button ID="btnAdminSubmitStatus" runat="server" Text="Submit" OnClick="btnAdminSubmitStatus_Click"
                            Visible="false" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:PlaceHolder>
    <div style="clear: both">
    </div>
    <asp:PlaceHolder ID="phAdminRemarks" runat="server" Visible="false">
        <div style="margin-left: 400px;">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="vertical-align: top;">Remarks:
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbAdminRemarks" runat="server" />
                    </td>
                </tr>
                <asp:PlaceHolder ID="phAdminRemarksEdit" runat="server">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtAdminRemarks" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnAdminSubmitRemarks" runat="server" Text="Submit Remarks" OnClick="btnAdminSubmitRemarks_Click" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </table>
        </div>
    </asp:PlaceHolder>
    <div style="clear: both">
    </div>
    <br />
    <asp:Literal ID="ltInstructions" runat="server"><p>Complete the entry details below. You may save your entry as draft at any point and return to complete before the entry deadlines.</p></asp:Literal>
    <div class="errorDiv" style="text-align: left;">
        <asp:Label ID="lbConfirmation" runat="server"></asp:Label>
    </div>
    <br />
    <hr />
    <p>
        *required fields
    </p>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError2" runat="server"></asp:Label>
    </div>
    <div class="leftContainer">
        <h2>Entry Information</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">Entry Title*:
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:TextBox ID="txtCampaign" runat="server" MaxLength="35"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">Client Name*:
                </td>
                <td style="padding-bottom: 10px; width: 200px;">
                    <asp:TextBox ID="txtClient" runat="server" MaxLength="35"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;">Brand Name*:<br />
                    <span style="font-size: 12px;">(If not applicable, please enter Client Name)</span>
                </td>
                <td style="vertical-align: top;">
                    <asp:TextBox ID="txtBrand" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <div class="leftContainer">
        <h2>Categories</h2>
        <asp:PlaceHolder ID="phCategory" runat="server">Are you submitting into a Single Market
            Category or Multi-market Category?<br />
            <br />
            <asp:RadioButtonList ID="rblCategoryMarket" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblCategoryMarket_SelectedIndexChanged"
                CssClass="rdb" Width="100%" RepeatDirection="Horizontal" RepeatLayout="Table">
                <asp:ListItem Value="SM">Single Market&nbsp;&nbsp;<a href="#" class="tooltip"><img src="../images/icon-info.png" width="15" height="15" /><span>Single Market Categories are open to all cases, whether they have run in one market or across multiple markets. You should select the market with the strongest results to feature and enter success for the selected market.</span></a></asp:ListItem>
                <asp:ListItem Value="MM">Multi-Market&nbsp;&nbsp;<a href="#" class="tooltip"><img src="../images/icon-info.png" width="15" height="15" /><span>Multi-market Categories are for cases that have run across two or more markets in Asia Pacific. You may select at least two and up to three markets to feature in your case.</span></a></asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:PlaceHolder ID="phCategorySelection" runat="server">
                <%--Please select either Products & Services Category or Specialty Category.
                <br />
                The same case can only be entered into <span style="font-weight: bold;">one</span>
                Products and Services Category and <span style="font-weight: bold;">multiple</span>
                Specialty Categories.<br />--%>

                <asp:Literal ID="litProductService" runat="server" Visible="true">
                    Please select either Products & Services Category or Specialty Category.
                    <br />
                    The same case can only be entered into <span style="font-weight: bold;">one</span>
                    Products and Services Category and <span style="font-weight: bold;">multiple</span>
                    Specialty Categories.<br /><br />
                </asp:Literal>
                <asp:RadioButtonList ID="rblCategoryPS" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblCategoryPS_SelectedIndexChanged"
                    CssClass="rdb" Width="100%" RepeatDirection="Horizontal" RepeatLayout="Table">
                    <asp:ListItem Value="PSC">Products and Services Category</asp:ListItem>
                    <asp:ListItem Value="SC">Specialty Category</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <div class="category" id="DivCategoryPSDetail" runat="server" visible="true">
                    Please select:
                    <asp:DropDownList ID="ddlCategoryPSDetail" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryPSDetail_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <br />
            </asp:PlaceHolder>
        </asp:PlaceHolder>
    </div>
    <div style="clear: both">
    </div>
    <hr />
    <h2>Declaration</h2>
    Your case must run between
    <asp:Label runat="server" ID="lblCaseStartDate" Font-Bold="true"></asp:Label>
    to
    <asp:Label runat="server" ID="lblCaseEndDate" Font-Bold="true"></asp:Label>. It
    is fine if your case started before the Qualifying Period. Elements of your work
    may have been introduced earlier and continued through this period, but your case
    must be based on data relative to the Qualifying Period and the results you provide
    must be within this time frame.<br />

    <asp:Label runat="server" ID="liteffort1"></asp:Label>
    <br />
    <asp:Label runat="server" ID="liteffort2"></asp:Label>
    <br />

    <asp:Label runat="server" ID="lblSpecialCaseEndDate"></asp:Label>
    <br />
    <br />

    <asp:PlaceHolder ID="phDateSelection" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">Case Start Date*:
                </td>
                <td style="padding-bottom: 10px">
                    <asp:DropDownList ID="ddlStartDay" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlStartMonth" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlStartYear" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">Case End Date*:
                </td>
                <td style="padding-bottom: 10px">
                    <asp:DropDownList ID="ddlEndDay" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckOngoing_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlEndMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckOngoing_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlEndYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckOngoing_SelectedIndexChanged">
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkIsCampaignOngoing" runat="server" Text="Campaign Ongoing" AutoPostBack="false" OnCheckedChanged="chkIsCampaignOngoing_CheckedChanged" onclick="CheckOngoing();" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2"><%--after 30 Sep 2018--%>
                </td>
            </tr>
        </table>
        <%--<script type="text/javascript">
        function CheckOngoing()
        {
            var dd = $('#ContentPlaceHolder1_ddlEndDay').val();
            var mm = $('#ContentPlaceHolder1_ddlEndMonth').val();
            var yyyy = $('#ContentPlaceHolder1_ddlEndYear').val();

            if (dd != '' && mm != '' && yyyy != '') {
                var dateCheck = new Date(yyyy + '-' + mm + '-' + dd);
                var dateCheckAgainst = new Date('2017-09-30');

                if (dateCheck > dateCheckAgainst)
                    $('#ContentPlaceHolder1_chkIsCampaignOngoing').prop('checked', true);
                else
                    $('#ContentPlaceHolder1_chkIsCampaignOngoing').prop('checked', false);
            }
        }

        $("#ContentPlaceHolder1_ddlEndDay").change(CheckOngoing);
        $("#ContentPlaceHolder1_ddlEndMonth").change(CheckOngoing);
        $("#ContentPlaceHolder1_ddlEndYear").change(CheckOngoing);
    </script>--%>
    </asp:PlaceHolder>
    <br />
    <asp:PlaceHolder ID="phEffectiveness" runat="server">
        <div class="market">
            Effectiveness can be proven in these markets*:
            <asp:Label ID="lbEffectivenessSM" runat="server" Visible="false">(Please select the one country you are entering this case on.)<br /></asp:Label>
            <asp:Label ID="lbEffectivenessMM" runat="server" Visible="false">(Please select <span style="font-weight:bold;">minimum 2</span> countries and <span style="font-weight:bold;">maximum 3</span> countries that you will be entering this case on. These countries should be the exact markets that this case will be presented on in the Entry Form.)</asp:Label>
            <asp:DropDownList ID="ddlEffectiveness" runat="server" Visible="False" onchange="Onchange_ddlEffectiveness()">
            </asp:DropDownList>
            <input runat="server" id="cblCaseDataHidden" type="hidden"/>
            <asp:CheckBoxList ID="cblEffectiveness" runat="server" Visible="False" CssClass="cbl"
                RepeatDirection="Vertical" RepeatColumns="6">
            </asp:CheckBoxList>
        </div>
        <script>
            TimeRequest = setInterval(function () {
                Onchange_ddlEffectiveness();
            }, 1000);

            function Onchange_ddlEffectiveness() {
                CurrentSelected = $("#<%= cblCaseDataHidden.ClientID %>").val();
                if (CurrentSelected != null && CurrentSelected != "")
                {
                    if (CurrentSelected != "") {
                        var CheckboxId = document.getElementById(CurrentSelected);
                        CheckboxId.checked = false;
                        $('#' + CurrentSelected).attr('disabled', false);
                        $('#' + CurrentSelected).attr('readonly', false);
                    }
                }

                var checked_checkboxes = $("[id*=cblCaseData]");
                checked_checkboxes.each(function () {
                    if ($(this).is(":disabled") && '<%=(ViewState["Mode"] == "view")%>' == "False")
                    {
                        var Id = $(this).attr('id');
                        var CheckboxId = document.getElementById(Id);
                        CheckboxId.checked = false;
                        $(this).addClass('disabled').remove('notdisabled');
                        $('#' + Id).attr('disabled', false);
                        $('#' + Id).attr('readonly', false);
                    }

                    if ('<%=(ViewState["Mode"] == "view")%>' != "False")
                    {
                        var Id = $(this).attr('id');
                        $('#' + Id).attr('disabled', 'disabled');
                        $('#' + Id).attr('disabled', 'disabled');
                    }
                });

                checked_checkboxes.each(function () {
                    var Id = $(this).attr('id');
                    var value = $(this).val();
                    var CheckboxId = document.getElementById(Id);
                    var ddlEffectiveness = $("#<%= ddlEffectiveness.ClientID %>").val();

                    if (value == ddlEffectiveness)
                    {
                        CheckboxId.checked = true;
                        $('#' + Id).attr('disabled', 'disabled');
                        $("#<%= cblCaseDataHidden.ClientID %>").val(Id)
                    }
                });

                return false;
            }
        </script>
    </asp:PlaceHolder>
    <br />
    
    <div class="leftContainer">
        Certification of Entry by Company Representative*:&nbsp;&nbsp;<a href="#" class="tooltip"><img
            src="../images/icon-info.png" width="15" height="15" /><span>Representative can be from
                either agency or client.</span></a><br />
        <br />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">Salutation*:
                </td>
                <td style="padding-bottom: 10px">
                    <asp:DropDownList ID="ddlRepSalutation" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">First Name*:
                </td>
                <td style="padding-bottom: 10px">
                    <asp:TextBox ID="txtRepFirstname" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">Last Name*:
                </td>
                <td style="padding-bottom: 10px">
                    <asp:TextBox ID="txtRepLastname" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">Job Title*:
                </td>
                <td style="padding-bottom: 10px">
                    <asp:TextBox ID="txtRepJob" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px; width: 200px;">Company*:
                </td>
                <td style="padding-bottom: 10px">
                    <asp:TextBox ID="txtRepCompany" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Contact Number*:
                </td>
                <td>
                    <span class="txt62">
                        <asp:TextBox ID="txtRepContactCountry" runat="server" MaxLength="5"></asp:TextBox></span>
                    <span class="txt62">
                        <asp:TextBox ID="txtRepContactArea" runat="server" MaxLength="5" CssClass="txt62"></asp:TextBox></span>
                    <span class="txt90">
                        <asp:TextBox ID="txtRepContactNumber" runat="server" MaxLength="20" CssClass="txt90"></asp:TextBox></span>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px">&nbsp;
                </td>
                <td style="padding-bottom: 10px; font-size: 10px">Country Code Area Code&nbsp;&nbsp;&nbsp;Number
                </td>
            </tr>
            <tr>
                <td>Mobile Number:
                </td>
                <td>
                    <span class="txt62">
                        <asp:TextBox ID="txtRepMobileCountry" runat="server" MaxLength="5"></asp:TextBox></span>
                    <span class="txt62">
                        <asp:TextBox ID="txtRepMobileArea" runat="server" MaxLength="5"></asp:TextBox></span>
                    <span class="txt90">
                        <asp:TextBox ID="txtRepMobileNumber" runat="server"></asp:TextBox></span>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px">&nbsp;
                </td>
                <td style="padding-bottom: 10px; font-size: 10px">Country Code Area Code&nbsp;&nbsp;&nbsp;Number
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px">Email*:
                </td>
                <td style="padding-bottom: 10px; font-size: 10px">
                    <span style="padding-bottom: 10px">
                        <asp:TextBox ID="txtRepEmail" runat="server" MaxLength="100"></asp:TextBox>
                    </span>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <div style="clear: both">
        <hr />
        <h2>Credits & Effie Index Instructions</h2>
        <ul>
            <li style="margin-left: 17px">It is the entrant’s responsibility to ensure that all
                credits are submitted correctly.</li>
            <li style="margin-left: 17px">All company credits will be used to tally the <a href="http://www.effieindex.com"
                target="_blank">Effie Index</a> results, APAC Effie Special Awards - Agency of the Year, 
                Independent Agency of the Year, and Agency Network of the Year, Brand of the Year, and Marketer of the Year.  
                Beyond the points tally, they are used in the results, trophies and Awards Annual. 
                It's critical that all companies are credited properly - whether lead, co-lead or contributing. 
                We strongly advise that entrants communicate with their other agency offices, corporate office, 
                and /PR department to ensure all agency names are entered correctly. 
                This information should be communicated with contributing companies as well.</li>
            <li style="margin-left: 17px">If this case is also submitted into a local Effie competition,
                please ensure that the company names/credits are spelt in the same way.</li>
            <li style="margin-left: 17px">Once the entry is submitted, credit changes will only
                be permitted on a case by case basis and an admin fee of SGD$200 applies. At no
                time will APAC Effie permit individual or company credits to be removed or replaced.</li>
        </ul>
        <br />
        Case Summary*:&nbsp;&nbsp;<a href="#" class="tooltip"><img src="../images/icon-info.png"
            width="15" height="15" /><span>Provide a 90-word case summary about the case and its
                goals. Indicate objectives and how the evidence of results directly relates to those
                objectives. The case summary may be published or used for promotional purposes if
                the case is a winner or finalist.</span></a>
        <br />
        <br />
        <asp:TextBox ID="txtSummary" runat="server" TextMode="MultiLine"></asp:TextBox>
        <br />

        <p runat="server" style="text-align: right; font-size: 12px">
            Remaining word count : <font color="red"><span id="show_remaining_words">90</span></font>
            (max 90)
        </p>

        <asp:HiddenField ID="hldProfileCount" runat="server" />
        <br />
        Company Credits*:
        <br />
        <br />
        All entries are required to credit the client and at least one lead agency. A maximum
        of eight companies may be credited. You may credit one more client and/or one more
        lead agency and/or up to four more contributing agencies.<br />
        <br />
        <span style="font-weight: bold;">If this case becomes a finalist or winner, the company
            credits submitted here may be used on trophies and/or certificates. Please ensure
            all credits information are correct.</span>
        <br />
        <br />
        <a name="CCTABLE"></a>
        <asp:PlaceHolder ID="phCC" runat="server" Visible="false">
            <div id="popup">
                <uc1:CompanyCredit ID="cc1" runat="server" Visible="false" />
                <%--<uc1:CompanyCredit ID="cc2" runat="server" Visible="false" />--%>
            </div>
            <div class="overlay">
            </div>
        </asp:PlaceHolder>
        <asp:GridView ID="gvCC" runat="server" CssClass="tabledata" Width="100%" AutoGenerateColumns="false"
            OnRowDataBound="gvCC_OnRowDataBound" OnRowCommand="gvCC_OnRowCommand">
            <Columns>
                <asp:BoundField DataField="ContactType" HeaderText="Type" />
                <asp:BoundField DataField="Company" HeaderText="Company Name" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkView" runat="server" Text="View" CommandName="view" Visible="false" />&nbsp;
                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="modify" />&nbsp;
                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="remove" />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:PlaceHolder ID="phAddCC" runat="server">
            <asp:Button ID="btnAddCC" runat="server" OnClick="btnAddCC_Click" Text="Add" />&nbsp;Click to Add more company credits</asp:PlaceHolder>
        <br />
        <br />
        Individual Credits*:<br />
        You may credit up to ten individuals who contributed to the campaign. Please credit
        all main client and agency team members and make sure spelling is correct. All individuals
        listed may be credited in the Effie Awards journal (if published) and in the online
        Winners Showcase. Each credit is for an individual. 
        The organiser will disregard the data if there are more than one name provided for each credit.
        <br />
        <br />
        <asp:PlaceHolder ID="phIC" runat="server">
            <asp:Repeater runat="server" ID="rptIndCredits" OnItemDataBound="rptIndCredits_ItemDataBound" OnItemCommand="rptIndCredits_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="padding-bottom: 10px">No.
                            </td>
                            <td style="padding-bottom: 10px">Contact Name
                            </td>
                            <td style="padding-bottom: 10px">Title
                            </td>
                            <td style="padding-bottom: 10px">Email
                            </td>
                            <td style="padding-bottom: 10px">Company
                            </td>
                            <td style="padding-bottom: 10px">&nbsp;
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="padding-bottom: 10px">
                            <asp:Label runat="server" ID="lblCounter"></asp:Label>.
                        </td>
                        <td style="padding-bottom: 10px">
                            <asp:TextBox ID="txtICName" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="padding-bottom: 10px">
                            <asp:TextBox ID="txtICTitle" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="padding-bottom: 10px">
                            <asp:TextBox ID="txtICEmail" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                        <td style="padding-bottom: 10px">
                            <asp:DropDownList ID="ddlICCompany" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-bottom: 10px">
                            <asp:LinkButton ID="lnkClear" runat="server" Text="Clear" CommandName="clear" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </asp:PlaceHolder>
        <br />
        <hr />
        <h2>Case Data</h2>
        Product Classification*:
        <asp:DropDownList ID="ddlProductClassification" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProductClassification_SelectedIndexChanged">
            <asp:ListItem Value="">Select</asp:ListItem>
            <asp:ListItem Value="Business and industrial"> Business and industrial</asp:ListItem>
            <asp:ListItem Value="Drink and beverage"> Drink and beverage</asp:ListItem>
            <asp:ListItem Value="Financial services"> Financial services</asp:ListItem>
            <asp:ListItem Value="Food"> Food</asp:ListItem>
            <asp:ListItem Value="Government and nonprofit"> Government and nonprofit</asp:ListItem>
            <asp:ListItem Value="Household and domestic"> Household and domestic</asp:ListItem>
            <asp:ListItem Value="Leisure and entertainment"> Leisure and entertainment</asp:ListItem>
            <asp:ListItem Value="Media and publishing"> Media and publishing</asp:ListItem>
            <asp:ListItem Value="Motor and auto"> Motor and auto</asp:ListItem>
            <asp:ListItem Value="Pharmaceutical and healthcare"> Pharmaceutical and healthcare</asp:ListItem>
            <asp:ListItem Value="Retail"> Retail</asp:ListItem>
            <asp:ListItem Value="Telecoms"> Telecoms</asp:ListItem>
            <asp:ListItem Value="Tobacco"> Tobacco</asp:ListItem>
            <asp:ListItem Value="Toiletries and cosmetics">Toiletries and cosmetics</asp:ListItem>
            <asp:ListItem Value="Travel, transport and tourism"> Travel, transport and tourism</asp:ListItem>
            <asp:ListItem Value="Utilities and services"> Utilities and services</asp:ListItem>
            <asp:ListItem Value="Wearing apparel"> Wearing apparel</asp:ListItem>
            <asp:ListItem Value="Others">Others</asp:ListItem>
        </asp:DropDownList>
        <asp:PlaceHolder ID="phddlProductClassificationOther" runat="server" Visible="false">Others:
            <asp:TextBox ID="txtProductClassificationOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
        <br />
        <br />
        <br />
        Case Objective(s)* (Check all that apply):
        <asp:CheckBoxList ID="cblObjective" runat="server" RepeatColumns="2" RepeatDirection="Vertical"
            Width="100%" AutoPostBack="true" OnSelectedIndexChanged="cblObjective_SelectedIndexChanged">
            <asp:ListItem Value="Attract, support distribution, suppliers">Attract, support distribution, suppliers</asp:ListItem>
            <asp:ListItem Value="Brand launch">Brand launch</asp:ListItem>
            <asp:ListItem Value="Brand relaunch, reposition">Brand relaunch, reposition</asp:ListItem>
            <asp:ListItem Value="Build brand equity, Increase brand loyalty, Retain Existing Customers">Build brand equity, Increase brand loyalty, Retain Existing Customers</asp:ListItem>
            <asp:ListItem Value="Build, change corporate image">Build, change corporate image</asp:ListItem>
            <asp:ListItem Value="Build, defend brand position">Build, defend brand position</asp:ListItem>
            <asp:ListItem Value="Develop, revitalise market (category growth), Gain new customers">Develop, revitalise market (category growth), Gain new customers</asp:ListItem>
            <asp:ListItem Value="Gain trial">Gain trial</asp:ListItem>
            <asp:ListItem Value="Government and social aims">Government and social aims</asp:ListItem>
            <asp:ListItem Value="Increase awareness">Increase awareness</asp:ListItem>
            <asp:ListItem Value="Increase sales, volume, increase market share">Increase sales, volume, increase market share</asp:ListItem>
            <asp:ListItem Value="Maintain price premium, avoid price cut">Maintain price premium, avoid price cut</asp:ListItem>
            <asp:ListItem Value="Reach out to new audience">Reach out to new audience</asp:ListItem>
            <asp:ListItem Value="Recruitment">Recruitment</asp:ListItem>
            <asp:ListItem Value="Support event, exhibition, promotion, sponsorship">Support event, exhibition, promotion, sponsorship</asp:ListItem>
            <asp:ListItem Value="Others">Others</asp:ListItem>
        </asp:CheckBoxList>
        <br />
        Select the overarching case objective*:
        <asp:DropDownList ID="ddlObjective2" runat="server">
            <asp:ListItem Value="">Select</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <br />
        Target audience(s)* (Check all that apply):
        <asp:CheckBoxList ID="cblTargetAudience" runat="server" RepeatColumns="2" RepeatDirection="Vertical"
            Width="100%" AutoPostBack="true" OnSelectedIndexChanged="cblTargetAudience_SelectedIndexChanged">
            <asp:ListItem Value="Boomers (45+)">Boomers (45+)</asp:ListItem>
            <asp:ListItem Value="Brand Influencers">Brand Influencers</asp:ListItem>
            <asp:ListItem Value="Children (12 and below)">Children (12 and below)</asp:ListItem>
            <%-- <asp:ListItem Value="General consumers">General consumers</asp:ListItem>--%>
            <asp:ListItem Value="Housewives/Homemakers">Housewives/Homemakers</asp:ListItem>
            <asp:ListItem Value="Mass, public">Mass, public</asp:ListItem>
            <asp:ListItem Value="Others">Others, please specify</asp:ListItem>
            <asp:ListItem Value="Men">Men</asp:ListItem>
            <asp:ListItem Value="Parents/Families">Parents/Families</asp:ListItem>
            <asp:ListItem Value="Teens (13-19)">Teens (13-19)</asp:ListItem>
            <asp:ListItem Value="Women">Women</asp:ListItem>
            <asp:ListItem Value="Young people (20-35)">Young people (20-35)</asp:ListItem>
        </asp:CheckBoxList>
        <asp:PlaceHolder ID="phcblTargetAudienceOther" runat="server" Visible="false">Others:
            <asp:TextBox ID="txtTargetAudienceOther" runat="server" MaxLength="100" /><br />
        </asp:PlaceHolder>
        <br />
        Select the primary target audience*:
        <asp:DropDownList ID="ddlTargetAudiencePri" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTargetAudiencePri_SelectedIndexChanged">
            <asp:ListItem Value="">Select</asp:ListItem>
        </asp:DropDownList>
        <asp:PlaceHolder ID="phddlTargetAudiencePriOther" runat="server" Visible="false">Others:
            <asp:TextBox ID="txtTargetAudiencePriOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
        <br />
        <br />
        <br />
        <asp:PlaceHolder ID="phHeroTouchpoint" runat="server">Hero touch point*:<br />
            Indicate the three most integral touch points for your case, in order of importance*:
            (If less than three touch points are used, please select Not Applicable for the
            remaining Touch Points)<br />
            Touch Point A:
            <asp:DropDownList ID="ddlHeroTouchPoint" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHeroTouchPoint_SelectedIndexChanged" />
            <asp:PlaceHolder ID="phHeroTouchPointOther" runat="server" Visible="false">Others:
                <asp:TextBox ID="txtHeroTouchPointOther" runat="server" MaxLength="100" /></asp:PlaceHolder>
            <br />
            <br />
            Touch Point B:
            <asp:DropDownList ID="ddlHeroTouchPoint2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHeroTouchPoint2_SelectedIndexChanged" />
            <asp:PlaceHolder ID="phHeroTouchPointOther2" runat="server" Visible="false">Others:
                <asp:TextBox ID="txtHeroTouchPointOther2" runat="server" MaxLength="100" /></asp:PlaceHolder>
            <br />
            <br />
            Touch Point C:
            <asp:DropDownList ID="ddlHeroTouchPoint3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHeroTouchPoint3_SelectedIndexChanged" />
            <asp:PlaceHolder ID="phHeroTouchPointOther3" runat="server" Visible="false">Others:
                <asp:TextBox ID="txtHeroTouchPointOther3" runat="server" MaxLength="100" /></asp:PlaceHolder>
            <br />
            <br />
            <br />
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phSocialPlatforms" runat="server">Social Media Platforms used*
            (Check all that apply):<br />
            <asp:CheckBoxList ID="cblSocialPlatforms" runat="server" CssClass="cbl" Width="100%"
                RepeatColumns="5" RepeatDirection="Vertical" AutoPostBack="true" OnSelectedIndexChanged="cblSocialPlatforms_SelectedIndexChanged">
            </asp:CheckBoxList>
            <asp:PlaceHolder ID="phSocialPlatformsOthers" runat="server" Visible="false">Others:
                <asp:TextBox ID="txtSocialPlatformsOthers" runat="server" MaxLength="100" /></asp:PlaceHolder>
            <br />
            <br />
            <br />
        </asp:PlaceHolder>
        Was research used to identify problem or confirm directions of campaign?* (Check
        all that apply):
        <div class="cdbBlock">
            <asp:CheckBoxList ID="cblResearch" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                AutoPostBack="true" OnSelectedIndexChanged="cblResearch_SelectedIndexChanged"
                Width="100%">
                <asp:ListItem Value="Qualitative (focus group, indepth interviews, ethnography, etc.)">Qualitative (focus group, indepth interviews, ethnography, etc.)</asp:ListItem>
                <asp:ListItem Value="Segmentation (psychographic)">Segmentation (psychographic)</asp:ListItem>
                <asp:ListItem Value="Brand Tracker/U&amp;A">Brand Tracker/U&amp;A</asp:ListItem>
                <asp:ListItem Value="Other Quantitative (e.g. product placement, telephone, street interviews, etc.)">Other Quantitative (e.g. product placement, telephone, street interviews, etc.)</asp:ListItem>
                <asp:ListItem Value="Not Applicable">Not Applicable</asp:ListItem>
                <asp:ListItem Value="Others">Others</asp:ListItem>
            </asp:CheckBoxList>
        </div>
        <asp:Panel ID="pnlResearchOther" runat="server" Visible="false">
            Please specify
            <asp:TextBox ID="txtResearchOther" runat="server"></asp:TextBox>
        </asp:Panel>
        <br />
        Select the most important research done for your case*:
        <asp:DropDownList ID="ddlResearchImp" runat="server">
            <asp:ListItem Value="">Select</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <br />
        Countr(ies) in which case ran* (Check all that apply):&nbsp;&nbsp;<a href="#" class="tooltip"><img
            src="../images/icon-info.png" width="15" height="15" /><span>Please check all the countries
                that your campaign has run in, not limiting to the specific market(s) that this
                entry is based on.</span></a>
        <asp:CheckBoxList ID="cblCaseData" Name="GroupcblCaseData" runat="server" CssClass="GroupcblCaseData" Width="100%" RepeatColumns="6"
            RepeatDirection="Vertical">
        </asp:CheckBoxList>
        <br />
        <br />
        <br />
        <p style='font-weight: bold;'>
            Effie has partnered with the PVBLIC Foundation to support the <a href="https://sustainabledevelopment.un.org/post2015/transformingourworld"
                target="_blank">UN's 2030 Agenda for Sustainable Development</a> and its <a href="https://sustainabledevelopment.un.org/sdgs"
                    target="_blank">17 Sustainable Development Goals</a> (SDGs). Please help
            us to recognize the achievements of our industry in creating positive change by
            providing the below information:
        </p>
        <br />
        <br />
        Please tag your case against all SDGs it is aligned with. Choose all that applies.*:
        <asp:CheckBoxList ID="cblSDGOption2" runat="server" CssClass="cbl" Width="100%" RepeatColumns="3"
            RepeatDirection="Vertical" AutoPostBack="true" OnSelectedIndexChanged="cblSDGOption2_SelectedIndexChanged">
        </asp:CheckBoxList>
        <br />
        Select the goal most closely aligned with your entered case:
        <br />
        <br />
        <asp:DropDownList ID="ddlSDGOption1" runat="server">
        </asp:DropDownList>
        <br />
        <div style="clear: both">
        </div>
        <hr />
        <h2>Publishing Policy & Permission</h2>
        Entries that become Finalists and Winners in the 2020 APAC Effie Awards Competition will be showcased in various ways. Publication is at the sole discretion of the Effie Awards. <span style="font-weight: bold">Works submitted must be original or you must have received rights to submit it.</span><br />
        <br />
        The Creative Materials, Case Image & Case Summary you enter into the competition becomes the property of the APAC Effie Awards and Effie Worldwide, and will not be returned. By entering your work in the competition, the APAC Effie Awards and Effie Worldwide is automatically granted the right to make copies, reproduce and display the creative materials and case summaries for education and publicity purposes.<br />
        <br />
        The Effie Awards offers entrants the opportunity to have their written case published on the Effie Case Database /website, partner web sites and/or other publications as approved by the Effie Awards, and in the spirit of learning that Effie represents, we encourage you to share your cases so that we may inspire the industry and Make Marketing Better.<br />
        <br />
        By providing the permission to publish your written case, you are <span style="font-weight: bold">1.Bettering the industry, 2.Bettering the future leaders of our industry and 3.Showcasing your team’s success in achieving one of the top marketing honours of the year.</span><br />
        <br />
        The Effie Awards entry and judging process is designed to help all entrants present their work effectively while ensuring the confidentiality of classified information. We respect that entries may have information deemed confidential by the client.  Entrants may select from the following options:<br />
        <br />
        <div class="rdbBlock">
            <asp:RadioButtonList ID="rblPermission" runat="server">
                <asp:ListItem Value="PUBLISH" Selected="True"></asp:ListItem>
                <asp:ListItem Value="PUBLISH_EDIT"></asp:ListItem>
                <asp:ListItem Value="PUBLISH_VIEW3YEARS"></asp:ListItem>
                <asp:ListItem Value="PUBLISH_EDIT_VIEW3YEARS"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <br />
        <div style="clear: both">
        </div>
        <hr />
        <h2>Competition Terms & Rules</h2>
        <br />
        <asp:CheckBox ID="chkAgree" runat="server" />
        I agree to the competition rules*. (Click <a href="javascript:void;" onclick="window.open('TC.htm','tc','height=600,width=1000,scrollbars=1');">here</a> to view competition rules.)<br />
        <br />
        <!--
            <div class="leftContainer">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
              <tr>
                <td style="padding-bottom:10px">Name*:</td>
                <td style="padding-bottom:10px"><asp:TextBox ID="txtName" runat="server" MaxLength="100"></asp:TextBox></td>
              </tr>
              <tr>
                <td style="padding-bottom:10px">Title*:</td>
                <td style="padding-bottom:10px"><asp:TextBox ID="txtTitle" runat="server" MaxLength="100"></asp:TextBox></td>
              </tr>
              <tr>
                <td>Company Name*:</td>
                <td><asp:TextBox ID="txtCompany" runat="server" MaxLength="100"></asp:TextBox></td>
              </tr>
            </table>
            <br />
            </div>
            -->
        <div style="clear: both">
        </div>
        <div class="errorDiv">
            <asp:Label ID="lbError" runat="server"></asp:Label>
        </div>
        <div class="errorDiv" style="text-align: left;">
            <asp:Label ID="lbConfirmation2" runat="server"></asp:Label>
        </div>
        <br />
        <div style="margin: 0 auto; width: 50%">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="12%">
                        <span style="padding-bottom: 10px">
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                                CommandArgument="DO_NOT_DISABLE" />
                        </span>
                    </td>
                    <td width="68%" align="center">
                        <span style="padding-bottom: 10px">
                            <%--Add New Entry has been disabled after deadline--%>
                            <asp:Button ID="btnSaveDraft" runat="server" OnClick="btnSaveDraft_Click" Text="Save as Draft"
                                CommandArgument="DO_NOT_DISABLE" />
                        </span>
                    </td>
                    <td width="20%">
                        <span style="padding-bottom: 10px">
                            <%--Add New Entry has been disabled after deadline--%>
                            <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" Width="130px"
                                CommandArgument="DO_NOT_DISABLE" />
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <asp:PlaceHolder ID="phButtonsReviewMode" runat="server" Visible="false">
            <div style="margin: 0 auto; width: 60%">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="12%">
                            <span style="padding-bottom: 10px">
                                <%--Add New Entry has been disabled after deadline--%>
                                <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Edit" Width="130px"
                                    CommandArgument="DO_NOT_DISABLE" />
                            </span>
                        </td>
                        <td width="68%" align="center">
                            <span style="padding-bottom: 10px">
                                <%--Add New Entry has been disabled after deadline--%>
                                <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" Text="Proceed with Payment"
                                    CommandArgument="DO_NOT_DISABLE" />
                            </span>
                        </td>
                        <td width="20%">
                            <span style="padding-bottom: 10px">
                                <%--Add New disabled after deadline--%>
                                <asp:Button ID="btnSaveAdd" runat="server" OnClick="btnSaveAdd_Click" Text="Save and Add New Entry"
                                    CommandArgument="DO_NOT_DISABLE" />
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phButtonsReviewDashboardMode" runat="server" Visible="false">
            <div style="margin: 0 auto; width: 60%">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="12%">
                            <span style="padding-bottom: 10px">
                                <asp:Button ID="btnCancel2" runat="server" OnClick="btnCancel_Click" Text="Back to Entry Overview"
                                    CommandArgument="DO_NOT_DISABLE" />
                            </span>
                        </td>
                        <td width="68%" align="center">
                            <span style="padding-bottom: 10px">
                                <%--Add New Entry has been disabled after deadline--%>
                                <asp:Button ID="btnEdit2" runat="server" OnClick="btnEdit_Click" Text="Edit" Width="130px"
                                    CommandArgument="DO_NOT_DISABLE" />
                            </span>
                        </td>
                        <td width="20%">
                            <span style="padding-bottom: 10px">
                                <%--Add New Entry has been disabled after deadline--%>
                                <asp:Button ID="btnConfirm2" runat="server" OnClick="btnConfirm_Click" Text="Proceed with Payment"
                                    CommandArgument="DO_NOT_DISABLE" OnClientClick="return AlertPayment();" />
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="pbButtonsAdminMode" runat="server" Visible="false">
            <div style="margin: 0 auto; width: 60%">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="12%">
                            <span style="padding-bottom: 10px">
                                <asp:Button ID="btnCancelAdmin" runat="server" OnClick="btnCancelAdmin_Click" Text="Back"
                                    CommandArgument="DO_NOT_DISABLE" />
                            </span>
                        </td>
                        <td width="68%" align="center">
                            <span style="padding-bottom: 10px">
                                <asp:Button ID="btnSubmitAdmin" runat="server" OnClick="btnSubmitAdmin_Click" Text="Submit"
                                    CommandArgument="DO_NOT_DISABLE" />
                            </span>
                        </td>
                        <td width="20%">
                            <span style="padding-bottom: 10px">&nbsp; </span>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:PlaceHolder>
    </div>
    <!-- InstanceEndEditable -->
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" BackColor="#EEEEEE"
        Transparency="20" BackgroundPosition="Center">
        <div style="position: relative; top: 50%;">
            <img src="../images/progress.gif" />
        </div>
    </telerik:RadAjaxLoadingPanel>
    <script type="text/javascript">        cnt();</script>
</asp:Content>
