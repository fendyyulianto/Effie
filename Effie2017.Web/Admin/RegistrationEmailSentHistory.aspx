
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistrationEmailSentHistory.aspx.cs"
    Inherits="Admin_RegistrationEmailSentHistory" %>


<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Email Sent History</title>
    <link href="../css/style-forms.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        function FancyClose() {
            window.parent.$.fancybox.close();
            return false;
        }


    </script>
    <style>
        .rgCurrentPage
        {
            font-size: 1.2em;
            height: 15px;
            min-width: 15px;
            text-align: center;
            line-height: 19px;
            border: 1px solid transparent !important;
            color: #666666 !important;
            font-weight: bolder;
            margin: 2px;
            padding: 2px 7px;
        }
        
        .rgNumPart a
        {
            font-size: 1.2em;
            height: 15px;
            min-width: 15px;
            text-align: center;
            border: 1px solid #376AAC;
            line-height: 19px;
            margin: 2px;
            padding: 2px 7px;
        }
        .rgNumPart a:hover
        {
            border: 1px solid #808080 !important;
            color: #808080 !important;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form runat="server">
    <telerik:RadScriptManager ID="radScrptMgr" runat="server">
    </telerik:RadScriptManager>
    <!-- InstanceBeginEditable name="content" -->
    <h2>
        Email Sent History</h2>
    <div style="clear: both">
    </div>
    <hr />
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>
    <telerik:RadGrid ID="radGridTemplateHistory" runat="server" Skin="" AutoGenerateColumns="false"
        EnableEmbeddedSkins="false" MasterTableView-CssClass="tabledata" AllowPaging="true"
        AllowSorting="true" OnItemDataBound="radGridTemplateHistory_ItemDataBound" OnNeedDataSource="radGridTemplateHistory_NeedDataSource"
        OnItemCommand="radGridTemplateHistory_ItemCommand">
        <PagerStyle AlwaysVisible="true" Mode="NumericPages" />
        <MasterTableView TableLayout="Fixed">
            <Columns>
                <telerik:GridTemplateColumn HeaderText="No" HeaderStyle-Width="30px">
                    <ItemTemplate>
                        <%# Container.DataSetIndex+1 %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Type" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblType"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <%--<telerik:GridTemplateColumn HeaderText="Registration Id" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnRegistrationId" runat="server" CommandName="ViewRegistration"></asp:LinkButton><br />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>--%>
                <telerik:GridTemplateColumn HeaderText="Name" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkRegistrationName" runat="server" Text="" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Email Template" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblEmailTemplate"></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridBoundColumn DataField="DateCreated" HeaderText="Date Sent" DataFormatString="{0:dd/MM/yy H:mm}">
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <br />
    <br />
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:Button ID="btnBack" runat="server" Text="Close" OnClientClick="window.parent.$.fancybox.close();return false;" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
