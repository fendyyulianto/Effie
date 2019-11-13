<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="EntryProcessing.aspx.cs" Inherits="Admin_EntryProcessing" %>

<%@ Register Src="~/Admin/Controls/Payment.ascx" TagName="Payment" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/lib/jquery.mousewheel-3.0.6.pack.js"></script>
    <!-- Add fancyBox main JS and CSS files -->
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/jquery.fancybox.js?v=2.1.5"></script>
    <link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/jquery.fancybox.css?v=2.1.5"
        media="screen" />
    <!-- Add Button helper (this is optional) -->
    <link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-buttons.css?v=1.0.5" />
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-buttons.js?v=1.0.5"></script>
    <!-- Add Thumbnail helper (this is optional) -->
    <link rel="stylesheet" type="text/css" href="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-thumbs.css?v=1.0.7" />
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-thumbs.js?v=1.0.7"></script>
    <!-- Add Media helper (this is optional) -->
    <script type="text/javascript" src="../js/fancyapps-fancyBox-v2.1.5/source/helpers/jquery.fancybox-media.js?v=1.0.6"></script>

    <script type="text/javascript">
        function CheckAll(id) {
            var masterTable = $find("<%= radGridEntry.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == true) {
                for (var i = 0; i < row.length; i++) {
                    var checkBox = masterTable.get_dataItems()[i].findElement("chkboxSelect");
                    if (checkBox != null)
                        checkBox.checked = true;  // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    var checkBox = masterTable.get_dataItems()[i].findElement("chkboxSelect");
                    if (checkBox != null)
                        checkBox.checked = false; // for unchecking the checkboxes
                }
            }
        }
        function unCheckHeader(id, clientId) {

            var masterTable = $find("<%= radGridEntry.ClientID %>").get_masterTableView();
            var row = masterTable.get_dataItems();
            if (id.checked == false) {
                var hidden = document.getElementById("hdfCheck");
                var checkBox = document.getElementById(clientId);
                checkBox.checked = false;
            }
        }

        $(document).ready(function () {
            $('.fancybox').fancybox({
                beforeClose: function () {
                    $('body').fadeOut({
                        //duration: 700, complete: function () {
                        //    window.location.reload();
                        //}
                    });
                    return;
                },
                autoSize: false,
                height: 700,
                width: 800,
                closeClick: false, // prevents closing when clicking INSIDE fancybox
                helpers: {
                    overlay: { closeClick: false } // prevents closing when clicking OUTSIDE fancybox
                }
            });

        });

        function CloseFancyBox() {
            $("a[title='Close']").click();
        }

        //function triggerSend(params) {
        //    if (window.confirm('Are you sure to continue ?') == false) {
        //        return false;
        //    } else {
        //        __doPostBack('btnSend', params);
        //    }
        //}

        function ClearDataOnLoad() {

            theForm.__EVENTTARGET.value = "";
            theForm.__EVENTARGUMENT.value = "";
            theForm.submit();
        }

    </script>
    <style>
        .cbl td {
            width: 10%;
            height: 30px;
        }

        .ModalPopUpSmall {
            opacity: 1;
            width: 550px;
            height: 190px;
            padding: 25px;
            margin-left: 115px;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            margin: auto;
            position: fixed;
            border: 6px solid #bbbbbb;
            background-color: #fff;
            z-index: 1002;
            overflow-y: scroll;
        }

        .ModalPopUpBig {
            left: 150px;
            top: 30px;
            opacity: 1;
            width: 800px;
            height: 550px;
            padding: 25px;
            margin-left: 115px;
            top: 15px;
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

        a.tooltip span {
            width: 200px;
        }
    </style>

    <h2><%=PageTitle %></h2>

    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>Advance Search/Filter</p>
        <table width="100%">
            <tr>
                <td>
                    <label>
                        <span>Search</span> :</label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                    on

                    <asp:DropDownList ID="ddlSearch" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="title" Text="Title" />
                        <asp:ListItem Value="EntryID" Text="Entry ID" />
                        <asp:ListItem Value="company" Text="Company" />
                        <asp:ListItem Value="firstname" Text="First Name" />
                        <asp:ListItem Value="lastname" Text="Last Name" />
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td style="width: 20%;">
                    <label>
                        <span>Entry Status</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlEntryStatus" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <%--<asp:ListItem Value="PPN" Text="Pending Payment" />
                        <asp:ListItem Value="UPN" Text="Pending Upload" />--%>
                        <asp:ListItem Value="UPC" Text="Pending Completion" />
                        <asp:ListItem Value="OK" Text="Closed" />
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>DL</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDeadline" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="D1" Text="D1" />
                        <asp:ListItem Value="D2" Text="D2" />
                        <asp:ListItem Value="D3" Text="D3" />
                        <asp:ListItem Value="D4" Text="D4" />
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td style="width: 20%;">
                    <label>
                        <span>Processing Status</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlProcessingStatus" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="UPV" Text="Pending Verification" />
                        <asp:ListItem Value="REP" Text="Reopened" />
                        <asp:ListItem Value="OK" Text="Completed" />
                        <asp:ListItem Value="PREP" Text="Pending Reopen" />
                    </asp:DropDownList>
                </td>
                <td style="width: 20%;">
                    <label>
                        <span>Market</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlMarket" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="SM" Text="Single Market" />
                        <asp:ListItem Value="MM" Text="Multi Market" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    <label><span runat="server" id="lblsearchAssignTo">Assign</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlAssignedTo" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Category</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Country</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>DQ Flag</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDQFlag" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="DQ" Text="DQ" />
                        <asp:ListItem Value="None" Text="None" />
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <br />


    
    <div class="errorDiv"> <asp:Label ID="lblError" runat="server"></asp:Label></div>
    <telerik:RadTabStrip runat="server" ID="rtabEntry" CssClass="tabledata" MultiPageID="RadMultiPage1" SelectedIndex="1" 
        OnTabClick="rtabEntry_TabClick">
        <Tabs>
            <%--<telerik:RadTab Text="All Entries" Width="150px" Value="ALL" />--%>
            <telerik:RadTab Text="All Entries Processing" Width="150px" Value="ALLP" />
            <telerik:RadTab Text="Pending Verification" Width="150px" Value="UPV" />
            <%--<telerik:RadTab Text="Pending Reopen" Width="150px" Value="PREP" />--%>
            <telerik:RadTab Text="Reopened" Width="150px" Value="REP" />
            <telerik:RadTab Text="Completed" Width="150px" Value="OK" />
        </Tabs>
    </telerik:RadTabStrip>

    <br />

    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="Windows7" PageSize="50" 
        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSortCommand="radGridEntry_SortCommand"
        OnItemDataBound="radGridEntry_ItemDataBound" OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView AllowCustomSorting="true">
            <Columns>
                <telerik:GridTemplateColumn HeaderStyle-Width="130px" UniqueName="SelectColumn">
                    <HeaderTemplate>
                        <center>
                            All<br/>
                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" />
                        </center>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <center>
                            <asp:CheckBox runat="server" ID="chkboxSelect" /></center>
                        <asp:HiddenField ID="hdfId" runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Deadline" UniqueName="Deadline" HeaderText="DL" HeaderStyle-Width="80px" ></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Serial" UniqueName="EntryID" HeaderText="Entry ID" HeaderStyle-Width="80px"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px" UniqueName="No">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="DateCreated" UniqueName="DateCreated" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}" ItemStyle-Width="60px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Campaign" UniqueName="Campaign" HeaderText="Title" ItemStyle-Width="205px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Client" UniqueName="Client" HeaderText="Ciient" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CategoryMarket" UniqueName="CategoryMarket" HeaderText="Category" ItemStyle-Width="200px"></telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn HeaderText="Submitted By" DataField="SubmittedBy" UniqueName="SubmittedBy" ItemStyle-Width="80px">
                    
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Firstname" UniqueName="Firstname" HeaderText="First Name" ItemStyle-Width="100px" AllowSorting="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Lastname" UniqueName="Lastname" HeaderText="Last Name" ItemStyle-Width="100px" AllowSorting="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Country" HeaderText="Country" ItemStyle-Width="100px" AllowSorting="true">
                </telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn DataField="AdminidAssignedto" UniqueName="AdminidAssignedto" HeaderText="Assign" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" UniqueName="Status" HeaderText="Entry Status" ItemStyle-Width="10"></telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn DataField="MaterialsSubmitted" UniqueName="MaterialsSubmitted" HeaderText="Date Closed" ItemStyle-Width="60px" >
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ReopeningDeadline" UniqueName="ReopeningDeadline" HeaderText="Reopening Deadline" ItemStyle-Width="60px">
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn HeaderText="Reopening Fee" UniqueName="ReopeningFee" ItemStyle-Width="70px" SortExpression="Invoice">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkInvoice" runat="server" Text="" CssClass="tblLinkBlack" Target="_blank"></asp:HyperLink>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridBoundColumn DataField="ProcessingStatus" UniqueName="ProcessingStatus" HeaderText="Processing Status" ItemStyle-Width="100px" ></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DQFlag" UniqueName="DQFlag" HeaderText="DQ Flag" ItemStyle-Width="100px"></telerik:GridBoundColumn>
               
                <telerik:GridBoundColumn DataField="NotificationSentDate" UniqueName="NotificationSentDate" HeaderText="Notify Date" ItemStyle-Width="60px">
                </telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn DataField="ReopeningDate" UniqueName="ReopeningDate" HeaderText="Date Reopen" ItemStyle-Width="60px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Country" HeaderText="Country" ItemStyle-Width="100px" Visible="false" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateVerified" UniqueName="DateVerified" HeaderText="Date Verified" ItemStyle-Width="60px">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey" ItemStyle-Width="700px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkBtnView" runat="server" Text="View Entry PDF" CssClass="tblLinkBlack" Target="_blank" Visible="false"></asp:HyperLink>
                        <%--<asp:LinkButton ID="lnkBtnView" runat="server" Text="View" CssClass="tblLinkBlack" CommandName="View"></asp:LinkButton>--%>
                        <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CssClass="tblLinkBlack" CommandName="Edit"></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtnUpdatePayment" runat="server" Text="Update Payment" CssClass="tblLinkBlack" CommandName="Payment"></asp:LinkButton>
                        <asp:LinkButton ID="lbkAdhocInvoice" runat="server" Text="AH Invoice" CssClass="tblLinkBlack" CommandName="adhoc"></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnUpdateDQI" runat="server" Text="Update DQ" CssClass="tblLinkBlack" CommandName="UpdateDQI"></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnViewDQI" runat="server" Text="View DQ" CssClass="tblLinkBlack" CommandName="ViewDQI"></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnUpdateStatus" runat="server" Text="Update Status" CssClass="tblLinkBlack" CommandName="UpdateStatus"></asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnReopen" runat="server" Text="Reopen" CssClass="tblLinkBlack" CommandName="Reopen" OnClientClick="return confirm('Confirm to Reopen?');"></asp:LinkButton>
                        <asp:LinkButton ID="lnkCompleteProcessing" runat="server" Text="Complete" CssClass="tblLinkBlack" CommandName="CompleteProcessing" OnClientClick="return confirm('Confirm to Complete Processing?');"></asp:LinkButton>
                        <asp:LinkButton ID="lnkResetStatus" runat="server" Text="Reset Status" CssClass="tblLinkBlack" CommandName="ResetStatus" Target="_blank" Visible="false" OnClientClick="return confirm('Confirm to Reset Status?');"></asp:LinkButton>
                        <%--<asp:LinkButton ID="lnkEmailHistory" runat="server" Text="Email History2" CssClass="tblLinkBlack" CommandName="EmailHistory" Target="_blank"></asp:LinkButton>--%>
                        <asp:HyperLink ID="hlkEmailHistory" runat="server" Text="Email History" CssClass="tblLinkBlack" Target="_blank"></asp:HyperLink><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <table>
        <tr>
            <td>
                <asp:Button ID="btnAssignToST" runat="server" Text="Assign User" OnClick="btnAssignToST_Click" />

                <asp:Button ID="btnComplete" runat="server" Text="Complete Processing" OnClick="btnComplete_Click" OnClientClick="return confirm('Confirm to Complete Processing?');" />

                <asp:Button ID="btnUpdateStatus" runat="server" Text="Update Status" OnClick="btnUpdateStatus_Click" />

                <asp:Button ID="btnUpdateDQIssue" runat="server" Text="Update DQ" OnClick="btnUpdateDQIssue_Click" />

                <asp:Button ID="btnDQFlagNotification" runat="server" Text="" OnClick="btnDQFlagNotification_Click" />

                <asp:Button ID="btnDeadlineReminder" runat="server" Text="Deadline Reminder" OnClick="btnDQFlagNotification_Click" />

                <asp:Button ID="btnReport" runat="server" Text="Report" OnClick="btnReport_Click" />

            </td>
        </tr>
    </table>

    <div class="errorDiv"> <asp:Label ID="lblError2" runat="server"></asp:Label></div>


<%--    <asp:PlaceHolder ID="phEmailHistory" runat="server" Visible="false">
            <div class="fancybox-overlay fancybox-overlay-fixed" style="width: auto; height: auto; display: block;">
	            <div class="fancybox-wrap fancybox-desktop fancybox-type-iframe fancybox-opened" tabindex="-1" style="width: 970px; height: auto; position: absolute; top: 20px; left: 189px; opacity: 1; overflow: visible;">
		            <div class="fancybox-skin" style="padding: 15px; width: auto; height: auto;">
			            <div class="fancybox-outer">
			            <div class="fancybox-inner" style="overflow: auto; width: 940px; height: 350px;">
			                <iframe ID="iframeEmailHistory" runat="server" style=" width: 100%; height: 100%; " src="./RegistrationEmailSentHistory.aspx?regId=ff909700-4ec6-4089-be27-e99a48bd5fed&EntryId=b8309da4-2c54-4a11-a687-64f06acaa3d0"></iframe>
			            </div>
			            </div>
		            <a title="Close" class="fancybox-item fancybox-close" href="javascript:;"></a>
		            </div>
	            </div>
            </div>
    </asp:PlaceHolder>--%>

    <asp:PlaceHolder ID="phPay" runat="server" Visible="false">
        <div class="ModalPopUpBig">
            <uc:Payment ID="up1" runat="server" />
        </div>
        <div class="overlay"></div>
    </asp:PlaceHolder>



    <asp:PlaceHolder ID="PHUpdateDQ" runat="server" Visible="false">
        <div class="ModalPopUpBig">
            <h2>
                <asp:Label runat="server" ID="Label1"> Update DQ</asp:Label></h2>
            <hr />
            <div style="display: none;">
                <br />
                Flag Description :<br />
                <br />
                <asp:TextBox ID="txtFlagDescription" runat="server" TextMode="MultiLine" placeholder="Click here to enter text"></asp:TextBox>
                <br />
                <br />
            </div>

            <asp:CheckBoxList ID="cblFlagReason" runat="server" Visible="false"></asp:CheckBoxList>

            <table class="tabledata" rules="all" border="1">
                <asp:Repeater runat="server" ID="rptFlagReason" OnItemDataBound="rptFlagReason_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbItem" runat="server" />
                                <asp:CheckBox ID="isHasOther" runat="server" Visible="false" />
                            </td>
                            <td>
                                <asp:HiddenField ID="hdItemId" runat="server" />
                                <asp:HiddenField ID="hdAttrType" runat="server" />
                                <asp:Literal ID="Title" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr id="trOther" runat="server" visible="false">
                            <td>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtOtherDQ" runat="server" TextMode="MultiLine"> </asp:TextBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <br />
            <br />
            <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmitPHUpdateDQ_Click" />
            &nbsp;
             <asp:Button runat="server" ID="btnCloseQD" Text="Close" OnClick="btnClose_Click" />
        </div>
        <div class="overlay">
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phAssignTo" runat="server" Visible="false">
        <div class="ModalPopUpSmall">
            <h2>
                <asp:Label runat="server" ID="Label4">Assign To Users</asp:Label></h2>
            <hr />
            <br />
            <table width="100%">
                <tr runat="server" id="Tr3">
                    <td width="30%">User :
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlAssignTo" Style="width: 100%;">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <asp:Button runat="server" ID="btnAssignTo" Text="Submit" OnClick="btnSubmitAssignTo_Click" />
            &nbsp;
             <asp:Button runat="server" ID="Button4" Text="Close" OnClick="btnClose_Click" />
        </div>
        <div class="overlay">
        </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phUpdateStatus" runat="server" Visible="false">
        <div class="ModalPopUpSmall">
            <h2>
                <asp:Label runat="server" ID="Label3"> Update Status</asp:Label></h2>
            <hr />
            <br />
            <table width="100%">
                <tr runat="server" id="Tr2">
                    <td style="width: 20%;">
                        <label>
                            <span>Processing Status</span> :</label>
                    </td>
                    <td style="width: 30%;">
                        <asp:DropDownList ID="ddlupdateProcessingStatus" runat="server" Style="width: 100%;">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <asp:Button runat="server" ID="btnphUpdateStatus" Text="Submit" OnClick="btnphUpdateStatus_Click" />
            &nbsp;
             <asp:Button runat="server" ID="Button3" Text="Close" OnClick="btnClose_Click" />
        </div>
        <div class="overlay">
        </div>
    </asp:PlaceHolder>


    <asp:PlaceHolder ID="phDQFlagNotification" runat="server" Visible="false">
        <div id="divDQFlagNotification" class="ModalPopUpSmall" runat="server">
            <h2>
                <asp:Label runat="server" ID="lblTitle"></asp:Label></h2>
            <hr />
            <br />
            <table width="100%">
                <tr runat="server" id="templateRow">
                    <td width="120px">Select Template:
                    </td>
                    <td width="200px">
                        <asp:DropDownList runat="server" ID="ddlTemplateList" style="width:300px" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateList_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td width="200px">
                        <asp:HyperLink runat="server" ID="hlkPreview" Visible="false" Text="Preview Template"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                         <asp:Button ID="btnSend" runat="server" Text="Send"  OnClick="btnSendMail_Click" OnClientClick="return confirm('Are you sure to continue ?');" />
                        <%--<asp:Button ID="btnSend" runat="server" Text="Send"  OnClientClick="return triggerSend('param1');" ClientIDMode="Static"  />--%>
                        <%--<input type="submit" id="btnSend" value="Send" onclick="return triggerSend('inviteEmail'); return false;" />--%>
                        &nbsp;
                        <asp:Button runat="server" ID="btnClose" Text="Close" OnClick="btnClose_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="lblTempError" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <div runat="server" id="divEditTamplate">
                <hr>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td valign="top" width="130px">Template Name:
                            </td>
                            <td style="padding-bottom: 10px">
                                <asp:TextBox Enabled="false" ID="txtTemplateName" MaxLength="100" runat="server" Width="300px"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom: 10px" valign="top">Subject*:
                            </td>
                            <td style="padding-bottom: 10px">
                                <asp:TextBox ID="txtTemplateSubject" MaxLength="200" runat="server" Width="450px"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="padding-bottom: 10px; vertical-align: top;" valign="top">Body*:
                            </td>
                            <td style="padding-bottom: 10px"></td>
                            <td valign="top"></td>
                        </tr>
                        
                        <tr>
                            <td style="padding-bottom: 10px; vertical-align: top;" valign="top" colspan="3">
                                <telerik:RadEditor ID="rEditorBody" runat="server" Width="100%" Height="400px" ToolsFile="~/css/ToolsFile.xml"
                                    EditModes="Design" AutoResizeHeight="false" ContentAreaMode="Iframe">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="~/css/main.css" />
                                    </CssFiles>
                                </telerik:RadEditor>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-bottom: 10px; vertical-align: top;" valign="top">Placeholder:
                            </td>
                            <td style="padding-bottom: 10px">
                                <table class="tabledata" runat="server" id="Table1">
                                    <tr>
                                        <th>Placeholder
                                        </th>
                                        <th>Description
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>#FIRSTNAME#
                                        </td>
                                        <td>First Name
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>#LASTNAME#
                                        </td>
                                        <td>Last Name
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>#ENTRYID#
                                        </td>
                                        <td>Entry ID
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>#INVOICE#
                                        </td>
                                        <td>Invoice
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>#FLAGREASON#
                                        </td>
                                        <td>Flag Reason(s)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>#ENTRYDATA#
                                        </td>
                                        <td>Entry Data(s)
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td style="padding-bottom: 10px" valign="top">Is Invitation Email:
                            </td>
                            <td style="padding-bottom: 10px">
                                <asp:CheckBox runat="server" ID="chkInvitation" />
                            </td>
                            <td></td>
                        </tr>
                    </tbody>
                </table>
                <br />
            </div>

        </div>
        <div class="overlay">
        </div>


    </asp:PlaceHolder>
</asp:Content>

