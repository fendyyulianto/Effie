<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadForAll.aspx.cs" Inherits="Main_UploadForAll" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="gen_UploadFile" Src="~/Controls/gen_UploadFile.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:Literal ID="ltrForCompatibility" runat="server"></asp:Literal>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Add New Entry</title>
    <link href="../css/style-forms.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        function HideDvUpload() {
            $('#dvUpload').slideUp({ duration: 700, complete: function () { $('#btnSubmit').click(); } });
            return false;
        }

        function FancyClose() {
            window.parent.CloseFancyBox();
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager ID="radScrptMgr" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadProgressManager ID="radProgMgr" runat="server" />
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
            <uc:gen_UploadFile ID="ucGen_UploadFileForAll1" runat="server" isRequired="true"
                replaceFile="true" createDirectory="true" />
            <uc:gen_UploadFile ID="ucGen_UploadFileForAll2" runat="server" isRequired="true"
                replaceFile="true" createDirectory="true" />
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblDoneMsg" runat="server" ForeColor="Green"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <div class="errorDiv">
                            <asp:Label ID="lblError" runat="server"></asp:Label></div>
                    </td>
                </tr>
               
                <tr>
                    <td align="center" width="52%">
                        <asp:Button ID="btnSubmitDummy" runat="server" Text="Upload" OnClientClick="return HideDvUpload();" />
                        <asp:Button ID="btnCloseDummy" runat="server" Text="OK" Visible="false" OnClientClick="return FancyClose();" />
                        <div style="display: none">
                            <asp:Button ID="btnSubmit" runat="server" Text="Upload" OnClick="btnSubmit_Click" /></div>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lblRequired" runat="server" ForeColor="Red" Font-Size="12px"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <telerik:RadProgressArea ID="radProgArea" runat="server" HeaderText="File Upload Progress">
        </telerik:RadProgressArea>
    </div>
    </form>
</body>
</html>
