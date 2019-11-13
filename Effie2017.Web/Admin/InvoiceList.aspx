<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="InvoiceList.aspx.cs" Inherits="Admin_InvoiceList" %>

<%@ Register Src="~/Admin/Controls/Payment.ascx" TagName="Payment" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
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
                    var checkBox = masterTable.get_dataItems()[i].findElement("chkbox");
                    if (checkBox != null)
                        checkBox.checked = true;  // for checking the checkboxes
                }
            }
            else {
                for (var i = 0; i < row.length; i++) {
                    var checkBox = masterTable.get_dataItems()[i].findElement("chkbox");
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
                        //    //window.location.reload();
                        //}
                    });
                    return;
                },
                autoSize: false,
                height: 700,
                width: 800,
                closeClick: false, // prevents closing when clicking INSIDE fancybox
                helpers: {
                    overlay: { closeClick: false} // prevents closing when clicking OUTSIDE fancybox
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
        #popupReminderEmail
        {
            opacity: 1;
            width: 500px;
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

        .ModalPopUpBig
        {
            left: 150px;
            top: 30px;
            opacity:1;
	        width:800px;
	        height:550px;
	        padding:25px;
	        margin-left:115px;
	        top:15px;
	        margin-top:0px;
	        position:fixed;
	        border:6px solid #bbbbbb;
	        background-color:#fff;
	        z-index:100;
	        overflow-y: scroll;
        } 

        #popup 
        {
            opacity:1;
	        width:800px;
	        height:550px;
	        padding:25px;
	        margin-left:115px;
	        top:0;
	        margin-top:0px;
	        position:fixed;
	        border:6px solid #bbbbbb;
	        background-color:#fff;
	        z-index:100;
	        overflow-y: scroll;
        } 
        
        .overlay {
	        background-color:#000;
	        position:fixed;
	        top:0;
	        right:0;
	        min-height:935px;
	        min-width:1800px;
	        opacity:0.5;
	        filter: alpha(opacity=50);
        }       
        a.tooltip span 
        {
            width:200px;
        }       

        .ModalPopUpSmall
        {
            opacity: 1;
            width: 500px;
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
         
    </style>
    <h2>
        Invoices</h2>
    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>
            Advance Search/Filter</p>
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
                        <asp:ListItem Value="invoiceno" Text="Invoice Number" />
                        <asp:ListItem Value="company" Text="Company" />
                        <asp:ListItem Value="firstname" Text="First Name" />
                        <asp:ListItem Value="lastname" Text="Last Name" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 20%;">
                    <label>
                        <span>Payment Status</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlPaymentStatus" runat="server">
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
                <td>
                    <label>
                        <span>Country</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server">
                    </asp:DropDownList>
                </td>
                 <td style="width: 20%;">
                    <label><span runat="server" id="lblsearchAssignTo">Assign</span> :</label>
                </td>
                <td style="width: 30%;">
                    <asp:DropDownList ID="ddlAssignedTo" runat="server" >
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
    <telerik:RadTabStrip runat="server" ID="rtabEntry" CssClass="tabledata" MultiPageID="RadMultiPage1"
        SelectedIndex="0" OnTabClick="rtabEntry_TabClick">
        <Tabs>
            <telerik:RadTab Text="All Invoices" Width="150px" Value="">
            </telerik:RadTab>
            <telerik:RadTab Text="Pending Payment" Width="150px" Value="NOT">
            </telerik:RadTab>
            <telerik:RadTab Text="Paid" Width="150px" Value="OK">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>
    <br />
    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="Windows7" AutoGenerateColumns="false"
        OnSortCommand="radGridEntry_SortCommand" AllowPaging="true" AllowSorting="true" PageSize="50"
        OnItemDataBound="radGridEntry_ItemDataBound" OnNeedDataSource="radGridEntry_NeedDataSource"
        OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView AllowCustomSorting="true">
            <Columns>
                <telerik:GridTemplateColumn HeaderText="" UniqueName="SelectALL"  HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="30px">
                    <HeaderTemplate>
                        <center>
                            All<br />
                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" /></center>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkbox" runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridBoundColumn DataField="Deadline" UniqueName="Deadline" HeaderText="DL" HeaderStyle-Width="80px"  AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateSubmitted" HeaderText="Date" DataFormatString="{0:dd/MM/yy H:mm}"
                    SortExpression="DateSubmitted" ItemStyle-Width="60px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Invoice" HeaderText="Invoice" ItemStyle-Width="80px"
                    SortExpression="Invoice">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Submitted By" ItemStyle-Width="75px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnBuSubmittedBy" runat="server" Text="View" CssClass="tblLinkBlack"
                            CommandName="User"></asp:LinkButton>
                        <a href="#" class="tooltip">
                            <img src="../images/icon-info.png" width="15" height="15" /><asp:Label ID="lblSubmittedDetails"
                                runat="server" /></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Firstname" HeaderText="First Name" ItemStyle-Width="100px"
                    SortExpression="Firstname">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Lastname" HeaderText="Last Name" ItemStyle-Width="100px"
                    SortExpression="Lastname">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Country" HeaderText="Country" ItemStyle-Width="100px" AllowSorting="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="GrandAmount" HeaderText="Invoice Amount (SGD)"
                    SortExpression="GrandAmount" DataFormatString="{0:N}" ItemStyle-Width="40px"
                    ItemStyle-HorizontalAlign="Right">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="AmountReceived" HeaderText="Amount Paid (SGD)"
                    SortExpression="AmountReceived" DataFormatString="{0:N}" ItemStyle-Width="40px"
                    ItemStyle-HorizontalAlign="Right">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="AmountBalance2" HeaderText="Balance (SGD)" DataFormatString="{0:N}"
                    SortExpression="AmountBalance2" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Right">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Payment Mode" ItemStyle-Width="60px" SortExpression="PaymentMethod">
                    <ItemTemplate>
                        <asp:Label ID="lbPaymentMode" runat="server" Text="" CssClass="tblLinkBlack" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="PayStatus" HeaderText="Payment Status" SortExpression="PayStatus">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Entrytatus" HeaderText="Entry Status" SortExpression="Entrytatus">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateReminder" HeaderText="Date Reminder" ItemStyle-Width="60px"
                    DataFormatString="{0:dd/MM/yy H:mm}" >
                </telerik:GridBoundColumn>
                 <telerik:GridBoundColumn DataField="AdminidAssignedto" UniqueName="AdminidAssignedto" HeaderText="Assign" ItemStyle-Width="100px" AllowSorting="false"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="415px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkInvoice" runat="server" Text="View" CssClass="tblLinkBlack"
                            Target="_blank" />&nbsp;
                        <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CssClass="tblLinkBlack"
                            CommandName="Edit"></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkBtnUpdatePayment" runat="server" Text="Update Payment" CssClass="tblLinkBlack"
                            CommandName="Payment"></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkBtnResend" runat="server" Text="Resend Invoice" CssClass="tblLinkBlack"
                            CommandName="Resend" OnClientClick="return confirm('Confirm to resend invoice?');"></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkBtnHistory" runat="server" Text="Payment History" CssClass="tblLinkBlack"
                            CommandName="History"></asp:LinkButton>&nbsp;
                        <asp:HyperLink ID="hlkEmailHistory" runat="server" Text="Email History" CssClass="tblLinkBlack"
                            Target="_blank"></asp:HyperLink><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />&nbsp;
    <asp:Button ID="btnEmailReminder" runat="server" Text="Send Email" OnClick="btnEmailReminder_Click"  />
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
    <asp:PlaceHolder ID="phPay" runat="server" Visible="false">
        <div id="popup">
            <uc:Payment ID="up1" runat="server" />
        </div>
        <div class="overlay">
        </div>
    </asp:PlaceHolder>

        <asp:PlaceHolder ID="phSelectTemplate" runat="server" Visible="false">
        <div ID="phPopupEmailReminder" runat="server">
            <h2>
                <asp:Label runat="server" ID="lblTitle"></asp:Label></h2>
            <hr />
            <br />
            <table width="100%">
                <tr runat="server" id="roundRow">
                    <td width="30%">
                        Select Rounds :
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRounds">
                        </asp:DropDownList>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr runat="server" id="templateRow">
                    <td width="30%">
                        Select Template :
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTemplateList" style="width:300px" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateList_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:HyperLink runat="server" ID="hlkPreview" Visible="false" Text="Preview"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Button ID="btnSend" runat="server" Text="Send"  OnClick="btnSend_Click" OnClientClick="return confirm('Are you sure to continue ?');" />
                        <%--<asp:Button ID="btnSend" runat="server" Text="Send"  OnClientClick="return triggerSend('param1');" ClientIDMode="Static"  />--%>
                        <%--<input type="submit" id="btnSend" value="Send" onclick="return triggerSend('inviteEmail');return false;" />--%>
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
                        <tr runat="server" id="Tr1" visible="false">
                            <td width="20%" valign="top">Choose Default Template*:
                            </td>
                            <td style="padding-bottom: 10px" valign="top">
                                <asp:DropDownList runat="server" ID="DropDownList1" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateList_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:HiddenField runat="server" ID="hdfRounds" />
                                <asp:HiddenField runat="server" ID="hdfEmailCategory" />
                            </td>
                            <td valign="top"></td>
                        </tr>
                        <tr>
                            <td valign="top" width="130px">Template Name:
                            </td>
                            <td style="padding-bottom: 10px">
                                <asp:TextBox ID="txtTemplateName" MaxLength="100" Enabled="false" runat="server" Width="300px"></asp:TextBox>
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
