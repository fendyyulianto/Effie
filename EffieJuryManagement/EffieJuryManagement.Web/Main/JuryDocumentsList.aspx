<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JuryDocumentsList.aspx.cs"
    Inherits="Main_JuryDocumentsList" MasterPageFile="~/Common/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            var masterTable = $find("<%= radGridJury.ClientID %>").get_masterTableView();
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

        $(document).ready(function () {
            $('.fancybox').fancybox({
                beforeShow: function () {
                    // added 50px to avoid scrollbars inside fancybox
                    this.width = ($('.fancybox-iframe').contents().find('html').width()) + 50;
                    this.height = ($('.fancybox-iframe').contents().find('html').height()) + 200;
                },
                beforeClose: function () {
                    $('body').fadeOut({ duration: 700, complete: function () { location.reload(); } });
                    return;
                },
                autoSize: false,
                fitToView: false,
                maxWidth: 940,
                closeClick: false, // prevents closing when clicking INSIDE fancybox
                helpers: {
                    overlay: { closeClick: false} // prevents closing when clicking OUTSIDE fancybox
                }
            });


        });

        function CloseFancyBox() {
            $("a[title='Close']").click();
        }

        function triggerSend(params) {
            if (window.confirm('Are you sure to continue ?') == false) {
                return false;
            } else {
                __doPostBack('btnSend', params);
            }
        }

        function ClearDataOnLoad() {

            theForm.__EVENTTARGET.value = "";
            theForm.__EVENTARGUMENT.value = "";
            theForm.submit();
        }
    </script>
    <style>
        #popup
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
        .overlay
        {
            background-color: #000;
            position: fixed;
            top: 0;
            right: 0;
            min-height: 935px;
            min-width: 1800px;
            opacity: 0.5;
            filter: alpha(opacity=50);
        }
        a.tooltip span
        {
            width: 200px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <h2>
        Jury Updates</h2>
    <asp:PlaceHolder ID="phAdvanceSearch" runat="server">
        <p>
            Advance Search/Filter</p>
        <table width="100%">
            <tr>
                <td>
                    <label>
                        <span>Search</span> :</label>
                </td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                    on
                    <asp:DropDownList ID="ddlSearch" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="judgeId" Text="Judge Id" />
                        <asp:ListItem Value="name" Text="Name" />
                        <asp:ListItem Value="company" Text="Company" />
                        <asp:ListItem Value="title" Text="Title" />
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Type</span> :</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="Agency" Text="Agency" />
                        <asp:ListItem Value="Client" Text="Client" />
                        <asp:ListItem Value="Media Company" Text="Media Company" />
                        <asp:ListItem Value="Others" Text="Others" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        <span>Upload Photo</span> :</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlUploadPhoto">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <label>
                        <span>Upload Bio Attachment</span> :</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlUploadBio">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <div style="clear: both">
    </div>
    <hr />
    <div class="yearColumn">
        <span>Filter by Effie Year : &nbsp;&nbsp;</span>
        <asp:Repeater runat="server" ID="rptEffieYears" OnItemDataBound="rptEffieYears_ItemDataBound"
            OnItemCommand="rptEffieYears_ItemCommand">
            <HeaderTemplate>
                <ul class="yearTab">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <asp:LinkButton runat="server" ID="lnkYear" CommandName="year"></asp:LinkButton></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div style="clear: both">
    </div>
    <br />
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label></div>
    <telerik:RadGrid ID="radGridJury" runat="server" Skin="Windows7" AutoGenerateColumns="false"
        PageSize="50" AllowPaging="true" AllowSorting="true" OnItemDataBound="radGridJury_ItemDataBound"
        OnNeedDataSource="radGridJury_NeedDataSource" OnItemCommand="radGridJury_ItemCommand">
        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
        <MasterTableView>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="" ItemStyle-Width="60px">
                    <HeaderTemplate>
                        <center>
                            Select All
                            <asp:CheckBox ID="checkAll" runat="server" onclick="CheckAll(this)" /></center>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <center>
                            <asp:CheckBox ID="chkbox" runat="server" /></center>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Type" HeaderText="Type" ItemStyle-Width="70px">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Judge Id" ItemStyle-Width="70px" SortExpression="SerialNo">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnJuryId" runat="server" CommandName="ViewJury"></asp:LinkButton><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Judge Name" ItemStyle-Width="150px" SortExpression="Firstname">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkJuryName" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Company" HeaderText="Company" ItemStyle-Width="200px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Designation" HeaderText="Title" ItemStyle-Width="200px">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Photo">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="lnkPhoto" Target="_blank" CssClass="fancybox fancybox.iframe tblLinkRed"></asp:HyperLink>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Bio Attachment">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="lnkProfile" Target="_blank" ></asp:HyperLink>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jury Update By Judge" SortExpression="DateJuryModified">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblJuryUpdateJudge"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Jury Update By Admin" SortExpression="DateModified">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblJuryUpdateAdmin"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" OnClick="btnSendEmail_Click" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" />&nbsp;
                <asp:Button ID="btnDownload" runat="server" Text="Download (Zip)" OnClick="btnDownload_Click" />
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phSelectTemplate" runat="server" Visible="false">
        <div id="popup">
            <h2>
                <asp:Label runat="server" ID="lblTitle"></asp:Label></h2>
            <hr />
            <br />
            <table width="100%">
                <tr>
                    <td width="30%">
                        Select Template :
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTemplateList" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateList_SelectedIndexChanged">
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
                        <%--<asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" OnClientClick="return confirm('Are you sure to continue ?');" />--%>
                        <input type="submit" id="btnSend" value="Send" onclick="return triggerSend('genericEmail');return false;" />
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
        </div>
        <div class="overlay">
        </div>
    </asp:PlaceHolder>
</asp:Content>
