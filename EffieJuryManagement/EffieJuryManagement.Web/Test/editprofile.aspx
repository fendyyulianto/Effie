<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editprofile.aspx.cs" Inherits="Test_editprofile" %>

<%@ Register TagPrefix="uc" TagName="gen_UploadFile" Src="~/Controls/gen_UploadFile.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <h2>
        Import Invitation List
    </h2>
    <hr />
    <br />
    <div class="errorDiv">
        <asp:Label ID="lbError" runat="server"></asp:Label></div>    
    <div id="dvUpload">
        <h2>
            <asp:Label ID="lblTitle" runat="server"></asp:Label></h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="82%">
                    <asp:Label ID="lblDesc" runat="server"></asp:Label>
                </td>
                <td width="18%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <uc:gen_UploadFile ID="ucGen_UploadFileForImport" runat="server" isRequired="true"
            replaceFile="true" createDirectory="false" />
        <br />
        <br />
        <p>
            Click <a href="../Storage/ImportTemplate/ImportJuryTemplate.xlsx" target="_blank">here</a>
            to download the Import template.</p>
             <br />
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Upload" OnClick="btnSubmit_Click" />&nbsp;&nbsp;
        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
    </div>
    </form>
</body>
</html>
