<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestSendEmail.aspx.cs" Inherits="Testing_CheckBox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="head" runat="server">Email : </asp:Label><asp:TextBox ID="EmailId" runat="server" />
        <asp:Label ID="Label1" runat="server">Text : </asp:Label><asp:TextBox ID="EmailBodyId" runat="server" />

        <asp:Button ID="btnCheck" runat="server" Text="Send" OnClick="btnCheck_Click" />

        <asp:PlaceHolder ID="plc" runat="server"></asp:PlaceHolder>

    </div>
    </form>
</body>
</html>
