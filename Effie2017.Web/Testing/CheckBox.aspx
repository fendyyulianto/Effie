<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckBox.aspx.cs" Inherits="Testing_CheckBox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:Panel ID="tasf" runat="server" Visible="false">
        <table>
        <asp:Repeater runat="server" ID="rptIndCredits" OnItemDataBound="rptIndCredits_ItemDataBound" ><ItemTemplate>
            <tr>
                <td><asp:HiddenField ID="hdItemId" runat="server" /><asp:Label ID="head" runat="server"></asp:Label></td>
                <td><asp:CheckBox id="chk1" runat="server" /></td>
                <td><asp:CheckBox id="chk2" runat="server" /></td>
                <td><asp:CheckBox id="chk3" runat="server" /></td>
            </tr>
        </ItemTemplate></asp:Repeater>
        </table></asp:Panel>

        <br />
        <br />

        <asp:CheckBox id="chk" runat="server" Text="check" />
        <asp:CheckBox id="CheckBox1" runat="server" Text="check2" />
        <asp:CheckBox id="CheckBox2" runat="server" Text="check3" />

        <br />
        <br />

        <asp:Button ID="btnCheck" runat="server" Text="Check" OnClick="btnCheck_Click" />

        <asp:PlaceHolder ID="plc" runat="server"></asp:PlaceHolder>

    </div>
    </form>
</body>
</html>
