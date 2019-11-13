<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmailPreview.aspx.cs" Inherits="Main_EmailPreview" %>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Email Preview</title>
    <link href="../css/style-forms.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        function FancyClose() {
            window.parent.CloseFancyBox();
            return false;
        }
    </script>
</head>
<body>
    <form runat="server">
    <div>
        <asp:Literal runat="server" ID="ltrlPreviewText"></asp:Literal>
        <br />
        <br />
        <table width="100%">
            <tr>
                <td align="center" colspan="3">
                    <asp:Button ID="btnBack" runat="server" Text="Close" OnClientClick="window.parent.CloseFancyBox();return false;" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
