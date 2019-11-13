<%@ Page Title="" Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs" Inherits="Main_Dashboard" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
        function DeleteConfirmation(itemName) {
            return confirm('Are you sure you want to delete ' + itemName + '?');
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('.fancybox').fancybox({
                beforeClose: function () {
                    <% IsPostBackDes = false; %>
                },
                autoSize: false,
                height: 250,
                width: 450
            });

            $('.fancybox2').fancybox({
                beforeClose: function () {
                    <% IsPostBackDes = false; %>
                },
                autoSize: false,
                height: 450,
                width: 800
            });

            $('.fancybox3').fancybox({
                beforeClose: function () {
                    <% IsPostBackDes = false; %>
                },
                autoSize: false,
                height: 550,
                width: 800
            });
        });

        function CloseFancyBox() {
            <% IsPostBackDes = false; %>
            $("a[title='Close']").click();
        }

        function ValidateSubmitPayment() {
            if ($(':checkbox:checked').length == 0)
                return true;
            else
                return confirm('Please ensure you have reviewed all entries before submitting.\nClick OK to continue with submission or Cancel to review entries.');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <!-- InstanceBeginEditable name="content" -->
    <h1>Entry Overview</h1>
    <h2>Entries Submitted</h2>
    <div style="padding-left: 91%;">
        <input id="btnRefresh" value="Refresh" type="submit" onclick="window.location.href = './Dashboard.aspx'; return false" />
    </div>
    <div class="errorDiv">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>

    <telerik:RadGrid ID="radGridEntry" runat="server" Skin="" MasterTableView-CssClass="tabledata"
        AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" OnItemDataBound="radGridEntry_ItemDataBound"
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Auto">
            <Columns>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="No" SortExpression="No">
                    <ItemTemplate>
                        <asp:Label ID="lblno" runat="server" CssClass="tblLinkBlack" ></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="DateModified" HeaderText="Date" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateSubmitted" HeaderText="Date" HeaderStyle-Width="60">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Serial" HeaderText="Entry ID" HeaderStyle-Width="65">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="Campaign" HeaderText="Title" SortExpression="Campaign">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnViewCampaign" runat="server" CssClass="tblLinkBlack" CommandName="View"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Brand" HeaderText="Brand">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CategoryMarket" HeaderText="Category" HeaderStyle-Width="100">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PayStatus" HeaderText="Payment Status" HeaderStyle-Width="65">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" HeaderText="Upload Status" HeaderStyle-Width="57">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Entry Form*" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="75">
                    <ItemTemplate>
                        <%--<asp:HyperLink ID="lnkUploadEntry" runat="server" Text="Upload Entry Forms" CssClass="tblLinkDisable"
                            Enabled="false" Visible="false"></asp:HyperLink><br />--%>

                        <asp:HyperLink ID="lnkEntryForm" runat="server" Text="Entry Form" CssClass="tblLinkDisable"
                            Enabled="false" Visible="false"></asp:HyperLink>
                        
                        <asp:HyperLink ID="lnkEntry1" runat="server" Text="<br>View PDF" CssClass="tblLinkBlack"
                            Visible="false"></asp:HyperLink><br />
                        <asp:HyperLink ID="lnkEntry2" runat="server" Text="<br>View Word" CssClass="tblLinkBlack"
                            Target="_blank" Visible="false"></asp:HyperLink>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Authorisation Form*" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="85">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkUploadAuthorisation" runat="server" Text="Upload PDF" CssClass="tblLinkDisable"
                            Enabled="false" Visible="false"></asp:HyperLink><br />
                        <asp:HyperLink ID="lnkAuthorisation1" runat="server" Text="<br>View PDF" CssClass="tblLinkBlack"
                            Target="_blank" Visible="false"></asp:HyperLink>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Case Image*" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="77">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkUploadCase" runat="server" Text="Upload Image" CssClass="tblLinkDisable"
                            Enabled="false" Visible="false"></asp:HyperLink><br />
                        <asp:HyperLink ID="lnkCase1" runat="server" Text="<br>View Image" CssClass="tblLinkBlack"
                            Target="_blank" Visible="false"></asp:HyperLink>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Creative Materials*" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="75">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkUploadCreative" runat="server" Text="Upload" CssClass="tblLinkDisable"
                            Enabled="false" Visible="false"></asp:HyperLink><br />
                        <asp:HyperLink ID="lnkCreative1" runat="server" Text="<br>View PDF" CssClass="tblLinkBlack"
                            Target="_blank" Visible="false"></asp:HyperLink>
                        <asp:LinkButton ID="lnkBtnCreative2" runat="server" Text="<br>View Video" CssClass="tblLinkBlack"
                            CommandName="lnkBtnCreative2" Visible="false"></asp:LinkButton>
                        <asp:HyperLink ID="lnkCreative3" runat="server" Text="<br>View Translation" CssClass="tblLinkBlack"
                            Target="_blank" Visible="false"></asp:HyperLink>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Confirm Uploads" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="55">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkConfirm" runat="server" Text="Confirm"></asp:HyperLink>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="55">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnCloning" runat="server" Text="Clone Entry" CommandName="Cloning"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Edit" Visible="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Delete" Visible="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnDelete" runat="server" Text="Delete" CommandName="Delete"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <h2> Entries Pending Submission</h2>
    <div class="errorDiv">
        <asp:Label ID="lblError2" runat="server"></asp:Label>
    </div>

    <telerik:RadGrid ID="radGridEntryPending" runat="server" Skin="" MasterTableView-CssClass="tabledata"
        AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" OnItemDataBound="radGridEntry_ItemDataBound"
        OnNeedDataSource="radGridEntry_NeedDataSource" OnItemCommand="radGridEntry_ItemCommand">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Auto">
            <Columns>
                <telerik:GridTemplateColumn HeaderText="No" SortExpression="No">
                    <ItemTemplate>
                        <asp:Label ID="lblno" runat="server" CssClass="tblLinkBlack" ></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateModified" HeaderText="Date">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateSubmitted" HeaderText="Date" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Serial" HeaderText="Entry ID">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn DataField="Campaign" HeaderText="Title" SortExpression="Campaign">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnViewCampaign" runat="server" CssClass="tblLinkBlack" CommandName="View"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Brand" HeaderText="Brand">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="CategoryMarket" HeaderText="Category">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PayStatus" HeaderText="Payment Status" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Status" HeaderText="Entry Status">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Submit#" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="63" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSubmit" runat="server" CssClass="chkSubmitPayment" Enabled="false" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="165">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnView" runat="server" Text="View" CssClass="tblLinkBlack"
                            CommandName="View" Visible="false"></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CssClass="tblLinkBlack"
                            CommandName="Edit"></asp:LinkButton>
                        <asp:LinkButton ID="lnkBtnDelete" runat="server" Text="Delete" CssClass="tblLinkBlack"
                            CommandName="Delete"></asp:LinkButton>
                        <asp:LinkButton ID="LinkCloning" runat="server" Text="Clone Entry" CssClass="tblLinkBlack"
                            CommandName="Cloning"></asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>

    <br/>

    <div style="display:none;">
    <h2> Login History </h2>
    <telerik:RadGrid ID="radLoginHistories" runat="server" Skin="" MasterTableView-CssClass="tabledata"
        AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" OnItemDataBound="radLoginHistories_ItemDataBound"
        OnNeedDataSource="radLoginHistories_NeedDataSource" OnItemCommand="radLoginHistories_ItemCommand">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Auto">
            <Columns>
                <telerik:GridTemplateColumn HeaderText="No" SortExpression="No" HeaderStyle-Width="25">
                    <ItemTemplate>
                        <asp:Label ID="lblno" runat="server" CssClass="tblLinkBlack" ></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Date" HeaderText="Date">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Time" HeaderText="Time">
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br/>
    </div>
    
    <div>
        Note: Click on an entry title to view its details.
    </div>
    <br/>
    <table style="width: 950px; text-align: center;">
        <tr>
            <td style=" width: 475px; vertical-align: top;">
                <asp:PlaceHolder ID="phAddNewEntry" runat="server">
                    <input name="newEntry" type="submit" id="newEntry" value="Add New Entry" onclick="window.location.href = './Entry.aspx'; return false;" />
                <br /><br />
                <div style="text-align:left;width: 300px;margin-left: 100px;">
                    Tip: All entries, except drafts, may be cloned to help you save time in submitting new entries
                </div>
                </asp:PlaceHolder>
            </td>
            <td style=" width: 475px; ">
                <asp:PlaceHolder ID="phSubmit" runat="server">
                    <asp:Button ID="btnSubmit" runat="server" Text="Click to Submit" OnClick="btnSubmit_Click" OnClientClick="return ValidateSubmitPayment();" />
                    <div style="text-align:left;width: 300px;margin-left: 100px;">
                       <br/> # You may select multiple Entries to submit.<br /><br />
                        Tip: Combining multiple entries into a single invoice helps to save on admin charges.
                    </div>  
                </asp:PlaceHolder>
            </td>
        </tr>
    </table>

    <div class="popup" style="visibility: hidden">
        <h2>Entry Form File Uploads</h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="30%">Upload PDF*
                </td>
                <td width="52%">
                    <input type="text" name="textfield" id="textfield" />
                </td>
                <td width="18%">
                    <input type="submit" name="button" id="button" value="Browse" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td style="font-size: 10px">PDF format (max file size 1mb)
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>Upload Word*
                </td>
                <td>
                    <input type="text" name="textfield" id="textfield" />
                </td>
                <td>
                    <input type="submit" name="button" id="button" value="Browse" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td style="font-size: 10px">MS Word fomat (max file size 1 mb)
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td>
                    <input type="submit" name="button" id="button" value="Upload" />
                </td>
                <td>&nbsp;
                </td>
            </tr>
        </table>
        <br />
        <span style="font-size: 12px;">* both files are required</span>
    </div>
    <asp:Literal ID="ltrJs" runat="server"></asp:Literal>
    <!-- InstanceEndEditable -->

    <div style="display:none"><asp:Button ID="btnRebind" runat="server" OnClick="btnRebind_Click" /></div>

</asp:Content>
