<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadConfirm.aspx.cs" Inherits="Main_UploadConfirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:Literal ID="ltrForCompatibility" runat="server"></asp:Literal>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Add New Entry</title>
    <link href="../css/style-forms.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        function FancyClose() {
            window.parent.CloseFancyBox();
            return false;
        }

        function CheckedTheChk(idToChecked) {
            $('#' + idToChecked).prop('checked', true);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>
            <span>Preview Files and Confirm Uploads</span></h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="82%">
                    <span>Please preview <span>all</span> uploaded files. You will not be able to change
                        once you click confirm upload:<br />
                        <br />
                    </span>
                </td>
                <td width="18%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <h2>
            <span>Entry Forms</span></h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="30%">
                    <label>
                        Upload PDF</label>
                </td>
                <td width="70%">
                    <asp:HyperLink ID="lnkFileDownload0" runat="server" Text="Preview File" Target="_blank"
                        onclick="CheckedTheChk('chkFileViewed0');"></asp:HyperLink>
                    <asp:CheckBox ID="chkFileViewed0" runat="server" Text="Previewed" onclick="return false;" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="font-size: 10px">
                </td>
            </tr>
        </table>
        <h2>
            <span>Authorisation Form</span></h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="30%">
                    <label>
                        Upload PDF</label>
                </td>
                <td width="70%">
                    <asp:HyperLink ID="lnkFileDownload2" runat="server" Text="Preview File" Target="_blank"
                        onclick="CheckedTheChk('chkFileViewed2')"></asp:HyperLink>
                    <asp:CheckBox ID="chkFileViewed2" runat="server" Text="Previewed" onclick="return false;" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="font-size: 10px">
                </td>
            </tr>
        </table>
        <h2>
            <span>Case Image</span></h2>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="30%">
                    <label>
                        Upload JPG</label>
                </td>
                <td width="70%">
                    <asp:HyperLink ID="lnkFileDownload3" runat="server" Text="Preview File" Target="_blank"
                        onclick="CheckedTheChk('chkFileViewed3')"></asp:HyperLink>
                    <asp:CheckBox ID="chkFileViewed3" runat="server" Text="Previewed" onclick="return false;" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="font-size: 10px">
                </td>
            </tr>
        </table>
        <h2>
            <span>Creative Materials</span></h2>
        <table id="tblFile4" runat="server" visible="false" width="100%" border="0" cellspacing="0"
            cellpadding="0">
            <tr>
                <td width="30%">
                    <label>
                        Upload PDF</label>
                </td>
                <td width="70%">
                    <asp:HyperLink ID="lnkFileDownload4" runat="server" Text="Preview File" Target="_blank"
                        onclick="CheckedTheChk('chkFileViewed4')"></asp:HyperLink>
                    <asp:CheckBox ID="chkFileViewed4" runat="server" Text="Previewed" onclick="return false;" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="font-size: 10px">
                </td>
            </tr>
        </table>
        <table id="tblFile5" runat="server" visible="false" width="100%" border="0" cellspacing="0"
            cellpadding="0">
            <tr>
                <td width="30%">
                    <label>
                        Upload Video</label>
                </td>
                <td width="70%">
                    <asp:HyperLink ID="lnkFileDownload5" runat="server" Text="Preview File" Target="_self"
                        onclick="CheckedTheChk('chkFileViewed5')"></asp:HyperLink>
                    <asp:CheckBox ID="chkFileViewed5" runat="server" Text="Previewed" onclick="return false;" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td style="font-size: 10px">
                </td>
            </tr>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="30%">
                    <asp:Label ID="lblDoneMsg" runat="server"></asp:Label>
                </td>
                <td width="52%">
                    <div class="errorDiv">
                        <asp:Label ID="lblError" runat="server"></asp:Label></div>
                    <asp:Button ID="btnSubmit" runat="server" Text="Confirm Uploads" OnClick="btnSubmit_Click" />
                </td>
                <td width="18%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <span style="font-size: 12px;">
            <asp:Label ID="lblRequired" runat="server"></asp:Label></span>
        <asp:Literal ID="ltrJs" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
