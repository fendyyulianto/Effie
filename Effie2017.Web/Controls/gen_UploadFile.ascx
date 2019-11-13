<%@ Control Language="C#" AutoEventWireup="true" CodeFile="gen_UploadFile.ascx.cs"
    Inherits="Controls_gen_UploadFile" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td width="30%">
            <label for="<%= fu.ClientID %>">
                <asp:Label ID="lblfu" runat="server"></asp:Label></label>
        </td>
        <td width="70%">
            <asp:FileUpload ID="fu" runat="server" />
            <asp:Panel ID="pnlFileDownload" runat="server" Visible="false">
                <asp:HyperLink ID="lnkFileDownload" runat="server" Target="_blank"></asp:HyperLink>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td style="font-size: 10px">
            <asp:Label ID="lblfuMsg" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlFileViewer" runat="server" Visible="false">
    Uploaded Image:<br />
    <asp:Image ID="imgFileViewer" runat="server" CssClass="formImageViewer" />
</asp:Panel>
<asp:Panel ID="pnlDelete" runat="server" Visible="false">
    Click
    <asp:Button ID="btnDelete" runat="server" Text="here" OnClick="btnDelete_Click" />
    to delete uploaded file.
</asp:Panel>
