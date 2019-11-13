<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="Admin_UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Add mousewheel plugin (this is optional) -->
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
            var masterTable = $find("<%= radGridUser.ClientID %>").get_masterTableView();
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

            var masterTable = $find("<%= radGridUser.ClientID %>").get_masterTableView();
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
            });

            $('.fancybox2').fancybox({
                beforeClose: function () {
                    $('body').fadeOut({
                        //duration: 700, complete: function () {
                        //    //window.location.reload();
                        //}
                    });
                    return;
                },
                autoSize: false,
                height: 450,
                width: 800
            });

            $('.fancybox3').fancybox({
                beforeClose: function () {
                    $('body').fadeOut({
                        //duration: 700, complete: function () {
                        //    //window.location.reload();
                        //}
                    });
                    return;
                },
                autoSize: false,
                height: 550,
                width: 800
            });
        });

        function CloseFancyBox() {
            $("a[title='Close']").click();
        }

        function ValidateSubmitPayment() {
            if ($(':checkbox:checked').length == 0)
                return true;
            else
                return confirm('Please ensure you have reviewed all entries before submitting.\nClick OK to continue with submission or Cancel to review entries.');
        }
        //function triggerSend(params) {
        //    if (window.confirm('Are you sure to continue ?') == false) {
        //        return false;
        //    } else {
        //        __doPostBack('btnSend', params);
        //    }
        //}
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <h2>User</h2>


    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>Advance Search/Filter</p>
        <table width="100%">
            <tr>
                <td>
                    <label>
                        <span>Search</span> :</label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox> on

                    <asp:DropDownList ID="ddlSearch" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="company" Text="Company" />
                        <asp:ListItem Value="email" Text="Email" />
                        <asp:ListItem Value="firstname" Text="First Name" />
                        <asp:ListItem Value="lastname" Text="Last Name" />
                        <asp:ListItem Value="contact" Text="Contact" />
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
                <td style="width: 10%;">
                    <label>
                        <span>Active</span>:</label>
                </td>
                <td style="width: 40%;">
                    <asp:DropDownList ID="ddlActive" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="ACT" Text="Active" />
                        <asp:ListItem Value="INA" Text="Inactive" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Entry Status</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEntryStatus" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="display:none;">
                    <label>
                        <span>Member</span> :</label>
                </td>
                <td style="display:none;">
                    <asp:CheckBoxList ID="cblMember" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Text="CAAA" />
                        <asp:ListItem Value="2" Text="Effie Partner" />
                        <%--<asp:ListItem Value="3" Text="Effie Program" />--%>
                    </asp:CheckBoxList>
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
                <td style="display:none;">
                    <label><span>Verified</span> :</label>
                </td>
                <td style="display:none;">
                    <asp:DropDownList ID="ddlIsVerified" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="true" Text="Yes" />
                        <asp:ListItem Value="false" Text="No" />
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

    <telerik:RadTabStrip runat="server" ID="rtabUser" CssClass="tabledata" MultiPageID="RadMultiPage1" SelectedIndex="0"
        OnTabClick="rtabUser_TabClick">
        <Tabs>
            <telerik:RadTab Text="All Users" Width="150px" Value="">
            </telerik:RadTab>
            <telerik:RadTab Text="Pending Authentication" Width="150px" Value="DIS">
            </telerik:RadTab>
            <telerik:RadTab Text="Active" Width="150px" Value="ACT">
            </telerik:RadTab>
            <telerik:RadTab Text="Inactive" Width="150px" Value="INA">
            </telerik:RadTab>
            <telerik:RadTab Text="Disabled" Width="150px" Value="XXX">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>

    <br />

          <telerik:RadGrid ID="radGridUser" runat="server" Skin="Windows7" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" PageSize="50"
            OnItemDataBound="radGridEntry_ItemDataBound" OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand" OnSortCommand="radGridEntry_SortCommand">
            <PagerStyle AlwaysVisible="true" />
            <MasterTableView  AllowCustomSorting="true">
                <Columns>
                    
                    <telerik:GridTemplateColumn HeaderText="" HeaderStyle-CssClass="darkGrey"
                        HeaderStyle-Width="50px">
                        <HeaderTemplate>
                        <center>
                            All<br />
                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" /></center>
                    </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkbox" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                        <ItemTemplate>
                            <%# Container.DataSetIndex+1 %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="DateCreated" HeaderText="Date Signed Up" DataFormatString="{0:dd/MM/yy H:mm}" ItemStyle-Width="60px"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Company" ItemStyle-Width="130px" SortExpression="Company">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnSubmittedBy" runat="server" CssClass="tblLinkBlack" CommandName="View"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Email" ItemStyle-Width="110px" SortExpression="Email">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkEmail" runat="server" CssClass="tblLinkBlack" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="Firstname" HeaderText="First Name" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Lastname" HeaderText="Last Name" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Job" HeaderText="Title" ItemStyle-Width="100px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Contact" HeaderText="Contact" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Country" HeaderText="Country" ItemStyle-Width="70px"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="CAAAA" HeaderStyle-Width="50px" SortExpression="CAAAA" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lbCAAAA" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Effie Partner" HeaderStyle-Width="70px" SortExpression="APEP" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lbAPEP" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="EProg" HeaderText="Effie Program" ItemStyle-Width="80px" Visible="false"></telerik:GridBoundColumn>

                    <telerik:GridTemplateColumn HeaderText="Verified" HeaderStyle-Width="50px" SortExpression="IsVerified" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lbIsVerified" runat="server" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridBoundColumn DataField="DateReminder" HeaderText="Date Reminder"  DataFormatString="{0:dd/MM/yy H:mm}"  ItemStyle-Width="60px">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey" HeaderStyle-Width="160px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnView" runat="server" Text="View" CssClass="tblLinkBlack" CommandName="View"></asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CssClass="tblLinkBlack" CommandName="Edit"></asp:LinkButton>&nbsp;
                              <asp:HyperLink ID="hlkEmailHistory" runat="server" Text="Email History" CssClass="tblLinkBlack" Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>

        <br />

        <asp:Button ID="btnExport" runat="server" Text="Export" onclick="btnExport_Click" />&nbsp;
        <asp:Button ID="btnVerify" runat="server" Text="Verify Users" onclick="btnVerify_Click" />&nbsp;
    
        <asp:Button ID="btnEmailReminder" runat="server" Text="Send Email" OnClick="btnEmailReminder_Click"/>

        <div class="errorDiv"><asp:Label ID="lblError" runat="server"></asp:Label></div>


    
    
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
                            <td style="padding-bottom: 10px">
                                <telerik:RadEditor ID="rEditorBody" runat="server" Width="100%" Height="400px" ToolsFile="~/css/ToolsFile.xml"
                                    EditModes="Design" AutoResizeHeight="false" ContentAreaMode="Iframe">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="~/css/main.css" />
                                    </CssFiles>
                                </telerik:RadEditor>
                            </td>
                            <td valign="top">
                                
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
