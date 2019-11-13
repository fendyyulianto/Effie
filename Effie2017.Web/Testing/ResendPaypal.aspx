<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResendPaypal.aspx.cs" Inherits="Testing_ResendPaypal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtGroupId" runat="server"></asp:TextBox>
        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
        <asp:Button ID="btnGo2" runat="server" Text="Go2" OnClick="btnGo2_Click" />
    </div>
    </form>
</body>
</html>
