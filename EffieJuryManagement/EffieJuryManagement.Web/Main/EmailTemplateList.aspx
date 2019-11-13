<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailTemplateList.aspx.cs"
    Inherits="Main_EmailTemplateList" MasterPageFile="~/Common/MasterPage.master" %>

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
        $(document).ready(function () {
            $('.fancybox').fancybox({
                beforeClose: function () {
                    $('body').fadeOut({ duration: 700, complete: function () { location.reload(); } });
                    return;
                },
                autoSize: false,
                height: 700,
                width: 850,
                closeClick: false, // prevents closing when clicking INSIDE fancybox
                helpers: {
                    overlay: { closeClick: false} // prevents closing when clicking OUTSIDE fancybox
                }
            });

            
        });

        function CloseFancyBox() {
            $("a[title='Close']").click();
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <h2>
        Email Templates List
    </h2>
    <hr />
    <br />
    <div style="padding-left: 86%;">
        <input name="newJury" type="submit" id="newJury" runat="server" value="Add New Template" onclick="window.location.href = './EmailTemplate.aspx'; return false;" /></div>
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <telerik:RadGrid ID="radGridJury" runat="server" Skin="Windows7" AutoGenerateColumns="false" PageSize="50"
        AllowPaging="true" AllowSorting="true" OnItemDataBound="radGridJury_ItemDataBound"
        OnNeedDataSource="radGridJury_NeedDataSource" OnItemCommand="radGridJury_ItemCommand">
        <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
        <MasterTableView TableLayout="Auto">
            <Columns>
                <telerik:GridTemplateColumn HeaderText="Id" Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdfId" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="No" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                 <telerik:GridTemplateColumn  HeaderText="Tempalte Name" ItemStyle-Width="200px" Visible="false">
                 <ItemTemplate>
                 <asp:Label runat="server" ID="lblTempalteName"></asp:Label>
                 </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="Title" SortExpression="Title" HeaderText="Template">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Subject" SortExpression="Subject" HeaderText="Subject">
                </telerik:GridBoundColumn>
                <telerik:GridCheckBoxColumn DataField="IsActive" HeaderText="Is Active" SortExpression="IsActive">
                </telerik:GridCheckBoxColumn>
                <telerik:GridBoundColumn DataField="DateCreated" SortExpression="DateCreated" HeaderText="DateCreated"
                    DataFormatString="{0 : dd/MMM/yyyy}">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="DateModified" SortExpression="DateModified" HeaderText="DateModified"
                    DataFormatString="{0 : dd/MMM/yyyy}">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="Actions" HeaderStyle-CssClass="darkGrey"
                    HeaderStyle-Width="200px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Edit" CssClass="tblLinkBlack"
                            CommandName="edit"></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="lnkBtnDelete" runat="server" Text="Delete" CssClass="tblLinkBlack"
                            OnClientClick="return confirm('Are you sure want ot delete ?')" CommandName="delete"></asp:LinkButton>&nbsp;
                        <asp:HyperLink ID="hlkPreview" runat="server" Text="Preview" CssClass="tblLinkBlack" ></asp:HyperLink><br />
                        <asp:LinkButton ID="hlCloneTamplate" runat="server" Text="Clone Template" CssClass="tblLinkBlack" CommandName="CloneTamplate" ></asp:LinkButton><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <br />
</asp:Content>
